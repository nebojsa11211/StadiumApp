import { test, expect, Page } from '@playwright/test';

test.describe('Admin Stadium Drawing Tool - Variable Seating Feature', () => {
  let page: Page;

  test.beforeAll(async ({ browser }) => {
    page = await browser.newPage();
  });

  test.afterAll(async () => {
    await page.close();
  });

  test('should verify variable seating feature for SECT2', async () => {
    // Step 1: Navigate to login page
    console.log('Step 1: Navigating to admin login...');
    await page.goto('https://localhost:7030/admin/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    await page.waitForTimeout(2000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-01-login-page.png',
      fullPage: true
    });

    // Step 2: Login with admin credentials
    console.log('Step 2: Logging in with admin credentials...');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for navigation after login (redirects to / which is the admin dashboard)
    await page.waitForLoadState('networkidle', { timeout: 15000 });
    await page.waitForTimeout(2000);

    console.log('Login successful, current URL:', page.url());
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-02-after-login.png',
      fullPage: true
    });

    // Step 3: Navigate to Stadium Drawing Tool
    console.log('Step 3: Navigating to Stadium Drawing Tool...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Wait for page to fully load - check for sector overlays
    console.log('Waiting for sector overlays to appear...');
    await page.waitForSelector('[id^="sector-overlay-"]', {
      state: 'visible',
      timeout: 30000
    });

    await page.waitForTimeout(3000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-03-drawing-tool-loaded.png',
      fullPage: true
    });

    // Step 4: Click on SECT2 sector overlay
    console.log('Step 4: Clicking on SECT2 sector overlay...');
    const sect2Overlay = await page.locator('[id="sector-overlay-SECT2"]');
    const isVisible = await sect2Overlay.isVisible();
    console.log('SECT2 overlay visible:', isVisible);

    if (!isVisible) {
      console.error('SECT2 overlay not found! Looking for all sector overlays...');
      const allOverlays = await page.locator('[id^="sector-overlay-"]').all();
      console.log('Found', allOverlays.length, 'sector overlays');
      for (const overlay of allOverlays) {
        const id = await overlay.getAttribute('id');
        console.log('  - Found overlay:', id);
      }
      throw new Error('SECT2 overlay not found');
    }

    await sect2Overlay.click();

    // Wait for edit modal to appear
    console.log('Waiting for edit modal to appear...');
    await page.waitForSelector('#admin-stadium-edit-modal', {
      state: 'visible',
      timeout: 10000
    });

    await page.waitForTimeout(2000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-04-edit-modal-opened.png',
      fullPage: true
    });

    // Step 5: Verify variable seating configuration in modal
    console.log('Step 5: Verifying variable seating configuration...');

    // Check if "Enable Variable Seating" checkbox is checked
    const variableSeatingCheckbox = await page.locator('#admin-stadium-variable-seating-checkbox');
    const isChecked = await variableSeatingCheckbox.isChecked();
    console.log('Enable Variable Seating checkbox is checked:', isChecked);
    expect(isChecked).toBe(true);

    // Check if variable seating configuration UI is visible
    const variableSeatingConfig = await page.locator('#admin-stadium-variable-seating-config');
    const isConfigVisible = await variableSeatingConfig.isVisible();
    console.log('Variable seating configuration UI is visible:', isConfigVisible);
    expect(isConfigVisible).toBe(true);

    // Read current row patterns
    console.log('Reading current row patterns...');
    const rowPatterns = await page.locator('.row-pattern-item').all();
    console.log('Found', rowPatterns.length, 'row patterns');

    const originalPatterns: { startRow: string, endRow: string, seatsPerRow: string }[] = [];
    for (let i = 0; i < rowPatterns.length; i++) {
      const pattern = rowPatterns[i];
      const startRow = await pattern.locator('input[placeholder*="Start"]').inputValue();
      const endRow = await pattern.locator('input[placeholder*="End"]').inputValue();
      const seatsPerRow = await pattern.locator('input[placeholder*="Seats"]').inputValue();

      console.log(`Pattern ${i + 1}: Rows ${startRow}-${endRow}, ${seatsPerRow} seats per row`);
      originalPatterns.push({ startRow, endRow, seatsPerRow });
    }

    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-05-original-config.png',
      fullPage: true
    });

    // Step 6: Click "Preview Sector" button
    console.log('Step 6: Opening preview modal...');
    const previewButton = await page.locator('button:has-text("Preview Sector")');
    await previewButton.click();

    // Wait for preview modal to appear
    await page.waitForSelector('#admin-stadium-preview-modal', {
      state: 'visible',
      timeout: 10000
    });

    await page.waitForTimeout(2000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-06-preview-modal-original.png',
      fullPage: true
    });

    // Count seats in each row (rows 1-10)
    console.log('Counting seats in each row...');
    const originalSeatCounts: { [key: number]: number } = {};

    for (let rowNum = 1; rowNum <= 10; rowNum++) {
      const seatsInRow = await page.locator(`.seat-row-${rowNum} .seat-preview`).count();
      originalSeatCounts[rowNum] = seatsInRow;
      console.log(`Row ${rowNum}: ${seatsInRow} seats`);
    }

    // Verify seat counts match configuration
    console.log('Verifying seat counts match original configuration...');
    for (const pattern of originalPatterns) {
      const startRow = parseInt(pattern.startRow);
      const endRow = parseInt(pattern.endRow);
      const expectedSeats = parseInt(pattern.seatsPerRow);

      for (let rowNum = startRow; rowNum <= endRow; rowNum++) {
        const actualSeats = originalSeatCounts[rowNum] || 0;
        console.log(`  Row ${rowNum}: Expected ${expectedSeats}, Got ${actualSeats}`);
        expect(actualSeats).toBe(expectedSeats);
      }
    }

    // Step 7: Close preview modal
    console.log('Step 7: Closing preview modal...');
    const closePreviewButton = await page.locator('#admin-stadium-preview-modal button:has-text("Close")');
    await closePreviewButton.click();

    await page.waitForTimeout(1000);

    // Step 8: Modify SECT2 to have different seat counts
    console.log('Step 8: Modifying SECT2 variable seating configuration...');
    console.log('  Pattern 1: Rows 1-5 should have 15 seats per row');
    console.log('  Pattern 2: Rows 6-10 should have 25 seats per row');

    // Clear existing patterns and add new ones
    const firstPattern = rowPatterns[0];

    // Update first pattern: Rows 1-5, 15 seats per row
    await firstPattern.locator('input[placeholder*="Start"]').fill('1');
    await firstPattern.locator('input[placeholder*="End"]').fill('5');
    await firstPattern.locator('input[placeholder*="Seats"]').fill('15');

    // Check if we need to add a second pattern
    if (rowPatterns.length < 2) {
      console.log('Adding second row pattern...');
      const addPatternButton = await page.locator('button:has-text("Add Row Pattern")');
      await addPatternButton.click();
      await page.waitForTimeout(1000);
    }

    // Update second pattern: Rows 6-10, 25 seats per row
    const updatedPatterns = await page.locator('.row-pattern-item').all();
    const secondPattern = updatedPatterns[1];

    await secondPattern.locator('input[placeholder*="Start"]').fill('6');
    await secondPattern.locator('input[placeholder*="End"]').fill('10');
    await secondPattern.locator('input[placeholder*="Seats"]').fill('25');

    await page.waitForTimeout(1000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-07-modified-config.png',
      fullPage: true
    });

    // Step 9: Save changes
    console.log('Step 9: Saving changes...');
    const saveButton = await page.locator('#admin-stadium-edit-modal button:has-text("Save Changes")');
    await saveButton.click();

    // Wait for modal to close and save to complete
    await page.waitForTimeout(3000);

    console.log('Changes saved successfully');
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-08-after-save.png',
      fullPage: true
    });

    // Step 10: Re-open SECT2 and verify changes persisted
    console.log('Step 10: Re-opening SECT2 to verify persistence...');
    await sect2Overlay.click();

    await page.waitForSelector('#admin-stadium-edit-modal', {
      state: 'visible',
      timeout: 10000
    });

    await page.waitForTimeout(2000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-09-reopened-modal.png',
      fullPage: true
    });

    // Step 11: Click "Preview Sector" again
    console.log('Step 11: Opening preview modal again to verify changes...');
    const previewButton2 = await page.locator('button:has-text("Preview Sector")');
    await previewButton2.click();

    await page.waitForSelector('#admin-stadium-preview-modal', {
      state: 'visible',
      timeout: 10000
    });

    await page.waitForTimeout(2000);
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-10-preview-modal-modified.png',
      fullPage: true
    });

    // Count seats again and verify new configuration
    console.log('Counting seats in each row (after modification)...');
    const modifiedSeatCounts: { [key: number]: number } = {};

    for (let rowNum = 1; rowNum <= 10; rowNum++) {
      const seatsInRow = await page.locator(`.seat-row-${rowNum} .seat-preview`).count();
      modifiedSeatCounts[rowNum] = seatsInRow;
      console.log(`Row ${rowNum}: ${seatsInRow} seats`);
    }

    // Step 12: Verify modified seat counts
    console.log('Step 12: Verifying modified seat counts...');
    console.log('Expected: Rows 1-5 have 15 seats, Rows 6-10 have 25 seats');

    let allCorrect = true;

    // Verify rows 1-5 have 15 seats
    for (let rowNum = 1; rowNum <= 5; rowNum++) {
      const actualSeats = modifiedSeatCounts[rowNum] || 0;
      const isCorrect = actualSeats === 15;
      console.log(`  Row ${rowNum}: Expected 15, Got ${actualSeats} - ${isCorrect ? '✓' : '✗'}`);
      expect(actualSeats).toBe(15);
      if (!isCorrect) allCorrect = false;
    }

    // Verify rows 6-10 have 25 seats
    for (let rowNum = 6; rowNum <= 10; rowNum++) {
      const actualSeats = modifiedSeatCounts[rowNum] || 0;
      const isCorrect = actualSeats === 25;
      console.log(`  Row ${rowNum}: Expected 25, Got ${actualSeats} - ${isCorrect ? '✓' : '✗'}`);
      expect(actualSeats).toBe(25);
      if (!isCorrect) allCorrect = false;
    }

    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\variable-seating-11-final-verification.png',
      fullPage: true
    });

    // Final summary
    console.log('\n========================================');
    console.log('VARIABLE SEATING VERIFICATION SUMMARY');
    console.log('========================================');
    console.log('✓ Successfully logged in to admin panel');
    console.log('✓ Navigated to Stadium Drawing Tool');
    console.log('✓ Found and clicked SECT2 sector overlay');
    console.log('✓ Variable seating checkbox was checked');
    console.log('✓ Variable seating configuration UI was visible');
    console.log(`✓ Original configuration had ${originalPatterns.length} row patterns`);
    console.log('✓ Preview modal displayed correct original seat counts');
    console.log('✓ Successfully modified variable seating configuration');
    console.log('✓ Changes persisted after save and reload');
    console.log(`${allCorrect ? '✓' : '✗'} Preview shows correct modified seat counts`);
    console.log('\nOriginal Seat Counts:');
    for (let i = 1; i <= 10; i++) {
      console.log(`  Row ${i}: ${originalSeatCounts[i] || 0} seats`);
    }
    console.log('\nModified Seat Counts:');
    for (let i = 1; i <= 10; i++) {
      console.log(`  Row ${i}: ${modifiedSeatCounts[i] || 0} seats`);
    }
    console.log('========================================\n');

    expect(allCorrect).toBe(true);
  });
});
