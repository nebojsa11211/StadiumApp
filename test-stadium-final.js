const { chromium } = require('playwright');

(async () => {
  console.log('🏟️ Final Stadium Visualization Test');

  const browser = await chromium.launch({
    headless: false,
    slowMo: 500,
    args: ['--ignore-certificate-errors-spki-list', '--ignore-certificate-errors', '--ignore-ssl-errors']
  });

  const context = await browser.newContext({
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  try {
    console.log('Step 1: Navigating to admin app');
    await page.goto('https://localhost:7030', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    await page.screenshot({
      path: '.playwright-mcp/final-test-step1-admin-home.png',
      fullPage: true
    });

    console.log('Step 2: Check current page status');
    const url = await page.url();
    const title = await page.title();
    console.log(`Current URL: ${url}`);
    console.log(`Page title: ${title}`);

    // Try to find Stadium Overview link in navigation regardless of auth status
    console.log('Step 3: Looking for Stadium Overview in navigation');

    const stadiumOverviewLink = await page.locator('a[href*="stadium-overview"], a:has-text("Stadium Overview")').first();
    const isVisible = await stadiumOverviewLink.isVisible().catch(() => false);

    console.log(`Stadium Overview link visible: ${isVisible}`);

    if (isVisible) {
      console.log('Step 4: Clicking Stadium Overview link');
      await stadiumOverviewLink.click();
      await page.waitForTimeout(3000);

      await page.screenshot({
        path: '.playwright-mcp/final-test-step4-stadium-overview.png',
        fullPage: true
      });
    } else {
      console.log('Step 4: Direct navigation to stadium overview');
      await page.goto('https://localhost:7030/stadium-overview', {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      await page.waitForTimeout(3000);

      await page.screenshot({
        path: '.playwright-mcp/final-test-step4-stadium-overview-direct.png',
        fullPage: true
      });
    }

    console.log('Step 5: Analyzing stadium visualization elements');

    const finalUrl = await page.url();
    console.log(`Final URL: ${finalUrl}`);

    // Check for various stadium elements
    const elements = {
      stadiumContainer: await page.locator('.stadium-container').isVisible().catch(() => false),
      infoPanel: await page.locator('.info-panel').isVisible().catch(() => false),
      stadiumSvg: await page.locator('.stadium-svg').isVisible().catch(() => false),
      stadiumData: await page.locator('.stadium-data').isVisible().catch(() => false),
      tribunes: await page.locator('.tribune').count().catch(() => 0),
      loadingSpinner: await page.locator('.loading, .spinner').isVisible().catch(() => false),
      errorMessage: await page.locator('.alert-danger, .error').isVisible().catch(() => false)
    };

    console.log('📊 Element Analysis:');
    for (const [key, value] of Object.entries(elements)) {
      console.log(`   ${key}: ${value}`);
    }

    // Check page text for important keywords
    const bodyText = await page.textContent('body').catch(() => '');
    const hasStadiumContent = bodyText.includes('Stadium') || bodyText.includes('tribunes') || bodyText.includes('capacity');
    const hasErrorContent = bodyText.includes('error') || bodyText.includes('failed') || bodyText.includes('not found');

    console.log(`🔍 Content Analysis:`);
    console.log(`   Has stadium content: ${hasStadiumContent}`);
    console.log(`   Has error content: ${hasErrorContent}`);

    if (bodyText.length < 200) {
      console.log(`   Page text preview: ${bodyText}`);
    } else {
      console.log(`   Page text preview (first 200 chars): ${bodyText.substring(0, 200)}...`);
    }

    // Take comprehensive final screenshot
    await page.screenshot({
      path: '.playwright-mcp/final-stadium-visualization-verification.png',
      fullPage: true
    });

    console.log('Step 6: Stadium visualization verification summary');

    if (elements.stadiumContainer && elements.infoPanel) {
      console.log('✅ SUCCESS: Stadium visualization appears to be working!');
      console.log('   - Stadium container is visible');
      console.log('   - Info panel is visible');
      if (elements.tribunes > 0) {
        console.log(`   - Found ${elements.tribunes} tribunes`);
      }
    } else if (finalUrl.includes('login')) {
      console.log('⚠️  AUTHENTICATION REQUIRED: Redirected to login page');
      console.log('   - Stadium visualization is protected');
      console.log('   - Authentication system is working as expected');
    } else if (elements.loadingSpinner) {
      console.log('⏳ LOADING: Stadium data is still loading');
    } else if (elements.errorMessage) {
      console.log('❌ ERROR: Stadium visualization has errors');
    } else {
      console.log('❓ UNCLEAR: Stadium visualization status uncertain');
    }

    console.log('🎯 Test completed! Check screenshots for visual confirmation.');

    // Keep browser open for 20 seconds for manual inspection
    console.log('🔍 Browser will stay open for 20 seconds for inspection...');
    await page.waitForTimeout(20000);

  } catch (error) {
    console.error('❌ Test error:', error.message);
    await page.screenshot({
      path: '.playwright-mcp/final-test-error.png',
      fullPage: true
    });
  } finally {
    await browser.close();
  }
})();