using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IOrderService
{
    Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto, int customerId);
    Task<OrderDto?> GetOrderByIdAsync(int orderId);
    Task<List<OrderDto>> GetOrdersAsync(OrderStatus? status = null);
    Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId);
    Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto, int userId);
    Task<bool> CancelOrderAsync(int orderId, int userId);
}

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto, int customerId)
    {
        // Validate ticket
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.TicketNumber == createOrderDto.TicketNumber && t.IsActive);

        if (ticket == null)
        {
            return null;
        }

        // Validate drinks and calculate total
        var drinkIds = createOrderDto.OrderItems.Select(oi => oi.DrinkId).ToList();
        var drinks = await _context.Drinks
            .Where(d => drinkIds.Contains(d.Id) && d.IsAvailable)
            .ToListAsync();

        if (drinks.Count != drinkIds.Count)
        {
            return null; // Some drinks not found or unavailable
        }

        // Check stock
        foreach (var orderItem in createOrderDto.OrderItems)
        {
            var drink = drinks.First(d => d.Id == orderItem.DrinkId);
            if (drink.StockQuantity < orderItem.Quantity)
            {
                return null; // Insufficient stock
            }
        }

        // Create order
        var order = new Order
        {
            TicketNumber = createOrderDto.TicketNumber,
            SeatNumber = ticket.SeatNumber,
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            CustomerNotes = createOrderDto.CustomerNotes,
            CreatedAt = DateTime.UtcNow
        };

        // Create order items
        decimal totalAmount = 0;
        foreach (var orderItemDto in createOrderDto.OrderItems)
        {
            var drink = drinks.First(d => d.Id == orderItemDto.DrinkId);
            var orderItem = new OrderItem
            {
                DrinkId = orderItemDto.DrinkId,
                Quantity = orderItemDto.Quantity,
                UnitPrice = drink.Price,
                TotalPrice = drink.Price * orderItemDto.Quantity,
                SpecialInstructions = orderItemDto.SpecialInstructions
            };

            order.OrderItems.Add(orderItem);
            totalAmount += orderItem.TotalPrice;

            // Update stock
            drink.StockQuantity -= orderItemDto.Quantity;
        }

        order.TotalAmount = totalAmount;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return await GetOrderByIdAsync(order.Id);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.PreparedByUser)
            .Include(o => o.DeliveredByUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order != null ? MapToOrderDto(order) : null;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(OrderStatus? status = null)
    {
        var query = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.PreparedByUser)
            .Include(o => o.DeliveredByUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Include(o => o.Payment)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        return orders.Select(MapToOrderDto).ToList();
    }

    public async Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.PreparedByUser)
            .Include(o => o.DeliveredByUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Include(o => o.Payment)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapToOrderDto).ToList();
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto, int userId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return false;
        }

        order.Status = updateDto.Status;
        order.Notes = updateDto.Notes;

        switch (updateDto.Status)
        {
            case OrderStatus.Accepted:
                order.AcceptedByUserId = userId;
                order.AcceptedAt = DateTime.UtcNow;
                break;
            case OrderStatus.Ready:
                order.PreparedByUserId = userId;
                order.PreparedAt = DateTime.UtcNow;
                break;
            case OrderStatus.Delivered:
                order.DeliveredByUserId = userId;
                order.DeliveredAt = DateTime.UtcNow;
                break;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelOrderAsync(int orderId, int userId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null || order.Status != OrderStatus.Pending)
        {
            return false;
        }

        // Restore stock
        foreach (var orderItem in order.OrderItems)
        {
            orderItem.Drink.StockQuantity += orderItem.Quantity;
        }

        order.Status = OrderStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            TicketNumber = order.TicketNumber,
            SeatNumber = order.SeatNumber,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer.Username,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            AcceptedAt = order.AcceptedAt,
            PreparedAt = order.PreparedAt,
            DeliveredAt = order.DeliveredAt,
            AcceptedByUserName = order.AcceptedByUser?.Username,
            PreparedByUserName = order.PreparedByUser?.Username,
            DeliveredByUserName = order.DeliveredByUser?.Username,
            Notes = order.Notes,
            CustomerNotes = order.CustomerNotes,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.Drink.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice,
                SpecialInstructions = oi.SpecialInstructions
            }).ToList(),
            Payment = order.Payment != null ? new PaymentDto
            {
                Id = order.Payment.Id,
                OrderId = order.Payment.OrderId,
                Amount = order.Payment.Amount,
                Method = order.Payment.Method,
                Status = order.Payment.Status,
                TransactionId = order.Payment.TransactionId,
                CreatedAt = order.Payment.CreatedAt,
                ProcessedAt = order.Payment.ProcessedAt,
                FailureReason = order.Payment.FailureReason
            } : null
        };
    }
}


