using Microsoft.AspNetCore.Components.Authorization;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.Shared.Services;

/// <summary>
/// Frontend authorization helper service implementation for role-based UI controls.
/// Provides consistent authorization logic across all frontend applications.
/// </summary>
public class AuthorizationHelperService : IAuthorizationHelperService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthorizationHelperService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public UserRole? GetCurrentUserRole()
    {
        var authState = GetAuthenticationState();
        var roleClaim = authState.User.FindFirst(ClaimTypes.Role);

        if (roleClaim != null && Enum.TryParse<UserRole>(roleClaim.Value, out var role))
        {
            return role;
        }

        return null;
    }

    public int? GetCurrentUserId()
    {
        var authState = GetAuthenticationState();
        var userIdClaim = authState.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    public string? GetCurrentUserEmail()
    {
        var authState = GetAuthenticationState();
        return authState.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    public bool IsAdmin()
    {
        return GetCurrentUserRole() == UserRole.Admin;
    }

    public bool IsStaff()
    {
        var role = GetCurrentUserRole();
        return role == UserRole.Admin || role == UserRole.Bartender || role == UserRole.Waiter;
    }

    public bool IsCustomer()
    {
        return GetCurrentUserRole() == UserRole.Customer;
    }

    public bool IsBartender()
    {
        return GetCurrentUserRole() == UserRole.Bartender;
    }

    public bool IsWaiter()
    {
        return GetCurrentUserRole() == UserRole.Waiter;
    }

    public bool IsAuthenticated()
    {
        var authState = GetAuthenticationState();
        return authState.User.Identity?.IsAuthenticated == true;
    }

    public bool CanReadOrders()
    {
        // All authenticated users can read orders (with ownership restrictions)
        return IsAuthenticated();
    }

    public bool CanCreateOrders()
    {
        // Customers and admins can create orders
        var role = GetCurrentUserRole();
        return role == UserRole.Customer || role == UserRole.Admin;
    }

    public bool CanUpdateOrders()
    {
        // Staff roles can update orders
        return IsStaff();
    }

    public bool CanDeleteOrders()
    {
        // Admins and customers can delete/cancel orders (with ownership restrictions)
        var role = GetCurrentUserRole();
        return role == UserRole.Admin || role == UserRole.Customer;
    }

    public bool CanManageUsers()
    {
        return IsAdmin();
    }

    public bool CanManageDrinks()
    {
        var role = GetCurrentUserRole();
        return role == UserRole.Admin || role == UserRole.Bartender;
    }

    public bool CanManageEvents()
    {
        return IsAdmin();
    }

    public bool CanViewAnalytics()
    {
        return IsStaff();
    }

    public bool CanManageSystem()
    {
        return IsAdmin();
    }

    public bool CanAccessLogs()
    {
        return IsAdmin();
    }

    public bool CanProcessPayments()
    {
        var role = GetCurrentUserRole();
        return role == UserRole.Customer || role == UserRole.Admin;
    }

    public bool CanManageQRCodes()
    {
        return IsStaff();
    }

    public bool CanManageStadiumStructure()
    {
        return IsAdmin();
    }

    public bool CanAccessOrder(int? orderCustomerId)
    {
        if (!IsAuthenticated())
            return false;

        // Staff can access all orders
        if (IsStaff())
            return true;

        // Customers can only access their own orders
        if (IsCustomer() && orderCustomerId.HasValue)
        {
            var currentUserId = GetCurrentUserId();
            return currentUserId == orderCustomerId.Value;
        }

        return false;
    }

    public bool CanAccessUserProfile(int? targetUserId)
    {
        if (!IsAuthenticated())
            return false;

        // Admins can access all profiles
        if (IsAdmin())
            return true;

        // Users can access their own profile
        if (targetUserId.HasValue)
        {
            var currentUserId = GetCurrentUserId();
            return currentUserId == targetUserId.Value;
        }

        return false;
    }

    public string GetUserDisplayName()
    {
        var email = GetCurrentUserEmail();
        if (!string.IsNullOrEmpty(email))
        {
            return email;
        }

        var userId = GetCurrentUserId();
        if (userId.HasValue)
        {
            return $"User {userId}";
        }

        return "Unknown User";
    }

    public string GetRoleCssClass()
    {
        return GetCurrentUserRole() switch
        {
            UserRole.Admin => "role-admin",
            UserRole.Bartender => "role-bartender",
            UserRole.Waiter => "role-waiter",
            UserRole.Customer => "role-customer",
            _ => "role-unknown"
        };
    }

    public string GetRoleColor()
    {
        return GetCurrentUserRole() switch
        {
            UserRole.Admin => "#dc3545", // Red
            UserRole.Bartender => "#28a745", // Green
            UserRole.Waiter => "#007bff", // Blue
            UserRole.Customer => "#6f42c1", // Purple
            _ => "#6c757d" // Gray
        };
    }

    public string GetRoleBadgeText()
    {
        return GetCurrentUserRole() switch
        {
            UserRole.Admin => "Admin",
            UserRole.Bartender => "Bartender",
            UserRole.Waiter => "Waiter",
            UserRole.Customer => "Customer",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Gets the current authentication state synchronously.
    /// Note: This should be used carefully as it may not reflect the latest state.
    /// For async operations, use GetAuthenticationStateAsync() directly.
    /// </summary>
    private AuthenticationState GetAuthenticationState()
    {
        try
        {
            // Try to get the current authentication state
            var task = _authenticationStateProvider.GetAuthenticationStateAsync();

            // Wait for the task to complete or timeout after 5 seconds
            if (task.Wait(TimeSpan.FromSeconds(5)))
            {
                return task.Result;
            }
            else
            {
                // If timeout, return an unauthenticated state
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
        catch
        {
            // If any error occurs, return an unauthenticated state
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}