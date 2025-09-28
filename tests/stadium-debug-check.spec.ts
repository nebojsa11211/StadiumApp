import { test, expect } from '@playwright/test';

test.describe('Stadium Debug Check', () => {
  test('debug what is on the stadium overview page', async ({ page }) => {
    // Set viewport to exactly 1920x1080
    await page.setViewportSize({ width: 1920, height: 1080 });

    console.log('🔧 Starting stadium debug check...');

    // Navigate to admin login page
    await page.goto('https://localhost:9030/login');
    console.log('📍 Navigated to admin login page');

    // Wait for page to load
    await page.waitForLoadState('networkidle');

    // Take screenshot of login page
    await page.screenshot({
      path: '.playwright-mcp/debug-01-login-page.png',
      fullPage: true
    });

    // Fill login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    console.log('✅ Filled login credentials');

    // Submit login
    await page.click('#admin-login-submit-btn');
    await page.waitForLoadState('networkidle');
    console.log('🔐 Logged in successfully');

    // Take screenshot after login
    await page.screenshot({
      path: '.playwright-mcp/debug-02-after-login.png',
      fullPage: true
    });

    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');
    console.log('🏟️ Navigated to stadium overview page');

    // Wait a bit more for any dynamic content
    await page.waitForTimeout(3000);

    // Take screenshot of stadium overview page
    await page.screenshot({
      path: '.playwright-mcp/debug-03-stadium-overview.png',
      fullPage: true
    });

    // Get page title and URL
    const title = await page.title();
    const url = page.url();
    console.log(`📄 Page title: ${title}`);
    console.log(`🔗 Current URL: ${url}`);

    // Check what elements are actually present
    const elements = await page.evaluate(() => {
      const results: any = {};

      // Check for common stadium elements
      const selectors = [
        '#admin-stadium-grid-layout',
        '#admin-stadium-stand-n',
        '#admin-stadium-stand-s',
        '#admin-stadium-stand-e',
        '#admin-stadium-stand-w',
        '.stadium-container',
        '.stadium-grid',
        '[id*="stadium"]',
        '[class*="stadium"]'
      ];

      selectors.forEach(selector => {
        const element = document.querySelector(selector);
        results[selector] = element ? 'FOUND' : 'NOT FOUND';
      });

      // Get all elements with stadium in id or class
      const stadiumElements = Array.from(document.querySelectorAll('*')).filter(el =>
        el.id.includes('stadium') ||
        Array.from(el.classList).some(cls => cls.includes('stadium'))
      );

      results.stadiumElementsCount = stadiumElements.length;
      results.stadiumElements = stadiumElements.map(el => ({
        tagName: el.tagName,
        id: el.id,
        className: el.className,
        textContent: el.textContent?.substring(0, 50)
      }));

      // Check page content
      results.bodyText = document.body.textContent?.substring(0, 500);

      return results;
    });

    console.log('\n🔍 ELEMENT CHECK RESULTS:');
    Object.entries(elements).forEach(([key, value]) => {
      if (key !== 'stadiumElements' && key !== 'bodyText') {
        console.log(`${key}: ${value}`);
      }
    });

    console.log(`\n🏟️ Stadium elements found: ${elements.stadiumElementsCount}`);
    if (elements.stadiumElements && elements.stadiumElements.length > 0) {
      elements.stadiumElements.forEach((el: any, index: number) => {
        console.log(`  ${index + 1}. <${el.tagName}> id="${el.id}" class="${el.className}"`);
      });
    }

    console.log(`\n📝 Page content preview: ${elements.bodyText}`);

    // Check if there are any error messages
    const errorMessages = await page.evaluate(() => {
      const errors = Array.from(document.querySelectorAll('.alert-danger, .error, [class*="error"]'));
      return errors.map(el => el.textContent);
    });

    if (errorMessages.length > 0) {
      console.log('\n⚠️ Error messages found:');
      errorMessages.forEach((msg, index) => {
        console.log(`  ${index + 1}. ${msg}`);
      });
    }

    // Check network errors in console
    const logs: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        logs.push(`Console Error: ${msg.text()}`);
      }
    });

    // Reload page to capture any console errors
    await page.reload();
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);

    if (logs.length > 0) {
      console.log('\n🚨 Console errors:');
      logs.forEach((log, index) => {
        console.log(`  ${index + 1}. ${log}`);
      });
    }

    // Final screenshot
    await page.screenshot({
      path: '.playwright-mcp/debug-04-final-state.png',
      fullPage: true
    });

    console.log('\n✅ Debug check complete - screenshots saved');
  });
});