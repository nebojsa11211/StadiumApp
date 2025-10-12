import { test, expect } from '@playwright/test';

test('verify stadium field proportions', async ({ page }) => {
  // Navigate to admin login
  await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });

  // Login
  await page.fill('input[type="email"]', 'admin@stadium.com');
  await page.fill('input[type="password"]', 'admin123');
  await page.click('button[type="submit"]');

  // Wait for any redirect
  await page.waitForLoadState('networkidle');
  await page.waitForTimeout(2000);

  // Navigate to stadium overview
  await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });
  await page.waitForTimeout(3000);

  // Wait for stadium field to be visible
  await page.waitForSelector('#admin-stadium-field', { timeout: 10000 });

  // Take screenshot
  await page.screenshot({ path: 'stadium-field-proportions.png', fullPage: true });

  // Get stadium field dimensions
  const fieldDimensions = await page.evaluate(() => {
    const field = document.querySelector('#admin-stadium-field');
    if (!field) return null;

    const rect = field.getBoundingClientRect();
    const computedStyle = window.getComputedStyle(field);

    return {
      width: rect.width,
      height: rect.height,
      aspectRatio: rect.width / rect.height,
      computedAspectRatio: computedStyle.aspectRatio
    };
  });

  console.log('Stadium Field Dimensions:', fieldDimensions);
  console.log(`Aspect Ratio: ${fieldDimensions?.aspectRatio?.toFixed(3)} (should be 1.000 for 100:100)`);

  // Verify aspect ratio is close to 1:1 (allowing small tolerance)
  if (fieldDimensions) {
    const tolerance = 0.05; // 5% tolerance
    expect(fieldDimensions.aspectRatio).toBeGreaterThan(1 - tolerance);
    expect(fieldDimensions.aspectRatio).toBeLessThan(1 + tolerance);
  }
});
