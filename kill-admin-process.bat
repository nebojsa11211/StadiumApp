@echo off
echo Killing StadiumDrinkOrdering.Admin processes...
taskkill /F /IM "StadiumDrinkOrdering.Admin.exe" 2>nul
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'StadiumDrinkOrdering.Admin'} | Stop-Process -Force -ErrorAction SilentlyContinue" 2>nul
exit /b 0