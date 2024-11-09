using tohdotnet.domain.Models;
using toh_dotnet.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();


builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<tohdotnetContext>("heroes");

var host = builder.Build();
host.Run();
