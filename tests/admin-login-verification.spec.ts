import { test, expect } from '@playwright/test';

/**
 * Admin Login Verification Test - Post Auth Fix
 *
 * This test verifies the complete admin authentication flow after the recent fix:
 * 1. Navigate to admin login page
 * 2. Login with admin credentials
 * 3. Verify successful login and redirect to dashboard
 * 4. Check dashboard page loads correctly
 * 5. Navigate to /admin/users page
 * 6. Verify users data loads and displays
 * 7. Take screenshots for visual verification
 *
 * Expected Results:
 * - Login successful with redirect to dashboard
 * - Dashboard loads without errors
 * - Users page accessible and displays data
 * - No authentication errors
 */

test.describe('Admin Login Verification - Post Auth Fix', () => {
  const adminUrl = 'https://localhost:7030';
  const credentials = {
    email: 'admin@stadium.com',
    password: 'admin123'
  };

  test('Complete admin authentication flow verification', async ({ page }) => {
    let loginSuccess = false;
    let dashboardLoaded = false;
    let usersDataVisible = false;
    const errors: string[] = [];

    console.log('🚀 Starting Admin Login Verification Test');
    console.log('='.repeat(60));

    try {
      // Step 1: Navigate to admin login page
      console.log('\n📍 Step 1: Navigating to admin login page...');
      await page.goto(`${adminUrl}/login`, {
        waitUntil: 'networkidle',
        timeout: 30000
      });

      // Take screenshot of login page
      await page.screenshot({
        path: 'test-results/admin-login-page.png',
        fullPage: true
      });
      console.log('✅ Login page loaded successfully');

      // Step 2: Fill in login credentials
      console.log('\n🔐 Step 2: Entering admin credentials...');

      // Wait for form elements to be visible
      await page.waitForSelector('#admin-login-email-input', {
        state: 'visible',
        timeout: 10000
      });

      await page.fill('#admin-login-email-input', credentials.email);
      await page.fill('#admin-login-password-input', credentials.password);
      console.log('✅ Credentials entered');

      // Take screenshot before login
      await page.screenshot({
        path: 'test-results/admin-before-login.png',
        fullPage: true
      });

      // Step 3: Submit login form
      console.log('\n🔑 Step 3: Submitting login form...');
      await page.click('#admin-login-submit-btn');

      // Wait for navigation and check if redirected
      try {
        await page.waitForURL(/dashboard|\/$/i, {
          timeout: 15000,
          waitUntil: 'networkidle'
        });
        loginSuccess = true;
        console.log('✅ Login successful - Redirected to dashboard');
      } catch (navError) {
        errors.push(`Login navigation failed: ${navError}`);
        console.error('❌ Login failed - No redirect detected');

        // Check for error messages on login page
        const errorElement = await page.$('.alert-danger, .error-message');
        if (errorElement) {
          const errorText = await errorElement.textContent();
          errors.push(`Login error message: ${errorText}`);
          console.error(`❌ Error message: ${errorText}`);
        }
      }

      // Step 4: Verify dashboard page
      if (loginSuccess) {
        console.log('\n📊 Step 4: Verifying dashboard page...');

        // Wait for dashboard content to load
        await page.waitForLoadState('networkidle', { timeout: 10000 });

        // Take screenshot of dashboard
        await page.screenshot({
          path: 'test-results/admin-dashboard-success.png',
          fullPage: true
        });

        // Check for dashboard elements
        const dashboardTitle = await page.textContent('h1, h2, .page-title', { timeout: 5000 }).catch(() => null);
        if (dashboardTitle) {
          console.log(`✅ Dashboard loaded - Title: ${dashboardTitle.trim()}`);
          dashboardLoaded = true;
        } else {
          errors.push('Dashboard title not found');
          console.error('❌ Dashboard title not found');
        }

        // Check for any error messages
        const hasErrors = await page.$('.alert-danger, .error-message');
        if (hasErrors) {
          const errorText = await hasErrors.textContent();
          errors.push(`Dashboard error: ${errorText}`);
          console.error(`❌ Dashboard error detected: ${errorText}`);
          dashboardLoaded = false;
        }
      }

      // Step 5: Navigate to users page
      if (loginSuccess && dashboardLoaded) {
        console.log('\n👥 Step 5: Navigating to /admin/users page...');

        try {
          await page.goto(`${adminUrl}/users`, {
            waitUntil: 'networkidle',
            timeout: 15000
          });
          console.log('✅ Users page URL loaded');

          // Wait for page to be fully loaded
          await page.waitForLoadState('networkidle', { timeout: 10000 });

          // Take screenshot of users page
          await page.screenshot({
            path: 'test-results/admin-users-page.png',
            fullPage: true
          });

          // Step 6: Check if users data is visible
          console.log('\n📋 Step 6: Verifying users data...');

          // Check for users table or data
          const usersTable = await page.$('table, .user-list, .users-container');
          if (usersTable) {
            console.log('✅ Users data container found');

            // Check for actual user rows
            const userRows = await page.$$('table tbody tr, .user-item, .user-row');
            if (userRows.length > 0) {
              console.log(`✅ Found ${userRows.length} user entries`);
              usersDataVisible = true;

              // Get sample user data
              const firstRowText = await userRows[0].textContent();
              console.log(`   Sample data: ${firstRowText?.trim().substring(0, 100)}...`);
            } else {
              // Check for "no users" message
              const noDataMsg = await page.$('.no-data, .empty-state');
              if (noDataMsg) {
                const msgText = await noDataMsg.textContent();
                console.log(`⚠️ No users message: ${msgText?.trim()}`);
                usersDataVisible = true; // Page loaded correctly, just no data
              } else {
                errors.push('No user rows or empty state message found');
                console.error('❌ No user data found');
              }
            }
          } else {
            errors.push('Users data container not found');
            console.error('❌ Users data container not found');
          }

          // Check for loading indicators
          const loadingIndicator = await page.$('.loading, .spinner');
          if (loadingIndicator) {
            console.log('⚠️ Loading indicator still present - data may still be loading');
          }

          // Check for error messages
          const pageError = await page.$('.alert-danger, .error-message');
          if (pageError) {
            const errorText = await pageError.textContent();
            errors.push(`Users page error: ${errorText}`);
            console.error(`❌ Users page error: ${errorText}`);
            usersDataVisible = false;
          }

        } catch (navError) {
          errors.push(`Users page navigation failed: ${navError}`);
          console.error(`❌ Failed to navigate to users page: ${navError}`);
        }
      }

      // Step 7: Final verification screenshot
      await page.screenshot({
        path: 'test-results/admin-final-state.png',
        fullPage: true
      });

    } catch (error) {
      errors.push(`Test execution error: ${error}`);
      console.error(`❌ Test execution error: ${error}`);

      // Take error screenshot
      await page.screenshot({
        path: 'test-results/admin-error-state.png',
        fullPage: true
      });
    }

    // Final Report
    console.log('\n' + '='.repeat(60));
    console.log('📊 ADMIN LOGIN VERIFICATION - FINAL REPORT');
    console.log('='.repeat(60));
    console.log(`\n✅ Login Success:        ${loginSuccess ? 'YES' : 'NO'}`);
    console.log(`✅ Dashboard Loaded:     ${dashboardLoaded ? 'YES' : 'NO'}`);
    console.log(`✅ Users Data Visible:   ${usersDataVisible ? 'YES' : 'NO'}`);

    if (errors.length > 0) {
      console.log(`\n❌ Errors Encountered (${errors.length}):`);
      errors.forEach((err, idx) => {
        console.log(`   ${idx + 1}. ${err}`);
      });
    } else {
      console.log('\n✨ No errors encountered - All checks passed!');
    }

    console.log('\n📸 Screenshots saved:');
    console.log('   - test-results/admin-login-page.png');
    console.log('   - test-results/admin-before-login.png');
    if (loginSuccess) {
      console.log('   - test-results/admin-dashboard-success.png');
      console.log('   - test-results/admin-users-page.png');
    }
    console.log('   - test-results/admin-final-state.png');

    console.log('\n' + '='.repeat(60));

    // Assertions for test framework
    expect(loginSuccess, 'Login should be successful').toBe(true);
    expect(dashboardLoaded, 'Dashboard should load correctly').toBe(true);
    expect(usersDataVisible, 'Users data should be visible').toBe(true);
    expect(errors.length, 'No errors should occur').toBe(0);
  });

  test('Verify authentication state persistence', async ({ page }) => {
    console.log('\n🔄 Testing authentication persistence...');

    // Login first
    await page.goto(`${adminUrl}/login`, { waitUntil: 'networkidle' });
    await page.fill('#admin-login-email-input', credentials.email);
    await page.fill('#admin-login-password-input', credentials.password);
    await page.click('#admin-login-submit-btn');

    await page.waitForURL(/dashboard|\/$/i, { timeout: 15000 });
    console.log('✅ Initial login successful');

    // Navigate to different page
    await page.goto(`${adminUrl}/users`, { waitUntil: 'networkidle' });
    console.log('✅ Navigated to users page');

    // Refresh the page
    await page.reload({ waitUntil: 'networkidle' });
    console.log('✅ Page refreshed');

    // Check if still authenticated (not redirected to login)
    const currentUrl = page.url();
    const isStillAuthenticated = !currentUrl.includes('/login');

    console.log(`Current URL: ${currentUrl}`);
    console.log(`Authentication persisted: ${isStillAuthenticated ? 'YES' : 'NO'}`);

    expect(isStillAuthenticated, 'Authentication should persist after page refresh').toBe(true);
  });

  test('Verify protected routes redirect to login', async ({ page }) => {
    console.log('\n🔒 Testing protected route access...');

    // Try to access dashboard without login
    await page.goto(`${adminUrl}/dashboard`, { waitUntil: 'networkidle' });

    // Should be redirected to login
    await page.waitForURL(/login/i, { timeout: 10000 });

    const currentUrl = page.url();
    const redirectedToLogin = currentUrl.includes('/login');

    console.log(`Current URL: ${currentUrl}`);
    console.log(`Redirected to login: ${redirectedToLogin ? 'YES' : 'NO'}`);

    expect(redirectedToLogin, 'Unauthenticated access should redirect to login').toBe(true);
  });
});
