import { Page, BrowserContext, expect } from '@playwright/test';

/**
 * Authentication helper functions for Admin modernization tests
 */

export interface AdminCredentials {
  email: string;
  password: string;
}

export const DEFAULT_ADMIN_CREDENTIALS: AdminCredentials = {
  email: 'admin@stadium.com',
  password: 'admin123'
};

/**
 * Logs in an admin user and returns to the dashboard
 */
export async function adminLogin(page: Page, credentials: AdminCredentials = DEFAULT_ADMIN_CREDENTIALS) {
  console.log(`🔐 Logging in admin user: ${credentials.email}`);

  // Navigate to admin application
  await page.goto('/');
  await page.waitForTimeout(3000);

  // Check if already logged in by looking for dashboard elements
  const logoutButton = page.locator('button:has-text("Logout")');
  const dashboardElements = page.locator('button:has-text("Refresh"), a[href*="/orders"], a[href*="/analytics"]');

  if (await logoutButton.isVisible({ timeout: 2000 }) || await dashboardElements.first().isVisible({ timeout: 2000 })) {
    console.log('✅ Admin already logged in, navigating to dashboard');

    // Navigate to dashboard if not already there
    const currentUrl = page.url();
    if (!currentUrl.includes('/dashboard')) {
      await page.goto('/dashboard');
    }

    await page.waitForLoadState('networkidle');
    console.log('✅ Admin login verified');
    return;
  }

  // Check if we need to click "Go to Login" button
  const goToLoginButton = page.locator('button:has-text("Go to Login")');
  if (await goToLoginButton.isVisible({ timeout: 5000 })) {
    await goToLoginButton.click();
    await page.waitForTimeout(2000);

    // Should be on login page now
    await expect(page).toHaveURL(/.*\/login/, { timeout: 10000 });

    // Fill in credentials using the correct selectors
    const emailInput = page.locator('#admin-login-email-input');
    const passwordInput = page.locator('#admin-login-password-input');

    if (await emailInput.isVisible({ timeout: 5000 })) {
      await emailInput.fill(credentials.email);
    } else {
      // Fallback to name-based selectors
      await page.fill('input[name="Email"], input[type="email"]', credentials.email);
    }

    if (await passwordInput.isVisible({ timeout: 5000 })) {
      await passwordInput.fill(credentials.password);
    } else {
      // Fallback to name-based selectors
      await page.fill('input[name="Password"], input[type="password"]', credentials.password);
    }

    // Submit login form
    const submitButton = page.locator('button[type="submit"]').first();
    await submitButton.click();

    // Wait for successful login redirect
    await page.waitForTimeout(3000);
  }

  // Ensure we end up on dashboard
  const currentUrl = page.url();
  if (!currentUrl.includes('/dashboard')) {
    await page.goto('/dashboard');
  }

  await page.waitForLoadState('networkidle');
  console.log('✅ Admin login successful');
}

/**
 * Logs out the current admin user
 */
export async function adminLogout(page: Page) {
  console.log('🔓 Logging out admin user');

  // Look for logout button or dropdown
  try {
    // First try direct logout button
    const logoutButton = page.locator('[data-testid="logout-button"]');
    if (await logoutButton.isVisible({ timeout: 2000 })) {
      await logoutButton.click();
    } else {
      // Try dropdown menu approach
      const userMenu = page.locator('[data-testid="user-menu"]');
      if (await userMenu.isVisible({ timeout: 2000 })) {
        await userMenu.click();
        await page.locator('[data-testid="logout-menu-item"]').click();
      } else {
        // Fallback to navbar logout
        await page.click('text="Logout"');
      }
    }

    // Verify redirect to login page
    await expect(page).toHaveURL(/.*\/login/, { timeout: 10000 });
    console.log('✅ Admin logout successful');
  } catch (error) {
    console.warn('⚠️ Could not find standard logout mechanism, trying alternative');
    // Alternative logout by clearing cookies and navigating
    await page.context().clearCookies();
    await page.goto('/login');
    await expect(page).toHaveURL(/.*\/login/);
  }
}

/**
 * Verifies that the user is authenticated and on the dashboard
 */
export async function verifyAuthenticated(page: Page) {
  // Should be on dashboard
  await expect(page).toHaveURL(/.*\/dashboard/);

  // Should see authenticated elements
  await expect(page.locator('[data-testid="user-menu"], .navbar .dropdown, text="Dashboard"')).toBeVisible({ timeout: 5000 });
}

/**
 * Verifies that the user is not authenticated and sees login form
 */
export async function verifyUnauthenticated(page: Page) {
  // Should be on login page or see login form
  const isOnLoginPage = page.url().includes('/login');
  const hasLoginForm = await page.locator('input[name="Email"], input[type="email"]').isVisible({ timeout: 5000 });

  if (!isOnLoginPage && !hasLoginForm) {
    throw new Error('Expected to be on login page or see login form, but neither condition was met');
  }
}

/**
 * Creates a new browser context with authentication
 */
export async function createAuthenticatedContext(page: Page): Promise<BrowserContext> {
  // Login in current page to establish session
  await adminLogin(page);

  // Return the context with authentication cookies
  return page.context();
}

/**
 * Checks if current page requires authentication
 */
export async function isAuthenticationRequired(page: Page): Promise<boolean> {
  const url = page.url();
  const hasLoginForm = await page.locator('input[name="Email"], input[type="email"]').isVisible({ timeout: 2000 });
  const hasAuthRequired = await page.locator('text="Authentication Required", text="Login Required"').isVisible({ timeout: 2000 });

  return url.includes('/login') || hasLoginForm || hasAuthRequired;
}

/**
 * Waits for page to be fully loaded and interactive
 */
export async function waitForPageReady(page: Page) {
  // Wait for network to be idle
  await page.waitForLoadState('networkidle');

  // Wait for any loading skeletons to disappear
  try {
    await page.waitForSelector('[data-testid="loading-skeleton"]', { state: 'hidden', timeout: 10000 });
  } catch {
    // Loading skeleton might not exist, that's fine
  }

  // Wait for main content to be visible
  await page.waitForSelector('main, .main-content, [data-testid="page-content"]', { timeout: 10000 });
}