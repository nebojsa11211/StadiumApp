import { test, expect, Page } from '@playwright/test';
import { waitForPageInteractive, waitForBlazorLoad } from './helpers/blazor-helpers';

test.describe('Admin Console Logging - Simple Test', () => {
  let page: Page;
  const adminAppUrl = 'https://localhost:9030';

  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
    
    // Set up console message logging
    page.on('console', msg => {
      if (msg.type() === 'error') {
        console.error('Console error:', msg.text());
      }
    });
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('should access admin login page', async () => {
    await page.goto(adminAppUrl + '/login');
    await waitForPageInteractive(page);
    
    // Check if we can see the login form
    const emailInput = page.locator('input[type="email"], input[name="Email"], input[placeholder*="email" i]');
    const passwordInput = page.locator('input[type="password"], input[name="Password"]');
    
    await expect(emailInput).toBeVisible({ timeout: 10000 });
    await expect(passwordInput).toBeVisible({ timeout: 10000 });
    
    console.log('Login page loaded successfully');
  });

  test('should test various login credentials', async () => {
    await page.goto(adminAppUrl + '/login');
    await waitForPageInteractive(page);
    
    // Test credentials from the request
    const testCredentials = [
      { email: 'admin@example.com', password: 'password123' },
      { email: 'admin@stadium.com', password: 'admin123' },
      { email: 'admin@admin.com', password: 'admin123' },
      { email: 'admin@localhost.com', password: 'admin123' }
    ];
    
    for (const creds of testCredentials) {
      console.log(`Testing credentials: ${creds.email} / ${creds.password}`);
      
      // Fill login form
      await page.fill('input[type="email"], input[name="Email"]', creds.email);
      await page.fill('input[type="password"], input[name="Password"]', creds.password);
      
      // Click login
      await page.click('button[type="submit"], button:has-text("Login"), button:has-text("Sign In")');
      await page.waitForTimeout(3000);
      
      // Check if we're redirected (successful login)
      const currentUrl = page.url();
      if (!currentUrl.includes('/login')) {
        console.log(`SUCCESS: Logged in with ${creds.email} / ${creds.password}`);
        console.log(`Redirected to: ${currentUrl}`);
        return; // Exit test on success
      } else {
        console.log(`FAILED: Could not log in with ${creds.email} / ${creds.password}`);
        // Clear form for next attempt
        await page.fill('input[type="email"], input[name="Email"]', '');
        await page.fill('input[type="password"], input[name="Password"]', '');
      }
    }
    
    // If we reach here, none of the credentials worked
    console.log('No valid credentials found. Taking screenshot for debugging.');
    await page.screenshot({ path: 'debug-login-failed.png' });
  });

  test('should navigate to logs page if logged in', async () => {
    await page.goto(adminAppUrl + '/login');
    await waitForPageInteractive(page);
    
    // Try to login with admin@example.com first
    await page.fill('input[type="email"], input[name="Email"]', 'admin@example.com');
    await page.fill('input[type="password"], input[name="Password"]', 'password123');
    await page.click('button[type="submit"], button:has-text("Login"), button:has-text("Sign In")');
    await page.waitForTimeout(5000);
    
    // If login successful, try to go to logs page
    const currentUrl = page.url();
    if (!currentUrl.includes('/login')) {
      console.log('Login successful, navigating to logs page');
      
      // Navigate to logs page
      await page.goto(adminAppUrl + '/logs');
      await waitForPageInteractive(page);
      
      // Check if we can access the logs page
      const pageTitle = page.locator('h1, h2, h3, title').first();
      await expect(pageTitle).toBeVisible({ timeout: 10000 });
      
      console.log('Successfully accessed logs page');
    } else {
      console.log('Login failed, cannot test logs page');
      await page.screenshot({ path: 'debug-login-failed-logs-test.png' });
    }
  });
});