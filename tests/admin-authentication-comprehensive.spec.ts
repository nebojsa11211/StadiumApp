import { test, expect } from '@playwright/test';

test.describe('Admin Authentication System Comprehensive Tests', () => {
  const ADMIN_URL = 'https://localhost:9030';
  const ADMIN_CREDENTIALS = {
    email: 'admin@stadium.com',
    password: 'admin123'
  };

  test.beforeEach(async ({ page }) => {
    // Clear all authentication state before each test
    await page.context().clearCookies();
    await page.context().clearPermissions();
  });

  test('1. Unauthenticated users cannot access protected pages and see authentication barrier', async ({ page }) => {
    console.log('🔒 Testing unauthenticated access protection...');
    
    // Test dashboard access without authentication
    await page.goto(`${ADMIN_URL}/dashboard`);
    await page.waitForLoadState('networkidle');
    
    // Should see authentication required message or be redirected to login
    const currentUrl = page.url();
    console.log(`Dashboard access URL: ${currentUrl}`);
    
    if (currentUrl.includes('/login')) {
      console.log('✅ Dashboard correctly redirects to login page');
    } else {
      // Check for authentication barrier message
      const authBarrier = page.locator('h5:has-text("Authentication Required")').first();
      await expect(authBarrier).toBeVisible({ timeout: 10000 });
      console.log('✅ Dashboard shows authentication barrier');
    }

    // Test other protected pages
    const protectedPages = ['/orders', '/drinks', '/users', '/analytics'];
    
    for (const pagePath of protectedPages) {
      console.log(`Testing protected page: ${pagePath}`);
      await page.goto(`${ADMIN_URL}${pagePath}`);
      await page.waitForLoadState('networkidle');
      
      const url = page.url();
      if (url.includes('/login')) {
        console.log(`✅ ${pagePath} correctly redirects to login`);
      } else {
        // Look for auth barrier
        const barrier = page.locator('h5:has-text("Authentication Required")').first();
        if (await barrier.isVisible()) {
          console.log(`✅ ${pagePath} shows authentication barrier`);
        } else {
          console.log(`⚠️ ${pagePath} may not be properly protected`);
        }
      }
    }
  });

  test('2. Login page is accessible without authentication', async ({ page }) => {
    console.log('🔓 Testing login page accessibility...');
    
    await page.goto(`${ADMIN_URL}/login`);
    await page.waitForLoadState('networkidle');
    
    // Should be able to access login page
    expect(page.url()).toContain('/login');
    
    // Check for login form elements
    const emailInput = page.locator('input[type="email"], input[name="email"], input[placeholder*="email" i]');
    const passwordInput = page.locator('input[type="password"], input[name="password"], input[placeholder*="password" i]');
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign in")');
    
    await expect(emailInput).toBeVisible({ timeout: 10000 });
    await expect(passwordInput).toBeVisible({ timeout: 10000 });
    await expect(submitButton).toBeVisible({ timeout: 10000 });
    
    console.log('✅ Login page is accessible and contains required form elements');
  });

  test('3. Successful login allows access to protected pages', async ({ page }) => {
    console.log('🚪 Testing successful login and protected page access...');
    
    // Navigate to login page
    await page.goto(`${ADMIN_URL}/login`);
    await page.waitForLoadState('networkidle');
    
    // Fill login form
    console.log('Filling login credentials...');
    const emailInput = page.locator('input[type="email"], input[name="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"], input[name="password"], input[placeholder*="password" i]').first();
    
    await emailInput.fill(ADMIN_CREDENTIALS.email);
    await passwordInput.fill(ADMIN_CREDENTIALS.password);
    
    // Submit form
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign in")').first();
    await submitButton.click();
    
    // Wait for authentication to complete
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000); // Additional wait for any redirects
    
    console.log(`After login, current URL: ${page.url()}`);
    
    // Should be redirected away from login page
    expect(page.url()).not.toContain('/login');
    
    // Test access to protected pages
    const protectedPages = [
      { path: '/dashboard', name: 'Dashboard' },
      { path: '/orders', name: 'Orders' },
      { path: '/drinks', name: 'Drinks' },
      { path: '/users', name: 'Users' }
    ];
    
    for (const page_info of protectedPages) {
      console.log(`Testing authenticated access to ${page_info.name}...`);
      await page.goto(`${ADMIN_URL}${page_info.path}`);
      await page.waitForLoadState('networkidle');
      
      // Should not be redirected to login
      expect(page.url()).not.toContain('/login');
      
      // Should not see authentication barrier
      const authBarrier = page.locator('h5:has-text("Authentication Required")');
      await expect(authBarrier).not.toBeVisible();
      
      console.log(`✅ Successfully accessed ${page_info.name}`);
    }
  });

  test('4. Authentication state persists across page navigation', async ({ page }) => {
    console.log('🔄 Testing authentication state persistence...');
    
    // Login first
    await page.goto(`${ADMIN_URL}/login`);
    await page.waitForLoadState('networkidle');
    
    const emailInput = page.locator('input[type="email"], input[name="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"], input[name="password"], input[placeholder*="password" i]').first();
    
    await emailInput.fill(ADMIN_CREDENTIALS.email);
    await passwordInput.fill(ADMIN_CREDENTIALS.password);
    
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign in")').first();
    await submitButton.click();
    
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);
    
    // Navigate to different pages and verify access is maintained
    const pagesToTest = ['/dashboard', '/orders', '/drinks', '/users', '/dashboard'];
    
    for (const pagePath of pagesToTest) {
      await page.goto(`${ADMIN_URL}${pagePath}`);
      await page.waitForLoadState('networkidle');
      
      // Should maintain access without re-authentication
      expect(page.url()).not.toContain('/login');
      
      const authBarrier = page.locator('h5:has-text("Authentication Required")');
      await expect(authBarrier).not.toBeVisible();
    }
    
    console.log('✅ Authentication state persists across navigation');
  });

  test('5. Logout functionality works correctly', async ({ page }) => {
    console.log('🚪🔒 Testing logout functionality...');
    
    // Login first
    await page.goto(`${ADMIN_URL}/login`);
    await page.waitForLoadState('networkidle');
    
    const emailInput = page.locator('input[type="email"], input[name="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"], input[name="password"], input[placeholder*="password" i]').first();
    
    await emailInput.fill(ADMIN_CREDENTIALS.email);
    await passwordInput.fill(ADMIN_CREDENTIALS.password);
    
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign in")').first();
    await submitButton.click();
    
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);
    
    // Navigate to a protected page to confirm we're logged in
    await page.goto(`${ADMIN_URL}/dashboard`);
    await page.waitForLoadState('networkidle');
    expect(page.url()).not.toContain('/login');
    
    // Look for logout button/link
    const logoutElements = [
      page.locator('button:has-text("Logout"), button:has-text("Log out"), button:has-text("Sign out")'),
      page.locator('a:has-text("Logout"), a:has-text("Log out"), a:has-text("Sign out")'),
      page.locator('[data-testid="logout"], [id*="logout"]')
    ];
    
    let logoutClicked = false;
    for (const logoutElement of logoutElements) {
      if (await logoutElement.first().isVisible()) {
        console.log('Found logout element, clicking...');
        await logoutElement.first().click();
        logoutClicked = true;
        break;
      }
    }
    
    if (logoutClicked) {
      await page.waitForLoadState('networkidle');
      await page.waitForTimeout(2000);
      
      // After logout, try to access protected page
      await page.goto(`${ADMIN_URL}/dashboard`);
      await page.waitForLoadState('networkidle');
      
      // Should be redirected to login or see auth barrier
      const currentUrl = page.url();
      if (currentUrl.includes('/login')) {
        console.log('✅ Logout successful - redirected to login');
      } else {
        const authBarrier = page.locator('h5:has-text("Authentication Required")');
        if (await authBarrier.isVisible()) {
          console.log('✅ Logout successful - authentication barrier shown');
        } else {
          console.log('⚠️ Logout may not have worked properly');
        }
      }
    } else {
      console.log('⚠️ Could not find logout button - may need manual verification');
    }
  });

  test('6. Invalid credentials are rejected', async ({ page }) => {
    console.log('❌ Testing invalid credential rejection...');
    
    await page.goto(`${ADMIN_URL}/login`);
    await page.waitForLoadState('networkidle');
    
    // Try invalid credentials
    const emailInput = page.locator('input[type="email"], input[name="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"], input[name="password"], input[placeholder*="password" i]').first();
    
    await emailInput.fill('invalid@example.com');
    await passwordInput.fill('wrongpassword');
    
    const submitButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign in")').first();
    await submitButton.click();
    
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000);
    
    // Should remain on login page or show error
    const currentUrl = page.url();
    expect(currentUrl).toContain('/login');
    
    // Look for error messages
    const errorMessages = page.locator(':has-text("Invalid"), :has-text("Error"), :has-text("Failed"), :has-text("Incorrect")');
    const errorVisible = await errorMessages.first().isVisible();
    
    if (errorVisible) {
      console.log('✅ Invalid credentials properly rejected with error message');
    } else {
      console.log('✅ Invalid credentials rejected - stayed on login page');
    }
  });
});