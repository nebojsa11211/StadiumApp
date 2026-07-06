using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Customer.Components.Mobile;

/// <summary>Maps a drink category to its glyph, thumbnail style, and Croatian label.</summary>
public static class DrinkVisuals
{
    public static string Emoji(DrinkCategory c) => c switch
    {
        DrinkCategory.Beer => "🍺",
        DrinkCategory.SoftDrink => "🥤",
        DrinkCategory.Water => "💧",
        DrinkCategory.Coffee => "☕",
        DrinkCategory.Tea => "🍵",
        DrinkCategory.Juice => "🧃",
        DrinkCategory.EnergyDrink => "⚡",
        _ => "🥤"
    };

    public static string Thumb(DrinkCategory c) => c switch
    {
        DrinkCategory.Beer => "beer",
        DrinkCategory.SoftDrink => "soft",
        DrinkCategory.Water => "water",
        DrinkCategory.Coffee => "coffee",
        DrinkCategory.Tea => "coffee",
        DrinkCategory.Juice => "soft",
        DrinkCategory.EnergyDrink => "snack",
        _ => "soft"
    };

    public static string Label(DrinkCategory c) => c switch
    {
        DrinkCategory.Beer => "Pivo",
        DrinkCategory.SoftDrink => "Gazirano",
        DrinkCategory.Water => "Voda",
        DrinkCategory.Coffee => "Kava",
        DrinkCategory.Tea => "Čaj",
        DrinkCategory.Juice => "Sok",
        DrinkCategory.EnergyDrink => "Energetsko",
        _ => c.ToString()
    };
}

/// <summary>Payload emitted by the drink detail sheet when the fan confirms an add.</summary>
public record DrinkOrderRequest(int DrinkId, int Quantity, string? Note);
