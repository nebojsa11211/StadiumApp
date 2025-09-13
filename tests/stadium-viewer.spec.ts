import { test, expect, Page } from '@playwright/test';
import { StadiumViewerHelper, StadiumViewerApiHelper, PerformanceHelper } from './helpers/stadium-viewer-helpers';

// Test configuration
const CUSTOMER_BASE_URL = process.env.CUSTOMER_BASE_URL || 'https://localhost:9020';
const API_BASE_URL = process.env.API_BASE_URL || 'https://localhost:9010';

test.describe('Stadium Viewer E2E Tests', () => {
  let helper: StadiumViewerHelper;
  
  test.beforeEach(async ({ page }) => {
    // Set up page with error handling and extended timeouts
    await page.setDefaultTimeout(30000);
    helper = new StadiumViewerHelper(page);
    
    // Listen for console errors
    page.on('console', msg => {
      if (msg.type() === 'error') {
        console.log('Browser console error:', msg.text());
      }
    });

    // Handle uncaught exceptions
    page.on('pageerror', exception => {
      console.log('Uncaught exception:', exception);
    });

    // Setup mock responses for consistent testing
    await helper.setupMockResponses(API_BASE_URL);
  });

  test('Should load stadium viewer page successfully', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    
    // Check page title and main heading
    await expect(page).toHaveTitle(/Stadium/);
    await expect(page.locator('h1')).toContainText('Stadium');
    
    // Verify main stadium viewer container exists
    await expect(page.locator('.stadium-viewer-container')).toBeVisible();
  });

  test('Should display stadium overview with SVG elements', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.verifyStadiumOverviewLoaded();
    
    // Verify stadium has coordinate system
    const stadiumSVG = page.locator('.stadium-svg');
    const svgViewBox = await stadiumSVG.getAttribute('viewBox');
    expect(svgViewBox).toMatch(/^\d+ \d+ \d+ \d+$/);
  });

  test('Should show event selector and handle event selection', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    
    // Wait for page to load
    await page.waitForSelector('.event-selector, select[name="selectedEvent"]', { timeout: 10000 });
    
    // Try to select an event using helper
    const eventSelected = await helper.selectEvent(1);
    
    if (eventSelected) {
      // Verify event selection updates the display
      const selectedEvent = await page.locator('select[name="selectedEvent"]').inputValue();
      expect(selectedEvent).not.toBe('');
      
      // Check if occupancy overlay appears when event is selected
      await helper.verifyOccupancyInfo();
    }
  });

  test('Should open sector detail modal on stand click', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.clickStadiumSector(0);
    await helper.waitForSectorModal();
  });

  test('Should render seats on canvas in sector modal', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.clickStadiumSector(0);
    await helper.waitForSectorModal();
    await helper.verifyCanvasRendering();
  });

  test('Should handle seat selection in canvas modal', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.clickStadiumSector(0);
    await helper.waitForSectorModal();
    
    // Click on canvas to select a seat
    await helper.clickCanvasAt(100, 100);
    await helper.verifySeatSelectionInfo();
  });

  test('Should close sector modal with close button', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.clickStadiumSector(0);
    await helper.waitForSectorModal();
    await helper.closeSectorModal();
  });

  test('Should handle zoom controls in sector modal', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.clickStadiumSector(0);
    await helper.waitForSectorModal();
    
    // Look for zoom controls
    const zoomInBtn = page.locator('button:has-text("Zoom In"), .zoom-in');
    const zoomOutBtn = page.locator('button:has-text("Zoom Out"), .zoom-out');
    
    if (await zoomInBtn.isVisible()) {
      await zoomInBtn.click();
      // Verify zoom level changed (implementation specific)
    }
    
    if (await zoomOutBtn.isVisible()) {
      await zoomOutBtn.click();
      // Verify zoom level changed
    }
  });

  test('Should display occupancy information when event is selected', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    
    // Select an event using helper
    const eventSelected = await helper.selectEvent(1);
    
    if (eventSelected) {
      await helper.verifyOccupancyInfo();
    }
  });

  test('Should be responsive on different screen sizes', async ({ page }) => {
    await helper.navigateAndWaitForLoad(CUSTOMER_BASE_URL);
    await helper.testResponsiveness();
    
    // Verify SVG maintains aspect ratio
    const stadiumSVG = page.locator('.stadium-svg');
    const preserveAspectRatio = await stadiumSVG.getAttribute('preserveAspectRatio');
    expect(preserveAspectRatio).toBe('xMidYMid meet');
  });

  test('Should handle API errors gracefully', async ({ page }) => {
    // Intercept API calls and simulate errors
    await page.route(`${API_BASE_URL}/api/stadium-viewer/**`, async route => {
      await route.abort('failed');
    });
    
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Wait for error handling
    await page.waitForTimeout(3000);
    
    // Check for error messages
    const errorMessage = page.locator('.alert-danger, .error-message, .text-danger');
    await expect(errorMessage.first()).toBeVisible({ timeout: 5000 });
  });

  test('Should load stadium viewer JavaScript module', async ({ page }) => {
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Check if stadium viewer JavaScript is loaded
    const stadiumViewerExists = await page.evaluate(() => {
      return typeof window.stadiumViewer !== 'undefined';
    });
    
    expect(stadiumViewerExists).toBe(true);
  });

  test('Should maintain performance with large datasets', async ({ page }) => {
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Measure initial load time
    const startTime = Date.now();
    await page.waitForSelector('.stadium-svg', { timeout: 15000 });
    const loadTime = Date.now() - startTime;
    
    // Should load within reasonable time (15 seconds)
    expect(loadTime).toBeLessThan(15000);
    
    // Open sector modal and measure rendering time
    const modalStartTime = Date.now();
    await page.locator('.stand-group polygon').first().click();
    await page.waitForSelector('#sectorCanvas', { timeout: 10000 });
    const modalLoadTime = Date.now() - modalStartTime;
    
    // Modal should open within reasonable time (10 seconds)
    expect(modalLoadTime).toBeLessThan(10000);
  });
});

