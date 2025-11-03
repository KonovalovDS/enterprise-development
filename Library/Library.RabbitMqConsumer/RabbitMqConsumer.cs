using AutoMapper;
using Library.Application.Contracts.BookDtos;
using Library.Application.Contracts.BorrowRecordDtos;
using Library.Application.Contracts.CustomerDtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Library.RabbitMqConsumer;

/// <summary>
/// RabbitMQ consumer service that listens to messages for books, customers, and borrow records.
/// Implements <see cref="BackgroundService"/> to run continuously in the background.
/// Processes incoming messages using JSON deserialization and AutoMapper, then stores data via repositories.
/// </summary>
/// <param name="connectionFactory">The factory used to create RabbitMQ connections.</param>
/// <param name="logger">Logger instance for logging consumer activities, errors and info.</param>
/// <param name="mapper">AutoMapper instance used to map DTOs to entity objects.</param>
/// <param name="scopeFactory">Service scope factory used to create scoped service providers for repositories.</param>
public class RabbitMqConsumer(
    IConnectionFactory connectionFactory,
    ILogger<RabbitMqConsumer> logger,
    IMapper mapper,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    /// <summary>
    /// RabbitMQ connection object used to establish communication with the broker.
    /// </summary>
    private IConnection? _connection;

    /// <summary>
    /// RabbitMQ channel object used for declaring exchanges, queues, and consuming messages.
    /// </summary>
    private IChannel? _channel;

    /// <summary>
    /// Name of the RabbitMQ exchange to which messages are published.
    /// </summary>
    private const string ExchangeName = "data-exchange";

    /// <summary>
    /// Name of the RabbitMQ queue from which messages are consumed.
    /// </summary>
    private const string QueueName = "data-queue";

    /// <summary>
    /// Executes the consumer service asynchronously, connecting to RabbitMQ, binding queues 
    /// and starting message consumption.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token used to stop the background service.</param>
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

    /// <summary>
    /// Processes an incoming RabbitMQ message based on its routing key.
    /// </summary>
    /// <param name="routingKey">Routing key of the message.</param>
    /// <param name="json">Message payload as a JSON string.</param>
    private async Task ProcessMessageAsync(string routingKey, string json)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var recordRepository = scope.ServiceProvider.GetRequiredService<IBorrowRecordRepository>();

            switch (routingKey)
            {
                case "book.create":
                    await ProcessBookMessageAsync(json, bookRepository);
                    break;
                case "customer.create":
                    await ProcessCustomerMessageAsync(json, customerRepository);
                    break;
                case "record.create":
                    await ProcessRecordMessageAsync(json, recordRepository);
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

    /// <summary>
    /// Processes a book creation message.
    /// </summary>
    /// <param name="json">JSON string payload.</param>
    /// <param name="bookRepository">Repository instance to save the book entity.</param>
    private async Task ProcessBookMessageAsync(string json, IBookRepository bookRepository)
    {
        try
        {
            logger.LogInformation("Processing book message: {Json}", json);
            var bookDto = JsonSerializer.Deserialize<BookEditDto>(json);
            if (bookDto == null)
            {
                logger.LogWarning("Received invalid book message: {Json}", json);
                return;
            }
            var book = mapper.Map<Book>(bookDto);
            await bookRepository.AddAsync(book);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing book message");
            throw;
        }
    }

    /// <summary>
    /// Processes a customer creation message.
    /// </summary>
    /// <param name="json">JSON string payload.</param>
    /// <param name="customerRepository">Repository instance to save the customer entity.</param>
    private async Task ProcessCustomerMessageAsync(string json, ICustomerRepository customerRepository)
    {
        try
        {
            logger.LogInformation("Processing customer message: {Json}", json);
            var customerDto = JsonSerializer.Deserialize<CustomerEditDto>(json);
            if (customerDto == null)
            {
                logger.LogWarning("Received invalid customer message: {Json}", json);
                return;
            }
            var customer = mapper.Map<Customer>(customerDto);
            await customerRepository.AddAsync(customer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing customer message");
            throw;
        }
    }

    /// <summary>
    /// Processes a borrow record creation message.
    /// </summary>
    /// <param name="json">JSON string payload.</param>
    /// <param name="borrowRecordRepository">Repository instance to save the borrow record entity.</param>
    private async Task ProcessRecordMessageAsync(string json, IBorrowRecordRepository borrowRecordRepository)
    {
        try
        {
            logger.LogInformation("Processing record message: {Json}", json);
            var recordDto = JsonSerializer.Deserialize<BorrowRecordEditDto>(json);
            if (recordDto == null)
            {
                logger.LogWarning("Received invalid borrow record message: {Json}", json);
                return;
            }
            var record = mapper.Map<BorrowRecord>(recordDto);

            await borrowRecordRepository.AddAsync(record);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing borrow record message");
            throw;
        }
    }

    /// <summary>
    /// Stops the consumer service by closing the RabbitMQ channel and connection.
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