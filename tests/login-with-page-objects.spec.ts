import { test, expect } from '@playwright/test';
import { AdminLoginPage, CustomerLoginPage, StaffLoginPage } from './pages/LoginPage';
import { testConfig } from './config';
import { loginAsAdmin, loginAsCustomer, loginAsStaff } from './helpers/auth-helpers';
import { waitForPageInteractive } from './helpers/blazor-helpers';

test.describe('Login Tests with Page Objects', () => {
  test.describe('Admin Login', () => {
    test('should display admin login page correctly', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.expectLoginForm();
    });

    test('should login admin successfully', async ({ page }) => {
      await loginAsAdmin(page);
      await expect(page).toHaveURL(testConfig.adminApp + '/');
    });

    test('should show error for invalid admin credentials', async ({ page }) => {
      const loginPage = new AdminLoginPage(page);
      await loginPage.navigate();
      await loginPage.login('wrong@email.com', 'wrongpassword');
      await loginPage.expectError('Invalid credentials');
    });
  });

  test.describe('Customer Login', () => {
    test('should display customer login page correctly', async ({ page }) => {
      const loginPage = new CustomerLoginPage(page);
      await loginPage.navigate();
      await loginPage.expectLoginForm();
      
      // Customer page has additional elements
      await expect(page.locator('#customer-login-register-link')).toBeVisible();
    });

    test('should login customer successfully', async ({ page }) => {
      await loginAsCustomer(page);
      await expect(page).toHaveURL(testConfig.customerApp + '/');
    });

    test('should navigate to registration page', async ({ page }) => {
      const loginPage = new CustomerLoginPage(page);
      await loginPage.navigate();
      await loginPage.clickRegisterLink();
      await expect(page).toHaveURL(testConfig.customerApp + '/register');
    });
  });

  test.describe('Staff Login', () => {
    test('should display staff login page correctly', async ({ page }) => {
      const loginPage = new StaffLoginPage(page);
      await loginPage.navigate();
      await loginPage.expectLoginForm();
    });

    test('should login staff successfully', async ({ page }) => {
      await loginAsStaff(page);
      await expect(page).toHaveURL(testConfig.staffApp + '/');
    });
  });
});

test.describe('API Health Checks', () => {
  test('should verify all app endpoints are accessible', async ({ request }) => {
    // Test API health
    const apiResponse = await request.get(`${testConfig.api}/health`);
    expect(apiResponse.ok()).toBeTruthy();
    
    // Test Admin app
    const adminResponse = await request.get(`${testConfig.adminApp}/`);
    expect(adminResponse.ok()).toBeTruthy();
    
    // Test Customer app
    const customerResponse = await request.get(`${testConfig.customerApp}/`);
    expect(customerResponse.ok()).toBeTruthy();
    
    // Test Staff app
    const staffResponse = await request.get(`${testConfig.staffApp}/`);
    expect(staffResponse.ok()).toBeTruthy();
  });
});