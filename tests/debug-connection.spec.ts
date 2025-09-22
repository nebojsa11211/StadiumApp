import { test, expect } from '@playwright/test';

test('Debug Connection Test', async ({ page }) => {
    test.setTimeout(30000);

    console.log('🧪 Starting debug connection test...');

    try {
        // Test basic navigation
        console.log('🌐 Testing page navigation...');

        // First try to go to a simple URL to see if browser works at all
        await page.goto('https://www.google.com', { timeout: 10000 });
        console.log('✅ Google access successful');

        // Now test our Customer app
        console.log('🏠 Testing Customer app access...');
        const response = await page.goto('https://localhost:7020/', {
            timeout: 15000,
            waitUntil: 'domcontentloaded'
        });

        console.log(`📡 Response status: ${response?.status()}`);
        console.log(`📍 Current URL: ${page.url()}`);

        // Get page title
        const title = await page.title();
        console.log(`📄 Page title: "${title}"`);

        // Check for any visible text
        const bodyText = await page.locator('body').textContent();
        console.log(`📝 Body text length: ${bodyText?.length || 0} characters`);

        if (bodyText && bodyText.length > 0) {
            console.log(`📝 First 200 chars: "${bodyText.substring(0, 200)}..."`);
        }

        // Check if page loaded successfully
        expect(response?.status()).toBe(200);
        console.log('✅ Debug test completed successfully');

    } catch (error) {
        console.error('❌ Debug test failed:', error);
        throw error;
    }
});