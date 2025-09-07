using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IShoppingCartService
{
    Task<ShoppingCart> GetOrCreateCartAsync(string sessionId, int? userId = null);
    Task<bool> AddSeatToCartAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber, decimal price, int? userId = null);
    Task<bool> RemoveSeatFromCartAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber);
    Task<ShoppingCart?> GetCartAsync(string sessionId);
    Task<bool> ClearCartAsync(string sessionId);
    Task<bool> ReserveSeatAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber, int? userId = null);
    Task<bool> ReleaseSeatAsync(string sessionId, int eventId, int sectorId, int rowNumber, int seatNumber);
    Task<bool> IsSeatAvailableAsync(int eventId, int sectorId, int rowNumber, int seatNumber);
    Task CleanupExpiredReservationsAsync();
    Task<decimal> CalculateCartTotalAsync(string sessionId);
}