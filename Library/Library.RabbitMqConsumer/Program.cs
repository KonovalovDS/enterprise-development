using Library.Application.Contracts.Mappers;
using Library.Domain.Interfaces;
using Library.Infrastructure.Persistence;
using Library.Infrastructure.Repositories;
using Library.RabbitMqConsumer;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "DefaultConnection");

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

builder.Services.AddSingleton<IConnectionFactory>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("RabbitMQ");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("RabbitMQ connection string is not configured");
    }

    return new ConnectionFactory
    {
        Uri = new Uri(connectionString)
    };
});

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();

host.Run();
