using Microsoft.EntityFrameworkCore;
using Npgsql;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IWalletService
{
    /// <summary>True if the user may hold a wallet (has ≥1 non-cancelled season ticket linked).</summary>
    Task<bool> IsEligibleAsync(int userId);

    /// <summary>Wallet snapshot for the fan, including eligibility and whether a wallet exists yet.</summary>
    Task<WalletSummaryDto> GetSummaryAsync(int userId);

    /// <summary>Returns the user's wallet, creating it on first use if the user is eligible.
    /// Returns null if the user is not eligible and has no wallet.</summary>
    Task<Wallet?> GetOrCreateForUserAsync(int userId);

    Task<WalletTransactionListDto> GetTransactionsAsync(int userId, int page, int pageSize);

    /// <summary>Add funds. Idempotent on <see cref="InitiateDepositDto.IdempotencyKey"/>. With the mock
    /// gateway the deposit captures and credits immediately.</summary>
    Task<DepositResultDto> InitiateDepositAsync(int userId, InitiateDepositDto dto);

    /// <summary>Verify + process a provider deposit webhook (async gateway). Returns true if it was a
    /// recognised wallet-deposit settlement. Throws on an invalid signature (caller maps to HTTP 400).</summary>
    Task<bool> HandleDepositWebhookAsync(string payload, string signatureHeader);

    /// <summary>Settlement state of one async deposit intent for the given fan. Reports Completed only once
    /// the webhook has credited the wallet — a definitive success signal the browser can poll after
    /// confirming the card. Scoped to the user's own wallet.</summary>
    Task<DepositStatusDto> GetDepositStatusAsync(int userId, string providerIntentId);

    /// <summary>Atomically debit the wallet for a purchase. Non-negative balance is guaranteed and the
    /// operation is idempotent on <paramref name="idempotencyKey"/> (safe to retry). Used by the order
    /// spend path.</summary>
    Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> TryDebitAsync(
        int userId, decimal amount, string idempotencyKey, string referenceType, int? referenceId, string description);

    /// <summary>Credit funds back (e.g. cancelled order). Idempotent on <paramref name="idempotencyKey"/>.</summary>
    Task<WalletTransaction?> RefundAsync(
        int walletId, decimal amount, string idempotencyKey, string referenceType, int? referenceId, string description, int? actorUserId);

    /// <summary>Credit cash onto a fan's wallet at the bar counter, performed by a staff member. Works for
    /// ANY registered account (no season-ticket eligibility gate), creating the wallet on first top-up.
    /// Idempotent on <paramref name="idempotencyKey"/>; records the acting staff for cash reconciliation.
    /// Returns <see cref="WalletDebitOutcome.WalletFrozen"/> if the wallet is frozen/closed.</summary>
    Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> StaffCashDepositAsync(
        int userId, decimal amount, int staffUserId, string idempotencyKey);

    // ---- Admin operations ----

    /// <summary>Paginated wallet list joined with owner, optionally filtered by username/email.</summary>
    Task<WalletAdminListDto> GetWalletsAsync(string? search, int page, int pageSize);

    /// <summary>Ledger for a specific wallet (admin view, by wallet id rather than the caller's user).</summary>
    Task<WalletTransactionListDto?> GetTransactionsByWalletIdAsync(int walletId, int page, int pageSize);

    /// <summary>Manual admin adjustment (signed). Negative amounts are guarded against overdraw. Posts an
    /// <see cref="WalletTransactionType.Adjustment"/> ledger entry with the reason. Returns null if the
    /// wallet is missing/frozen or a negative adjustment would overdraw.</summary>
    Task<WalletTransaction?> AdjustAsync(int walletId, decimal amount, string reason, int actorUserId);

    /// <summary>Set a wallet's status (Active/Frozen/Closed). Returns false if the wallet doesn't exist.</summary>
    Task<bool> SetWalletStatusAsync(int walletId, string status, int actorUserId);
}

public class WalletService : IWalletService
{
    private readonly ApplicationDbContext _context;
    private readonly IWalletPaymentGateway _gateway;

