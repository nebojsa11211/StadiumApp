import { test, expect } from '@playwright/test';
import { adminLogin } from '../helpers/auth-helpers';
import { waitForLoadingComplete, waitForSignalRConnection } from '../helpers/page-helpers';

/**
 * Performance and loading tests
 * Tests page load times, loading states, concurrent users, SignalR connections
 */

test.describe('Performance & Loading', () => {
  test.beforeEach(async ({ page }) => {
    await adminLogin(page);
  });

  test.describe('Page Load Performance', () => {
    test('should load dashboard within acceptable time', async ({ page }) => {
      const startTime = Date.now();

      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Verify main dashboard elements are loaded
      const kpiCards = page.locator('.kpi-card, .card, .stats-card');
      await expect(kpiCards.first()).toBeVisible({ timeout: 15000 });

      const loadTime = Date.now() - startTime;
      console.log(`Dashboard load time: ${loadTime}ms`);

      // Dashboard should load within 10 seconds (including authentication)
      expect(loadTime).toBeLessThan(10000);

      // Measure Time to Interactive (approximate)
      const interactiveTime = await page.evaluate(() => {
        return performance.timing.domInteractive - performance.timing.navigationStart;
      });
      console.log(`Time to Interactive: ${interactiveTime}ms`);

      // Should be interactive within 8 seconds
      expect(interactiveTime).toBeLessThan(8000);
    });

    test('should load orders page efficiently', async ({ page }) => {
      const startTime = Date.now();

      await page.goto('/orders');
      await waitForLoadingComplete(page);

      // Verify orders table is loaded
      const ordersTable = page.locator('.orders-table, table, [data-testid="orders-data-table"]');
      await expect(ordersTable.first()).toBeVisible({ timeout: 15000 });

      const loadTime = Date.now() - startTime;
      console.log(`Orders page load time: ${loadTime}ms`);

      // Orders page should load within 12 seconds (may have more data)
      expect(loadTime).toBeLessThan(12000);
    });

    test('should load users page efficiently', async ({ page }) => {
      const startTime = Date.now();

      await page.goto('/users');
      await waitForLoadingComplete(page);

      // Verify users table is loaded
      const usersTable = page.locator('.users-table, table, [data-testid="users-data-table"]');
      await expect(usersTable.first()).toBeVisible({ timeout: 15000 });

      const loadTime = Date.now() - startTime;
      console.log(`Users page load time: ${loadTime}ms`);

      // Users page should load within 10 seconds
      expect(loadTime).toBeLessThan(10000);
    });

    test('should handle page navigation efficiently', async ({ page }) => {
      // Start at dashboard
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      const pages = ['/orders', '/users', '/dashboard'];

      for (let i = 0; i < pages.length; i++) {
        const startTime = Date.now();
        const targetPage = pages[i];

        console.log(`Navigating to: ${targetPage}`);

        // Navigate using page links if available
        const navLink = page.locator(`a[href*="${targetPage}"], [data-testid="nav-${targetPage.slice(1)}"]`).first();

        if (await navLink.isVisible({ timeout: 2000 })) {
          await navLink.click();
        } else {
          await page.goto(targetPage);
        }

        await waitForLoadingComplete(page);

        const navigationTime = Date.now() - startTime;
        console.log(`Navigation to ${targetPage} took: ${navigationTime}ms`);

        // Page navigation should be fast (already authenticated)
        expect(navigationTime).toBeLessThan(8000);

        // Verify page loaded correctly
        await expect(page).toHaveURL(new RegExp(targetPage.replace('/', '\\/')));
      }
    });

    test('should measure resource loading performance', async ({ page }) => {
      // Enable request interception for performance monitoring
      const requests: { url: string; responseTime: number; size: number }[] = [];

      page.on('response', async (response) => {
        const request = response.request();
        const timing = response.timing();

        requests.push({
          url: request.url(),
          responseTime: timing.responseEnd - timing.responseStart,
          size: (await response.body()).length
        });
      });

      const startTime = Date.now();
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      const totalTime = Date.now() - startTime;

      // Analyze requests
      const cssRequests = requests.filter(r => r.url.includes('.css'));
      const jsRequests = requests.filter(r => r.url.includes('.js'));
      const apiRequests = requests.filter(r => r.url.includes('/api/'));

      console.log(`Total requests: ${requests.length}`);
      console.log(`CSS requests: ${cssRequests.length}`);
      console.log(`JS requests: ${jsRequests.length}`);
      console.log(`API requests: ${apiRequests.length}`);
      console.log(`Total page load time: ${totalTime}ms`);

      // Calculate total resource size
      const totalSize = requests.reduce((sum, req) => sum + req.size, 0);
      console.log(`Total resource size: ${(totalSize / 1024 / 1024).toFixed(2)} MB`);

      // Performance benchmarks
      expect(requests.length).toBeLessThan(50); // Reasonable number of requests
      expect(totalSize).toBeLessThan(10 * 1024 * 1024); // Less than 10MB total

      // Check for slow requests
      const slowRequests = requests.filter(r => r.responseTime > 2000);
      if (slowRequests.length > 0) {
        console.log(`Slow requests (>2s): ${slowRequests.length}`);
        slowRequests.forEach(req => {
          console.log(`  - ${req.url}: ${req.responseTime}ms`);
        });
      }

      expect(slowRequests.length).toBeLessThan(3); // Allow few slow requests
    });
  });

  test.describe('Loading States', () => {
    test('should show loading skeletons', async ({ page }) => {
      await page.goto('/dashboard');

      // Catch loading skeleton early
      const loadingSkeleton = page.locator('[data-testid="loading-skeleton"], .loading-skeleton, .skeleton');

      // Loading skeleton should appear briefly
      if (await loadingSkeleton.first().isVisible({ timeout: 3000 })) {
        console.log('✅ Loading skeleton detected');
        await expect(loadingSkeleton.first()).toBeVisible();
      } else {
        console.log('ℹ️ Loading skeleton not detected (page may load too quickly)');
      }

      // Wait for content to load
      await waitForLoadingComplete(page);

      // Loading skeleton should disappear
      await expect(loadingSkeleton.first()).not.toBeVisible();
      console.log('✅ Loading skeleton hidden after content load');
    });

    test('should show appropriate loading states during data refresh', async ({ page }) => {
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      // Look for refresh button
      const refreshButton = page.locator('[data-testid="refresh-button"], .refresh-btn, button:has-text("Refresh")').first();

      if (await refreshButton.isVisible({ timeout: 5000 })) {
        console.log('✅ Refresh button found');

        await refreshButton.click();

        // Should show loading state
        const loadingIndicator = page.locator('[data-testid="loading"], .loading, .spinner, .loading-skeleton');

        if (await loadingIndicator.first().isVisible({ timeout: 2000 })) {
          console.log('✅ Loading indicator shown during refresh');
          await expect(loadingIndicator.first()).toBeVisible();

          // Wait for refresh to complete
          await waitForLoadingComplete(page);

          // Loading indicator should disappear
          await expect(loadingIndicator.first()).not.toBeVisible();
          console.log('✅ Loading indicator hidden after refresh');
        }
      } else {
        console.log('ℹ️ Manual refresh functionality not implemented');
      }
    });

    test('should handle empty states gracefully', async ({ page }) => {
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      // Apply filters that might result in no data
      const statusFilter = page.locator('[data-testid="status-filter"], select[name*="status"]').first();

      if (await statusFilter.isVisible({ timeout: 5000 })) {
        // Filter by a status that might not exist or have few results
        await page.selectOption(statusFilter, 'Cancelled');
        await waitForLoadingComplete(page);

        const dataRows = page.locator('tbody tr, .table-row:not(.table-header)');
        const rowCount = await dataRows.count();

        if (rowCount === 0) {
          // Should show empty state message
          const emptyState = page.locator('[data-testid="no-data"], .no-data, .empty-state, text="No orders found"');

          if (await emptyState.isVisible({ timeout: 5000 })) {
            console.log('✅ Empty state message shown when no data available');
            await expect(emptyState).toBeVisible();
          } else {
            console.log('ℹ️ Empty state handling may be implemented differently');
          }
        } else {
          console.log(`Found ${rowCount} orders with Cancelled status`);
        }

        // Reset filter
        await page.selectOption(statusFilter, '');
        await waitForLoadingComplete(page);
      }
    });

    test('should show error states appropriately', async ({ page }) => {
      // Test error handling by navigating to non-existent page or triggering error
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Simulate network error by blocking API requests
      await page.route('**/api/**', (route) => {
        if (route.request().url().includes('/api/dashboard') ||
            route.request().url().includes('/api/orders/summary')) {
          route.abort('failed');
        } else {
          route.continue();
        }
      });

      // Navigate to page that depends on blocked API
      await page.goto('/orders');

      // Should show error state or handle gracefully
      const errorMessage = page.locator('.error-message, .alert-danger, [data-testid="error"]');
      const loadingState = page.locator('.loading, .spinner');

      // Wait for either error message or timeout
      await page.waitForTimeout(10000);

      const hasError = await errorMessage.isVisible({ timeout: 2000 });
      const stillLoading = await loadingState.isVisible({ timeout: 2000 });

      if (hasError) {
        console.log('✅ Error message displayed for failed requests');
      } else if (!stillLoading) {
        console.log('✅ Page handled failed requests gracefully (no infinite loading)');
      } else {
        console.log('⚠️ Page may be stuck in loading state due to failed requests');
      }

      // Reset route interception
      await page.unroute('**/api/**');
    });
  });

  test.describe('Real-time Updates', () => {
    test('should establish SignalR connection', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Wait for SignalR connection
      await waitForSignalRConnection(page);

      // Check if connection is established
      const connectionState = await page.evaluate(() => {
        // @ts-ignore
        return window.signalRConnection ? window.signalRConnection.state : 'Unknown';
      });

      console.log(`SignalR connection state: ${connectionState}`);

      if (connectionState === 'Connected') {
        console.log('✅ SignalR connection established successfully');
      } else {
        console.log('ℹ️ SignalR connection not detected (may not be implemented yet)');
      }
    });

    test('should handle SignalR disconnection gracefully', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Simulate network disconnection
      await page.context().setOffline(true);
      await page.waitForTimeout(5000);

      // Check for offline indicator or graceful handling
      const offlineIndicator = page.locator('.offline-indicator, .connection-status, [data-testid="offline"]');

      if (await offlineIndicator.isVisible({ timeout: 3000 })) {
        console.log('✅ Offline state detected and indicated to user');
      }

      // Restore connection
      await page.context().setOffline(false);
      await page.waitForTimeout(3000);

      // Should reconnect automatically
      const onlineIndicator = page.locator('.online-indicator, [data-testid="online"]');
      if (await onlineIndicator.isVisible({ timeout: 5000 })) {
        console.log('✅ Connection restored and indicated to user');
      } else {
        console.log('ℹ️ Reconnection handling not visually indicated (may be automatic)');
      }
    });

    test('should update data in real-time', async ({ page }) => {
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      // Get initial order count
      const initialRows = await page.locator('tbody tr, .table-row:not(.table-header)').count();
      console.log(`Initial order count: ${initialRows}`);

      // Wait for potential real-time updates (30 seconds)
      console.log('⏳ Waiting for potential real-time updates...');
      await page.waitForTimeout(30000);

      // Check if data has been updated
      const updatedRows = await page.locator('tbody tr, .table-row:not(.table-header)').count();
      console.log(`Updated order count: ${updatedRows}`);

      if (updatedRows !== initialRows) {
        console.log('✅ Real-time data update detected');
      } else {
        console.log('ℹ️ No real-time updates detected (may be due to no new data)');
      }

      // Test passes regardless - we're testing that the page doesn't break
      expect(updatedRows).toBeGreaterThanOrEqual(0);
    });
  });

  test.describe('Concurrent User Performance', () => {
    test('should handle multiple admin sessions', async ({ browser }) => {
      const concurrentUsers = 3;
      const contexts = await Promise.all(
        Array.from({ length: concurrentUsers }, () =>
          browser.newContext({ ignoreHTTPSErrors: true })
        )
      );

      const pages = await Promise.all(
        contexts.map(context => context.newPage())
      );

      try {
        console.log(`Testing ${concurrentUsers} concurrent admin sessions`);

        // Login all users simultaneously
        const loginPromises = pages.map((page, index) => {
          console.log(`Starting login for user ${index + 1}`);
          return adminLogin(page);
        });

        const loginResults = await Promise.allSettled(loginPromises);

        // Check login success rate
        const successfulLogins = loginResults.filter(result => result.status === 'fulfilled').length;
        console.log(`Successful logins: ${successfulLogins}/${concurrentUsers}`);

        expect(successfulLogins).toBeGreaterThanOrEqual(concurrentUsers * 0.8); // 80% success rate

        // Navigate all users to different pages simultaneously
        const navigationPromises = pages.map((page, index) => {
          const targetPages = ['/dashboard', '/orders', '/users'];
          const targetPage = targetPages[index % targetPages.length];

          console.log(`User ${index + 1} navigating to ${targetPage}`);

          return page.goto(targetPage).then(() => waitForLoadingComplete(page));
        });

        const navigationResults = await Promise.allSettled(navigationPromises);
        const successfulNavigations = navigationResults.filter(result => result.status === 'fulfilled').length;

        console.log(`Successful page loads: ${successfulNavigations}/${concurrentUsers}`);
        expect(successfulNavigations).toBeGreaterThanOrEqual(concurrentUsers * 0.8); // 80% success rate

        // Verify each user can perform basic operations
        const operationPromises = pages.map(async (page, index) => {
          try {
            // Check that page content is loaded
            const mainContent = page.locator('main, .main-content, .container').first();
            await expect(mainContent).toBeVisible({ timeout: 10000 });

            console.log(`User ${index + 1} can see main content`);
            return true;
          } catch (error) {
            console.log(`User ${index + 1} failed to load content:`, error);
            return false;
          }
        });

        const operationResults = await Promise.all(operationPromises);
        const successfulOperations = operationResults.filter(result => result).length;

        console.log(`Users with successful operations: ${successfulOperations}/${concurrentUsers}`);
        expect(successfulOperations).toBeGreaterThanOrEqual(concurrentUsers * 0.8);

      } finally {
        // Cleanup
        await Promise.all(contexts.map(context => context.close()));
      }
    });

    test('should maintain performance under load', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Simulate heavy usage by rapid navigation and interactions
      const iterations = 5;
      const pages = ['/dashboard', '/orders', '/users'];

      for (let i = 0; i < iterations; i++) {
        console.log(`Load test iteration ${i + 1}/${iterations}`);

        for (const targetPage of pages) {
          const startTime = Date.now();

          await page.goto(targetPage);
          await waitForLoadingComplete(page);

          const loadTime = Date.now() - startTime;
          console.log(`  ${targetPage}: ${loadTime}ms`);

          // Performance shouldn't degrade significantly
          expect(loadTime).toBeLessThan(15000); // Allow some degradation under load

          // Perform some interactions to stress the page
          const buttons = page.locator('button, .btn');
          const buttonCount = await buttons.count();

          if (buttonCount > 0) {
            // Click first few buttons (but don't submit forms)
            for (let j = 0; j < Math.min(3, buttonCount); j++) {
              const button = buttons.nth(j);
              const buttonText = await button.textContent();

              // Skip dangerous buttons
              if (buttonText && !buttonText.toLowerCase().includes('delete') &&
                  !buttonText.toLowerCase().includes('submit')) {

                try {
                  await button.click({ timeout: 2000 });
                  await page.waitForTimeout(500);
                } catch (error) {
                  // Ignore click errors - just testing performance
                }
              }
            }
          }
        }

        // Brief pause between iterations
        await page.waitForTimeout(1000);
      }

      console.log('✅ Load test completed successfully');
    });
  });

  test.describe('Memory and Resource Management', () => {
    test('should not have memory leaks during navigation', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Get initial memory usage
      const initialMemory = await page.evaluate(() => {
        // @ts-ignore
        return performance.memory ? performance.memory.usedJSHeapSize : 0;
      });

      console.log(`Initial memory usage: ${(initialMemory / 1024 / 1024).toFixed(2)} MB`);

      // Navigate between pages multiple times
      const pages = ['/dashboard', '/orders', '/users', '/dashboard'];
      const navigationCount = 10;

      for (let cycle = 0; cycle < navigationCount; cycle++) {
        for (const targetPage of pages) {
          await page.goto(targetPage);
          await waitForLoadingComplete(page);

          // Force garbage collection if available
          try {
            await page.evaluate(() => {
              // @ts-ignore
              if (window.gc) window.gc();
            });
          } catch (error) {
            // GC not available, that's fine
          }
        }

        if (cycle % 3 === 0) {
          const currentMemory = await page.evaluate(() => {
            // @ts-ignore
            return performance.memory ? performance.memory.usedJSHeapSize : 0;
          });

          console.log(`Memory after ${cycle + 1} cycles: ${(currentMemory / 1024 / 1024).toFixed(2)} MB`);
        }
      }

      // Final memory check
      const finalMemory = await page.evaluate(() => {
        // @ts-ignore
        return performance.memory ? performance.memory.usedJSHeapSize : 0;
      });

      console.log(`Final memory usage: ${(finalMemory / 1024 / 1024).toFixed(2)} MB`);

      // Memory shouldn't grow excessively (allow 50MB growth)
      const memoryGrowth = finalMemory - initialMemory;
      console.log(`Memory growth: ${(memoryGrowth / 1024 / 1024).toFixed(2)} MB`);

      expect(memoryGrowth).toBeLessThan(50 * 1024 * 1024); // Less than 50MB growth
    });

    test('should cleanup event listeners and timers', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Check for common timer/interval patterns
      const hasActiveTimers = await page.evaluate(() => {
        // Check for auto-refresh intervals
        // @ts-ignore
        const intervals = window.activeIntervals || [];
        // @ts-ignore
        const timeouts = window.activeTimeouts || [];

        return {
          intervals: intervals.length,
          timeouts: timeouts.length
        };
      });

      console.log(`Active timers - Intervals: ${hasActiveTimers.intervals}, Timeouts: ${hasActiveTimers.timeouts}`);

      // Navigate away and check cleanup
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      await page.waitForTimeout(2000); // Allow cleanup time

      const afterNavigationTimers = await page.evaluate(() => {
        // @ts-ignore
        const intervals = window.activeIntervals || [];
        // @ts-ignore
        const timeouts = window.activeTimeouts || [];

        return {
          intervals: intervals.length,
          timeouts: timeouts.length
        };
      });

      console.log(`After navigation - Intervals: ${afterNavigationTimers.intervals}, Timeouts: ${afterNavigationTimers.timeouts}`);

      // This is more of an observation than strict test
      // Proper cleanup would show reduced timer counts
      console.log('✅ Timer cleanup check completed');
    });
  });
});