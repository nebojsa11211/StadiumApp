namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Post-event statistics for a single event: ticketing/occupancy on one side and
/// drink ordering on the other, plus combined revenue. Powers the admin
/// "/admin/event-statistics/{id}" page reachable from a completed event's card.
/// All figures exclude cancelled tickets/orders so they reflect realised sales.
/// </summary>
public class EventStatisticsDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    /// <summary>Authoritative lifecycle status name (e.g. "Completed", "Cancelled").</summary>
    public string Status { get; set; } = string.Empty;

    // --- Ticketing / occupancy ---

    /// <summary>Non-cancelled tickets issued for the event (single-event + season).</summary>
    public int TotalTicketsSold { get; set; }

    /// <summary>Of <see cref="TotalTicketsSold"/>, how many are season-pass–derived.</summary>
    public int SeasonTicketsSold { get; set; }

    /// <summary>Physical stadium capacity (sum of drawn overlay sectors); 0 when none drawn.</summary>
    public int TotalCapacity { get; set; }

    /// <summary>Tickets sold as a percentage of <see cref="TotalCapacity"/>, rounded to 1 dp.</summary>
    public decimal OccupancyPercent { get; set; }

    /// <summary>
    /// Sum of non-cancelled ticket prices. Season-derived tickets are priced at 0 (the pass is paid
    /// for once per season), so their money is reported separately in <see cref="SeasonTicketRevenue"/>.
    /// </summary>
    public decimal TicketRevenue { get; set; }

    /// <summary>
    /// This event's amortized share of the season passes covering it: each non-cancelled pass's price
    /// split evenly across the events it grants access to. Included in <see cref="TotalRevenue"/>.
    /// </summary>
    public decimal SeasonTicketRevenue { get; set; }

    // --- Drink ordering ---

    /// <summary>Non-cancelled drink orders placed during the event.</summary>
    public int TotalOrders { get; set; }

    /// <summary>Total drink units sold across all non-cancelled orders.</summary>
    public int TotalDrinksSold { get; set; }

    /// <summary>Sum of non-cancelled order totals.</summary>
    public decimal DrinksRevenue { get; set; }

    /// <summary>Mean value of a non-cancelled order; 0 when there were no orders.</summary>
    public decimal AverageOrderValue { get; set; }

    /// <summary>Name of the best-selling drink by quantity, or null when no drinks were sold.</summary>
    public string? MostPopularDrink { get; set; }

    // --- Combined ---

    /// <summary><see cref="TicketRevenue"/> + <see cref="SeasonTicketRevenue"/> + <see cref="DrinksRevenue"/>.</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>When these figures were computed (UTC).</summary>
    public DateTime CalculatedAt { get; set; }
}
