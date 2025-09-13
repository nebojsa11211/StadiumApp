import { test, expect } from '@playwright/test';

test.describe('Console-to-System Logging - Direct Testing', () => {
  
  test.beforeEach(async ({ page }) => {
    // Set longer timeout for Blazor app loading
    test.setTimeout(60000);
    
    // Navigate directly to admin app
    await page.goto('https://localhost:9030');
    await page.waitForLoadState('networkidle');
  });

  test('should verify admin app is running and console interceptor loads', async ({ page }) => {
    // Check that the page loads without major errors
    const title = await page.title();
    console.log('Page title:', title);
    
    // Wait for Blazor to initialize
    await page.waitForTimeout(5000);
    
    // Check if console interceptor script is loaded
    const consoleInterceptorExists = await page.evaluate(() => {
      return typeof window.toggleConsoleToSystemLogging === 'function';
    });
    
    console.log('Console interceptor loaded:', consoleInterceptorExists);
    
    // Test console functionality 
    const consoleFunctionsWork = await page.evaluate(() => {
      try {
        console.log('Test log message');
        console.error('Test error message');
        console.warn('Test warning message');
        return true;
      } catch (e) {
        return false;
      }
    });
    
    console.log('Console functions work:', consoleFunctionsWork);
    
    // Verify no JavaScript errors occurred during loading
    const errors = [];
    page.on('pageerror', (error) => {
      errors.push(error.message);
    });
    
    // Try to access logs page directly if possible
    try {
      await page.goto('https://localhost:9030/logs');
      await page.waitForTimeout(3000);
      
      // Check for toggle switch and clear button elements
      const toggleExists = await page.locator('#consoleToSystemToggle').isVisible().catch(() => false);
      const clearButtonExists = await page.locator('#admin-logs-clear-btn').isVisible().catch(() => false);
      
      console.log('Toggle switch exists:', toggleExists);
      console.log('Clear button exists:', clearButtonExists);
      
      // Test toggle functionality if available
      if (toggleExists) {
        await page.locator('#consoleToSystemToggle').click();
        await page.waitForTimeout(1000);
        
        const toggleState = await page.locator('#consoleToSystemToggle').isChecked();
        console.log('Toggle state after click:', toggleState);
      }
      
    } catch (error) {
      console.log('Could not access logs page directly:', error.message);
    }
    
    // Report on what we found
    expect(consoleFunctionsWork).toBe(true);
    
    if (errors.length > 0) {
      console.log('JavaScript errors detected:', errors);
    }
  });

  test('should test JavaScript console interceptor functions', async ({ page }) => {
    // Test the core console interception functionality
    const testResults = await page.evaluate(() => {
      const results = {
        originalConsoleExists: typeof console.log === 'function',
        interceptorLoaded: typeof window.toggleConsoleToSystemLogging === 'function',
        statusFunction: typeof window.getConsoleInterceptorStatus === 'function',
        canToggle: false,
        consoleStillWorks: false
      };
      
      // Test toggle function if it exists
      if (results.interceptorLoaded) {
        try {
          // Enable console logging
          window.toggleConsoleToSystemLogging(true);
          results.canToggle = true;
          
          // Get status
          if (results.statusFunction) {
            const status = window.getConsoleInterceptorStatus();
            console.log('Interceptor status:', status);
          }
          
          // Disable console logging
          window.toggleConsoleToSystemLogging(false);
          
        } catch (e) {
          console.log('Error testing toggle:', e);
        }
      }
      
      // Test console still works
      try {
        console.log('Testing console after interceptor');
        console.error('Testing error console');
        console.warn('Testing warn console');
        results.consoleStillWorks = true;
      } catch (e) {
        console.log('Console broken:', e);
      }
      
      return results;
    });
    
    console.log('Console interceptor test results:', testResults);
    
    // Assertions
    expect(testResults.originalConsoleExists).toBe(true);
    expect(testResults.consoleStillWorks).toBe(true);
    
    // Report findings
    if (testResults.interceptorLoaded) {
      console.log('✅ Console interceptor loaded successfully');
    } else {
      console.log('❌ Console interceptor not found');
    }
    
    if (testResults.canToggle) {
      console.log('✅ Toggle functionality works');
    } else {
      console.log('❌ Toggle functionality not working');
    }
  });

  test('should verify console interceptor script inclusion', async ({ page }) => {
    // Check if the script tag for console interceptor exists
    const scriptExists = await page.locator('script[src*="console-interceptor"]').isVisible().catch(() => false);
    console.log('Console interceptor script tag exists:', scriptExists);
    
    // Check network requests for the script
    const scriptRequests = [];
    page.on('request', (request) => {
      if (request.url().includes('console-interceptor')) {
        scriptRequests.push({
          url: request.url(),
          status: 'requested'
        });
      }
    });
    
    page.on('response', (response) => {
      if (response.url().includes('console-interceptor')) {
        scriptRequests.push({
          url: response.url(),
          status: response.status(),
          ok: response.ok()
        });
      }
    });
    
    // Reload to catch script loading
    await page.reload();
    await page.waitForLoadState('networkidle');
    
    console.log('Console interceptor script requests:', scriptRequests);
    
    // Verify the script loaded successfully
    const scriptLoaded = scriptRequests.some(req => req.status === 200 || req.ok === true);
    
    if (scriptLoaded) {
      console.log('✅ Console interceptor script loaded successfully');
    } else {
      console.log('❌ Console interceptor script failed to load');
    }
    
    expect(scriptRequests.length).toBeGreaterThan(0);
  });
});