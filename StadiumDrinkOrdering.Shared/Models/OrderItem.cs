using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class OrderItem
{
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int DrinkId { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, 999.99)]
    public decimal UnitPrice { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal TotalPrice { get; set; }
    
    [StringLength(200)]
    public string? SpecialInstructions { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Drink Drink { get; set; } = null!;
}



