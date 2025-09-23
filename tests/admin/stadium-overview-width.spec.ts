import { test, expect, Page } from '@playwright/test';

/**
 * Stadium Overview Width Tests - Full Admin Layout Main Area Usage
 * Tests the comprehensive CSS changes to ensure stadium overview containers
 * use the full width of admin-layout-main area by overriding Bootstrap padding
 */

// Test data and configuration
const ADMIN_URLS = {
  home: 'https://localhost:7030',
  stadium: 'https://localhost:7030/stadium-overview'
};

const VIEWPORT_SIZES = {
  desktop: { width: 1920, height: 1080 },
  laptop: { width: 1366, height: 768 },
  tablet: { width: 768, height: 1024 },
  mobile: { width: 375, height: 667 }
};

const CONTAINER_SELECTORS = {
  adminLayoutMain: '.admin-layout-main',
  stadiumOverviewContainer: '.stadium-overview-container',
  stadiumLayoutContainer: '.stadium-layout-container',
  stadiumViewerContainer: '.stadium-viewer-container',
  bootstrapContainerFluid: '.container-fluid'
};

const COMPONENT_SELECTORS = {
  operationsCenter: '[data-testid="stadium-operations-center"], .viewer-header h3',
  intelligenceHub: '[data-testid="stadium-intelligence-hub"], .stadium-main-view',
  controlPanel: '[data-testid="stadium-control-panel"], .viewer-controls',
  stadiumViewer: '.stadium-viewer-container',
  legendPanel: '.legend-panel'
};

/**
 * Helper function to wait for stadium overview page to fully load
 */
async function waitForStadiumOverviewLoad(page: Page): Promise<void> {
  await page.waitForLoadState('networkidle');

  // Wait for key stadium components to be visible
  await page.waitForSelector('.stadium-viewer-container', { state: 'visible', timeout: 10000 });
  await page.waitForSelector('.viewer-header', { state: 'visible', timeout: 5000 });

  // Wait for any loading spinners to disappear
  await page.waitForFunction(() => {
    const spinners = document.querySelectorAll('.loading-spinner, .spinner-sm');
    return spinners.length === 0 || Array.from(spinners).every(s =>
      window.getComputedStyle(s as Element).display === 'none'
    );
  }, { timeout: 10000 });

  // Additional wait for CSS transitions and animations
  await page.waitForTimeout(1000);
}

/**
 * Helper function to get computed styles for width measurements
 */
async function getElementWidthInfo(page: Page, selector: string) {
  return await page.evaluate((sel) => {
    const element = document.querySelector(sel);
    if (!element) return null;

    const computedStyle = window.getComputedStyle(element);
    const rect = element.getBoundingClientRect();

    return {
      width: rect.width,
      offsetWidth: (element as HTMLElement).offsetWidth,
      clientWidth: (element as HTMLElement).clientWidth,
      computedWidth: parseFloat(computedStyle.width),
      marginLeft: parseFloat(computedStyle.marginLeft),
      marginRight: parseFloat(computedStyle.marginRight),
      paddingLeft: parseFloat(computedStyle.paddingLeft),
      paddingRight: parseFloat(computedStyle.paddingRight),
      maxWidth: computedStyle.maxWidth,
      position: computedStyle.position
    };
  }, selector);
}

/**
 * Helper function to verify CSS override properties
 */
async function verifyCSSOverrides(page: Page, selector: string) {
  return await page.evaluate((sel) => {
    const element = document.querySelector(sel);
    if (!element) return null;

    const computedStyle = window.getComputedStyle(element);

    return {
      paddingLeft: computedStyle.paddingLeft,
      paddingRight: computedStyle.paddingRight,
      marginLeft: computedStyle.marginLeft,
      marginRight: computedStyle.marginRight,
      width: computedStyle.width,
      maxWidth: computedStyle.maxWidth
    };
  }, selector);
}

