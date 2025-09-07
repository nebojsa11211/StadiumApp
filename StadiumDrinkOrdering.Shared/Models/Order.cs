using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Order
{
    public int Id { get; set; }
    
    // New event-based fields
    public int? EventId { get; set; }
    public int? SeatId { get; set; }
    public int? PaymentId { get; set; }
    public int? SessionId { get; set; }
    public int? TicketSessionId { get; set; }
    
    [StringLength(500)]
    public string? DeliveryNotes { get; set; }
    
    public DateTime? EstimatedDeliveryTime { get; set; }
    public DateTime? ActualDeliveryTime { get; set; }
    
    public int? AssignedStaffId { get; set; }
    
    // Legacy fields (keeping for backward compatibility)
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
    public virtual Event? Event { get; set; }
    public virtual Seat? Seat { get; set; }
    public virtual Payment? PaymentDetails { get; set; }
    public virtual OrderSession? Session { get; set; }
    public virtual TicketSession? TicketSession { get; set; }
    public virtual User? AssignedStaff { get; set; }
    
    // Legacy navigation properties
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
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7
}

