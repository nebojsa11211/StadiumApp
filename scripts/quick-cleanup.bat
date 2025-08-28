@echo off
echo === Docker Cleanup for Visual Studio ===
echo Cleaning up existing containers...

cd /d "D:\StadionTest"

echo Stopping and removing containers...
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down --remove-orphans --volumes

echo Force removing any remaining containers...
docker rm -f stadium-sqlserver-debug 2>nul
docker rm -f stadium-api-debug 2>nul
docker rm -f stadium-customer-debug 2>nul
docker rm -f stadium-admin-debug 2>nul
docker rm -f stadium-sqlserver-dev 2>nul
docker rm -f stadiumdrinkordering.api 2>nul
docker rm -f stadiumdrinkordering.customer 2>nul
docker rm -f stadiumdrinkordering.admin 2>nul

echo Cleaning up networks...
docker network prune -f

echo === Cleanup Complete ===
echo You can now start debugging in Visual Studio
pause
