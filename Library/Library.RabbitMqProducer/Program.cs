using Library.RabbitMqProducer;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

builder.Services.AddSingleton<RabbitMqProducer>();

var host = builder.Build();
host.Run();
