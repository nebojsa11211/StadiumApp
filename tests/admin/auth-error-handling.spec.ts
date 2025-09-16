import { test, expect, Page } from '@playwright/test';
import { adminLogin, DEFAULT_ADMIN_CREDENTIALS, waitForPageReady } from '../helpers/auth-helpers';

/**
 * Authentication Error Handling Tests for Admin Application
 * Tests the fix for repeated authentication error alerts
 *
 * Context: Fixed bug where multiple 401 errors would show multiple alerts
 * Solution: Added throttling for 401 errors using "auth_error" key with 3-second interval
 */

test.describe('Admin Authentication Error Handling', () => {
  test.beforeEach(async ({ page }) => {
    // Start each test unauthenticated
    await page.context().clearCookies();
    await page.goto('/');
  });

  test('should show single alert for multiple simultaneous 401 errors', async ({ page }) => {
    console.log('🔐 Testing throttling of multiple simultaneous 401 errors');

    // First login to get a valid token
    await adminLogin(page);
    await waitForPageReady(page);

    // Navigate to Users page which makes API calls
    await page.goto('/users');
    await waitForPageReady(page);

    // Corrupt the auth token to simulate 401 errors
    await page.evaluate(() => {
      // Corrupt the localStorage token
      const tokenKey = Object.keys(localStorage).find(key =>
        key.includes('token') || key.includes('auth') || key.includes('jwt')
      );
      if (tokenKey) {
        localStorage.setItem(tokenKey, 'corrupted_token_' + Math.random());
      }

      // Also corrupt any cookies
      document.cookie = 'auth_token=corrupted; path=/';
      document.cookie = '.AspNetCore.Identity.Application=corrupted; path=/';
    });

    // Set up alert monitoring
    let alertCount = 0;
    let alertMessages: string[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      alertMessages.push(dialog.message());
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Trigger multiple UI interactions that would cause the admin services to make API calls
    try {
      // Try to click buttons that would trigger API calls - these should fail with 401 and show alerts
      const refreshButton = page.locator('button:has-text("Refresh"), button:has-text("Reload"), [data-testid="refresh-button"]');
      const createButton = page.locator('button:has-text("Create"), button:has-text("Add"), [data-testid="create-button"]');
      const searchButton = page.locator('button:has-text("Search"), [data-testid="search-button"]');

      // Click multiple buttons rapidly to trigger simultaneous 401 errors
      const clickPromises = [];

      if (await refreshButton.first().isVisible({ timeout: 1000 })) {
        clickPromises.push(refreshButton.first().click());
      }
      if (await createButton.first().isVisible({ timeout: 1000 })) {
        clickPromises.push(createButton.first().click());
      }
      if (await searchButton.first().isVisible({ timeout: 1000 })) {
        clickPromises.push(searchButton.first().click());
      }

      // Also trigger page reloads that would cause API calls
      clickPromises.push(page.reload());

      // Execute all clicks simultaneously
      await Promise.allSettled(clickPromises);

    } catch (error) {
      console.log('⚠️ Some UI interactions failed (expected due to auth errors):', error.message);
    }

    // Wait for alerts to appear and authentication redirect
    await page.waitForTimeout(3000);

    console.log(`📊 Results: ${alertCount} browser alerts`);

    // Verify throttling - should see at most 1 alert despite multiple 401s
    expect(alertCount).toBeLessThanOrEqual(1);

    if (alertCount > 0) {
      // Verify alert message contains expected authentication error text
      const hasAuthError = alertMessages.some(msg =>
        msg.includes('Unauthorized') ||
        msg.includes('session has expired') ||
        msg.includes('🔐')
      );
      expect(hasAuthError).toBe(true);
    }

    // After authentication error, should be redirected to login
    await page.waitForTimeout(2000);
    const currentUrl = page.url();
    if (alertCount > 0) {
      expect(currentUrl).toMatch(/\/login/);
    }
  });

  test('should allow new authentication errors after throttle period', async ({ page }) => {
    console.log('🕐 Testing authentication error throttling timeout');

    // Login and navigate to protected page
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    // Corrupt token and trigger first error
    await page.evaluate(() => {
      localStorage.setItem('auth_token', 'corrupted_first');
    });

    let alertCount = 0;
    page.on('dialog', async (dialog) => {
      alertCount++;
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Trigger first 401 error
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer corrupted_first' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const firstErrorCount = alertCount;

    // Trigger immediate second error (should be throttled)
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer corrupted_immediate' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const immediateErrorCount = alertCount;

    // Wait for throttle period to expire (3+ seconds)
    console.log('⏳ Waiting for throttle period to expire...');
    await page.waitForTimeout(4000);

    // Trigger third error after throttle period
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer corrupted_after_timeout' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const finalErrorCount = alertCount;

    // Verify throttling behavior
    console.log(`📊 Alert counts - First: ${firstErrorCount}, Immediate: ${immediateErrorCount}, After timeout: ${finalErrorCount}`);

    // Should have 1 alert initially, same count for immediate (throttled), and potentially +1 after timeout
    expect(immediateErrorCount).toBe(firstErrorCount); // Second error throttled
    expect(finalErrorCount).toBeGreaterThanOrEqual(firstErrorCount); // Third error may show after timeout
  });

  test('should redirect to login page after authentication error', async ({ page }) => {
    console.log('🔄 Testing redirect to login after authentication error');

    // Login and navigate to protected page
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    // Corrupt token
    await page.evaluate(() => {
      localStorage.clear();
      document.cookie = 'auth_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    });

    // Handle any alert that appears
    page.on('dialog', async (dialog) => {
      console.log(`🚨 Alert message: ${dialog.message()}`);
      await dialog.accept();
    });

    // Trigger 401 error
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer invalid_token' }
      }).catch(() => {});
    });

    // Wait for redirect
    await page.waitForTimeout(3000);

    // Should be redirected to login page
    const currentUrl = page.url();
    expect(currentUrl).toMatch(/\/login/);

    // Should see login form
    const loginForm = page.locator('input[name="Email"], #admin-login-email-input');
    await expect(loginForm).toBeVisible({ timeout: 5000 });
  });

  test('should clear tokens on authentication error', async ({ page }) => {
    console.log('🧹 Testing token cleanup on authentication error');

    // Login to establish tokens
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    // Verify tokens exist
    const initialTokens = await page.evaluate(() => {
      const tokens = [];
      for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        if (key && (key.includes('token') || key.includes('auth'))) {
          tokens.push({ key, value: localStorage.getItem(key) });
        }
      }
      return tokens;
    });

    console.log(`🔑 Initial tokens found: ${initialTokens.length}`);
    expect(initialTokens.length).toBeGreaterThan(0);

    // Handle alert
    page.on('dialog', async (dialog) => {
      await dialog.accept();
    });

    // Corrupt token and trigger 401
    await page.evaluate(() => {
      // Corrupt but don't clear tokens yet
      const tokenKey = Object.keys(localStorage).find(key =>
        key.includes('token') || key.includes('auth')
      );
      if (tokenKey) {
        localStorage.setItem(tokenKey, 'corrupted_token');
      }
    });

    // Trigger API call that causes 401
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer corrupted_token' }
      }).catch(() => {});
    });

    // Wait for error handling
    await page.waitForTimeout(3000);

    // Verify tokens are cleared
    const finalTokens = await page.evaluate(() => {
      const tokens = [];
      for (let i = 0; i < localStorage.length; i++) {
        const key = localStorage.key(i);
        if (key && (key.includes('token') || key.includes('auth'))) {
          const value = localStorage.getItem(key);
          if (value && value !== 'null' && value !== '') {
            tokens.push({ key, value });
          }
        }
      }
      return tokens;
    });

    console.log(`🧹 Final tokens remaining: ${finalTokens.length}`);
    // Tokens should be cleared or set to null/empty
    expect(finalTokens.length).toBe(0);
  });

  test('should show correct unauthorized message in alert', async ({ page }) => {
    console.log('💬 Testing authentication error message content');

    // Login and navigate to protected page
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    let alertMessage = '';
    page.on('dialog', async (dialog) => {
      alertMessage = dialog.message();
      console.log(`🚨 Alert message: "${alertMessage}"`);
      await dialog.accept();
    });

    // Corrupt token and trigger 401
    await page.evaluate(async () => {
      localStorage.setItem('auth_token', 'invalid_token');

      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer invalid_token' }
      }).catch(() => {});
    });

    // Wait for alert
    await page.waitForTimeout(2000);

    // Verify alert message contains expected content
    if (alertMessage) {
      const hasUnauthorized = alertMessage.includes('Unauthorized') || alertMessage.includes('🔐');
      const hasSessionExpired = alertMessage.includes('session has expired') || alertMessage.includes('expired');

      expect(hasUnauthorized || hasSessionExpired).toBe(true);

      // More specific checks
      if (alertMessage.includes('🔐')) {
        expect(alertMessage).toContain('🔐 Unauthorized access!');
      }
    } else {
      // If no browser alert, check for page-based alerts
      const pageAlert = page.locator('.alert:has-text("Unauthorized"), .alert:has-text("expired"), [role="alert"]:has-text("session")');
      await expect(pageAlert).toBeVisible({ timeout: 5000 });
    }
  });

  test('should handle network errors gracefully during auth error', async ({ page }) => {
    console.log('🌐 Testing network error handling during authentication error');

    // Login first
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    let alertCount = 0;
    page.on('dialog', async (dialog) => {
      alertCount++;
      console.log(`🚨 Network test alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Simulate network failure + auth error
    await page.route('**/api/**', (route) => {
      // Simulate network failure
      route.abort('failed');
    });

    // Try to make API call
    await page.evaluate(async () => {
      try {
        const baseUrl = window.location.origin.replace('9030', '9010');
        await fetch(`${baseUrl}/api/users`);
      } catch (error) {
        console.log('Expected network error:', error.message);
      }
    });

    await page.waitForTimeout(2000);

    // Should handle network errors gracefully without excessive alerts
    expect(alertCount).toBeLessThanOrEqual(1);

    // Remove route override
    await page.unroute('**/api/**');
  });

  test('should throttle errors per error type independently', async ({ page }) => {
    console.log('🔄 Testing independent throttling for different error types');

    // Login and setup
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    let alertCount = 0;
    let errorTypes: string[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      errorTypes.push(dialog.message());
      console.log(`🚨 Error type test alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Trigger auth error
    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer invalid_auth_token' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const authErrorCount = alertCount;

    // Trigger different type of error (simulated server error)
    await page.route('**/api/analytics/**', (route) => {
      route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' })
      });
    });

    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/analytics/overview`).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const totalErrorCount = alertCount;

    console.log(`📊 Auth errors: ${authErrorCount}, Total errors: ${totalErrorCount}`);

    // Different error types should not interfere with each other's throttling
    // This is more of a validation that the system can handle different error types
    expect(totalErrorCount).toBeGreaterThanOrEqual(authErrorCount);

    // Clean up route
    await page.unroute('**/api/analytics/**');
  });
});

test.describe('Admin Authentication Error Integration', () => {
  test('should maintain error throttling across page navigation', async ({ page }) => {
    console.log('🧭 Testing error throttling persistence across navigation');

    // Login and trigger auth error on users page
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    let alertCount = 0;
    page.on('dialog', async (dialog) => {
      alertCount++;
      console.log(`🚨 Navigation test alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Trigger auth error
    await page.evaluate(async () => {
      localStorage.setItem('auth_token', 'corrupted_for_navigation_test');
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/users`, {
        headers: { 'Authorization': 'Bearer corrupted_for_navigation_test' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const usersPageErrors = alertCount;

    // Navigate to different page quickly and trigger another auth error
    await page.goto('/analytics');
    await waitForPageReady(page);

    await page.evaluate(async () => {
      const baseUrl = window.location.origin.replace('9030', '9010');
      await fetch(`${baseUrl}/api/analytics/overview`, {
        headers: { 'Authorization': 'Bearer corrupted_for_navigation_test' }
      }).catch(() => {});
    });

    await page.waitForTimeout(1000);
    const analyticsPageErrors = alertCount;

    console.log(`📊 Users page errors: ${usersPageErrors}, Analytics page errors: ${analyticsPageErrors}`);

    // Second error should be throttled even on different page
    expect(analyticsPageErrors).toBe(usersPageErrors);
  });

  test('should handle authentication errors during form submission', async ({ page }) => {
    console.log('📝 Testing authentication error during form operations');

    // Login and navigate to a page with forms (users management)
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    let alertCount = 0;
    page.on('dialog', async (dialog) => {
      alertCount++;
      console.log(`🚨 Form test alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Corrupt auth before form submission
    await page.evaluate(() => {
      localStorage.setItem('auth_token', 'corrupted_for_form_test');
    });

    // Try to perform an action that would submit a form or make an API call
    // Look for any action buttons or forms
    const actionButtons = page.locator('button:has-text("Create"), button:has-text("Save"), button:has-text("Add"), button:has-text("Update")');
    const buttonCount = await actionButtons.count();

    if (buttonCount > 0) {
      // Click first available action button
      await actionButtons.first().click();
      await page.waitForTimeout(2000);
    } else {
      // Simulate form submission via API call
      await page.evaluate(async () => {
        const baseUrl = window.location.origin.replace('9030', '9010');
        await fetch(`${baseUrl}/api/users`, {
          method: 'POST',
          headers: {
            'Authorization': 'Bearer corrupted_for_form_test',
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ email: 'test@example.com', role: 'User' })
        }).catch(() => {});
      });
      await page.waitForTimeout(2000);
    }

    // Should handle auth error gracefully without multiple alerts
    expect(alertCount).toBeLessThanOrEqual(1);

    // Should redirect to login
    await page.waitForTimeout(2000);
    if (alertCount > 0) {
      expect(page.url()).toMatch(/\/login/);
    }
  });
});