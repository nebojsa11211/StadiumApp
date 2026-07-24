using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Destructive admin maintenance operations. Admin-only. Used from the Settings page to wipe
/// transactional data (the "purge") or perform a scoped factory reset that can also remove the
/// stadium structure, catalog, directories and every non-admin account — always keeping the admin
/// performing the reset so they can still log in.
/// </summary>
[Route("api/admin/maintenance")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class AdminMaintenanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStadiumLayoutService _stadiumLayoutService;
    private readonly ILogger<AdminMaintenanceController> _logger;

    public AdminMaintenanceController(
        ApplicationDbContext context,
        IStadiumLayoutService stadiumLayoutService,
        ILogger<AdminMaintenanceController> logger)
    {
        _context = context;
        _stadiumLayoutService = stadiumLayoutService;
        _logger = logger;
    }

    /// <summary>
    /// Deletes ALL tickets, orders, events, seasons, wallet transactions and wallet balances.
    /// Runs inside a single transaction (via the retry-safe execution strategy) so it either wipes
    /// everything or nothing. Children are removed before parents to respect the RESTRICT foreign
    /// keys the model uses (sessions → tickets, payments → wallet transactions, tickets → events…).
    /// </summary>
    [HttpPost("purge-transactional-data")]
    public async Task<ActionResult<PurgeDataResultDto>> PurgeTransactionalData()
    {
        var result = new PurgeDataResultDto();

        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();
                result.TotalRowsDeleted = await DeleteOperationalDataAsync(result);
                await tx.CommitAsync();
            });

            _logger.LogWarning(
                "Admin purge removed all transactional data. Tickets={Tickets}, Orders={Orders}, " +
                "Events={Events}, Seasons={Seasons}, WalletTransactions={WalletTransactions}, " +
                "Wallets={Wallets}, TotalRows={Total}",
                result.Tickets, result.Orders, result.Events, result.Seasons,
                result.WalletTransactions, result.Wallets, result.TotalRowsDeleted);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admin purge of transactional data failed and was rolled back");
            return StatusCode(500, new { message = "Failed to delete data. No changes were made.", error = ex.Message });
        }
    }

    /// <summary>
    /// Scoped factory reset. Each selected area is wiped inside one transaction (all-or-nothing).
    /// The account performing the reset is NEVER deleted, and the run refuses to commit unless at
    /// least one Admin account remains — so there is always a way to log back in.
    /// </summary>
    [HttpPost("factory-reset")]
    public async Task<ActionResult<FactoryResetResultDto>> FactoryReset([FromBody] FactoryResetRequestDto request)
    {
        // Identify the acting admin so we can exclude their account by id, independent of role filters.
        if (!int.TryParse(User.GetUserIdFromClaims(), out var actingAdminId))
            return Unauthorized(new { message = "Could not identify the acting admin from the token." });

        // Any deeper area requires the operational layer cleared first: tickets/orders/prepared-by
        // RESTRICT-reference the sectors, seats, drinks, teams and users those areas remove. We force
        // it server-side rather than trusting the client's checkbox state.
        var wipeOperational = request.Operational
            || request.StadiumStructure || request.Users || request.Catalog || request.Directories;

        if (!wipeOperational && !request.ResetToFirstRun)
            return BadRequest(new { message = "Select at least one area to reset." });

        var result = new FactoryResetResultDto { ResetToFirstRun = request.ResetToFirstRun };

        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync();

                if (wipeOperational)
                    result.Operational = await DeleteOperationalDataAsync();

                if (request.StadiumStructure)
                    result.StadiumStructure = await DeleteStadiumStructureAsync();

                if (request.Catalog)
                    result.Catalog = await DeleteCatalogAsync();

                if (request.Directories)
                    result.Directories = await DeleteDirectoriesAsync();

                if (request.Users)
                    result.Users = await DeleteNonAdminUsersAsync(actingAdminId);

                // Safety net: never let a reset commit with zero admins. The acting admin is always
                // excluded above, so this can only fire if the DB somehow had no Admin at all.
                if (!await _context.Users.AnyAsync(u => u.Role == UserRole.Admin))
                {
                    _context.Users.Add(BuildDefaultAdmin());
                    await _context.SaveChangesAsync();
                }
                result.AdminPreserved = true;

                if (request.ResetToFirstRun)
                {
                    // Re-arm the first-run setup gate so the admin is guided through rebuilding.
                    var venue = await _context.Venues.OrderBy(v => v.Id).FirstOrDefaultAsync();
                    if (venue is not null)
                    {
                        venue.SetupDismissed = false;
                        await _context.SaveChangesAsync();
                    }
                }

                result.TotalRowsDeleted = result.Operational + result.StadiumStructure
                    + result.Catalog + result.Directories + result.Users;

                await tx.CommitAsync();
            });

            // Structure/overlay data feeds a cached layout — drop it so the next render rebuilds clean.
            await _stadiumLayoutService.RefreshLayoutCacheAsync();

            _logger.LogWarning(
                "Admin factory-reset by user #{AdminId}. Operational={Op}, Structure={St}, Catalog={Cat}, " +
                "Directories={Dir}, Users={Usr}, ResetToFirstRun={Reset}, TotalRows={Total}",
                actingAdminId, result.Operational, result.StadiumStructure, result.Catalog,
                result.Directories, result.Users, request.ResetToFirstRun, result.TotalRowsDeleted);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admin factory-reset failed and was rolled back");
            return StatusCode(500, new { message = "Failed to reset. No changes were made.", error = ex.Message });
        }
    }

    /// <summary>
    /// Removes the whole operational layer (tickets, orders, events, seasons, sessions, carts,
    /// reservations and the wallet ledger) in child→parent order. Populates <paramref name="detail"/>
    /// with the notable per-table counts when the caller wants them. Assumes it runs inside a transaction.
    /// </summary>
    private async Task<int> DeleteOperationalDataAsync(PurgeDataResultDto? detail = null)
    {
        var total = 0;

        // 1. Sessions that RESTRICT-reference tickets/events must go first.
        total += await _context.TicketSessions.ExecuteDeleteAsync();
        total += await _context.OrderSessions.ExecuteDeleteAsync();

        // 2. Order graph. Payments RESTRICT-reference wallet transactions, so clear them
        //    (and the order items) before both orders and the wallet ledger.
        total += await _context.OrderItems.ExecuteDeleteAsync();
        total += await _context.Payments.ExecuteDeleteAsync();
        var orders = await _context.Orders.ExecuteDeleteAsync();
        total += orders;

        // 3. Tickets (now free of session/order references) before their events/seasons.
        var tickets = await _context.Tickets.ExecuteDeleteAsync();
        total += tickets;

        // 4. Season passes then seasons.
        total += await _context.SeasonTickets.ExecuteDeleteAsync();
        var seasons = await _context.Seasons.ExecuteDeleteAsync();
        total += seasons;

        // 5. Event-owned data, then the events. (Analytics, staff assignments, sector prices, cart
        //    items and seat reservations cascade, but we remove them explicitly so the run does not
        //    depend on DB cascade wiring.)
        total += await _context.EventAnalytics.ExecuteDeleteAsync();
        total += await _context.EventStaffAssignments.ExecuteDeleteAsync();
        total += await _context.EventSectorPrices.ExecuteDeleteAsync();
        total += await _context.CartItems.ExecuteDeleteAsync();
        total += await _context.SeatReservations.ExecuteDeleteAsync();
        var events = await _context.Events.ExecuteDeleteAsync();
        total += events;

        // 6. Wallet ledger then balances (payments referencing transactions are already gone).
        var walletTransactions = await _context.WalletTransactions.ExecuteDeleteAsync();
        total += walletTransactions;
        var wallets = await _context.Wallets.ExecuteDeleteAsync();
        total += wallets;

        if (detail is not null)
        {
            detail.Orders = orders;
            detail.Tickets = tickets;
            detail.Seasons = seasons;
            detail.Events = events;
            detail.WalletTransactions = walletTransactions;
            detail.Wallets = wallets;
        }

        return total;
    }

    /// <summary>
    /// Removes the entire stadium structure — the drawing-tool overlay model (seats → overlays →
    /// sectors → rings → tribunes) and the legacy seed tables (seats → sections, plus the oldest
    /// StadiumSeats). Operational rows that reference seats (tickets, reservations) must already be gone.
    /// </summary>
    private async Task<int> DeleteStadiumStructureAsync()
    {
        var total = 0;

        // Overlay/hierarchy model: seats reference both a sector and an overlay (cascade), so first.
        total += await _context.StadiumSeatsNew.ExecuteDeleteAsync();
        total += await _context.StadiumSectorOverlays.ExecuteDeleteAsync();
        total += await _context.Sectors.ExecuteDeleteAsync();
        total += await _context.Rings.ExecuteDeleteAsync();
        total += await _context.Tribunes.ExecuteDeleteAsync();

        // Legacy tables: Seat RESTRICT-references StadiumSection, so seats before sections.
        total += await _context.Seats.ExecuteDeleteAsync();
        total += await _context.StadiumSections.ExecuteDeleteAsync();
        total += await _context.StadiumSeats.ExecuteDeleteAsync();

        return total;
    }

    /// <summary>Removes the drink catalog. Drink RESTRICT-references its category, so drinks (and the
    /// stock-movement ledger that references them) go before categories.</summary>
    private async Task<int> DeleteCatalogAsync()
    {
        var total = 0;
        total += await _context.StockMovements.ExecuteDeleteAsync();
        total += await _context.Drinks.ExecuteDeleteAsync();
        total += await _context.Categories.ExecuteDeleteAsync();
        return total;
    }

    /// <summary>Removes the team and club directories. Events (which SetNull-reference them) are already
    /// gone; clubs are cascade-children of the venue, which survives.</summary>
    private async Task<int> DeleteDirectoriesAsync()
    {
        var total = 0;
        total += await _context.Teams.ExecuteDeleteAsync();
        total += await _context.Clubs.ExecuteDeleteAsync();
        return total;
    }

    /// <summary>
    /// Removes every account except the acting admin, plus the disposable per-account state (tokens,
    /// notifications, carts) and the brute-force ledger. The walk-up guest is among those deleted and is
    /// re-created lazily on the next walk-up order. Clearing all refresh tokens also forces a clean
    /// re-login for the acting admin. Order/season/wallet rows referencing users are already gone.
    /// </summary>
    private async Task<int> DeleteNonAdminUsersAsync(int actingAdminId)
    {
        var total = 0;

        total += await _context.RefreshTokens.ExecuteDeleteAsync();
        total += await _context.AccountActivationTokens.ExecuteDeleteAsync();
        total += await _context.Notifications.ExecuteDeleteAsync();
        total += await _context.ShoppingCarts.ExecuteDeleteAsync();
        total += await _context.FailedAttempts.ExecuteDeleteAsync();
        total += await _context.AccountLockouts.ExecuteDeleteAsync();
        total += await _context.IPBans.ExecuteDeleteAsync();

        total += await _context.Users.Where(u => u.Id != actingAdminId).ExecuteDeleteAsync();

        return total;
    }

    /// <summary>The default admin used only by the zero-admin safety net (never expected to fire, since
    /// the acting admin is always preserved). Mirrors the seed admin created on startup.</summary>
    private static User BuildDefaultAdmin()
    {
        const string adminEmail = "nebojsa.medancic+adminStadion@gmail.com";
        return new User
        {
            Username = adminEmail,
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// One-off: re-attribute existing walk-up (ticket-session) orders that are still pinned to the shared
    /// "walk-up guest" account to the real account behind their ticket — the same resolution new orders
    /// now use (season owner → email account, provisioning a claimable shell when the ticket carries an
    /// email but has none yet). Orders whose ticket carries no resolvable identity stay on the guest.
    /// Idempotent: re-running only touches orders still on the guest, so it is safe to run repeatedly.
    /// </summary>
    [HttpPost("backfill-walkup-order-accounts")]
    public async Task<ActionResult> BackfillWalkUpOrderAccounts([FromServices] IWalkUpAccountResolver resolver)
    {
        var guestId = await resolver.GetGuestCustomerIdAsync();

        // Only guest-attributed orders that still carry their ticket link can be re-resolved.
        var guestOrders = await _context.Orders
            .Where(o => o.CustomerId == guestId && o.TicketSessionId != null)
            .Include(o => o.TicketSession!)
                .ThenInclude(ts => ts.Ticket)
            .ToListAsync();

        var reattributed = 0;
        foreach (var order in guestOrders)
        {
            var ticket = order.TicketSession?.Ticket;
            if (ticket is null)
                continue;

            var resolvedId = await resolver.ResolveCustomerIdAsync(ticket);
            if (resolvedId != guestId)
            {
                order.CustomerId = resolvedId;
                reattributed++;
            }
        }

        if (reattributed > 0)
            await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Backfill re-attributed {Reattributed} walk-up order(s) from the guest account to real accounts (of {Candidates} guest-attributed candidates).",
            reattributed, guestOrders.Count);

        return Ok(new { reattributed, candidates = guestOrders.Count });
    }
}
