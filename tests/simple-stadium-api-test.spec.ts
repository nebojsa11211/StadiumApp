import { test, expect } from '@playwright/test';

test.describe('Stadium Overview API Diagnosis', () => {
  test('check if API is accessible and responding', async ({ page, request }) => {
    // Test 1: Check if API health endpoint responds
    console.log('=== Test 1: API Health Check ===');
    try {
      const healthResponse = await request.get('https://localhost:7010/api/health');
      console.log(`Health check status: ${healthResponse.status()}`);
    } catch (e) {
      console.log(`Health check failed: ${e}`);
    }

    // Test 2: Check StadiumViewer/overview endpoint directly
    console.log('\n=== Test 2: StadiumViewer/overview Direct API Call ===');
    try {
      const viewerResponse = await request.get('https://localhost:7010/StadiumViewer/overview');
      console.log(`StadiumViewer status: ${viewerResponse.status()}`);
      const responseBody = await viewerResponse.text();
      console.log(`Response body (first 500 chars): ${responseBody.substring(0, 500)}`);

      if (viewerResponse.ok()) {
        const jsonData = JSON.parse(responseBody);
        console.log(`Stadium name: ${jsonData.name || 'N/A'}`);
        console.log(`Number of stands: ${jsonData.stands?.length || 0}`);
      }
    } catch (e) {
      console.log(`StadiumViewer API call failed: ${e}`);
    }

    // Test 3: Login to Admin and check browser console for errors
    console.log('\n=== Test 3: Admin App Login and Console Check ===');

    const consoleLogs: string[] = [];
    const networkErrors: Array<{ url: string; error: string }> = [];

    page.on('console', msg => {
      const text = `[${msg.type()}] ${msg.text()}`;
      consoleLogs.push(text);
      console.log(text);
    });

    page.on('requestfailed', request => {
      const error = {
        url: request.url(),
        error: request.failure()?.errorText || 'Unknown error'
      };
      networkErrors.push(error);
      console.log(`Request failed: ${request.url()} - ${error.error}`);
    });

    // Navigate and login
    await page.goto('https://localhost:7030/login');
    await page.waitForLoadState('networkidle');

    await page.fill('#customer-login-email-input', 'admin@stadium.com');
    await page.fill('#customer-login-password-input', 'admin123');
    await page.click('#customer-login-submit-btn');

    await page.waitForURL('**/admin/**', { timeout: 10000 });
    console.log('Login successful');

    // Test 4: Navigate to Stadium Overview and monitor network
    console.log('\n=== Test 4: Navigate to Stadium Overview ===');

    const apiCalls: Array<{ url: string; status: number; timing: number }> = [];

    page.on('response', async response => {
      const url = response.url();
      if (url.includes('StadiumViewer') || url.includes('stadium')) {
        const timing = response.request().timing().responseEnd;
        apiCalls.push({
          url,
          status: response.status(),
          timing
        });
        console.log(`API call: ${url} - Status: ${response.status()} - Timing: ${timing}ms`);

        if (!response.ok()) {
          const body = await response.text().catch(() => 'Could not read body');
          console.log(`Error response body: ${body.substring(0, 300)}`);
        }
      }
    });

    await page.goto('https://localhost:7030/admin/stadium-overview');

    // Wait and observe
    await page.waitForTimeout(5000);

    // Check page state
    console.log('\n=== Page State After 5 Seconds ===');
    const isLoading = await page.locator('#admin-stadium-loading-state').isVisible();
    const hasError = await page.locator('#admin-stadium-error-state').isVisible();
    const hasLayout = await page.locator('#admin-stadium-grid-layout').isVisible();

    console.log(`Loading state visible: ${isLoading}`);
    console.log(`Error state visible: ${hasError}`);
    console.log(`Stadium layout visible: ${hasLayout}`);

    // Take screenshot
    await page.screenshot({ path: 'test-results/stadium-overview-state.png', fullPage: true });

    // Test 5: Check if the page is stuck
    console.log('\n=== Test 5: Extended Wait (30 seconds) ===');
    try {
      await page.waitForSelector('#admin-stadium-grid-layout', { timeout: 30000 });
      console.log('✅ Stadium layout appeared!');
    } catch {
      console.log('❌ Stadium layout did not appear after 30 seconds');

      // Get final state
      const finalIsLoading = await page.locator('#admin-stadium-loading-state').isVisible();
      const finalHasError = await page.locator('#admin-stadium-error-state').isVisible();
      const finalErrorText = finalHasError
        ? await page.locator('#admin-stadium-error-state').textContent()
        : 'N/A';

      console.log(`Final loading state: ${finalIsLoading}`);
      console.log(`Final error state: ${finalHasError}`);
      console.log(`Error text: ${finalErrorText}`);

      await page.screenshot({ path: 'test-results/stadium-overview-stuck.png', fullPage: true });
    }

    // Summary
    console.log('\n=== DIAGNOSTIC SUMMARY ===');
    console.log(`Total API calls to stadium endpoints: ${apiCalls.length}`);
    console.log(`Network errors: ${networkErrors.length}`);
    console.log(`Console error count: ${consoleLogs.filter(l => l.includes('[error]')).length}`);

    if (apiCalls.length === 0) {
      console.log('⚠️ WARNING: No API calls to stadium endpoints detected!');
      console.log('This suggests the page is not making the expected API request.');
    }

    if (networkErrors.length > 0) {
      console.log('\nNetwork Errors:');
      networkErrors.forEach(e => console.log(`  - ${e.url}: ${e.error}`));
    }
  });
});
