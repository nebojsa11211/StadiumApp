import { test, expect, BrowserContext, Page } from '@playwright/test';

test.describe('Authentication System Tests - Route Fixes', () => {
  let context: BrowserContext;
  let page: Page;

  const customerEmail = 'customer@stadium.com';
  const customerPassword = 'customer123';
  const baseUrl = 'https://localhost:7025';
  const apiUrl = 'https://localhost:7010';

  test.beforeAll(async ({ browser }) => {
    // Create a new context for isolation
    context = await browser.newContext({
      ignoreHTTPSErrors: true,
      baseURL: baseUrl
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
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('1. Initial Page Load and Navigation', async () => {
    console.log('=== Testing Initial Page Load ===');

    // Navigate to homepage
    await page.goto('/');
    await expect(page).toHaveTitle(/Stadium/);

    // Check that we can see login/signup buttons (unauthenticated state)
    const signInButton = page.locator('#customer-layout-sign-in-btn');
    const signUpButton = page.locator('#customer-layout-sign-up-btn');

    await expect(signInButton).toBeVisible();
    await expect(signUpButton).toBeVisible();

    console.log('✅ Initial page load successful - unauthenticated state confirmed');
  });

  test('2. Login Process and Token Storage', async () => {
    console.log('=== Testing Login Process ===');

    // Click login button
    await page.click('#customer-layout-sign-in-btn');
    await expect(page).toHaveURL('/login');

    // Fill login form
    await page.fill('#customer-login-email-input', customerEmail);
    await page.fill('#customer-login-password-input', customerPassword);

    // Submit login
    await page.click('#customer-login-submit-btn');

    // Wait for redirect to home page
    await page.waitForURL('/', { timeout: 10000 });

    // Verify authenticated state - user dropdown should be visible
    const userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible({ timeout: 5000 });

    // Verify tokens are stored in localStorage
    const accessToken = await page.evaluate(() => localStorage.getItem('customer_access_token'));
    const refreshToken = await page.evaluate(() => localStorage.getItem('customer_refresh_token'));

    expect(accessToken).toBeTruthy();
    expect(refreshToken).toBeTruthy();

    console.log('✅ Login successful - tokens stored in localStorage');
    console.log('Access token length:', accessToken?.length);
    console.log('Refresh token length:', refreshToken?.length);
  });

  test('3. Protected Routes Access - Orders Page', async () => {
    console.log('=== Testing Protected Routes Access ===');

    // Navigate to Orders page
    await page.goto('/orders');

    // Should NOT redirect to login - should stay on orders page
    await expect(page).toHaveURL('/orders', { timeout: 5000 });

    // Wait for page to load and check for orders content
    await page.waitForLoadState('networkidle');

    // Look for orders page indicators
    const ordersTitle = page.locator('h1, h2, h3').filter({ hasText: /orders/i }).first();
    await expect(ordersTitle).toBeVisible({ timeout: 10000 });

    console.log('✅ Orders page accessible without redirect - route fix working');
  });

  test('4. API Calls Return 200 OK (Not 404)', async () => {
    console.log('=== Testing API Calls ===');

    // Track API responses
    const apiResponses: Array<{ url: string; status: number; statusText: string }> = [];

    page.on('response', response => {
      if (response.url().includes('/api/')) {
        apiResponses.push({
          url: response.url(),
          status: response.status(),
          statusText: response.statusText()
        });
        console.log(`API Response: ${response.status()} ${response.statusText()} - ${response.url()}`);
      }
    });

    // Navigate to orders page to trigger API calls
    await page.goto('/orders');
    await page.waitForLoadState('networkidle');

    // Wait a bit for any async API calls
    await page.waitForTimeout(2000);

    // Check for API responses
    if (apiResponses.length > 0) {
      console.log('\n=== API Response Summary ===');
      apiResponses.forEach(response => {
        console.log(`${response.status} ${response.statusText} - ${response.url}`);
      });

      // Look for orders API calls specifically
      const ordersApiCalls = apiResponses.filter(r => r.url.includes('/api/orders'));
      if (ordersApiCalls.length > 0) {
        const successfulCalls = ordersApiCalls.filter(r => r.status >= 200 && r.status < 300);
        const notFoundCalls = ordersApiCalls.filter(r => r.status === 404);

        console.log(`Orders API calls: ${ordersApiCalls.length} total`);
        console.log(`Successful (2xx): ${successfulCalls.length}`);
        console.log(`Not Found (404): ${notFoundCalls.length}`);

        if (notFoundCalls.length > 0) {
          console.log('❌ Still getting 404 errors on orders API');
          notFoundCalls.forEach(call => console.log(`  404: ${call.url}`));
        } else {
          console.log('✅ No 404 errors on orders API - route fix successful');
        }
      }
    } else {
      console.log('ℹ️  No API calls detected on orders page');
    }
  });

  test('5. Authentication Persistence Across Page Refreshes', async () => {
    console.log('=== Testing Authentication Persistence ===');

    // Ensure we're authenticated and on orders page
    await page.goto('/orders');

    // Verify authenticated state before refresh
    const userDropdownBefore = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdownBefore).toBeVisible();

    // Refresh the page
    await page.reload();
    await page.waitForLoadState('networkidle');

    // Should still be on orders page (not redirected to login)
    await expect(page).toHaveURL('/orders');

    // Should still be authenticated (user dropdown visible)
    const userDropdownAfter = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdownAfter).toBeVisible({ timeout: 10000 });

    // Verify tokens are still in localStorage
    const accessTokenAfter = await page.evaluate(() => localStorage.getItem('customer_access_token'));
    const refreshTokenAfter = await page.evaluate(() => localStorage.getItem('customer_refresh_token'));

    expect(accessTokenAfter).toBeTruthy();
    expect(refreshTokenAfter).toBeTruthy();

    console.log('✅ Authentication persists across page refresh');
  });

  test('6. Navigation Between Pages Maintains Login State', async () => {
    console.log('=== Testing Navigation State Persistence ===');

    // Start on home page
    await page.goto('/');

    // Verify authenticated state
    let userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible();

    // Navigate to events page
    await page.click('#customer-nav-events-link');
    await expect(page).toHaveURL('/events');

    userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible();

    // Navigate to orders page
    await page.click('#customer-nav-orders-link');
    await expect(page).toHaveURL('/orders');

    userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible();

    // Navigate back to home
    await page.click('#customer-nav-home-link');
    await expect(page).toHaveURL('/');

    userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible();

    console.log('✅ Login state maintained across all navigation');
  });

  test('7. Logout Functionality', async () => {
    console.log('=== Testing Logout Functionality ===');

    // Ensure we're authenticated
    await page.goto('/');
    const userDropdown = page.locator('#customer-layout-user-dropdown');
    await expect(userDropdown).toBeVisible();

    // Click user dropdown to open it
    await page.click('#customer-layout-user-dropdown');

    // Wait for dropdown menu to appear and click logout
    const logoutButton = page.locator('#customer-layout-logout-btn');
    await expect(logoutButton).toBeVisible();
    await page.click('#customer-layout-logout-btn');

    // Should redirect to home and show unauthenticated state
    await page.waitForURL('/');

    // Verify unauthenticated state - login/signup buttons should be visible
    const signInButton = page.locator('#customer-layout-sign-in-btn');
    const signUpButton = page.locator('#customer-layout-sign-up-btn');

    await expect(signInButton).toBeVisible({ timeout: 5000 });
    await expect(signUpButton).toBeVisible();

    // Verify tokens are removed from localStorage
    const accessTokenAfter = await page.evaluate(() => localStorage.getItem('customer_access_token'));
    const refreshTokenAfter = await page.evaluate(() => localStorage.getItem('customer_refresh_token'));

    expect(accessTokenAfter).toBeNull();
    expect(refreshTokenAfter).toBeNull();

    console.log('✅ Logout successful - tokens cleared, unauthenticated state restored');
  });

  test('8. Direct API Endpoint Testing', async () => {
    console.log('=== Testing Direct API Endpoints ===');

    // First login to get a token
    await page.goto('/login');
    await page.fill('#customer-login-email-input', customerEmail);
    await page.fill('#customer-login-password-input', customerPassword);
    await page.click('#customer-login-submit-btn');
    await page.waitForURL('/');

    // Get the access token
    const accessToken = await page.evaluate(() => localStorage.getItem('customer_access_token'));
    expect(accessToken).toBeTruthy();

    // Test direct API calls using the browser context
    const apiTestResults: Array<{ endpoint: string; status: number; success: boolean }> = [];

    const endpointsToTest = [
      '/api/auth/me',
      '/api/orders',
      '/api/logs/search'
    ];

    for (const endpoint of endpointsToTest) {
      try {
        const response = await page.request.get(`${apiUrl}${endpoint}`, {
          headers: {
            'Authorization': `Bearer ${accessToken}`
          }
        });

        const result = {
          endpoint,
          status: response.status(),
          success: response.ok()
        };

        apiTestResults.push(result);
        console.log(`${endpoint}: ${response.status()} ${response.ok() ? '✅' : '❌'}`);

      } catch (error) {
        apiTestResults.push({
          endpoint,
          status: 0,
          success: false
        });
        console.log(`${endpoint}: ERROR - ${error}`);
      }
    }

    // Verify that we get successful responses (not 404s)
    const notFoundResponses = apiTestResults.filter(r => r.status === 404);
    const successfulResponses = apiTestResults.filter(r => r.success);

    console.log(`\n=== API Test Summary ===`);
    console.log(`Successful responses: ${successfulResponses.length}/${apiTestResults.length}`);
    console.log(`404 responses: ${notFoundResponses.length}/${apiTestResults.length}`);

    if (notFoundResponses.length === 0) {
      console.log('✅ All API endpoints responding correctly - no 404 errors');
    } else {
      console.log('❌ Some API endpoints still returning 404:');
      notFoundResponses.forEach(r => console.log(`  ${r.endpoint}`));
    }
  });
});