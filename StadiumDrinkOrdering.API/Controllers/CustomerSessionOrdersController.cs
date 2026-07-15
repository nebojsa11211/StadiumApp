using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Anonymous drink ordering for walk-up fans. Every action is gated by possession of a valid, active
/// <see cref="TicketSession"/> token (issued by <c>POST /TicketAuth/validate</c> after a QR scan) — so
/// there is no login, but a caller can only order for the seat they scanned and only read their own
/// orders. Orders are attributed to the ticket holder's real (or auto-provisioned claimable) account —
/// falling back to a shared "walk-up guest" only for an identity-less ticket; delivery routing uses the
/// order's ticket/seat, and status reads are scoped by the originating ticket session.
/// </summary>
[ApiController]
[Route("customer/session")]
[AllowAnonymous]
public class CustomerSessionOrdersController : ControllerBase
{
    private readonly ITicketAuthService _ticketAuth;
    private readonly IOrderService _orderService;
    private readonly IWalletService _walletService;
    private readonly IWalkUpAccountResolver _walkUpAccounts;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CustomerSessionOrdersController> _logger;

    public CustomerSessionOrdersController(
        ITicketAuthService ticketAuth,
        IOrderService orderService,
        IWalletService walletService,
        IWalkUpAccountResolver walkUpAccounts,
        ApplicationDbContext db,
        ILogger<CustomerSessionOrdersController> logger)
    {
        _ticketAuth = ticketAuth;
        _orderService = orderService;
        _walletService = walletService;
        _walkUpAccounts = walkUpAccounts;
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Places a drink order for the scanned seat. When <see cref="SessionOrderRequest.PayWithWallet"/> is set,
    /// it is charged to the HALFTIME wallet of the account behind the ticket and attributed to that account.
    /// Otherwise it is created unpaid (settled at the bar / on delivery); the fan's chosen offline method from
    /// <see cref="SessionOrderRequest.PaymentMethod"/> (cash / card) is recorded as a Pending payment so staff
    /// know how it will be settled.
    /// </summary>
    [HttpPost("order")]
    public async Task<ActionResult<SessionOrderResultDto>> CreateSessionOrder([FromBody] SessionOrderRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.SessionToken))
            return BadRequest(new SessionOrderResultDto { Error = "Missing session token." });

        if (request.Items is null || request.Items.Count == 0)
            return BadRequest(new SessionOrderResultDto { Error = "Your cart is empty." });

        var session = await _ticketAuth.GetTicketSessionAsync(request.SessionToken);
        if (session is null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            return Unauthorized(new SessionOrderResultDto { Error = "Your session has expired. Please scan your ticket again." });

        // Ordering is only open while the event is live (Active/InProgress) AND within the event's
        // optional drink-ordering window — enforces the countdown-only rule and any early bar close.
        var orderEvent = session.Ticket?.Event;
        if (orderEvent is null || !orderEvent.AreDrinkSalesOpenAt(DateTime.UtcNow))
            return BadRequest(new SessionOrderResultDto
            {
                Error = orderEvent?.DrinkSalesBlockedReason(DateTime.UtcNow) ?? "Ordering is not available for this event."
            });

        // Who the order is attributed to and how it is funded. Unpaid walk-up orders go to the shared guest;
        // a wallet-paid order is charged to — and attributed to — the fan account the ticket resolves to, so
        // the existing wallet-debit path (which debits the order's customer) charges the right wallet.
        int customerId;
        PaymentMethod? paymentMethod;
        if (request.PayWithWallet)
        {
            var ownerId = await ResolveWalletOwnerAsync(session);
            if (ownerId is not null)
            {
                // Registered fan: charge and attribute the order to their user wallet.
                customerId = ownerId.Value;
                paymentMethod = PaymentMethod.DigitalWallet;
            }
            else if (await HasActiveTicketWalletAsync(session))
            {
                // Anonymous bearer balance loaded on the ticket itself. Pay via TicketWallet — that debit
                // targets the ticket's wallet by ticket id, independent of the order's customer — but still
                // attribute the order to the ticket holder's real account when the ticket identifies one
                // (only a truly identity-less ticket falls back to the shared guest).
                customerId = await _walkUpAccounts.ResolveCustomerIdAsync(session.Ticket);
                paymentMethod = PaymentMethod.TicketWallet;
            }
            else
            {
                return BadRequest(new SessionOrderResultDto { Error = "No wallet is linked to this ticket." });
            }
        }
        else
        {
            customerId = await _walkUpAccounts.ResolveCustomerIdAsync(session.Ticket);
            // Offline payment: no money moves here (settled at the bar / on delivery), but keep the fan's
            // chosen method so staff see cash vs card up front. Only accept genuine offline methods — a
            // wallet enum value must never reach a wallet via this unpaid path. Null stays fully unpaid.
            paymentMethod = request.PaymentMethod is PaymentMethod.Cash or PaymentMethod.CreditCard
                or PaymentMethod.DebitCard or PaymentMethod.BankTransfer
                ? request.PaymentMethod
                : null;
        }

        var createDto = new CreateOrderDto
        {
            TicketSessionId = session.Id,
            TicketNumber = session.Ticket?.TicketNumber,
            CustomerNotes = request.CustomerNotes,
            OrderItems = request.Items
                .Where(i => i.Quantity > 0)
                .Select(i => new CreateOrderItemDto
                {
                    DrinkId = i.DrinkId,
                    Quantity = i.Quantity,
                    SpecialInstructions = i.SpecialInstructions
                })
                .ToList(),
            PaymentMethod = paymentMethod
        };

        if (createDto.OrderItems.Count == 0)
            return BadRequest(new SessionOrderResultDto { Error = "Your cart is empty." });

        var result = await _orderService.CreateOrderAsync(createDto, customerId);
        if (result.Outcome != CreateOrderOutcome.Success || result.Order is null)
        {
            // Insufficient funds is a normal, recoverable outcome (no order, no money moved) — signal it so the
            // fan can top up and retry. Everything else is a plain failure.
            if (result.Outcome == CreateOrderOutcome.InsufficientFunds)
                return StatusCode(StatusCodes.Status402PaymentRequired,
                    new SessionOrderResultDto { InsufficientFunds = true, Error = result.Error ?? "Insufficient wallet balance." });

            return BadRequest(new SessionOrderResultDto { Error = result.Error ?? "Unable to place order." });
        }

        var order = result.Order;
        return Ok(new SessionOrderResultDto
        {
            Success = true,
            OrderId = order.Id,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            SeatPath = string.IsNullOrWhiteSpace(order.SeatPath) ? BuildSeat(session) : order.SeatPath
        });
    }

