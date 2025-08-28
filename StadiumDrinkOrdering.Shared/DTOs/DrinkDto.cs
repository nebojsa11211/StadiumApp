using StadiumDrinkOrdering.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class DrinkDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public DrinkCategory Category { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateDrinkDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [Range(0.01, 999.99)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [Required]
    public DrinkCategory Category { get; set; }
    
    public bool IsAvailable { get; set; } = true;
}

public class UpdateDrinkDto
{
    [StringLength(100)]
    public string? Name { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Range(0.01, 999.99)]
    public decimal? Price { get; set; }
    
    [Range(0, int.MaxValue)]
    public int? StockQuantity { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public DrinkCategory? Category { get; set; }
    
    public bool? IsAvailable { get; set; }
}



