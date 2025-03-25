using DNS_BLM.Application.Services;

namespace Tests.Mocks;

public class MockNotificationService : INotificationService
{
    public Task Notify(string subject, List<string> message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}
