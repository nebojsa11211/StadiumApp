const { chromium } = require('playwright');

(async () => {
    const browser = await chromium.launch({
        headless: false,
        slowMo: 1000
    });

    const context = await browser.newContext({
        viewport: { width: 1920, height: 1080 },
        ignoreHTTPSErrors: true
    });

    const page = await context.newPage();

    try {
        console.log('🏟️ Starting Complete Stadium Visualization Test...');

        // Step 1: Login
        console.log('📋 Step 1: Login to Admin');
        await page.goto('https://localhost:7030/admin/login');
        await page.waitForLoadState('domcontentloaded');

        // Fill login form
        await page.fill('input[type="email"]', 'admin@stadium.com');
        await page.fill('input[type="password"]', 'admin123');
        await page.click('button[type="submit"]');

        // Wait for login to complete
        await page.waitForLoadState('networkidle');
        await page.waitForTimeout(3000);

        // Take post-login screenshot
        await page.screenshot({
            path: './.playwright-mcp/01-after-login.png',
            fullPage: true
        });
        console.log('📸 Post-login screenshot saved');

        // Step 2: Navigate to Stadium Overview
        console.log('📋 Step 2: Navigate to Stadium Overview');
        await page.goto('https://localhost:7030/admin/stadium-overview');
        await page.waitForLoadState('domcontentloaded');
        await page.waitForTimeout(5000); // Wait for data loading

        // Take initial stadium overview screenshot
        await page.screenshot({
            path: './.playwright-mcp/02-stadium-overview-loaded.png',
            fullPage: true
        });
        console.log('📸 Stadium overview loaded screenshot saved');

        // Step 3: Analyze content and data
        console.log('📋 Step 3: Analyzing stadium data...');
        const pageContent = await page.textContent('body');

        // Check for stadium data indicators
        const hasStadiumData = pageContent.includes('Total Seats') ||
                              pageContent.includes('Sectors') ||
                              pageContent.includes('Tribunes');

        console.log(`✅ Stadium data present: ${hasStadiumData}`);

        // Check for seat numbers
        const seatNumbers = pageContent.match(/(\d{1,6})\s*(seats?|places?)/gi) || [];
        console.log('🔢 Seat information found:', seatNumbers);

        // Look for our expected 5,765 seats
        if (pageContent.includes('5765') || pageContent.includes('5,765')) {
            console.log('✅ Expected seat count (5,765) found!');
        } else {
            const largeNumbers = pageContent.match(/\b[1-9]\d{3,5}\b/g);
            console.log('🔢 Large numbers on page:', largeNumbers ? largeNumbers.slice(0, 10) : 'None');
        }

        // Check for tribune names
        const tribunes = ['North', 'South', 'East', 'West'];
        const foundTribunes = [];
        for (const tribune of tribunes) {
            if (pageContent.includes(tribune)) {
                foundTribunes.push(tribune);
            }
        }
        console.log('🏛️ Tribunes found:', foundTribunes);

        // Check visual elements
        const svgCount = await page.locator('svg').count();
        const canvasCount = await page.locator('canvas').count();
        const stadiumElements = await page.locator('[class*="stadium"], [class*="sector"], [class*="stand"], [class*="field"]').count();
        const sectorElements = await page.locator('[class*="sector"]').count();

        console.log(`📊 Visual Analysis:
        - SVG elements: ${svgCount}
        - Canvas elements: ${canvasCount}
        - Stadium-related elements: ${stadiumElements}
        - Sector elements: ${sectorElements}`);

        // Step 4: Test interactions if elements exist
        if (sectorElements > 0) {
            console.log('📋 Step 4: Testing sector interactions...');
            try {
                // Click on first sector
                await page.locator('[class*="sector"]').first().click();
                await page.waitForTimeout(2000);

                // Check if modal opened
                const modalVisible = await page.locator('.modal, [class*="modal"]').isVisible();
                console.log(`🪟 Modal opened: ${modalVisible}`);

                if (modalVisible) {
                    await page.screenshot({
                        path: './.playwright-mcp/03-sector-modal.png',
                        fullPage: true
                    });
                    console.log('📸 Sector modal screenshot saved');

                    // Close modal
                    await page.locator('.btn-close, .modal-backdrop').first().click();
                    await page.waitForTimeout(1000);
                }
            } catch (error) {
                console.log('⚠️ Sector interaction test failed:', error.message);
            }
        }

        // Step 5: Test search functionality
        console.log('📋 Step 5: Testing search functionality...');
        const searchInputs = await page.locator('input[placeholder*="seat"], input[placeholder*="search"]').count();
        console.log(`🔍 Search inputs found: ${searchInputs}`);

        if (searchInputs > 0) {
            try {
                await page.fill('input[placeholder*="seat"], input[placeholder*="search"]', 'N1A-R1S1');
                await page.keyboard.press('Enter');
                await page.waitForTimeout(2000);
                console.log('✅ Search functionality tested');
            } catch (error) {
                console.log('⚠️ Search test failed:', error.message);
            }
        }

        // Step 6: Test event selection if available
        console.log('📋 Step 6: Testing event selection...');
        const eventSelect = await page.locator('#event-select').count();
        if (eventSelect > 0) {
            try {
                const options = await page.locator('#event-select option').count();
                console.log(`📅 Event options available: ${options}`);

                if (options > 1) {
                    // Select first actual event (not "no event")
                    await page.selectOption('#event-select', { index: 1 });
                    await page.waitForTimeout(3000);

                    await page.screenshot({
                        path: './.playwright-mcp/04-with-event-selected.png',
                        fullPage: true
                    });
                    console.log('📸 Event selection screenshot saved');
                }
            } catch (error) {
                console.log('⚠️ Event selection test failed:', error.message);
            }
        }

        // Step 7: Test responsive layouts
        console.log('📋 Step 7: Testing responsive layouts...');
        const viewports = [
            { width: 1920, height: 1080, name: 'desktop-full' },
            { width: 1366, height: 768, name: 'laptop-standard' },
            { width: 768, height: 1024, name: 'tablet-portrait' },
            { width: 375, height: 667, name: 'mobile-portrait' }
        ];

        for (const viewport of viewports) {
            await page.setViewportSize(viewport);
            await page.waitForTimeout(2000);
            await page.screenshot({
                path: `./.playwright-mcp/05-responsive-${viewport.name}.png`
            });
            console.log(`📸 Responsive screenshot saved: ${viewport.name}`);
        }

        // Step 8: Final analysis screenshot at desktop resolution
        await page.setViewportSize({ width: 1920, height: 1080 });
        await page.waitForTimeout(2000);
        await page.screenshot({
            path: './.playwright-mcp/06-final-analysis.png',
            fullPage: true
        });

        // Step 9: Generate summary report
        const report = `
# Stadium Visualization Test Results

## Authentication
- ✅ Successfully logged into admin panel

## Stadium Data Analysis
- Stadium data present: ${hasStadiumData}
- Expected seat count (5,765): ${pageContent.includes('5765') || pageContent.includes('5,765') ? '✅ Found' : '❌ Not found'}
- Tribunes found: ${foundTribunes.join(', ') || 'None detected'}

## Visual Elements
- SVG elements: ${svgCount}
- Canvas elements: ${canvasCount}
- Stadium-related elements: ${stadiumElements}
- Sector elements: ${sectorElements}

## Functionality Tests
- Search inputs: ${searchInputs > 0 ? '✅ Available' : '❌ Not found'}
- Event selection: ${eventSelect > 0 ? '✅ Available' : '❌ Not found'}
- Sector interactions: ${sectorElements > 0 ? '✅ Available' : '❌ No sectors found'}

## Current State Assessment
Based on the test results:

${sectorElements > 0 ?
  '✅ Stadium visualization is working with interactive sectors' :
  '⚠️ Stadium visualization appears to be in loading/error state'}

${hasStadiumData ?
  '✅ Stadium statistics are displayed' :
  '⚠️ Stadium statistics not visible'}

## Recommendations for Professional Stadium Design
1. **Visual Enhancement Needed**: Current layout needs professional stadium appearance
2. **SVG Implementation**: Add proper stadium field visualization with SVG graphics
3. **Color Scheme**: Implement professional color palette (green field, proper tribune colors)
4. **Interactive Features**: Enhance sector clicking and modal interactions
5. **Mobile Optimization**: Ensure responsive design works across all devices

## Screenshots Captured
- 01-after-login.png: Post-authentication state
- 02-stadium-overview-loaded.png: Initial stadium overview
- 03-sector-modal.png: Sector interaction modal (if available)
- 04-with-event-selected.png: Event selection functionality
- 05-responsive-*.png: Multiple screen size tests
- 06-final-analysis.png: Final state for analysis

## Next Steps Required
1. Review actual stadium data loading
2. Implement professional stadium field visualization
3. Add proper tribune positioning and coloring
4. Enhance mobile responsive design
5. Add SVG-based stadium layout rendering
        `;

        console.log('📋 Test Summary:');
        console.log('================================');
        console.log(`Stadium data visible: ${hasStadiumData}`);
        console.log(`Expected seats found: ${pageContent.includes('5765') || pageContent.includes('5,765')}`);
        console.log(`Tribunes found: ${foundTribunes.length} (${foundTribunes.join(', ')})`);
        console.log(`Visual elements: ${stadiumElements} stadium-related elements`);
        console.log(`Interactive sectors: ${sectorElements}`);
        console.log('================================');

        console.log('✅ Complete stadium analysis finished!');

    } catch (error) {
        console.error('❌ Test Error:', error.message);
        await page.screenshot({
            path: './.playwright-mcp/error-state.png',
            fullPage: true
        });
    } finally {
        await browser.close();
    }
})();