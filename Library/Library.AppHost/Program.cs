var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");

var postgresDb = postgres.AddDatabase("librarydb");

var pgConnectionString = builder.AddConnectionString(
    "DefaultConnection",
    ReferenceExpression.Create($"{postgresDb}"));

var api = builder.AddProject<Projects.Api>("LibraryApi")
    .WithReference(pgConnectionString)
    .WaitFor(pgConnectionString);

builder.Build().Run();
