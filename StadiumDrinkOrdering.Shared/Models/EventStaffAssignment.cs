using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// The two functions a staff member can be given for an event. Stored in
/// <see cref="EventStaffAssignment.Role"/> and surfaced in the Admin event modal.
/// </summary>
public static class EventStaffRoles
{
    /// <summary>Delivers drinks to the seats in their assigned sectors (the Runner app).</summary>
    public const string Runner = "Runner";
    /// <summary>Works the bar / prepares drinks (the Bar app). Displayed as "Barman".</summary>
    public const string Bartender = "Bartender";

    /// <summary>Normalizes an arbitrary input to one of the two valid roles, defaulting to
    /// <paramref name="fallback"/> (or Runner) when it doesn't match either.</summary>
    public static string Normalize(string? value, string? fallback = null)
    {
        if (string.Equals(value, Bartender, StringComparison.OrdinalIgnoreCase)) return Bartender;
        if (string.Equals(value, Runner, StringComparison.OrdinalIgnoreCase)) return Runner;
        return fallback ?? Runner;
    }

    /// <summary>The default event function for a staff member with the given system role:
    /// a Bartender mans the bar, everyone else (Waiter) runs.</summary>
    public static string DefaultFor(UserRole systemRole) =>
        systemRole == UserRole.Bartender ? Bartender : Runner;
}

public class EventStaffAssignment
{
    public int Id { get; set; }
    
    [Required]
    public int EventId { get; set; }
    
    [Required]
    public int StaffId { get; set; }

    /// <summary>
    /// JSON array of overlay-sector ids (StadiumSectorOverlay.Id) this staff member covers for the event,
    /// e.g. <c>[12,13]</c>. Null/empty means no specific sectors. Widened well beyond a handful of ids so a
    /// runner covering many sectors still fits.
    /// </summary>
    [StringLength(1000)]
    public string? AssignedSections { get; set; }

    /// <summary>
    /// This staff member's function for the event: <see cref="EventStaffRoles.Runner"/> (delivers to the
    /// assigned sectors) or <see cref="EventStaffRoles.Bartender"/> (works the bar).
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = string.Empty;
    
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