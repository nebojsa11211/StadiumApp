# Test database connection script
$connectionString = "Host=aws-1-eu-north-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.ylaccqjfrqzruhgbzpnw;Password=d!hZ5A9@t+e!Nn2;SSL Mode=Require;Trust Server Certificate=true"

Write-Host "Testing database connection..." -ForegroundColor Yellow
Write-Host "Host: aws-1-eu-north-1.pooler.supabase.com" -ForegroundColor Cyan
Write-Host "Port: 6543" -ForegroundColor Cyan
Write-Host "Database: postgres" -ForegroundColor Cyan

# Create a simple C# script to test the connection
$testScript = @"
using System;
using Npgsql;

var connectionString = @"$connectionString";
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
"@

# Save and run the test script
$testScript | Out-File -FilePath "test-connection.csx" -Encoding UTF8
dotnet script test-connection.csx 2>$null

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nAlternative test using psql (if available):" -ForegroundColor Yellow
    $env:PGPASSWORD = "d!hZ5A9@t+e!Nn2"
    psql -h aws-1-eu-north-1.pooler.supabase.com -p 6543 -U postgres.ylaccqjfrqzruhgbzpnw -d postgres -c "SELECT 1" 2>$null
}