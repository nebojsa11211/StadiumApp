const { chromium } = require('playwright');

async function testStadiumCenterCircle() {
    const browser = await chromium.launch({
        headless: false,
        slowMo: 1000
    });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 },
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        console.log('🔗 Navigating to admin login page...');
        await page.goto('https://localhost:9030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Take screenshot of login page
        await page.screenshot({
            path: '.playwright-mcp/stadium-center-circle-01-login.png',
            fullPage: true
        });

        console.log('🔐 Logging in as admin...');
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login redirect to complete (could be dashboard or home)
        console.log('⏳ Waiting for login to complete...');
        await page.waitForFunction(() => !window.location.pathname.includes('/login'), { timeout: 10000 });
        await page.waitForLoadState('networkidle');

        console.log('🏟️ Navigating to stadium overview...');
        await page.goto('https://localhost:9030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Wait for stadium rendering
        await page.waitForTimeout(3000);

        // Take full page screenshot
        console.log('📸 Taking full page screenshot...');
        await page.screenshot({
            path: '.playwright-mcp/stadium-center-circle-02-full-page.png',
            fullPage: true
        });

        // Check if field markings exist
        const fieldMarkings = await page.locator('#admin-stadium-field-markings').isVisible();
        console.log(`🏈 Field markings visible: ${fieldMarkings}`);

        // Check if center circle exists
        const centerCircle = await page.locator('.center-circle').isVisible();
        console.log(`⭕ Center circle visible: ${centerCircle}`);

        // Check if center line exists
        const centerLine = await page.locator('.center-line').isVisible();
        console.log(`📏 Center line visible: ${centerLine}`);

        // Take a focused screenshot of the stadium field area
        const stadiumField = page.locator('#admin-stadium-field');
        if (await stadiumField.isVisible()) {
            console.log('📸 Taking focused stadium field screenshot...');
            await stadiumField.screenshot({
                path: '.playwright-mcp/stadium-center-circle-03-field-only.png'
            });
        }

        // Inspect CSS positioning with dev tools
        console.log('🔍 Inspecting CSS positioning...');
        const fieldMarkingsInfo = await page.evaluate(() => {
            const fieldMarkings = document.querySelector('#admin-stadium-field-markings');
            const centerCircle = document.querySelector('.center-circle');
            const centerLine = document.querySelector('.center-line');

            if (!fieldMarkings) return { error: 'Field markings not found' };

            const fieldStyle = window.getComputedStyle(fieldMarkings);
            const fieldRect = fieldMarkings.getBoundingClientRect();

            const result = {
                fieldMarkings: {
                    position: fieldStyle.position,
                    inset: fieldStyle.inset,
                    top: fieldStyle.top,
                    left: fieldStyle.left,
                    right: fieldStyle.right,
                    bottom: fieldStyle.bottom,
                    width: fieldRect.width,
                    height: fieldRect.height,
                    x: fieldRect.x,
                    y: fieldRect.y
                }
            };

            if (centerCircle) {
                const circleStyle = window.getComputedStyle(centerCircle);
                const circleRect = centerCircle.getBoundingClientRect();
                result.centerCircle = {
                    position: circleStyle.position,
                    top: circleStyle.top,
                    left: circleStyle.left,
                    transform: circleStyle.transform,
                    width: circleRect.width,
                    height: circleRect.height,
                    x: circleRect.x,
                    y: circleRect.y,
                    centerX: circleRect.x + circleRect.width / 2,
                    centerY: circleRect.y + circleRect.height / 2
                };

                // Calculate if center circle is actually centered
                const fieldCenterX = fieldRect.x + fieldRect.width / 2;
                const fieldCenterY = fieldRect.y + fieldRect.height / 2;

                result.centerCircle.isCenteredX = Math.abs(result.centerCircle.centerX - fieldCenterX) < 5;
                result.centerCircle.isCenteredY = Math.abs(result.centerCircle.centerY - fieldCenterY) < 5;
                result.centerCircle.fieldCenterX = fieldCenterX;
                result.centerCircle.fieldCenterY = fieldCenterY;
                result.centerCircle.offsetX = result.centerCircle.centerX - fieldCenterX;
                result.centerCircle.offsetY = result.centerCircle.centerY - fieldCenterY;
            }

            if (centerLine) {
                const lineStyle = window.getComputedStyle(centerLine);
                const lineRect = centerLine.getBoundingClientRect();
                result.centerLine = {
                    position: lineStyle.position,
                    top: lineStyle.top,
                    left: lineStyle.left,
                    width: lineRect.width,
                    height: lineRect.height,
                    x: lineRect.x,
                    y: lineRect.y
                };
            }

            return result;
        });

        console.log('📊 CSS Positioning Analysis:');
        console.log(JSON.stringify(fieldMarkingsInfo, null, 2));

        // Take screenshot with dev tools open (if possible)
        await page.keyboard.press('F12');
        await page.waitForTimeout(2000);

        console.log('📸 Taking screenshot with dev tools...');
        await page.screenshot({
            path: '.playwright-mcp/stadium-center-circle-04-with-devtools.png',
            fullPage: true
        });

        // Verify center circle positioning
        if (fieldMarkingsInfo.centerCircle) {
            const { isCenteredX, isCenteredY, offsetX, offsetY } = fieldMarkingsInfo.centerCircle;

            console.log('\n🎯 CENTER CIRCLE POSITIONING ANALYSIS:');
            console.log(`✅ Center Circle Found: YES`);
            console.log(`📍 Horizontally Centered: ${isCenteredX ? 'YES' : 'NO'} (offset: ${offsetX.toFixed(2)}px)`);
            console.log(`📍 Vertically Centered: ${isCenteredY ? 'YES' : 'NO'} (offset: ${offsetY.toFixed(2)}px)`);

            if (isCenteredX && isCenteredY) {
                console.log('🎉 SUCCESS: Center circle is properly positioned in the center of the field!');
            } else {
                console.log('❌ ISSUE: Center circle is not properly centered');
                console.log(`   - Horizontal offset: ${offsetX.toFixed(2)}px`);
                console.log(`   - Vertical offset: ${offsetY.toFixed(2)}px`);
            }
        } else {
            console.log('❌ ISSUE: Center circle element not found');
        }

        console.log('\n✅ Test completed successfully!');

    } catch (error) {
        console.error('❌ Error during test:', error);

        // Take error screenshot
        try {
            const context = await browser.contexts()[0];
            const page = context.pages()[0];
            if (page) {
                await page.screenshot({
                    path: '.playwright-mcp/stadium-center-circle-error.png',
                    fullPage: true
                });
            }
        } catch (screenshotError) {
            console.error('Failed to take error screenshot:', screenshotError);
        }
    } finally {
        await browser.close();
    }
}

// Run the test
testStadiumCenterCircle().catch(console.error);