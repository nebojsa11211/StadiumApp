namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Installation-wide settings surfaced on the Admin settings page. Reads are public so the
/// Customer app can check <see cref="TicketSalesEnabled"/> to show/hide its buy flow; writes
/// require an Admin. Persisted on the singleton <c>Venue</c> row.
/// </summary>
public class AppSettingsDto
{
    /// <summary>
    /// Master switch: does this system sell tickets directly to customers? When false the
    /// Customer buy flow is hidden and the cart/order API rejects new purchases.
    /// </summary>
    public bool TicketSalesEnabled { get; set; } = true;

    /// <summary>Wallet is offered as a drink payment method in the Customer app.</summary>
    public bool WalletPaymentEnabled { get; set; } = true;

    /// <summary>Card is offered as a drink payment method in the Customer app.</summary>
    public bool CardPaymentEnabled { get; set; } = true;

    /// <summary>Cash on delivery is offered as a drink payment method in the Customer app.</summary>
    public bool CashPaymentEnabled { get; set; } = true;

    /// <summary>True when at least one payment method is enabled — the invariant the settings API enforces.</summary>
    public bool HasAnyPaymentMethod => WalletPaymentEnabled || CardPaymentEnabled || CashPaymentEnabled;
}
