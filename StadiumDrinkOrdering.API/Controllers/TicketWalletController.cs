using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Bar-counter operations on an <b>anonymous ticket wallet</b> — a bearer stored-value balance loaded on
/// the ticket itself, no account required. A bartender scans a ticket (resolved via
/// <c>api/bar/topup/resolve</c>, which returns the ticket id + wallet balance when the ticket has no
/// linked account), loads cash onto it during the match, and hands back any remainder afterward.
/// Staff-scoped: Bartender/Waiter/Admin. All debits are server-authoritative and online-only.
/// </summary>
[Route("api/bar/ticket-wallet")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
public class TicketWalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly IStadiumAuthorizationService _authorizationService;
    private readonly ILoggingService _loggingService;

    public TicketWalletController(
        IWalletService walletService,
        IStadiumAuthorizationService authorizationService,
        ILoggingService loggingService)
    {
        _walletService = walletService;
        _authorizationService = authorizationService;
        _loggingService = loggingService;
    }

    /// <summary>Loads the confirmed cash amount onto the ticket's wallet (creating it on first use).</summary>
    [HttpPost("topup")]
    public async Task<ActionResult<TicketWalletResultDto>> Topup([FromBody] TicketWalletTopupRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = _authorizationService.GetCurrentUserId(User);
        if (staffId is null or 0)
            return Unauthorized();

        var (outcome, txn) = await _walletService.TopUpTicketWalletAsync(
            request.TicketId, request.Amount, staffId.Value, request.IdempotencyKey);

        if (outcome is not (WalletDebitOutcome.Success or WalletDebitOutcome.AlreadyApplied))
        {
            var reason = outcome switch
            {
                WalletDebitOutcome.WalletFrozen => "WalletFrozen",       // frozen or already cashed out (Closed)
                WalletDebitOutcome.WalletNotFound => "TicketNotFound",
                WalletDebitOutcome.LimitExceeded => "LimitExceeded",
                _ => "InvalidAmount"
            };
            return Ok(new TicketWalletResultDto { Success = false, FailureReason = reason });
        }

        await _loggingService.LogUserActionAsync(
            action: "TicketWalletTopup",
            category: "Audit",
            userId: staffId.Value.ToString(),
            details: $"Loaded {request.Amount:0.00} EUR on ticket #{request.TicketId} (txn {txn?.Id}, {outcome})");

        return Ok(new TicketWalletResultDto
        {
            Success = true,
            NewBalance = txn?.BalanceAfter ?? 0m,
            Status = WalletStatus.Active,
            WalletTransactionId = txn?.Id ?? 0
        });
    }

    /// <summary>Hands back the remaining balance and closes the ticket wallet (terminal). The client
    /// should send a fresh idempotency key per attempt: a network retry reuses it and safely completes
    /// the same payout, while a re-scan of an already-cashed-out ticket returns AlreadyCashedOut with
    /// nothing to pay.</summary>
    [HttpPost("cashout")]
    public async Task<ActionResult<TicketWalletResultDto>> Cashout([FromBody] TicketWalletCashoutRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = _authorizationService.GetCurrentUserId(User);
        if (staffId is null or 0)
            return Unauthorized();

        var result = await _walletService.CashOutTicketWalletAsync(
            request.TicketId, staffId.Value, request.IdempotencyKey);

        // Success, or an idempotent retry of the SAME payout (same key ⇒ RefundAmount carries the
        // original figure) → report success so an interrupted client can finish. A re-scan of an
        // already-closed wallet resolves to AlreadyCashedOut with RefundAmount 0 → surfaced as a failure
        // so the bartender never hands cash twice.
        var isPayout = result.Outcome == TicketWalletCashOutOutcome.Success
            || (result.Outcome == TicketWalletCashOutOutcome.AlreadyCashedOut && result.RefundAmount > 0);

        if (!isPayout)
        {
            var reason = result.Outcome switch
            {
                TicketWalletCashOutOutcome.AlreadyCashedOut => "AlreadyCashedOut",
                TicketWalletCashOutOutcome.WalletFrozen => "WalletFrozen",
                TicketWalletCashOutOutcome.TicketWalletNotFound => "TicketNotFound",
                TicketWalletCashOutOutcome.NothingToCashOut => "NothingToCashOut",
                _ => "InvalidAmount"
            };
            return Ok(new TicketWalletResultDto { Success = false, FailureReason = reason, Status = WalletStatus.Closed });
        }

        await _loggingService.LogUserActionAsync(
            action: "TicketWalletCashOut",
            category: "Audit",
            userId: staffId.Value.ToString(),
            details: $"Cashed out {result.RefundAmount:0.00} EUR from ticket #{request.TicketId} (txn {result.TransactionId}, {result.Outcome})");

        return Ok(new TicketWalletResultDto
        {
            Success = true,
            NewBalance = 0m,
            RefundAmount = result.RefundAmount,
            Status = WalletStatus.Closed,
            WalletTransactionId = result.TransactionId
        });
    }

    /// <summary>Claims the ticket's balance onto a registered account by email: moves the bearer balance
    /// to the account's wallet and emails a set-password link. The balance becomes recoverable and there's
    /// no cash-out queue to stand in. Idempotent per ticket.</summary>
    [HttpPost("claim")]
    public async Task<ActionResult<TicketWalletClaimResultDto>> Claim([FromBody] TicketWalletClaimRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = _authorizationService.GetCurrentUserId(User);
        if (staffId is null or 0)
            return Unauthorized();

        var result = await _walletService.ClaimTicketWalletAsync(request.TicketId, request.Email, staffId.Value);

        if (result.Outcome != TicketWalletClaimOutcome.Claimed)
        {
            var reason = result.Outcome switch
            {
                TicketWalletClaimOutcome.AlreadyClaimed => "AlreadyClaimed",
                TicketWalletClaimOutcome.NothingToClaim => "NothingToClaim",
                TicketWalletClaimOutcome.TicketWalletNotFound => "TicketNotFound",
                TicketWalletClaimOutcome.WalletFrozen => "WalletFrozen",
                TicketWalletClaimOutcome.InvalidEmail => "InvalidEmail",
                TicketWalletClaimOutcome.AccountUnavailable => "AccountUnavailable",
                _ => "Failed"
            };
            return Ok(new TicketWalletClaimResultDto { Success = false, FailureReason = reason, Email = result.Email });
        }

        await _loggingService.LogUserActionAsync(
            action: "TicketWalletClaim",
            category: "Audit",
            userId: staffId.Value.ToString(),
            details: $"Claimed {result.Amount:0.00} EUR from ticket #{request.TicketId} to account #{result.UserId} ({result.Email})");

        return Ok(new TicketWalletClaimResultDto
        {
            Success = true,
            ClaimedAmount = result.Amount,
            Email = result.Email
        });
    }
}
