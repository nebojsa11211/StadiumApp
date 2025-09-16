using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Authorization.Handlers;

/// <summary>
/// Authorization handler for user profile ownership verification.
/// Ensures users can only access their own profile data while allowing admins to manage all users.
/// </summary>
public class UserOwnershipHandler : AuthorizationHandler<UserOwnershipRequirement>
{
    private readonly ILogger<UserOwnershipHandler> _logger;

    public UserOwnershipHandler(ILogger<UserOwnershipHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserOwnershipRequirement requirement)
    {
        try
        {
            // Get user information from claims
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
            {
                _logger.LogWarning("User ownership check failed: Missing user ID or role claims");
                context.Fail();
                return Task.CompletedTask;
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("User ownership check failed: Invalid user ID format: {UserId}", userIdClaim.Value);
                context.Fail();
                return Task.CompletedTask;
            }

            var userRole = userRoleClaim.Value;

            // Admin access check
            if (requirement.AllowAdminAccess && userRole == UserRole.Admin.ToString())
            {
                _logger.LogDebug("User profile access granted to admin user: {UserId}", userId);
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Check if user is accessing their own profile
            var targetUserId = GetUserIdFromContext(context);
            if (targetUserId.HasValue)
            {
                if (userId == targetUserId.Value)
                {
                    _logger.LogDebug("User profile access granted to user: {UserId} for their own profile", userId);
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
                else
                {
                    _logger.LogWarning("User profile access denied: User {UserId} attempted to access profile of user {TargetUserId}",
                        userId, targetUserId.Value);
                }
            }
            else
            {
                // No specific user ID - allow general user operations
                _logger.LogDebug("General user profile access granted to user: {UserId}", userId);
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Default deny
            _logger.LogWarning("User profile access denied for user: {UserId} with role: {UserRole}", userId, userRole);
            context.Fail();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user ownership authorization check");
            context.Fail();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Extracts target user ID from the authorization context (route data, resource, etc.)
    /// </summary>
    private static int? GetUserIdFromContext(AuthorizationHandlerContext context)
    {
        // Try to get user ID from route data
        if (context.Resource is Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues.TryGetValue("id", out var routeId) &&
                int.TryParse(routeId?.ToString(), out var userIdFromRoute))
            {
                return userIdFromRoute;
            }

            if (httpContext.Request.RouteValues.TryGetValue("userId", out var routeUserId) &&
                int.TryParse(routeUserId?.ToString(), out var userIdFromRouteParam))
            {
                return userIdFromRouteParam;
            }

            // Try to get from query parameters
            if (httpContext.Request.Query.TryGetValue("userId", out var queryId) &&
                int.TryParse(queryId.FirstOrDefault(), out var userIdFromQuery))
            {
                return userIdFromQuery;
            }
        }

        // Try to get from resource object
        if (context.Resource is User user)
        {
            return user.Id;
        }

        if (context.Resource is int userId)
        {
            return userId;
        }

        return null;
    }
}