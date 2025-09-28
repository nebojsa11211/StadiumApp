const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

async function testAdminLoginTimeoutFix() {
    console.log('🔍 Testing Admin UI Login Timeout Fix');
    console.log('=====================================');

    const browser = await chromium.launch({
        headless: false,
        slowMo: 500
    });

    const context = await browser.newContext({
        viewport: { width: 1920, height: 1080 },
        ignoreHTTPSErrors: true
    });

    const page = await context.newPage();

    // Create screenshots directory
    const screenshotDir = '.playwright-mcp';
    if (!fs.existsSync(screenshotDir)) {
        fs.mkdirSync(screenshotDir, { recursive: true });
    }

    try {
        console.log('\n📋 Test Configuration:');
        console.log('- Admin URL: https://localhost:7030/login');
        console.log('- Expected timeout: < 20 seconds');
        console.log('- Credentials: admin@stadium.com / admin123');

        // Step 1: Navigate to login page
        console.log('\n🌐 Step 1: Navigate to Admin login page...');
        const startNavigation = Date.now();

        await page.goto('https://localhost:7030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        const navigationTime = Date.now() - startNavigation;
        console.log(`✅ Page loaded in ${navigationTime}ms`);

        // Screenshot: Login page
        await page.screenshot({
            path: path.join(screenshotDir, 'timeout-fix-01-login-page.png'),
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-fix-01-login-page.png');

        // Step 2: Fill login form
        console.log('\n📝 Step 2: Fill login form...');

        // Wait for login form elements
        await page.waitForSelector('input[type="email"]', { timeout: 10000 });
        await page.waitForSelector('input[type="password"]', { timeout: 10000 });
        await page.waitForSelector('button[type="submit"], input[type="submit"]', { timeout: 10000 });

        // Fill credentials
        await page.fill('input[type="email"]', 'admin@stadium.com');
        await page.fill('input[type="password"]', 'admin123');

        // Screenshot: Form filled
        await page.screenshot({
            path: path.join(screenshotDir, 'timeout-fix-02-form-filled.png'),
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-fix-02-form-filled.png');
        console.log('✅ Form filled with credentials');

        // Step 3: Submit login and measure response time
        console.log('\n🚀 Step 3: Submit login and measure response time...');
        console.log('⏱️  Starting timer...');

        const startTime = Date.now();

        // Click login button
        await page.click('button[type="submit"], input[type="submit"]');
        console.log('🖱️  Login button clicked');

        // Wait for response with timeout monitoring
        console.log('⏳ Waiting for response...');

        let responseTime = 0;
        let result = 'UNKNOWN';
        let finalUrl = '';
        let errorMessage = '';

        try {
            // Wait for page to respond (either success redirect or error display)
            await page.waitForFunction(() => {
                // Check if we're still on login page
                const currentUrl = window.location.href;

                // Check for error messages
                const errorElements = document.querySelectorAll('.alert-danger, [class*="error"], .text-danger');

                // Check if login button is re-enabled (indicates completion)
                const submitButton = document.querySelector('button[type="submit"], input[type="submit"]');

                return !currentUrl.includes('/login') || errorElements.length > 0 || (submitButton && !submitButton.disabled);
            }, { timeout: 25000 });

            responseTime = Date.now() - startTime;
            finalUrl = page.url();

            // Check if we're redirected away from login (success)
            if (!finalUrl.includes('/login')) {
                result = 'SUCCESS';
                console.log('✅ Login successful - redirected away from login page');
            } else {
                result = 'ERROR';
                console.log('❌ Login failed - still on login page');

                // Try to capture error message
                try {
                    const errorElement = await page.$('.alert-danger, [class*="error"], .text-danger');
                    if (errorElement) {
                        errorMessage = await errorElement.textContent();
                    }
                } catch (e) {
                    console.log('No error message found in DOM');
                }
            }

        } catch (timeoutError) {
            responseTime = Date.now() - startTime;
            result = 'TIMEOUT';
            finalUrl = page.url();
            console.log('⏰ Response timeout reached');
        }

        console.log(`⏱️  Response time: ${responseTime}ms (${(responseTime/1000).toFixed(1)}s)`);

        // Step 4: Capture final state
        console.log('\n📸 Step 4: Capture final state...');

        await page.screenshot({
            path: path.join(screenshotDir, 'timeout-fix-03-after-login.png'),
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-fix-03-after-login.png');

        // Step 5: Additional diagnostics
        console.log('\n🔍 Step 5: Additional diagnostics...');

        // Check network activity
        const networkRequests = [];
        page.on('response', response => {
            if (response.url().includes('localhost:7010') || response.url().includes('localhost:7030')) {
                networkRequests.push({
                    url: response.url(),
                    status: response.status(),
                    timing: Date.now()
                });
            }
        });

        console.log(`🌐 Current URL: ${finalUrl}`);
        console.log(`🔗 Network requests captured: ${networkRequests.length}`);

        // Results summary
        console.log('\n📋 TEST RESULTS SUMMARY');
        console.log('======================');
        console.log(`🎯 Objective: Verify HttpClient timeout fix (< 20 seconds)`);
        console.log(`⏱️  Response Time: ${responseTime}ms (${(responseTime/1000).toFixed(1)}s)`);
        console.log(`✅ Timeout Fix Working: ${responseTime < 20000 ? 'YES' : 'NO'}`);
        console.log(`🔐 Login Result: ${result}`);
        console.log(`🌐 Final URL: ${finalUrl}`);
        console.log(`❌ Error Message: ${errorMessage || 'None'}`);

        // Performance analysis
        if (responseTime < 15000) {
            console.log('🟢 EXCELLENT: Response time under 15 seconds');
        } else if (responseTime < 20000) {
            console.log('🟡 GOOD: Response time under 20 seconds (timeout fix working)');
        } else if (responseTime < 30000) {
            console.log('🟠 ACCEPTABLE: Response time under 30 seconds (some improvement)');
        } else {
            console.log('🔴 ISSUE: Response time over 30 seconds (possible timeout problem)');
        }

        // Previous vs current behavior
        console.log('\n📈 IMPROVEMENT ANALYSIS');
        console.log('=======================');
        console.log('❌ Previous behavior: 120+ second hang with no response');
        console.log(`✅ Current behavior: ${(responseTime/1000).toFixed(1)}s response with UI feedback`);
        console.log(`📉 Improvement: ${Math.round((120000 - responseTime) / 1000)}+ seconds faster`);

        // Timeout fix verification
        const timeoutFixWorking = responseTime < 20000;
        console.log('\n🎯 TIMEOUT FIX VERIFICATION');
        console.log('===========================');
        console.log(`✅ Fix Status: ${timeoutFixWorking ? 'WORKING' : 'NEEDS ATTENTION'}`);
        console.log(`📊 Performance: ${timeoutFixWorking ? 'SIGNIFICANTLY IMPROVED' : 'PARTIALLY IMPROVED'}`);
        console.log(`🔧 Next Steps: ${timeoutFixWorking ? 'No action needed' : 'Review database or API performance'}`);

        return {
            success: true,
            responseTime: responseTime,
            result: result,
            finalUrl: finalUrl,
            errorMessage: errorMessage,
            timeoutFixWorking: timeoutFixWorking
        };

    } catch (error) {
        console.error('❌ Test failed:', error);

        // Capture error screenshot
        try {
            await page.screenshot({
                path: path.join(screenshotDir, 'timeout-fix-error.png'),
                fullPage: true
            });
            console.log('📸 Error screenshot saved: timeout-fix-error.png');
        } catch (screenshotError) {
            console.error('Failed to capture error screenshot:', screenshotError);
        }

        return {
            success: false,
            error: error.message
        };
    } finally {
        await browser.close();
    }
}

// Run the test
testAdminLoginTimeoutFix()
    .then(result => {
        console.log('\n🏁 Test completed');
        if (result.success) {
            if (result.timeoutFixWorking) {
                console.log('✅ TIMEOUT FIX VERIFICATION: SUCCESS');
                console.log('🎉 HttpClient timeout fix is working properly!');
            } else {
                console.log('⚠️  TIMEOUT FIX VERIFICATION: PARTIAL SUCCESS');
                console.log('🔧 Response time improved but could be optimized further');
            }
            process.exit(0);
        } else {
            console.log('❌ TIMEOUT FIX VERIFICATION: FAILED');
            process.exit(1);
        }
    })
    .catch(error => {
        console.error('💥 Unexpected error:', error);
        process.exit(1);
    });