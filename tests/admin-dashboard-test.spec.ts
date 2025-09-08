import { test, expect } from '@playwright/test';

test.describe('Admin Dashboard', () => {
  test('should display dynamic dashboard with statistics and recent activity', async ({ page }) => {
    // Navigate to admin login
    await page.goto('http://localhost:9002/login');
    
    // Login as admin
    await page.fill('input[name="email"]', 'admin@stadium.com');
    await page.fill('input[name="password"]', 'Admin123!');
    await page.click('button[type="submit"]');
    
    // Wait for navigation to dashboard
    await page.waitForURL('http://localhost:9002/', { timeout: 10000 });
    
    // Check for key statistics cards
    await expect(page.locator('text=Today\'s Revenue')).toBeVisible();
    await expect(page.locator('text=Active Orders')).toBeVisible();
    await expect(page.locator('text=Tickets Sold Today')).toBeVisible();
    await expect(page.locator('text=Online Users')).toBeVisible();
    
    // Check for Recent Activity section
    await expect(page.locator('text=Recent Activity')).toBeVisible();
    
    // Check for Quick Actions section
    await expect(page.locator('text=Quick Actions')).toBeVisible();
    await expect(page.locator('text=Process New Orders')).toBeVisible();
    await expect(page.locator('text=Manage Events')).toBeVisible();
    await expect(page.locator('text=View Analytics')).toBeVisible();
    await expect(page.locator('text=System Logs')).toBeVisible();
    
    // Verify no redundant "Manage Orders" hero section exists
    const heroSection = page.locator('.hero-section');
    await expect(heroSection).toHaveCount(0);
    
    // Verify the page has dynamic content (check for currency symbol indicating revenue)
    await expect(page.locator('text=/\\$[0-9,]+\\.[0-9]{2}/')).toBeVisible();
  });
  
  test('should navigate to different sections from quick actions', async ({ page }) => {
    // Navigate to admin dashboard
    await page.goto('http://localhost:9002/login');
    await page.fill('input[name="email"]', 'admin@stadium.com');
    await page.fill('input[name="password"]', 'Admin123!');
    await page.click('button[type="submit"]');
    await page.waitForURL('http://localhost:9002/');
    
    // Test Process New Orders button
    await page.click('text=Process New Orders');
    await expect(page).toHaveURL('http://localhost:9002/orders');
    await page.goBack();
    
    // Test View Analytics button
    await page.click('text=View Analytics');
    await expect(page).toHaveURL('http://localhost:9002/analytics');
    await page.goBack();
    
    // Test System Logs button
    await page.click('text=System Logs');
    await expect(page).toHaveURL('http://localhost:9002/logs');
  });
});