using DNS_BLM.Application.Services;

namespace Tests;

public class UnitTest1 : DatabaseTestCase
{
    private readonly INotificationService _notificationService;

    public UnitTest1(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Just a demo test
    /// </summary>
    [Fact]
    public void Test1()
    {
        _notificationService.Notify("Test", new List<string> { "Test" });
        Assert.True(true);
    }
}