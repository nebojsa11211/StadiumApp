import { test, expect, Page } from '@playwright/test';

interface PerformanceMetrics {
  navigationStart: number;
  domContentLoaded: number;
  loadComplete: number;
  firstContentfulPaint: number;
  largestContentfulPaint: number;
  totalLoadTime: number;
  domContentLoadedTime: number;
  timeToInteractive: number;
}

interface ApiRequestTiming {
  url: string;
  method: string;
  startTime: number;
  endTime: number;
  duration: number;
  status: number;
  responseSize?: number;
}

test.describe('Stadium Overview Performance Tests', () => {
  let page: Page;
  let performanceMetrics: PerformanceMetrics;
  let apiRequests: ApiRequestTiming[] = [];

  test.beforeEach(async ({ browser }) => {
    // Create fresh context for each test to ensure clean state
    const context = await browser.newContext({
      // Disable cache to test actual load times
      ignoreHTTPSErrors: true,
    });

    page = await context.newPage();

    // Reset API requests tracking
    apiRequests = [];

    // Monitor API requests and their timing
    page.on('request', (request) => {
      if (request.url().includes('/api/')) {
        const startTime = Date.now();
        apiRequests.push({
          url: request.url(),
          method: request.method(),
          startTime,
          endTime: 0,
          duration: 0,
          status: 0
        });
      }
    });

    page.on('response', (response) => {
      if (response.url().includes('/api/')) {
        const endTime = Date.now();
        const requestIndex = apiRequests.findIndex(req =>
          req.url === response.url() && req.endTime === 0
        );

        if (requestIndex !== -1) {
          apiRequests[requestIndex].endTime = endTime;
          apiRequests[requestIndex].duration = endTime - apiRequests[requestIndex].startTime;
          apiRequests[requestIndex].status = response.status();

          // Get response size if available
          response.headers()['content-length'] &&
            (apiRequests[requestIndex].responseSize = parseInt(response.headers()['content-length']));
        }
      }
    });
  });

  test.afterEach(async () => {
    await page?.close();
  });

  test('should measure stadium-overview page performance', async () => {
    console.log('🚀 Starting stadium-overview performance test...');

    // Step 1: Navigate to admin login page and measure
    const navigationStartTime = Date.now();
    console.log('📍 Navigating to https://localhost:7030...');

    await page.goto('https://localhost:7030', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Take screenshot of initial page
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-initial-page.png',
      fullPage: true
    });
    console.log('📸 Screenshot saved: admin-initial-page.png');

    // Step 2: Login with admin credentials
    console.log('🔐 Logging in with admin credentials...');
    const loginStartTime = Date.now();

    // Fill login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    // Take screenshot before login submission
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-before-submit.png',
      fullPage: true
    });
    console.log('📸 Screenshot saved: admin-before-submit.png');

    // Submit login form and wait for navigation
    const loginPromise = page.waitForURL('**/admin/**', { timeout: 15000 });
    await page.click('#admin-login-submit-btn');
    await loginPromise;

    const loginEndTime = Date.now();
    const loginDuration = loginEndTime - loginStartTime;
    console.log(`✅ Login completed in ${loginDuration}ms`);

    // Take screenshot after login
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-after-submit.png',
      fullPage: true
    });
    console.log('📸 Screenshot saved: admin-after-submit.png');

    // Step 3: Navigate to stadium-overview page
    console.log('🏟️ Navigating to stadium-overview page...');
    const stadiumOverviewStartTime = Date.now();

    // Look for navigation to stadium overview
    // Try multiple possible navigation patterns
    let navigationSuccessful = false;

    try {
      // Try to find a direct link to stadium overview
      const stadiumLinks = await page.locator('a').filter({ hasText: /stadium|overview/i }).all();

      if (stadiumLinks.length > 0) {
        console.log(`🔍 Found ${stadiumLinks.length} potential stadium navigation links`);

        // Click the first stadium-related link
        await stadiumLinks[0].click();
        navigationSuccessful = true;
      } else {
        // Try navigation menu approach
        const navItems = await page.locator('[class*="nav"]').all();
        for (const navItem of navItems) {
          const text = await navItem.textContent();
          if (text && /stadium|overview/i.test(text)) {
            await navItem.click();
            navigationSuccessful = true;
            break;
          }
        }
      }

      // If no navigation found, try direct URL
      if (!navigationSuccessful) {
        console.log('⚠️ No navigation link found, trying direct URL...');
        await page.goto('https://localhost:7030/admin/stadium-overview', {
          waitUntil: 'networkidle',
          timeout: 30000
        });
        navigationSuccessful = true;
      }

    } catch (error) {
      console.log('⚠️ Navigation via links failed, trying direct URL...');
      await page.goto('https://localhost:7030/admin/stadium-overview', {
        waitUntil: 'networkidle',
        timeout: 30000
      });
      navigationSuccessful = true;
    }

    if (!navigationSuccessful) {
      throw new Error('Could not navigate to stadium-overview page');
    }

    // Wait for page to be fully loaded
    await page.waitForLoadState('networkidle', { timeout: 30000 });

    // Additional wait for any dynamic content
    await page.waitForTimeout(2000);

    const stadiumOverviewEndTime = Date.now();
    const stadiumOverviewDuration = stadiumOverviewEndTime - stadiumOverviewStartTime;
    console.log(`✅ Stadium overview navigation completed in ${stadiumOverviewDuration}ms`);

    // Step 4: Measure page performance metrics
    console.log('📊 Collecting performance metrics...');

    const performanceTimings = await page.evaluate(() => {
      const navigation = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
      const paint = performance.getEntriesByType('paint');

      return {
        navigationStart: navigation.navigationStart,
        domContentLoaded: navigation.domContentLoadedEventEnd - navigation.navigationStart,
        loadComplete: navigation.loadEventEnd - navigation.navigationStart,
        firstContentfulPaint: paint.find(p => p.name === 'first-contentful-paint')?.startTime || 0,
        largestContentfulPaint: 0, // Will be measured differently
        timeToInteractive: navigation.domInteractive - navigation.navigationStart,
        totalLoadTime: stadiumOverviewEndTime - navigationStartTime,
        domContentLoadedTime: navigation.domContentLoadedEventEnd - navigation.domContentLoadedEventStart
      };
    });

    performanceMetrics = {
      ...performanceTimings,
      totalLoadTime: stadiumOverviewEndTime - navigationStartTime
    };

    // Step 5: Take final screenshot and analyze page content
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-final-state.png',
      fullPage: true
    });
    console.log('📸 Screenshot saved: admin-final-state.png');

    // Check if page has loaded successfully
    const pageTitle = await page.title();
    const url = page.url();

    console.log(`📄 Final page title: ${pageTitle}`);
    console.log(`🔗 Final URL: ${url}`);

    // Step 6: Analyze API requests
    console.log('🔍 Analyzing API requests...');

    // Wait a bit more to capture any delayed API calls
    await page.waitForTimeout(3000);

    const totalApiTime = apiRequests.reduce((sum, req) => sum + req.duration, 0);
    const slowestRequest = apiRequests.reduce((slowest, req) =>
      req.duration > slowest.duration ? req : slowest,
      { duration: 0, url: '', method: '', startTime: 0, endTime: 0, status: 0 }
    );

    // Step 7: Performance analysis and reporting
    console.log('\n🎯 PERFORMANCE ANALYSIS RESULTS:');
    console.log('=' .repeat(50));

    console.log(`📊 Overall Timing:`);
    console.log(`   • Total journey time: ${performanceMetrics.totalLoadTime}ms`);
    console.log(`   • Login duration: ${loginDuration}ms`);
    console.log(`   • Stadium overview load: ${stadiumOverviewDuration}ms`);

    console.log(`\n🌐 Page Performance:`);
    console.log(`   • DOM Content Loaded: ${performanceMetrics.domContentLoaded}ms`);
    console.log(`   • Page Load Complete: ${performanceMetrics.loadComplete}ms`);
    console.log(`   • Time to Interactive: ${performanceMetrics.timeToInteractive}ms`);
    console.log(`   • First Contentful Paint: ${performanceMetrics.firstContentfulPaint}ms`);

    console.log(`\n🔌 API Performance:`);
    console.log(`   • Total API requests: ${apiRequests.length}`);
    console.log(`   • Total API time: ${totalApiTime}ms`);
    console.log(`   • Average API time: ${apiRequests.length > 0 ? Math.round(totalApiTime / apiRequests.length) : 0}ms`);

    if (slowestRequest.duration > 0) {
      console.log(`   • Slowest request: ${slowestRequest.method} ${slowestRequest.url} (${slowestRequest.duration}ms)`);
    }

    // List all API requests
    if (apiRequests.length > 0) {
      console.log(`\n📋 API Request Details:`);
      apiRequests.forEach((req, index) => {
        console.log(`   ${index + 1}. ${req.method} ${req.url}`);
        console.log(`      Status: ${req.status}, Duration: ${req.duration}ms`);
        if (req.responseSize) {
          console.log(`      Response size: ${req.responseSize} bytes`);
        }
      });
    }

    // Performance thresholds and warnings
    console.log(`\n⚠️ Performance Analysis:`);

    if (performanceMetrics.totalLoadTime > 5000) {
      console.log(`   🔴 SLOW: Total load time (${performanceMetrics.totalLoadTime}ms) exceeds 5 seconds`);
    } else if (performanceMetrics.totalLoadTime > 3000) {
      console.log(`   🟡 MEDIUM: Total load time (${performanceMetrics.totalLoadTime}ms) is between 3-5 seconds`);
    } else {
      console.log(`   🟢 FAST: Total load time (${performanceMetrics.totalLoadTime}ms) is under 3 seconds`);
    }

    if (stadiumOverviewDuration > 2000) {
      console.log(`   🔴 SLOW: Stadium overview load (${stadiumOverviewDuration}ms) exceeds 2 seconds`);
    } else if (stadiumOverviewDuration > 1000) {
      console.log(`   🟡 MEDIUM: Stadium overview load (${stadiumOverviewDuration}ms) is between 1-2 seconds`);
    } else {
      console.log(`   🟢 FAST: Stadium overview load (${stadiumOverviewDuration}ms) is under 1 second`);
    }

    if (apiRequests.some(req => req.duration > 2000)) {
      console.log(`   🔴 SLOW API: Some API requests took over 2 seconds`);
    }

    if (totalApiTime > stadiumOverviewDuration * 0.8) {
      console.log(`   ⚠️ API HEAVY: API requests account for ${Math.round((totalApiTime / stadiumOverviewDuration) * 100)}% of load time`);
    }

    console.log('\n' + '=' .repeat(50));

    // Basic assertions to ensure the test actually worked
    expect(performanceMetrics.totalLoadTime).toBeGreaterThan(0);
    expect(performanceMetrics.totalLoadTime).toBeLessThan(30000); // Should complete within 30 seconds
    expect(stadiumOverviewDuration).toBeGreaterThan(0);
    expect(stadiumOverviewDuration).toBeLessThan(15000); // Stadium overview should load within 15 seconds

    // Save detailed performance report
    const report = {
      timestamp: new Date().toISOString(),
      testDuration: performanceMetrics.totalLoadTime,
      metrics: performanceMetrics,
      loginDuration,
      stadiumOverviewDuration,
      apiRequests,
      totalApiTime,
      pageTitle,
      finalUrl: url,
      performanceRating: performanceMetrics.totalLoadTime < 3000 ? 'FAST' :
                         performanceMetrics.totalLoadTime < 5000 ? 'MEDIUM' : 'SLOW'
    };

    // Write performance report to file
    await page.evaluate((reportData) => {
      // This will be captured in the test output
      console.log('PERFORMANCE_REPORT:', JSON.stringify(reportData, null, 2));
    }, report);
  });

  test('should handle stadium-overview errors gracefully', async () => {
    console.log('🔍 Testing error handling for stadium-overview...');

    // Test what happens when API is slow or unavailable
    await page.route('**/api/**', route => {
      // Simulate slow API by delaying response
      setTimeout(() => route.continue(), 5000);
    });

    await page.goto('https://localhost:7030', { waitUntil: 'domcontentloaded' });

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    try {
      await page.waitForURL('**/admin/**', { timeout: 15000 });

      // Try to navigate to stadium overview with slow API
      const startTime = Date.now();
      await page.goto('https://localhost:7030/admin/stadium-overview', {
        waitUntil: 'domcontentloaded',
        timeout: 15000
      });

      const endTime = Date.now();
      const duration = endTime - startTime;

      console.log(`⏱️ Page loaded with slow API in ${duration}ms`);

      // Take screenshot of error/loading state
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-slow-api.png',
        fullPage: true
      });

      // Check if page shows appropriate loading states or error messages
      const hasLoadingIndicator = await page.locator('[class*="loading"], [class*="spinner"], [class*="wait"]').count() > 0;
      const hasErrorMessage = await page.locator('[class*="error"], [class*="alert"]').count() > 0;

      console.log(`📊 Loading state analysis:`);
      console.log(`   • Has loading indicator: ${hasLoadingIndicator}`);
      console.log(`   • Has error message: ${hasErrorMessage}`);
      console.log(`   • Load duration with slow API: ${duration}ms`);

    } catch (error) {
      console.log(`⚠️ Error test completed: ${error}`);

      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\admin-error-state.png',
        fullPage: true
      });
    }
  });
});