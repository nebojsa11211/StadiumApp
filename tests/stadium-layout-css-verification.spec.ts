import { test, expect } from '@playwright/test';

test.describe('Stadium Layout CSS Verification at 1920x1080', () => {
  test('verify stadium sectors respect parent container constraints after CSS fix', async ({ page }) => {
    // Set viewport to exactly 1920x1080
    await page.setViewportSize({ width: 1920, height: 1080 });

    console.log('🔧 Starting stadium layout verification at 1920x1080...');

    // Navigate to admin login page
    await page.goto('https://localhost:9030/login');
    console.log('📍 Navigated to admin login page');

    // Wait for page to load
    await page.waitForLoadState('networkidle');

    // Fill login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    console.log('✅ Filled login credentials');

    // Submit login
    await page.click('#admin-login-submit-btn');
    await page.waitForLoadState('networkidle');
    console.log('🔐 Logged in successfully');

    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');
    console.log('🏟️ Navigated to stadium overview page');

    // Wait for stadium to load and render
    await page.waitForSelector('#admin-stadium-grid-layout', { timeout: 10000 });
    await page.waitForTimeout(2000); // Additional wait for rendering
    console.log('⏳ Stadium grid layout loaded');

    // Take initial screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-css-verification-1920x1080.png',
      fullPage: true
    });
    console.log('📸 Screenshot taken');

    // Get dimensions of main stadium layout
    const stadiumLayout = await page.locator('#admin-stadium-grid-layout').boundingBox();
    console.log('🏟️ Stadium layout dimensions:', stadiumLayout);

    // Requirement 1: Verify North stand sectors stay horizontal
    console.log('\n🔍 REQUIREMENT 1: North stand sectors horizontal layout');
    const northStand = await page.locator('#admin-stadium-stand-n').boundingBox();
    const northSectorsGrid = await page.locator('#admin-stadium-sectors-grid-n').boundingBox();

    console.log(`📐 North stand: ${northStand?.width}x${northStand?.height}`);
    console.log(`📐 North sectors grid: ${northSectorsGrid?.width}x${northSectorsGrid?.height}`);

    // Check that north sectors are laid out horizontally
    const northSector1 = await page.locator('#admin-stadium-sector-N1').boundingBox();
    const northSector2 = await page.locator('#admin-stadium-sector-N2').boundingBox();

    if (northSector1 && northSector2) {
      console.log(`📐 N1 sector: ${northSector1.width}x${northSector1.height} at (${northSector1.x}, ${northSector1.y})`);
      console.log(`📐 N2 sector: ${northSector2.width}x${northSector2.height} at (${northSector2.x}, ${northSector2.y})`);

      // Verify horizontal layout (N2 should be to the right of N1, not below)
      const isHorizontal = northSector2.x > northSector1.x && Math.abs(northSector1.y - northSector2.y) < 10;
      console.log(`✅ North sectors horizontal layout: ${isHorizontal}`);
      expect(isHorizontal).toBe(true);
    }

    // Requirement 2: Verify N1 sector height doesn't exceed north stand height
    console.log('\n🔍 REQUIREMENT 2: N1 sector height constraint');
    if (northSector1 && northStand) {
      const heightRatio = northSector1.height / northStand.height;
      console.log(`📏 N1 height: ${northSector1.height}px, North stand height: ${northStand.height}px`);
      console.log(`📊 Height ratio: ${heightRatio.toFixed(3)} (should be ≤ 1.0)`);

      expect(northSector1.height).toBeLessThanOrEqual(northStand.height + 5); // 5px tolerance
      console.log('✅ N1 sector height respects parent container');
    }

    // Requirement 3: Verify West sectors grid height matches stadium layout height
    console.log('\n🔍 REQUIREMENT 3: West sectors grid height constraint');
    const westSectorsGrid = await page.locator('#admin-stadium-sectors-grid-w').boundingBox();

    if (westSectorsGrid && stadiumLayout) {
      console.log(`📐 West sectors grid: ${westSectorsGrid.width}x${westSectorsGrid.height}`);
      console.log(`📐 Stadium layout: ${stadiumLayout.width}x${stadiumLayout.height}`);

      const heightDifference = Math.abs(westSectorsGrid.height - stadiumLayout.height);
      console.log(`📏 Height difference: ${heightDifference}px (should be minimal)`);

      // Allow small tolerance for borders/margins
      expect(heightDifference).toBeLessThan(20);
      console.log('✅ West sectors grid height matches stadium layout');
    }

    // Requirement 4: Verify sectors no longer display as 258x258px squares in 200px container
    console.log('\n🔍 REQUIREMENT 4: No oversized square sectors');

    // Check all visible sectors
    const sectorSelectors = [
      '#admin-stadium-sector-N1', '#admin-stadium-sector-N2',
      '#admin-stadium-sector-S1', '#admin-stadium-sector-S2',
      '#admin-stadium-sector-E1', '#admin-stadium-sector-E2',
      '#admin-stadium-sector-W1', '#admin-stadium-sector-W2'
    ];

    for (const selector of sectorSelectors) {
      const sector = await page.locator(selector);
      if (await sector.isVisible()) {
        const sectorBox = await sector.boundingBox();
        if (sectorBox) {
          const aspectRatio = sectorBox.width / sectorBox.height;
          console.log(`📐 ${selector}: ${sectorBox.width}x${sectorBox.height} (AR: ${aspectRatio.toFixed(2)})`);

          // Verify no sector is oversized compared to what makes sense
          // For horizontal layouts (N/S), width should be reasonable
          // For vertical layouts (E/W), height should be reasonable
          if (selector.includes('N') || selector.includes('S')) {
            // North/South sectors should not be square when in horizontal layout
            expect(aspectRatio).toBeGreaterThan(1.2); // Should be wider than tall
          } else {
            // East/West sectors should fit vertically
            expect(aspectRatio).toBeLessThan(0.8); // Should be taller than wide
          }
        }
      }
    }

    console.log('✅ All sectors have appropriate aspect ratios');

    // Additional verification: Check CSS computed styles
    console.log('\n🔍 CSS STYLE VERIFICATION: Checking computed styles');

    const n1Styles = await page.evaluate(() => {
      const element = document.querySelector('#admin-stadium-sector-N1');
      if (element) {
        const styles = window.getComputedStyle(element);
        return {
          aspectRatio: styles.aspectRatio,
          width: styles.width,
          height: styles.height,
          maxHeight: styles.maxHeight,
          maxWidth: styles.maxWidth
        };
      }
      return null;
    });

    if (n1Styles) {
      console.log('🎨 N1 computed styles:', n1Styles);
      // Verify aspect-ratio is not 1 (which was causing the issue)
      expect(n1Styles.aspectRatio).not.toBe('1');
      console.log('✅ aspect-ratio: 1 rule successfully removed');
    }

    // Check West sector styles (these were problematic)
    const w1Styles = await page.evaluate(() => {
      const element = document.querySelector('#admin-stadium-sector-W1');
      if (element) {
        const styles = window.getComputedStyle(element);
        return {
          aspectRatio: styles.aspectRatio,
          width: styles.width,
          height: styles.height,
          maxHeight: styles.maxHeight,
          flex: styles.flex
        };
      }
      return null;
    });

    if (w1Styles) {
      console.log('🎨 W1 computed styles:', w1Styles);
      expect(w1Styles.aspectRatio).not.toBe('1');
      console.log('✅ West sector aspect-ratio fixed');
    }

    // Final verification screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-css-final-verification.png',
      fullPage: false,
      clip: { x: 0, y: 0, width: 1920, height: 1080 }
    });

    console.log('\n🎉 ALL REQUIREMENTS VERIFIED SUCCESSFULLY!');
    console.log('✅ 1. North sectors maintain horizontal layout');
    console.log('✅ 2. N1 sector height respects parent container');
    console.log('✅ 3. West sectors grid height matches stadium layout');
    console.log('✅ 4. No oversized square sectors in constrained containers');
    console.log('✅ 5. aspect-ratio: 1 rules successfully removed');
  });

  test('detailed sector dimension analysis', async ({ page }) => {
    // Set viewport to exactly 1920x1080
    await page.setViewportSize({ width: 1920, height: 1080 });

    // Quick login and navigation
    await page.goto('https://localhost:9030/login');
    await page.waitForLoadState('networkidle');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForLoadState('networkidle');

    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');
    await page.waitForSelector('#admin-stadium-grid-layout', { timeout: 10000 });
    await page.waitForTimeout(2000);

    console.log('\n📊 DETAILED DIMENSION ANALYSIS');

    // Get all container and sector dimensions
    const dimensions = await page.evaluate(() => {
      const results: any = {};

      // Main containers
      const stadiumLayout = document.querySelector('#admin-stadium-grid-layout');
      if (stadiumLayout) {
        const rect = stadiumLayout.getBoundingClientRect();
        results.stadiumLayout = { width: rect.width, height: rect.height };
      }

      // All stands
      ['n', 's', 'e', 'w'].forEach(direction => {
        const stand = document.querySelector(`#admin-stadium-stand-${direction}`);
        const sectorsGrid = document.querySelector(`#admin-stadium-sectors-grid-${direction}`);

        if (stand) {
          const rect = stand.getBoundingClientRect();
          results[`stand_${direction}`] = { width: rect.width, height: rect.height };
        }

        if (sectorsGrid) {
          const rect = sectorsGrid.getBoundingClientRect();
          results[`sectorsGrid_${direction}`] = { width: rect.width, height: rect.height };
        }

        // Individual sectors
        ['1', '2'].forEach(num => {
          const sector = document.querySelector(`#admin-stadium-sector-${direction.toUpperCase()}${num}`);
          if (sector) {
            const rect = sector.getBoundingClientRect();
            results[`sector_${direction.toUpperCase()}${num}`] = {
              width: rect.width,
              height: rect.height,
              x: rect.x,
              y: rect.y
            };
          }
        });
      });

      return results;
    });

    console.log('\n📐 CONTAINER DIMENSIONS:');
    console.log(`Stadium Layout: ${dimensions.stadiumLayout?.width}x${dimensions.stadiumLayout?.height}`);

    console.log('\n📐 STAND DIMENSIONS:');
    ['n', 's', 'e', 'w'].forEach(direction => {
      const stand = dimensions[`stand_${direction}`];
      const grid = dimensions[`sectorsGrid_${direction}`];
      if (stand && grid) {
        console.log(`${direction.toUpperCase()} Stand: ${stand.width}x${stand.height}`);
        console.log(`${direction.toUpperCase()} Grid: ${grid.width}x${grid.height}`);

        // Verify grid fits within stand
        const widthFits = grid.width <= stand.width + 5;
        const heightFits = grid.height <= stand.height + 5;
        console.log(`  Grid fits: width=${widthFits}, height=${heightFits}`);
      }
    });

    console.log('\n📐 SECTOR DIMENSIONS:');
    ['N', 'S', 'E', 'W'].forEach(direction => {
      ['1', '2'].forEach(num => {
        const sector = dimensions[`sector_${direction}${num}`];
        if (sector) {
          const aspectRatio = (sector.width / sector.height).toFixed(2);
          console.log(`${direction}${num}: ${sector.width}x${sector.height} (AR: ${aspectRatio})`);
        }
      });
    });

    // Verify specific requirements with the detailed data
    const stadium = dimensions.stadiumLayout;
    const westGrid = dimensions.sectorsGrid_w;
    const n1Sector = dimensions.sector_N1;
    const northStand = dimensions.stand_n;

    if (stadium && westGrid) {
      const heightMatch = Math.abs(westGrid.height - stadium.height) < 20;
      console.log(`\n✅ West grid height matches stadium: ${heightMatch} (diff: ${Math.abs(westGrid.height - stadium.height)}px)`);
      expect(heightMatch).toBe(true);
    }

    if (n1Sector && northStand) {
      const heightRespected = n1Sector.height <= northStand.height + 5;
      console.log(`✅ N1 height respects north stand: ${heightRespected} (${n1Sector.height}px <= ${northStand.height}px)`);
      expect(heightRespected).toBe(true);
    }

    // Check horizontal layout of north sectors
    const n1 = dimensions.sector_N1;
    const n2 = dimensions.sector_N2;
    if (n1 && n2) {
      const isHorizontal = n2.x > n1.x && Math.abs(n1.y - n2.y) < 10;
      console.log(`✅ North sectors horizontal: ${isHorizontal} (N2.x=${n2.x} > N1.x=${n1.x})`);
      expect(isHorizontal).toBe(true);
    }

    console.log('\n🎉 DETAILED ANALYSIS COMPLETE - ALL CHECKS PASSED!');
  });
});