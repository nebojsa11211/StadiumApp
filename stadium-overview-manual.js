const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors'],
    slowMo: 500  // Slow down actions for visibility
  });

  const context = await browser.newContext({
    ignoreHTTPSErrors: true,
    viewport: { width: 1920, height: 1080 }
  });

  const page = await context.newPage();

  try {
    console.log('Opening admin app...');
    await page.goto('https://localhost:7030');

    // Wait for the page to load completely
    await page.waitForLoadState('networkidle');

    console.log('Current URL:', page.url());

    // If on login page, perform login
    if (page.url().includes('/login') || await page.locator('input[type="email"]').isVisible({ timeout: 5000 })) {
      console.log('On login page, attempting to login...');

      // Fill and submit login form
      await page.fill('input[type="email"]', 'admin@stadium.com');
      await page.fill('input[type="password"]', 'admin123');

      // Click login button and wait for navigation
      await Promise.all([
        page.waitForNavigation({ timeout: 15000 }),
        page.click('button[type="submit"]')
      ]);

      console.log('Login completed, current URL:', page.url());
    }

    // Navigate to Stadium Overview
    console.log('Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');

    // Wait for the stadium layout to load
    await page.waitForSelector('#admin-stadium-container', { timeout: 10000 });
    await page.waitForTimeout(3000); // Additional wait for dynamic content

    console.log('Taking full page screenshot...');
    await page.screenshot({
      path: '.playwright-mcp/stadium-field-centered-final.png',
      fullPage: true
    });

    console.log('✅ Screenshot saved successfully!');

    // Keep browser open for manual inspection
    console.log('Browser will stay open for 30 seconds for manual inspection...');
    await page.waitForTimeout(30000);

  } catch (error) {
    console.error('❌ Error:', error.message);

    // Take error screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-error.png',
      fullPage: true
    });

    console.log('Error screenshot saved');
  } finally {
    await browser.close();
  }
})();