using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CreateEventDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>The kind of event. Defaults to "Match"; other values e.g. "Concert" or free text.</summary>
    [StringLength(50)]
    public string EventType { get; set; } = "Match";

    /// <summary>
    /// Home side of a "Match" fixture — the name of one of the venue's resident clubs. Required when
    /// <see cref="EventType"/> is "Match"; ignored (and cleared) for other event types.
    /// </summary>
    [StringLength(100)]
    public string? HomeTeam { get; set; }

    /// <summary>Away/visiting side of a "Match" fixture (free text). Required when the type is "Match".</summary>
    [StringLength(100)]
    public string? AwayTeam { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Start of the ticket-sales window. Optional: null means sales open as soon as the event is put
    /// on sale. When both bounds are supplied, this must be before <see cref="TicketSalesEndDate"/>.
    /// </summary>
    public DateTime? TicketSalesStartDate { get; set; }

    /// <summary>
    /// End of the ticket-sales window. Optional: null means sales stay open while the event is on sale.
    /// </summary>
    public DateTime? TicketSalesEndDate { get; set; }

    /// <summary>
    /// Start of the drink-ordering window (within the live event). Optional: null means ordering opens
    /// as soon as the event goes live. When both bounds are supplied, this must be before
    /// <see cref="DrinkSalesEndDate"/>.
    /// </summary>
    public DateTime? DrinkSalesStartDate { get; set; }

    /// <summary>
    /// End of the drink-ordering window. Optional: null means ordering stays open while the event is live.
    /// </summary>
    public DateTime? DrinkSalesEndDate { get; set; }

    [Required]
    [Range(1, 100000)]
    public int Capacity { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }

    /// <summary>
    /// Optional per-sector ticket-price overrides for this event. Each entry with a non-null price
    /// overrides that sector's default; sectors omitted (or with a null price) keep their default.
    /// </summary>
    public List<EventSectorPriceInputDto>? SectorPrices { get; set; }

    /// <summary>
    /// Optional staff assigned to work this event — for each, their function (Runner/Barman) and the
    /// sectors they cover. Non-staff ids are ignored. Null means "no staff assigned".
    /// </summary>
    public List<EventStaffInputDto>? Staff { get; set; }

    /// <summary>
    /// Optional poster generated in the admin form, as raw base64 (no <c>data:</c> prefix). Null means
    /// the event is created without a poster.
    /// </summary>
    public string? PosterImageBase64 { get; set; }

    /// <summary>MIME type of <see cref="PosterImageBase64"/>; defaults to image/png when omitted.</summary>
    [StringLength(100)]
    public string? PosterContentType { get; set; }

    /// <summary>The prompt the poster was generated from, kept for later reference/regeneration.</summary>
    [StringLength(2000)]
    public string? PosterPrompt { get; set; }

    /// <summary>
    /// Optional downscaled JPEG of the poster as raw base64, produced in the admin browser. Used by
    /// the customer fixture strip so it never serves the multi-MB original.
    /// </summary>
    public string? PosterThumbnailBase64 { get; set; }

    /// <summary>
    /// Whether the admin confirmed the generated text is correct. False stores the poster as
    /// pending review, and fans keep seeing the plain fixture card until it is approved.
    /// </summary>
    public bool PosterApproved { get; set; }

    /// <summary>Event facts baked into the artwork, used later to detect a stale poster.</summary>
    [StringLength(500)]
    public string? PosterSourceSignature { get; set; }
}