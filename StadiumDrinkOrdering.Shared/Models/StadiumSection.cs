using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class StadiumSection
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    public string SectionCode { get; set; } = string.Empty; // A, B, VIP, etc.
    
    [Required]
    [StringLength(50)]
    public string SectionName { get; set; } = string.Empty;
    
    [Required]
    public int TotalRows { get; set; }
    
    [Required]
    public int SeatsPerRow { get; set; }
    
    public decimal PriceMultiplier { get; set; } = 1.0m; // For VIP pricing
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(7)]
    public string Color { get; set; } = "#007bff"; // For visual representation
    
    // Navigation properties
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}

public class Seat
{
    public int Id { get; set; }
    
    [Required]
    public int SectionId { get; set; }
    
    [Required]
    public int RowNumber { get; set; }
    
    [Required]
    public int SeatNumber { get; set; }
    
    [Required]
    [StringLength(20)]
    public string SeatCode { get; set; } = string.Empty; // A-R1-S1
    
    public bool IsAccessible { get; set; } = false;
    
    // Coordinates for visual mapping
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    
    // Navigation properties
    public virtual StadiumSection Section { get; set; } = null!;
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}