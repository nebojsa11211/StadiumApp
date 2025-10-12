import { test, expect, ConsoleMessage } from '@playwright/test';
import * as fs from 'fs';
import * as path from 'path';

test.describe('Visual Sector Comparison - Drawing Tool vs Stadium Overview', () => {
  let consoleMessages: ConsoleMessage[] = [];
  let sectorData: any[] = [];

  test.beforeEach(async ({ page }) => {
    // Capture console messages
    page.on('console', (msg) => {
      consoleMessages.push(msg);
      console.log(`[BROWSER ${msg.type().toUpperCase()}]: ${msg.text()}`);
    });

    // Capture page errors
    page.on('pageerror', (error) => {
      console.error(`[PAGE ERROR]: ${error.message}`);
    });
  });

  test('Compare Drawing Tool and Stadium Overview with API data extraction', async ({ page, request }) => {
    console.log('\n=== VISUAL SECTOR COMPARISON TEST ===\n');

    // Step 1: Login as admin
    console.log('Step 1: Logging in as admin...');
    await page.goto('https://localhost:7030/login');
    await page.waitForLoadState('networkidle');

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for login to complete - admin redirects to root '/' after login
    await page.waitForURL(/\/(admin\/dashboard|admin|)$/, { timeout: 10000 });
    console.log('✓ Login successful\n');

    // Step 2: Navigate to Drawing Tool and take screenshot
    console.log('Step 2: Navigating to Drawing Tool page...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool');
    await page.waitForLoadState('networkidle');

    // Wait 3 seconds for full load
    console.log('Waiting 3 seconds for full page load...');
    await page.waitForTimeout(3000);

    const drawingToolScreenshot = 'drawing-tool-comparison.png';
    await page.screenshot({
      path: drawingToolScreenshot,
      fullPage: true
    });
    console.log(`✓ Drawing Tool screenshot saved: ${path.resolve(drawingToolScreenshot)}\n`);

    // Step 3: Navigate to Stadium Overview and take screenshot
    console.log('Step 3: Navigating to Stadium Overview page...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');

    // Wait 3 seconds for full load
    console.log('Waiting 3 seconds for full page load...');
    await page.waitForTimeout(3000);

    const overviewScreenshot = 'stadium-overview-comparison.png';
    await page.screenshot({
      path: overviewScreenshot,
      fullPage: true
    });
    console.log(`✓ Stadium Overview screenshot saved: ${path.resolve(overviewScreenshot)}\n`);

    // Step 4: Fetch API data
    console.log('Step 4: Fetching sector data from API...');
    try {
      const apiResponse = await request.get('https://localhost:7010/api/StadiumSectorOverlay', {
        ignoreHTTPSErrors: true
      });

      if (apiResponse.ok()) {
        sectorData = await apiResponse.json();
        console.log(`✓ Successfully fetched ${sectorData.length} sectors from API\n`);

        // Log each sector details
        console.log('=== SECTOR DATA DETAILS ===\n');
        sectorData.forEach((sector, index) => {
          console.log(`Sector ${index + 1}:`);
          console.log(`  SectorCode: ${sector.sectorCode}`);
          console.log(`  ShapeType: ${sector.shapeType}`);
          console.log(`  Position: Top=${sector.topPercent}%, Left=${sector.leftPercent}%`);
          console.log(`  Size: Width=${sector.widthPercent}%, Height=${sector.heightPercent}%`);

          if (sector.shapeData) {
            const shapeDataPreview = sector.shapeData.length > 100
              ? sector.shapeData.substring(0, 100) + '...'
              : sector.shapeData;
            console.log(`  ShapeData: ${shapeDataPreview}`);
          } else {
            console.log(`  ShapeData: (none)`);
          }
          console.log('');
        });
      } else {
        console.error(`✗ API request failed with status: ${apiResponse.status()}`);
        sectorData = [];
      }
    } catch (error) {
      console.error(`✗ Error fetching API data: ${error}`);
      sectorData = [];
    }

    // Step 5: Analyze console logs
    console.log('=== CONSOLE LOGS ANALYSIS ===\n');
    const errors = consoleMessages.filter(msg => msg.type() === 'error');
    const warnings = consoleMessages.filter(msg => msg.type() === 'warning');

    if (errors.length > 0) {
      console.log(`Found ${errors.length} console errors:`);
      errors.forEach((err, i) => {
        console.log(`  ${i + 1}. ${err.text()}`);
      });
      console.log('');
    } else {
      console.log('✓ No console errors found\n');
    }

    if (warnings.length > 0) {
      console.log(`Found ${warnings.length} console warnings:`);
      warnings.forEach((warn, i) => {
        console.log(`  ${i + 1}. ${warn.text()}`);
      });
      console.log('');
    } else {
      console.log('✓ No console warnings found\n');
    }

    // Step 6: Generate detailed markdown report
    console.log('Step 6: Generating detailed report...');
    const reportContent = generateReport(sectorData, drawingToolScreenshot, overviewScreenshot, errors, warnings);

    const reportPath = 'VISUAL_SECTOR_COMPARISON_REPORT.md';
    fs.writeFileSync(reportPath, reportContent);
    console.log(`✓ Detailed report saved: ${path.resolve(reportPath)}\n`);

    // Final summary
    console.log('=== TEST SUMMARY ===');
    console.log(`Total sectors in database: ${sectorData.length}`);
    console.log(`Console errors: ${errors.length}`);
    console.log(`Console warnings: ${warnings.length}`);
    console.log(`Drawing Tool screenshot: ${path.resolve(drawingToolScreenshot)}`);
    console.log(`Stadium Overview screenshot: ${path.resolve(overviewScreenshot)}`);
    console.log(`Report: ${path.resolve(reportPath)}`);
    console.log('\n=== TEST COMPLETED ===\n');
  });
});

