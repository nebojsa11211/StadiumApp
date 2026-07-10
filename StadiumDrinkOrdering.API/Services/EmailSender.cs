using System.Net;
using System.Net.Mail;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>Sends transactional emails (e.g. shell-account activation links). Two implementations exist:
/// a real SMTP sender and a dev sender that just logs the message. Selected in Program.cs by whether
/// <c>Email:Host</c> is configured.</summary>
public interface IEmailSender
{
    Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody = null, CancellationToken ct = default);

    /// <summary>True when a real transport is configured (SMTP). False for the dev logger, so callers can
    /// surface the link another way in development.</summary>
    bool IsRealTransport { get; }
}

/// <summary>Config for the SMTP sender (appsettings <c>Email</c> section). Absent host ⇒ dev logger.</summary>
public class EmailSettings
{
    public string? Host { get; set; }
    public int Port { get; set; } = 587;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string FromAddress { get; set; } = "no-reply@stadium.local";
    public string FromName { get; set; } = "Stadium";
    public bool EnableSsl { get; set; } = true;

    /// <summary>Recipient domains never actually mailed (fake/seed data). Sends to these are skipped +
    /// logged, so demo <c>@example.com</c> accounts don't trigger real delivery or bounces.</summary>
    public string[] SkipDomains { get; set; } = new[] { "example.com", "test.com", "stadium.local" };
}

/// <summary>Dev/no-transport sender: writes the email (and any activation link) to the log instead of
/// sending it, so the flow is fully testable without SMTP credentials.</summary>
public class LoggingEmailSender : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger;
    public LoggingEmailSender(ILogger<LoggingEmailSender> logger) => _logger = logger;

    public bool IsRealTransport => false;

    public Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody = null, CancellationToken ct = default)
    {
        // Log the plain-text body (or a stripped HTML fallback) prominently — it contains the activation link.
        var body = string.IsNullOrWhiteSpace(textBody) ? htmlBody : textBody;
        _logger.LogWarning(
            "📧 [DEV EMAIL — not actually sent] To: {To} | Subject: {Subject}\n{Body}",
            toEmail, subject, body);
        return Task.CompletedTask;
    }
}

/// <summary>Real SMTP sender over <see cref="System.Net.Mail"/>. Skips configured fake domains so seed
/// data never triggers delivery.</summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(EmailSettings settings, ILogger<SmtpEmailSender> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public bool IsRealTransport => true;

    public async Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody = null, CancellationToken ct = default)
    {
        var domain = toEmail.Contains('@') ? toEmail[(toEmail.LastIndexOf('@') + 1)..].ToLowerInvariant() : string.Empty;
        if (_settings.SkipDomains.Any(d => string.Equals(d, domain, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogInformation("Skipping email to fake/seed domain {Domain} ({To}): {Subject}", domain, toEmail, subject);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress, _settings.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = string.IsNullOrEmpty(_settings.Username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(_settings.Username, _settings.Password)
        };

        await client.SendMailAsync(message, ct);
    }
}
