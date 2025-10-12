import { test, expect } from '@playwright/test';

test.describe('Admin Stadium Overview Diagnosis', () => {
  test('diagnose loading issue and capture detailed information', async ({ page }) => {
    // Enable console logging
    const consoleLogs: string[] = [];
    const consoleErrors: string[] = [];
    const networkRequests: Array<{ url: string; status: number; response?: string }> = [];

    page.on('console', (msg) => {
      const text = msg.text();
      consoleLogs.push(`[${msg.type()}] ${text}`);
      if (msg.type() === 'error') {
        consoleErrors.push(text);
      }
    });

    // Track network requests
    page.on('response', async (response) => {
      const url = response.url();
      if (url.includes('api') || url.includes('StadiumViewer')) {
        try {
          const body = await response.text();
          networkRequests.push({
            url,
            status: response.status(),
            response: body.substring(0, 500) // First 500 chars
          });
        } catch (e) {
          networkRequests.push({
            url,
            status: response.status(),
            response: 'Could not read response body'
          });
        }
      }
    });

    // Navigate to login first
    console.log('Step 1: Navigating to login page...');
    await page.goto('https://localhost:7030/login');
    await page.waitForLoadState('networkidle');

    // Take screenshot of login page
    await page.screenshot({ path: 'test-results/1-login-page.png', fullPage: true });

    // Login
    console.log('Step 2: Logging in...');
    await page.fill('#customer-login-email-input', 'admin@stadium.com');
    await page.fill('#customer-login-password-input', 'admin123');
    await page.click('#customer-login-submit-btn');

    // Wait for navigation after login
    await page.waitForURL('**/admin/**', { timeout: 10000 });
    await page.waitForLoadState('networkidle');

    // Take screenshot after login
    await page.screenshot({ path: 'test-results/2-after-login.png', fullPage: true });
    console.log('Login successful');

    // Navigate to Stadium Overview
    console.log('Step 3: Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');

    // Wait a bit for initial render
    await page.waitForTimeout(2000);

    // Take screenshot of initial state
    await page.screenshot({ path: 'test-results/3-stadium-overview-initial.png', fullPage: true });

    // Check for loading state
    const loadingState = await page.locator('#admin-stadium-loading-state').isVisible();
    console.log(`Loading state visible: ${loadingState}`);

    // Check for error state
    const errorState = await page.locator('#admin-stadium-error-state').isVisible();
    console.log(`Error state visible: ${errorState}`);

    // Check for empty state
    const emptyState = await page.locator('#admin-stadium-empty-state').isVisible();
    console.log(`Empty state visible: ${emptyState}`);

    // Check for stadium layout
    const stadiumLayout = await page.locator('#admin-stadium-grid-layout').isVisible();
    console.log(`Stadium layout visible: ${stadiumLayout}`);

    // Wait up to 30 seconds to see if loading completes
    console.log('Step 4: Waiting up to 30 seconds for loading to complete...');
    try {
      await page.waitForSelector('#admin-stadium-grid-layout', { timeout: 30000 });
      console.log('Stadium layout appeared!');
    } catch (e) {
      console.log('Stadium layout did not appear after 30 seconds');
    }

    // Take final screenshot
    await page.screenshot({ path: 'test-results/4-stadium-overview-final.png', fullPage: true });

    // Get the loading indicator text if visible
    if (loadingState) {
      const loadingText = await page.locator('#admin-stadium-loading-state').textContent();
      console.log('Loading state text:', loadingText);
    }

    // Get error message if visible
    if (errorState) {
      const errorText = await page.locator('#admin-stadium-error-state').textContent();
      console.log('Error state text:', errorText);
    }

    // Check page source for isLoading variable
    const pageContent = await page.content();
    console.log('Page includes loading state div:', pageContent.includes('admin-stadium-loading-state'));

    // Print all console logs
    console.log('\n=== CONSOLE LOGS ===');
    consoleLogs.forEach(log => console.log(log));

    // Print all console errors
    if (consoleErrors.length > 0) {
      console.log('\n=== CONSOLE ERRORS ===');
      consoleErrors.forEach(err => console.log(err));
    }

    // Print network requests
    console.log('\n=== NETWORK REQUESTS ===');
    networkRequests.forEach(req => {
      console.log(`${req.status} - ${req.url}`);
      if (req.status !== 200) {
        console.log(`  Response: ${req.response}`);
      }
    });

    // Try to evaluate the component state via browser console
    try {
      const componentState = await page.evaluate(() => {
        // Try to find Blazor component state
        const loadingDiv = document.getElementById('admin-stadium-loading-state');
        const errorDiv = document.getElementById('admin-stadium-error-state');
        const layoutDiv = document.getElementById('admin-stadium-grid-layout');

        return {
          loadingVisible: loadingDiv ? window.getComputedStyle(loadingDiv).display !== 'none' : false,
          errorVisible: errorDiv ? window.getComputedStyle(errorDiv).display !== 'none' : false,
          layoutVisible: layoutDiv ? window.getComputedStyle(layoutDiv).display !== 'none' : false,
          loadingContent: loadingDiv?.textContent || '',
          errorContent: errorDiv?.textContent || ''
        };
      });
      console.log('\n=== COMPONENT STATE ===');
      console.log(JSON.stringify(componentState, null, 2));
    } catch (e) {
      console.log('Could not evaluate component state:', e);
    }

    // Generate diagnostic report
    const report = {
      loadingStateVisible: loadingState,
      errorStateVisible: errorState,
      emptyStateVisible: emptyState,
      stadiumLayoutVisible: stadiumLayout,
      consoleErrorsCount: consoleErrors.length,
      networkRequestsCount: networkRequests.length,
      apiFailures: networkRequests.filter(r => r.status >= 400),
      consoleLogs: consoleLogs.slice(-20), // Last 20 logs
      consoleErrors,
      networkRequests: networkRequests.slice(-10) // Last 10 requests
    };

    console.log('\n=== DIAGNOSTIC REPORT ===');
    console.log(JSON.stringify(report, null, 2));

    // Create a text report file
    const fs = require('fs');
    fs.writeFileSync('test-results/diagnostic-report.json', JSON.stringify(report, null, 2));
  });
});
