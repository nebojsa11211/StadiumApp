import { test, expect } from '@playwright/test';

const ADMIN_URL = 'https://localhost:7030';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Stadium Navigation Tests', () => {
  test('Navigate to stadium overview via sidebar menu', async ({ page, context }) => {
    console.log('🎯 Testing stadium navigation via sidebar menu...');

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

      console.log('✅ Loaded login page');

      // Take screenshot of login page
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\nav-01-login.png',
        fullPage: true
      });

      // Fill and submit login form
      await page.fill('#admin-login-email-input', ADMIN_EMAIL);
      await page.fill('#admin-login-password-input', ADMIN_PASSWORD);

      console.log('✅ Filled login form');

      await page.click('#admin-login-submit-btn');

      // Wait for redirect after login and check if we're on dashboard
      await page.waitForTimeout(5000);

      let currentUrl = page.url();
      console.log(`📍 Current URL after login: ${currentUrl}`);

      // Take screenshot after login
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\nav-02-after-login.png',
        fullPage: true
      });

      // If still on login page, there's an authentication issue
      if (currentUrl.includes('/login')) {
        console.log('⚠️ Still on login page - authentication may have failed');

        // Check for error messages
        const errorMessages = await page.locator('.alert-danger, .text-danger, .error').allTextContents();
        if (errorMessages.length > 0) {
          console.log('❌ Login errors found:', errorMessages);
        }

        // Let's try waiting longer for login to process
        await page.waitForTimeout(10000);
        currentUrl = page.url();
        console.log(`📍 URL after waiting longer: ${currentUrl}`);
      }

      // If we're not on login page anymore, try to navigate to stadium overview
      if (!currentUrl.includes('/login')) {
        console.log('✅ Successfully logged in');

        // Try clicking on Stadium Overview link in sidebar
        const stadiumOverviewLink = page.locator('text="Stadium Overview"');

        if (await stadiumOverviewLink.isVisible()) {
          console.log('🔗 Found Stadium Overview link in sidebar');

          await stadiumOverviewLink.click();
          await page.waitForTimeout(3000);

          console.log('✅ Clicked Stadium Overview link');

          currentUrl = page.url();
          console.log(`📍 URL after clicking link: ${currentUrl}`);

        } else {
          console.log('⚠️ Stadium Overview link not found in sidebar');

          // Let's see what navigation options are available
          const navLinks = await page.locator('nav a, .nav-link, .sidebar a').allTextContents();
          console.log('🔗 Available navigation links:', navLinks);
        }

        // Take screenshot of current page
        await page.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\nav-03-stadium-page.png',
          fullPage: true
        });

        // Check if we're now on stadium overview page
        if (currentUrl.includes('stadium-overview')) {
          console.log('✅ Successfully navigated to stadium overview');

          // Now check for stadium elements
          await page.waitForTimeout(3000);

          const stadiumContainer = page.locator('#admin-stadium-container');
          const isStadiumVisible = await stadiumContainer.isVisible().catch(() => false);

          console.log(`🏟️ Stadium container visible: ${isStadiumVisible ? '✅ YES' : '❌ NO'}`);

          if (isStadiumVisible) {
            console.log('✅ Stadium container found!');

            // Get stadium data from page
            const stadiumData = await page.evaluate(() => {
              const container = document.querySelector('#admin-stadium-container');
              if (container) {
                return {
                  innerHTML: container.innerHTML.substring(0, 500),
                  childElementCount: container.childElementCount,
                  classes: container.className
                };
              }
              return null;
            });

            console.log('🏟️ Stadium container data:', stadiumData);

          } else {
            console.log('❌ Stadium container not found');

            // Check what elements are on the page
            const pageContent = await page.locator('main, .content, .page-content').textContent();
            console.log('📄 Page content preview:', pageContent?.substring(0, 300));

            // Check for any stadium-related elements by different selectors
            const stadiumElements = await page.locator('[class*="stadium"], [id*="stadium"], [class*="field"]').count();
            console.log(`🔍 Found ${stadiumElements} stadium-related elements`);
          }

          // Take final screenshot
          await page.screenshot({
            path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\nav-04-final.png',
            fullPage: true
          });

        } else {
          console.log('❌ Not on stadium overview page');
          console.log(`📍 Current URL: ${currentUrl}`);
        }

      } else {
        console.log('❌ Login failed - still on login page');
      }

    } catch (error) {
      console.log(`❌ Test error: ${error.message}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\nav-error.png',
        fullPage: true
      });

      throw error;
    }
  });

  test('Direct navigation to dashboard and then stadium overview', async ({ page, context }) => {
    console.log('🎯 Testing direct dashboard navigation...');

    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    await page.setViewportSize({ width: 1920, height: 1080 });

    // Login first
    await page.goto(`${ADMIN_URL}/login`, { waitUntil: 'domcontentloaded' });
    await page.fill('#admin-login-email-input', ADMIN_EMAIL);
    await page.fill('#admin-login-password-input', ADMIN_PASSWORD);
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(5000);

    // Try navigating directly to dashboard
    await page.goto(`${ADMIN_URL}/admin/dashboard`, {
      waitUntil: 'domcontentloaded',
      timeout: 15000
    });

    await page.waitForTimeout(3000);

    const currentUrl = page.url();
    console.log(`📍 Dashboard URL: ${currentUrl}`);

    // Take screenshot of dashboard
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\direct-01-dashboard.png',
      fullPage: true
    });

    // From dashboard, try to go to stadium overview
    if (!currentUrl.includes('/login')) {
      console.log('✅ Successfully accessed dashboard');

      await page.goto(`${ADMIN_URL}/admin/stadium-overview`, {
        waitUntil: 'domcontentloaded',
        timeout: 15000
      });

      await page.waitForTimeout(3000);

      const stadiumUrl = page.url();
      console.log(`📍 Stadium URL: ${stadiumUrl}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\.playwright-mcp\\direct-02-stadium.png',
        fullPage: true
      });

      if (stadiumUrl.includes('stadium-overview')) {
        console.log('✅ Successfully accessed stadium overview');

        // Check stadium visibility
        const stadiumContainer = page.locator('#admin-stadium-container');
        const isVisible = await stadiumContainer.isVisible().catch(() => false);

        console.log(`🏟️ Stadium visible: ${isVisible ? '✅ YES' : '❌ NO'}`);

      } else {
        console.log('❌ Redirected away from stadium overview');
      }

    } else {
      console.log('❌ Could not access dashboard - redirected to login');
    }
  });
});