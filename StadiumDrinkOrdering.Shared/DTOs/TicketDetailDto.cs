using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Full drill-down for a single ticket shown when an admin clicks a ticket row on /tickets.
/// Combines the ticket/event/seat/customer facts, the complete spending breakdown
/// (ticket face price + every drink order placed against the ticket + fees) and a
/// ready-to-render QR image so the client can preview the ticket card without another round trip.
/// </summary>
public class TicketDetailDto
{
    // --- Identity ---
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public TicketKind Kind { get; set; } = TicketKind.SingleEvent;
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime PurchaseDate { get; set; }

    // --- Event & seat info ---
    public int? EventId { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public string? Section { get; set; }
    public string? Row { get; set; }
    public string? SeatNumber { get; set; }

    // --- Customer ---
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    /// <summary>Croatian OIB captured on the ticket (11 digits), or null for a foreign/legacy holder.</summary>
    public string? CustomerOib { get; set; }
    /// <summary>Identity document number for a foreign holder with no OIB.</summary>
    public string? CustomerDocumentNumber { get; set; }

    // --- Wallet (fan stored-value account, resolved from the ticket's customer email) ---
    /// <summary>True when the ticket's customer email maps to a registered account that owns a wallet.</summary>
    public bool CustomerHasWallet { get; set; }
    /// <summary>Current wallet balance of the fan owning this ticket, when a wallet exists.</summary>
    public decimal? WalletBalance { get; set; }
    /// <summary>Wallet lifecycle status (Active/Frozen/Closed) when a wallet exists.</summary>
    public string? WalletStatus { get; set; }

    // --- QR / card ---
    public string? QRCodeToken { get; set; }
    /// <summary>data:image/png;base64,... QR image for the on-screen ticket-card preview.</summary>
    public string? QrImageDataUri { get; set; }

    // --- Spending breakdown ---
    /// <summary>Face value of the seat/ticket itself.</summary>
    public decimal TicketPrice { get; set; }
    /// <summary>Drink orders placed against this ticket during the event.</summary>
    public List<TicketDetailOrderDto> DrinkOrders { get; set; } = new();
    /// <summary>Sum of all drink orders.</summary>
    public decimal DrinksTotal { get; set; }
    /// <summary>Ticket price + drinks total.</summary>
    public decimal GrandTotal { get; set; }
    public string Currency { get; set; } = "EUR";
}

/// <summary>A single drink order placed against a ticket, with its line items and payment.</summary>
public class TicketDetailOrderDto
{
    public int OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal OrderTotal { get; set; }
    public List<TicketDetailOrderItemDto> Items { get; set; } = new();
    public TicketDetailPaymentDto? Payment { get; set; }
}

/// <summary>A single line on a drink order.</summary>
public class TicketDetailOrderItemDto
{
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

/// <summary>Payment record backing a drink order, when present.</summary>
public class TicketDetailPaymentDto
{
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime Date { get; set; }
}
