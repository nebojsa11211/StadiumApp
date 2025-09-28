const { chromium } = require('playwright');

async function testAdminLoginTimeout() {
    const browser = await chromium.launch({
        headless: false,
        slowMo: 500
    });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 }
        });

        const page = await context.newPage();

        console.log('🎯 Starting Admin UI authentication timeout test...');
        console.log('Expected: HttpClient timeout within 15-20 seconds');
        console.log('');

        // Step 1: Navigate to admin login page
        console.log('Step 1: Navigating to admin login page...');
        const navStartTime = Date.now();

        await page.goto('https://localhost:7030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        const navEndTime = Date.now();
        console.log(`✅ Page loaded in ${navEndTime - navStartTime}ms`);

        // Take initial screenshot
        await page.screenshot({
            path: '.playwright-mcp/timeout-test-01-login-page.png',
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-test-01-login-page.png');

        // Step 2: Fill in login form
        console.log('');
        console.log('Step 2: Filling login form with admin credentials...');

        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');

        // Take screenshot with form filled
        await page.screenshot({
            path: '.playwright-mcp/timeout-test-02-form-filled.png',
            fullPage: true
        });
        console.log('📸 Screenshot saved: timeout-test-02-form-filled.png');

        // Step 3: Click login and measure response time
        console.log('');
        console.log('Step 3: Clicking login button and measuring response time...');
        console.log('⏱️  Starting timer...');

        const loginStartTime = Date.now();
        console.log(`Login attempt started at: ${new Date(loginStartTime).toISOString()}`);

        // Click the login button
        await page.click('#admin-login-submit-btn');

        // Wait for either success redirect or error message with timeout
        const timeoutPromise = new Promise((resolve) => {
            setTimeout(() => resolve('TIMEOUT_REACHED'), 25000); // 25 second max wait
        });

        const errorPromise = page.waitForSelector('.alert-danger', {
            timeout: 25000
        }).then(() => 'ERROR_DISPLAYED').catch(() => 'NO_ERROR');

        const redirectPromise = page.waitForURL('**/dashboard', {
            timeout: 25000
        }).then(() => 'REDIRECT_SUCCESS').catch(() => 'NO_REDIRECT');

        // Race between timeout, error, and redirect
        const result = await Promise.race([timeoutPromise, errorPromise, redirectPromise]);

        const loginEndTime = Date.now();
        const responseTime = loginEndTime - loginStartTime;

        console.log(`Login attempt ended at: ${new Date(loginEndTime).toISOString()}`);
        console.log(`⏱️  Total response time: ${responseTime}ms (${(responseTime / 1000).toFixed(2)} seconds)`);

        // Step 4: Analyze results
        console.log('');
        console.log('Step 4: Analyzing results...');
        console.log(`Result type: ${result}`);

        if (result === 'ERROR_DISPLAYED') {
            // Take screenshot of error message
            await page.screenshot({
                path: '.playwright-mcp/timeout-test-03-error-message.png',
                fullPage: true
            });
            console.log('📸 Screenshot saved: timeout-test-03-error-message.png');

            // Get the error message text
            const errorText = await page.textContent('.alert-danger');
            console.log(`Error message: "${errorText}"`);

            // Verify timing expectations
            if (responseTime >= 14000 && responseTime <= 20000) {
                console.log('✅ SUCCESS: Response time within expected range (14-20 seconds)');
                console.log('✅ SUCCESS: HttpClient timeout fix is working correctly');

                if (errorText && errorText.includes('HttpClient.Timeout') && errorText.includes('15 seconds')) {
                    console.log('✅ SUCCESS: Error message confirms 15-second HttpClient timeout');
                } else {
                    console.log('⚠️  WARNING: Error message may not mention HttpClient timeout');
                }
            } else if (responseTime > 120000) {
                console.log('❌ FAILURE: Response took too long - HttpClient timeout not working');
            } else if (responseTime < 14000) {
                console.log('⚠️  WARNING: Response faster than expected - may indicate different error');
            }

        } else if (result === 'REDIRECT_SUCCESS') {
            console.log('⚠️  UNEXPECTED: Login succeeded - API might be working now');
            await page.screenshot({
                path: '.playwright-mcp/timeout-test-03-success-redirect.png',
                fullPage: true
            });

        } else if (result === 'TIMEOUT_REACHED') {
            console.log('❌ FAILURE: Browser test timeout reached - response took longer than 25 seconds');
            await page.screenshot({
                path: '.playwright-mcp/timeout-test-03-browser-timeout.png',
                fullPage: true
            });
        }

        // Step 5: Summary
        console.log('');
        console.log('='.repeat(60));
        console.log('📋 TEST SUMMARY');
        console.log('='.repeat(60));
        console.log(`Navigation time: ${navEndTime - navStartTime}ms`);
        console.log(`Login response time: ${responseTime}ms (${(responseTime / 1000).toFixed(2)} seconds)`);
        console.log(`Result: ${result}`);

        if (responseTime >= 14000 && responseTime <= 20000 && result === 'ERROR_DISPLAYED') {
            console.log('');
            console.log('🎉 OVERALL RESULT: HttpClient timeout fix is WORKING CORRECTLY');
            console.log('   - Response completed within expected timeframe');
            console.log('   - No browser hanging or indefinite waiting');
            console.log('   - Error message displayed as expected');
        } else {
            console.log('');
            console.log('⚠️  OVERALL RESULT: Unexpected behavior - review needed');
        }

        console.log('='.repeat(60));

    } catch (error) {
        console.error('❌ Test failed with error:', error.message);

        // Take error screenshot
        try {
            await page.screenshot({
                path: '.playwright-mcp/timeout-test-error.png',
                fullPage: true
            });
            console.log('📸 Error screenshot saved: timeout-test-error.png');
        } catch (screenshotError) {
            console.error('Failed to take error screenshot:', screenshotError.message);
        }

    } finally {
        await browser.close();
        console.log('');
        console.log('🏁 Test completed - browser closed');
    }
}

// Run the test
testAdminLoginTimeout().catch(console.error);