using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UpdateEventDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>The kind of event (e.g. "Match", "Concert"). Null leaves the existing value unchanged.</summary>
    [StringLength(50)]
    public string? EventType { get; set; }

    /// <summary>
    /// Home side of a "Match" fixture — the name of one of the venue's resident clubs. Required when
    /// the (effective) type is "Match"; cleared for other types. Null leaves the existing value unchanged.
    /// </summary>
    [StringLength(100)]
    public string? HomeTeam { get; set; }

    /// <summary>Away/visiting side of a "Match" fixture (free text). Null leaves the existing value unchanged.</summary>
    [StringLength(100)]
    public string? AwayTeam { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    /// <summary>End of the event window; must be after <see cref="Date"/> when supplied.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Start of the ticket-sales window. Null leaves the existing value unchanged. When the effective
    /// start and end are both set, the start must be before the end.
    /// </summary>
    public DateTime? TicketSalesStartDate { get; set; }

    /// <summary>End of the ticket-sales window. Null leaves the existing value unchanged.</summary>
    public DateTime? TicketSalesEndDate { get; set; }

    /// <summary>
    /// Start of the drink-ordering window. Null leaves the existing value unchanged. When the effective
    /// start and end are both set, the start must be before the end.
    /// </summary>
    public DateTime? DrinkSalesStartDate { get; set; }

    /// <summary>End of the drink-ordering window. Null leaves the existing value unchanged.</summary>
    public DateTime? DrinkSalesEndDate { get; set; }

    [Range(1, 100000)]
    public int? Capacity { get; set; }

    [Range(0.01, 10000)]
    public decimal? BasePrice { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>Optional season this event belongs to. Linking backfills season passes with a match ticket.</summary>
    public int? SeasonId { get; set; }

    /// <summary>
    /// Per-sector ticket-price overrides for this event. When non-null, the supplied set fully
    /// replaces the event's existing overrides: entries with a price upsert an override, entries
    /// with a null price (or omitted sectors) clear it. Null leaves existing overrides unchanged.
    /// </summary>
    public List<EventSectorPriceInputDto>? SectorPrices { get; set; }

    /// <summary>
    /// Staff assigned to work this event — for each, their function (Runner/Barman) and covered sectors.
    /// When non-null, the supplied set fully replaces the event's existing assignments (omitted members
    /// are unassigned). Non-staff ids are ignored. Null leaves existing assignments unchanged.
    /// </summary>
    public List<EventStaffInputDto>? Staff { get; set; }

    /// <summary>
    /// A newly generated poster as raw base64 (no <c>data:</c> prefix), replacing any existing one.
    /// Null leaves the current poster untouched — to remove it, set <see cref="RemovePoster"/>.
    /// </summary>
    public string? PosterImageBase64 { get; set; }

    /// <summary>MIME type of <see cref="PosterImageBase64"/>; defaults to image/png when omitted.</summary>
    [StringLength(100)]
    public string? PosterContentType { get; set; }

    /// <summary>The prompt the poster was generated from, kept for later reference/regeneration.</summary>
    [StringLength(2000)]
    public string? PosterPrompt { get; set; }

    /// <summary>
    /// Optional downscaled JPEG of the poster as raw base64, accompanying
    /// <see cref="PosterImageBase64"/>. Used by the customer fixture strip.
    /// </summary>
    public string? PosterThumbnailBase64 { get; set; }

    /// <summary>
    /// Whether the admin confirmed the generated text is correct. A newly generated poster saved
    /// with false stays pending review, and the fixture card keeps falling back to the plain layout.
    /// </summary>
    public bool PosterApproved { get; set; }

    /// <summary>Event facts baked into the artwork, used later to detect a stale poster.</summary>
    [StringLength(500)]
    public string? PosterSourceSignature { get; set; }

    /// <summary>
    /// Approves the event's existing poster without supplying a new image — the "looks correct"
    /// action in the admin modal.
    /// </summary>
    public bool ApproveExistingPoster { get; set; }

    /// <summary>
    /// When true, deletes the event's existing poster. Ignored if <see cref="PosterImageBase64"/> is
    /// also supplied (a replacement wins over a removal).
    /// </summary>
    public bool RemovePoster { get; set; }
}