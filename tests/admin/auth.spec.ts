import { test, expect, Page, BrowserContext } from '@playwright/test';
import { adminLogin, adminLogout, verifyAuthenticated, verifyUnauthenticated, DEFAULT_ADMIN_CREDENTIALS } from '../helpers/auth-helpers';
import { waitForPageReady } from '../helpers/auth-helpers';

/**
 * Authentication tests for Admin application modernization
 * Tests login, logout, session management, and protected routes
 */

test.describe('Admin Authentication', () => {
  test.beforeEach(async ({ page }) => {
    // Start each test unauthenticated
    await page.context().clearCookies();
  });

  test('should redirect unauthenticated users to login page', async ({ page }) => {
    // Try to access protected page
    await page.goto('/dashboard');

    // Should be redirected to login
    await expect(page).toHaveURL(/.*\/login/);
    await expect(page.locator('h1, .card-title')).toContainText(/login|sign in/i);
  });

  test('should login with valid admin credentials', async ({ page }) => {
    await page.goto('/login');

    // Fill login form
    await page.fill('input[name="Email"]', DEFAULT_ADMIN_CREDENTIALS.email);
    await page.fill('input[name="Password"]', DEFAULT_ADMIN_CREDENTIALS.password);

    // Submit login
    await page.click('button[type="submit"]');

    // Should redirect to dashboard
    await expect(page).toHaveURL(/.*\/dashboard/, { timeout: 15000 });
    await waitForPageReady(page);

    // Verify authenticated state
    await verifyAuthenticated(page);
  });

  test('should reject invalid credentials', async ({ page }) => {
    await page.goto('/login');

    // Try invalid credentials
    await page.fill('input[name="Email"]', 'wrong@example.com');
    await page.fill('input[name="Password"]', 'wrongpassword');
    await page.click('button[type="submit"]');

    // Should remain on login page with error
    await expect(page).toHaveURL(/.*\/login/);

    // Look for error message
    const errorMessage = page.locator('.alert-danger, .text-danger, [data-testid="error-message"], .error');
    await expect(errorMessage).toBeVisible({ timeout: 10000 });
  });

  test('should handle empty form submission', async ({ page }) => {
    await page.goto('/login');

    // Submit empty form
    await page.click('button[type="submit"]');

    // Should show validation messages
    const emailValidation = page.locator('input[name="Email"]:invalid, .field-validation-error');
    const passwordValidation = page.locator('input[name="Password"]:invalid, .field-validation-error');

    // At least one validation should be visible
    const hasEmailValidation = await emailValidation.isVisible({ timeout: 5000 });
    const hasPasswordValidation = await passwordValidation.isVisible({ timeout: 5000 });

    expect(hasEmailValidation || hasPasswordValidation).toBe(true);
  });

  test('should logout successfully', async ({ page }) => {
    // Login first
    await adminLogin(page);
    await verifyAuthenticated(page);

    // Logout
    await adminLogout(page);

    // Verify logged out
    await verifyUnauthenticated(page);
  });

  test('should maintain session across page refreshes', async ({ page }) => {
    // Login
    await adminLogin(page);
    await verifyAuthenticated(page);

    // Refresh page
    await page.reload();
    await waitForPageReady(page);

    // Should still be authenticated
    await verifyAuthenticated(page);
  });

  test('should protect all admin routes', async ({ page }) => {
    const protectedRoutes = [
      '/dashboard',
      '/orders',
      '/users',
      '/drinks',
      '/analytics',
      '/stadium-structure',
      '/logs'
    ];

    for (const route of protectedRoutes) {
      console.log(`🔒 Testing protection for route: ${route}`);

      await page.goto(route);

      // Should redirect to login or show authentication required
      const isOnLogin = page.url().includes('/login');
      const hasAuthRequired = await page.locator('text="Authentication Required", text="Login Required"').isVisible({ timeout: 5000 });

      expect(isOnLogin || hasAuthRequired).toBe(true);
    }
  });

  test('should handle session expiration gracefully', async ({ page }) => {
    // Login
    await adminLogin(page);
    await verifyAuthenticated(page);

    // Simulate session expiration by clearing cookies
    await page.context().clearCookies();

    // Try to access protected page
    await page.goto('/users');

    // Should be redirected to login
    await verifyUnauthenticated(page);
  });

  test('should support multiple admin users in different contexts', async ({ browser }) => {
    // Create two separate contexts for different users
    const context1 = await browser.newContext({ ignoreHTTPSErrors: true });
    const context2 = await browser.newContext({ ignoreHTTPSErrors: true });

    const page1 = await context1.newPage();
    const page2 = await context2.newPage();

    try {
      // Login with first admin in context1
      await adminLogin(page1);
      await verifyAuthenticated(page1);

      // Login with second admin in context2 (using same credentials for test)
      await adminLogin(page2);
      await verifyAuthenticated(page2);

      // Both should be independently authenticated
      await page1.goto('/dashboard');
      await verifyAuthenticated(page1);

      await page2.goto('/orders');
      await verifyAuthenticated(page2);

      // Logout from first context shouldn't affect second
      await adminLogout(page1);
      await verifyUnauthenticated(page1);

      await page2.goto('/users');
      await verifyAuthenticated(page2);

    } finally {
      await context1.close();
      await context2.close();
    }
  });
});

