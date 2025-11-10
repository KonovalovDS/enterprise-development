using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("PostgreSQL");

var postgresDb = postgres.AddDatabase("LibraryDB");

builder.AddProject<Projects.Library_Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var rabbitMq = builder.AddRabbitMQ("RabbitMQ", username, password)
    .WithManagementPlugin();

builder.AddProject<Projects.Library_RabbitMqConsumer>("RabbitMqConsumer")
    .WithReference(rabbitMq)
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.Library_RabbitMqProducer>("RabbitMqProducer")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithEnvironment("RABBITMQ_PUBLISH_DELAY_MS", "100");


builder.Build().Run();
