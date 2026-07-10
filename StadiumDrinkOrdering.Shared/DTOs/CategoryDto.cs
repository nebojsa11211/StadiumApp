using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public int DrinkCount { get; set; }
}

public class CreateCategoryDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(16)]
    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }
}

public class UpdateCategoryDto
{
    [StringLength(50)]
    public string? Name { get; set; }

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(16)]
    public string? Icon { get; set; }

    public bool? IsActive { get; set; }

    public int? SortOrder { get; set; }
}
