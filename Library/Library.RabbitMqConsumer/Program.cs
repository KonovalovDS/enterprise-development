using Library.RabbitMqConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();
host.Run();
