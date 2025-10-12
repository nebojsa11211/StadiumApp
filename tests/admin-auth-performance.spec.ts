import { test, expect, type Page } from '@playwright/test';

test.describe('Admin Authentication Performance Tests', () => {
  let page: Page;

  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('should complete authentication within optimized timeouts', async () => {
    console.log('🔄 Starting authentication performance test...');

    // Start timing the entire flow
    const startTime = Date.now();

    // Step 1: Navigate to admin application
    console.log('📍 Navigating to admin application...');
    const navigationStart = Date.now();

    try {
      await page.goto('https://localhost:7030', {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      const navigationTime = Date.now() - navigationStart;
      console.log(`✅ Navigation completed in ${navigationTime}ms`);

      // Should redirect to login if not authenticated
      await page.waitForURL(/login/, { timeout: 10000 });
      console.log('✅ Redirected to login page successfully');

    } catch (error) {
      console.error('❌ Navigation failed:', error);
      throw error;
    }

    // Step 2: Perform login with performance measurement
    console.log('🔐 Starting login process...');
    const loginStart = Date.now();

    try {
      // Fill in credentials
      await page.fill('#admin-login-email-input', 'admin@stadium.com');
      await page.fill('#admin-login-password-input', 'admin123');

      console.log('📝 Credentials filled, submitting form...');

      // Submit login form and measure response time
      const submitStart = Date.now();
      await page.click('#admin-login-submit-btn');

      // Wait for successful authentication (redirect to dashboard)
      await page.waitForURL(/dashboard|stadium/, { timeout: 120000 });

      const loginTime = Date.now() - loginStart;
      const submitTime = Date.now() - submitStart;
      console.log(`✅ Login completed in ${loginTime}ms (submit to redirect: ${submitTime}ms)`);

      // Verify we're authenticated by checking for user elements
      const userElement = await page.locator('[id*="user-dropdown"], [id*="logout"], .user-info').first();
      await expect(userElement).toBeVisible({ timeout: 10000 });
      console.log('✅ Authentication verified - user elements visible');

    } catch (error) {
      console.error('❌ Login failed:', error);
      // Take screenshot for debugging
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\login-failure.png',
        fullPage: true
      });
      throw error;
    }

    // Step 3: Navigate to stadium-overview
    console.log('🏟️ Navigating to stadium-overview...');
    const overviewStart = Date.now();

    try {
      await page.goto('https://localhost:7030/stadium-overview', {
        waitUntil: 'networkidle',
        timeout: 60000
      });

      const overviewTime = Date.now() - overviewStart;
      console.log(`✅ Stadium-overview loaded in ${overviewTime}ms`);

      // Wait for stadium content to load
      await expect(page.locator('h1, .stadium-content, [id*="stadium"]')).toBeVisible({ timeout: 30000 });
      console.log('✅ Stadium-overview content verified');

    } catch (error) {
      console.error('❌ Stadium-overview navigation failed:', error);
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-failure.png',
        fullPage: true
      });
      throw error;
    }

    // Take success screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-success.png',
      fullPage: true
    });

    const totalTime = Date.now() - startTime;
    console.log(`🎉 Complete authentication flow completed in ${totalTime}ms`);

    // Performance assertions based on optimized timeouts
    expect(navigationTime).toBeLessThan(30000); // Navigation should be under 30s
    expect(loginTime).toBeLessThan(30000); // Login should be under 30s (was timing out at 15-25s before)
    expect(overviewTime).toBeLessThan(60000); // Overview should be under 60s
    expect(totalTime).toBeLessThan(120000); // Total flow should be under 2 minutes

    console.log('✅ All performance requirements met!');
  });

  test('should handle database operations efficiently', async () => {
    console.log('🔄 Testing database operation performance...');

    await page.goto('https://localhost:7030/login', { waitUntil: 'networkidle' });

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    const dbOperationStart = Date.now();
    await page.click('#admin-login-submit-btn');

    // This will test the database query performance with 90s timeout
    await page.waitForURL(/dashboard|stadium/, { timeout: 90000 });

    const dbOperationTime = Date.now() - dbOperationStart;
    console.log(`✅ Database authentication completed in ${dbOperationTime}ms`);

    // Should be much faster than the previous 90s timeout limit
    expect(dbOperationTime).toBeLessThan(30000); // Should complete well under the 90s database timeout
  });
});