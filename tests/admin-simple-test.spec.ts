import { test, expect } from '@playwright/test';

test.describe('Admin Application Basic Test', () => {
  test('Check admin application loads and find debug panel', async ({ page, context }) => {
    // Set longer timeout for navigation
    page.setDefaultTimeout(60000);

    console.log('🚀 Starting admin application test...');

    // Navigate to admin application
    console.log('📍 Navigating to https://localhost:9030');
    await page.goto('https://localhost:9030', { waitUntil: 'domcontentloaded' });

    // Wait a bit for Blazor to initialize
    await page.waitForTimeout(5000);

    // Take screenshot of what we see
    await page.screenshot({ path: 'admin-initial-page.png', fullPage: true });
    console.log('📸 Screenshot taken: admin-initial-page.png');

    // Get page title
    const title = await page.title();
    console.log(`📄 Page title: "${title}"`);

    // Get page URL
    const url = page.url();
    console.log(`🌐 Current URL: ${url}`);

    // Check what's visible on the page
    const bodyText = await page.locator('body').innerText();
    console.log('📝 Page content preview:', bodyText.substring(0, 500));

    // Check for common login elements
    const emailInput = await page.locator('input[type="email"]').count();
    const passwordInput = await page.locator('input[type="password"]').count();
    const loginButton = await page.locator('button[type="submit"]').count();

    console.log(`🔍 Login elements found:`);
    console.log(`   Email inputs: ${emailInput}`);
    console.log(`   Password inputs: ${passwordInput}`);
    console.log(`   Submit buttons: ${loginButton}`);

    // Check for loading indicators
    const spinners = await page.locator('.spinner-border').count();
    const loadingText = await page.locator('text=Loading').count();

    console.log(`⏳ Loading indicators:`);
    console.log(`   Spinners: ${spinners}`);
    console.log(`   Loading text: ${loadingText}`);

    // If we see loading indicators, wait for them to disappear
    if (spinners > 0) {
      console.log('⏳ Waiting for spinner to disappear...');
      await page.waitForSelector('.spinner-border', { state: 'detached', timeout: 30000 });
      console.log('✅ Spinner disappeared');

      // Take another screenshot after loading
      await page.screenshot({ path: 'admin-after-loading.png', fullPage: true });
      console.log('📸 Screenshot taken: admin-after-loading.png');
    }

    // Try to find login form again
    const emailInputAfter = await page.locator('input[type="email"]').count();
    if (emailInputAfter > 0) {
      console.log('✅ Login form is now visible, attempting login...');

      await page.fill('input[type="email"]', 'admin@stadium.com');
      await page.fill('input[type="password"]', 'admin123');
      await page.click('button[type="submit"]');

      // Wait for navigation
      await page.waitForTimeout(5000);

      // Take screenshot after login attempt
      await page.screenshot({ path: 'admin-after-login.png', fullPage: true });
      console.log('📸 Screenshot taken: admin-after-login.png');

      // Look for the modal debug panel
      const debugPanel = await page.locator('text=🔧 Modal Debug Panel').count();
      console.log(`🔧 Modal Debug Panel found: ${debugPanel > 0}`);

      if (debugPanel > 0) {
        console.log('🎉 SUCCESS: Debug panel is visible!');

        // Try clicking the debug buttons
        const buttons = [
          'Test Blazor Click',
          'Test JS Direct',
          'Test Bootstrap',
          'Test jQuery'
        ];

        for (const buttonText of buttons) {
          const button = page.locator(`button:has-text("${buttonText}")`);
          const buttonCount = await button.count();
          console.log(`🔘 Button "${buttonText}": ${buttonCount > 0 ? 'Found' : 'Not found'}`);

          if (buttonCount > 0) {
            console.log(`🖱️ Clicking "${buttonText}"...`);
            await button.click();
            await page.waitForTimeout(1000);
          }
        }
      }

    } else {
      console.log('❌ Login form not found after loading');
    }

    // Final screenshot
    await page.screenshot({ path: 'admin-final-state.png', fullPage: true });
    console.log('📸 Final screenshot taken: admin-final-state.png');
  });
});