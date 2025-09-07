using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/customer/cart")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _cartService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ShoppingCartController> _logger;

    public ShoppingCartController(IShoppingCartService cartService, ApplicationDbContext context, ILogger<ShoppingCartController> logger)
    {
        _cartService = cartService;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get current shopping cart contents
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ShoppingCartDto>> GetCart([FromQuery] string sessionId)
    {
        try
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Session ID is required");
            }

            var cart = await _cartService.GetCartAsync(sessionId);
            if (cart == null)
            {
                return Ok(new ShoppingCartDto { SessionId = sessionId, Items = new List<CartItemDto>() });
            }

            // Get event names for each item
            var eventIds = cart.Items.Select(i => i.EventId).Distinct().ToList();
            var events = await _context.Events
                .Where(e => eventIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.EventName);

            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                SessionId = cart.SessionId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                ExpiresAt = cart.ExpiresAt,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    EventId = i.EventId,
                    EventName = events.TryGetValue(i.EventId, out var eventName) ? eventName : "Unknown Event",
                    SectorId = i.SectorId,
                    RowNumber = i.RowNumber,
                    SeatNumber = i.SeatNumber,
                    SeatCode = i.SeatCode,
                    Price = i.Price,
                    AddedAt = i.AddedAt,
                    ReservedUntil = i.ReservedUntil
                }).ToList()
            };

            cartDto.TotalPrice = cartDto.Items.Sum(i => i.Price);
            return Ok(cartDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving shopping cart for session {SessionId}", sessionId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Add a seat to the shopping cart
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult> AddSeatToCart([FromBody] AddSeatToCartRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest("Session ID is required");
            }

            var success = await _cartService.AddSeatToCartAsync(
                request.SessionId,
                request.EventId,
                request.SectorId,
                request.RowNumber,
                request.SeatNumber,
                request.Price,
                request.UserId);

            if (success)
            {
                return Ok(new { message = "Seat added to cart successfully" });
            }
            else
            {
                return Conflict("Seat is no longer available");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding seat to cart");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Remove a seat from the shopping cart
    /// </summary>
    [HttpDelete("remove")]
    public async Task<ActionResult> RemoveSeatFromCart([FromBody] RemoveSeatFromCartRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest("Session ID is required");
            }

            var success = await _cartService.RemoveSeatFromCartAsync(
                request.SessionId,
                request.EventId,
                request.SectorId,
                request.RowNumber,
                request.SeatNumber);

            if (success)
            {
                return Ok(new { message = "Seat removed from cart successfully" });
            }
            else
            {
                return NotFound("Seat not found in cart");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing seat from cart");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Clear all items from the shopping cart
    /// </summary>
    [HttpDelete("clear")]
    public async Task<ActionResult> ClearCart([FromQuery] string sessionId)
    {
        try
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Session ID is required");
            }

            var success = await _cartService.ClearCartAsync(sessionId);
            
            if (success)
            {
                return Ok(new { message = "Cart cleared successfully" });
            }
            else
            {
                return StatusCode(500, "Failed to clear cart");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for session {SessionId}", sessionId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Check if a specific seat is available
    /// </summary>
    [HttpGet("seat-availability")]
    public async Task<ActionResult<SeatAvailabilityResponse>> CheckSeatAvailability(
        [FromQuery] int eventId,
        [FromQuery] int sectorId,
        [FromQuery] int rowNumber,
        [FromQuery] int seatNumber)
    {
        try
        {
            var isAvailable = await _cartService.IsSeatAvailableAsync(eventId, sectorId, rowNumber, seatNumber);
            
            return Ok(new SeatAvailabilityResponse
            {
                EventId = eventId,
                SectorId = sectorId,
                RowNumber = rowNumber,
                SeatNumber = seatNumber,
                IsAvailable = isAvailable
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking seat availability");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get cart summary with totals
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<CartSummaryDto>> GetCartSummary([FromQuery] string sessionId)
    {
        try
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return BadRequest("Session ID is required");
            }

            var cart = await _cartService.GetCartAsync(sessionId);
            var total = await _cartService.CalculateCartTotalAsync(sessionId);

            return Ok(new CartSummaryDto
            {
                SessionId = sessionId,
                ItemCount = cart?.Items.Count ?? 0,
                SubTotal = total,
                ServiceFee = total * 0.05m, // 5% service fee
                Tax = total * 0.08m, // 8% tax
                Total = total + (total * 0.05m) + (total * 0.08m)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart summary for session {SessionId}", sessionId);
            return StatusCode(500, "Internal server error");
        }
    }
}

// Request/Response DTOs
public class AddSeatToCartRequest
{
    public string SessionId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public int? UserId { get; set; }
}

public class RemoveSeatFromCartRequest
{
    public string SessionId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
}

public class SeatAvailabilityResponse
{
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public bool IsAvailable { get; set; }
}

public class ShoppingCartDto
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime ReservedUntil { get; set; }
}

public class CartSummaryDto
{
    public string SessionId { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}