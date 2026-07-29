namespace StadiumDrinkOrdering.Runner.Models;

/// <summary>
/// Client-side mirror of <c>StadiumDrinkOrdering.Shared.Models.OrderSlipCode</c> — the format of the
/// QR printed on a bar slip. Duplicated rather than referenced because the Runner deliberately does
/// not reference Shared (see the note in the .csproj). Keep the two in sync; the format is frozen by
/// every slip already printed, so it should not change.
///
/// Decoding happens entirely here on the device: a scan resolves to an order id with no network
/// round-trip, which is what lets a runner scan a stack of slips back-to-back at bar speed.
/// </summary>
public static class OrderSlipCode
{
    public const string Prefix = "SDO-ORDER:";

    /// <summary>
    /// Reads an order id out of a scanned or typed code. Accepts the canonical <c>SDO-ORDER:123</c>
    /// payload (case-insensitively) plus the bare <c>#123</c> / <c>123</c> forms. A ticket QR (a
    /// <c>https://…/t/{token}</c> deep link) will not parse, so the caller can tell the runner they
    /// scanned the wrong thing.
    /// </summary>
    public static bool TryParse(string? scanned, out int orderId)
    {
        orderId = 0;
        if (string.IsNullOrWhiteSpace(scanned))
            return false;

        var value = scanned.Trim();

        if (value.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            value = value[Prefix.Length..].Trim();

        value = value.TrimStart('#').Trim();

        return int.TryParse(value, out orderId) && orderId > 0;
    }
}
