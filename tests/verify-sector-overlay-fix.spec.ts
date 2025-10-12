import { test, expect } from '@playwright/test';

test.describe('Verify Sector Overlay Fix', () => {
  test.setTimeout(60000);

  async function loginAsAdmin(page) {
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(2000);
  }

  test('Verify Sectors Are Visible After Fix', async ({ page }) => {
    console.log('\n==============================================');
    console.log('SECTOR OVERLAY FIX VERIFICATION');
    console.log('==============================================\n');

    await loginAsAdmin(page);
    await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Check sector count
    const sectorCount = await page.locator('.sector-overlay').count();
    console.log(`✓ Sector overlays found: ${sectorCount}`);

    expect(sectorCount).toBeGreaterThan(0);

    // Check first sector's computed styles
    const firstSector = await page.locator('.sector-overlay').first();
    const sectorDetails = await firstSector.evaluate((el: HTMLElement) => {
      const style = window.getComputedStyle(el);
      const inlineStyle = el.getAttribute('style');
      const boundingBox = el.getBoundingClientRect();

      return {
        inlineStyle,
        computedWidth: style.width,
        computedHeight: style.height,
        computedTop: style.top,
        computedLeft: style.left,
        boundingBox: {
          width: boundingBox.width,
          height: boundingBox.height,
          x: boundingBox.x,
          y: boundingBox.y
        }
      };
    });

    console.log('\n📐 First Sector Details:');
    console.log(`  Inline Style: ${sectorDetails.inlineStyle}`);
    console.log(`  Computed Width: ${sectorDetails.computedWidth}`);
    console.log(`  Computed Height: ${sectorDetails.computedHeight}`);
    console.log(`  Computed Position: ${sectorDetails.computedTop}, ${sectorDetails.computedLeft}`);
    console.log(`  Bounding Box: ${sectorDetails.boundingBox.width}x${sectorDetails.boundingBox.height} at (${sectorDetails.boundingBox.x}, ${sectorDetails.boundingBox.y})`);

    // Verify inline style uses PERIODS not COMMAS
    const hasCommas = sectorDetails.inlineStyle?.includes(',');
    const hasPeriods = sectorDetails.inlineStyle?.includes('.');

    console.log(`\n🔍 Number Format Check:`);
    console.log(`  Contains commas: ${hasCommas ? '❌ WRONG' : '✅ CORRECT'}`);
    console.log(`  Contains periods: ${hasPeriods ? '✅ CORRECT' : '❌ WRONG'}`);

    // Verify sector is visible size (not 4px)
    const width = parseFloat(sectorDetails.computedWidth);
    const height = parseFloat(sectorDetails.computedHeight);

    console.log(`\n📏 Size Verification:`);
    console.log(`  Width > 10px: ${width > 10 ? '✅ PASS' : '❌ FAIL'}`);
    console.log(`  Height > 10px: ${height > 10 ? '✅ PASS' : '❌ FAIL'}`);

    // Take screenshot
    await page.screenshot({
      path: 'sector-overlay-fixed.png',
      fullPage: true
    });
    console.log('\n✓ Screenshot saved: sector-overlay-fixed.png');

    // Assertions
    expect(hasCommas).toBe(false); // Should NOT have commas
    expect(hasPeriods).toBe(true);  // Should HAVE periods
    expect(width).toBeGreaterThan(10); // Should be visible size
    expect(height).toBeGreaterThan(10); // Should be visible size

    console.log('\n==============================================');
    console.log('✅ FIX VERIFIED - SECTORS ARE VISIBLE!');
    console.log('==============================================\n');
  });
});
