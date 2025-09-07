using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/customer/orders")]
public class CustomerOrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerOrdersController> _logger;

    public CustomerOrdersController(ApplicationDbContext context, ILogger<CustomerOrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Process a ticket order with payment simulation
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<TicketOrderResultDto>> ProcessTicketOrder([FromBody] CreateTicketOrderRequest request)
    {
        try
        {
            // Validate request
            if (request?.CustomerInfo == null || request?.PaymentInfo == null || 
                !request.Items?.Any() == true)
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid order request" 
                });
            }

            // Validate payment info (basic simulation validation)
            if (string.IsNullOrEmpty(request.PaymentInfo.CardNumber) || 
                request.PaymentInfo.CardNumber.Replace(" ", "").Length < 12)
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid card number" 
                });
            }

            if (string.IsNullOrEmpty(request.PaymentInfo.CVV) || 
                request.PaymentInfo.CVV.Length < 3)
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid CVV" 
                });
            }

            // Simulate payment processing delay
            await Task.Delay(1000);

            // Get the event
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == request.Items.First().EventId);
            
            if (eventEntity == null)
            {
                return BadRequest(new TicketOrderResultDto 
                { 
                    Success = false, 
                    ErrorMessage = "Event not found" 
                });
            }

            // For ticket orders, we don't create a traditional Order record
            // Instead, we create Payment record first to track the transaction
            var customerName = $"{request.CustomerInfo.FirstName} {request.CustomerInfo.LastName}";
            
            // Create payment record
            var payment = new Payment
            {
                OrderId = null, // For direct ticket sales, no order needed
                Amount = request.TotalAmount,
                PaymentMethod = "Credit Card",
                TransactionId = $"txn_{DateTime.UtcNow.Ticks}_{Guid.NewGuid().ToString("N")[..8]}",
                Status = "Completed",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(); // Save to get the payment ID

            // Generate tickets for each item
            var tickets = new List<Ticket>();
            foreach (var item in request.Items)
            {
                var ticket = new Ticket
                {
                    TicketNumber = $"TK{DateTime.Now.Ticks}{tickets.Count + 1:D4}",
                    EventId = item.EventId,
                    EventName = eventEntity.EventName,
                    EventDate = eventEntity.EventDate,
                    CustomerName = customerName,
                    CustomerEmail = request.CustomerInfo.Email,
                    CustomerPhone = request.CustomerInfo.Phone,
                    Price = item.Price,
                    PurchaseDate = DateTime.UtcNow,
                    Status = "Active",
                    IsUsed = false,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    QRCode = "",
                    SeatNumber = item.SeatNumber.ToString(),
                    Row = item.RowNumber.ToString(),
                    Section = item.SectionName
                };

                tickets.Add(ticket);
            }

            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            // Clear the shopping cart
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                var cart = await _context.ShoppingCarts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.SessionId == request.SessionId);
                
                if (cart != null)
                {
                    _context.CartItems.RemoveRange(cart.Items);
                    _context.ShoppingCarts.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new TicketOrderResultDto
            {
                Success = true,
                OrderId = payment.Id, // Use payment ID as order reference
                TransactionId = payment.TransactionId,
                TotalAmount = request.TotalAmount,
                TicketCount = tickets.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing ticket order");
            return StatusCode(500, new TicketOrderResultDto 
            { 
                Success = false, 
                ErrorMessage = "An error occurred processing your order" 
            });
        }
    }

    /// <summary>
    /// Get order confirmation details
    /// </summary>
    [HttpGet("{orderId}/confirmation")]
    public async Task<ActionResult<OrderConfirmationDto>> GetOrderConfirmation(int orderId)
    {
        try
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == orderId);

            if (payment == null)
            {
                return NotFound();
            }

            // Find tickets by transaction ID or payment date
            var tickets = await _context.Tickets
                .Where(t => t.PurchaseDate.Date == payment.CreatedAt.Date)
                .OrderBy(t => t.PurchaseDate)
                .ToListAsync();

            if (!tickets.Any())
            {
                return NotFound();
            }

            var firstTicket = tickets.First();
            
            var confirmation = new OrderConfirmationDto
            {
                OrderId = orderId,
                OrderDate = payment.CreatedAt,
                CustomerName = firstTicket.CustomerName,
                PaymentStatus = "Completed",
                EventName = firstTicket.EventName,
                EventDate = firstTicket.EventDate ?? DateTime.MinValue,
                TotalAmount = payment.Amount,
                Tickets = tickets.Select(t => new OrderTicketDto
                {
                    TicketNumber = t.TicketNumber,
                    SeatCode = $"{t.Section}-R{t.Row}-S{t.SeatNumber}",
                    SectionName = t.Section,
                    Price = t.Price,
                    QRCodeToken = t.QRCodeToken
                }).ToList()
            };

            return Ok(confirmation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order confirmation for order {OrderId}", orderId);
            return StatusCode(500, "Internal server error");
        }
    }
}