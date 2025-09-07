namespace StadiumDrinkOrdering.Shared.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime ReservedUntil { get; set; }
    
    // Navigation properties
    public ShoppingCart ShoppingCart { get; set; } = null!;
    public Event Event { get; set; } = null!;
}

public class SeatReservation
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime ReservedUntil { get; set; }
    public ReservationStatus Status { get; set; }
    
    // Navigation properties
    public Event Event { get; set; } = null!;
    public User? User { get; set; }
}

public enum ReservationStatus
{
    Active,
    Expired,
    Purchased,
    Released
}