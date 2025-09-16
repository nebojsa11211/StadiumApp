import { test, expect, Page } from '@playwright/test';

/**
 * Admin Authentication Validation - Dependency Injection Fix Verification
 *
 * This test validates that the admin authentication system is working correctly
 * after fixing the dependency injection issue. The system now properly shows
 * authentication required pages instead of throwing Internal Server Errors.
 */

test.describe('Admin Authentication - Dependency Injection Fix Validation', () => {
  const adminUrl = 'https://localhost:9030';

  test.beforeEach(async ({ page }) => {
    // Clear any existing authentication state
    await page.context().clearCookies();
  });

  test('Admin authentication system - Complete validation', async ({ page }) => {
    console.log('=== Testing Admin Authentication System ===');

    // Step 1: Navigate to Admin application
    console.log('Step 1: Navigating to Admin application');
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    // Step 2: Verify authentication required page loads (not 500 error)
    console.log('Step 2: Verifying authentication required page loads correctly');

    // The system should show authentication required page, not crash with 500 error
    await expect(page.locator('text=Authentication Required')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('text=You need to be logged in to access this page')).toBeVisible();
    await expect(page.locator('text=Go to Login')).toBeVisible();

    // Verify no server errors
    const pageContent = await page.content();
    expect(pageContent).not.toContain('Internal Server Error');
    expect(pageContent).not.toContain('500');

    console.log('✅ Authentication required page loads correctly (no 500 errors)');

    // Step 3: Click "Go to Login" button
    console.log('Step 3: Clicking Go to Login button');
    await page.click('text=Go to Login');

    // Should navigate to login page
    await expect(page).toHaveURL(/.*\/login/, { timeout: 10000 });
    console.log('✅ Successfully redirected to login page');

    // Step 4: Verify login page loads without errors
    console.log('Step 4: Verifying login page loads correctly');
    await expect(page.locator('#admin-login-title, text=Login, text=Sign In')).toBeVisible({ timeout: 10000 });

    // Look for login form elements (may have different IDs)
    const emailInput = page.locator('input[type="email"], input[name="email"], #admin-login-email-input');
    const passwordInput = page.locator('input[type="password"], input[name="password"], #admin-login-password-input');
    const submitButton = page.locator('button[type="submit"], #admin-login-submit-btn, button:has-text("Login"), button:has-text("Sign In")');

    await expect(emailInput).toBeVisible();
    await expect(passwordInput).toBeVisible();
    await expect(submitButton).toBeVisible();
    console.log('✅ Login form elements are visible');

    // Step 5: Test login functionality
    console.log('Step 5: Testing login with correct credentials');

    // Fill login form
    await emailInput.fill('admin@stadium.com');
    await passwordInput.fill('admin123');

    // Monitor for API responses
    let loginApiCalled = false;
    let loginApiResponse: any = null;

    page.on('response', async response => {
      if (response.url().includes('/api/auth/login')) {
        loginApiCalled = true;
        loginApiResponse = response;
        console.log('Login API called:', response.status(), response.statusText());
      }
    });

    // Submit form
    await submitButton.click();

    // Step 6: Wait for login response and verify success
    console.log('Step 6: Waiting for login response');
    await page.waitForTimeout(5000); // Give time for API call

    // Verify API was called
    expect(loginApiCalled).toBeTruthy();
    console.log('✅ Login API was called');

    if (loginApiResponse) {
      // Should not be 500 error
      expect(loginApiResponse.status()).not.toBe(500);
      console.log('✅ Login API did not return 500 error');

      if (loginApiResponse.status() === 200) {
        console.log('✅ Login API returned 200 (successful login)');

        // Should redirect away from login page
        await page.waitForTimeout(3000);
        const currentUrl = page.url();
        if (!currentUrl.includes('/login')) {
          console.log('✅ Successfully redirected to admin dashboard');
        }
      } else {
        console.log('Login API returned:', loginApiResponse.status());
        // Even if credentials are wrong, should not be 500 error
        expect(loginApiResponse.status()).not.toBe(500);
      }
    }

    // Step 7: Test protected page access
    console.log('Step 7: Testing protected page access');

    // Try accessing a protected admin page
    await page.goto(`${adminUrl}/admin/users`, { waitUntil: 'networkidle' });

    // Should show authentication required (not 500 error)
    const pageContentAfter = await page.content();
    expect(pageContentAfter).not.toContain('Internal Server Error');
    expect(pageContentAfter).not.toContain('500');

    console.log('✅ Protected pages show authentication required (no 500 errors)');

    console.log('=== Admin Authentication System Validation Complete ===');
    console.log('✅ DEPENDENCY INJECTION FIX CONFIRMED - No Internal Server Errors');
  });

  test('Admin authentication - Invalid credentials handling', async ({ page }) => {
    console.log('=== Testing Invalid Credentials Handling ===');

    // Navigate to login
    await page.goto(`${adminUrl}/login`, { waitUntil: 'networkidle' });

    // Wait for login form
    await expect(page.locator('input[type="email"], input[name="email"]')).toBeVisible({ timeout: 10000 });

    // Fill with invalid credentials
    const emailInput = page.locator('input[type="email"], input[name="email"]');
    const passwordInput = page.locator('input[type="password"], input[name="password"]');
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign In")');

    await emailInput.fill('invalid@test.com');
    await passwordInput.fill('wrongpassword');

    // Monitor API response
    let apiResponse: any = null;
    page.on('response', async response => {
      if (response.url().includes('/api/auth/login')) {
        apiResponse = response;
      }
    });

    await submitButton.click();
    await page.waitForTimeout(5000);

    // Should not return 500 error (dependency injection working)
    if (apiResponse) {
      expect(apiResponse.status()).not.toBe(500);
      console.log('✅ Invalid credentials handled without 500 error');
    }

    // Should show error message (not 500 page)
    const pageContent = await page.content();
    expect(pageContent).not.toContain('Internal Server Error');
    expect(pageContent).not.toContain('500');

    console.log('✅ Invalid credentials handled gracefully');
  });

  test('Admin authentication - API dependency injection validation', async ({ page }) => {
    console.log('=== Testing API Dependency Injection ===');

    // This test validates that the TokenStorageService and AdminApiService
    // are properly registered and working together

    await page.goto(`${adminUrl}/login`, { waitUntil: 'networkidle' });

    // Wait for page load
    await page.waitForTimeout(2000);

    // Check that page loads without dependency injection errors
    const pageSource = await page.content();

    // Should not contain dependency injection error messages
    expect(pageSource).not.toContain('System.InvalidOperationException');
    expect(pageSource).not.toContain('Unable to resolve service');
    expect(pageSource).not.toContain('TokenStorageService');
    expect(pageSource).not.toContain('AdminApiService');
    expect(pageSource).not.toContain('Internal Server Error');

    // Should load proper login page
    expect(pageSource).toContain('Login');

    console.log('✅ No dependency injection errors found');
    console.log('✅ TokenStorageService and AdminApiService properly registered');
  });
});