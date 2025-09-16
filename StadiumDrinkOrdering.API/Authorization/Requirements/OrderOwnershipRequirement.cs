using Microsoft.AspNetCore.Authorization;

namespace StadiumDrinkOrdering.API.Authorization;

/// <summary>
/// Authorization requirement to ensure users can only access their own orders.
/// Customers can only view/modify orders they created, while staff can access all orders.
/// </summary>
public class OrderOwnershipRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Indicates whether staff roles should have administrative access to all orders
    /// </summary>
    public bool AllowStaffAccess { get; set; } = true;

    /// <summary>
    /// Indicates whether admin role should have full access to all orders
    /// </summary>
    public bool AllowAdminAccess { get; set; } = true;
}