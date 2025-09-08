import { test, expect, Page, Locator } from '@playwright/test';
import { loginAsAdmin } from './helpers/auth-helpers';
import { waitForPageInteractive, waitForElementStable, retryAction } from './helpers/blazor-helpers';

// Test configuration - using port 7005 for Admin app
const ADMIN_URL = 'http://localhost:7005';
const ADMIN_CREDENTIALS = {
  email: 'admin@stadium.com',
  password: 'password123'
};

class DataGridPage {
  constructor(private page: Page) {}

  // Locators for DataGrid page elements
  get tableSelector() { return this.page.locator('select.form-select'); }
  get generateButton() { return this.page.locator('button:has-text("Generate")'); }
  get deleteButton() { return this.page.locator('button:has-text("Clear Table")'); }
  get refreshButton() { return this.page.locator('button:has-text("Refresh")'); }
  get recordCountBadge() { return this.page.locator('.badge.bg-primary'); }
  
  // Modal elements
  get generateModal() { return this.page.locator('.modal.show'); }
  get deleteModal() { return this.page.locator('.modal.show'); }
  get recordCountInput() { return this.page.locator('#recordCount'); }
  get generateModalButton() { return this.page.locator('.modal .btn-warning:has-text("Generate")'); }
  get deleteModalButton() { return this.page.locator('.modal .btn-danger:has-text("Delete All Records")'); }
  get deleteConfirmationInput() { return this.page.locator('input[placeholder*="Type DELETE"]'); }
  get modalCloseButton() { return this.page.locator('.btn-close'); }
  
  // Toast notification
  get toastMessage() { return this.page.locator('.toast.show'); }
  get toastTitle() { return this.page.locator('.toast .toast-header strong'); }
  get toastBody() { return this.page.locator('.toast .toast-body'); }
  
  // Validation messages
  get generateValidationMessage() { return this.page.locator('.modal .alert-warning'); }
  get deleteValidationMessage() { return this.page.locator('.modal .alert-danger'); }

  async navigate() {
    await this.page.goto(`${ADMIN_URL}/datagrid`);
    await waitForPageInteractive(this.page);
  }

  async selectTable(tableName: string) {
    await this.tableSelector.selectOption(tableName);
    await waitForPageInteractive(this.page);
    // Wait for the table data to load
    await this.page.waitForTimeout(2000);
  }

  async getRecordCount(): Promise<number> {
    try {
      const badge = await this.recordCountBadge.textContent();
      if (badge) {
        const match = badge.match(/(\d+)\s+Records/);
        return match ? parseInt(match[1]) : 0;
      }
    } catch (error) {
      console.log('Could not get record count:', error);
    }
    return 0;
  }

  async clickGenerate() {
    await this.generateButton.click();
    await this.generateModal.waitFor({ state: 'visible', timeout: 10000 });
  }

  async clickDelete() {
    await this.deleteButton.click();
    await this.deleteModal.waitFor({ state: 'visible', timeout: 10000 });
  }

  async generateData(recordCount: number = 10) {
    // Set record count
    await this.recordCountInput.fill(recordCount.toString());
    
    // Click generate button
    await this.generateModalButton.click();
    
    // Wait for modal to close and toast to appear
    await this.generateModal.waitFor({ state: 'hidden', timeout: 30000 });
    await this.waitForToastMessage();
  }

  async deleteAllData() {
    // Type DELETE confirmation
    await this.deleteConfirmationInput.fill('DELETE');
    
    // Click delete button
    await this.deleteModalButton.click();
    
    // Wait for modal to close and toast to appear
    await this.deleteModal.waitFor({ state: 'hidden', timeout: 30000 });
    await this.waitForToastMessage();
  }

  async waitForToastMessage(timeout: number = 10000): Promise<string> {
    await this.toastMessage.waitFor({ state: 'visible', timeout });
    const message = await this.toastBody.textContent() || '';
    
    // Wait for toast to stabilize
    await this.page.waitForTimeout(1000);
    return message;
  }

  async closeModal() {
    await this.modalCloseButton.click();
    await this.page.waitForTimeout(500);
  }

  async hasValidationMessage(): Promise<boolean> {
    return (await this.generateValidationMessage.isVisible()) || (await this.deleteValidationMessage.isVisible());
  }

  async getValidationMessage(): Promise<string> {
    if (await this.generateValidationMessage.isVisible()) {
      return await this.generateValidationMessage.textContent() || '';
    }
    if (await this.deleteValidationMessage.isVisible()) {
      return await this.deleteValidationMessage.textContent() || '';
    }
    return '';
  }
}

