using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UpdateEventDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>The kind of event (e.g. "Match", "Concert"). Null leaves the existing value unchanged.</summary>
    [StringLength(50)]
    public string? EventType { get; set; }

    /// <summary>
    /// Home side of a "Match" fixture — the name of one of the venue's resident clubs. Required when
    /// the (effective) type is "Match"; cleared for other types. Null leaves the existing value unchanged.
    /// </summary>
    [StringLength(100)]
    public string? HomeTeam { get; set; }

    /// <summary>Away/visiting side of a "Match" fixture (free text). Null leaves the existing value unchanged.</summary>
    [StringLength(100)]
    public string? AwayTeam { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    [Range(1, 100000)]
    public int? Capacity { get; set; }

    [Range(0.01, 10000)]
    public decimal? BasePrice { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }

    /// <summary>
    /// Per-sector ticket-price overrides for this event. When non-null, the supplied set fully
    /// replaces the event's existing overrides: entries with a price upsert an override, entries
    /// with a null price (or omitted sectors) clear it. Null leaves existing overrides unchanged.
    /// </summary>
    public List<EventSectorPriceInputDto>? SectorPrices { get; set; }
}