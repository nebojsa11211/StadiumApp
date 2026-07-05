using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Admin wallet management: inspect any fan's wallet + ledger, freeze/unfreeze, post manual
/// adjustments, and issue refunds. Every mutation is audit-logged with the acting admin. Balances are
/// never edited directly — corrections go through the ledger (Adjustment/Refund entries).
/// </summary>
[Route("api/admin/wallets")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class AdminWalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly IStadiumAuthorizationService _authorizationService;
    private readonly ILoggingService _loggingService;
    private readonly ILogger<AdminWalletController> _logger;

    public AdminWalletController(
        IWalletService walletService,
        IStadiumAuthorizationService authorizationService,
        ILoggingService loggingService,
        ILogger<AdminWalletController> logger)
    {
        _walletService = walletService;
        _authorizationService = authorizationService;
        _loggingService = loggingService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<WalletAdminListDto>> GetWallets(
        [FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        => Ok(await _walletService.GetWalletsAsync(search, page, pageSize));

    [HttpGet("{id:int}/transactions")]
    public async Task<ActionResult<WalletTransactionListDto>> GetTransactions(
        int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var result = await _walletService.GetTransactionsByWalletIdAsync(id, page, pageSize);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:int}/adjust")]
    public async Task<ActionResult<WalletTransactionDto>> Adjust(int id, [FromBody] AdjustWalletDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var adminId = _authorizationService.GetCurrentUserId(User) ?? 0;
        var txn = await _walletService.AdjustAsync(id, dto.Amount, dto.Reason, adminId);
        if (txn == null)
            return BadRequest("Adjustment failed — wallet missing/frozen, or a negative adjustment would overdraw.");

        await Audit(adminId, "WalletAdjust", $"Wallet {id} adjusted by {dto.Amount:0.00} (txn {txn.Id}). Reason: {dto.Reason}");
        return Ok(ToDto(txn));
    }

    [HttpPost("{id:int}/freeze")]
    public Task<IActionResult> Freeze(int id) => SetStatus(id, WalletStatus.Frozen, "WalletFreeze");

    [HttpPost("{id:int}/unfreeze")]
    public Task<IActionResult> Unfreeze(int id) => SetStatus(id, WalletStatus.Active, "WalletUnfreeze");

    [HttpPost("{id:int}/refund")]
    public async Task<ActionResult<WalletTransactionDto>> Refund(int id, [FromBody] RefundWalletDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var adminId = _authorizationService.GetCurrentUserId(User) ?? 0;
        var txn = await _walletService.RefundAsync(
            id, dto.Amount, idempotencyKey: $"admin-refund-{Guid.NewGuid():N}",
            referenceType: "AdminRefund", referenceId: null, description: dto.Reason, actorUserId: adminId);
        if (txn == null)
            return BadRequest("Refund failed — wallet missing or frozen.");

        await Audit(adminId, "WalletRefund", $"Wallet {id} refunded {dto.Amount:0.00} (txn {txn.Id}). Reason: {dto.Reason}");
        return Ok(ToDto(txn));
    }

    private async Task<IActionResult> SetStatus(int id, string status, string action)
    {
        var adminId = _authorizationService.GetCurrentUserId(User) ?? 0;
        var ok = await _walletService.SetWalletStatusAsync(id, status, adminId);
        if (!ok) return NotFound();

        await Audit(adminId, action, $"Wallet {id} status set to {status}.");
        return Ok(new { status });
    }

    private Task Audit(int adminId, string action, string details) =>
        _loggingService.LogUserActionAsync(action, "Audit", adminId.ToString(), details: details);

    private static WalletTransactionDto ToDto(WalletTransaction t) => new()
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
    };
}
