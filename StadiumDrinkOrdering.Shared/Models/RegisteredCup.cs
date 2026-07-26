using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>Lifecycle of a registered personal/premium cup.</summary>
public enum RegisteredCupStatus
{
    Active = 1,
    Retired = 2,
    Lost = 3
}

/// <summary>
/// A customer's own reusable cup (BYOC) — or a premium individually-tracked club cup — identified by a
/// unique scannable <see cref="QrToken"/>, mirroring <see cref="Ticket.QRCodeToken"/>. The QR does three
/// jobs at once: it is the <b>approved-cup / hygiene gate</b> (only registered cups of a known
/// <see cref="CupType"/> volume are accepted for a measured pour), the <b>loyalty/attribution</b> link to
/// an owner (for "cups saved" stats), and the <b>discount anti-fraud</b> proof required to claim the BYOC
/// discount. Ownership reuses the <see cref="WalletOwnerType"/> User/Ticket pattern; at most one owner is
/// set (an unassigned approved cup has neither). See docs/reusable-cups-design.md.
/// </summary>
public class RegisteredCup
{
    public int Id { get; set; }

    /// <summary>Unique scannable token printed/etched on the cup. Resolved at order time like a ticket QR.</summary>
    [Required]
    [StringLength(200)]
    public string QrToken { get; set; } = string.Empty;

    /// <summary>Approved cup model → known serving volume. Null only for a not-yet-classified cup.</summary>
    public int? CupTypeId { get; set; }

    /// <summary>Which kind of owner this cup belongs to (mirrors the wallet owner discriminator).</summary>
    public WalletOwnerType OwnerType { get; set; } = WalletOwnerType.User;

    /// <summary>Owning fan (user-owned cups). Null for a ticket-owned or unassigned cup.</summary>
    public int? UserId { get; set; }

    /// <summary>Owning ticket (guest-owned cups). Null for a user-owned or unassigned cup.</summary>
    public int? TicketId { get; set; }

    /// <summary>Whether this cup is approved for service (hygiene/volume). Unapproved cups are rejected
    /// when the venue requires approved cups.</summary>
    public bool IsApproved { get; set; } = true;

    public RegisteredCupStatus Status { get; set; } = RegisteredCupStatus.Active;

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual CupType? CupType { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual Ticket? Ticket { get; set; }
}
