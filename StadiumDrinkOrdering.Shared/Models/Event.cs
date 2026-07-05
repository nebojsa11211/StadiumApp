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
    
    /// <summary>Start of the event (kept as the historical <c>EventDate</c> column).</summary>
    [Required]
    public DateTime EventDate { get; set; }

    /// <summary>
    /// End of the event. Together with <see cref="EventDate"/> this defines the window during
    /// which the event is considered "live" (see <see cref="IsLiveAt"/>). Nullable so legacy
    /// events and externally-ingested events without an explicit end remain valid.
    /// </summary>
    public DateTime? EventEndDate { get; set; }

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

    /// <summary>
    /// The season (e.g. "2026/2027") this event belongs to. Nullable so legacy events and
    /// one-off (non-league) events remain valid. Events linked to a season are covered by
    /// that season's season tickets.
    /// </summary>
    public int? SeasonId { get; set; }

    /// <summary>
    /// True when the event is in a live lifecycle phase (<see cref="EventStatus.Active"/> or
    /// <see cref="EventStatus.InProgress"/> — the authoritative game-day state) and has not yet
    /// ended at <paramref name="nowUtc"/>. When an explicit <see cref="EventEndDate"/> is set the
    /// window closes at that instant; otherwise the event is treated as live only through the end
    /// of its start day, so a past event with no end date does not stay "live" forever.
    /// </summary>
    public bool IsLiveAt(DateTime nowUtc) =>
        EventLifecycle.PhaseOf(Status) == EventPhase.Active
        && (EventEndDate.HasValue
            ? nowUtc <= EventEndDate.Value
            : nowUtc.Date <= EventDate.Date);

    // Navigation properties
    public virtual Season? Season { get; set; }
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