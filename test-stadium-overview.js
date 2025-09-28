const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors']
  });
  const context = await browser.newContext({
    ignoreHTTPSErrors: true
  });
  const page = await context.newPage();

  try {
    // Navigate to admin app
    console.log('Navigating to admin app...');
    await page.goto('https://localhost:7030');
    await page.waitForLoadState('networkidle');

    // Check if we're on login page or already authenticated
    const isLoginVisible = await page.locator('input[type="email"]').isVisible({ timeout: 3000 }).catch(() => false);
    if (page.url().includes('/login') || isLoginVisible) {
      console.log('Logging in...');

      // Wait for page to be fully loaded
      await page.waitForTimeout(1000);

      // Fill login credentials
      await page.fill('input[type="email"]', 'admin@stadium.com');
      await page.fill('input[type="password"]', 'admin123');

      // Click login button
      const loginButton = page.locator('button:has-text("Login"), input[type="submit"]');
      await loginButton.click();

      // Wait for redirect after login
      try {
        await page.waitForURL('**/admin/**', { timeout: 10000 });
      } catch (e) {
        console.log('Login redirect failed, checking page content...');
        await page.screenshot({ path: '.playwright-mcp/login-attempt.png' });
        console.log('Current URL:', page.url());
      }
      await page.waitForLoadState('networkidle');
    }

    // Navigate to Stadium Overview page
    console.log('Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');

    // Wait a bit for any dynamic content to load
    await page.waitForTimeout(2000);

    // Take screenshot
    console.log('Taking screenshot...');
    await page.screenshot({
      path: '.playwright-mcp/stadium-overview-field-centered.png',
      fullPage: true
    });

    console.log('Screenshot saved to: .playwright-mcp/stadium-overview-field-centered.png');

  } catch (error) {
    console.error('Error:', error);
  } finally {
    await browser.close();
  }
})();