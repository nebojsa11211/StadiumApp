using QRCoder;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Bar.Services;

/// <summary>
/// Builds the QR image printed on a bar slip, encoding the canonical
/// <see cref="OrderSlipCode"/> payload the Runner's scanner expects.
///
/// Deliberately uncached: a slip is generated only when the bartender actually prints one, and
/// encoding a dozen-character string takes about a millisecond — far cheaper than holding every
/// order's PNG in memory for the lifetime of a long-running match-day server process.
/// </summary>
public static class OrderSlipQr
{
    /// <summary>
    /// A data-URI PNG of the slip QR for <paramref name="orderId"/>. Module size is tuned for print:
    /// the slip CSS scales it to roughly 28 mm, comfortably above the ~20 mm floor a phone camera
    /// needs to lock focus at arm's length.
    /// </summary>
    public static string DataUri(int orderId)
    {
        using var generator = new QRCodeGenerator();
        // ECC level Q (~25% recovery) so a scan still succeeds through the beer splashes, thumb
        // smudges and creases a slip picks up sitting on a tray at a bar.
        using var data = generator.CreateQrCode(OrderSlipCode.For(orderId), QRCodeGenerator.ECCLevel.Q);
        using var png = new PngByteQRCode(data);
        return "data:image/png;base64," + Convert.ToBase64String(png.GetGraphic(10));
    }
}
