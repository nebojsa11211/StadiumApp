#!/usr/bin/env pwsh
# =============================================================================
# Ensure the local PostgreSQL dev database is running.
# =============================================================================
# WHY THIS EXISTS:
#   The local dev DB container (stadium-postgres-local) is defined in the main
#   docker-compose.yml alongside the app services. Running `docker-compose down`
#   (the documented "stop services" command) DELETES it and the network, so the
#   local VS API then fails with:
#     "The maximum number of retries (3) was exceeded ... NpgsqlRetryingExecutionStrategy"
#   The DATA is safe (named volume `stadium-pg-data` is `external: true`), only the
#   container disappears. This script recreates/starts it idempotently.
#
# USAGE:
#   .\ensure-localdb.ps1            # run before debugging, or any time the API can't reach the DB
# =============================================================================

$ErrorActionPreference = 'Stop'
$container = 'stadium-postgres-local'
$composeFile = Join-Path $PSScriptRoot 'docker-compose.yml'

function Test-Healthy {
    $status = docker inspect -f '{{if .State.Health}}{{.State.Health.Status}}{{else}}{{.State.Status}}{{end}}' $container 2>$null
    return ($status -eq 'healthy' -or $status -eq 'running')
}

Write-Host "Checking local dev database ($container)..." -ForegroundColor Cyan

$exists = docker ps -a --filter "name=^$container$" --format '{{.Names}}' 2>$null
$running = docker ps    --filter "name=^$container$" --format '{{.Names}}' 2>$null

if ($running) {
    Write-Host "  Already running." -ForegroundColor Green
}
elseif ($exists) {
    Write-Host "  Container exists but is stopped - starting it..." -ForegroundColor Yellow
    docker start $container | Out-Null
}
else {
    Write-Host "  Container missing (likely removed by 'docker-compose down') - recreating from compose..." -ForegroundColor Yellow
    docker-compose -f $composeFile up -d postgres | Out-Null
}

# Wait for healthy (up to ~30s)
Write-Host "  Waiting for database to become healthy..." -ForegroundColor Gray
for ($i = 0; $i -lt 15; $i++) {
    if (Test-Healthy) {
        Write-Host "  Database is healthy and accepting connections (localhost:5432/stadiumdb)." -ForegroundColor Green
        exit 0
    }
    Start-Sleep -Seconds 2
}

Write-Host "  WARNING: database did not report healthy within 30s. Check 'docker logs $container'." -ForegroundColor Red
exit 1
