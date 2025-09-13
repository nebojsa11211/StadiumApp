@echo off
echo ========================================
echo 🚀 Auto Multi-Tab Launcher for Visual Studio
echo ========================================
echo.

:: Wait for services to start
echo ⏳ Waiting for Docker services to start...
timeout /t 15 /nobreak >nul

:: URLs to open in separate tabs
echo 🌐 Opening all Stadium services in browser tabs...
echo 📍 Opening: Customer App (https://localhost:7020)
start https://localhost:7020

timeout /t 1 /nobreak >nul
echo 📍 Opening: Admin App (https://localhost:7030)
start https://localhost:7030

timeout /t 1 /nobreak >nul
echo 📍 Opening: API Swagger (https://localhost:7010/swagger)
start https://localhost:7010/swagger

echo.
echo ✅ All three browser tabs opened successfully!
echo 📋 All services are now ready for debugging
echo.
pause
