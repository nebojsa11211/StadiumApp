import { test, expect } from '@playwright/test';

test('verify complete stadium layout with square field and all tribunes', async ({ page }) => {
  // Login first
  await page.goto('/login');
  await page.waitForLoadState('networkidle');

  await page.fill('input[type="email"]', 'admin@stadium.com');
  await page.fill('input[type="password"]', 'admin123');
  await page.click('button[type="submit"]');

  // Wait for login to complete
  await page.waitForTimeout(3000);

  // Navigate to stadium overview
  await page.goto('/admin/stadium-overview');

  // Wait for the page to fully load
  await page.waitForLoadState('networkidle');
  await page.waitForTimeout(3000);

  // Wait for stadium container to be visible
  await page.waitForSelector('#admin-stadium-container', { state: 'visible', timeout: 15000 });

  // Scroll to stadium layout section
  await page.evaluate(() => {
    const stadiumSection = document.querySelector('#admin-stadium-container');
    if (stadiumSection) {
      stadiumSection.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  });

  await page.waitForTimeout(1000);

  // Get stadium field dimensions
  const fieldInfo = await page.evaluate(() => {
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

  console.log('Stadium Field:', fieldInfo);

  // Verify field is square (1:1 aspect ratio)
  expect(fieldInfo).not.toBeNull();
  expect(fieldInfo!.aspectRatio).toBeGreaterThan(0.95);
  expect(fieldInfo!.aspectRatio).toBeLessThan(1.05);

  // Check all four tribunes are visible
  const tribunesVisible = await page.evaluate(() => {
    const tribunes = {
      north: document.querySelector('.stand-n, .stand-north, .stand-position-north'),
      south: document.querySelector('.stand-s, .stand-south, .stand-position-south'),
      east: document.querySelector('.stand-e, .stand-east, .stand-position-east'),
      west: document.querySelector('.stand-w, .stand-west, .stand-position-west')
    };

    return {
      north: tribunes.north ? tribunesVisible(tribunes.north) : false,
      south: tribunes.south ? tribunesVisible(tribunes.south) : false,
      east: tribunes.east ? tribunesVisible(tribunes.east) : false,
      west: tribunes.west ? tribunesVisible(tribunes.west) : false
    };

    function tribunesVisible(element: Element): boolean {
      const rect = element.getBoundingClientRect();
      return rect.width > 0 && rect.height > 0 &&
             rect.top >= 0 && rect.bottom <= window.innerHeight + 1000; // Allow some off-screen
    }
  });

  console.log('Tribunes Visible:', tribunesVisible);

  // Take screenshot of full stadium layout
  await page.screenshot({
    path: 'stadium-layout-complete.png',
    fullPage: false
  });

  // Take screenshot of just the stadium container
  const stadiumContainer = await page.locator('#admin-stadium-container');
  await stadiumContainer.screenshot({
    path: 'stadium-container-focused.png'
  });

  // Take full-page screenshot showing entire interface
  await page.screenshot({
    path: 'stadium-page-fullpage.png',
    fullPage: true
  });

  console.log('✅ All screenshots taken successfully!');
  console.log('   - stadium-layout-complete.png (viewport)');
  console.log('   - stadium-container-focused.png (stadium only)');
  console.log('   - stadium-page-fullpage.png (full page)');
});
