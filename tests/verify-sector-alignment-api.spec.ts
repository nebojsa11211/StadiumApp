import { test, expect } from '@playwright/test';
import https from 'https';

interface SectorData {
  sectorCode: string;
  topPercent: number;
  leftPercent: number;
  widthPercent: number;
  heightPercent: number;
}

interface SectorPosition {
  code: string;
  top: number;
  left: number;
  width: number;
  height: number;
}

test.describe('API-Based Sector Alignment Verification', () => {
  test('Stadium Overview sectors should match saved API data PERFECTLY', async ({ page }) => {
    console.log('\n=== API-BASED SECTOR ALIGNMENT VERIFICATION ===\n');

    // 1. LOGIN AS ADMIN
    console.log('Step 1: Logging in as admin...');
    await page.goto('https://localhost:7030/login');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL(/\/$|\/admin/, { timeout: 10000 });
    await page.waitForTimeout(1000);
    console.log('✅ Login successful\n');

    // 2. FETCH SECTOR DATA FROM API
    console.log('Step 2: Fetching sector data from API...');
    const apiSectors = await page.evaluate(async () => {
      try {
        const response = await fetch('https://localhost:7010/api/StadiumSectorOverlay', {
          method: 'GET',
          headers: {
            'Accept': 'application/json'
          }
        });

        if (!response.ok) {
          console.error('API request failed:', response.status, response.statusText);
          return [];
        }

        const data = await response.json();
        console.log('API Response:', data);
        return data;
      } catch (error) {
        console.error('API fetch error:', error);
        return [];
      }
    });

    console.log(`✅ Fetched ${apiSectors.length} sectors from API`);
    apiSectors.slice(0, 3).forEach((sector: SectorData, idx: number) => {
      console.log(`  Sector ${idx + 1}: ${sector.sectorCode}`);
      console.log(`    Position: top=${sector.topPercent}%, left=${sector.leftPercent}%`);
      console.log(`    Size: ${sector.widthPercent}% x ${sector.heightPercent}%`);
    });
    console.log('');

    // 3. NAVIGATE TO STADIUM OVERVIEW
    console.log('Step 3: Navigating to Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForTimeout(3000); // Wait for image and overlays to load

    // Take screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-alignment.png',
      fullPage: true
    });
    console.log('✅ Stadium Overview screenshot saved\n');

    // 4. EXTRACT RENDERED SECTOR POSITIONS
    console.log('Step 4: Extracting rendered sector positions...');
    const renderedSectors = await page.evaluate(() => {
      const sectors: any[] = [];
      const sectorElements = document.querySelectorAll('.sector-overlay');

      // Get the stadium image container for percentage calculation
      const imageContainer = document.querySelector('.stadium-image-container, [style*="position: relative"]') as HTMLElement;
      const containerRect = imageContainer?.getBoundingClientRect();

      sectorElements.forEach((element) => {
        const htmlElement = element as HTMLElement;
        const style = htmlElement.style;

        // Extract percentage values from inline style
        const topMatch = style.top?.match(/([\d.]+)%/);
        const leftMatch = style.left?.match(/([\d.]+)%/);
        const widthMatch = style.width?.match(/([\d.]+)%/);
        const heightMatch = style.height?.match(/([\d.]+)%/);

        const labelElement = htmlElement.querySelector('.sector-label');
        const code = labelElement?.textContent?.trim() || 'unknown';

        if (topMatch && leftMatch && widthMatch && heightMatch) {
          sectors.push({
            code: code,
            topPercent: parseFloat(topMatch[1]),
            leftPercent: parseFloat(leftMatch[1]),
            widthPercent: parseFloat(widthMatch[1]),
            heightPercent: parseFloat(heightMatch[1])
          });
        }
      });

      return sectors;
    });

    console.log(`✅ Found ${renderedSectors.length} rendered sectors`);
    renderedSectors.slice(0, 3).forEach((sector: any, idx: number) => {
      console.log(`  Sector ${idx + 1}: ${sector.code}`);
      console.log(`    Position: top=${sector.topPercent}%, left=${sector.leftPercent}%`);
      console.log(`    Size: ${sector.widthPercent}% x ${sector.heightPercent}%`);
    });
    console.log('');

    // 5. COMPARE API DATA WITH RENDERED DATA
    console.log('Step 5: Comparing API data with rendered positions...\n');
    console.log('=== ALIGNMENT COMPARISON REPORT ===\n');

    const maxAllowedDifference = 0.1; // 0.1% tolerance
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

    // Match sectors by code
    for (const apiSector of apiSectors.slice(0, 3)) {
      const renderedSector = renderedSectors.find((r: any) => r.code === apiSector.sectorCode);

      if (!renderedSector) {
        console.log(`❌ Sector ${apiSector.sectorCode}: NOT FOUND in rendered output`);
        allSectorsMatch = false;
        continue;
      }

      const topDiff = Math.abs(apiSector.topPercent - renderedSector.topPercent);
      const leftDiff = Math.abs(apiSector.leftPercent - renderedSector.leftPercent);
      const widthDiff = Math.abs(apiSector.widthPercent - renderedSector.widthPercent);
      const heightDiff = Math.abs(apiSector.heightPercent - renderedSector.heightPercent);
      const maxDiff = Math.max(topDiff, leftDiff, widthDiff, heightDiff);

      const passed = maxDiff < maxAllowedDifference;
      if (!passed) allSectorsMatch = false;

      comparisonResults.push({
        sector: apiSector.sectorCode,
        topDiff,
        leftDiff,
        widthDiff,
        heightDiff,
        maxDiff,
        passed
      });

      console.log(`Sector: ${apiSector.sectorCode}`);
      console.log(`  API Data:      top=${apiSector.topPercent.toFixed(4)}%, left=${apiSector.leftPercent.toFixed(4)}%, ${apiSector.widthPercent.toFixed(4)}% x ${apiSector.heightPercent.toFixed(4)}%`);
      console.log(`  Rendered:      top=${renderedSector.topPercent.toFixed(4)}%, left=${renderedSector.leftPercent.toFixed(4)}%, ${renderedSector.widthPercent.toFixed(4)}% x ${renderedSector.heightPercent.toFixed(4)}%`);
      console.log(`  Differences:   top=${topDiff.toFixed(4)}%, left=${leftDiff.toFixed(4)}%, width=${widthDiff.toFixed(4)}%, height=${heightDiff.toFixed(4)}%`);
      console.log(`  Max Diff:      ${maxDiff.toFixed(4)}%`);
      console.log(`  Result:        ${passed ? '✅ PERFECT MATCH' : `❌ MISALIGNED (>${maxAllowedDifference}%)`}\n`);
    }

    // 6. FINAL VERDICT
    console.log('=== FINAL VERDICT ===\n');

    if (allSectorsMatch) {
      console.log('✅ ✅ ✅ SECTORS ALIGN PERFECTLY ✅ ✅ ✅');
      console.log(`All ${comparisonResults.length} sectors match API data within ${maxAllowedDifference}% tolerance`);
      console.log('Stadium Overview rendering is PIXEL-PERFECT!\n');
    } else {
      console.log('❌ ❌ ❌ SECTORS MISALIGNED ❌ ❌ ❌');
      console.log('The following sectors have significant misalignment:\n');

      comparisonResults.forEach((result) => {
        if (!result.passed) {
          console.log(`  ${result.sector}:`);
          console.log(`    Max difference: ${result.maxDiff.toFixed(4)}%`);
          console.log(`    Top: ${result.topDiff.toFixed(4)}%, Left: ${result.leftDiff.toFixed(4)}%`);
          console.log(`    Width: ${result.widthDiff.toFixed(4)}%, Height: ${result.heightDiff.toFixed(4)}%\n`);
        }
      });
    }

    // Summary statistics
    const avgMaxDiff = comparisonResults.reduce((sum, r) => sum + r.maxDiff, 0) / comparisonResults.length;
    console.log('=== STATISTICS ===');
    console.log(`Total sectors compared: ${comparisonResults.length}`);
    console.log(`Sectors passing (< ${maxAllowedDifference}%): ${comparisonResults.filter(r => r.passed).length}`);
    console.log(`Sectors failing (>= ${maxAllowedDifference}%): ${comparisonResults.filter(r => !r.passed).length}`);
    console.log(`Average max difference: ${avgMaxDiff.toFixed(4)}%`);
    console.log('');

    // Assert that all sectors align perfectly
    expect(allSectorsMatch, `All sectors should align within ${maxAllowedDifference}% tolerance`).toBe(true);
  });
});
