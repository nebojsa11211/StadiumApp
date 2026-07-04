using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CreateEventDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Required]
    [Range(1, 100000)]
    public int Capacity { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }
}