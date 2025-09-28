const { chromium } = require('playwright');
const fs = require('fs');

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
    console.log('🚀 Testing critical stadium layout fixes...');

    // Navigate to admin login
    console.log('📋 Navigating to admin login...');
    await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Login
    console.log('🔐 Logging in as admin...');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForNavigation({ waitUntil: 'networkidle' });

    // Navigate to stadium overview
    console.log('🏟️ Navigating to stadium overview...');
    await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Wait for stadium to load
    await page.waitForSelector('.stadium-grid-layout', { timeout: 10000 });
    await page.waitForTimeout(2000);

    console.log('📊 Analyzing stadium layout...');

    // Check grid layout properties
    const gridProperties = await page.evaluate(() => {
      const stadium = document.querySelector('.stadium-grid-layout');
      if (!stadium) return { error: 'Stadium grid not found' };

      const styles = window.getComputedStyle(stadium);
      return {
        display: styles.display,
        gridTemplateColumns: styles.gridTemplateColumns,
        gridTemplateRows: styles.gridTemplateRows,
        width: styles.width,
        height: styles.height,
        position: styles.position
      };
    });

    console.log('🎯 Grid Properties:', gridProperties);

    // Check field properties
    const fieldProperties = await page.evaluate(() => {
      const field = document.querySelector('.stadium-field');
      if (!field) return { error: 'Stadium field not found' };

      const styles = window.getComputedStyle(field);
      const rect = field.getBoundingClientRect();
      return {
        gridArea: styles.gridArea,
        width: `${rect.width}px`,
        height: `${rect.height}px`,
        backgroundColor: styles.backgroundColor,
        borderRadius: styles.borderRadius,
        visibility: styles.visibility,
        display: styles.display
      };
    });

    console.log('🟢 Field Properties:', fieldProperties);

    // Check tribune visibility and positioning
    const tribuneInfo = await page.evaluate(() => {
      const tribunes = {
        north: document.querySelector('.stand-n, .stand-position-north'),
        south: document.querySelector('.stand-s, .stand-position-south'),
        east: document.querySelector('.stand-e, .stand-position-east'),
        west: document.querySelector('.stand-w, .stand-position-west')
      };

      const info = {};
      Object.entries(tribunes).forEach(([direction, tribune]) => {
        if (tribune) {
          const styles = window.getComputedStyle(tribune);
          const rect = tribune.getBoundingClientRect();
          info[direction] = {
            found: true,
            gridArea: styles.gridArea,
            width: `${rect.width}px`,
            height: `${rect.height}px`,
            visibility: styles.visibility,
            display: styles.display,
            className: tribune.className
          };
        } else {
          info[direction] = { found: false };
        }
      });

      return info;
    });

    console.log('🏟️ Tribune Information:', JSON.stringify(tribuneInfo, null, 2));

    // Check overall container dimensions
    const containerInfo = await page.evaluate(() => {
      const container = document.querySelector('.stadium-container, .stadium-overview-container');
      if (!container) return { error: 'Stadium container not found' };

      const rect = container.getBoundingClientRect();
      const styles = window.getComputedStyle(container);
      return {
        width: `${rect.width}px`,
        height: `${rect.height}px`,
        maxHeight: styles.maxHeight,
        minHeight: styles.minHeight,
        overflow: styles.overflow
      };
    });

    console.log('📦 Container Information:', containerInfo);

    // Take screenshot
    console.log('📸 Taking screenshot...');
    await page.screenshot({
      path: 'stadium-critical-fixes-verification.png',
      fullPage: false
    });

    // Check if all 4 tribunes are visible
    const allTribunesVisible = Object.values(tribuneInfo).every(tribune =>
      tribune.found &&
      tribune.visibility === 'visible' &&
      tribune.display !== 'none'
    );

    console.log('✅ Assessment Results:');
    console.log(`- Grid Layout: ${gridProperties.display === 'grid' ? '✅ WORKING' : '❌ BROKEN'}`);
    console.log(`- Field Visible: ${fieldProperties.visibility === 'visible' ? '✅ VISIBLE' : '❌ HIDDEN'}`);
    console.log(`- All 4 Tribunes: ${allTribunesVisible ? '✅ VISIBLE' : '❌ MISSING'}`);
    console.log(`- Grid Template Columns: ${gridProperties.gridTemplateColumns}`);
    console.log(`- Grid Template Rows: ${gridProperties.gridTemplateRows}`);
    console.log(`- Field Size: ${fieldProperties.width} x ${fieldProperties.height}`);

    if (allTribunesVisible && fieldProperties.visibility === 'visible' && gridProperties.display === 'grid') {
      console.log('🎉 SUCCESS: Stadium layout fixes are working properly!');
    } else {
      console.log('❌ ISSUE: Stadium layout still has problems');
    }

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({ path: 'stadium-critical-fixes-error.png' });
  }

  await browser.close();
})();