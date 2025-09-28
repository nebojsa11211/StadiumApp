const { chromium } = require('playwright');
const fs = require('fs');

async function investigateStadiumOverview() {
    const browser = await chromium.launch({
        headless: false,
        args: ['--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
    });

    const context = await browser.newContext({
        ignoreHTTPSErrors: true,
        viewport: { width: 1920, height: 1080 }
    });

    const page = await context.newPage();

    console.log('🏟️ Starting Stadium Overview Investigation...');

    const consoleMessages = [];
    const apiRequests = [];
    const networkErrors = [];

    // Capture console messages
    page.on('console', msg => {
        const message = {
            type: msg.type(),
            text: msg.text(),
            location: msg.location()
        };
        consoleMessages.push(message);
        console.log(`Console [${msg.type()}]: ${msg.text()}`);
    });

    // Capture network requests
    page.on('response', response => {
        if (response.url().includes('/api/')) {
            apiRequests.push({
                url: response.url(),
                status: response.status(),
                contentType: response.headers()['content-type'],
                ok: response.ok()
            });
            console.log(`API Request: ${response.url()} - ${response.status()}`);
        }
    });

    // Capture network errors
    page.on('requestfailed', request => {
        networkErrors.push({
            url: request.url(),
            failure: request.failure()
        });
        console.log(`Network Error: ${request.url()} - ${request.failure()?.errorText}`);
    });

    try {
        // Navigate directly to Stadium Overview (assuming already logged in)
        console.log('📍 Navigating to Stadium Overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Wait for page to load
        await page.waitForTimeout(3000);

        // Take initial screenshot
        await page.screenshot({ path: 'stadium-overview-1-initial.png', fullPage: true });
        console.log('📸 Screenshot 1: Stadium Overview initial load');

        // Check page title and URL
        const pageTitle = await page.title();
        const currentUrl = page.url();
        console.log(`Page Title: ${pageTitle}`);
        console.log(`Current URL: ${currentUrl}`);

        // Check if we're on the right page or redirected to login
        if (currentUrl.includes('/login')) {
            console.log('🔐 Redirected to login - need to authenticate first');

            // Login
            await page.fill('#admin-login-email-input', 'admin@stadium.com');
            await page.fill('#admin-login-password-input', 'admin123');
            await page.click('#admin-login-submit-btn');
            await page.waitForTimeout(2000);

            // Navigate back to stadium overview
            await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });
            await page.waitForTimeout(3000);

            await page.screenshot({ path: 'stadium-overview-2-after-login.png', fullPage: true });
            console.log('📸 Screenshot 2: After login redirect');
        }

        // Investigate stadium container elements
        console.log('🔍 Investigating stadium elements...');

        const stadiumElements = [
            '#admin-stadium-container',
            '#admin-stadium-flex-layout',
            '#stadium-field',
            '[id*="stadium"]',
            '[class*="stadium"]',
            '.stadium-container',
            '.stadium-visualization'
        ];

        const elementAnalysis = {};

        for (const selector of stadiumElements) {
            try {
                const elements = await page.locator(selector).all();
                elementAnalysis[selector] = {
                    count: elements.length,
                    details: []
                };

                for (let i = 0; i < Math.min(elements.length, 5); i++) {
                    const element = elements[i];
                    const isVisible = await element.isVisible();
                    const boundingBox = await element.boundingBox();
                    const innerHTML = await element.innerHTML().catch(() => 'Unable to get innerHTML');
                    const computedStyle = await page.evaluate(el => {
                        const styles = window.getComputedStyle(el);
                        return {
                            display: styles.display,
                            visibility: styles.visibility,
                            opacity: styles.opacity,
                            width: styles.width,
                            height: styles.height,
                            backgroundColor: styles.backgroundColor,
                            color: styles.color,
                            position: styles.position,
                            zIndex: styles.zIndex
                        };
                    }, element).catch(() => null);

                    elementAnalysis[selector].details.push({
                        index: i,
                        isVisible,
                        boundingBox,
                        innerHTML: innerHTML.substring(0, 500),
                        computedStyle
                    });
                }
            } catch (error) {
                elementAnalysis[selector] = { error: error.message };
            }
        }

        // Check for loading states
        const loadingStates = {
            hasLoadingSpinner: await page.locator('[class*="loading"], [class*="spinner"]').count() > 0,
            hasErrorMessage: await page.locator('[class*="error"], [class*="alert-danger"]').count() > 0,
            hasEmptyState: await page.locator('[class*="empty"], [class*="no-data"]').count() > 0
        };

        // Check for stadium data in page content
        const pageContent = await page.content();
        const stadiumDataIndicators = {
            hasTribuneText: pageContent.includes('tribune') || pageContent.includes('Tribune'),
            hasSectorText: pageContent.includes('sector') || pageContent.includes('Sector'),
            hasSeatText: pageContent.includes('seat') || pageContent.includes('Seat'),
            hasStadiumJson: pageContent.includes('"stadium"') || pageContent.includes('"Stadium"'),
            hasCoordinatesData: pageContent.includes('coordinates') || pageContent.includes('x":') || pageContent.includes('y":')
        };

        // Take screenshot of main content area
        const mainContent = page.locator('main, [role="main"], .main-content, .content');
        if (await mainContent.count() > 0) {
            await mainContent.first().screenshot({ path: 'stadium-overview-3-main-content.png' });
            console.log('📸 Screenshot 3: Main content area');
        }

        // Try to find and focus on any stadium visualization area
        const stadiumVizArea = page.locator('.stadium, #stadium, [class*="stadium-viz"]');
        if (await stadiumVizArea.count() > 0) {
            await stadiumVizArea.first().screenshot({ path: 'stadium-overview-4-visualization.png' });
            console.log('📸 Screenshot 4: Stadium visualization area');
        }

        // Check for any SVG elements (often used for stadium layouts)
        const svgElements = await page.locator('svg').count();
        const canvasElements = await page.locator('canvas').count();
        console.log(`SVG elements found: ${svgElements}`);
        console.log(`Canvas elements found: ${canvasElements}`);

        // Get text content analysis
        const pageText = await page.textContent('body');
        const stadiumRelatedText = {
            containsStadium: pageText?.includes('stadium') || pageText?.includes('Stadium'),
            containsVisualization: pageText?.includes('visualization') || pageText?.includes('Visualization'),
            containsOverview: pageText?.includes('overview') || pageText?.includes('Overview'),
            containsEmpty: pageText?.includes('empty') || pageText?.includes('No data'),
            containsError: pageText?.includes('error') || pageText?.includes('Error')
        };

        // Final screenshot
        await page.screenshot({ path: 'stadium-overview-5-final.png', fullPage: true });
        console.log('📸 Screenshot 5: Final state');

        // Generate comprehensive report
        const report = {
            timestamp: new Date().toISOString(),
            pageInfo: {
                title: pageTitle,
                url: currentUrl,
                redirectedToLogin: currentUrl.includes('/login')
            },
            stadiumElements: elementAnalysis,
            loadingStates,
            stadiumDataIndicators,
            stadiumRelatedText,
            visualElements: {
                svgCount: svgElements,
                canvasCount: canvasElements
            },
            consoleMessages,
            apiRequests,
            networkErrors,
            recommendations: []
        };

        // Add recommendations based on findings
        if (Object.values(elementAnalysis).every(el => el.count === 0 || el.error)) {
            report.recommendations.push('No stadium elements found - stadium visualization may not be implemented');
        }

        if (loadingStates.hasErrorMessage) {
            report.recommendations.push('Error messages detected - check for API or data loading issues');
        }

        if (!stadiumDataIndicators.hasTribuneText && !stadiumDataIndicators.hasSectorText) {
            report.recommendations.push('No stadium structure data detected - may need to import stadium data');
        }

        if (apiRequests.length === 0) {
            report.recommendations.push('No API requests detected - stadium data may not be loading from backend');
        }

        if (svgElements === 0 && canvasElements === 0) {
            report.recommendations.push('No SVG or Canvas elements found - visualization may use HTML/CSS only');
        }

        // Save detailed report
        fs.writeFileSync('stadium-overview-investigation-report.json', JSON.stringify(report, null, 2));
        console.log('📄 Investigation report saved');

        console.log('\n🔍 INVESTIGATION SUMMARY:');
        console.log(`- Stadium elements found: ${Object.entries(elementAnalysis).map(([k,v]) => `${k}: ${v.count || 0}`).join(', ')}`);
        console.log(`- Loading states: ${JSON.stringify(loadingStates)}`);
        console.log(`- API requests: ${apiRequests.length}`);
        console.log(`- Console messages: ${consoleMessages.length}`);
        console.log(`- Stadium data indicators: ${Object.entries(stadiumDataIndicators).filter(([k,v]) => v).map(([k]) => k).join(', ') || 'None'}`);

    } catch (error) {
        console.error('❌ Investigation failed:', error);
        await page.screenshot({ path: 'stadium-overview-error.png', fullPage: true });
    } finally {
        await browser.close();
        console.log('✅ Investigation completed');
    }
}

investigateStadiumOverview().catch(console.error);