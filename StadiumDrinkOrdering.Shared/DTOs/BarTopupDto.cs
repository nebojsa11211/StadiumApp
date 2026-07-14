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

    // ---- Anonymous ticket wallet (no-account bearer balance loaded on the ticket itself) ----

    /// <summary>The matched ticket's id, set when a concrete ticket was scanned/typed. Required to load
    /// or cash out an anonymous ticket wallet.</summary>
    public int? TicketId { get; set; }

    /// <summary>True when this match can use an anonymous ticket wallet — i.e. a concrete ticket was
    /// matched but it has no linked account, so funds can be loaded on the ticket instead. When true,
    /// <see cref="Balance"/>/<see cref="WalletStatus"/>/<see cref="WalletExists"/> describe that ticket
    /// wallet (a Closed status means it was already cashed out and cannot be topped up again).</summary>
    public bool AllowTicketWallet { get; set; }

    /// <summary>Ticket the wallet is bound to (number the bartender can read back to the guest).</summary>
    public string? TicketNumber { get; set; }

    /// <summary>Per-ticket balance cap, for the UI to show/enforce the top-up limit.</summary>
    public decimal TicketWalletMaxBalance { get; set; }

    // ---- Provision-an-account (OIB-identified ticket with no account yet) ----

    /// <summary>True when the matched ticket carries a real identity (name + OIB) but no account exists
    /// yet: the bartender can create one on the spot by entering an email, so cash lands on the fan's own
    /// (claimable) wallet rather than a bearer ticket balance. <see cref="FullName"/>/<see cref="Oib"/>
    /// carry the ticket's identity to confirm, and <see cref="TicketId"/> the ticket to provision from.
    /// Usually accompanies <see cref="AllowTicketWallet"/> — loading on the ticket stays the fallback.</summary>
    public bool CanProvisionAccount { get; set; }
}

/// <summary>Staff request to create (or reuse) a claimable account for an OIB-identified ticket that has
/// no account yet, so cash can be loaded onto the fan's own wallet. The name/OIB come from the ticket;
/// only the email — which the account is keyed on — is collected at the counter.</summary>
public class BarTopupProvisionRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
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

/// <summary>
/// One row of the bar-counter cash history: a top-up onto an account wallet, a load onto an anonymous
/// ticket wallet, or a ticket cash-out. Carries who it was for (account holder or ticket) and which
/// staff member performed it, for the bar's own history / cash-drawer reconciliation view.
/// </summary>
public class BarTopupHistoryItemDto
{
    public long TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>Underlying ledger reference: "CashTopup", "TicketTopup", or "TicketCashOut".</summary>
    public string? ReferenceType { get; set; }

    /// <summary>Signed amount — credits (top-ups) positive, cash handed back (cash-out) negative.</summary>
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Currency { get; set; } = "EUR";

    /// <summary>"User" for an account wallet, "Ticket" for an anonymous bearer balance.</summary>
    public string OwnerType { get; set; } = "User";

    /// <summary>Display name of the account holder, or the ticket's holder name for a bearer wallet.</summary>
    public string? CustomerName { get; set; }
    public string? Email { get; set; }

    /// <summary>Set for a ticket-owned wallet — the number the bartender can read back.</summary>
    public string? TicketNumber { get; set; }

    /// <summary>Event the movement relates to, resolved via the ticket for ticket/cash-out rows. Null for
    /// account cash top-ups, which aren't tied to a specific ticket (and so no event).</summary>
    public string? EventName { get; set; }

    /// <summary>Date of <see cref="EventName"/>, when known.</summary>
    public DateTime? EventDate { get; set; }

    /// <summary>Staff member who performed it (for cash-drawer reconciliation).</summary>
    public int? StaffUserId { get; set; }
    public string? StaffEmail { get; set; }
}

/// <summary>Paginated bar-counter cash history plus running totals across the FULL filtered set.</summary>
public class BarTopupHistoryListDto
{
    public List<BarTopupHistoryItemDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    /// <summary>Total cash taken in (sum of positive amounts) across the whole filtered set.</summary>
    public decimal TotalIn { get; set; }

    /// <summary>Total cash handed back (sum of cash-outs, as a positive figure) across the whole set.</summary>
    public decimal TotalOut { get; set; }
}
