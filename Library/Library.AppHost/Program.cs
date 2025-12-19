using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

var builder = DistributedApplication.CreateBuilder(args);

var keyBytes = RandomNumberGenerator.GetBytes(32);
var keyBase64 = Convert.ToBase64String(keyBytes);

var jwtSecret = builder.AddParameter(
    "JwtSettingsSecretKey",
    value: keyBase64,
    secret: true
);

var postgres = builder.AddPostgres("PostgreSQL");

var postgresDb = postgres.AddDatabase("LibraryDB");

var api = builder.AddProject<Projects.Library_Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WithEnvironment("JwtSettingsIssuer", "Library.Api")
    .WithEnvironment("JwtSettingsAudience", "Library.Api")
    .WithEnvironment("JwtSettingsSecretKey", jwtSecret)
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
    .WithEnvironment("RABBITMQ_PUBLISH_DELAY_MS", "100")
    .WithExplicitStart();

builder.AddProject<Projects.Library_Client>("Client")
    .WithReference(api)
    .WaitFor(api); 

builder.Build().Run();
