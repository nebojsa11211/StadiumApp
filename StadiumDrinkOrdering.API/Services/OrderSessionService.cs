using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Services;

public interface IOrderSessionService
{
    Task<OrderSession?> CreateSessionAsync(int ticketId, string ipAddress, string userAgent);
    Task<OrderSession?> GetSessionByTokenAsync(string sessionToken);
    Task<OrderSession?> GetActiveSessionByTicketAsync(int ticketId);
    Task<bool> ValidateSessionAsync(string sessionToken);
    Task<OrderSession?> ExtendSessionAsync(string sessionToken);
    Task<bool> InvalidateSessionAsync(string sessionToken);
    Task<OrderSession?> AddToCartAsync(string sessionToken, int drinkId, int quantity, string? specialInstructions = null);
    Task<OrderSession?> RemoveFromCartAsync(string sessionToken, int drinkId);
    Task<OrderSession?> UpdateCartItemAsync(string sessionToken, int drinkId, int quantity);
    Task<OrderSession?> ClearCartAsync(string sessionToken);
    Task<Order?> CheckoutSessionAsync(string sessionToken, int customerId);
    Task CleanupExpiredSessionsAsync();
}

public class OrderSessionService : IOrderSessionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderSessionService> _logger;

    public OrderSessionService(ApplicationDbContext context, ILogger<OrderSessionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OrderSession?> CreateSessionAsync(int ticketId, string ipAddress, string userAgent)
    {
        try
        {
            // Check if ticket exists and is valid
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(t => t.Id == ticketId && t.IsActive && !t.IsUsed);

            if (ticket == null)
            {
                _logger.LogWarning("Attempted to create session for invalid ticket {TicketId}", ticketId);
                return null;
            }

            // Check if there's already an active session
            var existingSession = await GetActiveSessionByTicketAsync(ticketId);
            if (existingSession != null)
            {
                // Extend existing session
                existingSession.ExpiresAt = DateTime.UtcNow.AddHours(2);
                existingSession.LastActivity = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existingSession;
            }

            // Create new session
            var session = new OrderSession
            {
                TicketId = ticketId,
                SessionToken = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                IsActive = true,
                IpAddress = ipAddress?.Length > 50 ? ipAddress[..50] : ipAddress,
                UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
                LastActivity = DateTime.UtcNow,
                CartData = JsonSerializer.Serialize(new List<CartItem>()),
                CartTotal = 0,
                ItemCount = 0
            };

            _context.OrderSessions.Add(session);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created order session {SessionToken} for ticket {TicketId}", session.SessionToken, ticketId);
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session for ticket {TicketId}", ticketId);
            return null;
        }
    }

    public async Task<OrderSession?> GetSessionByTokenAsync(string sessionToken)
    {
        return await _context.OrderSessions
            .Include(s => s.Ticket)
                .ThenInclude(t => t.Event)
            .Include(s => s.Ticket)
                .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Section)
            .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<OrderSession?> GetActiveSessionByTicketAsync(int ticketId)
    {
        return await _context.OrderSessions
            .Include(s => s.Ticket)
            .FirstOrDefaultAsync(s => s.TicketId == ticketId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<bool> ValidateSessionAsync(string sessionToken)
    {
        var session = await GetSessionByTokenAsync(sessionToken);
        return session != null;
    }

    public async Task<OrderSession?> ExtendSessionAsync(string sessionToken)
    {
        var session = await GetSessionByTokenAsync(sessionToken);
        if (session != null)
        {
            session.ExpiresAt = DateTime.UtcNow.AddHours(2);
            session.LastActivity = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return session;
    }

    public async Task<bool> InvalidateSessionAsync(string sessionToken)
    {
        var session = await _context.OrderSessions
            .FirstOrDefaultAsync(s => s.SessionToken == sessionToken);

        if (session != null)
        {
            session.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Invalidated session {SessionToken}", sessionToken);
            return true;
        }
        return false;
    }

    public async Task<OrderSession?> AddToCartAsync(string sessionToken, int drinkId, int quantity, string? specialInstructions = null)
    {
        try
        {
            var session = await GetSessionByTokenAsync(sessionToken);
            if (session == null) return null;

            var drink = await _context.Drinks.FindAsync(drinkId);
            if (drink == null || !drink.IsAvailable) return null;

            // Parse current cart
            var cartItems = string.IsNullOrEmpty(session.CartData) 
                ? new List<CartItem>() 
                : JsonSerializer.Deserialize<List<CartItem>>(session.CartData) ?? new List<CartItem>();

            // Check if item already exists in cart
            var existingItem = cartItems.FirstOrDefault(i => i.DrinkId == drinkId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
                if (!string.IsNullOrEmpty(specialInstructions))
                {
                    existingItem.SpecialInstructions = specialInstructions;
                }
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    DrinkId = drinkId,
                    DrinkName = drink.Name,
                    Quantity = quantity,
                    UnitPrice = drink.Price,
                    TotalPrice = quantity * drink.Price,
                    SpecialInstructions = specialInstructions
                });
            }

            // Update session
            session.CartData = JsonSerializer.Serialize(cartItems);
            session.CartTotal = cartItems.Sum(i => i.TotalPrice);
            session.ItemCount = cartItems.Sum(i => i.Quantity);
            session.LastActivity = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Added {Quantity}x {DrinkName} to cart for session {SessionToken}", 
                quantity, drink.Name, sessionToken);
                
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart for session {SessionToken}", sessionToken);
            return null;
        }
    }

    public async Task<OrderSession?> RemoveFromCartAsync(string sessionToken, int drinkId)
    {
        try
        {
            var session = await GetSessionByTokenAsync(sessionToken);
            if (session == null) return null;

            var cartItems = string.IsNullOrEmpty(session.CartData)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(session.CartData) ?? new List<CartItem>();

            cartItems.RemoveAll(i => i.DrinkId == drinkId);

            session.CartData = JsonSerializer.Serialize(cartItems);
            session.CartTotal = cartItems.Sum(i => i.TotalPrice);
            session.ItemCount = cartItems.Sum(i => i.Quantity);
            session.LastActivity = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing item from cart for session {SessionToken}", sessionToken);
            return null;
        }
    }

    public async Task<OrderSession?> UpdateCartItemAsync(string sessionToken, int drinkId, int quantity)
    {
        try
        {
            var session = await GetSessionByTokenAsync(sessionToken);
            if (session == null) return null;

            var cartItems = string.IsNullOrEmpty(session.CartData)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(session.CartData) ?? new List<CartItem>();

            var item = cartItems.FirstOrDefault(i => i.DrinkId == drinkId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cartItems.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                    item.TotalPrice = quantity * item.UnitPrice;
                }

                session.CartData = JsonSerializer.Serialize(cartItems);
                session.CartTotal = cartItems.Sum(i => i.TotalPrice);
                session.ItemCount = cartItems.Sum(i => i.Quantity);
                session.LastActivity = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item for session {SessionToken}", sessionToken);
            return null;
        }
    }

    public async Task<OrderSession?> ClearCartAsync(string sessionToken)
    {
        try
        {
            var session = await GetSessionByTokenAsync(sessionToken);
            if (session == null) return null;

            session.CartData = JsonSerializer.Serialize(new List<CartItem>());
            session.CartTotal = 0;
            session.ItemCount = 0;
            session.LastActivity = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for session {SessionToken}", sessionToken);
            return null;
        }
    }

    public async Task<Order?> CheckoutSessionAsync(string sessionToken, int customerId)
    {
        try
        {
            var session = await GetSessionByTokenAsync(sessionToken);
            if (session == null || session.CartTotal == 0) return null;

            var cartItems = JsonSerializer.Deserialize<List<CartItem>>(session.CartData ?? "[]") ?? new List<CartItem>();
            if (!cartItems.Any()) return null;

            // Create the order
            var order = new Order
            {
                TicketNumber = session.Ticket?.TicketNumber ?? "UNKNOWN",
                SeatNumber = session.Ticket?.Seat?.SeatCode ?? session.Ticket?.SeatNumber ?? "UNKNOWN", 
                CustomerId = customerId,
                TotalAmount = session.CartTotal ?? 0,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                EventId = session.Ticket?.EventId,
                SeatId = session.Ticket?.SeatId,
                SessionId = session.Id
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order items
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    DrinkId = cartItem.DrinkId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    TotalPrice = cartItem.TotalPrice,
                    SpecialInstructions = cartItem.SpecialInstructions
                };

                _context.OrderItems.Add(orderItem);
            }

            // Clear the cart but keep session active for potential additional orders
            session.CartData = JsonSerializer.Serialize(new List<CartItem>());
            session.CartTotal = 0;
            session.ItemCount = 0;
            session.LastActivity = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Created order {OrderId} from session {SessionToken}", order.Id, sessionToken);
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking out session {SessionToken}", sessionToken);
            return null;
        }
    }

    public async Task CleanupExpiredSessionsAsync()
    {
        try
        {
            var expiredSessions = await _context.OrderSessions
                .Where(s => s.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            foreach (var session in expiredSessions)
            {
                session.IsActive = false;
            }

            if (expiredSessions.Any())
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired sessions");
        }
    }
}

// Cart item model for JSON serialization
public class CartItem
{
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SpecialInstructions { get; set; }
}