using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Installation-wide reusable-cup configuration, surfaced on the Admin settings "Cups" tab and
/// persisted on the singleton <c>Venue</c> row. Reads are public so the Customer app can gate its cup
/// UI (like <see cref="AppSettingsDto"/>); writes require an Admin and are rejected unless
/// <see cref="IsValid"/>. See docs/reusable-cups-design.md.
/// </summary>
public class CupsSettingsDto
{
    /// <summary>Master switch for the whole reusable-cup feature.</summary>
    public bool CupsEnabled { get; set; }

    // ---- Modes ----
    public bool CupDepositModeEnabled { get; set; }
    public bool CupHonorModeEnabled { get; set; }
    public bool CupByocEnabled { get; set; }

    /// <summary>Refundable deposit charged per cup (deposit mode).</summary>
    [Range(0, 9999.99)]
    public decimal CupDepositAmount { get; set; } = 2.00m;

    // ---- Deposit binding (any combination) ----
    /// <summary>Deposit recorded against the ticket/wallet; refunded on scanning that ticket.</summary>
    public bool CupDepositBindTicketWallet { get; set; } = true;

    /// <summary>A printed return-token QR is the proof of deposit. Requires a receipt printer.</summary>
    public bool CupDepositBindReturnToken { get; set; }

    /// <summary>Deposit cups are individually QR'd and scanned in/out.</summary>
    public bool CupDepositBindCupQr { get; set; }

    // ---- Refund path ----
    /// <summary>Default: refund to the wallet (instant, stays in the ecosystem).</summary>
    public bool CupRefundToWallet { get; set; } = true;

    /// <summary>Alternate: refund to the original payment method (card async / cash).</summary>
    public bool CupRefundToOriginalMethod { get; set; }

    /// <summary>When an unreturned deposit forfeits to breakage.</summary>
    public CupRefundWindow CupRefundWindow { get; set; } = CupRefundWindow.EndOfEvent;

    // ---- BYOC ----
    /// <summary>Per-serving discount applied when a valid approved personal cup is scanned.</summary>
    [Range(0, 9999.99)]
    public decimal CupByocDiscountAmount { get; set; }

    /// <summary>Only registered/approved cups are accepted (hygiene + volume standardisation).</summary>
    public bool CupByocRequireApprovedCup { get; set; } = true;

    // ---- Invariants (enforced by the settings API and the UI) ----

    /// <summary>At least one cup mode is enabled.</summary>
    public bool HasAnyMode => CupDepositModeEnabled || CupHonorModeEnabled || CupByocEnabled;

    /// <summary>At least one deposit-binding combination is enabled.</summary>
    public bool HasAnyDepositBinding =>
        CupDepositBindTicketWallet || CupDepositBindReturnToken || CupDepositBindCupQr;

    /// <summary>At least one refund path is enabled.</summary>
    public bool HasAnyRefundPath => CupRefundToWallet || CupRefundToOriginalMethod;

    /// <summary>
    /// Whether the config is internally consistent enough to save: with cups off anything goes; with
    /// cups on at least one mode must be enabled, and if deposit mode is on it needs both a binding and
    /// a refund path (otherwise deposits could be taken but never returned).
    /// </summary>
    public bool IsValid =>
        !CupsEnabled ||
        (HasAnyMode && (!CupDepositModeEnabled || (HasAnyDepositBinding && HasAnyRefundPath)));
}
