using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

public class Drink
{
    public int Id { get; set; }
    
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
    public int CategoryId { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [JsonIgnore] // Ignore to prevent circular references in JSON serialization
    public virtual Category? Category { get; set; }

    [JsonIgnore] // Ignore to prevent circular references in JSON serialization
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

