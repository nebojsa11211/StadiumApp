import { test, expect } from '@playwright/test';

const CUSTOMER_BASE_URL = 'https://localhost:7020';

test('Simple Customer App Access Test', async ({ page }) => {
    console.log('🧪 Testing basic Customer app access...');

    // Set a longer timeout for this test
    test.setTimeout(60000);

    try {
        // Navigate to customer app homepage
        console.log('🚀 Navigating to Customer app...');
        await page.goto(`${CUSTOMER_BASE_URL}/`, {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Check if page loaded
        console.log('✅ Page loaded successfully');

        // Check if we can see some content
        const pageContent = await page.content();
        console.log(`📄 Page content length: ${pageContent.length} characters`);

        // Look for any error messages or basic structure
        const hasError = await page.locator('text=Error').isVisible().catch(() => false);
        const hasTitle = await page.locator('title').isVisible().catch(() => false);

        console.log(`🔍 Has error: ${hasError}, Has title: ${hasTitle}`);

        // Try to navigate to login page
        console.log('🔐 Testing login page access...');
        await page.goto(`${CUSTOMER_BASE_URL}/login`, {
            waitUntil: 'networkidle',
            timeout: 15000
        });

        // Check for login form elements
        const loginElements = {
            emailInput: await page.locator('input[type="email"]').isVisible().catch(() => false),
            passwordInput: await page.locator('input[type="password"]').isVisible().catch(() => false),
            submitButton: await page.locator('button[type="submit"]').isVisible().catch(() => false)
        };

        console.log('🔐 Login form elements:', loginElements);

        console.log('✅ Simple test completed successfully');

    } catch (error) {
        console.error('❌ Test failed:', error);
        throw error;
    }
});