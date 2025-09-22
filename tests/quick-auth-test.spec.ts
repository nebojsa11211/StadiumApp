import { test, expect } from '@playwright/test';

test.describe('Quick Authentication Test', () => {
  const customerEmail = 'customer@stadium.com';
  const customerPassword = 'customer123';
  const baseUrl = 'https://localhost:7025';

  test('Basic Login Test', async ({ page }) => {
    // Ignore HTTPS errors for self-signed certificates
    await page.goto(baseUrl, { waitUntil: 'networkidle', timeout: 30000 });

    console.log('✓ Page loaded successfully');

    // Check title
    await expect(page).toHaveTitle(/Stadium/);
    console.log('✓ Page title confirmed');

    // Look for sign-in button
    const signInButton = page.locator('#customer-layout-sign-in-btn');
    await expect(signInButton).toBeVisible({ timeout: 10000 });
    console.log('✓ Sign-in button found');

    // Click sign-in
    await signInButton.click();
    await expect(page).toHaveURL(/login/, { timeout: 10000 });
    console.log('✓ Navigated to login page');

    // Fill login form
    await page.fill('#customer-login-email-input', customerEmail);
    await page.fill('#customer-login-password-input', customerPassword);
    console.log('✓ Login form filled');

    // Submit
    await page.click('#customer-login-submit-btn');
    console.log('✓ Login form submitted');

    // Wait for redirect or error
    await page.waitForTimeout(5000);

    const currentUrl = page.url();
    console.log('Current URL after login:', currentUrl);

    // Check if we're authenticated
    const userDropdown = page.locator('#customer-layout-user-dropdown');
    const isAuthenticated = await userDropdown.isVisible();
    console.log('Authentication status:', isAuthenticated ? '✓ Authenticated' : '❌ Not authenticated');

    // Check localStorage tokens
    const accessToken = await page.evaluate(() => localStorage.getItem('customer_access_token'));
    const refreshToken = await page.evaluate(() => localStorage.getItem('customer_refresh_token'));

    console.log('Access token exists:', accessToken ? 'YES' : 'NO');
    console.log('Refresh token exists:', refreshToken ? 'YES' : 'NO');

    if (accessToken) {
      console.log('Access token length:', accessToken.length);
    }
  });

  test('Protected Route Test', async ({ page }) => {
    // Direct navigation to orders page
    console.log('Testing direct navigation to /orders...');

    await page.goto(`${baseUrl}/orders`, { waitUntil: 'networkidle', timeout: 30000 });

    const finalUrl = page.url();
    console.log('Final URL after navigation to /orders:', finalUrl);

    if (finalUrl.includes('/login')) {
      console.log('✓ Correctly redirected to login (unauthenticated)');
    } else if (finalUrl.includes('/orders')) {
      console.log('❌ Stayed on orders page (should redirect if unauthenticated)');
    } else {
      console.log('? Unexpected URL:', finalUrl);
    }
  });
});