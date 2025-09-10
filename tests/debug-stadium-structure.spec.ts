import { test, expect } from '@playwright/test';

/**
 * Diagnostic test to understand what's being rendered in the stadium overview page
 */

test('Debug stadium page structure', async ({ page }) => {
  await page.goto('http://localhost:7031/admin/stadium-overview');
  
  // Wait for page to load
  await page.waitForTimeout(5000);
  
  console.log('=== PAGE DEBUGGING ===');
  
  // Check if main container exists
  const mainContainer = page.locator('#stadium-layout-container');
  const isContainerVisible = await mainContainer.isVisible();
  console.log(`Main container visible: ${isContainerVisible}`);
  
  // Check stadium-main-view
  const stadiumMainView = page.locator('#stadium-main-view');
  const stadiumMainViewExists = await stadiumMainView.count();
  console.log(`stadium-main-view count: ${stadiumMainViewExists}`);
  
  if (stadiumMainViewExists > 0) {
    const isVisible = await stadiumMainView.isVisible();
    console.log(`stadium-main-view visible: ${isVisible}`);
  }
  
  // Check SVG container
  const svgContainer = page.locator('#stadium-svg-container');
  const svgContainerExists = await svgContainer.count();
  console.log(`SVG container count: ${svgContainerExists}`);
  
  // Check SVG element
  const svg = page.locator('#stadium-svg');
  const svgExists = await svg.count();
  console.log(`SVG element count: ${svgExists}`);
  
  // Look for "No Stadium Data" text
  const noDataText = page.locator(':has-text("No Stadium Data")');
  const noDataExists = await noDataText.count();
  console.log(`"No Stadium Data" elements: ${noDataExists}`);
  
  // Get the page HTML to see what's actually rendered
  const bodyHTML = await page.locator('body').innerHTML();
  
  // Look for specific patterns
  const hasStadiumViewer = bodyHTML.includes('stadium-viewer-container');
  const hasStadiumMainView = bodyHTML.includes('stadium-main-view');
  const hasSVGContainer = bodyHTML.includes('stadium-svg-container');
  const hasSVGElement = bodyHTML.includes('<svg');
  const hasHNKSectors = bodyHTML.includes('sector-I4') || bodyHTML.includes('sector-S5');
  
  console.log(`HTML contains stadium-viewer-container: ${hasStadiumViewer}`);
  console.log(`HTML contains stadium-main-view: ${hasStadiumMainView}`);
  console.log(`HTML contains stadium-svg-container: ${hasSVGContainer}`);
  console.log(`HTML contains SVG element: ${hasSVGElement}`);
  console.log(`HTML contains HNK sectors: ${hasHNKSectors}`);
  
  // Save a full screenshot
  await page.screenshot({ 
    path: 'test-results/debug-stadium-full-page.png',
    fullPage: true 
  });
  
  // Try to locate any SVG elements on the page
  const allSVGs = page.locator('svg');
  const svgCount = await allSVGs.count();
  console.log(`Total SVG elements on page: ${svgCount}`);
  
  // Check for any elements with specific IDs we expect
  const expectedIds = ['stadium-svg', 'stadium-svg-container', 'stadium-main-view', 'sector-I4', 'sector-S5'];
  for (const id of expectedIds) {
    const element = page.locator(`#${id}`);
    const count = await element.count();
    console.log(`Element #${id} count: ${count}`);
  }
  
  console.log('=== END DEBUGGING ===');
});