    public WalletService(ApplicationDbContext context, IWalletPaymentGateway gateway)
    {
        _context = context;
        _gateway = gateway;
    }

    public async Task<bool> IsEligibleAsync(int userId) =>
        // A non-cancelled season ticket linked to this user makes them eligible.
        await _context.SeasonTickets
            .AnyAsync(st => st.UserId == userId && st.Status != TicketStatuses.Cancelled);

    public async Task<WalletSummaryDto> GetSummaryAsync(int userId)
    {
        var wallet = await _context.Wallets.AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == userId);

        // Deposit UX depends on the gateway: an async gateway (Stripe) needs the browser to mount a card
        // field with the publishable key; the synchronous mock needs neither.
        var requiresCardEntry = _gateway.SettlesAsynchronously;
        var publishableKey = requiresCardEntry ? _gateway.PublishableKey : null;

        if (wallet != null)
        {
            return new WalletSummaryDto
            {
                WalletId = wallet.Id,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                Status = wallet.Status,
                Exists = true,
                // Once a wallet exists it stays usable; still report current eligibility for the UI.
                IsEligible = await IsEligibleAsync(userId),
                RequiresCardEntry = requiresCardEntry,
                PublishableKey = publishableKey
            };
        }

        return new WalletSummaryDto
        {
            Exists = false,
            IsEligible = await IsEligibleAsync(userId),
            RequiresCardEntry = requiresCardEntry,
            PublishableKey = publishableKey
        };
    }

    public Task<Wallet?> GetOrCreateForUserAsync(int userId) =>
        GetOrCreateForUserCoreAsync(userId, requireEligibility: true);

    /// <summary>Shared get-or-create. The customer deposit path gates on season-ticket eligibility; the
    /// staff bar cash top-up path does not (any registered account can hold cash loaded at the counter).</summary>
    private async Task<Wallet?> GetOrCreateForUserCoreAsync(int userId, bool requireEligibility)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet != null)
            return wallet;

        if (requireEligibility && !await IsEligibleAsync(userId))
            return null;

        // Never create a wallet for a non-existent account (would violate the UserId FK). The eligibility
        // check implies a real user, but the ungated staff path must verify it explicitly.
        if (!requireEligibility && !await _context.Users.AnyAsync(u => u.Id == userId))
            return null;

        wallet = new Wallet
        {
            UserId = userId,
            Balance = 0m,
            Currency = "EUR",
            Status = WalletStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
        _context.Wallets.Add(wallet);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Lost a race to create the (unique-per-user) wallet — load the winner, or rethrow if the
            // failure was something other than the uniqueness collision.
            _context.ChangeTracker.Clear();
            var winner = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (winner == null) throw;
            wallet = winner;
        }

        return wallet;
    }

    public async Task<WalletTransactionListDto> GetTransactionsAsync(int userId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var wallet = await _context.Wallets.AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
            return new WalletTransactionListDto { Page = page, PageSize = pageSize };

        var query = _context.WalletTransactions.AsNoTracking()
            .Where(t => t.WalletId == wallet.Id);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new WalletTransactionDto
            {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                BalanceAfter = t.BalanceAfter,
                Status = t.Status,
                Description = t.Description,
                ReferenceType = t.ReferenceType,
                ReferenceId = t.ReferenceId,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return new WalletTransactionListDto
        {
            Transactions = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<DepositResultDto> InitiateDepositAsync(int userId, InitiateDepositDto dto)
    {
        // Idempotent replay: same key already recorded → return its outcome without re-charging.
        var existing = await _context.WalletTransactions.AsNoTracking()
            .FirstOrDefaultAsync(t => t.IdempotencyKey == dto.IdempotencyKey);
        if (existing != null)
        {
            return new DepositResultDto
            {
                Success = existing.Status == WalletTransactionStatus.Completed,
                WalletTransactionId = existing.Id,
                Status = existing.Status,
                NewBalance = existing.BalanceAfter
            };
        }

        var wallet = await GetOrCreateForUserAsync(userId);
        if (wallet == null)
            return new DepositResultDto { Success = false, FailureReason = "NotEligible" };
        if (wallet.Status != WalletStatus.Active)
            return new DepositResultDto { Success = false, FailureReason = "WalletFrozen" };

        // Async gateway (real, e.g. Stripe): create a payment intent and hand the client secret back to
        // the browser. The wallet is NOT credited here — settlement arrives via the signed webhook, which
        // calls ConfirmDepositFromWebhookAsync (idempotent on the provider intent id).
        if (_gateway.SettlesAsynchronously)
        {
            var intent = await _gateway.CreateDepositIntentAsync(
                dto.Amount, wallet.Currency, new WalletDepositContext(userId, dto.IdempotencyKey, dto.Method));
            return new DepositResultDto
            {
                Success = true,
                Status = WalletTransactionStatus.Pending,
                RequiresAction = true,
                ClientSecret = intent.ClientSecret,
                PublishableKey = intent.PublishableKey,
                ProviderIntentId = intent.ProviderIntentId,
                NewBalance = wallet.Balance
            };
        }

        // Synchronous gateway (mock/dev): capture and credit immediately.
        var gatewayResult = await _gateway.AuthorizeDepositAsync(
            dto.Amount, wallet.Currency, dto.Method, reference: dto.IdempotencyKey);

        if (!gatewayResult.Success)
        {
            return new DepositResultDto { Success = false, FailureReason = gatewayResult.FailureReason ?? "GatewayDeclined" };
        }

        // Credit the wallet (concurrency-safe, idempotent) and record the funding Payment atomically.
        var applied = await ApplyLedgerEntryAsync(
            userId, WalletTransactionType.Deposit, +dto.Amount, dto.IdempotencyKey,
            referenceType: "Deposit", referenceId: null,
            description: $"Deposit via {dto.Method}", actorUserId: userId, requireFunds: false,
            afterInsert: (ctx, txn) =>
            {
                ctx.Payments.Add(new Payment
                {
                    // Link via the navigation, not txn.Id — the identity is only generated on SaveChanges,
                    // so EF must fix up the FK after inserting the transaction.
                    WalletTransaction = txn,
                    PaymentMethod = dto.Method,
                    TransactionId = gatewayResult.GatewayTransactionId,
                    Amount = dto.Amount,
                    Currency = wallet.Currency,
                    Status = "Completed",
                    PaymentDate = DateTime.UtcNow,
                    ProcessedAt = DateTime.UtcNow,
                    PaymentGatewayResponse = gatewayResult.RawResponse
                });
            });

        return new DepositResultDto
        {
            Success = applied.Outcome is WalletDebitOutcome.Success or WalletDebitOutcome.AlreadyApplied,
            WalletTransactionId = applied.Transaction?.Id ?? 0,
            Status = applied.Transaction?.Status ?? WalletTransactionStatus.Failed,
            NewBalance = applied.Transaction?.BalanceAfter ?? wallet.Balance
        };
    }

    public async Task<bool> HandleDepositWebhookAsync(string payload, string signatureHeader)
    {
        // Throws on invalid signature (bubbles to the controller as HTTP 400).
        var settlement = await _gateway.HandleWebhookAsync(payload, signatureHeader);
        if (settlement == null)
            return false; // not a wallet-deposit event we handle

        if (settlement.Succeeded)
            await CreditSettledDepositAsync(settlement);

        return true;
    }

    /// <summary>Credits a webhook-settled deposit. Idempotent on the provider intent id, so Stripe's
    /// at-least-once webhook delivery (and manual replays) credit the wallet exactly once.</summary>
    private async Task CreditSettledDepositAsync(WalletDepositSettlement s)
    {
        var wallet = await GetOrCreateForUserAsync(s.UserId);
        if (wallet == null)
            return; // fan has no wallet / lost eligibility — nothing to credit (shouldn't happen for a real deposit)

        await ApplyLedgerEntryAsync(
            s.UserId, WalletTransactionType.Deposit, +s.Amount,
            idempotencyKey: $"deposit-{s.ProviderIntentId}",
            referenceType: "Deposit", referenceId: null,
            description: $"Deposit via {s.Method}", actorUserId: s.UserId, requireFunds: false,
            afterInsert: (ctx, txn) =>
            {
                ctx.Payments.Add(new Payment
                {
                    WalletTransaction = txn,
                    PaymentMethod = s.Method,
                    TransactionId = s.ProviderIntentId,
                    Amount = s.Amount,
                    Currency = s.Currency,
                    Status = "Completed",
                    PaymentDate = DateTime.UtcNow,
                    ProcessedAt = DateTime.UtcNow,
                    PaymentGatewayResponse = s.RawResponse
                });
            });
    }

    public async Task<DepositStatusDto> GetDepositStatusAsync(int userId, string providerIntentId)
    {
        var wallet = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
            return new DepositStatusDto { Status = WalletTransactionStatus.Pending };

        // The webhook credits under this exact key (see CreditSettledDepositAsync). Filtering by the caller's
        // own wallet means one fan can never probe another's deposit, even with a guessed intent id.
        var key = $"deposit-{providerIntentId}";
        var txn = await _context.WalletTransactions.AsNoTracking()
            .FirstOrDefaultAsync(t => t.WalletId == wallet.Id && t.IdempotencyKey == key);

        if (txn == null)
        {
            // Not credited yet — webhook hasn't arrived (or the payment never succeeded).
            return new DepositStatusDto
            {
                Status = WalletTransactionStatus.Pending,
                Settled = false,
                NewBalance = wallet.Balance
            };
        }

        return new DepositStatusDto
        {
            Status = txn.Status,
            Settled = txn.Status == WalletTransactionStatus.Completed,
            NewBalance = txn.BalanceAfter,
            WalletTransactionId = txn.Id
        };
    }

    public async Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> TryDebitAsync(
        int userId, decimal amount, string idempotencyKey, string referenceType, int? referenceId, string description)
    {
        if (amount <= 0)
            return (WalletDebitOutcome.InsufficientFunds, null);

        var wallet = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
            return (WalletDebitOutcome.WalletNotFound, null);

        return await ApplyLedgerEntryAsync(
            userId, WalletTransactionType.Payment, -amount, idempotencyKey,
            referenceType, referenceId, description, actorUserId: userId, requireFunds: true);
    }

    public async Task<WalletTransaction?> RefundAsync(
        int walletId, decimal amount, string idempotencyKey, string referenceType, int? referenceId, string description, int? actorUserId)
    {
        if (amount <= 0)
            return null;

        var wallet = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == walletId);
        if (wallet == null)
            return null;

        var applied = await ApplyLedgerEntryAsync(
            wallet.UserId, WalletTransactionType.Refund, +amount, idempotencyKey,
            referenceType, referenceId, description, actorUserId, requireFunds: false);
        return applied.Transaction;
    }

    public async Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> StaffCashDepositAsync(
        int userId, decimal amount, int staffUserId, string idempotencyKey)
    {
        if (amount <= 0)
            return (WalletDebitOutcome.InsufficientFunds, null);

        // Ensure a wallet exists for this account — the bar cash top-up is NOT gated on season tickets, so
        // any registered fan can load cash. Returns null only if the account itself doesn't exist.
        var wallet = await GetOrCreateForUserCoreAsync(userId, requireEligibility: false);
        if (wallet == null)
            return (WalletDebitOutcome.WalletNotFound, null);

        // Credit as a Deposit tagged CashTopup, stamped with the acting staff for cash-drawer reconciliation.
        // Reuses the guarded, idempotent ledger primitive (a frozen wallet is rejected → WalletFrozen).
        return await ApplyLedgerEntryAsync(
            userId, WalletTransactionType.Deposit, +amount, idempotencyKey,
            referenceType: "CashTopup", referenceId: null,
            description: $"Cash top-up at bar (staff #{staffUserId})",
            actorUserId: staffUserId, requireFunds: false);
    }

    public async Task<WalletAdminListDto> GetWalletsAsync(string? search, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 25;

        var query = from w in _context.Wallets.AsNoTracking()
                    join u in _context.Users.AsNoTracking() on w.UserId equals u.Id
                    select new WalletAdminDto
                    {
                        WalletId = w.Id,
                        UserId = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        Balance = w.Balance,
                        Currency = w.Currency,
                        Status = w.Status,
                        CreatedAt = w.CreatedAt
                    };

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(x => x.Username.ToLower().Contains(term) || x.Email.ToLower().Contains(term));
        }

        var total = await query.CountAsync();
        var wallets = await query
            .OrderByDescending(x => x.Balance)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new WalletAdminListDto { Wallets = wallets, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<WalletTransactionListDto?> GetTransactionsByWalletIdAsync(int walletId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 25;

        if (!await _context.Wallets.AnyAsync(w => w.Id == walletId))
            return null;

        var query = _context.WalletTransactions.AsNoTracking().Where(t => t.WalletId == walletId);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new WalletTransactionDto
            {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                BalanceAfter = t.BalanceAfter,
                Status = t.Status,
                Description = t.Description,
                ReferenceType = t.ReferenceType,
                ReferenceId = t.ReferenceId,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return new WalletTransactionListDto { Transactions = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<WalletTransaction?> AdjustAsync(int walletId, decimal amount, string reason, int actorUserId)
    {
        if (amount == 0)
            return null;

        var wallet = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == walletId);
        if (wallet == null)
            return null;

        // Negative adjustments are guarded against overdraw; positive ones are unconditional credits.
        var applied = await ApplyLedgerEntryAsync(
            wallet.UserId, WalletTransactionType.Adjustment, amount,
            idempotencyKey: $"adjust-{Guid.NewGuid():N}",
            referenceType: "Adjustment", referenceId: null,
            description: reason, actorUserId: actorUserId, requireFunds: amount < 0);

        return applied.Outcome == WalletDebitOutcome.Success ? applied.Transaction : null;
    }

    public async Task<bool> SetWalletStatusAsync(int walletId, string status, int actorUserId)
    {
        // Single-statement update — runs under the retrying execution strategy automatically and bypasses
        // the xmin token (status flips don't need optimistic-concurrency arbitration).
        var rows = await _context.Wallets
            .Where(w => w.Id == walletId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(w => w.Status, status)
                .SetProperty(w => w.UpdatedAt, DateTime.UtcNow));
        return rows > 0;
    }

    /// <summary>
    /// The single mutation primitive for the ledger. In one transaction it runs an atomic guarded UPDATE
    /// on the wallet balance (<c>SET Balance = Balance + amount WHERE Status='Active' [AND Balance+amount>=0]</c>)
    /// then appends the ledger row. The UPDATE's row lock serialises concurrent writers on the same wallet —
    /// they queue rather than collide, so there is no retry storm — and the funds predicate makes overdraft
    /// impossible (0 rows affected ⇒ rejected). Idempotent: a pre-existing row for
    /// <paramref name="idempotencyKey"/> is returned unchanged, and a concurrent same-key insert that trips
    /// the unique index rolls this transaction back (undoing the balance change) and resolves to the winner.
    /// </summary>
    private async Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> ApplyLedgerEntryAsync(
        int userId, WalletTransactionType type, decimal signedAmount, string idempotencyKey,
        string? referenceType, int? referenceId, string? description, int? actorUserId, bool requireFunds,
        Action<ApplicationDbContext, WalletTransaction>? afterInsert = null)
    {
        // Idempotent replay: a key already recorded returns its original entry, no second mutation.
        var prior = await _context.WalletTransactions.AsNoTracking()
            .FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
        if (prior != null)
            return (WalletDebitOutcome.AlreadyApplied, prior);

        var wallet = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
            return (WalletDebitOutcome.WalletNotFound, null);

        // The guarded UPDATE + ledger insert (the atomic core). The UPDATE's row lock serialises concurrent
        // writers on this wallet — they queue, they don't collide (no retry storm) — and the
        // "Balance + amount >= 0" predicate makes overdraft impossible (0 rows affected => rejected).
        async Task<(WalletDebitOutcome Outcome, WalletTransaction? Transaction)> RunGuardedAsync()
        {
            var now = DateTime.UtcNow;

            var fundsGuard = requireFunds ? @" AND ""Balance"" + @amt >= 0" : string.Empty;
            var sql = $@"UPDATE ""Wallets"" SET ""Balance"" = ""Balance"" + @amt, ""UpdatedAt"" = @now
                        WHERE ""Id"" = @id AND ""Status"" = '{WalletStatus.Active}'" + fundsGuard;
            var affected = await _context.Database.ExecuteSqlRawAsync(sql,
                new NpgsqlParameter("amt", signedAmount),
                new NpgsqlParameter("now", now),
                new NpgsqlParameter("id", wallet.Id));

            if (affected == 0)
            {
                // Distinguish why the guard rejected the write.
                var current = await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == wallet.Id);
                if (current == null) return (WalletDebitOutcome.WalletNotFound, null);
                if (current.Status != WalletStatus.Active) return (WalletDebitOutcome.WalletFrozen, null);
                return (WalletDebitOutcome.InsufficientFunds, null);
            }

            // Read the post-update balance (same connection/transaction sees our uncommitted change).
            var newBalance = (await _context.Wallets.AsNoTracking().FirstAsync(w => w.Id == wallet.Id)).Balance;

            var txn = new WalletTransaction
            {
                WalletId = wallet.Id,
                Type = type,
                Amount = signedAmount,
                BalanceAfter = newBalance,
                Currency = wallet.Currency,
                Status = WalletTransactionStatus.Completed,
                IdempotencyKey = idempotencyKey,
                ReferenceType = referenceType,
                ReferenceId = referenceId,
                Description = description,
                CreatedByUserId = actorUserId,
                CreatedAt = now,
                CompletedAt = now
            };
            _context.WalletTransactions.Add(txn);
            afterInsert?.Invoke(_context, txn);

            await _context.SaveChangesAsync();
            return (WalletDebitOutcome.Success, txn);
        }

        // If a caller already owns a transaction, run inside it (they commit/rollback).
        if (_context.Database.CurrentTransaction != null)
        {
            try
            {
                return await RunGuardedAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                _context.ChangeTracker.Clear();
                var winner = await _context.WalletTransactions.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
                if (winner != null) return (WalletDebitOutcome.AlreadyApplied, winner);
                throw;
            }
        }

        // Otherwise run begin→work→commit as ONE retriable unit. The API enables EnableRetryOnFailure
        // (NpgsqlRetryingExecutionStrategy), which forbids a manually-opened transaction unless it is
        // executed through the strategy — so the whole unit goes through CreateExecutionStrategy().
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            // Re-check idempotency inside the retriable unit in case a transient retry follows a partial run.
            _context.ChangeTracker.Clear();
            var replay = await _context.WalletTransactions.AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
            if (replay != null) return (WalletDebitOutcome.AlreadyApplied, replay);

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await RunGuardedAsync();
                if (result.Outcome != WalletDebitOutcome.Success)
                {
                    await tx.RollbackAsync();
                    return result;
                }
                await tx.CommitAsync();
                return result;
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                // A concurrent same-key insert won the unique index. Roll back OUR balance change (same
                // transaction) and return the winner — the mutation is applied exactly once.
                await tx.RollbackAsync();
                _context.ChangeTracker.Clear();
                var winner = await _context.WalletTransactions.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
                if (winner != null) return (WalletDebitOutcome.AlreadyApplied, winner);
                throw;
            }
        });
    }

    private static bool IsUniqueViolation(DbUpdateException ex) =>
        ex.InnerException is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation;
}
