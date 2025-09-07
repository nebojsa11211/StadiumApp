using System;
using System.Threading.Tasks;
using Npgsql;

class TestDbConnection
{
    static async Task Main(string[] args)
    {
        var connectionString = "Host=aws-1-eu-north-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.ylaccqjfrqzruhgbzpnw;Password=d!hZ5A9@t+e!Nn2;SSL Mode=Require;Trust Server Certificate=true;Timeout=60";
        
        Console.WriteLine("Testing database connection...");
        Console.WriteLine($"Connection String: {connectionString.Replace("Password=d!hZ5A9@t+e!Nn2", "Password=***")}");
        Console.WriteLine();
        
        try
        {
            using var dataSource = NpgsqlDataSource.Create(connectionString);
            using var conn = await dataSource.OpenConnectionAsync();
            
            Console.WriteLine("✓ Successfully connected to PostgreSQL!");
            
            // Test query
            using var cmd = new NpgsqlCommand("SELECT version(), current_database(), current_user", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                Console.WriteLine($"Database Version: {reader.GetString(0)}");
                Console.WriteLine($"Current Database: {reader.GetString(1)}");
                Console.WriteLine($"Current User: {reader.GetString(2)}");
            }
            
            // Test if we can query tables
            using var cmd2 = new NpgsqlCommand("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public'", conn);
            var tableCount = await cmd2.ExecuteScalarAsync();
            Console.WriteLine($"Public tables count: {tableCount}");
            
            Console.WriteLine("\n✓ Database connection test completed successfully!");
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine($"✗ PostgreSQL Error: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.SqlState}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ General Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}