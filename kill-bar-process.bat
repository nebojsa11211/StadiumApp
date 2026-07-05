@echo off
echo Killing StadiumDrinkOrdering.Bar processes...
taskkill /F /IM "StadiumDrinkOrdering.Bar.exe" 2>nul
powershell -Command "Get-Process | Where-Object {$_.ProcessName -eq 'StadiumDrinkOrdering.Bar'} | Stop-Process -Force -ErrorAction SilentlyContinue" 2>nul
exit /b 0
