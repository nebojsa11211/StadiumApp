using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Authorization.Handlers;

/// <summary>
/// Authorization handler for ticket ownership verification.
/// Ensures customers can only access their own tickets while allowing staff to validate any ticket.
/// </summary>
public class TicketOwnershipHandler : AuthorizationHandler<TicketOwnershipRequirement>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TicketOwnershipHandler> _logger;

    public TicketOwnershipHandler(ApplicationDbContext context, ILogger<TicketOwnershipHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TicketOwnershipRequirement requirement)
    {
        try
        {
            // Get user information from claims
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
            {
                _logger.LogWarning("Ticket ownership check failed: Missing user ID or role claims");
                context.Fail();
                return;
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("Ticket ownership check failed: Invalid user ID format: {UserId}", userIdClaim.Value);
                context.Fail();
                return;
            }

            var userRole = userRoleClaim.Value;

            // Admin access check
            if (requirement.AllowAdminAccess && userRole == UserRole.Admin.ToString())
            {
                _logger.LogDebug("Ticket access granted to admin user: {UserId}", userId);
                context.Succeed(requirement);
                return;
            }

            // Staff access check (for ticket validation)
            if (requirement.AllowStaffAccess &&
                (userRole == UserRole.Bartender.ToString() || userRole == UserRole.Waiter.ToString()))
            {
                _logger.LogDebug("Ticket access granted to staff user: {UserId} with role: {UserRole}", userId, userRole);
                context.Succeed(requirement);
                return;
            }

            // For customers, check ticket ownership
            if (userRole == UserRole.Customer.ToString())
            {
                var ticketId = GetTicketIdFromContext(context);
                var ticketNumber = GetTicketNumberFromContext(context);

                if (ticketId.HasValue)
                {
                    var isOwner = await IsTicketOwner(userId, ticketId.Value);
                    if (isOwner)
                    {
                        _logger.LogDebug("Ticket access granted to customer: {UserId} for ticket: {TicketId}", userId, ticketId.Value);
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        _logger.LogWarning("Ticket access denied: Customer {UserId} attempted to access ticket {TicketId} they don't own",
                            userId, ticketId.Value);
                    }
                }
                else if (!string.IsNullOrEmpty(ticketNumber))
                {
                    var isOwner = await IsTicketOwnerByNumber(userId, ticketNumber);
                    if (isOwner)
                    {
                        _logger.LogDebug("Ticket access granted to customer: {UserId} for ticket number: {TicketNumber}", userId, ticketNumber);
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        _logger.LogWarning("Ticket access denied: Customer {UserId} attempted to access ticket {TicketNumber} they don't own",
                            userId, ticketNumber);
                    }
                }
                else
                {
                    // No specific ticket ID or number - allow general ticket operations for customers
                    _logger.LogDebug("General ticket access granted to customer: {UserId}", userId);
                    context.Succeed(requirement);
                    return;
                }
            }

            // Default deny
            _logger.LogWarning("Ticket access denied for user: {UserId} with role: {UserRole}", userId, userRole);
            context.Fail();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during ticket ownership authorization check");
            context.Fail();
        }
    }

    /// <summary>
    /// Extracts ticket ID from the authorization context (route data, resource, etc.)
    /// </summary>
    private static int? GetTicketIdFromContext(AuthorizationHandlerContext context)
    {
        // Try to get ticket ID from route data
        if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues.TryGetValue("id", out var routeId) &&
                int.TryParse(routeId?.ToString(), out var ticketIdFromRoute))
            {
                return ticketIdFromRoute;
            }

            if (httpContext.Request.RouteValues.TryGetValue("ticketId", out var routeTicketId) &&
                int.TryParse(routeTicketId?.ToString(), out var ticketIdFromRouteParam))
            {
                return ticketIdFromRouteParam;
            }

            // Try to get from query parameters
            if (httpContext.Request.Query.TryGetValue("ticketId", out var queryId) &&
                int.TryParse(queryId.FirstOrDefault(), out var ticketIdFromQuery))
            {
                return ticketIdFromQuery;
            }
        }

        // Try to get from resource object
        if (context.Resource is Ticket ticket)
        {
            return ticket.Id;
        }

        if (context.Resource is int ticketId)
        {
            return ticketId;
        }

        return null;
    }

    /// <summary>
    /// Extracts ticket number from the authorization context (route data, resource, etc.)
    /// </summary>
    private static string? GetTicketNumberFromContext(AuthorizationHandlerContext context)
    {
        // Try to get ticket number from route data
        if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues.TryGetValue("ticketNumber", out var routeTicketNumber))
            {
                return routeTicketNumber?.ToString();
            }

            // Try to get from query parameters
            if (httpContext.Request.Query.TryGetValue("ticketNumber", out var queryTicketNumber))
            {
                return queryTicketNumber.FirstOrDefault();
            }
        }

        // Try to get from resource object
        if (context.Resource is Ticket ticket)
        {
            return ticket.TicketNumber;
        }

        return null;
    }

    /// <summary>
    /// Checks if the specified user owns the specified ticket by ID
    /// </summary>
    private async Task<bool> IsTicketOwner(int userId, int ticketId)
    {
        try
        {
            var ticketExists = await _context.Tickets
                .AnyAsync(t => t.Id == ticketId && t.Orders.Any(o => o.CustomerId == userId));

            return ticketExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ticket ownership for user {UserId} and ticket {TicketId}", userId, ticketId);
            return false;
        }
    }

    /// <summary>
    /// Checks if the specified user owns the specified ticket by ticket number
    /// </summary>
    private async Task<bool> IsTicketOwnerByNumber(int userId, string ticketNumber)
    {
        try
        {
            var ticketExists = await _context.Tickets
                .AnyAsync(t => t.TicketNumber == ticketNumber && t.Orders.Any(o => o.CustomerId == userId));

            return ticketExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ticket ownership for user {UserId} and ticket number {TicketNumber}", userId, ticketNumber);
            return false;
        }
    }
}