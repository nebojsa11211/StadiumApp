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
    public const string TicketSold = "TicketSold";
    public const string TicketRefunded = "TicketRefunded";
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
}

/// <summary>An event as defined by the external ticketing system.</summary>
public class ExternalEventDto
{
    public string ExternalEventId { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = "Football";
    public DateTime EventDate { get; set; }
    public int TotalSeats { get; set; }
    public decimal? BaseTicketPrice { get; set; }
    public string? Description { get; set; }
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
    public DateTime GeneratedAt { get; set; }
    public List<SectorSalesDto> Sectors { get; set; } = new();
}

public class SectorSalesDto
{
    public string SectionCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int Sold { get; set; }
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
