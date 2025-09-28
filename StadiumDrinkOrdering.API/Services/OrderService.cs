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
        Ticket? ticket = null;
        TicketSession? ticketSession = null;
        
        // Prefer TicketSessionId over TicketNumber for enhanced authentication
        if (createOrderDto.TicketSessionId.HasValue)
        {
            ticketSession = await _context.TicketSessions
                .Include(ts => ts.Ticket)
                .Include(ts => ts.Event)
                .Include(ts => ts.Seat)
                    .ThenInclude(s => s.Section)
                .FirstOrDefaultAsync(ts => ts.Id == createOrderDto.TicketSessionId && 
                                          ts.IsActive && 
                                          ts.ExpiresAt > DateTime.UtcNow);
            
            if (ticketSession == null)
            {
                return null; // Invalid or expired ticket session
            }
            
            ticket = ticketSession.Ticket;
        }
        else if (!string.IsNullOrEmpty(createOrderDto.TicketNumber))
        {
            // Legacy fallback for backward compatibility
            ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TicketNumber == createOrderDto.TicketNumber && t.IsActive);
        }

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

        // Create order with enhanced session information
        var order = new Order
        {
            TicketNumber = createOrderDto.TicketNumber ?? ticket.TicketNumber,
            SeatNumber = ticketSession?.SeatNumber ?? ticket.SeatNumber ?? string.Empty,
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            CustomerNotes = createOrderDto.CustomerNotes,
            CreatedAt = DateTime.UtcNow,
            // New enhanced fields
            TicketSessionId = ticketSession?.Id,
            EventId = ticketSession?.EventId ?? ticket.EventId,
            SeatId = ticketSession?.SeatId ?? ticket.SeatId
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
            .Include(o => o.Event)
            .Include(o => o.Seat)
                .ThenInclude(s => s.Section)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order != null ? MapToOrderDto(order) : null;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(OrderStatus? status = null)
    {
        try
        {
            var query = _context.Orders
    .Include(o => o.Customer)
    .Include(o => o.AcceptedByUser)
    .Include(o => o.PreparedByUser)
    .Include(o => o.DeliveredByUser)
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Drink)
    .Include(o => o.Payment)
    .Include(o => o.Event)
    .Include(o => o.Seat)
        .ThenInclude(s => s.Section)
    .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
            var tmp = orders.Select(MapToOrderDto).ToList();
            return tmp; 
        }
        catch (Exception ex)
         {
            return null;
        }

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
            .Include(o => o.Event)
            .Include(o => o.Seat)
                .ThenInclude(s => s.Section)
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
            EventId = order.EventId,
            SeatId = order.SeatId,
            AcceptedAt = order.AcceptedAt,
            PreparedAt = order.PreparedAt,
            DeliveredAt = order.DeliveredAt,
            AcceptedByUserName = order.AcceptedByUser?.Username,
            PreparedByUserName = order.PreparedByUser?.Username,
            DeliveredByUserName = order.DeliveredByUser?.Username,
            Notes = order.Notes,
            CustomerNotes = order.CustomerNotes,
            Event = order.Event,
            Seat = order.Seat,
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
                OrderId = order.Payment.OrderId ?? 0,
                Amount = order.Payment.Amount,
                Method = Enum.TryParse<PaymentMethod>(order.Payment.PaymentMethod, out var method) ? method : PaymentMethod.CreditCard,
                Status = Enum.TryParse<PaymentStatus>(order.Payment.Status, out var status) ? status : PaymentStatus.Pending,
                TransactionId = order.Payment.TransactionId,
                CreatedAt = order.Payment.CreatedAt,
                ProcessedAt = order.Payment.ProcessedAt,
                FailureReason = order.Payment.FailureReason
            } : null
        };
    }
}


