using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// One immutable, append-only entry in a <see cref="Wallet"/>'s ledger — the source of truth for
/// the wallet balance. A row is never updated or deleted once <see cref="WalletTransactionStatus.Completed"/>;
/// corrections are posted as new <see cref="WalletTransactionType.Reversal"/> / <see cref="WalletTransactionType.Adjustment"/>
/// rows. Deposit rows are the only ones that transition (<c>Pending → Completed/Failed</c>) while a
/// payment gateway settles.
///
/// <para><b>Idempotency:</b> every mutation carries a caller-supplied <see cref="IdempotencyKey"/> with
/// a unique index. Replaying the same key (deposit webhook re-fire, order-create retry, double cancel)
/// returns the original row instead of posting a second one.</para>
/// </summary>
public class WalletTransaction
{
    public long Id { get; set; }

    [Required]
    public int WalletId { get; set; }

    [Required]
    public WalletTransactionType Type { get; set; }

    /// <summary>
    /// Signed amount: credits (Deposit/Refund/positive Adjustment) are positive, debits (Payment/
    /// Reversal/negative Adjustment) are negative. Summing all Completed amounts reproduces the balance.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>Wallet balance immediately after this entry was applied — a running snapshot that makes
    /// the ledger self-auditing and reconciliation cheap.</summary>
    public decimal BalanceAfter { get; set; }

    [StringLength(3)]
    public string Currency { get; set; } = "EUR";

    /// <summary>Pending (deposit mid-gateway) / Completed / Failed.</summary>
    [StringLength(20)]
    public string Status { get; set; } = WalletTransactionStatus.Completed;

    /// <summary>
    /// Unique anti-double-charge key. For deposits it is the gateway transaction id; for order spends
    /// a client/order-scoped key (e.g. <c>order-{guid}</c>); for refunds <c>refund-order-{orderId}</c>.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;

    /// <summary>What this entry references, e.g. "Order", "Payment", "Deposit". Free-form so the ledger
    /// stays decoupled from the referenced tables.</summary>
    [StringLength(50)]
    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>The fan or admin who caused this entry (null for system/gateway-initiated).</summary>
    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public virtual Wallet Wallet { get; set; } = null!;
}

public enum WalletTransactionType
{
    /// <summary>Fan added funds via a payment method (credit).</summary>
    Deposit = 0,
    /// <summary>Fan spent balance on a purchase, e.g. a drink order (debit).</summary>
    Payment = 1,
    /// <summary>Money returned to the wallet, e.g. a cancelled order (credit).</summary>
    Refund = 2,
    /// <summary>Reversal of a prior entry, e.g. a failed/charged-back deposit (debit).</summary>
    Reversal = 3,
    /// <summary>Manual admin correction/comp, positive or negative, with a mandatory reason.</summary>
    Adjustment = 4
}

/// <summary>Status of a single ledger entry. String constants match the codebase's status-column style.</summary>
public static class WalletTransactionStatus
{
    /// <summary>Deposit awaiting gateway settlement; not yet reflected in the balance.</summary>
    public const string Pending = "Pending";
    public const string Completed = "Completed";
    public const string Failed = "Failed";
}
