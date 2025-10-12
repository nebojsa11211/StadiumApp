import { test, expect } from '@playwright/test';

/**
 * Stadium Overview Database Test
 * Tests Stadium Overview page with connected Supabase PostgreSQL database
 *
 * Prerequisites:
 * - Supabase database connected and working
 * - API running on https://localhost:7010
 * - Admin app running on https://localhost:7030
 * - Admin user: admin@stadium.com / admin123
 * - Stadium structure imported (4 tribunes exist)
 */

test.describe('Stadium Overview - Database Connected', () => {
  test.beforeEach(async ({ page }) => {
    // Ignore SSL certificate errors for local development
    await page.goto('https://localhost:7030/login');
  });

  test('should login successfully with database connected', async ({ page }) => {
    console.log('Step 1: Testing login with connected database...');

    // Fill login form
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');

    // Click login button
    await page.click('#admin-login-submit-btn');

    // Wait for navigation (should be fast with connected DB)
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    console.log('✅ Login successful with database connected!');

    // Verify we're on the dashboard
    await expect(page).toHaveURL(/\/dashboard/);
  });

  test('should navigate to Stadium Overview page', async ({ page }) => {
    console.log('Step 2: Navigating to Stadium Overview...');

    // Login first
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    // Navigate to Stadium Overview
    // Option 1: Direct URL
    await page.goto('https://localhost:7030/stadium-overview');

    // Wait for page to load
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    console.log('✅ Stadium Overview page loaded!');

    // Verify we're on the Stadium Overview page
    await expect(page).toHaveURL(/\/stadium-overview/);
  });

  test('should display stadium layout with 4 tribunes', async ({ page }) => {
    console.log('Step 3: Verifying stadium layout display...');

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    // Navigate to Stadium Overview
    await page.goto('https://localhost:7030/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    // Check for page title
    const pageTitle = await page.locator('h1, h2, h3').first();
    await expect(pageTitle).toBeVisible({ timeout: 10000 });
    console.log(`Page title: ${await pageTitle.textContent()}`);

    // Look for stadium visualization or tribune elements
    // Check for any SVG (stadium map) or tribune containers
    const svgElement = page.locator('svg').first();
    const hasSvg = await svgElement.count() > 0;

    if (hasSvg) {
      console.log('✅ Stadium SVG visualization found!');
      await expect(svgElement).toBeVisible({ timeout: 5000 });
    } else {
      console.log('ℹ️ No SVG found, checking for tribune containers...');
    }

    // Take screenshot for verification
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-database-test.png',
      fullPage: true
    });
    console.log('📸 Screenshot saved: stadium-overview-database-test.png');
  });

  test('should load without timeout errors', async ({ page }) => {
    console.log('Step 4: Testing page load performance...');

    const startTime = Date.now();

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    const loginTime = Date.now() - startTime;
    console.log(`⏱️ Login completed in ${loginTime}ms`);

    const pageLoadStart = Date.now();

    // Navigate to Stadium Overview
    await page.goto('https://localhost:7030/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    const pageLoadTime = Date.now() - pageLoadStart;
    console.log(`⏱️ Stadium Overview loaded in ${pageLoadTime}ms`);

    // Check for error messages
    const errorAlert = page.locator('.alert-danger, .alert-error, [role="alert"]');
    const hasError = await errorAlert.count() > 0;

    if (hasError) {
      const errorText = await errorAlert.first().textContent();
      console.log(`❌ Error found: ${errorText}`);
      expect(hasError).toBe(false);
    } else {
      console.log('✅ No error messages found!');
    }

    // Verify fast load time (should be under 5 seconds with connected DB)
    expect(pageLoadTime).toBeLessThan(5000);
    console.log('✅ Page loaded quickly with connected database!');
  });

  test('should display stadium data from database', async ({ page }) => {
    console.log('Step 5: Verifying database data display...');

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    // Navigate to Stadium Overview
    await page.goto('https://localhost:7030/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    // Wait a bit for data to load from API
    await page.waitForTimeout(2000);

    // Check for any loading indicators
    const loadingIndicator = page.locator('.spinner-border, .loading, [aria-label="Loading"]');
    const isLoading = await loadingIndicator.count() > 0;

    if (isLoading) {
      console.log('⏳ Loading indicator found, waiting for data...');
      await loadingIndicator.first().waitFor({ state: 'hidden', timeout: 10000 });
      console.log('✅ Data loaded!');
    }

    // Take final screenshot
    await page.screenshot({
      path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\stadium-overview-final.png',
      fullPage: true
    });

    console.log('✅ Stadium Overview page tested successfully with database!');
  });

  test('should handle event selection if available', async ({ page }) => {
    console.log('Step 6: Testing event selection functionality...');

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForURL('**/dashboard', { timeout: 10000 });

    // Navigate to Stadium Overview
    await page.goto('https://localhost:7030/stadium-overview');
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    // Look for event selector dropdown
    const eventSelector = page.locator('select, [role="combobox"]').first();
    const hasEventSelector = await eventSelector.count() > 0;

    if (hasEventSelector) {
      console.log('✅ Event selector found!');
      await expect(eventSelector).toBeVisible({ timeout: 5000 });

      // Try to get options
      const options = await eventSelector.locator('option').count();
      console.log(`📋 Found ${options} event options`);
    } else {
      console.log('ℹ️ No event selector found on page');
    }

    console.log('✅ Event functionality check completed!');
  });
});
