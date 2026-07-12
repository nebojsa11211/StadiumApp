using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>Admin request to add a delta amount of stock to a drink (the "Add stock" action).</summary>
public class RestockDrinkDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity to add must be at least 1.")]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }
}

/// <summary>A single stock-ledger entry surfaced to the admin history view.</summary>
public class StockMovementDto
{
    public int Id { get; set; }
    public int DrinkId { get; set; }
    public int Delta { get; set; }
    public int QuantityAfter { get; set; }

    /// <summary>Movement kind name (Restock / Sale / OrderCancelled / ManualAdjustment).</summary>
    public string Type { get; set; } = string.Empty;

    public string? Note { get; set; }
    public string? UserEmail { get; set; }
    public int? OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}
