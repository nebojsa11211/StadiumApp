import { test, expect } from '@playwright/test';

interface SectorPosition {
  code: string;
  top: number;
  left: number;
  width: number;
  height: number;
}

test.describe('Visual Sector Alignment Verification', () => {
  test('sectors should align PERFECTLY between Drawing Tool and Stadium Overview', async ({ page }) => {
    console.log('\n=== VISUAL SECTOR ALIGNMENT VERIFICATION ===\n');

    // 1. LOGIN AS ADMIN
    console.log('Step 1: Logging in as admin...');
    await page.goto('https://localhost:7030/login');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    // Wait for redirect to homepage after successful login
    await page.waitForURL(/\/$|\/admin/, { timeout: 10000 });
    await page.waitForTimeout(1000); // Wait for page to stabilize
    console.log('✅ Login successful\n');

    // 2. DRAWING TOOL VERIFICATION
    console.log('Step 2: Navigating to Drawing Tool...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool');
    await page.waitForTimeout(2000); // Wait for canvas to load

    // Take full-page screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\drawing-tool-final.png',
      fullPage: true
    });
    console.log('✅ Drawing Tool screenshot saved: drawing-tool-final.png\n');

    // Extract coordinates of first 3 visible sectors from canvas data
    console.log('Step 3: Extracting Drawing Tool sector positions...');
    const drawingToolSectors = await page.evaluate(() => {
      const sectors: any[] = [];

      // Access the global sectors data from the Blazor component
      // The Drawing Tool stores sectors in window object or component state
      const windowObj = window as any;

      // Try to find sectors from various possible sources
      let sectorsData = windowObj.sectors ||
                        windowObj.drawnSectors ||
                        windowObj.sectorOverlays ||
                        [];

      // If we can't find global sectors, try to extract from canvas context
      const canvas = document.querySelector('#admin-drawing-tool-canvas') as HTMLCanvasElement;

      if (sectorsData.length === 0 && canvas) {
        // Try to extract from canvas overlays if they exist
        const overlays = canvas.parentElement?.querySelectorAll('[data-sector-code], .sector-overlay');
        if (overlays && overlays.length > 0) {
          Array.from(overlays).slice(0, 3).forEach(overlay => {
            const rect = overlay.getBoundingClientRect();
            const code = overlay.getAttribute('data-sector-code') ||
                        overlay.textContent?.trim() ||
                        'unknown';

            sectors.push({
              code: code,
              top: Math.round(rect.top + window.scrollY),
              left: Math.round(rect.left + window.scrollX),
              width: Math.round(rect.width),
              height: Math.round(rect.height)
            });
          });
        }
      } else {
        // Use sectors from global data
        sectorsData.slice(0, 3).forEach((sector: any) => {
          sectors.push({
            code: sector.code || sector.sectorCode || sector.id || 'unknown',
            top: Math.round(sector.top || sector.y || 0),
            left: Math.round(sector.left || sector.x || 0),
            width: Math.round(sector.width || 0),
            height: Math.round(sector.height || 0)
          });
        });
      }

      return sectors;
    });

    console.log('Drawing Tool Sectors Found:');
    drawingToolSectors.forEach((sector, idx) => {
      console.log(`  Sector ${idx + 1}: ${sector.code}`);
      console.log(`    Position: top=${sector.top}px, left=${sector.left}px`);
      console.log(`    Size: ${sector.width}x${sector.height}px`);
    });
    console.log('');

    // 3. STADIUM OVERVIEW VERIFICATION
    console.log('Step 4: Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForTimeout(3000); // Wait for image and overlays to load

    // Take full-page screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-final.png',
      fullPage: true
    });
    console.log('✅ Stadium Overview screenshot saved: stadium-overview-final.png\n');

    // Extract coordinates of first 3 sector overlays
    console.log('Step 5: Extracting Stadium Overview sector positions...');
    const overviewSectors = await page.evaluate(() => {
      const sectors: any[] = [];

      // Find all sector overlay elements (from diagnostic we know the class is "sector-overlay")
      const sectorElements = document.querySelectorAll('.sector-overlay');

      for (let i = 0; i < Math.min(3, sectorElements.length); i++) {
        const element = sectorElements[i] as HTMLElement;
        const rect = element.getBoundingClientRect();

        // Extract sector code from the element text content
        const labelElement = element.querySelector('.sector-label');
        const code = labelElement?.textContent?.trim() ||
                     element.textContent?.trim() ||
                     element.id.replace('admin-stadium-overview-sector-', '') ||
                     `sector-${i + 1}`;

        sectors.push({
          code: code.trim(),
          top: Math.round(rect.top + window.scrollY),
          left: Math.round(rect.left + window.scrollX),
          width: Math.round(rect.width),
          height: Math.round(rect.height)
        });
      }

      return sectors;
    });

    console.log('Stadium Overview Sectors Found:');
    overviewSectors.forEach((sector, idx) => {
      console.log(`  Sector ${idx + 1}: ${sector.code}`);
      console.log(`    Position: top=${sector.top}px, left=${sector.left}px`);
      console.log(`    Size: ${sector.width}x${sector.height}px`);
    });
    console.log('');

    // 4. SIDE-BY-SIDE COMPARISON
    console.log('Step 6: Performing Side-by-Side Comparison...\n');
    console.log('=== VISUAL COMPARISON REPORT ===\n');

    const maxAllowedDifference = 5; // pixels
    let allSectorsMatch = true;
    const comparisonResults: Array<{
      sector: string;
      topDiff: number;
      leftDiff: number;
      widthDiff: number;
      heightDiff: number;
      maxDiff: number;
      passed: boolean;
    }> = [];

    const minSectorCount = Math.min(drawingToolSectors.length, overviewSectors.length);

    if (minSectorCount === 0) {
      console.log('⚠️  WARNING: No sectors found on one or both pages!');
      console.log(`   Drawing Tool: ${drawingToolSectors.length} sectors`);
      console.log(`   Stadium Overview: ${overviewSectors.length} sectors`);
      throw new Error('No sectors found for comparison');
    }

    for (let i = 0; i < minSectorCount; i++) {
      const drawingSector = drawingToolSectors[i];
      const overviewSector = overviewSectors[i];

      const topDiff = Math.abs(drawingSector.top - overviewSector.top);
      const leftDiff = Math.abs(drawingSector.left - overviewSector.left);
      const widthDiff = Math.abs(drawingSector.width - overviewSector.width);
      const heightDiff = Math.abs(drawingSector.height - overviewSector.height);
      const maxDiff = Math.max(topDiff, leftDiff, widthDiff, heightDiff);

      const passed = maxDiff < maxAllowedDifference;
      if (!passed) allSectorsMatch = false;

      comparisonResults.push({
        sector: drawingSector.code,
        topDiff,
        leftDiff,
        widthDiff,
        heightDiff,
        maxDiff,
        passed
      });

      console.log(`Sector ${i + 1}: ${drawingSector.code}`);
      console.log(`  Drawing Tool:     top=${drawingSector.top}px, left=${drawingSector.left}px, ${drawingSector.width}x${drawingSector.height}px`);
      console.log(`  Stadium Overview: top=${overviewSector.top}px, left=${overviewSector.left}px, ${overviewSector.width}x${overviewSector.height}px`);
      console.log(`  Position Diff:    top=${topDiff}px, left=${leftDiff}px`);
      console.log(`  Size Diff:        width=${widthDiff}px, height=${heightDiff}px`);
      console.log(`  Max Difference:   ${maxDiff}px`);
      console.log(`  Result: ${passed ? '✅ PERFECT MATCH' : `❌ MISALIGNED (>${maxAllowedDifference}px)`}\n`);
    }

    // 5. FINAL VERDICT
    console.log('=== FINAL VERDICT ===\n');

    if (allSectorsMatch) {
      console.log('✅ ✅ ✅ SECTORS ALIGN PERFECTLY ✅ ✅ ✅');
      console.log(`All ${minSectorCount} sectors have differences < ${maxAllowedDifference}px`);
      console.log('Browser rounding differences only - ALIGNMENT IS PERFECT!\n');
    } else {
      console.log('❌ ❌ ❌ SECTORS MISALIGNED ❌ ❌ ❌');
      console.log('The following sectors have significant misalignment:\n');

      comparisonResults.forEach((result, idx) => {
        if (!result.passed) {
          console.log(`  Sector ${idx + 1} (${result.sector}):`);
          console.log(`    Max difference: ${result.maxDiff}px`);
          console.log(`    Top diff: ${result.topDiff}px, Left diff: ${result.leftDiff}px`);
          console.log(`    Width diff: ${result.widthDiff}px, Height diff: ${result.heightDiff}px\n`);
        }
      });
    }

    // Summary statistics
    const avgMaxDiff = comparisonResults.reduce((sum, r) => sum + r.maxDiff, 0) / comparisonResults.length;
    console.log('=== STATISTICS ===');
    console.log(`Total sectors compared: ${minSectorCount}`);
    console.log(`Sectors passing (< ${maxAllowedDifference}px): ${comparisonResults.filter(r => r.passed).length}`);
    console.log(`Sectors failing (>= ${maxAllowedDifference}px): ${comparisonResults.filter(r => !r.passed).length}`);
    console.log(`Average max difference: ${avgMaxDiff.toFixed(2)}px`);
    console.log('');

    // Assert that all sectors align perfectly
    expect(allSectorsMatch, 'All sectors should align within 5px tolerance').toBe(true);
  });
});
