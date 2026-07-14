using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A stored-value account owned by <b>exactly one</b> of: a registered <see cref="User"/> (the fan
/// wallet) or a <see cref="Ticket"/> (an anonymous, no-account bearer wallet loaded at the counter).
/// <see cref="OwnerType"/> is the discriminator and the <c>(UserId IS NULL) &lt;&gt; (TicketId IS NULL)</c>
/// check constraint enforces the exactly-one rule.
///
/// For a user wallet, eligibility to create one is gated on the user holding at least one active
/// <see cref="SeasonTicket"/> (see the linker in AuthService), but once created the wallet — and its
/// balance — persist independently of the pass: deposited money is the fan's and is never confiscated
/// when a season ends. A ticket wallet is a bearer instrument: whoever holds the ticket controls the
/// balance, and a cashed-out ticket wallet is moved to <see cref="WalletStatus.Closed"/> (terminal).
///
/// The append-only <see cref="WalletTransaction"/> ledger is the source of truth for the balance;
/// <see cref="Balance"/> is a cached projection kept consistent inside the same DB transaction as
/// every ledger append. The invariant <c>Balance == Σ(Completed ledger amounts)</c> is verifiable.
/// </summary>
public class Wallet
{
    public int Id { get; set; }

    /// <summary>Which kind of owner this wallet belongs to — see <see cref="WalletOwnerType"/>.</summary>
    public WalletOwnerType OwnerType { get; set; } = WalletOwnerType.User;

    /// <summary>Owning fan (user wallets only). Unique when set — a user has at most one wallet.
    /// Null for a ticket-owned wallet.</summary>
    public int? UserId { get; set; }

    /// <summary>Owning ticket (anonymous ticket wallets only). Unique when set — a ticket has at most
    /// one wallet. Null for a user-owned wallet.</summary>
    public int? TicketId { get; set; }

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
    public virtual User? User { get; set; }
    public virtual Ticket? Ticket { get; set; }
    public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}

/// <summary>Which kind of owner a <see cref="Wallet"/> belongs to. Stored as an int discriminator.</summary>
public enum WalletOwnerType
{
    /// <summary>Owned by a registered <see cref="User"/> (the fan wallet). <c>UserId</c> is set.</summary>
    User = 0,
    /// <summary>Owned by a <see cref="Ticket"/> (anonymous bearer wallet). <c>TicketId</c> is set.</summary>
    Ticket = 1
}

/// <summary>Lifecycle states for a <see cref="Wallet"/>. String-valued to match the codebase's
/// existing status-column convention (e.g. <see cref="TicketStatuses"/>).</summary>
public static class WalletStatus
{
    public const string Active = "Active";
    public const string Frozen = "Frozen";
    /// <summary>Terminal. A user wallet closed by an admin, or a ticket wallet that has been cashed out.</summary>
    public const string Closed = "Closed";
}
