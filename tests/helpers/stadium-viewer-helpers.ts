import { Page, expect } from '@playwright/test';

/**
 * Stadium Viewer Test Helper Functions
 * Provides common operations and utilities for testing stadium viewer functionality
 */

// Mock data for testing
export const mockStadiumData = {
  stadiumId: 'test-stadium-001',
  name: 'Test Stadium Arena',
  field: {
    polygon: [
      { x: 200, y: 150 },
      { x: 600, y: 150 },
      { x: 600, y: 450 },
      { x: 200, y: 450 }
    ],
    fillColor: '#2d5a2d',
    strokeColor: '#ffffff'
  },
  stands: [
    {
      standId: 'north-stand',
      name: 'North Stand',
      tribuneCode: 'N',
      color: '#1e40af',
      polygon: [
        { x: 150, y: 50 },
        { x: 650, y: 50 },
        { x: 600, y: 150 },
        { x: 200, y: 150 }
      ],
      sectors: [
        {
          sectorId: 'n1a',
          name: 'N1A',
          color: '#3b82f6',
          polygon: [
            { x: 150, y: 50 },
            { x: 300, y: 50 },
            { x: 280, y: 150 },
            { x: 200, y: 150 }
          ],
          totalSeats: 500,
          availableSeats: 450,
          occupancyPercentage: 10.0
        },
        {
          sectorId: 'n1b',
          name: 'N1B',
          color: '#3b82f6',
          polygon: [
            { x: 300, y: 50 },
            { x: 500, y: 50 },
            { x: 480, y: 150 },
            { x: 320, y: 150 }
          ],
          totalSeats: 600,
          availableSeats: 500,
          occupancyPercentage: 16.7
        }
      ]
    },
    {
      standId: 'south-stand',
      name: 'South Stand',
      tribuneCode: 'S',
      color: '#dc2626',
      polygon: [
        { x: 200, y: 450 },
        { x: 600, y: 450 },
        { x: 650, y: 550 },
        { x: 150, y: 550 }
      ],
      sectors: [
        {
          sectorId: 's1a',
          name: 'S1A',
          color: '#ef4444',
          polygon: [
            { x: 200, y: 450 },
            { x: 280, y: 450 },
            { x: 300, y: 550 },
            { x: 150, y: 550 }
          ],
          totalSeats: 400,
          availableSeats: 320,
          occupancyPercentage: 20.0
        }
      ]
    }
  ],
  coordinateSystem: {
    width: 800,
    height: 600,
    units: 'meters',
    scale: 1.0
  }
};

export const mockSectorDetails = {
  sectorId: 'n1a',
  name: 'North Stand Section A',
  tribuneName: 'North Tribune',
  ringName: 'Ring 1',
  totalSeats: 500,
  layout: {
    rows: 25,
    seatsPerRow: 20,
    startRow: 1,
    startSeat: 1
  },
  seats: Array.from({ length: 500 }, (_, i) => ({
    seatId: `n1a-seat-${i + 1}`,
    row: Math.floor(i / 20) + 1,
    seatNumber: (i % 20) + 1,
    coordinates: {
      x: (i % 20) * 25 + 50,
      y: Math.floor(i / 20) * 20 + 50
    },
    status: i % 10 === 0 ? 'occupied' : 'available',
    price: 75.00
  }))
};

export const mockEventData = [
  {
    eventId: 1,
    eventName: 'Championship Final',
    eventDate: '2024-12-25',
    isActive: true
  },
  {
    eventId: 2,
    eventName: 'Season Opener',
    eventDate: '2024-08-15',
    isActive: true
  }
];

/**
 * Stadium Viewer Page Helper Class
 * Provides high-level operations for interacting with stadium viewer page
 */
export class StadiumViewerHelper {
  constructor(private page: Page) {}

  /**
   * Navigate to stadium viewer page and wait for it to load
   */
  async navigateAndWaitForLoad(baseUrl: string = 'http://localhost:9001') {
    await this.page.goto(`${baseUrl}/stadium-viewer`);
    await this.page.waitForSelector('.stadium-viewer-container', { timeout: 30000 });
    await this.page.waitForSelector('.stadium-svg', { timeout: 15000 });
  }

