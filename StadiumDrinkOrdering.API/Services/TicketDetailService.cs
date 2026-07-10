using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketDetailService
{
    /// <summary>
    /// Builds the full spending drill-down for a ticket: event/seat/customer facts, the ticket
    /// price, every drink order placed against the ticket (with items + payment) and a QR image
    /// for the ticket-card preview. Returns null if the ticket does not exist. Shared by the Admin
    /// (<c>/tickets</c>) and Customer (<c>customer/tickets</c>) detail views.
    /// </summary>
    Task<TicketDetailDto?> BuildDetailAsync(int ticketId);
}

public class TicketDetailService : ITicketDetailService
{
    private readonly ApplicationDbContext _context;
    private readonly IQRCodeService _qrCodeService;

    public TicketDetailService(ApplicationDbContext context, IQRCodeService qrCodeService)
    {
        _context = context;
        _qrCodeService = qrCodeService;
    }

    public async Task<TicketDetailDto?> BuildDetailAsync(int ticketId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Orders)
                .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drink)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
        {
            return null;
        }

        // Payments are keyed by OrderId; load them in one query rather than relying on the
        // ambiguous Order.Payment navigation.
        var orderIds = ticket.Orders.Select(o => o.Id).ToList();
        var payments = orderIds.Count == 0
            ? new List<Payment>()
            : await _context.Payments
                .Where(p => p.OrderId.HasValue && orderIds.Contains(p.OrderId.Value))
                .ToListAsync();

        var drinkOrders = ticket.Orders
            .OrderBy(o => o.CreatedAt)
            .Select(o =>
            {
                var payment = payments.FirstOrDefault(p => p.OrderId == o.Id);
                return new TicketDetailOrderDto
                {
                    OrderId = o.Id,
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt,
                    OrderTotal = o.TotalAmount,
                    Items = o.OrderItems.Select(oi => new TicketDetailOrderItemDto
                    {
                        DrinkName = oi.Drink != null ? oi.Drink.Name : $"Drink #{oi.DrinkId}",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList(),
                    Payment = payment == null ? null : new TicketDetailPaymentDto
                    {
                        Method = payment.PaymentMethod,
                        Status = payment.Status,
                        Amount = payment.Amount,
                        Currency = payment.Currency,
                        Date = payment.PaymentDate
                    }
                };
            })
            .ToList();

        var drinksTotal = drinkOrders.Sum(o => o.OrderTotal);

        // Resolve the fan's stored-value wallet, if the ticket's customer email maps to a registered
        // account that owns one. Wallets are keyed by UserId and a ticket only carries the email, so we
        // match the same way the bar top-up flow does (case-insensitive email → user → wallet).
        bool hasWallet = false;
        decimal? walletBalance = null;
        string? walletStatus = null;
        if (!string.IsNullOrWhiteSpace(ticket.CustomerEmail))
        {
            var email = ticket.CustomerEmail!.ToLower();
            var wallet = await _context.Wallets.AsNoTracking()
                .Where(w => _context.Users.Any(u => u.Id == w.UserId && u.Email.ToLower() == email))
                .FirstOrDefaultAsync();
            if (wallet != null)
            {
                hasWallet = true;
                walletBalance = wallet.Balance;
                walletStatus = wallet.Status;
            }
        }

        return new TicketDetailDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Kind = ticket.Kind,
            Status = ticket.Status,
            IsActive = ticket.IsActive,
            IsUsed = ticket.IsUsed,
            UsedAt = ticket.UsedAt,
            PurchaseDate = ticket.PurchaseDate,
            EventId = ticket.EventId,
            EventName = ticket.EventName,
            EventDate = ticket.EventDate,
            Section = ticket.Section,
            Row = ticket.Row,
            SeatNumber = ticket.SeatNumber,
            CustomerName = ticket.CustomerName,
            CustomerEmail = ticket.CustomerEmail,
            CustomerPhone = ticket.CustomerPhone,
            CustomerHasWallet = hasWallet,
            WalletBalance = walletBalance,
            WalletStatus = walletStatus,
            QRCodeToken = ticket.QRCodeToken,
            QrImageDataUri = await _qrCodeService.GetQrImageDataUriAsync(ticket),
            TicketPrice = ticket.Price,
            DrinkOrders = drinkOrders,
            DrinksTotal = drinksTotal,
            GrandTotal = ticket.Price + drinksTotal
        };
    }
}
