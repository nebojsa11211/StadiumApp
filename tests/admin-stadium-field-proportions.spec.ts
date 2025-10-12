import { test, expect, Page } from '@playwright/test';

/**
 * Stadium Field Proportions Tests
 * Verifies the fix for stadium field dimensions to maintain proper 2:1 ratio (100m x 50m)
 *
 * Context:
 * - Modified HNKRijekaStadiumConstants.cs: FIELD = (350, 285, 500, 250)
 * - Added preserveAspectRatio="xMidYMid meet" to StadiumSvgRenderer.razor
 *
 * Expected Results:
 * - SVG element has preserveAspectRatio="xMidYMid meet"
 * - Field rect has width="500" and height="250"
 * - Aspect ratio is exactly 2.0 (width/height = 500/250)
 * - Visual appearance shows properly proportioned football field across all viewports
 */

// Test configuration
const ADMIN_URLS = {
  home: 'https://localhost:7030',
  login: 'https://localhost:7030/login',
  stadiumOverview: 'https://localhost:7030/stadium-overview'
};

const ADMIN_CREDENTIALS = {
  email: 'admin@stadium.com',
  password: 'admin123'
};

const VIEWPORT_SIZES = {
  desktop: { width: 1920, height: 1080 },
  laptop: { width: 1366, height: 768 },
  tablet: { width: 768, height: 1024 },
  mobile: { width: 375, height: 667 }
};

const EXPECTED_FIELD_DIMENSIONS = {
  x: 350,
  y: 285,
  width: 500,
  height: 250,
  aspectRatio: 2.0 // width/height = 500/250 = 2.0
};

const SVG_SELECTORS = {
  mainSvg: '#stadium-svg-main',
  fieldRect: '.stadium-field',
  svgContainer: '.stadium-svg-renderer'
};

/**
 * Helper: Perform admin login
 */
