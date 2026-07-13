using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Logical (row-level) backup/restore of the PostgreSQL database, driven entirely from the EF model
/// so it works against remote Supabase without any external tooling (pg_dump etc.).
///
/// A backup is a single JSON document: metadata + per-table arrays of {column: value} rows read via
/// raw <c>SELECT *</c> (native column names/types, so it is schema-driven and value-converter agnostic;
/// the <c>xmin</c> concurrency column is a system column and is naturally excluded).
///
/// Restore is "replace": for each selected table it DELETEs existing rows then re-inserts the backup
/// rows with their original primary keys, all inside ONE transaction (all-or-nothing). Tables are
/// inserted parents-first and deleted children-first. The single circular/forward nullable foreign key
/// in the schema (e.g. Order.PaymentId → Payment, which is inserted after Orders) is handled generically
/// by inserting NULL first and issuing a fix-up UPDATE once every table is in place. Identity sequences
/// are advanced to MAX(id) after each table so future inserts don't collide.
/// </summary>
public class DatabaseBackupService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseBackupService> _logger;

    public DatabaseBackupService(ApplicationDbContext context, ILogger<DatabaseBackupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private sealed record TableDescriptor(string Key, string Group, string DisplayName, Type ClrType, bool DefaultSelected, bool Sensitive);

    // Group labels are resource KEYS resolved in the Admin UI; kept as stable identifiers here.
    private const string GroupDomain = "Domain";
    private const string GroupUsers = "UsersWallets";
    private const string GroupSecurity = "Security";
    private const string GroupLogs = "Logs";

    /// <summary>
    /// Every backup-able table in PARENTS-FIRST insert order. Non-nullable foreign keys always point to
    /// a table earlier in this list; nullable forward/cyclic references are deferred at restore time.
    /// Order is authoritative — do not reorder without checking foreign-key direction.
    /// </summary>
    private static readonly TableDescriptor[] Tables =
    {
        new("Users", GroupUsers, "Users", typeof(Shared.Models.User), true, true),
        new("Categories", GroupDomain, "Drink categories", typeof(Shared.Models.Category), true, false),
        new("Drinks", GroupDomain, "Drinks", typeof(Shared.Models.Drink), true, false),
        new("StockMovements", GroupDomain, "Stock movements", typeof(Shared.Models.StockMovement), true, false),
        new("Venues", GroupDomain, "Venue / branding", typeof(Shared.Models.Venue), true, false),
        new("Clubs", GroupDomain, "Clubs", typeof(Shared.Models.Club), true, false),
        new("StadiumSections", GroupDomain, "Stadium sections (seed)", typeof(Shared.Models.StadiumSection), true, false),
        new("Seats", GroupDomain, "Seats (seed)", typeof(Shared.Models.Seat), true, false),
        new("StadiumSeats", GroupDomain, "Stadium seats (legacy)", typeof(Shared.Models.StadiumSeat), true, false),
        new("Tribunes", GroupDomain, "Tribunes", typeof(Shared.Models.Tribune), true, false),
        new("Rings", GroupDomain, "Rings", typeof(Shared.Models.Ring), true, false),
        new("Sectors", GroupDomain, "Sectors", typeof(Shared.Models.Sector), true, false),
        new("StadiumSectorOverlays", GroupDomain, "Stadium sector overlays", typeof(Shared.Models.StadiumSectorOverlay), true, false),
        new("StadiumSeatsNew", GroupDomain, "Stadium seats (structure)", typeof(Shared.Models.StadiumSeatNew), true, false),
        new("Seasons", GroupDomain, "Seasons", typeof(Shared.Models.Season), true, false),
        new("Events", GroupDomain, "Events", typeof(Shared.Models.Event), true, false),
        new("SeasonTickets", GroupDomain, "Season tickets", typeof(Shared.Models.SeasonTicket), true, false),
        new("EventSectorPrices", GroupDomain, "Per-event sector prices", typeof(Shared.Models.EventSectorPrice), true, false),
        new("EventAnalytics", GroupDomain, "Event analytics", typeof(Shared.Models.EventAnalytics), true, false),
        new("EventStaffAssignments", GroupDomain, "Event staff assignments", typeof(Shared.Models.EventStaffAssignment), true, false),
        new("Tickets", GroupDomain, "Tickets", typeof(Shared.Models.Ticket), true, false),
        new("TicketSessions", GroupDomain, "Ticket sessions", typeof(Shared.Models.TicketSession), true, false),
        new("OrderSessions", GroupDomain, "Order sessions", typeof(Shared.Models.OrderSession), true, false),
        new("Wallets", GroupUsers, "Wallets", typeof(Shared.Models.Wallet), true, true),
        new("WalletTransactions", GroupUsers, "Wallet transactions", typeof(Shared.Models.WalletTransaction), true, true),
        new("Orders", GroupDomain, "Orders", typeof(Shared.Models.Order), true, false),
        new("Payments", GroupDomain, "Payments", typeof(Shared.Models.Payment), true, false),
        new("OrderItems", GroupDomain, "Order items", typeof(Shared.Models.OrderItem), true, false),
        new("ShoppingCarts", GroupDomain, "Shopping carts", typeof(Shared.Models.ShoppingCart), true, false),
        new("CartItems", GroupDomain, "Cart items", typeof(Shared.Models.CartItem), true, false),
        new("SeatReservations", GroupDomain, "Seat reservations", typeof(Shared.Models.SeatReservation), true, false),
        new("Notifications", GroupDomain, "Notifications", typeof(Shared.Models.Notification), true, false),
        new("IntegrationInboxEntries", GroupDomain, "Integration inbox", typeof(Shared.Models.IntegrationInboxEntry), true, false),
        new("EmailTemplates", GroupDomain, "Email templates", typeof(Shared.Models.EmailTemplate), true, false),
        new("RefreshTokens", GroupSecurity, "Refresh tokens", typeof(Shared.Models.RefreshToken), false, true),
        new("AccountActivationTokens", GroupSecurity, "Account activation tokens", typeof(Shared.Models.AccountActivationToken), false, true),
        new("FailedAttempts", GroupSecurity, "Failed login attempts", typeof(Models.FailedAttempt), false, true),
        new("AccountLockouts", GroupSecurity, "Account lockouts", typeof(Models.AccountLockout), false, true),
        new("IPBans", GroupSecurity, "IP bans", typeof(Models.IPBan), false, true),
        new("LogEntries", GroupLogs, "Application logs", typeof(Shared.Models.LogEntry), false, false),
    };

    private static readonly Dictionary<string, TableDescriptor> ByKey =
        Tables.ToDictionary(t => t.Key, StringComparer.OrdinalIgnoreCase);

    // ---- Physical (SQL) metadata resolved from the EF model ----------------------------------

    private string QualifiedTable(Type clrType)
    {
        var et = _context.Model.FindEntityType(clrType)
                 ?? throw new InvalidOperationException($"No EF entity type for {clrType.Name}");
        var table = et.GetTableName() ?? clrType.Name;
        var schema = et.GetSchema();
        return schema is null ? Quote(table) : $"{Quote(schema)}.{Quote(table)}";
    }

    private (string? Column, Type? ClrType) IntPrimaryKey(Type clrType)
    {
        var et = _context.Model.FindEntityType(clrType);
        var pk = et?.FindPrimaryKey();
        if (pk is null || pk.Properties.Count != 1) return (null, null);
        var p = pk.Properties[0];
        var t = Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType;
        if (t != typeof(int) && t != typeof(long) && t != typeof(short)) return (null, null);
        return (p.GetColumnName(), t);
    }

    private static string Quote(string identifier) => "\"" + identifier.Replace("\"", "\"\"") + "\"";

    // ---- Public API ---------------------------------------------------------------------------

    /// <summary>Lists every backup-able table with its live row count.</summary>
    public async Task<List<BackupTableInfoDto>> GetTablesAsync(CancellationToken ct = default)
    {
        var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
        var wasClosed = conn.State != System.Data.ConnectionState.Open;
        if (wasClosed) await conn.OpenAsync(ct);
        try
        {
            var result = new List<BackupTableInfoDto>();
            foreach (var t in Tables)
            {
                int count;
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT COUNT(*) FROM {QualifiedTable(t.ClrType)}";
                    count = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
                }
                result.Add(new BackupTableInfoDto
                {
                    Key = t.Key,
                    Group = t.Group,
                    DisplayName = t.DisplayName,
                    RowCount = count,
                    DefaultSelected = t.DefaultSelected,
                    Sensitive = t.Sensitive,
                });
            }
            return result;
        }
        finally
        {
            if (wasClosed) await conn.CloseAsync();
        }
    }

    /// <summary>Serializes the selected tables (or all, if none specified) into a backup JSON document.</summary>
    public async Task<byte[]> ExportAsync(IEnumerable<string>? selectedKeys, string? createdBy, CancellationToken ct = default)
    {
        var selected = ResolveSelected(selectedKeys);

        var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
        var wasClosed = conn.State != System.Data.ConnectionState.Open;
        if (wasClosed) await conn.OpenAsync(ct);
        try
        {
            var file = new BackupFileDto
            {
                Meta = new BackupMetadataDto
                {
                    CreatedUtc = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    Tables = selected.Select(t => t.Key).ToList(),
                }
            };

            foreach (var t in selected)
            {
                var rows = new List<Dictionary<string, JsonElement>>();
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT * FROM {QualifiedTable(t.ClrType)}";
                await using var reader = await cmd.ExecuteReaderAsync(ct);
                while (await reader.ReadAsync(ct))
                {
                    var row = new Dictionary<string, JsonElement>(reader.FieldCount);
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var value = await reader.IsDBNullAsync(i, ct) ? null : reader.GetValue(i);
                        // Round-trip through the serializer so every value becomes a JsonElement of the
                        // right shape (DateTime->ISO string, byte[]->base64, decimal->number, etc.).
                        row[reader.GetName(i)] = JsonSerializer.SerializeToElement(value);
                    }
                    rows.Add(row);
                }
                file.Tables[t.Key] = rows;
            }

            return JsonSerializer.SerializeToUtf8Bytes(file, new JsonSerializerOptions { WriteIndented = false });
        }
        finally
        {
            if (wasClosed) await conn.CloseAsync();
        }
    }

    /// <summary>Parses a backup file and reports which known tables it contains, without applying it.</summary>
    public BackupPreviewDto Preview(BackupFileDto file)
    {
        var preview = new BackupPreviewDto { Meta = file.Meta };
        foreach (var (key, rows) in file.Tables)
        {
            if (ByKey.TryGetValue(key, out var desc))
            {
                preview.Tables.Add(new BackupTableInfoDto
                {
                    Key = desc.Key,
                    Group = desc.Group,
                    DisplayName = desc.DisplayName,
                    RowCount = rows.Count,
                    DefaultSelected = true,
                    Sensitive = desc.Sensitive,
                });
            }
            else
            {
                preview.UnknownTables.Add(key);
            }
        }
        // Present tables in canonical (parents-first) order.
        preview.Tables = preview.Tables
            .OrderBy(t => Array.FindIndex(Tables, d => d.Key == t.Key))
            .ToList();
        return preview;
    }

    /// <summary>
    /// Replaces the selected tables with the rows from the backup file, transactionally (all-or-nothing).
    /// Only tables that are BOTH selected AND present in the file are touched.
    /// </summary>
    public async Task<RestoreResultDto> RestoreAsync(BackupFileDto file, IEnumerable<string> selectedKeys, CancellationToken ct = default)
    {
        var selectedSet = new HashSet<string>(selectedKeys ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        // Canonical parents-first order, limited to tables selected AND present in the file.
        var ordered = Tables
            .Where(t => selectedSet.Contains(t.Key) && file.Tables.ContainsKey(t.Key))
            .ToList();

        var result = new RestoreResultDto();
        if (ordered.Count == 0)
        {
            result.Success = true;
            return result;
        }

        var orderIndex = ordered.Select((t, i) => (t.Key, i))
            .ToDictionary(x => x.Key, x => x.i, StringComparer.OrdinalIgnoreCase);

        var strategy = _context.Database.CreateExecutionStrategy();
        try
        {
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _context.Database.BeginTransactionAsync(ct);
                var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
                var dbTx = (NpgsqlTransaction)tx.GetDbTransaction();

                var perTable = ordered.ToDictionary(t => t.Key, t => new RestoreTableResultDto { Key = t.Key }, StringComparer.OrdinalIgnoreCase);
                var fixups = new List<FixUp>();

                // 1. Delete existing rows, children-first (reverse of insert order).
                foreach (var t in Enumerable.Reverse(ordered))
                {
                    await using var del = conn.CreateCommand();
                    del.Transaction = dbTx;
                    del.CommandText = $"DELETE FROM {QualifiedTable(t.ClrType)}";
                    perTable[t.Key].RowsDeleted = await del.ExecuteNonQueryAsync(ct);
                }

                // 2. Insert rows, parents-first.
                foreach (var t in ordered)
                {
                    var liveColumns = await GetLiveColumnsAsync(conn, dbTx, QualifiedTable(t.ClrType), ct);
                    var deferred = ComputeDeferredColumns(t.ClrType, selectedSet, orderIndex[t.Key], orderIndex);
                    var (pkColumn, _) = IntPrimaryKey(t.ClrType);

                    var rows = file.Tables[t.Key];
                    var inserted = 0;
                    foreach (var row in rows)
                    {
                        inserted += await InsertRowAsync(conn, dbTx, QualifiedTable(t.ClrType), t.Key,
                            liveColumns, row, deferred, pkColumn, fixups, ct);
                    }
                    perTable[t.Key].RowsInserted = inserted;
                }

                // 3. Apply deferred foreign-key fix-ups now that every table is populated.
                foreach (var f in fixups)
                {
                    await using var upd = conn.CreateCommand();
                    upd.Transaction = dbTx;
                    var sets = new List<string>();
                    var pi = 0;
                    foreach (var (col, val) in f.Columns)
                    {
                        var pn = $"@p{pi++}";
                        sets.Add($"{Quote(col)} = {pn}");
                        upd.Parameters.Add(new NpgsqlParameter(pn, val ?? DBNull.Value));
                    }
                    var pkParam = $"@p{pi}";
                    upd.Parameters.Add(new NpgsqlParameter(pkParam, f.PkValue ?? DBNull.Value));
                    upd.CommandText = $"UPDATE {f.QualifiedTable} SET {string.Join(", ", sets)} WHERE {Quote(f.PkColumn)} = {pkParam}";
                    await upd.ExecuteNonQueryAsync(ct);
                }

                // 4. Advance identity sequences past the restored primary keys.
                foreach (var t in ordered)
                {
                    var (pkColumn, _) = IntPrimaryKey(t.ClrType);
                    if (pkColumn is null) continue;
                    await ResetSequenceAsync(conn, dbTx, QualifiedTable(t.ClrType), pkColumn, ct);
                }

                await tx.CommitAsync(ct);

                foreach (var t in ordered)
                {
                    result.Tables.Add(perTable[t.Key]);
                    result.TotalRowsInserted += perTable[t.Key].RowsInserted;
                }
            });

            result.Success = true;
            _logger.LogWarning("Admin database restore replaced {TableCount} tables, inserting {Rows} rows total.",
                result.Tables.Count, result.TotalRowsInserted);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Admin database restore failed and was rolled back.");
            result.Success = false;
            result.Tables.Clear();
            result.TotalRowsInserted = 0;
            result.Error = ex.Message;
            return result;
        }
    }

    // ---- Restore internals --------------------------------------------------------------------

    private sealed record FixUp(string QualifiedTable, string PkColumn, object? PkValue, List<KeyValuePair<string, object?>> Columns);

    private async Task<List<(string Name, Type Type)>> GetLiveColumnsAsync(
        NpgsqlConnection conn, NpgsqlTransaction tx, string qualifiedTable, CancellationToken ct)
    {
        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = $"SELECT * FROM {qualifiedTable} WHERE 1 = 0";
        await using var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SchemaOnly, ct);
        var cols = new List<(string, Type)>(reader.FieldCount);
        for (var i = 0; i < reader.FieldCount; i++)
            cols.Add((reader.GetName(i), reader.GetFieldType(i)));
        return cols;
    }

    private HashSet<string> ComputeDeferredColumns(Type clrType, HashSet<string> selectedSet, int selfIndex, Dictionary<string, int> orderIndex)
    {
        var deferred = new HashSet<string>(StringComparer.Ordinal);
        var et = _context.Model.FindEntityType(clrType);
        if (et is null) return deferred;

        foreach (var fk in et.GetForeignKeys())
        {
            var principalKey = fk.PrincipalEntityType.GetTableName();
            if (principalKey is null || !orderIndex.TryGetValue(principalKey, out var principalIndex))
                continue; // principal not part of this restore → insert value as-is (best effort).

            var allNullable = fk.Properties.All(p => p.IsNullable);
            var forwardOrSelf = principalIndex >= selfIndex;
            if (allNullable && forwardOrSelf)
            {
                foreach (var p in fk.Properties)
                    deferred.Add(p.GetColumnName());
            }
        }
        return deferred;
    }

    private async Task<int> InsertRowAsync(
        NpgsqlConnection conn, NpgsqlTransaction tx, string qualifiedTable, string tableKey,
        List<(string Name, Type Type)> liveColumns, Dictionary<string, JsonElement> row,
        HashSet<string> deferred, string? pkColumn, List<FixUp> fixups, CancellationToken ct)
    {
        var columns = new List<string>();
        var paramNames = new List<string>();
        object? pkValue = null;
        List<KeyValuePair<string, object?>>? deferredValues = null;

        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;

        var pi = 0;
        foreach (var (colName, colType) in liveColumns)
        {
            if (!row.TryGetValue(colName, out var jsonValue))
                continue; // column absent from the backup row → let the DB default apply.

            var value = ConvertJson(jsonValue, colType);

            if (pkColumn is not null && string.Equals(colName, pkColumn, StringComparison.Ordinal))
                pkValue = value;

            if (deferred.Contains(colName))
            {
                // Insert NULL now; remember the real value for a post-insert UPDATE.
                if (value is not null)
                {
                    deferredValues ??= new List<KeyValuePair<string, object?>>();
                    deferredValues.Add(new KeyValuePair<string, object?>(colName, value));
                }
                continue;
            }

            var pn = $"@p{pi++}";
            columns.Add(Quote(colName));
            paramNames.Add(pn);
            cmd.Parameters.Add(new NpgsqlParameter(pn, value ?? DBNull.Value));
        }

        if (columns.Count == 0)
            return 0;

        cmd.CommandText = $"INSERT INTO {qualifiedTable} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", paramNames)})";
        await cmd.ExecuteNonQueryAsync(ct);

        if (deferredValues is not null && pkColumn is not null)
            fixups.Add(new FixUp(qualifiedTable, pkColumn, pkValue, deferredValues));

        return 1;
    }

    private async Task ResetSequenceAsync(NpgsqlConnection conn, NpgsqlTransaction tx, string qualifiedTable, string pkColumn, CancellationToken ct)
    {
        // Resolve the identity/serial sequence (NULL if the PK isn't sequence-backed) and, only then,
        // advance it — calling setval(NULL, ...) would error and abort the transaction.
        string? sequence;
        await using (var seqCmd = conn.CreateCommand())
        {
            seqCmd.Transaction = tx;
            seqCmd.CommandText = $"SELECT pg_get_serial_sequence('{qualifiedTable.Replace("'", "''")}', @col)";
            seqCmd.Parameters.Add(new NpgsqlParameter("@col", pkColumn));
            var scalar = await seqCmd.ExecuteScalarAsync(ct);
            sequence = scalar as string;
        }
        if (string.IsNullOrEmpty(sequence)) return;

        await using var setCmd = conn.CreateCommand();
        setCmd.Transaction = tx;
        // is_called=true so the next nextval() returns MAX+1; GREATEST guards an empty table.
        setCmd.CommandText =
            $"SELECT setval('{sequence.Replace("'", "''")}', GREATEST((SELECT COALESCE(MAX({Quote(pkColumn)}), 0) FROM {qualifiedTable}), 1), true)";
        await setCmd.ExecuteScalarAsync(ct);
    }

    private static object? ConvertJson(JsonElement el, Type targetType)
    {
        if (el.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return null;

        var t = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (t == typeof(string)) return el.GetString();
        if (t == typeof(bool)) return el.ValueKind == JsonValueKind.String ? bool.Parse(el.GetString()!) : el.GetBoolean();
        if (t == typeof(int)) return el.ValueKind == JsonValueKind.String ? int.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetInt32();
        if (t == typeof(long)) return el.ValueKind == JsonValueKind.String ? long.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetInt64();
        if (t == typeof(short)) return el.ValueKind == JsonValueKind.String ? short.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetInt16();
        if (t == typeof(byte)) return el.GetByte();
        if (t == typeof(decimal)) return el.ValueKind == JsonValueKind.String ? decimal.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetDecimal();
        if (t == typeof(double)) return el.ValueKind == JsonValueKind.String ? double.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetDouble();
        if (t == typeof(float)) return el.ValueKind == JsonValueKind.String ? float.Parse(el.GetString()!, CultureInfo.InvariantCulture) : el.GetSingle();
        if (t == typeof(Guid)) return el.ValueKind == JsonValueKind.String ? Guid.Parse(el.GetString()!) : el.GetGuid();
        if (t == typeof(byte[])) return el.ValueKind == JsonValueKind.String ? Convert.FromBase64String(el.GetString()!) : el.GetBytesFromBase64();
        if (t == typeof(TimeSpan)) return TimeSpan.Parse(el.GetString()!, CultureInfo.InvariantCulture);
        if (t == typeof(DateTimeOffset)) return el.GetDateTimeOffset();
        if (t == typeof(DateTime))
        {
            // PostgreSQL timestamptz requires a UTC DateTime; normalise regardless of how it was stored.
            var raw = el.GetString();
            if (raw is null) return el.GetDateTime();
            var dt = DateTime.Parse(raw, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        // Fallback: hand back the raw string representation.
        return el.ValueKind == JsonValueKind.String ? el.GetString() : el.GetRawText();
    }

    private static List<TableDescriptor> ResolveSelected(IEnumerable<string>? selectedKeys)
    {
        var keys = selectedKeys?.ToList();
        if (keys is null || keys.Count == 0)
            return Tables.ToList();
        var set = new HashSet<string>(keys, StringComparer.OrdinalIgnoreCase);
        return Tables.Where(t => set.Contains(t.Key)).ToList();
    }
}
