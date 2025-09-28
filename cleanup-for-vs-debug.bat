@echo off
REM =============================================================================
REM Stadium Drink Ordering - Visual Studio Debug Cleanup Script (Batch Version)
REM =============================================================================
REM This script kills all running StadiumDrinkOrdering dotnet processes
REM to prepare for clean Visual Studio debugging sessions.
REM
REM Usage: Double-click this file or run from command prompt
REM =============================================================================

echo.
echo 🔄 Stadium Drink Ordering - Visual Studio Debug Cleanup
echo =========================================================

echo.
echo 🔍 Stopping all dotnet processes...

REM Kill all dotnet processes
taskkill /IM dotnet.exe /F >nul 2>&1
if %ERRORLEVEL% == 0 (
    echo ✅ All dotnet processes stopped
) else (
    echo ℹ️  No dotnet processes found
)

echo.
echo 🔍 Checking for processes on ports 7010 and 7030...

REM Check and kill processes using port 7010 (API)
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :7010') do (
    if "%%a" NEQ "" (
        echo ⚠️  Killing process %%a using port 7010
        taskkill /PID %%a /F >nul 2>&1
    )
)

REM Check and kill processes using port 7030 (Admin)
for /f "tokens=5" %%a in ('netstat -aon ^| findstr :7030') do (
    if "%%a" NEQ "" (
        echo ⚠️  Killing process %%a using port 7030
        taskkill /PID %%a /F >nul 2>&1
    )
)

echo.
echo ✅ Port cleanup completed
echo.
echo 🎯 Cleanup completed! Ready for Visual Studio debugging.
echo =========================================================
echo You can now start debugging in Visual Studio:
echo   1. Set StadiumDrinkOrdering.API as startup project
echo   2. Press F5 to start API debugging
echo   3. Set StadiumDrinkOrdering.Admin as startup project
echo   4. Press F5 to start Admin debugging
echo.
pause