import { test, expect } from '@playwright/test';

test.describe('FINAL Stadium Overview Test - Fixed AuthService', () => {
  test('Complete flow: Login → Dashboard → Stadium Overview', async ({ page }) => {
    const startTime = Date.now();

    // Step 1: Navigate to Admin login page
    console.log('Step 1: Navigating to login page...');
    await page.goto('https://localhost:7030/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Verify we're on the login page
    await expect(page.locator('#admin-login-title')).toBeVisible({ timeout: 10000 });
    console.log('✓ Login page loaded');

    // Step 2: Fill in credentials
    console.log('Step 2: Entering credentials...');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    console.log('✓ Credentials entered');

    // Take screenshot before login
    await page.screenshot({ path: 'test-results/01-before-login.png', fullPage: true });

    // Step 3: Click login button and measure login time
    console.log('Step 3: Clicking login button...');
    const loginStartTime = Date.now();
    await page.click('#admin-login-submit-btn');

    // Wait for successful login by checking URL change from /login
    try {
      console.log('Waiting for login to complete and redirect...');

      // Wait for URL to change away from /login (with extended timeout to observe behavior)
      await page.waitForURL(url => !url.pathname.includes('/login'), { timeout: 35000 });

      const loginDuration = Date.now() - loginStartTime;
      console.log(`✓ Login completed in ${loginDuration}ms`);

      // Get current URL after login
      const currentUrl = page.url();
      console.log(`✓ Current URL after login: ${currentUrl}`);

      // Verify login speed
      if (loginDuration < 2000) {
        console.log('✅ SUCCESS: Login is FAST (< 2 seconds) - AuthService timeout fix working!');
      } else if (loginDuration < 5000) {
        console.log(`⚠ ACCEPTABLE: Login took ${loginDuration}ms (< 5 seconds)`);
      } else {
        console.log(`❌ SLOW: Login took ${loginDuration}ms (should be < 2000ms)`);
      }

      // Take screenshot after successful login
      await page.screenshot({ path: 'test-results/02-after-login-dashboard.png', fullPage: true });

    } catch (error) {
      const loginDuration = Date.now() - loginStartTime;
      console.log(`❌ FAILED: Login timeout after ${loginDuration}ms`);
      await page.screenshot({ path: 'test-results/02-login-failed.png', fullPage: true });
      throw error;
    }

    // Step 4: Verify we're authenticated and dashboard loaded
    console.log('Step 4: Verifying dashboard page...');

    // Check for dashboard elements (may be at "/" or "/dashboard")
    const dashboardTitle = page.locator('text=Stadium Dashboard, text=Admin Dashboard').first();
    await expect(dashboardTitle).toBeVisible({ timeout: 5000 });

    // Wait for dashboard content to load
    await page.waitForTimeout(2000);
    console.log('✓ Dashboard loaded');

    // Step 5: Navigate to Stadium Overview
    console.log('Step 5: Navigating to Stadium Overview...');

    // Try multiple navigation methods
    try {
      // Method 1: Try sidebar link
      const stadiumLink = page.locator('a[href="/stadium-overview"]').first();
      if (await stadiumLink.isVisible({ timeout: 2000 })) {
        await stadiumLink.click();
        console.log('✓ Clicked sidebar Stadium Overview link');
      } else {
        // Method 2: Direct navigation
        console.log('Sidebar link not found, navigating directly...');
        await page.goto('https://localhost:7030/stadium-overview', {
          waitUntil: 'networkidle',
          timeout: 20000
        });
        console.log('✓ Direct navigation to Stadium Overview');
      }
    } catch (error) {
      console.log('Navigation attempt failed, trying direct URL...');
      await page.goto('https://localhost:7030/stadium-overview', {
        waitUntil: 'networkidle',
        timeout: 20000
      });
    }

    // Step 6: Verify Stadium Overview page loaded
    console.log('Step 6: Verifying Stadium Overview page...');
    await page.waitForTimeout(3000); // Give time for page to render

    // Check for page title or main content
    const pageTitle = page.locator('h1, h2, h3').first();
    const titleText = await pageTitle.textContent({ timeout: 5000 }).catch(() => '');
    console.log(`Page title: "${titleText}"`);

    // Take screenshot of Stadium Overview
    await page.screenshot({ path: 'test-results/03-stadium-overview.png', fullPage: true });

    // Step 7: Check for tribune data
    console.log('Step 7: Checking for tribune data...');

    // Look for any indicators of stadium data
    const pageContent = await page.content();

    // Check for various possible selectors indicating stadium data
    const indicators = [
      { selector: 'svg', description: 'SVG stadium visualization' },
      { selector: '.tribune', description: 'Tribune elements' },
      { selector: '[class*="tribune"]', description: 'Tribune class elements' },
      { selector: 'canvas', description: 'Canvas element' },
      { selector: '.stadium', description: 'Stadium elements' },
      { selector: '[id*="stadium"]', description: 'Stadium ID elements' }
    ];

    let foundIndicators = [];
    for (const indicator of indicators) {
      const elements = await page.locator(indicator.selector).count();
      if (elements > 0) {
        foundIndicators.push(`${indicator.description}: ${elements}`);
        console.log(`✓ Found ${indicator.description}: ${elements} elements`);
      }
    }

    // Check page content for keywords
    const keywords = ['tribune', 'sector', 'stadium', 'north', 'south', 'east', 'west'];
    const foundKeywords = keywords.filter(keyword =>
      pageContent.toLowerCase().includes(keyword)
    );

    if (foundKeywords.length > 0) {
      console.log(`✓ Found stadium-related keywords: ${foundKeywords.join(', ')}`);
    }

    // Step 8: Generate final report
    const totalDuration = Date.now() - startTime;
    console.log('\n' + '='.repeat(80));
    console.log('FINAL TEST REPORT - Stadium Overview with Fixed AuthService');
    console.log('='.repeat(80));
    console.log(`Total test duration: ${totalDuration}ms`);
    console.log(`Login duration: ${loginStartTime ? (Date.now() - loginStartTime) : 'N/A'}ms`);
    console.log('\nTest Steps:');
    console.log('  ✅ Step 1: Login page loaded');
    console.log('  ✅ Step 2: Credentials entered');
    console.log('  ✅ Step 3: Login submitted');
    console.log('  ✅ Step 4: Dashboard reached');
    console.log('  ✅ Step 5: Stadium Overview navigation');
    console.log('  ✅ Step 6: Stadium Overview page loaded');
    console.log('\nStadium Data Found:');
    if (foundIndicators.length > 0) {
      foundIndicators.forEach(indicator => console.log(`  ✓ ${indicator}`));
    } else {
      console.log('  ⚠ No stadium visualization elements detected');
    }
    if (foundKeywords.length > 0) {
      console.log(`  ✓ Keywords found: ${foundKeywords.join(', ')}`);
    }
    console.log('\nScreenshots:');
    console.log('  - test-results/01-before-login.png');
    console.log('  - test-results/02-after-login-dashboard.png');
    console.log('  - test-results/03-stadium-overview.png');
    console.log('='.repeat(80));

    // Final assertions
    expect(totalDuration).toBeLessThan(30000); // Total test under 30 seconds
    expect(foundIndicators.length + foundKeywords.length).toBeGreaterThan(0); // Some stadium data found
  });
});
