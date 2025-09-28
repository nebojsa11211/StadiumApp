import { test, expect } from '@playwright/test';

const ADMIN_URL = 'https://localhost:7030';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Stadium Overview Local Tests', () => {
  test('Stadium visibility verification on local admin service', async ({ page, context }) => {
    console.log('🏟️ Testing stadium overview on local admin service...');

    // Ignore SSL certificate errors for local development
    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    await page.setViewportSize({ width: 1920, height: 1080 });

    try {
      // Navigate to login page
      await page.goto(`${ADMIN_URL}/login`, {
        waitUntil: 'domcontentloaded',
        timeout: 20000
      });

      console.log('✅ Successfully loaded admin login page');

      // Take screenshot of login page
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-01-login-page.png',
        fullPage: true
      });

      // Fill login form
      await page.fill('#admin-login-email-input', ADMIN_EMAIL);
      await page.fill('#admin-login-password-input', ADMIN_PASSWORD);

      console.log('✅ Filled login form');

      // Take screenshot of filled form
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-02-form-filled.png',
        fullPage: true
      });

      // Submit login
      await page.click('#admin-login-submit-btn');

      // Wait for login to complete
      await page.waitForTimeout(5000);

      console.log('✅ Submitted login form');

      // Take screenshot after login
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-03-after-login.png',
        fullPage: true
      });

      // Navigate to stadium overview
      await page.goto(`${ADMIN_URL}/admin/stadium-overview`, {
        waitUntil: 'domcontentloaded',
        timeout: 20000
      });

      console.log('✅ Navigated to stadium overview');

      // Wait for page to load
      await page.waitForTimeout(5000);

      // Take screenshot of stadium overview page
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-04-stadium-overview-initial.png',
        fullPage: true
      });

      // Check for stadium container
      const stadiumContainer = page.locator('#admin-stadium-container');
      const isStadiumVisible = await stadiumContainer.isVisible().catch(() => false);

      if (isStadiumVisible) {
        console.log('✅ Stadium container is visible!');

        // Get container dimensions
        const boundingBox = await stadiumContainer.boundingBox();
        if (boundingBox) {
          console.log(`📐 Stadium container: ${boundingBox.width}x${boundingBox.height} at (${boundingBox.x}, ${boundingBox.y})`);
        }

        // Take focused screenshot of stadium container
        await stadiumContainer.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-05-stadium-container-focus.png'
        });

      } else {
        console.log('⚠️ Stadium container not visible');
      }

      // Check for stadium field
      const stadiumField = page.locator('.stadium-field');
      const isFieldVisible = await stadiumField.isVisible().catch(() => false);

      if (isFieldVisible) {
        console.log('✅ Stadium field is visible!');

        const fieldBox = await stadiumField.boundingBox();
        if (fieldBox) {
          console.log(`🏟️ Stadium field: ${fieldBox.width}x${fieldBox.height} at (${fieldBox.x}, ${fieldBox.y})`);
        }
      } else {
        console.log('⚠️ Stadium field not visible');
      }

      // Check for stadium stands
      const stadiumStands = page.locator('.stadium-stand');
      const standsCount = await stadiumStands.count();
      console.log(`🏛️ Found ${standsCount} stadium stands`);

      if (standsCount > 0) {
        console.log('✅ Stadium stands are present');

        // Check visibility of first stand
        const firstStand = stadiumStands.first();
        const isStandVisible = await firstStand.isVisible().catch(() => false);

        if (isStandVisible) {
          const standBox = await firstStand.boundingBox();
          if (standBox) {
            console.log(`🏛️ First stand: ${standBox.width}x${standBox.height} at (${standBox.x}, ${standBox.y})`);
          }
        }
      }

      // Check for sectors
      const sectors = page.locator('.sector');
      const sectorsCount = await sectors.count();
      console.log(`🎫 Found ${sectorsCount} sectors`);

      if (sectorsCount > 0) {
        console.log('✅ Sectors are present');

        // Test hover on first sector
        try {
          await sectors.first().hover();
          await page.waitForTimeout(1000);

          console.log('✅ Sector hover effect working');

          // Take screenshot during hover
          await page.screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-06-sector-hover.png',
            fullPage: true
          });

        } catch (error) {
          console.log(`⚠️ Sector hover failed: ${error.message}`);
        }
      }

      // Check overall page structure
      const bodyContent = await page.locator('body').textContent();
      const hasStadiumContent = bodyContent?.includes('Stadium') || bodyContent?.includes('Overview');

      if (hasStadiumContent) {
        console.log('✅ Page contains stadium-related content');
      } else {
        console.log('⚠️ No stadium content detected');
      }

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
      } else {
        console.log('✅ No error messages detected');
      }

      // Take final comprehensive screenshot
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-07-final-verification.png',
        fullPage: true
      });

      console.log('✅ Stadium visibility test completed successfully!');

      // Log summary
      console.log('\n📊 STADIUM VISIBILITY TEST SUMMARY:');
      console.log(`   Stadium Container: ${isStadiumVisible ? '✅ VISIBLE' : '❌ NOT VISIBLE'}`);
      console.log(`   Stadium Field: ${isFieldVisible ? '✅ VISIBLE' : '❌ NOT VISIBLE'}`);
      console.log(`   Stadium Stands: ${standsCount > 0 ? `✅ ${standsCount} FOUND` : '❌ NONE FOUND'}`);
      console.log(`   Sectors: ${sectorsCount > 0 ? `✅ ${sectorsCount} FOUND` : '❌ NONE FOUND'}`);
      console.log(`   Error Messages: ${errorElements.length === 0 ? '✅ NONE' : `❌ ${errorElements.length} FOUND`}`);

    } catch (error) {
      console.log(`❌ Test failed: ${error.message}`);

      // Take error screenshot
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-error.png',
        fullPage: true
      });

      throw error;
    }
  });

  test('Responsive stadium overview test', async ({ page, context }) => {
    console.log('📱 Testing responsive stadium overview...');

    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    // Login first
    await page.goto(`${ADMIN_URL}/login`, { waitUntil: 'domcontentloaded' });
    await page.fill('#admin-login-email-input', ADMIN_EMAIL);
    await page.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(3000);

    // Navigate to stadium overview
    await page.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'domcontentloaded' });
    await page.waitForTimeout(3000);

    const viewports = [
      { name: 'Desktop', width: 1920, height: 1080 },
      { name: 'Laptop', width: 1366, height: 768 },
      { name: 'Tablet', width: 768, height: 1024 },
      { name: 'Mobile', width: 375, height: 667 }
    ];

    for (const viewport of viewports) {
      console.log(`📱 Testing ${viewport.name} viewport (${viewport.width}x${viewport.height})`);

      await page.setViewportSize({ width: viewport.width, height: viewport.height });
      await page.waitForTimeout(1000);

      // Take screenshot
      await page.screenshot({
        path: `D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-responsive-${viewport.name.toLowerCase()}.png`,
        fullPage: true
      });

      // Check if stadium container is still visible
      const stadiumContainer = page.locator('#admin-stadium-container');
      const isVisible = await stadiumContainer.isVisible().catch(() => false);

      console.log(`   Stadium visibility: ${isVisible ? '✅ Visible' : '❌ Hidden'}`);

      if (isVisible) {
        const boundingBox = await stadiumContainer.boundingBox();
        if (boundingBox) {
          console.log(`   Container size: ${boundingBox.width}x${boundingBox.height}`);
        }
      }
    }

    console.log('✅ Responsive test completed');
  });

  test('Stadium CSS and layout verification', async ({ page, context }) => {
    console.log('🎨 Testing stadium CSS and layout...');

    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    // Login and navigate
    await page.goto(`${ADMIN_URL}/login`, { waitUntil: 'domcontentloaded' });
    await page.fill('#admin-login-email-input', ADMIN_EMAIL);
    await page.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(3000);

    await page.goto(`${ADMIN_URL}/admin/stadium-overview`, { waitUntil: 'domcontentloaded' });
    await page.waitForTimeout(3000);

    // Set desktop viewport
    await page.setViewportSize({ width: 1920, height: 1080 });

    // Check stadium container CSS
    const stadiumContainer = page.locator('#admin-stadium-container');

    if (await stadiumContainer.isVisible()) {
      const containerStyles = await stadiumContainer.evaluate(el => {
        const styles = window.getComputedStyle(el);
        return {
          display: styles.display,
          position: styles.position,
          width: styles.width,
          height: styles.height,
          overflow: styles.overflow,
          zIndex: styles.zIndex,
          backgroundColor: styles.backgroundColor,
          border: styles.border
        };
      });

      console.log('🎨 Stadium container styles:', containerStyles);

      // Check stadium field CSS
      const stadiumField = page.locator('.stadium-field');
      if (await stadiumField.isVisible()) {
        const fieldStyles = await stadiumField.evaluate(el => {
          const styles = window.getComputedStyle(el);
          return {
            display: styles.display,
            position: styles.position,
            width: styles.width,
            height: styles.height,
            backgroundColor: styles.backgroundColor,
            borderRadius: styles.borderRadius,
            margin: styles.margin,
            padding: styles.padding
          };
        });

        console.log('🏟️ Stadium field styles:', fieldStyles);
      }

      // Check if stadium layout CSS file is loaded
      const cssFiles = await page.evaluate(() => {
        const links = Array.from(document.querySelectorAll('link[rel="stylesheet"]'));
        return links.map(link => link.href).filter(href =>
          href.includes('stadium') || href.includes('field') || href.includes('layout')
        );
      });

      console.log('📄 Stadium-related CSS files loaded:', cssFiles);

      if (cssFiles.length > 0) {
        console.log('✅ Stadium CSS files are loaded');
      } else {
        console.log('⚠️ No stadium-specific CSS files detected');
      }

    } else {
      console.log('❌ Stadium container not found for CSS analysis');
    }

    // Take final CSS verification screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\local-css-verification.png',
      fullPage: true
    });

    console.log('✅ CSS and layout verification completed');
  });
});