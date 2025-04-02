using DNS_BLM.Application.Services;

namespace Tests.Mocks;

public class MockScanner : IBlacklistScanner
{
    public Task<List<string>> Scan(List<string> domains)
    {
        return Task.FromResult(domains);
    }
}