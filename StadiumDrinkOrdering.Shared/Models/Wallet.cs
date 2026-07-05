using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A fan's stored-value account. One wallet per <see cref="User"/>. Eligibility to create one is
/// gated on the user holding at least one active <see cref="SeasonTicket"/> (see the linker in
/// AuthService), but once created the wallet — and its balance — persist independently of the pass:
/// deposited money is the fan's and is never confiscated when a season ends.
///
/// The append-only <see cref="WalletTransaction"/> ledger is the source of truth for the balance;
/// <see cref="Balance"/> is a cached projection kept consistent inside the same DB transaction as
/// every ledger append. The invariant <c>Balance == Σ(Completed ledger amounts)</c> is verifiable.
/// </summary>
public class Wallet
{
    public int Id { get; set; }

    /// <summary>Owning fan. Unique — a user has at most one wallet.</summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Cached running balance (projection of the ledger). Never written outside a guarded mutation
    /// in WalletService — debits go through a conditional <c>Balance >= amount</c> UPDATE so the
    /// balance can never go negative or race.
    /// </summary>
    [Range(0, 9999999.99)]
    public decimal Balance { get; set; }

    [StringLength(3)]
    public string Currency { get; set; } = "EUR";

    /// <summary>Active / Frozen / Closed (see <see cref="WalletStatus"/>).</summary>
    [StringLength(20)]
    public string Status { get; set; } = WalletStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// PostgreSQL <c>xmin</c> system column mapped as an optimistic-concurrency token
    /// (configured via <c>UseXminAsConcurrencyToken</c>). Defense-in-depth for any EF change-tracked
    /// mutation; the authoritative guard against overdraw is the conditional debit UPDATE.
    /// </summary>
    public uint Version { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}

/// <summary>Lifecycle states for a <see cref="Wallet"/>. String-valued to match the codebase's
/// existing status-column convention (e.g. <see cref="TicketStatuses"/>).</summary>
public static class WalletStatus
{
    public const string Active = "Active";
    public const string Frozen = "Frozen";
    public const string Closed = "Closed";
}