    /// <summary>Reads order status for tracking, scoped to the session that placed it.</summary>
    [HttpGet("order/{orderId:int}")]
    public async Task<ActionResult<OrderDto>> GetSessionOrder(int orderId, [FromQuery] string sessionToken)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
            return BadRequest("Missing session token.");

        var session = await _ticketAuth.GetTicketSessionAsync(sessionToken);
        if (session is null)
            return Unauthorized("Invalid or expired session.");

        // Only expose an order that belongs to this session's ticket.
        var owned = await _db.Orders.AnyAsync(o => o.Id == orderId && o.TicketSessionId == session.Id);
        if (!owned)
            return NotFound();

        var dto = await _orderService.GetOrderByIdAsync(orderId);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Wallet snapshot (balance/status) for the fan behind the scanned ticket, so the walk-up cart can offer
    /// paying from the HALFTIME wallet. Gated by the session token like every action here. The wallet owner is
    /// resolved from the ticket, never supplied by the client. Returns a non-existent wallet
    /// (<c>Exists=false</c>) — not an error — when the ticket maps to no account or that account has no wallet.
    /// </summary>
    [HttpGet("wallet")]
    public async Task<ActionResult<WalletSummaryDto>> GetSessionWallet([FromQuery] string sessionToken)
    {
        if (string.IsNullOrWhiteSpace(sessionToken))
            return BadRequest("Missing session token.");

        var session = await _ticketAuth.GetTicketSessionAsync(sessionToken);
        if (session is null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            return Unauthorized("Your session has expired. Please scan your ticket again.");

        var userId = await ResolveWalletOwnerAsync(session);
        if (userId is not null)
            return Ok(await _walletService.GetSummaryAsync(userId.Value));

        // No linked account: fall back to the anonymous bearer balance loaded on the ticket itself, so the
        // cart can still offer "pay with ticket balance".
        var ticketId = session.Ticket?.Id;
        if (ticketId is not null)
        {
            var tw = await _walletService.GetTicketWalletSummaryAsync(ticketId.Value);
            if (tw.Exists)
                return Ok(new WalletSummaryDto
                {
                    Exists = true,
                    Balance = tw.Balance,
                    Currency = tw.Currency,
                    Status = tw.Status,
                    IsEligible = true,
                    IsTicketWallet = true
                });
        }

        return Ok(new WalletSummaryDto { Exists = false, IsEligible = false });
    }

    /// <summary>
    /// Fan-initiated ONLINE top-up (card) of the wallet behind the scanned ticket, from the walk-up flow
    /// where there is no login. Gated by the session token like every action here; the wallet is resolved
    /// server-side — the registered fan's HALFTIME wallet when the ticket maps to an account, otherwise the
    /// ticket's own bearer wallet (created on first top-up). Idempotent on the request's idempotency key.
    /// </summary>
    [HttpPost("wallet/topup")]
    public async Task<ActionResult<DepositResultDto>> TopUpSessionWallet([FromBody] SessionWalletTopupRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.SessionToken))
            return BadRequest(new DepositResultDto { Success = false, FailureReason = "MissingSession" });
        if (!ModelState.IsValid)
            return BadRequest(new DepositResultDto { Success = false, FailureReason = "InvalidAmount" });

