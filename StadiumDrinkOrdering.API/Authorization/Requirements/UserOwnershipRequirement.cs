using Microsoft.AspNetCore.Authorization;

namespace StadiumDrinkOrdering.API.Authorization;

/// <summary>
/// Authorization requirement to ensure users can only access their own profile data.
/// Users can only view/modify their own profile, while admins can manage all users.
/// </summary>
public class UserOwnershipRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Indicates whether admin role should have full access to all user profiles
    /// </summary>
    public bool AllowAdminAccess { get; set; } = true;
}