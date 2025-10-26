using Library.Infrastructure.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Library.RabbitMqConsumer;

public class RabbitMqConsumerWorker(ILogger<RabbitMqConsumerWorker> logger, IConnection connection) : BackgroundService
{
    private IChannel? _channel;
    private const string ExchangeName = "data-exchange";
    private const string QueueName = "data-queue";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, durable: true);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

        await _channel.QueueBindAsync(QueueName, ExchangeName, "book.create");
        await _channel.QueueBindAsync(QueueName, ExchangeName, "customer.create");
        await _channel.QueueBindAsync(QueueName, ExchangeName, "record.create");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var routingKey = ea.RoutingKey;

            logger.LogInformation("Received message with routing key [{RoutingKey}]: {Json}", routingKey, json);

            await _channel.BasicAckAsync(ea.DeliveryTag, false);
            await Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
        await connection.CloseAsync();
        await connection.DisposeAsync();
    }
}
