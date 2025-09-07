using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.API.Services;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderSessionController : ControllerBase
{
    private readonly IOrderSessionService _orderSessionService;
    private readonly IQRCodeService _qrCodeService;
    private readonly ILogger<OrderSessionController> _logger;

    public OrderSessionController(
        IOrderSessionService orderSessionService,
        IQRCodeService qrCodeService,
        ILogger<OrderSessionController> logger)
    {
        _orderSessionService = orderSessionService;
        _qrCodeService = qrCodeService;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionRequest request)
    {
        try
        {
            // Validate QR token first
            var isValidQR = await _qrCodeService.ValidateQRCodeAsync(request.QRToken);
            if (!isValidQR)
            {
                return BadRequest(new { message = "Invalid or expired QR code" });
            }

            var ticket = await _qrCodeService.GetTicketByQRTokenAsync(request.QRToken);
            if (ticket == null)
            {
                return BadRequest(new { message = "Ticket not found" });
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var session = await _orderSessionService.CreateSessionAsync(ticket.Id, ipAddress, userAgent);
            if (session == null)
            {
                return StatusCode(500, new { message = "Failed to create order session" });
            }

            return Ok(new
            {
                sessionToken = session.SessionToken,
                expiresAt = session.ExpiresAt,
                ticket = new
                {
                    ticketNumber = ticket.TicketNumber,
                    eventName = ticket.Event?.EventName,
                    eventDate = ticket.Event?.EventDate,
                    seatInfo = new
                    {
                        section = ticket.Seat?.Section?.SectionName ?? ticket.Section,
                        row = ticket.Seat?.RowNumber.ToString() ?? ticket.Row,
                        seat = ticket.Seat?.SeatNumber.ToString() ?? ticket.SeatNumber
                    }
                },
                cart = new
                {
                    items = new object[] { },
                    total = 0,
                    itemCount = 0
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting session for QR token {QRToken}", request.QRToken);
            return StatusCode(500, new { message = "An error occurred while starting session" });
        }
    }

    [HttpGet("{sessionToken}")]
    public async Task<IActionResult> GetSession(string sessionToken)
    {
        try
        {
            var session = await _orderSessionService.GetSessionByTokenAsync(sessionToken);
            if (session == null)
            {
                return NotFound(new { message = "Session not found or expired" });
            }

            var cartItems = string.IsNullOrEmpty(session.CartData) 
                ? new object[] { }
                : System.Text.Json.JsonSerializer.Deserialize<object[]>(session.CartData) ?? new object[] { };

            return Ok(new
            {
                sessionToken = session.SessionToken,
                expiresAt = session.ExpiresAt,
                ticket = new
                {
                    ticketNumber = session.Ticket?.TicketNumber,
                    eventName = session.Ticket?.Event?.EventName,
                    eventDate = session.Ticket?.Event?.EventDate,
                    seatInfo = new
                    {
                        section = session.Ticket?.Seat?.Section?.SectionName ?? session.Ticket?.Section,
                        row = session.Ticket?.Seat?.RowNumber.ToString() ?? session.Ticket?.Row,
                        seat = session.Ticket?.Seat?.SeatNumber.ToString() ?? session.Ticket?.SeatNumber
                    }
                },
                cart = new
                {
                    items = cartItems,
                    total = session.CartTotal ?? 0,
                    itemCount = session.ItemCount
                },
                lastActivity = session.LastActivity
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while retrieving session" });
        }
    }

    [HttpPost("{sessionToken}/cart/add")]
    public async Task<IActionResult> AddToCart(string sessionToken, [FromBody] AddToCartRequest request)
    {
        try
        {
            var session = await _orderSessionService.AddToCartAsync(
                sessionToken, 
                request.DrinkId, 
                request.Quantity, 
                request.SpecialInstructions);

            if (session == null)
            {
                return BadRequest(new { message = "Session not found or item unavailable" });
            }

            var cartItems = string.IsNullOrEmpty(session.CartData)
                ? new object[] { }
                : System.Text.Json.JsonSerializer.Deserialize<object[]>(session.CartData) ?? new object[] { };

            return Ok(new
            {
                message = "Item added to cart",
                cart = new
                {
                    items = cartItems,
                    total = session.CartTotal ?? 0,
                    itemCount = session.ItemCount
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart for session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while adding item to cart" });
        }
    }

    [HttpPost("{sessionToken}/cart/remove/{drinkId}")]
    public async Task<IActionResult> RemoveFromCart(string sessionToken, int drinkId)
    {
        try
        {
            var session = await _orderSessionService.RemoveFromCartAsync(sessionToken, drinkId);
            if (session == null)
            {
                return BadRequest(new { message = "Session not found" });
            }

            var cartItems = string.IsNullOrEmpty(session.CartData)
                ? new object[] { }
                : System.Text.Json.JsonSerializer.Deserialize<object[]>(session.CartData) ?? new object[] { };

            return Ok(new
            {
                message = "Item removed from cart",
                cart = new
                {
                    items = cartItems,
                    total = session.CartTotal ?? 0,
                    itemCount = session.ItemCount
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing item from cart for session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while removing item from cart" });
        }
    }

    [HttpPut("{sessionToken}/cart/update")]
    public async Task<IActionResult> UpdateCartItem(string sessionToken, [FromBody] UpdateCartItemRequest request)
    {
        try
        {
            var session = await _orderSessionService.UpdateCartItemAsync(
                sessionToken, 
                request.DrinkId, 
                request.Quantity);

            if (session == null)
            {
                return BadRequest(new { message = "Session not found" });
            }

            var cartItems = string.IsNullOrEmpty(session.CartData)
                ? new object[] { }
                : System.Text.Json.JsonSerializer.Deserialize<object[]>(session.CartData) ?? new object[] { };

            return Ok(new
            {
                message = "Cart item updated",
                cart = new
                {
                    items = cartItems,
                    total = session.CartTotal ?? 0,
                    itemCount = session.ItemCount
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item for session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while updating cart item" });
        }
    }

    [HttpPost("{sessionToken}/cart/clear")]
    public async Task<IActionResult> ClearCart(string sessionToken)
    {
        try
        {
            var session = await _orderSessionService.ClearCartAsync(sessionToken);
            if (session == null)
            {
                return BadRequest(new { message = "Session not found" });
            }

            return Ok(new
            {
                message = "Cart cleared",
                cart = new
                {
                    items = new object[] { },
                    total = 0,
                    itemCount = 0
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while clearing cart" });
        }
    }

    [HttpPost("{sessionToken}/checkout")]
    [Authorize]
    public async Task<IActionResult> Checkout(string sessionToken)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var customerId))
            {
                return Unauthorized(new { message = "Invalid user" });
            }

            var order = await _orderSessionService.CheckoutSessionAsync(sessionToken, customerId);
            if (order == null)
            {
                return BadRequest(new { message = "Session not found or cart is empty" });
            }

            return Ok(new
            {
                message = "Order created successfully",
                orderId = order.Id,
                totalAmount = order.TotalAmount,
                status = order.Status.ToString(),
                createdAt = order.CreatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking out session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred during checkout" });
        }
    }

    [HttpPost("{sessionToken}/extend")]
    public async Task<IActionResult> ExtendSession(string sessionToken)
    {
        try
        {
            var session = await _orderSessionService.ExtendSessionAsync(sessionToken);
            if (session == null)
            {
                return BadRequest(new { message = "Session not found" });
            }

            return Ok(new
            {
                message = "Session extended",
                expiresAt = session.ExpiresAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extending session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while extending session" });
        }
    }

    [HttpPost("{sessionToken}/invalidate")]
    public async Task<IActionResult> InvalidateSession(string sessionToken)
    {
        try
        {
            var success = await _orderSessionService.InvalidateSessionAsync(sessionToken);
            if (!success)
            {
                return BadRequest(new { message = "Session not found" });
            }

            return Ok(new { message = "Session invalidated" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating session {SessionToken}", sessionToken);
            return StatusCode(500, new { message = "An error occurred while invalidating session" });
        }
    }
}

// Request DTOs
public class StartSessionRequest
{
    public string QRToken { get; set; } = string.Empty;
}

public class AddToCartRequest
{
    public int DrinkId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? SpecialInstructions { get; set; }
}

public class UpdateCartItemRequest
{
    public int DrinkId { get; set; }
    public int Quantity { get; set; }
}