function generateReport(
  sectors: any[],
  drawingToolPath: string,
  overviewPath: string,
  errors: ConsoleMessage[],
  warnings: ConsoleMessage[]
): string {
  const timestamp = new Date().toISOString();

  let report = `# Visual Sector Comparison Report\n\n`;
  report += `**Generated:** ${timestamp}\n\n`;
  report += `---\n\n`;

  // Screenshots section
  report += `## Screenshots\n\n`;
  report += `### Drawing Tool Page\n`;
  report += `![Drawing Tool](${drawingToolPath})\n`;
  report += `**Path:** \`${path.resolve(drawingToolPath)}\`\n\n`;

  report += `### Stadium Overview Page\n`;
  report += `![Stadium Overview](${overviewPath})\n`;
  report += `**Path:** \`${path.resolve(overviewPath)}\`\n\n`;

  // Database statistics
  report += `---\n\n`;
  report += `## Database Statistics\n\n`;
  report += `**Total Sectors:** ${sectors.length}\n\n`;

  // Shape type distribution
  const shapeTypes = sectors.reduce((acc, sector) => {
    acc[sector.shapeType] = (acc[sector.shapeType] || 0) + 1;
    return acc;
  }, {} as Record<string, number>);

  report += `### Shape Type Distribution\n\n`;
  Object.entries(shapeTypes).forEach(([type, count]) => {
    report += `- **${type}:** ${count} sectors\n`;
  });
  report += `\n`;

  // First 5 sectors with full data
  report += `---\n\n`;
  report += `## First 5 Sectors (Detailed Data)\n\n`;

  sectors.slice(0, 5).forEach((sector, index) => {
    report += `### ${index + 1}. Sector ${sector.sectorCode}\n\n`;
    report += `| Property | Value |\n`;
    report += `|----------|-------|\n`;
    report += `| **Sector Code** | ${sector.sectorCode} |\n`;
    report += `| **Shape Type** | ${sector.shapeType} |\n`;
    report += `| **Top Position** | ${sector.topPercent}% |\n`;
    report += `| **Left Position** | ${sector.leftPercent}% |\n`;
    report += `| **Width** | ${sector.widthPercent}% |\n`;
    report += `| **Height** | ${sector.heightPercent}% |\n`;

    if (sector.shapeData) {
      const shapeDataPreview = sector.shapeData.length > 200
        ? sector.shapeData.substring(0, 200) + '...'
        : sector.shapeData;
      report += `| **Shape Data** | \`${shapeDataPreview}\` |\n`;
    } else {
      report += `| **Shape Data** | *(none)* |\n`;
    }
    report += `\n`;
  });

  // All sectors summary table
  if (sectors.length > 5) {
    report += `---\n\n`;
    report += `## All Sectors Summary\n\n`;
    report += `| # | Sector Code | Shape Type | Position (Top, Left) | Size (W×H) |\n`;
    report += `|---|-------------|------------|---------------------|------------|\n`;

    sectors.forEach((sector, index) => {
      report += `| ${index + 1} | ${sector.sectorCode} | ${sector.shapeType} | `;
      report += `${sector.topPercent}%, ${sector.leftPercent}% | `;
      report += `${sector.widthPercent}%×${sector.heightPercent}% |\n`;
    });
    report += `\n`;
  }

  // Console logs section
  report += `---\n\n`;
  report += `## Console Logs Analysis\n\n`;

  if (errors.length > 0) {
    report += `### ❌ Errors (${errors.length})\n\n`;
    errors.forEach((err, i) => {
      report += `${i + 1}. \`${err.text()}\`\n`;
    });
    report += `\n`;
  } else {
    report += `### ✅ No Console Errors\n\n`;
  }

  if (warnings.length > 0) {
    report += `### ⚠️ Warnings (${warnings.length})\n\n`;
    warnings.forEach((warn, i) => {
      report += `${i + 1}. \`${warn.text()}\`\n`;
    });
    report += `\n`;
  } else {
    report += `### ✅ No Console Warnings\n\n`;
  }

  // Issues and recommendations
  report += `---\n\n`;
  report += `## Findings & Recommendations\n\n`;

  // Check for potential issues
  const issues: string[] = [];

  if (sectors.length === 0) {
    issues.push('⚠️ **No sectors found in database** - Stadium structure may not be imported');
  }

  const sectorsWithoutShapeData = sectors.filter(s => !s.shapeData);
  if (sectorsWithoutShapeData.length > 0) {
    issues.push(`⚠️ **${sectorsWithoutShapeData.length} sectors missing ShapeData** - These may display incorrectly`);
  }

  const invalidPositions = sectors.filter(s =>
    s.topPercent < 0 || s.topPercent > 100 ||
    s.leftPercent < 0 || s.leftPercent > 100
  );
  if (invalidPositions.length > 0) {
    issues.push(`⚠️ **${invalidPositions.length} sectors with invalid positions** - Outside 0-100% range`);
  }

  if (errors.length > 0) {
    issues.push(`❌ **${errors.length} console errors detected** - May indicate rendering or data issues`);
  }

  if (issues.length > 0) {
    report += `### Issues Detected\n\n`;
    issues.forEach(issue => {
      report += `${issue}\n\n`;
    });
  } else {
    report += `### ✅ No Issues Detected\n\n`;
    report += `All sectors have valid data and rendering appears correct.\n\n`;
  }

  // Recommendations
  report += `### Recommendations\n\n`;
  report += `1. **Visual Comparison:** Compare the two screenshots to verify sectors appear in same positions\n`;
  report += `2. **Shape Accuracy:** Verify that complex shapes (polygons, triangles) render identically\n`;
  report += `3. **Coordinate Consistency:** Check that TopPercent/LeftPercent match visual positions\n`;
  report += `4. **Shape Data Validation:** Ensure ShapeData JSON is valid for all custom shapes\n`;
  report += `5. **Console Logs:** Review any errors/warnings for rendering issues\n\n`;

  report += `---\n\n`;
  report += `*Report generated by Playwright visual comparison test*\n`;

  return report;
}
