const { chromium } = require('playwright');

async function testStadiumOverviewFix() {
    console.log('🔧 Testing Stadium Overview page after API timeout bypass...');

    const browser = await chromium.launch({
        headless: false,
        args: ['--ignore-certificate-errors', '--ignore-ssl-errors']
    });

    try {
        const context = await browser.newContext({
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Step 1: Login to admin
        console.log('🔐 Logging into admin interface...');
        await page.goto('https://localhost:9030/login');
        await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });

        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login redirect
        await page.waitForURL('**/dashboard', { timeout: 15000 });
        console.log('✅ Logged in successfully');

        // Step 2: Navigate to Stadium Overview page
        console.log('🏟️ Navigating to Stadium Overview page...');
        await page.goto('https://localhost:9030/stadium-overview');

        // Wait for page to load completely
        await page.waitForLoadState('domcontentloaded');

        // Check if the timeout error is gone
        try {
            const timeoutError = await page.locator('text=Stadium Layout Error').first();
            const hasTimeoutError = await timeoutError.isVisible({ timeout: 3000 });

            if (hasTimeoutError) {
                const errorText = await timeoutError.textContent();
                console.log('❌ Timeout error still present:', errorText);
                return { success: false, error: errorText };
            }
        } catch (e) {
            // Error element not found, which is good
        }

        // Look for the stadium container
        try {
            await page.waitForSelector('#admin-stadium-container', { timeout: 10000 });
            console.log('✅ Stadium container found!');

            // Check if the container has content
            const containerContent = await page.locator('#admin-stadium-container').innerHTML();

            if (containerContent.trim().length > 50) {
                console.log('✅ Stadium container has content');

                // Take screenshot to verify
                await page.screenshot({
                    path: 'stadium-overview-timeout-fixed.png',
                    fullPage: true
                });
                console.log('📸 Screenshot saved: stadium-overview-timeout-fixed.png');

                // Check for SVG element specifically
                const hasSvg = await page.locator('#admin-stadium-container svg').first().isVisible().catch(() => false);

                return {
                    success: true,
                    message: 'Stadium Overview page loaded successfully - timeout issue resolved',
                    hasContainer: true,
                    hasSvg: hasSvg,
                    screenshotPath: 'stadium-overview-timeout-fixed.png'
                };

            } else {
                console.log('⚠️ Stadium container found but appears empty');
                return {
                    success: false,
                    error: 'Stadium container is empty'
                };
            }

        } catch (error) {
            console.log('❌ Stadium container not found within timeout');

            // Take error screenshot
            await page.screenshot({
                path: 'stadium-overview-still-broken.png',
                fullPage: true
            });

            return {
                success: false,
                error: 'Stadium container not found',
                screenshotPath: 'stadium-overview-still-broken.png'
            };
        }

    } catch (error) {
        console.error('❌ Error during test:', error.message);

        // Take error screenshot
        await page.screenshot({
            path: 'stadium-test-error.png',
            fullPage: true
        }).catch(() => {});

        return {
            success: false,
            error: error.message,
            screenshotPath: 'stadium-test-error.png'
        };

    } finally {
        await browser.close();
    }
}

// Execute the test
if (require.main === module) {
    testStadiumOverviewFix()
        .then(result => {
            console.log('🎉 Test completed:', result);
            if (result.success) {
                console.log('✅ Stadium Overview timeout issue has been resolved!');
                console.log('✅ Stadium layout generation can now proceed');
            } else {
                console.log('❌ Stadium Overview still has issues:', result.error);
            }
        })
        .catch(error => {
            console.error('💥 Test failed:', error.message);
            process.exit(1);
        });
}

module.exports = { testStadiumOverviewFix };