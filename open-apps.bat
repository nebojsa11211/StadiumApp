@echo off
echo Opening Stadium Drink Ordering Applications...
echo.
echo Customer App: https://localhost:9020
echo Admin App: https://localhost:9030
echo.
start https://localhost:9020
timeout /t 2 /nobreak > nul
start https://localhost:9030
echo.
echo Both applications opened in Chrome!
pause