using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class EventStaffAssignment
{
    public int Id { get; set; }
    
    [Required]
    public int EventId { get; set; }
    
    [Required]
    public int StaffId { get; set; }
    
    [StringLength(100)]
    public string? AssignedSections { get; set; } // JSON array of section IDs
    
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = string.Empty; // Waiter, Bartender, Supervisor
    
    public DateTime? ShiftStart { get; set; }
    
    public DateTime? ShiftEnd { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Event Event { get; set; } = null!;
    public virtual User Staff { get; set; } = null!;
}

public class EventAnalytics
{
    public int Id { get; set; }
    
    [Required]
    public int EventId { get; set; }
    
    public int TotalTicketsSold { get; set; } = 0;
    
    public decimal TotalRevenue { get; set; } = 0;
    
    public decimal TicketRevenue { get; set; } = 0;
    
    public decimal DrinksRevenue { get; set; } = 0;
    
    public int TotalOrders { get; set; } = 0;
    
    public int TotalDrinksSold { get; set; } = 0;
    
    public decimal AverageOrderValue { get; set; } = 0;
    
    public DateTime? PeakOrderTime { get; set; }
    
    [StringLength(100)]
    public string? MostPopularDrink { get; set; }
    
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Event Event { get; set; } = null!;
}