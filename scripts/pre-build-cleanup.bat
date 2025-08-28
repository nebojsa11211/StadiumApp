@echo off
echo Cleaning up Docker containers before build...
cd /d "%~dp0.."
powershell -ExecutionPolicy Bypass -File "scripts\vs-debug-start.ps1" "%CD%"
echo Pre-build cleanup completed.
