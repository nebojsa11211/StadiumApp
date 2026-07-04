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

    // Navigation properties
    public virtual Season Season { get; set; } = null!;
    public virtual Seat Seat { get; set; } = null!;

    /// <summary>The per-event access tickets generated from this pass (one per season event).</summary>
    public virtual ICollection<Ticket> DerivedTickets { get; set; } = new List<Ticket>();
}
