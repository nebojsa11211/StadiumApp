using System;
using System.Collections.Generic;

namespace StadiumDrinkOrdering.Shared.DTOs.Integration;

/// <summary>
/// Well-known <see cref="TicketingWebhookEnvelope.EventType"/> values sent by the
/// external ticketing system (the simulator during development).
/// </summary>
public static class TicketingEventTypes
{
    public const string EventCreated = "EventCreated";
    public const string EventUpdated = "EventUpdated";
    /// <summary>Slides the event window to "now" and moves its lifecycle to Active (game-day) so live-only features unlock.</summary>
    public const string EventWentLive = "EventWentLive";
    public const string TicketSold = "TicketSold";
    public const string TicketRefunded = "TicketRefunded";

    // Season / season-ticket (annual pass) events.
    public const string SeasonCreated = "SeasonCreated";
    public const string SeasonUpdated = "SeasonUpdated";
    public const string SeasonTicketSold = "SeasonTicketSold";
    public const string SeasonTicketRefunded = "SeasonTicketRefunded";
}

/// <summary>
/// The single envelope posted to the API webhook endpoint. Only the payload relevant to
/// <see cref="EventType"/> is populated. The raw JSON body is HMAC-signed (see the
/// <c>X-Signature</c> header) so the receiver can verify authenticity before trusting it.
/// </summary>
public class TicketingWebhookEnvelope
{
    /// <summary>One of <see cref="TicketingEventTypes"/>.</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>Globally unique key for this delivery; used for idempotent processing.</summary>
    public string IdempotencyKey { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; }

    public string SourceSystem { get; set; } = "TicketingSimulator";

    /// <summary>Populated for EventCreated / EventUpdated.</summary>
    public ExternalEventDto? Event { get; set; }

    /// <summary>Populated for TicketSold / TicketRefunded.</summary>
    public ExternalTicketDto? Ticket { get; set; }

    /// <summary>Populated for SeasonCreated / SeasonUpdated.</summary>
    public ExternalSeasonDto? Season { get; set; }

    /// <summary>Populated for SeasonTicketSold / SeasonTicketRefunded.</summary>
    public ExternalSeasonTicketDto? SeasonTicket { get; set; }
}

/// <summary>An event as defined by the external ticketing system.</summary>
public class ExternalEventDto
{
    public string ExternalEventId { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = "Football";
    public DateTime EventDate { get; set; }
    /// <summary>End of the event window, if the external system supplies one.</summary>
    public DateTime? EventEndDate { get; set; }
    public int TotalSeats { get; set; }
    public decimal? BaseTicketPrice { get; set; }
    public string? Description { get; set; }

    /// <summary>
    /// Links this event to a season (by the external season id). When set, existing season
    /// tickets for that season are extended to cover this event (a derived access ticket is
    /// generated for each pass's seat).
    /// </summary>
    public string? ExternalSeasonId { get; set; }
}

/// <summary>A season (e.g. "2026/2027") as defined by the external ticketing system.</summary>
public class ExternalSeasonDto
{
    public string ExternalSeasonId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
}

/// <summary>A season-ticket (annual pass) sale/refund from the external ticketing system.</summary>
public class ExternalSeasonTicketDto
{
    public string ExternalSeasonTicketId { get; set; } = string.Empty;
    public string ExternalSeasonId { get; set; } = string.Empty;

    /// <summary>Maps to <c>StadiumSection.SectionCode</c> on our side; the pass's seat is held here.</summary>
    public string SectionCode { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public string? HolderName { get; set; }
    public string? HolderEmail { get; set; }
    public DateTime SoldAt { get; set; }
}

/// <summary>A single ticket sale/refund from the external ticketing system.</summary>
public class ExternalTicketDto
{
    public string ExternalTicketId { get; set; } = string.Empty;
    public string ExternalEventId { get; set; } = string.Empty;

    /// <summary>Maps to <c>StadiumSection.SectionCode</c> on our side.</summary>
    public string SectionCode { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime SoldAt { get; set; }
}

/// <summary>Result returned from the webhook endpoint.</summary>
public class TicketingWebhookResult
{
    public bool Accepted { get; set; }

    /// <summary>True when the idempotency key was already processed (no state change).</summary>
    public bool Duplicate { get; set; }

    public string Message { get; set; } = string.Empty;

    public int? EventId { get; set; }
    public int? TicketId { get; set; }

    public int TotalSoldForEvent { get; set; }
    public int SoldInSection { get; set; }

