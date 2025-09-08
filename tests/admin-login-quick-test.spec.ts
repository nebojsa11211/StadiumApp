import { test, expect } from '@playwright/test';
import { waitForPageInteractive } from './helpers/blazor-helpers';

test('quick admin login test', async ({ page }) => {
  // Navigate to admin login
  await page.goto('http://localhost:7005/login');
  await waitForPageInteractive(page);
  
  // Fill login form
  await page.locator('input[type="email"]').fill('admin@stadium.com');
  await page.locator('input[type="password"]').fill('password123');
  await page.locator('button[type="submit"]').click();
  
  // Wait and see what happens
  await page.waitForTimeout(10000); // Give it 10 seconds
  
  // Take screenshot for debugging
  await page.screenshot({ path: 'login-result.png' });
  
  console.log('Current URL:', page.url());
  console.log('Page title:', await page.title());
});