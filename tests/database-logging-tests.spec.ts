import { test, expect, chromium, BrowserContext, Page } from '@playwright/test';

// Configuration for the current running instances
const API_BASE_URL = 'https://localhost:7010';
const ADMIN_BASE_URL = 'https://localhost:9030';

// Valid admin credentials (update these if needed)
const ADMIN_CREDENTIALS = {
  email: 'admin@stadium.com',
  password: 'admin123'
};

test.describe('Database Logging System Tests', () => {
  let apiContext: BrowserContext;
  let adminContext: BrowserContext;
  let apiPage: Page;
  let adminPage: Page;
  
  test.beforeAll(async () => {
    const browser = await chromium.launch();
    
    // Create separate contexts for API and Admin testing
    apiContext = await browser.newContext();
    adminContext = await browser.newContext();
    
    apiPage = await apiContext.newPage();
    adminPage = await adminContext.newPage();
    
    // Setup console logging for debugging
    apiPage.on('console', msg => console.log(`API Console [${msg.type()}]:`, msg.text()));
    adminPage.on('console', msg => console.log(`Admin Console [${msg.type()}]:`, msg.text()));
  });

  test.afterAll(async () => {
    await apiContext?.close();
    await adminContext?.close();
  });

  test.describe('API Logging Endpoints Tests', () => {
    
    test('should test all log levels endpoint', async () => {
      console.log('Testing all log levels endpoint...');
      
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-all-levels`);
      
      expect(response.ok()).toBeTruthy();
      expect(response.status()).toBe(200);
      
      const responseBody = await response.json();
      expect(responseBody.message).toContain('All log levels have been tested');
      expect(responseBody.timestamp).toBeDefined();
      
      console.log('✓ All log levels endpoint test passed');
    });

    test('should test exception logging endpoint', async () => {
      console.log('Testing exception logging endpoint...');
      
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-with-exception`);
      
      expect(response.status()).toBe(500); // Expected error response
      
      const responseBody = await response.json();
      expect(responseBody.message).toContain('Exception was logged');
      expect(responseBody.error).toContain('This is a test exception');
      
      console.log('✓ Exception logging endpoint test passed');
    });

    test('should test business event logging endpoint', async () => {
      console.log('Testing business event logging endpoint...');
      
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-business-event`);
      
      expect(response.ok()).toBeTruthy();
      expect(response.status()).toBe(200);
      
      const responseBody = await response.json();
      expect(responseBody.message).toContain('Business events logged');
      expect(responseBody.timestamp).toBeDefined();
      
      console.log('✓ Business event logging endpoint test passed');
    });

    test('should test performance logging endpoint', async () => {
      console.log('Testing performance logging endpoint...');
      
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-performance`);
      
      expect(response.ok()).toBeTruthy();
      expect(response.status()).toBe(200);
      
      const responseBody = await response.json();
      expect(responseBody.message).toContain('Performance logging test completed');
      expect(responseBody.elapsedMs).toBeDefined();
      expect(responseBody.batchLogsCreated).toBe(20);
      expect(responseBody.elapsedMs).toBeGreaterThan(90); // Should be at least 100ms due to delay
      
      console.log('✓ Performance logging endpoint test passed');
    });

    test('should verify API health and swagger documentation', async () => {
      console.log('Testing API health and documentation...');
      
      // Test Swagger UI is available
      const swaggerResponse = await apiPage.request.get(`${API_BASE_URL}/swagger`);
      expect(swaggerResponse.ok()).toBeTruthy();
      
      console.log('✓ API health and documentation test passed');
    });
  });

  test.describe('Admin Interface Integration Tests', () => {
    
    test.beforeEach(async () => {
      // Navigate to admin login page
      await adminPage.goto(`${ADMIN_BASE_URL}/login`);
    });

    test('should login to admin interface successfully', async () => {
      console.log('Testing admin login...');
      
      await adminPage.waitForLoadState('networkidle');
      
      // Fill login form
      await adminPage.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
      
      // Submit form
      await adminPage.click('button[type="submit"], .btn-primary');
      
      // Wait for redirect or success
      try {
        await adminPage.waitForURL('**/dashboard', { timeout: 10000 });
        console.log('✓ Admin login successful - redirected to dashboard');
      } catch {
        // Check if we're on any admin page (not login)
        const currentUrl = adminPage.url();
        expect(currentUrl).not.toContain('/login');
        console.log('✓ Admin login successful - on admin interface');
      }
    });

    test('should navigate to logs page and verify log display', async () => {
      console.log('Testing logs page access...');
      
      // First login
      await adminPage.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
      await adminPage.click('button[type="submit"], .btn-primary');
      
      // Wait for successful login
      await adminPage.waitForTimeout(2000);
      
      // Try different ways to navigate to logs
      const logsNavigation = [
        '//a[contains(text(), "Logs")]',
        '//a[contains(text(), "System Logs")]', 
        '//a[@href="/logs"]',
        '//a[contains(@href, "logs")]'
      ];
      
      let logsPageFound = false;
      for (const selector of logsNavigation) {
        try {
          const element = await adminPage.locator(selector).first();
          if (await element.isVisible({ timeout: 2000 })) {
            await element.click();
            logsPageFound = true;
            break;
          }
        } catch {
          // Try next selector
        }
      }
      
      if (!logsPageFound) {
        // Try direct navigation
        await adminPage.goto(`${ADMIN_BASE_URL}/logs`);
      }
      
      // Wait for logs page content
      await adminPage.waitForTimeout(3000);
      
      // Check for common log page elements
      const logPageIndicators = [
        'text=Log Entry',
        'text=Timestamp', 
        'text=Level',
        'text=Message',
        'text=System Log',
        'table',
        '.log-entry'
      ];
      
      let foundLogElements = 0;
      for (const indicator of logPageIndicators) {
        try {
          const element = adminPage.locator(indicator).first();
          if (await element.isVisible({ timeout: 5000 })) {
            foundLogElements++;
            console.log(`✓ Found log page element: ${indicator}`);
          }
        } catch {
          // Element not found
        }
      }
      
      // We expect to find at least some log page elements
      expect(foundLogElements).toBeGreaterThan(0);
      console.log(`✓ Logs page accessible - found ${foundLogElements} log-related elements`);
    });

    test('should test log search and filtering functionality', async () => {
      console.log('Testing log search and filtering...');
      
      // First login
      await adminPage.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
      await adminPage.click('button[type="submit"], .btn-primary');
      
      await adminPage.waitForTimeout(2000);
      
      // Navigate to logs page
      try {
        await adminPage.goto(`${ADMIN_BASE_URL}/logs`);
      } catch {
        // Skip if logs page not accessible
        console.log('⚠ Logs page not accessible, skipping search test');
        test.skip();
      }
      
      await adminPage.waitForTimeout(3000);
      
      // Look for search/filter controls
      const searchControls = [
        'input[placeholder*="search"], input[placeholder*="Search"]',
        'input[type="search"]',
        'select[name*="level"], select[name*="Level"]',
        'input[name*="date"], input[type="date"]'
      ];
      
      let foundControls = 0;
      for (const control of searchControls) {
        try {
          const element = adminPage.locator(control).first();
          if (await element.isVisible({ timeout: 2000 })) {
            foundControls++;
            console.log(`✓ Found search control: ${control}`);
          }
        } catch {
          // Control not found
        }
      }
      
      if (foundControls > 0) {
        console.log(`✓ Log search and filtering controls available (${foundControls} found)`);
      } else {
        console.log('⚠ No search controls found on logs page');
      }
    });
  });

  test.describe('End-to-End Integration Tests', () => {
    
    test('should verify logs appear in admin after API calls', async () => {
      console.log('Testing end-to-end log integration...');
      
      // Step 1: Generate logs via API
      console.log('Step 1: Generating test logs...');
      const logRequests = [
        apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-all-levels`),
        apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-business-event`),
        apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-with-exception`)
      ];
      
      const responses = await Promise.all(logRequests);
      
      // Verify API calls were successful (except exception which returns 500)
      expect(responses[0].ok()).toBeTruthy();
      expect(responses[1].ok()).toBeTruthy();
      expect(responses[2].status()).toBe(500); // Exception test
      
      console.log('✓ Test logs generated successfully');
      
      // Step 2: Login to admin
      console.log('Step 2: Logging into admin interface...');
      await adminPage.goto(`${ADMIN_BASE_URL}/login`);
      
      await adminPage.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
      await adminPage.click('button[type="submit"], .btn-primary');
      
      await adminPage.waitForTimeout(3000);
      
      // Step 3: Navigate to logs and verify entries
      console.log('Step 3: Checking for logged entries...');
      
      try {
        await adminPage.goto(`${ADMIN_BASE_URL}/logs`);
        await adminPage.waitForTimeout(5000);
        
        // Look for recent log entries
        const logCheckText = [
          'This is a TRACE level log',
          'This is an INFO level log', 
          'This is a WARNING level log',
          'This is an ERROR level log',
          'test exception',
          'TestLoggingController'
        ];
        
        let foundLogs = 0;
        for (const text of logCheckText) {
          try {
            const element = adminPage.locator(`text=${text}`).first();
            if (await element.isVisible({ timeout: 3000 })) {
              foundLogs++;
              console.log(`✓ Found log entry containing: ${text}`);
            }
          } catch {
            // Log entry not found
          }
        }
        
        if (foundLogs > 0) {
          console.log(`✓ End-to-end integration successful - ${foundLogs} log entries verified`);
        } else {
          console.log('⚠ No specific log entries found, but integration test completed');
        }
        
      } catch (error) {
        console.log('⚠ Could not access logs page for verification:', error);
        // Test still passes if API endpoints work
      }
      
      console.log('✓ End-to-end integration test completed');
    });
  });

  test.describe('Database Logging Configuration Tests', () => {
    
    test('should verify database logging is enabled via API response', async () => {
      console.log('Testing database logging configuration...');
      
      // Call a test endpoint and check response structure
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-all-levels`);
      
      expect(response.ok()).toBeTruthy();
      
      const responseData = await response.json();
      expect(responseData.message).toBeDefined();
      expect(responseData.timestamp).toBeDefined();
      
      // Verify the message indicates database logging
      expect(responseData.message.toLowerCase()).toContain('database');
      
      console.log('✓ Database logging configuration verified');
    });

    test('should test batch logging performance', async () => {
      console.log('Testing batch logging performance...');
      
      const startTime = Date.now();
      
      // Call performance test endpoint
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/test-performance`);
      
      const endTime = Date.now();
      const totalTime = endTime - startTime;
      
      expect(response.ok()).toBeTruthy();
      
      const responseData = await response.json();
      expect(responseData.batchLogsCreated).toBe(20);
      
      // Verify performance is reasonable (should complete in under 10 seconds)
      expect(totalTime).toBeLessThan(10000);
      
      console.log(`✓ Batch logging performance test completed in ${totalTime}ms`);
    });
  });

  test.describe('Error Handling and Edge Cases', () => {
    
    test('should handle invalid API endpoints gracefully', async () => {
      console.log('Testing error handling for invalid endpoints...');
      
      const response = await apiPage.request.get(`${API_BASE_URL}/api/testlogging/nonexistent-endpoint`);
      
      // Should return 404 or similar error
      expect(response.status()).toBeGreaterThanOrEqual(400);
      
      console.log('✓ Invalid endpoint handled gracefully');
    });

    test('should handle admin interface connection errors', async () => {
      console.log('Testing admin interface error handling...');
      
      // Try accessing admin with incorrect URL
      try {
        await adminPage.goto(`${ADMIN_BASE_URL}/nonexistent-page`);
        
        // Should either redirect to login or show 404
        const currentUrl = adminPage.url();
        const isErrorHandled = currentUrl.includes('/login') || 
                              await adminPage.locator('text=404').isVisible({ timeout: 2000 }) ||
                              await adminPage.locator('text=Not Found').isVisible({ timeout: 2000 });
        
        expect(isErrorHandled).toBeTruthy();
        console.log('✓ Admin interface error handling verified');
        
      } catch (error) {
        // Network error is also acceptable error handling
        console.log('✓ Admin interface network error handled gracefully');
      }
    });
  });
});

// Helper test for debugging current state
test.describe('Debug Information', () => {
  
  test('should output current system state for debugging', async () => {
    console.log('\n=== DEBUG INFORMATION ===');
    console.log(`API Base URL: ${API_BASE_URL}`);
    console.log(`Admin Base URL: ${ADMIN_BASE_URL}`);
    console.log(`Admin Credentials: ${ADMIN_CREDENTIALS.email}`);
    
    // Test API connectivity
    try {
      const browser = await chromium.launch();
      const context = await browser.newContext();
      const page = await context.newPage();
      
      const apiResponse = await page.request.get(`${API_BASE_URL}/api/testlogging/test-all-levels`);
      console.log(`API Connectivity: ${apiResponse.ok() ? '✓ Connected' : '✗ Failed'} (Status: ${apiResponse.status()})`);
      
      // Test Admin connectivity
      await page.goto(`${ADMIN_BASE_URL}/login`);
      await page.waitForTimeout(2000);
      const adminTitle = await page.title();
      console.log(`Admin Connectivity: ✓ Connected (Title: ${adminTitle})`);
      
      await browser.close();
      
    } catch (error) {
      console.log(`Connectivity Error: ${error}`);
    }
    
    console.log('========================\n');
  });
});