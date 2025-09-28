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
        console.log('🏟️ Starting Stadium Visualization Analysis...');

        // Navigate to admin app
        console.log('📋 Step 1: Opening Admin App');
        await page.goto('https://localhost:7030/admin/stadium-overview');
        await page.waitForLoadState('domcontentloaded');
        await page.waitForTimeout(5000); // Wait for any loading states

        // Take initial screenshot
        console.log('📸 Taking initial screenshot...');
        await page.screenshot({
            path: './.playwright-mcp/stadium-overview-current.png',
            fullPage: true
        });

        // Analyze page content
        console.log('📋 Step 2: Analyzing page content...');
        const pageContent = await page.textContent('body');

        // Check for seat count
        if (pageContent.includes('5765') || pageContent.includes('5,765')) {
            console.log('✅ Expected seat count (5,765) found!');
        } else {
            const numbers = pageContent.match(/\b\d{3,6}\b/g);
            console.log('🔢 Numbers found on page:', numbers ? numbers.slice(0, 10) : 'None');
        }

        // Check for tribunes
        const tribunes = ['North', 'South', 'East', 'West'];
        for (const tribune of tribunes) {
            if (pageContent.includes(tribune)) {
                console.log(`✅ Found ${tribune} tribune`);
            }
        }

        // Check visual elements
        const svgCount = await page.locator('svg').count();
        const canvasCount = await page.locator('canvas').count();
        const stadiumDivs = await page.locator('[class*="stadium"], [class*="sector"], [class*="stand"]').count();

        console.log(`📊 Visual Elements:
        - SVG elements: ${svgCount}
        - Canvas elements: ${canvasCount}
        - Stadium-related divs: ${stadiumDivs}`);

        // Try different screen sizes
        const sizes = [
            { width: 1920, height: 1080, name: 'desktop' },
            { width: 1366, height: 768, name: 'laptop' },
            { width: 768, height: 1024, name: 'tablet' }
        ];

        for (const size of sizes) {
            await page.setViewportSize(size);
            await page.waitForTimeout(2000);
            await page.screenshot({
                path: `./.playwright-mcp/stadium-${size.name}.png`
            });
            console.log(`📸 Screenshot saved: stadium-${size.name}.png`);
        }

        console.log('✅ Stadium analysis completed!');

    } catch (error) {
        console.error('❌ Error:', error.message);
        await page.screenshot({
            path: './.playwright-mcp/stadium-error.png',
            fullPage: true
        });
    } finally {
        await browser.close();
    }
})();