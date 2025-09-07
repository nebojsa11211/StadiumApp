import { test, expect } from '@playwright/test';
import { AdminLoginPage, StaffLoginPage, CustomerMenuPage } from './pages/LoginPage';
import { testConfig } from './config';
import { waitForPageInteractive, navigateToPage } from './helpers/blazor-helpers';

test.describe('Corrected Login Tests', () => {
  
  test.describe('Admin Login Tests', () => {
    test('should display admin login page correctly', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.expectLoginForm();
    });

    test('should login admin successfully', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      
      await loginPage.login(
        testConfig.credentials.admin.email,
        testConfig.credentials.admin.password
      );
      
      // Wait for redirect to dashboard
      await page.waitForURL(testConfig.adminApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Verify we're on the admin dashboard
      await expect(page.locator('h1:has-text("Stadium Admin")')).toBeVisible();
      await expect(page.locator('button:has-text("Logout")')).toBeVisible();
    });

    test('should show error for invalid admin credentials', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      
      await loginPage.login('invalid@email.com', 'wrongpassword');
      
      // Wait a moment for error to appear
      await page.waitForTimeout(2000);
      
      // Check if we're still on login page or if there's an error message
      const currentUrl = page.url();
      expect(currentUrl).toContain('/login');
    });
  });

  test.describe('Staff Login Tests', () => {
    test('should display staff login page correctly after navigation from protected page', async ({ page }) => {
      // Navigate to staff app root (will show authentication required)
      await page.goto(testConfig.staffApp);
      
      // Wait for the authentication required message
      await expect(page.locator('h5:has-text("Authentication Required")')).toBeVisible();
      
      // Click Go to Login
      await page.click('button:has-text("Go to Login")');
      
      // Now verify login form
      const loginPage = new StaffLoginPage(page);
      await expect(page.locator(loginPage.titleSelector)).toContainText(loginPage.expectedTitle);
      await expect(page.locator(loginPage.emailSelector)).toBeVisible();
      await expect(page.locator(loginPage.passwordSelector)).toBeVisible();
      await expect(page.locator(loginPage.submitSelector)).toBeVisible();
    });

    test('should login staff successfully', async ({ page }) => {
      // Navigate to staff login directly
      await navigateToPage(page, testConfig.staffApp + '/login');
      
      const loginPage = new StaffLoginPage(page);
      await loginPage.login(
        testConfig.credentials.staff.email,
        testConfig.credentials.staff.password
      );
      
      // Wait for redirect to dashboard
      await page.waitForURL(testConfig.staffApp + '/', { timeout: 15000 });
      await waitForPageInteractive(page);
      
      // Verify we're logged in (check for staff-specific content)
      await expect(page.locator('body')).not.toContainText('Authentication Required');
    });
  });

  test.describe('Customer App Tests (No Login Required)', () => {
    test('should access customer menu without login', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      await menuPage.expectMenuVisible();
    });

    test('should display drink items in menu', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Check for various drink types
      await expect(page.locator('h5:has-text("Beer")')).toBeVisible();
      await expect(page.locator('h5:has-text("Coca Cola")')).toBeVisible();
      await expect(page.locator('h5:has-text("Water")')).toBeVisible();
      await expect(page.locator('h5:has-text("Coffee")')).toBeVisible();
    });

    test('should show cart information', async ({ page }) => {
      const menuPage = new CustomerMenuPage(page);
      await menuPage.navigate();
      
      // Check cart section exists
      await expect(page.locator('h5:has-text("Your Order")')).toBeVisible();
      await expect(page.locator('text=Your cart is empty')).toBeVisible();
    });
  });

  test.describe('API Health Checks', () => {
    test('should verify all app endpoints are accessible', async ({ request }) => {
      // Test API endpoint
      const apiResponse = await request.get(`${testConfig.api}/health`);
      expect(apiResponse.status()).toBe(200);

      // Test Admin app
      const adminResponse = await request.get(testConfig.adminApp);
      expect(adminResponse.status()).toBe(200);

      // Test Customer app  
      const customerResponse = await request.get(testConfig.customerApp);
      expect(customerResponse.status()).toBe(200);

      // Test Staff app
      const staffResponse = await request.get(testConfig.staffApp);
      expect(staffResponse.status()).toBe(200);
    });
  });
});