const { chromium } = require('playwright');

async function testTribuneTitlesRemoval() {
    console.log('🚀 Testing tribune titles removal in stadium overview...');

    const browser = await chromium.launch({
        headless: false,
        slowMo: 1000,
        ignoreHTTPSErrors: true
    });

    const page = await browser.newPage({
        ignoreHTTPSErrors: true
    });

    try {

        // Navigate to admin login page
        console.log('📍 Navigating to admin login...');
        await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
        await page.waitForTimeout(2000);

        // Fill login form
        console.log('🔑 Logging in as admin...');
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login to complete and page to load
        await page.waitForTimeout(5000);
        console.log('✅ Login attempted, checking current URL...');
        console.log('Current URL:', page.url());

        // Navigate to stadium overview
        console.log('🏟️ Navigating to stadium overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle', timeout: 15000 });
        await page.waitForTimeout(5000);

        // Wait for stadium layout to load completely
        console.log('⏳ Waiting for stadium layout to load...');

        // Wait for loading to disappear and stadium to appear
        try {
            await page.waitForSelector('text=Loading Stadium Layout', { timeout: 5000 });
            console.log('📍 Loading spinner detected');
        } catch (e) {
            console.log('ℹ️  No loading spinner found');
        }

        // Wait for loading to complete
        await page.waitForFunction(() => {
            const loadingText = document.querySelector('text=Loading Stadium Layout');
            return !loadingText || !loadingText.isVisible;
        }, { timeout: 30000 });

        // Give extra time for stadium SVG to render
        await page.waitForTimeout(10000);
        console.log('✅ Stadium layout should be loaded');

        // Take screenshot before checking
        await page.screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\stadium-tribune-titles-test.png',
            fullPage: true
        });
        console.log('📸 Screenshot taken: stadium-tribune-titles-test.png');

        // Check if tribune titles are hidden
        console.log('🔍 Checking tribune title visibility...');

        // Look for tribune title elements with various possible selectors
        const possibleTitleSelectors = ['.stand-title', '.tribune-title', '.tribune-name', 'text:has-text("Tribune")', 'text:has-text("TRIBUNE")'];
        let allTribuneTitles = [];

        for (const selector of possibleTitleSelectors) {
            try {
                const elements = await page.$$(selector);
                if (elements.length > 0) {
                    console.log(`Found ${elements.length} elements with selector: ${selector}`);
                    allTribuneTitles.push(...elements);
                }
            } catch (e) {
                // Ignore selector errors
            }
        }

        console.log(`Total tribune title elements found: ${allTribuneTitles.length}`);

        let visibleTitles = 0;
        let hiddenTitles = 0;

        for (let i = 0; i < allTribuneTitles.length; i++) {
            const isVisible = await allTribuneTitles[i].isVisible();
            if (isVisible) {
                visibleTitles++;
                const text = await allTribuneTitles[i].textContent();
                console.log(`❌ Tribune title still visible: "${text}"`);
            } else {
                hiddenTitles++;
                const text = await allTribuneTitles[i].textContent();
                console.log(`✅ Tribune title hidden: "${text}"`);
            }
        }

        // Check for sectors to ensure layout is intact with various selectors
        const possibleSectorSelectors = ['.sector', '.tribune-sector', '.stadium-sector', 'g[class*="sector"]', 'rect[class*="sector"]'];
        let allSectors = [];

        for (const selector of possibleSectorSelectors) {
            try {
                const elements = await page.$$(selector);
                if (elements.length > 0) {
                    console.log(`Found ${elements.length} sectors with selector: ${selector}`);
                    allSectors.push(...elements);
                }
            } catch (e) {
                // Ignore selector errors
            }
        }

        console.log(`Total sector elements found: ${allSectors.length}`);

        let visibleSectors = 0;
        for (let sector of allSectors) {
            const isVisible = await sector.isVisible();
            if (isVisible) {
                visibleSectors++;
            }
        }

        console.log('\n📊 RESULTS:');
        console.log(`Tribune titles found: ${allTribuneTitles.length}`);
        console.log(`Tribune titles visible: ${visibleTitles}`);
        console.log(`Tribune titles hidden: ${hiddenTitles}`);
        console.log(`Sectors found: ${allSectors.length}`);
        console.log(`Sectors visible: ${visibleSectors}`);

        if (visibleTitles === 0 && allTribuneTitles.length > 0) {
            console.log('✅ SUCCESS: All tribune titles are successfully hidden!');
        } else if (allTribuneTitles.length === 0) {
            console.log('ℹ️  No tribune title elements found in DOM');
        } else {
            console.log(`❌ ISSUE: ${visibleTitles} tribune titles are still visible`);
        }

        if (visibleSectors > 0) {
            console.log('✅ Stadium sectors are still visible and layout is intact');
        } else {
            console.log('❌ WARNING: No sectors visible - stadium layout may be broken');
        }

        // Take a final screenshot focused on stadium area
        await page.locator('.stadium-container').screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\stadium-layout-clean.png'
        });
        console.log('📸 Stadium layout screenshot: stadium-layout-clean.png');

    } catch (error) {
        console.error('❌ Test failed:', error.message);
        await page.screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\stadium-test-error.png',
            fullPage: true
        });
    } finally {
        await browser.close();
    }
}

testTribuneTitlesRemoval();