import { Page } from '@playwright/test';
import { testConfig } from '../config';
import { waitForPageInteractive } from './blazor-helpers';
import { AdminLoginPage, CustomerLoginPage, StaffLoginPage } from '../pages/LoginPage';

/**
 * Login as admin user
 */
export async function loginAsAdmin(page: Page, email?: string, password?: string) {
  const loginPage = new AdminLoginPage(page);
  await loginPage.navigate();
  await loginPage.login(
    email || testConfig.credentials.admin.email,
    password || testConfig.credentials.admin.password
  );
  await page.waitForURL(testConfig.adminApp + '/', { timeout: testConfig.timeouts.navigation });
  await waitForPageInteractive(page);
}

/**
 * Login as customer user
 */
export async function loginAsCustomer(page: Page, email?: string, password?: string) {
  const loginPage = new CustomerLoginPage(page);
  await loginPage.navigate();
  await loginPage.login(
    email || testConfig.credentials.customer.email,
    password || testConfig.credentials.customer.password
  );
  await page.waitForURL(testConfig.customerApp + '/', { timeout: testConfig.timeouts.navigation });
  await waitForPageInteractive(page);
}

/**
 * Login as staff user
 */
export async function loginAsStaff(page: Page, email?: string, password?: string) {
  const loginPage = new StaffLoginPage(page);
  await loginPage.navigate();
  await loginPage.login(
    email || testConfig.credentials.staff.email,
    password || testConfig.credentials.staff.password
  );
  await page.waitForURL(testConfig.staffApp + '/', { timeout: testConfig.timeouts.navigation });
  await waitForPageInteractive(page);
}

/**
 * Check if user is authenticated by looking for logout button or user menu
 */
export async function isAuthenticated(page: Page): Promise<boolean> {
  try {
    // Check for common authentication indicators
    const logoutButton = page.locator('button:has-text("Logout"), button:has-text("Sign Out")');
    const userMenu = page.locator('[class*="user-menu"], [class*="user-dropdown"]');
    
    const logoutVisible = await logoutButton.isVisible().catch(() => false);
    const menuVisible = await userMenu.isVisible().catch(() => false);
    
    return logoutVisible || menuVisible;
  } catch {
    return false;
  }
}

/**
 * Logout from the application
 */
export async function logout(page: Page) {
  // Try to find and click logout button
  const logoutSelectors = [
    'button:has-text("Logout")',
    'button:has-text("Sign Out")',
    'a:has-text("Logout")',
    'a:has-text("Sign Out")'
  ];
  
  for (const selector of logoutSelectors) {
    try {
      const element = page.locator(selector);
      if (await element.isVisible()) {
        await element.click();
        await page.waitForLoadState('networkidle');
        return;
      }
    } catch {
      // Try next selector
    }
  }
  
  // If no logout button found, navigate directly to logout endpoint
  const baseUrl = page.url().split('/').slice(0, 3).join('/');
  await page.goto(baseUrl + '/logout');
  await page.waitForLoadState('networkidle');
}

/**
 * Ensure user is not authenticated
 */
export async function ensureLoggedOut(page: Page) {
  if (await isAuthenticated(page)) {
    await logout(page);
  }
}

/**
 * Get authentication token from local storage or session storage
 */
export async function getAuthToken(page: Page): Promise<string | null> {
  return await page.evaluate(() => {
    // Try localStorage first
    const localToken = localStorage.getItem('authToken') || localStorage.getItem('token');
    if (localToken) return localToken;
    
    // Try sessionStorage
    const sessionToken = sessionStorage.getItem('authToken') || sessionStorage.getItem('token');
    return sessionToken;
  });
}

/**
 * Set authentication token in storage
 */
export async function setAuthToken(page: Page, token: string) {
  await page.evaluate((token) => {
    localStorage.setItem('authToken', token);
    sessionStorage.setItem('authToken', token);
  }, token);
}

/**
 * Clear all authentication data
 */
export async function clearAuthData(page: Page) {
  await page.evaluate(() => {
    localStorage.clear();
    sessionStorage.clear();
    // Clear cookies
    document.cookie.split(";").forEach((c) => {
      document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
    });
  });
}