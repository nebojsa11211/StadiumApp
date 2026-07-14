using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Staff request to load cash onto an <b>anonymous ticket wallet</b> (no account required). The balance
/// lives on the ticket — a bearer instrument. <see cref="TicketId"/> comes from a prior resolve call;
/// <see cref="IdempotencyKey"/> makes a re-submitted top-up a safe no-op.
/// </summary>
public class TicketWalletTopupRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Range(0.01, 100000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>Staff request to cash out (hand back) the remaining balance on a ticket wallet. Terminal —
/// the wallet is moved to Closed. <see cref="IdempotencyKey"/> should be stable per ticket wallet
/// (e.g. <c>cashout-{ticketId}</c>) so a re-scan/replay pays out at most once.</summary>
public class TicketWalletCashoutRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>Outcome of a ticket-wallet top-up or cash-out. On a top-up <see cref="NewBalance"/> is the
/// credited balance; on a cash-out <see cref="RefundAmount"/> is the cash to hand back and
/// <see cref="NewBalance"/> is 0.</summary>
public class TicketWalletResultDto
{
    public bool Success { get; set; }

    public decimal NewBalance { get; set; }

    /// <summary>Cash to hand back to the guest (cash-out only).</summary>
    public decimal? RefundAmount { get; set; }

    /// <summary>Wallet status after the operation (Active after top-up, Closed after cash-out).</summary>
    public string Status { get; set; } = Models.WalletStatus.Active;

    public long WalletTransactionId { get; set; }

    /// <summary>Set on failure: "WalletFrozen", "TicketNotFound", "InvalidAmount", "LimitExceeded",
    /// "AlreadyCashedOut", "NothingToCashOut".</summary>
    public string? FailureReason { get; set; }
}

/// <summary>Staff request to claim a ticket wallet's balance onto a registered account by email. Moves
/// the bearer balance to the account and emails a set-password link so the fan can finish signing up.</summary>
public class TicketWalletClaimRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
}

/// <summary>Outcome of a claim-by-email. On success <see cref="ClaimedAmount"/> was moved onto the
/// account's wallet and a set-password link was emailed to <see cref="Email"/>.</summary>
public class TicketWalletClaimResultDto
{
    public bool Success { get; set; }
    public decimal ClaimedAmount { get; set; }
    public string? Email { get; set; }

    /// <summary>Set when not a fresh claim: "AlreadyClaimed", "NothingToClaim", "TicketNotFound",
    /// "WalletFrozen", "InvalidEmail", "AccountUnavailable", "Failed".</summary>
    public string? FailureReason { get; set; }
}
