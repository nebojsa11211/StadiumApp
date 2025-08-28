using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(10)]
    public string SeatNumber { get; set; } = string.Empty;
    
    [Required]
    public int CustomerId { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal TotalAmount { get; set; }
    
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? AcceptedAt { get; set; }
    public DateTime? PreparedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    
    public int? AcceptedByUserId { get; set; }
    public int? PreparedByUserId { get; set; }
    public int? DeliveredByUserId { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [StringLength(500)]
    public string? CustomerNotes { get; set; }
    
    // Navigation properties
    public virtual User Customer { get; set; } = null!;
    public virtual User? AcceptedByUser { get; set; }
    public virtual User? PreparedByUser { get; set; }
    public virtual User? DeliveredByUser { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual Payment? Payment { get; set; }
}

public enum OrderStatus
{
    Pending = 1,
    Accepted = 2,
    InPreparation = 3,
    Ready = 4,
    Delivered = 5,
    Cancelled = 6
}

