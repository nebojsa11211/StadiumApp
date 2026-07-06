using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// An annual/season pass. It owns one fixed <see cref="Seat"/> for an entire
/// <see cref="Season"/>. Access to each match is materialized as derived
/// <see cref="Ticket"/> rows (<see cref="TicketKind.Season"/>) — one per event in the
/// season — so the existing seat-occupancy logic (which keys off <c>Ticket.SeatId</c> +
/// <see cref="TicketStatuses.CountsAsSold"/>) counts a season holder's seat as taken for
/// every match without any change to the availability queries. Refunding the pass cancels
/// its derived tickets together.
/// </summary>
public class SeasonTicket
{
    public int Id { get; set; }

    [Required]
    public int SeasonId { get; set; }

    /// <summary>The fixed seat held for the whole season.</summary>
    [Required]
    public int SeatId { get; set; }

    [StringLength(50)]
    public string SeasonTicketNumber { get; set; } = string.Empty;

    /// <summary>
    /// Stable token encoded in the physical season pass's QR (as <c>{host}/t/{PassToken}</c>). Scanning
    /// it resolves to the holder's derived ticket for whatever match is live now (see the customer
    /// access resolver). Distinct from per-event <see cref="Ticket.QRCodeToken"/>s. Backfilled for
    /// existing passes by migration.
    /// </summary>
    [StringLength(100)]
    public string? PassToken { get; set; }

    [StringLength(100)]
    public string? HolderName { get; set; }

    [StringLength(100)]
    public string? HolderEmail { get; set; }

    [StringLength(20)]
    public string? HolderPhone { get; set; }

    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    /// <summary>Active / Cancelled (mirrors <see cref="TicketStatuses"/>).</summary>
    [StringLength(20)]
    public string Status { get; set; } = TicketStatuses.Active;

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identifier of this season ticket in the external ticketing system, if it originated
    /// there. Unique (nullable) so external season tickets map 1:1 and ingestion is idempotent.
    /// </summary>
    [StringLength(100)]
    public string? ExternalSeasonTicketId { get; set; }

    [StringLength(100)]
    public string? SourceSystem { get; set; }

    /// <summary>
    /// The fan account this pass belongs to, once linked. A season ticket may be ingested externally
    /// before its holder has an account, so this is nullable and populated by the email-match linker
    /// (<c>HolderEmail == User.Email</c>) on register/login. Once set, the FK — not live email
    /// matching — is the authoritative owner and drives wallet eligibility.
    /// </summary>
    public int? UserId { get; set; }

    // Navigation properties
    public virtual Season Season { get; set; } = null!;
    public virtual Seat Seat { get; set; } = null!;
    public virtual User? User { get; set; }

    /// <summary>The per-event access tickets generated from this pass (one per season event).</summary>
    public virtual ICollection<Ticket> DerivedTickets { get; set; } = new List<Ticket>();
}