test.describe('Stadium Overview Width Tests', () => {

  test.beforeEach(async ({ page, context }) => {
    // Configure context for self-signed certificates
    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    // Navigate to admin app and handle auth if needed
    await page.goto(ADMIN_URLS.home, { waitUntil: 'networkidle', timeout: 30000 });

    // Wait for page to be ready
    await page.waitForLoadState('domcontentloaded');

    // Check if we need to handle authentication
    try {
      await page.waitForSelector('input[type="email"], input[type="username"], .login-form', { timeout: 3000 });

      // If login form is present, fill it
      const emailInput = await page.locator('input[type="email"], input[type="username"]').first();
      if (await emailInput.isVisible()) {
        await emailInput.fill('admin@stadium.com');

        const passwordInput = await page.locator('input[type="password"]').first();
        await passwordInput.fill('AdminPassword123!');

        const loginButton = await page.locator('button[type="submit"], .btn-login').first();
        await loginButton.click();

        await page.waitForLoadState('networkidle');
      }
    } catch (e) {
      // If no login form, continue - may already be authenticated
      console.log('No login form found or already authenticated');
    }

    // Navigate to stadium overview page
    await page.goto(ADMIN_URLS.stadium, { waitUntil: 'networkidle', timeout: 30000 });
    await waitForStadiumOverviewLoad(page);
  });

  test.describe('Full Width Usage Verification', () => {

    test('stadium overview container uses full admin-layout-main width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      // Get width measurements
      const adminLayoutMainInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.adminLayoutMain);
      const stadiumOverviewInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumOverviewContainer);

      expect(adminLayoutMainInfo).not.toBeNull();
      expect(stadiumOverviewInfo).not.toBeNull();

      if (adminLayoutMainInfo && stadiumOverviewInfo) {
        // Stadium overview should use nearly the full width of admin-layout-main
        // Allow for a small tolerance (within 10px) for borders/minor styling
        const widthDifference = Math.abs(adminLayoutMainInfo.width - stadiumOverviewInfo.width);
        expect(widthDifference).toBeLessThanOrEqual(10);

        // Verify no horizontal margins
        expect(stadiumOverviewInfo.marginLeft).toBe(0);
        expect(stadiumOverviewInfo.marginRight).toBe(0);

        // Verify no horizontal padding or overridden to 0
        expect(stadiumOverviewInfo.paddingLeft).toBe(0);
        expect(stadiumOverviewInfo.paddingRight).toBe(0);
      }
    });

    test('stadium layout container uses full available width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const parentInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumOverviewContainer);
      const layoutInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumLayoutContainer);

      expect(parentInfo).not.toBeNull();
      expect(layoutInfo).not.toBeNull();

      if (parentInfo && layoutInfo) {
        // Layout container should use full width of its parent
        const widthDifference = Math.abs(parentInfo.width - layoutInfo.width);
        expect(widthDifference).toBeLessThanOrEqual(5);

        // Verify CSS overrides
        expect(layoutInfo.marginLeft).toBe(0);
        expect(layoutInfo.marginRight).toBe(0);
        expect(layoutInfo.paddingLeft).toBe(0);
        expect(layoutInfo.paddingRight).toBe(0);
      }
    });

    test('stadium viewer container uses full available width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const parentInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumLayoutContainer);
      const viewerInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumViewerContainer);

      expect(parentInfo).not.toBeNull();
      expect(viewerInfo).not.toBeNull();

      if (parentInfo && viewerInfo) {
        // Viewer container should use full width allowing for gaps/padding
        const widthDifference = Math.abs(parentInfo.clientWidth - viewerInfo.offsetWidth);
        expect(widthDifference).toBeLessThanOrEqual(50); // Allow for container gaps

        // Verify no additional margins
        expect(viewerInfo.marginLeft).toBe(0);
        expect(viewerInfo.marginRight).toBe(0);
      }
    });
  });

  test.describe('Bootstrap Override Verification', () => {

    test('container-fluid padding overrides are applied', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      // Check if stadium overview container has container-fluid class
      const hasContainerFluid = await page.locator('.stadium-overview-container.container-fluid').count() > 0;

      if (hasContainerFluid) {
        const overrides = await verifyCSSOverrides(page, '.stadium-overview-container.container-fluid');
        expect(overrides).not.toBeNull();

        if (overrides) {
          // Verify Bootstrap padding is overridden to 0
          expect(overrides.paddingLeft).toBe('0px');
          expect(overrides.paddingRight).toBe('0px');

          // Verify max-width is none
          expect(overrides.maxWidth).toBe('none');
        }
      }
    });

    test('CSS important declarations are properly applied', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      // Test key CSS properties are overridden
      const cssOverrides = await page.evaluate(() => {
        const element = document.querySelector('.stadium-overview-container');
        if (!element) return null;

        const computedStyle = window.getComputedStyle(element);

        return {
          width: computedStyle.width,
          maxWidth: computedStyle.maxWidth,
          paddingLeft: computedStyle.paddingLeft,
          paddingRight: computedStyle.paddingRight,
          marginLeft: computedStyle.marginLeft,
          marginRight: computedStyle.marginRight
        };
      });

      expect(cssOverrides).not.toBeNull();

      if (cssOverrides) {
        // Verify important overrides
        expect(cssOverrides.paddingLeft).toBe('0px');
        expect(cssOverrides.paddingRight).toBe('0px');
        expect(cssOverrides.marginLeft).toBe('0px');
        expect(cssOverrides.marginRight).toBe('0px');
        expect(cssOverrides.maxWidth).toBe('none');
      }
    });
  });

  test.describe('Component Width Coverage', () => {

    test('Stadium Operations Center header spans full width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const containerInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumViewerContainer);

      // Try multiple selectors for the operations center header
      let headerInfo = null;
      const headerSelectors = ['.viewer-header', '.stadium-operations-center', 'h3'];

      for (const selector of headerSelectors) {
        headerInfo = await getElementWidthInfo(page, selector);
        if (headerInfo) break;
      }

      expect(containerInfo).not.toBeNull();
      expect(headerInfo).not.toBeNull();

      if (containerInfo && headerInfo) {
        // Header should span significant portion of container width
        const widthRatio = headerInfo.width / containerInfo.clientWidth;
        expect(widthRatio).toBeGreaterThan(0.8); // At least 80% width usage
      }
    });

    test('Stadium Intelligence Hub section spans full width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const containerInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumViewerContainer);

      // Try multiple selectors for the intelligence hub
      let hubInfo = null;
      const hubSelectors = ['.stadium-main-view', '.stadium-intelligence-hub', '.svg-container'];

      for (const selector of hubSelectors) {
        hubInfo = await getElementWidthInfo(page, selector);
        if (hubInfo) break;
      }

      expect(containerInfo).not.toBeNull();
      expect(hubInfo).not.toBeNull();

      if (containerInfo && hubInfo) {
        // Intelligence hub should use significant width
        const widthRatio = hubInfo.width / containerInfo.clientWidth;
        expect(widthRatio).toBeGreaterThan(0.75); // At least 75% width usage
      }
    });

    test('Stadium Control Panel uses full width', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const containerInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumViewerContainer);

      // Try multiple selectors for the control panel
      let controlInfo = null;
      const controlSelectors = ['.viewer-controls', '.stadium-control-panel', '.viewer-header .viewer-controls'];

      for (const selector of controlSelectors) {
        controlInfo = await getElementWidthInfo(page, selector);
        if (controlInfo) break;
      }

      expect(containerInfo).not.toBeNull();

      if (containerInfo && controlInfo) {
        // Control panel should use significant width
        const widthRatio = controlInfo.width / containerInfo.clientWidth;
        expect(widthRatio).toBeGreaterThan(0.7); // At least 70% width usage
      }
    });
  });

  test.describe('Responsive Width Testing', () => {

    test('full width usage on desktop (1920x1080)', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);
      await page.waitForTimeout(500); // Allow for responsive adjustments

      const measurements = await getMeasurements(page);
      verifyFullWidthUsage(measurements, 'desktop');
    });

    test('full width usage on laptop (1366x768)', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.laptop);
      await page.waitForTimeout(500);

      const measurements = await getMeasurements(page);
      verifyFullWidthUsage(measurements, 'laptop');
    });

    test('full width usage on tablet (768x1024)', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.tablet);
      await page.waitForTimeout(500);

      const measurements = await getMeasurements(page);
      verifyFullWidthUsage(measurements, 'tablet');
    });

    test('responsive layout maintains width principles on mobile', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.mobile);
      await page.waitForTimeout(500);

      // On mobile, layout may change but width principles should still apply
      const containerInfo = await getElementWidthInfo(page, CONTAINER_SELECTORS.stadiumOverviewContainer);
      expect(containerInfo).not.toBeNull();

      if (containerInfo) {
        // Should still use full available width
        expect(containerInfo.paddingLeft).toBe(0);
        expect(containerInfo.paddingRight).toBe(0);
        expect(containerInfo.marginLeft).toBe(0);
        expect(containerInfo.marginRight).toBe(0);
      }
    });
  });

  test.describe('Width Measurement Precision', () => {

    test('container widths vs admin-layout-main width comparison', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const adminMainWidth = await page.evaluate(() => {
        const element = document.querySelector('.admin-layout-main');
        return element ? element.getBoundingClientRect().width : null;
      });

      const containerWidths = await page.evaluate(() => {
        const containers = [
          '.stadium-overview-container',
          '.stadium-layout-container',
          '.stadium-viewer-container'
        ];

        return containers.map(selector => {
          const element = document.querySelector(selector);
          return {
            selector,
            width: element ? element.getBoundingClientRect().width : null,
            offsetWidth: element ? (element as HTMLElement).offsetWidth : null
          };
        });
      });

      expect(adminMainWidth).not.toBeNull();

      containerWidths.forEach(({ selector, width, offsetWidth }) => {
        if (width && adminMainWidth) {
          const usageRatio = width / adminMainWidth;
          console.log(`${selector}: ${width}px (${(usageRatio * 100).toFixed(1)}% of admin-main)`);

          // Each container should use at least 90% of available width
          expect(usageRatio).toBeGreaterThan(0.9);
        }
      });
    });

    test('actual vs expected width measurements', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);

      const measurements = await page.evaluate(() => {
        const viewport = { width: window.innerWidth, height: window.innerHeight };
        const adminMain = document.querySelector('.admin-layout-main');
        const stadiumContainer = document.querySelector('.stadium-overview-container');

        return {
          viewport,
          adminMain: adminMain ? {
            width: adminMain.getBoundingClientRect().width,
            clientWidth: (adminMain as HTMLElement).clientWidth,
            offsetWidth: (adminMain as HTMLElement).offsetWidth
          } : null,
          stadiumContainer: stadiumContainer ? {
            width: stadiumContainer.getBoundingClientRect().width,
            clientWidth: (stadiumContainer as HTMLElement).clientWidth,
            offsetWidth: (stadiumContainer as HTMLElement).offsetWidth
          } : null
        };
      });

      console.log('Width Measurements:', JSON.stringify(measurements, null, 2));

      expect(measurements.adminMain).not.toBeNull();
      expect(measurements.stadiumContainer).not.toBeNull();

      if (measurements.adminMain && measurements.stadiumContainer) {
        // Stadium container should match or be very close to admin main width
        const widthDifference = Math.abs(measurements.adminMain.width - measurements.stadiumContainer.width);
        expect(widthDifference).toBeLessThanOrEqual(20); // Allow 20px tolerance
      }
    });
  });
});

