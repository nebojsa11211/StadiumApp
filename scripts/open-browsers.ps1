# Browser Launch Script for Stadium Drink Ordering System
# This script opens the appropriate browser windows when debugging starts

param(
    [string]$Service = "customer",
    [int]$Delay = 10
)

Write-Host "Starting browser launch for $Service service..." -ForegroundColor Green
Write-Host "Waiting $Delay seconds for services to start..." -ForegroundColor Yellow

# Wait for services to start
Start-Sleep -Seconds $Delay

# URLs for different services
$urls = @{
    "customer" = "https://localhost:7020"
    "admin" = "https://localhost:7030"
    "api" = "https://localhost:7010/swagger"
    "all" = @("https://localhost:7020", "https://localhost:7030", "https://localhost:7010/swagger")
}

try {
    if ($Service -eq "all") {
        Write-Host "Opening all service URLs..." -ForegroundColor Green
        foreach ($url in $urls["all"]) {
            Write-Host "Opening: $url" -ForegroundColor Cyan
            Start-Process $url
            Start-Sleep -Seconds 1
        }
    } else {
        $url = $urls[$Service]
        Write-Host "Opening: $url" -ForegroundColor Cyan
        Start-Process $url
    }
    
    Write-Host "Browser launch completed successfully!" -ForegroundColor Green
} catch {
    Write-Host "Error opening browser: $_" -ForegroundColor Red
    Write-Host "Please manually navigate to the URLs above" -ForegroundColor Yellow
}

# Keep window open for 5 seconds to see the result
Start-Sleep -Seconds 5
