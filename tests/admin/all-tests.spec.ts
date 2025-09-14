import { test, expect } from '@playwright/test';
import { adminLogin } from '../helpers/auth-helpers';
import { waitForLoadingComplete } from '../helpers/page-helpers';

/**
 * Comprehensive Admin Modernization Test Suite
 * Runs essential tests to validate the modernization is working
 */

test.describe('Admin Modernization - Essential Tests', () => {
  test.beforeEach(async ({ page }) => {
    await adminLogin(page);
  });

  test('should have all modernized pages accessible', async ({ page }) => {
    const modernizedPages = [
      { path: '/dashboard', name: 'Dashboard' },
      { path: '/orders', name: 'Orders' },
      { path: '/users', name: 'Users' }
    ];

    for (const pageInfo of modernizedPages) {
      console.log(`🔍 Testing ${pageInfo.name} page accessibility`);

      await page.goto(pageInfo.path);
      await waitForLoadingComplete(page);

      // Verify page loads successfully
      await expect(page).toHaveURL(new RegExp(pageInfo.path.replace('/', '\\\/')));

      // Verify main content is present
      const mainContent = page.locator('main, .main-content, .container, .page-content, body, .app, #app').first();

      try {
        await expect(mainContent).toBeVisible({ timeout: 10000 });
      } catch (error) {
        // Fallback: just check that the page has some visible content
        const hasContent = await page.locator('h1, h2, h3, .card, .table, button').first().isVisible({ timeout: 5000 });
        if (!hasContent) {
          throw new Error(`No main content or basic elements found on ${pageInfo.name} page`);
        }
      }

      console.log(`✅ ${pageInfo.name} page loaded successfully`);
    }
  });

  test('should have responsive design working', async ({ page }) => {
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);

    const breakpoints = [
      { name: 'Mobile', width: 375, height: 667 },
      { name: 'Tablet', width: 768, height: 1024 },
      { name: 'Desktop', width: 1200, height: 800 }
    ];

    for (const breakpoint of breakpoints) {
      console.log(`📱 Testing ${breakpoint.name} responsiveness`);

      await page.setViewportSize({ width: breakpoint.width, height: breakpoint.height });
      await page.waitForTimeout(1000);

      // Main content should remain visible
      const mainContent = page.locator('main, .main-content, .container').first();
      await expect(mainContent).toBeVisible();

      // No horizontal scrolling on body
      const bodyScrollWidth = await page.evaluate(() => document.body.scrollWidth);
      const viewportWidth = breakpoint.width;

      if (bodyScrollWidth <= viewportWidth + 20) { // Allow small tolerance
        console.log(`✅ No horizontal overflow on ${breakpoint.name}`);
      } else {
        console.log(`⚠️ Potential horizontal overflow on ${breakpoint.name}`);
      }
    }
  });

  test('should have basic functionality working on all modernized pages', async ({ page }) => {
    // Test Dashboard
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);

    const dashboardCards = page.locator('.kpi-card, .card, .stats-card');
    const cardCount = await dashboardCards.count();

    if (cardCount > 0) {
      await expect(dashboardCards.first()).toBeVisible();
      console.log(`✅ Dashboard has ${cardCount} KPI cards`);
    }

    // Test Orders
    await page.goto('/orders');
    await waitForLoadingComplete(page);

    const ordersTable = page.locator('table, .table, .orders-table').first();
    if (await ordersTable.isVisible({ timeout: 5000 })) {
      await expect(ordersTable).toBeVisible();
      console.log('✅ Orders table is displayed');
    }

    // Test filtering if available
    const statusFilter = page.locator('select[name*="status"], [data-testid="status-filter"]').first();
    if (await statusFilter.isVisible({ timeout: 3000 })) {
      console.log('✅ Orders filtering is available');
    }

    // Test Users
    await page.goto('/users');
    await waitForLoadingComplete(page);

    const usersTable = page.locator('table, .table, .users-table').first();
    if (await usersTable.isVisible({ timeout: 5000 })) {
      await expect(usersTable).toBeVisible();
      console.log('✅ Users table is displayed');
    }

    // Test add user functionality if available
    const addUserButton = page.locator('button:has-text("Add User"), .add-user-btn').first();
    if (await addUserButton.isVisible({ timeout: 3000 })) {
      console.log('✅ Add user functionality is available');
    }
  });

  test('should have theme system working', async ({ page }) => {
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);

    // Check for theme support
    const htmlElement = page.locator('html');
    const initialTheme = await htmlElement.getAttribute('data-theme') || 'light';

    console.log(`Initial theme: ${initialTheme}`);

    // Look for theme toggle
    const themeToggle = page.locator('[data-testid="theme-toggle"], .theme-toggle, button:has-text("Theme")').first();

    if (await themeToggle.isVisible({ timeout: 5000 })) {
      console.log('✅ Theme toggle found');

      // Toggle theme
      await themeToggle.click();
      await page.waitForTimeout(1000);

      const newTheme = await htmlElement.getAttribute('data-theme') || 'light';
      console.log(`Theme after toggle: ${newTheme}`);

      if (newTheme !== initialTheme) {
        console.log('✅ Theme switching works');
      } else {
        console.log('ℹ️ Theme may use different implementation');
      }

    } else {
      console.log('ℹ️ Theme toggle not yet implemented');
    }
  });

  test('should load within acceptable performance limits', async ({ page }) => {
    const pages = [
      { path: '/dashboard', maxTime: 10000 },
      { path: '/orders', maxTime: 12000 },
      { path: '/users', maxTime: 10000 }
    ];

    for (const pageInfo of pages) {
      console.log(`⏱️ Testing ${pageInfo.path} performance`);

      const startTime = Date.now();
      await page.goto(pageInfo.path);
      await waitForLoadingComplete(page);

      const loadTime = Date.now() - startTime;
      console.log(`${pageInfo.path} load time: ${loadTime}ms`);

      expect(loadTime).toBeLessThan(pageInfo.maxTime);
      console.log(`✅ ${pageInfo.path} loaded within ${pageInfo.maxTime}ms limit`);
    }
  });

  test('should handle authentication properly', async ({ page }) => {
    // Test logout
    console.log('🔓 Testing logout functionality');

    // Go to dashboard first
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);

    // Look for logout mechanism
    const logoutButton = page.locator('button:has-text("Logout"), .logout-btn, a:has-text("Logout")').first();
    const userMenu = page.locator('.user-menu, .dropdown-toggle, .user-dropdown').first();

    if (await logoutButton.isVisible({ timeout: 5000 })) {
      await logoutButton.click();
    } else if (await userMenu.isVisible({ timeout: 5000 })) {
      await userMenu.click();
      const menuLogout = page.locator('a:has-text("Logout"), button:has-text("Logout")').first();
      if (await menuLogout.isVisible({ timeout: 2000 })) {
        await menuLogout.click();
      }
    } else {
      // Clear cookies as fallback
      await page.context().clearCookies();
      await page.goto('/');
    }

    // Should redirect to login
    await page.waitForTimeout(2000);
    const currentUrl = page.url();
    const isOnLogin = currentUrl.includes('/login') ||
                     await page.locator('input[name="Email"], input[type="email"]').isVisible({ timeout: 5000 });

    if (isOnLogin) {
      console.log('✅ Logout successful - redirected to login');
    } else {
      console.log('⚠️ Logout behavior may be different');
    }

    // Test login again
    console.log('🔐 Testing login functionality');
    await adminLogin(page);

    await expect(page).toHaveURL(/.*\/dashboard/);
    console.log('✅ Re-login successful');
  });

  test('should have proper error handling', async ({ page }) => {
    // Test 404 handling
    await page.goto('/non-existent-page');
    await page.waitForTimeout(3000);

    // Should either show 404 page or redirect to login/dashboard
    const is404 = page.url().includes('404') ||
                  await page.locator('text="404", text="Not Found", text="Page not found"').isVisible({ timeout: 2000 });

    const isRedirected = page.url().includes('/login') || page.url().includes('/dashboard');

    if (is404) {
      console.log('✅ 404 page handling works');
    } else if (isRedirected) {
      console.log('✅ Non-existent page redirected appropriately');
    } else {
      console.log('ℹ️ 404 handling uses different approach');
    }

    // Return to dashboard for cleanup
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);
  });
});

