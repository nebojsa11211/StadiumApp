using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A design/model of reusable cup the venue stocks (e.g. "0.5L Club Cup"). Carries the standard
/// serving volume (so a BYOC pour is a measured serving, not "a cupful"), the replacement
/// <see cref="UnitCost"/> used to value shrinkage, and optional branded artwork. A venue running a
/// single fungible pool can operate off one default type; the entity exists so premium/multi-design
/// cups and BYOC volume standards have a home. See docs/reusable-cups-design.md.
/// </summary>
public class CupType
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Standard serving volume in millilitres. Defines the measured pour for this cup.</summary>
    public int VolumeMl { get; set; }

    /// <summary>Replacement cost of one cup, used to value shrinkage (unreturned cups). Not a price.</summary>
    [Range(0, 9999.99)]
    public decimal UnitCost { get; set; }

    /// <summary>Optional branded cup artwork, stored in-DB (PostgreSQL bytea) with its content type.</summary>
    public byte[]? Logo { get; set; }

    [StringLength(100)]
    public string? LogoContentType { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
