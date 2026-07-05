using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Customer-facing wallet endpoints. The wallet is <b>always</b> resolved from the authenticated
/// user's id — the client never supplies a wallet or user id, so a fan can only ever see or move
/// their own funds.
/// </summary>
[Route("api/wallet")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly IStadiumAuthorizationService _authorizationService;
    private readonly ILoggingService _loggingService;
    private readonly ILogger<WalletController> _logger;

    public WalletController(
        IWalletService walletService,
        IStadiumAuthorizationService authorizationService,
        ILoggingService loggingService,
        ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _authorizationService = authorizationService;
        _loggingService = loggingService;
        _logger = logger;
    }

    /// <summary>Current wallet balance / status / eligibility for the signed-in fan.</summary>
    [HttpGet("me")]
    public async Task<ActionResult<WalletSummaryDto>> GetMyWallet()
    {
        var userId = _authorizationService.GetCurrentUserId(User);
        if (userId is null or 0)
            return Unauthorized();

        return Ok(await _walletService.GetSummaryAsync(userId.Value));
    }

    /// <summary>Paginated ledger for the signed-in fan.</summary>
    [HttpGet("me/transactions")]
    public async Task<ActionResult<WalletTransactionListDto>> GetMyTransactions(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = _authorizationService.GetCurrentUserId(User);
        if (userId is null or 0)
            return Unauthorized();

        return Ok(await _walletService.GetTransactionsAsync(userId.Value, page, pageSize));
    }

    /// <summary>Add funds to the signed-in fan's wallet. Idempotent on <see cref="InitiateDepositDto.IdempotencyKey"/>.</summary>
    [HttpPost("me/deposits")]
    public async Task<ActionResult<DepositResultDto>> Deposit([FromBody] InitiateDepositDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = _authorizationService.GetCurrentUserId(User);
        if (userId is null or 0)
            return Unauthorized();

        var result = await _walletService.InitiateDepositAsync(userId.Value, dto);
        if (!result.Success)
        {
            // NotEligible → 403 (has no season ticket); everything else is a normal declined-deposit body.
            if (result.FailureReason == "NotEligible")
                return Forbid();
            return Ok(result);
        }

        await _loggingService.LogUserActionAsync(
            action: "WalletDeposit",
            category: "Audit",
            userId: userId.Value.ToString(),
            details: $"Deposit initiated {dto.Amount:0.00} EUR (status {result.Status}, txn {result.WalletTransactionId})");

        return Ok(result);
    }

    /// <summary>
    /// Provider deposit webhook (e.g. Stripe <c>payment_intent.succeeded</c>). Unauthenticated — trust is
    /// established by the provider signature, verified inside the gateway. Credits the wallet exactly once
    /// (idempotent on the provider intent id). Returns 400 on a bad signature so the provider retries.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("deposits/webhook")]
    public async Task<IActionResult> DepositWebhook()
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault() ?? string.Empty;

        try
        {
            var handled = await _walletService.HandleDepositWebhookAsync(payload, signature);
            return Ok(new { handled });
        }
        catch (Exception ex)
        {
            // Invalid signature or malformed payload — 400 tells the provider to retry.
            _logger.LogWarning(ex, "Wallet deposit webhook rejected");
            return BadRequest();
        }
    }
}
