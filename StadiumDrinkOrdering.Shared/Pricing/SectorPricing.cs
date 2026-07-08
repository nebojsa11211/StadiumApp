namespace StadiumDrinkOrdering.Shared.Pricing;

/// <summary>
/// Central seat-pricing rules for the real (overlay) stadium. Every seat in a sector is sold at
/// the same price; this type resolves what that price is. The resolution is a three-level fallback:
///
///   1. a per-event, per-sector override (<c>EventSectorPrice</c>) — set by the admin on a specific event;
///   2. the sector's own default price (<c>StadiumSectorOverlay.Price</c>) — the same across all events;
///   3. the event's base price scaled by the sector-type multiplier (VIP/premium/family/standard).
///
/// Kept in Shared so the API (customer sell flow) and the Admin UI (form placeholders) agree on the
/// exact numbers without duplicating the multiplier table.
/// </summary>
public static class SectorPricing
{
    /// <summary>Price multiplier applied to the event base price for a given sector type.</summary>
    public static decimal TypeMultiplier(string? sectorType) => (sectorType?.ToLowerInvariant()) switch
    {
        "vip" => 2.0m,
        "premium" => 1.5m,
        "family" => 1.2m,
        _ => 1.0m
    };

    /// <summary>The base×multiplier price for a sector type — the lowest-priority fallback.</summary>
    public static decimal FromBase(decimal basePrice, string? sectorType)
        => basePrice * TypeMultiplier(sectorType);

    /// <summary>
    /// The default price a sector charges when no per-event override exists: the sector's own
    /// explicit price if set, otherwise base×multiplier. This is what the admin form shows as the
    /// placeholder for an un-overridden sector.
    /// </summary>
    public static decimal Default(decimal? sectorDefaultPrice, decimal basePrice, string? sectorType)
        => sectorDefaultPrice ?? FromBase(basePrice, sectorType);

    /// <summary>
    /// Full three-level resolution: per-event override wins, then the sector default, then base×multiplier.
    /// </summary>
    public static decimal Resolve(decimal? eventSectorPrice, decimal? sectorDefaultPrice, decimal basePrice, string? sectorType)
        => eventSectorPrice ?? Default(sectorDefaultPrice, basePrice, sectorType);
}
