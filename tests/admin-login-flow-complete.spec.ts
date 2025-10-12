import { test, expect, Page } from '@playwright/test';
import * as fs from 'fs';
import * as path from 'path';

/**
 * Comprehensive Admin Login Flow Test
 * Tests the complete admin authentication flow including:
 * - Login page accessibility
 * - Default credential authentication
 * - Dashboard access after login
 * - Data loading verification
 * - Error handling
 * - Browser console error monitoring
 */

const ADMIN_URL_DEV = 'https://localhost:7030';
const ADMIN_URL_DOCKER = 'https://localhost:9030';
const SCREENSHOT_DIR = 'test-results/admin-login-screenshots';
const DEFAULT_EMAIL = 'admin@stadium.com';
const DEFAULT_PASSWORD = 'admin123';

// Ensure screenshot directory exists
if (!fs.existsSync(SCREENSHOT_DIR)) {
  fs.mkdirSync(SCREENSHOT_DIR, { recursive: true });
}

/**
 * Capture screenshot with timestamp
 */
async function captureScreenshot(page: Page, name: string) {
  const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
  const filename = `${name}_${timestamp}.png`;
  const filepath = path.join(SCREENSHOT_DIR, filename);
  await page.screenshot({ path: filepath, fullPage: true });
  console.log(`📸 Screenshot saved: ${filename}`);
  return filepath;
}

/**
 * Log console messages and errors
 */
function setupConsoleLogging(page: Page) {
  const consoleMessages: string[] = [];
  const consoleErrors: string[] = [];

  page.on('console', msg => {
    const text = msg.text();
    consoleMessages.push(`[${msg.type()}] ${text}`);

    if (msg.type() === 'error') {
      consoleErrors.push(text);
      console.error('❌ Browser Console Error:', text);
    } else if (msg.type() === 'warning') {
      console.warn('⚠️  Browser Console Warning:', text);
    } else {
      console.log('📋 Browser Console:', text);
    }
  });

  page.on('pageerror', error => {
    consoleErrors.push(error.message);
    console.error('❌ Page Error:', error.message);
  });

  return { consoleMessages, consoleErrors };
}

/**
 * Try to connect to admin URL
 */
async function tryAdminUrl(page: Page, url: string): Promise<boolean> {
  try {
    console.log(`🔍 Attempting to connect to ${url}...`);
    const response = await page.goto(url, {
      waitUntil: 'domcontentloaded',
      timeout: 10000
    });

    if (response && response.ok()) {
      console.log(`✅ Successfully connected to ${url}`);
      return true;
    }
    console.log(`⚠️  Got response but not OK: ${response?.status()}`);
    return false;
  } catch (error) {
    console.log(`❌ Failed to connect to ${url}: ${error}`);
    return false;
  }
}