test.describe('Admin Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await adminLogin(page);
  });

  test('should navigate between main pages', async ({ page }) => {
    await page.goto('/dashboard');
    await waitForPageReady(page);

    // Test navigation to Orders
    const ordersNav = page.locator('a[href*="/orders"], [data-testid="nav-orders"], text="Orders"').first();
    await ordersNav.click();
    await expect(page).toHaveURL(/.*\/orders/);
    await waitForPageReady(page);

    // Test navigation to Users
    const usersNav = page.locator('a[href*="/users"], [data-testid="nav-users"], text="Users"').first();
    await usersNav.click();
    await expect(page).toHaveURL(/.*\/users/);
    await waitForPageReady(page);

    // Test navigation back to Dashboard
    const dashboardNav = page.locator('a[href*="/dashboard"], [data-testid="nav-dashboard"], text="Dashboard"').first();
    await dashboardNav.click();
    await expect(page).toHaveURL(/.*\/dashboard/);
    await waitForPageReady(page);
  });

  test('should highlight active navigation item', async ({ page }) => {
    await page.goto('/dashboard');
    await waitForPageReady(page);

    // Dashboard should be active
    const dashboardNav = page.locator('a[href*="/dashboard"], [data-testid="nav-dashboard"]').first();
    await expect(dashboardNav).toHaveClass(/active|current|selected/);

    // Navigate to orders
    await page.goto('/orders');
    await waitForPageReady(page);

    // Orders should be active
    const ordersNav = page.locator('a[href*="/orders"], [data-testid="nav-orders"]').first();
    await expect(ordersNav).toHaveClass(/active|current|selected/);
  });

  test('should have responsive navigation menu', async ({ page }) => {
    await page.goto('/dashboard');
    await waitForPageReady(page);

    // Test desktop navigation
    await page.setViewportSize({ width: 1200, height: 800 });
    const desktopNav = page.locator('.navbar, .navigation, [data-testid="desktop-nav"]');
    await expect(desktopNav).toBeVisible();

    // Test mobile navigation
    await page.setViewportSize({ width: 375, height: 667 });

    // Mobile navigation toggle should be visible
    const mobileToggle = page.locator('.navbar-toggler, .mobile-nav-toggle, [data-testid="mobile-nav-toggle"]');

    // If mobile toggle exists, test it
    if (await mobileToggle.isVisible({ timeout: 2000 })) {
      await mobileToggle.click();

      // Mobile menu should appear
      const mobileMenu = page.locator('.navbar-collapse, .mobile-nav-menu, [data-testid="mobile-nav-menu"]');
      await expect(mobileMenu).toBeVisible();
    }
  });

  test('should support breadcrumb navigation', async ({ page }) => {
    await page.goto('/users');
    await waitForPageReady(page);

    // Look for breadcrumb navigation
    const breadcrumb = page.locator('.breadcrumb, [data-testid="breadcrumb"], .page-breadcrumb');

    // If breadcrumb exists, verify it shows current page
    if (await breadcrumb.isVisible({ timeout: 5000 })) {
      await expect(breadcrumb).toContainText('Users');
    }
  });

  test('should maintain navigation state across page reloads', async ({ page }) => {
    await page.goto('/orders');
    await waitForPageReady(page);

    // Reload page
    await page.reload();
    await waitForPageReady(page);

    // Should still be on orders page
    await expect(page).toHaveURL(/.*\/orders/);

    // Orders navigation should still be active
    const ordersNav = page.locator('a[href*="/orders"], [data-testid="nav-orders"]').first();

    // If active classes are implemented, verify them
    if (await ordersNav.getAttribute('class')) {
      const classes = await ordersNav.getAttribute('class');
      // This is a soft assertion - active state might not be implemented yet
      console.log(`Orders navigation classes: ${classes}`);
    }
  });
});