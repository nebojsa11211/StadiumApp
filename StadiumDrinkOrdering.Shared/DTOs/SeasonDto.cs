using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class SeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>Number of events linked to this season.</summary>
    public int EventCount { get; set; }
    /// <summary>Number of active season tickets (passes) sold for this season.</summary>
    public int SeasonTicketCount { get; set; }
    /// <summary>Origin system if the season was ingested externally (else null).</summary>
    public string? SourceSystem { get; set; }
}

/// <summary>
/// The current season plus its next upcoming (or currently live) fixture, for the public mobile
/// landing (<c>/welcome</c>). Sourced from the season's own linked events so it does not depend on
/// the customer ticketing list's <c>IsActive</c> filter.
/// </summary>
public class CurrentSeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    /// <summary>Total number of events (matches) linked to this season.</summary>
    public int EventCount { get; set; }
    /// <summary>The next upcoming or live fixture, or null if the season has no future events.</summary>
    public UpcomingEventDto? NextEvent { get; set; }
}

/// <summary>A season fixture surfaced on the public landing.</summary>
public class UpcomingEventDto
{
    public int Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    /// <summary>True when the event is live right now (drink ordering open).</summary>
    public bool IsLive { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>A single season pass (annual ticket) with its fixed seat, for the admin drill-down.</summary>
public class SeasonTicketDto
{
    public int Id { get; set; }
    public string SeasonTicketNumber { get; set; } = string.Empty;
    public string? HolderName { get; set; }
    public string? HolderEmail { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string? SourceSystem { get; set; }
}

/// <summary>A season pass's scannable QR: the deep-link URL it encodes plus a ready-to-render PNG.</summary>
public class SeasonPassQrDto
{
    public int SeasonTicketId { get; set; }
    public string PassToken { get; set; } = string.Empty;
    /// <summary>The URL encoded in the QR — <c>{CustomerApp:BaseUrl}/t/{PassToken}</c>.</summary>
    public string Url { get; set; } = string.Empty;
    /// <summary>A <c>data:image/png;base64,…</c> QR image, ready for an &lt;img src&gt;.</summary>
    public string ImageDataUri { get; set; } = string.Empty;
}

public class CreateSeasonDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsCurrent { get; set; }
}

public class UpdateSeasonDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsCurrent { get; set; }
}
