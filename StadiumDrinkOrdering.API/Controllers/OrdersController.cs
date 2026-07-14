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
            var result = await _orderService.CreateOrderAsync(createOrderDto, userId);

            if (result.Outcome != CreateOrderOutcome.Success)
            {
                await _loggingService.LogWarningAsync("Order creation failed",
                    BusinessEventActions.OrderCreated, BusinessEventCategories.OrderProcessing,
                    userId.ToString(), userEmail, userRole,
                    details: $"Order creation rejected ({result.Outcome}): {result.Error}",
                    requestPath: Request.Path, httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(), userAgent: Request.Headers.UserAgent.ToString());

                // Wallet-payment rejections get 402 Payment Required so the client can distinguish them
                // from ordinary validation problems (400) and offer a top-up.
                return result.Outcome switch
                {
                    CreateOrderOutcome.InsufficientFunds or CreateOrderOutcome.NoWallet or CreateOrderOutcome.WalletFrozen
                        => StatusCode(StatusCodes.Status402PaymentRequired, result.Error),
                    _ => BadRequest(result.Error ?? "Unable to create order. Please check ticket number and drink availability.")
                };
            }

            var order = result.Order!;

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

    // Accepts a single status ("Pending"), a numeric value ("1"), or a comma-separated list of
    // statuses in any casing/hyphenation (e.g. "pending,accepted,in-preparation"), so callers can
    // fetch the "active" set in one request. Bound as a raw string (rather than OrderStatus?) so a
    // multi-value or hyphenated query doesn't fail [ApiController] model binding with a 400.
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] string? status = null)
    {
        var statuses = ParseStatuses(status);

        // Fast path: a single status uses the existing filtered query.
        if (statuses.Count == 1)
            return Ok(await _orderService.GetOrdersAsync(statuses[0]));

        var orders = await _orderService.GetOrdersAsync();
        if (statuses.Count > 1)
            orders = orders.Where(o => statuses.Contains(o.Status)).ToList();

        return Ok(orders);
    }

    /// <summary>
    /// Parses a status query value into distinct <see cref="OrderStatus"/> values. Tolerates
    /// comma-separated lists, numeric codes, and casing/hyphen/underscore/space variations
    /// (e.g. "in-preparation" → <see cref="OrderStatus.InPreparation"/>). Unknown tokens are ignored.
    /// </summary>
    private static List<OrderStatus> ParseStatuses(string? raw)
    {
        var result = new List<OrderStatus>();
        if (string.IsNullOrWhiteSpace(raw))
            return result;

        foreach (var token in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var normalized = token.Replace("-", "").Replace("_", "").Replace(" ", "");

            if (int.TryParse(normalized, out var num) && Enum.IsDefined(typeof(OrderStatus), num))
            {
                result.Add((OrderStatus)num);
                continue;
            }

            foreach (OrderStatus s in Enum.GetValues(typeof(OrderStatus)))
            {
                if (string.Equals(s.ToString(), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(s);
                    break;
                }
            }
        }

        return result.Distinct().ToList();
    }

    [HttpGet("my-orders")]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _orderService.GetOrdersByCustomerAsync(userId);
        return Ok(orders);
    }

    /// <summary>
    /// Returns the orders assigned to the currently authenticated staff member (derived from the
    /// JWT, never a client-supplied id). Used by the Runner app so a waiter only ever sees their
    /// own delivery queue. Replaces the never-implemented /orders/staff/{id} endpoint the old Staff
    /// app called with a hardcoded id.
    /// </summary>
    [HttpGet("mine")]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetMyAssignedOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _orderService.GetAssignedOrdersAsync(userId);
        return Ok(orders);
    }

    /// <summary>
    /// The shared delivery pool: prepared (Ready) orders not yet claimed by any runner. The Runner
    /// app shows this list; a waiter claims one via POST /orders/{id}/claim.
    /// </summary>
    [HttpGet("available-for-delivery")]
    [Authorize(Policy = AuthorizationPolicies.CanReadOrders)]
    public async Task<ActionResult<List<OrderDto>>> GetAvailableForDelivery()
    {
        var orders = await _orderService.GetAvailableForDeliveryAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Claims a Ready order from the shared pool for the current runner: assigns it to them and
    /// moves it to OutForDelivery. First claimer wins (409 for anyone else); re-claiming one you
    /// already hold is an idempotent success (supports the Runner's offline-outbox retries).
    /// </summary>
    [HttpPost("{id}/claim")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<IActionResult> ClaimOrder(int id)
    {
        var userId = GetCurrentUserId();
        var (outcome, order) = await _orderService.ClaimOrderForDeliveryAsync(id, userId);

        switch (outcome)
        {
            case ClaimOutcome.NotFound:
                return NotFound();

            case ClaimOutcome.AlreadyClaimed:
                return Conflict(new { message = "This order was already claimed by another runner." });

            default:
                await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
                {
                    Action = BusinessEventActions.OrderUpdated,
                    Category = BusinessEventCategories.OrderProcessing,
                    UserId = userId.ToString(),
                    UserEmail = User.FindFirst(ClaimTypes.Email)?.Value,
                    UserRole = GetCurrentUserRole(),
                    Details = $"Order #{id} claimed for delivery",
                    RequestPath = Request.Path,
                    HttpMethod = Request.Method,
                    Source = "API",
                    BusinessEntityType = "Order",
                    BusinessEntityId = id.ToString(),
                    BusinessEntityName = $"Order #{id}",
                    StatusAfter = OrderStatus.OutForDelivery.ToString()
                });
                return Ok(order);
        }
    }

    /// <summary>
    /// Claims several Ready orders from the shared pool for the current runner in one action — used
    /// by the Runner app when a waiter grabs multiple prepared drinks for a single trip. Returns the
    /// per-order breakdown (claimed / taken / not-found); a partially-stale selection still claims
    /// whatever is available rather than failing wholesale.
    /// </summary>
    [HttpPost("claim-batch")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<ActionResult<BatchClaimResultDto>> ClaimOrders([FromBody] BatchClaimRequestDto request)
    {
        if (request?.OrderIds == null || request.OrderIds.Count == 0)
            return BadRequest("No order ids supplied.");

        var userId = GetCurrentUserId();
        var result = await _orderService.ClaimOrdersForDeliveryAsync(request.OrderIds, userId);

        if (result.Claimed.Count > 0)
        {
            await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
            {
                Action = BusinessEventActions.OrderUpdated,
                Category = BusinessEventCategories.OrderProcessing,
                UserId = userId.ToString(),
                UserEmail = User.FindFirst(ClaimTypes.Email)?.Value,
                UserRole = GetCurrentUserRole(),
                Details = $"Batch-claimed {result.Claimed.Count} order(s) for delivery: {string.Join(", ", result.Claimed.Select(id => $"#{id}"))}",
                RequestPath = Request.Path,
                HttpMethod = Request.Method,
                Source = "API",
                BusinessEntityType = "Order",
                StatusAfter = OrderStatus.OutForDelivery.ToString()
            });
        }

        return Ok(result);
    }

    /// <summary>
    /// A runner reporting they couldn't hand order {id} over at the seat (fan absent, refused, wrong
    /// seat…). Moves it OutForDelivery → DeliveryFailed, releasing the runner's claim so it leaves
    /// their queue and surfaces on the Bar returns triage. Idempotent for the Runner's offline outbox:
    /// a replay for an order already DeliveryFailed is a success.
    /// </summary>
    [HttpPost("{id}/delivery-failed")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<IActionResult> ReportDeliveryFailed(int id, [FromBody] ReportDeliveryFailedDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        var outcome = await _orderService.ReportDeliveryFailedAsync(id, userId, dto);

        switch (outcome)
        {
            case DeliveryFailedOutcome.NotFound:
                return NotFound();

            case DeliveryFailedOutcome.NotDeliverable:
                return Conflict(new { message = "Order is not out for delivery by you and cannot be marked failed." });

            default: // Recorded or AlreadyFailed (idempotent) — both success
                await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
                {
                    Action = BusinessEventActions.OrderUpdated,
                    Category = BusinessEventCategories.OrderProcessing,
                    UserId = userId.ToString(),
                    UserEmail = User.FindFirst(ClaimTypes.Email)?.Value,
                    UserRole = GetCurrentUserRole(),
                    Details = $"Order #{id} delivery failed ({dto.Reason})",
                    RequestPath = Request.Path,
                    HttpMethod = Request.Method,
                    Source = "API",
                    BusinessEntityType = "Order",
                    BusinessEntityId = id.ToString(),
                    BusinessEntityName = $"Order #{id}",
                    StatusAfter = OrderStatus.DeliveryFailed.ToString()
                });
                return NoContent();
        }
    }

    /// <summary>
    /// Bar triage — requeue a returned (DeliveryFailed) order for another delivery attempt: back to
    /// Ready + unassigned so it re-enters the shared pool.
    /// </summary>
    [HttpPost("{id}/retry-delivery")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<IActionResult> RetryFailedDelivery(int id)
    {
        var userId = GetCurrentUserId();
        var ok = await _orderService.RetryFailedDeliveryAsync(id, userId);
        if (!ok)
            return Conflict(new { message = "Order is not in a failed-delivery state." });

        await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
        {
            Action = BusinessEventActions.OrderReady,
            Category = BusinessEventCategories.OrderProcessing,
            UserId = userId.ToString(),
            UserEmail = User.FindFirst(ClaimTypes.Email)?.Value,
            UserRole = GetCurrentUserRole(),
            Details = $"Order #{id} requeued for delivery after a failed attempt",
            Source = "API",
            BusinessEntityType = "Order",
            BusinessEntityId = id.ToString(),
            BusinessEntityName = $"Order #{id}",
            StatusAfter = OrderStatus.Ready.ToString()
        });
        return NoContent();
    }

    /// <summary>
    /// Bar triage — give up on a returned (DeliveryFailed) order: mark it Cancelled and refund any
    /// wallet-funded payment. Guarded by CanUpdateOrders (Bartender+) so it doesn't need the broader
    /// delete policy the generic /cancel endpoint requires; the drink was poured, so stock is not restored.
    /// </summary>
    [HttpPost("{id}/cancel-failed")]
    [Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
    public async Task<IActionResult> CancelFailedDelivery(int id)
    {
        var userId = GetCurrentUserId();
        var ok = await _orderService.CancelFailedDeliveryAsync(id, userId);
        if (!ok)
            return Conflict(new { message = "Order is not in a failed-delivery state." });

        await _loggingService.LogBusinessEventAsync(new LogUserActionRequest
        {
            Action = BusinessEventActions.OrderCancelled,
            Category = BusinessEventCategories.OrderProcessing,
            UserId = userId.ToString(),
            UserEmail = User.FindFirst(ClaimTypes.Email)?.Value,
            UserRole = GetCurrentUserRole(),
            Details = $"Order #{id} cancelled and refunded after a failed delivery",
            Source = "API",
            BusinessEntityType = "Order",
            BusinessEntityId = id.ToString(),
            BusinessEntityName = $"Order #{id}",
            StatusAfter = OrderStatus.Cancelled.ToString()
        });
        return NoContent();
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
                    UpdateTimestamp = DateTime.UtcNow,
                    ClientActionId = updateDto.ClientActionId
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


