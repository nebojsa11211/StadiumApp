using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StladiumDrinkOrdering.API.Data;
using StladiumDrinkOrdering.API.Services;
using StladiumDrinkOrdering.Shared.Models;
using StladiumDrinkOrdering.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace StladiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/customer/orders")]
public class CustomerOrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IShoppingCartService _cartService;
    private readonly IPaymentService _paymentService;
    private readonly IQRCodeService _qrCodeService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CustomerOrdersController> _logger;

    public CustomerOrdersController(
        ApplicationDbContext context,
        IShoppingCartService cartService,
        IPaymentService paymentService,
        IQRCodeService qrCodeService,
        INotificationService notificationService,
        ILogger<CustomerOrdersController> logger)
    {
        _context = context;
        _cartService = cartService;
        _paymentService = paymentService;
        _qrCodeService = qrCodeService;
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new ticket order from shopping cart
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<TicketOrderResultDto>> CreateTicketOrder([FromBody] CreateTicketOrderRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            _logger.LogInformation("Creating ticket order for session {SessionId}", request.SessionId);

            // Get shopping cart
            var cart = await _cartService.GetCartAsync(request.SessionId);
            if (cart == null || !cart.Items.Any())
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Shopping cart is empty or not found" 
                });
            }

            // Verify cart items match request items
            if (cart.Items.Count != request.Items.Count)
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Cart items don't match the order request" 
                });
            }

            // Validate all seats are still available
            foreach (var item in cart.Items)
            {
                var isAvailable = await _cartService.IsSeatAvailableAsync(
                    item.EventId, item.SectorId, item.RowNumber, item.SeatNumber);
                
                if (!isAvailable)
                {
                    return BadRequest(new TicketOrderResultDto 
                    { 
                        Success = false, 
                        ErrorMessage = $"Seat {item.SeatCode} is no longer available" 
                    });
                }
            }

            // Create customer order
            var order = new Order
            {
                CustomerName = $"{request.CustomerInfo.FirstName} {request.CustomerInfo.LastName}",
                CustomerEmail = request.CustomerInfo.Email,
                CustomerPhone = request.CustomerInfo.Phone,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = request.TotalAmount,
                OrderItems = new List<OrderItem>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderId = order.Id.ToString();
            var tickets = new List<Ticket>();

            // Create tickets for each seat
            foreach (var cartItem in cart.Items)
            {
                // Generate unique ticket number
                var ticketNumber = $"TKT{DateTime.Now:yyyyMMdd}{cartItem.SectorId:D2}{cartItem.RowNumber:D2}{cartItem.SeatNumber:D2}";
                
                // Generate QR code
                var qrData = $"{orderId}|{ticketNumber}|{cartItem.EventId}|{cartItem.SectorId}|{cartItem.RowNumber}|{cartItem.SeatNumber}";
                var qrCode = await _qrCodeService.GenerateQRCodeAsync(qrData);

                var ticket = new Ticket
                {
                    TicketNumber = ticketNumber,
                    EventId = cartItem.EventId,
                    SeatNumber = $"{cartItem.RowNumber}-{cartItem.SeatNumber}",
                    SectionName = cartItem.SectionName ?? "Unknown Section",
                    Price = cartItem.Price,
                    PurchaseDate = DateTime.UtcNow,
                    CustomerEmail = request.CustomerInfo.Email,
                    CustomerName = $"{request.CustomerInfo.FirstName} {request.CustomerInfo.LastName}",
                    QRCode = qrCode,
                    Status = "Active",
                    OrderId = order.Id
                };

                tickets.Add(ticket);
                _context.Tickets.Add(ticket);

                // Create order item
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ItemName = $"Ticket {cartItem.SeatCode}",
                    Quantity = 1,
                    Price = cartItem.Price,
                    TicketNumber = ticketNumber
                };

                order.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            // Process payment
            var paymentResult = await ProcessPaymentAsync(request.PaymentInfo, request.TotalAmount, orderId);
            
            if (!paymentResult.Success)
            {
                await transaction.RollbackAsync();
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = paymentResult.ErrorMessage ?? "Payment processing failed" 
                });
            }

            // Create payment record
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = request.TotalAmount,
                PaymentMethod = "Credit Card",
                PaymentDate = DateTime.UtcNow,
                Status = PaymentStatus.Completed,
                TransactionId = paymentResult.TransactionId,
                CardLast4 = request.PaymentInfo.CardNumber.Substring(Math.Max(0, request.PaymentInfo.CardNumber.Length - 4))
            };

            _context.Payments.Add(payment);

            // Update order status
            order.Status = OrderStatus.Confirmed;
            order.PaymentStatus = PaymentStatus.Completed;

            await _context.SaveChangesAsync();

            // Clear shopping cart
            await _cartService.ClearCartAsync(request.SessionId);

            await transaction.CommitAsync();

            // Send confirmation email (async)
            _ = Task.Run(async () =>
            {
                try
                {
                    await _notificationService.SendOrderConfirmationEmailAsync(
                        request.CustomerInfo.Email,
                        order,
                        tickets);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email for order {OrderId}", orderId);
                }
            });

            _logger.LogInformation("Successfully created order {OrderId} with {TicketCount} tickets", 
                orderId, tickets.Count);

            return Ok(new TicketOrderResultDto 
            { 
                Success = true, 
                OrderId = orderId,
                PaymentIntentId = paymentResult.TransactionId
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating ticket order for session {SessionId}", request.SessionId);
            
            return StatusCode(500, new TicketOrderResultDto 
            { 
                Success = false, 
                ErrorMessage = "An error occurred while processing your order. Please try again." 
            });
        }
    }

    /// <summary>
    /// Get order confirmation details
    /// </summary>
    [HttpGet("{orderId}/confirmation")]
    public async Task<ActionResult<OrderConfirmationDto>> GetOrderConfirmation(string orderId)
    {
        try
        {
            if (!int.TryParse(orderId, out int orderIdInt))
            {
                return BadRequest("Invalid order ID format");
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderIdInt);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var tickets = await _context.Tickets
                .Where(t => t.OrderId == orderIdInt)
                .ToListAsync();

            var evt = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == tickets.FirstOrDefault()!.EventId);

            var confirmation = new OrderConfirmationDto
            {
                OrderId = orderId,
                CustomerName = order.CustomerName ?? "Unknown Customer",
                EventName = evt?.EventName ?? "Unknown Event",
                EventDate = evt?.EventDate ?? DateTime.MinValue,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PaymentStatus = order.PaymentStatus.ToString(),
                Tickets = tickets.Select(t => new TicketDto
                {
                    TicketNumber = t.TicketNumber,
                    SeatCode = t.SeatNumber,
                    SectionName = t.SectionName,
                    Price = t.Price,
                    QRCode = t.QRCode ?? "QR_CODE_PLACEHOLDER"
                }).ToList()
            };

            return Ok(confirmation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order confirmation for order {OrderId}", orderId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get customer's orders history
    /// </summary>
    [HttpGet("my-orders")]
    public async Task<ActionResult<List<CustomerOrderDto>>> GetMyOrders([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required");
            }

            var orders = await _context.Orders
                .Where(o => o.CustomerEmail == email)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .Take(50)
                .ToListAsync();

            var customerOrders = new List<CustomerOrderDto>();

            foreach (var order in orders)
            {
                var tickets = await _context.Tickets
                    .Where(t => t.OrderId == order.Id)
                    .ToListAsync();

                var eventIds = tickets.Select(t => t.EventId).Distinct().ToList();
                var events = await _context.Events
                    .Where(e => eventIds.Contains(e.Id))
                    .ToListAsync();

                customerOrders.Add(new CustomerOrderDto
                {
                    OrderId = order.Id.ToString(),
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    PaymentStatus = order.PaymentStatus.ToString(),
                    TicketCount = tickets.Count,
                    EventNames = events.Select(e => e.EventName).ToList()
                });
            }

            return Ok(customerOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer orders for email {Email}", email);
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task<PaymentProcessingResult> ProcessPaymentAsync(PaymentInfoDto paymentInfo, decimal amount, string orderId)
    {
        try
        {
            // In a real implementation, this would integrate with Stripe or another payment processor
            // For now, simulate payment processing
            
            if (string.IsNullOrEmpty(paymentInfo.CardNumber) || paymentInfo.CardNumber.Length < 12)
            {
                return new PaymentProcessingResult 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid card number" 
                };
            }

            if (string.IsNullOrEmpty(paymentInfo.CVV) || paymentInfo.CVV.Length < 3)
            {
                return new PaymentProcessingResult 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid CVV" 
                };
            }

            // Simulate processing delay
            await Task.Delay(1000);

            // Generate mock transaction ID
            var transactionId = $"txn_{DateTime.UtcNow.Ticks}_{orderId}";

            _logger.LogInformation("Payment processed successfully for order {OrderId}, amount ${Amount}", 
                orderId, amount);

            return new PaymentProcessingResult 
            { 
                Success = true, 
                TransactionId = transactionId 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment for order {OrderId}", orderId);
            return new PaymentProcessingResult 
            { 
                Success = false, 
                ErrorMessage = "Payment processing error" 
            };
        }
    }
}

// Supporting DTOs
public class CreateTicketOrderRequest
{
    public CustomerInfoDto CustomerInfo { get; set; } = new();
    public PaymentInfoDto PaymentInfo { get; set; } = new();
    public string SessionId { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}

public class CustomerInfoDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class PaymentInfoDto
{
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public string CardholderName { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
}

public class TicketOrderResultDto
{
    public bool Success { get; set; }
    public string? OrderId { get; set; }
    public string? ErrorMessage { get; set; }
    public string? PaymentIntentId { get; set; }
}

public class OrderConfirmationDto
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}

public class TicketDto
{
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string QRCode { get; set; } = string.Empty;
}

public class CustomerOrderDto
{
    public string OrderId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public int TicketCount { get; set; }
    public List<string> EventNames { get; set; } = new();
}

public class PaymentProcessingResult
{
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
}