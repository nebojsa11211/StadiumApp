namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Thrown when a plain event delete is refused because some of the event's tickets carry anonymous
/// bearer wallets holding a stored-value balance. <c>Wallet.TicketId</c> is configured RESTRICT
/// precisely so a funded ticket cannot vanish underneath its money; without this guard the delete
/// reached the database and failed with an opaque foreign-key error that read like a bug.
///
/// The plain delete deliberately preserves money records (it keeps orders and payments, merely
/// unlinking them), so destroying a fan's balance here would contradict its contract. Wiping the
/// wallets is the purge's job — see <see cref="IEventService.PurgeEventAsync"/>. Carries a
/// user-facing message that controllers surface verbatim as a 409.
/// </summary>
public class TicketWalletsBlockDeleteException : Exception
{
    /// <summary>How many ticket wallets stand in the way.</summary>
    public int WalletCount { get; }

    /// <summary>Total balance still held on those wallets.</summary>
    public decimal TotalBalance { get; }

    public TicketWalletsBlockDeleteException(int walletCount, decimal totalBalance)
        // The surrounding text is hard-coded English (this message is not localised), so format the
        // amount invariantly too — under the hr request culture "N2" would render €50,00 mid-sentence.
        : base($"{walletCount} ticket wallet(s) attached to this event still hold " +
               $"€{totalBalance.ToString("N2", System.Globalization.CultureInfo.InvariantCulture)}. " +
               "Cash them out first, or use \"Delete everything\" to remove the event together with " +
               "its wallets, orders and payments. Nothing was deleted.")
    {
        WalletCount = walletCount;
        TotalBalance = totalBalance;
    }
}
