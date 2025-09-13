# Multi-Tab Browser Launch Script for Stadium Drink Ordering System
# Opens Customer, Admin, and API Swagger in separate tabs

param(
    [int]$Delay = 12
)

Write-Host "🚀 Starting multi-tab browser launch..." -ForegroundColor Green
Write-Host "⏳ Waiting $Delay seconds for all services to start..." -ForegroundColor Yellow

# Wait for all services to be ready
Start-Sleep -Seconds $Delay

# URLs to open
$urls = @(
    "https://localhost:7020",   # Customer App
    "https://localhost:7030",   # Admin App
    "https://localhost:7010/swagger"  # API Swagger
)

Write-Host "🌐 Opening all service URLs in browser tabs..." -ForegroundColor Cyan

try {
    # Open all URLs in new tabs
    foreach ($url in $urls) {
        Write-Host "📍 Opening: $url" -ForegroundColor Cyan
        Start-Process $url
        Start-Sleep -Seconds 1  # Small delay between tabs
    }
    
    Write-Host "✅ All browser tabs opened successfully!" -ForegroundColor Green
    Write-Host "📋 Opened URLs:" -ForegroundColor Green
    Write-Host "   • Customer App: https://localhost:7020" -ForegroundColor White
    Write-Host "   • Admin App: https://localhost:7030" -ForegroundColor White
    Write-Host "   • API Swagger: https://localhost:7010/swagger" -ForegroundColor White
    
} catch {
    Write-Host "❌ Error opening browser tabs: $_" -ForegroundColor Red
    Write-Host "💡 Please manually open these URLs:" -ForegroundColor Yellow
    $urls | ForEach-Object { Write-Host "   $_" -ForegroundColor Cyan }
}

# Keep window open briefly to see results
Start-Sleep -Seconds 3
