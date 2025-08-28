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
    "http://localhost:5002",   # Customer App
    "http://localhost:5003",   # Admin App
    "http://localhost:5001/swagger"  # API Swagger
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
    Write-Host "   • Customer App: http://localhost:5002" -ForegroundColor White
    Write-Host "   • Admin App: http://localhost:5003" -ForegroundColor White
    Write-Host "   • API Swagger: http://localhost:5001/swagger" -ForegroundColor White
    
} catch {
    Write-Host "❌ Error opening browser tabs: $_" -ForegroundColor Red
    Write-Host "💡 Please manually open these URLs:" -ForegroundColor Yellow
    $urls | ForEach-Object { Write-Host "   $_" -ForegroundColor Cyan }
}

# Keep window open briefly to see results
Start-Sleep -Seconds 3
