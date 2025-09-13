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
set "urls[0]=https://localhost:7020"
set "urls[1]=https://localhost:7030"
set "urls[2]=https://localhost:7010/swagger"

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
echo    • Customer App: https://localhost:7020
echo    • Admin App: https://localhost:7030
echo    • API Swagger: https://localhost:7010/swagger
echo.

pause