  /**
   * Setup mock API responses for testing
   */
  async setupMockResponses(apiBaseUrl: string = 'http://localhost:9000') {
    // Mock stadium overview API
    await this.page.route(`${apiBaseUrl}/api/stadium-viewer/overview`, async route => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockStadiumData)
      });
    });

    // Mock sector details API
    await this.page.route(`${apiBaseUrl}/api/stadium-viewer/sector/*/details`, async route => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockSectorDetails)
      });
    });

    // Mock events API
    await this.page.route(`${apiBaseUrl}/api/customer/ticketing/events`, async route => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockEventData)
      });
    });

    // Mock seat status API
    await this.page.route(`${apiBaseUrl}/api/stadium-viewer/event/*/seat-status`, async route => {
      const eventId = route.url().split('/')[7]; // Extract event ID from URL
      const seatStatuses = mockSectorDetails.seats.map(seat => ({
        seatId: seat.seatId,
        sectorId: mockSectorDetails.sectorId,
        status: seat.status,
        coordinates: seat.coordinates
      }));

      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(seatStatuses)
      });
    });
  }

  /**
   * Verify stadium overview elements are visible
   */
  async verifyStadiumOverviewLoaded() {
    await expect(this.page.locator('.stadium-svg')).toBeVisible();
    await expect(this.page.locator('.field-group')).toBeVisible();
    await expect(this.page.locator('.stand-group')).toHaveCount(mockStadiumData.stands.length);
  }

  /**
   * Select an event from the event selector
   */
  async selectEvent(eventIndex: number = 1) {
    const eventSelector = this.page.locator('select[name="selectedEvent"]');
    await expect(eventSelector).toBeVisible();
    
    const options = eventSelector.locator('option');
    const optionCount = await options.count();
    
    if (optionCount > eventIndex) {
      await eventSelector.selectOption({ index: eventIndex });
      return true;
    }
    return false;
  }

  /**
   * Click on a stadium sector/stand
   */
  async clickStadiumSector(sectorIndex: number = 0) {
    const sectors = this.page.locator('.sector-group polygon, .stand-group polygon');
    await expect(sectors.nth(sectorIndex)).toBeVisible();
    await sectors.nth(sectorIndex).click();
  }

  /**
   * Wait for sector modal to open and verify content
   */
  async waitForSectorModal() {
    await expect(this.page.locator('.sector-modal')).toBeVisible({ timeout: 10000 });
    await expect(this.page.locator('#sectorCanvas')).toBeVisible();
    
    // Wait for canvas to be initialized with proper dimensions
    const canvas = this.page.locator('#sectorCanvas');
    const width = await canvas.getAttribute('width');
    const height = await canvas.getAttribute('height');
    
    expect(parseInt(width || '0')).toBeGreaterThan(0);
    expect(parseInt(height || '0')).toBeGreaterThan(0);
  }

  /**
   * Close the sector modal
   */
  async closeSectorModal() {
    const closeButton = this.page.locator('.sector-modal .btn-close, .sector-modal button:has-text("Close")');
    await closeButton.first().click();
    await expect(this.page.locator('.sector-modal')).not.toBeVisible({ timeout: 5000 });
  }

  /**
   * Click on canvas at specific coordinates
   */
  async clickCanvasAt(x: number, y: number) {
    const canvas = this.page.locator('#sectorCanvas');
    await canvas.click({ position: { x, y } });
  }

  /**
   * Verify seat selection information is displayed
   */
  async verifySeatSelectionInfo() {
    const selectionInfo = this.page.locator('.seat-selection-info, .selected-seat-info');
    await expect(selectionInfo).toBeVisible({ timeout: 3000 });
  }

  /**
   * Verify occupancy information is displayed
   */
  async verifyOccupancyInfo() {
    await expect(this.page.locator('.occupancy-info')).toBeVisible();
    const occupancyText = await this.page.locator('.occupancy-info').textContent();
    expect(occupancyText).toMatch(/\d+%/);
  }

  /**
   * Test viewport responsiveness
   */
  async testResponsiveness() {
    const viewports = [
      { width: 1920, height: 1080, name: 'desktop' },
      { width: 1024, height: 768, name: 'tablet' },
      { width: 390, height: 844, name: 'mobile' }
    ];

    for (const viewport of viewports) {
      await this.page.setViewportSize(viewport);
      await expect(this.page.locator('.stadium-viewer-container')).toBeVisible();
      await expect(this.page.locator('.stadium-svg')).toBeVisible();
    }
  }

  /**
   * Verify canvas rendering performance
   */
  async verifyCanvasRendering() {
    const canvas = this.page.locator('#sectorCanvas');
    
    // Wait for canvas to be rendered (give time for JavaScript to draw)
    await this.page.waitForTimeout(2000);
    
    // Check if canvas has content by examining image data
    const hasContent = await this.page.evaluate(() => {
      const canvas = document.getElementById('sectorCanvas') as HTMLCanvasElement;
      if (!canvas) return false;
      
      const ctx = canvas.getContext('2d');
      if (!ctx) return false;
      
      // Check for any non-transparent pixels
      const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
      for (let i = 3; i < imageData.data.length; i += 4) {
        if (imageData.data[i] !== 0) return true;
      }
      return false;
    });
    
    expect(hasContent).toBe(true);
  }

  /**
   * Measure page load performance
   */
  async measureLoadPerformance() {
    const startTime = Date.now();
    await this.verifyStadiumOverviewLoaded();
    const loadTime = Date.now() - startTime;
    
    return {
      loadTime,
      isAcceptable: loadTime < 15000 // Should load within 15 seconds
    };
  }

  /**
   * Test error handling with network failures
   */
  async simulateNetworkError(apiBaseUrl: string = 'http://localhost:9000') {
    await this.page.route(`${apiBaseUrl}/api/stadium-viewer/**`, async route => {
      await route.abort('failed');
    });
  }

  /**
   * Verify error message display
   */
  async verifyErrorHandling() {
    const errorSelectors = [
      '.alert-danger',
      '.error-message',
      '.text-danger',
      '.error-container',
      '[class*="error"]'
    ];
    
    let errorFound = false;
    for (const selector of errorSelectors) {
      const element = this.page.locator(selector);
      if (await element.isVisible()) {
        errorFound = true;
        break;
      }
    }
    
    expect(errorFound).toBe(true);
  }

  /**
   * Verify accessibility features
   */
  async verifyAccessibility() {
    // Check for ARIA labels
    const stadiumSVG = this.page.locator('.stadium-svg');
    const ariaLabel = await stadiumSVG.getAttribute('aria-label');
    expect(ariaLabel).toBeTruthy();

    // Test keyboard navigation
    await this.page.keyboard.press('Tab');
    const focusedElement = this.page.locator(':focus');
    await expect(focusedElement).toBeVisible();

    // Check color contrast basics
    const container = this.page.locator('.stadium-viewer-container');
    const style = await container.evaluate(el => {
      const computed = window.getComputedStyle(el);
      return {
        color: computed.color,
        backgroundColor: computed.backgroundColor
      };
    });

    expect(style.color).toBeTruthy();
    expect(style.backgroundColor).toBeTruthy();
  }

  /**
   * Get canvas context for direct testing
   */
  async getCanvasImageData() {
    return await this.page.evaluate(() => {
      const canvas = document.getElementById('sectorCanvas') as HTMLCanvasElement;
      if (!canvas) return null;
      
      const ctx = canvas.getContext('2d');
      if (!ctx) return null;
      
      return ctx.getImageData(0, 0, canvas.width, canvas.height);
    });
  }

  /**
   * Verify JavaScript module loading
   */
  async verifyJavaScriptModules() {
    const stadiumViewerLoaded = await this.page.evaluate(() => {
      return typeof window.stadiumViewer !== 'undefined';
    });
    
    expect(stadiumViewerLoaded).toBe(true);
  }
}

