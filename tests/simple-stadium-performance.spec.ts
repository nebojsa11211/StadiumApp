import { test, expect } from '@playwright/test';

test.describe('Simple Stadium Overview Performance Test', () => {
  test('should measure stadium-overview page load time', async ({ page }) => {
    console.log('🚀 Starting simple stadium-overview performance test...');
    const startTime = Date.now();

    // Step 1: Navigate and login
    console.log('📍 Step 1: Navigating to admin page...');
    const navigationStart = Date.now();

    await page.goto('https://localhost:7030', {
      waitUntil: 'domcontentloaded',
      timeout: 30000
    });

    const pageLoadTime = Date.now() - navigationStart;
    console.log(`✅ Initial page load: ${pageLoadTime}ms`);

    // Take screenshot of login page
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-01-login-page.png',
      fullPage: true
    });

    // Step 2: Perform login with extended timeout
    console.log('🔐 Step 2: Performing login...');
    const loginStart = Date.now();

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-02-form-filled.png',
      fullPage: true
    });

    // Click login and wait for navigation with longer timeout
    try {
      const responsePromise = page.waitForResponse('**/api/auth/login', { timeout: 25000 });
      await page.click('#admin-login-submit-btn');

      const loginResponse = await responsePromise;
      const loginTime = Date.now() - loginStart;

      console.log(`✅ Login API response: ${loginResponse.status()} in ${loginTime}ms`);

      if (loginResponse.status() === 200) {
        // Wait for navigation to complete
        await page.waitForURL('**/admin/**', { timeout: 10000 });

        await page.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-03-after-login.png',
          fullPage: true
        });

        // Step 3: Navigate to stadium overview
        console.log('🏟️ Step 3: Navigating to Stadium Overview...');
        const stadiumNavStart = Date.now();

        // Click on Stadium Overview link
        await page.click('text=Stadium Overview');
        await page.waitForURL('**/admin/stadium-overview**', { timeout: 15000 });

        // Wait for page to load
        await page.waitForLoadState('networkidle', { timeout: 20000 });

        const stadiumLoadTime = Date.now() - stadiumNavStart;
        console.log(`✅ Stadium Overview navigation: ${stadiumLoadTime}ms`);

        // Take screenshot of stadium overview
        await page.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-04-stadium-overview.png',
          fullPage: true
        });

        // Step 4: Wait for any dynamic content to load
        console.log('⏳ Step 4: Waiting for dynamic content...');
        await page.waitForTimeout(3000);

        await page.screenshot({
          path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-05-final-state.png',
          fullPage: true
        });

        // Step 5: Performance analysis
        const totalTime = Date.now() - startTime;

        console.log('\n🎯 SIMPLE PERFORMANCE ANALYSIS:');
        console.log('=' .repeat(50));
        console.log(`📊 Timing Breakdown:`);
        console.log(`   • Initial page load: ${pageLoadTime}ms`);
        console.log(`   • Login process: ${loginTime}ms`);
        console.log(`   • Stadium overview load: ${stadiumLoadTime}ms`);
        console.log(`   • Total journey: ${totalTime}ms`);

        console.log(`\n⚠️ Performance Assessment:`);
        if (stadiumLoadTime > 3000) {
          console.log(`   🔴 SLOW: Stadium overview (${stadiumLoadTime}ms) > 3 seconds`);
        } else if (stadiumLoadTime > 1500) {
          console.log(`   🟡 MEDIUM: Stadium overview (${stadiumLoadTime}ms) 1.5-3 seconds`);
        } else {
          console.log(`   🟢 FAST: Stadium overview (${stadiumLoadTime}ms) < 1.5 seconds`);
        }

        if (loginTime > 15000) {
          console.log(`   🔴 CRITICAL: Login timeout (${loginTime}ms) > 15 seconds`);
        } else if (loginTime > 5000) {
          console.log(`   🟡 SLOW: Login process (${loginTime}ms) > 5 seconds`);
        } else {
          console.log(`   🟢 GOOD: Login process (${loginTime}ms) < 5 seconds`);
        }

        // Check for specific performance issues
        const pageTitle = await page.title();
        const url = page.url();
        console.log(`\n📄 Final state:`);
        console.log(`   • Page title: ${pageTitle}`);
        console.log(`   • URL: ${url}`);
        console.log(`   • Stadium page loaded: ${url.includes('stadium-overview')}`);

        console.log('\n' + '=' .repeat(50));

        // Assertions
        expect(stadiumLoadTime).toBeLessThan(10000); // Should load within 10 seconds
        expect(url).toContain('stadium-overview');
        expect(totalTime).toBeLessThan(30000); // Total journey under 30 seconds

      } else {
        console.log(`❌ Login failed with status: ${loginResponse.status()}`);
        const errorText = await loginResponse.text();
        console.log(`Error details: ${errorText}`);
        throw new Error(`Login failed: ${loginResponse.status()}`);
      }

    } catch (error) {
      console.log(`❌ Login process failed: ${error}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\simple-error-state.png',
        fullPage: true
      });

      // Try to get any error messages from the page
      const errorElements = await page.locator('[class*="error"], [class*="alert"], .alert-danger').all();
      if (errorElements.length > 0) {
        for (const element of errorElements) {
          const errorText = await element.textContent();
          console.log(`🔍 Page error: ${errorText}`);
        }
      }

      throw error;
    }
  });
});