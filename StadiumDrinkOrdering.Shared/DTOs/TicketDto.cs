using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string? Section { get; set; }
    public string? Row { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public bool IsActive { get; set; }
    /// <summary>Canonical lifecycle status: Active / Used / Cancelled (see <see cref="TicketStatuses"/>).</summary>
    public string? Status { get; set; }
    /// <summary>True once the ticket has been scanned/redeemed at the gate.</summary>
    public bool IsUsed { get; set; }
    public int? EventId { get; set; }
    public int? OrderId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerName { get; set; }
    public decimal Price { get; set; }
    public TicketKind Kind { get; set; } = TicketKind.SingleEvent;

    /// <summary>
    /// Set on <see cref="TicketKind.Season"/> tickets: the season pass this per-match admission was
    /// derived from. The pass itself is not a ticket (it lives in its own table), so this is what an
    /// admin follows to get from a single match row back to the annual pass that produced it.
    /// </summary>
    public int? SeasonTicketId { get; set; }

    /// <summary>The season owning <see cref="SeasonTicketId"/>, so the UI can deep-link to the pass.</summary>
    public int? SeasonId { get; set; }

    /// <summary>The pass's own number (e.g. <c>SEA-STK-…</c>), distinct from this ticket's number.</summary>
    public string? SeasonTicketNumber { get; set; }
}

public class ValidateTicketDto
{
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
}

public class TicketValidationResultDto
{
    public bool IsValid { get; set; }
    public TicketDto? Ticket { get; set; }
    public string? ErrorMessage { get; set; }
}



