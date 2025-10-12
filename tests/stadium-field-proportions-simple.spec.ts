import { test, expect } from '@playwright/test';

/**
 * Simplified Stadium Field Proportions Test
 * Verifies the stadium field maintains proper 2:1 aspect ratio (100m x 50m)
 *
 * Changes verified:
 * - HNKRijekaStadiumConstants.cs: FIELD = (350, 285, 500, 250)
 * - StadiumSvgRenderer.razor: preserveAspectRatio="xMidYMid meet"
 */

test.describe('Stadium Field Proportions Verification', () => {

  test('Verify stadium field has correct dimensions and 2:1 aspect ratio', async ({ page }) => {
    // Step 1: Navigate to login page
    await page.goto('https://localhost:7030/login');

    // Step 2: Login as admin
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Step 3: Wait for dashboard to load
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);

    // Step 4: Click on Stadium Overview in sidebar
    await page.click('text=Stadium Overview');
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000);

    // Step 4b: Scroll down to see the stadium layout
    await page.evaluate(() => window.scrollTo(0, document.body.scrollHeight));
    await page.waitForTimeout(1000);

    // Step 4c: Check what SVG elements exist on the page
    const svgCheck = await page.evaluate(() => {
      const allSvgs = document.querySelectorAll('svg');
      return Array.from(allSvgs).map(svg => ({
        id: svg.id || 'no-id',
        className: svg.className.baseVal || svg.className || 'no-class',
        visible: svg.getBoundingClientRect().height > 0
      }));
    });
    console.log('SVG elements found:', svgCheck);

    // Step 5: Verify SVG element exists
    const svgElement = await page.locator('#stadium-svg-main');
    await expect(svgElement).toBeVisible({ timeout: 10000 });

    // Step 6: Get SVG attributes
    const svgAttributes = await page.evaluate(() => {
      const svg = document.querySelector('#stadium-svg-main');
      if (!svg) return null;

      return {
        id: svg.id,
        preserveAspectRatio: svg.getAttribute('preserveAspectRatio'),
        viewBox: svg.getAttribute('viewBox'),
        exists: true
      };
    });

    console.log('SVG Attributes:', svgAttributes);

    // Step 7: Verify preserveAspectRatio attribute
    expect(svgAttributes?.preserveAspectRatio).toBe('xMidYMid meet');

    // Step 8: Get field rectangle attributes
    const fieldAttributes = await page.evaluate(() => {
      const field = document.querySelector('.stadium-field');
      if (!field) return null;

      const x = parseFloat(field.getAttribute('x') || '0');
      const y = parseFloat(field.getAttribute('y') || '0');
      const width = parseFloat(field.getAttribute('width') || '0');
      const height = parseFloat(field.getAttribute('height') || '0');

      return {
        x,
        y,
        width,
        height,
        aspectRatio: width / height
      };
    });

    console.log('Field Attributes:', fieldAttributes);

    // Step 9: Verify field dimensions
    expect(fieldAttributes?.x).toBe(350);
    expect(fieldAttributes?.y).toBe(285);
    expect(fieldAttributes?.width).toBe(500);
    expect(fieldAttributes?.height).toBe(250);

    // Step 10: Verify 2:1 aspect ratio
    expect(fieldAttributes?.aspectRatio).toBe(2.0);

    // Step 11: Take screenshot for visual verification
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\test-results\\stadium-field-verification.png',
      fullPage: false
    });

    // Step 12: Test responsive behavior - Desktop
    await page.setViewportSize({ width: 1920, height: 1080 });
    await page.waitForTimeout(1000);

    const desktopMetrics = await page.evaluate(() => {
      const svg = document.querySelector('#stadium-svg-main') as SVGSVGElement;
      const field = document.querySelector('.stadium-field') as SVGRectElement;
      if (!svg || !field) return null;

      const fieldRect = field.getBoundingClientRect();
      return {
        viewportAspectRatio: fieldRect.width / fieldRect.height,
        viewportWidth: fieldRect.width,
        viewportHeight: fieldRect.height
      };
    });

    console.log('Desktop Metrics:', desktopMetrics);
    expect(desktopMetrics?.viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
    expect(desktopMetrics?.viewportAspectRatio).toBeLessThanOrEqual(2.05);

    // Step 13: Test responsive behavior - Tablet
    await page.setViewportSize({ width: 768, height: 1024 });
    await page.waitForTimeout(1000);

    const tabletMetrics = await page.evaluate(() => {
      const field = document.querySelector('.stadium-field') as SVGRectElement;
      if (!field) return null;

      const fieldRect = field.getBoundingClientRect();
      return {
        viewportAspectRatio: fieldRect.width / fieldRect.height
      };
    });

    console.log('Tablet Metrics:', tabletMetrics);
    expect(tabletMetrics?.viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
    expect(tabletMetrics?.viewportAspectRatio).toBeLessThanOrEqual(2.05);

    // Step 14: Test responsive behavior - Mobile
    await page.setViewportSize({ width: 375, height: 667 });
    await page.waitForTimeout(1000);

    const mobileMetrics = await page.evaluate(() => {
      const field = document.querySelector('.stadium-field') as SVGRectElement;
      if (!field) return null;

      const fieldRect = field.getBoundingClientRect();
      return {
        viewportAspectRatio: fieldRect.width / fieldRect.height
      };
    });

    console.log('Mobile Metrics:', mobileMetrics);
    expect(mobileMetrics?.viewportAspectRatio).toBeGreaterThanOrEqual(1.95);
    expect(mobileMetrics?.viewportAspectRatio).toBeLessThanOrEqual(2.05);

    // Final verification summary
    console.log('\n=== FIELD PROPORTIONS VERIFICATION SUMMARY ===');
    console.log(`✓ SVG preserveAspectRatio: ${svgAttributes?.preserveAspectRatio}`);
    console.log(`✓ Field dimensions: ${fieldAttributes?.width}x${fieldAttributes?.height}`);
    console.log(`✓ Field position: (${fieldAttributes?.x}, ${fieldAttributes?.y})`);
    console.log(`✓ Aspect ratio: ${fieldAttributes?.aspectRatio.toFixed(2)} (expected: 2.0)`);
    console.log(`✓ Desktop viewport ratio: ${desktopMetrics?.viewportAspectRatio.toFixed(3)}`);
    console.log(`✓ Tablet viewport ratio: ${tabletMetrics?.viewportAspectRatio.toFixed(3)}`);
    console.log(`✓ Mobile viewport ratio: ${mobileMetrics?.viewportAspectRatio.toFixed(3)}`);
    console.log('==============================================\n');
  });
});
