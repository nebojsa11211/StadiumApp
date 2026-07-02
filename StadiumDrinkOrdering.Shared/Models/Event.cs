using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Event
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string EventType { get; set; } = string.Empty; // Football, Concert, Basketball, etc.
    
    [Required]
    public DateTime EventDate { get; set; }
    
    public int? VenueId { get; set; }
    
    [Required]
    public int TotalSeats { get; set; }
    
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Authoritative lifecycle state for the event. Drives what actions are permitted
    /// (ticket sales, drink ordering, analytics). See <see cref="EventLifecycle"/>.
    /// <c>IsActive</c> is kept as a separate "published/visible" flag.
    /// </summary>
    public EventStatus Status { get; set; } = EventStatus.Planned;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    
    public decimal? BaseTicketPrice { get; set; }

    /// <summary>
    /// Identifier of this event in the external ticketing system, if it originated there.
    /// Unique (nullable) so external events map 1:1 and ingestion is idempotent.
    /// </summary>
    [StringLength(100)]
    public string? ExternalEventId { get; set; }

    /// <summary>Name of the system that created this event (e.g. "TicketingSimulator"), if external.</summary>
    [StringLength(100)]
    public string? SourceSystem { get; set; }

    // Navigation properties
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<EventStaffAssignment> StaffAssignments { get; set; } = new List<EventStaffAssignment>();
    public virtual EventAnalytics? Analytics { get; set; }
}

public enum EventType
{
    Football = 1,
    Concert = 2,
    Basketball = 3,
    Baseball = 4,
    Hockey = 5,
    Tennis = 6,
    Boxing = 7,
    Other = 8
}

public enum EventStatus
{
    Planned = 1,
    OnSale = 2,
    SoldOut = 3,
    Active = 4,
    InProgress = 5,
    Completed = 6,
    Cancelled = 7
}