test.describe('Admin DataGrid Data Management', () => {
  let page: Page;
  let dataGridPage: DataGridPage;

  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
    dataGridPage = new DataGridPage(page);
    
    // Login as admin
    await page.goto(`${ADMIN_URL}/login`);
    await waitForPageInteractive(page);
    
    // Fill login form
    await page.locator('input[type="email"]').fill(ADMIN_CREDENTIALS.email);
    await page.locator('input[type="password"]').fill(ADMIN_CREDENTIALS.password);
    await page.locator('button[type="submit"]').click();
    
    // Wait for login to complete
    await page.waitForURL(`${ADMIN_URL}/`, { timeout: 30000 });
    await waitForPageInteractive(page);
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('should load DataGrid page and display available tables', async () => {
    await dataGridPage.navigate();
    
    // Verify page loaded
    await expect(page.locator('h3:has-text("Database Explorer")')).toBeVisible();
    
    // Verify table selector is present
    await expect(dataGridPage.tableSelector).toBeVisible();
    
    // Verify table options are available
    const options = await dataGridPage.tableSelector.locator('option').count();
    expect(options).toBeGreaterThan(1); // Should have at least default option + tables
  });

  test('should show dependency validation error when generating Tickets without Events', async () => {
    await dataGridPage.navigate();
    
    // Select Tickets table
    await dataGridPage.selectTable('Ticket');
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Should show validation message
    expect(await dataGridPage.hasValidationMessage()).toBe(true);
    
    const validationMessage = await dataGridPage.getValidationMessage();
    expect(validationMessage.toLowerCase()).toContain('dependencies');
    
    await dataGridPage.closeModal();
  });

  test('should show dependency validation error when generating Orders without Users/Drinks', async () => {
    await dataGridPage.navigate();
    
    // Select Orders table
    await dataGridPage.selectTable('Order');
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Should show validation message
    expect(await dataGridPage.hasValidationMessage()).toBe(true);
    
    const validationMessage = await dataGridPage.getValidationMessage();
    expect(validationMessage.toLowerCase()).toContain('dependencies');
    
    await dataGridPage.closeModal();
  });

  test('should successfully generate Events data', async () => {
    await dataGridPage.navigate();
    
    // Select Events table
    await dataGridPage.selectTable('Event');
    
    // Get initial record count
    const initialCount = await dataGridPage.getRecordCount();
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Should not show validation error
    expect(await dataGridPage.hasValidationMessage()).toBe(false);
    
    // Generate 5 records
    await dataGridPage.generateData(5);
    
    // Verify toast message appears
    const toastMessage = await dataGridPage.waitForToastMessage();
    expect(toastMessage.toLowerCase()).toContain('generated');
    
    // Verify record count increased
    await page.waitForTimeout(3000); // Wait for UI to update
    const newCount = await dataGridPage.getRecordCount();
    expect(newCount).toBeGreaterThan(initialCount);
  });

  test('should successfully generate Tickets after Events exist', async () => {
    await dataGridPage.navigate();
    
    // First, ensure Events exist
    await dataGridPage.selectTable('Event');
    await dataGridPage.clickGenerate();
    
    if (await dataGridPage.hasValidationMessage()) {
      await dataGridPage.generateData(3);
      await dataGridPage.waitForToastMessage();
    } else {
      await dataGridPage.closeModal();
    }
    
    // Now try to generate Tickets
    await dataGridPage.selectTable('Ticket');
    await dataGridPage.clickGenerate();
    
    // Should not show validation error now
    if (!(await dataGridPage.hasValidationMessage())) {
      await dataGridPage.generateData(5);
      
      // Verify success message
      const toastMessage = await dataGridPage.waitForToastMessage();
      expect(toastMessage.toLowerCase()).toContain('generated');
    } else {
      // If still showing validation, close modal for cleanup
      await dataGridPage.closeModal();
    }
  });

  test('should generate Users data successfully', async () => {
    await dataGridPage.navigate();
    
    // Select Users table
    await dataGridPage.selectTable('User');
    
    // Get initial record count
    const initialCount = await dataGridPage.getRecordCount();
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Generate records
    await dataGridPage.generateData(3);
    
    // Verify success message
    const toastMessage = await dataGridPage.waitForToastMessage();
    expect(toastMessage.toLowerCase()).toContain('generated');
    
    // Verify record count increased
    await page.waitForTimeout(3000);
    const newCount = await dataGridPage.getRecordCount();
    expect(newCount).toBeGreaterThan(initialCount);
  });

  test('should generate Drinks data successfully', async () => {
    await dataGridPage.navigate();
    
    // Select Drinks table
    await dataGridPage.selectTable('Drink');
    
    // Get initial record count
    const initialCount = await dataGridPage.getRecordCount();
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Generate records
    await dataGridPage.generateData(5);
    
    // Verify success message
    const toastMessage = await dataGridPage.waitForToastMessage();
    expect(toastMessage.toLowerCase()).toContain('generated');
    
    // Verify record count increased
    await page.waitForTimeout(3000);
    const newCount = await dataGridPage.getRecordCount();
    expect(newCount).toBeGreaterThan(initialCount);
  });

  test('should show dependency validation error when deleting Events with Tickets', async () => {
    await dataGridPage.navigate();
    
    // Select Events table
    await dataGridPage.selectTable('Event');
    
    // Click delete
    await dataGridPage.clickDelete();
    
    // Should show validation message if Tickets exist
    if (await dataGridPage.hasValidationMessage()) {
      const validationMessage = await dataGridPage.getValidationMessage();
      expect(validationMessage.toLowerCase()).toContain('dependencies');
    }
    
    await dataGridPage.closeModal();
  });

  test('should show dependency validation error when deleting Users with Orders', async () => {
    await dataGridPage.navigate();
    
    // Select Users table
    await dataGridPage.selectTable('User');
    
    // Click delete
    await dataGridPage.clickDelete();
    
    // Should show validation message if Orders exist
    if (await dataGridPage.hasValidationMessage()) {
      const validationMessage = await dataGridPage.getValidationMessage();
      expect(validationMessage.toLowerCase()).toContain('dependencies');
    }
    
    await dataGridPage.closeModal();
  });

  test('should validate record count input boundaries', async () => {
    await dataGridPage.navigate();
    
    // Select Events table
    await dataGridPage.selectTable('Event');
    
    // Click generate
    await dataGridPage.clickGenerate();
    
    // Test boundary values
    const testValues = [0, -1, 1001];
    
    for (const value of testValues) {
      await dataGridPage.recordCountInput.fill(value.toString());
      
      // Check if input accepts the value or shows validation
      const inputValue = await dataGridPage.recordCountInput.inputValue();
      
      if (value <= 0 || value > 1000) {
        // Should either not accept the value or show validation
        expect(parseInt(inputValue) !== value || await dataGridPage.hasValidationMessage()).toBe(true);
      }
    }
    
    await dataGridPage.closeModal();
  });

  test('should validate DELETE confirmation for delete operation', async () => {
    await dataGridPage.navigate();
    
    // Select Events table (assuming it has data or can be deleted)
    await dataGridPage.selectTable('Event');
    
    // Click delete
    await dataGridPage.clickDelete();
    
    // If no validation error (no dependencies)
    if (!(await dataGridPage.hasValidationMessage())) {
      // Try with wrong confirmation text
      await dataGridPage.deleteConfirmationInput.fill('DELETE_WRONG');
      
      // Delete button should be disabled
      const deleteButton = dataGridPage.deleteModalButton;
      expect(await deleteButton.isDisabled()).toBe(true);
      
      // Try with correct confirmation text
      await dataGridPage.deleteConfirmationInput.fill('DELETE');
      
      // Delete button should be enabled
      expect(await deleteButton.isDisabled()).toBe(false);
    }
    
    await dataGridPage.closeModal();
  });

  test('should handle valid data generation workflow (Events -> Tickets -> Users -> Drinks -> Orders)', async () => {
    await dataGridPage.navigate();
    
    const workflow = [
      { table: 'Event', count: 2 },
      { table: 'Ticket', count: 3 },
      { table: 'User', count: 2 },
      { table: 'Drink', count: 4 },
      { table: 'Order', count: 3 }
    ];
    
    for (const step of workflow) {
      console.log(`Testing generation of ${step.table} with ${step.count} records`);
      
      await dataGridPage.selectTable(step.table);
      await dataGridPage.clickGenerate();
      
      if (await dataGridPage.hasValidationMessage()) {
        console.log(`Skipping ${step.table} due to validation error`);
        await dataGridPage.closeModal();
        continue;
      }
      
      await dataGridPage.generateData(step.count);
      
      const toastMessage = await dataGridPage.waitForToastMessage();
      expect(toastMessage.toLowerCase()).toContain('generated');
      
      // Wait for UI to update
      await page.waitForTimeout(2000);
    }
  });

  test('should handle refresh data functionality', async () => {
    await dataGridPage.navigate();
    
    // Select Events table
    await dataGridPage.selectTable('Event');
    
    // Get initial record count
    const initialCount = await dataGridPage.getRecordCount();
    
    // Click refresh
    await dataGridPage.refreshButton.click();
    
    // Wait for refresh to complete
    await waitForPageInteractive(page);
    await page.waitForTimeout(2000);
    
    // Record count should remain the same after refresh
    const refreshedCount = await dataGridPage.getRecordCount();
    expect(refreshedCount).toBe(initialCount);
  });

  test('should display toast notifications correctly', async () => {
    await dataGridPage.navigate();
    
    // Select Events table
    await dataGridPage.selectTable('Event');
    
    // Generate data to trigger toast
    await dataGridPage.clickGenerate();
    
    if (!(await dataGridPage.hasValidationMessage())) {
      await dataGridPage.generateData(2);
      
      // Verify toast appears and has correct structure
      await dataGridPage.waitForToastMessage();
      
      expect(await dataGridPage.toastMessage.isVisible()).toBe(true);
      expect(await dataGridPage.toastTitle.isVisible()).toBe(true);
      expect(await dataGridPage.toastBody.isVisible()).toBe(true);
      
      const title = await dataGridPage.toastTitle.textContent();
      expect(title).toBeTruthy();
    } else {
      await dataGridPage.closeModal();
    }
  });
});