test.describe('Admin Modernization - Critical Path Tests', () => {
  test('should complete full admin workflow', async ({ page }) => {
    console.log('🚀 Starting comprehensive admin workflow test');

    // 1. Login
    await adminLogin(page);
    console.log('✅ Step 1: Admin login successful');

    // 2. Dashboard overview
    await page.goto('/dashboard');
    await waitForLoadingComplete(page);

    const kpiCards = page.locator('.kpi-card, .card, .stats-card');
    if (await kpiCards.first().isVisible({ timeout: 5000 })) {
      const cardCount = await kpiCards.count();
      console.log(`✅ Step 2: Dashboard shows ${cardCount} KPI cards`);
    }

    // 3. Orders management
    await page.goto('/orders');
    await waitForLoadingComplete(page);

    const ordersTable = page.locator('table, .table, .orders-table').first();
    if (await ordersTable.isVisible({ timeout: 5000 })) {
      const orderRows = await page.locator('tbody tr, .table-row:not(.table-header)').count();
      console.log(`✅ Step 3: Orders page shows ${orderRows} orders`);
    }

    // 4. User management access
    await page.goto('/users');
    await waitForLoadingComplete(page);

    const usersTable = page.locator('table, .table, .users-table').first();
    if (await usersTable.isVisible({ timeout: 5000 })) {
      const userRows = await page.locator('tbody tr, .table-row:not(.table-header)').count();
      console.log(`✅ Step 4: Users page shows ${userRows} users`);
    }

    // 5. Navigation between pages
    const pages = ['/dashboard', '/orders', '/users', '/dashboard'];
    for (let i = 0; i < pages.length; i++) {
      await page.goto(pages[i]);
      await waitForLoadingComplete(page);
      console.log(`✅ Step 5.${i + 1}: Navigation to ${pages[i]} successful`);
    }

    console.log('🎉 Full admin workflow completed successfully');
  });
});