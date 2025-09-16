using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.Services;

/// <summary>
/// Frontend authorization helper service interface for role-based UI controls.
/// Provides consistent authorization logic across all frontend applications.
/// </summary>
public interface IAuthorizationHelperService
{
    /// <summary>
    /// Gets the current user's role
    /// </summary>
    UserRole? GetCurrentUserRole();

    /// <summary>
    /// Gets the current user's ID
    /// </summary>
    int? GetCurrentUserId();

    /// <summary>
    /// Gets the current user's email
    /// </summary>
    string? GetCurrentUserEmail();

    /// <summary>
    /// Checks if the current user is an admin
    /// </summary>
    bool IsAdmin();

    /// <summary>
    /// Checks if the current user is staff (Admin, Bartender, or Waiter)
    /// </summary>
    bool IsStaff();

    /// <summary>
    /// Checks if the current user is a customer
    /// </summary>
    bool IsCustomer();

    /// <summary>
    /// Checks if the current user is a bartender
    /// </summary>
    bool IsBartender();

    /// <summary>
    /// Checks if the current user is a waiter
    /// </summary>
    bool IsWaiter();

    /// <summary>
    /// Checks if the current user is authenticated
    /// </summary>
    bool IsAuthenticated();

    /// <summary>
    /// Checks if the current user can read orders (any role-based access)
    /// </summary>
    bool CanReadOrders();

    /// <summary>
    /// Checks if the current user can create orders
    /// </summary>
    bool CanCreateOrders();

    /// <summary>
    /// Checks if the current user can update orders
    /// </summary>
    bool CanUpdateOrders();

    /// <summary>
    /// Checks if the current user can delete/cancel orders
    /// </summary>
    bool CanDeleteOrders();

    /// <summary>
    /// Checks if the current user can manage users
    /// </summary>
    bool CanManageUsers();

    /// <summary>
    /// Checks if the current user can manage drinks
    /// </summary>
    bool CanManageDrinks();

    /// <summary>
    /// Checks if the current user can manage events
    /// </summary>
    bool CanManageEvents();

    /// <summary>
    /// Checks if the current user can view analytics
    /// </summary>
    bool CanViewAnalytics();

    /// <summary>
    /// Checks if the current user can manage system settings
    /// </summary>
    bool CanManageSystem();

    /// <summary>
    /// Checks if the current user can access logs
    /// </summary>
    bool CanAccessLogs();

    /// <summary>
    /// Checks if the current user can process payments
    /// </summary>
    bool CanProcessPayments();

    /// <summary>
    /// Checks if the current user can manage QR codes
    /// </summary>
    bool CanManageQRCodes();

    /// <summary>
    /// Checks if the current user can manage stadium structure
    /// </summary>
    bool CanManageStadiumStructure();

    /// <summary>
    /// Checks if the current user can access a specific order (ownership-based)
    /// </summary>
    bool CanAccessOrder(int? orderCustomerId);

    /// <summary>
    /// Checks if the current user can access a specific user profile (ownership-based)
    /// </summary>
    bool CanAccessUserProfile(int? targetUserId);

    /// <summary>
    /// Gets the user's display name or email
    /// </summary>
    string GetUserDisplayName();

    /// <summary>
    /// Gets CSS classes for role-based styling
    /// </summary>
    string GetRoleCssClass();

    /// <summary>
    /// Gets role-specific color for UI elements
    /// </summary>
    string GetRoleColor();

    /// <summary>
    /// Gets role badge text for display
    /// </summary>
    string GetRoleBadgeText();
}