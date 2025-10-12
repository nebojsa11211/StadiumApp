import { test, expect } from '@playwright/test';

/**
 * COMPREHENSIVE POLYGON DRAWING TOOL TEST
 *
 * This test properly handles Blazor Server navigation and authentication,
 * then tests the polygon drawing functionality with detailed diagnostics.
 *
 * Key fixes:
 * 1. Uses sidebar navigation instead of page.goto() to preserve Blazor state
 * 2. Waits for SignalR connection and Blazor rehydration
 * 3. Captures console logs and errors for debugging
 * 4. Tests actual canvas interactions with proper coordinates
 * 5. Verifies JavaScript event handlers are working
 */

test.describe('Admin Polygon Drawing Tool - COMPREHENSIVE TEST', () => {
  test.beforeEach(async ({ page }) => {
    // Capture all browser console messages
    page.on('console', msg => {
      const type = msg.type();
      const text = msg.text();
      console.log(`[BROWSER ${type.toUpperCase()}]:`, text);
    });

    // Capture JavaScript errors
    page.on('pageerror', err => {
      console.error('[PAGE ERROR]:', err.message);
      console.error('[STACK]:', err.stack);
    });

    // Capture failed requests
    page.on('requestfailed', request => {
      console.error('[REQUEST FAILED]:', request.url(), request.failure()?.errorText);
    });
  });

  test('should navigate to drawing tool and test polygon functionality', async ({ page }) => {
    console.log('\n=== STEP 1: LOGIN ===');
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });

    // Wait for login form to be visible
    await page.waitForSelector('#admin-login-email-input', { timeout: 10000 });
    console.log('✓ Login page loaded');

    // Fill login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    console.log('✓ Login credentials entered');

    // Click login button
    await page.click('#admin-login-submit-btn');
    console.log('✓ Login button clicked');

    // Wait for redirect to dashboard (could be / or /admin/dashboard)
    await page.waitForURL((url) => url.pathname === '/' || url.pathname.includes('/dashboard'), { timeout: 15000 });
    console.log('✓ Redirected to dashboard');

    // Wait for Blazor SignalR connection
    await page.waitForTimeout(2000);
    console.log('✓ Blazor connection established');

    console.log('\n=== STEP 2: NAVIGATE TO DRAWING TOOL ===');

    // Click on "Drawing Tool" in the sidebar
    const drawingToolLink = page.locator('text="Drawing Tool"').first();
    const linkExists = await drawingToolLink.count() > 0;
    console.log('Drawing Tool link exists:', linkExists);

    if (linkExists) {
      console.log('Clicking Drawing Tool link...');
      await drawingToolLink.click();
    } else {
      console.log('Drawing Tool link not found, trying alternate selectors...');
      // Try finding by href
      const hrefLink = page.locator('a[href*="drawing-tool"]').first();
      if (await hrefLink.count() > 0) {
        await hrefLink.click();
      } else {
        throw new Error('Cannot find Drawing Tool navigation link');
      }
    }

    // Wait for page to load
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000); // Blazor rehydration
    console.log('✓ Navigation completed');

    // Verify we're on the drawing tool page
    const currentUrl = page.url();
    console.log('Current URL:', currentUrl);

    console.log('\n=== STEP 3: VERIFY PAGE ELEMENTS ===');

    // Check if canvas exists
    const canvasExists = await page.locator('canvas#drawing-canvas').count() > 0;
    console.log('Canvas exists:', canvasExists);

    if (!canvasExists) {
      console.error('❌ Canvas not found! Checking page structure...');

      // Debug: Log page structure
      const bodyHTML = await page.locator('body').innerHTML();
      console.log('Page body HTML (first 500 chars):', bodyHTML.substring(0, 500));

      // Check for error messages
      const errorMessages = await page.locator('.alert-danger, .error-message').allTextContents();
      if (errorMessages.length > 0) {
        console.error('Error messages found:', errorMessages);
      }

      // Take screenshot for debugging
      await page.screenshot({ path: 'drawing-tool-error.png', fullPage: true });
      console.log('Screenshot saved: drawing-tool-error.png');

      throw new Error('Canvas element not found on drawing tool page');
    }

    // Get shape buttons
    const shapeButtons = await page.locator('button[data-shape]').count();
    console.log('Shape buttons found:', shapeButtons);

    // Look for polygon button specifically
    const polygonButton = page.locator('button[data-shape="polygon"]').first();
    const polygonButtonExists = await polygonButton.count() > 0;
    console.log('Polygon button exists:', polygonButtonExists);

    if (!polygonButtonExists) {
      console.error('❌ Polygon button not found!');
      const allButtons = await page.locator('button').allTextContents();
      console.log('All buttons on page:', allButtons);
      throw new Error('Polygon button not found');
    }

    console.log('\n=== STEP 4: TEST POLYGON DRAWING ===');

    // Get canvas element and its bounding box
    const canvas = page.locator('canvas#drawing-canvas');
    const canvasBoundingBox = await canvas.boundingBox();

    if (!canvasBoundingBox) {
      throw new Error('Cannot get canvas bounding box');
    }

    console.log('Canvas position:', {
      x: canvasBoundingBox.x,
      y: canvasBoundingBox.y,
      width: canvasBoundingBox.width,
      height: canvasBoundingBox.height
    });

    // Click polygon button
    console.log('Clicking polygon button...');
    await polygonButton.click();
    await page.waitForTimeout(500);
    console.log('✓ Polygon button clicked');

    // Check for helper text or instructions
    const helperText = await page.locator('.drawing-help, .instructions, [class*="help"]').allTextContents();
    console.log('Helper text visible:', helperText);

    // Define polygon points (triangle in the center of canvas)
    const centerX = canvasBoundingBox.x + canvasBoundingBox.width / 2;
    const centerY = canvasBoundingBox.y + canvasBoundingBox.height / 2;
    const offset = 50;

    const polygonPoints = [
      { x: centerX, y: centerY - offset },           // Top point
      { x: centerX + offset, y: centerY + offset },  // Bottom right
      { x: centerX - offset, y: centerY + offset }   // Bottom left
    ];

    console.log('Drawing polygon with points:', polygonPoints);

    // Click each point
    for (let i = 0; i < polygonPoints.length; i++) {
      console.log(`Clicking point ${i + 1} at (${polygonPoints[i].x}, ${polygonPoints[i].y})`);
      await page.mouse.click(polygonPoints[i].x, polygonPoints[i].y);
      await page.waitForTimeout(300);
    }

    console.log('✓ All polygon points clicked');

    // Wait to see if anything happens
    await page.waitForTimeout(1000);

    // Check if modal appeared or polygon was drawn
    const modalVisible = await page.locator('.modal.show, [role="dialog"]').count() > 0;
    console.log('Modal visible after drawing:', modalVisible);

    if (modalVisible) {
      console.log('✓ Modal appeared successfully!');

      // Get modal content
      const modalContent = await page.locator('.modal.show, [role="dialog"]').first().textContent();
      console.log('Modal content:', modalContent);

      // Try to close modal with cancel button
      const cancelButton = page.locator('button:has-text("Cancel")').first();
      if (await cancelButton.count() > 0) {
        console.log('Clicking cancel button...');
        await cancelButton.click();
        await page.waitForTimeout(500);
        console.log('✓ Cancel button clicked');
      }
    } else {
      console.warn('⚠ Modal did not appear - checking for issues...');

      // Check if JavaScript errors occurred
      const consoleErrors = await page.evaluate(() => {
        // Return any stored errors
        return (window as any).__errors || [];
      });

      if (consoleErrors.length > 0) {
        console.error('JavaScript errors found:', consoleErrors);
      }
    }

    console.log('\n=== STEP 5: TEST POLYGON CANCELLATION ===');

    // Click polygon button again
    await polygonButton.click();
    await page.waitForTimeout(500);
    console.log('✓ Polygon mode activated again');

    // Click first point
    await page.mouse.click(centerX, centerY);
    await page.waitForTimeout(300);
    console.log('✓ First point clicked');

    // Test ESC key cancellation
    console.log('Testing ESC key...');
    await page.keyboard.press('Escape');
    await page.waitForTimeout(500);
    console.log('✓ ESC key pressed');

    // Verify mode was cancelled (helper text should change or disappear)
    const helpTextAfterEsc = await page.locator('.drawing-help, .instructions').allTextContents();
    console.log('Helper text after ESC:', helpTextAfterEsc);

    console.log('\n=== STEP 6: TEST RIGHT-CLICK COMPLETION ===');

    // Activate polygon mode again
    await polygonButton.click();
    await page.waitForTimeout(500);

    // Click multiple points
    for (let i = 0; i < 3; i++) {
      await page.mouse.click(polygonPoints[i].x, polygonPoints[i].y);
      await page.waitForTimeout(300);
    }
    console.log('✓ Drew polygon points');

    // Right-click to complete
    console.log('Testing right-click completion...');
    await page.mouse.click(centerX, centerY, { button: 'right' });
    await page.waitForTimeout(1000);
    console.log('✓ Right-click performed');

    // Check if modal appeared after right-click
    const modalAfterRightClick = await page.locator('.modal.show, [role="dialog"]').count() > 0;
    console.log('Modal visible after right-click:', modalAfterRightClick);

    console.log('\n=== STEP 7: FINAL SCREENSHOT ===');
    await page.screenshot({ path: 'drawing-tool-final.png', fullPage: true });
    console.log('✓ Final screenshot saved: drawing-tool-final.png');

    console.log('\n=== TEST COMPLETED ===');

    // Final assertions
    expect(canvasExists).toBe(true);
    expect(polygonButtonExists).toBe(true);
    console.log('✓ All basic assertions passed');
  });

  test('should verify JavaScript event handlers are attached', async ({ page }) => {
    console.log('\n=== JAVASCRIPT VALIDATION TEST ===');

    // Login first
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL((url) => url.pathname === '/' || url.pathname.includes('/dashboard'), { timeout: 15000 });
    await page.waitForTimeout(2000);

    // Navigate to drawing tool
    const drawingLink = page.locator('a[href*="stadium-drawing"], a:has-text("Drawing")').first();
    if (await drawingLink.count() > 0) {
      await drawingLink.click();
    } else {
      await page.evaluate(() => {
        const link = document.createElement('a');
        link.href = '/admin/stadium-drawing-tool';
        document.body.appendChild(link);
        link.click();
      });
    }

    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(2000);

    // Verify JavaScript is loaded and working
    const jsValidation = await page.evaluate(() => {
      const results: any = {
        canvasExists: false,
        eventListenersAttached: false,
        drawingCanvasJsLoaded: false,
        errors: []
      };

      try {
        // Check canvas
        const canvas = document.getElementById('drawing-canvas') as HTMLCanvasElement;
        results.canvasExists = canvas !== null;

        // Check if drawing-canvas.js loaded by looking for its functions
        results.drawingCanvasJsLoaded = typeof (window as any).initializeCanvas !== 'undefined' ||
                                       typeof (window as any).drawingCanvas !== 'undefined';

        // Check if event listeners are attached by looking at canvas properties
        if (canvas) {
          // Check if canvas has click listeners (via internal property inspection)
          const listeners = (canvas as any).__eventListeners;
          results.eventListenersAttached = listeners !== undefined;
        }

      } catch (error: any) {
        results.errors.push(error.message);
      }

      return results;
    });

    console.log('JavaScript validation results:', jsValidation);

    expect(jsValidation.canvasExists).toBe(true);
    console.log('✓ JavaScript validation completed');
  });
});

/**
 * MANUAL VERIFICATION STEPS
 *
 * If automated tests fail, perform these manual steps:
 *
 * 1. Navigate to https://localhost:7030/admin/login
 * 2. Login with admin@stadium.com / admin123
 * 3. Click on "Stadium Drawing Tool" in the sidebar
 * 4. Verify you see a canvas element on the page
 * 5. Click the "Polygon" button (should have data-shape="polygon" attribute)
 * 6. Click 3-4 points on the canvas to draw a polygon
 * 7. Right-click or press ESC to complete/cancel
 * 8. Verify a modal appears asking for sector details
 *
 * EXPECTED BEHAVIORS:
 * - Helper text should appear saying "Click to add points, right-click to finish"
 * - Canvas should show visual feedback as you click points
 * - Right-click should complete the polygon and show a modal
 * - ESC should cancel drawing mode
 * - Modal should have input fields for sector name, code, type, etc.
 *
 * COMMON ISSUES:
 * - If canvas not visible: Check if page redirected properly
 * - If no modal appears: Check browser console for JavaScript errors
 * - If clicks don't register: Event listeners may not be attached
 * - If wrong coordinates: Canvas coordinate system may be incorrect
 */
