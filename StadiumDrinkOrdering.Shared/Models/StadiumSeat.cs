using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class StadiumSeat
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Section { get; set; } = string.Empty;
    
    [Required]
    public int RowNumber { get; set; }
    
    [Required]
    public int SeatNumber { get; set; }
    
    [Required]
    public int XCoordinate { get; set; }
    
    [Required]
    public int YCoordinate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
