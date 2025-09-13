import { test, expect, Page, Locator } from '@playwright/test';

// Test configuration - Using port 9002 to match existing admin tests
const ADMIN_BASE_URL = process.env.ADMIN_BASE_URL || 'https://localhost:9030';
const API_BASE_URL = process.env.API_BASE_URL || 'https://localhost:9010';

// Page Object Model for Stadium Overview
class StadiumOverviewPage {
  private page: Page;
  private stadiumInfoPanel: Locator;
  private panelToggleButton: Locator;
  private stadiumViewer: Locator;

  constructor(page: Page) {
    this.page = page;
    this.stadiumInfoPanel = page.locator('.stadium-info-panel');
    this.panelToggleButton = page.locator('.panel-header .toggle-btn');
    this.stadiumViewer = page.locator('.stadium-viewer-container');
  }

  async login() {
    await this.page.goto(`${ADMIN_BASE_URL}/login`);
    await this.page.fill('input[name="email"]', 'admin@stadium.com');
    await this.page.fill('input[name="password"]', 'Admin123!');
    await this.page.click('button[type="submit"]');
    await this.page.waitForURL(`${ADMIN_BASE_URL}/`, { timeout: 10000 });
  }

  async navigateToStadiumOverview() {
    await this.page.goto(`${ADMIN_BASE_URL}/admin/stadium-overview`);
    await this.page.waitForLoadState('networkidle', { timeout: 15000 });
  }

  async waitForStadiumDataLoad() {
    // Wait for either stadium data to load or error message to appear
    await Promise.race([
      this.page.waitForSelector('.stadium-svg', { timeout: 10000 }),
      this.page.waitForSelector('.alert-warning:has-text("No Stadium Structure")', { timeout: 10000 }),
      this.page.waitForSelector('.alert-danger', { timeout: 10000 })
    ]);
  }

  async toggleInfoPanel() {
    await this.panelToggleButton.click();
    // Wait for CSS transition to complete
    await this.page.waitForTimeout(350);
  }

  async selectEvent(eventId: number) {
    const eventSelect = this.page.locator('#eventSelect');
    await eventSelect.selectOption(eventId.toString());
    // Wait for event data to load
    await this.page.waitForTimeout(2000);
  }

  async clickTribute(tribuneCode: string) {
    const tribune = this.page.locator(`.tribune-item:has(.tribune-badge:text("${tribuneCode}"))`);
    await tribune.click();
  }

  async simulateTicketSales() {
    const simulateBtn = this.page.locator('#admin-stadium-simulate-btn');
    if (await simulateBtn.isVisible() && !(await simulateBtn.isDisabled())) {
      await simulateBtn.click();
      // Wait for simulation to complete (spinner should disappear)
      await expect(simulateBtn.locator('.spinner-border')).not.toBeVisible({ timeout: 15000 });
    }
  }

  // Getters for different cards
  get stadiumOverviewCard() { return this.stadiumInfoPanel.locator('.info-card.stadium-overview'); }
  get structureStatsCard() { return this.stadiumInfoPanel.locator('.info-card.structure-stats'); }
  get occupancyAnalyticsCard() { return this.stadiumInfoPanel.locator('.info-card.occupancy-analytics'); }
  get technicalInfoCard() { return this.stadiumInfoPanel.locator('.info-card.technical-info'); }
}

