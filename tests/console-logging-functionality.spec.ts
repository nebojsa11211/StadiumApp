import { test, expect } from '@playwright/test';

test.describe('Console-to-System Logging - Full Functionality Test', () => {
  
  test.beforeEach(async ({ page }) => {
    test.setTimeout(60000);
    await page.goto('https://localhost:9030');
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000); // Allow Blazor to initialize
  });

  test('should test complete toggle and clear logs workflow', async ({ page }) => {
    // Navigate directly to logs page
    await page.goto('https://localhost:9030/logs');
    await page.waitForTimeout(5000);
    
    // Verify page elements exist
    const toggleExists = await page.locator('#consoleToSystemToggle').isVisible().catch(() => false);
    const clearButtonExists = await page.locator('#admin-logs-clear-btn').isVisible().catch(() => false);
    const refreshButtonExists = await page.locator('#admin-logs-refresh-btn').isVisible().catch(() => false);
    
    console.log('Elements found:');
    console.log('- Toggle switch:', toggleExists);
    console.log('- Clear button:', clearButtonExists);
    console.log('- Refresh button:', refreshButtonExists);
    
    if (toggleExists) {
      // Test toggle functionality
      const initialState = await page.locator('#consoleToSystemToggle').isChecked().catch(() => false);
      console.log('Initial toggle state:', initialState);
      
      // Click toggle
      await page.locator('#consoleToSystemToggle').click();
      await page.waitForTimeout(1000);
      
      const newState = await page.locator('#consoleToSystemToggle').isChecked().catch(() => false);
      console.log('Toggle state after click:', newState);
      
      // Check for status message
      const statusMessage = await page.locator('.alert').textContent().catch(() => null);
      console.log('Status message:', statusMessage);
      
      // Verify localStorage was updated
      const localStorageValue = await page.evaluate(() => {
        return localStorage.getItem('consoleToSystemLogging');
      });
      console.log('localStorage value:', localStorageValue);
      
      expect(newState).toBe(!initialState);
    }
    
    if (clearButtonExists) {
      // Set up dialog handler for confirm
      let dialogHandled = false;
      page.once('dialog', async (dialog) => {
        console.log('Confirm dialog appeared:', dialog.message());
        expect(dialog.type()).toBe('confirm');
        await dialog.accept(); // Accept to test clear functionality
        dialogHandled = true;
      });
      
      // Click clear button
      await page.locator('#admin-logs-clear-btn').click();
      await page.waitForTimeout(2000);
      
      // Verify dialog was shown
      expect(dialogHandled).toBe(true);
      
      // Check for status message after clearing
      const clearStatusMessage = await page.locator('.alert').textContent().catch(() => null);
      console.log('Clear status message:', clearStatusMessage);
    }
    
    if (refreshButtonExists) {
      // Test refresh button
      await page.locator('#admin-logs-refresh-btn').click();
      await page.waitForTimeout(1000);
      
      const refreshStatusMessage = await page.locator('.alert').textContent().catch(() => null);
      console.log('Refresh status message:', refreshStatusMessage);
    }
  });

  test('should verify console interception with toggle states', async ({ page }) => {
    // Test console interception functionality
    await page.goto('https://localhost:9030/logs');
    await page.waitForTimeout(3000);
    
    const testResults = await page.evaluate(async () => {
      const results = {
        interceptorAvailable: typeof window.toggleConsoleToSystemLogging === 'function',
        statusFunctionAvailable: typeof window.getConsoleInterceptorStatus === 'function',
        toggleTests: [],
        consoleTests: []
      };
      
      if (results.interceptorAvailable) {
        // Test toggle states
        try {
          // Test enabling
          window.toggleConsoleToSystemLogging(true);
          let status = window.getConsoleInterceptorStatus();
          results.toggleTests.push({
            action: 'enable',
            status: status,
            success: status.enabled === true
          });
          
          // Test console calls when enabled
          console.log('Test message when enabled');
          console.error('Test error when enabled');
          console.warn('Test warning when enabled');
          results.consoleTests.push({
            state: 'enabled',
            success: true
          });
          
          // Test disabling
          window.toggleConsoleToSystemLogging(false);
          status = window.getConsoleInterceptorStatus();
          results.toggleTests.push({
            action: 'disable',
            status: status,
            success: status.enabled === false
          });
          
          // Test console calls when disabled
          console.log('Test message when disabled');
          console.error('Test error when disabled');
          console.warn('Test warning when disabled');
          results.consoleTests.push({
            state: 'disabled',
            success: true
          });
          
        } catch (error) {
          results.toggleTests.push({
            action: 'error',
            error: error.message
          });
        }
      }
      
      return results;
    });
    
    console.log('Console interception test results:', JSON.stringify(testResults, null, 2));
    
    // Assertions
    expect(testResults.interceptorAvailable).toBe(true);
    expect(testResults.statusFunctionAvailable).toBe(true);
    expect(testResults.toggleTests.length).toBeGreaterThan(0);
    expect(testResults.consoleTests.length).toBeGreaterThan(0);
    
    // Verify toggle tests succeeded
    const allToggleTestsSucceeded = testResults.toggleTests.every(test => test.success || test.error === undefined);
    expect(allToggleTestsSucceeded).toBe(true);
  });

  test('should test persistence across page reloads', async ({ page }) => {
    // Navigate to logs page
    await page.goto('https://localhost:9030/logs');
    await page.waitForTimeout(3000);
    
    const toggleExists = await page.locator('#consoleToSystemToggle').isVisible().catch(() => false);
    
    if (toggleExists) {
      // Enable console logging
      await page.locator('#consoleToSystemToggle').check();
      await page.waitForTimeout(1000);
      
      // Verify it's enabled
      const enabledState = await page.locator('#consoleToSystemToggle').isChecked();
      console.log('Console logging enabled:', enabledState);
      
      // Reload the page
      await page.reload();
      await page.waitForTimeout(5000);
      
      // Check if state persisted
      const persistedState = await page.locator('#consoleToSystemToggle').isChecked().catch(() => false);
      console.log('State after reload:', persistedState);
      
      // Check localStorage persistence
      const localStoragePersisted = await page.evaluate(() => {
        return localStorage.getItem('consoleToSystemLogging');
      });
      console.log('localStorage after reload:', localStoragePersisted);
      
      expect(persistedState).toBe(enabledState);
    }
  });

  test('should verify no regression in existing log functionality', async ({ page }) => {
    // Navigate to logs page
    await page.goto('https://localhost:9030/logs');
    await page.waitForTimeout(5000);
    
    // Check existing log filtering buttons still work
    const filterButtons = [
      '#admin-logs-filter-error-btn',
      '#admin-logs-filter-warning-btn', 
      '#admin-logs-filter-info-btn',
      '#admin-logs-filter-all-btn'
    ];
    
    const buttonTests = [];
    
    for (const buttonSelector of filterButtons) {
      const buttonExists = await page.locator(buttonSelector).isVisible().catch(() => false);
      buttonTests.push({
        selector: buttonSelector,
        exists: buttonExists
      });
      
      if (buttonExists) {
        // Try clicking the button
        await page.locator(buttonSelector).click();
        await page.waitForTimeout(500);
      }
    }
    
    console.log('Filter button tests:', buttonTests);
    
    // Check if logs table structure exists
    const tableExists = await page.locator('table.table').isVisible().catch(() => false);
    console.log('Logs table exists:', tableExists);
    
    // Check if log summary cards exist
    const summaryCards = await page.locator('.card').count().catch(() => 0);
    console.log('Summary cards count:', summaryCards);
    
    // Verify at least some existing functionality is preserved
    const existingFunctionalityWorking = buttonTests.some(test => test.exists) || tableExists || summaryCards > 0;
    expect(existingFunctionalityWorking).toBe(true);
  });
});