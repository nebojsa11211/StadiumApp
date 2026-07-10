using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Staff request to identify the fan standing at the bar counter before loading cash. <see cref="Query"/>
/// is whatever the bartender scanned or typed: a ticket QR token, a printed ticket number, a season pass
/// token, the fan's email, or their OIB. The server resolves it to the owning registered account.
/// </summary>
public class BarTopupResolveRequestDto
{
    [Required]
    [StringLength(200)]
    public string Query { get; set; } = string.Empty;
}

/// <summary>
/// Result of resolving a scanned/typed value to a fan account for a bar cash top-up. Only accounts are
/// eligible: a ticket that resolves to no registered user reports <see cref="Found"/>=true but
/// <see cref="HasAccount"/>=false so the bartender can tell the fan to create/sign in to an account first.
/// </summary>
public class BarTopupResolveResultDto
{
    /// <summary>True when the query matched a ticket, pass, or account at all.</summary>
    public bool Found { get; set; }

    /// <summary>True when the match resolves to a registered user account that can hold a wallet.</summary>
    public bool HasAccount { get; set; }

    public int? UserId { get; set; }

    /// <summary>Full display name for the bartender to confirm against the person at the counter.</summary>
    public string? FullName { get; set; }
    public string? Email { get; set; }

    /// <summary>Croatian OIB — the second identity check shown to staff. May be blank on older accounts.</summary>
    public string? Oib { get; set; }
    public string? PhoneNumber { get; set; }

    /// <summary>Current wallet balance (0 if no wallet exists yet — one is created on first top-up).</summary>
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "EUR";

    /// <summary>Wallet status (Active/Frozen/Closed). A frozen wallet cannot be topped up.</summary>
    public string WalletStatus { get; set; } = Models.WalletStatus.Active;
    public bool WalletExists { get; set; }

    /// <summary>How the fan was matched: "Ticket", "SeasonPass", "Email", or "Oib". For staff context.</summary>
    public string? MatchedBy { get; set; }

    /// <summary>Human-readable explanation shown when <see cref="HasAccount"/> is false.</summary>
    public string? Message { get; set; }
}

/// <summary>Staff request to credit cash onto a fan's wallet. <see cref="UserId"/> comes from a prior
/// resolve call. <see cref="IdempotencyKey"/> makes a re-submitted top-up a safe no-op.</summary>
public class BarTopupRequestDto
{
    [Required]
    public int UserId { get; set; }

    [Range(0.01, 100000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>Outcome of a staff cash top-up. On success the wallet is credited immediately.</summary>
public class BarTopupResultDto
{
    public bool Success { get; set; }
    public decimal NewBalance { get; set; }
    public long WalletTransactionId { get; set; }

    /// <summary>Set on failure: "WalletFrozen", "AccountNotFound", "InvalidAmount".</summary>
    public string? FailureReason { get; set; }
}
