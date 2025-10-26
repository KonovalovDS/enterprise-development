using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Library.DataGenerator;

namespace Library.RabbitMqProducer;

public class RabbitMqProducerWorker(IConnection connection) : BackgroundService
{
    private IChannel? _channel;
    private readonly BogusGenerator _generator = new();
    private const string ExchangeName = "data-exchange";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, durable: true);

        var random = new Random();

        while (!stoppingToken.IsCancellationRequested)
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
                default:
                    routingKey = "record.create";
                    payload = _generator.GenerateRecord();
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

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
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