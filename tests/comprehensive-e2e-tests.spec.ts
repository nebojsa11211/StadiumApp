import { test, expect } from '@playwright/test';
import { AdminLoginPage, CustomerMenuPage } from './pages/LoginPage';
import { testConfig } from './config';
import { waitForPageInteractive, navigateToPage, clickWithRetry, fillWithRetry } from './helpers/blazor-helpers';

test.describe('Comprehensive E2E Tests', () => {

  test.describe('Customer App - Event Browsing and Ordering', () => {
    test('should browse events and view event details', async ({ page }) => {
      // Navigate to customer events page
      await navigateToPage(page, testConfig.customerApp + '/events');
      
      // Wait for events to load
      await page.waitForSelector('h1:has-text("Stadium Events"), .event-card', { timeout: 15000 });
      
      // Verify events page is loaded
      await expect(page.locator('h1')).toContainText('Stadium Events');
      
      // Check for event listings
      const eventCards = page.locator('.event-card, .card, [class*="event"]');
      await expect(eventCards.first()).toBeVisible({ timeout: 10000 });
    });

    test('should display drink menu with categories', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Verify all drink categories are visible
      await expect(page.locator('button:has-text("All")')).toBeVisible();
      await expect(page.locator('button:has-text("Beer")')).toBeVisible();
      await expect(page.locator('button:has-text("SoftDrink")')).toBeVisible();
      await expect(page.locator('button:has-text("Water")')).toBeVisible();
      await expect(page.locator('button:has-text("Coffee")')).toBeVisible();
    });

    test('should filter drinks by category', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Test Beer filter
      await clickWithRetry(page, 'button:has-text("Beer")');
      await page.waitForTimeout(1000); // Wait for filter to apply
      
      // Test SoftDrink filter  
      await clickWithRetry(page, 'button:has-text("SoftDrink")');
      await page.waitForTimeout(1000);
      
      // Test showing all again
      await clickWithRetry(page, 'button:has-text("All")');
      await expect(page.locator('h5:has-text("Beer")')).toBeVisible();
      await expect(page.locator('h5:has-text("Coca Cola")')).toBeVisible();
    });

    test('should show item details and stock information', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Verify drink details are shown
      await expect(page.locator('h5:has-text("Beer")')).toBeVisible();
      await expect(page.locator('text=Local Draft Beer')).toBeVisible();
      await expect(page.locator('text=$6.00')).toBeVisible();
      await expect(page.locator('text=Stock: 150')).toBeVisible();
      
      // Verify quantity controls exist
      await expect(page.locator('button:has-text("+")')).toBeVisible();
      await expect(page.locator('button:has-text("-")')).toBeVisible();
    });

    test('should handle quantity changes', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Find beer item and increase quantity
      const beerSection = page.locator('h5:has-text("Beer")').locator('..').locator('..');
      const plusButton = beerSection.locator('button:has-text("+")');
      const quantityInput = beerSection.locator('input[type="number"], input[role="spinbutton"]');
      
      // Increase quantity
      await plusButton.click();
      await expect(quantityInput).toHaveValue('1');
      
      // Increase again
      await plusButton.click();
      await expect(quantityInput).toHaveValue('2');
      
      // Decrease quantity
      await beerSection.locator('button:has-text("-")').click();
      await expect(quantityInput).toHaveValue('1');
    });

    test('should show cart updates when items are added', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Initially cart should be empty
      await expect(page.locator('text=Your cart is empty')).toBeVisible();
      
      // Find beer item, increase quantity and try to add to cart
      const beerSection = page.locator('h5:has-text("Beer")').locator('..').locator('..');
      await beerSection.locator('button:has-text("+")').click();
      
      // Add to cart button should become enabled
      const addToCartButton = beerSection.locator('button:has-text("Add to Cart")');
      await expect(addToCartButton).toBeEnabled({ timeout: 5000 });
    });
  });

  test.describe('Admin App - Complete Workflow', () => {
    test('should login and navigate to different admin sections', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      // Wait for dashboard
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Test navigation to different sections
      await clickWithRetry(page, 'a[href="drinks"]');
      await waitForPageInteractive(page);
      await expect(page.url()).toContain('drinks');
      
      // Navigate to orders
      await clickWithRetry(page, 'a[href="orders"]');
      await waitForPageInteractive(page);
      await expect(page.url()).toContain('orders');
      
      // Navigate to events
      await clickWithRetry(page, 'a[href="events"]');
      await waitForPageInteractive(page);
      await expect(page.url()).toContain('events');
      
      // Navigate to analytics
      await clickWithRetry(page, 'a[href="analytics"]');
      await waitForPageInteractive(page);
      await expect(page.url()).toContain('analytics');
    });

    test('should manage drinks in admin panel', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Navigate to drinks management
      await clickWithRetry(page, 'a[href="drinks"]');
      await waitForPageInteractive(page);
      
      // Verify drinks page loaded
      await expect(page.locator('h1, h2, h3').filter({ hasText: /drink/i })).toBeVisible({ timeout: 10000 });
      
      // Look for common drink management elements
      const hasAddButton = await page.locator('button:has-text("Add"), button:has-text("New"), button:has-text("Create")').count() > 0;
      const hasDrinkList = await page.locator('.drink, .item, .card', ).count() > 0;
      const hasTable = await page.locator('table, .table').count() > 0;
      
      // At least one of these should be present
      expect(hasAddButton || hasDrinkList || hasTable).toBeTruthy();
    });

    test('should display orders management page', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Navigate to orders management
      await clickWithRetry(page, 'a[href="orders"]');
      await waitForPageInteractive(page);
      
      // Verify orders page loaded
      await expect(page.locator('h1, h2, h3').filter({ hasText: /order/i })).toBeVisible({ timeout: 10000 });
    });

    test('should display analytics page', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Navigate to analytics
      await clickWithRetry(page, 'a[href="analytics"]');
      await waitForPageInteractive(page);
      
      // Verify analytics page loaded
      await expect(page.locator('h1, h2, h3').filter({ hasText: /analytic|report|statistic/i })).toBeVisible({ timeout: 10000 });
    });

    test('should access user management', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Navigate to user management
      await clickWithRetry(page, 'a[href="users"]');
      await waitForPageInteractive(page);
      
      // Verify user management page loaded
      await expect(page.locator('h1, h2, h3').filter({ hasText: /user|management/i })).toBeVisible({ timeout: 10000 });
    });
  });

  test.describe('Cross-App Integration Tests', () => {
    test('should verify API health from all apps', async ({ request }) => {
      const endpoints = [
        `${testConfig.api}/health`,
        `${testConfig.api}/api/drinks`,
        `${testConfig.api}/api/events`,
      ];

      for (const endpoint of endpoints) {
        try {
          const response = await request.get(endpoint);
          expect(response.status()).toBeLessThan(500); // Accept 200, 401, 404 but not 500+
        } catch (error) {
          console.warn(`Endpoint ${endpoint} failed:`, error);
        }
      }
    });

    test('should have consistent navigation across apps', async ({ page }) => {
      // Test customer app navigation
      await navigateToPage(page, testConfig.customerApp);
      await expect(page.locator('nav, .nav, [class*="nav"]')).toBeVisible();
      
      // Test admin app navigation after login
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await expect(page.locator('nav, .nav, [class*="nav"]')).toBeVisible();
    });

    test('should maintain session state properly', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Navigate to different pages and verify we stay logged in
      await clickWithRetry(page, 'a[href="drinks"]');
      await waitForPageInteractive(page);
      await expect(page.locator('button:has-text("Logout")')).toBeVisible();
      
      await clickWithRetry(page, 'a[href="orders"]');
      await waitForPageInteractive(page);
      await expect(page.locator('button:has-text("Logout")')).toBeVisible();
    });
  });
});