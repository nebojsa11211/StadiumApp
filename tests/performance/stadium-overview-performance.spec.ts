import { test, expect, Page } from '@playwright/test';

// Configure test to run against local services
test.describe.configure({ mode: 'serial' });

interface PerformanceMetrics {
  navigationTiming: any;
  networkRequests: Array<{
    url: string;
    method: string;
    status: number;
    duration: number;
    size: number;
    type: string;
  }>;
  consoleLogs: Array<{
    type: string;
    text: string;
    timestamp: number;
  }>;
  jsExecutionTime: number;
  domContentLoaded: number;
  loadEvent: number;
  timeToInteractive: number;
  largestContentfulPaint: number;
  firstContentfulPaint: number;
  cumulativeLayoutShift: number;
  totalBlockingTime: number;
}

test.describe('Stadium Overview Performance Tests', () => {
  let page: Page;
  let performanceMetrics: PerformanceMetrics;

  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();

    // Initialize performance metrics collection
    performanceMetrics = {
      navigationTiming: {},
      networkRequests: [],
      consoleLogs: [],
      jsExecutionTime: 0,
      domContentLoaded: 0,
      loadEvent: 0,
      timeToInteractive: 0,
      largestContentfulPaint: 0,
      firstContentfulPaint: 0,
      cumulativeLayoutShift: 0,
      totalBlockingTime: 0
    };

    // Capture console logs
    page.on('console', msg => {
      performanceMetrics.consoleLogs.push({
        type: msg.type(),
        text: msg.text(),
        timestamp: Date.now()
      });
    });

    // Capture network requests with simplified timing
    const requestTimings = new Map<string, number>();

    page.on('request', request => {
      requestTimings.set(request.url(), Date.now());
    });

    page.on('response', async response => {
      const request = response.request();
      const requestStartTime = requestTimings.get(request.url()) || Date.now();
      const responseTime = Date.now();
      const duration = responseTime - requestStartTime;

      let size = 0;
      try {
        const headers = await response.allHeaders();
        size = parseInt(headers['content-length'] || '0');
        if (isNaN(size)) size = 0;
      } catch (e) {
        size = 0;
      }

      performanceMetrics.networkRequests.push({
        url: request.url(),
        method: request.method(),
        status: response.status(),
        duration: duration,
        size: size,
        type: request.resourceType()
      });

      // Clean up timing entry
      requestTimings.delete(request.url());
    });
  });

  test('Comprehensive Stadium Overview Performance Analysis', async () => {
    console.log('🚀 Starting Stadium Overview Performance Test...\n');

    // Step 1: Navigate to login page and measure initial load
    console.log('📋 Step 1: Loading login page...');
    const loginStartTime = Date.now();

    await page.goto('https://localhost:7030/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    const loginLoadTime = Date.now() - loginStartTime;
    console.log(`✅ Login page loaded in: ${loginLoadTime}ms\n`);

    // Step 2: Perform login
    console.log('🔐 Step 2: Performing admin login...');
    const loginSubmitStartTime = Date.now();

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for navigation to complete
    await page.waitForURL('**/admin**', { timeout: 10000 });

    const loginCompleteTime = Date.now() - loginSubmitStartTime;
    console.log(`✅ Login completed in: ${loginCompleteTime}ms\n`);

    // Step 3: Navigate to stadium overview and capture detailed performance
    console.log('🏟️ Step 3: Loading stadium overview page...');
    const overviewStartTime = Date.now();

    // Clear previous network requests for clean measurement
    performanceMetrics.networkRequests = [];

    // Start performance measurement
    await page.evaluate(() => {
      performance.mark('overview-start');
    });

    // Navigate to stadium overview
    await page.goto('https://localhost:7030/admin/stadium-overview', {
      waitUntil: 'domcontentloaded',
      timeout: 30000
    });

    // Wait for page to be fully interactive
    await page.waitForLoadState('networkidle', { timeout: 30000 });

    // Mark end of navigation
    await page.evaluate(() => {
      performance.mark('overview-end');
      performance.measure('overview-navigation', 'overview-start', 'overview-end');
    });

    const overviewLoadTime = Date.now() - overviewStartTime;
    console.log(`✅ Stadium overview loaded in: ${overviewLoadTime}ms\n`);

    // Step 4: Collect detailed performance metrics
    console.log('📊 Step 4: Collecting performance metrics...');

    // Get navigation timing
    const navigationTiming = await page.evaluate(() => {
      const perf = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
      return {
        domainLookup: perf.domainLookupEnd - perf.domainLookupStart,
        connect: perf.connectEnd - perf.connectStart,
        request: perf.responseStart - perf.requestStart,
        response: perf.responseEnd - perf.responseStart,
        domContentLoaded: perf.domContentLoadedEventEnd - perf.navigationStart,
        loadEvent: perf.loadEventEnd - perf.navigationStart,
        totalTime: perf.loadEventEnd - perf.navigationStart
      };
    });

    // Get Web Vitals metrics
    const webVitals = await page.evaluate(() => {
      return new Promise((resolve) => {
        const vitals: any = {};

        // LCP - Largest Contentful Paint
        new PerformanceObserver((entryList) => {
          const entries = entryList.getEntries();
          vitals.lcp = entries[entries.length - 1].startTime;
        }).observe({ entryTypes: ['largest-contentful-paint'] });

        // FCP - First Contentful Paint
        new PerformanceObserver((entryList) => {
          vitals.fcp = entryList.getEntries()[0].startTime;
        }).observe({ entryTypes: ['paint'] });

        // CLS - Cumulative Layout Shift
        let clsValue = 0;
        new PerformanceObserver((entryList) => {
          for (const entry of entryList.getEntries()) {
            if (!(entry as any).hadRecentInput) {
              clsValue += (entry as any).value;
            }
          }
          vitals.cls = clsValue;
        }).observe({ entryTypes: ['layout-shift'] });

        // TBT - Total Blocking Time (approximation)
        const longTasks: number[] = [];
        new PerformanceObserver((entryList) => {
          for (const entry of entryList.getEntries()) {
            longTasks.push(entry.duration);
          }
          vitals.tbt = longTasks.reduce((sum, duration) => sum + Math.max(0, duration - 50), 0);
        }).observe({ entryTypes: ['longtask'] });

        // Wait a bit for metrics to be collected
        setTimeout(() => resolve(vitals), 2000);
      });
    });

    // Get resource timing
    const resourceTiming = await page.evaluate(() => {
      return performance.getEntriesByType('resource').map(entry => ({
        name: entry.name,
        duration: entry.duration,
        size: (entry as any).transferSize || 0,
        type: (entry as any).initiatorType
      }));
    });

    // Step 5: Analyze specific page elements
    console.log('🔍 Step 5: Analyzing page elements...');

    // Check if stadium structure is loaded
    const stadiumStructureLoaded = await page.evaluate(() => {
      const container = document.querySelector('#admin-stadium-overview-container');
      return container !== null;
    });

    // Check for any stadium sections
    const stadiumSectionsCount = await page.evaluate(() => {
      const sections = document.querySelectorAll('[class*="stadium-section"], [class*="sector"]');
      return sections.length;
    });

    // Check for loading states
    const loadingElements = await page.evaluate(() => {
      const loadingSpinners = document.querySelectorAll('.spinner-border, .loading, [class*="loading"]');
      const loadingTexts = Array.from(document.querySelectorAll('*')).filter(el =>
        el.textContent?.toLowerCase().includes('loading') ||
        el.textContent?.toLowerCase().includes('please wait')
      );
      return {
        spinners: loadingSpinners.length,
        texts: loadingTexts.length
      };
    });

    // Step 6: Generate comprehensive performance report
    console.log('\n🎯 STADIUM OVERVIEW PERFORMANCE ANALYSIS REPORT');
    console.log('=' .repeat(60));

    console.log('\n📈 TIMING METRICS:');
    console.log(`• Total Page Load Time: ${overviewLoadTime}ms`);
    console.log(`• Domain Lookup: ${navigationTiming.domainLookup}ms`);
    console.log(`• Connection: ${navigationTiming.connect}ms`);
    console.log(`• Request Time: ${navigationTiming.request}ms`);
    console.log(`• Response Time: ${navigationTiming.response}ms`);
    console.log(`• DOM Content Loaded: ${navigationTiming.domContentLoaded}ms`);
    console.log(`• Load Event: ${navigationTiming.loadEvent}ms`);

    console.log('\n🎨 WEB VITALS:');
    console.log(`• First Contentful Paint: ${webVitals.fcp?.toFixed(2) || 'N/A'}ms`);
    console.log(`• Largest Contentful Paint: ${webVitals.lcp?.toFixed(2) || 'N/A'}ms`);
    console.log(`• Cumulative Layout Shift: ${webVitals.cls?.toFixed(4) || 'N/A'}`);
    console.log(`• Total Blocking Time: ${webVitals.tbt?.toFixed(2) || 'N/A'}ms`);

    console.log('\n🌐 NETWORK ANALYSIS:');
    const totalRequests = performanceMetrics.networkRequests.length;
    const slowRequests = performanceMetrics.networkRequests.filter(req => req.duration > 1000);
    const failedRequests = performanceMetrics.networkRequests.filter(req => req.status >= 400);
    const totalDataTransferred = performanceMetrics.networkRequests.reduce((sum, req) => sum + req.size, 0);

    console.log(`• Total Network Requests: ${totalRequests}`);
    console.log(`• Failed Requests: ${failedRequests.length}`);
    console.log(`• Slow Requests (>1s): ${slowRequests.length}`);
    console.log(`• Total Data Transferred: ${(totalDataTransferred / 1024 / 1024).toFixed(2)} MB`);

    if (slowRequests.length > 0) {
      console.log('\n🐌 SLOWEST REQUESTS:');
      slowRequests
        .sort((a, b) => b.duration - a.duration)
        .slice(0, 5)
        .forEach(req => {
          console.log(`  • ${req.method} ${req.url.substring(req.url.lastIndexOf('/') + 1)} - ${req.duration.toFixed(0)}ms`);
        });
    }

    if (failedRequests.length > 0) {
      console.log('\n❌ FAILED REQUESTS:');
      failedRequests.forEach(req => {
        console.log(`  • ${req.status} ${req.method} ${req.url}`);
      });
    }

    console.log('\n📝 CONSOLE ANALYSIS:');
    const errors = performanceMetrics.consoleLogs.filter(log => log.type === 'error');
    const warnings = performanceMetrics.consoleLogs.filter(log => log.type === 'warning');

    console.log(`• Console Errors: ${errors.length}`);
    console.log(`• Console Warnings: ${warnings.length}`);
    console.log(`• Total Console Messages: ${performanceMetrics.consoleLogs.length}`);

    if (errors.length > 0) {
      console.log('\n🚨 CONSOLE ERRORS:');
      errors.forEach(error => {
        console.log(`  • ${error.text}`);
      });
    }

    if (warnings.length > 0) {
      console.log('\n⚠️ CONSOLE WARNINGS:');
      warnings.slice(0, 3).forEach(warning => {
        console.log(`  • ${warning.text}`);
      });
      if (warnings.length > 3) {
        console.log(`  • ... and ${warnings.length - 3} more warnings`);
      }
    }

    console.log('\n🏟️ STADIUM CONTENT ANALYSIS:');
    console.log(`• Stadium Structure Container: ${stadiumStructureLoaded ? 'Found' : 'Missing'}`);
    console.log(`• Stadium Sections Count: ${stadiumSectionsCount}`);
    console.log(`• Loading Spinners: ${loadingElements.spinners}`);
    console.log(`• Loading Text Elements: ${loadingElements.texts}`);

    console.log('\n🎯 PERFORMANCE RECOMMENDATIONS:');
    const recommendations: string[] = [];

    if (overviewLoadTime > 3000) {
      recommendations.push('🔴 Page load time exceeds 3 seconds - optimize critical resources');
    }

    if (webVitals.lcp && webVitals.lcp > 2500) {
      recommendations.push('🔴 LCP exceeds 2.5s - optimize largest content elements');
    }

    if (webVitals.fcp && webVitals.fcp > 1800) {
      recommendations.push('🟡 FCP exceeds 1.8s - optimize initial paint');
    }

    if (webVitals.cls && webVitals.cls > 0.1) {
      recommendations.push('🔴 CLS exceeds 0.1 - fix layout shifts');
    }

    if (slowRequests.length > 0) {
      recommendations.push(`🟡 ${slowRequests.length} slow network requests detected - optimize or cache`);
    }

    if (errors.length > 0) {
      recommendations.push(`🔴 ${errors.length} JavaScript errors detected - fix critical issues`);
    }

    if (totalDataTransferred > 5 * 1024 * 1024) {
      recommendations.push('🟡 High data usage (>5MB) - implement compression and optimization');
    }

    if (stadiumSectionsCount === 0) {
      recommendations.push('🔴 No stadium sections found - verify stadium structure loading');
    }

    if (loadingElements.spinners > 0 || loadingElements.texts > 0) {
      recommendations.push('🟡 Loading indicators still visible - check async operations');
    }

    if (recommendations.length === 0) {
      recommendations.push('✅ Performance is within acceptable ranges');
    }

    recommendations.forEach(rec => console.log(`  ${rec}`));

    console.log('\n📋 DETAILED RESOURCE BREAKDOWN:');
    const resourcesByType = resourceTiming.reduce((acc: any, resource) => {
      const type = resource.type || 'other';
      if (!acc[type]) acc[type] = { count: 0, totalDuration: 0, totalSize: 0 };
      acc[type].count++;
      acc[type].totalDuration += resource.duration;
      acc[type].totalSize += resource.size;
      return acc;
    }, {});

    Object.entries(resourcesByType).forEach(([type, stats]: [string, any]) => {
      console.log(`• ${type}: ${stats.count} files, ${stats.totalDuration.toFixed(0)}ms total, ${(stats.totalSize/1024).toFixed(0)}KB`);
    });

    console.log('\n' + '='.repeat(60));
    console.log('🏁 Performance analysis complete!');

    // Assertions for automated testing
    expect(overviewLoadTime).toBeLessThan(10000); // Should load within 10 seconds
    expect(errors.length).toBe(0); // No JavaScript errors
    expect(stadiumStructureLoaded).toBe(true); // Stadium structure should be present
  });

  test.afterEach(async () => {
    await page.close();
  });
});