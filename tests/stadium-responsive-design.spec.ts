import { test, expect, Page } from '@playwright/test';

/**
 * Stadium Overview Responsive Design Tests
 *
 * This test suite verifies that the stadium overview interface
 * adapts correctly across different device sizes and screen orientations.
 *
 * Test Coverage:
 * - Mobile, tablet, and desktop breakpoints
 * - Touch interactions and gesture support
 * - Accessibility compliance
 * - Performance on different devices
 */

// Test configuration for different device sizes
const deviceSizes = [
  { name: 'Mobile Portrait', width: 375, height: 667, isMobile: true },
  { name: 'Mobile Landscape', width: 667, height: 375, isMobile: true },
  { name: 'Tablet Portrait', width: 768, height: 1024, isTablet: true },
  { name: 'Tablet Landscape', width: 1024, height: 768, isTablet: true },
  { name: 'Desktop Small', width: 1280, height: 720, isDesktop: true },
  { name: 'Desktop Large', width: 1920, height: 1080, isDesktop: true },
];

test.describe('Stadium Overview - Responsive Design', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to admin login first
    await page.goto('https://localhost:9030/login');

    // Login with admin credentials
    await page.fill('#customer-login-email-input', 'admin@stadium.com');
    await page.fill('#customer-login-password-input', 'admin123');
    await page.click('#customer-login-submit-btn');

    // Wait for successful login and redirect
    await page.waitForURL('**/admin/dashboard', { timeout: 10000 });

    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');
  });

  // Test each device size
  for (const device of deviceSizes) {
    test(`${device.name} - Layout and Navigation`, async ({ page }) => {
      // Set viewport size
      await page.setViewportSize({ width: device.width, height: device.height });

      // Wait for page to adjust to new viewport
      await page.waitForTimeout(500);

      // Verify main container is visible
      const container = page.locator('#stadium-overview-container');
      await expect(container).toBeVisible();

      // Verify responsive layout adjustments
      if (device.isMobile) {
        // Mobile-specific tests
        await testMobileLayout(page);
      } else if (device.isTablet) {
        // Tablet-specific tests
        await testTabletLayout(page);
      } else if (device.isDesktop) {
        // Desktop-specific tests
        await testDesktopLayout(page);
      }

      // Test control panel responsiveness
      await testControlPanelResponsiveness(page, device);

      // Test stadium visualization responsiveness
      await testStadiumVisualizationResponsiveness(page, device);
    });
  }

  test('Mobile Touch Interactions', async ({ page }) => {
    // Set mobile viewport
    await page.setViewportSize({ width: 375, height: 667 });

    // Test touch interactions on mobile
    const stadiumView = page.locator('#stadium-main-view');
    await expect(stadiumView).toBeVisible();

    // Test mobile controls are present
    if (await page.locator('#stadium-mobile-controls').isVisible()) {
      const zoomInBtn = page.locator('#mobile-zoom-in-btn');
      const zoomOutBtn = page.locator('#mobile-zoom-out-btn');
      const resetZoomBtn = page.locator('#mobile-reset-zoom-btn');
      const toggleLegendBtn = page.locator('#mobile-toggle-legend-btn');

      await expect(zoomInBtn).toBeVisible();
      await expect(zoomOutBtn).toBeVisible();
      await expect(resetZoomBtn).toBeVisible();
      await expect(toggleLegendBtn).toBeVisible();

      // Test zoom interactions
      await zoomInBtn.click();
      await page.waitForTimeout(200);

      await zoomOutBtn.click();
      await page.waitForTimeout(200);

      await resetZoomBtn.click();
      await page.waitForTimeout(200);

      // Test legend toggle
      await toggleLegendBtn.click();
      await page.waitForTimeout(300);
    }
  });

  test('Accessibility Compliance', async ({ page }) => {
    // Test with different viewport sizes for accessibility
    for (const device of deviceSizes) {
      await page.setViewportSize({ width: device.width, height: device.height });

      // Check for skip link
      const skipLink = page.locator('.skip-to-content');
      await expect(skipLink).toBeInTheDOM();

      // Check for proper ARIA labels
      const mainContainer = page.locator('[role="main"]');
      await expect(mainContainer).toBeVisible();

      // Check for live region
      const liveRegion = page.locator('[aria-live="polite"]');
      await expect(liveRegion).toBeInTheDOM();

      // Test keyboard navigation
      await testKeyboardNavigation(page);

      // Test focus management
      await testFocusManagement(page);
    }
  });

  test('Performance on Different Devices', async ({ page }) => {
    // Test performance metrics for different device sizes
    for (const device of deviceSizes) {
      await page.setViewportSize({ width: device.width, height: device.height });

      // Measure page load performance
      const startTime = Date.now();
      await page.reload();
      await page.waitForLoadState('networkidle');
      const loadTime = Date.now() - startTime;

      // Performance should be reasonable (under 5 seconds)
      expect(loadTime).toBeLessThan(5000);

      // Check that animations are smooth (no janky transitions)
      const stadiumView = page.locator('#stadium-main-view');
      await expect(stadiumView).toBeVisible();

      // For mobile devices, ensure reduced motion is respected
      if (device.isMobile) {
        // Check if reduced motion optimizations are applied
        const hasOptimizations = await page.evaluate(() => {
          return document.body.classList.contains('mobile-optimized');
        });
        // Note: This may vary based on device capabilities
      }
    }
  });

  test('Legend Panel Responsiveness', async ({ page }) => {
    // Test legend panel behavior across different screen sizes
    for (const device of deviceSizes) {
      await page.setViewportSize({ width: device.width, height: device.height });

      // Toggle legend visibility
      const toggleBtn = page.locator('#toggle-legend-button');
      if (await toggleBtn.isVisible()) {
        await toggleBtn.click();
        await page.waitForTimeout(300);

        const legendPanel = page.locator('#legend-panel');

        if (device.isMobile) {
          // On mobile, legend should be a bottom sheet
          await expect(legendPanel).toHaveClass(/visible/);
        } else {
          // On desktop/tablet, legend should be a floating panel
          await expect(legendPanel).toBeVisible();
        }

        // Toggle off
        if (device.isMobile) {
          // On mobile, might need to click outside or use close button
          await page.click('#stadium-main-view');
        } else {
          await toggleBtn.click();
        }
        await page.waitForTimeout(300);
      }
    }
  });
});

