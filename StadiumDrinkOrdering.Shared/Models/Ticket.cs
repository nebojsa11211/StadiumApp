using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Ticket
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
    
    [Required]
    public int EventId { get; set; }
    
    public int? SeatId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string QRCode { get; set; } = string.Empty; // Base64 encoded QR image or URL
    
    [Required]
    [StringLength(100)]
    public string QRCodeToken { get; set; } = string.Empty; // Unique token for validation
    
    [StringLength(100)]
    public string? CustomerName { get; set; }
    
    [StringLength(100)]
    public string? CustomerEmail { get; set; }
    
    [StringLength(20)]
    public string? CustomerPhone { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }
    
    public bool IsUsed { get; set; } = false;
    
    public DateTime? UsedAt { get; set; }
    
    [StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Used, Cancelled
    
    // Legacy fields for backward compatibility
    [StringLength(10)]
    public string? SeatNumber { get; set; }
    
    [StringLength(20)]
    public string? Section { get; set; }
    
    [StringLength(20)]
    public string? Row { get; set; }
    
    [StringLength(100)]
    public string? EventName { get; set; }
    
    public DateTime? EventDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Event Event { get; set; } = null!;
    public virtual Seat Seat { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<OrderSession> OrderSessions { get; set; } = new List<OrderSession>();
}



