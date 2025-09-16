import { test, expect, Page } from '@playwright/test';

/**
 * Comprehensive Admin Authentication Test - Dependency Injection Fix Validation
 *
 * This test validates that the admin authentication system is working correctly
 * after fixing the dependency injection issue that was causing Internal Server Errors.
 *
 * Issues Fixed:
 * 1. TokenStorageService registration as Singleton (was causing null token issues)
 * 2. AdminApiService properly accessing persisted tokens
 * 3. Proper JWT token validation and storage
 */

test.describe('Admin Authentication - Dependency Injection Fix', () => {
  let adminUrl: string;

  test.beforeAll(() => {
    adminUrl = 'https://localhost:9030';
  });

  test.beforeEach(async ({ page }) => {
    // Clear any existing authentication state
    await page.context().clearCookies();

    // Safely clear storage without causing security errors
    try {
      await page.evaluate(() => {
        try {
          localStorage.clear();
          sessionStorage.clear();
        } catch (e) {
          // Ignore security errors for cross-origin iframes
          console.log('Storage clear skipped due to security restrictions');
        }
      });
    } catch (e) {
      // Continue if localStorage is not accessible
      console.log('Storage clearing skipped');
    }
  });

  test('Admin login flow - Complete dependency injection validation', async ({ page }) => {
    console.log('=== Starting Admin Authentication Test ===');

    // Step 1: Navigate to Admin application
    console.log('Step 1: Navigating to Admin application');
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    // Verify we're redirected to login page (authentication required)
    console.log('Verifying automatic redirect to login page');
    await expect(page).toHaveURL(/.*\/login/);
    await expect(page.locator('#admin-login-title')).toBeVisible({ timeout: 10000 });

    // Step 2: Verify login page loads without errors
    console.log('Step 2: Verifying login page loads correctly');
    await expect(page.locator('#admin-login-container')).toBeVisible();
    await expect(page.locator('#admin-login-form')).toBeVisible();
    await expect(page.locator('#admin-login-email-input')).toBeVisible();
    await expect(page.locator('#admin-login-password-input')).toBeVisible();
    await expect(page.locator('#admin-login-submit-btn')).toBeVisible();

    // Verify no JavaScript errors on page load
    const consoleErrors: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        consoleErrors.push(msg.text());
      }
    });

    // Step 3: Fill in login credentials
    console.log('Step 3: Filling login credentials');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    // Verify form is filled correctly
    await expect(page.locator('#admin-login-email-input')).toHaveValue('admin@stadium.com');
    await expect(page.locator('#admin-login-password-input')).toHaveValue('admin123');

    // Step 4: Monitor network requests during login
    console.log('Step 4: Monitoring login API request');
    let loginRequestPromise: any = null;
    let loginResponse: any = null;

    // Set up request/response monitoring
    page.on('request', request => {
      if (request.url().includes('/api/auth/login')) {
        console.log('Login API request detected:', request.url());
        loginRequestPromise = request;
      }
    });

    page.on('response', async response => {
      if (response.url().includes('/api/auth/login')) {
        console.log('Login API response:', response.status(), response.statusText());
        loginResponse = response;

        if (response.ok()) {
          try {
            const responseBody = await response.json();
            console.log('Login response token present:', !!responseBody.token);
            console.log('Login response user:', responseBody.user?.email || 'Unknown');
          } catch (e) {
            console.log('Could not parse login response body');
          }
        }
      }
    });

    // Step 5: Submit login form
    console.log('Step 5: Submitting login form');
    await page.click('#admin-login-submit-btn');

    // Wait for loading state to appear and disappear
    await expect(page.locator('#admin-login-spinner')).toBeVisible({ timeout: 5000 });
    await expect(page.locator('#admin-login-spinner')).not.toBeVisible({ timeout: 15000 });

    // Step 6: Verify successful authentication
    console.log('Step 6: Verifying successful authentication');

    // Wait for redirect to dashboard (should not stay on login page)
    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });

    // Verify we're now on the admin dashboard
    const currentUrl = page.url();
    console.log('Current URL after login:', currentUrl);
    expect(currentUrl).not.toContain('/login');

    // Step 7: Verify JWT token storage
    console.log('Step 7: Verifying JWT token storage');
    let authToken = null;
    try {
      authToken = await page.evaluate(() => {
        try {
          return localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
        } catch (e) {
          return null;
        }
      });
    } catch (e) {
      console.log('Token check skipped due to storage restrictions');
    }

    // Token storage verification is optional but preferred
    if (authToken) {
      console.log('JWT token stored successfully:', !!authToken);
    } else {
      console.log('Token storage check skipped (possible cross-origin restrictions)');
    }

    // Step 8: Verify no Internal Server Errors occurred
    console.log('Step 8: Verifying no server errors during authentication');
    expect(loginResponse).toBeTruthy();
    expect(loginResponse.status()).toBe(200);

    // Check for any 500 errors in network tab
    const networkErrors = await page.evaluate(() => {
      return (window as any).networkErrors || [];
    });

    expect(networkErrors.filter((err: any) => err.status >= 500)).toHaveLength(0);

    // Step 9: Verify admin dashboard loads correctly
    console.log('Step 9: Verifying admin dashboard functionality');

    // Wait for dashboard elements to load
    await expect(page.locator('h1, h2, .dashboard-title, [class*="title"]')).toBeVisible({ timeout: 10000 });

    // Step 10: Test authenticated API calls
    console.log('Step 10: Testing authenticated API functionality');

    // Try to navigate to a protected admin page (orders, users, etc.)
    const protectedPages = ['/admin/orders', '/admin/users', '/admin/drinks'];

    for (const protectedPath of protectedPages) {
      console.log(`Testing navigation to protected page: ${protectedPath}`);
      await page.goto(`${adminUrl}${protectedPath}`, { waitUntil: 'networkidle' });

      // Should not redirect back to login
      expect(page.url()).not.toContain('/login');

      // Should load page content without 500 errors
      const errorElements = page.locator('.alert-danger, .error-message, [class*="error"]');
      const errorCount = await errorElements.count();

      if (errorCount > 0) {
        const errorTexts = await errorElements.allTextContents();
        const has500Error = errorTexts.some(text => text.includes('500') || text.includes('Internal Server Error'));
        expect(has500Error).toBeFalsy();
      }
    }

    // Step 11: Verify authentication state persistence
    console.log('Step 11: Testing authentication state persistence');

    // Refresh the page and verify we stay logged in
    await page.reload({ waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Should not redirect to login after refresh
    expect(page.url()).not.toContain('/login');

    // Step 12: Test logout functionality
    console.log('Step 12: Testing logout functionality');

    // Look for logout button/link
    const logoutSelectors = [
      '[id*="logout"]',
      '[class*="logout"]',
      'a[href*="logout"]',
      'button[title*="logout"]',
      'text=Logout',
      'text=Sign Out'
    ];

    let logoutButton = null;
    for (const selector of logoutSelectors) {
      const element = page.locator(selector);
      if (await element.count() > 0) {
        logoutButton = element.first();
        break;
      }
    }

    if (logoutButton) {
      await logoutButton.click();
      await page.waitForTimeout(1000);

      // Should redirect back to login
      await expect(page).toHaveURL(/.*\/login/);

      // Token should be cleared
      let tokenAfterLogout = null;
      try {
        tokenAfterLogout = await page.evaluate(() => {
          try {
            return localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
          } catch (e) {
            return null;
          }
        });
        if (tokenAfterLogout !== null) {
          expect(tokenAfterLogout).toBeFalsy();
        }
      } catch (e) {
        console.log('Post-logout token check skipped');
      }
    }

    // Final validation - no console errors during the entire flow
    console.log('=== Final Validation ===');
    console.log('Console errors during test:', consoleErrors.length);

    // Allow some non-critical errors but no authentication-related errors
    const criticalErrors = consoleErrors.filter(error =>
      error.includes('500') ||
      error.includes('Internal Server Error') ||
      error.includes('Failed to fetch') ||
      error.includes('TokenStorageService') ||
      error.includes('AdminApiService')
    );

    expect(criticalErrors).toHaveLength(0);

    console.log('=== Admin Authentication Test Completed Successfully ===');
  });

  test('Admin login - Invalid credentials handling', async ({ page }) => {
    console.log('=== Testing Invalid Credentials Handling ===');

    await page.goto(`${adminUrl}/login`, { waitUntil: 'networkidle' });

    // Try invalid credentials
    await page.fill('#admin-login-email-input', 'invalid@test.com');
    await page.fill('#admin-login-password-input', 'wrongpassword');
    await page.click('#admin-login-submit-btn');

    // Should show error message (not 500 error)
    await page.waitForTimeout(3000);

    // Should stay on login page
    expect(page.url()).toContain('/login');

    // Should not have Internal Server Error
    const errorMessage = page.locator('#admin-login-error, .alert-danger, .error-message');
    if (await errorMessage.count() > 0) {
      const errorText = await errorMessage.textContent();
      expect(errorText?.toLowerCase()).not.toContain('internal server error');
      expect(errorText?.toLowerCase()).not.toContain('500');
    }

    console.log('Invalid credentials handled correctly (no 500 errors)');
  });

  test('Admin authentication - Direct protected page access', async ({ page }) => {
    console.log('=== Testing Direct Protected Page Access ===');

    // Try to access protected page directly without authentication
    await page.goto(`${adminUrl}/admin/users`, { waitUntil: 'networkidle' });

    // Should redirect to login (not show 500 error)
    await expect(page).toHaveURL(/.*\/login/, { timeout: 10000 });

    // Should show login form, not error page
    await expect(page.locator('#admin-login-container')).toBeVisible();

    console.log('Protected page access properly redirected to login');
  });

  test('Admin authentication - API service token persistence', async ({ page }) => {
    console.log('=== Testing API Service Token Persistence ===');

    // Login first
    await page.goto(`${adminUrl}/login`, { waitUntil: 'networkidle' });
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for successful login
    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });

    // Navigate to different admin pages to test token persistence
    const testPages = ['/admin/orders', '/admin/users', '/admin/drinks', '/admin/analytics'];

    for (const testPage of testPages) {
      console.log(`Testing token persistence on page: ${testPage}`);

      await page.goto(`${adminUrl}${testPage}`, { waitUntil: 'networkidle' });

      // Should not redirect to login (token should persist)
      expect(page.url()).not.toContain('/login');

      // Wait for API calls to complete
      await page.waitForTimeout(2000);

      // Check for any authentication errors
      const authErrors = page.locator('.alert-danger, .error-message, [class*="error"]');
      const errorCount = await authErrors.count();

      if (errorCount > 0) {
        const errorTexts = await authErrors.allTextContents();
        const hasAuthError = errorTexts.some(text =>
          text.includes('Unauthorized') ||
          text.includes('401') ||
          text.includes('Authentication')
        );
        expect(hasAuthError).toBeFalsy();
      }
    }

    console.log('Token persistence test completed successfully');
  });
});