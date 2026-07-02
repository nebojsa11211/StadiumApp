using System;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Canonical ticket <see cref="Ticket.Status"/> values and the single authoritative
/// definition of when a ticket counts as occupying a seat ("sold").
///
/// Historically "sold" was checked several inconsistent ways across the codebase
/// ("Active", "sold", "Paid", or "has a CustomerName"). All occupancy/analytics
/// readers should funnel through <see cref="CountsAsSold"/> so that externally
/// ingested tickets (and internally created ones) are counted uniformly.
/// </summary>
public static class TicketStatuses
{
    public const string Active = "Active";
    public const string Used = "Used";
    public const string Cancelled = "Cancelled";

    /// <summary>
    /// A ticket occupies its seat (counts toward "sold"/occupancy) unless it has been
    /// cancelled. An existing, non-cancelled ticket means the seat is taken, regardless
    /// of whether it has since been scanned/used.
    /// </summary>
    public static bool CountsAsSold(string? status)
        => !string.IsNullOrWhiteSpace(status)
           && !string.Equals(status.Trim(), Cancelled, StringComparison.OrdinalIgnoreCase);
}
