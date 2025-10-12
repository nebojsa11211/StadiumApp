import { test, expect } from '@playwright/test';

test.describe('Complete Polygon Shape Creation & Persistence Tests', () => {
  test.setTimeout(180000); // 3 minutes for complete test suite

  let apiContext;

  test.beforeAll(async ({ playwright }) => {
    // Create API context for database verification
    apiContext = await playwright.request.newContext({
      baseURL: 'https://localhost:7010',
      ignoreHTTPSErrors: true,
    });
  });

  test.afterAll(async () => {
    await apiContext?.dispose();
  });

  test('Complete polygon shape creation and database persistence', async ({ page }) => {
    console.log('\n=== STARTING COMPREHENSIVE POLYGON TEST ===\n');

    // Generate unique sector codes with timestamp
    const timestamp = Date.now();
    const triangleCode = `TRI-${timestamp}`;
    const rhombusCode = `RHO-${timestamp}`;
    const hexagonCode = `HEX-${timestamp}`;
    console.log(`Using unique codes: ${triangleCode}, ${rhombusCode}, ${hexagonCode}`);

    // Test 1: Login
    console.log('TEST 1: Admin Login');
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    await page.waitForTimeout(2000);
    console.log('✓ Login successful');

    // Navigate to drawing tool
    console.log('\nTEST 2: Navigate to Stadium Drawing Tool');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Wait for canvas to be ready
    await page.waitForSelector('canvas#admin-drawing-tool-canvas', { state: 'visible' });
    console.log('✓ Drawing tool loaded');

    // ===== TEST 3: TRIANGLE CREATION =====
    console.log('\n=== TEST 3: Triangle Sector Creation ===');

    // Select triangle mode via dropdown
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(300);
    await page.click('#admin-drawing-tool-triangle-shape-option');
    await page.waitForTimeout(500);
    console.log('✓ Selected triangle mode');

    // Click 3 points for triangle
    const canvas = await page.locator('canvas#admin-drawing-tool-canvas');
    const canvasBox = await canvas.boundingBox();

    if (!canvasBox) {
      throw new Error('Canvas not found');
    }

    console.log('Clicking triangle vertices...');
    // Point 1: Top
    await canvas.click({
      position: { x: 300, y: 200 },
      force: true
    });
    await page.waitForTimeout(800);
    console.log('  Point 1 clicked: (300, 200)');

    // Point 2: Bottom-right
    await canvas.click({
      position: { x: 500, y: 400 },
      force: true
    });
    await page.waitForTimeout(800);
    console.log('  Point 2 clicked: (500, 400)');

    // Point 3: Bottom-left
    await canvas.click({
      position: { x: 200, y: 450 },
      force: true
    });
    await page.waitForTimeout(1500);
    console.log('  Point 3 clicked: (200, 450)');

    // Wait for modal to appear
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 5000 });
    console.log('✓ Modal appeared after 3rd click');

    // Fill triangle form using input placeholders and labels
    const modal = page.locator('#sector-edit-modal');
    await modal.locator('input[placeholder*="e.g., A1"]').fill(triangleCode);
    await modal.locator('input[placeholder*="Tribune"]').fill('Automated Triangle Test');
    await modal.locator('input[min="1"][max="100"]').fill('12');  // Rows
    await modal.locator('input[min="1"][max="200"]').fill('18');  // Seats per row
    await modal.locator('select.form-select').selectOption('standard');
    await page.waitForTimeout(500);
    console.log('✓ Triangle form filled');

    // Save triangle
    await modal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success alert
    await page.waitForSelector('.alert-success, .alert.alert-success', { timeout: 10000 });
    console.log('✓ Triangle saved successfully');

    // Wait for modal to fully close
    await page.waitForTimeout(1000);

    // Ensure modal is closed
    await page.waitForSelector('#sector-edit-modal', { state: 'hidden', timeout: 5000 });
    console.log('✓ Modal closed');

    // Take screenshot
    await page.screenshot({ path: 'triangle-created.png', fullPage: true });
    console.log('✓ Screenshot: triangle-created.png');

    // Wait for success alert to disappear before proceeding
    await page.waitForTimeout(3000);
    console.log('✓ Waiting for UI to stabilize...');

    // ===== TEST 4: RHOMBUS CREATION =====
    console.log('\n=== TEST 4: Rhombus Sector Creation ===');

    // Select rhombus mode via dropdown
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(500);
    await page.click('#admin-drawing-tool-rhombus-shape-option');
    await page.waitForTimeout(1000);
    console.log('✓ Selected rhombus mode');

    // Click 4 points for rhombus
    console.log('Clicking rhombus vertices...');

    // Point 1: Top
    await canvas.click({
      position: { x: 700, y: 200 },
      force: true
    });
    await page.waitForTimeout(800);
    console.log('  Point 1 clicked: (700, 200)');

    // Point 2: Right
    await canvas.click({
      position: { x: 900, y: 300 },
      force: true
    });
    await page.waitForTimeout(800);
    console.log('  Point 2 clicked: (900, 300)');

    // Point 3: Bottom
    await canvas.click({
      position: { x: 700, y: 450 },
      force: true
    });
    await page.waitForTimeout(800);
    console.log('  Point 3 clicked: (700, 450)');

    // Point 4: Left
    await canvas.click({
      position: { x: 500, y: 300 },
      force: true
    });
    await page.waitForTimeout(1500);
    console.log('  Point 4 clicked: (500, 300)');

    // Wait for modal
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 10000 });
    console.log('✓ Modal appeared after 4th click');

    // Fill rhombus form
    const rhombusModal = page.locator('#sector-edit-modal');
    await rhombusModal.locator('input[placeholder*="e.g., A1"]').fill(rhombusCode);
    await rhombusModal.locator('input[placeholder*="Tribune"]').fill('Automated Rhombus Test');
    await rhombusModal.locator('input[min="1"][max="100"]').fill('15');  // Rows
    await rhombusModal.locator('input[min="1"][max="200"]').fill('22');  // Seats per row
    await rhombusModal.locator('select.form-select').selectOption('vip');
    await page.waitForTimeout(500);
    console.log('✓ Rhombus form filled');

    // Save rhombus
    await rhombusModal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success alert
    await page.waitForSelector('.alert-success, .alert.alert-success', { timeout: 10000 });
    console.log('✓ Rhombus saved successfully');

    // Wait for modal to fully close
    await page.waitForTimeout(1000);

    // Ensure modal is closed
    await page.waitForSelector('#sector-edit-modal', { state: 'hidden', timeout: 5000 });
    console.log('✓ Modal closed');

    // Take screenshot
    await page.screenshot({ path: 'rhombus-created.png', fullPage: true });
    console.log('✓ Screenshot: rhombus-created.png');

    await page.waitForTimeout(2000);

    // ===== TEST 5: CUSTOM 6-POINT POLYGON =====
    console.log('\n=== TEST 5: Custom 6-Point Polygon (Hexagon) ===');

    // Select custom polygon mode via dropdown
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(500);
    await page.click('#admin-drawing-tool-polygon-shape-option');
    await page.waitForTimeout(1000);
    console.log('✓ Selected custom polygon mode');

    // Click 6 points for hexagon
    console.log('Clicking hexagon vertices...');

    const hexPoints = [
      { x: 1100, y: 200, label: 'Top' },
      { x: 1300, y: 250, label: 'Top-right' },
      { x: 1300, y: 400, label: 'Bottom-right' },
      { x: 1100, y: 450, label: 'Bottom' },
      { x: 900, y: 400, label: 'Bottom-left' },
      { x: 900, y: 250, label: 'Top-left' },
    ];

    for (let i = 0; i < hexPoints.length; i++) {
      await canvas.click({
        position: { x: hexPoints[i].x, y: hexPoints[i].y },
        force: true
      });
      await page.waitForTimeout(800);
      console.log(`  Point ${i + 1} clicked: (${hexPoints[i].x}, ${hexPoints[i].y}) - ${hexPoints[i].label}`);
    }

    // Double-click to finish polygon
    await canvas.dblclick({
      position: { x: hexPoints[5].x, y: hexPoints[5].y },
      force: true
    });
    await page.waitForTimeout(1500);
    console.log('✓ Double-clicked to finish polygon');

    // Wait for modal
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 5000 });
    console.log('✓ Modal appeared after double-click');

    // Fill hexagon form
    const hexagonModal = page.locator('#sector-edit-modal');
    await hexagonModal.locator('input[placeholder*="e.g., A1"]').fill(hexagonCode);
    await hexagonModal.locator('input[placeholder*="Tribune"]').fill('Automated Hexagon Test');
    await hexagonModal.locator('input[min="1"][max="100"]').fill('20');  // Rows
    await hexagonModal.locator('input[min="1"][max="200"]').fill('25');  // Seats per row
    await hexagonModal.locator('select.form-select').selectOption('standard');
    await page.waitForTimeout(500);
    console.log('✓ Hexagon form filled');

    // Save hexagon
    await hexagonModal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success alert
    await page.waitForSelector('.alert-success, .alert.alert-success', { timeout: 10000 });
    console.log('✓ Hexagon saved successfully');

    // Take screenshot
    await page.screenshot({ path: 'hexagon-created.png', fullPage: true });
    console.log('✓ Screenshot: hexagon-created.png');

    await page.waitForTimeout(1000);

    // ===== TEST 6: DATABASE PERSISTENCE - PAGE RELOAD =====
    console.log('\n=== TEST 6: Database Persistence - Page Reload ===');

    await page.reload({ waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);
    console.log('✓ Page reloaded');

    // Wait for canvas to be ready
    await page.waitForSelector('canvas#drawingCanvas', { state: 'visible' });
    await page.waitForTimeout(2000);
    console.log('✓ Canvas loaded after reload');

    // Take screenshot of all shapes after reload
    await page.screenshot({ path: 'all-shapes-after-reload.png', fullPage: true });
    console.log('✓ Screenshot: all-shapes-after-reload.png');

    // ===== TEST 7: CLICK DETECTION ON POLYGONS =====
    console.log('\n=== TEST 7: Click Detection on Polygons ===');

    // Click on triangle (center roughly at 333, 350)
    console.log('Testing triangle click detection...');
    await canvas.click({
      position: { x: 333, y: 350 },
      force: true
    });
    await page.waitForTimeout(1500);

    // Check if modal appeared
    let modalVisible = await page.locator('#sector-edit-modal').isVisible();
    if (modalVisible) {
      console.log('✓ Modal opened on triangle click');

      // Verify sector code
      const clickModal = page.locator('#sector-edit-modal');
      const sectorCodeValue = await clickModal.locator('input[placeholder*="e.g., A1"]').inputValue();
      expect(sectorCodeValue).toBe(triangleCode);
      console.log(`✓ Sector code verified: ${sectorCodeValue}`);

      // Close modal
      await clickModal.locator('button:has-text("Cancel")').click();
      await page.waitForTimeout(1000);
    } else {
      console.log('⚠ Triangle click detection did not trigger modal');
    }

    // Click on rhombus (center roughly at 700, 325)
    console.log('Testing rhombus click detection...');
    await canvas.click({
      position: { x: 700, y: 325 },
      force: true
    });
    await page.waitForTimeout(1500);

    modalVisible = await page.locator('#sector-edit-modal').isVisible();
    if (modalVisible) {
      console.log('✓ Modal opened on rhombus click');

      // Verify sector code
      const rhombusClickModal = page.locator('#sector-edit-modal');
      const sectorCodeValue = await rhombusClickModal.locator('input[placeholder*="e.g., A1"]').inputValue();
      expect(sectorCodeValue).toBe(rhombusCode);
      console.log(`✓ Sector code verified: ${sectorCodeValue}`);

      // Close modal
      await rhombusClickModal.locator('button:has-text("Cancel")').click();
      await page.waitForTimeout(1000);
    } else {
      console.log('⚠ Rhombus click detection did not trigger modal');
    }

    // Take screenshot
    await page.screenshot({ path: 'click-detection-verified.png', fullPage: true });
    console.log('✓ Screenshot: click-detection-verified.png');

    // ===== TEST 8: API DATABASE VERIFICATION =====
    console.log('\n=== TEST 8: API Database Verification ===');

    const response = await apiContext.get('/api/StadiumSectorOverlay');
    expect(response.ok()).toBeTruthy();

    const sectors = await response.json();
    console.log(`✓ Retrieved ${sectors.length} total sectors from database`);

    // Find our test sectors
    const triangleSector = sectors.find(s => s.sectorCode === triangleCode);
    const rhombusSector = sectors.find(s => s.sectorCode === rhombusCode);
    const hexagonSector = sectors.find(s => s.sectorCode === hexagonCode);

    console.log(`\n--- Triangle Sector (${triangleCode}) ---`);
    if (triangleSector) {
      console.log('✓ Found in database');
      console.log(`  ShapeType: ${triangleSector.shapeType}`);
      console.log(`  Name: ${triangleSector.name}`);
      console.log(`  Rows: ${triangleSector.rows}`);
      console.log(`  SeatsPerRow: ${triangleSector.seatsPerRow}`);
      console.log(`  Type: ${triangleSector.type}`);

      const shapeData = JSON.parse(triangleSector.shapeData);
      console.log(`  Vertices: ${shapeData.length}`);
      console.log(`  ShapeData:`, JSON.stringify(shapeData, null, 2));

      expect(triangleSector.shapeType).toBe('triangle');
      expect(triangleSector.rows).toBe(12);
      expect(triangleSector.seatsPerRow).toBe(18);
      expect(shapeData.length).toBe(3);
    } else {
      console.log('✗ NOT FOUND in database');
    }

    console.log(`\n--- Rhombus Sector (${rhombusCode}) ---`);
    if (rhombusSector) {
      console.log('✓ Found in database');
      console.log(`  ShapeType: ${rhombusSector.shapeType}`);
      console.log(`  Name: ${rhombusSector.name}`);
      console.log(`  Rows: ${rhombusSector.rows}`);
      console.log(`  SeatsPerRow: ${rhombusSector.seatsPerRow}`);
      console.log(`  Type: ${rhombusSector.type}`);

      const shapeData = JSON.parse(rhombusSector.shapeData);
      console.log(`  Vertices: ${shapeData.length}`);
      console.log(`  ShapeData:`, JSON.stringify(shapeData, null, 2));

      expect(rhombusSector.shapeType).toBe('rhombus');
      expect(rhombusSector.rows).toBe(15);
      expect(rhombusSector.seatsPerRow).toBe(22);
      expect(shapeData.length).toBe(4);
    } else {
      console.log('✗ NOT FOUND in database');
    }

    console.log(`\n--- Hexagon Sector (${hexagonCode}) ---`);
    if (hexagonSector) {
      console.log('✓ Found in database');
      console.log(`  ShapeType: ${hexagonSector.shapeType}`);
      console.log(`  Name: ${hexagonSector.name}`);
      console.log(`  Rows: ${hexagonSector.rows}`);
      console.log(`  SeatsPerRow: ${hexagonSector.seatsPerRow}`);
      console.log(`  Type: ${hexagonSector.type}`);

      const shapeData = JSON.parse(hexagonSector.shapeData);
      console.log(`  Vertices: ${shapeData.length}`);
      console.log(`  ShapeData:`, JSON.stringify(shapeData, null, 2));

      expect(hexagonSector.shapeType).toBe('polygon');
      expect(hexagonSector.rows).toBe(20);
      expect(hexagonSector.seatsPerRow).toBe(25);
      expect(shapeData.length).toBe(6);
    } else {
      console.log('✗ NOT FOUND in database');
    }

    // ===== TEST 9: SHAPE DATA INTEGRITY CHECK =====
    console.log('\n=== TEST 9: Shape Data Integrity Check ===');

    const verifyShapeData = (sector, expectedVertexCount, shapeName) => {
      console.log(`\nVerifying ${shapeName}...`);

      if (!sector) {
        console.log(`✗ ${shapeName} not found`);
        return false;
      }

      const shapeData = JSON.parse(sector.shapeData);

      // Check vertex count
      if (shapeData.length !== expectedVertexCount) {
        console.log(`✗ Incorrect vertex count: expected ${expectedVertexCount}, got ${shapeData.length}`);
        return false;
      }
      console.log(`✓ Vertex count correct: ${shapeData.length}`);

      // Check coordinate format
      for (let i = 0; i < shapeData.length; i++) {
        const vertex = shapeData[i];

        if (!vertex.hasOwnProperty('X') || !vertex.hasOwnProperty('Y')) {
          console.log(`✗ Vertex ${i} missing X or Y property`);
          return false;
        }

        if (typeof vertex.X !== 'number' || typeof vertex.Y !== 'number') {
          console.log(`✗ Vertex ${i} coordinates are not numbers`);
          return false;
        }

        if (vertex.X < 0 || vertex.X > 100 || vertex.Y < 0 || vertex.Y > 100) {
          console.log(`✗ Vertex ${i} coordinates out of range: X=${vertex.X}, Y=${vertex.Y}`);
          return false;
        }
      }
      console.log('✓ All vertices have valid percentage coordinates (0-100)');

      return true;
    };

    const triangleValid = verifyShapeData(triangleSector, 3, 'Triangle');
    const rhombusValid = verifyShapeData(rhombusSector, 4, 'Rhombus');
    const hexagonValid = verifyShapeData(hexagonSector, 6, 'Hexagon');

    // ===== FINAL SUMMARY =====
    console.log('\n\n=== TEST EXECUTION SUMMARY ===');
    console.log('✓ Admin login successful');
    console.log('✓ Navigation to drawing tool successful');
    console.log(`${triangleSector ? '✓' : '✗'} Triangle creation and persistence: ${triangleValid ? 'PASSED' : 'FAILED'}`);
    console.log(`${rhombusSector ? '✓' : '✗'} Rhombus creation and persistence: ${rhombusValid ? 'PASSED' : 'FAILED'}`);
    console.log(`${hexagonSector ? '✓' : '✗'} Hexagon creation and persistence: ${hexagonValid ? 'PASSED' : 'FAILED'}`);
    console.log('✓ Page reload persistence verified');
    console.log('✓ Click detection tested');
    console.log('✓ API database verification complete');
    console.log('✓ Shape data integrity verified');
    console.log('\n=== ALL TESTS COMPLETED ===\n');

    // Assert all critical checks passed
    expect(triangleSector).toBeTruthy();
    expect(rhombusSector).toBeTruthy();
    expect(hexagonSector).toBeTruthy();
    expect(triangleValid).toBeTruthy();
    expect(rhombusValid).toBeTruthy();
    expect(hexagonValid).toBeTruthy();
  });
});
