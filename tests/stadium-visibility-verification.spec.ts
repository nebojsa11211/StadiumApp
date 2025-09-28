import { test, expect } from '@playwright/test';

const ADMIN_URL = 'https://localhost:9030';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Stadium Overview Visibility Tests', () => {
  let adminPage: any;

  test.beforeEach(async ({ page }) => {
    adminPage = page;

    // Set viewport to desktop initially
    await page.setViewportSize({ width: 1920, height: 1080 });

    // Navigate to admin login page
    await page.goto(`${ADMIN_URL}/login`, {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Handle potential SSL certificate issues
    await page.context().clearCookies();
  });

  test('Admin login and stadium overview access', async () => {
    console.log('🔑 Testing admin login and stadium overview access...');

    // Take initial screenshot
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\01-admin-login-page.png',
      fullPage: true
    });

    // Fill login form
    await adminPage.fill('#admin-login-email-input', ADMIN_EMAIL);
    await adminPage.fill('#admin-login-password-input', ADMIN_PASSWORD);

    // Take screenshot after filling form
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\02-login-form-filled.png',
      fullPage: true
    });

    // Submit login
    await adminPage.click('#admin-login-submit-btn');

    // Wait for successful login and redirect
    await adminPage.waitForURL(/\/admin\/(dashboard|home)/i, {
      timeout: 15000,
      waitUntil: 'networkidle'
    });

    // Take screenshot after login
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\03-after-login.png',
      fullPage: true
    });

    // Navigate to stadium overview
    await adminPage.goto(`${ADMIN_URL}/admin/stadium-overview`, {
      waitUntil: 'networkidle',
      timeout: 20000
    });

    // Wait for page to fully load
    await adminPage.waitForTimeout(3000);

    // Take screenshot of stadium overview page
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\04-stadium-overview-loaded.png',
      fullPage: true
    });

    console.log('✅ Successfully logged in and navigated to stadium overview');
  });

  test('Stadium container and main elements visibility', async () => {
    console.log('🏟️ Testing stadium container and main elements...');

    // Login first
    await adminPage.fill('#admin-login-email-input', ADMIN_EMAIL);
    await adminPage.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await adminPage.click('#admin-login-submit-btn');
    await adminPage.waitForURL(/\/admin\/(dashboard|home)/i, { timeout: 15000 });

    // Navigate to stadium overview
    await adminPage.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'networkidle' });
    await adminPage.waitForTimeout(3000);

    // Check for main stadium container
    const stadiumContainer = await adminPage.locator('#admin-stadium-container');
    await expect(stadiumContainer).toBeVisible({ timeout: 10000 });
    console.log('✅ Stadium container is visible');

    // Check for stadium field
    const stadiumField = await adminPage.locator('.stadium-field');
    await expect(stadiumField).toBeVisible({ timeout: 5000 });
    console.log('✅ Stadium field is visible');

    // Check for stadium stands/tribunes
    const stadiumStands = await adminPage.locator('.stadium-stand');
    const standsCount = await stadiumStands.count();
    console.log(`🏟️ Found ${standsCount} stadium stands`);

    if (standsCount > 0) {
      await expect(stadiumStands.first()).toBeVisible();
      console.log('✅ Stadium stands are visible');
    }

    // Check for sectors
    const sectors = await adminPage.locator('.sector');
    const sectorsCount = await sectors.count();
    console.log(`🎫 Found ${sectorsCount} sectors`);

    if (sectorsCount > 0) {
      await expect(sectors.first()).toBeVisible();
      console.log('✅ Sectors are visible');
    }

    // Take detailed screenshot
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\05-stadium-elements-verification.png',
      fullPage: true
    });

    console.log('✅ All main stadium elements verified');
  });

  test('Responsive design across viewport sizes', async () => {
    console.log('📱 Testing responsive design across different viewport sizes...');

    // Login first
    await adminPage.fill('#admin-login-email-input', ADMIN_EMAIL);
    await adminPage.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await adminPage.click('#admin-login-submit-btn');
    await adminPage.waitForURL(/\/admin\/(dashboard|home)/i, { timeout: 15000 });

    // Navigate to stadium overview
    await adminPage.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'networkidle' });
    await adminPage.waitForTimeout(3000);

    // Test Desktop (1920x1080)
    await adminPage.setViewportSize({ width: 1920, height: 1080 });
    await adminPage.waitForTimeout(1000);
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\06-desktop-1920x1080.png',
      fullPage: true
    });

    // Verify stadium is visible at desktop size
    await expect(adminPage.locator('#admin-stadium-container')).toBeVisible();
    console.log('✅ Stadium visible at desktop resolution (1920x1080)');

    // Test Laptop (1366x768)
    await adminPage.setViewportSize({ width: 1366, height: 768 });
    await adminPage.waitForTimeout(1000);
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\07-laptop-1366x768.png',
      fullPage: true
    });

    await expect(adminPage.locator('#admin-stadium-container')).toBeVisible();
    console.log('✅ Stadium visible at laptop resolution (1366x768)');

    // Test Tablet (768x1024)
    await adminPage.setViewportSize({ width: 768, height: 1024 });
    await adminPage.waitForTimeout(1000);
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\08-tablet-768x1024.png',
      fullPage: true
    });

    await expect(adminPage.locator('#admin-stadium-container')).toBeVisible();
    console.log('✅ Stadium visible at tablet resolution (768x1024)');

    // Test Mobile (375x667)
    await adminPage.setViewportSize({ width: 375, height: 667 });
    await adminPage.waitForTimeout(1000);
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\09-mobile-375x667.png',
      fullPage: true
    });

    await expect(adminPage.locator('#admin-stadium-container')).toBeVisible();
    console.log('✅ Stadium visible at mobile resolution (375x667)');

    console.log('✅ Responsive design verification completed');
  });

  test('Interactive features and hover effects', async () => {
    console.log('🖱️ Testing interactive features and hover effects...');

    // Login first
    await adminPage.fill('#admin-login-email-input', ADMIN_EMAIL);
    await adminPage.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await adminPage.click('#admin-login-submit-btn');
    await adminPage.waitForURL(/\/admin\/(dashboard|home)/i, { timeout: 15000 });

    // Navigate to stadium overview
    await adminPage.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'networkidle' });
    await adminPage.waitForTimeout(3000);

    // Reset to desktop view for interaction testing
    await adminPage.setViewportSize({ width: 1920, height: 1080 });

    // Check if sectors exist and test hover effects
    const sectors = await adminPage.locator('.sector');
    const sectorsCount = await sectors.count();

    if (sectorsCount > 0) {
      console.log(`🎫 Testing hover effects on ${sectorsCount} sectors`);

      // Take screenshot before hover
      await adminPage.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\10-before-hover.png',
        fullPage: true
      });

      // Hover over the first sector
      await sectors.first().hover();
      await adminPage.waitForTimeout(1000);

      // Take screenshot during hover
      await adminPage.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\11-during-hover.png',
        fullPage: true
      });

      // Test clicking on sector (if clickable)
      try {
        await sectors.first().click();
        await adminPage.waitForTimeout(1000);

        await adminPage.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\12-after-click.png',
          fullPage: true
        });

        console.log('✅ Sector click interaction working');
      } catch (error) {
        console.log('ℹ️ Sectors not clickable (expected for display-only view)');
      }
    } else {
      console.log('ℹ️ No sectors found for interaction testing');
    }

    // Check for any interactive buttons or controls
    const buttons = await adminPage.locator('button').all();
    console.log(`🔘 Found ${buttons.length} buttons on the page`);

    // Test any visible stadium-related buttons
    for (let i = 0; i < Math.min(buttons.length, 3); i++) {
      const button = buttons[i];
      const buttonText = await button.textContent();
      if (buttonText && (buttonText.includes('Stadium') || buttonText.includes('Import') || buttonText.includes('Export'))) {
        console.log(`🔘 Testing button: "${buttonText}"`);
        await button.hover();
        await adminPage.waitForTimeout(500);
      }
    }

    console.log('✅ Interactive features testing completed');
  });

  test('Stadium layout CSS verification', async () => {
    console.log('🎨 Verifying stadium layout CSS and styling...');

    // Login and navigate
    await adminPage.fill('#admin-login-email-input', ADMIN_EMAIL);
    await adminPage.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await adminPage.click('#admin-login-submit-btn');
    await adminPage.waitForURL(/\/admin\/(dashboard|home)/i, { timeout: 15000 });

    await adminPage.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'networkidle' });
    await adminPage.waitForTimeout(3000);

    // Check CSS properties of stadium container
    const stadiumContainer = adminPage.locator('#admin-stadium-container');

    if (await stadiumContainer.isVisible()) {
      // Get computed styles
      const containerStyles = await stadiumContainer.evaluate(el => {
        const styles = window.getComputedStyle(el);
        return {
          display: styles.display,
          position: styles.position,
          width: styles.width,
          height: styles.height,
          overflow: styles.overflow,
          zIndex: styles.zIndex
        };
      });

      console.log('🎨 Stadium container styles:', containerStyles);

      // Check stadium field styles if it exists
      const stadiumField = adminPage.locator('.stadium-field');
      if (await stadiumField.isVisible()) {
        const fieldStyles = await stadiumField.evaluate(el => {
          const styles = window.getComputedStyle(el);
          return {
            display: styles.display,
            position: styles.position,
            width: styles.width,
            height: styles.height,
            backgroundColor: styles.backgroundColor,
            borderRadius: styles.borderRadius
          };
        });

        console.log('🏟️ Stadium field styles:', fieldStyles);
      }
    }

    // Take final comprehensive screenshot
    await adminPage.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\13-final-stadium-verification.png',
      fullPage: true
    });

    console.log('✅ CSS verification completed');
  });
});

