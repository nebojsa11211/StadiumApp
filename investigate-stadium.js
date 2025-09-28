const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

async function investigateStadiumVisualization() {
    const browser = await chromium.launch({
        headless: false,
        args: ['--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
    });

    const context = await browser.newContext({
        ignoreHTTPSErrors: true,
        viewport: { width: 1920, height: 1080 }
    });

    const page = await context.newPage();

    console.log('🔍 Starting Stadium Visualization Investigation...');

    try {
        // Navigate to Admin app
        console.log('📍 Navigating to Admin app...');
        await page.goto('https://localhost:9030', { waitUntil: 'networkidle' });

        // Take screenshot of initial page
        await page.screenshot({ path: 'investigation-1-initial.png', fullPage: true });
        console.log('📸 Screenshot 1: Initial page');

        // Check if we're on login page
        const loginForm = await page.locator('#admin-login-form').isVisible().catch(() => false);
        if (loginForm) {
            console.log('🔐 Login required, attempting login...');

            // Fill login form
            await page.fill('#admin-login-email-input', 'admin@stadium.com');
            await page.fill('#admin-login-password-input', 'admin123');
            await page.click('#admin-login-submit-btn');

            // Wait for redirect
            await page.waitForURL(/dashboard|overview/i, { timeout: 10000 });
            await page.screenshot({ path: 'investigation-2-after-login.png', fullPage: true });
            console.log('📸 Screenshot 2: After login');
        }

        // Navigate to Stadium Overview
        console.log('🏟️ Navigating to Stadium Overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });
        await page.waitForTimeout(2000); // Allow for loading

        // Take screenshot of stadium overview page
        await page.screenshot({ path: 'investigation-3-stadium-overview.png', fullPage: true });
        console.log('📸 Screenshot 3: Stadium Overview page');

        // Check console errors
        const consoleMessages = [];
        page.on('console', msg => {
            consoleMessages.push({
                type: msg.type(),
                text: msg.text(),
                location: msg.location()
            });
        });

        // Wait a bit more for any delayed rendering
        await page.waitForTimeout(3000);

        // Investigate DOM structure
        console.log('🔍 Investigating DOM structure...');

        // Check for stadium container elements
        const stadiumContainer = await page.locator('#admin-stadium-container').isVisible().catch(() => false);
        const stadiumFlexLayout = await page.locator('#admin-stadium-flex-layout').isVisible().catch(() => false);
        const stadiumField = await page.locator('#stadium-field').isVisible().catch(() => false);

        console.log(`Stadium Container visible: ${stadiumContainer}`);
        console.log(`Stadium Flex Layout visible: ${stadiumFlexLayout}`);
        console.log(`Stadium Field visible: ${stadiumField}`);

        // Check for any stadium-related elements
        const allStadiumElements = await page.$$('[id*="stadium"], [class*="stadium"]');
        console.log(`Found ${allStadiumElements.length} stadium-related elements in DOM`);

        // Get detailed info about stadium elements
        const stadiumElementsInfo = [];
        for (let i = 0; i < Math.min(allStadiumElements.length, 10); i++) {
            const element = allStadiumElements[i];
            const id = await element.getAttribute('id');
            const className = await element.getAttribute('class');
            const isVisible = await element.isVisible();
            const boundingBox = await element.boundingBox();
            const computedStyle = await page.evaluate(el => {
                const styles = window.getComputedStyle(el);
                return {
                    display: styles.display,
                    visibility: styles.visibility,
                    opacity: styles.opacity,
                    width: styles.width,
                    height: styles.height,
                    position: styles.position
                };
            }, element);

            stadiumElementsInfo.push({
                index: i,
                id: id,
                className: className,
                isVisible: isVisible,
                boundingBox: boundingBox,
                computedStyle: computedStyle
            });
        }

        // Check network requests for API data
        const apiRequests = [];
        page.on('response', response => {
            if (response.url().includes('/api/')) {
                apiRequests.push({
                    url: response.url(),
                    status: response.status(),
                    contentType: response.headers()['content-type']
                });
            }
        });

        // Force refresh to capture API requests
        await page.reload({ waitUntil: 'networkidle' });
        await page.waitForTimeout(2000);

        // Check if there's loading state
        const loadingElements = await page.locator('[class*="loading"], [id*="loading"]').count();
        const errorElements = await page.locator('[class*="error"], [id*="error"]').count();
        const emptyElements = await page.locator('[class*="empty"], [id*="empty"]').count();

        console.log(`Loading elements: ${loadingElements}`);
        console.log(`Error elements: ${errorElements}`);
        console.log(`Empty elements: ${emptyElements}`);

        // Get page content for analysis
        const pageContent = await page.content();
        const hasStadiumData = pageContent.includes('tribune') || pageContent.includes('sector') || pageContent.includes('seat');

        // Check specific stadium-related text content
        const stadiumTexts = await page.locator('text=/stadium|tribune|sector|seat/i').count();
        console.log(`Found ${stadiumTexts} stadium-related text elements`);

        // Take a focused screenshot of the main content area
        const mainContent = page.locator('.main, [role="main"], .content');
        if (await mainContent.isVisible()) {
            await mainContent.screenshot({ path: 'investigation-4-main-content.png' });
            console.log('📸 Screenshot 4: Main content area');
        }

        // Generate investigation report
        const report = {
            timestamp: new Date().toISOString(),
            page_url: page.url(),
            stadium_elements: {
                container_visible: stadiumContainer,
                flex_layout_visible: stadiumFlexLayout,
                field_visible: stadiumField,
                total_stadium_elements: allStadiumElements.length,
                elements_details: stadiumElementsInfo
            },
            page_state: {
                loading_elements: loadingElements,
                error_elements: errorElements,
                empty_elements: emptyElements,
                has_stadium_data_in_html: hasStadiumData,
                stadium_text_elements: stadiumTexts
            },
            console_messages: consoleMessages,
            api_requests: apiRequests,
            page_title: await page.title(),
            viewport_size: await page.viewportSize()
        };

        // Save report
        fs.writeFileSync('stadium-investigation-report.json', JSON.stringify(report, null, 2));
        console.log('📄 Investigation report saved to stadium-investigation-report.json');

        // Take final screenshot
        await page.screenshot({ path: 'investigation-5-final.png', fullPage: true });
        console.log('📸 Screenshot 5: Final state');

    } catch (error) {
        console.error('❌ Investigation failed:', error);
        await page.screenshot({ path: 'investigation-error.png', fullPage: true });
    } finally {
        await browser.close();
        console.log('✅ Investigation completed');
    }
}

investigateStadiumVisualization().catch(console.error);