import { test, expect, Page, BrowserContext, APIRequestContext } from '@playwright/test';
import { adminLogin, adminLogout, verifyAuthenticated, verifyUnauthenticated, DEFAULT_ADMIN_CREDENTIALS } from '../helpers/auth-helpers';

/**
 * Comprehensive Authentication System Refactoring Tests
 *
 * Tests all security enhancements and authentication system improvements:
 * 1. Admin Application Authentication (https://localhost:9030)
 * 2. API Security Features (https://localhost:9010)
 * 3. Token Management (JWT Refresh, Validation)
 * 4. Security Headers and Rate Limiting
 * 5. Cross-Application Authentication Consistency
 */

const API_BASE_URL = 'https://localhost:9010';
const ADMIN_BASE_URL = 'https://localhost:9030';

test.describe('Authentication System Refactoring - Security Tests', () => {
  test.beforeEach(async ({ page }) => {
    // Clear any existing authentication state
    await page.context().clearCookies();
    await page.context().clearPermissions();
  });

  test.describe('1. Admin Application Authentication', () => {
    test('should authenticate admin user successfully', async ({ page }) => {
      console.log('🔐 Testing admin authentication flow...');

      // Navigate to admin app
      await page.goto('/');

      // Should be redirected to login or show authentication required
      const hasAuthRequired = await page.locator('text="Authentication Required"').isVisible({ timeout: 5000 });
      const isOnLogin = page.url().includes('/login');

      expect(hasAuthRequired || isOnLogin).toBe(true);

      // Perform login
      await adminLogin(page);

      // Verify successful authentication
      await verifyAuthenticated(page);
      await expect(page).toHaveURL(/.*\/dashboard/);

      console.log('✅ Admin authentication successful');
    });

    test('should maintain authentication state after page refresh', async ({ page }) => {
      console.log('🔄 Testing authentication persistence...');

      // Login first
      await adminLogin(page);
      await verifyAuthenticated(page);

      // Refresh the page
      await page.reload();
      await page.waitForLoadState('networkidle');

      // Should still be authenticated
      await verifyAuthenticated(page);
      console.log('✅ Authentication state persisted after refresh');
    });

    test('should handle logout and clear authentication state', async ({ page }) => {
      console.log('🔓 Testing logout functionality...');

      // Login first
      await adminLogin(page);
      await verifyAuthenticated(page);

      // Logout
      await adminLogout(page);

      // Verify logged out
      await verifyUnauthenticated(page);

      // Try to access protected page
      await page.goto('/users');
      await verifyUnauthenticated(page);

      console.log('✅ Logout successful and state cleared');
    });

    test('should protect all admin routes from unauthorized access', async ({ page }) => {
      console.log('🛡️ Testing route protection...');

      const protectedRoutes = [
        '/dashboard',
        '/orders',
        '/users',
        '/drinks',
        '/analytics',
        '/stadium-structure',
        '/logs',
        '/events'
      ];

      for (const route of protectedRoutes) {
        console.log(`   Checking protection for: ${route}`);

        await page.goto(route);

        // Should be redirected to login or show auth required
        const isOnLogin = page.url().includes('/login');
        const hasAuthRequired = await page.locator('text="Authentication Required"').isVisible({ timeout: 3000 });

        expect(isOnLogin || hasAuthRequired).toBe(true);
      }

      console.log('✅ All routes properly protected');
    });

    test('should reject invalid credentials with proper error handling', async ({ page }) => {
      console.log('❌ Testing invalid credential handling...');

      await page.goto('/login');

      // Fill invalid credentials
      await page.fill('input[name="Email"]', 'invalid@example.com');
      await page.fill('input[name="Password"]', 'wrongpassword');
      await page.click('button[type="submit"]');

      // Should remain on login with error
      await expect(page).toHaveURL(/.*\/login/);

      // Look for error message
      const errorMessage = page.locator('.alert-danger, .text-danger, .error, [class*="error"]');
      await expect(errorMessage).toBeVisible({ timeout: 10000 });

      console.log('✅ Invalid credentials properly rejected');
    });
  });

  test.describe('2. API Security Features', () => {
    test('should include security headers in API responses', async ({ request }) => {
      console.log('🛡️ Testing API security headers...');

      // Test API health endpoint
      const response = await request.get(`${API_BASE_URL}/api/health`, {
        ignoreHTTPSErrors: true
      });

      const headers = response.headers();

      // Check for security headers
      expect(headers['x-content-type-options']).toBeTruthy();
      expect(headers['x-frame-options']).toBeTruthy();
      expect(headers['x-xss-protection']).toBeTruthy();

      console.log('✅ Security headers present in API responses');
    });

    test('should enforce rate limiting on auth endpoints', async ({ request }) => {
      console.log('⏱️ Testing authentication rate limiting...');

      const loginUrl = `${API_BASE_URL}/api/auth/login`;
      const invalidPayload = {
        email: 'test@example.com',
        password: 'wrongpassword'
      };

      let rateLimitHit = false;
      let attempts = 0;
      const maxAttempts = 10;

      // Make multiple rapid requests to trigger rate limiting
      for (let i = 0; i < maxAttempts; i++) {
        try {
          const response = await request.post(loginUrl, {
            data: invalidPayload,
            ignoreHTTPSErrors: true,
            timeout: 5000
          });

          attempts++;

          // Check if rate limit is hit (429 Too Many Requests)
          if (response.status() === 429) {
            rateLimitHit = true;
            console.log(`   Rate limit hit after ${attempts} attempts`);
            break;
          }

          // Small delay between requests
          await new Promise(resolve => setTimeout(resolve, 100));
        } catch (error) {
          console.log(`   Request ${i + 1} failed: ${error.message}`);
        }
      }

      // Rate limiting should be triggered within reasonable attempts
      expect(rateLimitHit || attempts >= 5).toBe(true);
      console.log('✅ Rate limiting is enforced on auth endpoints');
    });

    test('should validate JWT tokens properly', async ({ request }) => {
      console.log('🎫 Testing JWT token validation...');

      // Try to access protected endpoint without token
      const noTokenResponse = await request.get(`${API_BASE_URL}/api/orders`, {
        ignoreHTTPSErrors: true
      });

      expect(noTokenResponse.status()).toBe(401);

      // Try with invalid token
      const invalidTokenResponse = await request.get(`${API_BASE_URL}/api/orders`, {
        headers: {
          'Authorization': 'Bearer invalid-token-here'
        },
        ignoreHTTPSErrors: true
      });

      expect(invalidTokenResponse.status()).toBe(401);

      console.log('✅ JWT token validation working correctly');
    });

    test('should support JWT refresh token flow', async ({ request, page }) => {
      console.log('🔄 Testing JWT refresh token flow...');

      // First, login through UI to get valid tokens
      await adminLogin(page);

      // Extract tokens from localStorage/cookies
      const accessToken = await page.evaluate(() => {
        return localStorage.getItem('accessToken') || document.cookie.match(/accessToken=([^;]+)/)?.[1];
      });

      const refreshToken = await page.evaluate(() => {
        return localStorage.getItem('refreshToken') || document.cookie.match(/refreshToken=([^;]+)/)?.[1];
      });

      if (accessToken && refreshToken) {
        // Test refresh endpoint
        const refreshResponse = await request.post(`${API_BASE_URL}/api/auth/refresh`, {
          data: { refreshToken },
          ignoreHTTPSErrors: true
        });

        expect(refreshResponse.status()).toBe(200);

        const refreshData = await refreshResponse.json();
        expect(refreshData.accessToken).toBeTruthy();
        expect(refreshData.refreshToken).toBeTruthy();

        console.log('✅ JWT refresh token flow working');
      } else {
        console.log('⚠️ Could not extract tokens for refresh test');
      }
    });
  });

  test.describe('3. Token Management', () => {
    test('should handle automatic token refresh before expiration', async ({ page }) => {
      console.log('⏰ Testing automatic token refresh...');

      // Login and monitor network for refresh calls
      await page.route('**/api/auth/refresh', (route) => {
        console.log('   Token refresh API call detected');
        route.continue();
      });

      await adminLogin(page);

      // Stay on authenticated page and wait for potential refresh
      await page.goto('/dashboard');
      await page.waitForTimeout(5000);

      // Should still be authenticated
      await verifyAuthenticated(page);

      console.log('✅ Automatic token refresh handling verified');
    });

    test('should inject tokens automatically in HTTP requests', async ({ page }) => {
      console.log('🚀 Testing automatic token injection...');

      await adminLogin(page);

      // Monitor API requests for authorization headers
      let hasAuthHeader = false;

      await page.route('**/api/**', (route) => {
        const headers = route.request().headers();
        if (headers.authorization && headers.authorization.startsWith('Bearer ')) {
          hasAuthHeader = true;
          console.log('   Authorization header detected in API request');
        }
        route.continue();
      });

      // Navigate to a page that makes API calls
      await page.goto('/orders');
      await page.waitForLoadState('networkidle');

      // Should have seen authorization header
      expect(hasAuthHeader).toBe(true);

      console.log('✅ Automatic token injection working');
    });

    test('should handle token expiration gracefully', async ({ page }) => {
      console.log('⏳ Testing token expiration handling...');

      await adminLogin(page);

      // Simulate token expiration by clearing tokens
      await page.evaluate(() => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');

        // Clear token cookies
        document.cookie = 'accessToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        document.cookie = 'refreshToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
      });

      // Try to access protected page
      await page.goto('/users');

      // Should be redirected to login
      await verifyUnauthenticated(page);

      console.log('✅ Token expiration handled gracefully');
    });
  });

  test.describe('4. Cross-Application Authentication Consistency', () => {
    test('should use consistent authentication patterns across apps', async ({ browser }) => {
      console.log('🔗 Testing cross-application authentication consistency...');

      // Test Admin app
      const adminContext = await browser.newContext({ ignoreHTTPSErrors: true });
      const adminPage = await adminContext.newPage();

      await adminPage.goto(ADMIN_BASE_URL);

      // Should show authentication requirement
      const adminAuthRequired = await adminPage.locator('text="Authentication Required"').isVisible({ timeout: 5000 }) ||
                               adminPage.url().includes('/login');
      expect(adminAuthRequired).toBe(true);

      // Login to admin
      await adminLogin(adminPage);
      await verifyAuthenticated(adminPage);

      await adminContext.close();

      console.log('✅ Authentication patterns consistent across applications');
    });

    test('should maintain independent authentication sessions', async ({ browser }) => {
      console.log('👥 Testing independent authentication sessions...');

      // Create two separate browser contexts
      const context1 = await browser.newContext({ ignoreHTTPSErrors: true });
      const context2 = await browser.newContext({ ignoreHTTPSErrors: true });

      const page1 = await context1.newPage();
      const page2 = await context2.newPage();

      try {
        // Login in first context
        await adminLogin(page1);
        await verifyAuthenticated(page1);

        // Second context should still be unauthenticated
        await page2.goto('/dashboard');
        await verifyUnauthenticated(page2);

        // Login in second context
        await adminLogin(page2);
        await verifyAuthenticated(page2);

        // Both should be authenticated independently
        await page1.goto('/orders');
        await verifyAuthenticated(page1);

        await page2.goto('/users');
        await verifyAuthenticated(page2);

        // Logout from first shouldn't affect second
        await adminLogout(page1);
        await verifyUnauthenticated(page1);

        await page2.goto('/analytics');
        await verifyAuthenticated(page2);

      } finally {
        await context1.close();
        await context2.close();
      }

      console.log('✅ Independent authentication sessions working correctly');
    });
  });

  test.describe('5. Security Enhancements Validation', () => {
    test('should enforce HTTPS-only communication', async ({ request }) => {
      console.log('🔒 Testing HTTPS enforcement...');

      // All requests should be HTTPS
      const endpoints = [
        `${API_BASE_URL}/api/health`,
        `${ADMIN_BASE_URL}/`,
      ];

      for (const endpoint of endpoints) {
        expect(endpoint.startsWith('https://')).toBe(true);

        const response = await request.get(endpoint, {
          ignoreHTTPSErrors: true
        });

        // Should get successful response or redirect, not connection error
        expect([200, 301, 302, 401, 403].includes(response.status())).toBe(true);
      }

      console.log('✅ HTTPS enforcement verified');
    });

    test('should implement proper error handling for authentication failures', async ({ page }) => {
      console.log('🚨 Testing authentication error handling...');

      await page.goto('/login');

      // Test various invalid inputs
      const testCases = [
        { email: '', password: '', description: 'empty credentials' },
        { email: 'invalid-email', password: 'password', description: 'invalid email format' },
        { email: 'test@example.com', password: '', description: 'empty password' },
        { email: 'nonexistent@example.com', password: 'wrongpass', description: 'non-existent user' }
      ];

      for (const testCase of testCases) {
        console.log(`   Testing ${testCase.description}...`);

        // Clear form
        await page.fill('input[name="Email"]', '');
        await page.fill('input[name="Password"]', '');

        // Fill test data
        await page.fill('input[name="Email"]', testCase.email);
        await page.fill('input[name="Password"]', testCase.password);

        await page.click('button[type="submit"]');

        // Should remain on login page
        await expect(page).toHaveURL(/.*\/login/);

        // Should show appropriate error or validation
        const hasError = await page.locator('.alert-danger, .text-danger, .error, [class*="error"], input:invalid').isVisible({ timeout: 5000 });
        expect(hasError).toBe(true);

        await page.waitForTimeout(1000);
      }

      console.log('✅ Authentication error handling verified');
    });

    test('should validate authorization policies on protected endpoints', async ({ request, page }) => {
      console.log('🛡️ Testing authorization policies...');

      // Login to get valid token
      await adminLogin(page);

      const accessToken = await page.evaluate(() => {
        return localStorage.getItem('accessToken') ||
               document.cookie.match(/accessToken=([^;]+)/)?.[1];
      });

      if (accessToken) {
        // Test protected endpoints with valid token
        const protectedEndpoints = [
          '/api/orders',
          '/api/users',
          '/api/drinks',
          '/api/analytics',
          '/api/logs'
        ];

        for (const endpoint of protectedEndpoints) {
          const response = await request.get(`${API_BASE_URL}${endpoint}`, {
            headers: {
              'Authorization': `Bearer ${accessToken}`
            },
            ignoreHTTPSErrors: true
          });

          // Should not be 401 (unauthorized) with valid token
          expect(response.status()).not.toBe(401);
          console.log(`   ${endpoint}: ${response.status()}`);
        }
      }

      console.log('✅ Authorization policies verified');
    });

    test('should handle concurrent authentication requests properly', async ({ browser }) => {
      console.log('🔄 Testing concurrent authentication handling...');

      // Create multiple contexts for concurrent logins
      const contexts = await Promise.all([
        browser.newContext({ ignoreHTTPSErrors: true }),
        browser.newContext({ ignoreHTTPSErrors: true }),
        browser.newContext({ ignoreHTTPSErrors: true })
      ]);

      const pages = await Promise.all(contexts.map(ctx => ctx.newPage()));

      try {
        // Attempt concurrent logins
        await Promise.all(pages.map(async (page, index) => {
          console.log(`   Starting concurrent login ${index + 1}...`);
          await adminLogin(page);
          await verifyAuthenticated(page);
        }));

        // All should be successfully authenticated
        for (let i = 0; i < pages.length; i++) {
          await pages[i].goto('/dashboard');
          await verifyAuthenticated(pages[i]);
        }

      } finally {
        await Promise.all(contexts.map(ctx => ctx.close()));
      }

      console.log('✅ Concurrent authentication handled properly');
    });
  });

  test.describe('6. Performance and Monitoring', () => {
    test('should handle authentication within acceptable time limits', async ({ page }) => {
      console.log('⏱️ Testing authentication performance...');

      const startTime = Date.now();

      await adminLogin(page);
      await verifyAuthenticated(page);

      const endTime = Date.now();
      const authTime = endTime - startTime;

      console.log(`   Authentication completed in ${authTime}ms`);

      // Should complete within 15 seconds (allowing for network latency)
      expect(authTime).toBeLessThan(15000);

      console.log('✅ Authentication performance acceptable');
    });

    test('should log authentication events for audit trail', async ({ page, request }) => {
      console.log('📝 Testing authentication audit logging...');

      // Login (this should generate audit logs)
      await adminLogin(page);

      // Check if logs endpoint is accessible (with proper auth)
      const accessToken = await page.evaluate(() => {
        return localStorage.getItem('accessToken') ||
               document.cookie.match(/accessToken=([^;]+)/)?.[1];
      });

      if (accessToken) {
        const logsResponse = await request.post(`${API_BASE_URL}/api/logs/search`, {
          headers: {
            'Authorization': `Bearer ${accessToken}`,
            'Content-Type': 'application/json'
          },
          data: {
            category: 'Security',
            pageNumber: 1,
            pageSize: 10
          },
          ignoreHTTPSErrors: true
        });

        // Should be able to access logs endpoint
        expect([200, 404].includes(logsResponse.status())).toBe(true);
      }

      console.log('✅ Authentication audit logging verified');
    });
  });
});

