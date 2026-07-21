# Re-stamps the LAN address baked into every app's launchSettings.json.
#
# The apps bind BOTH https://localhost:<port> and https://<lan-ip>:<port>, so they stay reachable
# from this machine and from a phone/tablet on the same network. The LAN address is a DHCP lease,
# so when it changes this script repoints all six launch profiles at the new one.
#
#   .\set-dev-host.ps1                 # auto-detect the current LAN IP
#   .\set-dev-host.ps1 -IpAddress 192.168.178.40
#
# A DHCP reservation on your router avoids needing this at all.

[CmdletBinding()]
param(
    [string]$IpAddress
)

$ErrorActionPreference = 'Stop'

if (-not $IpAddress) {
    $candidates = Get-NetIPAddress -AddressFamily IPv4 |
        Where-Object { $_.IPAddress -notlike '127.*' -and $_.PrefixOrigin -eq 'Dhcp' }

    if (-not $candidates) {
        Write-Host "Could not auto-detect a LAN IPv4 address. Pass one explicitly:" -ForegroundColor Red
        Write-Host "  .\set-dev-host.ps1 -IpAddress 192.168.1.50" -ForegroundColor Yellow
        exit 1
    }

    if ($candidates.Count -gt 1) {
        Write-Host "Multiple LAN addresses found - pick one with -IpAddress:" -ForegroundColor Yellow
        $candidates | Select-Object IPAddress, InterfaceAlias | Format-Table -AutoSize
        exit 1
    }

    $IpAddress = $candidates.IPAddress
    Write-Host "Auto-detected LAN address: $IpAddress" -ForegroundColor Cyan
}

if (-not [System.Net.IPAddress]::TryParse($IpAddress, [ref]$null)) {
    Write-Host "'$IpAddress' is not a valid IP address." -ForegroundColor Red
    exit 1
}

# project directory -> port
$apps = [ordered]@{
    'StadiumDrinkOrdering.API'                = 7010
    'StadiumDrinkOrdering.Customer'           = 7020
    'StadiumDrinkOrdering.Admin'              = 7030
    'StadiumDrinkOrdering.Bar'                = 7040
    'StadiumDrinkOrdering.TicketingSimulator' = 7050
    'StadiumDrinkOrdering.Runner'             = 7060
}

$changed = 0

foreach ($app in $apps.Keys) {
    $port = $apps[$app]
    $path = Join-Path $PSScriptRoot "$app\Properties\launchSettings.json"

    if (-not (Test-Path $path)) {
        Write-Host "  SKIP  $app (no launchSettings.json)" -ForegroundColor DarkYellow
        continue
    }

    $content = Get-Content $path -Raw

    # Rewrite the LAN half of the two-endpoint pair, PRESERVING which endpoint comes first.
    # Order is meaningful: Visual Studio opens the browser at the first url. Most apps list the LAN
    # address first so F5 lands on it; the Runner deliberately lists localhost first because the
    # Blazor WASM debug proxy (inspectUri -> wss://{url.hostname}/_framework/debug/ws-proxy) only
    # works over loopback. The bare "localhost:<port>" entries under iisSettings have no ';' and
    # are deliberately left alone.
    $lanFirst   = "https://[^`":;]+:$port;https://localhost:$port"
    $localFirst = "https://localhost:$port;https://[^`":;]+:$port"

    # The Runner is LAN-ONLY (it is the phone app), so it has a single url with no ';' - match that too,
    # otherwise it silently reports "already correct" while still pointing at the stale lease.
    $lanOnly = "https://(?!localhost)[^`":;]+:$port"

    if ($content -match $lanFirst) {
        $updated = [regex]::Replace($content, $lanFirst, "https://$($IpAddress):$port;https://localhost:$port")
    }
    elseif ($content -match $localFirst) {
        $updated = [regex]::Replace($content, $localFirst, "https://localhost:$port;https://$($IpAddress):$port")
    }
    elseif ($content -match $lanOnly) {
        $updated = [regex]::Replace($content, $lanOnly, "https://$($IpAddress):$port")
    }
    else {
        $updated = $content
    }

    if ($updated -eq $content) {
        Write-Host "  ----  $app already points at $IpAddress" -ForegroundColor DarkGray
        continue
    }

    Set-Content -Path $path -Value $updated -Encoding utf8 -NoNewline
    Write-Host "  OK    $app -> https://$($IpAddress):$port" -ForegroundColor Green
    $changed++
}

# Customer and Bar additionally pin a "Urls" key in appsettings.Development.json. WebApplication
# applies that key OVER the host urls from launchSettings, so if it is left at localhost those two
# apps silently ignore the LAN binding above. Keep both in sync.
foreach ($app in @('StadiumDrinkOrdering.Customer', 'StadiumDrinkOrdering.Bar')) {
    $port = $apps[$app]
    $path = Join-Path $PSScriptRoot "$app\appsettings.Development.json"

    if (-not (Test-Path $path)) { continue }

    $content = Get-Content $path -Raw
    $pattern     = "`"Urls`"\s*:\s*`"https://[^`"]*:$port(?:;https://[^`"]+)?`""
    $replacement = "`"Urls`": `"https://$($IpAddress):$port;https://localhost:$port`""
    $updated     = [regex]::Replace($content, $pattern, $replacement)

    if ($updated -eq $content) {
        Write-Host "  ----  $app appsettings.Development.json already correct" -ForegroundColor DarkGray
        continue
    }

    Set-Content -Path $path -Value $updated -Encoding utf8 -NoNewline
    Write-Host "  OK    $app appsettings.Development.json -> https://$($IpAddress):$port" -ForegroundColor Green
    $changed++
}

Write-Host ""
Write-Host "Updated $changed file(s) to $IpAddress." -ForegroundColor Green
Write-Host "Restart the apps in Visual Studio for this to take effect." -ForegroundColor Cyan