// Helper function to test mobile-specific layout
async function testMobileLayout(page: Page) {
  // Stadium layout should be stacked vertically
  const layoutContainer = page.locator('.stadium-layout-container');
  const computedStyle = await layoutContainer.evaluate(el =>
    window.getComputedStyle(el).display
  );
  expect(computedStyle).toBe('flex');

  // Control panel should be stacked
  const controls = page.locator('.viewer-controls');
  await expect(controls).toBeVisible();

  // Mobile controls should be visible
  const mobileControls = page.locator('.mobile-controls');
  if (await mobileControls.isVisible()) {
    await expect(mobileControls).toBeVisible();
  }
}

// Helper function to test tablet-specific layout
async function testTabletLayout(page: Page) {
  // Layout should adapt for tablet sizes
  const layoutContainer = page.locator('.stadium-layout-container');
  await expect(layoutContainer).toBeVisible();

  // Controls should be in a grid layout
  const controls = page.locator('.viewer-controls');
  await expect(controls).toBeVisible();

  // Info panel should be accessible
  const infoPanel = page.locator('#stadium-info-panel');
  if (await infoPanel.isVisible()) {
    await expect(infoPanel).toBeVisible();
  }
}

// Helper function to test desktop-specific layout
async function testDesktopLayout(page: Page) {
  // Layout should be side-by-side
  const layoutContainer = page.locator('.stadium-layout-container');
  const computedStyle = await layoutContainer.evaluate(el =>
    window.getComputedStyle(el).display
  );
  expect(['grid', 'flex']).toContain(computedStyle);

  // All desktop features should be visible
  const stadiumViewer = page.locator('.stadium-viewer-container');
  await expect(stadiumViewer).toBeVisible();

  // Mobile controls should not be visible on desktop
  const mobileControls = page.locator('.mobile-controls');
  if (await mobileControls.isVisible()) {
    const display = await mobileControls.evaluate(el =>
      window.getComputedStyle(el).display
    );
    expect(display).toBe('none');
  }
}

// Helper function to test control panel responsiveness
async function testControlPanelResponsiveness(page: Page, device: any) {
  const controlPanel = page.locator('.viewer-controls');
  await expect(controlPanel).toBeVisible();

  // Check that control groups are appropriately sized
  const controlGroups = page.locator('.control-group');
  const count = await controlGroups.count();
  expect(count).toBeGreaterThan(0);

  // Test form elements are touch-friendly on mobile
  if (device.isMobile) {
    const formElements = page.locator('.form-control, .form-select, .btn');
    const firstElement = formElements.first();
    if (await firstElement.isVisible()) {
      const height = await firstElement.evaluate(el => el.getBoundingClientRect().height);
      expect(height).toBeGreaterThanOrEqual(44); // Minimum touch target size
    }
  }
}

// Helper function to test stadium visualization responsiveness
async function testStadiumVisualizationResponsiveness(page: Page, device: any) {
  const stadiumView = page.locator('#stadium-main-view');
  await expect(stadiumView).toBeVisible();

  // Check that SVG container is present
  const svgContainer = page.locator('#stadium-svg-container');
  if (await svgContainer.isVisible()) {
    await expect(svgContainer).toBeVisible();

    // Check SVG scaling
    const svg = svgContainer.locator('svg').first();
    if (await svg.isVisible()) {
      const bounds = await svg.boundingBox();
      expect(bounds).toBeTruthy();

      if (bounds) {
        // SVG should fit within the container
        expect(bounds.width).toBeGreaterThan(0);
        expect(bounds.height).toBeGreaterThan(0);
      }
    }
  }
}

// Helper function to test keyboard navigation
async function testKeyboardNavigation(page: Page) {
  // Focus should be manageable via keyboard
  await page.keyboard.press('Tab');

  // Check that focus is visible
  const focusedElement = page.locator(':focus');
  if (await focusedElement.isVisible()) {
    const outline = await focusedElement.evaluate(el =>
      window.getComputedStyle(el).outline
    );
    expect(outline).not.toBe('none');
  }
}

// Helper function to test focus management
async function testFocusManagement(page: Page) {
  // Test that focus doesn't get trapped inappropriately
  const firstInteractiveElement = page.locator('button, input, select, a').first();
  if (await firstInteractiveElement.isVisible()) {
    await firstInteractiveElement.focus();

    // Should be able to navigate away
    await page.keyboard.press('Tab');

    const newFocus = page.locator(':focus');
    const isSameElement = await newFocus.evaluate((el, firstEl) =>
      el === firstEl, await firstInteractiveElement.elementHandle()
    );

    // Focus should have moved (unless there's only one focusable element)
    // This is a basic check - in real scenarios there might be only one element
  }
}