const { chromium } = require('playwright');

async function testWithCacheClear() {
    const browser = await chromium.launch({ headless: false });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 },
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Clear cache and force refresh
        await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
        await page.keyboard.press('F5'); // Force refresh
        await page.waitForTimeout(2000);

        // Login
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');
        await page.waitForFunction(() => !window.location.pathname.includes('/login'), { timeout: 10000 });

        // Navigate to stadium overview with cache busting
        await page.goto('https://localhost:9030/admin/stadium-overview?nocache=' + Date.now(), { waitUntil: 'networkidle' });
        await page.waitForTimeout(3000);

        // Force hard refresh
        await page.keyboard.down('Control');
        await page.keyboard.press('F5');
        await page.keyboard.up('Control');
        await page.waitForTimeout(3000);

        // Take screenshot
        await page.screenshot({
            path: '.playwright-mcp/stadium-after-css-fix.png',
            fullPage: true
        });

        // Check computed styles for debugging
        const styleInfo = await page.evaluate(() => {
            const circleEl = document.querySelector('.center-circle');
            if (!circleEl) return { error: 'Element not found' };

            const style = window.getComputedStyle(circleEl);
            const allRules = [];

            // Get all stylesheets and rules that apply
            for (let sheet of document.styleSheets) {
                try {
                    for (let rule of sheet.cssRules) {
                        if (rule.selectorText && rule.selectorText.includes('.center-circle')) {
                            allRules.push({
                                selector: rule.selectorText,
                                cssText: rule.cssText,
                                href: sheet.href
                            });
                        }
                    }
                } catch (e) {
                    // Cross-origin or other access issues
                }
            }

            return {
                computedPosition: style.position,
                computedTop: style.top,
                computedLeft: style.left,
                computedTransform: style.transform,
                allMatchingRules: allRules,
                elementExists: !!circleEl,
                parentElement: circleEl.parentElement?.className
            };
        });

        console.log('\n🔍 CSS DEBUGGING INFORMATION:');
        console.log('================================');
        console.log(JSON.stringify(styleInfo, null, 2));

        // Position analysis
        const positionInfo = await page.evaluate(() => {
            const fieldMarkings = document.querySelector('#admin-stadium-field-markings');
            const centerCircle = document.querySelector('.center-circle');

            if (!fieldMarkings || !centerCircle) {
                return { error: 'Required elements not found' };
            }

            const fieldRect = fieldMarkings.getBoundingClientRect();
            const circleRect = centerCircle.getBoundingClientRect();

            const fieldCenterX = fieldRect.x + fieldRect.width / 2;
            const fieldCenterY = fieldRect.y + fieldRect.height / 2;
            const circleCenterX = circleRect.x + circleRect.width / 2;
            const circleCenterY = circleRect.y + circleRect.height / 2;

            return {
                fieldCenter: { x: fieldCenterX, y: fieldCenterY },
                circleCenter: { x: circleCenterX, y: circleCenterY },
                offsetX: circleCenterX - fieldCenterX,
                offsetY: circleCenterY - fieldCenterY,
                isCentered: Math.abs(circleCenterX - fieldCenterX) < 5 && Math.abs(circleCenterY - fieldCenterY) < 5
            };
        });

        console.log('\n🎯 FINAL POSITION ANALYSIS:');
        console.log('============================');
        if (positionInfo.error) {
            console.log('❌ Error:', positionInfo.error);
        } else {
            console.log(`Field Center: (${positionInfo.fieldCenter.x.toFixed(1)}, ${positionInfo.fieldCenter.y.toFixed(1)})`);
            console.log(`Circle Center: (${positionInfo.circleCenter.x.toFixed(1)}, ${positionInfo.circleCenter.y.toFixed(1)})`);
            console.log(`Offset: (${positionInfo.offsetX.toFixed(2)}px, ${positionInfo.offsetY.toFixed(2)}px)`);
            console.log(`Is Centered: ${positionInfo.isCentered ? '✅ YES' : '❌ NO'}`);
        }

    } catch (error) {
        console.error('❌ Error:', error);
    } finally {
        await browser.close();
    }
}

testWithCacheClear();