async function loginAsAdmin(page: Page): Promise<void> {
  try {
    // Navigate directly to login page
    await page.goto(ADMIN_URLS.login, { waitUntil: 'networkidle', timeout: 30000 });

    // Wait for page to load
    await page.waitForLoadState('domcontentloaded');

    // Check if login form is present
    const loginFormVisible = await page.locator('input[type="email"], input[type="username"], .login-form').first().isVisible({ timeout: 5000 }).catch(() => false);

    if (loginFormVisible) {
      console.log('Login form detected, performing authentication...');

      // Fill login credentials
      const emailInput = await page.locator('input[type="email"], input[type="username"]').first();
      await emailInput.waitFor({ state: 'visible', timeout: 5000 });
      await emailInput.fill(ADMIN_CREDENTIALS.email);

      const passwordInput = await page.locator('input[type="password"]').first();
      await passwordInput.waitFor({ state: 'visible', timeout: 5000 });
      await passwordInput.fill(ADMIN_CREDENTIALS.password);

      // Submit login
      const loginButton = await page.locator('button[type="submit"], .btn-login, button:has-text("Login")').first();
      await loginButton.click();

      // Wait for navigation after login
      await page.waitForURL('**/', { timeout: 15000 });
      await page.waitForLoadState('networkidle', { timeout: 15000 });

      console.log('Login successful');
    } else {
      console.log('No login form found - already authenticated');
    }
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
}

/**
 * Helper: Navigate to stadium overview and wait for complete load
 */
async function navigateToStadiumOverview(page: Page): Promise<void> {
  // Click on the Stadium Overview menu item in the sidebar
  const stadiumOverviewLink = await page.locator('text=Stadium Overview').first();
  await stadiumOverviewLink.click();

  // Wait for navigation
  await page.waitForLoadState('networkidle', { timeout: 30000 });

  // Wait for SVG to be visible
  await page.waitForSelector(SVG_SELECTORS.mainSvg, { state: 'visible', timeout: 15000 });

  // Wait for field element to render
  await page.waitForSelector(SVG_SELECTORS.fieldRect, { state: 'visible', timeout: 10000 });

  // Wait for any loading spinners to disappear
  await page.waitForFunction(() => {
    const spinners = document.querySelectorAll('.loading-spinner, .spinner-sm, .stadium-loading');
    return spinners.length === 0 || Array.from(spinners).every(s =>
      window.getComputedStyle(s as Element).display === 'none'
    );
  }, { timeout: 10000 });

  // Additional wait for SVG rendering and animations
  await page.waitForTimeout(2000);
}

/**
 * Helper: Get SVG element attributes
 */
async function getSvgAttributes(page: Page): Promise<any> {
  return await page.evaluate((selector) => {
    const svgElement = document.querySelector(selector);
    if (!svgElement) return null;

    return {
      id: svgElement.id,
      preserveAspectRatio: svgElement.getAttribute('preserveAspectRatio'),
      viewBox: svgElement.getAttribute('viewBox'),
      width: svgElement.getAttribute('width'),
      height: svgElement.getAttribute('height'),
      className: svgElement.className.baseVal || svgElement.className,
      namespaceURI: svgElement.namespaceURI
    };
  }, SVG_SELECTORS.mainSvg);
}

/**
 * Helper: Get field rectangle attributes and computed dimensions
 */
async function getFieldAttributes(page: Page): Promise<any> {
  return await page.evaluate((selector) => {
    const fieldElement = document.querySelector(selector);
    if (!fieldElement) return null;

    // Get explicit SVG attributes
    const x = fieldElement.getAttribute('x');
    const y = fieldElement.getAttribute('y');
    const width = fieldElement.getAttribute('width');
    const height = fieldElement.getAttribute('height');

    // Get computed dimensions from bounding box
    const bbox = (fieldElement as SVGRectElement).getBBox();

    // Calculate aspect ratio
    const aspectRatio = bbox.width / bbox.height;

    return {
      // Explicit attributes
      x: x ? parseFloat(x) : null,
      y: y ? parseFloat(y) : null,
      width: width ? parseFloat(width) : null,
      height: height ? parseFloat(height) : null,
      // Computed dimensions
      bboxX: bbox.x,
      bboxY: bbox.y,
      bboxWidth: bbox.width,
      bboxHeight: bbox.height,
      aspectRatio: aspectRatio,
      // Additional attributes
      fill: fieldElement.getAttribute('fill'),
      stroke: fieldElement.getAttribute('stroke'),
      className: fieldElement.className.baseVal || fieldElement.className,
      rx: fieldElement.getAttribute('rx'),
      ry: fieldElement.getAttribute('ry')
    };
  }, SVG_SELECTORS.fieldRect);
}

/**
 * Helper: Capture field visual metrics
 */
async function captureFieldVisualMetrics(page: Page): Promise<any> {
  return await page.evaluate(() => {
    const svgElement = document.querySelector('#stadium-svg-main') as SVGSVGElement;
    const fieldElement = document.querySelector('.stadium-field') as SVGRectElement;

    if (!svgElement || !fieldElement) return null;

    // Get SVG viewport dimensions
    const svgRect = svgElement.getBoundingClientRect();

    // Get field element dimensions in viewport coordinates
    const fieldRect = fieldElement.getBoundingClientRect();

    // Get viewBox for coordinate system context
    const viewBox = svgElement.viewBox.baseVal;

    return {
      svg: {
        viewportWidth: svgRect.width,
        viewportHeight: svgRect.height,
        viewBoxX: viewBox.x,
        viewBoxY: viewBox.y,
        viewBoxWidth: viewBox.width,
        viewBoxHeight: viewBox.height
      },
      field: {
        viewportX: fieldRect.x,
        viewportY: fieldRect.y,
        viewportWidth: fieldRect.width,
        viewportHeight: fieldRect.height,
        viewportAspectRatio: fieldRect.width / fieldRect.height
      }
    };
  });
}

test.describe('Stadium Field Proportions Fix Verification', () => {

  test.beforeEach(async ({ page, context }) => {
    // Configure context for self-signed certificates
    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    // Perform admin login
    await loginAsAdmin(page);

    // Navigate to stadium overview page
    await navigateToStadiumOverview(page);
  });

  test.describe('SVG Element Configuration', () => {

    test('SVG element exists with correct ID', async ({ page }) => {
      const svgElement = await page.locator(SVG_SELECTORS.mainSvg);
      await expect(svgElement).toBeVisible();

      const svgAttributes = await getSvgAttributes(page);
      expect(svgAttributes).not.toBeNull();
      expect(svgAttributes.id).toBe('stadium-svg-main');
      expect(svgAttributes.namespaceURI).toBe('http://www.w3.org/2000/svg');
    });

    test('SVG has preserveAspectRatio="xMidYMid meet"', async ({ page }) => {
      const svgAttributes = await getSvgAttributes(page);
      expect(svgAttributes).not.toBeNull();

      // Verify the preserveAspectRatio attribute is set correctly
      expect(svgAttributes.preserveAspectRatio).toBe('xMidYMid meet');

      console.log('SVG preserveAspectRatio:', svgAttributes.preserveAspectRatio);
    });

    test('SVG has correct viewBox dimensions', async ({ page }) => {
      const svgAttributes = await getSvgAttributes(page);
      expect(svgAttributes).not.toBeNull();

      // ViewBox should be "0 0 1200 900" based on HNKRijekaStadiumConstants
      expect(svgAttributes.viewBox).toContain('1200');
      expect(svgAttributes.viewBox).toContain('900');

      console.log('SVG viewBox:', svgAttributes.viewBox);
    });
  });

  test.describe('Field Rectangle Dimensions', () => {

    test('Field rect has correct width and height attributes', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Verify explicit SVG attributes match expected values
      expect(fieldAttributes.width).toBe(EXPECTED_FIELD_DIMENSIONS.width);
      expect(fieldAttributes.height).toBe(EXPECTED_FIELD_DIMENSIONS.height);

      console.log(`Field dimensions: ${fieldAttributes.width} x ${fieldAttributes.height}`);
    });

    test('Field rect has correct x and y position', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Verify field position matches constants
      expect(fieldAttributes.x).toBe(EXPECTED_FIELD_DIMENSIONS.x);
      expect(fieldAttributes.y).toBe(EXPECTED_FIELD_DIMENSIONS.y);

      console.log(`Field position: (${fieldAttributes.x}, ${fieldAttributes.y})`);
    });

    test('Field rect computed dimensions match attributes', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // BBox dimensions should match explicit attributes
      expect(fieldAttributes.bboxWidth).toBe(fieldAttributes.width);
      expect(fieldAttributes.bboxHeight).toBe(fieldAttributes.height);
      expect(fieldAttributes.bboxX).toBe(fieldAttributes.x);
      expect(fieldAttributes.bboxY).toBe(fieldAttributes.y);
    });
  });

  test.describe('Aspect Ratio Verification', () => {

    test('Field maintains 2:1 aspect ratio (500:250)', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Calculate aspect ratio from attributes
      const aspectRatio = fieldAttributes.width / fieldAttributes.height;

      // Verify exact 2:1 ratio
      expect(aspectRatio).toBe(EXPECTED_FIELD_DIMENSIONS.aspectRatio);
      expect(aspectRatio).toBe(2.0);

      console.log(`Field aspect ratio: ${aspectRatio.toFixed(2)} (expected: 2.0)`);
    });

    test('Field computed aspect ratio is exactly 2.0', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Verify computed aspect ratio from BBox
      expect(fieldAttributes.aspectRatio).toBe(2.0);

      console.log(`Computed aspect ratio: ${fieldAttributes.aspectRatio.toFixed(2)}`);
    });

    test('Field dimensions represent proper football field proportions', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Football field: 100m x 50m = 2:1 ratio
      // Our SVG representation: 500 units x 250 units = 2:1 ratio
      const scaleFactorWidth = fieldAttributes.width / 100; // 500/100 = 5 units per meter
      const scaleFactorHeight = fieldAttributes.height / 50; // 250/50 = 5 units per meter

      // Both scale factors should be equal (5 units per meter)
      expect(scaleFactorWidth).toBe(scaleFactorHeight);
      expect(scaleFactorWidth).toBe(5);

      console.log(`Scale factor: ${scaleFactorWidth} SVG units per meter`);
    });
  });

  test.describe('Visual Appearance Verification', () => {

    test('Field visual metrics show proper proportions on desktop', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);
      await page.waitForTimeout(500); // Allow for responsive adjustments

      const metrics = await captureFieldVisualMetrics(page);
      expect(metrics).not.toBeNull();

      // Viewport rendering should maintain aspect ratio
      const viewportAspectRatio = metrics.field.viewportAspectRatio;

      // Allow small tolerance due to browser rendering (±0.05)
      expect(viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
      expect(viewportAspectRatio).toBeLessThanOrEqual(2.05);

      console.log(`Desktop viewport aspect ratio: ${viewportAspectRatio.toFixed(3)}`);
    });

    test('Field maintains proportions on laptop viewport', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.laptop);
      await page.waitForTimeout(500);

      const metrics = await captureFieldVisualMetrics(page);
      expect(metrics).not.toBeNull();

      const viewportAspectRatio = metrics.field.viewportAspectRatio;
      expect(viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
      expect(viewportAspectRatio).toBeLessThanOrEqual(2.05);

      console.log(`Laptop viewport aspect ratio: ${viewportAspectRatio.toFixed(3)}`);
    });

    test('Field maintains proportions on tablet viewport', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.tablet);
      await page.waitForTimeout(500);

      const metrics = await captureFieldVisualMetrics(page);
      expect(metrics).not.toBeNull();

      const viewportAspectRatio = metrics.field.viewportAspectRatio;
      expect(viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
      expect(viewportAspectRatio).toBeLessThanOrEqual(2.05);

      console.log(`Tablet viewport aspect ratio: ${viewportAspectRatio.toFixed(3)}`);
    });

    test('Field maintains proportions on mobile viewport', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.mobile);
      await page.waitForTimeout(500);

      const metrics = await captureFieldVisualMetrics(page);
      expect(metrics).not.toBeNull();

      const viewportAspectRatio = metrics.field.viewportAspectRatio;
      expect(viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
      expect(viewportAspectRatio).toBeLessThanOrEqual(2.05);

      console.log(`Mobile viewport aspect ratio: ${viewportAspectRatio.toFixed(3)}`);
    });
  });

  test.describe('SVG Rendering Quality', () => {

    test('Field element has proper styling attributes', async ({ page }) => {
      const fieldAttributes = await getFieldAttributes(page);
      expect(fieldAttributes).not.toBeNull();

      // Verify field has visual styling
      expect(fieldAttributes.fill).toBeTruthy();
      expect(fieldAttributes.stroke).toBeTruthy();

      // Verify rounded corners for realistic appearance
      expect(fieldAttributes.rx).toBeTruthy();
      expect(fieldAttributes.ry).toBeTruthy();

      console.log('Field styling:', {
        fill: fieldAttributes.fill,
        stroke: fieldAttributes.stroke,
        borderRadius: `rx=${fieldAttributes.rx}, ry=${fieldAttributes.ry}`
      });
    });

    test('SVG container is visible and properly sized', async ({ page }) => {
      const containerVisible = await page.locator(SVG_SELECTORS.svgContainer).isVisible();
      expect(containerVisible).toBe(true);

      const containerBox = await page.locator(SVG_SELECTORS.svgContainer).boundingBox();
      expect(containerBox).not.toBeNull();
      expect(containerBox!.width).toBeGreaterThan(0);
      expect(containerBox!.height).toBeGreaterThan(0);

      console.log(`SVG container size: ${containerBox!.width}px x ${containerBox!.height}px`);
    });

    test('Field is fully visible within SVG viewport', async ({ page }) => {
      const metrics = await captureFieldVisualMetrics(page);
      expect(metrics).not.toBeNull();

      // Field should be fully contained within SVG viewport
      expect(metrics.field.viewportX).toBeGreaterThanOrEqual(0);
      expect(metrics.field.viewportY).toBeGreaterThanOrEqual(0);
      expect(metrics.field.viewportWidth).toBeGreaterThan(0);
      expect(metrics.field.viewportHeight).toBeGreaterThan(0);

      console.log('Field viewport position:', {
        x: metrics.field.viewportX.toFixed(2),
        y: metrics.field.viewportY.toFixed(2),
        width: metrics.field.viewportWidth.toFixed(2),
        height: metrics.field.viewportHeight.toFixed(2)
      });
    });
  });

  test.describe('Cross-Viewport Consistency', () => {

    test('Field dimensions are consistent across all viewport sizes', async ({ page }) => {
      const viewportTests = [
        { name: 'Desktop', size: VIEWPORT_SIZES.desktop },
        { name: 'Laptop', size: VIEWPORT_SIZES.laptop },
        { name: 'Tablet', size: VIEWPORT_SIZES.tablet },
        { name: 'Mobile', size: VIEWPORT_SIZES.mobile }
      ];

      const results: any[] = [];

      for (const viewport of viewportTests) {
        await page.setViewportSize(viewport.size);
        await page.waitForTimeout(500);

        const fieldAttributes = await getFieldAttributes(page);
        expect(fieldAttributes).not.toBeNull();

        results.push({
          viewport: viewport.name,
          width: fieldAttributes.width,
          height: fieldAttributes.height,
          aspectRatio: fieldAttributes.aspectRatio
        });

        // Verify dimensions remain constant
        expect(fieldAttributes.width).toBe(EXPECTED_FIELD_DIMENSIONS.width);
        expect(fieldAttributes.height).toBe(EXPECTED_FIELD_DIMENSIONS.height);
        expect(fieldAttributes.aspectRatio).toBe(EXPECTED_FIELD_DIMENSIONS.aspectRatio);
      }

      console.log('Cross-viewport dimension consistency:');
      results.forEach(r => {
        console.log(`  ${r.viewport}: ${r.width}x${r.height} (ratio: ${r.aspectRatio.toFixed(2)})`);
      });
    });

    test('preserveAspectRatio maintains visual consistency across viewports', async ({ page }) => {
      const viewportTests = [
        { name: 'Desktop', size: VIEWPORT_SIZES.desktop },
        { name: 'Tablet', size: VIEWPORT_SIZES.tablet },
        { name: 'Mobile', size: VIEWPORT_SIZES.mobile }
      ];

      const visualResults: any[] = [];

      for (const viewport of viewportTests) {
        await page.setViewportSize(viewport.size);
        await page.waitForTimeout(500);

        const metrics = await captureFieldVisualMetrics(page);
        expect(metrics).not.toBeNull();

        visualResults.push({
          viewport: viewport.name,
          viewportAspectRatio: metrics.field.viewportAspectRatio,
          svgWidth: metrics.svg.viewportWidth,
          svgHeight: metrics.svg.viewportHeight
        });

        // All viewports should maintain aspect ratio within tolerance
        expect(metrics.field.viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
        expect(metrics.field.viewportAspectRatio).toBeLessThanOrEqual(2.05);
      }

      console.log('Visual aspect ratio consistency:');
      visualResults.forEach(r => {
        console.log(`  ${r.viewport}: ${r.viewportAspectRatio.toFixed(3)} (SVG: ${r.svgWidth.toFixed(0)}x${r.svgHeight.toFixed(0)})`);
      });
    });
  });

  test.describe('Screenshot Comparison', () => {

    test('Capture stadium field visual appearance on desktop', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.desktop);
      await page.waitForTimeout(1000);

      // Take screenshot of the entire stadium overview
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\test-results\\stadium-field-desktop.png',
        fullPage: false
      });

      // Take screenshot of just the SVG element
      const svgElement = await page.locator(SVG_SELECTORS.mainSvg);
      await svgElement.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\test-results\\stadium-field-svg-desktop.png'
      });

      console.log('Desktop screenshots saved to test-results/');
    });

    test('Capture stadium field visual appearance on mobile', async ({ page }) => {
      await page.setViewportSize(VIEWPORT_SIZES.mobile);
      await page.waitForTimeout(1000);

      // Take screenshot of the entire stadium overview
      await page.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\test-results\\stadium-field-mobile.png',
        fullPage: true
      });

      // Take screenshot of just the SVG element
      const svgElement = await page.locator(SVG_SELECTORS.mainSvg);
      await svgElement.screenshot({
        path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\test-results\\stadium-field-svg-mobile.png'
      });

      console.log('Mobile screenshots saved to test-results/');
    });
  });
});
