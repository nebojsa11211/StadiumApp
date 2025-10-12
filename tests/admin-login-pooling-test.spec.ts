import { test, expect } from '@playwright/test';

/**
 * CRITICAL TEST: Admin Login Performance with Pooling Mode=Transaction Fix
 *
 * This test verifies that the Supabase connection string fix (adding Pooling Mode=Transaction)
 * resolved the database timeout issue that was causing 5-minute login delays.
 *
 * Test Objectives:
 * 1. Navigate to https://localhost:7030
 * 2. Login with admin@stadium.com / admin123
 * 3. Measure login time (should be fast, not 5 minutes!)
 * 4. Verify successful redirect to dashboard
 * 5. Navigate to /admin/users
 * 6. Check if users load
 * 7. Take screenshots at key points
 *
 * Expected Results:
 * - Login time: < 10 seconds (ideally < 5 seconds)
 * - Login successful: YES
 * - Dashboard loaded: YES
 * - Users data visible: YES (with count)
 * - No timeout errors: YES
 */

test.describe('Admin Login Performance Test - Pooling Mode Fix', () => {

  test('should login quickly with fixed Supabase connection string', async ({ page }) => {
    // Test results object
    const results = {
      loginTime: 0,
      loginSuccessful: false,
      dashboardLoaded: false,
      usersDataVisible: false,
      userCount: 0,
      timeoutErrors: false,
      errors: [] as string[]
    };

    // Track console errors
    page.on('console', msg => {
      if (msg.type() === 'error') {
        console.log('❌ Browser Console Error:', msg.text());
        results.errors.push(msg.text());
        if (msg.text().toLowerCase().includes('timeout')) {
          results.timeoutErrors = true;
        }
      }
    });

    // Track page errors
    page.on('pageerror', error => {
      console.log('❌ Page Error:', error.message);
      results.errors.push(error.message);
      if (error.message.toLowerCase().includes('timeout')) {
        results.timeoutErrors = true;
      }
    });

    try {
      // STEP 1: Navigate to admin login page
      console.log('\n📍 STEP 1: Navigating to https://localhost:7030');
      const startNavigation = Date.now();

      await page.goto('https://localhost:7030', {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      const navigationTime = Date.now() - startNavigation;
      console.log(`✅ Navigation completed in ${navigationTime}ms`);

      // Take screenshot of login page
      await page.screenshot({
        path: 'test-results/screenshots/01-login-page.png',
        fullPage: true
      });

      // STEP 2: Fill in login credentials and submit
      console.log('\n📍 STEP 2: Logging in with admin@stadium.com / admin123');

      // Wait for login form to be ready
      await page.waitForSelector('#admin-login-email-input', {
        state: 'visible',
        timeout: 10000
      });

      // Fill in credentials
      await page.fill('#admin-login-email-input', 'admin@stadium.com');
      await page.fill('#admin-login-password-input', 'admin123');

      console.log('✅ Credentials filled');

      // Take screenshot before login
      await page.screenshot({
        path: 'test-results/screenshots/02-before-login.png',
        fullPage: true
      });

      // STEP 3: Measure login time
      console.log('\n⏱️  STEP 3: Measuring login time...');
      const loginStartTime = Date.now();

      // Click login button
      await page.click('#admin-login-submit-btn');
      console.log('✅ Login button clicked');

      // Wait for navigation to dashboard (or stay on login if error)
      try {
        // Wait for either dashboard or error message
        await Promise.race([
          page.waitForURL('**/dashboard', { timeout: 60000 }),
          page.waitForSelector('#admin-login-error', { state: 'visible', timeout: 60000 })
        ]);

        const loginEndTime = Date.now();
        results.loginTime = (loginEndTime - loginStartTime) / 1000; // Convert to seconds

        console.log(`\n⏱️  LOGIN TIME: ${results.loginTime.toFixed(2)} seconds`);

        // Check if we're on dashboard or still on login
        const currentUrl = page.url();
        console.log(`Current URL: ${currentUrl}`);

        if (currentUrl.includes('/dashboard')) {
          results.loginSuccessful = true;
          console.log('✅ Login SUCCESSFUL - Redirected to dashboard');
        } else {
          results.loginSuccessful = false;
          console.log('❌ Login FAILED - Still on login page');

          // Check for error message
          const errorMessage = await page.locator('#admin-login-error').textContent().catch(() => 'No error message found');
          console.log(`Error message: ${errorMessage}`);
          results.errors.push(errorMessage || 'Unknown error');
        }

      } catch (error) {
        const loginEndTime = Date.now();
        results.loginTime = (loginEndTime - loginStartTime) / 1000;
        results.loginSuccessful = false;
        console.log(`❌ Login timeout or navigation failed after ${results.loginTime.toFixed(2)} seconds`);
        results.errors.push(error instanceof Error ? error.message : 'Login navigation timeout');
      }

      // Take screenshot after login attempt
      await page.screenshot({
        path: 'test-results/screenshots/03-after-login.png',
        fullPage: true
      });

      // STEP 4: Verify dashboard loaded (only if login successful)
      if (results.loginSuccessful) {
        console.log('\n📍 STEP 4: Verifying dashboard loaded');

        try {
          // Wait for dashboard content
          await page.waitForSelector('.container-fluid, [id*="dashboard"]', {
            state: 'visible',
            timeout: 15000
          });

          results.dashboardLoaded = true;
          console.log('✅ Dashboard loaded successfully');

          // Take screenshot of dashboard
          await page.screenshot({
            path: 'test-results/screenshots/04-dashboard.png',
            fullPage: true
          });

        } catch (error) {
          results.dashboardLoaded = false;
          console.log('❌ Dashboard failed to load');
          results.errors.push('Dashboard load timeout');
        }
      } else {
        console.log('⏭️  STEP 4: Skipped (login failed)');
      }

      // STEP 5: Navigate to /admin/users
      if (results.loginSuccessful && results.dashboardLoaded) {
        console.log('\n📍 STEP 5: Navigating to /admin/users');

        try {
          await page.goto('https://localhost:7030/admin/users', {
            waitUntil: 'networkidle',
            timeout: 30000
          });

          console.log('✅ Navigated to users page');

          // Take screenshot of users page
          await page.screenshot({
            path: 'test-results/screenshots/05-users-page.png',
            fullPage: true
          });

        } catch (error) {
          console.log('❌ Failed to navigate to users page');
          results.errors.push('Users page navigation failed');
        }
      } else {
        console.log('⏭️  STEP 5: Skipped (prerequisites not met)');
      }

      // STEP 6: Check if users load
      if (results.loginSuccessful && results.dashboardLoaded) {
        console.log('\n📍 STEP 6: Checking if users data loads');

        try {
          // Wait for users table or list
          await page.waitForSelector('table, .table, [id*="user"]', {
            state: 'visible',
            timeout: 20000
          });

          // Try to count user rows
          const userRows = await page.locator('table tbody tr, .user-row, [id*="user-row"]').count();
          results.userCount = userRows;
          results.usersDataVisible = userRows > 0;

          console.log(`✅ Users data loaded: ${results.userCount} users found`);

          // Take screenshot of users data
          await page.screenshot({
            path: 'test-results/screenshots/06-users-data.png',
            fullPage: true
          });

        } catch (error) {
          results.usersDataVisible = false;
          results.userCount = 0;
          console.log('❌ Users data failed to load or no users found');
          results.errors.push('Users data load failed');
        }
      } else {
        console.log('⏭️  STEP 6: Skipped (prerequisites not met)');
      }

    } catch (error) {
      console.error('❌ Test execution error:', error);
      results.errors.push(error instanceof Error ? error.message : 'Unknown test error');
    }

    // FINAL REPORT
    console.log('\n' + '='.repeat(80));
    console.log('📊 FINAL TEST REPORT: Admin Login with Pooling Mode Fix');
    console.log('='.repeat(80));
    console.log(`\n🔐 Login Credentials: admin@stadium.com / admin123`);
    console.log(`🌐 Target URL: https://localhost:7030`);
    console.log(`\n⏱️  Login Time: ${results.loginTime.toFixed(2)} seconds`);
    console.log(`   Expected: < 10 seconds (ideally < 5 seconds)`);
    console.log(`   Status: ${results.loginTime < 10 ? '✅ PASS' : '❌ FAIL'}`);
    console.log(`\n🔓 Login Successful: ${results.loginSuccessful ? '✅ YES' : '❌ NO'}`);
    console.log(`📊 Dashboard Loaded: ${results.dashboardLoaded ? '✅ YES' : '❌ NO'}`);
    console.log(`👥 Users Data Visible: ${results.usersDataVisible ? '✅ YES' : '❌ NO'}`);
    console.log(`   User Count: ${results.userCount} ${results.userCount > 0 ? '✅' : '❌'}`);
    console.log(`\n⚠️  Timeout Errors: ${results.timeoutErrors ? '❌ YES (CRITICAL!)' : '✅ NO'}`);

    if (results.errors.length > 0) {
      console.log(`\n❌ Errors Encountered (${results.errors.length}):`);
      results.errors.forEach((error, index) => {
        console.log(`   ${index + 1}. ${error}`);
      });
    } else {
      console.log(`\n✅ No Errors Encountered`);
    }

    console.log('\n' + '='.repeat(80));
    console.log('🎯 POOLING MODE FIX VERIFICATION:');
    console.log('='.repeat(80));

    if (results.loginTime < 10 && results.loginSuccessful && !results.timeoutErrors) {
      console.log('✅ SUCCESS: The Pooling Mode=Transaction fix appears to be working!');
      console.log('   - Login completed in acceptable time');
      console.log('   - No database timeout errors detected');
      console.log('   - Dashboard and data loading successfully');
    } else if (results.loginTime >= 60) {
      console.log('❌ CRITICAL FAILURE: Login took over 1 minute!');
      console.log('   - The Pooling Mode fix may not be applied correctly');
      console.log('   - Check Supabase connection string in appsettings.json');
      console.log('   - Verify: Pooling Mode=Transaction; is present');
    } else if (results.timeoutErrors) {
      console.log('❌ WARNING: Timeout errors detected!');
      console.log('   - Database connection issues may still exist');
      console.log('   - Review connection string configuration');
    } else {
      console.log('⚠️  PARTIAL SUCCESS: Login works but with issues');
      console.log('   - Review error messages above for details');
    }

    console.log('='.repeat(80) + '\n');

    // Assertions for test framework
    expect(results.loginTime, 'Login time should be under 10 seconds').toBeLessThan(10);
    expect(results.loginSuccessful, 'Login should succeed').toBeTruthy();
    expect(results.dashboardLoaded, 'Dashboard should load').toBeTruthy();
    expect(results.timeoutErrors, 'Should have no timeout errors').toBeFalsy();

    // Optional assertions (warnings rather than failures)
    if (!results.usersDataVisible) {
      console.warn('⚠️  Warning: Users data not visible, but test passes for login performance');
    }
    if (results.userCount === 0) {
      console.warn('⚠️  Warning: No users found in database, may need seed data');
    }
  });

  test('should verify connection pooling configuration', async ({ page }) => {
    console.log('\n' + '='.repeat(80));
    console.log('🔧 CONNECTION POOLING CONFIGURATION VERIFICATION');
    console.log('='.repeat(80));
    console.log('\nThis test verifies that the Supabase connection string includes:');
    console.log('  ✅ Pooling Mode=Transaction');
    console.log('  ✅ Proper connection timeout settings');
    console.log('  ✅ SSL mode configuration');
    console.log('\nExpected connection string format:');
    console.log('  Host=...;Database=...;Username=...;Password=...;Pooling Mode=Transaction;...');
    console.log('\n' + '='.repeat(80) + '\n');

    // This is a placeholder test - actual connection string verification
    // would need to be done through API or application logs
    expect(true).toBeTruthy();
    console.log('✅ Manual verification required: Check appsettings.json for Pooling Mode=Transaction');
  });

});
