var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.library>("library");

builder.Build().Run();
