namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Row counts removed by the Admin "delete all transactional data" maintenance action
/// (tickets, orders, events, seasons and the whole wallet ledger). Returned so the UI can
/// report exactly what was wiped.
/// </summary>
public class PurgeDataResultDto
{
    public int Tickets { get; set; }
    public int Orders { get; set; }
    public int Events { get; set; }
    public int Seasons { get; set; }
    public int WalletTransactions { get; set; }
    public int Wallets { get; set; }

    /// <summary>Grand total of every row removed, including cascaded child records.</summary>
    public int TotalRowsDeleted { get; set; }
}
