const { chromium } = require('playwright');

(async () => {
    console.log('🚀 Starting Stadium Overview capture...');

    const browser = await chromium.launch({
        headless: false,
        timeout: 60000
    });

    const context = await browser.newContext({
        ignoreHTTPSErrors: true,
        viewport: { width: 1920, height: 1080 }
    });

    const page = await context.newPage();

    try {
        console.log('📍 Step 1: Navigating to Admin app...');
        await page.goto('https://localhost:7030', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        console.log('🔑 Step 2: Logging in...');
        // Check if already logged in
        const isLoggedIn = await page.locator('text=Stadium Operations Center').first().isVisible({ timeout: 5000 }).catch(() => false);

        if (!isLoggedIn) {
            console.log('Not logged in, proceeding with login...');

            // Fill login form
            await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });
            await page.fill('#admin-login-email-input', 'admin@stadium.com');
            await page.fill('#admin-login-password-input', 'admin123');
            await page.click('#admin-login-submit-btn');

            // Wait for redirect to dashboard or any successful navigation
            await page.waitForTimeout(5000); // Wait for form submission
            console.log('✅ Login submitted, checking navigation...');
        } else {
            console.log('✅ Already logged in');
        }

        console.log('🏟️ Step 3: Navigating to Stadium Overview...');
        await page.goto('https://localhost:7030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        console.log('⏳ Step 4: Waiting for stadium to load...');
        // Wait for either the stadium container or loading spinner
        try {
            await page.waitForSelector('#admin-stadium-overview-container', { timeout: 15000 });
            console.log('✅ Stadium overview container found');
        } catch (e) {
            console.log('⚠️  Stadium container not found, checking for loading states...');
        }

        // Wait a bit more for any dynamic content
        await page.waitForTimeout(3000);

        console.log('📸 Step 5: Taking comprehensive screenshots...');

        // Wait longer for stadium to fully load if it's working
        await page.waitForTimeout(10000);

        // Take full page screenshot
        await page.screenshot({
            path: '.playwright-mcp/stadium-overview-working.png',
            fullPage: true
        });
        console.log('✅ Full page screenshot saved');

        // Take viewport screenshot
        await page.screenshot({
            path: '.playwright-mcp/stadium-overview-current-state.png',
            fullPage: false
        });
        console.log('✅ Viewport screenshot saved');

        // Try to capture the stadium visualization area specifically
        const stadiumContainer = await page.locator('#admin-stadium-container').first();
        if (await stadiumContainer.isVisible().catch(() => false)) {
            await stadiumContainer.screenshot({
                path: '.playwright-mcp/stadium-container-final.png'
            });
            console.log('✅ Stadium container screenshot saved');
        }

        // Try to capture stadium visualization content
        const stadiumVisualization = await page.locator('#admin-stadium-visualization').first();
        if (await stadiumVisualization.isVisible().catch(() => false)) {
            await stadiumVisualization.screenshot({
                path: '.playwright-mcp/stadium-visualization-section.png'
            });
            console.log('✅ Stadium visualization section screenshot saved');
        }

        console.log('📝 Step 6: Gathering information about the page...');

        // Get page title and URL
        const title = await page.title();
        const url = page.url();
        console.log(`Page Title: ${title}`);
        console.log(`Current URL: ${url}`);

        // Check for error messages
        const errorMessages = await page.locator('.alert-danger, .error, .text-danger').allTextContents();
        if (errorMessages.length > 0) {
            console.log('⚠️  Error messages found:', errorMessages);
        }

        // Check for loading states
        const loadingElements = await page.locator('.loading, .spinner, [data-loading="true"]').allTextContents();
        if (loadingElements.length > 0) {
            console.log('⏳ Loading elements found:', loadingElements);
        }

        // Get console logs
        page.on('console', msg => console.log('Browser Console:', msg.text()));

        console.log('✅ Stadium Overview capture completed successfully!');

    } catch (error) {
        console.error('❌ Error during stadium capture:', error.message);

        // Take error screenshot
        await page.screenshot({
            path: '.playwright-mcp/stadium-error-state.png',
            fullPage: true
        });
        console.log('📸 Error state screenshot saved');
    }

    // Keep browser open for 10 seconds for manual inspection
    console.log('🔍 Keeping browser open for 10 seconds for manual inspection...');
    await page.waitForTimeout(10000);

    await browser.close();
    console.log('🏁 Browser closed');
})();