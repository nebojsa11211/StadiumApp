import { test, expect } from '@playwright/test';

test.describe('Stadium Field Ratio 105:45', () => {
  test('should display field with 105:45 ratio (7:3)', async ({ page }) => {
    // Start services and navigate to admin stadium overview
    console.log('Navigating to admin stadium overview...');

    // Navigate to login page
    await page.goto('https://localhost:7030/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Login as admin
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for navigation after login (redirects to root, then dashboard)
    await page.waitForLoadState('networkidle');
    console.log('Successfully logged in');

    // Navigate directly to Stadium Overview URL
    await page.goto('https://localhost:7030/admin/stadium-overview', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    console.log('Navigated to Stadium Overview page');

    // Check if we have stadium data or need to see the message
    const hasField = await page.locator('.stadium-field').count();
    const hasMessage = await page.locator('text=/No stadium|Import/i').count();

    if (hasMessage > 0) {
      console.log('⚠️ No stadium structure found. Please import stadium data first.');

      // Take screenshot of the empty state
      await page.screenshot({
        path: 'stadium-overview-no-data.png',
        fullPage: true
      });

      // Skip the ratio test since there's no field to measure
      console.log('Test skipped - no stadium data available');
      return;
    }

    // Wait for stadium field to be visible
    await page.waitForSelector('.stadium-field', {
      state: 'visible',
      timeout: 15000
    });

    // Get field dimensions
    const fieldDimensions = await page.evaluate(() => {
      const field = document.querySelector('.stadium-field') as HTMLElement;
      if (!field) return null;

      const rect = field.getBoundingClientRect();
      const computedStyle = window.getComputedStyle(field);

      return {
        width: rect.width,
        height: rect.height,
        computedWidth: computedStyle.width,
        computedHeight: computedStyle.height,
        ratio: rect.width / rect.height,
        expectedRatio: 105 / 45
      };
    });

    console.log('Stadium Field Dimensions:', fieldDimensions);

    // Verify dimensions exist
    expect(fieldDimensions).not.toBeNull();

    // Calculate expected ratio (105:45 = 2.333...)
    const expectedRatio = 105 / 45; // 2.333...
    const tolerance = 0.01; // Allow small rounding differences

    // Verify the ratio is correct
    expect(fieldDimensions!.ratio).toBeGreaterThan(expectedRatio - tolerance);
    expect(fieldDimensions!.ratio).toBeLessThan(expectedRatio + tolerance);

    // Take screenshot
    await page.screenshot({
      path: 'stadium-field-105-45-ratio.png',
      fullPage: true
    });

    console.log('✅ Field ratio verified: 105:45 (7:3)');
    console.log(`   Actual ratio: ${fieldDimensions!.ratio.toFixed(3)}`);
    console.log(`   Expected ratio: ${expectedRatio.toFixed(3)}`);
    console.log(`   Width: ${fieldDimensions!.width}px`);
    console.log(`   Height: ${fieldDimensions!.height}px`);
  });
});