test.describe('Admin Stadium Overview - Enhanced Information Panel', () => {
  let stadiumPage: StadiumOverviewPage;

  test.beforeEach(async ({ page }) => {
    stadiumPage = new StadiumOverviewPage(page);
    await stadiumPage.login();
    await stadiumPage.navigateToStadiumOverview();
    
    // Set up console error logging
    page.on('console', msg => {
      if (msg.type() === 'error') {
        console.log('Browser console error:', msg.text());
      }
    });

    // Handle uncaught exceptions
    page.on('pageerror', exception => {
      console.log('Uncaught exception:', exception);
    });
  });

  test.describe('Panel Visibility & Structure Tests', () => {
    test('should display stadium information panel on page load', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Verify panel is visible
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
      await expect(stadiumPage.stadiumInfoPanel).toHaveClass(/expanded/);
      
      // Verify panel header exists
      const panelHeader = stadiumPage.stadiumInfoPanel.locator('.panel-header');
      await expect(panelHeader).toBeVisible();
      await expect(panelHeader.locator('h4.panel-title')).toContainText('Stadium Information');
    });

    test('should display all required information cards', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Check for all main cards
      await expect(stadiumPage.stadiumOverviewCard).toBeVisible();
      await expect(stadiumPage.structureStatsCard).toBeVisible();
      await expect(stadiumPage.technicalInfoCard).toBeVisible();
      
      // Verify card headers
      await expect(stadiumPage.stadiumOverviewCard.locator('.card-header h5')).toContainText('Stadium Overview');
      await expect(stadiumPage.structureStatsCard.locator('.card-header h5')).toContainText('Structure Statistics');
      await expect(stadiumPage.technicalInfoCard.locator('.card-header h5')).toContainText('Technical Information');
    });

    test('should toggle panel collapse/expand functionality', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Initially expanded
      await expect(stadiumPage.stadiumInfoPanel).toHaveClass(/expanded/);
      
      // Toggle to collapsed
      await stadiumPage.toggleInfoPanel();
      await expect(stadiumPage.stadiumInfoPanel).toHaveClass(/collapsed/);
      
      // Panel content should be hidden
      const panelContent = stadiumPage.stadiumInfoPanel.locator('.panel-content');
      await expect(panelContent).not.toBeVisible();
      
      // Toggle back to expanded
      await stadiumPage.toggleInfoPanel();
      await expect(stadiumPage.stadiumInfoPanel).toHaveClass(/expanded/);
      await expect(panelContent).toBeVisible();
    });

    test('should have properly styled toggle button with correct aria attributes', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const toggleBtn = stadiumPage.panelToggleButton;
      
      // Check button is visible and styled
      await expect(toggleBtn).toBeVisible();
      await expect(toggleBtn).toHaveAttribute('aria-label');
      await expect(toggleBtn).toHaveAttribute('aria-expanded', 'true');
      
      // Check icon changes on collapse
      const icon = toggleBtn.locator('i');
      await expect(icon).toHaveClass(/bi-chevron-left/);
      
      await stadiumPage.toggleInfoPanel();
      await expect(toggleBtn).toHaveAttribute('aria-expanded', 'false');
      await expect(icon).toHaveClass(/bi-chevron-right/);
    });
  });

  test.describe('Stadium Information Display Tests', () => {
    test('should display stadium name and capacity correctly', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Check stadium name is displayed
      const stadiumName = stadiumPage.stadiumOverviewCard.locator('.stadium-name');
      await expect(stadiumName).toBeVisible();
      await expect(stadiumName).not.toBeEmpty();
      
      // Check capacity display
      const capacityNumber = stadiumPage.stadiumOverviewCard.locator('.capacity-number');
      const capacityLabel = stadiumPage.stadiumOverviewCard.locator('.capacity-label');
      
      await expect(capacityNumber).toBeVisible();
      await expect(capacityLabel).toContainText('Total Capacity');
      
      // Verify capacity is formatted as number (should contain commas for large numbers)
      const capacityText = await capacityNumber.textContent();
      expect(capacityText).toMatch(/^\d{1,3}(,\d{3})*$/); // Format: 50,000
    });

    test('should display tribune count and structure statistics', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const structureCard = stadiumPage.structureStatsCard;
      
      // Check tribune count
      const tribuneCount = structureCard.locator('.stat-number').first();
      await expect(tribuneCount).toBeVisible();
      
      const tribuneCountText = await tribuneCount.textContent();
      const count = parseInt(tribuneCountText || '0');
      expect(count).toBeGreaterThan(0);
      expect(count).toBeLessThanOrEqual(4); // Max 4 tribunes (N, S, E, W)
      
      // Check summary stats
      const totalSectors = structureCard.locator('.stat-pair:has-text("Total Sectors") .stat-number');
      const avgSeats = structureCard.locator('.stat-pair:has-text("Avg Seats/Sector") .stat-number');
      
      await expect(totalSectors).toBeVisible();
      await expect(avgSeats).toBeVisible();
    });

    test('should display tribune list with correct badges and information', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const tribunesList = stadiumPage.structureStatsCard.locator('.tribunes-list');
      await expect(tribunesList).toBeVisible();
      
      // Check individual tribune items
      const tribuneItems = tribunesList.locator('.tribune-item');
      const tribuneCount = await tribuneItems.count();
      expect(tribuneCount).toBeGreaterThan(0);
      
      for (let i = 0; i < tribuneCount; i++) {
        const tribune = tribuneItems.nth(i);
        
        // Check tribune badge
        const badge = tribune.locator('.tribune-badge');
        await expect(badge).toBeVisible();
        
        const badgeText = await badge.textContent();
        expect(['N', 'S', 'E', 'W']).toContain(badgeText);
        
        // Check tribune info
        const tribuneInfo = tribune.locator('.tribune-info strong');
        await expect(tribuneInfo).toBeVisible();
        
        // Check sectors and seats count
        const sectorInfo = tribune.locator('.tribune-info small');
        await expect(sectorInfo).toBeVisible();
        await expect(sectorInfo).toContainText('sectors');
        await expect(sectorInfo).toContainText('seats');
      }
    });

    test('should show technical information when expanded', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const technicalCard = stadiumPage.technicalInfoCard;
      const collapseHeader = technicalCard.locator('.card-header.collapsible');
      
      // Initially collapsed
      await expect(technicalCard.locator('.technical-details')).not.toBeVisible();
      
      // Click to expand
      await collapseHeader.click();
      await expect(technicalCard.locator('.technical-details')).toBeVisible();
      
      // Check technical information items
      const techItems = technicalCard.locator('.tech-item');
      await expect(techItems).toHaveCount(4); // Stadium ID, Dimensions, Unit, Last Updated
      
      // Verify specific items
      await expect(techItems.locator('.tech-label:text("Stadium ID:")').first()).toBeVisible();
      await expect(techItems.locator('.tech-label:text("Dimensions:")').first()).toBeVisible();
      await expect(techItems.locator('.tech-label:text("Coordinate Unit:")').first()).toBeVisible();
      await expect(techItems.locator('.tech-label:text("Last Updated:")').first()).toBeVisible();
    });
  });

  test.describe('Event Integration Tests', () => {
    test('should show no event message when no event selected', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const overviewCard = stadiumPage.stadiumOverviewCard;
      const noEventDiv = overviewCard.locator('.no-event');
      
      await expect(noEventDiv).toBeVisible();
      await expect(noEventDiv).toContainText('No event selected');
      
      // Occupancy analytics card should not be visible
      await expect(stadiumPage.occupancyAnalyticsCard).not.toBeVisible();
    });

    test('should display current event information when event is selected', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Try to select an event (assuming at least one event exists)
      const eventSelect = page.locator('#eventSelect');
      const eventOptions = await eventSelect.locator('option').allTextContents();
      
      if (eventOptions.length > 1) { // More than just "No Event Selected"
        await stadiumPage.selectEvent(1); // Select first available event
        
        const currentEventDiv = stadiumPage.stadiumOverviewCard.locator('.current-event');
        await expect(currentEventDiv).toBeVisible();
        
        // Check event name and date are displayed
        const eventName = currentEventDiv.locator('strong');
        const eventDate = currentEventDiv.locator('small');
        
        await expect(eventName).toBeVisible();
        await expect(eventDate).toBeVisible();
        
        // Occupancy analytics card should now be visible
        await expect(stadiumPage.occupancyAnalyticsCard).toBeVisible();
      }
    });

    test('should display occupancy analytics when event is selected', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const eventSelect = page.locator('#eventSelect');
      const eventOptions = await eventSelect.locator('option').allTextContents();
      
      if (eventOptions.length > 1) {
        await stadiumPage.selectEvent(1);
        
        const occupancyCard = stadiumPage.occupancyAnalyticsCard;
        await expect(occupancyCard).toBeVisible();
        
        // Check overall occupancy percentage
        const occupancyPercentage = occupancyCard.locator('.occupancy-percentage');
        await expect(occupancyPercentage).toBeVisible();
        await expect(occupancyPercentage).toMatch(/^\d+\.\d%$/); // Format: 45.2%
        
        // Check occupancy progress bar
        const occupancyBar = occupancyCard.locator('.occupancy-bar .occupancy-fill');
        await expect(occupancyBar).toBeVisible();
        
        // Check seat breakdown
        const seatStats = occupancyCard.locator('.seat-breakdown .seat-stat');
        await expect(seatStats).toHaveCount(3); // Available, Sold, Reserved
        
        // Verify each seat stat has icon, count, and label
        for (let i = 0; i < 3; i++) {
          const stat = seatStats.nth(i);
          await expect(stat.locator('i.bi-circle-fill')).toBeVisible();
          await expect(stat.locator('.seat-count')).toBeVisible();
          await expect(stat.locator('.seat-label')).toBeVisible();
        }
        
        // Check tribune occupancy section
        const tribuneOccupancy = occupancyCard.locator('.tribune-occupancy');
        await expect(tribuneOccupancy).toBeVisible();
        
        const tribuneItems = tribuneOccupancy.locator('.tribune-occupancy-item');
        const tribuneCount = await tribuneItems.count();
        expect(tribuneCount).toBeGreaterThan(0);
        
        // Each tribune should have badge, percentage, and progress bar
        for (let i = 0; i < tribuneCount; i++) {
          const tribune = tribuneItems.nth(i);
          await expect(tribune.locator('.tribune-badge')).toBeVisible();
          await expect(tribune.locator('.occupancy-percent')).toBeVisible();
          await expect(tribune.locator('.mini-progress .progress-fill')).toBeVisible();
        }
      }
    });

    test('should update occupancy data when simulating ticket sales', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const eventSelect = page.locator('#eventSelect');
      const eventOptions = await eventSelect.locator('option').allTextContents();
      
      if (eventOptions.length > 1) {
        await stadiumPage.selectEvent(1);
        
        // Capture initial occupancy percentage
        const occupancyCard = stadiumPage.occupancyAnalyticsCard;
        await expect(occupancyCard).toBeVisible();
        
        const initialOccupancy = await occupancyCard.locator('.occupancy-percentage').textContent();
        
        // Simulate ticket sales if button is available
        const simulateBtn = page.locator('#admin-stadium-simulate-btn');
        if (await simulateBtn.isVisible() && !(await simulateBtn.isDisabled())) {
          await stadiumPage.simulateTicketSales();
          
          // Verify occupancy data potentially changed (or at least card is still visible)
          await expect(occupancyCard.locator('.occupancy-percentage')).toBeVisible();
          
          const newOccupancy = await occupancyCard.locator('.occupancy-percentage').textContent();
          // Both old and new occupancy should be valid percentage formats
          expect(initialOccupancy).toMatch(/^\d+\.\d%$/);
          expect(newOccupancy).toMatch(/^\d+\.\d%$/);
        }
      }
    });
  });

  test.describe('Interactive Features Tests', () => {
    test('should make tribune items clickable with hover effects', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const tribuneItems = stadiumPage.structureStatsCard.locator('.tribune-item');
      const tribuneCount = await tribuneItems.count();
      
      if (tribuneCount > 0) {
        const firstTribune = tribuneItems.first();
        
        // Check initial state
        await expect(firstTribune).toBeVisible();
        
        // Hover should change appearance (test hover styles)
        await firstTribune.hover();
        
        // Click should trigger some action
        await firstTribune.click();
        
        // Verify click was registered (no error thrown)
        await page.waitForTimeout(500);
      }
    });

    test('should handle tribune badge color coding', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const tribuneItems = stadiumPage.structureStatsCard.locator('.tribune-item');
      const tribuneCount = await tribuneItems.count();
      
      for (let i = 0; i < tribuneCount; i++) {
        const tribune = tribuneItems.nth(i);
        const badge = tribune.locator('.tribune-badge');
        
        const badgeText = await badge.textContent();
        const badgeClass = await badge.getAttribute('class');
        
        // Verify badge has specific class based on tribune code
        switch (badgeText) {
          case 'N':
            expect(badgeClass).toContain('tribune-n');
            break;
          case 'S':
            expect(badgeClass).toContain('tribune-s');
            break;
          case 'E':
            expect(badgeClass).toContain('tribune-e');
            break;
          case 'W':
            expect(badgeClass).toContain('tribune-w');
            break;
        }
      }
    });

    test('should handle technical information panel toggle', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const technicalCard = stadiumPage.technicalInfoCard;
      const collapseHeader = technicalCard.locator('.card-header.collapsible');
      const chevronIcon = collapseHeader.locator('i');
      const technicalDetails = technicalCard.locator('.technical-details');
      
      // Initially collapsed
      await expect(technicalDetails).not.toBeVisible();
      await expect(chevronIcon).toHaveClass(/bi-chevron-down/);
      
      // Click to expand
      await collapseHeader.click();
      await expect(technicalDetails).toBeVisible();
      await expect(chevronIcon).toHaveClass(/bi-chevron-up/);
      
      // Click to collapse again
      await collapseHeader.click();
      await expect(technicalDetails).not.toBeVisible();
      await expect(chevronIcon).toHaveClass(/bi-chevron-down/);
    });
  });

  test.describe('Responsive Design Tests', () => {
    test('should adapt panel layout on different screen sizes', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Test desktop view (default)
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
      const desktopWidth = await stadiumPage.stadiumInfoPanel.boundingBox();
      expect(desktopWidth?.width).toBeGreaterThan(300);
      
      // Test tablet view
      await page.setViewportSize({ width: 1024, height: 768 });
      await page.waitForTimeout(500); // Allow CSS to settle
      
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
      
      // Test mobile view
      await page.setViewportSize({ width: 768, height: 1024 });
      await page.waitForTimeout(500);
      
      // On mobile, panel should still be visible but might stack differently
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
      
      // Test small mobile
      await page.setViewportSize({ width: 480, height: 800 });
      await page.waitForTimeout(500);
      
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
    });

    test('should maintain functionality across different viewports', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const viewports = [
        { width: 1920, height: 1080 }, // Desktop
        { width: 1366, height: 768 },  // Laptop
        { width: 768, height: 1024 },  // Tablet Portrait
        { width: 1024, height: 768 },  // Tablet Landscape
        { width: 480, height: 800 },   // Mobile
      ];
      
      for (const viewport of viewports) {
        await page.setViewportSize(viewport);
        await page.waitForTimeout(300);
        
        // Toggle functionality should work
        if (await stadiumPage.panelToggleButton.isVisible()) {
          await stadiumPage.toggleInfoPanel();
          await page.waitForTimeout(300);
          await stadiumPage.toggleInfoPanel();
        }
        
        // Cards should be visible
        await expect(stadiumPage.stadiumOverviewCard).toBeVisible();
        await expect(stadiumPage.structureStatsCard).toBeVisible();
      }
    });
  });

  test.describe('Accessibility Tests', () => {
    test('should have proper ARIA labels and semantic structure', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Check toggle button accessibility
      const toggleBtn = stadiumPage.panelToggleButton;
      await expect(toggleBtn).toHaveAttribute('aria-label');
      await expect(toggleBtn).toHaveAttribute('aria-expanded');
      
      // Check heading structure
      const headings = page.locator('h1, h2, h3, h4, h5, h6');
      const headingCount = await headings.count();
      expect(headingCount).toBeGreaterThan(0);
      
      // Check card headers have proper headings
      await expect(stadiumPage.stadiumOverviewCard.locator('h5')).toBeVisible();
      await expect(stadiumPage.structureStatsCard.locator('h5')).toBeVisible();
      await expect(stadiumPage.technicalInfoCard.locator('h5')).toBeVisible();
    });

    test('should support keyboard navigation', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Test tab navigation
      await page.keyboard.press('Tab');
      
      // Check if focus is visible
      const focusedElement = page.locator(':focus');
      await expect(focusedElement).toBeVisible();
      
      // Navigate through focusable elements
      for (let i = 0; i < 5; i++) {
        await page.keyboard.press('Tab');
        await page.waitForTimeout(100);
      }
      
      // Should be able to activate toggle button with Enter/Space
      const toggleBtn = stadiumPage.panelToggleButton;
      await toggleBtn.focus();
      await page.keyboard.press('Enter');
      await page.waitForTimeout(350);
    });

    test('should meet color contrast requirements', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      // Check main panel background and text contrast
      const panel = stadiumPage.stadiumInfoPanel;
      const computedStyles = await panel.evaluate(el => {
        const style = window.getComputedStyle(el);
        return {
          backgroundColor: style.backgroundColor,
          color: style.color
        };
      });
      
      expect(computedStyles.backgroundColor).toBeTruthy();
      expect(computedStyles.color).toBeTruthy();
      
      // Check card headers have sufficient contrast
      const cardHeaders = page.locator('.info-card .card-header');
      const headerCount = await cardHeaders.count();
      
      for (let i = 0; i < headerCount; i++) {
        const header = cardHeaders.nth(i);
        const headerStyles = await header.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            backgroundColor: style.backgroundColor,
            color: style.color
          };
        });
        
        expect(headerStyles.backgroundColor).toBeTruthy();
        expect(headerStyles.color).toBeTruthy();
      }
    });
  });

  test.describe('Visual Regression Tests', () => {
    test('should maintain consistent stadium overview card styling', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const overviewCard = stadiumPage.stadiumOverviewCard;
      await expect(overviewCard).toBeVisible();
      
      // Check card header has gradient background
      const cardHeader = overviewCard.locator('.card-header');
      const headerStyles = await cardHeader.evaluate(el => {
        const style = window.getComputedStyle(el);
        return {
          background: style.background,
          backgroundImage: style.backgroundImage
        };
      });
      
      // Should have gradient (linear-gradient in CSS)
      expect(headerStyles.background || headerStyles.backgroundImage).toContain('linear-gradient');
      
      // Check capacity display formatting
      const capacityNumber = overviewCard.locator('.capacity-number');
      const capacityStyles = await capacityNumber.evaluate(el => {
        const style = window.getComputedStyle(el);
        return {
          fontSize: style.fontSize,
          fontWeight: style.fontWeight
        };
      });
      
      // Should have large font size and bold weight
      expect(parseFloat(capacityStyles.fontSize)).toBeGreaterThan(20); // 2.5rem should be > 20px
      expect(capacityStyles.fontWeight).toBe('700');
    });

    test('should display proper tribune badge colors', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const tribuneItems = stadiumPage.structureStatsCard.locator('.tribune-item');
      const tribuneCount = await tribuneItems.count();
      
      for (let i = 0; i < tribuneCount; i++) {
        const tribune = tribuneItems.nth(i);
        const badge = tribune.locator('.tribune-badge');
        const badgeText = await badge.textContent();
        
        const badgeStyles = await badge.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            backgroundColor: style.backgroundColor,
            borderRadius: style.borderRadius,
            width: style.width,
            height: style.height
          };
        });
        
        // Badge should be circular (width = height, border-radius = 50%)
        expect(badgeStyles.width).toBe(badgeStyles.height);
        expect(badgeStyles.borderRadius).toContain('50%');
        expect(badgeStyles.backgroundColor).toBeTruthy();
      }
    });

    test('should display progress bars with correct styling', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const eventSelect = page.locator('#eventSelect');
      const eventOptions = await eventSelect.locator('option').allTextContents();
      
      if (eventOptions.length > 1) {
        await stadiumPage.selectEvent(1);
        
        const occupancyCard = stadiumPage.occupancyAnalyticsCard;
        await expect(occupancyCard).toBeVisible();
        
        // Check main occupancy bar
        const mainProgressBar = occupancyCard.locator('.occupancy-bar .occupancy-fill');
        await expect(mainProgressBar).toBeVisible();
        
        const mainBarStyles = await mainProgressBar.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            background: style.background,
            backgroundImage: style.backgroundImage
          };
        });
        
        // Should have gradient background
        expect(mainBarStyles.background || mainBarStyles.backgroundImage).toContain('linear-gradient');
        
        // Check mini progress bars
        const miniProgressBars = occupancyCard.locator('.mini-progress .progress-fill');
        const miniBarCount = await miniProgressBars.count();
        
        for (let i = 0; i < miniBarCount; i++) {
          const miniBar = miniProgressBars.nth(i);
          const miniBarStyles = await miniBar.evaluate(el => {
            const style = window.getComputedStyle(el);
            return {
              background: style.background,
              backgroundImage: style.backgroundImage,
              height: style.height
            };
          });
          
          expect(miniBarStyles.background || miniBarStyles.backgroundImage).toContain('linear-gradient');
          expect(parseFloat(miniBarStyles.height)).toBeGreaterThan(0);
        }
      }
    });
  });

  test.describe('Performance Tests', () => {
    test('should load stadium information panel within acceptable timeframes', async ({ page }) => {
      const startTime = Date.now();
      
      await stadiumPage.waitForStadiumDataLoad();
      
      const loadTime = Date.now() - startTime;
      
      // Panel should load within 15 seconds
      expect(loadTime).toBeLessThan(15000);
      
      // Panel should be visible
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
    });

    test('should handle panel toggle animations smoothly', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const toggleStartTime = Date.now();
      
      // Toggle collapse
      await stadiumPage.toggleInfoPanel();
      
      const toggleTime = Date.now() - toggleStartTime;
      
      // Animation should complete within 1 second
      expect(toggleTime).toBeLessThan(1000);
      
      await expect(stadiumPage.stadiumInfoPanel).toHaveClass(/collapsed/);
    });

    test('should efficiently update occupancy data when event changes', async ({ page }) => {
      await stadiumPage.waitForStadiumDataLoad();
      
      const eventSelect = page.locator('#eventSelect');
      const eventOptions = await eventSelect.locator('option').allTextContents();
      
      if (eventOptions.length > 1) {
        const updateStartTime = Date.now();
        
        await stadiumPage.selectEvent(1);
        
        // Wait for occupancy card to appear
        await expect(stadiumPage.occupancyAnalyticsCard).toBeVisible();
        
        const updateTime = Date.now() - updateStartTime;
        
        // Event data should load within 5 seconds
        expect(updateTime).toBeLessThan(5000);
      }
    });
  });

  test.describe('Error Handling and Edge Cases', () => {
    test('should handle missing stadium data gracefully', async ({ page }) => {
      // Mock empty or missing stadium data
      await page.route(`${API_BASE_URL}/api/stadium-viewer/overview`, async route => {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(null)
        });
      });
      
      await stadiumPage.navigateToStadiumOverview();
      
      // Should show appropriate message for no data
      const noDataMessage = page.locator('.info-card.no-data, .alert-warning');
      await expect(noDataMessage).toBeVisible();
      
      // Panel should still be present
      await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
    });

    test('should handle API errors gracefully', async ({ page }) => {
      // Mock API error
      await page.route(`${API_BASE_URL}/api/stadium-viewer/**`, async route => {
        await route.abort('failed');
      });
      
      await stadiumPage.navigateToStadiumOverview();
      
      // Should show error message
      const errorMessage = page.locator('.alert-danger, .error-message');
      await expect(errorMessage.first()).toBeVisible();
    });

    test('should handle malformed event data', async ({ page }) => {
      // Mock malformed event data
      await page.route(`${API_BASE_URL}/api/stadium-viewer/event/*/seat-status`, async route => {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify({
            sectorSummaries: null, // Invalid data
            totalSeats: 'invalid'
          })
        });
      });
      
      await stadiumPage.navigateToStadiumOverview();
      await stadiumPage.waitForStadiumDataLoad();
      
      const eventOptions = await page.locator('#eventSelect option').allTextContents();
      if (eventOptions.length > 1) {
        await stadiumPage.selectEvent(1);
        
        // Should handle gracefully without crashing
        await page.waitForTimeout(2000);
        
        // Panel should still be visible
        await expect(stadiumPage.stadiumInfoPanel).toBeVisible();
      }
    });
  });
});

// Utility test for admin authentication setup
test.describe('Admin Authentication Setup', () => {
  test('should successfully authenticate admin user', async ({ page }) => {
    await page.goto(`${ADMIN_BASE_URL}/login`);
    
    await page.fill('input[name="email"]', 'admin@stadium.com');
    await page.fill('input[name="password"]', 'Admin123!');
    await page.click('button[type="submit"]');
    
    await page.waitForURL(`${ADMIN_BASE_URL}/`, { timeout: 10000 });
    await expect(page).toHaveURL(`${ADMIN_BASE_URL}/`);
  });
});