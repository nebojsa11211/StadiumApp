using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Per-event configuration for a single overlay sector: a price override and/or a "disabled for this
/// event" flag. A row exists for a given (<see cref="EventId"/>, <see cref="SectorOverlayId"/>) pair
/// only when the admin has set at least one of these — a price override, a disable, or both.
///
/// <para><see cref="Price"/>: when set, every seat in that sector is sold at this price for this specific
/// event. When null (no override), pricing falls back to the sector's own default
/// (<see cref="StadiumSectorOverlay.Price"/>) and then to the event base price × the sector-type
/// multiplier. See <see cref="Pricing.SectorPricing"/> for the full resolution. This overrides only the
/// single-event ticket price; the season-ticket price (<see cref="StadiumSectorOverlay.SeasonTicketPrice"/>)
/// is not affected.</para>
///
/// <para><see cref="IsDisabled"/>: when true, the sector is closed for this event only — no seats in it can
/// be reserved or sold, and the customer flow shows it as unavailable. The same sector stays available
/// for every other event.</para>
/// </summary>
public class EventSectorPrice
{
    [Key]
    public int Id { get; set; }

    /// <summary>The event this configuration applies to.</summary>
    public int EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual Event? Event { get; set; }

    /// <summary>The overlay sector (<see cref="StadiumSectorOverlay.Id"/>) this configuration applies to.</summary>
    public int SectorOverlayId { get; set; }

    [ForeignKey(nameof(SectorOverlayId))]
    public virtual StadiumSectorOverlay? SectorOverlay { get; set; }

    /// <summary>
    /// The price charged for every seat in this sector for this event, or null to use the sector default.
    /// </summary>
    [Range(0, 100000)]
    public decimal? Price { get; set; }

    /// <summary>When true, this sector is closed for this event: no seats in it can be sold.</summary>
    public bool IsDisabled { get; set; }
}
