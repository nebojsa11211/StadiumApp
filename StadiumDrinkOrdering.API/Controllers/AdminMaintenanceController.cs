using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Destructive admin maintenance operations. Admin-only. Used from the Settings page to wipe all
/// transactional data (tickets, orders, events, seasons and the entire wallet ledger) while leaving
/// the stadium structure, users, drinks and venue branding intact.
/// </summary>
[Route("api/admin/maintenance")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class AdminMaintenanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminMaintenanceController> _logger;

    public AdminMaintenanceController(ApplicationDbContext context, ILogger<AdminMaintenanceController> logger)
    {
        _context = context;
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

                var total = 0;

                // 1. Sessions that RESTRICT-reference tickets/events must go first.
                total += await _context.TicketSessions.ExecuteDeleteAsync();
                total += await _context.OrderSessions.ExecuteDeleteAsync();

                // 2. Order graph. Payments RESTRICT-reference wallet transactions, so clear them
                //    (and the order items) before both orders and the wallet ledger.
                total += await _context.OrderItems.ExecuteDeleteAsync();
                total += await _context.Payments.ExecuteDeleteAsync();
                result.Orders = await _context.Orders.ExecuteDeleteAsync();
                total += result.Orders;

                // 3. Tickets (now free of session/order references) before their events/seasons.
                result.Tickets = await _context.Tickets.ExecuteDeleteAsync();
                total += result.Tickets;

                // 4. Season passes then seasons.
                total += await _context.SeasonTickets.ExecuteDeleteAsync();
                result.Seasons = await _context.Seasons.ExecuteDeleteAsync();
                total += result.Seasons;

                // 5. Event-owned data, then the events. (Analytics, staff assignments, sector
                //    prices, cart items and seat reservations cascade, but we remove them
                //    explicitly so the run does not depend on DB cascade wiring.)
                total += await _context.EventAnalytics.ExecuteDeleteAsync();
                total += await _context.EventStaffAssignments.ExecuteDeleteAsync();
                total += await _context.EventSectorPrices.ExecuteDeleteAsync();
                total += await _context.CartItems.ExecuteDeleteAsync();
                total += await _context.SeatReservations.ExecuteDeleteAsync();
                result.Events = await _context.Events.ExecuteDeleteAsync();
                total += result.Events;

                // 6. Wallet ledger then balances (payments referencing transactions are already gone).
                result.WalletTransactions = await _context.WalletTransactions.ExecuteDeleteAsync();
                total += result.WalletTransactions;
                result.Wallets = await _context.Wallets.ExecuteDeleteAsync();
                total += result.Wallets;

                result.TotalRowsDeleted = total;

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
