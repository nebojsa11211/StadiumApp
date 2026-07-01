@echo off
rem Releases the locked StadiumDrinkOrdering.API.exe before a CLI build.
rem Single taskkill by image name - no PowerShell spawn (that added ~0.5-1s per build).
taskkill /F /IM "StadiumDrinkOrdering.API.exe" >nul 2>&1
exit /b 0