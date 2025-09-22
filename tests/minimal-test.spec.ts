import { test, expect } from '@playwright/test';

test('Minimal Test', async ({ page }) => {
    test.setTimeout(10000);

    console.log('Starting minimal test...');

    // Just try to check if the page object works
    console.log('Page object created successfully');

    // Try very basic operation
    await page.setViewportSize({ width: 1280, height: 720 });
    console.log('Viewport set successfully');

    expect(true).toBe(true);
    console.log('Test completed successfully');
});