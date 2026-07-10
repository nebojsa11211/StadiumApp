namespace StadiumDrinkOrdering.Customer.Components.Mobile;

/// <summary>
/// Maps a drink's (now data-driven) category to its glyph, thumbnail style, and label.
/// Icon and label come from the category record; the thumbnail style is a best-effort
/// mapping of the canonical category name to one of the built-in CSS looks, with a
/// generic fallback for custom categories.
/// </summary>
public static class DrinkVisuals
{
    public static string Emoji(string? icon) =>
        string.IsNullOrWhiteSpace(icon) ? "🥤" : icon!;

    public static string Thumb(string? categoryName) => (categoryName ?? "").ToLowerInvariant() switch
    {
        "beer" => "beer",
        "softdrink" => "soft",
        "water" => "water",
        "coffee" => "coffee",
        "tea" => "coffee",
        "juice" => "soft",
        "energydrink" => "snack",
        _ => "soft"
    };

    public static string Label(string? displayName, string? name) =>
        !string.IsNullOrWhiteSpace(displayName) ? displayName! : (name ?? "");
}

/// <summary>Payload emitted by the drink detail sheet when the fan confirms an add.</summary>
public record DrinkOrderRequest(int DrinkId, int Quantity, string? Note);
