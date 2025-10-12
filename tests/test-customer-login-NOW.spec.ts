import { test, expect } from '@playwright/test';

test('Customer login test - LIVE TEST', async ({ page }) => {
  console.log('🔍 Starting customer login test...');

  // Navigate to customer login page on port 8081
  console.log('📍 Navigating to https://localhost:8081/login');
  await page.goto('https://localhost:8081/login', {
    waitUntil: 'networkidle',
    timeout: 30000
  });

  // Take screenshot of login page
  await page.screenshot({ path: 'test-login-page.png', fullPage: true });
  console.log('📸 Screenshot saved: test-login-page.png');

  // Fill in credentials
  console.log('✍️ Filling in customer@stadium.com credentials...');
  await page.fill('input[type="email"]', 'customer@stadium.com');
  await page.fill('input[type="password"]', 'customer123');

  // Take screenshot before clicking login
  await page.screenshot({ path: 'test-before-login.png', fullPage: true });
  console.log('📸 Screenshot saved: test-before-login.png');

  // Click login button
  console.log('🖱️ Clicking login button...');
  await page.click('button[type="submit"]');

  // Wait for navigation or response
  await page.waitForTimeout(5000);

  // Take screenshot after login attempt
  await page.screenshot({ path: 'test-after-login.png', fullPage: true });
  console.log('📸 Screenshot saved: test-after-login.png');

  // Check current URL
  const currentUrl = page.url();
  console.log('🌐 Current URL after login:', currentUrl);

  // Check for any error messages
  const errorElement = await page.locator('.alert-danger, .text-danger, [class*="error"], .invalid-feedback').first();
  const hasError = await errorElement.isVisible().catch(() => false);

  if (hasError) {
    const errorText = await errorElement.textContent();
    console.log('❌ Error message found:', errorText);
  } else {
    console.log('✅ No error messages visible');
  }

  // Check if we're still on login page or redirected
  if (currentUrl.includes('/login')) {
    console.log('⚠️ Still on login page - login FAILED');

    // Get page content for debugging
    const bodyText = await page.locator('body').textContent();
    console.log('📄 Page contains:', bodyText?.substring(0, 500));

    throw new Error('Login failed - still on login page');
  } else {
    console.log('✅ Redirected away from login - login SUCCEEDED!');
    console.log('🎉 New URL:', currentUrl);
  }
});
