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
    /// <summary>Ends a live event: closes its window at "now" and moves its lifecycle to Completed (past).</summary>
    public const string EventEnded = "EventEnded";
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

    /// <summary>Home side of a versus-style fixture, when the external system distinguishes teams.</summary>
    public string? HomeTeam { get; set; }
    /// <summary>Away side of a versus-style fixture. Null for non-versus events (e.g. concerts).</summary>
    public string? AwayTeam { get; set; }

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

    /// <summary>Croatian OIB of the pass holder (11 digits), when the external system captured one.</summary>
    public string? HolderOib { get; set; }

    /// <summary>Identity document number for a foreign pass holder with no OIB.</summary>
    public string? HolderDocumentNumber { get; set; }

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

    /// <summary>Croatian OIB of the buyer (11 digits), when the external system captured one.</summary>
    public string? CustomerOib { get; set; }

    /// <summary>Identity document number for a foreign buyer with no OIB.</summary>
    public string? CustomerDocumentNumber { get; set; }

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
/// Summary of an event the external system/simulator can list and resume selling into (e.g.
/// after a page reload). Includes events created in the Admin panel that have no external id
/// yet — for those <see cref="ExternalEventId"/> is null until the simulator adopts them.
/// </summary>
public class ExternalEventSummaryDto
{
    public int EventId { get; set; }

    /// <summary>
    /// The external ticketing id, or null for an Admin-created event that has not been adopted
    /// into the integration surface yet (see the adopt endpoint / EnsureExternalIdAsync).
    /// </summary>
    public string? ExternalEventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string? HomeTeam { get; set; }
    public string? AwayTeam { get; set; }
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

/// <summary>Outcome of an admin "simulate ticket sales" run (Stadium Overview testing tool).</summary>
public class SimulatedTicketSalesResult
{
    public bool Accepted { get; set; }
    public string Message { get; set; } = string.Empty;

    public int? EventId { get; set; }

    /// <summary>How many simulation tickets were actually created (sectors that were full are skipped).</summary>
    public int TicketsCreated { get; set; }
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

    /// <summary>Number of rows in the sector.</summary>
    public int Rows { get; set; }

    /// <summary>Seats per row (uniform mode); for variable seating this is the base row width.</summary>
    public int SeatsPerRow { get; set; }

    /// <summary>Sector classification: standard, vip, wheelchair, premium, family.</summary>
    public string Type { get; set; } = "standard";

    /// <summary>Display color for the sector on the stadium canvas (hex, e.g. <c>#007bff</c>).</summary>
    public string Color { get; set; } = "#007bff";

    /// <summary>Shape of the sector on the blueprint: rectangle, triangle, rhombus, circularsector, custompolygon.</summary>
    public string ShapeType { get; set; } = "rectangle";

    /// <summary>True when the sector uses variable seating (different seats per row); see <see cref="RowRanges"/>.</summary>
    public bool UseVariableSeating { get; set; }

    /// <summary>
    /// Per-range seat layout when <see cref="UseVariableSeating"/> is true (empty for uniform sectors,
    /// where every row has <see cref="SeatsPerRow"/> seats).
    /// </summary>
    public List<SectorRowRangeDto> RowRanges { get; set; } = new();

    // Position & size of the sector on the responsive stadium canvas (percentages, 0-100).
    public double TopPercent { get; set; }
    public double LeftPercent { get; set; }
    public double WidthPercent { get; set; }
    public double HeightPercent { get; set; }
}

/// <summary>A contiguous range of rows sharing the same seat count, for variable-seating sectors.</summary>
public class SectorRowRangeDto
{
    public int FromRow { get; set; }
    public int ToRow { get; set; }
    public int SeatsPerRow { get; set; }
}

/// <summary>Occupancy state of a single seat in a <see cref="SectorSeatMapDto"/>.</summary>
public enum SeatOccupancy
{
    Free = 0,
    /// <summary>Held by an ordinary single-match ticket.</summary>
    Sold = 1,
    /// <summary>Held by a season pass (a derived per-event access ticket).</summary>
    Season = 2
}

/// <summary>A single seat with its real row/number and occupancy for a specific event.</summary>
public class SectorSeatDto
{
    public int Row { get; set; }
    public int Number { get; set; }

    /// <summary>Positional seat code, e.g. <c>SECT1-R3-S12</c>.</summary>
    public string SeatCode { get; set; } = string.Empty;

    public SeatOccupancy Status { get; set; }

    /// <summary>External ticket id for a single-match sold seat (null for free/season seats).</summary>
    public string? ExternalTicketId { get; set; }

    /// <summary>Name on the ticket or season pass holding the seat, if any.</summary>
    public string? HolderName { get; set; }
}

/// <summary>
/// Full per-seat map of one sector for one event: every real seat position with its actual
/// row/number and whether it's free, single-match sold, or held by a season pass. Lets the
/// external system/simulator show the true seat layout the API assigned, not just counts.
/// </summary>
public class SectorSeatMapDto
{
    public string ExternalEventId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;

    /// <summary>Number of rows in the sector.</summary>
    public int Rows { get; set; }

    public int TotalSeats { get; set; }

    /// <summary>Occupied seats (single-match + season).</summary>
    public int SoldSeats { get; set; }

    /// <summary>Of <see cref="SoldSeats"/>, how many are held by season passes.</summary>
    public int SeasonSeats { get; set; }

    public int FreeSeats { get; set; }

    /// <summary>Every seat position in the sector, ordered by row then seat number.</summary>
    public List<SectorSeatDto> Seats { get; set; } = new();
}

/// <summary>
/// The ticket occupying a specific seat for an event, including its generated QR code, so the
/// external system/simulator can show the real ticket for a clicked seat. The QR is generated
/// on demand the first time a seat's ticket is requested.
/// </summary>
public class SeatTicketDto
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;

    public string SectionCode { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Number { get; set; }
    public string SeatCode { get; set; } = string.Empty;

    /// <summary><see cref="SeatOccupancy.Sold"/> or <see cref="SeatOccupancy.Season"/>.</summary>
    public SeatOccupancy Status { get; set; }

    public string? HolderName { get; set; }
    public string? ExternalTicketId { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }

    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }

    /// <summary>Data-URI PNG of the QR code (<c>data:image/png;base64,…</c>).</summary>
    public string? QRCode { get; set; }

    /// <summary>Opaque validation token encoded in the QR deep link.</summary>
    public string? QRCodeToken { get; set; }
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

/// <summary>
/// A login account exposed to the simulator's credentials tab so a tester can pick an
/// identity to sign into the Bar/Runner/Customer apps with. Development-only.
/// </summary>
public class IntegrationUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }

    /// <summary>Customer | Admin | Bartender | Waiter.</summary>
    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    /// <summary>Auto-provisioned shell account — has an unusable password hash, so it can never
    /// be logged into until the fan claims it.</summary>
    public bool IsShellAccount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Plaintext password, but only for the accounts the dev seeder creates with a known
    /// password. Null for every other user — stored passwords are BCrypt hashes and cannot
    /// be recovered.
    /// </summary>
    public string? KnownPassword { get; set; }
}
