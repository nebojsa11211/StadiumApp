using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Why a drink's stock level changed. The kind drives how the movement is displayed and lets us
/// tell an admin restock apart from an automatic sale/cancellation restore.
/// </summary>
public enum StockMovementType
{
    /// <summary>Admin added stock (delivery, correction upward). Positive delta.</summary>
    Restock = 1,

    /// <summary>Stock reserved for a placed order. Negative delta.</summary>
    Sale = 2,

    /// <summary>Stock returned to inventory because an order was cancelled. Positive delta.</summary>
    OrderCancelled = 3,

    /// <summary>Admin set an absolute stock value via the drink form (delta may be + or -).</summary>
    ManualAdjustment = 4
}

/// <summary>
/// One immutable entry in a drink's stock ledger. Every change to <see cref="Drink.StockQuantity"/>
/// writes a row here, so the current level is always reconstructable (running sum of <see cref="Delta"/>)
/// and every change is attributable (who, when, why). <see cref="QuantityAfter"/> is the balance snapshot
/// straight after this movement committed, so audit views need no recomputation.
/// </summary>
public class StockMovement
{
    public int Id { get; set; }

    [Required]
    public int DrinkId { get; set; }

    /// <summary>Signed change: positive adds stock, negative removes it.</summary>
    public int Delta { get; set; }

    /// <summary>Drink stock level immediately after this movement was applied.</summary>
    public int QuantityAfter { get; set; }

    public StockMovementType Type { get; set; }

    /// <summary>Optional free-text reason (e.g. "supplier delivery", correction note).</summary>
    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>Actor who caused the change; null for system/customer-driven movements.</summary>
    public int? UserId { get; set; }

    [StringLength(256)]
    public string? UserEmail { get; set; }

    /// <summary>Order this movement relates to, for Sale / OrderCancelled entries.</summary>
    public int? OrderId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [JsonIgnore]
    public virtual Drink? Drink { get; set; }

    [JsonIgnore]
    public virtual Order? Order { get; set; }
}
