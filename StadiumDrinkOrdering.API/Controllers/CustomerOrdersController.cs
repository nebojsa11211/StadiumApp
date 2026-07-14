using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("customer/orders")]
public class CustomerOrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IOverlaySeatService _overlaySeats;
    private readonly IAccountProvisioningService _accountProvisioning;
    private readonly ILogger<CustomerOrdersController> _logger;

    public CustomerOrdersController(
        ApplicationDbContext context,
        IOverlaySeatService overlaySeats,
        IAccountProvisioningService accountProvisioning,
        ILogger<CustomerOrdersController> logger)
    {
        _context = context;
        _overlaySeats = overlaySeats;
        _accountProvisioning = accountProvisioning;
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

            // Identity is mandatory at purchase: a domestic buyer must supply an 11-digit OIB, a foreign
            // buyer a document number. Enforced here (not only via model validation) so the failure comes
            // back in the TicketOrderResultDto shape the checkout UI expects.
            var identityError = TicketIdentity.Validate(
                request.CustomerInfo.IsForeigner, request.CustomerInfo.Oib, request.CustomerInfo.DocumentNumber)
                .FirstOrDefault();
            if (identityError != null)
            {
                return BadRequest(new TicketOrderResultDto
                {
                    Success = false,
                    ErrorMessage = identityError.ErrorMessage ?? "OIB je obavezan."
                });
            }

            // Installation master switch: when direct-to-customer sales are turned off, tickets
            // arrive only via the external integration, so reject new purchases.
            var salesEnabled = await _context.Venues
                .Select(v => (bool?)v.TicketSalesEnabled).FirstOrDefaultAsync() ?? true;
            if (!salesEnabled)
            {
                return BadRequest(new TicketOrderResultDto
                {
                    Success = false,
                    ErrorMessage = "Ticket sales are currently disabled."
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

            // Phase 1 gate: tickets/seats may only be purchased while the event is on sale AND within
            // its configured ticket-sales window.
            var salesBlockedReason = eventEntity.TicketSalesBlockedReason(DateTime.UtcNow);
            if (salesBlockedReason != null)
            {
                return BadRequest(new TicketOrderResultDto
                {
                    Success = false,
                    ErrorMessage = salesBlockedReason
                });
            }

            // Re-validate + resolve every seat against the real overlay stadium BEFORE charging.
            // This rejects seats taken since add-to-cart — including seats held by a season pass —
            // and gets the concrete Seat (Ticket.SeatId) so occupancy stays consistent everywhere.
            var resolvedSeatIds = new Dictionary<int, int>();      // item index -> Seat.Id
            var resolvedSectorCodes = new Dictionary<int, string>(); // item index -> overlay SectorCode
            var sectorCodeCache = new Dictionary<int, string>();     // overlay id -> SectorCode (per-sector cache)
            for (var i = 0; i < request.Items.Count; i++)
            {
                var item = request.Items[i];
                if (await _overlaySeats.IsSeatSoldAsync(item.EventId, item.SectorId, item.RowNumber, item.SeatNumber))
                {
                    return BadRequest(new TicketOrderResultDto
                    {
                        Success = false,
                        ErrorMessage = $"Seat row {item.RowNumber}, seat {item.SeatNumber} is no longer available. Please pick another seat."
                    });
                }

                var seat = await _overlaySeats.ResolveSeatAsync(item.SectorId, item.RowNumber, item.SeatNumber);
                if (seat == null)
                {
                    return BadRequest(new TicketOrderResultDto
                    {
                        Success = false,
                        ErrorMessage = "One of the selected seats is in an unknown sector."
                    });
                }
                resolvedSeatIds[i] = seat.Id;

                // Store the ticket's Section as the real overlay SectorCode (not the client-supplied
                // display name) so the admin ticket-detail blueprint locator can pin the seat.
                if (!sectorCodeCache.TryGetValue(item.SectorId, out var sectorCode))
                {
                    var overlay = await _overlaySeats.GetOverlayAsync(item.SectorId);
                    sectorCode = overlay?.SectorCode ?? item.SectionName ?? string.Empty;
                    sectorCodeCache[item.SectorId] = sectorCode;
                }
                resolvedSectorCodes[i] = sectorCode;
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
            for (var i = 0; i < request.Items.Count; i++)
            {
                var item = request.Items[i];
                var ticket = new Ticket
                {
                    TicketNumber = $"TK{DateTime.Now.Ticks}{tickets.Count + 1:D4}",
                    EventId = item.EventId,
                    EventName = eventEntity.EventName,
                    EventDate = eventEntity.EventDate,
                    CustomerName = customerName,
                    CustomerEmail = request.CustomerInfo.Email,
                    CustomerPhone = request.CustomerInfo.Phone,
                    CustomerOib = request.CustomerInfo.IsForeigner ? null : request.CustomerInfo.Oib?.Trim(),
                    CustomerDocumentNumber = request.CustomerInfo.IsForeigner ? request.CustomerInfo.DocumentNumber?.Trim() : null,
                    Price = item.Price,
                    PurchaseDate = DateTime.UtcNow,
                    Status = TicketStatuses.Active,
                    IsUsed = false,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    QRCode = "",
                    // Bind the concrete overlay Seat so the sale counts as occupied everywhere
                    // (admin overview, season-seat availability, analytics) via Ticket.SeatId.
                    SeatId = resolvedSeatIds[i],
                    SeatNumber = item.SeatNumber.ToString(),
                    Row = item.RowNumber.ToString(),
                    Section = resolvedSectorCodes[i]
                };

                tickets.Add(ticket);
            }

            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            // Give a guest buyer a claimable account (no-op if they already have one) so the ticket's
            // wallet can be topped up at the bar.
            await _accountProvisioning.EnsureShellAccountAsync(
                request.CustomerInfo.Email, customerName, request.CustomerInfo.Phone, "CustomerPurchase",
                oib: request.CustomerInfo.IsForeigner ? null : request.CustomerInfo.Oib?.Trim());

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

            // Tickets aren't linked to their Payment by a foreign key, so we can't ask for
            // "this order's tickets" directly. Matching by whole day (the previous behaviour)
            // swept in every ticket sold that day — including all externally-ingested/season
            // tickets — so a 2-seat order showed dozens of unrelated tickets.
            // An order's tickets are all created in the same request, a few ms after its Payment
            // row, and customer purchases have SourceSystem == null (external ones are tagged,
            // e.g. "TicketingSimulator"). Correlate on that tight purchase-time burst instead.
            // TODO: add Ticket.PaymentId for a robust link and drop this heuristic.
            var windowStart = payment.CreatedAt.AddSeconds(-2);
            var windowEnd = payment.CreatedAt.AddSeconds(10);
            var tickets = await _context.Tickets
                .Where(t => t.SourceSystem == null
                            && t.PurchaseDate >= windowStart
                            && t.PurchaseDate <= windowEnd)
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