@echo off
echo Killing StadiumDrinkOrdering.Customer processes...
taskkill /F /IM "StadiumDrinkOrdering.Customer.exe" 2>nul
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'StadiumDrinkOrdering.Customer'} | Stop-Process -Force -ErrorAction SilentlyContinue" 2>nul
exit /b 0