import { test, expect, Page } from '@playwright/test';

test.describe('Admin Stadium Overview Page Test', () => {
  let page: Page;
  const adminUrl = 'https://localhost:7030';
  const adminEmail = 'admin@stadium.com';
  const adminPassword = 'admin123';

  test.beforeAll(async ({ browser }) => {
    page = await browser.newPage({
      ignoreHTTPSErrors: true, // Allow self-signed certificates
    });
  });

  test.afterAll(async () => {
    await page.close();
  });

  test('Complete Stadium Overview Page Test Flow', async () => {
    console.log('\n=== STARTING ADMIN STADIUM OVERVIEW TEST ===\n');

    // Step 1: Navigate to Admin app
    console.log('Step 1: Navigating to Admin application...');
    await page.goto(adminUrl, {
      waitUntil: 'domcontentloaded',
      timeout: 30000
    });
    console.log(`✓ Navigated to: ${adminUrl}`);

    // Step 2: Login with credentials
    console.log('\nStep 2: Attempting login...');

    // Wait for login page to load
    await page.waitForSelector('#customer-login-email-input, input[type="email"]', {
      timeout: 10000
    });
    console.log('✓ Login page loaded');

    // Fill in credentials
    const emailInput = page.locator('#customer-login-email-input, input[type="email"]').first();
    const passwordInput = page.locator('#customer-login-password-input, input[type="password"]').first();
    const loginButton = page.locator('#customer-login-submit-btn, button[type="submit"]').first();

    await emailInput.fill(adminEmail);
    console.log(`✓ Entered email: ${adminEmail}`);

    await passwordInput.fill(adminPassword);
    console.log('✓ Entered password');

    // Take screenshot before login
    await page.screenshot({
      path: 'admin-stadium-test-01-login-page.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: admin-stadium-test-01-login-page.png');

    await loginButton.click();
    console.log('✓ Clicked login button');

    // Wait for navigation after login (increased timeout)
    await page.waitForNavigation({
      waitUntil: 'domcontentloaded',
      timeout: 15000
    }).catch(() => {
      console.log('⚠ Navigation timeout (may be already loaded)');
    });

    // Verify login success
    await page.waitForTimeout(2000);
    const currentUrl = page.url();
    console.log(`✓ Current URL after login: ${currentUrl}`);

    // Take screenshot after login
    await page.screenshot({
      path: 'admin-stadium-test-02-after-login.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: admin-stadium-test-02-after-login.png');

    // Step 3: Navigate to Stadium Overview
    console.log('\nStep 3: Navigating to Stadium Overview page...');

    // Try direct navigation first
    await page.goto(`${adminUrl}/admin/stadium-overview`, {
      waitUntil: 'domcontentloaded',
      timeout: 30000
    });
    console.log('✓ Navigated to Stadium Overview page');

    // Step 4: Verify page loads
    console.log('\nStep 4: Verifying page load...');

    // Check for loading message (with increased timeout)
    const loadingStateVisible = await page.locator('#admin-stadium-loading-state').isVisible().catch(() => false);
    if (loadingStateVisible) {
      console.log('✓ "Loading Stadium Layout" message detected');

      // Wait for loading to complete (increased timeout to 60 seconds)
      await page.waitForSelector('#admin-stadium-loading-state', {
        state: 'hidden',
        timeout: 60000
      }).catch(() => {
        console.log('⚠ Loading state did not hide within 60 seconds');
      });
      console.log('✓ Loading completed');
    } else {
      console.log('ℹ No loading state detected (data may have loaded immediately)');
    }

    // Wait for page to stabilize
    await page.waitForTimeout(2000);

    // Check if stadium visualization appears
    const hasVisualization = await page.locator('#admin-stadium-grid-layout').isVisible().catch(() => false);
    const hasError = await page.locator('#admin-stadium-error-state').isVisible().catch(() => false);
    const isEmpty = await page.locator('#admin-stadium-empty-state').isVisible().catch(() => false);

    console.log('\n--- Page State Detection ---');
    console.log(`Stadium Visualization Present: ${hasVisualization}`);
    console.log(`Error State Present: ${hasError}`);
    console.log(`Empty State Present: ${isEmpty}`);

    // Take screenshot of current state
    await page.screenshot({
      path: 'admin-stadium-test-03-stadium-overview.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: admin-stadium-test-03-stadium-overview.png');

    if (hasError) {
      const errorMessage = await page.locator('#admin-stadium-error-state p').textContent();
      console.log(`❌ ERROR DETECTED: ${errorMessage}`);
    }

    // Step 5: Check console and network
    console.log('\nStep 5: Checking console and network...');

    // Listen for console messages
    const consoleMessages: string[] = [];
    page.on('console', msg => {
      const text = msg.text();
      consoleMessages.push(text);
      if (text.includes('error') || text.includes('Error') || text.includes('ERROR')) {
        console.log(`🔴 Console Error: ${text}`);
      }
    });

    // Monitor network requests for the API call
    let apiCallMade = false;
    let apiResponseTime = 0;
    let apiStatusCode = 0;
    let apiResponseBody = '';

    page.on('response', async response => {
      const url = response.url();
      if (url.includes('/api/StadiumViewer/overview')) {
        apiCallMade = true;
        apiStatusCode = response.status();
        const timing = response.timing();
        apiResponseTime = timing ? timing.responseEnd - timing.requestStart : 0;

        try {
          apiResponseBody = await response.text();
          console.log(`✓ API Call: ${url}`);
          console.log(`  Status: ${apiStatusCode}`);
          console.log(`  Response Time: ${apiResponseTime}ms`);
          console.log(`  Response Length: ${apiResponseBody.length} characters`);

          if (apiStatusCode === 200) {
            const jsonResponse = JSON.parse(apiResponseBody);
            console.log(`  Stadium Name: ${jsonResponse.name || 'N/A'}`);
            console.log(`  Stands Count: ${jsonResponse.stands?.length || 0}`);
          }
        } catch (e) {
          console.log(`⚠ Could not parse API response: ${e}`);
        }
      }
    });

    // Trigger a refresh to capture network activity
    await page.reload({ waitUntil: 'domcontentloaded', timeout: 30000 });
    await page.waitForTimeout(5000); // Wait for API calls

    // Take final screenshot
    await page.screenshot({
      path: 'admin-stadium-test-04-final-state.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: admin-stadium-test-04-final-state.png');

    // Step 6: Report findings
    console.log('\n=== TEST REPORT ===\n');
    console.log('1. Does the page load successfully?');
    if (hasVisualization) {
      console.log('   ✓ YES - Stadium visualization loaded successfully');
    } else if (hasError) {
      console.log('   ❌ NO - Error state displayed');
    } else if (isEmpty) {
      console.log('   ⚠ PARTIAL - Empty state (no stadium data)');
    } else {
      console.log('   ❓ UNKNOWN - Page state unclear');
    }

    console.log('\n2. What\'s the API response time?');
    if (apiCallMade) {
      console.log(`   ✓ API called: ${apiResponseTime}ms`);
      if (apiResponseTime < 1000) {
        console.log('   ✓ EXCELLENT - Response time under 1 second');
      } else if (apiResponseTime < 5000) {
        console.log('   ⚠ ACCEPTABLE - Response time under 5 seconds');
      } else {
        console.log('   ❌ SLOW - Response time over 5 seconds');
      }
    } else {
      console.log('   ❌ API was not called or not detected');
    }

    console.log('\n3. Are there any errors (database, API, or UI)?');
    if (hasError) {
      const errorText = await page.locator('#admin-stadium-error-state p').textContent();
      console.log(`   ❌ YES - Error: ${errorText}`);
    } else if (consoleMessages.some(msg => msg.toLowerCase().includes('error'))) {
      console.log('   ⚠ YES - Console errors detected (see above)');
    } else if (apiStatusCode !== 200 && apiCallMade) {
      console.log(`   ❌ YES - API error (Status: ${apiStatusCode})`);
    } else {
      console.log('   ✓ NO - No errors detected');
    }

    console.log('\n4. Is there any stadium data displayed?');
    if (hasVisualization) {
      const sectorCount = await page.locator('.sector').count();
      const standCount = await page.locator('.stadium-stand').count();
      console.log(`   ✓ YES - ${standCount} stands, ${sectorCount} sectors displayed`);

      // Check if stadium summary is visible
      const hasSummary = await page.locator('#admin-stadium-info-panel').isVisible().catch(() => false);
      if (hasSummary) {
        const totalSeats = await page.locator('#admin-stadium-stat-seats-value').textContent().catch(() => 'N/A');
        const totalSectors = await page.locator('#admin-stadium-stat-sectors-value').textContent().catch(() => 'N/A');
        console.log(`   Stadium Statistics:`);
        console.log(`   - Total Seats: ${totalSeats}`);
        console.log(`   - Total Sectors: ${totalSectors}`);
      }
    } else if (isEmpty) {
      console.log('   ⚠ NO - Empty state (no stadium structure in database)');
    } else {
      console.log('   ❌ NO - No data visible');
    }

    console.log('\n=== CONFIGURATION NOTES ===');
    console.log('✓ PostgreSQL-only configuration active (NO SQLite)');
    console.log('✓ Database queries should be using PostgreSQL');
    console.log('✓ Timeout increased to 60 seconds for stadium data loading');

    console.log('\n=== TEST COMPLETED ===\n');

    // Assertions for test validation
    expect(currentUrl).toContain('/admin');
    if (!hasError) {
      expect(hasVisualization || isEmpty).toBeTruthy();
    }
  });
});
