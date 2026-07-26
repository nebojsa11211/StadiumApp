using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class OrderItem
{
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int DrinkId { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, 999.99)]
    public decimal UnitPrice { get; set; }
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal TotalPrice { get; set; }
    
    [StringLength(200)]
    public string? SpecialInstructions { get; set; }

    // ---- Reusable cup handling (per line) — see docs/reusable-cups-design.md ----

    /// <summary>How this drink line is served w.r.t. reusable cups. <see cref="CupMode.None"/> (disposable)
    /// by default, so existing/rows and callers that ignore cups are unaffected.</summary>
    public CupMode CupMode { get; set; } = CupMode.None;

    /// <summary>The reusable cup design used (deposit/honor). Null for None and unregistered BYOC.</summary>
    public int? CupTypeId { get; set; }

    /// <summary>Refundable deposit charged for this line's cup(s). 0 unless <see cref="CupMode.Deposit"/>.
    /// Flows into <see cref="Order.TotalAmount"/> as a separate refundable component.</summary>
    [Range(0, 9999.99)]
    public decimal CupDepositAmount { get; set; }

    /// <summary>The customer's scanned personal cup, for <see cref="CupMode.ByocQr"/>.</summary>
    public int? RegisteredCupId { get; set; }

    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Drink Drink { get; set; } = null!;
    public virtual CupType? CupType { get; set; }
    public virtual RegisteredCup? RegisteredCup { get; set; }
}



