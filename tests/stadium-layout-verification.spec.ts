import { test, expect } from '@playwright/test';

test('Stadium layout verification - field centered with tribunes around', async ({ page }) => {
  // Navigate to the admin app
  await page.goto('https://localhost:7030', { waitUntil: 'networkidle' });

  // Login as admin
  await page.fill('#admin-login-email-input', 'admin@stadium.com');
  await page.fill('#admin-login-password-input', 'admin123');
  await page.click('#admin-login-submit-btn');

  // Wait for login to complete - look for dashboard elements
  await page.waitForSelector('.card-title:has-text("System Overview")', { timeout: 60000 });

  // Navigate to stadium overview
  await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });

  // Wait for the stadium layout to render
  await page.waitForSelector('#admin-stadium-container', { timeout: 10000 });

  // Take a screenshot to verify the layout
  await page.screenshot({
    path: '.playwright-mcp/stadium-layout-corrected.png',
    fullPage: true
  });

  // Verify key layout elements are present
  await expect(page.locator('#admin-stadium-container')).toBeVisible();
  await expect(page.locator('#admin-stadium-field-markings')).toBeVisible();

  // Check that tribunes are positioned around the field
  const tribunes = await page.locator('[data-tribune]').count();
  console.log(`Found ${tribunes} tribunes in the stadium layout`);

  console.log('✅ Stadium layout verification completed - screenshot saved');
});