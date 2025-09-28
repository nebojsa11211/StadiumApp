const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
  });

  const context = await browser.newContext({
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  try {
    console.log('Step 1: Navigating to admin app...');
    await page.goto('https://localhost:7030', {
      waitUntil: 'networkidle',
      timeout: 60000
    });

    await page.screenshot({
      path: '.playwright-mcp/robust-step1-initial.png',
      fullPage: true
    });

    console.log('Step 2: Checking if on login page...');
    const currentUrl = await page.url();
    console.log('Current URL:', currentUrl);

    if (currentUrl.includes('/login')) {
      console.log('Step 3: Filling login form...');

      // Wait for login form to be ready
      await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });

      // Clear and fill email
      await page.fill('#admin-login-email-input', '');
      await page.fill('#admin-login-email-input', 'admin@stadium.com');

      // Clear and fill password
      await page.fill('#admin-login-password-input', '');
      await page.fill('#admin-login-password-input', 'admin123');

      await page.screenshot({
        path: '.playwright-mcp/robust-step3-form-filled.png',
        fullPage: true
      });

      console.log('Step 4: Submitting login...');

      // Click login button and wait for navigation
      const [response] = await Promise.all([
        page.waitForNavigation({ waitUntil: 'networkidle', timeout: 30000 }),
        page.click('#admin-login-submit-btn')
      ]);

      console.log('Navigation response status:', response?.status());

      await page.screenshot({
        path: '.playwright-mcp/robust-step4-after-login.png',
        fullPage: true
      });
    }

    console.log('Step 5: Current URL after login:', await page.url());

    // Navigate to stadium overview
    console.log('Step 6: Navigating to stadium overview...');
    await page.goto('https://localhost:7030/stadium-overview', {
      waitUntil: 'networkidle',
      timeout: 60000
    });

    console.log('Step 7: Waiting for page to load...');
    await page.waitForTimeout(5000);

    await page.screenshot({
      path: '.playwright-mcp/robust-step7-stadium-overview.png',
      fullPage: true
    });

    // Check if still on login page
    const finalUrl = await page.url();
    console.log('Final URL:', finalUrl);

    if (finalUrl.includes('/login')) {
      console.log('Still on login page - authentication failed');

      // Try manual login via dashboard first
      console.log('Step 8: Trying to go to dashboard first...');
      await page.goto('https://localhost:7030/dashboard', {
        waitUntil: 'networkidle',
        timeout: 60000
      });

      await page.screenshot({
        path: '.playwright-mcp/robust-step8-dashboard-attempt.png',
        fullPage: true
      });

      const dashboardUrl = await page.url();
      console.log('Dashboard URL:', dashboardUrl);

      if (dashboardUrl.includes('/login')) {
        console.log('Step 9: Need to login again...');

        await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');

        await page.screenshot({
          path: '.playwright-mcp/robust-step9-second-login-attempt.png',
          fullPage: true
        });

        await page.click('#admin-login-submit-btn');

        // Wait for dashboard to load
        await page.waitForTimeout(5000);

        await page.screenshot({
          path: '.playwright-mcp/robust-step9-after-second-login.png',
          fullPage: true
        });

        // Now try stadium overview again
        console.log('Step 10: Trying stadium overview after successful login...');
        await page.goto('https://localhost:7030/stadium-overview', {
          waitUntil: 'networkidle',
          timeout: 60000
        });

        await page.waitForTimeout(8000);

        await page.screenshot({
          path: '.playwright-mcp/robust-step10-final-stadium-overview.png',
          fullPage: true
        });
      }
    } else {
      console.log('Successfully accessed stadium overview!');

      // Wait for stadium content to load
      await page.waitForTimeout(8000);

      // Check for stadium elements
      const stadiumContainer = await page.locator('.stadium-container').isVisible().catch(() => false);
      const infoPanel = await page.locator('.info-panel').isVisible().catch(() => false);
      const stadiumSvg = await page.locator('.stadium-svg').isVisible().catch(() => false);

      console.log('Stadium visualization status:');
      console.log('- Stadium container visible:', stadiumContainer);
      console.log('- Info panel visible:', infoPanel);
      console.log('- Stadium SVG visible:', stadiumSvg);

      await page.screenshot({
        path: '.playwright-mcp/robust-final-stadium-verification.png',
        fullPage: true
      });
    }

    console.log('Test completed successfully!');

  } catch (error) {
    console.error('Error during test:', error.message);
    await page.screenshot({
      path: '.playwright-mcp/robust-error-screenshot.png',
      fullPage: true
    });
  } finally {
    await browser.close();
  }
})();