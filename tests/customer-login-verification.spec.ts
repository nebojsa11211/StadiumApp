import { test, expect, Page } from '@playwright/test';

test.describe('Customer Login Verification', () => {
  let page: Page;

  test.beforeAll(async ({ browser }) => {
    page = await browser.newPage();

    // Enable console logging to catch JavaScript errors
    page.on('console', msg => {
      console.log(`[BROWSER ${msg.type()}]:`, msg.text());
    });

    page.on('pageerror', error => {
      console.error('[PAGE ERROR]:', error);
    });

    page.on('requestfailed', request => {
      console.error('[REQUEST FAILED]:', request.url(), request.failure());
    });
  });

  test.afterAll(async () => {
    await page.close();
  });

  test('Customer login flow - complete verification', async () => {
    console.log('\n=== STEP 1: Navigate to login page ===');

    // Navigate to login page
    await page.goto('https://localhost:8081/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Take screenshot of login page
    await page.screenshot({
      path: 'customer-login-step-1-initial-page.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: customer-login-step-1-initial-page.png');

    // Verify we're on the login page
    await expect(page).toHaveURL(/.*login.*/, { timeout: 10000 });
    console.log('✓ Confirmed on login page');

    // Check for login form elements
    const emailInput = page.locator('input[type="email"], input[id*="email"], input[name*="email"]');
    const passwordInput = page.locator('input[type="password"], input[id*="password"], input[name*="password"]');
    const loginButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign In")');

    await expect(emailInput).toBeVisible({ timeout: 10000 });
    await expect(passwordInput).toBeVisible({ timeout: 10000 });
    await expect(loginButton).toBeVisible({ timeout: 10000 });
    console.log('✓ All login form elements are visible');

    console.log('\n=== STEP 2: Fill in credentials ===');

    // Fill in credentials
    await emailInput.fill('customer@stadium.com');
    console.log('✓ Email filled: customer@stadium.com');

    await passwordInput.fill('customer123');
    console.log('✓ Password filled: customer123');

    // Wait a moment to ensure form is ready
    await page.waitForTimeout(1000);

    // Take screenshot after filling form
    await page.screenshot({
      path: 'customer-login-step-2-form-filled.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: customer-login-step-2-form-filled.png');

    console.log('\n=== STEP 3: Submit login form ===');

    // Get current URL before submitting
    const urlBeforeLogin = page.url();
    console.log('URL before login:', urlBeforeLogin);

    // Click login button
    await loginButton.click();
    console.log('✓ Login button clicked');

    // Wait a moment for the request to be sent
    await page.waitForTimeout(2000);

    // Take screenshot immediately after clicking
    await page.screenshot({
      path: 'customer-login-step-3-after-submit.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: customer-login-step-3-after-submit.png');

    console.log('\n=== STEP 4: Verify login result ===');

    // Check for error messages
    const errorAlert = page.locator('.alert-danger, .error-message, [class*="error"]');
    const errorCount = await errorAlert.count();

    if (errorCount > 0) {
      const errorText = await errorAlert.first().textContent();
      console.error('❌ Error message found:', errorText);

      await page.screenshot({
        path: 'customer-login-step-4-error-state.png',
        fullPage: true
      });
      console.log('✓ Screenshot saved: customer-login-step-4-error-state.png');

      throw new Error(`Login failed with error: ${errorText}`);
    }

    console.log('✓ No error messages visible');

    // Wait for navigation or state change (max 10 seconds)
    let redirected = false;
    try {
      await page.waitForURL(url => !url.href.includes('/login'), { timeout: 10000 });
      redirected = true;
      console.log('✓ Successfully redirected away from login page');
    } catch (e) {
      console.warn('⚠ Still on login page after 10 seconds');
    }

    // Get current URL after login attempt
    const urlAfterLogin = page.url();
    console.log('URL after login:', urlAfterLogin);

    // Wait a bit more to ensure page has fully loaded
    await page.waitForTimeout(3000);

    // Take screenshot of final state
    await page.screenshot({
      path: 'customer-login-step-4-final-state.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: customer-login-step-4-final-state.png');

    console.log('\n=== STEP 5: Verify authenticated state ===');

    // Check for authentication indicators
    const authIndicators = [
      page.locator('text="Sign Out"'),
      page.locator('text="Logout"'),
      page.locator('text="My Profile"'),
      page.locator('text="My Orders"'),
      page.locator('[id*="user-dropdown"]'),
      page.locator('.dropdown:has-text("Profile")'),
      page.locator('a[href*="profile"]'),
      page.locator('a[href*="logout"]')
    ];

    let authIndicatorFound = false;
    for (const indicator of authIndicators) {
      const count = await indicator.count();
      if (count > 0) {
        const text = await indicator.first().textContent();
        console.log(`✓ Authentication indicator found: "${text}"`);
        authIndicatorFound = true;
        break;
      }
    }

    // Check localStorage for authentication tokens
    const hasToken = await page.evaluate(() => {
      const token = localStorage.getItem('token') ||
                    localStorage.getItem('authToken') ||
                    localStorage.getItem('customer_token');
      return !!token;
    });

    if (hasToken) {
      console.log('✓ Authentication token found in localStorage');
    } else {
      console.warn('⚠ No authentication token found in localStorage');
    }

    // Check if we're still on login page
    const stillOnLogin = page.url().includes('/login');

    console.log('\n=== FINAL VERIFICATION RESULTS ===');
    console.log('Redirected from login page:', redirected);
    console.log('Still on login page:', stillOnLogin);
    console.log('Authentication indicator visible:', authIndicatorFound);
    console.log('Token in localStorage:', hasToken);
    console.log('Final URL:', page.url());

    // Final assertions
    if (stillOnLogin && !authIndicatorFound && !hasToken) {
      throw new Error('LOGIN FAILED: Still on login page with no authentication indicators');
    }

    if (redirected || authIndicatorFound || hasToken) {
      console.log('\n✅ LOGIN SUCCESSFUL - User is authenticated');
    } else {
      throw new Error('LOGIN STATUS UNCLEAR - Please review screenshots');
    }
  });
});
