namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// The three coarse phases of an event's life. Derived from <see cref="EventStatus"/>.
/// </summary>
public enum EventPhase
{
    /// <summary>Future event: planning &amp; ticketing. Drink ordering is locked.</summary>
    Future = 1,
    /// <summary>Active event: game-day operations. Drink ordering is open via ticket session.</summary>
    Active = 2,
    /// <summary>Past event: analytics &amp; auditing. All ordering disabled.</summary>
    Past = 3
}

/// <summary>
/// Centralised, authoritative rules for the event lifecycle. This is the single source of
/// truth that answers "given a status, what is allowed?" and "can the event move from A to B?".
/// All gating (ticket sales, drink ordering, session validity, closure) routes through here
/// rather than ad-hoc checks on EventDate / IsActive.
/// </summary>
public static class EventLifecycle
{
    /// <summary>
    /// Allowed status transitions. Any transition not listed here is rejected.
    /// </summary>
    private static readonly IReadOnlyDictionary<EventStatus, EventStatus[]> AllowedTransitions =
        new Dictionary<EventStatus, EventStatus[]>
        {
            [EventStatus.Planned]    = new[] { EventStatus.OnSale, EventStatus.Cancelled },
            [EventStatus.OnSale]     = new[] { EventStatus.SoldOut, EventStatus.Active, EventStatus.Planned, EventStatus.Cancelled },
            [EventStatus.SoldOut]    = new[] { EventStatus.OnSale, EventStatus.Active, EventStatus.Cancelled },
            [EventStatus.Active]     = new[] { EventStatus.InProgress, EventStatus.Completed, EventStatus.Cancelled },
            [EventStatus.InProgress] = new[] { EventStatus.Completed, EventStatus.Cancelled },
            [EventStatus.Completed]  = Array.Empty<EventStatus>(),
            [EventStatus.Cancelled]  = Array.Empty<EventStatus>(),
        };

    /// <summary>Returns the set of statuses an event may legally move to from its current status.</summary>
    public static IReadOnlyCollection<EventStatus> AllowedNextStatuses(EventStatus current) =>
        AllowedTransitions.TryGetValue(current, out var next) ? next : Array.Empty<EventStatus>();

    /// <summary>True when <paramref name="from"/> → <paramref name="to"/> is a legal transition.</summary>
    public static bool CanTransition(EventStatus from, EventStatus to) =>
        from != to && AllowedTransitions.TryGetValue(from, out var next) && Array.IndexOf(next, to) >= 0;

    /// <summary>Maps a status to one of the three high-level lifecycle phases.</summary>
    public static EventPhase PhaseOf(EventStatus status) => status switch
    {
        EventStatus.Planned or EventStatus.OnSale or EventStatus.SoldOut => EventPhase.Future,
        EventStatus.Active or EventStatus.InProgress => EventPhase.Active,
        _ => EventPhase.Past // Completed, Cancelled
    };

    /// <summary>Terminal statuses cannot transition any further.</summary>
    public static bool IsTerminal(EventStatus status) =>
        status is EventStatus.Completed or EventStatus.Cancelled;

    /// <summary>
    /// Phase 1: tickets/seats may be sold only while the event is explicitly on sale.
    /// Planned (not yet published), SoldOut, in-progress and past events cannot sell.
    /// </summary>
    public static bool CanSellTickets(EventStatus status) => status == EventStatus.OnSale;

    /// <summary>
    /// Phase 2: drink ordering (and issuing/keeping a ticket session) is permitted only once
    /// the event is live. Future and past events are locked.
    /// </summary>
    public static bool CanOrderDrinks(EventStatus status) =>
        status is EventStatus.Active or EventStatus.InProgress;

    /// <summary>
    /// A human-friendly reason used in API responses when an action is blocked by lifecycle state.
    /// </summary>
    public static string OrderingBlockedReason(EventStatus status) => PhaseOf(status) switch
    {
        EventPhase.Future => "Drink ordering opens when the event goes live. This event has not started yet.",
        EventPhase.Past => "This event has ended. Drink ordering is no longer available.",
        _ => "Drink ordering is not currently available for this event."
    };
}
