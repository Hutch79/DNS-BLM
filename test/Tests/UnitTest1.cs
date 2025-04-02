using DNS_BLM.Application.Services;

namespace Tests;

public class UnitTest1 : DatabaseTestCase
{
    private readonly INotificationService _notificationService;
    private readonly IBlacklistScanner _scanner;

    public UnitTest1(INotificationService notificationService, IBlacklistScanner scanner)
    {
        _notificationService = notificationService;
        _scanner = scanner;
    }
    
    [Fact]
    public void Test1()
    {
        _notificationService.Notify("Test", new List<string> { "Test" });
        Assert.True(true);
    }
    
    [Fact]
    public async Task Test2()
    {
        var domains = new List<string> { "test.com" };
        var result = await _scanner.Scan(domains);
        
        Assert.Equal(domains.Count, result.Count);
    }
}