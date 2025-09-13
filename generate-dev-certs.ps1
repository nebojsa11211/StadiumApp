# PowerShell script to generate development certificates for Docker HTTPS

Write-Host "Generating .NET development certificates for Docker containers..." -ForegroundColor Green

# Create certificates directory
$certDir = Join-Path $PWD "certificates"
if (-Not (Test-Path $certDir)) {
    New-Item -ItemType Directory -Path $certDir -Force
}

Write-Host "Certificate directory: $certDir" -ForegroundColor Yellow

# Generate development certificate
Write-Host "Generating HTTPS development certificate..." -ForegroundColor Yellow
$certPath = Join-Path $certDir "aspnetcore.pfx"

# Remove any existing certificates first
dotnet dev-certs https --clean

# Generate new certificate
dotnet dev-certs https -ep $certPath -p "StadiumDev123!" --trust

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Certificate generated successfully at: $certPath" -ForegroundColor Green
    Write-Host "✓ Certificate password: StadiumDev123!" -ForegroundColor Green
    
    # Create a certificate info file
    $infoPath = Join-Path $certDir "cert-info.txt"
    @"
ASP.NET Core Development Certificate
Generated: $(Get-Date)
Location: $certPath
Password: StadiumDev123!

This certificate is used for HTTPS in Docker containers.
The certificate is trusted on this machine.
"@ | Out-File -FilePath $infoPath -Encoding UTF8
    
    Write-Host "✓ Certificate info saved to: $infoPath" -ForegroundColor Green
} else {
    Write-Host "✗ Failed to generate certificate" -ForegroundColor Red
    exit 1
}

# Also export for container use (without password for easier mounting)
$containerCertPath = Join-Path $certDir "aspnetcore-container.pfx"
dotnet dev-certs https -ep $containerCertPath -p ""

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Container certificate generated: $containerCertPath" -ForegroundColor Green
} else {
    Write-Host "⚠ Failed to generate container certificate, using password-protected version" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Certificate generation complete!" -ForegroundColor Green
Write-Host "You can now run: docker-compose up --build -d" -ForegroundColor Cyan
Write-Host ""
Write-Host "HTTPS URLs will be available at:" -ForegroundColor Yellow
Write-Host "  - API: https://localhost:9010" -ForegroundColor White
Write-Host "  - Customer: https://localhost:9020" -ForegroundColor White  
Write-Host "  - Admin: https://localhost:9030" -ForegroundColor White
Write-Host "  - Staff: https://localhost:9040" -ForegroundColor White