    // Season / season-ticket outcomes.
    public int? SeasonId { get; set; }
    public int? SeasonTicketId { get; set; }

    /// <summary>Number of per-event access tickets generated/cancelled for a season-ticket operation.</summary>
    public int DerivedTicketsAffected { get; set; }
}

/// <summary>
/// Authoritative per-event sales snapshot used by the reconciliation safety net to
/// detect and heal drift from missed webhooks.
/// </summary>
public class EventSalesSnapshotDto
{
    public string ExternalEventId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int TotalSold { get; set; }
    public int TotalSeats { get; set; }

    /// <summary>Of <see cref="TotalSold"/>, how many are held by season passes (subset of sold).</summary>
    public int TotalSeasonSold { get; set; }

    public DateTime GeneratedAt { get; set; }
    public List<SectorSalesDto> Sectors { get; set; } = new();
}

public class SectorSalesDto
{
    public string SectionCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int Sold { get; set; }

    /// <summary>Of <see cref="Sold"/>, how many are held by season passes (subset of sold).</summary>
    public int SeasonSold { get; set; }
}

/// <summary>
/// Summary of an externally-originated event, so the external system/simulator can list the
/// events it has already created and resume selling into one (e.g. after a page reload).
/// </summary>
public class ExternalEventSummaryDto
{
    public int EventId { get; set; }
    public string ExternalEventId { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public DateTime? EventEndDate { get; set; }
    public string? SourceSystem { get; set; }
    public decimal? BaseTicketPrice { get; set; }
    public int TotalSold { get; set; }
    public int TotalSeats { get; set; }

    /// <summary>Display name of the event's authoritative lifecycle status (e.g. "OnSale", "Active").</summary>
    public string StatusName { get; set; } = string.Empty;

    /// <summary>
    /// Whether the external system may still sell tickets/seats into this event. False once the
    /// event goes live (or is sold out / past), so the simulator disables ticket selling.
    /// </summary>
    public bool CanSellTickets { get; set; }

    /// <summary>
    /// Whether the event is live and drink ordering is open — the simulator uses this to enable
    /// its "simulate drink orders" controls.
    /// </summary>
    public bool CanOrderDrinks { get; set; }
}

/// <summary>Outcome of a simulated live drink order requested by the external system/simulator.</summary>
public class SimulatedDrinkOrderResult
{
    public bool Accepted { get; set; }
    public string Message { get; set; } = string.Empty;

    public int? OrderId { get; set; }
    public int? EventId { get; set; }

    /// <summary>Seat the order was attached to (a currently-occupied seat in the live event).</summary>
    public string? SeatNumber { get; set; }

    /// <summary>Human-readable summary of what was ordered, e.g. "2× Beer, 1× Water".</summary>
    public string? OrderSummary { get; set; }

    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Summary of a season known to our side, so the external system/simulator can list seasons,
/// link events to them, and sell season tickets into one.
/// </summary>
public class ExternalSeasonSummaryDto
{
    public int SeasonId { get; set; }
    public string? ExternalSeasonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public int EventCount { get; set; }
    public int SeasonTicketCount { get; set; }
    public string? SourceSystem { get; set; }
}

/// <summary>
/// Minimal reference to a sold external ticket (section + external id), used by the
/// simulator to rebuild its local per-sector state when resuming an existing event.
/// </summary>
public class ExternalTicketRefDto
{
    public string SectionCode { get; set; } = string.Empty;
    public string ExternalTicketId { get; set; } = string.Empty;
}

/// <summary>
/// Lightweight view of a stadium section, exposed so the external system/simulator can
/// discover valid sector codes and capacities to sell into.
/// </summary>
public class StadiumSectionInfoDto
{
    public int Id { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

/// <summary>
/// Real-time notification broadcast over SignalR when a ticket is sold or refunded,
/// so the Admin overview can update occupancy live.
/// </summary>
public class TicketSoldNotification
{
    public int EventId { get; set; }
    public string ExternalEventId { get; set; } = string.Empty;
    public string SectionCode { get; set; } = string.Empty;
    public int SoldInSection { get; set; }
    public int SectionCapacity { get; set; }
    public int TotalSold { get; set; }
    public int TotalSeats { get; set; }
    public decimal OccupancyPercentage { get; set; }
    public DateTime Timestamp { get; set; }

    /// <summary>Sold | Refunded</summary>
    public string Action { get; set; } = "Sold";
}
