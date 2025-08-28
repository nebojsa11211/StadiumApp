<#
.SYNOPSIS
    Fixes Visual Studio Docker debugging by ensuring containers are recreated
.DESCRIPTION
    This script ensures containers are recreated when Visual Studio starts debugging
#>

param(
    [string]$ProjectPath = "D:\StadionTest"
)

Write-Host "🔧 Fixing Visual Studio Docker debugging..." -ForegroundColor Cyan

Set-Location $ProjectPath

# Force cleanup of existing containers
Write-Host "🧹 Cleaning up existing containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down --remove-orphans --volumes --rmi all 2>$null

# Force remove any stubborn containers
$containers = @(
    "stadium-sqlserver-debug",
    "stadium-api-debug",
    "stadium-customer-debug",
    "stadium-admin-debug",
    "stadium-sqlserver-dev",
    "stadiumdrinkordering.api",
    "stadiumdrinkordering.customer",
    "stadiumdrinkordering.admin"
)

foreach ($container in $containers) {
    docker rm -f $container 2>$null
}

# Clean up networks and volumes
docker network prune -f 2>$null
docker volume rm stadiontest_sqlserver_debug_data 2>$null
docker volume rm sqlserver_debug_data 2>$null

# Build fresh images
Write-Host "🏗️ Building fresh images..." -ForegroundColor Green
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml build --no-cache

Write-Host "✅ Ready for Visual Studio debugging!" -ForegroundColor Green
Write-Host "You can now start debugging in Visual Studio (F5)" -ForegroundColor Cyan
