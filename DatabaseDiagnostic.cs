using Npgsql;
using System;
using System.Threading.Tasks;

class DatabaseDiagnostic
{
    static async Task Main(string[] args)
    {
        var connectionString = "Host=aws-0-eu-central-1.pooler.supabase.com;Database=postgres;Username=postgres.ylaccqjfrqzruhgbzpnw;Password=d!hZ5A9@t+e!Nn2;Port=6543;Ssl Mode=Require;Trust Server Certificate=true;Timeout=300;Command Timeout=300";

        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("DATABASE DIAGNOSTIC - CHECKING ROW COUNTS");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine();

        try
        {
            await using var conn = new NpgsqlConnection(connectionString);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Connecting to database...");
            var connectStart = DateTime.Now;
            await conn.OpenAsync();
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Connected successfully in {(DateTime.Now - connectStart).TotalSeconds:F2}s");
            Console.WriteLine();

            // Query LogEntries
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Querying LogEntries table...");
            var logStart = DateTime.Now;
            await using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"LogEntries\"", conn))
            {
                cmd.CommandTimeout = 300;
                var logCount = await cmd.ExecuteScalarAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] LogEntries row count: {logCount:N0} (query took {(DateTime.Now - logStart).TotalSeconds:F2}s)");
            }

            // Get LogEntries date range
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Querying LogEntries date range...");
            var dateStart = DateTime.Now;
            await using (var cmd = new NpgsqlCommand("SELECT MIN(\"Timestamp\"), MAX(\"Timestamp\") FROM \"LogEntries\"", conn))
            {
                cmd.CommandTimeout = 300;
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var oldest = reader.IsDBNull(0) ? (DateTime?)null : reader.GetDateTime(0);
                    var newest = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Oldest log: {oldest?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}");
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Newest log: {newest?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}");
                    if (oldest.HasValue && newest.HasValue)
                    {
                        var span = newest.Value - oldest.Value;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Time span: {span.TotalDays:F1} days");
                    }
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Date range query took {(DateTime.Now - dateStart).TotalSeconds:F2}s");
                }
            }
            Console.WriteLine();

            // Query IPBans
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Querying IPBans table...");
            var ipStart = DateTime.Now;
            await using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"IPBans\"", conn))
            {
                cmd.CommandTimeout = 300;
                var ipCount = await cmd.ExecuteScalarAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] IPBans row count: {ipCount:N0} (query took {(DateTime.Now - ipStart).TotalSeconds:F2}s)");
            }
            Console.WriteLine();

            // Query Users
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Querying Users table...");
            var userStart = DateTime.Now;
            await using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"Users\"", conn))
            {
                cmd.CommandTimeout = 300;
                var userCount = await cmd.ExecuteScalarAsync();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Users row count: {userCount:N0} (query took {(DateTime.Now - userStart).TotalSeconds:F2}s)");
            }
            Console.WriteLine();

            // Check table sizes
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Querying table sizes...");
            var sizeStart = DateTime.Now;
            await using (var cmd = new NpgsqlCommand(@"
                SELECT
                    relname as table_name,
                    pg_size_pretty(pg_total_relation_size(relid)) as total_size,
                    pg_size_pretty(pg_relation_size(relid)) as data_size
                FROM pg_catalog.pg_statio_user_tables
                WHERE relname IN ('LogEntries', 'IPBans', 'Users')
                ORDER BY pg_total_relation_size(relid) DESC", conn))
            {
                cmd.CommandTimeout = 300;
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"  {reader.GetString(0),-15} Total: {reader.GetString(1),-10} Data: {reader.GetString(2)}");
                }
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Size query took {(DateTime.Now - sizeStart).TotalSeconds:F2}s");
            }

            Console.WriteLine();
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine("DIAGNOSTIC COMPLETE");
            Console.WriteLine("=".PadRight(80, '='));
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"ERROR: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
            Console.WriteLine();
            Console.WriteLine("Stack trace:");
            Console.WriteLine(ex.StackTrace);
            Environment.ExitCode = 1;
        }
    }
}
