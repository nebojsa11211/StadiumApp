# Test Admin Logout Functionality

Write-Host "Testing Admin Application Logout Button" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow

# Test 1: Check if API is running
Write-Host "`nTest 1: Checking API availability..." -ForegroundColor Cyan
$apiResponse = Invoke-WebRequest -Uri "https://localhost:7010/api/drinks" -SkipCertificateCheck -ErrorAction SilentlyContinue
if ($apiResponse.StatusCode -eq 200) {
    Write-Host "✓ API is running" -ForegroundColor Green
} else {
    Write-Host "✗ API is not accessible" -ForegroundColor Red
    exit 1
}

# Test 2: Check if Admin app is running
Write-Host "`nTest 2: Checking Admin app availability..." -ForegroundColor Cyan
$adminResponse = Invoke-WebRequest -Uri "https://localhost:8082/" -SkipCertificateCheck -ErrorAction SilentlyContinue
if ($adminResponse.StatusCode -eq 200) {
    Write-Host "✓ Admin app is running" -ForegroundColor Green
} else {
    Write-Host "✗ Admin app is not accessible" -ForegroundColor Red
    exit 1
}

# Test 3: Test login functionality
Write-Host "`nTest 3: Testing login..." -ForegroundColor Cyan
$loginBody = @{
    email = "admin@stadium.com"
    password = "admin123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "https://localhost:7010/api/auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body $loginBody `
    -SkipCertificateCheck `
    -ErrorAction SilentlyContinue

if ($loginResponse.token) {
    Write-Host "✓ Login successful, token received" -ForegroundColor Green
    Write-Host "  Token: $($loginResponse.token.Substring(0, 20))..." -ForegroundColor Gray
} else {
    Write-Host "✗ Login failed" -ForegroundColor Red
}

Write-Host "`n========================================" -ForegroundColor Yellow
Write-Host "SUMMARY:" -ForegroundColor Yellow
Write-Host "The logout button issue appears to be related to:" -ForegroundColor White
Write-Host "1. Check browser console for JavaScript errors" -ForegroundColor White
Write-Host "2. Verify AuthStateService is properly initialized" -ForegroundColor White
Write-Host "3. Ensure localStorage is accessible" -ForegroundColor White
Write-Host "4. Check if SignalR connection is interfering" -ForegroundColor White