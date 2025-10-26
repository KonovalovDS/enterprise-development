using Library.Infrastructure.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Library.RabbitMqConsumer;

public class RabbitMqConsumer(IConnectionFactory connectionFactory, ILogger<RabbitMqConsumer> logger) : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private const string ExchangeName = "data-exchange";
    private const string QueueName = "data-queue";

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

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
            var routingKeys = new[] { "book.create", "customer.create", "record.create" };
            foreach (var routingKey in routingKeys)
            {
                await _channel.QueueBindAsync(QueueName, ExchangeName, routingKey);
            }
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var routingKey = ea.RoutingKey;

                    logger.LogInformation("Received message. RoutingKey: {RoutingKey}, Body: {Json}", routingKey, json);
                    await ProcessMessageAsync(routingKey, json);

                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message. RoutingKey: {RoutingKey}", ea.RoutingKey);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogCritical(ex, "RabbitMQ Consumer failed to start");
            throw;
        }
    }

    private async Task ProcessMessageAsync(string routingKey, string json)
    {
        try
        {
            switch (routingKey)
            {
                case "book.create":
                    await ProcessBookMessageAsync(json);
                    break;
                case "customer.create":
                    await ProcessCustomerMessageAsync(json);
                    break;
                case "record.create":
                    await ProcessRecordMessageAsync(json);
                    break;
                default:
                    logger.LogWarning("Unknown routing key: {RoutingKey}", routingKey);
                    break;
            }

            await Task.Delay(100);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ProcessMessageAsync for routing key: {RoutingKey}", routingKey);
            throw;
        }
    }

    private async Task ProcessBookMessageAsync(string json)
    {
        try
        {
            logger.LogInformation("Processing book message: {Json}", json);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing book message");
            throw;
        }
    }

    private async Task ProcessCustomerMessageAsync(string json)
    {
        try
        {
            logger.LogInformation("Processing customer message: {Json}", json);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing customer message");
            throw;
        }
    }

    private async Task ProcessRecordMessageAsync(string json)
    {
        try
        {
            logger.LogInformation("Processing record message: {Json}", json);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing record message");
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