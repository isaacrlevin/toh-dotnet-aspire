var builder = DistributedApplication.CreateBuilder(args);

var sqldb = builder.AddSqlServer("sql")
    .AddDatabase("heroes");


var heroApi = builder.AddProject<Projects.toh_dotnet_api>("heroesapi")
    .WithReference(sqldb)
  .WithExternalHttpEndpoints()
  .WaitFor(sqldb);

builder.AddNpmApp("angular", "../toh-dotnet.client.angular")
    .WithReference(heroApi)
    .WaitFor(heroApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddProject<Projects.toh_dotnet_MigrationService>("migrations")
    .WithReference(sqldb)
    .WaitFor(sqldb);

builder.Build().Run();
