const { chromium } = require('playwright');

(async () => {
  // Launch browser
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
  });

  const context = await browser.newContext({
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  try {
    console.log('Navigating to admin app...');

    // Navigate to admin app with extended timeout
    await page.goto('https://localhost:7030', {
      waitUntil: 'domcontentloaded',
      timeout: 60000
    });

    // Wait a bit for page to load
    await page.waitForTimeout(3000);

    // Take screenshot of initial page
    await page.screenshot({
      path: '.playwright-mcp/admin-initial-page.png',
      fullPage: true
    });
    console.log('Initial page screenshot taken');

    // Check if login form exists
    const loginForm = await page.locator('#admin-login-form').isVisible().catch(() => false);

    if (loginForm) {
      console.log('Login form found, logging in...');

      await page.fill('#admin-login-email-input', 'admin@stadium.com');
      await page.fill('#admin-login-password-input', 'admin123');

      // Take screenshot before login
      await page.screenshot({
        path: '.playwright-mcp/admin-before-login.png',
        fullPage: true
      });

      await page.click('#admin-login-submit-btn');

      // Wait for navigation or dashboard
      await page.waitForTimeout(5000);

      // Take screenshot after login attempt
      await page.screenshot({
        path: '.playwright-mcp/admin-after-login.png',
        fullPage: true
      });
      console.log('Login attempt completed');
    }

    // Try to navigate to stadium overview directly
    console.log('Navigating to stadium overview...');
    await page.goto('https://localhost:7030/stadium-overview', {
      waitUntil: 'domcontentloaded',
      timeout: 60000
    });

    // Wait for page content to load
    await page.waitForTimeout(5000);

    // Take screenshot of stadium overview page
    await page.screenshot({
      path: '.playwright-mcp/stadium-overview-page.png',
      fullPage: true
    });
    console.log('Stadium overview screenshot taken');

    // Try to find stadium elements
    const stadiumContainer = await page.locator('.stadium-container').isVisible().catch(() => false);
    const infoPanel = await page.locator('.info-panel').isVisible().catch(() => false);
    const stadiumSvg = await page.locator('.stadium-svg').isVisible().catch(() => false);

    console.log('Stadium container visible:', stadiumContainer);
    console.log('Info panel visible:', infoPanel);
    console.log('Stadium SVG visible:', stadiumSvg);

    // Get page content to understand what's loaded
    const pageTitle = await page.title();
    const url = await page.url();
    console.log('Page title:', pageTitle);
    console.log('Current URL:', url);

    // Wait for any potential dynamic content
    await page.waitForTimeout(10000);

    // Take final screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-overview-final.png',
      fullPage: true
    });
    console.log('Final stadium overview screenshot taken');

    console.log('Manual test completed successfully');

  } catch (error) {
    console.error('Error during test:', error.message);
    await page.screenshot({
      path: '.playwright-mcp/error-screenshot.png',
      fullPage: true
    });
  } finally {
    await browser.close();
  }
})();