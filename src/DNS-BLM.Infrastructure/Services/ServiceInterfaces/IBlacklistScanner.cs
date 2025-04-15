using DNS_BLM.Infrastructure.Dtos;

namespace DNS_BLM.Infrastructure.Services.ServiceInterfaces;

public interface IBlacklistScanner
{
    public Task Scan(List<string> domains, CancellationToken cancellationToken = default);
    public string ScannerName { get; }
}