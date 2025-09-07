using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly ApplicationDbContext _context;
    private readonly ISeatMappingService _seatMappingService;
    private readonly ILogger<ShoppingCartService> _logger;
    private const int RESERVATION_MINUTES = 15;

    public ShoppingCartService(
        ApplicationDbContext context, 
        ISeatMappingService seatMappingService,
        ILogger<ShoppingCartService> logger)
    {
        _context = context;
        _seatMappingService = seatMappingService;
        _logger = logger;
    }

    public async Task<ShoppingCart> GetOrCreateCartAsync(string sessionId, int? userId = null)
    {
        var existingCart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ExpiresAt > DateTime.UtcNow);

        if (existingCart != null)
        {
            // Update expiry and user if provided
            existingCart.UpdatedAt = DateTime.UtcNow;
            existingCart.ExpiresAt = DateTime.UtcNow.AddHours(2);
            if (userId.HasValue && existingCart.UserId == null)
            {
                existingCart.UserId = userId;
            }
            await _context.SaveChangesAsync();
            return existingCart;
        }

        // Create new cart
        var cart = new ShoppingCart
        {
            SessionId = sessionId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };

        _context.ShoppingCarts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<bool> AddSeatToCartAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber, decimal price, int? userId = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Check if seat is available
            if (!await IsSeatAvailableAsync(eventId, sectorId, rowNumber, seatNumber))
            {
                return false;
            }

            var cart = await GetOrCreateCartAsync(sessionId, userId);

            // Check if seat is already in cart
            var existingItem = cart.Items.FirstOrDefault(i => 
                i.EventId == eventId && 
                i.SectorId == sectorId && 
                i.RowNumber == rowNumber && 
                i.SeatNumber == seatNumber);

            if (existingItem != null)
            {
                return true; // Already in cart
            }

            // Reserve the seat
            if (!await ReserveSeatAsync(sessionId, eventId, sectorId, rowNumber, seatNumber, userId))
            {
                return false;
            }

            // Add to cart
            var seatCode = GenerateSeatCode(sectorId, rowNumber, seatNumber);
            var cartItem = new StadiumDrinkOrdering.Shared.Models.CartItem
            {
                ShoppingCartId = cart.Id,
                EventId = eventId,
                SectorId = sectorId,
                RowNumber = rowNumber,
                SeatNumber = seatNumber,
                SeatCode = seatCode,
                Price = price,
                AddedAt = DateTime.UtcNow,
                ReservedUntil = DateTime.UtcNow.AddMinutes(RESERVATION_MINUTES)
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Seat {SeatCode} added to cart for session {SessionId}", seatCode, sessionId);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error adding seat to cart for session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<bool> RemoveSeatFromCartAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);

            if (cart == null) return false;

            var cartItem = cart.Items.FirstOrDefault(i =>
                i.EventId == eventId &&
                i.SectorId == sectorId &&
                i.RowNumber == rowNumber &&
                i.SeatNumber == seatNumber);

            if (cartItem == null) return false;

            // Remove from cart
            _context.CartItems.Remove(cartItem);

            // Release seat reservation
            await ReleaseSeatAsync(sessionId, eventId, sectorId, rowNumber, seatNumber);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Seat removed from cart for session {SessionId}", sessionId);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error removing seat from cart for session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<ShoppingCart?> GetCartAsync(string sessionId)
    {
        var cart = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ExpiresAt > DateTime.UtcNow);
            
        if (cart != null)
        {
            cart.Items = await _context.CartItems
                .Where(i => i.ShoppingCartId == cart.Id)
                .ToListAsync();
        }
        
        return cart;
    }

    public async Task<bool> ClearCartAsync(string sessionId)
    {
        try
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);

            if (cart == null) return true;

            // Release all seat reservations
            foreach (var item in cart.Items)
            {
                await ReleaseSeatAsync(sessionId, item.EventId, item.SectorId, item.RowNumber, item.SeatNumber);
            }

            // Remove cart
            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart for session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<bool> ReserveSeatAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber, int? userId = null)
    {
        try
        {
            // Check for existing active reservation
            var existingReservation = await _context.SeatReservations
                .FirstOrDefaultAsync(r =>
                    r.EventId == eventId &&
                    r.SectorId == sectorId &&
                    r.RowNumber == rowNumber &&
                    r.SeatNumber == seatNumber &&
                    r.Status == ReservationStatus.Active &&
                    r.ReservedUntil > DateTime.UtcNow);

            if (existingReservation != null)
            {
                // If it's the same session, extend the reservation
                if (existingReservation.SessionId == sessionId)
                {
                    existingReservation.ReservedUntil = DateTime.UtcNow.AddMinutes(RESERVATION_MINUTES);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // Reserved by another session
            }

            // Create new reservation
            var reservation = new SeatReservation
            {
                EventId = eventId,
                SectorId = sectorId,
                RowNumber = rowNumber,
                SeatNumber = seatNumber,
                SeatCode = GenerateSeatCode(sectorId, rowNumber, seatNumber),
                SessionId = sessionId,
                UserId = userId,
                ReservedAt = DateTime.UtcNow,
                ReservedUntil = DateTime.UtcNow.AddMinutes(RESERVATION_MINUTES),
                Status = ReservationStatus.Active
            };

            _context.SeatReservations.Add(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reserving seat for session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<bool> ReleaseSeatAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber)
    {
        try
        {
            var reservation = await _context.SeatReservations
                .FirstOrDefaultAsync(r =>
                    r.SessionId == sessionId &&
                    r.EventId == eventId &&
                    r.SectorId == sectorId &&
                    r.RowNumber == rowNumber &&
                    r.SeatNumber == seatNumber &&
                    r.Status == ReservationStatus.Active);

            if (reservation != null)
            {
                reservation.Status = ReservationStatus.Released;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing seat for session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<bool> IsSeatAvailableAsync(int eventId, int sectorId, int rowNumber, int seatNumber)
    {
        // Check if seat is already sold (has a ticket)
        var seatCode = GenerateSeatCode(sectorId, rowNumber, seatNumber);
        var isTicketSold = await _context.Tickets
            .AnyAsync(t => t.EventId == eventId && 
                          (t.SeatNumber == seatNumber.ToString() && 
                           t.Row == rowNumber.ToString() && 
                           t.Section.Contains(sectorId.ToString())));

        if (isTicketSold) return false;

        // Check if seat is currently reserved by another session
        var isReserved = await _context.SeatReservations
            .AnyAsync(r => r.EventId == eventId &&
                          r.SectorId == sectorId &&
                          r.RowNumber == rowNumber &&
                          r.SeatNumber == seatNumber &&
                          r.Status == ReservationStatus.Active &&
                          r.ReservedUntil > DateTime.UtcNow);

        return !isReserved;
    }

    public async Task CleanupExpiredReservationsAsync()
    {
        try
        {
            var expiredReservations = await _context.SeatReservations
                .Where(r => r.Status == ReservationStatus.Active && r.ReservedUntil <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var reservation in expiredReservations)
            {
                reservation.Status = ReservationStatus.Expired;
            }

            if (expiredReservations.Any())
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired seat reservations", expiredReservations.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired reservations");
        }
    }

    public async Task<decimal> CalculateCartTotalAsync(string sessionId)
    {
        var cart = await GetCartAsync(sessionId);
        return cart?.Items.Sum(i => i.Price) ?? 0m;
    }

    private async Task CleanupExpiredCartItemsAsync(string sessionId)
    {
        // Find the cart first
        var cart = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => c.SessionId == sessionId);
            
        if (cart == null) return;
        
        // Then find expired items for this cart
        var expiredItems = await _context.CartItems
            .Where(i => i.ShoppingCartId == cart.Id && i.ReservedUntil <= DateTime.UtcNow)
            .ToListAsync();

        if (expiredItems.Any())
        {
            _context.CartItems.RemoveRange(expiredItems);
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateSeatCode(int sectorId, int rowNumber, int seatNumber)
    {
        return $"{sectorId}-R{rowNumber}-S{seatNumber}";
    }
}