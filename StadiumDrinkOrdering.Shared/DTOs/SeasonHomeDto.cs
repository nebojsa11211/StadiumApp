namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// The season-member landing payload: their pass (fixed seat), the match that's live right now (if
/// any) with a token to start ordering, and upcoming fixtures. Drives the mobile <c>/home</c> screen.
/// </summary>
public class SeasonHomeDto
{
    public bool HasPass { get; set; }
    public string HolderName { get; set; } = string.Empty;
    public string SeasonName { get; set; } = string.Empty;
    public string SeatLabel { get; set; } = string.Empty;
    public string PassNumber { get; set; } = string.Empty;

    /// <summary>The member's match that is live now, or null between fixtures (countdown-only state).</summary>
    public SeasonLiveEventDto? LiveEvent { get; set; }

    public List<SeasonFixtureDto> Upcoming { get; set; } = new();
}

public class SeasonLiveEventDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }

    /// <summary>QR token of the member's derived ticket for this event — feeds the ordering session.
    /// Null if the per-event ticket hasn't been materialized yet (ordering unavailable).</summary>
    public string? SessionQrToken { get; set; }
}

public class SeasonFixtureDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
