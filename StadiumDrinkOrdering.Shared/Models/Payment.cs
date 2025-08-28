using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Payment
{
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Amount { get; set; }
    
    [Required]
    public PaymentMethod Method { get; set; }
    
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    [StringLength(100)]
    public string? TransactionId { get; set; }
    
    [StringLength(100)]
    public string? PaymentGatewayReference { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    
    [StringLength(500)]
    public string? FailureReason { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
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



