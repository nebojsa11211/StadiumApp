# Mints the LAN development certificate used by the Runner (and any other app bound to the LAN IP).
#
# The ASP.NET dev certificate is only valid for the name "localhost", so hitting an app by IP from a
# phone produces a certificate warning. This script mints a self-signed cert whose SANs include the
# LAN IP, exports it as certificates\stadium-lan.pfx (private key, gitignored) plus
# certificates\stadium-lan.cer (public half, committed - this is the file you install on a phone).
#
#   .\generate-lan-cert.ps1                        # auto-detect the current LAN IP
#   .\generate-lan-cert.ps1 -IpAddress 192.168.178.40
#   .\generate-lan-cert.ps1 -Trust                 # also add it to this machine's trusted roots
#
# The .pfx is *.pfx-ignored by git, so every developer/machine runs this once. Re-running mints a NEW
# cert: the .cer changes, so any phone that already trusts the old one must install the new .cer.

[CmdletBinding()]
param(
    [string]$IpAddress,
    [switch]$Trust
)

$ErrorActionPreference = 'Stop'

$certDir  = Join-Path $PSScriptRoot 'certificates'
$pfxPath  = Join-Path $certDir 'stadium-lan.pfx'
$cerPath  = Join-Path $certDir 'stadium-lan.cer'
$password = 'StadiumDev123!'   # matches Kestrel__Certificates__Default__Password in launchSettings.json

if (-not (Test-Path $certDir)) {
    New-Item -ItemType Directory -Path $certDir -Force | Out-Null
}

if (-not $IpAddress) {
    $candidates = Get-NetIPAddress -AddressFamily IPv4 |
        Where-Object { $_.IPAddress -notlike '127.*' -and $_.PrefixOrigin -eq 'Dhcp' }

    if (-not $candidates) {
        Write-Host "Could not auto-detect a LAN IPv4 address. Pass one explicitly:" -ForegroundColor Red
        Write-Host "  .\generate-lan-cert.ps1 -IpAddress 192.168.1.50" -ForegroundColor Yellow
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

# "api" is the docker-compose service name; keeping it here means the same cert works in containers.
# The LAN address MUST go in as an IPAddress SAN, not a DNS one: browsers reject a DNS entry that
# merely looks like an IP when the url is a bare IP literal. That rules out -DnsName (which would
# encode it as DNS), so the whole SAN extension (2.5.29.17) is written by hand below.
$dnsNames = @('localhost', 'api', $env:COMPUTERNAME) | Where-Object { $_ } | Select-Object -Unique
$sanText  = (($dnsNames | ForEach-Object { "DNS=$_" }) + "IPAddress=$IpAddress") -join '&'

Write-Host "Minting certificate for: $($dnsNames -join ', '), $IpAddress" -ForegroundColor Yellow

$cert = New-SelfSignedCertificate `
    -Subject 'CN=StadiumApp LAN Dev Cert' `
    -CertStoreLocation 'Cert:\CurrentUser\My' `
    -KeyExportPolicy Exportable `
    -KeyLength 2048 `
    -KeyAlgorithm RSA `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment `
    -TextExtension @('2.5.29.37={text}1.3.6.1.5.5.7.3.1', "2.5.29.17={text}$sanText") `
    -NotAfter (Get-Date).AddYears(2)

$securePassword = ConvertTo-SecureString -String $password -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath $pfxPath -Password $securePassword | Out-Null
Export-Certificate  -Cert $cert -FilePath $cerPath -Type CERT | Out-Null

Write-Host "  OK    $pfxPath  (password: $password)" -ForegroundColor Green
Write-Host "  OK    $cerPath  (install this one on phones)" -ForegroundColor Green
Write-Host "  Thumbprint: $($cert.Thumbprint)" -ForegroundColor DarkGray

if ($Trust) {
    # Windows shows a confirmation dialog when writing to the CurrentUser root store - click Yes.
    $root = New-Object System.Security.Cryptography.X509Certificates.X509Store('Root', 'CurrentUser')
    $root.Open('ReadWrite')
    $root.Add($cert)
    $root.Close()
    Write-Host "  OK    added to Cert:\CurrentUser\Root (desktop browsers will trust it)" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "Not trusted yet. To silence the desktop browser warning, re-run with -Trust," -ForegroundColor Yellow
    Write-Host "or double-click stadium-lan.cer -> Install Certificate -> Trusted Root Certification Authorities." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Done. Restart the apps in Visual Studio for this to take effect." -ForegroundColor Cyan
Write-Host "If the LAN IP changed, also run: .\set-dev-host.ps1 -IpAddress $IpAddress" -ForegroundColor Cyan
