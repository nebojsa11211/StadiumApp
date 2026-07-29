using System;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// The single authoritative definition of what a printed bar slip's QR code contains, so the bar
/// (which prints it) and the Runner (which scans it) can never drift apart.
///
/// The payload is deliberately a short opaque-looking literal rather than a URL: it is decoded
/// entirely on the runner's device with no server round-trip, which is what makes rapid multi-scan
/// at the bar feel instant. It carries only the order id — the same number already printed in plain
/// text on the bar board and on the slip itself — so it reveals nothing a person holding the slip
/// can't already read. Claiming still goes through the authenticated pool/claim endpoints.
/// </summary>
public static class OrderSlipCode
{
    /// <summary>Marker that distinguishes an order slip from any other QR a runner might scan
    /// (ticket QRs are <c>https://…/t/{token}</c> deep links and will not parse here).</summary>
    public const string Prefix = "SDO-ORDER:";

    /// <summary>The exact string to encode into a slip's QR code.</summary>
    public static string For(int orderId) => Prefix + orderId.ToString();

    /// <summary>
    /// Reads an order id out of a scanned or typed code. Accepts the canonical
    /// <c>SDO-ORDER:123</c> payload (case-insensitively), plus the bare <c>#123</c> / <c>123</c>
    /// forms a runner may type by hand when a slip is smudged. Anything else — including a ticket
    /// deep link — is rejected so the caller can say "that isn't an order slip".
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
