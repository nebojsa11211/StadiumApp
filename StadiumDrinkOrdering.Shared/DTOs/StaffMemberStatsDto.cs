using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Activity statistics for a single staff/admin team member, shown when an
/// admin opens a member from the Users grid.
/// </summary>
public class StaffMemberStatsDto
{
    // Account
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Order-handling metrics (based on the workflow user-stamps on Order)
    public int OrdersAccepted { get; set; }
    public int OrdersInPreparation { get; set; }
    public int OrdersPrepared { get; set; }
    public int OrdersDelivered { get; set; }

    /// <summary>Distinct orders this member touched at any workflow stage.</summary>
    public int TotalOrdersHandled { get; set; }

    /// <summary>Total value of orders this member delivered.</summary>
    public decimal RevenueDelivered { get; set; }

    public DateTime? LastOrderHandledAt { get; set; }

    // Event assignments
    public int EventsAssigned { get; set; }
    public int UpcomingEventsAssigned { get; set; }
}
