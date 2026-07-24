namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Which areas the Admin "factory reset" should wipe. Each deeper area implies the operational
/// layer (tickets/orders/wallets…) is cleared first, because tickets, orders and prepared-by
/// references RESTRICT-point into the sectors, seats, drinks, teams and users those areas remove.
/// The acting admin's own account is always kept so they can log back in.
/// </summary>
public class FactoryResetRequestDto
{
    /// <summary>Tickets, orders, payments, events, seasons, sessions, carts, reservations and the wallet ledger.</summary>
    public bool Operational { get; set; }

    /// <summary>Tribunes, rings, sectors, seats, drawing-tool overlays and the legacy stadium tables.</summary>
    public bool StadiumStructure { get; set; }

    /// <summary>Every account except the acting admin (staff, waiters, fans, the walk-up guest) plus their
    /// tokens, notifications, carts and brute-force state. The walk-up guest is re-created lazily on next use.</summary>
    public bool Users { get; set; }

    /// <summary>The drink catalog: drinks, categories and stock-movement ledger.</summary>
    public bool Catalog { get; set; }

    /// <summary>Team and club directories.</summary>
    public bool Directories { get; set; }

    /// <summary>Re-arm the first-run setup gate (Venue.SetupDismissed = false) so the admin is walked
    /// through rebuilding after the wipe.</summary>
    public bool ResetToFirstRun { get; set; }
}

/// <summary>
/// Per-area row counts removed by a factory reset. Returned so the UI can report exactly what was wiped
/// and confirm an admin account survived.
/// </summary>
public class FactoryResetResultDto
{
    public int Operational { get; set; }
    public int StadiumStructure { get; set; }
    public int Users { get; set; }
    public int Catalog { get; set; }
    public int Directories { get; set; }

    /// <summary>Grand total of every row removed across all selected areas.</summary>
    public int TotalRowsDeleted { get; set; }

    /// <summary>True once the run has verified at least one Admin account remains.</summary>
    public bool AdminPreserved { get; set; }

    /// <summary>Whether the first-run setup gate was re-armed.</summary>
    public bool ResetToFirstRun { get; set; }
}
