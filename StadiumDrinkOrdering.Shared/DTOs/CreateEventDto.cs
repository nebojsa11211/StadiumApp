using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CreateEventDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>The kind of event. Defaults to "Match"; other values e.g. "Concert" or free text.</summary>
    [StringLength(50)]
    public string EventType { get; set; } = "Match";

    /// <summary>
    /// Home side of a "Match" fixture — the name of one of the venue's resident clubs. Required when
    /// <see cref="EventType"/> is "Match"; ignored (and cleared) for other event types.
    /// </summary>
    [StringLength(100)]
    public string? HomeTeam { get; set; }

    /// <summary>Away/visiting side of a "Match" fixture (free text). Required when the type is "Match".</summary>
    [StringLength(100)]
    public string? AwayTeam { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    [Required]
    [Range(1, 100000)]
    public int Capacity { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }

    /// <summary>
    /// Optional per-sector ticket-price overrides for this event. Each entry with a non-null price
    /// overrides that sector's default; sectors omitted (or with a null price) keep their default.
    /// </summary>
    public List<EventSectorPriceInputDto>? SectorPrices { get; set; }
}