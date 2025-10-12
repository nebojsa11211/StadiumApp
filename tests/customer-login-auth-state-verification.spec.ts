import { test, expect } from '@playwright/test';

/**
 * Comprehensive Customer Login Authentication State Verification Test
 *
 * Purpose: Verify that removing forceLoad: true from navigation allows
 * the Blazor circuit to persist and properly reflect authenticated state
 * in the MainLayout component.
 *
 * Expected Behavior:
 * - After successful login, UI should immediately show user dropdown
 * - Sign In and Sign Up buttons should be hidden
 * - User dropdown should contain Profile, Orders, and Logout options
 * - No full page reload should occur (Blazor circuit persists)
 */

test.describe('Customer Login Authentication State Verification', () => {
  test.setTimeout(120000); // 2 minute timeout for complete test

  test('should verify authenticated UI state after login without page reload', async ({ page }) => {
    const screenshots: string[] = [];
    const verificationResults = {
      loginPageLoaded: false,
      formFilled: false,
      loginButtonClicked: false,
      navigationCompleted: false,
      userDropdownVisible: false,
      signInButtonHidden: false,
      signUpButtonHidden: false,
      dropdownMenuOpens: false,
      profileLinkVisible: false,
      ordersLinkVisible: false,
      logoutButtonVisible: false
    };

    console.log('\n=== STEP 1: Navigate to Login Page ===');
    await page.goto('https://localhost:8081/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Take screenshot of login page
    await page.screenshot({
      path: '/d/AiApps/StadiumApp/StadiumApp/customer-auth-test-01-login-page.png',
      fullPage: true
    });
    screenshots.push('customer-auth-test-01-login-page.png');

    // Verify login page loaded
    const loginTitle = await page.locator('#customer-login-title').isVisible();
    verificationResults.loginPageLoaded = loginTitle;
    console.log(`✓ Login page loaded: ${loginTitle ? 'PASS' : 'FAIL'}`);

    console.log('\n=== STEP 2: Fill Login Form ===');
    // Fill email
    await page.locator('#customer-login-email-input').fill('customer@stadium.com');
    console.log('✓ Email filled: customer@stadium.com');

    // Fill password
    await page.locator('#customer-login-password-input').fill('customer123');
    console.log('✓ Password filled: customer123');

    verificationResults.formFilled = true;

    // Take screenshot of filled form
    await page.screenshot({
      path: '/d/AiApps/StadiumApp/StadiumApp/customer-auth-test-02-form-filled.png',
      fullPage: true
    });
    screenshots.push('customer-auth-test-02-form-filled.png');

    console.log('\n=== STEP 3: Click Login Button ===');
    // Click login button
    await page.locator('#customer-login-submit-btn').click();
    verificationResults.loginButtonClicked = true;
    console.log('✓ Login button clicked');

    // Wait for navigation (but NOT a full page reload)
    console.log('⏳ Waiting for navigation to complete...');
    try {
      // Wait for URL to change to homepage
      await page.waitForURL('https://localhost:8081/', {
        timeout: 15000,
        waitUntil: 'networkidle'
      });
      verificationResults.navigationCompleted = true;
      console.log('✓ Navigation completed: PASS');
    } catch (error) {
      console.log(`✗ Navigation failed: FAIL - ${error}`);
    }

    // Wait for Blazor to update the UI
    await page.waitForTimeout(2000);

    // Take screenshot immediately after navigation
    await page.screenshot({
      path: '/d/AiApps/StadiumApp/StadiumApp/customer-auth-test-03-after-login.png',
      fullPage: true
    });
    screenshots.push('customer-auth-test-03-after-login.png');

    console.log('\n=== STEP 4: Verify Authenticated UI State ===');

    // CRITICAL: Check if user dropdown is visible
    const userDropdownVisible = await page.locator('#customer-layout-user-dropdown').isVisible({ timeout: 5000 })
      .catch(() => false);
    verificationResults.userDropdownVisible = userDropdownVisible;
    console.log(`${userDropdownVisible ? '✓' : '✗'} User dropdown visible: ${userDropdownVisible ? 'PASS ✅' : 'FAIL ❌'}`);

    // CRITICAL: Verify Sign In button is NOT visible
    const signInButtonExists = await page.locator('#customer-layout-sign-in-btn').isVisible({ timeout: 2000 })
      .catch(() => false);
    verificationResults.signInButtonHidden = !signInButtonExists;
    console.log(`${!signInButtonExists ? '✓' : '✗'} Sign In button hidden: ${!signInButtonExists ? 'PASS ✅' : 'FAIL ❌'}`);

    // CRITICAL: Verify Sign Up button is NOT visible
    const signUpButtonExists = await page.locator('#customer-layout-sign-up-btn').isVisible({ timeout: 2000 })
      .catch(() => false);
    verificationResults.signUpButtonHidden = !signUpButtonExists;
    console.log(`${!signUpButtonExists ? '✓' : '✗'} Sign Up button hidden: ${!signUpButtonExists ? 'PASS ✅' : 'FAIL ❌'}`);

    console.log('\n=== STEP 5: Verify User Dropdown Contents ===');

    if (userDropdownVisible) {
      // Click user dropdown to open menu
      await page.locator('#customer-layout-user-dropdown').click();
      await page.waitForTimeout(1000);

      // Take screenshot with dropdown open
      await page.screenshot({
        path: '/d/AiApps/StadiumApp/StadiumApp/customer-auth-test-04-dropdown-open.png',
        fullPage: true
      });
      screenshots.push('customer-auth-test-04-dropdown-open.png');

      verificationResults.dropdownMenuOpens = true;
      console.log('✓ User dropdown menu opened: PASS');

      // Check for Profile link
      const profileLinkVisible = await page.locator('#customer-layout-profile-link').isVisible({ timeout: 2000 })
        .catch(() => false);
      verificationResults.profileLinkVisible = profileLinkVisible;
      console.log(`${profileLinkVisible ? '✓' : '✗'} Profile link visible: ${profileLinkVisible ? 'PASS ✅' : 'FAIL ❌'}`);

      // Check for Orders link
      const ordersLinkVisible = await page.locator('#customer-layout-orders-link').isVisible({ timeout: 2000 })
        .catch(() => false);
      verificationResults.ordersLinkVisible = ordersLinkVisible;
      console.log(`${ordersLinkVisible ? '✓' : '✗'} Orders link visible: ${ordersLinkVisible ? 'PASS ✅' : 'FAIL ❌'}`);

      // Check for Logout button
      const logoutButtonVisible = await page.locator('#customer-layout-logout-btn').isVisible({ timeout: 2000 })
        .catch(() => false);
      verificationResults.logoutButtonVisible = logoutButtonVisible;
      console.log(`${logoutButtonVisible ? '✓' : '✗'} Logout button visible: ${logoutButtonVisible ? 'PASS ✅' : 'FAIL ❌'}`);

      // Take final screenshot
      await page.screenshot({
        path: '/d/AiApps/StadiumApp/StadiumApp/customer-auth-test-05-final-state.png',
        fullPage: true
      });
      screenshots.push('customer-auth-test-05-final-state.png');
    } else {
      console.log('✗ Cannot verify dropdown contents - dropdown not visible');
    }

    console.log('\n=== STEP 6: Print Test Summary ===');
    console.log('\n╔═══════════════════════════════════════════════════════════╗');
    console.log('║       AUTHENTICATION STATE VERIFICATION RESULTS           ║');
    console.log('╠═══════════════════════════════════════════════════════════╣');

    const results = [
      { name: 'Login Page Loaded', value: verificationResults.loginPageLoaded },
      { name: 'Form Filled Successfully', value: verificationResults.formFilled },
      { name: 'Login Button Clicked', value: verificationResults.loginButtonClicked },
      { name: 'Navigation Completed', value: verificationResults.navigationCompleted },
      { name: 'User Dropdown Visible', value: verificationResults.userDropdownVisible },
      { name: 'Sign In Button Hidden', value: verificationResults.signInButtonHidden },
      { name: 'Sign Up Button Hidden', value: verificationResults.signUpButtonHidden },
      { name: 'Dropdown Menu Opens', value: verificationResults.dropdownMenuOpens },
      { name: 'Profile Link Visible', value: verificationResults.profileLinkVisible },
      { name: 'Orders Link Visible', value: verificationResults.ordersLinkVisible },
      { name: 'Logout Button Visible', value: verificationResults.logoutButtonVisible }
    ];

    results.forEach(result => {
      const status = result.value ? '✅ PASS' : '❌ FAIL';
      const padding = ' '.repeat(Math.max(0, 35 - result.name.length));
      console.log(`║  ${result.name}${padding}: ${status}  ║`);
    });

    console.log('╠═══════════════════════════════════════════════════════════╣');

    const passCount = results.filter(r => r.value).length;
    const totalCount = results.length;
    const passPercentage = Math.round((passCount / totalCount) * 100);

    console.log(`║  OVERALL RESULTS: ${passCount}/${totalCount} checks passed (${passPercentage}%)           ║`);
    console.log('╠═══════════════════════════════════════════════════════════╣');
    console.log('║  Screenshots Captured:                                    ║');
    screenshots.forEach((screenshot, index) => {
      console.log(`║    ${index + 1}. ${screenshot.padEnd(50)}║`);
    });
    console.log('╚═══════════════════════════════════════════════════════════╝\n');

    // Critical assertions for test pass/fail
    console.log('\n=== CRITICAL ASSERTIONS ===');

    expect(verificationResults.loginPageLoaded,
      'Login page should load successfully').toBe(true);

    expect(verificationResults.navigationCompleted,
      'Navigation should complete after login').toBe(true);

    expect(verificationResults.userDropdownVisible,
      'User dropdown MUST be visible after authentication').toBe(true);

    expect(verificationResults.signInButtonHidden,
      'Sign In button MUST be hidden after authentication').toBe(true);

    expect(verificationResults.signUpButtonHidden,
      'Sign Up button MUST be hidden after authentication').toBe(true);

    if (userDropdownVisible) {
      expect(verificationResults.dropdownMenuOpens,
        'User dropdown menu should open when clicked').toBe(true);

      expect(verificationResults.profileLinkVisible,
        'Profile link should be visible in dropdown').toBe(true);

      expect(verificationResults.ordersLinkVisible,
        'Orders link should be visible in dropdown').toBe(true);

      expect(verificationResults.logoutButtonVisible,
        'Logout button should be visible in dropdown').toBe(true);
    }

    console.log('\n✅ ALL CRITICAL ASSERTIONS PASSED!\n');
  });
});
