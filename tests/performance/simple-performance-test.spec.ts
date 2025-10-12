import { test, expect, Page } from '@playwright/test';

test.describe('Stadium Overview Performance Test', () => {
  test('Measure Stadium Overview Page Performance', async ({ page }) => {
    console.log('🚀 Starting Stadium Overview Performance Test...\n');

    // Set up performance monitoring
    const performanceData: any = {
      navigationTiming: {},
      networkRequests: [],
      consoleLogs: [],
      loadTime: 0,
      pageSize: 0
    };

    // Capture console logs
    page.on('console', msg => {
      performanceData.consoleLogs.push({
        type: msg.type(),
        text: msg.text(),
        timestamp: Date.now()
      });
    });

    // Capture network requests
    const requestTimes = new Map<string, number>();

    page.on('request', request => {
      requestTimes.set(request.url(), Date.now());
    });

    page.on('response', async response => {
      const request = response.request();
      const startTime = requestTimes.get(request.url()) || Date.now();
      const endTime = Date.now();
      const duration = endTime - startTime;

      let size = 0;
      try {
        const headers = await response.allHeaders();
        size = parseInt(headers['content-length'] || '0') || 0;
      } catch (e) {
        // Ignore
      }

      performanceData.networkRequests.push({
        url: request.url(),
        method: request.method(),
        status: response.status(),
        duration: duration,
        size: size,
        type: request.resourceType()
      });

      requestTimes.delete(request.url());
    });

    // Step 1: Navigate to login page
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

    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for navigation to complete (could redirect to root)
    await page.waitForLoadState('networkidle', { timeout: 10000 });

    // Check if we're redirected to root or admin page
    const currentUrl = page.url();
    console.log(`Current URL after login: ${currentUrl}`);

    const loginCompleteTime = Date.now() - loginSubmitStartTime;
    console.log(`✅ Login completed in: ${loginCompleteTime}ms\n`);

    // Step 3: Navigate to stadium overview and measure performance
    console.log('🏟️ Step 3: Loading stadium overview page...');
    const overviewStartTime = Date.now();

    // Clear previous requests for clean measurement
    performanceData.networkRequests = [];

    await page.goto('https://localhost:7030/admin/stadium-overview', {
      waitUntil: 'domcontentloaded',
      timeout: 60000
    });

    // Wait for the page to be fully loaded
    await page.waitForLoadState('networkidle', { timeout: 60000 });

    const overviewLoadTime = Date.now() - overviewStartTime;
    console.log(`✅ Stadium overview loaded in: ${overviewLoadTime}ms\n`);

    // Step 4: Collect performance metrics
    console.log('📊 Step 4: Collecting performance metrics...');

    // Get navigation timing
    const navigationTiming = await page.evaluate(() => {
      const perf = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming;
      return {
        domContentLoaded: perf.domContentLoadedEventEnd - perf.navigationStart,
        loadEvent: perf.loadEventEnd - perf.navigationStart,
        domainLookup: perf.domainLookupEnd - perf.domainLookupStart,
        connect: perf.connectEnd - perf.connectStart,
        request: perf.responseStart - perf.requestStart,
        response: perf.responseEnd - perf.responseStart,
        totalTime: perf.loadEventEnd - perf.navigationStart
      };
    });

    // Get Web Vitals (simplified)
    const webVitals = await page.evaluate(() => {
      return new Promise((resolve) => {
        const vitals: any = {};

        // Try to get paint timing
        try {
          const paintEntries = performance.getEntriesByType('paint');
          paintEntries.forEach(entry => {
            if (entry.name === 'first-contentful-paint') {
              vitals.fcp = entry.startTime;
            }
          });
        } catch (e) {
          // Ignore
        }

        // Simple timeout for collection
        setTimeout(() => resolve(vitals), 1000);
      });
    });

    // Check page content
    const stadiumContent = await page.evaluate(() => {
      const container = document.querySelector('#admin-stadium-overview-container');
      const hasContainer = container !== null;
      const containerText = container?.textContent || '';
      const hasStadiumData = containerText.includes('stadium') || containerText.includes('Stadium');

      return {
        hasContainer,
        hasStadiumData,
        containerSize: containerText.length,
        elementCount: document.querySelectorAll('*').length
      };
    });

    // Step 5: Generate performance report
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

    console.log('\n🌐 NETWORK ANALYSIS:');
    const totalRequests = performanceData.networkRequests.length;
    const slowRequests = performanceData.networkRequests.filter((req: any) => req.duration > 1000);
    const failedRequests = performanceData.networkRequests.filter((req: any) => req.status >= 400);
    const totalDataTransferred = performanceData.networkRequests.reduce((sum: number, req: any) => sum + (req.size || 0), 0);

    console.log(`• Total Network Requests: ${totalRequests}`);
    console.log(`• Failed Requests: ${failedRequests.length}`);
    console.log(`• Slow Requests (>1s): ${slowRequests.length}`);
    console.log(`• Total Data Transferred: ${(totalDataTransferred / 1024 / 1024).toFixed(2)} MB`);

    if (slowRequests.length > 0) {
      console.log('\n🐌 SLOWEST REQUESTS:');
      slowRequests
        .sort((a: any, b: any) => b.duration - a.duration)
        .slice(0, 5)
        .forEach((req: any) => {
          console.log(`  • ${req.method} ${req.url.substring(req.url.lastIndexOf('/') + 1)} - ${req.duration.toFixed(0)}ms`);
        });
    }

    if (failedRequests.length > 0) {
      console.log('\n❌ FAILED REQUESTS:');
      failedRequests.forEach((req: any) => {
        console.log(`  • ${req.status} ${req.method} ${req.url}`);
      });
    }

    console.log('\n📝 CONSOLE ANALYSIS:');
    const errors = performanceData.consoleLogs.filter((log: any) => log.type === 'error');
    const warnings = performanceData.consoleLogs.filter((log: any) => log.type === 'warning');

    console.log(`• Console Errors: ${errors.length}`);
    console.log(`• Console Warnings: ${warnings.length}`);
    console.log(`• Total Console Messages: ${performanceData.consoleLogs.length}`);

    if (errors.length > 0) {
      console.log('\n🚨 CONSOLE ERRORS:');
      errors.forEach((error: any) => {
        console.log(`  • ${error.text}`);
      });
    }

    if (warnings.length > 0) {
      console.log('\n⚠️ CONSOLE WARNINGS:');
      warnings.slice(0, 3).forEach((warning: any) => {
        console.log(`  • ${warning.text}`);
      });
      if (warnings.length > 3) {
        console.log(`  • ... and ${warnings.length - 3} more warnings`);
      }
    }

    console.log('\n🏟️ STADIUM CONTENT ANALYSIS:');
    console.log(`• Stadium Container Found: ${stadiumContent.hasContainer ? 'Yes' : 'No'}`);
    console.log(`• Has Stadium Data: ${stadiumContent.hasStadiumData ? 'Yes' : 'No'}`);
    console.log(`• Container Content Size: ${stadiumContent.containerSize} characters`);
    console.log(`• Total DOM Elements: ${stadiumContent.elementCount}`);

    console.log('\n🎯 PERFORMANCE RECOMMENDATIONS:');
    const recommendations: string[] = [];

    if (overviewLoadTime > 5000) {
      recommendations.push('🔴 Page load time exceeds 5 seconds - critical optimization needed');
    } else if (overviewLoadTime > 3000) {
      recommendations.push('🟡 Page load time exceeds 3 seconds - optimization recommended');
    } else {
      recommendations.push('✅ Page load time is acceptable');
    }

    if (webVitals.fcp && webVitals.fcp > 2500) {
      recommendations.push('🔴 FCP exceeds 2.5s - optimize initial content rendering');
    }

    if (slowRequests.length > 3) {
      recommendations.push(`🔴 ${slowRequests.length} slow network requests detected - optimize API endpoints`);
    } else if (slowRequests.length > 0) {
      recommendations.push(`🟡 ${slowRequests.length} slow network requests detected - monitor performance`);
    }

    if (errors.length > 0) {
      recommendations.push(`🔴 ${errors.length} JavaScript errors detected - fix critical issues`);
    }

    if (totalDataTransferred > 5 * 1024 * 1024) {
      recommendations.push('🟡 High data usage (>5MB) - implement compression and optimization');
    }

    if (!stadiumContent.hasContainer) {
      recommendations.push('🔴 Stadium container not found - verify page functionality');
    }

    if (!stadiumContent.hasStadiumData) {
      recommendations.push('🟡 No stadium data detected - verify data loading');
    }

    recommendations.forEach(rec => console.log(`  ${rec}`));

    console.log('\n📋 RESOURCE TYPE BREAKDOWN:');
    const resourcesByType = performanceData.networkRequests.reduce((acc: any, resource: any) => {
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

    // Basic assertions
    expect(overviewLoadTime).toBeLessThan(30000); // Should load within 30 seconds
    expect(errors.length).toBe(0); // No JavaScript errors
    expect(stadiumContent.hasContainer).toBe(true); // Stadium container should be present
  });
});