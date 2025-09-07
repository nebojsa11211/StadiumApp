using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class TicketSession
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string SessionId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string QRCodeToken { get; set; } = string.Empty;
    
    [Required]
    public int TicketId { get; set; }
    
    [Required]
    public int EventId { get; set; }
    
    public int? SeatId { get; set; }
    
    [StringLength(50)]
    public string SeatNumber { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Section { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Row { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? CustomerName { get; set; }
    
    [StringLength(200)]
    public string? CustomerEmail { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime? LastAccessedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual Ticket Ticket { get; set; } = null!;
    public virtual Event Event { get; set; } = null!;
    public virtual Seat? Seat { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}