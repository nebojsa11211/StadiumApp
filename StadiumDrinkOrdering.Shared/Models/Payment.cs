using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Payment
{
    public int Id { get; set; }
    
    public int? OrderId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty; // CreditCard, PayPal, Stripe, etc.
    
    [StringLength(100)]
    public string? TransactionId { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Amount { get; set; }
    
    [StringLength(3)]
    public string Currency { get; set; } = "EUR";
    
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    
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
}

public enum PaymentMethod
{
    CreditCard = 1,
    DebitCard = 2,
    DigitalWallet = 3,
    BankTransfer = 4,
    Cash = 5
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



