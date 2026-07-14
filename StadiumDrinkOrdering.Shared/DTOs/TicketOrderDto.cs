using System.ComponentModel.DataAnnotations;
// NOTE: this namespace also declares its own DTOs.ValidationResult (StadiumStructureDto.cs), which
// shadows the DataAnnotations one within this namespace. The IValidatableObject members below must
// therefore fully-qualify System.ComponentModel.DataAnnotations.ValidationResult.

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CreateTicketOrderRequest
{
    public CustomerInfoDto CustomerInfo { get; set; } = new();
    public PaymentInfoDto PaymentInfo { get; set; } = new();
    public string SessionId { get; set; } = string.Empty;
    public List<TicketOrderItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}

public class CustomerInfoDto : IValidatableObject
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

    /// <summary>True when the buyer is a foreign national identified by <see cref="DocumentNumber"/>
    /// instead of a Croatian <see cref="Oib"/>.</summary>
    public bool IsForeigner { get; set; }

    /// <summary>Croatian OIB — exactly 11 digits. Required for domestic buyers (see <see cref="IsForeigner"/>).</summary>
    [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB mora imati točno 11 znamenki.")]
    public string? Oib { get; set; }

    /// <summary>Identity document number — required for foreign buyers.</summary>
    [StringLength(50)]
    public string? DocumentNumber { get; set; }

    public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext) =>
        TicketIdentity.Validate(IsForeigner, Oib, DocumentNumber);
}

/// <summary>Shared conditional-identity rule: a domestic buyer must give an 11-digit OIB, a foreign
/// buyer must give a document number. Used by both the API request DTO and the Blazor checkout form.</summary>
public static class TicketIdentity
{
    public static IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(bool isForeigner, string? oib, string? documentNumber)
    {
        if (isForeigner)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Broj dokumenta je obavezan za strane državljane.", new[] { nameof(CustomerInfoDto.DocumentNumber) });
        }
        else
        {
            if (string.IsNullOrWhiteSpace(oib))
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("OIB je obavezan.", new[] { nameof(CustomerInfoDto.Oib) });
        }
    }
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

public class TicketOrderItemDto
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
    public List<OrderTicketDto> Tickets { get; set; } = new();
}

public class OrderTicketDto
{
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string QRCodeToken { get; set; } = string.Empty;
}

public class CheckoutFormDto : IValidatableObject
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

    /// <summary>True when the buyer identifies with a foreign document instead of a Croatian OIB.</summary>
    public bool IsForeigner { get; set; }

    /// <summary>Croatian OIB — exactly 11 digits. Required for domestic buyers.</summary>
    [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB mora imati točno 11 znamenki.")]
    public string? Oib { get; set; }

    /// <summary>Identity document number — required for foreign buyers.</summary>
    [StringLength(50)]
    public string? DocumentNumber { get; set; }

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

    public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext) =>
        TicketIdentity.Validate(IsForeigner, Oib, DocumentNumber);
}