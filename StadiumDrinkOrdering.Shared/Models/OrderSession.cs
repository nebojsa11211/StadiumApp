using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class OrderSession
{
    public int Id { get; set; }
    
    [Required]
    public int TicketId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string SessionToken { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(50)]
    public string? IpAddress { get; set; }
    
    [StringLength(500)]
    public string? UserAgent { get; set; }
    
    // Cart data stored as JSON
    public string? CartData { get; set; }
    
    public decimal? CartTotal { get; set; }
    
    // Session metadata
    public int ItemCount { get; set; } = 0;
    
    public DateTime? LastActivity { get; set; }
    
    // Navigation properties
    public virtual Ticket Ticket { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Notification
{
    public int Id { get; set; }
    
    public int? UserId { get; set; } // Nullable for broadcast notifications
    
    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty; // NewOrder, OrderReady, PaymentReceived, etc.
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;
    
    public string? Data { get; set; } // JSON payload
    
    public bool IsRead { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? ReadAt { get; set; }
    
    // Priority and targeting
    [StringLength(20)]
    public string Priority { get; set; } = "Normal"; // High, Normal, Low
    
    [StringLength(50)]
    public string? TargetRole { get; set; } // For role-based notifications
    
    public int? EventId { get; set; }
    
    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Event? Event { get; set; }
}