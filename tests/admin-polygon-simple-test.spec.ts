import { test, expect } from '@playwright/test';

test.describe('Simple Polygon Shape Tests (Individual)', () => {
  test.setTimeout(120000); // 2 minutes per test

  let apiContext;

  test.beforeAll(async ({ playwright }) => {
    apiContext = await playwright.request.newContext({
      baseURL: 'https://localhost:7010',
      ignoreHTTPSErrors: true,
    });
  });

  test.afterAll(async () => {
    await apiContext?.dispose();
  });

  // Helper function for login
  async function loginAsAdmin(page) {
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(2000);
  }

  test('Create Triangle Sector', async ({ page }) => {
    const sectorCode = `TRI-${Date.now()}`;
    console.log(`\n=== TRIANGLE TEST (Code: ${sectorCode}) ===`);

    // Login
    await loginAsAdmin(page);

    // Navigate to drawing tool
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);
    await page.waitForSelector('canvas#admin-drawing-tool-canvas', { state: 'visible' });

    // Select triangle mode
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(300);
    await page.click('#admin-drawing-tool-triangle-shape-option');
    await page.waitForTimeout(1000);
    console.log('✓ Triangle mode selected');

    // Click 3 vertices
    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    await canvas.click({ position: { x: 300, y: 200 }, force: true });
    await page.waitForTimeout(800);
    console.log('  Point 1: (300, 200)');

    await canvas.click({ position: { x: 500, y: 400 }, force: true });
    await page.waitForTimeout(800);
    console.log('  Point 2: (500, 400)');

    await canvas.click({ position: { x: 200, y: 450 }, force: true });
    await page.waitForTimeout(1500);
    console.log('  Point 3: (200, 450)');

    // Wait for modal
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 10000 });
    console.log('✓ Modal appeared');

    // Fill form
    const modal = page.locator('#sector-edit-modal');
    await modal.locator('input[placeholder*="e.g., A1"]').fill(sectorCode);
    await modal.locator('input[placeholder*="Tribune"]').fill('Test Triangle');
    await modal.locator('input[min="1"][max="100"]').fill('10');
    await modal.locator('input[min="1"][max="200"]').fill('15');
    await modal.locator('select.form-select').selectOption('standard');
    await page.waitForTimeout(500);
    console.log('✓ Form filled');

    // Save
    await modal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success
    await page.waitForSelector('.alert-success', { timeout: 10000 });
    console.log('✓ Triangle saved successfully');

    // Screenshot
    await page.screenshot({ path: `triangle-${sectorCode}.png`, fullPage: true });

    // Verify in database
    const response = await apiContext.get('/api/StadiumSectorOverlay');
    expect(response.ok()).toBeTruthy();
    const sectors = await response.json();
    const triangleSector = sectors.find(s => s.sectorCode === sectorCode);

    expect(triangleSector).toBeTruthy();
    expect(triangleSector.shapeType).toBe('triangle');
    const shapeData = JSON.parse(triangleSector.shapeData);
    expect(shapeData.length).toBe(3);
    console.log(`✓ Database verification: ${sectorCode} - 3 vertices confirmed`);
  });

  test('Create Rhombus Sector', async ({ page }) => {
    const sectorCode = `RHO-${Date.now()}`;
    console.log(`\n=== RHOMBUS TEST (Code: ${sectorCode}) ===`);

    // Login
    await loginAsAdmin(page);

    // Navigate to drawing tool
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);
    await page.waitForSelector('canvas#admin-drawing-tool-canvas', { state: 'visible' });

    // Select rhombus mode
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(300);
    await page.click('#admin-drawing-tool-rhombus-shape-option');
    await page.waitForTimeout(1000);
    console.log('✓ Rhombus mode selected');

    // Click 4 vertices
    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    await canvas.click({ position: { x: 700, y: 200 }, force: true });
    await page.waitForTimeout(800);
    console.log('  Point 1: (700, 200)');

    await canvas.click({ position: { x: 900, y: 300 }, force: true });
    await page.waitForTimeout(800);
    console.log('  Point 2: (900, 300)');

    await canvas.click({ position: { x: 700, y: 450 }, force: true });
    await page.waitForTimeout(800);
    console.log('  Point 3: (700, 450)');

    await canvas.click({ position: { x: 500, y: 300 }, force: true });
    await page.waitForTimeout(1500);
    console.log('  Point 4: (500, 300)');

    // Wait for modal
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 10000 });
    console.log('✓ Modal appeared');

    // Fill form
    const modal = page.locator('#sector-edit-modal');
    await modal.locator('input[placeholder*="e.g., A1"]').fill(sectorCode);
    await modal.locator('input[placeholder*="Tribune"]').fill('Test Rhombus');
    await modal.locator('input[min="1"][max="100"]').fill('12');
    await modal.locator('input[min="1"][max="200"]').fill('20');
    await modal.locator('select.form-select').selectOption('vip');
    await page.waitForTimeout(500);
    console.log('✓ Form filled');

    // Save
    await modal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success
    await page.waitForSelector('.alert-success', { timeout: 10000 });
    console.log('✓ Rhombus saved successfully');

    // Screenshot
    await page.screenshot({ path: `rhombus-${sectorCode}.png`, fullPage: true });

    // Verify in database
    const response = await apiContext.get('/api/StadiumSectorOverlay');
    expect(response.ok()).toBeTruthy();
    const sectors = await response.json();
    const rhombusSector = sectors.find(s => s.sectorCode === sectorCode);

    expect(rhombusSector).toBeTruthy();
    expect(rhombusSector.shapeType).toBe('rhombus');
    const shapeData = JSON.parse(rhombusSector.shapeData);
    expect(shapeData.length).toBe(4);
    console.log(`✓ Database verification: ${sectorCode} - 4 vertices confirmed`);
  });

  test('Create Hexagon (Custom Polygon)', async ({ page }) => {
    const sectorCode = `HEX-${Date.now()}`;
    console.log(`\n=== HEXAGON TEST (Code: ${sectorCode}) ===`);

    // Login
    await loginAsAdmin(page);

    // Navigate to drawing tool
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);
    await page.waitForSelector('canvas#admin-drawing-tool-canvas', { state: 'visible' });

    // Select polygon mode
    await page.click('#admin-drawing-tool-shape-dropdown-toggle');
    await page.waitForTimeout(300);
    await page.click('#admin-drawing-tool-polygon-shape-option');
    await page.waitForTimeout(1000);
    console.log('✓ Polygon mode selected');

    // Click 6 vertices
    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    const hexPoints = [
      { x: 1100, y: 200 },
      { x: 1300, y: 250 },
      { x: 1300, y: 400 },
      { x: 1100, y: 450 },
      { x: 900, y: 400 },
      { x: 900, y: 250 },
    ];

    for (let i = 0; i < hexPoints.length; i++) {
      await canvas.click({ position: hexPoints[i], force: true });
      await page.waitForTimeout(800);
      console.log(`  Point ${i + 1}: (${hexPoints[i].x}, ${hexPoints[i].y})`);
    }

    // Double-click to finish
    await canvas.dblclick({ position: hexPoints[5], force: true });
    await page.waitForTimeout(1500);
    console.log('✓ Double-clicked to finish polygon');

    // Wait for modal
    await page.waitForSelector('#sector-edit-modal', { state: 'visible', timeout: 10000 });
    console.log('✓ Modal appeared');

    // Fill form
    const modal = page.locator('#sector-edit-modal');
    await modal.locator('input[placeholder*="e.g., A1"]').fill(sectorCode);
    await modal.locator('input[placeholder*="Tribune"]').fill('Test Hexagon');
    await modal.locator('input[min="1"][max="100"]').fill('15');
    await modal.locator('input[min="1"][max="200"]').fill('25');
    await modal.locator('select.form-select').selectOption('standard');
    await page.waitForTimeout(500);
    console.log('✓ Form filled');

    // Save
    await modal.locator('button:has-text("Save")').click();
    await page.waitForTimeout(2000);

    // Wait for success
    await page.waitForSelector('.alert-success', { timeout: 10000 });
    console.log('✓ Hexagon saved successfully');

    // Screenshot
    await page.screenshot({ path: `hexagon-${sectorCode}.png`, fullPage: true });

    // Verify in database
    const response = await apiContext.get('/api/StadiumSectorOverlay');
    expect(response.ok()).toBeTruthy();
    const sectors = await response.json();
    const hexagonSector = sectors.find(s => s.sectorCode === sectorCode);

    expect(hexagonSector).toBeTruthy();
    expect(hexagonSector.shapeType).toBe('polygon');
    const shapeData = JSON.parse(hexagonSector.shapeData);
    expect(shapeData.length).toBe(6);
    console.log(`✓ Database verification: ${sectorCode} - 6 vertices confirmed`);
  });
});
