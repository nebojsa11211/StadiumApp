using System.ComponentModel.DataAnnotations;

namespace StladiumDrinkOrdering.Shared.DTOs;

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
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Phone { get; set; } = string.Empty;
}

public class PaymentInfoDto
{
    [Required]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    public string ExpiryDate { get; set; } = string.Empty;
    
    [Required]
    public string CVV { get; set; } = string.Empty;
    
    [Required]
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
    public int OrderId { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TicketCount { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class OrderConfirmationDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();
}

public class TicketDto
{
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string QRCodeToken { get; set; } = string.Empty;
}

public class CheckoutFormDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    [CreditCard]
    public string CardNumber { get; set; } = string.Empty;
    
    [Required]
    public string ExpiryDate { get; set; } = string.Empty;
    
    [Required]
    public string CVV { get; set; } = string.Empty;
    
    [Required]
    public string CardholderName { get; set; } = string.Empty;
    
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms and conditions")]
    public bool AgreeToTerms { get; set; }
}