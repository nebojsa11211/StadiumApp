using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UpdateEventDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Range(1, 100000)]
    public int? Capacity { get; set; }

    [Range(0.01, 10000)]
    public decimal? BasePrice { get; set; }

    public bool? IsActive { get; set; }
}