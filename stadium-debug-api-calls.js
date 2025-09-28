const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors', '--disable-web-security']
  });

  const context = await browser.newContext({
    viewport: { width: 1920, height: 1080 },
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  // Capture console logs and network requests
  page.on('console', msg => {
    console.log(`🖥️ Console [${msg.type()}]:`, msg.text());
  });

  page.on('requestfailed', request => {
    console.log(`❌ Request failed: ${request.method()} ${request.url()} - ${request.failure().errorText}`);
  });

  page.on('response', response => {
    const url = response.url();
    if (url.includes('stadium') || url.includes('api')) {
      console.log(`📡 API Response: ${response.status()} ${response.method || 'GET'} ${url}`);
    }
  });

  try {
    console.log('🚀 Debugging stadium API calls and data loading...');

    // Navigate to admin login
    await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForNavigation({ waitUntil: 'networkidle' });

    // Navigate to stadium overview
    console.log('🏟️ Navigating to stadium overview and monitoring API calls...');
    await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });

    // Wait and capture any API calls or errors
    await page.waitForTimeout(10000);

    // Check developer tools console errors
    const consoleErrors = await page.evaluate(() => {
      // Return any JavaScript errors that might be in the console
      return window.stadiumErrors || [];
    });

    console.log('📋 Console errors:', consoleErrors);

    // Check if we can manually call the API
    const apiTest = await page.evaluate(async () => {
      try {
        const response = await fetch('/api/stadium-viewer/overview');
        const text = await response.text();
        return {
          status: response.status,
          ok: response.ok,
          text: text.substring(0, 500)
        };
      } catch (error) {
        return {
          error: error.message
        };
      }
    });

    console.log('🔍 Manual API test result:', apiTest);

    // Try to force refresh or generate demo data
    console.log('🔧 Attempting to force demo data generation...');

    // First try to wait for any buttons to appear
    await page.waitForTimeout(5000);

    // Look for error/empty state buttons
    const buttonSelectors = [
      '#admin-stadium-demo-btn',
      '#admin-stadium-generate-btn',
      '#admin-stadium-retry-btn',
      '#admin-stadium-load-btn',
      'button[onclick*="demo"]',
      'button:has-text("Generate")',
      'button:has-text("Demo")',
      'button:has-text("Retry")'
    ];

    let buttonClicked = false;
    for (const selector of buttonSelectors) {
      try {
        const element = await page.$(selector);
        if (element) {
          const isVisible = await element.isVisible();
          const isEnabled = await element.isEnabled();
          console.log(`Found button ${selector}: visible=${isVisible}, enabled=${isEnabled}`);

          if (isVisible && isEnabled) {
            console.log(`🔘 Clicking button: ${selector}`);
            await element.click();
            buttonClicked = true;
            await page.waitForTimeout(5000);
            break;
          }
        }
      } catch (e) {
        // Continue to next selector
      }
    }

    if (!buttonClicked) {
      console.log('⚠️ No demo/retry buttons found or clickable');
    }

    // Take final screenshot
    await page.screenshot({
      path: 'stadium-debug-api-final.png',
      fullPage: true
    });

    // Check final state
    const finalState = await page.evaluate(() => {
      return {
        url: window.location.href,
        isLoading: !!document.querySelector('#admin-stadium-loading-state'),
        hasError: !!document.querySelector('#admin-stadium-error-state'),
        hasEmpty: !!document.querySelector('#admin-stadium-empty-state'),
        hasGrid: !!document.querySelector('#admin-stadium-grid-layout'),
        loadingText: document.querySelector('#admin-stadium-loading-state')?.textContent || '',
        errorText: document.querySelector('#admin-stadium-error-state')?.textContent || '',
        emptyText: document.querySelector('#admin-stadium-empty-state')?.textContent || '',
        visibleButtons: Array.from(document.querySelectorAll('button')).filter(b => b.offsetParent !== null).map(b => b.textContent.trim())
      };
    });

    console.log('📊 Final state analysis:');
    console.log(`- Loading: ${finalState.isLoading}`);
    console.log(`- Error: ${finalState.hasError}`);
    console.log(`- Empty: ${finalState.hasEmpty}`);
    console.log(`- Grid: ${finalState.hasGrid}`);
    console.log(`- Loading text: ${finalState.loadingText.substring(0, 100)}`);
    console.log(`- Error text: ${finalState.errorText.substring(0, 100)}`);
    console.log(`- Empty text: ${finalState.emptyText.substring(0, 100)}`);
    console.log(`- Visible buttons: ${finalState.visibleButtons.join(', ')}`);

  } catch (error) {
    console.error('❌ Debug test failed:', error.message);
    await page.screenshot({ path: 'stadium-debug-error.png' });
  }

  await browser.close();
})();