using StadiumDrinkOrdering.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? FailureReason { get; set; }
}

public class CreatePaymentDto
{
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Amount { get; set; }
    
    [Required]
    public PaymentMethod Method { get; set; }
    
    // Payment gateway specific fields
    public string? PaymentToken { get; set; }
    public string? CardLast4 { get; set; }
    public string? CardBrand { get; set; }
}

public class ProcessPaymentDto
{
    [Required]
    public string PaymentToken { get; set; } = string.Empty;
    
    [Required]
    public PaymentMethod Method { get; set; }
}

public class PaymentIntentDto
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Status { get; set; } = string.Empty;
}

public class PaymentConfirmationDto
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string? TransactionId { get; set; }
}



