<#
.SYNOPSIS
    Visual Studio Docker Debug Stop Script
.DESCRIPTION
    Forcefully cleans up all containers, networks, and ports when debugging stops
.PARAMETER ProjectPath
    The path to the project directory
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectPath
)

Write-Host "🛑 Stopping Visual Studio Docker Debug Session..." -ForegroundColor Red

# Change to project directory
Set-Location $ProjectPath

# Forcefully kill any processes using the required ports
Write-Host "🔥 Forcefully cleaning up ports: 9010, 9011, 9030, 9031, 9020, 9021, 9040, 9041..." -ForegroundColor Red
$ports = @(9010, 9011, 9030, 9031, 9020, 9021, 9040, 9041)
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
        Write-Host "   Port $port cleanup completed or not needed: $_" -ForegroundColor Gray
    }
}

# Forcefully remove all containers
Write-Host "🧹 Forcefully removing all containers..." -ForegroundColor Yellow
$containers = @(
    "stadium-api",
    "stadium-admin",
    "stadium-customer",
    "stadium-staff",
    "stadiumdrinkordering.api",
    "stadiumdrinkordering.customer",
    "stadiumdrinkordering.admin",
    "stadiumdrinkordering.staff"
)

foreach ($container in $containers) {
    try {
        Write-Host "   Force removing container: $container" -ForegroundColor Yellow
        docker rm -f $container 2>$null
    } catch {
        Write-Host "   Container $container already removed or not found" -ForegroundColor Gray
    }
}

# Stop and remove all containers via docker-compose
Write-Host "🧹 Cleaning up containers via docker-compose..." -ForegroundColor Yellow
docker-compose -f docker-compose.yml -f docker-compose.override.yml down --remove-orphans --volumes --rmi all

# Clean up networks
Write-Host "🌐 Cleaning up networks..." -ForegroundColor Yellow
docker network prune -f
try {
    docker network rm stadium-network 2>$null
    docker network rm stadiumdrinkordering_stadium-network 2>$null
    docker network rm stadiumapp_stadium-network 2>$null
} catch {
    Write-Host "   Networks already cleaned up" -ForegroundColor Gray
}

# Clean up any lingering volumes (PostgreSQL is external, so just cleanup temp volumes)
Write-Host "💾 Cleaning up temporary volumes..." -ForegroundColor Yellow
docker volume prune -f
Write-Host "Temporary volumes cleaned up successfully" -ForegroundColor Green

# Clean up any dangling volumes
Write-Host "💾 Cleaning up dangling volumes..." -ForegroundColor Yellow
docker volume prune -f

# Clean up any dangling images
Write-Host "🐳 Cleaning up images..." -ForegroundColor Yellow
docker image prune -f

# Final port verification
Write-Host "🔍 Verifying ports are freed..." -ForegroundColor Blue
foreach ($port in @(9010, 9030)) {
    $portCheck = Test-NetConnection -ComputerName "localhost" -Port $port -InformationLevel Quiet -ErrorAction SilentlyContinue
    if (!$portCheck) {
        Write-Host "   ✅ Port $port is freed" -ForegroundColor Green
    } else {
        Write-Warning "   ⚠️ Port $port may still be in use"
    }
}

Write-Host "✅ Debug session cleanup complete!" -ForegroundColor Green
