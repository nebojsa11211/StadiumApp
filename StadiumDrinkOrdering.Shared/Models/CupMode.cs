namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// How a drink line is served with respect to reusable cups. Stored as an int on
/// <see cref="OrderItem"/>. <see cref="None"/> is the default (disposable / no cup handling).
/// See docs/reusable-cups-design.md.
/// </summary>
public enum CupMode
{
    /// <summary>Disposable cup / no reusable-cup handling. Default.</summary>
    None = 0,

    /// <summary>Reusable club cup rented against a refundable deposit.</summary>
    Deposit = 1,

    /// <summary>Reusable club cup handed out on trust (no money held); tracked for shrinkage.</summary>
    HonorSystem = 2,

    /// <summary>Customer's own personal cup, identified by a scannable QR (must be approved).</summary>
    ByocQr = 3
}

/// <summary>
/// When an unreturned cup deposit stops being refundable and is recognised as breakage revenue.
/// Configured per venue.
/// </summary>
public enum CupRefundWindow
{
    /// <summary>Deposit is refundable until the event the order belongs to ends.</summary>
    EndOfEvent = 1,

    /// <summary>Deposit is refundable until 24h after the event ends (accommodates late returns).</summary>
    EndOfEventPlus24h = 2,

    /// <summary>Deposit never forfeits automatically (carries across events, e.g. into the wallet).</summary>
    NoExpiry = 3
}
