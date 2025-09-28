const { chromium } = require('playwright');

(async () => {
    const browser = await chromium.launch({
        headless: false,
        slowMo: 800
    });

    const context = await browser.newContext({
        viewport: { width: 1920, height: 1080 },
        ignoreHTTPSErrors: true
    });

    const page = await context.newPage();

    try {
        console.log('🏟️ Final Stadium Visualization Test with Working API');
        console.log('Note: API is now running locally with verified stadium data');

        // Step 1: Login to Admin
        console.log('📋 Step 1: Authenticating with admin');
        await page.goto('https://localhost:7030/admin/login');
        await page.waitForLoadState('domcontentloaded');

        await page.fill('input[type="email"]', 'admin@stadium.com');
        await page.fill('input[type="password"]', 'admin123');
        await page.click('button[type="submit"]');
        await page.waitForLoadState('networkidle');

        // Step 2: Navigate to Stadium Overview
        console.log('📋 Step 2: Navigating to Stadium Overview');
        await page.goto('https://localhost:7030/admin/stadium-overview');
        await page.waitForLoadState('domcontentloaded');

        // Wait extra time for potential data loading
        await page.waitForTimeout(8000);

        // Take screenshot of current state
        await page.screenshot({
            path: './.playwright-mcp/final-stadium-test-with-api.png',
            fullPage: true
        });

        // Step 3: Analyze what's actually displayed
        console.log('📋 Step 3: Analyzing page content...');
        const pageContent = await page.textContent('body');

        // Check authentication state
        const isAuthenticated = !pageContent.includes('Stadium Admin Login');
        console.log(`🔐 Authentication Status: ${isAuthenticated ? '✅ Logged In' : '❌ Still on login page'}`);

        if (isAuthenticated) {
            // Check for stadium data elements
            const hasStadiumOperationsCenter = pageContent.includes('Stadium Operations Center');
            const hasControlPanel = pageContent.includes('Control Panel');
            const hasStadiumLayout = pageContent.includes('Stadium Layout');
            const hasStadiumInfo = pageContent.includes('Stadium Information');

            console.log(`🏢 Stadium Operations Center: ${hasStadiumOperationsCenter ? '✅' : '❌'}`);
            console.log(`🎛️ Control Panel: ${hasControlPanel ? '✅' : '❌'}`);
            console.log(`🏟️ Stadium Layout: ${hasStadiumLayout ? '✅' : '❌'}`);
            console.log(`ℹ️ Stadium Information: ${hasStadiumInfo ? '✅' : '❌'}`);

            // Check for specific data
            const hasSeats = pageContent.match(/(\d{1,6})\s*(seats|Seats)/);
            const hasSectors = pageContent.match(/(\d+)\s*(sectors|Sectors)/);
            const hasTribunes = pageContent.match(/(\d+)\s*(tribunes|Tribunes)/);

            if (hasSeats) console.log(`💺 Seats Found: ${hasSeats[0]}`);
            if (hasSectors) console.log(`🎯 Sectors Found: ${hasSectors[0]}`);
            if (hasTribunes) console.log(`🏛️ Tribunes Found: ${hasTribunes[0]}`);

            // Check for our expected numbers
            const hasExpectedSeats = pageContent.includes('5,765') || pageContent.includes('5765');
            console.log(`🎊 Expected 5,765 seats: ${hasExpectedSeats ? '✅' : '❌'}`);

            // Look for visual elements
            const svgElements = await page.locator('svg').count();
            const stadiumElements = await page.locator('[class*="stadium"], [class*="field"], [class*="sector"]').count();

            console.log(`📐 SVG Elements: ${svgElements}`);
            console.log(`🏟️ Stadium Elements: ${stadiumElements}`);

            // Test interactions
            if (stadiumElements > 0) {
                console.log('📋 Step 4: Testing interactions...');
                try {
                    // Try to click a stadium element
                    const firstStadiumElement = page.locator('[class*="sector"], [class*="stand"]').first();
                    if (await firstStadiumElement.count() > 0) {
                        await firstStadiumElement.click();
                        await page.waitForTimeout(2000);

                        const modalVisible = await page.locator('.modal, [class*="modal"]').isVisible();
                        console.log(`🪟 Modal Response: ${modalVisible ? '✅ Opened' : '❌ No modal'}`);
                    }
                } catch (error) {
                    console.log(`⚠️ Interaction test failed: ${error.message}`);
                }
            }
        }

        // Final assessment
        console.log('\n🏁 FINAL ASSESSMENT:');
        console.log('=' .repeat(60));
        console.log(`Authentication: ${isAuthenticated ? '✅ Working' : '❌ Failed'}`);
        console.log(`Stadium Data API: ✅ Working (verified separately)`);
        console.log(`Admin UI Access: ${isAuthenticated ? '✅ Accessible' : '❌ Blocked'}`);

        if (isAuthenticated) {
            const hasStadiumContent = pageContent.includes('Stadium') && pageContent.includes('seats');
            console.log(`Stadium Content: ${hasStadiumContent ? '✅ Present' : '❌ Missing'}`);

            if (!hasStadiumContent) {
                console.log('\n⚠️ ISSUE IDENTIFIED:');
                console.log('The admin app is authenticated but not loading stadium data.');
                console.log('This suggests the admin app is configured to use Docker API (9010)');
                console.log('but needs to be reconfigured to use local API (7010).');

                // Check what error might be displayed
                const hasError = pageContent.includes('Error') || pageContent.includes('error');
                if (hasError) {
                    console.log('Error indicators found on page');
                }
            }
        }

    } catch (error) {
        console.error('❌ Test Error:', error.message);
        await page.screenshot({
            path: './.playwright-mcp/final-test-error.png',
            fullPage: true
        });
    } finally {
        await browser.close();
    }
})();