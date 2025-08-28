@echo off
setlocal enabledelayedexpansion

echo ========================================
echo 🚀 Stadium Drink Ordering - Multi-Tab Launcher
echo ========================================
echo.

:: Wait for services to start
echo ⏳ Waiting for services to start...
timeout /t 15 /nobreak >nul

:: URLs to open
set "urls[0]=http://localhost:5002"
set "urls[1]=http://localhost:5003"
set "urls[2]=http://localhost:5001/swagger"

:: Open all URLs in browser tabs
echo 🌐 Opening browser tabs...
for /L %%i in (0,1,2) do (
    echo 📍 Opening: !urls[%%i]!
    start !urls[%%i]!
    timeout /t 1 /nobreak >nul
)

echo.
echo ✅ All browser tabs opened successfully!
echo 📋 Opened URLs:
echo    • Customer App: http://localhost:5002
echo    • Admin App: http://localhost:5003
echo    • API Swagger: http://localhost:5001/swagger
echo.

pause
