import { test, expect } from '@playwright/test';

test('Debug Customer UI Layout', async ({ page }) => {
  // Navigate to customer app
  await page.goto('https://localhost:8081', {
    waitUntil: 'networkidle',
    timeout: 30000
  });

  // Wait for page to load
  await page.waitForTimeout(2000);

  // Take full page screenshot
  await page.screenshot({
    path: 'customer-ui-debug.png',
    fullPage: true
  });

  console.log('Screenshot saved: customer-ui-debug.png');

  // Get layout info
  const bodyHTML = await page.evaluate(() => {
    const body = document.body;
    return {
      width: body.scrollWidth,
      height: body.scrollHeight,
      className: body.className
    };
  });

  console.log('Body info:', bodyHTML);

  // Check if key elements exist
  const hasTopBar = await page.locator('.stadium-top-bar').count();
  const hasNav = await page.locator('.stadium-nav').count();
  const hasNavLinks = await page.locator('.nav-links').count();

  console.log('Layout elements:');
  console.log('- Top bar:', hasTopBar > 0 ? 'YES' : 'NO');
  console.log('- Navigation:', hasNav > 0 ? 'YES' : 'NO');
  console.log('- Nav links:', hasNavLinks > 0 ? 'YES' : 'NO');

  // Get computed styles of main elements
  const styles = await page.evaluate(() => {
    const topBar = document.querySelector('.stadium-top-bar');
    const nav = document.querySelector('.stadium-nav');

    return {
      topBar: topBar ? window.getComputedStyle(topBar).cssText : 'NOT FOUND',
      nav: nav ? window.getComputedStyle(nav).cssText : 'NOT FOUND'
    };
  });

  console.log('Styles:', JSON.stringify(styles, null, 2));
});
