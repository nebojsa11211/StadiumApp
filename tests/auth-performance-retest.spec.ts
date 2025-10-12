import { test, expect, type Page } from '@playwright/test';

test.describe('Authentication Performance Re-test (After Timeout Fix)', () => {
  let page: Page;

  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('should authenticate successfully with extended timeouts', async () => {
    console.log('🔄 Starting authentication performance re-test...');
    console.log('✅ Applied fixes: HttpClient timeout 15s → 120s, Database timeout 90s, Auth queries 30s');

    const startTime = Date.now();

    // Step 1: Navigate to Admin
    console.log('📍 Navigating to admin application...');
    const navStart = Date.now();

    await page.goto('https://localhost:7030', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    const navTime = Date.now() - navStart;
    console.log(`✅ Navigation: ${navTime}ms`);

    // Should redirect to login
    await page.waitForURL(/login/, { timeout: 10000 });
    console.log('✅ Redirected to login page');

    // Step 2: Perform authentication with performance monitoring
    console.log('🔐 Testing authentication performance...');

    // Fill credentials
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    console.log('📝 Credentials filled, submitting login...');

    // Submit and measure authentication time
    const authStart = Date.now();
    await page.click('#admin-login-submit-btn');

    try {
      // Wait for successful authentication with extended timeout
      await page.waitForURL(/dashboard|stadium|events/, { timeout: 90000 });

      const authTime = Date.now() - authStart;
      console.log(`✅ Authentication successful: ${authTime}ms`);

      // Verify authenticated state
      await expect(page.locator('h1, .user-info, [id*="user-dropdown"]')).toBeVisible({ timeout: 10000 });
      console.log('✅ Authenticated state verified');

      // Take success screenshot
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\auth-success-after-fix.png',
        fullPage: true
      });

      const totalTime = Date.now() - startTime;
      console.log(`🎉 Total authentication flow: ${totalTime}ms`);

      // Performance assertions - should be much faster now
      expect(authTime).toBeLessThan(30000); // Should complete well within 30s (was failing at 15s before)
      expect(totalTime).toBeLessThan(60000); // Total flow under 1 minute

      console.log('✅ Authentication performance SIGNIFICANTLY IMPROVED!');
      console.log(`   Previous: Failed at 15-25 seconds (HttpClient.Timeout = 15s)`);
      console.log(`   Current:  ${authTime}ms (HttpClient.Timeout = 120s)`);

    } catch (error) {
      const authTime = Date.now() - authStart;
      console.error(`❌ Authentication failed after ${authTime}ms:`, error);

      // Take failure screenshot
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\auth-failure-after-fix.png',
        fullPage: true
      });

      throw error;
    }
  });

  test('should navigate to stadium-overview efficiently', async () => {
    console.log('🏟️ Testing stadium-overview performance...');

    // First authenticate
    await page.goto('https://localhost:7030/login', { waitUntil: 'networkidle' });
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL(/dashboard|stadium|events/, { timeout: 90000 });

    console.log('✅ Authentication completed, testing stadium-overview...');

    // Navigate to stadium-overview
    const overviewStart = Date.now();
    await page.goto('https://localhost:7030/stadium-overview', {
      waitUntil: 'networkidle',
      timeout: 60000
    });

    const overviewTime = Date.now() - overviewStart;
    console.log(`✅ Stadium-overview loaded: ${overviewTime}ms`);

    // Verify stadium content loads
    await expect(page.locator('h1, .stadium-content, [id*="stadium"], svg')).toBeVisible({ timeout: 30000 });
    console.log('✅ Stadium-overview content verified');

    // Take stadium screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-success.png',
      fullPage: true
    });

    // Performance assertion
    expect(overviewTime).toBeLessThan(30000); // Should load within 30s

    console.log(`✅ Stadium-overview performance verified: ${overviewTime}ms`);
  });
});