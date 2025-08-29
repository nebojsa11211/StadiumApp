import { test, expect } from '@playwright/test';

test.describe('Stadium Visibility Tests', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the stadium HTML file
    await page.goto('file:///D:/AiApps/StadiumApp/StadiumApp/stadium-complete.html');
    
    // Wait for the stadium to load and animate
    await page.waitForTimeout(1000);
  });

  test('should display stadium container and header', async ({ page }) => {
    // Check if main container is visible
    const stadiumContainer = page.locator('.stadium-container');
    await expect(stadiumContainer).toBeVisible();
    
    // Check if header is visible and contains correct text
    const header = page.locator('.stadium-header h1');
    await expect(header).toBeVisible();
    await expect(header).toContainText('Stadium Seating Map');
  });

  test('should display SVG stadium with correct dimensions', async ({ page }) => {
    // Check if SVG is visible
    const svg = page.locator('.stadium-svg');
    await expect(svg).toBeVisible();
    
    // Verify SVG has correct viewBox
    await expect(svg).toHaveAttribute('viewBox', '0 0 900 700');
    
    // Check if SVG has proper ARIA label
    await expect(svg).toHaveAttribute('aria-label', 'Interactive stadium seating map with individual seats');
  });

  test('should display stadium pitch and outline', async ({ page }) => {
    // Check stadium outline
    const outline = page.locator('.stadium-outline');
    await expect(outline).toBeVisible();
    
    // Check pitch
    const pitch = page.locator('.pitch');
    await expect(pitch).toBeVisible();
    
    // Check pitch lines
    const pitchLines = page.locator('.pitch-lines');
    await expect(pitchLines).toBeVisible();
  });

  test('should display all four sectors', async ({ page }) => {
    // Check all four stand groups are visible
    const northStand = page.locator('[data-sector="north"]');
    const southStand = page.locator('[data-sector="south"]');
    const eastStand = page.locator('[data-sector="east"]');
    const westStand = page.locator('[data-sector="west"]');
    
    await expect(northStand).toBeVisible();
    await expect(southStand).toBeVisible();
    await expect(eastStand).toBeVisible();
    await expect(westStand).toBeVisible();
    
    // Check sector labels
    await expect(page.locator('text=NORTH')).toBeVisible();
    await expect(page.locator('text=SOUTH')).toBeVisible();
    await expect(page.locator('text=EAST')).toBeVisible();
    await expect(page.locator('text=WEST')).toBeVisible();
  });

  test('should display individual seats', async ({ page }) => {
    // Wait for seats to be generated
    await page.waitForTimeout(500);
    
    // Check if seats containers exist
    const seatsContainers = page.locator('.seats-container');
    await expect(seatsContainers).toHaveCount(4); // One for each sector
    
    // Check if seat groups exist
    const seatGroups = page.locator('.seat-group');
    expect(await seatGroups.count()).toBeGreaterThan(0);
    
    // Check if seat circles are visible
    const seatCircles = page.locator('.seat-circle');
    expect(await seatCircles.count()).toBeGreaterThan(100); // Should have many seats
    
    // Check if seat numbers are visible
    const seatNumbers = page.locator('.seat-number');
    expect(await seatNumbers.count()).toBeGreaterThan(100);
  });

  test('should display controls and legend', async ({ page }) => {
    // Check controls are visible
    const controls = page.locator('.stadium-controls');
    await expect(controls).toBeVisible();
    
    // Check control buttons
    await expect(page.locator('button:has-text("Clear All")')).toBeVisible();
    await expect(page.locator('button:has-text("Select North")')).toBeVisible();
    await expect(page.locator('button:has-text("Export PNG")')).toBeVisible();
    
    // Check legend
    const legend = page.locator('.seat-legend');
    await expect(legend).toBeVisible();
    
    // Check legend items
    await expect(page.locator('.legend-item:has-text("Available")')).toBeVisible();
    await expect(page.locator('.legend-item:has-text("Selected")')).toBeVisible();
    await expect(page.locator('.legend-item:has-text("Occupied")')).toBeVisible();
    await expect(page.locator('.legend-item:has-text("VIP")')).toBeVisible();
  });

  test('should have responsive layout', async ({ page }) => {
    // Test desktop view
    await page.setViewportSize({ width: 1200, height: 800 });
    const svg = page.locator('.stadium-svg');
    await expect(svg).toBeVisible();
    
    // Test tablet view
    await page.setViewportSize({ width: 768, height: 1024 });
    await expect(svg).toBeVisible();
    
    // Test mobile view
    await page.setViewportSize({ width: 375, height: 667 });
    await expect(svg).toBeVisible();
    
    // Stadium should still be visible on small screens
    const stadiumContainer = page.locator('.stadium-container');
    await expect(stadiumContainer).toBeVisible();
  });

  test('should handle hover interactions', async ({ page }) => {
    // Hover over a sector
    const northStand = page.locator('[data-sector="north"]');
    await northStand.hover();
    
    // Check if tooltip appears (it should become visible)
    const tooltip = page.locator('.tooltip');
    await expect(tooltip).toHaveClass(/visible/);
    
    // Move away and check tooltip disappears
    await page.locator('body').hover();
    await page.waitForTimeout(300);
    await expect(tooltip).not.toHaveClass(/visible/);
  });

  test('should handle seat interactions', async ({ page }) => {
    // Wait for seats to be generated
    await page.waitForTimeout(500);
    
    // Find a seat and hover over it
    const firstSeat = page.locator('.seat-group').first();
    await expect(firstSeat).toBeVisible();
    
    await firstSeat.hover();
    
    // Check if tooltip shows seat information
    const tooltip = page.locator('.tooltip');
    await expect(tooltip).toHaveClass(/visible/);
    
    // Click the seat if it's not occupied
    const isOccupied = await firstSeat.evaluate(el => el.classList.contains('occupied'));
    if (!isOccupied) {
      await firstSeat.click();
      // Check if seat becomes selected
      await expect(firstSeat).toHaveClass(/selected/);
    }
  });

  test('should handle keyboard navigation', async ({ page }) => {
    // Focus on first sector
    const northStand = page.locator('[data-sector="north"]');
    await northStand.focus();
    
    // Check if focus is visible
    await expect(northStand).toBeFocused();
    
    // Press Enter to select
    await page.keyboard.press('Enter');
    
    // Tab to next focusable element
    await page.keyboard.press('Tab');
  });

  test('should take screenshot for visual verification', async ({ page }) => {
    // Take a full page screenshot
    await expect(page).toHaveScreenshot('stadium-full-view.png');
    
    // Take a screenshot of just the SVG
    const svg = page.locator('.stadium-svg');
    await expect(svg).toHaveScreenshot('stadium-svg-only.png');
  });
});

