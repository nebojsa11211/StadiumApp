<#
.SYNOPSIS
    Visual Studio Docker Debug Start Script
.DESCRIPTION
    Ensures containers are recreated fresh on each debug session by forcefully cleaning up ports
.PARAMETER ProjectPath
    The path to the project directory
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectPath
)

Write-Host "🚀 Starting Visual Studio Docker Debug Session..." -ForegroundColor Green

# Change to project directory
Set-Location $ProjectPath

# Forcefully kill any processes using the required ports
Write-Host "🔥 Forcefully cleaning up ports: 5001, 5002, 5003, 9000, 9001, 9002..." -ForegroundColor Red
$ports = @(5001, 5002, 5003, 9000, 9001, 9002, 1433)
foreach ($port in $ports) {
    try {
        # Kill processes using the port
        $processes = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($processes) {
            foreach ($process in $processes) {
                $processId = $process.OwningProcess
                Write-Host "   Killing process $processId using port $port..." -ForegroundColor Yellow
                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
            }
        }
    } catch {
        Write-Host "   Port $port is already free or cleanup failed: $_" -ForegroundColor Gray
    }
}

# Forcefully remove any existing containers that might conflict
Write-Host "🧹 Forcefully removing existing containers..." -ForegroundColor Yellow
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
    try {
        Write-Host "   Removing container: $container" -ForegroundColor Yellow
        docker rm -f $container 2>$null
    } catch {
        Write-Host "   Container $container already removed or not found" -ForegroundColor Gray
    }
}

# Clean up any existing networks
Write-Host "🌐 Cleaning up networks..." -ForegroundColor Yellow
docker network prune -f
try {
    docker network rm stadium-network-debug 2>$null
    docker network rm stadiontest_stadium-network 2>$null
    docker network rm stadium-network 2>$null
} catch {
    Write-Host "   Networks already cleaned up" -ForegroundColor Gray
}

# Clean up volumes to ensure fresh data
Write-Host "💾 Cleaning up debug volumes..." -ForegroundColor Yellow
try {
    docker volume rm stadiontest_sqlserver_debug_data 2>$null
    docker volume rm sqlserver_debug_data 2>$null
} catch {
    Write-Host "   Volumes already cleaned up" -ForegroundColor Gray
}

# Build fresh images
Write-Host "🏗️ Building fresh container images..." -ForegroundColor Cyan
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml build --no-cache

# Start containers
Write-Host "▶️ Starting fresh containers..." -ForegroundColor Green
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml up -d

# Wait for services to be ready
Write-Host "⏳ Waiting for services to be ready..." -ForegroundColor Blue
$maxAttempts = 30
$attempt = 0
$servicesReady = $false

while ($attempt -lt $maxAttempts -and !$servicesReady) {
    $attempt++
    try {
        $runningContainers = docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml ps --services --filter "status=running" | Measure-Object | Select-Object -ExpandProperty Count
        $totalServices = 4  # sqlserver, api, customer, admin
        
        if ($runningContainers -ge $totalServices) {
            $servicesReady = $true
            Write-Host "✅ All services are running!" -ForegroundColor Green
            break
        }
    } catch {
        Write-Host "   Error checking services: $_" -ForegroundColor Red
    }
    
    Write-Host "   Attempt $attempt/$maxAttempts - waiting for services..." -ForegroundColor Gray
    Start-Sleep -Seconds 2
}

if (!$servicesReady) {
    Write-Warning "⚠️ Some services may still be starting..."
}

# Final port verification
Write-Host "🔍 Verifying ports are available..." -ForegroundColor Blue
foreach ($port in @(5001, 5002, 5003)) {
    $portCheck = Test-NetConnection -ComputerName "localhost" -Port $port -InformationLevel Quiet -ErrorAction SilentlyContinue
    if ($portCheck) {
        Write-Host "   ✅ Port $port is available" -ForegroundColor Green
    } else {
        Write-Warning "   ⚠️ Port $port may not be ready yet"
    }
}

Write-Host "🎯 Debug session ready!" -ForegroundColor Green
Write-Host "   API: http://localhost:5001" -ForegroundColor Cyan
Write-Host "   Customer: http://localhost:5002" -ForegroundColor Cyan
Write-Host "   Admin: http://localhost:5003" -ForegroundColor Cyan
