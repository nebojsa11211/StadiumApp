@echo off
setlocal enabledelayedexpansion

rem ====================================================================
rem  One ngrok tunnel for all six Stadium apps, through a local proxy.
rem
rem  The free ngrok plan has ONE static domain and ONE online agent per
rem  account, so a tunnel per app is impossible - the second one dies with
rem  ERR_NGROK_334 and its window closes before the error can be read.
rem  Instead ngrok tunnels tools\proxy\proxy.js on https://localhost:7000,
rem  which routes by path prefix:
rem
rem      /backend   API                 7010   (Swagger at /backend/swagger)
rem      /customer  Customer            7020
rem      /admin     Admin               7030
rem      /bar       Bar                 7040
rem      /sim       TicketingSimulator  7050
rem      /runner    Runner (WASM)       7060
rem
rem  IMPORTANT: start the apps on their "ngrok" launch profile, not "https".
rem  That profile sets Proxy__Enabled / Proxy__Prefix (and binds localhost
rem  only); without them an app composes localhost URLs and its pages load
rem  CSS/scripts from the root of the public hostname, which is nobody's.
rem
rem  "start-ngrok.bat auto" does the same work without any question. That is
rem  how TunnelLauncher (StadiumDrinkOrdering.Shared\Hosting\TunnelLauncher.cs)
rem  calls it when an app starts on the "ngrok" profile and the tunnel is not
rem  up, so F5 in Visual Studio is the whole procedure. At that moment no app
rem  is listening yet - not even the caller - so the app checks are skipped.
rem  Error pauses are kept: in "auto" mode this runs in its own window, which
rem  would otherwise vanish together with the only explanation.
rem
rem  Once per machine: ngrok on PATH + "ngrok config add-authtoken <token>"
rem  (the token is a password - it is in no file in this repo), and Node.js.
rem  The proxy's node_modules are installed below on first run.
rem
rem  See Ngrok.md.
rem ====================================================================

rem  The domain MUST be passed (--url). Without it ngrok opens the tunnel on a
rem  random hostname and the address everybody has written down stays offline
rem  (ERR_NGROK_3200) although the tunnel is running fine.
set "DOMAIN=lustrous-unfrozen-legacy.ngrok-free.dev"
set "PROXY_PORT=7000"
set "PROXY=%~dp0tools\proxy\proxy.js"
for %%D in ("%PROXY%") do set "PROXYDIR=%%~dpD"
set "MISSING="
set "AUTO="
if /i "%~1"=="auto" set "AUTO=1"

where ngrok >nul 2>&1
if errorlevel 1 (
  echo ERROR: ngrok is not on PATH.
  pause
  exit /b 1
)

if defined AUTO goto :proxy

echo ========================================
echo   Checking apps
echo ========================================

call :check 7010 API                /backend
call :check 7020 Customer           /customer
call :check 7030 Admin              /admin
call :check 7040 Bar                /bar
call :check 7050 TicketingSimulator /sim
call :check 7060 Runner             /runner

if defined MISSING (
  echo.
  echo Not running:!MISSING!
  echo Start them in Visual Studio on the "ngrok" profile.
  echo Their paths will answer 502 through the tunnel.
  echo.
  set /p "GOON=Continue anyway? (y/n): "
  if /i not "!GOON!"=="y" exit /b 1
)

:proxy
if defined AUTO (
  echo ========================================
  echo   Tunnel started by an app
  echo ========================================
  echo   The apps are still starting, so they are not checked.
  echo   Until each one is up, its path answers 502 through the tunnel.
  echo.
)

rem --- proxy on 7000 ---
netstat -ano | findstr /c:"LISTENING" | findstr /c:":%PROXY_PORT% " >nul
if errorlevel 1 (
  echo.
  echo Proxy is not running, starting it...
  if not exist "%PROXY%" (
    echo ERROR: proxy not found at %PROXY%
    pause
    exit /b 1
  )
  call :dependencies
  if errorlevel 1 exit /b 1
  rem  cmd /k, not /c: if node crashes (port taken, broken proxy.js) the error
  rem  stays on screen instead of disappearing with the window.
  rem  --watch: node restarts the proxy itself when proxy.js changes. The proxy
  rem  window outlives Stop Debugging, so without it an edited proxy.js keeps
  rem  running the OLD code until someone closes that window by hand.
  start "Stadium proxy (%PROXY_PORT%)" cmd /k node --watch "%PROXY%"
  call :waitproxy
  if errorlevel 1 (
    echo ERROR: the proxy did not start listening on %PROXY_PORT% within 20 seconds.
    echo   The reason is in the "Stadium proxy (%PROXY_PORT%)" window.
    pause
    exit /b 1
  )
  echo   proxy OK
) else (
  echo   proxy already running on %PROXY_PORT%
)

