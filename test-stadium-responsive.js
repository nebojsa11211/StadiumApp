const { chromium } = require('@playwright/test');

(async () => {
    const browser = await chromium.launch({ headless: false });

    try {
        // Test different screen sizes
        const screenSizes = [
            { name: 'Desktop Large', width: 1920, height: 1080 },
            { name: 'Desktop Standard', width: 1440, height: 900 },
            { name: 'Laptop', width: 1366, height: 768 },
            { name: 'Tablet Landscape', width: 1024, height: 768 },
            { name: 'Tablet Portrait', width: 768, height: 1024 },
            { name: 'Mobile Large', width: 414, height: 896 },
            { name: 'Mobile Small', width: 375, height: 667 }
        ];

        for (const size of screenSizes) {
            console.log(`\n📱 Testing ${size.name} (${size.width}x${size.height})`);

            const context = await browser.newContext({
                viewport: { width: size.width, height: size.height },
                ignoreHTTPSErrors: true
            });

            const page = await context.newPage();

            // Navigate to admin login
            await page.goto('https://localhost:9030/login');
            await page.waitForLoadState('networkidle');

            // Check if we're already on login page
            const loginInput = await page.locator('input[type="email"]');
            if (await loginInput.count() > 0) {
                // Login
                await page.fill('input[type="email"]', 'admin@stadium.com');
                await page.fill('input[type="password"]', 'admin123');
                await page.click('button[type="submit"]');

                // Wait for navigation
                await page.waitForLoadState('networkidle');
                await page.waitForTimeout(2000);
            }

            // Navigate to stadium overview
            await page.goto('https://localhost:9030/admin/stadium-overview');
            await page.waitForLoadState('networkidle');
            await page.waitForTimeout(1000);

            // Check stadium container visibility
            const container = await page.locator('#admin-stadium-container');
            const isVisible = await container.isVisible();
            const boundingBox = await container.boundingBox();

            if (isVisible && boundingBox) {
                console.log(`  ✅ Stadium container is visible`);
                console.log(`     Width: ${boundingBox.width}px, Height: ${boundingBox.height}px`);

                // Check if container has reasonable dimensions
                if (boundingBox.width < 100 || boundingBox.height < 100) {
                    console.log(`  ⚠️ WARNING: Container dimensions seem too small!`);
                }

                // Check stadium grid layout
                const gridLayout = await page.locator('.stadium-grid-layout');
                if (await gridLayout.count() > 0) {
                    const gridVisible = await gridLayout.isVisible();
                    console.log(`     Stadium grid: ${gridVisible ? '✅ Visible' : '❌ Not visible'}`);
                }

                // Check for any error states
                const errorState = await page.locator('.error-state');
                if (await errorState.count() > 0 && await errorState.isVisible()) {
                    console.log(`  ⚠️ Error state displayed`);
                }

                // Check for loading state
                const loadingState = await page.locator('.loading-state');
                if (await loadingState.count() > 0 && await loadingState.isVisible()) {
                    console.log(`  ⚠️ Still loading...`);
                }

            } else {
                console.log(`  ❌ Stadium container is NOT visible!`);

                // Try to diagnose the issue
                const computedStyle = await container.evaluate(el => {
                    const style = window.getComputedStyle(el);
                    return {
                        display: style.display,
                        visibility: style.visibility,
                        opacity: style.opacity,
                        width: style.width,
                        height: style.height,
                        overflow: style.overflow
                    };
                });
                console.log(`     Computed styles:`, computedStyle);
            }

            // Take screenshot for visual verification
            await page.screenshot({
                path: `.playwright-mcp/stadium-${size.name.toLowerCase().replace(' ', '-')}-${size.width}x${size.height}.png`,
                fullPage: true
            });

            await context.close();
        }

        console.log('\n✨ Test completed! Screenshots saved in .playwright-mcp/');

    } catch (error) {
        console.error('❌ Error:', error.message);
    } finally {
        await browser.close();
    }
})();