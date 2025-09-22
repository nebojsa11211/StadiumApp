@echo off
echo Killing StadiumDrinkOrdering.API processes...
taskkill /F /IM "StadiumDrinkOrdering.API.exe" 2>nul
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'StadiumDrinkOrdering.API'} | Stop-Process -Force -ErrorAction SilentlyContinue" 2>nul
exit /b 0