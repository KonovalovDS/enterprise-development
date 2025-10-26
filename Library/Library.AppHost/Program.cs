using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("PostgreSQL");

var postgresDb = postgres.AddDatabase("LibraryDB");

var api = builder.AddProject<Projects.Library_Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

var rabbitMq = builder.AddRabbitMQ("RabbitMQ");

var consumerService = builder.AddProject<Projects.Library_RabbitMqConsumer>("RabbitMqConsumer")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq);

var producerService = builder.AddProject<Projects.Library_RabbitMqProducer>("RabbitMqProducer")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq);


builder.Build().Run();