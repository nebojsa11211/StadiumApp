using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>Lifecycle of a single cup deposit.</summary>
public enum CupDepositStatus
{
    /// <summary>Deposit taken and outstanding — the cup is in the wild, money owed back on return.</summary>
    Held = 1,

    /// <summary>Cup returned and deposit refunded.</summary>
    Refunded = 2,

    /// <summary>Refund window elapsed unreturned — deposit recognised as breakage revenue.</summary>
    Forfeited = 3,

    /// <summary>The order was cancelled: the deposit was refunded as part of the order-total refund (or
    /// never collected), so it must not be cup-refunded again. Terminal, non-refundable.</summary>
    Voided = 4
}

/// <summary>
/// A single refundable cup deposit — the <b>link and state</b>, not the money itself. The money moves
/// on the append-only wallet ledger (<see cref="WalletTransaction"/>) with a matching audit
/// <see cref="Payment"/>; this row records which ticket/wallet/order the deposit belongs to so a return
/// station can find "N outstanding deposits on this ticket" and refund up to that count. A deposit is a
/// <b>liability, not revenue</b> — segregated here until refunded (→ removed) or forfeited (→ breakage).
/// Binding can be by ticket/wallet, a printed return token, and/or a cup QR (any combination, per venue).
/// See docs/reusable-cups-design.md.
/// </summary>
public class CupDeposit
{
    public int Id { get; set; }

    /// <summary>Order the deposit was taken on. Null if taken standalone.</summary>
    public int? OrderId { get; set; }

    /// <summary>Ticket the deposit is bound to (ticket/wallet binding). Return scans this.</summary>
    public int? TicketId { get; set; }

    /// <summary>Wallet the deposit was charged to / will be refunded to (wallet-credit default path).</summary>
    public int? WalletId { get; set; }

    [Required]
    public int CupTypeId { get; set; }

    [Range(0.01, 9999.99)]
    public decimal Amount { get; set; }

    public CupDepositStatus Status { get; set; } = CupDepositStatus.Held;

    /// <summary>The wallet ledger entry that took the deposit (debit). Null for non-wallet charges.</summary>
    public long? ChargeTransactionId { get; set; }

    /// <summary>The wallet ledger entry that refunded the deposit (credit). Set once refunded.</summary>
    public long? RefundTransactionId { get; set; }

    /// <summary>Printed return-token QR value, when the return-token binding is used.</summary>
    [StringLength(200)]
    public string? ReturnTokenQr { get; set; }

    /// <summary>The physical deposit cup's own QR, bound at the bar when the drink is poured (cup-QR
    /// binding). Lets staff return the deposit by scanning the cup itself, not just the ticket. Null until
    /// assigned. See docs/reusable-cups-design.md.</summary>
    [StringLength(200)]
    public string? CupQrToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>When the deposit reached a terminal state (refunded/forfeited).</summary>
    public DateTime? ResolvedAt { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Order? Order { get; set; }

    [JsonIgnore]
    public virtual CupType? CupType { get; set; }
}
