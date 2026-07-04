using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UpdateEventDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Range(1, 100000)]
    public int? Capacity { get; set; }

    [Range(0.01, 10000)]
    public decimal? BasePrice { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }
}