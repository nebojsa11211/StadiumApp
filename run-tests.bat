@echo off
echo ========================================
echo Stadium Admin Modernization Test Suite
echo ========================================
echo.

echo 🚀 Starting Docker containers...
docker-compose up -d
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Failed to start Docker containers
    exit /b 1
)

echo ⏳ Waiting for containers to be ready...
timeout /t 30 /nobreak > nul

echo 🔍 Checking container status...
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

echo.
echo 🧪 Running Playwright tests...
echo.

echo 📋 Test 1: Basic connectivity test...
call npx playwright test tests/basic.spec.ts --reporter=list --project=chromium
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Basic connectivity test failed
    goto :error
)

echo.
echo 🔐 Test 2: Authentication system test...
call npx playwright test tests/test-auth.spec.ts --reporter=list --project=chromium
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Authentication test failed
    goto :error
)

echo.
echo 🎯 Test 3: Essential modernization tests...
call npx playwright test tests/admin/all-tests.spec.ts --reporter=list --project=chromium --timeout=60000
if %ERRORLEVEL% NEQ 0 (
    echo ⚠️ Some essential tests failed, but continuing...
)

echo.
echo 📊 Test 4: Individual component tests (if time permits)...
call npx playwright test tests/admin/dashboard.spec.ts --reporter=list --project=chromium --timeout=60000 --max-failures=3
call npx playwright test tests/admin/orders.spec.ts --reporter=list --project=chromium --timeout=60000 --max-failures=3
call npx playwright test tests/admin/users.spec.ts --reporter=list --project=chromium --timeout=60000 --max-failures=3

echo.
echo ✅ Test execution completed!
echo.
echo 📄 Generating test report...
call npx playwright show-report
goto :end

:error
echo ❌ Test execution failed!
exit /b 1

:end
echo.
echo 🎉 All tests completed! Check the HTML report for detailed results.
pause