using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>Distinguishes an ordinary single-match ticket from a season-pass–derived one.</summary>
public enum TicketKind
{
    SingleEvent = 0,
    Season = 1
}

public class Ticket
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
    
    [Required]
    public int EventId { get; set; }
    
    public int? SeatId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string QRCode { get; set; } = string.Empty; // Base64 encoded QR image or URL
    
    [Required]
    [StringLength(100)]
    public string QRCodeToken { get; set; } = string.Empty; // Unique token for validation
    
    [StringLength(100)]
    public string? CustomerName { get; set; }
    
    [StringLength(100)]
    public string? CustomerEmail { get; set; }
    
    [StringLength(20)]
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// Croatian personal identifier (OIB) of the ticket holder — exactly 11 digits when set. Captured
    /// at purchase for every domestic buyer; the stable key a bar top-up resolves to (and can provision
    /// an account from) even when no email was given. Null for foreign holders (see
    /// <see cref="CustomerDocumentNumber"/>) and for legacy/simulated tickets created before capture.
    /// </summary>
    [StringLength(11)]
    public string? CustomerOib { get; set; }

    /// <summary>
    /// Identity document number for a foreign holder who has no Croatian OIB (passport / ID card).
    /// Free-form. Mutually exclusive with <see cref="CustomerOib"/>: a domestic buyer supplies the OIB,
    /// a foreigner supplies this instead.
    /// </summary>
    [StringLength(50)]
    public string? CustomerDocumentNumber { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }
    
    public bool IsUsed { get; set; } = false;
    
    public DateTime? UsedAt { get; set; }
    
    [StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Used, Cancelled

    /// <summary>
    /// Whether this is an ordinary single-match ticket or a per-event access ticket derived
    /// from a <see cref="SeasonTicket"/>. Season-derived tickets share the fixed seat of their
    /// pass across every event in the season.
    /// </summary>
    public TicketKind Kind { get; set; } = TicketKind.SingleEvent;

    /// <summary>Set on <see cref="TicketKind.Season"/> tickets: the pass they were generated from.</summary>
    public int? SeasonTicketId { get; set; }
    
    // Legacy fields for backward compatibility
    [StringLength(10)]
    public string? SeatNumber { get; set; }
    
    [StringLength(20)]
    public string? Section { get; set; }
    
    [StringLength(20)]
    public string? Row { get; set; }
    
    [StringLength(100)]
    public string? EventName { get; set; }
    
    public DateTime? EventDate { get; set; }
    
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identifier of this ticket in the external ticketing system, if it originated there.
    /// Unique (nullable) so external sales map 1:1 and ingestion is idempotent.
    /// </summary>
    [StringLength(100)]
    public string? ExternalTicketId { get; set; }

    /// <summary>Name of the system that created this ticket (e.g. "TicketingSimulator"), if external.</summary>
    [StringLength(100)]
    public string? SourceSystem { get; set; }

    // Navigation properties
    public virtual Event Event { get; set; } = null!;
    public virtual Seat Seat { get; set; } = null!;
    public virtual SeasonTicket? SeasonTicket { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<OrderSession> OrderSessions { get; set; } = new List<OrderSession>();
}



