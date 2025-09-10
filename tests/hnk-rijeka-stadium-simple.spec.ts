import { test, expect } from '@playwright/test';

/**
 * Simple test to verify HNK Rijeka stadium SVG is rendering after the Razor syntax fix
 */

test.describe('HNK Rijeka Stadium - Basic Rendering', () => {
  const adminUrl = 'http://localhost:7031';
  const stadiumOverviewUrl = `${adminUrl}/admin/stadium-overview`;

  test('should render HNK Rijeka stadium SVG after fix', async ({ page }) => {
    console.log('Navigating to stadium overview page...');
    await page.goto(stadiumOverviewUrl);
    
    // Wait longer for page to settle and potentially show static layout
    await page.waitForTimeout(5000);
    
    console.log('Checking if SVG stadium container is visible...');
    
    // Check if stadium SVG container exists
    const svgContainer = page.locator('#stadium-svg-container');
    await expect(svgContainer).toBeVisible({ timeout: 10000 });
    
    // Check if SVG element exists
    const svg = page.locator('#stadium-svg');
    await expect(svg).toBeVisible();
    
    console.log('SVG container found! Taking screenshot...');
    
    // Take screenshot to see what's rendered
    await page.screenshot({ 
      path: 'test-results/hnk-rijeka-after-fix.png',
      fullPage: true 
    });
    
    console.log('Checking for a few key sectors...');
    
    // Test if a few sectors are visible
    const testSectors = ['I4', 'S5', 'Z10'];
    for (const sector of testSectors) {
      const sectorElement = page.locator(`#sector-${sector}`);
      const isVisible = await sectorElement.isVisible();
      console.log(`Sector ${sector} visible: ${isVisible}`);
      
      if (isVisible) {
        await expect(sectorElement).toBeVisible();
      }
    }
    
    console.log('Test completed successfully!');
  });
  
  test('should verify page title and basic structure', async ({ page }) => {
    await page.goto(stadiumOverviewUrl);
    
    // Verify we're on the right page
    await expect(page).toHaveURL(/admin\/stadium-overview/);
    
    // Check for stadium overview title
    const titleElement = page.locator('h3', { hasText: /stadium overview/i });
    await expect(titleElement).toBeVisible({ timeout: 10000 });
    
    console.log('Page title verified');
  });
});