using System.Reflection;
using DNS_BLM.Api.Services;
using DNS_BLM.Api.TimedTasks;
using DNS_BLM.Application;
using DNS_BLM.Infrastructure;
using Scalar.AspNetCore;
using Sentry.Extensibility;

var builder = WebApplication.CreateBuilder(args);
if (builder.Configuration.GetSection("DNS-BLM:Sentry").Exists())
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = builder.Configuration.GetSection("DNS-BLM:Sentry").GetValue<string>("Dsn");
        o.TracesSampleRate = builder.Configuration.GetSection("DNS-BLM:Sentry").GetValue<double>("TracesSampleRate");
        o.MaxRequestBodySize = builder.Configuration.GetSection("DNS-BLM:Sentry").GetValue<RequestSize>("MaxRequestBodySize");
        o.SendDefaultPii = builder.Configuration.GetSection("DNS-BLM:Sentry").GetValue<bool>("SendDefaultPii");
    });
}

if (builder.Environment.IsDevelopment() || builder.Configuration.GetSection("DNS-BLM").GetValue<bool>("Debug"))
    builder.Logging.AddFilter("DNS_BLM", LogLevel.Debug);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationModule(builder.Configuration);
builder.Services.AddInfrastructureModule(builder.Configuration);

builder.Services.AddTimedTaskModules();


var app = builder.Build();

// if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var version = Assembly.GetEntryAssembly()!.GetSemanticVersion();
app.Logger.LogInformation($"DNS-BLM Version: {version}");

app.Run();