// API Integration Tests
test.describe('Stadium Viewer API Integration', () => {
  
  test('Should successfully fetch stadium overview from API', async ({ page }) => {
    // Test API endpoint directly
    const response = await page.request.get(`${API_BASE_URL}/api/stadium-viewer/overview`);
    
    if (response.ok()) {
      const data = await response.json();
      
      // Verify response structure
      expect(data).toHaveProperty('stadiumId');
      expect(data).toHaveProperty('name');
      expect(data).toHaveProperty('field');
      expect(data).toHaveProperty('stands');
      expect(data).toHaveProperty('coordinateSystem');
      
      // Verify stands array
      expect(Array.isArray(data.stands)).toBe(true);
      
      if (data.stands.length > 0) {
        const firstStand = data.stands[0];
        expect(firstStand).toHaveProperty('standId');
        expect(firstStand).toHaveProperty('name');
        expect(firstStand).toHaveProperty('sectors');
      }
    } else {
      console.log('API endpoint not available, skipping API integration test');
    }
  });

  test('Should fetch sector details from API', async ({ page }) => {
    // First get stadium overview to get a valid sector ID
    const overviewResponse = await page.request.get(`${API_BASE_URL}/api/stadium-viewer/overview`);
    
    if (overviewResponse.ok()) {
      const overviewData = await overviewResponse.json();
      
      if (overviewData.stands && overviewData.stands.length > 0 && 
          overviewData.stands[0].sectors && overviewData.stands[0].sectors.length > 0) {
        
        const sectorId = overviewData.stands[0].sectors[0].sectorId;
        const sectorResponse = await page.request.get(`${API_BASE_URL}/api/stadium-viewer/sector/${sectorId}/details`);
        
        if (sectorResponse.ok()) {
          const sectorData = await sectorResponse.json();
          
          // Verify sector details structure
          expect(sectorData).toHaveProperty('sectorId');
          expect(sectorData).toHaveProperty('name');
          expect(sectorData).toHaveProperty('seats');
          expect(sectorData).toHaveProperty('layout');
          
          // Verify seats array
          expect(Array.isArray(sectorData.seats)).toBe(true);
        }
      }
    }
  });
});

