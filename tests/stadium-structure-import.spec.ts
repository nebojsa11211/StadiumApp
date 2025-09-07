import { test, expect } from '@playwright/test';
import { readFile } from 'fs/promises';
import path from 'path';

test.describe('Stadium Structure Import and Visualization Tests', () => {
  // Admin credentials for authentication
  const adminEmail = 'admin@stadium.com';
  const adminPassword = 'admin123';
  
  test.beforeEach(async ({ page }) => {
    // Login as admin before each test
    await page.goto('/login');
    await page.locator('input[type="email"]').fill(adminEmail);
    await page.locator('input[type="password"]').fill(adminPassword);
    await page.locator('button[type="submit"]').click();
    
    // Wait for successful login
    await page.waitForURL('**/bartender');
  });

  test('should import stadium structure JSON file successfully', async ({ page }) => {
    // Navigate to stadium structure management page
    await page.goto('/admin/stadium-structure');
    
    // Verify page loaded correctly
    await expect(page.locator('h3')).toContainText('Stadium Structure Management');
    
    // Check if file input is present
    const fileInput = page.locator('input[type="file"]');
    await expect(fileInput).toBeVisible();
    await expect(fileInput).toHaveAttribute('accept', '.json');
    
    // Read the JSON file content
    const jsonFilePath = path.join(__dirname, '..', 'stadium-structure.json');
    const jsonContent = await readFile(jsonFilePath, 'utf-8');
    
    // Upload the stadium structure JSON file
    await fileInput.setInputFiles({
      name: 'stadium-structure.json',
      mimeType: 'application/json',
      buffer: Buffer.from(jsonContent)
    });
    
    // Verify file is selected
    await expect(page.locator('.form-text.text-success')).toContainText('Selected: stadium-structure.json');
    
    // Click import button
    const importButton = page.locator('button:has-text("Import Structure")');
    await expect(importButton).toBeEnabled();
    await importButton.click();
    
    // Wait for import to complete and check for success message
    await expect(page.locator('.alert-success')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('.alert-success h5')).toContainText('Import Successful!');
    await expect(page.locator('.alert-success p')).toContainText('Stadium structure has been imported successfully');
    
    // Verify current structure summary is updated
    const currentStructureCard = page.locator('.card:has(.card-header:has-text("Current Structure"))');
    await expect(currentStructureCard.locator('text=Sectors:')).toBeVisible();
    await expect(currentStructureCard.locator('text=Total Seats:')).toBeVisible();
    
    // Check that sector count matches expected value (12 sections from JSON)
    const sectorsRow = currentStructureCard.locator('div.row:has(strong:has-text("Sectors:"))');
    await expect(sectorsRow.locator('div.col-sm-6').nth(1)).toContainText('12');
    
    // Verify total seats calculation (sum of all seats from JSON structure)
    const expectedTotalSeats = 
      (25 * 20) + (25 * 22) + (25 * 20) + // North Lower Ring: N1, N2, N3
      (30 * 18) + (30 * 20) +             // North Upper Ring: N4, N5
      (25 * 20) + (25 * 22) + (25 * 20) + // South Lower Ring: S1, S2, S3
      (15 * 16) + (12 * 14) +             // East VIP Ring: VIP1, VIP2
      (22 * 18) + (22 * 20) + (22 * 18);  // West Lower Ring: W1, W2, W3
    
    const totalSeatsRow = currentStructureCard.locator('div.row:has(strong:has-text("Total Seats:"))');
    await expect(totalSeatsRow.locator('.badge.bg-primary')).toContainText(expectedTotalSeats.toString());
  });

  test('should display stadium visualization correctly after import', async ({ page }) => {
    // First import the structure (same as previous test)
    await page.goto('/admin/stadium-structure');
    
    const jsonFilePath = path.join(__dirname, '..', 'stadium-structure.json');
    const jsonContent = await readFile(jsonFilePath, 'utf-8');
    
    await page.locator('input[type="file"]').setInputFiles({
      name: 'stadium-structure.json',
      mimeType: 'application/json',
      buffer: Buffer.from(jsonContent)
    });
    
    await page.locator('button:has-text("Import Structure")').click();
    await expect(page.locator('.alert-success')).toBeVisible({ timeout: 10000 });
    
    // Navigate to stadium overview page
    await page.goto('/admin/stadium-overview');
    
    // Verify page loaded correctly
    await expect(page.locator('h3')).toContainText('Stadium Structure Overview');
    
    // Check that stadium visualization is displayed (not loading state)
    await expect(page.locator('.stadium-visualization')).toBeVisible();
    await expect(page.locator('.spinner-border')).toHaveCount(0);
    
    // Verify North Tribune is displayed
    const northTribune = page.locator('.tribune-section.tribune-north');
    await expect(northTribune).toBeVisible();
    await expect(northTribune.locator('.tribune-title')).toContainText('North Stand (N)');
    
    // Check North Tribune sectors
    await expect(northTribune.locator('.sector-card:has(.sector-name:has-text("North Lower 1 (N1)"))').first()).toBeVisible();
    await expect(northTribune.locator('.sector-card:has(.sector-name:has-text("North Lower 2 (N2)"))').first()).toBeVisible();
    await expect(northTribune.locator('.sector-card:has(.sector-name:has-text("North Lower 3 (N3)"))').first()).toBeVisible();
    await expect(northTribune.locator('.sector-card:has(.sector-name:has-text("North Upper 1 (N4)"))').first()).toBeVisible();
    await expect(northTribune.locator('.sector-card:has(.sector-name:has-text("North Upper 2 (N5)"))').first()).toBeVisible();
    
    // Verify South Tribune is displayed
    const southTribune = page.locator('.tribune-section.tribune-south');
    await expect(southTribune).toBeVisible();
    await expect(southTribune.locator('.tribune-title')).toContainText('South Stand (S)');
    
    // Check South Tribune sectors
    await expect(southTribune.locator('.sector-card:has(.sector-name:has-text("South Lower 1 (S1)"))').first()).toBeVisible();
    await expect(southTribune.locator('.sector-card:has(.sector-name:has-text("South Lower 2 (S2)"))').first()).toBeVisible();
    await expect(southTribune.locator('.sector-card:has(.sector-name:has-text("South Lower 3 (S3)"))').first()).toBeVisible();
    
    // Verify East Tribune (VIP sections) is displayed
    const eastTribune = page.locator('.side-tribune.east-tribune .tribune-section.tribune-east');
    await expect(eastTribune).toBeVisible();
    await expect(eastTribune.locator('.tribune-title')).toContainText('East Stand (E)');
    
    // Check VIP sectors
    await expect(eastTribune.locator('.sector-card:has(.sector-name:has-text("VIP East Premium"))').first()).toBeVisible();
    await expect(eastTribune.locator('.sector-card:has(.sector-name:has-text("VIP East Club"))').first()).toBeVisible();
    
    // Verify West Tribune is displayed
    const westTribune = page.locator('.side-tribune.west-tribune .tribune-section.tribune-west');
    await expect(westTribune).toBeVisible();
    await expect(westTribune.locator('.tribune-title')).toContainText('West Stand (W)');
    
    // Check West Tribune sectors
    await expect(westTribune.locator('.sector-card:has(.sector-name:has-text("West Lower 1"))').first()).toBeVisible();
    await expect(westTribune.locator('.sector-card:has(.sector-name:has-text("West Lower 2"))').first()).toBeVisible();
    await expect(westTribune.locator('.sector-card:has(.sector-name:has-text("West Lower 3"))').first()).toBeVisible();
    
    // Verify playing field statistics
    const playingField = page.locator('.playing-field');
    await expect(playingField).toBeVisible();
    await expect(playingField.locator('h5')).toContainText('PLAYING FIELD');
    
    // Check total statistics in the playing field
    const fieldStats = playingField.locator('.field-stats');
    
    // Calculate expected totals
    const expectedTotalSeats = 
      (25 * 20) + (25 * 22) + (25 * 20) + // North Lower Ring: N1, N2, N3
      (30 * 18) + (30 * 20) +             // North Upper Ring: N4, N5
      (25 * 20) + (25 * 22) + (25 * 20) + // South Lower Ring: S1, S2, S3
      (15 * 16) + (12 * 14) +             // East VIP Ring: VIP1, VIP2
      (22 * 18) + (22 * 20) + (22 * 18);  // West Lower Ring: W1, W2, W3
    const expectedTotalSectors = 12;
    const expectedTotalTribunes = 4;
    
    await expect(fieldStats.locator('div').first()).toContainText(`${expectedTotalSeats} Total Seats`);
    await expect(fieldStats.locator('div').nth(1)).toContainText(`${expectedTotalSectors} Sectors`);
    await expect(fieldStats.locator('div').nth(2)).toContainText(`${expectedTotalTribunes} Tribunes`);
  });

  test('should verify individual sector seat counts match imported data', async ({ page }) => {
    // Import structure first
    await page.goto('/admin/stadium-structure');
    
    const jsonFilePath = path.join(__dirname, '..', 'stadium-structure.json');
    const jsonContent = await readFile(jsonFilePath, 'utf-8');
    
    await page.locator('input[type="file"]').setInputFiles({
      name: 'stadium-structure.json',
      mimeType: 'application/json',
      buffer: Buffer.from(jsonContent)
    });
    
    await page.locator('button:has-text("Import Structure")').click();
    await expect(page.locator('.alert-success')).toBeVisible({ timeout: 10000 });
    
    // Navigate to overview
    await page.goto('/admin/stadium-overview');
    await expect(page.locator('.stadium-visualization')).toBeVisible();
    
    // Verify specific sector seat counts
    const sectorTests = [
      { name: 'North Lower 1 (N1)', rows: 25, seatsPerRow: 20, total: 500 },
      { name: 'North Lower 2 (N2)', rows: 25, seatsPerRow: 22, total: 550 },
      { name: 'North Lower 3 (N3)', rows: 25, seatsPerRow: 20, total: 500 },
      { name: 'North Upper 1 (N4)', rows: 30, seatsPerRow: 18, total: 540 },
      { name: 'North Upper 2 (N5)', rows: 30, seatsPerRow: 20, total: 600 },
      { name: 'South Lower 1 (S1)', rows: 25, seatsPerRow: 20, total: 500 },
      { name: 'South Lower 2 (S2)', rows: 25, seatsPerRow: 22, total: 550 },
      { name: 'South Lower 3 (S3)', rows: 25, seatsPerRow: 20, total: 500 },
    ];
    
    for (const sector of sectorTests) {
      const sectorCard = page.locator(`.sector-card:has(.sector-name:has-text("${sector.name}"))`).first();
      await expect(sectorCard).toBeVisible();
      
      // Check rows
      await expect(sectorCard.locator('.sector-details div').filter({ hasText: 'Rows:' }))
        .toContainText(`Rows: ${sector.rows}`);
      
      // Check seats per row
      await expect(sectorCard.locator('.sector-details div').filter({ hasText: 'Seats/Row:' }))
        .toContainText(`Seats/Row: ${sector.seatsPerRow}`);
      
      // Check total seats
      await expect(sectorCard.locator('.sector-details div .badge.bg-primary'))
        .toContainText(`${sector.total} seats`);
    }
    
    // Verify VIP sectors (displayed differently)
    const vipSectorTests = [
      { name: 'VIP East Premium', rows: 15, seatsPerRow: 16, total: 240 },
      { name: 'VIP East Club', rows: 12, seatsPerRow: 14, total: 168 }
    ];
    
    for (const vipSector of vipSectorTests) {
      const vipCard = page.locator(`.sector-card:has(.sector-name:has-text("${vipSector.name}"))`).first();
      await expect(vipCard).toBeVisible();
      
      // VIP sections use compact display format
      await expect(vipCard.locator('div').filter({ hasText: `${vipSector.rows}×${vipSector.seatsPerRow}` }))
        .toBeVisible();
      await expect(vipCard.locator('.badge.bg-secondary'))
        .toContainText(vipSector.total.toString());
    }
  });

  test('should handle import errors gracefully', async ({ page }) => {
    await page.goto('/admin/stadium-structure');
    
    // Upload an invalid JSON file
    await page.locator('input[type="file"]').setInputFiles({
      name: 'invalid.json',
      mimeType: 'application/json',
      buffer: Buffer.from('{ invalid json content')
    });
    
    await page.locator('button:has-text("Import Structure")').click();
    
    // Check for error message
    await expect(page.locator('.alert-danger')).toBeVisible({ timeout: 10000 });
    await expect(page.locator('.alert-danger')).toContainText('Import Error:');
  });

  test('should display empty state when no structure exists', async ({ page }) => {
    // Clear any existing structure first
    await page.goto('/admin/stadium-structure');
    
    // Check if there's existing data to clear
    const clearButton = page.locator('button:has-text("Clear Structure")');
    if (await clearButton.isEnabled()) {
      await clearButton.click();
      await page.locator('button:has-text("Delete Structure")').click();
      await expect(page.locator('.alert-success')).toBeVisible({ timeout: 10000 });
    }
    
    // Navigate to overview
    await page.goto('/admin/stadium-overview');
    
    // Verify empty state is displayed
    await expect(page.locator('.alert-info')).toBeVisible();
    await expect(page.locator('.alert-info h5')).toContainText('No Stadium Structure');
    await expect(page.locator('.alert-info p')).toContainText('No stadium structure data found');
    await expect(page.locator('.alert-info a[href="/admin/stadium-structure"]')).toBeVisible();
  });
});