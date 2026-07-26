using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// What a Bar staff member sees after scanning a ticket (or return token / cup QR) at the returns
/// station: who the deposits belong to and how many cups / how much money are outstanding. See
/// docs/reusable-cups-design.md.
/// </summary>
public class CupReturnLookupDto
{
    public bool Found { get; set; }
    public string? Error { get; set; }

    public int TicketId { get; set; }
    public string? TicketNumber { get; set; }
    public string? HolderName { get; set; }

    /// <summary>Number of cups still out on this ticket with a held deposit (refundable).</summary>
    public int OutstandingCups { get; set; }

    /// <summary>Total refundable deposit currently held for this ticket.</summary>
    public decimal OutstandingAmount { get; set; }

    public string Currency { get; set; } = "EUR";
}

/// <summary>Request to process a cup return at the Bar: refund up to <see cref="Count"/> held deposits
/// for the scanned ticket. Idempotent on <see cref="IdempotencyKey"/>.</summary>
public class CupReturnRequestDto
{
    /// <summary>Scanned/typed value that identifies the ticket (QR token, ticket number, etc.).</summary>
    [Required]
    public string Query { get; set; } = string.Empty;

    /// <summary>How many cups the customer is handing back.</summary>
    [Range(1, 100)]
    public int Count { get; set; } = 1;

    public string? IdempotencyKey { get; set; }
}

/// <summary>Outcome of a cup return: how many deposits were refunded, the total credited, and the
/// resulting wallet balance the customer can see.</summary>
public class CupReturnResultDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }

    public int RefundedCups { get; set; }
    public decimal RefundedAmount { get; set; }

    /// <summary>Balance of the wallet the refund was credited to, after the refund.</summary>
    public decimal WalletBalance { get; set; }
    public string Currency { get; set; } = "EUR";
}

/// <summary>Bulk return of honor-system (non-deposit) cups — just decrements the outstanding count for
/// shrinkage tracking; no money moves.</summary>
public class HonorCupReturnRequestDto
{
    [Range(1, 1000)]
    public int Count { get; set; } = 1;
}

/// <summary>Bind a scanned physical cup's QR to a held deposit at the bar (cup-QR binding), so the deposit
/// can later be returned by scanning the cup itself.</summary>
public class CupAssignRequestDto
{
    /// <summary>The ticket the cups were issued to (scanned/typed).</summary>
    [Required]
    public string Query { get; set; } = string.Empty;

    /// <summary>The scanned QR of the physical cup being handed over.</summary>
    [Required]
    public string CupQrToken { get; set; } = string.Empty;
}

/// <summary>Result of binding a cup QR to a held deposit.</summary>
public class CupAssignResultDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool Assigned { get; set; }
    /// <summary>Held deposits on this ticket still without a cup QR bound.</summary>
    public int RemainingUnassigned { get; set; }
    public string? HolderName { get; set; }
}

/// <summary>Reporting snapshot for the admin cup dashboard.</summary>
public class CupDashboardDto
{
    /// <summary>Cups currently in the wild (running sum of the cup ledger's deltas).</summary>
    public int OutstandingCups { get; set; }

    /// <summary>Money currently owed back as held deposits (the liability).</summary>
    public decimal OutstandingDepositLiability { get; set; }

    public int CupsIssued { get; set; }
    public int CupsReturned { get; set; }

    /// <summary>Cups issued that were returned, as a percentage (0–100).</summary>
    public double ReturnRatePercent { get; set; }

    public int DepositsRefundedCount { get; set; }
    public decimal DepositsRefundedAmount { get; set; }

    public int DepositsForfeitedCount { get; set; }
    public decimal DepositsForfeitedAmount { get; set; }

    /// <summary>Estimated cost of cups that never came back (shrinkage), valued at cup replacement cost.</summary>
    public decimal ShrinkageCost { get; set; }

    /// <summary>Bring-your-own-cup servings — single-use cups avoided (a sustainability metric).</summary>
    public int CupsSaved { get; set; }

    /// <summary>Total BYOC discount given to customers who brought their own cup.</summary>
    public decimal ByocDiscountGiven { get; set; }
}

/// <summary>Per-event cup figures for the admin dashboard breakdown.</summary>
public class CupEventReportDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    /// <summary>Held deposits (cups still out with money owed) for this event.</summary>
    public int DepositsHeld { get; set; }
    public decimal DepositLiability { get; set; }
    public int DepositsRefunded { get; set; }
    public int DepositsForfeited { get; set; }
    /// <summary>BYOC servings for this event (single-use cups avoided).</summary>
    public int CupsSaved { get; set; }
}

/// <summary>A reusable cup design (model) — the catalog entry a registered cup points at.</summary>
public class CupTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int VolumeMl { get; set; }
    public decimal UnitCost { get; set; }
}

// ---- Registered (BYOC / premium) cups --------------------------------------------------------

/// <summary>Admin view of a registered personal/premium cup.</summary>
public class RegisteredCupDto
{
    public int Id { get; set; }
    public string QrToken { get; set; } = string.Empty;
    public int? CupTypeId { get; set; }
    public string? CupTypeName { get; set; }
    public int? VolumeMl { get; set; }
    public bool IsApproved { get; set; }
    /// <summary>Active / Retired / Lost.</summary>
    public string Status { get; set; } = "Active";
    /// <summary>Owner label (fan email or ticket number), or null for unassigned club stock.</summary>
    public string? OwnerLabel { get; set; }
    public DateTime RegisteredAt { get; set; }
}

/// <summary>Create or update a registered cup (admin). On create, <see cref="QrToken"/> is required and
/// must be unique.</summary>
public class RegisteredCupUpsertDto
{
    [Required]
    [StringLength(200)]
    public string QrToken { get; set; } = string.Empty;

    /// <summary>Approved cup model (defines the standard serving volume). Optional.</summary>
    public int? CupTypeId { get; set; }

    public bool IsApproved { get; set; } = true;

    /// <summary>Active / Retired / Lost. Defaults to Active.</summary>
    public string Status { get; set; } = "Active";
}

/// <summary>Paged list of registered cups.</summary>
public class RegisteredCupListDto
{
    public List<RegisteredCupDto> Cups { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

/// <summary>Result of resolving a scanned cup QR — what the customer app shows before adding a BYOC drink.</summary>
public class CupResolveDto
{
    public bool Found { get; set; }
    /// <summary>True when the cup may be used for a discounted BYOC serving right now.</summary>
    public bool Usable { get; set; }
    public string? CupTypeName { get; set; }
    public int? VolumeMl { get; set; }
    /// <summary>The per-serving discount the customer would get (venue config).</summary>
    public decimal Discount { get; set; }
    /// <summary>Why the cup can't be used, when <see cref="Usable"/> is false.</summary>
    public string? Reason { get; set; }
}
