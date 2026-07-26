using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Reusable-cup operations that happen after an order is placed: processing returns (refunding held
/// deposits), honor-system returns, the breakage sweep (forfeiting deposits past the refund window),
/// and the admin dashboard figures. Issuing cups at order time lives in <see cref="OrderService"/>.
/// The wallet-credit refund path (the configured default) is fully implemented here; see
/// docs/reusable-cups-design.md.
/// </summary>
public interface ICupService
{
    /// <summary>Resolve a scanned/typed value to a ticket and report its outstanding cup deposits.</summary>
    Task<CupReturnLookupDto> LookupAsync(string query);

    /// <summary>Refund up to <paramref name="count"/> of the ticket's oldest held deposits, crediting the
    /// ticket's wallet. Idempotent per deposit (already-refunded deposits are simply not re-selected).</summary>
    Task<CupReturnResultDto> ReturnDepositsAsync(string query, int count, string? idempotencyKey, int staffUserId);

    /// <summary>Record a bulk return of honor-system cups (no money moves) — decrements the outstanding
    /// count for shrinkage tracking. Returns how many were actually recorded.</summary>
    Task<int> ReturnHonorCupsAsync(int count, int staffUserId, string? staffEmail);

    /// <summary>Forfeit held deposits whose refund window has elapsed (their event is over), turning the
    /// liability into breakage. Returns the number forfeited. Respects the venue's configured window.</summary>
    Task<int> SweepForfeitedDepositsAsync();

    /// <summary>Reporting snapshot for the admin cup dashboard.</summary>
    Task<CupDashboardDto> GetDashboardAsync();

    /// <summary>Per-event cup breakdown for the admin dashboard.</summary>
    Task<List<CupEventReportDto>> GetEventReportAsync();

    /// <summary>Bind a scanned physical cup's QR to the ticket's oldest still-unbound held deposit
    /// (cup-QR binding), so the deposit can later be returned by scanning the cup itself.</summary>
    Task<CupAssignResultDto> AssignCupAsync(string query, string cupQrToken, int staffUserId);
}

public class CupService : ICupService
{
    /// <summary>Fungible-pool cup type used for all Phase-1 deposit/honor cups (seeded row).</summary>
    private const int DefaultCupTypeId = 1;

    private readonly ApplicationDbContext _context;
    private readonly IWalletService _walletService;
    private readonly ILogger<CupService> _logger;

    public CupService(ApplicationDbContext context, IWalletService walletService, ILogger<CupService> logger)
    {
        _context = context;
        _walletService = walletService;
        _logger = logger;
    }

    public async Task<CupReturnLookupDto> LookupAsync(string query)
    {
        var (ticket, _) = await ResolveReturnTargetAsync(query);
        if (ticket is null)
            return new CupReturnLookupDto { Found = false, Error = "No ticket or cup matched that code." };

        var held = await _context.CupDeposits
            .Where(d => d.TicketId == ticket.Id && d.Status == CupDepositStatus.Held)
            .ToListAsync();

        return new CupReturnLookupDto
        {
            Found = true,
            TicketId = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            HolderName = string.IsNullOrWhiteSpace(ticket.CustomerEmail) ? ticket.TicketNumber : ticket.CustomerEmail,
            OutstandingCups = held.Count,
            OutstandingAmount = held.Sum(d => d.Amount)
        };
    }

    public async Task<CupReturnResultDto> ReturnDepositsAsync(string query, int count, string? idempotencyKey, int staffUserId)
    {
        if (count < 1)
            return new CupReturnResultDto { Success = false, Error = "Nothing to return." };

        var (ticket, specificDepositId) = await ResolveReturnTargetAsync(query);
        if (ticket is null)
            return new CupReturnResultDto { Success = false, Error = "No ticket or cup matched that code." };

        // Oldest held deposits first, capped at how many cups the customer is handing back. When the scanned
        // value was a specific cup's QR (cup-QR binding), return only that cup's deposit.
        var q = _context.CupDeposits.Where(d => d.TicketId == ticket.Id && d.Status == CupDepositStatus.Held);
        if (specificDepositId is int sid)
            q = q.Where(d => d.Id == sid);
        var toRefund = await q
            .OrderBy(d => d.Id)
            .Take(specificDepositId is null ? count : 1)
            .Select(d => new { d.Id, d.Amount, d.WalletId })
            .ToListAsync();

        if (toRefund.Count == 0)
            return new CupReturnResultDto { Success = true, RefundedCups = 0, RefundedAmount = 0m };

        // Each deposit is refunded to the wallet that PAID it (refund-to-original-wallet). Deposits paid
        // offline (cash/card) carry no wallet, so those fall back to the ticket's own bearer wallet, created
        // lazily on first use, so whoever holds the ticket gets the money back.
        int? ticketWalletId = null;

        // Outstanding-in-the-wild count, decremented locally as each cup comes back. WalletService.RefundAsync
        // commits and clears the change tracker, so each deposit's status + ledger rows are written in their
        // own SaveChanges after the refund succeeds.
        var outstanding = await CurrentOutstandingAsync();
        var refundedCups = 0;
        var refundedAmount = 0m;
        var lastWalletId = 0;

        foreach (var dep in toRefund)
        {
            var targetWalletId = dep.WalletId ?? (ticketWalletId ??= await GetOrCreateTicketWalletIdAsync(ticket.Id));

            var txn = await _walletService.RefundAsync(
                targetWalletId, dep.Amount, idempotencyKey: $"cup-refund-{dep.Id}",
                referenceType: "CupDeposit", referenceId: dep.Id,
                description: "Reusable cup deposit refund", actorUserId: staffUserId);

            if (txn is null)
                continue; // wallet closed/frozen or transient failure — leave the deposit held

            var deposit = await _context.CupDeposits.FirstOrDefaultAsync(d => d.Id == dep.Id && d.Status == CupDepositStatus.Held);
            if (deposit is null)
                continue; // refunded concurrently — the credit above is idempotent, so no double pay

            deposit.Status = CupDepositStatus.Refunded;
            deposit.RefundTransactionId = txn.Id;
            deposit.WalletId = targetWalletId;
            deposit.ResolvedAt = DateTime.UtcNow;
            lastWalletId = targetWalletId;

            outstanding -= 1;
            _context.CupMovements.Add(new CupMovement
            {
                CupTypeId = DefaultCupTypeId,
                Delta = -1,
                QuantityAfter = outstanding,
                Type = CupMovementType.Returned,
                Mode = CupMode.Deposit,
                TicketId = ticket.Id,
                CupDepositId = deposit.Id,
                UserId = staffUserId,
                CreatedAt = DateTime.UtcNow
            });
            _context.CupMovements.Add(new CupMovement
            {
                CupTypeId = DefaultCupTypeId,
                Delta = 0,
                QuantityAfter = outstanding,
                Type = CupMovementType.DepositRefunded,
                Mode = CupMode.Deposit,
                TicketId = ticket.Id,
                CupDepositId = deposit.Id,
                UserId = staffUserId,
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            refundedCups++;
            refundedAmount += dep.Amount;
        }

        var balance = lastWalletId != 0
            ? await _context.Wallets.Where(w => w.Id == lastWalletId).Select(w => w.Balance).FirstOrDefaultAsync()
            : 0m;

        return new CupReturnResultDto
        {
            Success = true,
            RefundedCups = refundedCups,
            RefundedAmount = refundedAmount,
            WalletBalance = balance
        };
    }

    public async Task<int> ReturnHonorCupsAsync(int count, int staffUserId, string? staffEmail)
    {
        if (count < 1)
            return 0;

        var outstanding = await CurrentOutstandingAsync();
        // Never drive the count below zero (over-return / miscount).
        var delta = Math.Min(count, Math.Max(outstanding, 0));
        if (delta == 0)
            return 0;

        outstanding -= delta;
        _context.CupMovements.Add(new CupMovement
        {
            CupTypeId = DefaultCupTypeId,
            Delta = -delta,
            QuantityAfter = outstanding,
            Type = CupMovementType.Returned,
            Mode = CupMode.HonorSystem,
            UserId = staffUserId,
            UserEmail = staffEmail,
            Note = "Honor-system cups returned",
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        return delta;
    }

    public async Task<int> SweepForfeitedDepositsAsync()
    {
        var window = await _context.Venues.Select(v => v.CupRefundWindow).FirstOrDefaultAsync();
        if (window == CupRefundWindow.NoExpiry)
            return 0;

        // Held deposits whose event is over. EndOfEvent forfeits as soon as the event completes; the
        // +24h window keeps them refundable for a grace day after the event date.
        var candidates = await (
            from d in _context.CupDeposits
            where d.Status == CupDepositStatus.Held && d.OrderId != null
            join o in _context.Orders on d.OrderId equals o.Id
            join e in _context.Events on o.EventId equals e.Id
            where e.Status == EventStatus.Completed
            select new { d.Id, e.EventDate }).ToListAsync();

        var cutoff = DateTime.UtcNow.AddHours(-24);
        var ids = window == CupRefundWindow.EndOfEventPlus24h
            ? candidates.Where(c => c.EventDate < cutoff).Select(c => c.Id).ToList()
            : candidates.Select(c => c.Id).ToList();

        if (ids.Count == 0)
            return 0;

        // Forfeiting doesn't change the physical count (the cup is still out), only the liability, so the
        // ledger entries carry Delta 0 and the outstanding snapshot is unchanged.
        var outstanding = await CurrentOutstandingAsync();
        var deposits = await _context.CupDeposits
            .Where(d => ids.Contains(d.Id) && d.Status == CupDepositStatus.Held)
            .ToListAsync();

        foreach (var d in deposits)
        {
            d.Status = CupDepositStatus.Forfeited;
            d.ResolvedAt = DateTime.UtcNow;
            _context.CupMovements.Add(new CupMovement
            {
                CupTypeId = DefaultCupTypeId,
                Delta = 0,
                QuantityAfter = outstanding,
                Type = CupMovementType.Forfeited,
                Mode = CupMode.Deposit,
                TicketId = d.TicketId,
                OrderId = d.OrderId,
                CupDepositId = d.Id,
                Note = "Deposit forfeited (refund window elapsed)",
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Cup breakage sweep forfeited {Count} deposits ({Window}).", deposits.Count, window);
        return deposits.Count;
    }

    public async Task<CupDashboardDto> GetDashboardAsync()
    {
        var issued = await _context.CupMovements.Where(m => m.Type == CupMovementType.Issued)
            .Select(m => (int?)m.Delta).SumAsync() ?? 0;
        var returnedNeg = await _context.CupMovements.Where(m => m.Type == CupMovementType.Returned)
            .Select(m => (int?)m.Delta).SumAsync() ?? 0;
        var returned = -returnedNeg;
        var outstanding = await CurrentOutstandingAsync();

        var liability = await _context.CupDeposits.Where(d => d.Status == CupDepositStatus.Held)
            .Select(d => (decimal?)d.Amount).SumAsync() ?? 0m;
        var refundedCount = await _context.CupDeposits.CountAsync(d => d.Status == CupDepositStatus.Refunded);
        var refundedAmt = await _context.CupDeposits.Where(d => d.Status == CupDepositStatus.Refunded)
            .Select(d => (decimal?)d.Amount).SumAsync() ?? 0m;
        var forfeitedCount = await _context.CupDeposits.CountAsync(d => d.Status == CupDepositStatus.Forfeited);
        var forfeitedAmt = await _context.CupDeposits.Where(d => d.Status == CupDepositStatus.Forfeited)
            .Select(d => (decimal?)d.Amount).SumAsync() ?? 0m;

        var unitCost = await _context.CupTypes.Where(c => c.Id == DefaultCupTypeId)
            .Select(c => (decimal?)c.UnitCost).FirstOrDefaultAsync() ?? 0m;

        // BYOC (bring-your-own-cup) servings: single-use cups avoided, plus the discount handed out.
        // The discount is the difference the BYOC line was reduced by (UnitPrice*Qty − the stored TotalPrice).
        var cupsSaved = await _context.OrderItems.Where(oi => oi.CupMode == CupMode.ByocQr)
            .Select(oi => (int?)oi.Quantity).SumAsync() ?? 0;
        var byocDiscount = await _context.OrderItems.Where(oi => oi.CupMode == CupMode.ByocQr)
            .Select(oi => (decimal?)(oi.UnitPrice * oi.Quantity - oi.TotalPrice)).SumAsync() ?? 0m;

        return new CupDashboardDto
        {
            OutstandingCups = outstanding,
            OutstandingDepositLiability = liability,
            CupsIssued = issued,
            CupsReturned = returned,
            ReturnRatePercent = issued > 0 ? Math.Round((double)returned / issued * 100, 1) : 0,
            DepositsRefundedCount = refundedCount,
            DepositsRefundedAmount = refundedAmt,
            DepositsForfeitedCount = forfeitedCount,
            DepositsForfeitedAmount = forfeitedAmt,
            ShrinkageCost = forfeitedCount * unitCost,
            CupsSaved = cupsSaved,
            ByocDiscountGiven = byocDiscount
        };
    }

    public async Task<List<CupEventReportDto>> GetEventReportAsync()
    {
        // Deposits attributed to an event via order → event; grouped in memory to keep the SQL simple.
        var depRows = await (
            from d in _context.CupDeposits
            where d.OrderId != null
            join o in _context.Orders on d.OrderId equals o.Id
            join e in _context.Events on o.EventId equals e.Id
            select new { EventId = e.Id, e.EventName, d.Status, d.Amount }).ToListAsync();

        var byocRows = await (
            from oi in _context.OrderItems
            where oi.CupMode == CupMode.ByocQr
            join o in _context.Orders on oi.OrderId equals o.Id
            join e in _context.Events on o.EventId equals e.Id
            select new { EventId = e.Id, e.EventName, oi.Quantity }).ToListAsync();

        var byEvent = new Dictionary<int, CupEventReportDto>();
        CupEventReportDto Row(int id, string name) =>
            byEvent.TryGetValue(id, out var r) ? r : byEvent[id] = new CupEventReportDto { EventId = id, EventName = name };

        foreach (var d in depRows)
        {
            var r = Row(d.EventId, d.EventName);
            switch (d.Status)
            {
                case CupDepositStatus.Held: r.DepositsHeld++; r.DepositLiability += d.Amount; break;
                case CupDepositStatus.Refunded: r.DepositsRefunded++; break;
                case CupDepositStatus.Forfeited: r.DepositsForfeited++; break;
            }
        }
        foreach (var b in byocRows)
            Row(b.EventId, b.EventName).CupsSaved += b.Quantity;

        return byEvent.Values
            .OrderByDescending(r => r.DepositLiability)
            .ThenByDescending(r => r.CupsSaved)
            .ThenBy(r => r.EventName)
            .ToList();
    }

    public async Task<CupAssignResultDto> AssignCupAsync(string query, string cupQrToken, int staffUserId)
    {
        var cup = (cupQrToken ?? string.Empty).Trim();
        if (cup.Length == 0)
            return new CupAssignResultDto { Success = false, Error = "Scan the cup's QR." };

        var ticket = await ResolveTicketAsync(query);
        if (ticket is null)
            return new CupAssignResultDto { Success = false, Error = "No ticket matched that code." };

        var holder = string.IsNullOrWhiteSpace(ticket.CustomerEmail) ? ticket.TicketNumber : ticket.CustomerEmail;

        // Don't bind the same cup QR to two live deposits.
        if (await _context.CupDeposits.AnyAsync(d => d.CupQrToken == cup && d.Status == CupDepositStatus.Held))
            return new CupAssignResultDto { Success = false, Error = "That cup is already assigned to a held deposit.", HolderName = holder };

        var deposit = await _context.CupDeposits
            .Where(d => d.TicketId == ticket.Id && d.Status == CupDepositStatus.Held && d.CupQrToken == null)
            .OrderBy(d => d.Id)
            .FirstOrDefaultAsync();
        if (deposit is null)
            return new CupAssignResultDto { Success = false, Error = "No unassigned cup deposits on this ticket.", HolderName = holder };

        deposit.CupQrToken = cup;
        await _context.SaveChangesAsync();

        var remaining = await _context.CupDeposits
            .CountAsync(d => d.TicketId == ticket.Id && d.Status == CupDepositStatus.Held && d.CupQrToken == null);

        return new CupAssignResultDto { Success = true, Assigned = true, RemainingUnassigned = remaining, HolderName = holder };
    }

    private async Task<int> CurrentOutstandingAsync() =>
        await _context.CupMovements.Where(m => m.CupTypeId == DefaultCupTypeId)
            .Select(m => (int?)m.Delta).SumAsync() ?? 0;

    private async Task<Ticket?> ResolveTicketAsync(string query)
    {
        query = (query ?? string.Empty).Trim();
        if (query.Length == 0)
            return null;
        return await _context.Tickets.FirstOrDefaultAsync(t => t.QRCodeToken == query)
            ?? await _context.Tickets.FirstOrDefaultAsync(t => t.TicketNumber == query);
    }

    /// <summary>Resolve a scanned/typed value for a return: first as a bound cup QR (→ that cup's held
    /// deposit + its ticket), otherwise as a ticket. The second item is the specific deposit id when a cup
    /// QR matched, so the return refunds just that cup.</summary>
    private async Task<(Ticket? ticket, int? specificDepositId)> ResolveReturnTargetAsync(string query)
    {
        query = (query ?? string.Empty).Trim();
        if (query.Length == 0)
            return (null, null);

        var dep = await _context.CupDeposits
            .Where(d => d.CupQrToken == query && d.Status == CupDepositStatus.Held)
            .OrderBy(d => d.Id)
            .Select(d => new { d.Id, d.TicketId })
            .FirstOrDefaultAsync();
        if (dep?.TicketId is int tid)
        {
            var t = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == tid);
            if (t is not null)
                return (t, dep.Id);
        }

        return (await ResolveTicketAsync(query), null);
    }

    /// <summary>Return the id of the ticket's bearer wallet, creating an Active zero-balance one if the
    /// ticket has never had a wallet (so a cash/card-paid deposit can still be refunded as wallet credit).</summary>
    private async Task<int> GetOrCreateTicketWalletIdAsync(int ticketId)
    {
        var existing = await _context.Wallets
            .Where(w => w.OwnerType == WalletOwnerType.Ticket && w.TicketId == ticketId)
            .Select(w => (int?)w.Id)
            .FirstOrDefaultAsync();
        if (existing is not null)
            return existing.Value;

        var wallet = new Wallet
        {
            OwnerType = WalletOwnerType.Ticket,
            TicketId = ticketId,
            Balance = 0m,
            Currency = "EUR",
            Status = WalletStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
        return wallet.Id;
    }
}