rem --- are the Blazor apps on the "ngrok" profile? ---
rem  An app on "ngrok" renders <base href="/admin/">; one on "https" renders
rem  <base href="/"> and its page breaks behind the proxy. Skipped in "auto"
rem  mode: the apps are only starting and every check would just time out.
if defined AUTO goto :tunnel
call :profile admin Admin
call :profile customer Customer
call :profile bar Bar
call :profile sim TicketingSimulator

:tunnel
rem --- only one ngrok agent may be online per account (ERR_NGROK_334) ---
tasklist /fi "imagename eq ngrok.exe" | findstr /i ngrok.exe >nul
if not errorlevel 1 (
  echo.
  echo ngrok is already running. Stopping it - the account has one domain...
  taskkill /im ngrok.exe /f >nul 2>&1
  timeout /t 2 >nul
)

echo.
echo ========================================
echo  https://%DOMAIN%/backend/swagger  API
echo  https://%DOMAIN%/customer/        Customer
echo  https://%DOMAIN%/admin/           Admin
echo  https://%DOMAIN%/bar/             Bar
echo  https://%DOMAIN%/sim/             TicketingSimulator
echo  https://%DOMAIN%/runner/          Runner
echo.
echo  Tunnel goes to the proxy https://localhost:%PROXY_PORT%
echo  Traffic inspector: http://127.0.0.1:4040 (ngrok's own loopback UI)
echo  CTRL+C stops the tunnel - and with it the public addresses.
echo ========================================
echo.

rem  Upstream is HTTPS (project rule). ngrok does not verify the local dev
rem  certificate, and TLS to the outside world uses ngrok's own certificate.
ngrok http https://localhost:%PROXY_PORT% --url=https://%DOMAIN%
exit /b 0

:check
netstat -ano | findstr /c:"LISTENING" | findstr /c:":%~1 " >nul
if errorlevel 1 (
  echo   %~3  %~2 ^(%~1^) - NOT RUNNING
  set "MISSING=!MISSING! %~2"
) else (
  echo   %~3  %~2 ^(%~1^) - OK
)
exit /b 0

:profile
set "BASE="
curl -k -s -m 10 "https://localhost:%PROXY_PORT%/%~1/" 2>nul | findstr /c:"<base href=" > "%TEMP%\stadium-ngrok-base.txt"
if errorlevel 1 exit /b 0
findstr /c:"/%~1/" "%TEMP%\stadium-ngrok-base.txt" >nul
if errorlevel 1 (
  echo.
  echo WARNING: %~2 is running on the "https" profile, not "ngrok".
  echo   Through the tunnel its page loads without CSS and links leave /%~1.
  echo   Stop it and start it again on the "ngrok" profile.
)
del "%TEMP%\stadium-ngrok-base.txt" >nul 2>&1
exit /b 0

rem ====================================================================
rem  Wait for the proxy instead of sleeping a fixed time: on first run,
rem  right after npm install while antivirus scans the fresh packages, node
rem  can take a few seconds. Poll once a second, up to 20 times.
rem ====================================================================
:waitproxy
for /l %%I in (1,1,20) do (
  netstat -ano | findstr /c:"LISTENING" | findstr /c:":%PROXY_PORT% " >nul
  if not errorlevel 1 exit /b 0
  timeout /t 1 >nul
)
exit /b 1

rem ====================================================================
rem  node looks for node_modules next to the script, so install there.
rem  package.json / package-lock.json are in git, node_modules is ignored.
rem ====================================================================
:dependencies
if exist "%PROXYDIR%node_modules" exit /b 0
echo   no node_modules, installing proxy dependencies...
where npm >nul 2>&1
if errorlevel 1 (
  echo ERROR: npm is not on PATH, and the proxy needs express and http-proxy-middleware.
  pause
  exit /b 1
)
pushd "%PROXYDIR%"
call npm install --no-audit --no-fund
popd
if not exist "%PROXYDIR%node_modules" (
  echo ERROR: npm install failed in %PROXYDIR%
  pause
  exit /b 1
)
echo   dependencies OK
exit /b 0
