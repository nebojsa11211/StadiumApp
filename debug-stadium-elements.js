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
    console.log('🚀 Debugging stadium elements...');

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

    // Take initial screenshot
    await page.screenshot({ path: 'stadium-debug-initial.png' });

    // Debug: Find all stadium-related elements
    const allStadiumElements = await page.evaluate(() => {
      const elements = document.querySelectorAll('[class*="stadium"]');
      return Array.from(elements).map(el => ({
        tagName: el.tagName,
        className: el.className,
        id: el.id,
        innerHTML: el.innerHTML.substring(0, 100) + '...'
      }));
    });

    console.log('🏟️ All stadium-related elements found:');
    allStadiumElements.forEach((el, index) => {
      console.log(`${index + 1}. ${el.tagName}.${el.className} (id: ${el.id})`);
    });

    // Check page content
    const pageContent = await page.evaluate(() => {
      return {
        title: document.title,
        hasStadiumOverview: !!document.querySelector('.stadium-overview'),
        hasStadiumContainer: !!document.querySelector('.stadium-container'),
        hasStadiumGrid: !!document.querySelector('.stadium-grid-layout'),
        hasStadiumField: !!document.querySelector('.stadium-field'),
        bodyClassList: document.body.className,
        mainContentHTML: document.querySelector('main')?.innerHTML.substring(0, 500) || 'No main found'
      };
    });

    console.log('📋 Page Analysis:', JSON.stringify(pageContent, null, 2));

    // Check if CSS files are loaded
    const cssInfo = await page.evaluate(() => {
      const stylesheets = Array.from(document.styleSheets);
      return stylesheets.map(sheet => {
        try {
          return {
            href: sheet.href,
            rules: sheet.cssRules ? sheet.cssRules.length : 'Cannot access rules'
          };
        } catch (e) {
          return {
            href: sheet.href,
            error: e.message
          };
        }
      });
    });

    console.log('🎨 CSS Files:', JSON.stringify(cssInfo, null, 2));

    // Look for any error messages on page
    const errorMessages = await page.evaluate(() => {
      const errors = document.querySelectorAll('.alert-danger, .error, [class*="error"]');
      return Array.from(errors).map(el => el.textContent.trim());
    });

    if (errorMessages.length > 0) {
      console.log('❌ Error messages found:', errorMessages);
    }

    console.log('📸 Screenshots saved: stadium-debug-initial.png');

  } catch (error) {
    console.error('❌ Debug failed:', error.message);
    await page.screenshot({ path: 'stadium-debug-error.png' });
  }

  await browser.close();
})();