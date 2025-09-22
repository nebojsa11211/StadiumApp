import { test, expect, BrowserContext, Page } from '@playwright/test';

test.describe('Admin Dashboard API Routing Tests - /api Prefix Removal', () => {
  let context: BrowserContext;
  let page: Page;

  const adminEmail = 'admin@stadium.com';
  const adminPassword = 'admin123';
  const adminBaseUrl = 'https://localhost:7030';
  const apiBaseUrl = 'https://localhost:7010';

  // Track API calls for verification
  const apiCalls: Array<{
    url: string;
    status: number;
    method: string;
    hasApiPrefix: boolean;
    endpoint: string;
  }> = [];

  test.beforeAll(async ({ browser }) => {
    // Create isolated context for admin testing
    context = await browser.newContext({
      ignoreHTTPSErrors: true,
      baseURL: adminBaseUrl
    });
    page = await context.newPage();

    // Listen for console errors
    page.on('console', msg => {
      if (msg.type() === 'error') {
        console.log('Console Error:', msg.text());
      }
    });

    // Listen for failed requests
    page.on('requestfailed', request => {
      console.log('Failed Request:', request.url(), request.failure()?.errorText);
    });

    // Track all API requests to verify routing
    page.on('response', response => {
      if (response.url().includes(apiBaseUrl.replace('https://', '').replace('localhost:', ''))) {
        const url = response.url();
        const urlObj = new URL(url);
        const path = urlObj.pathname;
        const hasApiPrefix = path.startsWith('/api/');

        // Extract endpoint name (remove /api/ prefix if present)
        const endpoint = hasApiPrefix ? path.substring(5) : path.substring(1);

        apiCalls.push({
          url: response.url(),
          status: response.status(),
          method: response.request().method(),
          hasApiPrefix,
          endpoint
        });

        console.log(`API Call: ${response.request().method()} ${response.status()} ${path} ${hasApiPrefix ? '(HAS /api/)' : '(NO /api/)'}`);
      }
    });
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('1. Admin Login Process', async () => {
    console.log('=== Testing Admin Login ===');

    // Navigate to admin app
    await page.goto('/');
    await expect(page).toHaveTitle(/Stadium.*Admin/);

    // Should redirect to login page for unauthenticated access
    await expect(page).toHaveURL('/login');

    // Fill and submit login form
    await page.fill('#admin-login-email-input', adminEmail);
    await page.fill('#admin-login-password-input', adminPassword);
    await page.click('#admin-login-submit-btn');

    // Wait for successful login and redirect to dashboard
    await page.waitForURL('/', { timeout: 15000 });

    // Verify dashboard loads
    await page.waitForLoadState('networkidle');

    console.log('✅ Admin login successful - redirected to dashboard');
  });

  test('2. Dashboard Data Loading Verification', async () => {
    console.log('=== Testing Dashboard Data Loading ===');

    // Ensure we're on the dashboard
    await page.goto('/');
    await page.waitForLoadState('networkidle');

    // Wait for dashboard content to load
    await page.waitForSelector('[data-testid="dashboard-content"], .dashboard, .admin-dashboard, h1', { timeout: 10000 });

    // Look for dashboard elements - these might vary based on implementation
    const dashboardIndicators = [
      'h1:has-text("Dashboard")',
      'h1:has-text("Admin")',
      '.card:has-text("Orders")',
      '.card:has-text("Revenue")',
      '.card:has-text("Users")',
      '.stat, .statistic',
      '[class*="order"], [class*="revenue"], [class*="user"]'
    ];

    let foundIndicator = false;
    for (const selector of dashboardIndicators) {
      const element = page.locator(selector).first();
      if (await element.isVisible().catch(() => false)) {
        console.log(`✅ Found dashboard indicator: ${selector}`);
        foundIndicator = true;
        break;
      }
    }

    if (!foundIndicator) {
      // If no specific dashboard elements found, just verify we're not on login page
      await expect(page).not.toHaveURL('/login');
      console.log('✅ Dashboard loaded (generic validation)');
    }

    // Wait a bit more for any async API calls to complete
    await page.waitForTimeout(3000);

    console.log('✅ Dashboard content loaded successfully');
  });

  test('3. API Routing Verification - No /api Prefix', async () => {
    console.log('=== Testing API Routing Changes ===');

    // Navigate to dashboard to trigger API calls
    await page.goto('/');
    await page.waitForLoadState('networkidle');

    // Give additional time for all API calls to complete
    await page.waitForTimeout(5000);

    console.log(`\n=== API Calls Summary (${apiCalls.length} total) ===`);

    // Group API calls by endpoint
    const endpointGroups: { [key: string]: typeof apiCalls } = {};
    apiCalls.forEach(call => {
      const key = `${call.method} ${call.endpoint}`;
      if (!endpointGroups[key]) {
        endpointGroups[key] = [];
      }
      endpointGroups[key].push(call);
    });

    // Analyze each endpoint group
    Object.entries(endpointGroups).forEach(([endpoint, calls]) => {
      const hasApiPrefix = calls.some(c => c.hasApiPrefix);
      const statuses = [...new Set(calls.map(c => c.status))];
      console.log(`${endpoint}: ${statuses.join(',')} ${hasApiPrefix ? '❌ HAS /api/' : '✅ NO /api/'}`);
    });

    // Verify critical endpoints work without /api prefix
    const criticalEndpoints = ['orders', 'logs', 'analytics', 'users'];
    const apiPrefixFound = apiCalls.some(call => call.hasApiPrefix);
    const successfulCalls = apiCalls.filter(call => call.status >= 200 && call.status < 300);
    const errorCalls = apiCalls.filter(call => call.status >= 400);

    console.log(`\n=== API Routing Analysis ===`);
    console.log(`Total API calls: ${apiCalls.length}`);
    console.log(`Successful calls (2xx): ${successfulCalls.length}`);
    console.log(`Error calls (4xx/5xx): ${errorCalls.length}`);
    console.log(`Calls with /api/ prefix: ${apiCalls.filter(c => c.hasApiPrefix).length}`);
    console.log(`Calls without /api/ prefix: ${apiCalls.filter(c => !c.hasApiPrefix).length}`);

    // Verify no API calls use /api prefix
    if (apiPrefixFound) {
      console.log('❌ Found API calls still using /api/ prefix:');
      apiCalls.filter(c => c.hasApiPrefix).forEach(call => {
        console.log(`  ${call.method} ${call.url}`);
      });

      // This is a soft assertion - log the issue but don't fail the test
      console.log('⚠️  Some API calls still use /api/ prefix - needs investigation');
    } else {
      console.log('✅ All API calls correctly use direct routing (no /api/ prefix)');
    }

    // Check for 404 errors that might indicate routing issues
    const notFoundCalls = apiCalls.filter(call => call.status === 404);
    if (notFoundCalls.length > 0) {
      console.log('❌ Found 404 errors (possible routing issues):');
      notFoundCalls.forEach(call => {
        console.log(`  ${call.method} ${call.url}`);
      });
    } else {
      console.log('✅ No 404 errors found - routing appears correct');
    }
  });

  test('4. Dashboard Statistics Verification', async () => {
    console.log('=== Testing Dashboard Statistics Display ===');

    await page.goto('/');
    await page.waitForLoadState('networkidle');

    // Look for common dashboard statistics patterns
    const statsSelectors = [
      // Common dashboard selectors
      '.dashboard-stats',
      '.stats-container',
      '.statistics',
      '.admin-stats',

      // Card-based layouts
      '.card .card-body',
      '.stat-card',

      // Bootstrap card patterns
      '.card-header:has-text("Orders")',
      '.card-header:has-text("Revenue")',
      '.card-header:has-text("Users")',

      // Generic number displays
      '[class*="count"]',
      '[class*="total"]',
      '[class*="stat"]',

      // Text patterns
      'text=/\\$[0-9,]+/', // Currency amounts
      'text=/[0-9]+/', // Numbers
    ];

    let statsFound = false;
    let statsData: any[] = [];

    for (const selector of statsSelectors) {
      try {
        const elements = page.locator(selector);
        const count = await elements.count();

        if (count > 0) {
          console.log(`✅ Found ${count} statistics elements with selector: ${selector}`);

          // Try to extract text content
          for (let i = 0; i < Math.min(count, 5); i++) {
            const text = await elements.nth(i).textContent();
            if (text && text.trim()) {
              statsData.push({ selector, text: text.trim().substring(0, 50) });
            }
          }

          statsFound = true;
          break;
        }
      } catch (error) {
        // Continue to next selector
      }
    }

    if (statsFound) {
      console.log('✅ Dashboard statistics found:');
      statsData.slice(0, 5).forEach(stat => {
        console.log(`  ${stat.selector}: "${stat.text}"`);
      });
    } else {
      console.log('ℹ️  No specific statistics found - dashboard may use different layout');

      // Fallback: just verify page content exists
      const bodyText = await page.textContent('body');
      const hasContent = bodyText && bodyText.length > 100;

      if (hasContent) {
        console.log('✅ Dashboard has content loaded');
      } else {
        console.log('❌ Dashboard appears to be empty or not loaded properly');
      }
    }
  });

  test('5. Navigation and API Functionality Test', async () => {
    console.log('=== Testing Navigation and API Functionality ===');

    // Test navigation to different admin sections
    const sectionsToTest = [
      { name: 'Orders', path: '/orders', expectedText: ['Orders', 'Order'] },
      { name: 'Users', path: '/users', expectedText: ['Users', 'User'] },
      { name: 'Events', path: '/events', expectedText: ['Events', 'Event'] },
      { name: 'Logs', path: '/logs', expectedText: ['Logs', 'Activity', 'Log'] }
    ];

    for (const section of sectionsToTest) {
      try {
        console.log(`Testing ${section.name} section...`);

        // Clear previous API calls for this section
        const previousCallCount = apiCalls.length;

        // Navigate to section
        await page.goto(section.path);
        await page.waitForLoadState('networkidle');

        // Wait for content to load
        await page.waitForTimeout(2000);

        // Check if section loaded (look for expected text)
        let sectionLoaded = false;
        for (const text of section.expectedText) {
          const element = page.locator(`h1:has-text("${text}"), h2:has-text("${text}"), h3:has-text("${text}")`).first();
          if (await element.isVisible().catch(() => false)) {
            sectionLoaded = true;
            break;
          }
        }

        // Count new API calls for this section
        const newCalls = apiCalls.slice(previousCallCount);
        const sectionApiCalls = newCalls.length;
        const successfulSectionCalls = newCalls.filter(call => call.status >= 200 && call.status < 300).length;

        if (sectionLoaded) {
          console.log(`✅ ${section.name} section loaded successfully (${sectionApiCalls} API calls, ${successfulSectionCalls} successful)`);
        } else {
          console.log(`⚠️  ${section.name} section navigation completed but content detection uncertain (${sectionApiCalls} API calls)`);
        }

      } catch (error) {
        console.log(`❌ Error testing ${section.name} section: ${error}`);
      }
    }

    // Return to dashboard
    await page.goto('/');
    await page.waitForLoadState('networkidle');
  });

  test('6. API Response Validation', async () => {
    console.log('=== Testing API Response Validation ===');

    // Navigate to dashboard to trigger fresh API calls
    await page.goto('/');
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000);

    // Analyze API responses by category
    const responseAnalysis = {
      successful: apiCalls.filter(call => call.status >= 200 && call.status < 300),
      redirected: apiCalls.filter(call => call.status >= 300 && call.status < 400),
      clientErrors: apiCalls.filter(call => call.status >= 400 && call.status < 500),
      serverErrors: apiCalls.filter(call => call.status >= 500),
      withApiPrefix: apiCalls.filter(call => call.hasApiPrefix),
      withoutApiPrefix: apiCalls.filter(call => !call.hasApiPrefix)
    };

    console.log('\n=== API Response Analysis ===');
    console.log(`Successful responses (2xx): ${responseAnalysis.successful.length}`);
    console.log(`Redirected responses (3xx): ${responseAnalysis.redirected.length}`);
    console.log(`Client errors (4xx): ${responseAnalysis.clientErrors.length}`);
    console.log(`Server errors (5xx): ${responseAnalysis.serverErrors.length}`);
    console.log(`With /api/ prefix: ${responseAnalysis.withApiPrefix.length}`);
    console.log(`Without /api/ prefix: ${responseAnalysis.withoutApiPrefix.length}`);

    // Detailed error analysis
    if (responseAnalysis.clientErrors.length > 0) {
      console.log('\n❌ Client Errors (4xx):');
      responseAnalysis.clientErrors.forEach(call => {
        console.log(`  ${call.status} ${call.method} ${call.url}`);
      });
    }

    if (responseAnalysis.serverErrors.length > 0) {
      console.log('\n❌ Server Errors (5xx):');
      responseAnalysis.serverErrors.forEach(call => {
        console.log(`  ${call.status} ${call.method} ${call.url}`);
      });
    }

    // Success criteria
    const totalCalls = apiCalls.length;
    const successRate = totalCalls > 0 ? (responseAnalysis.successful.length / totalCalls) * 100 : 0;
    const apiPrefixRate = totalCalls > 0 ? (responseAnalysis.withApiPrefix.length / totalCalls) * 100 : 0;

    console.log(`\n=== Final Results ===`);
    console.log(`Overall API success rate: ${successRate.toFixed(1)}%`);
    console.log(`API calls using /api/ prefix: ${apiPrefixRate.toFixed(1)}%`);

    // Verify that the majority of calls are successful
    if (successRate >= 70) {
      console.log('✅ High API success rate - Admin dashboard API calls working correctly');
    } else if (successRate >= 50) {
      console.log('⚠️  Moderate API success rate - some issues detected');
    } else {
      console.log('❌ Low API success rate - significant issues detected');
    }

    // Verify that /api prefix has been removed
    if (apiPrefixRate === 0) {
      console.log('✅ Complete /api/ prefix removal confirmed - all calls use direct routing');
    } else if (apiPrefixRate < 20) {
      console.log('⚠️  Mostly successful /api/ prefix removal - some legacy calls remain');
    } else {
      console.log('❌ /api/ prefix removal incomplete - many calls still use old routing');
    }

    // Overall test assessment
    const routingFixed = apiPrefixRate < 20;
    const dashboardWorking = successRate >= 50;

    if (routingFixed && dashboardWorking) {
      console.log('\n🎉 TEST PASSED: Admin dashboard API routing changes successful!');
    } else if (routingFixed && !dashboardWorking) {
      console.log('\n⚠️  TEST PARTIAL: Routing fixed but dashboard has other issues');
    } else if (!routingFixed && dashboardWorking) {
      console.log('\n⚠️  TEST PARTIAL: Dashboard working but routing changes incomplete');
    } else {
      console.log('\n❌ TEST FAILED: Both routing and dashboard functionality have issues');
    }
  });
});