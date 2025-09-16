using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Authorization.Services;

/// <summary>
/// Implementation of advanced authorization operations and helper methods.
/// Provides centralized authorization logic for complex business rules.
/// </summary>
public class StadiumAuthorizationService : IStadiumAuthorizationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StadiumAuthorizationService> _logger;

    public StadiumAuthorizationService(ApplicationDbContext context, ILogger<StadiumAuthorizationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CanAccessOrderAsync(ClaimsPrincipal user, int orderId)
    {
        try
        {
            var userId = GetCurrentUserId(user);
            var userRole = GetCurrentUserRole(user);

            if (!userId.HasValue || !userRole.HasValue)
            {
                return false;
            }

            // Admin and staff can access all orders
            if (IsAdmin(user) || IsStaff(user))
            {
                return true;
            }

            // Customers can only access their own orders
            if (userRole == UserRole.Customer)
            {
                var orderExists = await _context.Orders
                    .AnyAsync(o => o.Id == orderId && o.CustomerId == userId.Value);
                return orderExists;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking order access for user and order {OrderId}", orderId);
            return false;
        }
    }

    public async Task<bool> CanModifyOrderAsync(ClaimsPrincipal user, int orderId)
    {
        try
        {
            var userId = GetCurrentUserId(user);
            var userRole = GetCurrentUserRole(user);

            if (!userId.HasValue || !userRole.HasValue)
            {
                return false;
            }

            // Admin and staff can modify orders
            if (IsAdmin(user) || IsStaff(user))
            {
                return true;
            }

            // Customers can only cancel their own orders (limited modification)
            if (userRole == UserRole.Customer)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == userId.Value);

                // Customer can only modify orders that are pending or in early stages
                return order != null && (order.Status == OrderStatus.Pending);
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking order modification rights for user and order {OrderId}", orderId);
            return false;
        }
    }

    public async Task<bool> CanAccessUserProfileAsync(ClaimsPrincipal user, int targetUserId)
    {
        try
        {
            var userId = GetCurrentUserId(user);

            if (!userId.HasValue)
            {
                return false;
            }

            // Admin can access all profiles
            if (IsAdmin(user))
            {
                return true;
            }

            // Users can access their own profile
            return userId.Value == targetUserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user profile access for user and target {TargetUserId}", targetUserId);
            return false;
        }
    }

    public async Task<bool> CanAccessTicketAsync(ClaimsPrincipal user, int ticketId)
    {
        try
        {
            var userId = GetCurrentUserId(user);
            var userRole = GetCurrentUserRole(user);

            if (!userId.HasValue || !userRole.HasValue)
            {
                return false;
            }

            // Admin and staff can access all tickets
            if (IsAdmin(user) || IsStaff(user))
            {
                return true;
            }

            // Customers can only access their own tickets
            if (userRole == UserRole.Customer)
            {
                var ticketExists = await _context.Tickets
                    .AnyAsync(t => t.Id == ticketId && t.Orders.Any(o => o.CustomerId == userId.Value));
                return ticketExists;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ticket access for user and ticket {TicketId}", ticketId);
            return false;
        }
    }

    public async Task<bool> CanAccessTicketByNumberAsync(ClaimsPrincipal user, string ticketNumber)
    {
        try
        {
            var userId = GetCurrentUserId(user);
            var userRole = GetCurrentUserRole(user);

            if (!userId.HasValue || !userRole.HasValue || string.IsNullOrEmpty(ticketNumber))
            {
                return false;
            }

            // Admin and staff can access all tickets
            if (IsAdmin(user) || IsStaff(user))
            {
                return true;
            }

            // Customers can only access their own tickets
            if (userRole == UserRole.Customer)
            {
                var ticketExists = await _context.Tickets
                    .AnyAsync(t => t.TicketNumber == ticketNumber && t.Orders.Any(o => o.CustomerId == userId.Value));
                return ticketExists;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ticket access for user and ticket number {TicketNumber}", ticketNumber);
            return false;
        }
    }

    public bool IsAdmin(ClaimsPrincipal user)
    {
        var userRole = GetCurrentUserRole(user);
        return userRole == UserRole.Admin;
    }

    public bool IsStaff(ClaimsPrincipal user)
    {
        var userRole = GetCurrentUserRole(user);
        return userRole == UserRole.Admin ||
               userRole == UserRole.Bartender ||
               userRole == UserRole.Waiter;
    }

    public bool IsCustomer(ClaimsPrincipal user)
    {
        var userRole = GetCurrentUserRole(user);
        return userRole == UserRole.Customer;
    }

    public int? GetCurrentUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    public UserRole? GetCurrentUserRole(ClaimsPrincipal user)
    {
        var roleClaim = user.FindFirst(ClaimTypes.Role);
        if (roleClaim != null && Enum.TryParse<UserRole>(roleClaim.Value, out var role))
        {
            return role;
        }
        return null;
    }

    public string? GetCurrentUserEmail(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<bool> ValidateResourceOwnershipAsync(ClaimsPrincipal user, string resourceType, int resourceId)
    {
        try
        {
            return resourceType.ToLower() switch
            {
                "order" => await CanAccessOrderAsync(user, resourceId),
                "ticket" => await CanAccessTicketAsync(user, resourceId),
                "user" => await CanAccessUserProfileAsync(user, resourceId),
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating resource ownership for user, resource type {ResourceType} and ID {ResourceId}",
                resourceType, resourceId);
            return false;
        }
    }

    public bool CanAccessAnalytics(ClaimsPrincipal user)
    {
        // Analytics access is restricted to staff roles
        return IsStaff(user);
    }

    public bool CanManageSystem(ClaimsPrincipal user)
    {
        // System management is restricted to admins only
        return IsAdmin(user);
    }

    public bool CanAccessLogs(ClaimsPrincipal user)
    {
        // Log access is restricted to admins only
        return IsAdmin(user);
    }
}