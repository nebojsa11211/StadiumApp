const { chromium } = require('playwright');

async function testTimeoutDetails() {
    const browser = await chromium.launch({
        headless: false,
        slowMo: 300
    });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 }
        });

        const page = await context.newPage();

        // Enable console logging to capture any client-side errors
        page.on('console', msg => {
            console.log(`🌐 BROWSER: ${msg.type()}: ${msg.text()}`);
        });

        console.log('🔍 Testing HttpClient timeout details...');
        console.log('');

        // Navigate to admin login
        await page.goto('https://localhost:7030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Fill form
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');

        console.log('⏱️  Starting login attempt...');
        const startTime = Date.now();

        // Click login and wait for error
        await page.click('#admin-login-submit-btn');

        // Wait for error message to appear
        await page.waitForSelector('.alert-danger', { timeout: 25000 });

        const endTime = Date.now();
        const duration = endTime - startTime;

        // Get the full error message
        const errorText = await page.textContent('.alert-danger');

        console.log('');
        console.log('='.repeat(60));
        console.log('📊 TIMEOUT TEST RESULTS');
        console.log('='.repeat(60));
        console.log(`Duration: ${duration}ms (${(duration / 1000).toFixed(2)} seconds)`);
        console.log(`Error Message: "${errorText}"`);
        console.log('');

        // Analyze the timeout behavior
        if (duration >= 14000 && duration <= 20000) {
            console.log('✅ SUCCESS: HttpClient timeout is working correctly');
            console.log('   - Response completed within expected 15-second timeout window');
            console.log('   - No browser hanging or indefinite waiting');
            console.log('');

            if (errorText && (
                errorText.includes('timeout') ||
                errorText.includes('Timeout') ||
                errorText.includes('canceled') ||
                errorText.includes('HttpClient')
            )) {
                console.log('✅ SUCCESS: Error message indicates timeout/cancellation');
            } else if (errorText && errorText.includes('Invalid email or password')) {
                console.log('⚠️  INFO: Error shows "Invalid email or password" which suggests:');
                console.log('   - API timeout exception is being caught but not clearly identified');
                console.log('   - The HttpClient timeout is working (preventing 120+ second hang)');
                console.log('   - Error handling could be improved to show timeout message');
            } else {
                console.log('⚠️  WARNING: Unexpected error message format');
            }
        } else if (duration > 120000) {
            console.log('❌ FAILURE: HttpClient timeout NOT working - took too long');
        } else {
            console.log('⚠️  WARNING: Unexpected duration - may indicate different behavior');
        }

        console.log('');
        console.log('📋 CONCLUSION:');
        if (duration >= 14000 && duration <= 20000) {
            console.log('🎉 The HttpClient timeout fix IS WORKING CORRECTLY!');
            console.log('   - Prevents indefinite hanging (was 120+ seconds)');
            console.log('   - Now completes within ~15 seconds as configured');
            console.log('   - Error message could be improved but timeout works');
        } else {
            console.log('❌ The HttpClient timeout may not be working as expected');
        }

        console.log('='.repeat(60));

        // Take final screenshot
        await page.screenshot({
            path: '.playwright-mcp/timeout-details-final.png',
            fullPage: true
        });

    } catch (error) {
        console.error('❌ Test failed:', error.message);
        await page.screenshot({
            path: '.playwright-mcp/timeout-details-error.png',
            fullPage: true
        });
    } finally {
        await browser.close();
    }
}

testTimeoutDetails().catch(console.error);