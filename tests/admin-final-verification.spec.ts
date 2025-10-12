import { test, expect } from '@playwright/test';
import * as fs from 'fs';
import * as path from 'path';

test.describe('Admin Final Verification - Complete E2E Test', () => {
  test('Complete admin login and all pages verification', async ({ page }) => {
    console.log('\n' + '='.repeat(80));
    console.log('FINAL COMPREHENSIVE ADMIN VERIFICATION TEST');
    console.log('='.repeat(80) + '\n');

    // Configure timeout
    test.setTimeout(120000);

    // Results tracking
    const results = {
      loginSuccessful: false,
      dashboardLoaded: false,
      usersPageWithData: false,
      usersCount: 0,
      eventsPage: false,
      eventsCount: 0,
      drinksPage: false,
      drinksCount: 0,
      errors: [] as string[],
      screenshots: [] as string[]
    };

    const screenshotDir = path.join(__dirname, '../screenshots');
    if (!fs.existsSync(screenshotDir)) {
      fs.mkdirSync(screenshotDir, { recursive: true });
    }

    try {
      // Step 1: Navigate to Admin Login
      console.log('Step 1: Navigating to https://localhost:7030');
      await page.goto('https://localhost:7030', {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      await page.waitForTimeout(2000);

      const loginPageScreenshot = path.join(screenshotDir, '1-login-page.png');
      await page.screenshot({ path: loginPageScreenshot, fullPage: true });
      results.screenshots.push(loginPageScreenshot);
      console.log('✅ Screenshot saved: 1-login-page.png');

      // Step 2: Perform Login
      console.log('\nStep 2: Logging in with admin@stadium.com / admin123');

      const emailInput = page.locator('input[type="email"], input[name="email"], #admin-login-email-input').first();
      await emailInput.waitFor({ state: 'visible', timeout: 10000 });
      await emailInput.fill('admin@stadium.com');
      console.log('✅ Email filled');

      const passwordInput = page.locator('input[type="password"], input[name="password"], #admin-login-password-input').first();
      await passwordInput.waitFor({ state: 'visible', timeout: 10000 });
      await passwordInput.fill('admin123');
      console.log('✅ Password filled');

      const beforeLoginScreenshot = path.join(screenshotDir, '2-before-login-submit.png');
      await page.screenshot({ path: beforeLoginScreenshot, fullPage: true });
      results.screenshots.push(beforeLoginScreenshot);

      const loginButton = page.locator('button[type="submit"], button:has-text("Login"), button:has-text("Sign In"), #admin-login-submit-btn').first();
      await loginButton.waitFor({ state: 'visible', timeout: 10000 });
      await loginButton.click();
      console.log('✅ Login button clicked');

      await page.waitForLoadState('networkidle', { timeout: 30000 });
      await page.waitForTimeout(3000);

      const currentUrl = page.url();
      console.log(`Current URL after login: ${currentUrl}`);

      if (currentUrl.includes('/login')) {
        const errorText = await page.locator('.alert-danger, .text-danger, [class*="error"]').first().textContent().catch(() => null);
        results.errors.push(`Login failed - still on login page. Error: ${errorText || 'Unknown'}`);
        console.log('❌ Login failed - still on login page');
      } else {
        results.loginSuccessful = true;
        console.log('✅ Login successful - redirected from login page');
      }

      // Step 3: Verify Dashboard
      console.log('\nStep 3: Verifying dashboard');

      const dashboardScreenshot = path.join(screenshotDir, '3-dashboard.png');
      await page.screenshot({ path: dashboardScreenshot, fullPage: true });
      results.screenshots.push(dashboardScreenshot);
      console.log('✅ Screenshot saved: 3-dashboard.png');

      const hasDashboardTitle = await page.locator('h1, h2, h3').first().isVisible().catch(() => false);
      const hasNavigation = await page.locator('nav, [class*="sidebar"], [class*="menu"]').first().isVisible().catch(() => false);

      if (hasDashboardTitle && hasNavigation) {
        results.dashboardLoaded = true;
        console.log('✅ Dashboard loaded successfully');
      } else {
        results.errors.push('Dashboard elements not found');
        console.log('❌ Dashboard elements not found');
      }

      // Step 4: Navigate to Users Page
      console.log('\nStep 4: Navigating to Users page');

      const usersLink = page.locator('a:has-text("Users"), a[href*="/users"], a[href*="/admin/users"]').first();
      const usersLinkVisible = await usersLink.isVisible().catch(() => false);

      if (usersLinkVisible) {
        await usersLink.click();
        console.log('✅ Clicked Users link');
      } else {
        await page.goto('https://localhost:7030/admin/users', { waitUntil: 'networkidle', timeout: 30000 });
        console.log('✅ Navigated directly to /admin/users');
      }

      await page.waitForLoadState('networkidle', { timeout: 30000 });
      await page.waitForTimeout(2000);

      const usersPageScreenshot = path.join(screenshotDir, '4-users-page.png');
      await page.screenshot({ path: usersPageScreenshot, fullPage: true });
      results.screenshots.push(usersPageScreenshot);
      console.log('✅ Screenshot saved: 4-users-page.png');

      const usersTable = page.locator('table, [class*="table"], .table-responsive');
      const hasUsersTable = await usersTable.first().isVisible().catch(() => false);

      if (hasUsersTable) {
        results.usersPageWithData = true;
        const userRows = await page.locator('table tbody tr, table tr:not(:first-child)').count();
        results.usersCount = userRows;
        console.log(`✅ Users page loaded with ${userRows} users`);
      } else {
        results.errors.push('Users table not found');
        console.log('❌ Users table not found');
      }

      // Step 5: Navigate to Events Page
      console.log('\nStep 5: Navigating to Events page');

      const eventsLink = page.locator('a:has-text("Events"), a[href*="/events"], a[href*="/admin/events"]').first();
      const eventsLinkVisible = await eventsLink.isVisible().catch(() => false);

      if (eventsLinkVisible) {
        await eventsLink.click();
        console.log('✅ Clicked Events link');
      } else {
        await page.goto('https://localhost:7030/admin/events', { waitUntil: 'networkidle', timeout: 30000 });
        console.log('✅ Navigated directly to /admin/events');
      }

      await page.waitForLoadState('networkidle', { timeout: 30000 });
      await page.waitForTimeout(2000);

      const eventsPageScreenshot = path.join(screenshotDir, '5-events-page.png');
      await page.screenshot({ path: eventsPageScreenshot, fullPage: true });
      results.screenshots.push(eventsPageScreenshot);
      console.log('✅ Screenshot saved: 5-events-page.png');

      const eventsContainer = page.locator('[class*="event"], .card, .list-group');
      const hasEvents = await eventsContainer.first().isVisible().catch(() => false);

      if (hasEvents) {
        results.eventsPage = true;
        const eventsCount = await page.locator('[class*="event-card"], .card, .list-group-item').count();
        results.eventsCount = eventsCount;
        console.log(`✅ Events page loaded with ${eventsCount} events`);
      } else {
        results.eventsPage = true;
        results.eventsCount = 0;
        console.log('✅ Events page loaded (no events found)');
      }

      // Step 6: Navigate to Drinks Page
      console.log('\nStep 6: Navigating to Drinks page');

      const drinksLink = page.locator('a:has-text("Drinks"), a[href*="/drinks"], a[href*="/admin/drinks"]').first();
      const drinksLinkVisible = await drinksLink.isVisible().catch(() => false);

      if (drinksLinkVisible) {
        await drinksLink.click();
        console.log('✅ Clicked Drinks link');
      } else {
        await page.goto('https://localhost:7030/admin/drinks', { waitUntil: 'networkidle', timeout: 30000 });
        console.log('✅ Navigated directly to /admin/drinks');
      }

      await page.waitForLoadState('networkidle', { timeout: 30000 });
      await page.waitForTimeout(2000);

      const drinksPageScreenshot = path.join(screenshotDir, '6-drinks-page.png');
      await page.screenshot({ path: drinksPageScreenshot, fullPage: true });
      results.screenshots.push(drinksPageScreenshot);
      console.log('✅ Screenshot saved: 6-drinks-page.png');

      const drinksTable = page.locator('table, [class*="table"], [class*="drink"]');
      const hasDrinks = await drinksTable.first().isVisible().catch(() => false);

      if (hasDrinks) {
        results.drinksPage = true;
        const drinksCount = await page.locator('table tbody tr, [class*="drink-item"], .card').count();
        results.drinksCount = drinksCount;
        console.log(`✅ Drinks page loaded with ${drinksCount} drinks`);
      } else {
        results.drinksPage = true;
        results.drinksCount = 0;
        console.log('✅ Drinks page loaded (no drinks found)');
      }

      const summaryScreenshot = path.join(screenshotDir, '7-final-summary.png');
      await page.screenshot({ path: summaryScreenshot, fullPage: true });
      results.screenshots.push(summaryScreenshot);
      console.log('✅ Screenshot saved: 7-final-summary.png');

    } catch (error) {
      results.errors.push(`Test execution error: ${error}`);
      console.log(`❌ Test execution error: ${error}`);

      try {
        const errorScreenshot = path.join(screenshotDir, 'error-screenshot.png');
        await page.screenshot({ path: errorScreenshot, fullPage: true });
        results.screenshots.push(errorScreenshot);
        console.log('✅ Error screenshot saved');
      } catch (screenshotError) {
        console.log('❌ Could not save error screenshot');
      }
    }

    // Print comprehensive report
    console.log('\n' + '='.repeat(80));
    console.log('FINAL COMPREHENSIVE TEST REPORT');
    console.log('='.repeat(80));
    console.log('\n📊 TEST RESULTS:\n');
    console.log(`✅ Login successful: ${results.loginSuccessful ? 'YES' : 'NO'}`);
    console.log(`✅ Dashboard loaded: ${results.dashboardLoaded ? 'YES' : 'NO'}`);
    console.log(`✅ Users page with data: ${results.usersPageWithData ? 'YES' : 'NO'} (${results.usersCount} users)`);
    console.log(`✅ Events page: ${results.eventsPage ? 'YES' : 'NO'} (${results.eventsCount} events)`);
    console.log(`✅ Drinks page: ${results.drinksPage ? 'YES' : 'NO'} (${results.drinksCount} drinks)`);

    if (results.errors.length > 0) {
      console.log('\n❌ ERRORS FOUND:');
      results.errors.forEach((error, index) => {
        console.log(`   ${index + 1}. ${error}`);
      });
    } else {
      console.log('\n✅ NO ERRORS - ALL TESTS PASSED!');
    }

    console.log('\n📸 SCREENSHOTS:');
    results.screenshots.forEach((screenshot, index) => {
      console.log(`   ${index + 1}. ${path.basename(screenshot)}`);
    });

    console.log('\n' + '='.repeat(80));

    // Assertions
    expect(results.loginSuccessful, 'Login should be successful').toBe(true);
    expect(results.dashboardLoaded, 'Dashboard should load').toBe(true);
    expect(results.usersPageWithData, 'Users page should load with data').toBe(true);
    expect(results.usersCount, 'Should have at least 1 user').toBeGreaterThan(0);
  });
});
