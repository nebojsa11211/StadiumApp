namespace StadiumDrinkOrdering.API.Data;

/// <summary>
/// The "common" starter catalog an admin can generate from the Drinks page when the venue has no
/// (or only a partial) assortment yet. Kept as plain data so the seeding endpoint stays trivial.
/// Category names deliberately match the ones seeded by the model (Beer, SoftDrink, ...), so
/// generating the catalog on an existing database reuses those categories instead of duplicating them.
/// </summary>
public static class DefaultDrinkCatalog
{
    public record CatalogCategory(string Name, string DisplayName, string Icon, int SortOrder);

    public record CatalogDrink(string CategoryName, string Name, string Description, decimal Price, int StockQuantity);

    public static readonly IReadOnlyList<CatalogCategory> Categories = new[]
    {
        new CatalogCategory("Beer", "Pivo", "🍺", 1),
        new CatalogCategory("SoftDrink", "Gazirano", "🥤", 2),
        new CatalogCategory("Water", "Voda", "💧", 3),
        new CatalogCategory("Coffee", "Kava", "☕", 4),
        new CatalogCategory("Tea", "Čaj", "🍵", 5),
        new CatalogCategory("Juice", "Sok", "🧃", 6),
        new CatalogCategory("EnergyDrink", "Energetsko", "⚡", 7),
        new CatalogCategory("Cocktail", "Koktel", "🍸", 8),
    };

    public static readonly IReadOnlyList<CatalogDrink> Drinks = new[]
    {
        // Beer
        new CatalogDrink("Beer", "Ožujsko", "Draft lager, 0.5 L", 4.00m, 200),
        new CatalogDrink("Beer", "Karlovačko", "Draft lager, 0.5 L", 4.00m, 200),
        new CatalogDrink("Beer", "Heineken", "Bottled lager, 0.33 L", 4.50m, 150),
        new CatalogDrink("Beer", "Ožujsko Cool", "Non-alcoholic beer, 0.33 L", 3.50m, 100),

        // Soft drinks
        new CatalogDrink("SoftDrink", "Coca-Cola", "Chilled, 0.5 L", 3.00m, 240),
        new CatalogDrink("SoftDrink", "Coca-Cola Zero", "Sugar free, 0.5 L", 3.00m, 160),
        new CatalogDrink("SoftDrink", "Fanta", "Orange, 0.5 L", 3.00m, 160),
        new CatalogDrink("SoftDrink", "Sprite", "Lemon-lime, 0.5 L", 3.00m, 160),

        // Water
        new CatalogDrink("Water", "Jana", "Still water, 0.5 L", 2.00m, 300),
        new CatalogDrink("Water", "Jamnica", "Sparkling water, 0.5 L", 2.20m, 200),

        // Coffee
        new CatalogDrink("Coffee", "Espresso", "Single shot", 2.00m, 120),
        new CatalogDrink("Coffee", "Cappuccino", "Espresso with steamed milk", 2.50m, 120),
        new CatalogDrink("Coffee", "Iced Coffee", "Cold coffee with milk, 0.3 L", 3.00m, 80),

        // Tea
        new CatalogDrink("Tea", "Hot Tea", "Black or herbal, 0.25 L", 2.20m, 100),
        new CatalogDrink("Tea", "Iced Tea", "Lemon or peach, 0.5 L", 3.00m, 120),

        // Juice
        new CatalogDrink("Juice", "Orange Juice", "100% juice, 0.33 L", 3.20m, 120),
        new CatalogDrink("Juice", "Apple Juice", "100% juice, 0.33 L", 3.20m, 120),
        new CatalogDrink("Juice", "Lemonade", "Freshly squeezed, 0.4 L", 3.50m, 80),

        // Energy
        new CatalogDrink("EnergyDrink", "Red Bull", "Energy drink, 0.25 L", 4.00m, 120),
        new CatalogDrink("EnergyDrink", "Monster", "Energy drink, 0.5 L", 4.50m, 100),

        // Cocktails
        new CatalogDrink("Cocktail", "Aperol Spritz", "Aperol, prosecco, soda", 7.00m, 60),
        new CatalogDrink("Cocktail", "Gin & Tonic", "Gin with tonic water", 7.50m, 60),
        new CatalogDrink("Cocktail", "Mojito", "Rum, lime, mint, soda", 7.00m, 60),
    };

    /// <summary>
    /// Loose name key used to decide whether an item already exists: case-insensitive and ignoring
    /// spaces/hyphens, so "Coca-Cola" matches an existing "Coca Cola" instead of adding a near-duplicate.
    /// </summary>
    public static string NameKey(string name) =>
        new string(name.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
}
