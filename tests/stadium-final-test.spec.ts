import { test, expect } from '@playwright/test';

const ADMIN_URL = 'https://localhost:7030';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Final Stadium Overview Tests', () => {
  test('Complete stadium visibility verification', async ({ page, context }) => {
    console.log('🏆 Final stadium visibility verification test...');

    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    await page.setViewportSize({ width: 1920, height: 1080 });

    try {
      // Step 1: Navigate to login
      console.log('📍 Step 1: Navigate to login page');
      await page.goto(`${ADMIN_URL}/login`, {
        waitUntil: 'domcontentloaded',
        timeout: 20000
      });

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-step1-login.png',
        fullPage: true
      });

      // Step 2: Fill and submit login
      console.log('📍 Step 2: Login with admin credentials');
      await page.fill('#admin-login-email-input', ADMIN_EMAIL);
      await page.fill('#admin-login-password-input', ADMIN_PASSWORD);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-step2-form-filled.png',
        fullPage: true
      });

      await page.click('#admin-login-submit-btn');

      // Step 3: Wait for authentication and check for redirect
      console.log('📍 Step 3: Wait for authentication');
      await page.waitForTimeout(8000);

      const currentUrl = page.url();
      console.log(`📍 Current URL after login: ${currentUrl}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-step3-after-login.png',
        fullPage: true
      });

      // Check for any error messages
      const loginErrors = await page.locator('.alert-danger, .text-danger, .error').allTextContents();
      if (loginErrors.length > 0) {
        console.log('❌ Login errors detected:', loginErrors);
      } else {
        console.log('✅ No login errors detected');
      }

      // Step 4: Navigate to stadium overview (regardless of login status)
      console.log('📍 Step 4: Navigate to stadium overview');
      await page.goto(`${ADMIN_URL}/admin/stadium-overview`, {
        waitUntil: 'domcontentloaded',
        timeout: 20000
      });

      await page.waitForTimeout(5000);

      const stadiumUrl = page.url();
      console.log(`📍 Stadium overview URL: ${stadiumUrl}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-step4-stadium-page.png',
        fullPage: true
      });

      // Step 5: Comprehensive stadium element analysis
      console.log('📍 Step 5: Analyze stadium elements');

      // Check all possible stadium selectors
      const stadiumSelectors = [
        '#admin-stadium-container',
        '.stadium-container',
        '.stadium-overview',
        '.stadium-field',
        '.stadium-field-layout',
        '.stadium-stand',
        '.sector',
        '[class*="stadium"]',
        '[id*="stadium"]'
      ];

      const stadiumElements = {};
      for (const selector of stadiumSelectors) {
        const elements = await page.locator(selector).count();
        const isVisible = elements > 0 ? await page.locator(selector).first().isVisible().catch(() => false) : false;
        stadiumElements[selector] = { count: elements, visible: isVisible };
      }

      console.log('🏟️ Stadium element analysis:', stadiumElements);

      // Check page content
      const pageTitle = await page.title();
      const bodyText = await page.locator('body').textContent();
      const hasStadiumContent = bodyText?.includes('Stadium') || bodyText?.includes('Overview');

      console.log(`📄 Page title: ${pageTitle}`);
      console.log(`🔍 Has stadium content: ${hasStadiumContent}`);

      // Check for error messages on stadium page
      const stadiumErrors = await page.locator('.alert-danger, .error, [class*="error"]').allTextContents();
      if (stadiumErrors.length > 0) {
        console.log('⚠️ Stadium page errors:', stadiumErrors);
      } else {
        console.log('✅ No errors on stadium page');
      }

      // Check what CSS files are loaded
      const cssFiles = await page.evaluate(() => {
        const links = Array.from(document.querySelectorAll('link[rel="stylesheet"]'));
        return links.map(link => link.href);
      });

      const stadiumCssFiles = cssFiles.filter(href =>
        href.includes('stadium') || href.includes('field') || href.includes('layout')
      );

      console.log(`📄 Stadium CSS files loaded: ${stadiumCssFiles.length}`);
      stadiumCssFiles.forEach(file => console.log(`   - ${file}`));

      // Take detailed screenshot of specific areas
      if (stadiumElements['#admin-stadium-container'].count > 0) {
        try {
          await page.locator('#admin-stadium-container').screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-stadium-container.png'
          });
          console.log('✅ Stadium container screenshot captured');
        } catch (error) {
          console.log('⚠️ Could not capture stadium container screenshot');
        }
      }

      // Step 6: Final assessment
      console.log('📍 Step 6: Final assessment');

      const mainStadiumContainer = stadiumElements['#admin-stadium-container'];
      const stadiumField = stadiumElements['.stadium-field'];
      const stadiumStands = stadiumElements['.stadium-stand'];
      const sectors = stadiumElements['.sector'];

      const stadiumVisible = mainStadiumContainer.visible || stadiumField.visible || stadiumStands.visible || sectors.visible;

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-complete-analysis.png',
        fullPage: true
      });

      // Generate comprehensive report
      console.log('\n🏟️ STADIUM VISIBILITY FINAL REPORT:');
      console.log('=====================================');
      console.log(`Authentication Status: ${!currentUrl.includes('/login') ? '✅ SUCCESS' : '❌ FAILED'}`);
      console.log(`Stadium Page Access: ${stadiumUrl.includes('stadium-overview') ? '✅ SUCCESS' : '❌ FAILED'}`);
      console.log(`Stadium Container (#admin-stadium-container): ${mainStadiumContainer.visible ? '✅ VISIBLE' : '❌ NOT VISIBLE'}`);
      console.log(`Stadium Field (.stadium-field): ${stadiumField.visible ? '✅ VISIBLE' : '❌ NOT VISIBLE'}`);
      console.log(`Stadium Stands (.stadium-stand): ${stadiumStands.count > 0 ? `✅ ${stadiumStands.count} FOUND` : '❌ NONE FOUND'}`);
      console.log(`Sectors (.sector): ${sectors.count > 0 ? `✅ ${sectors.count} FOUND` : '❌ NONE FOUND'}`);
      console.log(`Stadium CSS Files: ${stadiumCssFiles.length > 0 ? `✅ ${stadiumCssFiles.length} LOADED` : '❌ NONE LOADED'}`);
      console.log(`Overall Stadium Visibility: ${stadiumVisible ? '✅ VISIBLE' : '❌ NOT VISIBLE'}`);
      console.log('=====================================');

      if (loginErrors.length > 0) {
        console.log('🔧 AUTHENTICATION ISSUES:');
        loginErrors.forEach(error => console.log(`   - ${error}`));
      }

      if (stadiumErrors.length > 0) {
        console.log('🔧 STADIUM PAGE ISSUES:');
        stadiumErrors.forEach(error => console.log(`   - ${error}`));
      }

      if (!stadiumVisible) {
        console.log('🔧 NEXT STEPS TO FIX STADIUM VISIBILITY:');
        console.log('   1. Check if stadium data exists in database');
        console.log('   2. Verify stadium structure import has been completed');
        console.log('   3. Check stadium rendering logic in StadiumOverview.razor');
        console.log('   4. Verify stadium CSS files are properly loaded');
        console.log('   5. Check for JavaScript errors in browser console');
      }

    } catch (error) {
      console.log(`❌ Test error: ${error.message}`);
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\final-error.png',
        fullPage: true
      });
      throw error;
    }
  });
});