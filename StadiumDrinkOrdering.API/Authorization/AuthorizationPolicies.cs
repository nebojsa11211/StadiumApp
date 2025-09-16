using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Authorization;

/// <summary>
/// Centralized authorization policy definitions for the Stadium Drink Ordering system.
/// Provides comprehensive role-based and resource-based authorization policies.
/// </summary>
public static class AuthorizationPolicies
{
    // ====================================================================================
    // CORE ROLE-BASED POLICIES
    // ====================================================================================

    /// <summary>
    /// Admin-only access policy for system administration functions
    /// </summary>
    public const string RequireAdminRole = "RequireAdminRole";

    /// <summary>
    /// Staff access policy for operations personnel (Admin + Bartender + Waiter)
    /// </summary>
    public const string RequireStaffRole = "RequireStaffRole";

    /// <summary>
    /// Customer access policy for authenticated customers
    /// </summary>
    public const string RequireCustomerRole = "RequireCustomerRole";

    /// <summary>
    /// Any authenticated user policy
    /// </summary>
    public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";

    // ====================================================================================
    // OPERATION-BASED POLICIES
    // ====================================================================================

    /// <summary>
    /// Read access to orders - varies by role and ownership
    /// </summary>
    public const string CanReadOrders = "CanReadOrders";

    /// <summary>
    /// Write access to create new orders
    /// </summary>
    public const string CanCreateOrders = "CanCreateOrders";

    /// <summary>
    /// Update existing orders (status changes, modifications)
    /// </summary>
    public const string CanUpdateOrders = "CanUpdateOrders";

    /// <summary>
    /// Delete/cancel orders
    /// </summary>
    public const string CanDeleteOrders = "CanDeleteOrders";

    // ====================================================================================
    // RESOURCE-BASED POLICIES
    // ====================================================================================

    /// <summary>
    /// Access own orders only - customers can only see their orders
    /// </summary>
    public const string CanAccessOwnOrders = "CanAccessOwnOrders";

    /// <summary>
    /// Access own user profile data only
    /// </summary>
    public const string CanAccessOwnProfile = "CanAccessOwnProfile";

    /// <summary>
    /// Access own tickets only
    /// </summary>
    public const string CanAccessOwnTickets = "CanAccessOwnTickets";

    // ====================================================================================
    // ADMINISTRATIVE POLICIES
    // ====================================================================================

    /// <summary>
    /// Manage users (create, update, delete user accounts)
    /// </summary>
    public const string CanManageUsers = "CanManageUsers";

    /// <summary>
    /// Manage drinks catalog
    /// </summary>
    public const string CanManageDrinks = "CanManageDrinks";

    /// <summary>
    /// Manage events and stadium structure
    /// </summary>
    public const string CanManageEvents = "CanManageEvents";

    /// <summary>
    /// Access analytics and reporting data
    /// </summary>
    public const string CanViewAnalytics = "CanViewAnalytics";

    /// <summary>
    /// Manage system configuration and settings
    /// </summary>
    public const string CanManageSystem = "CanManageSystem";

    // ====================================================================================
    // SPECIAL ACCESS POLICIES
    // ====================================================================================

    /// <summary>
    /// Access centralized logging system
    /// </summary>
    public const string CanAccessLogs = "CanAccessLogs";

    /// <summary>
    /// Perform payment operations
    /// </summary>
    public const string CanProcessPayments = "CanProcessPayments";

    /// <summary>
    /// Generate and validate QR codes
    /// </summary>
    public const string CanManageQRCodes = "CanManageQRCodes";

    /// <summary>
    /// Import/export stadium structure data
    /// </summary>
    public const string CanManageStadiumStructure = "CanManageStadiumStructure";

    // ====================================================================================
    // SIGNALR HUB POLICIES
    // ====================================================================================

    /// <summary>
    /// Access bartender SignalR hub for real-time order updates
    /// </summary>
    public const string CanAccessBartenderHub = "CanAccessBartenderHub";

    /// <summary>
    /// Access customer SignalR hub for order notifications
    /// </summary>
    public const string CanAccessCustomerHub = "CanAccessCustomerHub";

    /// <summary>
    /// Extension method to configure all authorization policies
    /// </summary>
    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // ====================================================================================
        // CORE ROLE-BASED POLICIES
        // ====================================================================================

        options.AddPolicy(RequireAdminRole, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        options.AddPolicy(RequireStaffRole, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString()));

        options.AddPolicy(RequireCustomerRole, policy =>
            policy.RequireRole(UserRole.Customer.ToString()));

        options.AddPolicy(RequireAuthenticatedUser, policy =>
            policy.RequireAuthenticatedUser());

        // ====================================================================================
        // OPERATION-BASED POLICIES
        // ====================================================================================

        options.AddPolicy(CanReadOrders, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString(),
                UserRole.Customer.ToString()));

        options.AddPolicy(CanCreateOrders, policy =>
            policy.RequireRole(
                UserRole.Customer.ToString(),
                UserRole.Admin.ToString()));

        options.AddPolicy(CanUpdateOrders, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString()));

        options.AddPolicy(CanDeleteOrders, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Customer.ToString())); // Customers can cancel their own orders

        // ====================================================================================
        // RESOURCE-BASED POLICIES
        // ====================================================================================

        options.AddPolicy(CanAccessOwnOrders, policy =>
            policy.Requirements.Add(new OrderOwnershipRequirement()));

        options.AddPolicy(CanAccessOwnProfile, policy =>
            policy.Requirements.Add(new UserOwnershipRequirement()));

        options.AddPolicy(CanAccessOwnTickets, policy =>
            policy.Requirements.Add(new TicketOwnershipRequirement()));

        // ====================================================================================
        // ADMINISTRATIVE POLICIES
        // ====================================================================================

        options.AddPolicy(CanManageUsers, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        options.AddPolicy(CanManageDrinks, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString()));

        options.AddPolicy(CanManageEvents, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        options.AddPolicy(CanViewAnalytics, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString()));

        options.AddPolicy(CanManageSystem, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        // ====================================================================================
        // SPECIAL ACCESS POLICIES
        // ====================================================================================

        options.AddPolicy(CanAccessLogs, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        options.AddPolicy(CanProcessPayments, policy =>
            policy.RequireRole(
                UserRole.Customer.ToString(),
                UserRole.Admin.ToString()));

        options.AddPolicy(CanManageQRCodes, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString()));

        options.AddPolicy(CanManageStadiumStructure, policy =>
            policy.RequireRole(UserRole.Admin.ToString()));

        // ====================================================================================
        // SIGNALR HUB POLICIES
        // ====================================================================================

        options.AddPolicy(CanAccessBartenderHub, policy =>
            policy.RequireRole(
                UserRole.Admin.ToString(),
                UserRole.Bartender.ToString(),
                UserRole.Waiter.ToString()));

        options.AddPolicy(CanAccessCustomerHub, policy =>
            policy.RequireRole(
                UserRole.Customer.ToString(),
                UserRole.Admin.ToString()));
    }
}