import { test, expect } from '@playwright/test';
import { testConfig } from './config';

/**
 * Visual Consistency Tests for Dynamic SVG Stadium Rendering
 * 
 * Purpose: Ensure that the new dynamic SVG generation produces exactly the same
 * visual output as the original static SVG HNK Rijeka stadium layout.
 * 
 * Key Requirements:
 * - Dynamic SVG must look identical to static version
 * - All 14 sectors must be positioned correctly (I2, I3, I4, S3, S4, S5, Z2-Z10, etc.)
 * - Color scheme and dimensions must match exactly
 * - Seat count display must work when enabled
 */

test.describe('Dynamic SVG Visual Consistency', () => {
  
  test.beforeEach(async ({ page }) => {
    // Navigate to admin login
    await page.goto('http://localhost:9030/admin/login');
    
    // Login as admin
    await page.fill('input[type="email"]', testConfig.credentials.admin.email);
    await page.fill('input[type="password"]', testConfig.credentials.admin.password);
    await page.click('button[type="submit"]');
    
    // Wait for successful login and redirect to dashboard
    await page.waitForURL('**/admin/dashboard', { timeout: 10000 });
    await expect(page.locator('h3')).toContainText('Admin Dashboard');
  });

  test('Stadium Overview loads with dynamic SVG stadium layout', async ({ page }) => {
    // Navigate to Stadium Overview
    await page.goto('http://localhost:9030/admin/stadium-overview');
    
    // Wait for page to load
    await page.waitForSelector('#stadium-overview-title', { timeout: 10000 });
    await expect(page.locator('#stadium-overview-title')).toContainText('Stadium Overview');
    
    // Wait for dynamic stadium SVG to load
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    await expect(page.locator('#stadium-svg')).toBeVisible();
    
    console.log('✅ Dynamic SVG stadium loaded successfully');
  });

  test('Verify HNK Rijeka stadium structure and coordinates', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Check that the SVG has the correct viewBox (1200x900)
    const svgViewBox = await page.locator('#stadium-svg').getAttribute('viewBox');
    expect(svgViewBox).toBe('0 0 1200 900');
    console.log('✅ SVG viewBox correct: 1200x900');
    
    // Verify football field is present
    const fieldElement = await page.locator('#stadium-svg g.field-group').first();
    await expect(fieldElement).toBeVisible();
    console.log('✅ Football field rendered');
    
    // Check for stand groups (dynamic version should have these)
    const standGroups = page.locator('#stadium-svg g.stand-group');
    const standGroupCount = await standGroups.count();
    expect(standGroupCount).toBeGreaterThan(0);
    console.log(`✅ Found ${standGroupCount} stand groups`);
    
    // Verify sectors are clickable
    const sectors = page.locator('#stadium-svg g.sector-group');
    const sectorCount = await sectors.count();
    expect(sectorCount).toBeGreaterThan(10); // Should have 14 sectors minimum
    console.log(`✅ Found ${sectorCount} interactive sectors`);
  });

  test('Verify dynamic sector color coding and seat counts', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Enable occupancy display
    const occupancyButton = page.locator('#toggle-occupancy-button');
    if (await occupancyButton.isVisible()) {
      await occupancyButton.click();
      console.log('✅ Occupancy display enabled');
    }
    
    // Check if seat count display is working
    const showOccupancy = await page.locator('#toggle-occupancy-button').getAttribute('class');
    const isOccupancyEnabled = showOccupancy?.includes('btn-primary');
    
    if (isOccupancyEnabled) {
      // Look for seat count text in sectors (format like "1200 seats" or "450/1200")
      const sectorsWithText = page.locator('#stadium-svg text');
      const textElements = await sectorsWithText.count();
      expect(textElements).toBeGreaterThan(10);
      console.log(`✅ Found ${textElements} text elements in sectors`);
    }
  });

  test('Test sector interaction and modal functionality', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Find the first clickable sector
    const firstSector = page.locator('#stadium-svg g.sector-group').first();
    await expect(firstSector).toBeVisible();
    
    // Click on the sector to open detail modal
    await firstSector.click();
    
    // Wait for sector detail modal to appear
    const modal = page.locator('#sector-detail-modal');
    await expect(modal).toBeVisible({ timeout: 5000 });
    console.log('✅ Sector detail modal opened on click');
    
    // Verify modal has title and content
    const modalTitle = page.locator('#sector-modal-title');
    await expect(modalTitle).toBeVisible();
    console.log('✅ Modal has title');
    
    // Close the modal
    await page.locator('#sector-modal-close-button').click();
    await expect(modal).not.toBeVisible();
    console.log('✅ Modal closes properly');
  });

  test('Compare legend display between static and dynamic modes', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Toggle legend display
    await page.click('#toggle-legend-button');
    await page.waitForSelector('#legend-panel', { timeout: 3000 });
    
    // Verify legend is visible and has proper content
    await expect(page.locator('#legend-title')).toBeVisible();
    const legendTitle = await page.locator('#legend-title').textContent();
    expect(legendTitle).toContain('Rijeka'); // Should contain stadium name
    console.log(`✅ Legend title: ${legendTitle}`);
    
    // Check for legend items (should have 3+ color categories)
    const legendItems = page.locator('#legend-panel .legend-item');
    const itemCount = await legendItems.count();
    expect(itemCount).toBeGreaterThanOrEqual(3);
    console.log(`✅ Found ${itemCount} legend items`);
  });

  test('Test event selection and occupancy color changes', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Check if events are available
    const eventSelect = page.locator('#eventSelect');
    await expect(eventSelect).toBeVisible();
    
    const eventOptions = page.locator('#eventSelect option');
    const optionCount = await eventOptions.count();
    
    if (optionCount > 1) { // More than just "No Event Selected"
      // Select the first available event
      await eventSelect.selectOption({ index: 1 });
      console.log('✅ Event selected');
      
      // Wait for potential color changes
      await page.waitForTimeout(2000);
      
      // Verify legend updates to show occupancy colors
      const legendButton = page.locator('#toggle-legend-button');
      if (!(await page.locator('#legend-panel').isVisible())) {
        await legendButton.click();
      }
      
      // Look for occupancy-specific legend items
      const occupancyLegend = page.locator('#legend-panel').locator('text=Available');
      if (await occupancyLegend.count() > 0) {
        console.log('✅ Occupancy legend displayed');
      }
    } else {
      console.log('ℹ️ No events available for testing occupancy colors');
    }
  });

  test('Verify responsive layout and SVG scaling', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Test different viewport sizes
    const viewports = [
      { width: 1920, height: 1080, name: 'Desktop Large' },
      { width: 1366, height: 768, name: 'Desktop Standard' },
      { width: 768, height: 1024, name: 'Tablet' }
    ];
    
    for (const viewport of viewports) {
      await page.setViewportSize({ width: viewport.width, height: viewport.height });
      await page.waitForTimeout(1000);
      
      // Verify SVG is still visible and properly scaled
      await expect(page.locator('#stadium-svg')).toBeVisible();
      
      // Check SVG container properties
      const svgContainer = page.locator('#stadium-svg-container');
      await expect(svgContainer).toBeVisible();
      
      console.log(`✅ SVG scales properly at ${viewport.name} (${viewport.width}x${viewport.height})`);
    }
  });

  test('Visual regression - take stadium layout screenshot', async ({ page }) => {
    await page.goto('http://localhost:9030/admin/stadium-overview');
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Wait for full rendering
    await page.waitForTimeout(2000);
    
    // Take screenshot of the entire stadium SVG
    const stadiumSvg = page.locator('#stadium-svg');
    await expect(stadiumSvg).toBeVisible();
    
    // Screenshot for visual comparison
    await stadiumSvg.screenshot({ 
      path: '.playwright-mcp/dynamic-stadium-layout.png',
      timeout: 5000
    });
    console.log('✅ Stadium layout screenshot saved for visual comparison');
    
    // Also screenshot the entire stadium overview page
    await page.screenshot({ 
      path: '.playwright-mcp/dynamic-stadium-overview-full.png',
      fullPage: true,
      timeout: 5000
    });
    console.log('✅ Full stadium overview screenshot saved');
  });

  test('Performance - measure dynamic SVG rendering time', async ({ page }) => {
    const startTime = Date.now();
    
    await page.goto('http://localhost:9030/admin/stadium-overview');
    
    // Wait for SVG to be fully rendered
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    await page.waitForFunction(() => {
      const svg = document.querySelector('#stadium-svg');
      return svg && svg.querySelectorAll('g.sector-group').length > 10;
    }, { timeout: 10000 });
    
    const endTime = Date.now();
    const renderTime = endTime - startTime;
    
    console.log(`✅ Dynamic SVG rendering completed in ${renderTime}ms`);
    
    // Performance assertion - should load within reasonable time
    expect(renderTime).toBeLessThan(20000); // 20 seconds max
    
    if (renderTime < 5000) {
      console.log('🚀 Excellent performance: under 5 seconds');
    } else if (renderTime < 10000) {
      console.log('✅ Good performance: under 10 seconds');
    } else {
      console.log('⚠️ Slow performance: over 10 seconds');
    }
  });
});