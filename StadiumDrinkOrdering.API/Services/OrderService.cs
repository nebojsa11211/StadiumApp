using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IOrderService
{
    Task<CreateOrderResult> CreateOrderAsync(CreateOrderDto createOrderDto, int customerId);
    Task<OrderDto?> GetOrderByIdAsync(int orderId);
    Task<List<OrderDto>> GetOrdersAsync(OrderStatus? status = null);
    Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId);
    Task<List<OrderDto>> GetAssignedOrdersAsync(int staffId);
    Task<List<OrderDto>> GetAvailableForDeliveryAsync();
    Task<(ClaimOutcome Outcome, OrderDto? Order)> ClaimOrderForDeliveryAsync(int orderId, int staffId);
    Task<BatchClaimResultDto> ClaimOrdersForDeliveryAsync(IEnumerable<int> orderIds, int staffId);
    Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto, int userId);
    Task<bool> CancelOrderAsync(int orderId, int userId);

    /// <summary>
    /// Cancels every still-in-flight (non-terminal) order for an event. Called when the event reaches a
    /// terminal lifecycle state so no order is left stranded (e.g. as OutForDelivery) after the event is
    /// over. Mirrors <see cref="CancelOrderAsync"/> for each order: restores drink stock, stamps the
    /// cancellation with <paramref name="reason"/>, and refunds any wallet-funded payment (idempotently).
    /// Returns the number of orders cancelled. <paramref name="actorUserId"/> is null for system sweeps.
    /// </summary>
    Task<int> CancelOpenOrdersForEventAsync(int eventId, string reason, int? actorUserId = null);
}

/// <summary>Result of a Runner attempting to claim a Ready order from the shared delivery pool.</summary>
public enum ClaimOutcome
{
    Claimed,
    NotFound,
    AlreadyClaimed
}

/// <summary>Outcome of creating an order, distinguishing ordinary validation failures from
/// wallet-payment rejections so the API can respond specifically (e.g. 402 for insufficient funds).</summary>
public enum CreateOrderOutcome
{
    Success,
    ValidationFailed,
    InsufficientFunds,
    NoWallet,
    WalletFrozen
}

public class CreateOrderResult
{
    public CreateOrderOutcome Outcome { get; init; }
    public OrderDto? Order { get; init; }
    public string? Error { get; init; }

