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
    /// thousands of emails) — the fan can still claim it by registering with the same email.</para>
    /// <para><paramref name="oib"/>, when supplied, is stamped on a newly created shell and back-filled onto
    /// an existing account that has none — so the fan's OIB (captured on the ticket) becomes resolvable at
    /// the bar even if this account was created without it.</para></summary>
    Task EnsureShellAccountAsync(string? email, string? fullName, string? phone, string source, bool sendActivation = true, string? oib = null);

    /// <summary>Bar counter: create (or reuse) a claimable account keyed on <paramref name="email"/> for a
    /// fan identified by an OIB-bearing ticket, stamping <paramref name="oib"/>, and return its user id so
    /// the cash top-up can proceed onto that account's wallet. Unlike <see cref="EnsureShellAccountAsync"/>
    /// this is synchronous to the caller and returns the id (null only if the email is unusable or creation
    /// genuinely fails). Sends the set-password activation link for a freshly created shell.</summary>
    Task<int?> ProvisionAndGetUserIdAsync(string email, string? fullName, string? phone, string? oib, string source);

    /// <summary>Provision (or reuse) a claimable account for a ticket holder identified only by
    /// <paramref name="oib"/> — no email. The shell is keyed on a synthetic, non-deliverable placeholder
    /// email derived from the OIB, so <b>no activation mail is sent</b> (there is no real address to send
    /// to). Idempotent per OIB: reuses any existing account already carrying the OIB (a real fan, or a
    /// shell provisioned from an email-bearing ticket) instead of creating a duplicate. The fan later
    /// claims it by registering with their real email + this OIB — the OIB merge in registration turns
    /// that into a claim and swaps the placeholder for their real email. Returns the user id, or null when
    /// the OIB is blank.</summary>
    Task<int?> ProvisionOibShellAndGetUserIdAsync(string? oib, string? fullName, string? phone, string source);
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

    public async Task EnsureShellAccountAsync(string? email, string? fullName, string? phone, string source, bool sendActivation = true, string? oib = null)
    {
        try
        {
            await EnsureCoreAsync(email, fullName, phone, oib, source, sendActivation);
        }
        catch (Exception ex)
        {
            // Provisioning must never break ticket creation.
            _logger.LogError(ex, "Failed to provision shell account for {Email} (source: {Source})", email, source);
        }
    }

    public async Task<int?> ProvisionAndGetUserIdAsync(string email, string? fullName, string? phone, string? oib, string source)
        // Same create-or-reuse logic, but the bar flow needs the resulting id, so exceptions surface to the
        // caller (which reports a clean failure) instead of being swallowed.
        => await EnsureCoreAsync(email, fullName, phone, oib, source, sendActivation: true);

    /// <summary>Reserved, non-routable domain for placeholder emails on OIB-only shells. An address here is
    /// never a real inbox — no mail is ever sent to it, and it is swapped for the fan's real email on claim.</summary>
    public const string OibPlaceholderEmailDomain = "oib.stadium.local";

    private static string SyntheticOibEmail(string oib) => $"{oib}@{OibPlaceholderEmailDomain}";

    public async Task<int?> ProvisionOibShellAndGetUserIdAsync(string? oib, string? fullName, string? phone, string source)
    {
        if (string.IsNullOrWhiteSpace(oib))
            return null;

        var normalizedOib = oib.Trim();

        // Fresh scope → isolated DbContext, consistent with the email path.
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Reuse any account that already carries this OIB (a registered fan, or a shell provisioned from an
        // email-bearing ticket whose OIB was stamped) rather than creating a second row for the same person.
        var existing = await db.Users.FirstOrDefaultAsync(u => u.Oib == normalizedOib);
        if (existing != null)
            return existing.Id;

        var syntheticEmail = SyntheticOibEmail(normalizedOib);
        var (first, last) = SplitName(fullName);
        var shell = new User
        {
            // Deterministic per OIB and unique (Email + Username are both unique), so repeated orders reuse
            // the same shell. The placeholder address is never emailed.
            Username = syntheticEmail,
            Email = syntheticEmail,
            FirstName = first,
            LastName = last,
            PhoneNumber = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            Oib = normalizedOib,
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
            // Lost a race — a concurrent order for the same OIB created the shell first. Reload + reuse.
            db.ChangeTracker.Clear();
            var winner = await db.Users.FirstOrDefaultAsync(u => u.Oib == normalizedOib)
                         ?? await db.Users.FirstOrDefaultAsync(u => u.Email == syntheticEmail);
            if (winner == null) throw;
            return winner.Id;
        }

        _logger.LogInformation("Provisioned OIB-only shell account #{UserId} for OIB {Oib} (source: {Source})",
            shell.Id, normalizedOib, source);
        return shell.Id;
    }

    /// <summary>Create-or-reuse the account keyed on <paramref name="email"/> and return its id. Stamps
    /// <paramref name="oib"/> on a new shell and back-fills it onto an existing account that lacks one.
    /// Returns null only when the email is empty/invalid.</summary>
    private async Task<int?> EnsureCoreAsync(string? email, string? fullName, string? phone, string? oib, string source, bool sendActivation)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return null;

        var normalized = email.Trim();
        var lower = normalized.ToLowerInvariant();
        var normalizedOib = string.IsNullOrWhiteSpace(oib) ? null : oib.Trim();

        // Fresh scope → isolated DbContext, so we don't flush the caller's half-built entity graph or
        // get rolled back if their ingestion transaction later fails.
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var existing = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == lower);
        if (existing != null)
        {
            // Account already exists (real or a shell from an earlier ticket) — don't recreate. Back-fill
            // the OIB when the account has none so the fan becomes resolvable by OIB at the bar.
            if (normalizedOib != null && string.IsNullOrWhiteSpace(existing.Oib))
            {
                existing.Oib = normalizedOib;
                await db.SaveChangesAsync();
            }
            // Still make sure any season passes under this email are linked to it.
            await LinkSeasonTicketsAsync(db, existing.Id, lower);
            return existing.Id;
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
            Oib = normalizedOib,
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
            return winner.Id;
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
        return shell.Id;
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
