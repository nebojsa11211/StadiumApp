using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>Sends transactional emails (e.g. shell-account activation links). The active
/// implementation reads its SMTP configuration from the singleton <c>Venue</c> row at send time
/// (edited on the Admin settings page), falling back to the appsettings <c>Email</c> section, and
/// finally to just logging the message when nothing is configured.</summary>
public interface IEmailSender
{
    Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody = null, CancellationToken ct = default);

    /// <summary>True when a real transport is (or can be) configured. Best-effort: the actual send
    /// still falls back to logging if no SMTP host is set at send time.</summary>
    bool IsRealTransport { get; }
}

/// <summary>Appsettings fallback config for SMTP (<c>Email</c> section). Used only when the venue row
/// has no SMTP host configured, so existing deployments that set this keep working.</summary>
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

/// <summary>A fully-resolved SMTP configuration ready to send with. Built from either the venue row
/// or the appsettings fallback so both the runtime sender and the "test email" endpoint share one
/// delivery path.</summary>
public sealed record ResolvedEmailConfig(
    string Host, int Port, string? Username, string? Password,
    bool EnableSsl, string FromAddress, string FromName, string[] SkipDomains)
{
    public static ResolvedEmailConfig FromVenue(Venue venue, string[] skipDomains) => new(
        Host: venue.SmtpHost!.Trim(),
        Port: venue.SmtpPort,
        Username: venue.SmtpUsername,
        Password: venue.SmtpPassword,
        EnableSsl: venue.SmtpUseSsl,
        FromAddress: string.IsNullOrWhiteSpace(venue.EmailFromAddress) ? "no-reply@stadium.local" : venue.EmailFromAddress.Trim(),
        FromName: string.IsNullOrWhiteSpace(venue.EmailFromName) ? "Stadium" : venue.EmailFromName.Trim(),
        SkipDomains: skipDomains);

    public static ResolvedEmailConfig FromSettings(EmailSettings s) => new(
        Host: s.Host!.Trim(),
        Port: s.Port,
        Username: s.Username,
        Password: s.Password,
        EnableSsl: s.EnableSsl,
        FromAddress: s.FromAddress,
        FromName: s.FromName,
        SkipDomains: s.SkipDomains);
}

/// <summary>Raw SMTP delivery over <see cref="System.Net.Mail"/>. Shared by the runtime sender and the
/// admin "send test email" endpoint. Skips configured fake/seed domains so seed data never triggers
/// delivery.</summary>
public static class SmtpMailer
{
    /// <returns>True if the message was sent; false if it was skipped (fake/seed recipient domain).</returns>
    public static async Task<bool> SendAsync(
        ResolvedEmailConfig cfg, string toEmail, string subject, string htmlBody, string? textBody,
        ILogger logger, CancellationToken ct = default)
    {
        var domain = toEmail.Contains('@') ? toEmail[(toEmail.LastIndexOf('@') + 1)..].ToLowerInvariant() : string.Empty;
        if (cfg.SkipDomains.Any(d => string.Equals(d, domain, StringComparison.OrdinalIgnoreCase)))
        {
            logger.LogInformation("Skipping email to fake/seed domain {Domain} ({To}): {Subject}", domain, toEmail, subject);
            return false;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(cfg.FromAddress, cfg.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        using var client = new SmtpClient(cfg.Host, cfg.Port)
        {
            EnableSsl = cfg.EnableSsl,
            Credentials = string.IsNullOrEmpty(cfg.Username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(cfg.Username, cfg.Password)
        };

        await client.SendMailAsync(message, ct);
        return true;
    }
}

/// <summary>Runtime email sender. Resolves SMTP config from the venue row (then appsettings) at each
/// send; if nothing is configured or email is disabled, logs the message (with any activation link)
/// instead of sending, so the flow stays testable without SMTP credentials.</summary>
public class DbBackedEmailSender : IEmailSender
{
    private readonly ApplicationDbContext _db;
    private readonly EmailSettings _fallback;
    private readonly ILogger<DbBackedEmailSender> _logger;

    public DbBackedEmailSender(ApplicationDbContext db, EmailSettings fallback, ILogger<DbBackedEmailSender> logger)
    {
        _db = db;
        _fallback = fallback;
        _logger = logger;
    }

    public bool IsRealTransport => true;

    public async Task SendAsync(string toEmail, string subject, string htmlBody, string? textBody = null, CancellationToken ct = default)
    {
        var cfg = await ResolveConfigAsync(ct);
        if (cfg is null)
        {
            // No transport configured — log the plain-text body (or HTML fallback); it contains the link.
            var body = string.IsNullOrWhiteSpace(textBody) ? htmlBody : textBody;
            _logger.LogWarning(
                "📧 [EMAIL not sent — SMTP not configured/enabled] To: {To} | Subject: {Subject}\n{Body}",
                toEmail, subject, body);
            return;
        }

        await SmtpMailer.SendAsync(cfg, toEmail, subject, htmlBody, textBody, _logger, ct);
    }

    private async Task<ResolvedEmailConfig?> ResolveConfigAsync(CancellationToken ct)
    {
        var venue = await _db.Venues.AsNoTracking().FirstOrDefaultAsync(ct);
        if (venue is { EmailEnabled: true } && !string.IsNullOrWhiteSpace(venue.SmtpHost))
            return ResolvedEmailConfig.FromVenue(venue, _fallback.SkipDomains);

        if (!string.IsNullOrWhiteSpace(_fallback.Host))
            return ResolvedEmailConfig.FromSettings(_fallback);

        return null;
    }
}
