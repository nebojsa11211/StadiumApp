const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

async function testAdminLoginTimeoutFix() {
    console.log('🔍 Testing Admin UI Login Timeout Fix');
    console.log('=====================================');

    const browser = await chromium.launch({
        headless: false,
        slowMo: 1000 // Add delay for visibility
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
        await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });
        await page.waitForSelector('#admin-login-password-input', { timeout: 10000 });
        await page.waitForSelector('#admin-login-submit-btn', { timeout: 10000 });

        // Fill credentials
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');

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
        await page.click('#admin-login-submit-btn');
        console.log('🖱️  Login button clicked');

        // Wait for either success (redirect) or error message
        const loginPromise = Promise.race([
            // Success: redirect to dashboard or another page
            page.waitForURL(url => typeof url === 'string' && !url.includes('/login'), { timeout: 25000 }).then(() => 'SUCCESS'),

            // Error: error message appears
            page.waitForSelector('#admin-login-error, .alert-danger, [class*="error"]', { timeout: 25000 }).then(() => 'ERROR'),

            // Loading state changes
            page.waitForFunction(() => {
                const button = document.querySelector('#admin-login-submit-btn');
                return button && !button.disabled;
            }, { timeout: 25000 }).then(() => 'COMPLETED')
        ]);

        const result = await loginPromise;
        const responseTime = Date.now() - startTime;

        console.log(`⏱️  Response time: ${responseTime}ms (${(responseTime/1000).toFixed(1)}s)`);

        // Step 4: Capture final state
        console.log('\n📸 Step 4: Capture final state...');

        await page.screenshot({
            path: path.join(screenshotDir, 'timeout-fix-03-after-login.png'),
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-fix-03-after-login.png');

        // Step 5: Analyze results
        console.log('\n📊 Step 5: Analyze results...');

        // Check for error messages
        const errorElements = await page.$$('#admin-login-error, .alert-danger, [class*="error"]');
        let errorMessage = '';

        if (errorElements.length > 0) {
            errorMessage = await errorElements[0].textContent();
            console.log(`❌ Error message displayed: "${errorMessage}"`);
        }

        // Check current URL
        const currentUrl = page.url();
        console.log(`🌐 Current URL: ${currentUrl}`);

        // Check if login was successful (redirected away from login page)
        const loginSuccessful = !currentUrl.includes('/login');

        // Results summary
        console.log('\n📋 TEST RESULTS SUMMARY');
        console.log('======================');
        console.log(`🎯 Objective: Verify HttpClient timeout fix (< 20 seconds)`);
        console.log(`⏱️  Response Time: ${responseTime}ms (${(responseTime/1000).toFixed(1)}s)`);
        console.log(`✅ Timeout Fix Working: ${responseTime < 20000 ? 'YES' : 'NO'}`);
        console.log(`🔐 Login Result: ${result}`);
        console.log(`🌐 Login Successful: ${loginSuccessful ? 'YES' : 'NO'}`);
        console.log(`❌ Error Message: ${errorMessage || 'None'}`);

        // Performance analysis
        if (responseTime < 15000) {
            console.log('🟢 EXCELLENT: Response time under 15 seconds');
        } else if (responseTime < 20000) {
            console.log('🟡 GOOD: Response time under 20 seconds (timeout fix working)');
        } else {
            console.log('🔴 ISSUE: Response time over 20 seconds (possible timeout problem)');
        }

        // Previous vs current behavior
        console.log('\n📈 IMPROVEMENT ANALYSIS');
        console.log('=======================');
        console.log('❌ Previous behavior: 120+ second hang with no response');
        console.log(`✅ Current behavior: ${(responseTime/1000).toFixed(1)}s response with UI feedback`);
        console.log(`📉 Improvement: ${Math.round((120000 - responseTime) / 1000)}+ seconds faster`);

        return {
            success: true,
            responseTime: responseTime,
            loginSuccessful: loginSuccessful,
            errorMessage: errorMessage,
            timeoutFixWorking: responseTime < 20000
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
        if (result.success && result.timeoutFixWorking) {
            console.log('✅ TIMEOUT FIX VERIFICATION: SUCCESS');
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