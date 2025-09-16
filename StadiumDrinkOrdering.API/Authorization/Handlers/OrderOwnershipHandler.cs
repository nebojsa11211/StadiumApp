using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Authorization.Handlers;

/// <summary>
/// Authorization handler for order ownership verification.
/// Ensures customers can only access their own orders while allowing staff administrative access.
/// </summary>
public class OrderOwnershipHandler : AuthorizationHandler<OrderOwnershipRequirement>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderOwnershipHandler> _logger;

    public OrderOwnershipHandler(ApplicationDbContext context, ILogger<OrderOwnershipHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OrderOwnershipRequirement requirement)
    {
        try
        {
            // Get user information from claims
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
            {
                _logger.LogWarning("Order ownership check failed: Missing user ID or role claims");
                context.Fail();
                return;
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("Order ownership check failed: Invalid user ID format: {UserId}", userIdClaim.Value);
                context.Fail();
                return;
            }

            var userRole = userRoleClaim.Value;

            // Admin access check
            if (requirement.AllowAdminAccess && userRole == UserRole.Admin.ToString())
            {
                _logger.LogDebug("Order access granted to admin user: {UserId}", userId);
                context.Succeed(requirement);
                return;
            }

            // Staff access check
            if (requirement.AllowStaffAccess &&
                (userRole == UserRole.Bartender.ToString() || userRole == UserRole.Waiter.ToString()))
            {
                _logger.LogDebug("Order access granted to staff user: {UserId} with role: {UserRole}", userId, userRole);
                context.Succeed(requirement);
                return;
            }

            // For customers, check order ownership
            if (userRole == UserRole.Customer.ToString())
            {
                var orderId = GetOrderIdFromContext(context);
                if (orderId.HasValue)
                {
                    var isOwner = await IsOrderOwner(userId, orderId.Value);
                    if (isOwner)
                    {
                        _logger.LogDebug("Order access granted to customer: {UserId} for order: {OrderId}", userId, orderId.Value);
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        _logger.LogWarning("Order access denied: Customer {UserId} attempted to access order {OrderId} they don't own",
                            userId, orderId.Value);
                    }
                }
                else
                {
                    // No specific order ID - allow general order operations for customers
                    _logger.LogDebug("General order access granted to customer: {UserId}", userId);
                    context.Succeed(requirement);
                    return;
                }
            }

            // Default deny
            _logger.LogWarning("Order access denied for user: {UserId} with role: {UserRole}", userId, userRole);
            context.Fail();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during order ownership authorization check");
            context.Fail();
        }
    }

    /// <summary>
    /// Extracts order ID from the authorization context (route data, resource, etc.)
    /// </summary>
    private static int? GetOrderIdFromContext(AuthorizationHandlerContext context)
    {
        // Try to get order ID from route data
        if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues.TryGetValue("id", out var routeId) &&
                int.TryParse(routeId?.ToString(), out var orderIdFromRoute))
            {
                return orderIdFromRoute;
            }

            // Try to get from query parameters
            if (httpContext.Request.Query.TryGetValue("orderId", out var queryId) &&
                int.TryParse(queryId.FirstOrDefault(), out var orderIdFromQuery))
            {
                return orderIdFromQuery;
            }
        }

        // Try to get from resource object
        if (context.Resource is Order order)
        {
            return order.Id;
        }

        if (context.Resource is int orderId)
        {
            return orderId;
        }

        return null;
    }

    /// <summary>
    /// Checks if the specified user owns the specified order
    /// </summary>
    private async Task<bool> IsOrderOwner(int userId, int orderId)
    {
        try
        {
            var orderExists = await _context.Orders
                .AnyAsync(o => o.Id == orderId && o.CustomerId == userId);

            return orderExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking order ownership for user {UserId} and order {OrderId}", userId, orderId);
            return false;
        }
    }
}