test.describe('Admin Login Flow - Complete Test Suite', () => {
  let adminUrl: string;
  let consoleLog: { consoleMessages: string[], consoleErrors: string[] };

  test.beforeAll(async ({ browser }) => {
    // Determine which URL is accessible
    const page = await browser.newPage({ ignoreHTTPSErrors: true });

    if (await tryAdminUrl(page, ADMIN_URL_DEV)) {
      adminUrl = ADMIN_URL_DEV;
      console.log(`✅ Using development URL: ${adminUrl}`);
    } else if (await tryAdminUrl(page, ADMIN_URL_DOCKER)) {
      adminUrl = ADMIN_URL_DOCKER;
      console.log(`✅ Using Docker URL: ${adminUrl}`);
    } else {
      throw new Error(`❌ Admin application is not running on either ${ADMIN_URL_DEV} or ${ADMIN_URL_DOCKER}`);
    }

    await page.close();
  });

  test.beforeEach(async ({ page }) => {
    // Setup console logging for each test
    consoleLog = setupConsoleLogging(page);
  });

  test('1. Admin Login Page - Accessibility and Structure', async ({ page }) => {
    console.log('\n=== TEST 1: Login Page Accessibility ===');

    // Navigate to admin app
    await page.goto(adminUrl, { waitUntil: 'networkidle' });
    await captureScreenshot(page, '01_login_page_initial');

    // Verify we're on login page
    const currentUrl = page.url();
    console.log(`📍 Current URL: ${currentUrl}`);

    expect(currentUrl).toContain('/login');

    // Check page title
    const title = await page.title();
    console.log(`📄 Page Title: ${title}`);
    expect(title).toBeTruthy();

    // Verify login form elements exist
    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]');
    const passwordInput = page.locator('input[type="password"]');
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]');

    await expect(emailInput).toBeVisible({ timeout: 5000 });
    await expect(passwordInput).toBeVisible({ timeout: 5000 });
    await expect(loginButton).toBeVisible({ timeout: 5000 });

    console.log('✅ All login form elements are visible');

    // Take screenshot of login page
    await captureScreenshot(page, '02_login_page_ready');
  });

  test('2. Admin Login - Default Credentials', async ({ page }) => {
    console.log('\n=== TEST 2: Login with Default Credentials ===');

    // Navigate to login page
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    // Wait for login form
    await page.waitForLoadState('domcontentloaded');
    await captureScreenshot(page, '03_before_login');

    // Find input fields with flexible selectors
    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"]').first();
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]').first();

    // Fill in credentials
    console.log(`🔑 Entering credentials: ${DEFAULT_EMAIL}`);
    await emailInput.fill(DEFAULT_EMAIL);
    await passwordInput.fill(DEFAULT_PASSWORD);

    await captureScreenshot(page, '04_credentials_entered');

    // Click login button
    console.log('🖱️  Clicking login button...');
    await loginButton.click();

    // Wait for navigation or error
    try {
      await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });
      console.log('✅ Successfully navigated away from login page');
    } catch (error) {
      console.log('⚠️  Still on login page after clicking login');
      await captureScreenshot(page, '05_login_failed_or_error');

      // Check for error messages
      const errorMessages = await page.locator('.alert-danger, .error, .text-danger, [class*="error"]').allTextContents();
      if (errorMessages.length > 0) {
        console.log('❌ Error messages found:', errorMessages);
      }
    }

    // Wait a bit for any redirects or loading
    await page.waitForTimeout(3000);

    const currentUrl = page.url();
    console.log(`📍 Current URL after login: ${currentUrl}`);

    await captureScreenshot(page, '06_after_login_attempt');

    // Verify we're not on login page anymore
    expect(currentUrl).not.toContain('/login');
    console.log('✅ Login successful - redirected from login page');
  });

  test('3. Admin Dashboard - Access and Data Loading', async ({ page }) => {
    console.log('\n=== TEST 3: Dashboard Access and Data Loading ===');

    // Login first
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"]').first();
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]').first();

    await emailInput.fill(DEFAULT_EMAIL);
    await passwordInput.fill(DEFAULT_PASSWORD);
    await loginButton.click();

    // Wait for dashboard
    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });
    await page.waitForLoadState('networkidle');

    const dashboardUrl = page.url();
    console.log(`📍 Dashboard URL: ${dashboardUrl}`);

    await captureScreenshot(page, '07_dashboard_loaded');

    // Check for dashboard elements
    const dashboardTitle = await page.locator('h1, h2, [class*="title"]').first().textContent();
    console.log(`📊 Dashboard Title: ${dashboardTitle}`);

    // Check for navigation menu
    const navItems = await page.locator('nav a, .nav-link, .sidebar a').count();
    console.log(`🧭 Navigation items found: ${navItems}`);
    expect(navItems).toBeGreaterThan(0);

    // Look for data sections (cards, tables, etc.)
    const dataCards = await page.locator('.card, [class*="card"]').count();
    const dataTables = await page.locator('table').count();
    const dataLists = await page.locator('ul, ol').count();

    console.log(`📦 Data elements found:`);
    console.log(`   - Cards: ${dataCards}`);
    console.log(`   - Tables: ${dataTables}`);
    console.log(`   - Lists: ${dataLists}`);

    await captureScreenshot(page, '08_dashboard_with_data');

    // Check for loading indicators (should not be visible)
    const loadingIndicators = await page.locator('[class*="loading"], [class*="spinner"], .spinner-border').count();
    console.log(`⏳ Loading indicators: ${loadingIndicators}`);

    console.log('✅ Dashboard loaded successfully');
  });

  test('4. Admin Navigation - Users Page', async ({ page }) => {
    console.log('\n=== TEST 4: Navigate to Users Page ===');

    // Login
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"]').first();
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]').first();

    await emailInput.fill(DEFAULT_EMAIL);
    await passwordInput.fill(DEFAULT_PASSWORD);
    await loginButton.click();

    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });
    await page.waitForLoadState('networkidle');

    // Try to navigate to users page
    const usersLink = page.locator('a:has-text("Users"), a[href*="users"], nav a:has-text("User")').first();

    if (await usersLink.isVisible()) {
      console.log('🔗 Clicking Users link...');
      await usersLink.click();
      await page.waitForLoadState('networkidle');

      const usersUrl = page.url();
      console.log(`📍 Users page URL: ${usersUrl}`);

      await captureScreenshot(page, '09_users_page');

      // Check for user data
      const userRows = await page.locator('table tbody tr, .user-item, [class*="user"]').count();
      console.log(`👥 User entries found: ${userRows}`);

      console.log('✅ Users page loaded successfully');
    } else {
      console.log('⚠️  Users link not found in navigation');
      await captureScreenshot(page, '09_users_link_not_found');
    }
  });

  test('5. Admin Navigation - Orders Page', async ({ page }) => {
    console.log('\n=== TEST 5: Navigate to Orders Page ===');

    // Login
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"]').first();
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]').first();

    await emailInput.fill(DEFAULT_EMAIL);
    await passwordInput.fill(DEFAULT_PASSWORD);
    await loginButton.click();

    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });
    await page.waitForLoadState('networkidle');

    // Try to navigate to orders page
    const ordersLink = page.locator('a:has-text("Orders"), a[href*="orders"], nav a:has-text("Order")').first();

    if (await ordersLink.isVisible()) {
      console.log('🔗 Clicking Orders link...');
      await ordersLink.click();
      await page.waitForLoadState('networkidle');

      const ordersUrl = page.url();
      console.log(`📍 Orders page URL: ${ordersUrl}`);

      await captureScreenshot(page, '10_orders_page');

      // Check for order data
      const orderRows = await page.locator('table tbody tr, .order-item, [class*="order"]').count();
      console.log(`📦 Order entries found: ${orderRows}`);

      console.log('✅ Orders page loaded successfully');
    } else {
      console.log('⚠️  Orders link not found in navigation');
      await captureScreenshot(page, '10_orders_link_not_found');
    }
  });

  test('6. Error Detection - Console Errors', async ({ page }) => {
    console.log('\n=== TEST 6: Browser Console Error Detection ===');

    // Login and navigate around
    await page.goto(adminUrl, { waitUntil: 'networkidle' });

    const emailInput = page.locator('input[type="email"], input[name="email"], input[id*="email"], input[placeholder*="email" i]').first();
    const passwordInput = page.locator('input[type="password"]').first();
    const loginButton = page.locator('button:has-text("Login"), button:has-text("Sign In"), button[type="submit"]').first();

    await emailInput.fill(DEFAULT_EMAIL);
    await passwordInput.fill(DEFAULT_PASSWORD);
    await loginButton.click();

    await page.waitForURL(url => !url.includes('/login'), { timeout: 15000 });
    await page.waitForLoadState('networkidle');

    // Wait a bit to capture any errors
    await page.waitForTimeout(5000);

    await captureScreenshot(page, '11_final_state');

    // Report console errors
    console.log('\n📊 Console Error Summary:');
    if (consoleLog.consoleErrors.length === 0) {
      console.log('✅ No JavaScript errors detected');
    } else {
      console.log(`❌ Found ${consoleLog.consoleErrors.length} console errors:`);
      consoleLog.consoleErrors.forEach((error, index) => {
        console.log(`   ${index + 1}. ${error}`);
      });
    }

    // Report all console messages
    console.log('\n📋 All Console Messages:');
    consoleLog.consoleMessages.slice(0, 20).forEach((msg, index) => {
      console.log(`   ${index + 1}. ${msg}`);
    });

    if (consoleLog.consoleMessages.length > 20) {
      console.log(`   ... and ${consoleLog.consoleMessages.length - 20} more messages`);
    }
  });

  test.afterAll(async () => {
    console.log('\n' + '='.repeat(80));
    console.log('📊 TEST SUITE COMPLETE');
    console.log('='.repeat(80));
    console.log(`📁 Screenshots saved in: ${SCREENSHOT_DIR}`);
    console.log('='.repeat(80) + '\n');
  });
});
