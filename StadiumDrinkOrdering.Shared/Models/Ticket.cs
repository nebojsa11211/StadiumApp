using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Ticket
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(10)]
    public string SeatNumber { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? Section { get; set; }
    
    [StringLength(20)]
    public string? Row { get; set; }
    
    [StringLength(100)]
    public string? EventName { get; set; }
    
    public DateTime? EventDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}



