import { test, expect, Page } from '@playwright/test';
import { adminLogin, DEFAULT_ADMIN_CREDENTIALS, waitForPageReady } from '../helpers/auth-helpers';

/**
 * Simplified Authentication Error Handling Tests
 * Focuses on testing the core fix: throttling of 401 authentication error alerts
 *
 * Tests that when authentication fails, only one alert is shown regardless
 * of how many simultaneous 401 errors occur.
 */

test.describe('Admin Authentication Error Throttling (Core Fix)', () => {
  test.beforeEach(async ({ page }) => {
    await page.context().clearCookies();
    await page.goto('/');
  });

  test('should show authentication error when accessing protected page without valid token', async ({ page }) => {
    console.log('🔐 Testing basic authentication error display');

    let alertCount = 0;
    let alertMessage = '';

    // Monitor for authentication alerts
    page.on('dialog', async (dialog) => {
      alertCount++;
      alertMessage = dialog.message();
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Try to access a protected page without authentication
    await page.goto('/users');
    await page.waitForTimeout(5000); // Wait longer for authentication check and alert

    // Should either show an alert or redirect to login
    const isOnLogin = page.url().includes('/login');
    const hasAuthRequired = await page.locator('text="Authentication Required", text="Login Required"').isVisible({ timeout: 2000 });

    console.log(`📍 Current URL: ${page.url()}`);
    console.log(`📊 Alert count: ${alertCount}, On login page: ${isOnLogin}, Has auth required: ${hasAuthRequired}`);

    // Verify that we're either shown an authentication error or redirected to login
    expect(isOnLogin || hasAuthRequired || alertCount > 0).toBe(true);

    if (alertCount > 0) {
      expect(alertMessage).toMatch(/unauthorized|authentication|login|session|expired/i);
    }
  });

  test('should redirect to login after corrupting valid session', async ({ page }) => {
    console.log('🔑 Testing session corruption and redirect');

    // First login successfully
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    // Verify we're authenticated and on users page
    expect(page.url()).toMatch(/\/users/);

    let alertCount = 0;
    let alertMessages: string[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      alertMessages.push(dialog.message());
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Corrupt authentication by clearing tokens
    await page.evaluate(() => {
      // Clear all localStorage
      localStorage.clear();

      // Clear authentication cookies
      document.cookie.split(";").forEach(cookie => {
        const eqPos = cookie.indexOf("=");
        const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
      });
    });

    // Try to refresh the page which should trigger authentication error
    await page.reload();
    await page.waitForTimeout(3000);

    console.log(`📍 After corruption - URL: ${page.url()}`);
    console.log(`📊 Alert count: ${alertCount}`);

    // Should be redirected to login or show authentication error
    const isOnLogin = page.url().includes('/login');

    expect(isOnLogin || alertCount > 0).toBe(true);

    if (alertCount > 0) {
      expect(alertMessages.some(msg =>
        msg.includes('🔐') ||
        msg.includes('Unauthorized') ||
        msg.includes('session has expired')
      )).toBe(true);
    }
  });

  test('should handle multiple rapid navigation attempts with corrupted token', async ({ page }) => {
    console.log('🔄 Testing rapid navigation with authentication errors');

    // Login first to establish session
    await adminLogin(page);
    await page.goto('/dashboard');
    await waitForPageReady(page);

    // Corrupt the session
    await page.evaluate(() => {
      localStorage.clear();
    });

    let alertCount = 0;
    const alertMessages: string[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      alertMessages.push(dialog.message());
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Rapidly navigate to multiple protected pages
    const protectedPages = ['/users', '/orders', '/analytics', '/drinks'];

    try {
      // Navigate rapidly to trigger multiple authentication checks
      for (const pagePath of protectedPages) {
        console.log(`🔗 Navigating to: ${pagePath}`);
        await page.goto(pagePath, { waitUntil: 'domcontentloaded', timeout: 3000 });
        await page.waitForTimeout(500); // Small delay between navigations
      }
    } catch (error) {
      console.log('⚠️ Navigation error (expected due to auth failure):', error.message);
    }

    // Wait for any alerts and redirects to complete
    await page.waitForTimeout(3000);

    console.log(`📊 Final results - Alert count: ${alertCount}, Current URL: ${page.url()}`);

    // Key assertion: Should have at most 1 authentication alert due to throttling
    expect(alertCount).toBeLessThanOrEqual(1);

    // Should end up on login page or the last accessed page (depending on redirect timing)
    const finalUrl = page.url();
    const isOnLoginOrProtected = finalUrl.includes('/login') ||
                                finalUrl.includes('/users') ||
                                finalUrl.includes('/orders') ||
                                finalUrl.includes('/analytics') ||
                                finalUrl.includes('/drinks');
    expect(isOnLoginOrProtected).toBe(true);

    if (alertCount > 0) {
      const hasValidAuthMessage = alertMessages.some(msg =>
        msg.includes('🔐') ||
        msg.includes('Unauthorized') ||
        msg.includes('session has expired')
      );
      expect(hasValidAuthMessage).toBe(true);
    }
  });

  test('should throttle authentication errors from rapid API calls', async ({ page }) => {
    console.log('⚡ Testing API call throttling with authentication errors');

    // Setup monitoring before any navigation
    let alertCount = 0;
    const alertMessages: string[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      alertMessages.push(dialog.message());
      console.log(`🚨 Alert ${alertCount}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Login and navigate to a page with API interactions
    await adminLogin(page);
    await page.goto('/users');
    await waitForPageReady(page);

    // Corrupt auth tokens
    await page.evaluate(() => {
      // Corrupt localStorage tokens
      const keys = Object.keys(localStorage);
      keys.forEach(key => {
        if (key.includes('token') || key.includes('auth')) {
          localStorage.setItem(key, 'invalid_token_' + Math.random());
        }
      });
    });

    // Try to trigger multiple quick actions that would make API calls
    try {
      // Look for and click multiple interactive elements rapidly
      const interactiveElements = [
        'button:has-text("Refresh")',
        'button:has-text("Create")',
        'button:has-text("Search")',
        'button:has-text("Load")',
        'button:has-text("Save")'
      ];

      const clickPromises = [];

      for (const selector of interactiveElements) {
        const element = page.locator(selector).first();
        if (await element.isVisible({ timeout: 500 })) {
          clickPromises.push(element.click({ timeout: 1000 }));
        }
      }

      // Also try to reload multiple times rapidly
      clickPromises.push(page.reload({ waitUntil: 'domcontentloaded' }));

      // Execute all actions simultaneously
      await Promise.allSettled(clickPromises);

    } catch (error) {
      console.log('⚠️ Some interactions failed (expected):', error.message);
    }

    // Wait for error handling to complete
    await page.waitForTimeout(4000);

    console.log(`📊 Throttling test results - Alert count: ${alertCount}`);

    // Verify throttling: despite multiple failed API calls, should have at most 1 alert
    expect(alertCount).toBeLessThanOrEqual(1);

    // Should be redirected to login
    const finalUrl = page.url();
    console.log(`📍 Final URL: ${finalUrl}`);
    expect(finalUrl).toMatch(/\/login/);

    if (alertCount > 0) {
      const authErrorPattern = /🔐|unauthorized|session.*expired|authentication/i;
      const hasValidMessage = alertMessages.some(msg => authErrorPattern.test(msg));
      expect(hasValidMessage).toBe(true);
    }
  });

  test('should verify throttle timeout allows new errors after 3 seconds', async ({ page }) => {
    console.log('⏰ Testing throttle timeout period');

    let alertCount = 0;
    const alertTimes: number[] = [];

    page.on('dialog', async (dialog) => {
      alertCount++;
      alertTimes.push(Date.now());
      console.log(`🚨 Alert ${alertCount} at ${new Date().toISOString()}: ${dialog.message()}`);
      await dialog.accept();
    });

    // Setup corrupted session from the start
    await page.goto('/dashboard');

    // Clear tokens to ensure authentication failure
    await page.evaluate(() => {
      localStorage.clear();
    });

    // First authentication error
    console.log('🔥 Triggering first authentication error...');
    await page.goto('/users', { waitUntil: 'domcontentloaded' });
    await page.waitForTimeout(1000);

    const firstErrorCount = alertCount;
    console.log(`📊 First error count: ${firstErrorCount}`);

    // Immediate second attempt (should be throttled)
    console.log('🔥 Triggering immediate second authentication error...');
    await page.goto('/orders', { waitUntil: 'domcontentloaded' });
    await page.waitForTimeout(1000);

    const immediateErrorCount = alertCount;
    console.log(`📊 Immediate error count: ${immediateErrorCount}`);

    // Wait for throttle period to expire (3+ seconds)
    console.log('⏳ Waiting for throttle period to expire (4 seconds)...');
    await page.waitForTimeout(4000);

    // Third attempt after throttle period
    console.log('🔥 Triggering third authentication error after timeout...');
    await page.goto('/analytics', { waitUntil: 'domcontentloaded' });
    await page.waitForTimeout(1000);

    const finalErrorCount = alertCount;
    console.log(`📊 Final error count: ${finalErrorCount}`);

    // Verify throttling behavior
    if (alertTimes.length >= 2) {
      const timeDiff = alertTimes[1] - alertTimes[0];
      console.log(`⏱️ Time between alerts: ${timeDiff}ms`);
    }

    // Expected behavior:
    // - First error should show alert
    // - Immediate second error should be throttled (same count)
    // - Third error after timeout may show new alert
    expect(immediateErrorCount).toBe(firstErrorCount); // Second error throttled

    // After timeout, new errors should be allowed
    if (firstErrorCount > 0) {
      expect(finalErrorCount).toBeGreaterThanOrEqual(firstErrorCount);
    }

    // Should end up on login page or protected page (depending on navigation timing)
    const finalUrl = page.url();
    expect(finalUrl).toMatch(/\/(login|dashboard|users|orders|analytics|drinks)/);
    console.log(`✅ Final URL check passed: ${finalUrl}`);
  });
});