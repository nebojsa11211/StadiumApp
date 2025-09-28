#!/usr/bin/env pwsh
# =============================================================================
# Stadium Drink Ordering - Visual Studio Debug Cleanup Script
# =============================================================================
# This script kills all running StadiumDrinkOrdering dotnet processes
# to prepare for clean Visual Studio debugging sessions.
#
# Usage:
#   PowerShell: .\cleanup-for-vs-debug.ps1
#   CMD: powershell -ExecutionPolicy Bypass -File "cleanup-for-vs-debug.ps1"
# =============================================================================

Write-Host "🔄 Stadium Drink Ordering - Visual Studio Debug Cleanup" -ForegroundColor Cyan
Write-Host "=========================================================" -ForegroundColor Cyan

# Function to safely stop processes
function Stop-ProcessSafely {
    param(
        [string]$ProcessName,
        [string]$DisplayName
    )

    try {
        $processes = Get-Process -Name $ProcessName -ErrorAction SilentlyContinue
        if ($processes) {
            Write-Host "🔍 Found $($processes.Count) $DisplayName process(es)" -ForegroundColor Yellow
            foreach ($process in $processes) {
                Write-Host "   Stopping PID $($process.Id) - $($process.ProcessName)" -ForegroundColor Gray
                Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
            }
            Write-Host "✅ All $DisplayName processes stopped" -ForegroundColor Green
        } else {
            Write-Host "ℹ️  No $DisplayName processes found" -ForegroundColor Gray
        }
    }
    catch {
        Write-Host "⚠️  Error stopping $DisplayName processes: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Kill all dotnet processes (includes both API and Admin)
Stop-ProcessSafely -ProcessName "dotnet" -DisplayName "dotnet"

# Also kill any Visual Studio processes that might be related
Stop-ProcessSafely -ProcessName "StadiumDrinkOrdering.API" -DisplayName "API"
Stop-ProcessSafely -ProcessName "StadiumDrinkOrdering.Admin" -DisplayName "Admin"

# Check for any remaining processes on key ports
Write-Host ""
Write-Host "🔍 Checking ports 7010 (API) and 7030 (Admin)..." -ForegroundColor Cyan

try {
    $port7010 = Get-NetTCPConnection -LocalPort 7010 -ErrorAction SilentlyContinue
    $port7030 = Get-NetTCPConnection -LocalPort 7030 -ErrorAction SilentlyContinue

    if ($port7010) {
        Write-Host "⚠️  Port 7010 (API) is still in use by PID $($port7010.OwningProcess)" -ForegroundColor Yellow
        try {
            Stop-Process -Id $port7010.OwningProcess -Force
            Write-Host "✅ Killed process using port 7010" -ForegroundColor Green
        }
        catch {
            Write-Host "❌ Failed to kill process on port 7010" -ForegroundColor Red
        }
    } else {
        Write-Host "✅ Port 7010 (API) is free" -ForegroundColor Green
    }

    if ($port7030) {
        Write-Host "⚠️  Port 7030 (Admin) is still in use by PID $($port7030.OwningProcess)" -ForegroundColor Yellow
        try {
            Stop-Process -Id $port7030.OwningProcess -Force
            Write-Host "✅ Killed process using port 7030" -ForegroundColor Green
        }
        catch {
            Write-Host "❌ Failed to kill process on port 7030" -ForegroundColor Red
        }
    } else {
        Write-Host "✅ Port 7030 (Admin) is free" -ForegroundColor Green
    }
}
catch {
    Write-Host "ℹ️  Port check completed with warnings (this is normal)" -ForegroundColor Gray
}

Write-Host ""
Write-Host "🎯 Cleanup completed! Ready for Visual Studio debugging." -ForegroundColor Green
Write-Host "=========================================================" -ForegroundColor Cyan
Write-Host "You can now start debugging in Visual Studio:" -ForegroundColor White
Write-Host "  1. Set StadiumDrinkOrdering.API as startup project" -ForegroundColor Gray
Write-Host "  2. Press F5 to start API debugging" -ForegroundColor Gray
Write-Host "  3. Set StadiumDrinkOrdering.Admin as startup project" -ForegroundColor Gray
Write-Host "  4. Press F5 to start Admin debugging" -ForegroundColor Gray
Write-Host ""