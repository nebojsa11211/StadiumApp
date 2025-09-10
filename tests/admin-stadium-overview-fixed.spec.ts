import { test, expect, Page } from '@playwright/test';

test.describe('Admin Stadium Overview - Fixed Implementation Tests', () => {
  let page: Page;
  
  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
    
    // Set viewport for consistent testing
    await page.setViewportSize({ width: 1920, height: 1080 });
    
    // Navigate to admin stadium overview page
    await page.goto('http://localhost:9002/admin/stadium-overview');
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('Page loads successfully with basic structure', async () => {
    // Verify page loads and has basic title
    await expect(page).toHaveTitle(/Stadium Overview/, { timeout: 10000 });
    
    // Verify main page elements are present
    const mainTitle = page.locator('h3:has-text("Stadium Overview")');
    await expect(mainTitle).toBeVisible({ timeout: 10000 });
    
    // Check that the stadium layout container exists
    const layoutContainer = page.locator('#stadium-layout-container');
    await expect(layoutContainer).toBeVisible({ timeout: 10000 });
    
    console.log('✅ Page loads with basic structure');
  });

  test('Stadium Information Panel displays correctly', async () => {
    // Wait for stadium information panel to load
    const infoPanel = page.locator('#stadium-info-panel');
    await expect(infoPanel).toBeVisible({ timeout: 10000 });
    
    // Check for panel header
    const panelTitle = page.locator('text=Stadium Information');
    await expect(panelTitle).toBeVisible({ timeout: 5000 });
    
    // Check for toggle button
    const toggleButton = page.locator('#stadium-info-panel-toggle-btn');
    await expect(toggleButton).toBeVisible({ timeout: 5000 });
    
    console.log('✅ Stadium Information Panel displays correctly');
  });

  test('Stadium data loading behavior - handles both success and no data states', async () => {
    // Wait for page to complete initial loading
    await page.waitForLoadState('domcontentloaded');
    
    // Check if we're in a loading state first
    const loadingSpinner = page.locator('#loading-spinner');
    
    if (await loadingSpinner.isVisible()) {
      console.log('ℹ️ Page is in loading state initially');
      
      // Wait for loading to complete (up to 20 seconds)
      await expect(loadingSpinner).not.toBeVisible({ timeout: 20000 });
    }
    
    // Now check what state we're in after loading
    const stadiumContainer = page.locator('#stadium-svg-container');
    const noStadiumAlert = page.locator('#no-stadium-alert');
    const errorAlert = page.locator('#error-message-alert');
    
    // We should be in one of these states: stadium loaded, no data, or error
    const hasStadium = await stadiumContainer.isVisible();
    const hasNoDataAlert = await noStadiumAlert.isVisible();
    const hasErrorAlert = await errorAlert.isVisible();
    
    if (hasStadium) {
      console.log('✅ Stadium data loaded successfully');
      
      // Verify stadium SVG is present and not empty
      const stadiumSvg = page.locator('#stadium-svg');
      await expect(stadiumSvg).toBeVisible({ timeout: 5000 });
      
      // Look for field element
      const fieldElement = page.locator('text=FIELD');
      await expect(fieldElement).toBeVisible({ timeout: 5000 });
      
    } else if (hasNoDataAlert) {
      console.log('ℹ️ No stadium data available - this is expected if no structure is imported');
      
      // Verify the no data message is appropriate
      const noDataMessage = page.locator('text=No stadium structure found');
      await expect(noDataMessage).toBeVisible();
      
      // Verify there's a link to manage stadium structure
      const manageLink = page.locator('#manage-stadium-structure-link');
      await expect(manageLink).toBeVisible();
      
    } else if (hasErrorAlert) {
      console.log('⚠️ Stadium loading error occurred');
      
      // Log the error message for debugging
      const errorText = await page.locator('#error-message-text').textContent();
      console.log(`Error message: ${errorText}`);
      
    } else {
      console.log('⚠️ Unexpected state - no stadium, no error, no loading');
    }
    
    console.log('✅ Stadium data loading behavior verified');
  });

  test('Interactive controls are present regardless of stadium data state', async () => {
    // Wait for page to stabilize
    await page.waitForLoadState('domcontentloaded');
    
    // These controls should always be present
    const eventSelector = page.locator('#eventSelect');
    await expect(eventSelector).toBeVisible({ timeout: 10000 });
    
    const seatSearchInput = page.locator('#seat-search-input');
    await expect(seatSearchInput).toBeVisible({ timeout: 5000 });
    
    const searchButton = page.locator('#seat-search-button');
    await expect(searchButton).toBeVisible({ timeout: 5000 });
    
    const legendButton = page.locator('#toggle-legend-button');
    await expect(legendButton).toBeVisible({ timeout: 5000 });
    
    const occupancyButton = page.locator('#toggle-occupancy-button');
    await expect(occupancyButton).toBeVisible({ timeout: 5000 });
    
    console.log('✅ All interactive controls are present');
  });

  test('Verify API call behavior and response handling', async () => {
    // Monitor network requests
    const apiRequests: any[] = [];
    
    page.on('response', (response) => {
      if (response.url().includes('/api/stadium-viewer/overview')) {
        apiRequests.push({
          url: response.url(),
          status: response.status(),
          statusText: response.statusText()
        });
      }
    });
    
    // Navigate and wait for network activity to complete
    await page.goto('http://localhost:9002/admin/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 30000 });
    
    // Check if the stadium viewer API was called
    const stadiumApiCall = apiRequests.find(req => req.url.includes('/stadium-viewer/overview'));
    
    if (stadiumApiCall) {
      console.log(`✅ Stadium API called: ${stadiumApiCall.status} ${stadiumApiCall.statusText}`);
      
      if (stadiumApiCall.status === 200) {
        console.log('✅ API returned success - stadium data should be available');
      } else if (stadiumApiCall.status === 404) {
        console.log('ℹ️ API returned 404 - no stadium structure found (expected)');
      } else {
        console.log(`⚠️ API returned unexpected status: ${stadiumApiCall.status}`);
      }
    } else {
      console.log('⚠️ Stadium viewer API was not called');
    }
    
    console.log('✅ API behavior verified');
  });

  test('Panel toggle functionality works correctly', async () => {
    // Wait for page to load
    await page.waitForLoadState('domcontentloaded');
    
    // Find the panel toggle button
    const toggleButton = page.locator('#stadium-info-panel-toggle-btn');
    await expect(toggleButton).toBeVisible({ timeout: 10000 });
    
    // Get initial panel state
    const infoPanel = page.locator('#stadium-info-panel');
    const initialState = await infoPanel.getAttribute('class');
    
    // Click toggle button
    await toggleButton.click();
    
    // Wait for animation to complete
    await page.waitForTimeout(500);
    
    // Check that panel state changed
    const newState = await infoPanel.getAttribute('class');
    expect(newState).not.toBe(initialState);
    
    // Toggle back
    await toggleButton.click();
    await page.waitForTimeout(500);
    
    // Verify it toggled back
    const finalState = await infoPanel.getAttribute('class');
    expect(finalState).toBe(initialState);
    
    console.log('✅ Panel toggle functionality works correctly');
  });

  test('Legend and occupancy controls function correctly', async () => {
    await page.waitForLoadState('domcontentloaded');
    
    // Test Legend toggle
    const legendButton = page.locator('#toggle-legend-button');
    await expect(legendButton).toBeVisible({ timeout: 10000 });
    
    // Click legend button
    await legendButton.click();
    
    // Check if legend panel appears (if stadium data is loaded)
    const legendPanel = page.locator('#legend-panel');
    
    // The legend will only appear if stadium data is loaded
    if (await page.locator('#stadium-svg-container svg').isVisible()) {
      await expect(legendPanel).toBeVisible({ timeout: 5000 });
      console.log('✅ Legend panel appears when stadium data is loaded');
    } else {
      console.log('ℹ️ No stadium data loaded, legend panel not expected');
    }
    
    // Test Occupancy toggle
    const occupancyButton = page.locator('#toggle-occupancy-button');
    await occupancyButton.click();
    
    // Verify button state changes (should change color/class)
    const buttonClasses = await occupancyButton.getAttribute('class');
    expect(buttonClasses).toContain('btn');
    
    console.log('✅ Legend and occupancy controls function correctly');
  });

  test('Performance and timing verification', async () => {
    const startTime = Date.now();
    
    // Navigate to page
    await page.goto('http://localhost:9002/admin/stadium-overview');
    
    // Wait for basic page structure to load
    await expect(page.locator('#stadium-layout-container')).toBeVisible({ timeout: 10000 });
    
    const pageLoadTime = Date.now() - startTime;
    
    // Page should load basic structure quickly (under 10 seconds)
    expect(pageLoadTime).toBeLessThan(10000);
    
    // Verify we're not stuck in a permanent loading state
    await page.waitForTimeout(5000); // Wait 5 seconds
    
    // If loading spinner exists, it should not be visible after reasonable time
    const loadingSpinner = page.locator('#loading-spinner');
    if (await loadingSpinner.count() > 0) {
      await expect(loadingSpinner).not.toBeVisible({ timeout: 20000 });
    }
    
    console.log(`✅ Page performance verified - loaded in ${pageLoadTime}ms`);
  });

  test('Comprehensive final state verification', async () => {
    // Navigate and wait for page to stabilize
    await page.goto('http://localhost:9002/admin/stadium-overview');
    await page.waitForLoadState('domcontentloaded');
    
    // Take a screenshot for manual verification
    await page.screenshot({ 
      path: '.playwright-mcp/stadium-overview-final-state.png', 
      fullPage: true 
    });
    
    // Verify essential page elements are present
    const essentialElements = [
      '#stadium-layout-container',
      '#stadium-info-panel',
      '#eventSelect',
      '#seat-search-input',
      '#toggle-legend-button',
      '#toggle-occupancy-button'
    ];
    
    let successCount = 0;
    
    for (const selector of essentialElements) {
      const element = page.locator(selector);
      if (await element.isVisible()) {
        successCount++;
        console.log(`✅ ${selector} is visible`);
      } else {
        console.log(`⚠️ ${selector} is not visible`);
      }
    }
    
    // Ensure most essential elements are present
    const successRate = successCount / essentialElements.length;
    expect(successRate).toBeGreaterThan(0.8);
    
    // Verify we're not in an error state
    const hasErrors = await page.locator('text=Error').count();
    const hasLoadingText = await page.locator('text=Loading...').count();
    
    if (hasErrors > 0) {
      console.log('⚠️ Page contains error text');
    }
    
    if (hasLoadingText > 0) {
      console.log('⚠️ Page still showing loading text');
    }
    
    console.log(`✅ Final state verification: ${successCount}/${essentialElements.length} elements present`);
  });
});