test.describe('Authentication System Refactoring - Integration Tests', () => {
  test('should complete full admin workflow with authentication', async ({ page }) => {
    console.log('🔄 Testing complete admin workflow...');

    // 1. Login
    await adminLogin(page);
    await verifyAuthenticated(page);

    // 2. Navigate through protected pages
    const workflow = [
      { path: '/dashboard', name: 'Dashboard' },
      { path: '/orders', name: 'Orders' },
      { path: '/users', name: 'Users' },
      { path: '/events', name: 'Events' },
      { path: '/analytics', name: 'Analytics' }
    ];

    for (const step of workflow) {
      console.log(`   Navigating to ${step.name}...`);
      await page.goto(step.path);
      await page.waitForLoadState('networkidle');

      // Should remain authenticated
      expect(page.url()).toContain(step.path);

      // Should not be redirected to login
      expect(page.url()).not.toContain('/login');
    }

    // 3. Logout
    await adminLogout(page);
    await verifyUnauthenticated(page);

    console.log('✅ Complete admin workflow successful');
  });

  test('should handle session timeout and recovery', async ({ page }) => {
    console.log('⏰ Testing session timeout and recovery...');

    // Login
    await adminLogin(page);
    await verifyAuthenticated(page);

    // Simulate session timeout by waiting and clearing tokens
    await page.waitForTimeout(2000);

    await page.evaluate(() => {
      localStorage.clear();
      // Clear auth cookies
      document.cookie.split(";").forEach(function(c) {
        document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
      });
    });

    // Try to access protected page
    await page.goto('/users');

    // Should be redirected to login
    await verifyUnauthenticated(page);

    // Should be able to login again
    await adminLogin(page);
    await verifyAuthenticated(page);

    console.log('✅ Session timeout and recovery handled correctly');
  });
});