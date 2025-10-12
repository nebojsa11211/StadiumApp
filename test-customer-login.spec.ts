import { test, expect } from '@playwright/test';

test('Customer login test with customer@stadium.com', async ({ page }) => {
  // Navigate to customer login page
  await page.goto('https://localhost:7020/login');

  // Wait for page to load
  await page.waitForLoadState('networkidle');

  // Take screenshot of login page
  await page.screenshot({ path: 'customer-login-page.png' });

  // Fill in credentials
  await page.fill('input[type="email"]', 'customer@stadium.com');
  await page.fill('input[type="password"]', 'customer123');

  // Take screenshot before clicking login
  await page.screenshot({ path: 'customer-before-login.png' });

  // Click login button
  await page.click('button[type="submit"]');

  // Wait for navigation or response
  await page.waitForTimeout(3000);

  // Take screenshot after login attempt
  await page.screenshot({ path: 'customer-after-login.png' });

  // Check current URL
  const currentUrl = page.url();
  console.log('Current URL after login:', currentUrl);

  // Check for any error messages
  const errorElement = await page.locator('.alert-danger, .text-danger, [class*="error"]').first();
  const hasError = await errorElement.isVisible().catch(() => false);

  if (hasError) {
    const errorText = await errorElement.textContent();
    console.log('Error message:', errorText);
  }

  // Check if we're still on login page or redirected
  if (currentUrl.includes('/login')) {
    console.log('Still on login page - login failed');
  } else {
    console.log('Redirected away from login - login may have succeeded');
  }
});
