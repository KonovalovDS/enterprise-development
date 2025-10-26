using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var postgresDb = postgres.AddDatabase("librarydb");

var api = builder.AddProject<Projects.Library_Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

var rabbitMq = builder.AddRabbitMQ("rabbitmq");

var consumerService = builder.AddProject<Projects.Library_RabbitMqConsumer>("consumer-service")
    .WithReference(rabbitMq);

var producerService = builder.AddProject<Projects.Library_RabbitMqProducer>("producer-service")
    .WithReference(rabbitMq);


builder.Build().Run();