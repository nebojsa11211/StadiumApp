using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILoggingService _loggingService;
    private readonly IStadiumAuthorizationService _authorizationService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        ILoggingService loggingService,
        IStadiumAuthorizationService authorizationService,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _loggingService = loggingService;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanCreateOrders)]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = GetCurrentUserRole();

        try
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto, userId);

            if (order == null)
            {
                await _loggingService.LogWarningAsync("Order creation failed",
                    BusinessEventActions.OrderCreated, BusinessEventCategories.OrderProcessing,
                    userId.ToString(), userEmail, userRole,
                    details: $"Failed to create order - ticket validation or drink availability issue",
                    requestPath: Request.Path, httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());

                return BadRequest("Unable to create order. Please check ticket number and drink availability.");
            }

            // Calculate total amount from order items
            var totalAmount = order.OrderItems?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;

            // Log successful order creation as business event
            await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
            {
                Action = BusinessEventActions.OrderCreated,
                Category = BusinessEventCategories.OrderProcessing,
                UserId = userId.ToString(),
                UserEmail = userEmail,
                UserRole = userRole,
                Details = $"Order created with {order.OrderItems?.Count ?? 0} items, total amount: {totalAmount:C}",
                RequestPath = Request.Path,
                HttpMethod = Request.Method,
                Source = "API",
                BusinessEntityType = "Order",
                BusinessEntityId = order.Id.ToString(),
                BusinessEntityName = $"Order #{order.Id}",
                MonetaryAmount = totalAmount,
                Currency = "USD",
                Quantity = order.OrderItems?.Sum(i => i.Quantity) ?? 0,
                StatusAfter = order.Status.ToString(),
                MetadataJson = System.Text.Json.JsonSerializer.Serialize(new { 
                    ItemCount = order.OrderItems?.Count ?? 0,
                    TicketNumber = order.TicketNumber 
                })
            });

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, BusinessEventActions.OrderCreated, BusinessEventCategories.OrderProcessing,
                userId.ToString(), userEmail, userRole,
                details: "Exception occurred during order creation",
                requestPath: Request.Path, httpMethod: Request.Method,
                ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());
            throw;
        }
    }

    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.CanAccessOwnOrders)]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        // Check if user can access this specific order
        var canAccess = await _authorizationService.CanAccessOrderAsync(User, id);
        if (!canAccess)
        {
            return Forbid("You do not have permission to access this order.");
        }

        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] OrderStatus? status = null)
    {
        var orders = await _orderService.GetOrdersAsync(status);
        return Ok(orders);
    }

    [HttpGet("my-orders")]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _orderService.GetOrdersByCustomerAsync(userId);
        return Ok(orders);
    }

    [HttpPut("{id}/status")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = GetCurrentUserRole();

        try
        {
            // Get current order to capture the previous status
            var currentOrder = await _orderService.GetOrderByIdAsync(id);
            var previousStatus = currentOrder?.Status.ToString() ?? "Unknown";

            var success = await _orderService.UpdateOrderStatusAsync(id, updateDto, userId);

            if (!success)
            {
                await _loggingService.LogWarningAsync("Order status update failed",
                    BusinessEventActions.OrderUpdated, BusinessEventCategories.OrderProcessing,
                    userId.ToString(), userEmail, userRole,
                    details: $"Failed to update order #{id} status to {updateDto.Status}",
                    requestPath: Request.Path, httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());

                return NotFound();
            }

            // Determine the appropriate business event action based on new status
            var businessAction = updateDto.Status.ToString().ToLower() switch
            {
                "in preparation" or "inpreparation" => BusinessEventActions.OrderInPreparation,
                "ready" => BusinessEventActions.OrderReady,
                "out for delivery" or "outfordelivery" => BusinessEventActions.OrderUpdated,
                "delivered" => BusinessEventActions.OrderDelivered,
                "cancelled" or "canceled" => BusinessEventActions.OrderCancelled,
                "paid" => BusinessEventActions.OrderPaid,
                _ => BusinessEventActions.OrderUpdated
            };

            // Log successful status update as business event
            await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
            {
                Action = businessAction,
                Category = BusinessEventCategories.OrderProcessing,
                UserId = userId.ToString(),
                UserEmail = userEmail,
                UserRole = userRole,
                Details = $"Order status updated from {previousStatus} to {updateDto.Status}",
                RequestPath = Request.Path,
                HttpMethod = Request.Method,
                Source = "API",
                BusinessEntityType = "Order",
                BusinessEntityId = id.ToString(),
                BusinessEntityName = $"Order #{id}",
                StatusBefore = previousStatus,
                StatusAfter = updateDto.Status.ToString(),
                MetadataJson = System.Text.Json.JsonSerializer.Serialize(new { 
                    UpdatedBy = userRole,
                    UpdateTimestamp = DateTime.UtcNow
                })
            });

            return NoContent();
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, BusinessEventActions.OrderUpdated, BusinessEventCategories.OrderProcessing,
                userId.ToString(), userEmail, userRole,
                details: $"Exception occurred during order #{id} status update",
                requestPath: Request.Path, httpMethod: Request.Method,
                ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());
            throw;
        }
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = AuthorizationPolicies.CanDeleteOrders)]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var userId = _authorizationService.GetCurrentUserId(User);
        var userEmail = _authorizationService.GetCurrentUserEmail(User);
        var userRole = _authorizationService.GetCurrentUserRole(User)?.ToString();

        try
        {
            // Check if user has permission to cancel this specific order
            var canModify = await _authorizationService.CanModifyOrderAsync(User, id);
            if (!canModify)
            {
                await _loggingService.LogWarningAsync("Unauthorized order cancellation attempt",
                    BusinessEventActions.OrderCancelled, BusinessEventCategories.OrderProcessing,
                    userId.ToString(), userEmail, userRole,
                    details: $"User attempted to cancel order #{id} without permission",
                    requestPath: Request.Path, httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());

                return Forbid("You do not have permission to cancel this order.");
            }

            // Get current order details for logging
            var currentOrder = await _orderService.GetOrderByIdAsync(id);
            var totalAmount = currentOrder?.OrderItems?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;

            var success = await _orderService.CancelOrderAsync(id, userId!.Value);
            if (!success)
            {
                await _loggingService.LogWarningAsync("Order cancellation failed",
                    BusinessEventActions.OrderCancelled, BusinessEventCategories.OrderProcessing,
                    userId.ToString(), userEmail, userRole,
                    details: $"Failed to cancel order #{id} - order may not exist or cannot be cancelled",
                    requestPath: Request.Path, httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());

                return BadRequest("Unable to cancel order. Order may not exist or cannot be cancelled.");
            }

            // Log successful order cancellation as business event
            await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
            {
                Action = BusinessEventActions.OrderCancelled,
                Category = BusinessEventCategories.OrderProcessing,
                UserId = userId.ToString(),
                UserEmail = userEmail,
                UserRole = userRole,
                Details = $"Order #{id} was cancelled by {userRole}",
                RequestPath = Request.Path,
                HttpMethod = Request.Method,
                Source = "API",
                BusinessEntityType = "Order",
                BusinessEntityId = id.ToString(),
                BusinessEntityName = $"Order #{id}",
                MonetaryAmount = totalAmount,
                Currency = "USD",
                StatusBefore = currentOrder?.Status.ToString() ?? "Unknown",
                StatusAfter = "Cancelled",
                MetadataJson = System.Text.Json.JsonSerializer.Serialize(new { 
                    CancelledBy = userRole,
                    CancellationReason = "User requested",
                    CancellationTimestamp = DateTime.UtcNow
                })
            });

            return NoContent();
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, BusinessEventActions.OrderCancelled, BusinessEventCategories.OrderProcessing,
                userId.ToString(), userEmail, userRole,
                details: $"Exception occurred during order #{id} cancellation",
                requestPath: Request.Path, httpMethod: Request.Method,
                ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());
            throw;
        }
    }

    private int GetCurrentUserId()
    {
        return _authorizationService.GetCurrentUserId(User) ?? 0;
    }

    private string GetCurrentUserRole()
    {
        return _authorizationService.GetCurrentUserRole(User)?.ToString() ?? "";
    }

    private string? GetClientIpAddress()
    {
        // Try to get the real IP address
        var xForwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(xForwardedFor))
        {
            return xForwardedFor.Split(',')[0].Trim();
        }

        var xRealIp = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(xRealIp))
        {
            return xRealIp;
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    [HttpGet("statistics")]
    [Authorize(Policy = AuthorizationPolicies.CanViewAnalytics)]
    public async Task<IActionResult> GetOrderStatistics()
    {
        try
        {
            var allOrders = await _orderService.GetOrdersAsync();
            var statistics = new
            {
                Active = allOrders?.Count(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled) ?? 0,
                Pending = allOrders?.Count(o => o.Status == OrderStatus.Pending) ?? 0,
                InPreparation = allOrders?.Count(o => o.Status == OrderStatus.InPreparation) ?? 0
            };
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order statistics");
            return StatusCode(500, new { message = "An error occurred while getting order statistics" });
        }
    }
}


