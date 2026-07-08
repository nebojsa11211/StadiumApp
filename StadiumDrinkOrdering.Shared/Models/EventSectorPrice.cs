using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A per-event override of a sector's ticket price. When a row exists for a given
/// (<see cref="EventId"/>, <see cref="SectorOverlayId"/>) pair, every seat in that sector is sold at
/// <see cref="Price"/> for that specific event. When no row exists, pricing falls back to the sector's
/// own default (<see cref="StadiumSectorOverlay.Price"/>) and then to the event base price × the
/// sector-type multiplier. See <see cref="Pricing.SectorPricing"/> for the full resolution.
///
/// This overrides only the single-event ticket price; the season-ticket price
/// (<see cref="StadiumSectorOverlay.SeasonTicketPrice"/>) is not affected.
/// </summary>
public class EventSectorPrice
{
    [Key]
    public int Id { get; set; }

    /// <summary>The event this price applies to.</summary>
    public int EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual Event? Event { get; set; }

    /// <summary>The overlay sector (<see cref="StadiumSectorOverlay.Id"/>) this price applies to.</summary>
    public int SectorOverlayId { get; set; }

    [ForeignKey(nameof(SectorOverlayId))]
    public virtual StadiumSectorOverlay? SectorOverlay { get; set; }

    /// <summary>The price charged for every seat in this sector for this event.</summary>
    [Range(0, 100000)]
    public decimal Price { get; set; }
}
