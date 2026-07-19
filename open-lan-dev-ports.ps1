# Opens the Stadium dev ports to the local network so the apps can be reached from a phone/tablet.
# RUN AS ADMINISTRATOR. Private network profile only - these rules are never applied to Public
# networks, so plugging into cafe wifi does not expose the dev stack.
#
# To undo:  .\open-lan-dev-ports.ps1 -Remove

[CmdletBinding()]
param(
    [switch]$Remove
)

$ErrorActionPreference = 'Stop'

$ruleName = 'StadiumApp Dev (LAN)'
$ports    = @(7010, 7020, 7030, 7040, 7050, 7060)

$identity  = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal($identity)
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "This script must be run as Administrator." -ForegroundColor Red
    Write-Host "Right-click PowerShell -> Run as administrator, then re-run it." -ForegroundColor Yellow
    exit 1
}

$existing = Get-NetFirewallRule -DisplayName $ruleName -ErrorAction SilentlyContinue
if ($existing) {
    $existing | Remove-NetFirewallRule
    Write-Host "Removed existing '$ruleName' rule." -ForegroundColor Yellow
}

if ($Remove) {
    Write-Host "Done - Stadium dev ports are no longer open to the LAN." -ForegroundColor Green
    exit 0
}

New-NetFirewallRule `
    -DisplayName $ruleName `
    -Description 'Stadium Drink Ordering local development apps (API/Customer/Admin/Bar/Simulator/Runner).' `
    -Direction Inbound `
    -Action Allow `
    -Protocol TCP `
    -LocalPort $ports `
    -Profile Private `
    -Enabled True | Out-Null

Write-Host "Opened TCP $($ports -join ', ') on the Private network profile." -ForegroundColor Green
Write-Host ""

$lanIp = (Get-NetIPAddress -AddressFamily IPv4 |
    Where-Object { $_.IPAddress -notlike '127.*' -and $_.PrefixOrigin -eq 'Dhcp' } |
    Select-Object -First 1 -ExpandProperty IPAddress)

if ($lanIp) {
    Write-Host "Reach the apps from another device on this network at:" -ForegroundColor Cyan
    Write-Host "  API       https://$($lanIp):7010" -ForegroundColor White
    Write-Host "  Customer  https://$($lanIp):7020" -ForegroundColor White
    Write-Host "  Admin     https://$($lanIp):7030" -ForegroundColor White
    Write-Host "  Bar       https://$($lanIp):7040" -ForegroundColor White
    Write-Host "  Simulator https://$($lanIp):7050" -ForegroundColor White
    Write-Host "  Runner    https://$($lanIp):7060" -ForegroundColor White
    Write-Host ""
    Write-Host "No LAN certificate is installed yet, so the phone will show an HTTPS warning" -ForegroundColor Yellow
    Write-Host "you must tap through. Service workers stay blocked until a trusted cert exists," -ForegroundColor Yellow
    Write-Host "so the Runner will run as a normal page but will NOT install as a PWA." -ForegroundColor Yellow
}
