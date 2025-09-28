const { chromium } = require('playwright');

async function checkCenterCirclePosition() {
    const browser = await chromium.launch({ headless: false });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 },
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Navigate and login
        await page.goto('https://localhost:9030/login');
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login and navigate to stadium overview
        await page.waitForFunction(() => !window.location.pathname.includes('/login'), { timeout: 10000 });
        await page.goto('https://localhost:9030/admin/stadium-overview');
        await page.waitForLoadState('networkidle');
        await page.waitForTimeout(3000);

        // Check positioning
        const positionInfo = await page.evaluate(() => {
            const fieldMarkings = document.querySelector('#admin-stadium-field-markings');
            const stadiumField = document.querySelector('#admin-stadium-field');
            const centerCircle = document.querySelector('.center-circle');
            const centerLine = document.querySelector('.center-line');

            if (!fieldMarkings || !stadiumField || !centerCircle) {
                return { error: 'Required elements not found' };
            }

            const fieldStyle = window.getComputedStyle(fieldMarkings);
            const fieldRect = fieldMarkings.getBoundingClientRect();
            const stadiumFieldRect = stadiumField.getBoundingClientRect();
            const circleStyle = window.getComputedStyle(centerCircle);
            const circleRect = centerCircle.getBoundingClientRect();

            // Calculate centers
            const fieldCenterX = fieldRect.x + fieldRect.width / 2;
            const fieldCenterY = fieldRect.y + fieldRect.height / 2;
            const circleCenterX = circleRect.x + circleRect.width / 2;
            const circleCenterY = circleRect.y + circleRect.height / 2;

            return {
                stadiumField: {
                    rect: {
                        x: stadiumFieldRect.x,
                        y: stadiumFieldRect.y,
                        width: stadiumFieldRect.width,
                        height: stadiumFieldRect.height
                    }
                },
                fieldMarkings: {
                    position: fieldStyle.position,
                    inset: fieldStyle.inset,
                    top: fieldStyle.top,
                    left: fieldStyle.left,
                    right: fieldStyle.right,
                    bottom: fieldStyle.bottom,
                    rect: {
                        x: fieldRect.x,
                        y: fieldRect.y,
                        width: fieldRect.width,
                        height: fieldRect.height
                    },
                    centerX: fieldCenterX,
                    centerY: fieldCenterY
                },
                centerCircle: {
                    position: circleStyle.position,
                    top: circleStyle.top,
                    left: circleStyle.left,
                    transform: circleStyle.transform,
                    rect: {
                        x: circleRect.x,
                        y: circleRect.y,
                        width: circleRect.width,
                        height: circleRect.height
                    },
                    centerX: circleCenterX,
                    centerY: circleCenterY,
                    offsetFromFieldCenterX: circleCenterX - fieldCenterX,
                    offsetFromFieldCenterY: circleCenterY - fieldCenterY,
                    isCenteredX: Math.abs(circleCenterX - fieldCenterX) < 5,
                    isCenteredY: Math.abs(circleCenterY - fieldCenterY) < 5
                }
            };
        });

        console.log('\n🎯 CENTER CIRCLE POSITION ANALYSIS:');
        console.log('=====================================');

        if (positionInfo.error) {
            console.log('❌ Error:', positionInfo.error);
            return;
        }

        const { fieldMarkings, centerCircle } = positionInfo;

        console.log('📏 Field Markings:');
        console.log(`   Position: ${fieldMarkings.position}`);
        console.log(`   Inset: ${fieldMarkings.inset}`);
        console.log(`   Dimensions: ${fieldMarkings.rect.width}x${fieldMarkings.rect.height}`);
        console.log(`   Center: (${fieldMarkings.centerX.toFixed(1)}, ${fieldMarkings.centerY.toFixed(1)})`);

        console.log('\n⭕ Center Circle:');
        console.log(`   Position: ${centerCircle.position}`);
        console.log(`   Top: ${centerCircle.top}`);
        console.log(`   Left: ${centerCircle.left}`);
        console.log(`   Transform: ${centerCircle.transform}`);
        console.log(`   Dimensions: ${centerCircle.rect.width}x${centerCircle.rect.height}`);
        console.log(`   Center: (${centerCircle.centerX.toFixed(1)}, ${centerCircle.centerY.toFixed(1)})`);

        console.log('\n🎯 POSITIONING RESULTS:');
        console.log(`   Horizontal Offset: ${centerCircle.offsetFromFieldCenterX.toFixed(2)}px`);
        console.log(`   Vertical Offset: ${centerCircle.offsetFromFieldCenterY.toFixed(2)}px`);
        console.log(`   Horizontally Centered: ${centerCircle.isCenteredX ? '✅ YES' : '❌ NO'}`);
        console.log(`   Vertically Centered: ${centerCircle.isCenteredY ? '✅ YES' : '❌ NO'}`);

        if (centerCircle.isCenteredX && centerCircle.isCenteredY) {
            console.log('\n🎉 SUCCESS: Center circle is properly positioned in the center of the field!');
        } else {
            console.log('\n⚠️ ISSUE: Center circle is not properly centered:');
            if (!centerCircle.isCenteredX) {
                console.log(`   - Horizontal misalignment: ${centerCircle.offsetFromFieldCenterX.toFixed(2)}px`);
            }
            if (!centerCircle.isCenteredY) {
                console.log(`   - Vertical misalignment: ${centerCircle.offsetFromFieldCenterY.toFixed(2)}px`);
            }
        }

        // Take final screenshot
        await page.screenshot({
            path: '.playwright-mcp/center-circle-final-check.png',
            fullPage: true
        });

        console.log('\n📸 Screenshot saved: center-circle-final-check.png');

    } catch (error) {
        console.error('❌ Error:', error);
    } finally {
        await browser.close();
    }
}

checkCenterCirclePosition();