import { test, expect, Page, BrowserContext } from '@playwright/test';
import { loginAsAdmin } from './helpers/auth-helpers';
import { waitForPageInteractive } from './helpers/blazor-helpers';

/**
 * Comprehensive test to reproduce and investigate null reference exception 
 * that occurs after using "Clear All Logs" functionality in admin interface
 */

let adminContext: BrowserContext;
let adminPage: Page;
let consoleMessages: Array<{ type: string, text: string, location?: string }> = [];
let networkRequests: Array<{ url: string, method: string, status?: number, response?: any }> = [];

test.describe('Admin Logs Clear All Functionality - Null Reference Exception Investigation', () => {
  
  test.beforeAll(async ({ browser }) => {
    // Create dedicated browser context for admin
    adminContext = await browser.newContext({
      viewport: { width: 1280, height: 720 },
      ignoreHTTPSErrors: true
    });
    
    adminPage = await adminContext.newPage();
    
    // Set up console monitoring
    adminPage.on('console', msg => {
      consoleMessages.push({
        type: msg.type(),
        text: msg.text(),
        location: msg.location()?.url
      });
      
      // Log all console messages for debugging
      console.log(`[CONSOLE ${msg.type().toUpperCase()}] ${msg.text()}`);
      if (msg.location()) {
        console.log(`  Location: ${msg.location()?.url}:${msg.location()?.lineNumber}`);
      }
    });
    
    // Set up network request monitoring
    adminPage.on('request', request => {
      networkRequests.push({
        url: request.url(),
        method: request.method()
      });
      console.log(`[REQUEST] ${request.method()} ${request.url()}`);
    });
    
    adminPage.on('response', response => {
      const request = networkRequests.find(req => req.url === response.url());
      if (request) {
        request.status = response.status();
        console.log(`[RESPONSE] ${response.status()} ${response.url()}`);
      }
    });
    
    // Set up page error monitoring
    adminPage.on('pageerror', error => {
      console.log(`[PAGE ERROR] ${error.message}`);
      console.log(`[PAGE ERROR STACK] ${error.stack}`);
    });
    
    // Set up request failure monitoring
    adminPage.on('requestfailed', request => {
      console.log(`[REQUEST FAILED] ${request.method()} ${request.url()} - ${request.failure()?.errorText}`);
    });
  });
  
  test.afterAll(async () => {
    // Close admin context
    await adminContext?.close();
  });

  test('should reproduce null reference exception after clearing all logs', async () => {
    console.log('=== Starting Admin Logs Clear All Test ===');
    
    // Clear monitoring arrays
    consoleMessages = [];
    networkRequests = [];
    
    // Step 1: Navigate directly to admin login page
    console.log('Step 1: Navigating to admin login page...');
    await adminPage.goto('http://localhost:9002/login');
    await waitForPageInteractive(adminPage);
    
    // Step 2: Login as admin
    console.log('Step 2: Filling login form...');
    await adminPage.fill('#admin-login-email-input', 'admin@stadium.com');
    await adminPage.fill('#admin-login-password-input', 'admin123');
    await adminPage.click('#admin-login-submit-btn');
    
    // Wait for login to complete
    await adminPage.waitForURL('http://localhost:9002/', { timeout: 30000 });
    console.log('Admin login completed');
    
    // Step 3: Navigate to logs page
    console.log('Step 3: Navigating to logs page...');
    await adminPage.goto('http://localhost:9002/logs');
    await waitForPageInteractive(adminPage);
    console.log('Logs page loaded');
    
    // Step 4: Wait for page to fully load and take initial screenshot
    await adminPage.waitForSelector('#admin-logs-clear-btn', { timeout: 30000 });
    await adminPage.screenshot({ path: 'logs-page-before-clear.png', fullPage: true });
    console.log('Initial logs page screenshot taken');
    
    // Step 5: Verify initial state before clearing logs
    console.log('Step 5: Verifying initial state...');
    
    // Check if log summary is visible
    const summaryExists = await adminPage.locator('.card.bg-info').isVisible().catch(() => false);
    console.log(`Log summary cards visible: ${summaryExists}`);
    
    // Get initial log counts if available
    if (summaryExists) {
      const totalLogs = await adminPage.locator('.card.bg-info h3').textContent().catch(() => 'N/A');
      const errors = await adminPage.locator('.card.bg-danger h3').textContent().catch(() => 'N/A');
      console.log(`Initial state - Total logs: ${totalLogs}, Errors: ${errors}`);
    }
    
    // Step 6: Clear all logs and monitor for exceptions
    console.log('Step 6: Clicking Clear All Logs button...');
    
    // Monitor for dialog events
    adminPage.on('dialog', async dialog => {
      console.log(`[DIALOG] Type: ${dialog.type()}, Message: ${dialog.message()}`);
      await dialog.accept();
      console.log('[DIALOG] Accepted confirmation dialog');
    });
    
    // Click the clear all logs button
    await adminPage.click('#admin-logs-clear-btn');
    console.log('Clear All Logs button clicked');
    
    // Step 7: Wait for operation to complete and monitor for errors
    console.log('Step 7: Waiting for clear operation to complete...');
    
    // Wait for either success or error message
    try {
      await adminPage.waitForSelector('.alert', { timeout: 15000 });
      console.log('Alert message appeared');
      
      const alertText = await adminPage.locator('.alert').textContent();
      const isSuccess = await adminPage.locator('.alert-success').isVisible().catch(() => false);
      const isError = await adminPage.locator('.alert-danger').isVisible().catch(() => false);
      
      console.log(`Alert text: "${alertText}"`);
      console.log(`Is success alert: ${isSuccess}`);
      console.log(`Is error alert: ${isError}`);
      
    } catch (error) {
      console.log('No alert message appeared within timeout');
    }
    
    // Step 8: Take screenshot after clear operation
    await adminPage.screenshot({ path: 'logs-page-after-clear.png', fullPage: true });
    console.log('Post-clear screenshot taken');
    
    // Step 9: Check for null reference exceptions in console
    console.log('Step 9: Analyzing console messages for null reference exceptions...');
    
    const errorMessages = consoleMessages.filter(msg => 
      msg.type === 'error' || 
      msg.text.toLowerCase().includes('null') ||
      msg.text.toLowerCase().includes('reference') ||
      msg.text.toLowerCase().includes('undefined') ||
      msg.text.toLowerCase().includes('exception')
    );
    
    if (errorMessages.length > 0) {
      console.log('=== FOUND POTENTIAL NULL REFERENCE ERRORS ===');
      errorMessages.forEach((msg, index) => {
        console.log(`Error ${index + 1}:`);
        console.log(`  Type: ${msg.type}`);
        console.log(`  Text: ${msg.text}`);
        console.log(`  Location: ${msg.location || 'N/A'}`);
        console.log('---');
      });
    }
    
    // Step 10: Check network requests for failures
    console.log('Step 10: Analyzing network requests...');
    
    const failedRequests = networkRequests.filter(req => 
      req.status && (req.status >= 400 || req.status === 0)
    );
    
    if (failedRequests.length > 0) {
      console.log('=== FOUND FAILED NETWORK REQUESTS ===');
      failedRequests.forEach((req, index) => {
        console.log(`Failed Request ${index + 1}:`);
        console.log(`  Method: ${req.method}`);
        console.log(`  URL: ${req.url}`);
        console.log(`  Status: ${req.status}`);
        console.log('---');
      });
    }
    
    // Step 11: Try refreshing page to see if issues persist
    console.log('Step 11: Refreshing page to test post-clear state...');
    
    const preRefreshMessages = consoleMessages.length;
    await adminPage.reload();
    await waitForPageInteractive(adminPage);
    
    // Wait a bit for any additional errors to appear
    await adminPage.waitForTimeout(3000);
    
    const postRefreshMessages = consoleMessages.slice(preRefreshMessages);
    const postRefreshErrors = postRefreshMessages.filter(msg => 
      msg.type === 'error' || 
      msg.text.toLowerCase().includes('null') ||
      msg.text.toLowerCase().includes('reference')
    );
    
    if (postRefreshErrors.length > 0) {
      console.log('=== FOUND ERRORS AFTER PAGE REFRESH ===');
      postRefreshErrors.forEach((msg, index) => {
        console.log(`Post-refresh Error ${index + 1}:`);
        console.log(`  Type: ${msg.type}`);
        console.log(`  Text: ${msg.text}`);
        console.log(`  Location: ${msg.location || 'N/A'}`);
        console.log('---');
      });
    }
    
    // Step 12: Take final screenshot
    await adminPage.screenshot({ path: 'logs-page-after-refresh.png', fullPage: true });
    console.log('Final screenshot taken');
    
    // Step 13: Validate log summary after clearing
    console.log('Step 13: Validating log summary state...');
    
    try {
      await adminPage.waitForSelector('.card.bg-info h3', { timeout: 10000 });
      
      const finalTotalLogs = await adminPage.locator('.card.bg-info h3').textContent();
      const finalErrors = await adminPage.locator('.card.bg-danger h3').textContent();
      
      console.log(`Final state - Total logs: ${finalTotalLogs}, Errors: ${finalErrors}`);
      
      // Check if counts are properly updated (should be 0 after clearing)
      if (finalTotalLogs !== '0') {
        console.log(`WARNING: Total logs count is "${finalTotalLogs}" instead of "0"`);
      }
      
    } catch (error) {
      console.log('Could not read final log counts - this might indicate the null reference issue');
      console.log(`Error: ${error.message}`);
    }
    
    // Step 14: Generate comprehensive report
    console.log('=== COMPREHENSIVE TEST REPORT ===');
    console.log(`Total console messages captured: ${consoleMessages.length}`);
    console.log(`Total network requests made: ${networkRequests.length}`);
    console.log(`Console errors found: ${errorMessages.length}`);
    console.log(`Failed network requests: ${failedRequests.length}`);
    
    if (errorMessages.length > 0 || failedRequests.length > 0) {
      console.log('\n🚨 POTENTIAL ISSUES DETECTED:');
      
      if (errorMessages.length > 0) {
        console.log(`- ${errorMessages.length} console error(s) detected`);
      }
      
      if (failedRequests.length > 0) {
        console.log(`- ${failedRequests.length} failed network request(s) detected`);
      }
    } else {
      console.log('\n✅ No obvious errors detected, but null reference exception may be subtle');
    }
    
    // Assertions to validate the test results
    expect(consoleMessages.length).toBeGreaterThan(0); // Should have some console activity
    
    // This assertion might fail if there are actual null reference exceptions
    // If it fails, that confirms the bug exists
    try {
      await expect(adminPage.locator('.card.bg-info h3')).toContainText('0');
      console.log('✅ Log count properly shows 0 after clearing');
    } catch (error) {
      console.log('❌ Log count assertion failed - possible null reference issue');
      throw new Error(`Log count validation failed: ${error.message}`);
    }
  });
  
  test('should test additional edge cases around log clearing', async () => {
    console.log('=== Testing Edge Cases ===');
    
    // Clear previous monitoring data
    consoleMessages = [];
    networkRequests = [];
    
    // Navigate back to logs page
    await adminPage.goto('http://localhost:9002/logs');
    await waitForPageInteractive(adminPage);
    
    // Test 1: Multiple rapid clicks on clear button
    console.log('Edge Case 1: Testing rapid multiple clicks on clear button...');
    
    adminPage.on('dialog', async dialog => {
      await dialog.accept();
    });
    
    // Try clicking the clear button multiple times quickly
    try {
      await adminPage.click('#admin-logs-clear-btn');
      await adminPage.click('#admin-logs-clear-btn');
      await adminPage.click('#admin-logs-clear-btn');
      console.log('Multiple clicks completed');
    } catch (error) {
      console.log(`Multiple clicks test error: ${error.message}`);
    }
    
    await adminPage.waitForTimeout(3000);
    
    // Test 2: Try filtering after clearing
    console.log('Edge Case 2: Testing filtering after clearing logs...');
    
    try {
      await adminPage.click('#admin-logs-filter-error-btn');
      console.log('Error filter clicked');
      
      await adminPage.waitForTimeout(2000);
      
      await adminPage.click('#admin-logs-filter-all-btn');
      console.log('All filter clicked');
      
    } catch (error) {
      console.log(`Filtering test error: ${error.message}`);
    }
    
    // Test 3: Try refresh button after clearing
    console.log('Edge Case 3: Testing refresh button after clearing...');
    
    try {
      await adminPage.click('#admin-logs-refresh-btn');
      console.log('Refresh button clicked');
      
      await adminPage.waitForTimeout(3000);
      
    } catch (error) {
      console.log(`Refresh test error: ${error.message}`);
    }
    
    // Final error analysis
    const edgeCaseErrors = consoleMessages.filter(msg => 
      msg.type === 'error' || 
      msg.text.toLowerCase().includes('null') ||
      msg.text.toLowerCase().includes('reference')
    );
    
    console.log(`Edge case testing found ${edgeCaseErrors.length} additional errors`);
    
    if (edgeCaseErrors.length > 0) {
      console.log('=== EDGE CASE ERRORS ===');
      edgeCaseErrors.forEach((msg, index) => {
        console.log(`Edge Case Error ${index + 1}: ${msg.text}`);
      });
    }
  });
});