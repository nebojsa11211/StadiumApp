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
echo 📍 Opening: Customer App (http://localhost:5002)
start http://localhost:5002

timeout /t 1 /nobreak >nul
echo 📍 Opening: Admin App (http://localhost:5003)
start http://localhost:5003

timeout /t 1 /nobreak >nul
echo 📍 Opening: API Swagger (http://localhost:5001/swagger)
start http://localhost:5001/swagger

echo.
echo ✅ All three browser tabs opened successfully!
echo 📋 All services are now ready for debugging
echo.
pause
