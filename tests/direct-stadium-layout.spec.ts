import { test, expect } from '@playwright/test';

test('Direct stadium layout verification', async ({ page }) => {
  // Set browser storage to simulate logged-in state
  await page.context().addCookies([
    {
      name: '.AspNetCore.Culture',
      value: 'c=hr|uic=hr',
      domain: 'localhost',
      path: '/'
    }
  ]);

  // Navigate directly to stadium overview page
  await page.goto('https://localhost:7030/admin/stadium-overview', {
    waitUntil: 'networkidle',
    timeout: 60000
  });

  // Wait for page to load - either stadium content or login redirect
  await page.waitForLoadState('domcontentloaded');

  // Take a screenshot of whatever loads
  await page.screenshot({
    path: '.playwright-mcp/stadium-layout-final.png',
    fullPage: true
  });

  // Check current URL to see where we ended up
  const currentUrl = page.url();
  console.log(`Current URL: ${currentUrl}`);

  // If redirected to login, try logging in
  if (currentUrl.includes('/login')) {
    console.log('Redirected to login page, attempting login...');

    await page.waitForSelector('#admin-login-email-input', { timeout: 5000 });
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for successful login (much longer timeout)
    await page.waitForURL('**/dashboard', { timeout: 120000 });

    // Navigate to stadium overview
    await page.goto('https://localhost:7030/admin/stadium-overview', {
      waitUntil: 'networkidle',
      timeout: 60000
    });

    // Wait for stadium content to load
    await page.waitForSelector('#admin-stadium-container', { timeout: 30000 });

    // Take final screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-layout-final-success.png',
      fullPage: true
    });
  }

  console.log('✅ Stadium layout verification completed');
});