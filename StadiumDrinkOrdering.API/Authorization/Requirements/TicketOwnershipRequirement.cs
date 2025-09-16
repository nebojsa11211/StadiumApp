using Microsoft.AspNetCore.Authorization;

namespace StadiumDrinkOrdering.API.Authorization;

/// <summary>
/// Authorization requirement to ensure users can only access their own tickets.
/// Customers can only view tickets they purchased, while staff can validate any ticket.
/// </summary>
public class TicketOwnershipRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Indicates whether staff roles should have access to validate all tickets
    /// </summary>
    public bool AllowStaffAccess { get; set; } = true;

    /// <summary>
    /// Indicates whether admin role should have full access to all tickets
    /// </summary>
    public bool AllowAdminAccess { get; set; } = true;
}