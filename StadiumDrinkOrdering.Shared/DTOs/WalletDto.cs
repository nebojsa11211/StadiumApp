using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>Customer-facing snapshot of a fan's wallet. Always derived from the authenticated
/// user server-side — the client never supplies a wallet or user id.</summary>
public class WalletSummaryDto
{
    public int WalletId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Status { get; set; } = WalletStatus.Active;

    /// <summary>Whether the user is eligible to hold a wallet (has ≥1 active season ticket). A wallet
    /// that already exists stays usable even if this later becomes false.</summary>
    public bool IsEligible { get; set; }

    /// <summary>False when the user is eligible but no wallet has been created yet.</summary>
    public bool Exists { get; set; }

    /// <summary>True when the configured gateway settles asynchronously and the browser must collect +
    /// confirm card details (real gateway, e.g. Stripe). False for the synchronous mock, which credits
    /// immediately on submit and needs no card entry.</summary>
    public bool RequiresCardEntry { get; set; }

    /// <summary>Publishable (public) gateway key the browser uses to mount the card field, when
    /// <see cref="RequiresCardEntry"/> is true. Null otherwise.</summary>
    public string? PublishableKey { get; set; }
}

public class WalletTransactionDto
{
    public long Id { get; set; }
    public WalletTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Status { get; set; } = WalletTransactionStatus.Completed;
    public string? Description { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>Paginated ledger, following the codebase's per-feature list-DTO convention.</summary>
public class WalletTransactionListDto
{
    public List<WalletTransactionDto> Transactions { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

/// <summary>Request to add funds. <see cref="IdempotencyKey"/> makes a re-submitted deposit a no-op.</summary>
public class InitiateDepositDto
{
    [Range(0.01, 100000)]
    public decimal Amount { get; set; }

    /// <summary>Funding method presented to the gateway (CreditCard, PayPal, …).</summary>
    [Required]
    [StringLength(50)]
    public string Method { get; set; } = "CreditCard";

    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>Outcome of initiating a deposit. With the mock gateway the deposit settles immediately
/// (<see cref="Status"/> = Completed); a real gateway would return <c>Pending</c> plus a
/// <see cref="RedirectUrl"/>/client secret and settle later via webhook.</summary>
public class DepositResultDto
{
    public bool Success { get; set; }
    public long WalletTransactionId { get; set; }
    public string Status { get; set; } = WalletTransactionStatus.Pending;
    public decimal NewBalance { get; set; }
    public string? RedirectUrl { get; set; }
    public string? FailureReason { get; set; }

    /// <summary>Set for async (real gateway) deposits: the browser confirms the card with these before the
    /// wallet is credited via webhook. Null for the synchronous mock, which credits immediately.</summary>
    public string? ClientSecret { get; set; }
    public string? PublishableKey { get; set; }

    /// <summary>Provider intent id for an async deposit. The browser polls
    /// <c>GET api/wallet/me/deposits/{id}/status</c> with this after confirming the card to learn whether
    /// the webhook has credited the wallet. Null for the synchronous mock.</summary>
    public string? ProviderIntentId { get; set; }

    /// <summary>True when the deposit needs browser-side card confirmation (async gateway).</summary>
    public bool RequiresAction { get; set; }
}

/// <summary>Settlement state of a specific async deposit intent, as recorded in <b>this</b> system's ledger.
/// <see cref="Settled"/> flips to true only once the signed webhook has credited the wallet — a definitive
/// success signal, unlike inferring it from a balance change.</summary>
public class DepositStatusDto
{
    /// <summary>Pending until the webhook credits the wallet, then Completed. (Never Failed here: a declined
    /// card is reported to the browser by the confirmation call, not persisted as a ledger row.)</summary>
    public string Status { get; set; } = WalletTransactionStatus.Pending;

    /// <summary>True once the wallet has been credited for this intent.</summary>
    public bool Settled { get; set; }

    /// <summary>Wallet balance after the credit (or the current balance while still pending).</summary>
    public decimal NewBalance { get; set; }

    /// <summary>The credited ledger entry's id, once settled; 0 while pending.</summary>
    public long WalletTransactionId { get; set; }
}

/// <summary>Admin view of a wallet, joined with its owner for the management console.</summary>
public class WalletAdminDto
{
    public int WalletId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Status { get; set; } = WalletStatus.Active;
    public DateTime CreatedAt { get; set; }
}

public class WalletAdminListDto
{
    public List<WalletAdminDto> Wallets { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

/// <summary>Admin manual adjustment (comp/correction). Positive credits, negative debits (guarded so
/// it can't overdraw). A reason is mandatory and recorded on the ledger entry.</summary>
public class AdjustWalletDto
{
    [Range(-100000, 100000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 3)]
    public string Reason { get; set; } = string.Empty;
}

/// <summary>Admin-initiated refund to the wallet (e.g. goodwill / out-of-band correction).</summary>
public class RefundWalletDto
{
    [Range(0.01, 100000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 3)]
    public string Reason { get; set; } = string.Empty;
}

/// <summary>Result of a wallet debit (used by the order-spend path).</summary>
public enum WalletDebitOutcome
{
    Success,
    InsufficientFunds,
    WalletNotFound,
    WalletFrozen,
    AlreadyApplied // idempotent replay of a prior debit with the same key
}
