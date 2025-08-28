@echo off
echo === Pre-Debug Cleanup for Visual Studio ===
echo Cleaning up Docker containers before debugging...

cd /d "D:\StadionTest"

echo Stopping existing containers...
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down --remove-orphans --volumes

echo Removing any remaining containers...
docker rm -f stadium-sqlserver-debug 2>nul
docker rm -f stadium-api-debug 2>nul
docker rm -f stadium-customer-debug 2>nul
docker rm -f stadium-admin-debug 2>nul

echo Cleaning up networks...
docker network prune -f

echo === Cleanup Complete ===
echo You can now start debugging in Visual Studio
echo Press any key to continue...
pause
