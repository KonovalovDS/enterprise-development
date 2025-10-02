var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var postgresDb = postgres.AddDatabase("librarydb");

var api = builder.AddProject<Projects.Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.Build().Run();