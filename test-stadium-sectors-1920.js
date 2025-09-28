const { chromium } = require('@playwright/test');

(async () => {
    const browser = await chromium.launch({ headless: false });

    try {
        console.log('📊 Testing Stadium Sectors at 1920x1080 resolution');

        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 },
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Navigate to admin login
        console.log('🔐 Logging in...');
        await page.goto('https://localhost:9030/login');
        await page.waitForLoadState('networkidle');

        // Login if needed
        const loginInput = await page.locator('input[type="email"]');
        if (await loginInput.count() > 0) {
            await page.fill('input[type="email"]', 'admin@stadium.com');
            await page.fill('input[type="password"]', 'admin123');
            await page.click('button[type="submit"]');
            await page.waitForLoadState('networkidle');
            await page.waitForTimeout(2000);
        }

        // Navigate to stadium overview
        console.log('🏟️ Navigating to Stadium Overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview');
        await page.waitForLoadState('networkidle');
        await page.waitForTimeout(2000);

        // Check North stand sectors
        console.log('\n📍 Checking NORTH Stand:');
        const northStand = await page.locator('#admin-stadium-stand-n, .stand-n, .stand-position-north').first();
        if (await northStand.count() > 0) {
            const northBounds = await northStand.boundingBox();
            console.log(`   Stand dimensions: ${northBounds.width}x${northBounds.height}px`);

            const northSectorsGrid = await northStand.locator('.sectors-grid').first();
            if (await northSectorsGrid.count() > 0) {
                const gridBounds = await northSectorsGrid.boundingBox();
                console.log(`   Sectors grid: ${gridBounds.width}x${gridBounds.height}px`);

                const northSectors = await northSectorsGrid.locator('.sector');
                const sectorCount = await northSectors.count();
                console.log(`   Number of sectors: ${sectorCount}`);

                if (sectorCount > 0) {
                    const firstSector = northSectors.first();
                    const firstBounds = await firstSector.boundingBox();
                    console.log(`   First sector size: ${firstBounds.width}x${firstBounds.height}px`);

                    // Check if sectors are properly arranged horizontally
                    if (sectorCount > 1) {
                        const secondSector = northSectors.nth(1);
                        const secondBounds = await secondSector.boundingBox();

                        if (secondBounds.x > firstBounds.x) {
                            console.log(`   ✅ Sectors arranged horizontally`);
                        } else {
                            console.log(`   ⚠️ Sectors may be stacked vertically`);
                        }
                    }
                }
            }
        } else {
            console.log('   ❌ North stand not found');
        }

        // Check South stand sectors
        console.log('\n📍 Checking SOUTH Stand:');
        const southStand = await page.locator('#admin-stadium-stand-s, .stand-s, .stand-position-south').first();
        if (await southStand.count() > 0) {
            const southBounds = await southStand.boundingBox();
            console.log(`   Stand dimensions: ${southBounds.width}x${southBounds.height}px`);

            const southSectorsGrid = await southStand.locator('.sectors-grid').first();
            if (await southSectorsGrid.count() > 0) {
                const gridBounds = await southSectorsGrid.boundingBox();
                console.log(`   Sectors grid: ${gridBounds.width}x${gridBounds.height}px`);

                const southSectors = await southSectorsGrid.locator('.sector');
                const sectorCount = await southSectors.count();
                console.log(`   Number of sectors: ${sectorCount}`);

                if (sectorCount > 0) {
                    const firstSector = southSectors.first();
                    const firstBounds = await firstSector.boundingBox();
                    console.log(`   First sector size: ${firstBounds.width}x${firstBounds.height}px`);

                    // Check if sectors are properly arranged horizontally
                    if (sectorCount > 1) {
                        const secondSector = southSectors.nth(1);
                        const secondBounds = await secondSector.boundingBox();

                        if (secondBounds.x > firstBounds.x) {
                            console.log(`   ✅ Sectors arranged horizontally`);
                        } else {
                            console.log(`   ⚠️ Sectors may be stacked vertically`);
                        }
                    }
                }
            }
        } else {
            console.log('   ❌ South stand not found');
        }

        // Take screenshot for visual verification
        await page.screenshot({
            path: '.playwright-mcp/stadium-sectors-1920x1080-fixed.png',
            fullPage: false
        });

        console.log('\n📸 Screenshot saved to .playwright-mcp/stadium-sectors-1920x1080-fixed.png');
        console.log('✅ Test completed successfully!');

    } catch (error) {
        console.error('❌ Error:', error.message);
    } finally {
        await browser.close();
    }
})();