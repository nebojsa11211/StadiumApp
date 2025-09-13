import { test, expect } from '@playwright/test';

/**
 * Comprehensive Playwright tests for HNK Rijeka stadium implementation in the admin project
 * Testing the static stadium layout that shows when API data is not available
 * 
 * Stadium Structure:
 * - North stand: I4, I3, I2 (blue sectors)
 * - West stand: S5, S4, S3 (orange sectors)
 * - South stand: Z10, Z8, Z6A, Z4, Z2, Z9, Z5A, Z3 (blue sectors)
 * Total: 11 labeled sectors
 */

test.describe('HNK Rijeka Stadium Layout Tests', () => {
  const adminUrl = 'https://localhost:7030';
  const stadiumOverviewUrl = `${adminUrl}/admin/stadium-overview`;

  // Expected HNK Rijeka sectors by stand
  const expectedSectors = {
    north: ['I4', 'I3', 'I2'],
    west: ['S5', 'S4', 'S3'],
    south: ['Z10', 'Z8', 'Z6A', 'Z4', 'Z2', 'Z9', 'Z5A', 'Z3'],
    east: [] // No east sectors in current layout
  };

  const allExpectedSectors = [
    ...expectedSectors.north,
    ...expectedSectors.west,
    ...expectedSectors.south
  ];

  test.beforeEach(async ({ page }) => {
    // Set a longer timeout for navigation as the page may need to load API fallbacks
    page.setDefaultTimeout(30000);
    
    // Navigate to stadium overview page
    await page.goto(stadiumOverviewUrl);
    
    // Wait for the page to settle and potentially fallback to static layout
    await page.waitForTimeout(3000);
  });

  test('should navigate to stadium overview page successfully', async ({ page }) => {
    // Verify we're on the stadium overview page
    await expect(page).toHaveURL(/admin\/stadium-overview/);
    
    // Check for the page title or heading
    await expect(page.locator('h1, h2, h3')).toContainText(/stadium|overview/i, { timeout: 10000 });
  });

  test('should render SVG stadium container successfully', async ({ page }) => {
    // Wait for SVG container to be present
    const svgContainer = page.locator('#stadium-svg-container');
    await expect(svgContainer).toBeVisible({ timeout: 15000 });
    
    // Verify SVG element is present
    const svg = page.locator('#stadium-svg');
    await expect(svg).toBeVisible();
    
    // Check SVG has the correct viewBox for HNK Rijeka stadium
    await expect(svg).toHaveAttribute('viewBox', '0 0 1200 900');
    
    // Verify aria-label indicates HNK Rijeka stadium
    await expect(svg).toHaveAttribute('aria-label', /HNK Rijeka/i);
  });

  test('should render football pitch correctly', async ({ page }) => {
    // Wait for the field group to be present
    const fieldGroup = page.locator('#field');
    await expect(fieldGroup).toBeVisible({ timeout: 10000 });
    
    // Check for pitch markings (main rectangle)
    const pitchRect = fieldGroup.locator('rect').first();
    await expect(pitchRect).toBeVisible();
    
    // Verify field has proper aria-label
    await expect(fieldGroup).toHaveAttribute('aria-label', 'Football pitch');
  });

  test('should display all expected HNK Rijeka sectors', async ({ page }) => {
    console.log('Testing all expected sectors:', allExpectedSectors);
    
    // Wait for stadium to load
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Test each expected sector
    for (const sectorCode of allExpectedSectors) {
      console.log(`Testing sector: ${sectorCode}`);
      
      // Check for sector group element
      const sectorGroup = page.locator(`#sector-${sectorCode}`);
      await expect(sectorGroup).toBeVisible({ 
        timeout: 5000 
      }).catch(() => {
        throw new Error(`Sector ${sectorCode} group not visible`);
      });
      
      // Verify sector has proper accessibility attributes
      await expect(sectorGroup).toHaveAttribute('aria-label', `Sector ${sectorCode}`);
      await expect(sectorGroup).toHaveAttribute('role', 'button');
      await expect(sectorGroup).toHaveAttribute('tabindex', '0');
      
      // Check that sector text is present and readable
      const sectorText = sectorGroup.locator('text');
      await expect(sectorText).toHaveText(sectorCode);
      
      console.log(`✓ Sector ${sectorCode} verified`);
    }
  });

  test('should verify North stand sectors (I4, I3, I2) - blue color', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    for (const sector of expectedSectors.north) {
      const sectorGroup = page.locator(`#sector-${sector}`);
      await expect(sectorGroup).toBeVisible();
      
      // Verify blue color (#4A90E2) for north sectors
      const rect = sectorGroup.locator('rect');
      await expect(rect).toHaveAttribute('fill', '#4A90E2');
      
      // Check sector is part of north stand
      const northStand = page.locator('#stand-north');
      await expect(northStand).toContainText(sector);
    }
  });

  test('should verify West stand sectors (S5, S4, S3) - orange color', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    for (const sector of expectedSectors.west) {
      const sectorGroup = page.locator(`#sector-${sector}`);
      await expect(sectorGroup).toBeVisible();
      
      // Verify orange color (#F39C12) for west sectors
      const rect = sectorGroup.locator('rect');
      await expect(rect).toHaveAttribute('fill', '#F39C12');
      
      // Check sector is part of west stand
      const westStand = page.locator('#stand-west');
      await expect(westStand).toContainText(sector);
    }
  });

  test('should verify South stand sectors (Z10, Z8, Z6A, Z4, Z2, Z9, Z5A, Z3) - blue color', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    for (const sector of expectedSectors.south) {
      const sectorGroup = page.locator(`#sector-${sector}`);
      await expect(sectorGroup).toBeVisible();
      
      // Verify blue color (#4A90E2) for south sectors
      const rect = sectorGroup.locator('rect');
      await expect(rect).toHaveAttribute('fill', '#4A90E2');
      
      // Check sector is part of south stand
      const southStand = page.locator('#stand-south');
      await expect(southStand).toContainText(sector);
    }
  });

  test('should test sector interactivity and click events', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Test clicking on a few representative sectors
    const testSectors = ['I4', 'S5', 'Z10', 'Z3'];
    
    for (const sectorCode of testSectors) {
      console.log(`Testing click on sector: ${sectorCode}`);
      
      const sectorGroup = page.locator(`#sector-${sectorCode}`);
      await expect(sectorGroup).toBeVisible();
      
      // Click on sector (should be clickable)
      await sectorGroup.click();
      
      // Wait a moment for any potential UI changes
      await page.waitForTimeout(500);
      
      // Verify sector is still visible after click
      await expect(sectorGroup).toBeVisible();
      
      console.log(`✓ Sector ${sectorCode} click test passed`);
    }
  });

  test('should test sector hover effects', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    const testSector = 'I4';
    const sectorGroup = page.locator(`#sector-${testSector}`);
    
    // Hover over sector
    await sectorGroup.hover();
    
    // Verify sector is still visible and accessible
    await expect(sectorGroup).toBeVisible();
    
    // Move away from sector
    await page.locator('#field').hover();
    
    // Verify sector is still visible
    await expect(sectorGroup).toBeVisible();
  });

  test('should verify legend functionality', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Look for legend button or toggle
    const legendButton = page.locator('button').filter({ hasText: /legend/i }).first();
    
    if (await legendButton.isVisible()) {
      console.log('Legend button found, testing toggle functionality');
      
      // Click to show legend
      await legendButton.click();
      await page.waitForTimeout(500);
      
      // Check if legend panel is visible
      const legendPanel = page.locator('#legend-panel');
      if (await legendPanel.isVisible()) {
        // Verify legend content
        await expect(legendPanel).toContainText(/HNK Rijeka/i);
        
        // Test toggle off
        await legendButton.click();
        await page.waitForTimeout(500);
      }
    } else {
      console.log('Legend button not found - legend may be always visible or not implemented');
      
      // Check if legend is always visible
      const legendPanel = page.locator('#legend-panel');
      if (await legendPanel.isVisible()) {
        await expect(legendPanel).toContainText(/HNK Rijeka/i);
      }
    }
  });

  test('should verify stadium description and accessibility', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Check SVG has proper description
    const description = page.locator('#stadium-description');
    await expect(description).toBeVisible();
    await expect(description).toContainText(/HNK Rijeka Stadium/);
    await expect(description).toContainText(/14 sectors/);
    await expect(description).toContainText(/North.*I2.*I3.*I4/);
    await expect(description).toContainText(/West.*S3.*S4.*S5/);
    await expect(description).toContainText(/South.*Z/);
  });

  test('should handle keyboard navigation for sectors', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Test keyboard navigation on first few sectors
    const testSectors = ['I4', 'I3', 'S5'];
    
    for (const sectorCode of testSectors) {
      const sectorGroup = page.locator(`#sector-${sectorCode}`);
      
      // Focus on sector
      await sectorGroup.focus();
      
      // Verify it has focus
      await expect(sectorGroup).toBeFocused();
      
      // Test Enter key
      await page.keyboard.press('Enter');
      await page.waitForTimeout(300);
      
      // Verify sector is still focusable
      await expect(sectorGroup).toBeVisible();
    }
  });

  test('should verify responsive design and mobile compatibility', async ({ page }) => {
    // Test mobile viewport
    await page.setViewportSize({ width: 390, height: 844 });
    await page.goto(stadiumOverviewUrl);
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Verify SVG is still visible and responsive
    const svg = page.locator('#stadium-svg');
    await expect(svg).toBeVisible();
    
    // Check a few sectors are still visible
    for (const sector of ['I4', 'S5', 'Z10']) {
      await expect(page.locator(`#sector-${sector}`)).toBeVisible();
    }
    
    // Test tablet viewport
    await page.setViewportSize({ width: 768, height: 1024 });
    await page.reload();
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Verify still works
    await expect(svg).toBeVisible();
    
    // Reset to desktop
    await page.setViewportSize({ width: 1920, height: 1080 });
  });

  test('should take screenshots for visual verification', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Take full stadium screenshot
    await page.screenshot({ 
      path: 'test-results/hnk-rijeka-stadium-full.png',
      fullPage: true 
    });
    
    // Take stadium SVG only
    const svg = page.locator('#stadium-svg');
    await svg.screenshot({ 
      path: 'test-results/hnk-rijeka-stadium-svg.png' 
    });
    
    // Take screenshots of individual stands
    const northStand = page.locator('#stand-north');
    await northStand.screenshot({ 
      path: 'test-results/hnk-rijeka-north-stand.png' 
    });
    
    const westStand = page.locator('#stand-west');
    await westStand.screenshot({ 
      path: 'test-results/hnk-rijeka-west-stand.png' 
    });
    
    const southStand = page.locator('#stand-south');
    await southStand.screenshot({ 
      path: 'test-results/hnk-rijeka-south-stand.png' 
    });
    
    console.log('Screenshots saved to test-results/ directory');
  });

  test('should verify no loading errors or failures', async ({ page }) => {
    // Listen for console errors
    const consoleMessages: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        consoleMessages.push(msg.text());
      }
    });
    
    // Listen for page errors
    const pageErrors: string[] = [];
    page.on('pageerror', error => {
      pageErrors.push(error.message);
    });
    
    await page.goto(stadiumOverviewUrl);
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Wait a bit more to catch any delayed errors
    await page.waitForTimeout(2000);
    
    // Check we have the expected number of sectors visible
    const visibleSectors = await page.locator('.hnk-sector').count();
    expect(visibleSectors).toBe(allExpectedSectors.length);
    
    // Report any errors found (but don't fail the test if they're expected API errors)
    if (consoleMessages.length > 0) {
      console.log('Console messages found:', consoleMessages);
    }
    
    if (pageErrors.length > 0) {
      console.log('Page errors found:', pageErrors);
    }
    
    // Critical errors that would indicate stadium rendering failure
    const criticalErrors = [...consoleMessages, ...pageErrors].filter(error => 
      error.toLowerCase().includes('svg') || 
      error.toLowerCase().includes('stadium') ||
      error.toLowerCase().includes('sector')
    );
    
    expect(criticalErrors).toHaveLength(0);
  });

  test('should verify all 11 sectors are rendered correctly with proper structure', async ({ page }) => {
    await page.waitForSelector('#stadium-svg', { timeout: 15000 });
    
    // Count total sectors
    const totalSectors = await page.locator('.hnk-sector').count();
    expect(totalSectors).toBe(11);
    
    // Verify sector distribution by stand
    const northSectors = await page.locator('#stand-north .hnk-sector').count();
    expect(northSectors).toBe(3); // I4, I3, I2
    
    const westSectors = await page.locator('#stand-west .hnk-sector').count();
    expect(westSectors).toBe(3); // S5, S4, S3
    
    const southSectors = await page.locator('#stand-south .hnk-sector').count();
    expect(southSectors).toBe(8); // Z10, Z8, Z6A, Z4, Z2, Z9, Z5A, Z3
    
    // Verify each sector has proper structure
    for (const sectorCode of allExpectedSectors) {
      const sector = page.locator(`#sector-${sectorCode}`);
      
      // Should have a rectangle for visual representation
      await expect(sector.locator('rect')).toBeVisible();
      
      // Should have text label
      await expect(sector.locator('text')).toHaveText(sectorCode);
      
      // Should have proper CSS class
      await expect(sector).toHaveClass(/hnk-sector/);
    }
  });
});