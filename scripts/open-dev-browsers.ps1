# Opens each Stadium app in its OWN browser window (not tabs in one window).
#
# Why this exists: Visual Studio's "launchBrowser" just hands the URL to the OS
# default browser (ShellExecute). Chrome/Edge answer that by adding a TAB to the
# existing window. There is no VS setting to change that -- the browser decides.
# So every launchSettings.json has "launchBrowser": false and we open the
# windows explicitly here with --new-window.
#
# This is NOT wired into the build or into any app. Run it yourself, by hand,
# after F5. See CLAUDE.md: no build-spawned or app-spawned browser automation.
#
#   .\scripts\open-dev-browsers.ps1                  # all apps, own window each
#   .\scripts\open-dev-browsers.ps1 -Only admin,bar  # just those two
#   .\scripts\open-dev-browsers.ps1 -Isolated        # + separate browser profiles
#   .\scripts\open-dev-browsers.ps1 -TimeoutSeconds 90

[CmdletBinding()]
param(
    [string[]]$Only,
    [switch]$Isolated,
    [int]$TimeoutSeconds = 60
)

$ErrorActionPreference = 'Stop'

# Ports per CLAUDE.md. Runner = 7060 and Simulator = 7050 must NOT be shared.
$apps = [ordered]@{
    api       = @{ Port = 7010; Url = 'https://localhost:7010/swagger' }
    customer  = @{ Port = 7020; Url = 'https://localhost:7020' }
    admin     = @{ Port = 7030; Url = 'https://localhost:7030' }
    bar       = @{ Port = 7040; Url = 'https://localhost:7040' }
    simulator = @{ Port = 7050; Url = 'https://localhost:7050' }
    runner    = @{ Port = 7060; Url = 'https://localhost:7060' }
}

if ($Only) {
    $unknown = $Only | Where-Object { -not $apps.Contains($_) }
    if ($unknown) {
        throw "Unknown app(s): $($unknown -join ', '). Valid: $($apps.Keys -join ', ')"
    }
    $selected = $Only
} else {
    $selected = @($apps.Keys)
}

# Find a Chromium browser. Only Chrome/Edge support --new-window reliably;
# without one we cannot guarantee separate windows, so say so rather than
# silently falling back to Start-Process (which is what produces tabs).
$browser = $null
$candidates = @(
    "$env:ProgramFiles\Google\Chrome\Application\chrome.exe",
    "${env:ProgramFiles(x86)}\Google\Chrome\Application\chrome.exe",
    "$env:LOCALAPPDATA\Google\Chrome\Application\chrome.exe",
    "$env:ProgramFiles\Microsoft\Edge\Application\msedge.exe",
    "${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe"
)
foreach ($c in $candidates) {
    if (Test-Path $c) { $browser = $c; break }
}
if (-not $browser) {
    Write-Host "No Chrome/Edge found. Cannot force separate windows." -ForegroundColor Red
    Write-Host "Open these manually:" -ForegroundColor Yellow
    $selected | ForEach-Object { Write-Host "   $($apps[$_].Url)" -ForegroundColor Cyan }
    exit 1
}
Write-Host "Browser: $browser" -ForegroundColor DarkGray

function Wait-ForPort {
    param([int]$Port, [int]$Timeout)
    $deadline = (Get-Date).AddSeconds($Timeout)
    while ((Get-Date) -lt $deadline) {
        # 127.0.0.1, not "localhost": localhost can resolve to ::1 first and
        # give a misleading failure (same IPv6 gotcha as the EF tooling).
        $conn = Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue
        if ($conn) { return $true }
        Start-Sleep -Milliseconds 500
    }
    return $false
}

$opened = @()
$skipped = @()

foreach ($name in $selected) {
    $app = $apps[$name]
    Write-Host "Waiting for $name on port $($app.Port)..." -ForegroundColor Yellow

    if (-not (Wait-ForPort -Port $app.Port -Timeout $TimeoutSeconds)) {
        Write-Host "  $name never started listening on $($app.Port) - skipped." -ForegroundColor Red
        $skipped += $name
        continue
    }

    $browserArgs = @('--new-window', $app.Url)

    if ($Isolated) {
        # A distinct --user-data-dir gives a fully independent browser instance
        # (own cookies/localStorage). Usually unnecessary here since each app is
        # on its own port and therefore already its own origin -- use this only
        # when you want e.g. two different logins side by side.
        $profileDir = Join-Path $env:LOCALAPPDATA "StadiumDev\browser-profiles\$name"
        New-Item -ItemType Directory -Force -Path $profileDir | Out-Null
        $browserArgs = @("--user-data-dir=$profileDir", '--no-first-run', '--new-window', $app.Url)
    }

    Start-Process -FilePath $browser -ArgumentList $browserArgs | Out-Null
    Write-Host "  opened $($app.Url)" -ForegroundColor Green
    $opened += $name

    # Let the window materialize before asking for the next one; firing them
    # back-to-back can make Chrome coalesce them into one window.
    Start-Sleep -Milliseconds 900
}

Write-Host ""
Write-Host "Opened $($opened.Count) window(s): $($opened -join ', ')" -ForegroundColor Green
if ($skipped) {
    Write-Host "Skipped (not listening): $($skipped -join ', ')" -ForegroundColor Yellow
}
