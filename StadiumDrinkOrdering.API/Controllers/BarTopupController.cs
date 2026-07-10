using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Bar-counter cash top-up. A bartender scans (or types) the fan's ticket / season pass / email / OIB,
/// the server resolves it to the owning registered account, and — after the staff confirms identity —
/// credits the cash they handed over onto that account's wallet. Staff-scoped: only Bartender/Waiter/Admin.
/// The wallet is per-user, so a top-up here shows up in the fan's own /wallet history immediately.
/// </summary>
[Route("api/bar/topup")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
public class BarTopupController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ApplicationDbContext _db;
    private readonly IStadiumAuthorizationService _authorizationService;
    private readonly ILoggingService _loggingService;
    private readonly ILogger<BarTopupController> _logger;

    public BarTopupController(
        IWalletService walletService,
        ApplicationDbContext db,
        IStadiumAuthorizationService authorizationService,
        ILoggingService loggingService,
        ILogger<BarTopupController> logger)
    {
        _walletService = walletService;
        _db = db;
        _authorizationService = authorizationService;
        _loggingService = loggingService;
        _logger = logger;
    }

    /// <summary>
    /// Resolves a scanned/typed value to the fan account to be topped up, returning identity fields
    /// (full name, email, OIB) for the bartender to confirm plus the current wallet balance.
    /// </summary>
    [HttpPost("resolve")]
    public async Task<ActionResult<BarTopupResolveResultDto>> Resolve([FromBody] BarTopupResolveRequestDto request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Query))
            return BadRequest(new BarTopupResolveResultDto { Found = false, Message = "Unesite ili skenirajte kôd." });

        var query = request.Query.Trim();
        var lower = query.ToLower();

        string? email = null;
        int? userId = null;
        string? matchedBy = null;

        // 1) A direct ticket QR token, or 2) a printed ticket number typed manually (case-insensitive).
        var ticket = await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.QRCodeToken == query)
                     ?? await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.TicketNumber.ToLower() == lower);
        if (ticket != null)
        {
            email = ticket.CustomerEmail;
            matchedBy = "Ticket";
        }

        // 3) A season pass token — carries the linked account directly (UserId) or the holder email.
        if (matchedBy == null)
        {
            var pass = await _db.SeasonTickets.AsNoTracking().FirstOrDefaultAsync(st => st.PassToken == query);
            if (pass != null)
            {
                userId = pass.UserId;
                email = pass.HolderEmail;
                matchedBy = "SeasonPass";
            }
        }

        // 4) A raw email typed in.
        if (matchedBy == null && query.Contains('@'))
        {
            email = query;
            matchedBy = "Email";
        }

        // 5) A raw OIB (11 digits) typed in.
        if (matchedBy == null && lower.Length == 11 && lower.All(char.IsDigit))
        {
            var byOib = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Oib == query);
            if (byOib != null)
            {
                userId = byOib.Id;
                matchedBy = "Oib";
            }
        }

        if (matchedBy == null)
            return Ok(new BarTopupResolveResultDto { Found = false, Message = "Ništa nije pronađeno za taj kôd." });

        // Resolve to a registered account: prefer an explicit UserId, else match the email.
        User? user = null;
        if (userId is > 0)
            user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null && !string.IsNullOrWhiteSpace(email))
            user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToLower() == email!.ToLower());

        if (user == null)
        {
            // Message depends on how they searched: a scanned ticket/pass exists but isn't linked to an
            // account, vs. a typed email/OIB that simply matches no account at all.
            var noAccountMessage = matchedBy is "Ticket" or "SeasonPass"
                ? "Ulaznica je pronađena, ali nije povezana s korisničkim računom. Gost se mora prijaviti ili izraditi račun."
                : matchedBy == "Oib"
                    ? "Nijedan korisnički račun nema taj OIB. Gost mora izraditi račun."
                    : "Nijedan korisnički račun nema tu e-poštu. Gost mora izraditi račun.";
            return Ok(new BarTopupResolveResultDto
            {
                Found = matchedBy is "Ticket" or "SeasonPass", // a raw email/OIB with no account isn't really a "find"
                HasAccount = false,
                MatchedBy = matchedBy,
                Email = email,
                Message = noAccountMessage
            });
        }

        if (!user.IsActive)
            return Ok(new BarTopupResolveResultDto
            {
                Found = true,
                HasAccount = false,
                MatchedBy = matchedBy,
                FullName = FullName(user),
                Email = user.Email,
                Message = "Korisnički račun je deaktiviran."
            });

        var summary = await _walletService.GetSummaryAsync(user.Id);

        return Ok(new BarTopupResolveResultDto
        {
            Found = true,
            HasAccount = true,
            UserId = user.Id,
            FullName = FullName(user),
            Email = user.Email,
            Oib = user.Oib,
            PhoneNumber = user.PhoneNumber,
            Balance = summary.Balance,
            Currency = summary.Currency,
            WalletStatus = summary.Status,
            WalletExists = summary.Exists,
            MatchedBy = matchedBy
        });
    }

    /// <summary>Credits the confirmed cash amount onto the resolved account's wallet.</summary>
    [HttpPost]
    public async Task<ActionResult<BarTopupResultDto>> Topup([FromBody] BarTopupRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = _authorizationService.GetCurrentUserId(User);
        if (staffId is null or 0)
            return Unauthorized();

        var (outcome, txn) = await _walletService.StaffCashDepositAsync(
            request.UserId, request.Amount, staffId.Value, request.IdempotencyKey);

        if (outcome is not (WalletDebitOutcome.Success or WalletDebitOutcome.AlreadyApplied))
        {
            var reason = outcome switch
            {
                WalletDebitOutcome.WalletFrozen => "WalletFrozen",
                WalletDebitOutcome.WalletNotFound => "AccountNotFound",
                _ => "InvalidAmount"
            };
            return Ok(new BarTopupResultDto { Success = false, FailureReason = reason });
        }

        await _loggingService.LogUserActionAsync(
            action: "BarCashTopup",
            category: "Audit",
            userId: staffId.Value.ToString(),
            details: $"Cash top-up {request.Amount:0.00} EUR onto wallet of user #{request.UserId} (txn {txn?.Id}, {outcome})");

        return Ok(new BarTopupResultDto
        {
            Success = true,
            NewBalance = txn?.BalanceAfter ?? 0m,
            WalletTransactionId = txn?.Id ?? 0
        });
    }

    private static string FullName(User u)
    {
        var name = $"{u.FirstName} {u.LastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? u.Username : name;
    }
}
