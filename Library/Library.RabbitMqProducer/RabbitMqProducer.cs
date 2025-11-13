using Library.DataGenerator;
using Microsoft.Extensions.Configuration;
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
    ILogger<RabbitMqProducer> logger,
    IConfiguration configuration) : BackgroundService
{
    /// <summary>
    /// Starting counter for bogus books records generator
    /// </summary>
    private const int BooksCount = 1;

    /// <summary>
    /// Starting counter for bogus customers records generator
    /// </summary>
    private const int CustomersCount = 1;

    /// <summary>
    /// Data generator that produces books, customers and borrow records contracts.
    /// </summary>
    private readonly BogusGenerator _generator = new(BooksCount, CustomersCount);

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
                var connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
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
            await using var connection = await ConnectWithRetryAsync(stoppingToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            var delayMs = configuration.GetValue<int>("RABBITMQ_PUBLISH_DELAY_MS", 100);

            await channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false, 
                cancellationToken: stoppingToken);

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

                    await channel.BasicPublishAsync(
                        exchange: ExchangeName,
                        routingKey: routingKey,
                        mandatory: false,
                        basicProperties: new BasicProperties { Persistent = true },
                        body: body, 
                        cancellationToken: stoppingToken);

                    logger.LogInformation("Sent message. RoutingKey: {RoutingKey}, Type: {Type}",
                        routingKey, payload.GetType().Name);

                    await Task.Delay(delayMs, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Error sending message");
                    await Task.Delay(delayMs, stoppingToken);
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw;
        }
    }
}