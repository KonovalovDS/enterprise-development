using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Library.DataGenerator;

namespace Library.RabbitMqProducer;

public class RabbitMqProducer(IConnectionFactory connectionFactory, ILogger<RabbitMqProducer> logger) : BackgroundService
{
    private readonly BogusGenerator _generator = new();
    private IConnection? _connection;
    private IChannel? _channel;
    private const string ExchangeName = "data-exchange";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _connection = await connectionFactory.CreateConnectionAsync();
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

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogError(ex, "Error sending message");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            throw;
        }
    }

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

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}