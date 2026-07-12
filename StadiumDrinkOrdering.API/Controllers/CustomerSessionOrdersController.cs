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
/// orders. Orders are attributed to a shared "walk-up guest" user; delivery routing uses the order's
/// ticket/seat, and status reads are scoped by the originating ticket session.
/// </summary>
[ApiController]
[Route("customer/session")]
[AllowAnonymous]
public class CustomerSessionOrdersController : ControllerBase
{
    private const string GuestEmail = "walkup-guest@stadium.local";

    private readonly ITicketAuthService _ticketAuth;
    private readonly IOrderService _orderService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CustomerSessionOrdersController> _logger;

    public CustomerSessionOrdersController(
        ITicketAuthService ticketAuth,
        IOrderService orderService,
        ApplicationDbContext db,
        ILogger<CustomerSessionOrdersController> logger)
    {
        _ticketAuth = ticketAuth;
        _orderService = orderService;
        _db = db;
        _logger = logger;
    }

    /// <summary>Places a drink order for the scanned seat. Created unpaid (walk-up fans have no wallet).</summary>
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

        var guestId = await GetGuestCustomerIdAsync();

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
            PaymentMethod = null // walk-up: order is created unpaid
        };

        if (createDto.OrderItems.Count == 0)
            return BadRequest(new SessionOrderResultDto { Error = "Your cart is empty." });

        var result = await _orderService.CreateOrderAsync(createDto, guestId);
        if (result.Outcome != CreateOrderOutcome.Success || result.Order is null)
            return BadRequest(new SessionOrderResultDto { Error = result.Error ?? "Unable to place order." });

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

    private static string BuildSeat(TicketSession s)
    {
        var parts = new[] { s.Section, string.IsNullOrWhiteSpace(s.Row) ? "" : $"Red {s.Row}", string.IsNullOrWhiteSpace(s.SeatNumber) ? "" : $"Sjed. {s.SeatNumber}" }
            .Where(p => !string.IsNullOrWhiteSpace(p));
        return string.Join(" · ", parts);
    }

    /// <summary>
    /// Resolves the shared "walk-up guest" customer that anonymous orders are attributed to, creating it
    /// on first use. Orders still carry the real ticket/seat, so this never affects delivery routing.
    /// </summary>
    private async Task<int> GetGuestCustomerIdAsync()
    {
        var guest = await _db.Users.FirstOrDefaultAsync(u => u.Email == GuestEmail);
        if (guest is not null)
            return guest.Id;

        guest = new User
        {
            Username = "Walk-up Guest",
            Email = GuestEmail,
            PasswordHash = "!disabled!", // non-usable; this account never logs in
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _db.Users.Add(guest);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Created walk-up guest customer #{Id}", guest.Id);
        return guest.Id;
    }
}
