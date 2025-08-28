using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var order = await _orderService.CreateOrderAsync(createOrderDto, userId);

        if (order == null)
        {
            return BadRequest("Unable to create order. Please check ticket number and drink availability.");
        }

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        // Customers can only see their own orders
        if (userRole == "Customer" && order.CustomerId != userId)
        {
            return Forbid();
        }

        return Ok(order);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Bartender,Waiter")]
    public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] OrderStatus? status = null)
    {
        var orders = await _orderService.GetOrdersAsync(status);
        return Ok(orders);
    }

    [HttpGet("my-orders")]
    public async Task<ActionResult<List<OrderDto>>> GetMyOrders()
    {
        var userId = GetCurrentUserId();
        var orders = await _orderService.GetOrdersByCustomerAsync(userId);
        return Ok(orders);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Bartender,Waiter")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetCurrentUserId();
        var success = await _orderService.UpdateOrderStatusAsync(id, updateDto, userId);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        // Check if user has permission to cancel
        if (userRole == "Customer")
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null || order.CustomerId != userId)
            {
                return Forbid();
            }
        }

        var success = await _orderService.CancelOrderAsync(id, userId);
        if (!success)
        {
            return BadRequest("Unable to cancel order. Order may not exist or cannot be cancelled.");
        }

        return NoContent();
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(userIdClaim?.Value ?? "0");
    }

    private string GetCurrentUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? "";
    }
}


