using System.Net;
using System.Net.Mail;
using System.Text;
using DNS_BLM.Infrastructure.Services.ServiceInterfaces;
using Microsoft.Extensions.Configuration;

namespace DNS_BLM.Infrastructure.Services.NotificationServices;

public class MailNotificationService : INotificationService, IDisposable
{
    private readonly IConfiguration _configuration;
    private bool _disposed = false;
    private SmtpClient? _smtpClient;

    public MailNotificationService(IConfiguration configuration)
    {
        _configuration = configuration;

        var host = _configuration.GetValue<string>("DNS-BLM:Mail:Host");
        var port = _configuration.GetValue<int>("DNS-BLM:Mail:Port");
        var username = _configuration.GetValue<string>("DNS-BLM:Mail:Username");
        var password = _configuration.GetValue<string>("DNS-BLM:Mail:Password");
        var enableSsl = _configuration.GetValue<bool>("DNS-BLM:Mail:EnableSsl");
        
        _smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = enableSsl,
        };
    }

    public async Task Notify(string subject, string message)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MailNotificationService));

        var from = _configuration.GetValue<string>("DNS-BLM:Mail:From");
        using (var mailMessage = new MailMessage
               {
                   From = new MailAddress(from),
                   Subject = subject,
                   Body = message,
                   IsBodyHtml = false
               })
        {
            var reportReceiver = _configuration.GetValue<string>("DNS-BLM:ReportReceiver");
            mailMessage.To.Add(reportReceiver);
            await _smtpClient!.SendMailAsync(mailMessage);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _smtpClient != null)
            {
                _smtpClient.Dispose();
                _smtpClient = null;
            }
            _disposed = true;
        }
    }
}