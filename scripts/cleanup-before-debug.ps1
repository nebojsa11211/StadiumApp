<#
.SYNOPSIS
    Simple cleanup script for Visual Studio Docker debugging
.DESCRIPTION
    Cleans up existing containers before Visual Studio starts debugging
#>

param(
    [string]$ProjectPath = "D:\StadionTest"
)

Write-Host "🧹 Cleaning up Docker containers before debug..." -ForegroundColor Yellow

# Change to project directory
Set-Location $ProjectPath

# Simple cleanup - stop and remove existing containers
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down --remove-orphans --volumes 2>$null

# Force remove any stubborn containers
docker rm -f stadium-sqlserver-debug 2>$null
docker rm -f stadium-api-debug 2>$null
docker rm -f stadium-customer-debug 2>$null
docker rm -f stadium-admin-debug 2>$null

# Clean up networks
docker network prune -f 2>$null

Write-Host "✅ Cleanup complete - ready for fresh containers!" -ForegroundColor Green