test.describe('Stadium JavaScript API Tests', () => {
  test('should initialize window.stadium object', async ({ page }) => {
    await page.goto('file:///D:/AiApps/StadiumApp/StadiumApp/stadium-complete.html');
    await page.waitForTimeout(1000);
    
    // Check if stadium API is available
    const stadiumAPI = await page.evaluate(() => {
      return typeof window.stadium === 'object' && window.stadium !== null;
    });
    expect(stadiumAPI).toBe(true);
    
    // Check if API methods exist
    const apiMethods = await page.evaluate(() => {
      const stadium = window.stadium;
      return {
        hasSelectSector: typeof stadium.selectSector === 'function',
        hasSelectSeat: typeof stadium.selectSeat === 'function',
        hasClearAllSelections: typeof stadium.clearAllSelections === 'function',
        hasGetSelectedSeats: typeof stadium.getSelectedSeats === 'function',
        hasSetSeatStatus: typeof stadium.setSeatStatus === 'function'
      };
    });
    
    expect(apiMethods.hasSelectSector).toBe(true);
    expect(apiMethods.hasSelectSeat).toBe(true);
    expect(apiMethods.hasClearAllSelections).toBe(true);
    expect(apiMethods.hasGetSelectedSeats).toBe(true);
    expect(apiMethods.hasSetSeatStatus).toBe(true);
  });

  test('should handle programmatic seat selection', async ({ page }) => {
    await page.goto('file:///D:/AiApps/StadiumApp/StadiumApp/stadium-complete.html');
    await page.waitForTimeout(1000);
    
    // Select a seat programmatically
    await page.evaluate(() => {
      window.stadium.selectSeat('north', 'A01');
    });
    
    // Check if seat is visually selected
    const selectedSeat = page.locator('[data-seat-number="A01"]');
    await expect(selectedSeat).toHaveClass(/selected/);
    
    // Clear selections
    await page.evaluate(() => {
      window.stadium.clearAllSelections();
    });
    
    // Check if seat is no longer selected
    await expect(selectedSeat).not.toHaveClass(/selected/);
  });
});

test.describe('Stadium Performance Tests', () => {
  test('should load within reasonable time', async ({ page }) => {
    const startTime = Date.now();
    
    await page.goto('file:///D:/AiApps/StadiumApp/StadiumApp/stadium-complete.html');
    await page.waitForLoadState('domcontentloaded');
    
    // Wait for seats to be generated
    await page.waitForTimeout(1000);
    
    const loadTime = Date.now() - startTime;
    expect(loadTime).toBeLessThan(3000); // Should load within 3 seconds
    
    console.log(`Stadium loaded in ${loadTime}ms`);
  });

  test('should handle many seat interactions without performance issues', async ({ page }) => {
    await page.goto('file:///D:/AiApps/StadiumApp/StadiumApp/stadium-complete.html');
    await page.waitForTimeout(1000);
    
    // Get all available seats
    const availableSeats = page.locator('.seat-group:not(.occupied)');
    const seatCount = await availableSeats.count();
    
    console.log(`Found ${seatCount} available seats`);
    
    // Click first 10 seats rapidly
    const maxSeats = Math.min(10, seatCount);
    for (let i = 0; i < maxSeats; i++) {
      await availableSeats.nth(i).click();
    }
    
    // Verify selections
    const selectedSeats = page.locator('.seat-group.selected');
    expect(await selectedSeats.count()).toBe(maxSeats);
  });
});