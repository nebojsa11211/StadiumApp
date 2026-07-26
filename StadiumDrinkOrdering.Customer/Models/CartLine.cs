using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Customer.Models;

/// <summary>
/// One line of the drink cart as stored in <c>OrderSession.CartData</c> (JSON). Mirrors the shape
/// the API serialises so we can round-trip the server-side session cart.
/// </summary>
public class CartLine
{
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SpecialInstructions { get; set; }

    /// <summary>How this line is served w.r.t. reusable cups (None/Deposit/Honor/BYOC). The deposit/discount
    /// amount is applied server-side from venue config at checkout.</summary>
    public CupMode CupMode { get; set; } = CupMode.None;

    /// <summary>For BYOC lines: the scanned/entered QR of the customer's own cup.</summary>
    public string? CupQrToken { get; set; }
}
