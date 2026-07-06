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
}