/**
 * API Testing Helper Functions
 */
export class StadiumViewerApiHelper {
  constructor(private page: Page) {}

  /**
   * Test stadium overview API endpoint
   */
  async testOverviewEndpoint(apiBaseUrl: string = 'http://localhost:9000') {
    const response = await this.page.request.get(`${apiBaseUrl}/api/stadium-viewer/overview`);
    
    if (!response.ok()) {
      return { success: false, error: `API returned ${response.status()}` };
    }

    const data = await response.json();
    
    // Validate response structure
    const requiredFields = ['stadiumId', 'name', 'field', 'stands', 'coordinateSystem'];
    for (const field of requiredFields) {
      if (!data.hasOwnProperty(field)) {
        return { success: false, error: `Missing required field: ${field}` };
      }
    }

    return { success: true, data };
  }

  /**
   * Test sector details API endpoint
   */
  async testSectorDetailsEndpoint(sectorId: string, apiBaseUrl: string = 'http://localhost:9000') {
    const response = await this.page.request.get(`${apiBaseUrl}/api/stadium-viewer/sector/${sectorId}/details`);
    
    if (!response.ok()) {
      return { success: false, error: `API returned ${response.status()}` };
    }

    const data = await response.json();
    
    // Validate response structure
    const requiredFields = ['sectorId', 'name', 'seats', 'layout'];
    for (const field of requiredFields) {
      if (!data.hasOwnProperty(field)) {
        return { success: false, error: `Missing required field: ${field}` };
      }
    }

    return { success: true, data };
  }

  /**
   * Test seat status API endpoint
   */
  async testSeatStatusEndpoint(eventId: number, apiBaseUrl: string = 'http://localhost:9000') {
    const response = await this.page.request.get(`${apiBaseUrl}/api/stadium-viewer/event/${eventId}/seat-status`);
    
    if (!response.ok()) {
      return { success: false, error: `API returned ${response.status()}` };
    }

    const data = await response.json();
    
    // Validate array response
    if (!Array.isArray(data)) {
      return { success: false, error: 'Response should be an array of seat statuses' };
    }

    return { success: true, data };
  }
}

/**
 * Performance testing utilities
 */
export class PerformanceHelper {
  /**
   * Measure timing for a specific operation
   */
  static async measureOperation<T>(operation: () => Promise<T>): Promise<{ result: T; duration: number }> {
    const start = Date.now();
    const result = await operation();
    const duration = Date.now() - start;
    
    return { result, duration };
  }

  /**
   * Create performance assertions
   */
  static expectPerformance(duration: number, maxDuration: number, operation: string) {
    if (duration > maxDuration) {
      throw new Error(`Performance test failed: ${operation} took ${duration}ms, expected under ${maxDuration}ms`);
    }
  }
}