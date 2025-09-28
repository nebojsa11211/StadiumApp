const { chromium } = require('@playwright/test');

(async () => {
    const browser = await chromium.launch({ headless: false });

    try {
        // Wait for container to fully start
        console.log('⏱️ Waiting for container to start...');
        await new Promise(resolve => setTimeout(resolve, 10000));

        console.log('📊 Testing Proper Sector Layout at Multiple Resolutions');

        const resolutions = [
            { name: '1920x1080', width: 1920, height: 1080 },
            { name: '1440x900', width: 1440, height: 900 },
            { name: '1366x768', width: 1366, height: 768 }
        ];

        for (const res of resolutions) {
            console.log(`\n🖥️ Testing at ${res.name}:`);

            const context = await browser.newContext({
                viewport: { width: res.width, height: res.height },
                ignoreHTTPSErrors: true
            });

            const page = await context.newPage();

            // Navigate and login
            await page.goto('https://localhost:9030/login');
            await page.waitForLoadState('networkidle');

            const loginInput = await page.locator('input[type="email"]');
            if (await loginInput.count() > 0) {
                await page.fill('input[type="email"]', 'admin@stadium.com');
                await page.fill('input[type="password"]', 'admin123');
                await page.click('button[type="submit"]');
                await page.waitForLoadState('networkidle');
                await page.waitForTimeout(2000);
            }

            // Navigate to stadium overview
            await page.goto('https://localhost:9030/admin/stadium-overview');
            await page.waitForLoadState('networkidle');
            await page.waitForTimeout(2000);

            // Get stadium grid dimensions
            const gridLayout = await page.locator('.stadium-grid-layout').first();
            if (await gridLayout.count() > 0) {
                const gridBounds = await gridLayout.boundingBox();
                console.log(`  Stadium grid: ${gridBounds.width}x${gridBounds.height}px`);
            }

            // Check North stand
            console.log('  📍 North Stand:');
            const northStand = await page.locator('#admin-stadium-stand-n, .stand-n, .stand-position-north').first();
            if (await northStand.count() > 0) {
                const bounds = await northStand.boundingBox();
                console.log(`     Container: ${bounds.width}x${bounds.height}px`);

                const northGrid = await northStand.locator('.sectors-grid').first();
                if (await northGrid.count() > 0) {
                    const gridBounds = await northGrid.boundingBox();
                    console.log(`     Sectors grid: ${gridBounds.width}x${gridBounds.height}px`);
                }

                const sectors = await northStand.locator('.sector');
                const count = await sectors.count();
                if (count > 0) {
                    const first = await sectors.first().boundingBox();
                    console.log(`     First sector: ${first.width}x${first.height}px`);

                    // Check if sector height exceeds parent
                    if (first.height > bounds.height) {
                        console.log(`     ❌ PROBLEM: Sector height (${first.height}px) exceeds parent (${bounds.height}px)`);
                    } else {
                        console.log(`     ✅ OK: Sector fits within parent`);
                    }
                }
            }

            // Check West stand
            console.log('  📍 West Stand:');
            const westStand = await page.locator('#admin-stadium-stand-w, .stand-w, .stand-position-west').first();
            if (await westStand.count() > 0) {
                const bounds = await westStand.boundingBox();
                console.log(`     Container: ${bounds.width}x${bounds.height}px`);

                const westGrid = await westStand.locator('.sectors-grid').first();
                if (await westGrid.count() > 0) {
                    const gridBounds = await westGrid.boundingBox();
                    console.log(`     Sectors grid: ${gridBounds.width}x${gridBounds.height}px`);

                    // Check if grid fills parent height
                    const fillPercent = (gridBounds.height / bounds.height) * 100;
                    console.log(`     Grid fills ${fillPercent.toFixed(1)}% of parent height`);

                    if (fillPercent < 80) {
                        console.log(`     ⚠️ Grid should fill more of parent height`);
                    } else {
                        console.log(`     ✅ Grid properly fills parent height`);
                    }
                }
            }

            // Take screenshot
            await page.screenshot({
                path: `.playwright-mcp/stadium-proper-layout-${res.name}.png`,
                fullPage: false
            });

            await context.close();
        }

        console.log('\n✅ Testing completed! Screenshots saved.');

    } catch (error) {
        console.error('❌ Error:', error.message);
    } finally {
        await browser.close();
    }
})();