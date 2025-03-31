using DNS_BLM.Application;
using DNS_BLM.Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tests.Mocks;

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

        services.AddSingleton<INotificationService, MockNotificationService>();
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