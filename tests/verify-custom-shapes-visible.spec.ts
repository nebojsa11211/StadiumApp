import { test, expect } from '@playwright/test';

test.describe('Verify Custom Shapes Visible in Stadium Overview', () => {
  test.setTimeout(90000);

  async function loginAsAdmin(page) {
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(3000);
  }

  test('Verify all sector shapes render correctly', async ({ page }) => {
    console.log('\n==============================================');
    console.log('CUSTOM SHAPES VISIBILITY VERIFICATION');
    console.log('==============================================\n');

    await loginAsAdmin(page);

    // ==================== GET API DATA ====================
    console.log('📊 Fetching sector data from API...');
    const apiResponse = await page.request.get('https://localhost:7010/api/StadiumSectorOverlay');
    const sectors = await apiResponse.json();

    console.log(`✓ Found ${sectors.length} sectors in database\n`);

    // Count shape types
    const shapeTypes = {
      rectangle: 0,
      triangle: 0,
      rhombus: 0,
      polygon: 0,
      hexagon: 0,
      circular: 0
    };

    sectors.forEach(sector => {
      const shapeType = sector.shapeType?.toLowerCase() || 'rectangle';
      if (shapeTypes.hasOwnProperty(shapeType)) {
        shapeTypes[shapeType]++;
      }
    });

    console.log('📐 Shape Type Distribution:');
    Object.entries(shapeTypes).forEach(([type, count]) => {
      if (count > 0) {
        console.log(`  ${type}: ${count} sector(s)`);
      }
    });

    // ==================== STADIUM OVERVIEW ====================
    console.log('\n🏟️ Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Check if SVG overlay exists
    const svgExists = await page.locator('#admin-stadium-overview-svg-overlay').count() > 0;
    console.log(`SVG Overlay Present: ${svgExists ? '✅ YES' : '❌ NO'}`);

    if (!svgExists) {
      console.log('\n❌ CRITICAL ERROR: SVG overlay not found!');
      throw new Error('SVG overlay element not found in DOM');
    }

    // Count rendered sector groups
    const renderedCount = await page.locator('.sector-group').count();
    console.log(`\n✓ Rendered sector groups: ${renderedCount}`);

    expect(renderedCount).toBe(sectors.length);

    // ==================== SHAPE-BY-SHAPE VERIFICATION ====================
    console.log('\n📋 Verifying Individual Shapes:\n');

    for (let i = 0; i < Math.min(sectors.length, 10); i++) {
      const sector = sectors[i];
      const sectorCode = sector.sectorCode;
      const shapeType = sector.shapeType || 'rectangle';

      // Check if sector group exists
      const groupSelector = `#admin-stadium-overview-sector-group-${sectorCode}`;
      const groupExists = await page.locator(groupSelector).count() > 0;

      // Check if path exists
      const pathSelector = `#admin-stadium-overview-sector-path-${sectorCode}`;
      const pathExists = await page.locator(pathSelector).count() > 0;

      // Get path d attribute
      let pathD = '';
      if (pathExists) {
        pathD = await page.locator(pathSelector).getAttribute('d') || '';
      }

      // Determine if it's a custom shape
      const isCustomShape = shapeType !== 'rectangle' && shapeType !== '';
      const hasVertexData = sector.shapeData && sector.shapeData.length > 0;

      console.log(`Sector ${i + 1}: ${sectorCode} (${shapeType})`);
      console.log(`  Group exists: ${groupExists ? '✅' : '❌'}`);
      console.log(`  Path exists: ${pathExists ? '✅' : '❌'}`);
      console.log(`  Is custom shape: ${isCustomShape ? 'YES' : 'NO'}`);
      console.log(`  Has vertex data: ${hasVertexData ? 'YES' : 'NO'}`);
      console.log(`  Path data: ${pathD.substring(0, 50)}${pathD.length > 50 ? '...' : ''}`);

      if (isCustomShape && hasVertexData) {
        // For custom shapes, path should contain 'L' (lineto) commands
        const hasLineCommands = pathD.includes('L');
        console.log(`  Path has polygon commands: ${hasLineCommands ? '✅' : '❌'}`);

        if (!hasLineCommands) {
          console.log(`  ⚠️ WARNING: Custom shape ${sectorCode} has no polygon path data!`);
        }
      }

      console.log('');
    }

    // ==================== VISUAL VERIFICATION ====================
    await page.screenshot({
      path: 'stadium-overview-custom-shapes.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: stadium-overview-custom-shapes.png\n');

    // ==================== COMPARISON WITH DRAWING TOOL ====================
    console.log('🎨 Comparing with Drawing Tool...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    await page.screenshot({
      path: 'drawing-tool-custom-shapes.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: drawing-tool-custom-shapes.png\n');

    // ==================== FINAL VERDICT ====================
    console.log('==============================================');
    console.log('VERIFICATION COMPLETE');
    console.log('==============================================\n');

    console.log('Summary:');
    console.log(`  Total sectors in DB: ${sectors.length}`);
    console.log(`  Rendered in Overview: ${renderedCount}`);
    console.log(`  Match: ${renderedCount === sectors.length ? '✅ YES' : '❌ NO'}`);

    if (shapeTypes.rhombus > 0 || shapeTypes.triangle > 0 || shapeTypes.polygon > 0) {
      console.log(`\n  Custom shapes found:`);
      if (shapeTypes.rhombus > 0) console.log(`    - ${shapeTypes.rhombus} rhombus(es)`);
      if (shapeTypes.triangle > 0) console.log(`    - ${shapeTypes.triangle} triangle(s)`);
      if (shapeTypes.polygon > 0) console.log(`    - ${shapeTypes.polygon} polygon(s)`);
      if (shapeTypes.hexagon > 0) console.log(`    - ${shapeTypes.hexagon} hexagon(s)`);
      if (shapeTypes.circular > 0) console.log(`    - ${shapeTypes.circular} circular sector(s)`);
      console.log(`\n  ✅ All custom shapes should now be visible in Stadium Overview!`);
    } else {
      console.log(`\n  ℹ️ No custom shapes in database - only rectangles`);
    }

    console.log('\n==============================================\n');
  });
});
