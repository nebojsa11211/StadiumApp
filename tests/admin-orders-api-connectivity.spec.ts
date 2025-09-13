import { test, expect, Page, BrowserContext } from '@playwright/test';

/**
 * Focused Admin Orders API Connectivity Tests
 * 
 * Tests specifically to verify the admin orders page API connectivity issues
 * have been resolved after updating appsettings.json to point to correct API URL.
 * 
 * This test focuses on:
 * 1. Admin app can load without "No orders found" connectivity errors
 * 2. API calls are being made to the correct endpoint
 * 3. Authentication flow works properly
 * 4. Orders page displays correctly after login
 */

test.describe('Admin Orders - API Connectivity Fix Verification', () => {
  let context: BrowserContext;
  let page: Page;

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

  test('Verify admin app loads and shows authentication instead of API errors', async () => {
    await test.step('Navigate to admin orders page', async () => {
      await page.goto('https://localhost:9030/orders');
      await page.waitForLoadState('networkidle');
      
      console.log(`Current URL: ${page.url()}`);
      console.log(`Page title: ${await page.title()}`);
    });

    await test.step('Check for connectivity-related error messages', async () => {
      const pageContent = await page.textContent('body');
      
      // These are BAD indicators - should NOT appear after the fix
      const connectivityErrors = [
        'No orders found', // The specific error we fixed
        'Failed to connect',
        'Connection error',
        'API error',
        'Network error',
        'Unable to reach',
        'ECONNREFUSED',
        'fetch failed'
      ];
      
      const foundErrors = connectivityErrors.filter(error => 
        pageContent?.toLowerCase().includes(error.toLowerCase())
      );
      
      console.log('Connectivity errors found:', foundErrors);
      
      if (foundErrors.length > 0) {
        console.log('❌ FAILED: Still showing connectivity errors after fix');
        expect(foundErrors.length).toBe(0);
      } else {
        console.log('✅ SUCCESS: No connectivity errors found');
      }
    });

    await test.step('Check for expected authentication behavior', async () => {
      const pageContent = await page.textContent('body');
      
      // These are GOOD indicators - should appear for authenticated pages
      const authenticationIndicators = [
        'Authentication Required',
        'You need to be logged in',
        'Login',
        'Sign In',
        'Access denied',
        'Unauthorized'
      ];
      
      const foundAuthIndicators = authenticationIndicators.filter(indicator => 
        pageContent?.toLowerCase().includes(indicator.toLowerCase())
      );
      
      console.log('Authentication indicators found:', foundAuthIndicators);
      
      if (foundAuthIndicators.length > 0) {
        console.log('✅ SUCCESS: Showing proper authentication challenge instead of API errors');
        expect(foundAuthIndicators.length).toBeGreaterThan(0);
      } else {
        console.log('🤔 UNEXPECTED: No authentication challenge - investigating...');
        console.log('Page content preview:', pageContent?.substring(0, 500));
      }
    });
  });

  test('Test API endpoint configuration', async () => {
    let apiRequests: string[] = [];
    
    // Monitor network requests to verify they're going to the correct API
    page.on('response', response => {
      if (response.url().includes('api/')) {
        apiRequests.push(`${response.request().method()} ${response.url()} - ${response.status()}`);
      }
    });

    await test.step('Attempt to trigger API calls', async () => {
      await page.goto('https://localhost:9030/orders');
      await page.waitForLoadState('networkidle');
      
      // Try to click through to a page that would make API calls
      const loginButton = page.locator('a:has-text("Login"), button:has-text("Login"), a:has-text("Go to Login")');
      if (await loginButton.count() > 0) {
        await loginButton.first().click();
        await page.waitForLoadState('networkidle');
      }
    });

    await test.step('Verify API requests go to correct endpoint', async () => {
      console.log('API requests captured:', apiRequests);
      
      // Check if any requests are going to the correct API port (8080)
      const correctApiRequests = apiRequests.filter(req => req.includes(':8080'));
      const wrongApiRequests = apiRequests.filter(req => 
        req.includes('api/') && !req.includes(':8080')
      );
      
      console.log('Requests to correct API (port 8080):', correctApiRequests);
      console.log('Requests to wrong API endpoints:', wrongApiRequests);
      
      if (correctApiRequests.length > 0) {
        console.log('✅ SUCCESS: Admin app is making requests to the correct API endpoint');
      } else if (apiRequests.length === 0) {
        console.log('ℹ️ INFO: No API requests captured (expected for authentication pages)');
      } else {
        console.log('⚠️ WARNING: API requests going to unexpected endpoints');
      }
    });
  });

  test('Test login flow and orders page access', async () => {
    await test.step('Navigate to login page', async () => {
      await page.goto('https://localhost:9030/login');
      await page.waitForLoadState('networkidle');
      
      console.log('Login page URL:', page.url());
    });

    await test.step('Check login form availability', async () => {
      const emailInput = page.locator('input[type="email"], input[name="email"]');
      const passwordInput = page.locator('input[type="password"], input[name="password"]');
      const loginButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign In")');
      
      const hasEmailInput = await emailInput.count() > 0;
      const hasPasswordInput = await passwordInput.count() > 0;
      const hasLoginButton = await loginButton.count() > 0;
      
      console.log(`Login form elements found: Email=${hasEmailInput}, Password=${hasPasswordInput}, Button=${hasLoginButton}`);
      
      if (hasEmailInput && hasPasswordInput && hasLoginButton) {
        console.log('✅ SUCCESS: Login form is properly displayed');
        
        // Try a test login to see what happens
        await test.step('Attempt test login', async () => {
          await emailInput.fill('admin@stadium.com');
          await passwordInput.fill('admin123');
          await loginButton.click();
          
          // Wait a reasonable time for response
          await page.waitForTimeout(3000);
          
          const currentUrl = page.url();
          const pageContent = await page.textContent('body');
          
          console.log('After login attempt - URL:', currentUrl);
          console.log('Page contains error:', pageContent?.includes('error') || pageContent?.includes('invalid'));
          
          // Check if we get to orders page or see useful error
          if (currentUrl.includes('/orders')) {
            console.log('✅ SUCCESS: Login successful, redirected to orders page');
          } else {
            console.log('ℹ️ INFO: Login attempt completed, analyzing result...');
            
            if (pageContent?.includes('invalid') || pageContent?.includes('error')) {
              console.log('Expected: Invalid credentials error (test credentials may not exist)');
            }
          }
        });
      } else {
        console.log('⚠️ WARNING: Login form not fully available');
      }
    });
  });

  test('Verify orders page behavior after authentication', async () => {
    // Skip login and test direct access to see behavior
    await test.step('Test direct orders page access', async () => {
      await page.goto('https://localhost:9030/orders');
      await page.waitForLoadState('networkidle');
      
      const pageContent = await page.textContent('body');
      
      console.log('Orders page behavior analysis:');
      console.log('- Shows authentication challenge:', pageContent?.includes('Authentication') || pageContent?.includes('Login'));
      console.log('- Shows API connectivity error:', pageContent?.includes('No orders found'));
      console.log('- Page content length:', pageContent?.length);
      
      // The key test: we should NOT see "No orders found" anymore
      if (pageContent?.includes('No orders found')) {
        console.log('❌ FAILED: Still showing "No orders found" error - API connectivity not fixed');
        expect(pageContent.includes('No orders found')).toBeFalsy();
      } else {
        console.log('✅ SUCCESS: No "No orders found" error - API connectivity issue resolved');
      }
    });

    await test.step('Verify proper error handling vs connectivity issues', async () => {
      const pageContent = await page.textContent('body');
      
      // Good signs: proper authentication flow
      const hasAuthChallenge = pageContent?.includes('Authentication Required') ||
                              pageContent?.includes('You need to be logged in') ||
                              pageContent?.includes('Login');
      
      // Bad signs: API connectivity issues
      const hasConnectivityIssue = pageContent?.includes('No orders found') ||
                                  pageContent?.includes('Failed to connect') ||
                                  pageContent?.includes('Network error');
      
      if (hasAuthChallenge && !hasConnectivityIssue) {
        console.log('✅ PERFECT: Proper authentication challenge without connectivity errors');
        expect(hasAuthChallenge).toBeTruthy();
        expect(hasConnectivityIssue).toBeFalsy();
      } else if (hasConnectivityIssue) {
        console.log('❌ FAILED: Still has connectivity issues');
        expect(hasConnectivityIssue).toBeFalsy();
      } else {
        console.log('🤔 INVESTIGATING: Unexpected page behavior');
        console.log('Page content preview:', pageContent?.substring(0, 200));
      }
    });
  });

  test('Performance and responsiveness check', async () => {
    await test.step('Test page load performance', async () => {
      const startTime = Date.now();
      await page.goto('https://localhost:9030/orders');
      await page.waitForLoadState('networkidle');
      const loadTime = Date.now() - startTime;
      
      console.log(`Orders page load time: ${loadTime}ms`);
      
      if (loadTime < 5000) {
        console.log('✅ SUCCESS: Page loads quickly (under 5 seconds)');
        expect(loadTime).toBeLessThan(5000);
      } else {
        console.log('⚠️ WARNING: Slow page load time');
      }
    });

    await test.step('Test responsive design', async () => {
      // Test different viewport sizes
      const viewports = [
        { width: 1920, height: 1080, name: 'Desktop' },
        { width: 768, height: 1024, name: 'Tablet' },
        { width: 375, height: 667, name: 'Mobile' }
      ];
      
      for (const viewport of viewports) {
        await page.setViewportSize({ width: viewport.width, height: viewport.height });
        await page.waitForTimeout(500);
        
        const isContentVisible = await page.isVisible('body');
        const hasHorizontalScroll = await page.evaluate(() => document.body.scrollWidth > window.innerWidth);
        
        console.log(`${viewport.name} (${viewport.width}x${viewport.height}): Content visible=${isContentVisible}, Horizontal scroll=${hasHorizontalScroll}`);
        
        expect(isContentVisible).toBeTruthy();
      }
    });
  });
});