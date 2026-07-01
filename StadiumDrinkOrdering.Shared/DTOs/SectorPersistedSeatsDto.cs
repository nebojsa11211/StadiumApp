namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Summary of the persisted (database) seats that back a drawing-tool overlay sector.
/// The overlay is matched to a hierarchy <c>Sector</c> by its code; if a match with
/// generated seats exists, <see cref="IsPersisted"/> is true and the real per-row
/// counts are returned. Otherwise the caller falls back to a computed layout.
/// </summary>
public class SectorPersistedSeatsDto
{
    /// <summary>True when a hierarchy sector with generated seats was matched by code.</summary>
    public bool IsPersisted { get; set; }

    /// <summary>The overlay sector code that was used to resolve the match.</summary>
    public string SectorCode { get; set; } = string.Empty;

    /// <summary>Id of the matched hierarchy sector, if any (set even when it has no seats).</summary>
    public int? MatchedSectorId { get; set; }

    /// <summary>Name of the matched hierarchy sector, if any.</summary>
    public string? MatchedSectorName { get; set; }

    /// <summary>Total persisted seats across all rows of the matched sector.</summary>
    public int TotalSeats { get; set; }

    /// <summary>Persisted seats currently flagged available.</summary>
    public int AvailableSeats { get; set; }

    /// <summary>Per-row persisted seat counts, ordered by row number.</summary>
    public List<PersistedRowDto> Rows { get; set; } = new();
}

/// <summary>Persisted seat counts for a single row of a sector.</summary>
public class PersistedRowDto
{
    public int RowNumber { get; set; }
    public int SeatCount { get; set; }
    public int AvailableCount { get; set; }
}

/// <summary>A single persisted seat with its database-assigned unique code.</summary>
public class PersistedSeatDto
{
    public int Id { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string UniqueCode { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}
