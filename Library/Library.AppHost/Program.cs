var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var postgresDb = postgres.AddDatabase("librarydb");

var api = builder.AddProject<Projects.Library_Api>("LibraryApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.AddProject<Projects.Library_DataGenerator>("library-datagenerator");

builder.Build().Run();