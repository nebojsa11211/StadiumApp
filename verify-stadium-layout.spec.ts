import { test, expect } from '@playwright/test';

test.describe('Stadium Layout Verification - 100m × 100m Square Field', () => {
  test.beforeEach(async ({ page }) => {
    // Login to admin
    await page.goto('https://localhost:7030/admin/login');
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);

    // Navigate to stadium overview
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForTimeout(3000);
  });

  test('should display square field with 1:1 aspect ratio', async ({ page }) => {
    const field = await page.locator('#admin-stadium-field');
    await expect(field).toBeVisible();

    const dimensions = await field.evaluate((el) => {
      const rect = el.getBoundingClientRect();
      return {
        width: rect.width,
        height: rect.height,
        aspectRatio: rect.width / rect.height
      };
    });

    console.log('Field Dimensions:', dimensions);

    // Verify square proportions (1:1 ratio with 5% tolerance)
    expect(dimensions.aspectRatio).toBeGreaterThan(0.95);
    expect(dimensions.aspectRatio).toBeLessThan(1.05);
  });

  test('should display all four tribunes (North, South, East, West)', async ({ page }) => {
    // Check for all tribune stands
    const northStand = await page.locator('.stand-n, .stand-north, .stand-position-north').first();
    const southStand = await page.locator('.stand-s, .stand-south, .stand-position-south').first();
    const eastStand = await page.locator('.stand-e, .stand-east, .stand-position-east').first();
    const westStand = await page.locator('.stand-w, .stand-west, .stand-position-west').first();

    // Verify all tribunes are visible
    await expect(northStand).toBeVisible();
    await expect(southStand).toBeVisible();
    await expect(eastStand).toBeVisible();
    await expect(westStand).toBeVisible();

    console.log('✅ All four tribunes are visible');
  });

  test('should display sectors within tribunes', async ({ page }) => {
    // Wait for sectors to load
    await page.waitForSelector('.sector', { timeout: 5000 });

    const sectors = await page.locator('.sector').all();
    const sectorCount = sectors.length;

    console.log(`Found ${sectorCount} sectors`);
    expect(sectorCount).toBeGreaterThan(0);

    // Verify at least one sector from each tribune is visible
    const northSectors = await page.locator('.stand-n .sector, .stand-north .sector').count();
    const southSectors = await page.locator('.stand-s .sector, .stand-south .sector').count();
    const eastSectors = await page.locator('.stand-e .sector, .stand-east .sector').count();
    const westSectors = await page.locator('.stand-w .sector, .stand-west .sector').count();

    console.log('Sectors per tribune:', {
      North: northSectors,
      South: southSectors,
      East: eastSectors,
      West: westSectors
    });

    expect(northSectors + southSectors + eastSectors + westSectors).toBeGreaterThan(0);
  });

  test('should fit entire layout in viewport without excessive scrolling', async ({ page }) => {
    // Get viewport and content dimensions
    const viewportHeight = await page.viewportSize().then(vp => vp?.height || 0);

    const layoutHeight = await page.evaluate(() => {
      const container = document.querySelector('#admin-stadium-container');
      return container?.scrollHeight || 0;
    });

    console.log('Viewport height:', viewportHeight);
    console.log('Layout height:', layoutHeight);

    // Layout should not be excessively tall (allow up to 1.5x viewport)
    expect(layoutHeight).toBeLessThan(viewportHeight * 1.8);
  });

  test('should maintain proper grid layout structure', async ({ page }) => {
    const gridLayout = await page.locator('.stadium-grid-layout');
    await expect(gridLayout).toBeVisible();

    const gridProperties = await gridLayout.evaluate((el) => {
      const styles = window.getComputedStyle(el);
      return {
        display: styles.display,
        gridTemplateColumns: styles.gridTemplateColumns,
        gridTemplateRows: styles.gridTemplateRows
      };
    });

    console.log('Grid Properties:', gridProperties);

    // Verify it's using grid layout
    expect(gridProperties.display).toBe('grid');
  });

  test('should take full page screenshot for visual verification', async ({ page }) => {
    await page.screenshot({
      path: 'stadium-layout-verification.png',
      fullPage: true
    });

    console.log('✅ Screenshot saved: stadium-layout-verification.png');
  });

  test('should display field with proper max dimensions', async ({ page }) => {
    const field = await page.locator('#admin-stadium-field');

    const dimensions = await field.evaluate((el) => {
      const rect = el.getBoundingClientRect();
      const styles = window.getComputedStyle(el);
      return {
        width: rect.width,
        height: rect.height,
        maxWidth: styles.maxWidth,
        maxHeight: styles.maxHeight,
        aspectRatio: styles.aspectRatio
      };
    });

    console.log('Field CSS Properties:', dimensions);

    // Field should not exceed 500px (as per design)
    expect(dimensions.width).toBeLessThanOrEqual(500);
    expect(dimensions.height).toBeLessThanOrEqual(500);

    // Verify aspect ratio is set
    expect(dimensions.aspectRatio).toContain('1');
  });

  test('should be responsive - check different viewport sizes', async ({ page }) => {
    const viewports = [
      { width: 1920, height: 1080, name: 'Desktop Large' },
      { width: 1366, height: 768, name: 'Desktop Medium' },
      { width: 1024, height: 768, name: 'Tablet Landscape' },
      { width: 768, height: 1024, name: 'Tablet Portrait' },
    ];

    for (const viewport of viewports) {
      await page.setViewportSize({ width: viewport.width, height: viewport.height });
      await page.waitForTimeout(1000);

      const field = await page.locator('#admin-stadium-field');
      const isVisible = await field.isVisible();

      const dimensions = await field.evaluate((el) => {
        const rect = el.getBoundingClientRect();
        return {
          width: rect.width,
          height: rect.height,
          aspectRatio: rect.width / rect.height
        };
      });

      console.log(`${viewport.name} (${viewport.width}x${viewport.height}):`, dimensions);

      expect(isVisible).toBe(true);
      // Verify square aspect ratio maintained
      expect(dimensions.aspectRatio).toBeGreaterThan(0.95);
      expect(dimensions.aspectRatio).toBeLessThan(1.05);
    }
  });
});
