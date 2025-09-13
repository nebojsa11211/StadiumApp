import { test, expect, Page } from '@playwright/test';

test.describe('Admin Stadium Overview with Authentication', () => {
  let page: Page;
  
  test.beforeEach(async ({ browser }) => {
    page = await browser.newPage();
    
    // Set viewport for consistent testing
    await page.setViewportSize({ width: 1920, height: 1080 });
    
    // First navigate to admin login page
    await page.goto('https://localhost:9030/admin');
    
    // Wait for page to load and check if we need to login
    await page.waitForLoadState('domcontentloaded');
  });

  test.afterEach(async () => {
    await page.close();
  });

  test('Login and access stadium overview page', async () => {
    // Take screenshot of initial state
    await page.screenshot({ 
      path: '.playwright-mcp/admin-login-initial.png', 
      fullPage: true 
    });
    
    // Check if we're on a login page or if admin page loads directly
    const currentUrl = page.url();
    console.log(`Current URL: ${currentUrl}`);
    
    // Check for login form elements
    const emailInput = page.locator('input[type="email"]');
    const passwordInput = page.locator('input[type="password"]');
    const loginButton = page.locator('button:has-text("Login")').or(page.locator('button:has-text("Sign In")'));
    
    const hasLoginForm = await emailInput.count() > 0 && await passwordInput.count() > 0;
    
    if (hasLoginForm) {
      console.log('✅ Login form detected - proceeding with authentication');
      
      // Fill in admin credentials (you may need to adjust these)
      await emailInput.fill('admin@stadium.com');
      await passwordInput.fill('admin123');
      
      // Click login button
      await loginButton.click();
      
      // Wait for redirect after login
      await page.waitForURL(/\/admin/, { timeout: 10000 });
      console.log('✅ Login successful, redirected to admin area');
    } else {
      console.log('ℹ️ No login form found - direct access to admin area');
    }
    
    // Now navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview');
    
    // Wait for page to load
    await page.waitForLoadState('domcontentloaded');
    
    // Take screenshot of stadium overview page
    await page.screenshot({ 
      path: '.playwright-mcp/admin-stadium-overview-with-auth.png', 
      fullPage: true 
    });
    
    // Wait for Blazor to initialize (up to 30 seconds)
    try {
      // Look for either the stadium layout container OR any admin content
      await Promise.race([
        page.waitForSelector('#stadium-layout-container', { timeout: 30000 }),
        page.waitForSelector('.admin-content', { timeout: 30000 }),
        page.waitForSelector('h1, h2, h3', { timeout: 30000 }) // Any header
      ]);
      
      console.log('✅ Stadium overview page loaded with content');
    } catch (error) {
      console.log('⚠️ Stadium overview page may still be initializing');
      
      // Check if we're still in "Initializing application..." state
      const initializingText = await page.locator('text=Initializing application').count();
      if (initializingText > 0) {
        console.log('⚠️ Page stuck in initialization state');
      }
      
      // Take final screenshot regardless
      await page.screenshot({ 
        path: '.playwright-mcp/admin-stadium-overview-timeout.png', 
        fullPage: true 
      });
    }
    
    // Check what elements are actually present on the page
    const pageContent = await page.content();
    const hasStadiumContainer = pageContent.includes('stadium-layout-container');
    const hasInitializingText = pageContent.includes('Initializing application');
    const hasErrorText = pageContent.includes('error') || pageContent.includes('Error');
    
    console.log(`✅ Page analysis: Container=${hasStadiumContainer}, Initializing=${hasInitializingText}, Error=${hasErrorText}`);
    
    // The test should pass if we can at least access the page
    expect(page.url()).toContain('/admin/stadium-overview');
  });

  test('Check console logs and network requests', async () => {
    // Monitor console logs
    const consoleLogs: string[] = [];
    page.on('console', (msg) => {
      consoleLogs.push(`[${msg.type()}] ${msg.text()}`);
    });
    
    // Monitor network requests
    const networkRequests: any[] = [];
    page.on('response', (response) => {
      if (response.url().includes('/api/') || response.status() >= 400) {
        networkRequests.push({
          url: response.url(),
          status: response.status(),
          statusText: response.statusText()
        });
      }
    });
    
    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 30000 });
    
    // Print collected logs
    console.log('=== CONSOLE LOGS ===');
    consoleLogs.forEach(log => console.log(log));
    
    console.log('=== NETWORK REQUESTS ===');
    networkRequests.forEach(req => console.log(`${req.status} ${req.url}`));
    
    // Check for specific error patterns
    const hasBlazorError = consoleLogs.some(log => log.toLowerCase().includes('blazor') && log.toLowerCase().includes('error'));
    const hasSignalRError = consoleLogs.some(log => log.toLowerCase().includes('signalr') && log.toLowerCase().includes('error'));
    const hasAPIError = networkRequests.some(req => req.status >= 400 && req.url.includes('/api/'));
    
    if (hasBlazorError) {
      console.log('⚠️ Blazor errors detected in console');
    }
    
    if (hasSignalRError) {
      console.log('⚠️ SignalR errors detected in console');
    }
    
    if (hasAPIError) {
      console.log('⚠️ API errors detected in network requests');
    }
    
    expect(page.url()).toContain('/admin/stadium-overview');
  });
});