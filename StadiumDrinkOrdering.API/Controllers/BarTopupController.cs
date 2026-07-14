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
    private readonly IAccountProvisioningService _accountProvisioning;
    private readonly ILogger<BarTopupController> _logger;

    public BarTopupController(
        IWalletService walletService,
        ApplicationDbContext db,
        IStadiumAuthorizationService authorizationService,
        ILoggingService loggingService,
        IAccountProvisioningService accountProvisioning,
        ILogger<BarTopupController> logger)
    {
        _walletService = walletService;
        _db = db;
        _authorizationService = authorizationService;
        _loggingService = loggingService;
        _accountProvisioning = accountProvisioning;
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
        string? oib = null; // identity carried by the matched ticket/pass, used to resolve or provision an account

        // 1) A direct ticket QR token, or 2) a printed ticket number typed manually (case-insensitive).
        var ticket = await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.QRCodeToken == query)
                     ?? await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.TicketNumber.ToLower() == lower);
        if (ticket != null)
        {
            email = ticket.CustomerEmail;
            oib = ticket.CustomerOib;
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
                oib = pass.HolderOib;
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
            oib = query;
            var byOib = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Oib == query);
            if (byOib != null)
            {
                userId = byOib.Id;
                matchedBy = "Oib";
            }
        }

        if (matchedBy == null)
            return Ok(new BarTopupResolveResultDto { Found = false, Message = "Ništa nije pronađeno za taj kôd." });

        // Resolve to a registered account: prefer an explicit UserId, else the email, else the OIB captured
        // on the ticket/pass — so a fan whose account was created without an email (or under a different one)
        // is still found by the OIB printed on their ticket.
        User? user = null;
        if (userId is > 0)
            user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null && !string.IsNullOrWhiteSpace(email))
            user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToLower() == email!.ToLower());
        if (user == null && !string.IsNullOrWhiteSpace(oib))
        {
            user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Oib == oib);
            if (user != null)
                matchedBy = "Oib";
        }

        if (user == null)
        {
            // No linked account. If a CONCRETE ticket was scanned, offer an anonymous ticket wallet
            // (load funds on the ticket itself) instead of a dead end. A season pass with no account, or
            // a raw email/OIB, has no ticket to bind a bearer balance to, so those stay a dead end.
            if (ticket != null)
            {
                var tw = await _walletService.GetTicketWalletSummaryAsync(ticket.Id);
                var closed = string.Equals(tw.Status, WalletStatus.Closed, StringComparison.OrdinalIgnoreCase);
                // The ticket carries a real identity (OIB) → the bartender can create a claimable account on
                // the spot (enter an email) so cash lands on the fan's own wallet. Independent of the ticket
                // wallet being closed — provisioning is about the person, not that bearer balance.
                var canProvision = !string.IsNullOrWhiteSpace(ticket.CustomerOib);
                return Ok(new BarTopupResolveResultDto
                {
                    Found = true,
                    HasAccount = false,
                    AllowTicketWallet = true,
                    CanProvisionAccount = canProvision,
                    MatchedBy = matchedBy,
                    Email = email,
                    FullName = ticket.CustomerName,
                    Oib = ticket.CustomerOib,
                    TicketId = ticket.Id,
                    TicketNumber = ticket.TicketNumber,
                    Balance = tw.Balance,
                    Currency = tw.Currency,
                    WalletStatus = tw.Status,
                    WalletExists = tw.Exists,
                    TicketWalletMaxBalance = _walletService.TicketWalletMaxBalance,
                    Message = closed
                        ? "Ova ulaznica je već isplaćena — saldo je zatvoren."
                        : canProvision
                            ? "Nema povezanog računa. Unesite e-poštu da kreirate račun (sredstva idu na osobni novčanik gosta) ili učitajte izravno na ulaznicu."
                            : "Nema povezanog računa. Sredstva možete učitati izravno na ulaznicu."
                });
            }

            // Message depends on how they searched: a scanned pass exists but isn't linked to an
            // account, vs. a typed email/OIB that simply matches no account at all.
            var noAccountMessage = matchedBy == "SeasonPass"
                ? "Sezonska ulaznica je pronađena, ali nije povezana s korisničkim računom. Gost se mora prijaviti ili izraditi račun."
                : matchedBy == "Oib"
                    ? "Nijedan korisnički račun nema taj OIB. Gost mora izraditi račun."
                    : "Nijedan korisnički račun nema tu e-poštu. Gost mora izraditi račun.";
            return Ok(new BarTopupResolveResultDto
            {
                Found = matchedBy == "SeasonPass", // a raw email/OIB with no account isn't really a "find"
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

        // Link the OIB carried by the scanned ticket onto an account that has none, so this fan resolves by
        // OIB on future scans even if the match this time was by email/UserId.
        if (!string.IsNullOrWhiteSpace(oib) && string.IsNullOrWhiteSpace(user.Oib))
        {
            await _db.Users.Where(u => u.Id == user.Id && (u.Oib == null || u.Oib == ""))
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.Oib, oib));
            user.Oib = oib; // reflect in the response
        }

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

    /// <summary>
    /// Recent bar-counter cash movements (account top-ups, ticket loads, ticket cash-outs) for the staff
    /// history / cash-drawer reconciliation view. <paramref name="onlyMine"/> scopes to the calling staff
    /// member; <paramref name="search"/> filters by holder name, email, or ticket number.
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<BarTopupHistoryListDto>> History(
        [FromQuery] string? search = null, [FromQuery] bool onlyMine = false,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        int? staffFilter = null;
        if (onlyMine)
        {
            var staffId = _authorizationService.GetCurrentUserId(User);
            if (staffId is null or 0)
                return Unauthorized();
            staffFilter = staffId;
        }

        return Ok(await _walletService.GetBarTopupHistoryAsync(search, staffFilter, page, pageSize));
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

    /// <summary>
    /// Creates (or reuses) a claimable account for an OIB-identified ticket that has no account yet, keyed
    /// on the email the bartender collected at the counter, and returns a resolve result for that account so
    /// the cash top-up can proceed onto the fan's own wallet. The name/OIB come from the ticket; a
    /// set-password link is emailed for a freshly created shell.
    /// </summary>
    [HttpPost("provision")]
    public async Task<ActionResult<BarTopupResolveResultDto>> Provision([FromBody] BarTopupProvisionRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = _authorizationService.GetCurrentUserId(User);
        if (staffId is null or 0)
            return Unauthorized();

        var ticket = await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.TicketId);
        if (ticket == null)
            return Ok(new BarTopupResolveResultDto { Found = false, Message = "Ulaznica nije pronađena." });

        var userId = await _accountProvisioning.ProvisionAndGetUserIdAsync(
            request.Email.Trim(), ticket.CustomerName, ticket.CustomerPhone, ticket.CustomerOib, "BarProvision");
        if (userId is null or 0)
            return Ok(new BarTopupResolveResultDto { Found = false, Message = "Račun nije moguće kreirati. Provjerite e-poštu." });

        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Ok(new BarTopupResolveResultDto { Found = false, Message = "Račun nije moguće kreirati." });

        await _loggingService.LogUserActionAsync(
            action: "BarProvisionAccount",
            category: "Audit",
            userId: staffId.Value.ToString(),
            details: $"Provisioned account #{user.Id} from ticket #{ticket.Id} (email {request.Email}) at bar");

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
            MatchedBy = "Provisioned"
        });
    }

    private static string FullName(User u)
    {
        var name = $"{u.FirstName} {u.LastName}".Trim();
        return string.IsNullOrWhiteSpace(name) ? u.Username : name;
    }
}