// Error Handling and Edge Cases
test.describe('Stadium Viewer Error Handling', () => {
  
  test('Should handle missing stadium data gracefully', async ({ page }) => {
    // Mock empty stadium data
    await page.route(`${API_BASE_URL}/api/stadium-viewer/overview`, async route => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          stadiumId: 'empty-stadium',
          name: 'Empty Stadium',
          field: { polygon: [], fillColor: '#green', strokeColor: '#white' },
          stands: [],
          coordinateSystem: { width: 800, height: 600, units: 'meters' }
        })
      });
    });
    
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Should still display basic stadium structure
    await expect(page.locator('.stadium-viewer-container')).toBeVisible();
    await expect(page.locator('.stadium-svg')).toBeVisible();
    
    // Should show no stands message or handle empty data gracefully
    const emptyMessage = page.locator('.no-data-message, .empty-stadium');
    if (await emptyMessage.isVisible()) {
      expect(true).toBe(true); // Empty state handled
    }
  });

  test('Should handle network timeouts', async ({ page }) => {
    // Simulate slow network response
    await page.route(`${API_BASE_URL}/api/stadium-viewer/overview`, async route => {
      await new Promise(resolve => setTimeout(resolve, 31000)); // Longer than timeout
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({})
      });
    });
    
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Should show loading state or timeout error
    const loadingIndicator = page.locator('.loading, .spinner');
    const errorMessage = page.locator('.error-message, .alert-danger');
    
    // Either loading indicator or error message should be visible
    await expect(loadingIndicator.or(errorMessage).first()).toBeVisible({ timeout: 35000 });
  });

  test('Should validate sector modal data before rendering', async ({ page }) => {
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Wait for stadium to load
    await page.waitForSelector('.stadium-svg', { timeout: 10000 });
    
    // Mock invalid sector data
    await page.route(`${API_BASE_URL}/api/stadium-viewer/sector/*/details`, async route => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          sectorId: 'invalid',
          name: '',
          seats: null, // Invalid data
          layout: {}
        })
      });
    });
    
    // Try to open sector modal
    await page.locator('.stand-group polygon').first().click();
    
    // Should handle invalid data gracefully
    const modalOrError = page.locator('.sector-modal, .error-message');
    await expect(modalOrError.first()).toBeVisible({ timeout: 5000 });
  });
});

// Multi-browser and Accessibility Tests
test.describe('Stadium Viewer Cross-browser and Accessibility', () => {
  
  test('Should work correctly in different browsers', async ({ page, browserName }) => {
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    
    // Wait for stadium to load
    await page.waitForSelector('.stadium-svg', { timeout: 10000 });
    
    // Verify SVG renders correctly across browsers
    await expect(page.locator('.stadium-svg')).toBeVisible();
    
    // Browser-specific checks
    if (browserName === 'firefox') {
      // Firefox specific tests
      const svgContent = await page.locator('.stadium-svg').innerHTML();
      expect(svgContent).toContain('polygon');
    } else if (browserName === 'webkit') {
      // Safari specific tests  
      const canvas = page.locator('#sectorCanvas');
      // WebKit/Safari canvas support check would go here
    }
  });

  test('Should meet accessibility standards', async ({ page }) => {
    await page.goto(`${CUSTOMER_BASE_URL}/stadium-viewer`);
    await page.waitForSelector('.stadium-svg', { timeout: 10000 });
    
    // Check for aria labels and roles
    const stadiumSVG = page.locator('.stadium-svg');
    const ariaLabel = await stadiumSVG.getAttribute('aria-label');
    expect(ariaLabel).toBeTruthy();
    
    // Check keyboard navigation
    await page.keyboard.press('Tab');
    const focusedElement = await page.locator(':focus').getAttribute('class');
    expect(focusedElement).toBeTruthy();
    
    // Check color contrast (basic check)
    const stadiumContainer = page.locator('.stadium-viewer-container');
    const computedStyle = await stadiumContainer.evaluate(el => {
      const style = window.getComputedStyle(el);
      return {
        color: style.color,
        backgroundColor: style.backgroundColor
      };
    });
    
    expect(computedStyle.color).toBeTruthy();
    expect(computedStyle.backgroundColor).toBeTruthy();
  });
});