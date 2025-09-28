const { chromium } = require('playwright');

(async () => {
  console.log('🚀 Starting simple admin test...');

  const browser = await chromium.launch({
    headless: false, // Keep visible so we can see what's happening
    slowMo: 1000, // Slow down interactions
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors']
  });

  const context = await browser.newContext({
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  try {
    console.log('📋 Step 1: Going to admin login page');
    await page.goto('https://localhost:7030/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    await page.screenshot({
      path: '.playwright-mcp/simple-step1-login-page.png',
      fullPage: true
    });

    console.log('📋 Step 2: Waiting for login form');
    await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });

    console.log('📋 Step 3: Filling login credentials');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    await page.screenshot({
      path: '.playwright-mcp/simple-step3-filled-form.png',
      fullPage: true
    });

    console.log('📋 Step 4: Clicking login button');
    await page.click('#admin-login-submit-btn');

    // Wait for response or URL change
    console.log('📋 Step 5: Waiting for login response...');
    await page.waitForTimeout(5000);

    await page.screenshot({
      path: '.playwright-mcp/simple-step5-after-login-click.png',
      fullPage: true
    });

    const currentUrl = await page.url();
    console.log('Current URL after login attempt:', currentUrl);

    if (currentUrl.includes('dashboard')) {
      console.log('✅ Login successful! Proceeding to stadium overview...');

      console.log('📋 Step 6: Going to stadium overview');
      await page.goto('https://localhost:7030/stadium-overview', {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      await page.waitForTimeout(3000);

      await page.screenshot({
        path: '.playwright-mcp/simple-step6-stadium-overview.png',
        fullPage: true
      });

      // Check for stadium elements
      const stadiumContainer = await page.locator('.stadium-container').isVisible().catch(() => false);
      const infoPanel = await page.locator('.info-panel').isVisible().catch(() => false);

      console.log('🏟️ Stadium container visible:', stadiumContainer);
      console.log('📊 Info panel visible:', infoPanel);

      if (stadiumContainer || infoPanel) {
        console.log('✅ Stadium visualization elements found!');
      } else {
        console.log('❌ Stadium visualization elements not found');

        // Check page content
        const pageText = await page.textContent('body');
        console.log('Page content preview:', pageText.substring(0, 200) + '...');
      }

      // Take final screenshot
      await page.screenshot({
        path: '.playwright-mcp/simple-final-stadium-page.png',
        fullPage: true
      });

    } else {
      console.log('❌ Login failed, still on login page');

      // Check for error messages
      const errorElements = await page.locator('.alert-danger, .text-danger').count();
      console.log('Error elements found:', errorElements);

      if (errorElements > 0) {
        const errorText = await page.locator('.alert-danger, .text-danger').first().textContent();
        console.log('Error message:', errorText);
      }
    }

    console.log('🎉 Test completed!');

    // Keep browser open for 30 seconds to allow manual inspection
    console.log('🔍 Keeping browser open for 30 seconds for manual inspection...');
    await page.waitForTimeout(30000);

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({
      path: '.playwright-mcp/simple-error.png',
      fullPage: true
    });
  } finally {
    await browser.close();
    console.log('🏁 Browser closed');
  }
})();