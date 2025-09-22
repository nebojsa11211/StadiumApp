import { test, expect } from '@playwright/test';

test.describe('Admin Orders Navigation Test', () => {
  test('should successfully navigate to orders page after login', async ({ page }) => {
    console.log('Starting admin authentication and orders navigation test...');

    // Step 1: Navigate to admin application
    console.log('Step 1: Navigating to admin application...');
    await page.goto('https://localhost:9030', { waitUntil: 'networkidle' });

    // Take screenshot of initial page
    await page.screenshot({
      path: '.playwright-mcp/admin-initial-page.png',
      fullPage: true
    });

    // Verify we're on the login page
    await expect(page.locator('h2')).toContainText('Admin Login');
    console.log('✅ Successfully reached admin login page');

    // Step 2: Log in with admin credentials
    console.log('Step 2: Logging in with admin credentials...');

    // Fill in login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    // Take screenshot before login
    await page.screenshot({
      path: '.playwright-mcp/admin-before-login.png',
      fullPage: true
    });

    // Click login button
    await page.click('#admin-login-submit-btn');

    // Step 3: Wait for successful login and page to fully load
    console.log('Step 3: Waiting for successful login and page load...');

    // Wait for navigation away from login page
    await page.waitForURL(url => !url.includes('/login'), { timeout: 10000 });

    // Wait for the main layout to load
    await page.waitForSelector('#admin-layout-main', { timeout: 10000 });

    // Additional wait to ensure all authentication state is settled
    await page.waitForTimeout(2000);

    // Take screenshot after successful login
    await page.screenshot({
      path: '.playwright-mcp/admin-after-login.png',
      fullPage: true
    });

    // Verify we're logged in by checking for admin layout elements
    await expect(page.locator('#admin-layout-main')).toBeVisible();
    console.log('✅ Successfully logged in to admin application');

    // Step 4: Click on Orders navigation link
    console.log('Step 4: Clicking on Orders navigation link...');

    // Wait for navigation menu to be ready
    await page.waitForSelector('#admin-nav-orders-link', { timeout: 5000 });

    // Take screenshot before clicking orders link
    await page.screenshot({
      path: '.playwright-mcp/admin-before-orders-click.png',
      fullPage: true
    });

    // Click the orders navigation link
    await page.click('#admin-nav-orders-link');

    // Step 5: Verify orders page loads correctly
    console.log('Step 5: Verifying orders page loads correctly...');

    // Wait for navigation to orders page
    await page.waitForURL(url => url.includes('/orders'), { timeout: 10000 });

    // Wait for orders page content to load
    await page.waitForSelector('h1, h2, .orders-container, [data-testid="orders-page"]', { timeout: 10000 });

    // Additional wait to ensure page is fully rendered
    await page.waitForTimeout(2000);

    // Take screenshot of orders page
    await page.screenshot({
      path: '.playwright-mcp/admin-orders-page-loaded.png',
      fullPage: true
    });

    // Verify we're on the orders page and not redirected back to login
    const currentUrl = page.url();
    expect(currentUrl).toContain('/orders');
    expect(currentUrl).not.toContain('/login');

    // Check for orders page content (title or main container)
    const hasOrdersTitle = await page.locator('h1, h2').filter({ hasText: /orders/i }).count() > 0;
    const hasOrdersContainer = await page.locator('.orders-container, [data-testid="orders-page"], main').count() > 0;

    expect(hasOrdersTitle || hasOrdersContainer).toBeTruthy();

    console.log('✅ Successfully navigated to orders page without redirect to login');
    console.log(`Final URL: ${currentUrl}`);

    // Additional verification: Check that we're still authenticated
    const isLoginPage = currentUrl.includes('/login');
    expect(isLoginPage).toBeFalsy();

    // Log final test results
    console.log('=== TEST RESULTS ===');
    console.log('✅ Admin application accessible');
    console.log('✅ Login successful with admin credentials');
    console.log('✅ Orders navigation link functional');
    console.log('✅ Orders page loads without authentication redirect');
    console.log('✅ User remains authenticated throughout navigation');
    console.log('=== TEST COMPLETED SUCCESSFULLY ===');
  });

  test('should maintain authentication state during orders navigation', async ({ page }) => {
    console.log('Testing authentication persistence during navigation...');

    // Login first
    await page.goto('https://localhost:9030');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for login to complete
    await page.waitForURL(url => !url.includes('/login'));
    await page.waitForTimeout(1000);

    // Navigate to orders multiple times to test persistence
    for (let i = 1; i <= 3; i++) {
      console.log(`Navigation attempt ${i}...`);

      await page.click('#admin-nav-orders-link');
      await page.waitForTimeout(1000);

      // Verify still on orders page and not redirected to login
      const url = page.url();
      expect(url).toContain('/orders');
      expect(url).not.toContain('/login');

      // Take screenshot for each navigation
      await page.screenshot({
        path: `.playwright-mcp/admin-orders-navigation-${i}.png`,
        fullPage: true
      });
    }

    console.log('✅ Authentication persists through multiple navigation attempts');
  });
});