using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Turns a ticket-holder's email into a claimable account. When a ticket/pass is created with an email
/// that has no account yet, this provisions a passwordless <b>shell account</b> (so the fan can hold a
/// wallet and be topped up at the bar immediately) and emails them a link to set a password and claim it.
/// Idempotent per email and best-effort: it runs in its own DbContext scope so it never disturbs — or is
/// rolled back by — the caller's ingestion transaction, and never throws into the caller.
/// </summary>
public interface IAccountProvisioningService
{
    /// <summary>Ensure an account exists for <paramref name="email"/>, creating a shell if not. No-op when an
    /// account already exists or the email is empty/invalid. Never throws.
    /// <para><paramref name="sendActivation"/> (default true) mints an activation token and emails the
    /// set-password link. Pass false for a <b>silent</b> shell (e.g. a bulk backfill that must not blast
    /// thousands of emails) — the fan can still claim it by registering with the same email.</para></summary>
    Task EnsureShellAccountAsync(string? email, string? fullName, string? phone, string source, bool sendActivation = true);
}

public class AccountProvisioningService : IAccountProvisioningService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountProvisioningService> _logger;

    public AccountProvisioningService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<AccountProvisioningService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnsureShellAccountAsync(string? email, string? fullName, string? phone, string source, bool sendActivation = true)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return;

            var normalized = email.Trim();
            var lower = normalized.ToLowerInvariant();

            // Fresh scope → isolated DbContext, so we don't flush the caller's half-built entity graph or
            // get rolled back if their ingestion transaction later fails.
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var existing = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == lower);
            if (existing != null)
            {
                // Account already exists (real or a shell from an earlier ticket) — nothing to create.
                // Still make sure any season passes under this email are linked to it.
                await LinkSeasonTicketsAsync(db, existing.Id, lower);
                return;
            }

            var (first, last) = SplitName(fullName);
            var shell = new User
            {
                // Email is unique, so it doubles as a guaranteed-unique username for the shell.
                Username = normalized.Length > 100 ? normalized[..100] : normalized,
                Email = normalized,
                FirstName = first,
                LastName = last,
                PhoneNumber = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
                // Not a real login — a random hash just satisfies the required column until the fan claims it.
                PasswordHash = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
                Role = UserRole.Customer,
                IsShellAccount = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            db.Users.Add(shell);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Lost a race — another ticket for the same email created the account first. Reload + link.
                db.ChangeTracker.Clear();
                var winner = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == lower);
                if (winner == null) throw;
                await LinkSeasonTicketsAsync(db, winner.Id, lower);
                return;
            }

            await LinkSeasonTicketsAsync(db, shell.Id, lower);

            // Optionally mint an activation token and email the set-password link. Skipped for a silent
            // backfill — the shell is still claimable by registering with this email (register-claim).
            if (sendActivation)
            {
                var token = AccountActivationToken.Create(shell.Id);
                db.AccountActivationTokens.Add(token);
                await db.SaveChangesAsync();
                await SendActivationEmailAsync(scope, shell, token.Token);
            }

            _logger.LogInformation("Provisioned shell account #{UserId} for {Email} (source: {Source}, email: {Sent})",
                shell.Id, normalized, source, sendActivation);
        }
        catch (Exception ex)
        {
            // Provisioning must never break ticket creation.
            _logger.LogError(ex, "Failed to provision shell account for {Email} (source: {Source})", email, source);
        }
    }

    private static async Task LinkSeasonTicketsAsync(ApplicationDbContext db, int userId, string lowerEmail)
    {
        await db.SeasonTickets
            .Where(st => st.UserId == null && st.HolderEmail != null && st.HolderEmail.ToLower() == lowerEmail)
            .ExecuteUpdateAsync(s => s.SetProperty(st => st.UserId, userId));
    }

    private async Task SendActivationEmailAsync(IServiceScope scope, User shell, string token)
    {
        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var templates = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
        var baseUrl = (_configuration["CustomerApp:BaseUrl"] ?? "https://localhost:7020").TrimEnd('/');
        var link = $"{baseUrl}/set-password?token={Uri.EscapeDataString(token)}";
        var name = $"{shell.FirstName} {shell.LastName}".Trim();
        var greeting = string.IsNullOrWhiteSpace(name) ? "Pozdrav" : $"Pozdrav {name}";

        var tokens = new Dictionary<string, string?>
        {
            ["Greeting"] = greeting,
            ["Name"] = name,
            ["ActivationLink"] = link,
            ["ExpiryDays"] = "14"
        };

        var rendered = await templates.RenderAsync(EmailTemplateCatalog.AccountActivation, tokens);
        if (rendered is null) return; // unknown key — should never happen for a catalog constant

        await emailSender.SendAsync(shell.Email, rendered.Subject, rendered.HtmlBody, rendered.TextBody);
    }

    private static (string? First, string? Last) SplitName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return (null, null);
        var parts = fullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 1 ? (parts[0], null) : (parts[0], parts[1]);
    }
}
