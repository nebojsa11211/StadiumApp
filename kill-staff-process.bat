@echo off
echo Killing StadiumDrinkOrdering.Staff processes...
taskkill /F /IM "StadiumDrinkOrdering.Staff.exe" 2>nul
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'StadiumDrinkOrdering.Staff'} | Stop-Process -Force -ErrorAction SilentlyContinue" 2>nul
exit /b 0