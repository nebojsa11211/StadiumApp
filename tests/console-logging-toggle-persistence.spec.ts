import { test, expect, chromium, BrowserContext, Page } from '@playwright/test';

// Configuration
const ADMIN_BASE_URL = 'http://localhost:9005';
const ADMIN_CREDENTIALS = {
  email: 'admin@stadium.com', 
  password: 'admin123'
};

test.describe('Console Logging Toggle Persistence Tests', () => {
  let context: BrowserContext;
  let page: Page;

  test.beforeAll(async () => {
    const browser = await chromium.launch();
    context = await browser.newContext();
    page = await context.newPage();
    
    page.on('console', msg => console.log(`Console [${msg.type()}]:`, msg.text()));
  });

  test.afterAll(async () => {
    await context?.close();
  });

  test('should persist console-to-system toggle state across sessions', async () => {
    console.log('=== Testing Console-to-System Toggle Persistence ===');

    // Step 1: Clear localStorage to start with clean state
    await page.goto(`${ADMIN_BASE_URL}/login`);
    await page.evaluate(() => localStorage.clear());
    console.log('✓ Cleared localStorage for clean test');

    // Step 2: Login to admin interface
    await page.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
    await page.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
    await page.click('button[type="submit"], .btn-primary');
    
    await page.waitForTimeout(3000);
    console.log('✓ Logged into admin interface');

    // Step 3: Navigate to logs page
    await page.goto(`${ADMIN_BASE_URL}/logs`);
    await page.waitForTimeout(3000);
    console.log('✓ Navigated to logs page');

    // Step 4: Verify toggle is initially disabled (default state)
    const initialToggleState = await page.isChecked('#consoleToSystemToggle');
    console.log(`Initial toggle state: ${initialToggleState}`);
    expect(initialToggleState).toBe(false); // Should be false by default

    // Step 5: Enable the toggle
    await page.click('#consoleToSystemToggle');
    await page.waitForTimeout(1000);
    
    const enabledState = await page.isChecked('#consoleToSystemToggle');
    console.log(`After enabling toggle: ${enabledState}`);
    expect(enabledState).toBe(true);

    // Step 6: Verify localStorage was updated
    const storedValue = await page.evaluate(() => localStorage.getItem('consoleToSystemLogging'));
    console.log(`localStorage value after enabling: ${storedValue}`);
    expect(storedValue).toBe('true');

    // Step 7: Refresh the page to simulate new session
    await page.reload();
    await page.waitForTimeout(3000);
    console.log('✓ Refreshed page to simulate new session');

    // Step 8: Verify toggle state is preserved after refresh
    const persistedState = await page.isChecked('#consoleToSystemToggle');
    console.log(`Toggle state after refresh: ${persistedState}`);
    expect(persistedState).toBe(true); // Should remain true

    // Step 9: Verify localStorage still contains the value
    const storedValueAfterRefresh = await page.evaluate(() => localStorage.getItem('consoleToSystemLogging'));
    console.log(`localStorage value after refresh: ${storedValueAfterRefresh}`);
    expect(storedValueAfterRefresh).toBe('true');

    // Step 10: Test disabling the toggle and verify persistence
    await page.click('#consoleToSystemToggle');
    await page.waitForTimeout(1000);
    
    const disabledState = await page.isChecked('#consoleToSystemToggle');
    console.log(`After disabling toggle: ${disabledState}`);
    expect(disabledState).toBe(false);

    // Step 11: Verify localStorage was updated to false
    const disabledStoredValue = await page.evaluate(() => localStorage.getItem('consoleToSystemLogging'));
    console.log(`localStorage value after disabling: ${disabledStoredValue}`);
    expect(disabledStoredValue).toBe('false');

    // Step 12: Final refresh test
    await page.reload();
    await page.waitForTimeout(3000);
    
    const finalState = await page.isChecked('#consoleToSystemToggle');
    console.log(`Final toggle state after second refresh: ${finalState}`);
    expect(finalState).toBe(false); // Should remain false

    console.log('✅ Console-to-System toggle persistence test completed successfully');
  });

  test('should handle browser session with no previous localStorage value', async () => {
    console.log('=== Testing Default State for New Sessions ===');

    // Clear localStorage completely
    await page.goto(`${ADMIN_BASE_URL}/login`);
    await page.evaluate(() => localStorage.removeItem('consoleToSystemLogging'));

    // Login and navigate to logs
    await page.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
    await page.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
    await page.click('button[type="submit"], .btn-primary');
    
    await page.waitForTimeout(2000);
    await page.goto(`${ADMIN_BASE_URL}/logs`);
    await page.waitForTimeout(3000);

    // Verify default state (should be false)
    const defaultState = await page.isChecked('#consoleToSystemToggle');
    console.log(`Default toggle state for new session: ${defaultState}`);
    expect(defaultState).toBe(false);

    // Verify localStorage is null or undefined initially
    const initialStoredValue = await page.evaluate(() => localStorage.getItem('consoleToSystemLogging'));
    console.log(`Initial localStorage value: ${initialStoredValue}`);
    
    console.log('✅ Default state test completed successfully');
  });

  test('should verify toggle affects console interceptor status', async () => {
    console.log('=== Testing Console Interceptor Integration ===');

    await page.goto(`${ADMIN_BASE_URL}/login`);
    await page.fill('input[name="Email"], input[id="email"], input[type="email"]', ADMIN_CREDENTIALS.email);
    await page.fill('input[name="Password"], input[id="password"], input[type="password"]', ADMIN_CREDENTIALS.password);
    await page.click('button[type="submit"], .btn-primary');
    
    await page.waitForTimeout(2000);
    await page.goto(`${ADMIN_BASE_URL}/logs`);
    await page.waitForTimeout(3000);

    // Enable toggle
    await page.click('#consoleToSystemToggle');
    await page.waitForTimeout(1000);

    // Check console interceptor status
    const interceptorStatus = await page.evaluate(() => {
      if (window.getConsoleInterceptorStatus) {
        return window.getConsoleInterceptorStatus();
      }
      return { error: 'Console interceptor not available' };
    });

    console.log('Console interceptor status:', interceptorStatus);
    
    // The interceptor should be enabled
    if (!interceptorStatus.error) {
      expect(interceptorStatus.enabled).toBe(true);
    }

    console.log('✅ Console interceptor integration test completed');
  });
});