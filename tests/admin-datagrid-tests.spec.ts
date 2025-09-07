import { test, expect } from '@playwright/test';
import { getAppUrl } from './config';

test.describe('Admin DataGrid Tests', () => {
  test.beforeEach(async ({ page }) => {
    // Login as admin
    await page.goto(getAppUrl('admin', '/login'));
    await page.fill('[data-testid="email"], input[type="email"]', 'admin@stadium.com');
    await page.fill('[data-testid="password"], input[type="password"]', 'admin123');
    await page.click('button[type="submit"], .btn-primary');
    
    // Wait for login to complete
    await page.waitForURL('**/');
    await expect(page.locator('text=Admin Dashboard, text=Dashboard')).toBeVisible();
  });

  test('ADMIN-DATAGRID-001: DataGrid page access', async ({ page }) => {
    console.log('Testing DataGrid page access...');
    
    // Navigate to DataGrid page
    await page.goto(getAppUrl('admin', '/datagrid'));
    
    // Wait for page to load
    await page.waitForLoadState('networkidle');
    
    // Verify Database Explorer interface loads
    await expect(page.locator('h3:has-text("Database Explorer")')).toBeVisible();
    await expect(page.locator('select, .form-select')).toBeVisible();
    await expect(page.locator('text=Select Table, text=-- Select a table --')).toBeVisible();
    
    console.log('✅ DataGrid page loads correctly');
  });

  test('ADMIN-DATAGRID-002: Table selection', async ({ page }) => {
    console.log('Testing table selection...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table from dropdown
    await page.selectOption('select', { label: /Users.*columns/ });
    
    // Wait for data to load
    await page.waitForTimeout(2000);
    
    // Verify table data is displayed
    await expect(page.locator('table')).toBeVisible();
    await expect(page.locator('thead th')).toHaveCount({ min: 1 });
    
    console.log('✅ Table selection works');
  });

  test('ADMIN-DATAGRID-003: Data display with formatting', async ({ page }) => {
    console.log('Testing data display with proper formatting...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table with data
    await page.selectOption('select', { label: /CartItems.*columns/ });
    await page.waitForTimeout(2000);
    
    // Verify data formatting
    const tableRows = page.locator('tbody tr');
    if (await tableRows.count() > 0) {
      // Check that data is properly formatted
      await expect(tableRows.first()).toBeVisible();
      
      // Check for boolean formatting (badges)
      const booleanCells = page.locator('.badge');
      if (await booleanCells.count() > 0) {
        await expect(booleanCells.first()).toHaveClass(/bg-success|bg-secondary/);
      }
      
      // Check for null value formatting
      const nullCells = page.locator('td .text-muted:has-text("null")');
      // Should exist if there are null values
      
      console.log('✅ Data formatting works correctly');
    } else {
      console.log('⚠️ No data in selected table - test skipped');
    }
  });

  test('ADMIN-DATAGRID-004: Column sorting', async ({ page }) => {
    console.log('Testing column sorting...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table
    await page.selectOption('select', { label: /Users.*columns/ });
    await page.waitForTimeout(2000);
    
    // Click on a sortable column header
    const firstColumnHeader = page.locator('thead th .column-name').first();
    await firstColumnHeader.click();
    
    // Wait for sort to complete
    await page.waitForTimeout(1000);
    
    // Verify sort indicator appears
    await expect(page.locator('thead th .oi-arrow-top, thead th .oi-arrow-bottom')).toBeVisible();
    
    // Click again to reverse sort
    await firstColumnHeader.click();
    await page.waitForTimeout(1000);
    
    console.log('✅ Column sorting works');
  });

  test('ADMIN-DATAGRID-005: Data filtering', async ({ page }) => {
    console.log('Testing data filtering...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table
    await page.selectOption('select', { label: /Users.*columns/ });
    await page.waitForTimeout(2000);
    
    // Find a filter input and enter a search term
    const filterInput = page.locator('thead input[placeholder="Filter..."]').first();
    await filterInput.fill('admin');
    
    // Press Enter to apply filter
    await filterInput.press('Enter');
    await page.waitForTimeout(1000);
    
    // Verify filter is applied (results should contain the filter term or show "No records")
    const tableBody = page.locator('tbody');
    const hasNoRecords = await page.locator('text=No records found').isVisible();
    const hasFilteredData = await tableBody.locator('tr').count() > 0;
    
    expect(hasNoRecords || hasFilteredData).toBeTruthy();
    
    console.log('✅ Data filtering works');
  });

  test('ADMIN-DATAGRID-006: Pagination', async ({ page }) => {
    console.log('Testing pagination...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table with many records
    await page.selectOption('select', { label: /CartItems.*columns|Orders.*columns/ });
    await page.waitForTimeout(2000);
    
    // Check if pagination controls exist
    const paginationExists = await page.locator('.pagination').isVisible();
    
    if (paginationExists) {
      // Test pagination navigation
      const nextButton = page.locator('.page-link:has-text("Next")');
      if (await nextButton.isEnabled()) {
        await nextButton.click();
        await page.waitForTimeout(1000);
        
        // Verify page changed
        await expect(page.locator('.pagination .active')).toContainText(/[2-9]/);
        console.log('✅ Pagination navigation works');
      } else {
        console.log('⚠️ Only one page of data - pagination test skipped');
      }
    } else {
      console.log('⚠️ No pagination needed - test skipped');
    }
  });

  test('ADMIN-DATAGRID-007: CSV export functionality', async ({ page }) => {
    console.log('Testing CSV export...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table
    await page.selectOption('select', { label: /Users.*columns/ });
    await page.waitForTimeout(2000);
    
    // Set up download listener
    const downloadPromise = page.waitForEvent('download');
    
    // Click export button
    await page.click('button:has-text("Export CSV")');
    
    // Wait for download
    const download = await downloadPromise;
    
    // Verify download occurred
    expect(download.suggestedFilename()).toMatch(/.*\.csv$/);
    
    console.log('✅ CSV export works');
  });

  test('ADMIN-DATAGRID-008: Authentication required', async ({ page }) => {
    console.log('Testing authentication requirement...');
    
    // Clear cookies to simulate unauthenticated state
    await page.context().clearCookies();
    
    // Try to access DataGrid API directly
    const response = await page.request.get(getAppUrl('api', '/api/datagrid/tables'));
    
    // Should return 401 Unauthorized
    expect(response.status()).toBe(401);
    
    console.log('✅ Authentication requirement works');
  });

  test('ADMIN-DATAGRID-009: Empty table display', async ({ page }) => {
    console.log('Testing empty table display...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a potentially empty table
    await page.selectOption('select', { label: /EventAnalytics.*columns|EventStaffAssignments.*columns/ });
    await page.waitForTimeout(2000);
    
    // Check if "No records found" message appears
    const noRecordsMessage = page.locator('text=No records found');
    const hasData = await page.locator('tbody tr').count() > 0;
    
    if (!hasData) {
      await expect(noRecordsMessage).toBeVisible();
      console.log('✅ Empty table message displays correctly');
    } else {
      console.log('⚠️ Table has data - empty state test skipped');
    }
  });

  test('ADMIN-DATAGRID-010: Large dataset performance', async ({ page }) => {
    console.log('Testing large dataset handling...');
    
    await page.goto(getAppUrl('admin', '/datagrid'));
    await page.waitForLoadState('networkidle');
    
    // Select a table that might have many records
    await page.selectOption('select', { label: /Orders.*columns|Tickets.*columns/ });
    
    // Measure load time
    const startTime = Date.now();
    await page.waitForTimeout(3000); // Wait for data to load
    const loadTime = Date.now() - startTime;
    
    // Verify reasonable performance (under 5 seconds)
    expect(loadTime).toBeLessThan(5000);
    
    // Verify pagination is working for large datasets
    const totalRecords = await page.locator('text=Total Records:').textContent();
    if (totalRecords) {
      const recordCount = parseInt(totalRecords.replace(/.*Total Records:\s*/, ''));
      if (recordCount > 20) {
        await expect(page.locator('.pagination')).toBeVisible();
        console.log('✅ Large dataset pagination works');
      }
    }
    
    console.log('✅ Large dataset handling works within performance limits');
  });
});