/**
 * Helper function to get all relevant measurements
 */
async function getMeasurements(page: Page) {
  return await page.evaluate(() => {
    const selectors = [
      '.admin-layout-main',
      '.stadium-overview-container',
      '.stadium-layout-container',
      '.stadium-viewer-container'
    ];

    return selectors.map(selector => {
      const element = document.querySelector(selector);
      if (!element) return { selector, found: false };

      const rect = element.getBoundingClientRect();
      const computed = window.getComputedStyle(element);

      return {
        selector,
        found: true,
        width: rect.width,
        offsetWidth: (element as HTMLElement).offsetWidth,
        clientWidth: (element as HTMLElement).clientWidth,
        paddingLeft: parseFloat(computed.paddingLeft),
        paddingRight: parseFloat(computed.paddingRight),
        marginLeft: parseFloat(computed.marginLeft),
        marginRight: parseFloat(computed.marginRight)
      };
    });
  });
}

/**
 * Helper function to verify full width usage
 */
function verifyFullWidthUsage(measurements: any[], context: string) {
  const adminMain = measurements.find(m => m.selector === '.admin-layout-main');
  const stadiumOverview = measurements.find(m => m.selector === '.stadium-overview-container');

  expect(adminMain?.found).toBe(true);
  expect(stadiumOverview?.found).toBe(true);

  if (adminMain && stadiumOverview) {
    const widthRatio = stadiumOverview.width / adminMain.width;
    console.log(`${context} width ratio: ${(widthRatio * 100).toFixed(1)}%`);

    // Should use at least 95% of available width
    expect(widthRatio).toBeGreaterThan(0.95);

    // Verify no unwanted padding/margins
    expect(stadiumOverview.paddingLeft).toBe(0);
    expect(stadiumOverview.paddingRight).toBe(0);
    expect(stadiumOverview.marginLeft).toBe(0);
    expect(stadiumOverview.marginRight).toBe(0);
  }
}