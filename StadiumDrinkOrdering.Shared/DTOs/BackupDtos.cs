using System.Text.Json;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// One selectable table in the Admin backup/restore UI. Mirrors an EF DbSet.
/// </summary>
public class BackupTableInfoDto
{
    /// <summary>Stable logical key (matches the physical table name), e.g. "Events".</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>UI grouping bucket, e.g. "Domain data", "Users &amp; wallets", "Security", "Logs".</summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>Human-friendly label shown next to the checkbox.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Live row count (for the "tables" listing) or count found in the file (for a preview).</summary>
    public int RowCount { get; set; }

    /// <summary>Whether this table is checked by default in the backup picker.</summary>
    public bool DefaultSelected { get; set; }

    /// <summary>True when the table holds sensitive data (password hashes, tokens, wallet balances).</summary>
    public bool Sensitive { get; set; }
}

/// <summary>Metadata block written at the top of every backup file.</summary>
public class BackupMetadataDto
{
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public string FormatVersion { get; set; } = "1.0";
    public List<string> Tables { get; set; } = new();
}

/// <summary>
/// The on-disk backup document: metadata plus, per table, an array of rows. Each row is an object of
/// {columnName: value} using the physical column names, so it round-trips through raw SQL faithfully.
/// </summary>
public class BackupFileDto
{
    public BackupMetadataDto Meta { get; set; } = new();
    public Dictionary<string, List<Dictionary<string, JsonElement>>> Tables { get; set; } = new();
}

/// <summary>Result of inspecting an uploaded backup file before applying it.</summary>
public class BackupPreviewDto
{
    public BackupMetadataDto Meta { get; set; } = new();

    /// <summary>Tables present in the file, with their row counts and grouping (only known tables).</summary>
    public List<BackupTableInfoDto> Tables { get; set; } = new();

    /// <summary>Table keys found in the file that this application no longer knows about.</summary>
    public List<string> UnknownTables { get; set; } = new();
}

/// <summary>Per-table outcome of a restore.</summary>
public class RestoreTableResultDto
{
    public string Key { get; set; } = string.Empty;
    public int RowsDeleted { get; set; }
    public int RowsInserted { get; set; }
}

/// <summary>Overall outcome of a restore operation.</summary>
public class RestoreResultDto
{
    public List<RestoreTableResultDto> Tables { get; set; } = new();
    public int TotalRowsInserted { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}
