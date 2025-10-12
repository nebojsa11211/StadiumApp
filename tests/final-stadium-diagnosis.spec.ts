import { test, expect } from '@playwright/test';

test.describe('Stadium Overview Loading Diagnosis', () => {
  test('diagnose stadium overview loading issue', async ({ page }) => {
    console.log('\n========================================');
    console.log('STADIUM OVERVIEW LOADING DIAGNOSIS');
    console.log('========================================\n');

    // Track all console messages
    const consoleLogs: string[] = [];
    page.on('console', msg => {
      const text = `[${msg.type()}] ${msg.text()}`;
      consoleLogs.push(text);
      if (msg.type() === 'error' || msg.type() === 'warning') {
        console.log(text);
      }
    });

    // Track network requests to StadiumViewer
    const stadiumRequests: Array<{
      url: string;
      status: number;
      method: string;
      timing: number;
      responseText?: string;
    }> = [];

    page.on('response', async response => {
      const url = response.url();
      if (url.includes('StadiumViewer') || url.includes('stadium')) {
        const request = response.request();
        const timing = request.timing().responseEnd;

        let responseText = undefined;
        if (!response.ok()) {
          try {
            responseText = await response.text();
          } catch (e) {
            responseText = 'Could not read response';
          }
        }

        const reqInfo = {
          url,
          status: response.status(),
          method: request.method(),
          timing,
          responseText
        };

        stadiumRequests.push(reqInfo);

        console.log(`\n📡 API Request: ${request.method()} ${url}`);
        console.log(`   Status: ${response.status()}`);
        console.log(`   Timing: ${timing}ms`);

        if (!response.ok() && responseText) {
          console.log(`   Error Response: ${responseText.substring(0, 200)}`);
        }
      }
    });

    // Step 1: Login
    console.log('\n=== STEP 1: LOGIN ===');
    await page.goto('https://localhost:7030/login');
    await page.waitForLoadState('networkidle');

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    await page.screenshot({ path: 'test-results/1-before-login.png' });

    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/admin/**', { timeout: 15000 });
    await page.waitForLoadState('networkidle');

    console.log('✅ Login successful');
    await page.screenshot({ path: 'test-results/2-after-login.png' });

    // Step 2: Navigate to Stadium Overview
    console.log('\n=== STEP 2: NAVIGATE TO STADIUM OVERVIEW ===');
    await page.goto('https://localhost:7030/admin/stadium-overview');

    // Wait for initial render
    await page.waitForTimeout(1000);

    await page.screenshot({ path: 'test-results/3-stadium-initial.png', fullPage: true });

    // Check immediate state
    const initialState = {
      loading: await page.locator('#admin-stadium-loading-state').isVisible(),
      error: await page.locator('#admin-stadium-error-state').isVisible(),
      empty: await page.locator('#admin-stadium-empty-state').isVisible(),
      layout: await page.locator('#admin-stadium-grid-layout').isVisible()
    };

    console.log('\nInitial Page State (after 1s):');
    console.log(`  Loading: ${initialState.loading}`);
    console.log(`  Error: ${initialState.error}`);
    console.log(`  Empty: ${initialState.empty}`);
    console.log(`  Layout: ${initialState.layout}`);

    if (initialState.loading) {
      const loadingText = await page.locator('#admin-stadium-loading-state').textContent();
      console.log(`  Loading Text: "${loadingText?.trim().substring(0, 100)}..."`);
    }

    // Step 3: Wait and monitor
    console.log('\n=== STEP 3: MONITORING (30 SECONDS) ===');

    let layoutAppeared = false;
    for (let i = 0; i < 6; i++) {
      await page.waitForTimeout(5000);
      const seconds = (i + 1) * 5;

      const currentState = {
        loading: await page.locator('#admin-stadium-loading-state').isVisible(),
        error: await page.locator('#admin-stadium-error-state').isVisible(),
        layout: await page.locator('#admin-stadium-grid-layout').isVisible()
      };

      console.log(`\nAt ${seconds}s:`);
      console.log(`  Loading: ${currentState.loading}`);
      console.log(`  Error: ${currentState.error}`);
      console.log(`  Layout: ${currentState.layout}`);

      if (currentState.layout) {
        layoutAppeared = true;
        console.log(`\n✅ Stadium layout appeared after ${seconds} seconds!`);
        break;
      }

      if (currentState.error) {
        const errorText = await page.locator('#admin-stadium-error-state').textContent();
        console.log(`\n❌ Error state detected: ${errorText}`);
        break;
      }

      // Take screenshot every 10 seconds
      if (seconds % 10 === 0) {
        await page.screenshot({
          path: `test-results/4-stadium-at-${seconds}s.png`,
          fullPage: true
        });
      }
    }

    // Final screenshot
    await page.screenshot({ path: 'test-results/5-stadium-final.png', fullPage: true });

    // Step 4: Analyze Results
    console.log('\n=== STEP 4: ANALYSIS ===');

    if (!layoutAppeared) {
      console.log('\n❌ ISSUE DETECTED: Stadium layout never appeared!');

      // Check if still loading
      const stillLoading = await page.locator('#admin-stadium-loading-state').isVisible();
      if (stillLoading) {
        console.log('\n🔍 Page is stuck in loading state');
        console.log(`   Total API calls to stadium endpoints: ${stadiumRequests.length}`);

        if (stadiumRequests.length === 0) {
          console.log('\n⚠️ CRITICAL: No API calls were made to StadiumViewer endpoints!');
          console.log('   This suggests the component is not calling LoadStadiumData()');
          console.log('   Possible causes:');
          console.log('   1. Exception thrown in OnInitializedAsync()');
          console.log('   2. LoadStadiumData() never called');
          console.log('   3. API service not properly injected');
          console.log('   4. isLoading flag not being set to false');
        } else {
          console.log('\n📊 API Calls Made:');
          stadiumRequests.forEach((req, idx) => {
            console.log(`\n   Call ${idx + 1}:`);
            console.log(`   ${req.method} ${req.url}`);
            console.log(`   Status: ${req.status}`);
            console.log(`   Time: ${req.timing}ms`);
            if (req.responseText) {
              console.log(`   Error: ${req.responseText.substring(0, 150)}`);
            }
          });
        }

        // Check browser console for errors
        const errorLogs = consoleLogs.filter(l =>
          l.includes('[error]') || l.includes('Exception') || l.includes('Failed')
        );

        if (errorLogs.length > 0) {
          console.log('\n⚠️ Console Errors Detected:');
          errorLogs.forEach(log => console.log(`   ${log}`));
        }
      }

      // Try to extract component state from browser
      try {
        const componentState = await page.evaluate(() => {
          const loadingDiv = document.getElementById('admin-stadium-loading-state');
          const errorDiv = document.getElementById('admin-stadium-error-state');
          const layoutDiv = document.getElementById('admin-stadium-grid-layout');

          return {
            loadingDisplay: loadingDiv ? window.getComputedStyle(loadingDiv).display : 'N/A',
            errorDisplay: errorDiv ? window.getComputedStyle(errorDiv).display : 'N/A',
            layoutDisplay: layoutDiv ? window.getComputedStyle(layoutDiv).display : 'N/A',
            loadingHTML: loadingDiv?.innerHTML.substring(0, 200) || 'N/A',
            errorHTML: errorDiv?.innerHTML.substring(0, 200) || 'N/A'
          };
        });

        console.log('\n🔍 Component DOM State:');
        console.log(JSON.stringify(componentState, null, 2));
      } catch (e) {
        console.log('\n❌ Could not extract component state');
      }
    } else {
      console.log('\n✅ Stadium layout loaded successfully!');
      console.log(`   API calls made: ${stadiumRequests.length}`);
      console.log(`   Time to first layout: <30 seconds`);
    }

    // Summary Report
    console.log('\n========================================');
    console.log('DIAGNOSTIC SUMMARY');
    console.log('========================================');
    console.log(`Layout Appeared: ${layoutAppeared ? 'YES' : 'NO'}`);
    console.log(`API Calls Made: ${stadiumRequests.length}`);
    console.log(`Console Errors: ${consoleLogs.filter(l => l.includes('[error]')).length}`);
    console.log(`Network Failures: ${stadiumRequests.filter(r => r.status >= 400).length}`);
    console.log('========================================\n');

    // Save detailed report
    const report = {
      testResult: layoutAppeared ? 'PASS' : 'FAIL',
      layoutAppeared,
      apiCallsCount: stadiumRequests.length,
      apiCalls: stadiumRequests,
      consoleErrors: consoleLogs.filter(l => l.includes('[error]') || l.includes('Exception')),
      allLogs: consoleLogs
    };

    const fs = require('fs');
    fs.writeFileSync(
      'test-results/stadium-diagnostic-report.json',
      JSON.stringify(report, null, 2)
    );

    console.log('📄 Detailed report saved to: test-results/stadium-diagnostic-report.json');
  });
});
