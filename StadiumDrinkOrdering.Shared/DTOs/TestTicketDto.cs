namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Dev-only: a single pickable ticket for the Customer scan-page test combobox, so a developer can
/// choose a real ticket of the live event instead of typing/scanning a code. Never exposed in production —
/// the endpoint that returns these (<c>GET /events/live/test-tickets</c>) 404s outside Development.
/// </summary>
public class TestTicketDto
{
    /// <summary>The human-readable ticket number, used as the value navigated to at <c>/t/{TicketNumber}</c>.</summary>
    public string TicketNumber { get; set; } = string.Empty;

    /// <summary>Friendly label for the option (seat/section and holder name when known).</summary>
    public string Label { get; set; } = string.Empty;
}
