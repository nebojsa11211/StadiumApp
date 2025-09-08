@echo off
echo Opening Stadium Drink Ordering Applications...
echo.
echo Customer App: http://localhost:9001
echo Admin App: http://localhost:9002
echo.
start http://localhost:9001
timeout /t 2 /nobreak > nul
start http://localhost:9002
echo.
echo Both applications opened in Chrome!
pause