import { test, expect } from '@playwright/test';

/**
 * CORRECTED POLYGON DRAWING TOOL TEST
 *
 * This test uses the ACTUAL element IDs and interaction patterns from the implementation.
 * Key corrections:
 * 1. Canvas ID: admin-drawing-tool-canvas (not drawing-canvas)
 * 2. Polygon selection: via dropdown menu (not direct button)
 * 3. Completion: double-click (not right-click)
 * 4. JavaScript functions: initializeDrawingCanvas, setShapeMode, drawSectorOverlays
 */

test.describe('Admin Polygon Drawing Tool - CORRECTED TEST', () => {
  test.beforeEach(async ({ page }) => {
    // Capture console for debugging
    page.on('console', msg => console.log(`[BROWSER ${msg.type().toUpperCase()}]:`, msg.text()));
    page.on('pageerror', err => console.error('[PAGE ERROR]:', err.message));
  });

  test('should properly test polygon drawing with correct IDs and flow', async ({ page }) => {
    console.log('\n========== CORRECTED POLYGON DRAWING TEST ==========\n');

    // ===== STEP 1: LOGIN =====
    console.log('STEP 1: Login to Admin');
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForSelector('#admin-login-email-input');

    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for dashboard (could be / or /dashboard)
    await page.waitForURL((url) => url.pathname === '/' || url.pathname.includes('/dashboard'));
    console.log('âś“ Logged in successfully');

    // Wait for Blazor
    await page.waitForTimeout(2000);

    // ===== STEP 2: NAVIGATE TO DRAWING TOOL =====
    console.log('\nSTEP 2: Navigate to Drawing Tool');
    const drawingToolLink = page.locator('text="Drawing Tool"').first();
    await drawingToolLink.click();

    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000); // Blazor rehydration
    console.log('âś“ Navigated to drawing tool page');

    // ===== STEP 3: VERIFY CORRECT CANVAS ID =====
    console.log('\nSTEP 3: Verify Canvas Element');
    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    const canvasExists = await canvas.count() > 0;
    expect(canvasExists).toBe(true);
    console.log('âś“ Canvas found with correct ID: admin-drawing-tool-canvas');

    // Get canvas dimensions
    const canvasBoundingBox = await canvas.boundingBox();
    expect(canvasBoundingBox).not.toBeNull();
    console.log(`âś“ Canvas dimensions: ${canvasBoundingBox?.width}x${canvasBoundingBox?.height}`);

    // ===== STEP 4: VERIFY JAVASCRIPT FUNCTIONS =====
    console.log('\nSTEP 4: Verify JavaScript Functions');
    const jsValidation = await page.evaluate(() => {
      return {
        canvasElement: document.getElementById('admin-drawing-tool-canvas') !== null,
        initFunctionExists: typeof (window as any).initializeDrawingCanvas === 'function',
        setShapeModeExists: typeof (window as any).setShapeMode === 'function',
        drawOverlaysExists: typeof (window as any).drawSectorOverlays === 'function'
      };
    });

    console.log('JavaScript validation:', jsValidation);
    expect(jsValidation.canvasElement).toBe(true);
    expect(jsValidation.initFunctionExists).toBe(true);
    expect(jsValidation.setShapeModeExists).toBe(true);
    expect(jsValidation.drawOverlaysExists).toBe(true);
    console.log('âś“ All required JavaScript functions exist');

    // ===== STEP 5: ACTIVATE POLYGON MODE VIA DROPDOWN =====
    console.log('\nSTEP 5: Activate Polygon Mode');

    // Find and click the shape dropdown toggle
    const shapeDropdownToggle = page.locator('#admin-drawing-tool-shape-dropdown-toggle');
    const dropdownExists = await shapeDropdownToggle.count() > 0;
    expect(dropdownExists).toBe(true);
    console.log('âś“ Shape dropdown toggle found');

    // Click dropdown to open menu
    await shapeDropdownToggle.click();
    await page.waitForTimeout(500);
    console.log('âś“ Dropdown menu opened');

    // Take screenshot of dropdown
    await page.screenshot({ path: 'polygon-dropdown-open.png' });

    // Find and click polygon option
    const polygonOption = page.locator('#admin-drawing-tool-polygon-shape-option');
    const polygonOptionExists = await polygonOption.count() > 0;
    expect(polygonOptionExists).toBe(true);
    console.log('âś“ Polygon option found in dropdown');

    await polygonOption.click();
    await page.waitForTimeout(500);
    console.log('âś“ Polygon mode activated');

    // Verify helper text appears
    const helperText = await page.locator('small:has-text("Click multiple points")').first().textContent();
    console.log('Helper text:', helperText);

    // ===== STEP 6: DRAW POLYGON ON CANVAS =====
    console.log('\nSTEP 6: Draw Polygon on Canvas');

    if (!canvasBoundingBox) {
      throw new Error('Canvas bounding box is null');
    }

    // Calculate polygon points (triangle in center)
    const centerX = canvasBoundingBox.x + canvasBoundingBox.width / 2;
    const centerY = canvasBoundingBox.y + canvasBoundingBox.height / 2;
    const offset = 80;

    const polygonPoints = [
      { x: centerX, y: centerY - offset },           // Top
      { x: centerX + offset, y: centerY + offset },  // Bottom right
      { x: centerX - offset, y: centerY + offset }   // Bottom left
    ];

    console.log('Drawing polygon with 3 points...');

    // Click each point
    for (let i = 0; i < polygonPoints.length; i++) {
      const point = polygonPoints[i];
      console.log(`  Point ${i + 1}: (${Math.round(point.x)}, ${Math.round(point.y)})`);
      await page.mouse.click(point.x, point.y);
      await page.waitForTimeout(400);
    }
    console.log('âś“ All polygon points clicked');

    // ===== STEP 7: COMPLETE POLYGON WITH DOUBLE-CLICK =====
    console.log('\nSTEP 7: Complete Polygon');

    // Double-click on the last point to complete polygon
    const lastPoint = polygonPoints[polygonPoints.length - 1];
    console.log('Double-clicking to complete polygon...');
    await page.mouse.dblclick(lastPoint.x, lastPoint.y);
    await page.waitForTimeout(1000);
    console.log('âś“ Double-click performed');

    // ===== STEP 8: VERIFY MODAL APPEARS =====
    console.log('\nSTEP 8: Verify Sector Edit Modal');

    // Check if modal appeared
    const modal = page.locator('.modal.show, [role="dialog"]');
    const modalCount = await modal.count();
    console.log('Modal count:', modalCount);

    if (modalCount > 0) {
      console.log('âś“âś“âś“ SUCCESS: Modal appeared after polygon drawing! âś“âś“âś“');

      // Get modal content
      const modalContent = await modal.first().textContent();
      console.log('Modal content preview:', modalContent?.substring(0, 200));

      // Look for sector-related inputs
      const sectorCodeInput = await page.locator('input[name="sectorCode"], #sector-code-input').count();
      const sectorNameInput = await page.locator('input[name="name"], input[name="sectorName"]').count();
      console.log('Sector code input found:', sectorCodeInput > 0);
      console.log('Sector name input found:', sectorNameInput > 0);

      // Take screenshot of modal
      await page.screenshot({ path: 'polygon-modal-success.png', fullPage: true });
      console.log('âś“ Screenshot saved: polygon-modal-success.png');

      // Test cancel button
      const cancelButton = page.locator('button:has-text("Cancel")').first();
      if (await cancelButton.count() > 0) {
        console.log('Testing cancel button...');
        await cancelButton.click();
        await page.waitForTimeout(500);
        console.log('âś“ Cancel button works');

        // Verify modal closed
        const modalAfterCancel = await page.locator('.modal.show').count();
        expect(modalAfterCancel).toBe(0);
        console.log('âś“ Modal closed successfully');
      }
    } else {
      console.error('âťŚ FAILURE: Modal did not appear after double-click');

      // Debug: check what happened
      await page.screenshot({ path: 'polygon-no-modal-error.png', fullPage: true });
      console.log('Screenshot saved: polygon-no-modal-error.png');

      // Check for any alerts or error messages
      const alerts = await page.locator('.alert').allTextContents();
      if (alerts.length > 0) {
        console.log('Alert messages:', alerts);
      }

      throw new Error('Sector edit modal did not appear after polygon completion');
    }

    console.log('\n========== TEST COMPLETED SUCCESSFULLY ==========\n');
  });

  test('should test polygon cancellation with ESC key', async ({ page }) => {
    console.log('\n========== POLYGON CANCELLATION TEST ==========\n');

    // Login
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL((url) => url.pathname === '/' || url.pathname.includes('/dashboard'));
    await page.waitForTimeout(2000);

    // Navigate to drawing tool
    await page.locator('text="Drawing Tool"').first().click();
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);

    // Activate polygon mode
    await page.locator('#admin-drawing-tool-shape-dropdown-toggle').click();
    await page.waitForTimeout(300);
    await page.locator('#admin-drawing-tool-polygon-shape-option').click();
    await page.waitForTimeout(500);
    console.log('âś“ Polygon mode activated');

    // Click first point
    const canvas = page.locator('canvas#admin-drawing-tool-canvas');
    const box = await canvas.boundingBox();
    if (!box) throw new Error('Canvas box is null');

    await page.mouse.click(box.x + 100, box.y + 100);
    await page.waitForTimeout(300);
    console.log('âś“ First point clicked');

    // Press ESC to cancel
    console.log('Pressing ESC to cancel...');
    await page.keyboard.press('Escape');
    await page.waitForTimeout(500);
    console.log('âś“ ESC pressed');

    // Verify no modal appeared
    const modalCount = await page.locator('.modal.show').count();
    expect(modalCount).toBe(0);
    console.log('âś“ No modal appeared (correct - polygon was cancelled)');

    console.log('\n========== CANCELLATION TEST PASSED ==========\n');
  });
});

/**
 * MANUAL VERIFICATION STEPS (if tests still fail)
 *
 * 1. Navigate to https://localhost:7030/admin/login
 * 2. Login: admin@stadium.com / admin123
 * 3. Click "Drawing Tool" in sidebar
 * 4. Verify canvas with ID "admin-drawing-tool-canvas" is visible
 * 5. Click dropdown button "âž• Create Sector (Rectangle)"
 * 6. Click "â¬ˇ Custom Polygon (multi-click)" option
 * 7. Click 3-4 points on the canvas
 * 8. Double-click the last point (or any point) to finish
 * 9. Modal should appear asking for sector details
 * 10. Fill in sector code, name, and other details
 * 11. Click Save to create sector OR Cancel to discard
 *
 * EXPECTED BEHAVIOR:
 * - Helper text should say "Click multiple points to create custom polygon. Double-click to finish (min 3 points)"
 * - Each click should visually add a point to the polygon
 * - Double-click completes the polygon
 * - Modal appears with sector configuration form
 * - Polygon is saved to database when clicking Save button
 */
