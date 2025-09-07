import { test, expect } from '@playwright/test';
import { testConfig, getAppUrl } from './config';

test.describe('Authentication Protection Verification', () => {
  test.use({ 
    storageState: { cookies: [], origins: [] } // Clear storage for clean test
  });

  test('should block all unauthenticated access to ticket purchasing pages', async ({ page }) => {
    console.log('Testing unauthenticated access protection...');
    
    // Test 1: Events page should require authentication
    console.log('Testing /events page...');
    await page.goto(getAppUrl('customer', '/events'));
    await page.waitForLoadState('networkidle'); // Better than waitForTimeout
    
    // Should see authentication required message, NOT events content
    const authHeading = page.locator('h4:has-text("🔐 Authentication Required")');
    const signInButton = page.locator('button:has-text("Sign In")');
    
    await expect(authHeading).toBeVisible();
    await expect(signInButton).toBeVisible();
    console.log('✅ Events page properly protected');
    
    // Test 2: Event details page should require authentication  
    console.log('Testing /events/1 page...');
    await page.goto(getAppUrl('customer', '/events/1'));
    await page.waitForLoadState('networkidle');
    
    await expect(page.locator('h4:has-text("🔐 Authentication Required")')).toBeVisible();
    console.log('✅ Event details page properly protected');
    
    // Test 3: Checkout page should require authentication
    console.log('Testing /checkout page...');
    await page.goto(getAppUrl('customer', '/checkout'));
    await page.waitForLoadState('networkidle');
    
    await expect(page.locator('h4:has-text("🔐 Authentication Required")')).toBeVisible();
    console.log('✅ Checkout page properly protected');
    
    // Test 4: Verify Sign In buttons work (redirect to login)
    console.log('Testing Sign In button redirect...');
    await page.click('button:has-text("Sign In")');
    await page.waitForLoadState('networkidle');
    
    // Should be redirected to login page
    expect(page.url()).toContain('/login');
    console.log('✅ Sign In button redirects to login page');
    
    console.log('🎉 All authentication protection tests PASSED!');
  });

  test('should allow access to non-protected pages', async ({ page }) => {
    console.log('Testing access to non-protected pages...');
    
    // Home page should be accessible
    await page.goto(getAppUrl('customer', '/'));
    await page.waitForLoadState('networkidle');
    
    // Should NOT see authentication required message
    await expect(page.locator('h4:has-text("🔐 Authentication Required")')).not.toBeVisible();
    await expect(page.locator('h1:has-text("🏟️ Welcome to Stadium Drinks")')).toBeVisible();
    console.log('✅ Home page accessible without authentication');
    
    // Login page should be accessible
    await page.goto(getAppUrl('customer', '/login'));
    await page.waitForLoadState('networkidle');
    
    // Using the actual ID from the Customer Login page
    await expect(page.locator('#customer-login-title')).toBeVisible();
    console.log('✅ Login page accessible without authentication');
    
    console.log('🎉 Non-protected pages work correctly!');
  });
});