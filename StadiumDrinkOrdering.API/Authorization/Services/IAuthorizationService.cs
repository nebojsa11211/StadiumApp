using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Authorization.Services;

/// <summary>
/// Service interface for advanced authorization operations and helper methods.
/// Provides centralized authorization logic for complex business rules.
/// </summary>
public interface IStadiumAuthorizationService
{
    /// <summary>
    /// Checks if the current user can access the specified order
    /// </summary>
    Task<bool> CanAccessOrderAsync(ClaimsPrincipal user, int orderId);

    /// <summary>
    /// Checks if the current user can modify the specified order
    /// </summary>
    Task<bool> CanModifyOrderAsync(ClaimsPrincipal user, int orderId);

    /// <summary>
    /// Checks if the current user can access the specified user profile
    /// </summary>
    Task<bool> CanAccessUserProfileAsync(ClaimsPrincipal user, int targetUserId);

    /// <summary>
    /// Checks if the current user can access the specified ticket
    /// </summary>
    Task<bool> CanAccessTicketAsync(ClaimsPrincipal user, int ticketId);

    /// <summary>
    /// Checks if the current user can access the specified ticket by number
    /// </summary>
    Task<bool> CanAccessTicketByNumberAsync(ClaimsPrincipal user, string ticketNumber);

    /// <summary>
    /// Checks if the current user has admin privileges
    /// </summary>
    bool IsAdmin(ClaimsPrincipal user);

    /// <summary>
    /// Checks if the current user has staff privileges (Admin, Bartender, or Waiter)
    /// </summary>
    bool IsStaff(ClaimsPrincipal user);

    /// <summary>
    /// Checks if the current user is a customer
    /// </summary>
    bool IsCustomer(ClaimsPrincipal user);

    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    int? GetCurrentUserId(ClaimsPrincipal user);

    /// <summary>
    /// Gets the current user role from claims
    /// </summary>
    UserRole? GetCurrentUserRole(ClaimsPrincipal user);

    /// <summary>
    /// Gets the current user email from claims
    /// </summary>
    string? GetCurrentUserEmail(ClaimsPrincipal user);

    /// <summary>
    /// Validates that a user can perform an action on a resource based on ownership
    /// </summary>
    Task<bool> ValidateResourceOwnershipAsync(ClaimsPrincipal user, string resourceType, int resourceId);

    /// <summary>
    /// Checks if the current user can access analytics data
    /// </summary>
    bool CanAccessAnalytics(ClaimsPrincipal user);

    /// <summary>
    /// Checks if the current user can manage system settings
    /// </summary>
    bool CanManageSystem(ClaimsPrincipal user);

    /// <summary>
    /// Checks if the current user can access centralized logs
    /// </summary>
    bool CanAccessLogs(ClaimsPrincipal user);
}