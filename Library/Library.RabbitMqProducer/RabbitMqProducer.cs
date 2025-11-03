using Library.DataGenerator;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Library.RabbitMqProducer;

/// <summary>
/// RabbitMQ producer service that generates and sends book, customer, and borrow record contracts.
/// This service runs as a <see cref="BackgroundService"/> and periodically publishes messages.
/// </summary>
/// <param name="connectionFactory">Factory used to create RabbitMQ connections.</param>
/// <param name="logger">Logger instance for logging publishing activity and errors.</param>
public class RabbitMqProducer(
    IConnectionFactory connectionFactory,
    ILogger<RabbitMqProducer> logger) : BackgroundService
{
    /// <summary>
    /// Data generator that produces books, customers and borrow records contracts.
    /// </summary>
    private readonly BogusGenerator _generator = new();

    /// <summary>
    /// RabbitMQ connection object used to establish communication with the broker.
    /// </summary>
    private IConnection? _connection;

    /// <summary>
    /// RabbitMQ channel object used for declaring exchanges and publishing messages.
    /// </summary>
    private IChannel? _channel;

    /// <summary>
    /// Name of the RabbitMQ exchange to which messages are published.
    /// </summary>
    private const string ExchangeName = "data-exchange";

    /// <summary>
    /// Attempts to establish a connection to RabbitMQ using the <see cref="IConnectionFactory"/>,
    /// retrying on failure up to a specified number of times with a delay between attempts.
    /// </summary>
    /// <param name="stoppingToken">A <see cref="CancellationToken"/> to cancel connection attempts.</param>
    /// <param name="maxRetries">The maximum number of retry attempts before throwing an exception.</param>
    /// <param name="delayMs">The delay in milliseconds between retry attempts.</param>
    private async Task<IConnection> ConnectWithRetryAsync(
        CancellationToken stoppingToken,
        int maxRetries = 5,
        int delayMs = 1000)
    {
        var attempt = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                attempt++;
                var connection = await connectionFactory.CreateConnectionAsync();
                logger.LogInformation("Successfully connected to RabbitMQ on attempt {Attempt}", attempt);
                return connection;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to connect to RabbitMQ on attempt {Attempt}", attempt);
                if (attempt >= maxRetries)
                {
                    logger.LogError("Maximum retry attempts reached ({MaxRetries}). Throwing exception.", maxRetries);
                    throw;
                }
                await Task.Delay(delayMs, stoppingToken);
            }
        }
        throw new OperationCanceledException("Connection attempt was cancelled.");
    }

    /// <summary>
    /// Executes the producer service asynchronously, generating data and publishing it
    /// to RabbitMQ until the <paramref name="stoppingToken"/> signals cancellation.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token to stop the background service.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _connection = await ConnectWithRetryAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            var random = new Random();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var typeChoice = random.Next(3);
                    string routingKey;
                    object payload;

                    switch (typeChoice)
                    {
                        case 0:
                            routingKey = "book.create";
                            payload = _generator.GenerateBook();
                            break;
                        case 1:
                            routingKey = "customer.create";
                            payload = _generator.GenerateCustomer();
                            break;
                        case 2:
                            routingKey = "record.create";
                            payload = _generator.GenerateRecord();
                            break;
                        default:
                            routingKey = "data";
                            payload = "test";
                            break;
                    }

                    var json = JsonSerializer.Serialize(payload);
                    var body = Encoding.UTF8.GetBytes(json);

                    await _channel.BasicPublishAsync(
                        exchange: ExchangeName,
                        routingKey: routingKey,
                        mandatory: false,
                        basicProperties: new BasicProperties { Persistent = true },
                        body: body);

                    logger.LogInformation("Sent message. RoutingKey: {RoutingKey}, Type: {Type}",
                        routingKey, payload.GetType().Name);

                    await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Error sending message");
                    await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw;
        }
    }

    /// <summary>
    /// Stops the producer service by closing the RabbitMQ channel and connection.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for stopping the service.</param>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_channel != null && _channel.IsOpen)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null && _connection.IsOpen)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during RabbitMQ Consumer shutdown");
        }
        finally
        {
            await base.StopAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Disposes the RabbitMQ channel and connection resources.
    /// </summary>
    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}