        var session = await _ticketAuth.GetTicketSessionAsync(request.SessionToken);
        if (session is null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            return Unauthorized(new DepositResultDto { Success = false, FailureReason = "SessionExpired" });

        var dto = new InitiateDepositDto
        {
            Amount = request.Amount,
            Method = request.Method,
            IdempotencyKey = request.IdempotencyKey
        };

        // Fund whichever wallet the balance is displayed from, mirroring GetSessionWallet's resolution: the
        // registered fan's account wallet when the ticket links to one, otherwise the ticket's bearer wallet.
        var userId = await ResolveWalletOwnerAsync(session);
        DepositResultDto result;
        if (userId is not null)
        {
            result = await _walletService.InitiateDepositAsync(userId.Value, dto);
        }
        else
        {
            var ticketId = session.Ticket?.Id;
            if (ticketId is null)
                return BadRequest(new DepositResultDto { Success = false, FailureReason = "TicketNotFound" });
            result = await _walletService.InitiateTicketDepositAsync(ticketId.Value, dto);
        }

        return Ok(result);
    }

    /// <summary>
    /// Fan-initiated self-service WITHDRAWAL (refund to card) of part or all of the balance in the wallet
    /// behind the scanned ticket, from the walk-up flow where there is no login. Gated by the session token
    /// like every action here; the wallet is resolved server-side — the registered fan's HALFTIME wallet when
    /// the ticket maps to an account, otherwise the ticket's own bearer wallet. Idempotent on the request's
    /// idempotency key.
    /// </summary>
    [HttpPost("wallet/withdraw")]
    public async Task<ActionResult<WalletWithdrawResultDto>> WithdrawFromSessionWallet([FromBody] SessionWalletWithdrawRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.SessionToken))
            return BadRequest(new WalletWithdrawResultDto { Success = false, FailureReason = "MissingSession" });
        if (!ModelState.IsValid)
            return BadRequest(new WalletWithdrawResultDto { Success = false, FailureReason = "InvalidAmount" });

        var session = await _ticketAuth.GetTicketSessionAsync(request.SessionToken);
        if (session is null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            return Unauthorized(new WalletWithdrawResultDto { Success = false, FailureReason = "SessionExpired" });

        // Withdraw from whichever wallet the balance is displayed from, mirroring GetSessionWallet's
        // resolution: the registered fan's account wallet when the ticket links to one, else the ticket's
        // bearer wallet.
        var userId = await ResolveWalletOwnerAsync(session);
        WalletWithdrawResultDto result;
        if (userId is not null)
        {
            result = await _walletService.WithdrawAsync(userId.Value, request.Amount, request.IdempotencyKey);
        }
        else
        {
            var ticketId = session.Ticket?.Id;
            if (ticketId is null)
                return BadRequest(new WalletWithdrawResultDto { Success = false, FailureReason = "TicketNotFound" });
            result = await _walletService.WithdrawFromTicketWalletAsync(ticketId.Value, request.Amount, request.IdempotencyKey);
        }

        return Ok(result);
    }

    /// <summary>True when the scanned ticket carries its own Active (anonymous, bearer) wallet — the balance
    /// loaded on the ticket at the counter. Balance sufficiency is left to the guarded debit (→ 402 on
    /// insufficient funds); this only decides whether the ticket-wallet payment path applies at all.</summary>
    private async Task<bool> HasActiveTicketWalletAsync(TicketSession session)
    {
        var ticketId = session.Ticket?.Id;
        if (ticketId is null)
            return false;
        var tw = await _walletService.GetTicketWalletSummaryAsync(ticketId.Value);
        return tw.Exists && string.Equals(tw.Status, WalletStatus.Active, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Maps a scanned ticket to the fan account that owns its wallet. A season-pass–derived ticket points at
    /// its pass, whose linked account is the authoritative owner; a single-event ticket is matched best-effort
    /// on the buyer's email (they may hold a bar-topped-up wallet). No match → null (no wallet to show).
    /// </summary>
    private async Task<int?> ResolveWalletOwnerAsync(TicketSession session)
    {
        var ticket = session.Ticket;
        if (ticket is null)
            return null;

        if (ticket.SeasonTicketId is int seasonTicketId)
        {
            var ownerId = await _db.SeasonTickets
                .Where(st => st.Id == seasonTicketId && st.UserId != null)
                .Select(st => st.UserId)
                .FirstOrDefaultAsync();
            if (ownerId is not null)
                return ownerId;
        }

        if (!string.IsNullOrWhiteSpace(ticket.CustomerEmail))
            return await _db.Users
                .Where(u => u.Email == ticket.CustomerEmail)
                .Select(u => (int?)u.Id)
                .FirstOrDefaultAsync();

        return null;
    }

    private static string BuildSeat(TicketSession s)
    {
        var parts = new[] { s.Section, string.IsNullOrWhiteSpace(s.Row) ? "" : $"Red {s.Row}", string.IsNullOrWhiteSpace(s.SeatNumber) ? "" : $"Sjed. {s.SeatNumber}" }
            .Where(p => !string.IsNullOrWhiteSpace(p));
        return string.Join(" · ", parts);
    }

}
