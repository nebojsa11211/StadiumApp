using System;
using Npgsql;

var connectionString = @"Host=aws-1-eu-north-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.ylaccqjfrqzruhgbzpnw;Password=d!hZ5A9@t+e!Nn2;SSL Mode=Require;Trust Server Certificate=true";
try 
{
    using var conn = new NpgsqlConnection(connectionString);
    conn.Open();
    Console.WriteLine("SUCCESS: Connected to database!");
    
    using var cmd = new NpgsqlCommand("SELECT version()", conn);
    var version = cmd.ExecuteScalar();
    Console.WriteLine($"PostgreSQL Version: {version}");
    
    conn.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}