    public static CreateOrderResult Ok(OrderDto order) => new() { Outcome = CreateOrderOutcome.Success, Order = order };
    public static CreateOrderResult Fail(CreateOrderOutcome outcome, string error) => new() { Outcome = outcome, Error = error };
}

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IWalletService _walletService;

    public OrderService(ApplicationDbContext context, IWalletService walletService)
    {
        _context = context;
        _walletService = walletService;
    }

    public async Task<CreateOrderResult> CreateOrderAsync(CreateOrderDto createOrderDto, int customerId)
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
                return CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed, "Invalid or expired ticket session.");
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
            return CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed, "Ticket not found or inactive.");
        }

        // Enforce the live-event rule centrally: a drink order may be placed only while its event is in a
        // drink-ordering phase (Active/InProgress). This backstops every caller — including the legacy
        // OrdersController path — so an order can't be created for a not-yet-live, already-finished, or
        // unlinked event, which is what previously left orders stranded (e.g. OutForDelivery) after close.
        // Ordering must also respect the event's optional drink-ordering window (bars can close before
        // the final whistle), so we need the window fields — not just the status. Prefer the already-loaded
        // session event; otherwise fetch the few fields the gate needs and evaluate them in memory.
        var orderEventId = ticketSession?.EventId ?? ticket.EventId;
        var orderEvent = ticketSession?.Event;
        if (orderEvent is null)
        {
            var row = await _context.Events
                .Where(e => e.Id == orderEventId)
                .Select(e => new { e.Status, e.DrinkSalesStartDate, e.DrinkSalesEndDate })
                .FirstOrDefaultAsync();
            if (row is not null)
                orderEvent = new Event
                {
                    Status = row.Status,
                    DrinkSalesStartDate = row.DrinkSalesStartDate,
                    DrinkSalesEndDate = row.DrinkSalesEndDate
                };
        }
        if (orderEvent is null || !orderEvent.AreDrinkSalesOpenAt(DateTime.UtcNow))
        {
            return CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed,
                orderEvent is not null
                    ? orderEvent.DrinkSalesBlockedReason(DateTime.UtcNow)!
                    : "Drink ordering is unavailable: this ticket is not linked to a live event.");
        }

        // Validate drinks and calculate total
        var drinkIds = createOrderDto.OrderItems.Select(oi => oi.DrinkId).ToList();
        var drinks = await _context.Drinks
            .Where(d => drinkIds.Contains(d.Id) && d.IsAvailable)
            .ToListAsync();

        if (drinks.Count != drinkIds.Count)
        {
            return CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed, "One or more drinks were not found or are unavailable.");
        }

        // Check stock
        foreach (var orderItem in createOrderDto.OrderItems)
        {
            var drink = drinks.First(d => d.Id == orderItem.DrinkId);
            if (drink.StockQuantity < orderItem.Quantity)
            {
                return CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed, $"Insufficient stock for {drink.Name}.");
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

            // Ledger: record the reservation. Linked via the Order navigation so EF stamps the
            // generated OrderId when the order row is inserted in the SaveChanges below; the whole
            // set (order + items + movements + stock decrement) commits atomically.
            _context.StockMovements.Add(new StockMovement
            {
                DrinkId = drink.Id,
                Delta = -orderItemDto.Quantity,
                QuantityAfter = drink.StockQuantity,
                Type = StockMovementType.Sale,
                Order = order,
                CreatedAt = DateTime.UtcNow
            });
        }

        order.TotalAmount = totalAmount;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Wallet payment: charge the fan's wallet for the total. The order already exists (Pending) so
        // the debit can reference it, and an insufficient-funds result moves NO money — we simply undo
        // the just-created order and restore stock, leaving zero side effects. (Legacy callers that omit
        // PaymentMethod skip this entirely and keep the create-unpaid behaviour.)
        if (createOrderDto.PaymentMethod == PaymentMethod.DigitalWallet)
        {
            var debit = await _walletService.TryDebitAsync(
                customerId, totalAmount, idempotencyKey: $"order-{order.Id}",
                referenceType: "Order", referenceId: order.Id, description: $"Drink order #{order.Id}");

            if (debit.Outcome is not (WalletDebitOutcome.Success or WalletDebitOutcome.AlreadyApplied))
            {
                await CompensateUnpaidOrderAsync(order.Id);
                return debit.Outcome switch
                {
                    WalletDebitOutcome.InsufficientFunds => CreateOrderResult.Fail(CreateOrderOutcome.InsufficientFunds, "Insufficient wallet balance for this order."),
                    WalletDebitOutcome.WalletFrozen => CreateOrderResult.Fail(CreateOrderOutcome.WalletFrozen, "Your wallet is frozen."),
                    _ => CreateOrderResult.Fail(CreateOrderOutcome.NoWallet, "No wallet found. Add funds before paying with your wallet.")
                };
            }

            // Record the funding Payment (ledger already holds the authoritative debit).
            _context.Payments.Add(new Payment
            {
                OrderId = order.Id,
                WalletTransactionId = debit.Transaction!.Id,
                PaymentMethod = PaymentMethod.DigitalWallet.ToString(),
                TransactionId = debit.Transaction.Id.ToString(),
                Amount = totalAmount,
                Currency = debit.Transaction.Currency,
                Status = "Completed",
                PaymentDate = DateTime.UtcNow,
                ProcessedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        var dto = await GetOrderByIdAsync(order.Id);
        return dto != null
            ? CreateOrderResult.Ok(dto)
            : CreateOrderResult.Fail(CreateOrderOutcome.ValidationFailed, "Order could not be loaded after creation.");
    }

    /// <summary>
    /// Undoes a just-created, unpaid order when its wallet debit was declined. Because the decline moved
    /// no money, this only restores drink stock and removes the order — no wallet refund is involved.
    /// Runs after WalletService cleared the change tracker, so it reloads the order fresh.
    /// </summary>
    private async Task CompensateUnpaidOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            return;

        foreach (var item in order.OrderItems)
            item.Drink.StockQuantity += item.Quantity;

        // The order is being erased as if it never happened, so remove its Sale ledger rows rather than
        // leaving a sale + compensating-restore pair pointing at a now-deleted order.
        var saleMovements = await _context.StockMovements
            .Where(m => m.OrderId == orderId)
            .ToListAsync();
        _context.StockMovements.RemoveRange(saleMovements);

        _context.OrderItems.RemoveRange(order.OrderItems);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.InPreparationByUser)
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
    .Include(o => o.InPreparationByUser)
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
            .Include(o => o.InPreparationByUser)
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

    public async Task<List<OrderDto>> GetAssignedOrdersAsync(int staffId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.InPreparationByUser)
            .Include(o => o.PreparedByUser)
            .Include(o => o.DeliveredByUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Include(o => o.Payment)
            .Include(o => o.Event)
            .Include(o => o.Seat)
                .ThenInclude(s => s.Section)
            .Where(o => o.AssignedStaffId == staffId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapToOrderDto).ToList();
    }

    public async Task<List<OrderDto>> GetAvailableForDeliveryAsync()
    {
        // Shared delivery pool: orders that are prepared (Ready) and not yet claimed by any runner.
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.AcceptedByUser)
            .Include(o => o.InPreparationByUser)
            .Include(o => o.PreparedByUser)
            .Include(o => o.DeliveredByUser)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Include(o => o.Payment)
            .Include(o => o.Event)
            .Include(o => o.Seat)
                .ThenInclude(s => s.Section)
            .Where(o => o.Status == OrderStatus.Ready && o.AssignedStaffId == null)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(MapToOrderDto).ToList();
    }

    public async Task<(ClaimOutcome Outcome, OrderDto? Order)> ClaimOrderForDeliveryAsync(int orderId, int staffId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            return (ClaimOutcome.NotFound, null);
        }

        // Idempotent re-claim: if this runner already holds the order (their offline outbox retried
        // the claim after a lost response), report success rather than a conflict.
        if (order.AssignedStaffId == staffId && order.Status == OrderStatus.OutForDelivery)
        {
            return (ClaimOutcome.Claimed, await GetOrderByIdAsync(orderId));
        }

        // First runner to claim a Ready, unassigned order wins; anyone else gets AlreadyClaimed.
        if (order.Status != OrderStatus.Ready || order.AssignedStaffId != null)
        {
            return (ClaimOutcome.AlreadyClaimed, null);
        }

        order.AssignedStaffId = staffId;
        order.Status = OrderStatus.OutForDelivery;
        await _context.SaveChangesAsync();

        return (ClaimOutcome.Claimed, await GetOrderByIdAsync(orderId));
    }

    /// <summary>
    /// Claims several Ready orders for one runner in a single trip. Each order is resolved
    /// independently into Claimed / Taken / NotFound so a partially-stale selection still succeeds
    /// for whatever is genuinely available. All assignments commit in one SaveChanges. Uses the same
    /// first-claimer-wins rule as the single claim; an order this runner already holds is reported
    /// as Claimed (idempotent) so offline-outbox retries don't surface as conflicts.
    /// </summary>
    public async Task<BatchClaimResultDto> ClaimOrdersForDeliveryAsync(IEnumerable<int> orderIds, int staffId)
    {
        var result = new BatchClaimResultDto();
        var ids = orderIds.Distinct().ToList();
        if (ids.Count == 0)
            return result;

        var orders = await _context.Orders
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();

        foreach (var id in ids)
        {
            var order = orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                result.NotFound.Add(id);
                continue;
            }

            // Idempotent re-claim of one this runner already holds.
            if (order.AssignedStaffId == staffId && order.Status == OrderStatus.OutForDelivery)
            {
                result.Claimed.Add(id);
                continue;
            }

            if (order.Status != OrderStatus.Ready || order.AssignedStaffId != null)
            {
                result.Taken.Add(id);
                continue;
            }

            order.AssignedStaffId = staffId;
            order.Status = OrderStatus.OutForDelivery;
            result.Claimed.Add(id);
        }

        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto, int userId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return false;
        }

        // Idempotent no-op: a replayed update targeting the status the order is already in
        // (e.g. the Runner's offline outbox retrying "Delivered" after a lost response) succeeds
        // without re-stamping DeliveredByUserId/DeliveredAt or overwriting Notes.
        if (order.Status == updateDto.Status)
        {
            return true;
        }

        order.Status = updateDto.Status;
        order.Notes = updateDto.Notes;

        switch (updateDto.Status)
        {
            case OrderStatus.Accepted:
                order.AcceptedByUserId = userId;
                order.AcceptedAt = DateTime.UtcNow;
                break;
            case OrderStatus.InPreparation:
                order.InPreparationByUserId = userId;
                order.InPreparationAt = DateTime.UtcNow;
                // The Bar board collapses the separate "Accept" step into starting preparation, so
                // orders jump straight from Pending → InPreparation. Backfill the acceptance stamps
                // (once) so acknowledgement analytics and customer-facing "accepted" status stay intact.
                if (order.AcceptedAt == null)
                {
                    order.AcceptedByUserId = userId;
                    order.AcceptedAt = order.InPreparationAt;
                }
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

        // Detect wallet funding before mutating anything.
        var walletPayment = await _context.Payments.AsNoTracking()
            .FirstOrDefaultAsync(p => p.OrderId == orderId
                && p.PaymentMethod == "DigitalWallet"
                && p.Status == "Completed");
        var customerId = order.CustomerId;

        // Restore stock and record the return to inventory in the ledger.
        foreach (var orderItem in order.OrderItems)
        {
            orderItem.Drink.StockQuantity += orderItem.Quantity;
            _context.StockMovements.Add(new StockMovement
            {
                DrinkId = orderItem.DrinkId,
                Delta = orderItem.Quantity,
                QuantityAfter = orderItem.Drink.StockQuantity,
                Type = StockMovementType.OrderCancelled,
                OrderId = order.Id,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        order.Status = OrderStatus.Cancelled;
        await _context.SaveChangesAsync();

        // Refund the wallet AFTER the cancellation is committed. Done last because WalletService clears
        // the change tracker; the refund is idempotent (key refund-order-{id}) so it can't double-credit,
        // and a failure here leaves the order cancelled-but-unrefunded for reconciliation rather than
        // blocking the cancel.
        if (walletPayment != null)
        {
            var wallet = await _context.Wallets.AsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == customerId);
            if (wallet != null)
            {
                await _walletService.RefundAsync(
                    wallet.Id, walletPayment.Amount, idempotencyKey: $"refund-order-{orderId}",
                    referenceType: "Order", referenceId: orderId,
                    description: $"Refund for cancelled order #{orderId}", actorUserId: userId);
            }
        }

        return true;
    }

    public async Task<int> CancelOpenOrdersForEventAsync(int eventId, string reason, int? actorUserId = null)
    {
        var openStatuses = new[]
        {
            OrderStatus.Pending, OrderStatus.Accepted, OrderStatus.InPreparation,
            OrderStatus.Ready, OrderStatus.OutForDelivery
        };

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Where(o => o.EventId == eventId && openStatuses.Contains(o.Status))
            .ToListAsync();

        if (orders.Count == 0)
            return 0;

        var orderIds = orders.Select(o => o.Id).ToList();

        // Detect wallet funding for every affected order before mutating anything, so refunds can be
        // issued after the cancellations commit (WalletService clears the change tracker).
        var walletPayments = await _context.Payments.AsNoTracking()
            .Where(p => p.OrderId != null && orderIds.Contains(p.OrderId.Value)
                && p.PaymentMethod == "DigitalWallet"
                && p.Status == "Completed")
            .ToListAsync();
        var paymentByOrderId = walletPayments
            .GroupBy(p => p.OrderId!.Value)
            .ToDictionary(g => g.Key, g => g.First());
        var customerByOrderId = orders.ToDictionary(o => o.Id, o => o.CustomerId);

        var now = DateTime.UtcNow;
        foreach (var order in orders)
        {
            // Restore reserved stock for the cancelled order and record each return in the ledger.
            foreach (var item in order.OrderItems)
            {
                item.Drink.StockQuantity += item.Quantity;
                _context.StockMovements.Add(new StockMovement
                {
                    DrinkId = item.DrinkId,
                    Delta = item.Quantity,
                    QuantityAfter = item.Drink.StockQuantity,
                    Type = StockMovementType.OrderCancelled,
                    OrderId = order.Id,
                    UserId = actorUserId,
                    Note = reason,
                    CreatedAt = now
                });
            }

            order.Status = OrderStatus.Cancelled;
            order.CancelledAt = now;
            order.Notes = AppendNote(order.Notes, reason);
        }

        await _context.SaveChangesAsync();

        // Refund wallet-funded orders after the cancellations are committed. Each refund is idempotent
        // (key refund-order-{id}) so a retry can't double-credit; a failure here leaves the order
        // cancelled-but-unrefunded for reconciliation rather than blocking the whole sweep.
        foreach (var (orderId, payment) in paymentByOrderId)
        {
            var wallet = await _context.Wallets.AsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == customerByOrderId[orderId]);
            if (wallet != null)
            {
                await _walletService.RefundAsync(
                    wallet.Id, payment.Amount, idempotencyKey: $"refund-order-{orderId}",
                    referenceType: "Order", referenceId: orderId,
                    description: $"Refund for order #{orderId} ({reason})", actorUserId: actorUserId);
            }
        }

        return orders.Count;
    }

    // Appends a system note to an order's existing note, keeping within the Notes column's 500-char limit.
    private static string AppendNote(string? existing, string note)
    {
        var combined = string.IsNullOrWhiteSpace(existing) ? note : $"{existing} | {note}";
        return combined.Length > 500 ? combined[..500] : combined;
    }

    // Full seat location for staff-facing views (Runner/Bar): section + row + seat when the order is
    // linked to a real Seat, otherwise the legacy free-text SeatNumber string.
    private static string BuildSeatPath(Order order)
    {
        if (order.Seat?.Section != null)
        {
            return $"{order.Seat.Section.SectionName} · Row {order.Seat.RowNumber} · Seat {order.Seat.SeatNumber}";
        }
        return order.SeatNumber;
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            TicketNumber = order.TicketNumber,
            SeatNumber = order.SeatNumber,
            SeatPath = BuildSeatPath(order),
            CustomerId = order.CustomerId,
            CustomerName = order.Customer.Username,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            EventId = order.EventId,
            SeatId = order.SeatId,
            AcceptedAt = order.AcceptedAt,
            InPreparationAt = order.InPreparationAt,
            PreparedAt = order.PreparedAt,
            DeliveredAt = order.DeliveredAt,
            AcceptedByUserId = order.AcceptedByUserId,
            InPreparationByUserId = order.InPreparationByUserId,
            PreparedByUserId = order.PreparedByUserId,
            DeliveredByUserId = order.DeliveredByUserId,
            AcceptedByUserName = order.AcceptedByUser?.Username,
            InPreparationByUserName = order.InPreparationByUser?.Username,
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


