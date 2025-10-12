import { test, expect } from '@playwright/test';

test.describe('Stadium Overlay Configuration Test', () => {
  test('should load stadium overlay with sector overlays visible', async ({ page }) => {
    // Navigate to login
    await page.goto('https://localhost:7030/admin/login');

    // Login
    await page.fill('#customer-login-email-input', 'admin@stadium.com');
    await page.fill('#customer-login-password-input', 'admin123');
    await page.click('#customer-login-submit-btn');

    // Wait for navigation
    await page.waitForURL('**/admin/dashboard', { timeout: 10000 });

    // Navigate to stadium overview
    await page.goto('https://localhost:7030/admin/stadium-overview');

    // Wait for page to load
    await page.waitForLoadState('networkidle');

    // Take screenshot of the page
    await page.screenshot({ path: 'stadium-overlay-loaded.png', fullPage: true });

    // Check if sector overlays are present
    const sectorOverlays = await page.locator('.sector-overlay').count();
    console.log(`Found ${sectorOverlays} sector overlays`);

    // Check if stadium image is present
    const stadiumImage = await page.locator('.stadium-blueprint-img').count();
    console.log(`Found ${stadiumImage} stadium images`);

    // Check for error messages
    const errorMessage = await page.locator('.alert-info').textContent();
    console.log(`Page message: ${errorMessage}`);

    // Assertions
    expect(sectorOverlays).toBeGreaterThan(0);
    expect(stadiumImage).toBe(1);
  });
});
