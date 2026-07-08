using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// One row of the per-event sector-pricing editor: an overlay sector together with its default
/// price and the current per-event override (if any). Used to render the "Sector prices" table in
/// the Admin event modal.
/// </summary>
public class EventSectorPriceDto
{
    /// <summary>Overlay sector id (StadiumSectorOverlay.Id).</summary>
    public int SectorOverlayId { get; set; }
    public string SectorCode { get; set; } = string.Empty;
    public string SectorName { get; set; } = string.Empty;
    /// <summary>Sector type (standard/vip/premium/family/wheelchair) — drives the base multiplier.</summary>
    public string Type { get; set; } = "standard";

    /// <summary>
    /// The sector's own explicit default price, if the admin set one on the sector (nullable). When
    /// null, the effective default is the event base price × the sector-type multiplier.
    /// </summary>
    public decimal? SectorDefaultPrice { get; set; }

    /// <summary>
    /// The per-event override for this sector, if one exists. Null means "use the sector default".
    /// </summary>
    public decimal? EventPrice { get; set; }
}

/// <summary>
/// A per-event sector price supplied by the Admin form when saving an event. A null
/// <see cref="Price"/> clears any existing override for that sector (falls back to the default).
/// </summary>
public class EventSectorPriceInputDto
{
    public int SectorOverlayId { get; set; }

    [Range(0, 100000)]
    public decimal? Price { get; set; }
}
