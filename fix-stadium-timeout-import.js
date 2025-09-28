const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

async function fixStadiumTimeoutByImporting() {
    console.log('🔧 Starting automated stadium data import to fix timeout issue...');

    const browser = await chromium.launch({
        headless: false,
        args: ['--ignore-certificate-errors', '--ignore-ssl-errors']
    });

    try {
        const context = await browser.newContext({
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Step 1: Login to admin
        console.log('🔐 Logging into admin interface...');
        await page.goto('https://localhost:9030/login');
        await page.waitForSelector('#admin-login-email-input');

        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login redirect
        await page.waitForURL('**/dashboard', { timeout: 10000 });
        console.log('✅ Logged in successfully');

        // Step 2: Navigate to Stadium Structure management
        console.log('📍 Navigating to Stadium Structure management...');
        await page.goto('https://localhost:9030/stadium-structure');
        await page.waitForSelector('h1:has-text("Stadium Structure Management")', { timeout: 10000 });
        console.log('✅ Reached Stadium Structure page');

        // Step 3: Check if stadium data already exists
        const hasExistingData = await page.locator('text=Clear Stadium Structure').isVisible({ timeout: 2000 }).catch(() => false);

        if (hasExistingData) {
            console.log('⚠️ Stadium data already exists, clearing first...');
            await page.click('text=Clear Stadium Structure');
            await page.click('text=Yes, Clear Structure');
            await page.waitForSelector('text=Stadium structure cleared successfully', { timeout: 5000 });
            console.log('✅ Existing stadium data cleared');
        }

        // Step 4: Import stadium structure JSON
        console.log('📁 Importing stadium structure from JSON file...');

        const fileInput = page.locator('input[type="file"]');
        const stadiumJsonPath = path.resolve('test-stadium-4tribunes.json');

        if (!fs.existsSync(stadiumJsonPath)) {
            throw new Error(`Stadium JSON file not found: ${stadiumJsonPath}`);
        }

        await fileInput.setInputFiles(stadiumJsonPath);
        console.log('📤 File selected, starting import...');

        // Click import button
        await page.click('button:has-text("Import Stadium Structure")');

        // Wait for import success
        try {
            await page.waitForSelector('text=Stadium structure imported successfully', { timeout: 30000 });
            console.log('✅ Stadium structure imported successfully!');

            // Check import summary
            const summary = await page.locator('.alert-success').textContent();
            console.log('📊 Import Summary:', summary);

        } catch (error) {
            // Check for any error messages
            const errorElement = await page.locator('.alert-danger').first();
            if (await errorElement.isVisible()) {
                const errorText = await errorElement.textContent();
                console.error('❌ Import failed:', errorText);
                throw new Error(`Stadium import failed: ${errorText}`);
            }
            throw error;
        }

        // Step 5: Test Stadium Overview page
        console.log('🏟️ Testing Stadium Overview page...');
        await page.goto('https://localhost:9030/stadium-overview');

        // Wait for stadium container to appear (no more timeout error)
        try {
            await page.waitForSelector('#admin-stadium-container', { timeout: 15000 });
            console.log('✅ Stadium Overview loaded successfully - timeout issue resolved!');

            // Check if stadium visualization is working
            const stadiumSvg = await page.locator('#admin-stadium-container svg').first();
            if (await stadiumSvg.isVisible()) {
                console.log('🎯 Stadium visualization is rendering correctly');

                // Take screenshot for verification
                await page.screenshot({
                    path: 'stadium-timeout-fixed-verification.png',
                    fullPage: true
                });
                console.log('📸 Verification screenshot saved');

                return {
                    success: true,
                    message: 'Stadium timeout issue resolved successfully',
                    stadiumVisible: true,
                    screenshotPath: 'stadium-timeout-fixed-verification.png'
                };
            } else {
                console.log('⚠️ Stadium container loaded but no SVG visualization found');
                return {
                    success: true,
                    message: 'Stadium data imported but visualization needs review',
                    stadiumVisible: false
                };
            }

        } catch (timeoutError) {
            console.error('❌ Stadium Overview still showing timeout error');

            // Check for error message
            const errorElement = await page.locator('text=Stadium Layout Error').first();
            if (await errorElement.isVisible()) {
                const errorText = await errorElement.textContent();
                console.error('Error details:', errorText);
            }

            throw new Error('Stadium Overview timeout still persists after data import');
        }

    } catch (error) {
        console.error('❌ Error during stadium timeout fix:', error.message);

        // Take error screenshot
        await page.screenshot({
            path: 'stadium-timeout-fix-error.png',
            fullPage: true
        }).catch(() => {});

        throw error;

    } finally {
        await browser.close();
    }
}

// Execute the fix
if (require.main === module) {
    fixStadiumTimeoutByImporting()
        .then(result => {
            console.log('🎉 Stadium timeout fix completed:', result);
        })
        .catch(error => {
            console.error('💥 Stadium timeout fix failed:', error.message);
            process.exit(1);
        });
}

module.exports = { fixStadiumTimeoutByImporting };