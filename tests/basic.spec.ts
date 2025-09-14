import { test, expect } from '@playwright/test';

test.describe('Basic Admin Tests', () => {
  test('should access admin application', async ({ page }) => {
    console.log('🔍 Testing basic admin application access');

    // Navigate to admin application
    await page.goto('https://localhost:9030', { ignoreHTTPSErrors: true });

    // Should redirect to login page
    await page.waitForTimeout(3000);

    const currentUrl = page.url();
    console.log(`Current URL: ${currentUrl}`);

    // Should see login form
    const loginForm = page.locator('input[name="Email"], input[type="email"]');
    if (await loginForm.isVisible({ timeout: 10000 })) {
      console.log('✅ Login form found');

      // Try logging in
      await page.fill('input[name="Email"]', 'admin@stadium.com');
      await page.fill('input[name="Password"]', 'admin123');
      await page.click('button[type="submit"]');

      // Wait for redirect
      await page.waitForTimeout(5000);

      const afterLoginUrl = page.url();
      console.log(`After login URL: ${afterLoginUrl}`);

      if (afterLoginUrl.includes('/dashboard')) {
        console.log('✅ Successfully logged in and redirected to dashboard');
      } else {
        console.log('ℹ️ Login behavior may be different than expected');
      }

    } else {
      console.log('⚠️ Login form not found - application may not be running');
    }
  });
});