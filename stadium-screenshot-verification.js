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

  try {
    console.log('🚀 Taking stadium overview screenshot after CSS fixes...');

    // Navigate to admin login
    await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForNavigation({ waitUntil: 'networkidle' });

    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(5000);

    // Take screenshot of current state
    await page.screenshot({
      path: 'stadium-css-fixes-verification.png',
      fullPage: true
    });

    // Check what's visible on the page
    const pageAnalysis = await page.evaluate(() => {
      return {
        title: document.title,
        url: window.location.href,
        stadiumContainer: {
          exists: !!document.querySelector('#admin-stadium-container'),
          visible: document.querySelector('#admin-stadium-container')?.offsetParent !== null
        },
        stadiumStates: {
          loading: !!document.querySelector('#admin-stadium-loading-state'),
          error: !!document.querySelector('#admin-stadium-error-state'),
          empty: !!document.querySelector('#admin-stadium-empty-state'),
          grid: !!document.querySelector('#admin-stadium-grid-layout')
        },
        visibleText: document.body.innerText.substring(0, 500)
      };
    });

    console.log('📋 Page Analysis:', JSON.stringify(pageAnalysis, null, 2));

    // Try to click demo button if stadium is empty
    if (pageAnalysis.stadiumStates.empty || pageAnalysis.stadiumStates.error) {
      console.log('🔧 Stadium is empty/error - trying to generate demo layout...');

      const demoButtons = [
        '#admin-stadium-demo-btn',
        '#admin-stadium-generate-btn',
        'button:has-text("Generate Demo Layout")',
        'button:has-text("Generate Demo")'
      ];

      for (const selector of demoButtons) {
        try {
          if (await page.isVisible(selector)) {
            await page.click(selector);
            console.log(`✅ Clicked demo button: ${selector}`);
            await page.waitForTimeout(3000);
            break;
          }
        } catch (e) {
          // Continue to next selector
        }
      }

      // Take another screenshot after demo generation
      await page.screenshot({
        path: 'stadium-after-demo-generation.png',
        fullPage: true
      });

      // Check state again
      const newState = await page.evaluate(() => {
        return {
          hasGrid: !!document.querySelector('#admin-stadium-grid-layout'),
          hasField: !!document.querySelector('#admin-stadium-field'),
          standsCount: document.querySelectorAll('.stadium-stand').length
        };
      });

      console.log('🔄 After demo generation:', JSON.stringify(newState, null, 2));
    }

    console.log('📸 Screenshots saved:');
    console.log('- stadium-css-fixes-verification.png (initial state)');
    console.log('- stadium-after-demo-generation.png (after demo generation if applicable)');

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({ path: 'stadium-screenshot-error.png' });
  }

  await browser.close();
})();