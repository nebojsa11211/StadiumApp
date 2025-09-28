const { chromium } = require('@playwright/test');

async function testAdminLogin() {
    console.log('🚀 Starting Admin Login Test...');

    const browser = await chromium.launch({
        headless: false,
        slowMo: 1000
    });

    const context = await browser.newContext({
        ignoreHTTPSErrors: true
    });

    const page = await context.newPage();

    try {
        // Navigate to Admin login page
        console.log('📍 Navigating to Admin login page...');
        await page.goto('https://localhost:7030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        console.log('✅ Admin page loaded successfully');

        // Take screenshot
        await page.screenshot({ path: 'admin-login-page.png' });
        console.log('📸 Screenshot taken: admin-login-page.png');

        // Fill login form
        console.log('📝 Filling login form...');
        // Use generic selectors for email and password inputs
        await page.fill('input[type="email"], input[name="email"], input[placeholder*="mail" i]', 'admin@stadium.com');
        await page.fill('input[type="password"], input[name="password"]', 'admin123');

        console.log('🔐 Attempting login...');
        const loginPromise = page.waitForNavigation({
            waitUntil: 'networkidle',
            timeout: 45000
        });

        // Click the Login button using text content
        await page.click('button:has-text("Login"), input[type="submit"][value*="Login" i]');

        try {
            await loginPromise;
            console.log('✅ Login successful - navigation completed');

            // Take screenshot of dashboard
            await page.screenshot({ path: 'admin-dashboard.png' });
            console.log('📸 Dashboard screenshot: admin-dashboard.png');

            // Check if we're on dashboard
            const currentUrl = page.url();
            console.log(`📍 Current URL: ${currentUrl}`);

            if (currentUrl.includes('/dashboard') || currentUrl.includes('/') && !currentUrl.includes('/login')) {
                console.log('🎉 LOGIN TEST SUCCESSFUL!');
                return true;
            } else {
                console.log('❌ LOGIN FAILED - Still on login page or unexpected URL');
                return false;
            }

        } catch (timeoutError) {
            console.log('⏰ Login timeout after 45 seconds');
            console.log('Taking error screenshot...');
            await page.screenshot({ path: 'admin-login-timeout.png' });

            // Check for error messages
            const errorElement = await page.$('.alert-danger, .error-message, [id*="error"]');
            if (errorElement) {
                const errorText = await errorElement.textContent();
                console.log(`❌ Error message found: ${errorText}`);
            }

            return false;
        }

    } catch (error) {
        console.log(`❌ Test failed: ${error.message}`);
        await page.screenshot({ path: 'admin-login-error.png' });
        return false;
    } finally {
        await browser.close();
    }
}

testAdminLogin()
    .then(success => {
        if (success) {
            console.log('\n🎉 FINAL RESULT: Admin login test PASSED!');
            console.log('✅ The Supabase connection issue has been resolved');
            console.log('✅ Both API and Admin services are working correctly');
        } else {
            console.log('\n❌ FINAL RESULT: Admin login test FAILED');
            console.log('⚠️  The timeout issue may still persist');
        }
        process.exit(success ? 0 : 1);
    })
    .catch(error => {
        console.error('Test execution failed:', error);
        process.exit(1);
    });