const { chromium } = require('playwright');
const path = require('path');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors']
  });

  const context = await browser.newContext({
    viewport: { width: 1920, height: 1080 }
  });

  const page = await context.newPage();

  try {
    const demoPath = path.resolve(__dirname, 'stadium-demo.html');
    console.log('Opening demo HTML file:', demoPath);

    await page.goto(`file://${demoPath}`);
    await page.waitForLoadState('networkidle');

    // Wait a moment for all styles to apply
    await page.waitForTimeout(2000);

    console.log('Taking screenshot of NK Osijek-style stadium layout...');
    await page.screenshot({
      path: '.playwright-mcp/stadium-layout-final-success.png',
      fullPage: true
    });

    console.log('✅ Demo screenshot saved successfully!');
    console.log('📁 Location: .playwright-mcp/stadium-layout-final-success.png');

    // Keep browser open for 10 seconds to inspect
    await page.waitForTimeout(10000);

  } catch (error) {
    console.error('❌ Error:', error.message);
  } finally {
    await browser.close();
  }
})();