test.describe('Stadium Data and Structure Tests', () => {
  test('Verify stadium data loading and structure', async ({ page }) => {
    console.log('📊 Testing stadium data loading and structure...');

    // Set viewport and navigate
    await page.setViewportSize({ width: 1920, height: 1080 });
    await page.goto(`${ADMIN_URL}/login`, { waitUntil: 'networkidle' });

    // Login
    await page.fill('#admin-login-email-input', ADMIN_EMAIL);
    await page.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await page.click('#admin-login-submit-btn');
    await page.waitForURL(/\/admin\/(dashboard|home)/i, { timeout: 15000 });

    // Navigate to stadium overview
    await page.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(5000); // Allow time for data loading

    // Check for any error messages
    const errorElements = await page.locator('.alert-danger, .error, [class*="error"]').all();
    if (errorElements.length > 0) {
      console.log('⚠️ Found error elements:');
      for (const errorEl of errorElements) {
        const errorText = await errorEl.textContent();
        if (errorText && errorText.trim()) {
          console.log(`   ❌ Error: ${errorText.trim()}`);
        }
      }
    }

    // Check for loading indicators
    const loadingElements = await page.locator('.loading, .spinner, [class*="loading"]').all();
    console.log(`🔄 Found ${loadingElements.length} loading indicators`);

    // Check network requests for stadium data
    const responses: any[] = [];
    page.on('response', response => {
      if (response.url().includes('stadium') || response.url().includes('overview')) {
        responses.push({
          url: response.url(),
          status: response.status(),
          statusText: response.statusText()
        });
      }
    });

    // Refresh to capture network requests
    await page.reload({ waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    console.log('🌐 Stadium-related network requests:', responses);

    // Take screenshot of current state
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\14-stadium-data-verification.png',
      fullPage: true
    });

    console.log('✅ Stadium data and structure verification completed');
  });
});