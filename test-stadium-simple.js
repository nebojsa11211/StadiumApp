const { chromium } = require('playwright');

async function testStadiumSimple() {
    console.log('🏟️ Testing stadium overview page...');

    const browser = await chromium.launch({
        headless: false,
        slowMo: 500,
        ignoreHTTPSErrors: true
    });

    const page = await browser.newPage({
        ignoreHTTPSErrors: true
    });

    try {
        // Navigate to admin login page
        console.log('📍 Going to admin login...');
        await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
        await page.waitForTimeout(3000);

        // Fill login form
        console.log('🔑 Logging in...');
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');
        await page.waitForTimeout(5000);

        // Navigate to stadium overview
        console.log('🏟️ Going to stadium overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });
        await page.waitForTimeout(10000);

        // Take full page screenshot
        console.log('📸 Taking full page screenshot...');
        await page.screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\stadium-overview-full-page.png',
            fullPage: true
        });

        // Get page HTML for analysis
        const html = await page.content();
        console.log('📄 Page loaded, checking content...');

        // Check for loading indicators
        const hasLoadingSpinner = html.includes('Loading Stadium Layout') || html.includes('loading');
        const hasStadiumContainer = html.includes('stadium-container') || html.includes('stadium-grid');
        const hasTribuneText = html.includes('Tribune') || html.includes('TRIBUNE');
        const hasSectorElements = html.includes('sector') || html.includes('Sector');

        console.log('\n📊 PAGE ANALYSIS:');
        console.log(`Loading indicator present: ${hasLoadingSpinner}`);
        console.log(`Stadium container present: ${hasStadiumContainer}`);
        console.log(`Tribune text in HTML: ${hasTribuneText}`);
        console.log(`Sector elements present: ${hasSectorElements}`);

        // Search for specific text patterns
        if (hasTribuneText) {
            const tribuneMatches = html.match(/\b\w*[Tt]ribune\w*\b/g);
            console.log(`Tribune text found: ${tribuneMatches ? tribuneMatches.join(', ') : 'none'}`);
        }

        // Check computed styles for .stand-title elements
        console.log('\n🎨 CSS ANALYSIS:');
        const titleElements = await page.$$('.stand-title');
        console.log(`Found ${titleElements.length} .stand-title elements`);

        for (let i = 0; i < titleElements.length; i++) {
            const element = titleElements[i];
            const isVisible = await element.isVisible();
            const text = await element.textContent();
            const display = await element.evaluate(el => window.getComputedStyle(el).display);
            console.log(`Title ${i + 1}: "${text}" - display: ${display}, visible: ${isVisible}`);
        }

        // Check for sectors
        const sectorElements = await page.$$('.sector');
        console.log(`Found ${sectorElements.length} .sector elements`);

        if (sectorElements.length > 0) {
            let visibleSectors = 0;
            for (const sector of sectorElements) {
                if (await sector.isVisible()) {
                    visibleSectors++;
                }
            }
            console.log(`Visible sectors: ${visibleSectors} / ${sectorElements.length}`);
        }

        console.log('\n✅ FINAL RESULT:');
        if (titleElements.length > 0) {
            const allHidden = titleElements.every(async el => !(await el.isVisible()));
            if (allHidden) {
                console.log('🎯 SUCCESS: All tribune titles are hidden with CSS!');
            } else {
                console.log('⚠️  PARTIAL: Some tribune titles may still be visible');
            }
        } else {
            console.log('ℹ️  No .stand-title elements found in DOM');
        }

        if (sectorElements.length > 0) {
            console.log('✅ Stadium layout has sectors and appears to be working');
        } else {
            console.log('❌ No stadium sectors found - layout may not be loaded');
        }

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

testStadiumSimple();