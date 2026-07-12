using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Event
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;
    
    /// <summary>
    /// The kind of event. Defaults to "Match" (a sporting fixture); other common values are
    /// "Concert" and "Other" (free text). Externally-ingested events may carry their own value
    /// (e.g. "Football"). This is the real event type — it does not encode location: every event
    /// is held at the one configured venue, so location is derived from Venue Settings, not stored.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string EventType { get; set; } = "Match";

    /// <summary>
    /// Home side of a versus-style fixture (e.g. "Istra"). Nullable so non-versus events
    /// (concerts, one-off shows) and legacy events remain valid. When both teams are set the
    /// landing renders a two-crest card; otherwise it shows a single title.
    /// </summary>
    [StringLength(100)]
    public string? HomeTeam { get; set; }

    /// <summary>Away side of a versus-style fixture (e.g. "Zagreb"). Null for non-versus events.</summary>
    [StringLength(100)]
    public string? AwayTeam { get; set; }

    /// <summary>Start of the event (kept as the historical <c>EventDate</c> column).</summary>
    [Required]
    public DateTime EventDate { get; set; }

    /// <summary>
    /// End of the event. Together with <see cref="EventDate"/> this defines the window during
    /// which the event is considered "live" (see <see cref="IsLiveAt"/>). Nullable so legacy
    /// events and externally-ingested events without an explicit end remain valid.
    /// </summary>
    public DateTime? EventEndDate { get; set; }

    /// <summary>
    /// Start of the ticket-sales window. Before this instant tickets cannot be sold even when the
    /// event is <see cref="EventStatus.OnSale"/>. Null means sales are open as soon as the event goes
    /// on sale (no lower bound). See <see cref="AreTicketSalesOpenAt"/>.
    /// </summary>
    public DateTime? TicketSalesStartDate { get; set; }

    /// <summary>
    /// End of the ticket-sales window. After this instant tickets can no longer be sold. Null means
    /// sales stay open for as long as the event is <see cref="EventStatus.OnSale"/> (no upper bound).
    /// </summary>
    public DateTime? TicketSalesEndDate { get; set; }

    /// <summary>
    /// Start of the drink-ordering window within the live event. Before this instant drinks cannot be
    /// ordered even while the event is live. Null means ordering opens as soon as the event goes live
    /// (no lower bound). See <see cref="AreDrinkSalesOpenAt"/>.
    /// </summary>
    public DateTime? DrinkSalesStartDate { get; set; }

    /// <summary>
    /// End of the drink-ordering window. After this instant drinks can no longer be ordered even while
    /// the event is still live (e.g. bars close before the final whistle). Null means ordering stays
    /// open for as long as the event is live (no upper bound).
    /// </summary>
    public DateTime? DrinkSalesEndDate { get; set; }

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

    /// <summary>
    /// True when <paramref name="nowUtc"/> falls within the optional ticket-sales window. A null
    /// bound leaves that side open, so an event with no window configured is always "within". This
    /// is purely the time check and ignores lifecycle status (see <see cref="AreTicketSalesOpenAt"/>).
    /// </summary>
    public bool IsWithinSalesWindow(DateTime nowUtc) =>
        (!TicketSalesStartDate.HasValue || nowUtc >= TicketSalesStartDate.Value)
        && (!TicketSalesEndDate.HasValue || nowUtc <= TicketSalesEndDate.Value);

    /// <summary>
    /// True when tickets/seats may be sold right now: the lifecycle permits sales
    /// (<see cref="EventLifecycle.CanSellTickets"/> — the event is on sale) AND the current time is
    /// inside the configured sales window (<see cref="IsWithinSalesWindow"/>).
    /// </summary>
    public bool AreTicketSalesOpenAt(DateTime nowUtc) =>
        EventLifecycle.CanSellTickets(Status) && IsWithinSalesWindow(nowUtc);

    /// <summary>
    /// A human-friendly reason why ticket sales are currently closed, or null when they are open.
    /// Distinguishes "not on sale", "not opened yet", and "closed" so the UI/API can be specific.
    /// </summary>
    public string? TicketSalesBlockedReason(DateTime nowUtc)
    {
        if (!EventLifecycle.CanSellTickets(Status))
            return "Tickets for this event are not currently on sale.";
        if (TicketSalesStartDate.HasValue && nowUtc < TicketSalesStartDate.Value)
            return "Ticket sales for this event have not opened yet.";
        if (TicketSalesEndDate.HasValue && nowUtc > TicketSalesEndDate.Value)
            return "Ticket sales for this event have closed.";
        return null;
    }

    /// <summary>
    /// True when <paramref name="nowUtc"/> falls within the optional drink-ordering window. A null
    /// bound leaves that side open, so an event with no window configured is always "within". This is
    /// purely the time check and ignores lifecycle status (see <see cref="AreDrinkSalesOpenAt"/>).
    /// </summary>
    public bool IsWithinDrinkSalesWindow(DateTime nowUtc) =>
        (!DrinkSalesStartDate.HasValue || nowUtc >= DrinkSalesStartDate.Value)
        && (!DrinkSalesEndDate.HasValue || nowUtc <= DrinkSalesEndDate.Value);

    /// <summary>
    /// True when drinks may be ordered right now: the lifecycle permits ordering
    /// (<see cref="EventLifecycle.CanOrderDrinks"/> — the event is live) AND the current time is inside
    /// the configured drink-ordering window (<see cref="IsWithinDrinkSalesWindow"/>). A null window adds
    /// no restriction beyond the live-phase rule, so existing events behave exactly as before.
    /// </summary>
    public bool AreDrinkSalesOpenAt(DateTime nowUtc) =>
        EventLifecycle.CanOrderDrinks(Status) && IsWithinDrinkSalesWindow(nowUtc);

    /// <summary>
    /// A human-friendly reason why drink ordering is currently closed, or null when it is open.
    /// Distinguishes the lifecycle reason (not live / ended) from the window reasons ("not opened yet",
    /// "closed") so the UI/API can be specific.
    /// </summary>
    public string? DrinkSalesBlockedReason(DateTime nowUtc)
    {
        if (!EventLifecycle.CanOrderDrinks(Status))
            return EventLifecycle.OrderingBlockedReason(Status);
        if (DrinkSalesStartDate.HasValue && nowUtc < DrinkSalesStartDate.Value)
            return "Drink ordering for this event has not opened yet.";
        if (DrinkSalesEndDate.HasValue && nowUtc > DrinkSalesEndDate.Value)
            return "Drink ordering for this event has closed.";
        return null;
    }

    // Navigation properties
    public virtual Season? Season { get; set; }
    /// <summary>Per-sector ticket-price overrides for this event (see <see cref="EventSectorPrice"/>).</summary>
    public virtual ICollection<EventSectorPrice> SectorPrices { get; set; } = new List<EventSectorPrice>();
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