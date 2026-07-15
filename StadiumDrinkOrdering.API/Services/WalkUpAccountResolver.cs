using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Decides which account a walk-up (ticket-session) drink order is attributed to. The ticket holder's
/// real account when one exists, else an auto-provisioned claimable shell — keyed on the ticket's email,
/// or (when the ticket carries only an OIB) on an OIB-derived placeholder that the fan can later claim by
/// registering with their real email + OIB. Only a genuinely identity-less ticket (no email, no OIB, no
/// season link — e.g. a cash bearer ticket) falls back to the shared "walk-up guest". Centralised so live
/// ordering and the one-off backfill of legacy guest orders attribute identically.
/// </summary>
public interface IWalkUpAccountResolver
{
    /// <summary>The account id to attribute an order for <paramref name="ticket"/> to. Never the guest when
    /// the ticket resolves to a real or shell account; falls back to the shared guest for a null or
    /// identity-less ticket.</summary>
    Task<int> ResolveCustomerIdAsync(Ticket? ticket);

    /// <summary>The shared "walk-up guest" account id, created on first use.</summary>
    Task<int> GetGuestCustomerIdAsync();
}

public class WalkUpAccountResolver : IWalkUpAccountResolver
{
    public const string GuestEmail = "walkup-guest@stadium.local";

    private readonly ApplicationDbContext _db;
    private readonly IAccountProvisioningService _accountProvisioning;
    private readonly ILogger<WalkUpAccountResolver> _logger;

    public WalkUpAccountResolver(
        ApplicationDbContext db,
        IAccountProvisioningService accountProvisioning,
        ILogger<WalkUpAccountResolver> logger)
    {
        _db = db;
        _accountProvisioning = accountProvisioning;
        _logger = logger;
    }

    public async Task<int> ResolveCustomerIdAsync(Ticket? ticket)
    {
        if (ticket is not null)
        {
            // Season pass → its linked account is the authoritative owner.
            if (ticket.SeasonTicketId is int seasonTicketId)
            {
                var ownerId = await _db.SeasonTickets
                    .Where(st => st.Id == seasonTicketId && st.UserId != null)
                    .Select(st => st.UserId)
                    .FirstOrDefaultAsync();
                if (ownerId is not null)
                    return ownerId.Value;
            }

            // Single-event ticket with an email → find-or-provision the holder's account. Idempotent per
            // email (returns the existing account without re-emailing); only a genuinely new shell triggers
            // the set-password activation mail.
            if (!string.IsNullOrWhiteSpace(ticket.CustomerEmail))
            {
                var userId = await _accountProvisioning.ProvisionAndGetUserIdAsync(
                    ticket.CustomerEmail, ticket.CustomerName, ticket.CustomerPhone, ticket.CustomerOib, "WalkUpOrder");
                if (userId is not null)
                    return userId.Value;
            }

            // No email to key a shell on, but the ticket carries an OIB → provision (or reuse) an OIB-keyed
            // claimable shell. Registration now merges by OIB, so such a shell can be claimed later by the
            // fan registering with their real email + this OIB — it won't orphan or duplicate their account.
            if (!string.IsNullOrWhiteSpace(ticket.CustomerOib))
            {
                var userId = await _accountProvisioning.ProvisionOibShellAndGetUserIdAsync(
                    ticket.CustomerOib, ticket.CustomerName, ticket.CustomerPhone, "WalkUpOrder");
                if (userId is not null)
                    return userId.Value;
            }
        }

        // Only a ticket with neither email, OIB, nor a season link (a truly anonymous cash bearer ticket)
        // falls back to the shared guest.
        return await GetGuestCustomerIdAsync();
    }

    public async Task<int> GetGuestCustomerIdAsync()
    {
        var guest = await _db.Users.FirstOrDefaultAsync(u => u.Email == GuestEmail);
        if (guest is not null)
            return guest.Id;

        guest = new User
        {
            Username = "Walk-up Guest",
            Email = GuestEmail,
            PasswordHash = "!disabled!", // non-usable; this account never logs in
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _db.Users.Add(guest);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Created walk-up guest customer #{Id}", guest.Id);
        return guest.Id;
    }
}
