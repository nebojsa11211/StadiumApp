using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Payment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    /// <summary>
    /// Set when this payment funds a wallet <b>deposit</b> rather than an order. A Payment references
    /// either an <see cref="OrderId"/> (spend) or a <see cref="WalletTransactionId"/> (top-up); both
    /// are nullable so the row can serve either flow.
    /// </summary>
    public long? WalletTransactionId { get; set; }

    /// <summary>
    /// How the money moved. This is the rail only — it says nothing about which way the money went;
    /// see <see cref="Direction"/> for that. The gateway/provider that processed it (Stripe, etc.) is
    /// recoverable from <see cref="TransactionId"/> / <see cref="PaymentGatewayResponse"/>.
    /// </summary>
    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard;

    /// <summary>
    /// Which way the money moved. <see cref="PaymentDirection.In"/> is the default — a customer paying
    /// the venue. <see cref="PaymentDirection.Out"/> marks a payout back to the fan (wallet cash-out to
    /// card, or cash handed over at the counter); those rows also carry <see cref="RefundAmount"/> and
    /// <see cref="RefundDate"/>.
    /// </summary>
    [Required]
    public PaymentDirection Direction { get; set; } = PaymentDirection.In;

    [StringLength(100)]
    public string? TransactionId { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Amount { get; set; }
    
    [StringLength(3)]
    public string Currency { get; set; } = "EUR";
    
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    
    public string? PaymentGatewayResponse { get; set; } // JSON response from payment provider
    
    public decimal? RefundAmount { get; set; }
    
    public DateTime? RefundDate { get; set; }
    
    [StringLength(500)]
    public string? RefundReason { get; set; }
    
    // Legacy fields for compatibility
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    
    [StringLength(500)]
    public string? FailureReason { get; set; }
    
    // Navigation properties
    public virtual Order? Order { get; set; }
    public virtual WalletTransaction? WalletTransaction { get; set; }
}

public enum PaymentMethod
{
    CreditCard = 1,
    DebitCard = 2,
    DigitalWallet = 3,   // registered fan's user wallet
    BankTransfer = 4,
    Cash = 5,
    TicketWallet = 6     // anonymous bearer balance loaded on the order's ticket
}

/// <summary>
/// Which way money moved on a <see cref="Payment"/>. Kept separate from <see cref="PaymentMethod"/>
/// so the rail (card, cash, wallet) stays orthogonal to the direction: a card charge and a card
/// refund are the same rail travelled in opposite directions.
/// </summary>
public enum PaymentDirection
{
    In = 1,   // customer pays the venue
    Out = 2   // venue pays the fan back (wallet cash-out, refund)
}

public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Refunded = 5,
    Cancelled = 6
}



