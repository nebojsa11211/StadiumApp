import { test, expect } from '@playwright/test';

/**
 * FOCUSED POLYGON BUG FIX VERIFICATION
 *
 * Critical Bug Fixes:
 * 1. Rhombus modal triggers after 4th click
 * 2. Custom polygons save with ShapeType "custompolygon" (not "rectangle")
 */

test.describe('Critical Polygon Bug Fixes', () => {
  const timestamp = Date.now();

  test.beforeAll(async () => {
    console.log('\n⏰ Waiting 20 seconds for services to start...\n');
    await new Promise(resolve => setTimeout(resolve, 20000));
  });

  test('Verify Rhombus Modal After 4th Click + Custom Polygon ShapeType', async ({ page, request }) => {
    console.log('\n🔷 CRITICAL BUG FIX VERIFICATION TEST\n');

    // Login
    console.log('1️⃣ Logging in...');
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.screenshot({ path: `bug-fix-01-login.png`, fullPage: true });
    await page.click('button[type="submit"]');

    // Wait for redirect (could be / or /admin/dashboard)
    await page.waitForTimeout(3000);
    const currentUrl = page.url();
    console.log(`   Current URL after login: ${currentUrl}`);
    await page.screenshot({ path: `bug-fix-02-after-login.png`, fullPage: true });

    // Navigate to drawing tool
    console.log('2️⃣ Navigating to Stadium Drawing Tool...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);
    await page.screenshot({ path: `bug-fix-03-drawing-tool.png`, fullPage: true });

    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    await expect(canvas).toBeVisible({ timeout: 5000 });

    // ========================
    // TEST BUG FIX #1: RHOMBUS MODAL AFTER 4TH CLICK
    // ========================
    console.log('\n🔷 BUG FIX #1: Testing Rhombus Modal After 4th Click...');

    const rhombusCode = `RHO-BUG-${timestamp}`;

    // Click the "Create Sector" dropdown to reveal shape options
    const createSectorDropdown = page.locator('button:has-text("Create Sector"), #admin-drawing-tool-create-sector-dropdown');
    await expect(createSectorDropdown).toBeVisible({ timeout: 5000 });
    await createSectorDropdown.click();
    await page.waitForTimeout(500);

    // Click the Rhombus option from dropdown
    const rhombusOption = page.locator('#admin-drawing-tool-rhombus-shape-option');
    await expect(rhombusOption).toBeVisible({ timeout: 3000 });
    await rhombusOption.click();
    await page.waitForTimeout(500);
    console.log('   Selected Rhombus tool');

    // Click 4 points
    const rhombusVertices = [
      { x: 550, y: 200 },
      { x: 750, y: 300 },
      { x: 550, y: 400 },
      { x: 350, y: 300 }
    ];

    for (let i = 0; i < rhombusVertices.length; i++) {
      console.log(`   Click ${i + 1}/4: (${rhombusVertices[i].x}, ${rhombusVertices[i].y})`);
      await canvas.click({ position: rhombusVertices[i] });
      await page.waitForTimeout(400);
    }

    // CRITICAL: Modal should appear after 4th click
    console.log('   🚨 Verifying modal appeared after 4th click...');
    await page.screenshot({ path: `bug-fix-04-rhombus-4-clicks.png`, fullPage: true });

    const modal = page.locator('.modal.show, [role="dialog"]').first();
    await expect(modal).toBeVisible({ timeout: 3000 });
    console.log('   ✅ ✅ ✅ BUG FIX #1 VERIFIED: Modal appeared after 4th click!');
    await page.screenshot({ path: `bug-fix-05-rhombus-modal.png`, fullPage: true });

    // Fill and save rhombus - clear existing values and fill new ones
    console.log('   Filling rhombus form...');

    // Wait for modal to be fully rendered
    await page.waitForTimeout(1000);

    // Find and fill Sector Code field
    const sectorCodeInput = page.locator('.modal.show input').first();
    await sectorCodeInput.click({ clickCount: 3 }); // Select all
    await sectorCodeInput.fill(rhombusCode);

    // Find and fill Sector Name field (second input)
    const sectorNameInput = page.locator('.modal.show input').nth(1);
    await sectorNameInput.click({ clickCount: 3 }); // Select all
    await sectorNameInput.fill('Rhombus Bug Fix Test');

    // Rows and Seats are already filled with acceptable values (10, 20)
    console.log('   Form filled with test data');
    await page.screenshot({ path: `bug-fix-06-rhombus-filled.png`, fullPage: true });

    // Click Save Sector button in modal (the second one shown, inside the modal)
    const saveSectorButton = page.locator('.modal.show button:has-text("Save Sector")');
    await saveSectorButton.click();
    console.log('   Clicked Save Sector button');
    await page.waitForTimeout(3000);

    // Check for success (modal should close and shape should appear)
    await page.screenshot({ path: `bug-fix-07-rhombus-saved.png`, fullPage: true });
    console.log(`   ✅ Rhombus saved: ${rhombusCode}`);

    // ========================
    // TEST BUG FIX #2: CUSTOM POLYGON SHAPETYPE
    // ========================
    console.log('\n⬡ BUG FIX #2: Testing Custom Polygon ShapeType...');

    const hexagonCode = `HEX-BUG-${timestamp}`;

    // Close any open modals first (in case edit modal opened from clicking rhombus)
    const cancelButton = page.locator('button:has-text("Cancel")');
    if (await cancelButton.isVisible()) {
      await cancelButton.click();
      await page.waitForTimeout(1000);
      console.log('   Closed any open modal');
    }

    // Click the "Create Sector" dropdown again
    await createSectorDropdown.click();
    await page.waitForTimeout(500);

    // Click the Custom Polygon option
    const polygonOption = page.locator('#admin-drawing-tool-polygon-shape-option');
    await expect(polygonOption).toBeVisible({ timeout: 3000 });
    await polygonOption.click();
    await page.waitForTimeout(500);
    console.log('   Selected Custom Polygon tool');

    // Click 6 points for hexagon
    const hexagonVertices = [
      { x: 150, y: 500 },
      { x: 300, y: 450 },
      { x: 400, y: 550 },
      { x: 350, y: 650 },
      { x: 200, y: 650 },
      { x: 100, y: 600 }
    ];

    for (let i = 0; i < hexagonVertices.length; i++) {
      console.log(`   Click ${i + 1}/6: (${hexagonVertices[i].x}, ${hexagonVertices[i].y})`);
      await canvas.click({ position: hexagonVertices[i] });
      await page.waitForTimeout(400);
    }

    // Double-click to finish
    console.log('   Double-clicking to finish polygon...');
    await canvas.dblclick({ position: hexagonVertices[hexagonVertices.length - 1] });
    await page.waitForTimeout(1000);
    await page.screenshot({ path: `bug-fix-08-hexagon-drawn.png`, fullPage: true });

    const hexModal = page.locator('.modal.show, [role="dialog"]').first();
    await expect(hexModal).toBeVisible({ timeout: 3000 });
    console.log('   ✅ Modal appeared after double-click');
    await page.screenshot({ path: `bug-fix-09-hexagon-modal.png`, fullPage: true });

    // Fill and save hexagon
    console.log('   Filling hexagon form...');

    // Wait for modal to be fully rendered
    await page.waitForTimeout(1000);

    // Find and fill Sector Code field
    const hexSectorCodeInput = page.locator('.modal.show input').first();
    await hexSectorCodeInput.click({ clickCount: 3 }); // Select all
    await hexSectorCodeInput.fill(hexagonCode);

    // Find and fill Sector Name field (second input)
    const hexSectorNameInput = page.locator('.modal.show input').nth(1);
    await hexSectorNameInput.click({ clickCount: 3 }); // Select all
    await hexSectorNameInput.fill('Hexagon Bug Fix Test');

    // Rows and Seats are already filled
    console.log('   Form filled with test data');
    await page.screenshot({ path: `bug-fix-10-hexagon-filled.png`, fullPage: true });

    // Click Save Sector button in modal
    const hexSaveSectorButton = page.locator('.modal.show button:has-text("Save Sector")');
    await hexSaveSectorButton.click();
    console.log('   Clicked Save Sector button');
    await page.waitForTimeout(3000);

    // Check for success (modal should close and shape should appear)
    await page.screenshot({ path: `bug-fix-11-hexagon-saved.png`, fullPage: true });
    console.log(`   ✅ Hexagon saved: ${hexagonCode}`);

    // ========================
    // DATABASE VERIFICATION
    // ========================
    console.log('\n💾 DATABASE VERIFICATION: Checking ShapeTypes...');

    const response = await request.get('https://localhost:7010/api/StadiumSectorOverlay', {
      ignoreHTTPSErrors: true
    });

    expect(response.ok()).toBeTruthy();
    const sectors = await response.json();
    console.log(`   Retrieved ${sectors.length} total sectors`);

    // Find our test sectors
    const rhombus = sectors.find((s: any) => s.sectorCode === rhombusCode);
    const hexagon = sectors.find((s: any) => s.sectorCode === hexagonCode);

    // Verify Rhombus
    console.log(`\n   RHOMBUS (${rhombusCode}):`);
    expect(rhombus).toBeTruthy();
    console.log(`      ShapeType: ${rhombus.shapeType}`);
    console.log(`      Expected: "rhombus" or enum 4`);
    expect(rhombus.shapeType.toString()).toMatch(/rhombus|4/i);
    console.log('      ✅ Rhombus ShapeType CORRECT');

    // Verify Hexagon - CRITICAL TEST
    console.log(`\n   HEXAGON (${hexagonCode}):`);
    expect(hexagon).toBeTruthy();
    console.log(`      ShapeType: ${hexagon.shapeType}`);
    console.log(`      Expected: "custompolygon" or enum 5 (NOT "rectangle"!)`);

    const isCustomPolygon = hexagon.shapeType.toString().toLowerCase().includes('custompolygon') ||
                           hexagon.shapeType === 5 ||
                           hexagon.shapeType === 'CustomPolygon';

    const isRectangle = hexagon.shapeType.toString().toLowerCase().includes('rectangle') ||
                       hexagon.shapeType === 0;

    if (isRectangle) {
      console.log('      ❌ ❌ ❌ BUG FIX #2 FAILED: ShapeType is "rectangle"');
      throw new Error('BUG FIX #2 FAILED: Hexagon has ShapeType "rectangle" instead of "custompolygon"');
    }

    expect(isCustomPolygon).toBeTruthy();
    console.log('      ✅ ✅ ✅ BUG FIX #2 VERIFIED: ShapeType is "custompolygon"!');

    console.log('\n📊 COMPLETE DATABASE DATA:');
    console.log('\n   RHOMBUS:');
    console.log(JSON.stringify(rhombus, null, 2));
    console.log('\n   HEXAGON:');
    console.log(JSON.stringify(hexagon, null, 2));

    console.log('\n' + '='.repeat(80));
    console.log('🎉 ALL CRITICAL BUG FIXES VERIFIED SUCCESSFULLY!');
    console.log('='.repeat(80));
    console.log('✅ Bug Fix #1: Rhombus modal appears after 4th click');
    console.log('✅ Bug Fix #2: Custom polygons save with ShapeType "custompolygon"');
    console.log('='.repeat(80) + '\n');
  });
});
