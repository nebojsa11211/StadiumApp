import { test, expect } from '@playwright/test';
import { testConfig, getAppUrl } from './config';

test.describe('Customer Authentication Flow', () => {
  test('should block unauthenticated access to ticket purchasing pages', async ({ page }) => {
    // Test 1: Events page should require authentication
    await page.goto(getAppUrl('customer', '/events'));
    await page.waitForLoadState('networkidle');
    
    // Should see authentication required message
    await expect(page.locator('h4')).toContainText('🔐 Authentication Required');
    await expect(page.locator('h5')).toContainText('Please Sign In');
    await expect(page.getByRole('button', { name: 'Sign In' })).toBeVisible();
    await expect(page.getByRole('button', { name: 'Create Account' })).toBeVisible();
    
    // Test 2: Event details page should require authentication
    await page.goto(getAppUrl('customer', '/events/1'));
    await page.waitForLoadState('networkidle');
    
    // Should see authentication required message
    await expect(page.locator('h4')).toContainText('🔐 Authentication Required');
    
    // Test 3: Checkout page should require authentication
    await page.goto(getAppUrl('customer', '/checkout'));
    await page.waitForLoadState('networkidle');
    
    // Should see authentication required message
    await expect(page.locator('h4')).toContainText('🔐 Authentication Required');
  });

  test('should allow authenticated users to access ticket purchasing pages', async ({ page }) => {
    // First login
    await page.goto(getAppUrl('customer', '/login'));
    await page.waitForLoadState('networkidle');
    
    // Fill in demo credentials using the correct selectors
    await page.fill('#customer-login-email-input', testConfig.credentials.customer.email);
    await page.fill('#customer-login-password-input', testConfig.credentials.customer.password);
    await page.click('#customer-login-submit-btn');
    
    // Wait for login to complete
    await page.waitForLoadState('networkidle');
    
    // Test authenticated access to events page
    await page.goto(getAppUrl('customer', '/events'));
    await page.waitForLoadState('networkidle');
    
    // Should see events list, not authentication message
    await expect(page.locator('h2')).toContainText('🎫 Available Events');
    await expect(page.getByRole('button', { name: 'Buy Tickets' }).first()).toBeVisible();
    
    // Test authenticated access to event details
    await page.click(page.getByRole('button', { name: 'Buy Tickets' }).first());
    await page.waitForLoadState('networkidle');
    
    // Should see event details page
    await expect(page.locator('h4').first()).toContainText('Championship Match');
    await expect(page.locator('h5')).toContainText('🏟️ Stadium Sections');
  });

  test('should redirect to login with return URL when accessing protected pages', async ({ page }) => {
    // Try to access events page when not authenticated
    await page.goto(getAppUrl('customer', '/events'));
    await page.waitForLoadState('networkidle');
    
    // Click Sign In button
    await page.click('button:has-text("Sign In")');
    await page.waitForLoadState('networkidle');
    
    // Should be on login page with return URL
    expect(page.url()).toContain('/login');
    expect(page.url()).toContain('returnUrl');
    
    // Fill credentials and login
    await page.fill('#customer-login-email-input', testConfig.credentials.customer.email);
    await page.fill('#customer-login-password-input', testConfig.credentials.customer.password);
    await page.click('#customer-login-submit-btn');
    
    // Wait for redirect
    await page.waitForLoadState('networkidle');
    
    // Should be redirected back to events page
    expect(page.url()).toContain('/events');
    await expect(page.locator('h2')).toContainText('🎫 Available Events');
  });
});