using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Why the count of cups "in the wild" changed. Cups are a rotating pool (issued → out → returned →
/// reissued), so unlike <see cref="StockMovement"/> — which drains a consumable — this ledger tracks a
/// loop. The current outstanding count is the running sum of <see cref="CupMovement.Delta"/>.
/// </summary>
public enum CupMovementType
{
    /// <summary>A cup was handed out with a drink. Delta +1.</summary>
    Issued = 1,

    /// <summary>A cup was returned to the venue. Delta -1.</summary>
    Returned = 2,

    /// <summary>A refundable deposit was charged for an issued cup (no count change; audit).</summary>
    DepositCharged = 3,

    /// <summary>A deposit was refunded on return (no count change; audit).</summary>
    DepositRefunded = 4,

    /// <summary>The refund window elapsed with the cup unreturned; deposit becomes breakage (no count change).</summary>
    Forfeited = 5,

    /// <summary>Manual correction for cups lost/broken/miscounted. Delta typically negative.</summary>
    Shrinkage = 6
}

/// <summary>
/// One immutable entry in the reusable-cup ledger, mirroring <see cref="StockMovement"/>. Every change
/// to the number of cups outstanding writes a row here (who, when, why), so the outstanding count is
/// reconstructable as the running sum of <see cref="Delta"/> and <see cref="QuantityAfter"/> is the
/// snapshot straight after this movement committed. The money side of a deposit lives on
/// <see cref="CupDeposit"/> + the wallet ledger; this ledger is the physical-cup count.
/// See docs/reusable-cups-design.md.
/// </summary>
public class CupMovement
{
    public int Id { get; set; }

    [Required]
    public int CupTypeId { get; set; }

    /// <summary>Signed change to cups-outstanding: +1 issued, -1 returned; 0 for pure audit entries.</summary>
    public int Delta { get; set; }

    /// <summary>Cups-outstanding for this type immediately after this movement was applied.</summary>
    public int QuantityAfter { get; set; }

    public CupMovementType Type { get; set; }

    /// <summary>Which cup mode drove this movement (deposit/honor/BYOC).</summary>
    public CupMode Mode { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>Actor who caused the change; null for customer-driven movements.</summary>
    public int? UserId { get; set; }

    [StringLength(256)]
    public string? UserEmail { get; set; }

    /// <summary>Order this movement relates to, for issue-at-order-time entries.</summary>
    public int? OrderId { get; set; }

    /// <summary>Ticket the cup/deposit is bound to, for return lookups.</summary>
    public int? TicketId { get; set; }

    /// <summary>The deposit record this movement relates to, when applicable.</summary>
    public int? CupDepositId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [JsonIgnore]
    public virtual CupType? CupType { get; set; }

    [JsonIgnore]
    public virtual Order? Order { get; set; }
}
