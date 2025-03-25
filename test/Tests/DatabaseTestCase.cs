using DNS_BLM.Application;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class DatabaseTestCase : IAsyncLifetime
{
    public IConfigurationRoot ConfigurationRoot { get; private set; }
    public IMediator Mediator { get; private set; }
    
    public Task InitializeAsync()
    {
        var services = new ServiceCollection();
        ConfigurationRoot = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<DatabaseTestCase>()
            .Build();
        services.AddHttpContextAccessor();
        
        services.Configure<IConfigurationRoot>(ConfigurationRoot);
        services.AddLogging();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ModuleRegistry).Assembly);
        });
        
        var serviceProvider = services.BuildServiceProvider();
        Mediator = serviceProvider.GetRequiredService<IMediator>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}