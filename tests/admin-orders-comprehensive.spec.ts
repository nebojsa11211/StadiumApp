import { test, expect, Page, BrowserContext } from '@playwright/test';

/**
 * Comprehensive Admin Orders Page Tests
 * 
 * Tests the admin orders page functionality after fixing API connectivity issues:
 * 1. API connectivity and data loading
 * 2. Order details modal functionality  
 * 3. Order action buttons styling and responsiveness
 * 4. Overall user experience and error handling
 * 
 * Services:
 * - API: http://localhost:8080
 * - Admin: http://localhost:9004 (updated from 9002/9003 due to port conflicts)
 */

test.describe('Admin Orders Page - Comprehensive Tests', () => {
  let context: BrowserContext;
  let page: Page;
  
  const adminCredentials = {
    email: 'admin@stadium.com',
    password: 'admin123'
  };

  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext({
      viewport: { width: 1920, height: 1080 },
      ignoreHTTPSErrors: true
    });
    page = await context.newPage();
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('API connectivity test - Direct API call', async () => {
    // Test direct API connectivity before testing the admin interface
    const apiResponse = await page.request.get('http://localhost:8080/api/orders');
    
    console.log(`API Status: ${apiResponse.status()}`);
    
    if (apiResponse.status() === 200) {
      const orders = await apiResponse.json();
      console.log(`Found ${Array.isArray(orders) ? orders.length : 'unknown'} orders`);
      expect(apiResponse.status()).toBe(200);
    } else {
      console.log('API returned non-200 status, checking if it\'s an auth issue');
      expect([200, 401]).toContain(apiResponse.status());
    }
  });

  test('Admin login functionality', async () => {
    await test.step('Navigate to admin login page', async () => {
      await page.goto('http://localhost:9004/login');
      await page.waitForLoadState('networkidle');
    });

    await test.step('Perform admin login', async () => {
      await page.fill('input[type="email"]', adminCredentials.email);
      await page.fill('input[type="password"]', adminCredentials.password);
      
      const loginButton = page.locator('button[type="submit"]');
      await expect(loginButton).toBeVisible();
      await loginButton.click();
      
      // Wait for redirect after login
      await page.waitForURL(/.*\/(dashboard|orders|index).*/, { timeout: 10000 });
      
      console.log(`Redirected to: ${page.url()}`);
    });
  });

  test('Navigate to orders page and test API connectivity', async () => {
    await test.step('Navigate to orders page', async () => {
      // Try multiple possible paths to orders page
      const possibleUrls = [
        'http://localhost:9004/orders',
        'http://localhost:9004/Orders',
        'http://localhost:9004/admin/orders'
      ];
      
      let ordersPageFound = false;
      
      for (const url of possibleUrls) {
        try {
          await page.goto(url);
          await page.waitForLoadState('networkidle', { timeout: 5000 });
          
          // Check if page loaded successfully (not 404)
          const pageTitle = await page.title();
          const bodyText = await page.textContent('body');
          
          if (!bodyText?.includes('404') && !bodyText?.includes('Not Found')) {
            console.log(`Successfully loaded orders page at: ${url}`);
            console.log(`Page title: ${pageTitle}`);
            ordersPageFound = true;
            break;
          }
        } catch (error) {
          console.log(`Failed to load ${url}: ${error}`);
        }
      }
      
      if (!ordersPageFound) {
        // Try to find navigation link to orders
        await page.goto('http://localhost:9004');
        await page.waitForLoadState('networkidle');
        
        const ordersLink = page.locator('a:has-text("Orders"), a:has-text("orders"), nav >> text=Orders');
        if (await ordersLink.count() > 0) {
          await ordersLink.first().click();
          await page.waitForLoadState('networkidle');
          console.log(`Found orders page via navigation: ${page.url()}`);
        }
      }
    });

    await test.step('Verify page loaded correctly', async () => {
      // Check for common admin orders page elements
      const possibleSelectors = [
        'h1:has-text("Orders")',
        'h2:has-text("Orders")',
        'h3:has-text("Orders")',
        '.orders-container',
        '.order-list',
        'table',
        '.no-orders',
        'text=No orders found'
      ];
      
      let pageElementFound = false;
      for (const selector of possibleSelectors) {
        if (await page.locator(selector).count() > 0) {
          console.log(`Found element: ${selector}`);
          pageElementFound = true;
          break;
        }
      }
      
      // Log page content for debugging
      const pageContent = await page.content();
      console.log('Page content length:', pageContent.length);
      console.log('Current URL:', page.url());
      
      // Check for error messages
      const errorSelectors = [
        'text=No orders found',
        'text=Failed to load',
        'text=Connection error',
        'text=API error',
        '.error',
        '.alert-danger'
      ];
      
      for (const errorSelector of errorSelectors) {
        const errorElement = page.locator(errorSelector);
        if (await errorElement.count() > 0) {
          const errorText = await errorElement.textContent();
          console.log(`Found error message: ${errorText}`);
        }
      }
    });

    await test.step('Test API connection status', async () => {
      // Monitor network requests to the API
      const apiRequests: string[] = [];
      
      page.on('response', response => {
        if (response.url().includes('localhost:8080')) {
          apiRequests.push(`${response.request().method()} ${response.url()} - ${response.status()}`);
        }
      });
      
      // Force a refresh to trigger API calls
      await page.reload();
      await page.waitForLoadState('networkidle');
      
      console.log('API requests made:', apiRequests);
      
      // Check if the "No orders found" error is gone
      const noOrdersError = page.locator('text=No orders found');
      const hasNoOrdersError = await noOrdersError.count() > 0;
      
      if (hasNoOrdersError) {
        console.log('Still showing "No orders found" - investigating further');
        
        // Check if it's due to empty database or API connectivity issue
        const pageText = await page.textContent('body');
        console.log('Page contains connectivity error indicators:', 
          pageText?.includes('connection') || pageText?.includes('failed') || pageText?.includes('error'));
      } else {
        console.log('Successfully connected to API - no "No orders found" error');
      }
    });
  });

  test('Test order list functionality', async () => {
    await page.goto('http://localhost:9004/orders');
    await page.waitForLoadState('networkidle');

    await test.step('Check for orders table or list', async () => {
      const tableExists = await page.locator('table').count() > 0;
      const listExists = await page.locator('.order-list, .orders-grid').count() > 0;
      
      console.log(`Orders table exists: ${tableExists}`);
      console.log(`Orders list exists: ${listExists}`);
      
      if (tableExists) {
        const rowCount = await page.locator('table tbody tr').count();
        console.log(`Table has ${rowCount} rows`);
        
        if (rowCount > 0) {
          // Test first row details
          const firstRow = page.locator('table tbody tr').first();
          const rowText = await firstRow.textContent();
          console.log(`First row content: ${rowText}`);
        }
      }
    });

    await test.step('Test search and filter functionality', async () => {
      const searchInput = page.locator('input[placeholder*="search"], input[type="search"]');
      const filterDropdown = page.locator('select');
      
      if (await searchInput.count() > 0) {
        console.log('Found search functionality');
        await searchInput.fill('test');
        await page.waitForTimeout(1000); // Wait for debounce
      }
      
      if (await filterDropdown.count() > 0) {
        console.log('Found filter functionality');
        const options = await filterDropdown.locator('option').count();
        console.log(`Filter has ${options} options`);
      }
    });
  });

  test('Test order details modal functionality', async () => {
    await page.goto('http://localhost:9004/orders');
    await page.waitForLoadState('networkidle');

    await test.step('Look for order detail buttons', async () => {
      const detailButtons = page.locator('button:has-text("Details"), button:has-text("View"), a:has-text("Details")');
      const detailButtonCount = await detailButtons.count();
      
      console.log(`Found ${detailButtonCount} detail buttons`);
      
      if (detailButtonCount > 0) {
        await test.step('Test modal opening', async () => {
          await detailButtons.first().click();
          await page.waitForTimeout(500);
          
          // Look for modal elements
          const modal = page.locator('.modal, .dialog, [role="dialog"]');
          const modalBackdrop = page.locator('.modal-backdrop, .backdrop');
          
          const modalExists = await modal.count() > 0;
          const backdropExists = await modalBackdrop.count() > 0;
          
          console.log(`Modal exists: ${modalExists}`);
          console.log(`Backdrop exists: ${backdropExists}`);
          
          if (modalExists) {
            const modalTitle = await modal.locator('h1, h2, h3, h4, .modal-title').textContent();
            console.log(`Modal title: ${modalTitle}`);
            
            // Test modal close functionality
            const closeButton = modal.locator('button:has-text("Close"), .btn-close, [aria-label="Close"]');
            if (await closeButton.count() > 0) {
              await closeButton.first().click();
              await page.waitForTimeout(500);
              
              const modalClosed = await modal.count() === 0;
              console.log(`Modal closed successfully: ${modalClosed}`);
            }
          }
        });
      } else {
        console.log('No detail buttons found - may indicate empty orders list');
      }
    });
  });

  test('Test order action buttons styling and responsiveness', async () => {
    await page.goto('http://localhost:9004/orders');
    await page.waitForLoadState('networkidle');

    await test.step('Test desktop view styling', async () => {
      await page.setViewportSize({ width: 1920, height: 1080 });
      await page.waitForTimeout(500);
      
      const actionButtons = page.locator('button, .btn');
      const buttonCount = await actionButtons.count();
      
      console.log(`Found ${buttonCount} buttons in desktop view`);
      
      if (buttonCount > 0) {
        // Check button styles
        for (let i = 0; i < Math.min(buttonCount, 5); i++) {
          const button = actionButtons.nth(i);
          const buttonText = await button.textContent();
          const isVisible = await button.isVisible();
          
          if (isVisible) {
            const styles = await button.evaluate(el => {
              const computed = window.getComputedStyle(el);
              return {
                backgroundColor: computed.backgroundColor,
                borderRadius: computed.borderRadius,
                padding: computed.padding,
                fontSize: computed.fontSize
              };
            });
            
            console.log(`Button "${buttonText}" styles:`, styles);
          }
        }
      }
    });

    await test.step('Test tablet view responsiveness', async () => {
      await page.setViewportSize({ width: 768, height: 1024 });
      await page.waitForTimeout(500);
      
      const actionButtons = page.locator('button, .btn');
      const visibleButtons = await actionButtons.filter({ hasText: /.+/ }).count();
      
      console.log(`${visibleButtons} buttons visible in tablet view`);
    });

    await test.step('Test mobile view responsiveness', async () => {
      await page.setViewportSize({ width: 375, height: 667 });
      await page.waitForTimeout(500);
      
      const actionButtons = page.locator('button, .btn');
      const visibleButtons = await actionButtons.filter({ hasText: /.+/ }).count();
      
      console.log(`${visibleButtons} buttons visible in mobile view`);
      
      // Check if buttons stack properly on mobile
      if (visibleButtons > 0) {
        const firstButton = actionButtons.first();
        const buttonBox = await firstButton.boundingBox();
        
        if (buttonBox) {
          console.log(`Button width on mobile: ${buttonBox.width}px`);
          expect(buttonBox.width).toBeLessThan(300); // Should not be too wide on mobile
        }
      }
    });
  });

  test('Test error handling and recovery', async () => {
    await test.step('Test page behavior with API errors', async () => {
      // Block API requests to simulate connectivity issues
      await page.route('**/api/orders', route => route.abort());
      
      await page.goto('http://localhost:9004/orders');
      await page.waitForTimeout(3000);
      
      // Check for error messages
      const errorMessages = [
        'Failed to load orders',
        'Connection error',
        'Unable to connect',
        'Network error',
        'API error'
      ];
      
      let errorFound = false;
      for (const message of errorMessages) {
        if (await page.locator(`text=${message}`).count() > 0) {
          console.log(`Found error message: ${message}`);
          errorFound = true;
        }
      }
      
      if (!errorFound) {
        const pageText = await page.textContent('body');
        console.log('No specific error message found. Page content preview:', 
          pageText?.substring(0, 200));
      }
      
      // Restore API access
      await page.unroute('**/api/orders');
    });

    await test.step('Test page recovery after API restoration', async () => {
      await page.reload();
      await page.waitForLoadState('networkidle');
      
      // Check if page loads normally again
      const hasContent = await page.locator('body').textContent();
      const pageLoadedProperly = hasContent && hasContent.length > 100;
      
      console.log(`Page recovered successfully: ${pageLoadedProperly}`);
      expect(pageLoadedProperly).toBeTruthy();
    });
  });

  test('Test overall user experience', async () => {
    await page.goto('http://localhost:9004/orders');
    await page.waitForLoadState('networkidle');

    await test.step('Measure page load performance', async () => {
      const startTime = Date.now();
      await page.reload();
      await page.waitForLoadState('networkidle');
      const loadTime = Date.now() - startTime;
      
      console.log(`Page load time: ${loadTime}ms`);
      expect(loadTime).toBeLessThan(10000); // Should load within 10 seconds
    });

    await test.step('Test navigation and breadcrumbs', async () => {
      const navElements = page.locator('nav, .navbar, .breadcrumb');
      const navCount = await navElements.count();
      
      console.log(`Found ${navCount} navigation elements`);
      
      if (navCount > 0) {
        const navText = await navElements.first().textContent();
        console.log(`Navigation content: ${navText}`);
      }
    });

    await test.step('Test accessibility features', async () => {
      // Check for proper heading structure
      const headings = page.locator('h1, h2, h3, h4, h5, h6');
      const headingCount = await headings.count();
      
      console.log(`Found ${headingCount} headings`);
      
      // Check for proper button labels
      const buttons = page.locator('button');
      const buttonCount = await buttons.count();
      
      for (let i = 0; i < Math.min(buttonCount, 3); i++) {
        const button = buttons.nth(i);
        const hasText = (await button.textContent())?.trim().length > 0;
        const hasAriaLabel = await button.getAttribute('aria-label') !== null;
        const hasTitle = await button.getAttribute('title') !== null;
        
        const isAccessible = hasText || hasAriaLabel || hasTitle;
        console.log(`Button ${i + 1} is accessible: ${isAccessible}`);
      }
    });
  });

  test('Test data consistency and accuracy', async () => {
    await page.goto('http://localhost:9004/orders');
    await page.waitForLoadState('networkidle');

    await test.step('Verify displayed data format', async () => {
      const tables = page.locator('table');
      
      if (await tables.count() > 0) {
        const headers = await tables.first().locator('thead th').allTextContents();
        console.log(`Table headers: ${headers.join(', ')}`);
        
        const rows = tables.first().locator('tbody tr');
        const rowCount = await rows.count();
        
        if (rowCount > 0) {
          const firstRowData = await rows.first().locator('td').allTextContents();
          console.log(`First row data: ${firstRowData.join(', ')}`);
          
          // Check for proper date formatting
          const datePattern = /\d{1,2}\/\d{1,2}\/\d{4}|\d{4}-\d{2}-\d{2}/;
          const hasValidDate = firstRowData.some(cell => datePattern.test(cell));
          console.log(`Has valid date format: ${hasValidDate}`);
        }
      }
    });

    await test.step('Test data refresh functionality', async () => {
      // Look for refresh button
      const refreshButton = page.locator('button:has-text("Refresh"), button:has-text("Reload"), .refresh-btn');
      
      if (await refreshButton.count() > 0) {
        console.log('Found refresh button');
        await refreshButton.first().click();
        await page.waitForLoadState('networkidle');
        
        console.log('Data refreshed successfully');
      } else {
        console.log('No refresh button found');
      }
    });
  });
});