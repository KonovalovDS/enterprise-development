using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Library.DataGenerator;

public class RabbitMQProducer(IConnection connection, IConfiguration configuration) : IAsyncDisposable
{
    private readonly IConnection _connection = connection;
    private readonly string _exchangeName = configuration["RabbitMQ:ExchangeName"] ?? "data-exchange";
    private IChannel? _channel;

    public async Task InitializeAsync()
    {
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _exchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false);
    }

    public async Task SendMessageAsync<T>(T message, string? routingKey = null)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        await SendMessageAsync(jsonMessage, routingKey);
    }

    public async Task SendMessageAsync(string message, string? routingKey = null)
    {
        var finalRoutingKey = routingKey ?? "data.routing";
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await _channel.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: finalRoutingKey,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
        await _connection.CloseAsync();
        await _connection.DisposeAsync();
    }
}