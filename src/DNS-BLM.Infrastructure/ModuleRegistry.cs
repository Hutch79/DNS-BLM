using System.Net.Http.Headers;
using DNS_BLM.Infrastructure.Services;
using DNS_BLM.Infrastructure.Services.NotificationServices;
using DNS_BLM.Infrastructure.Services.ScannerServices;
using DNS_BLM.Infrastructure.Services.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DNS_BLM.Infrastructure;

public static class ModuleRegistry
{
    public static void AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<INotificationService, MailNotificationService>();
        services.AddSingleton<MessageService>();

        string? virusTotalApiKey = configuration["DNS-BLM:API_Credentials:VirusTotal"];
        if (virusTotalApiKey != null)
        {
            services.AddSingleton<IBlacklistScanner, VirusTotalService>();
            services.AddHttpClient("VirusTotal", client =>
            {
                client.BaseAddress = new Uri("https://www.virustotal.com/api/v3/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-apikey", virusTotalApiKey);
            });
        }
    }
}