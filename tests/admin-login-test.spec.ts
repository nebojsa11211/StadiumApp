import { test, expect } from '@playwright/test';

test('admin login test with connection pooling', async ({ page }) => {
  const startTime = Date.now();

  console.log('Starting admin login test...');

  // Navigate to admin login
  await page.goto('https://localhost:7030/login');
  console.log('Navigated to login page');

  // Fill login form
  await page.fill('#customer-login-email-input', 'admin@stadium.com');
  await page.fill('#customer-login-password-input', 'admin123');
  console.log('Filled login credentials');

  // Click login button
  await page.click('#customer-login-submit-btn');
  console.log('Clicked login button');

  // Wait for redirect to dashboard (max 30 seconds)
  try {
    await page.waitForURL('**/admin', { timeout: 30000 });
    const loginTime = ((Date.now() - startTime) / 1000).toFixed(2);
    console.log(`✅ Login successful in ${loginTime} seconds`);

    // Verify we're on the dashboard
    await expect(page).toHaveURL(/.*admin$/);

    // Report results
    if (parseFloat(loginTime) < 10) {
      console.log('✅ PASS: Login completed quickly (under 10 seconds)');
    } else if (parseFloat(loginTime) < 20) {
      console.log('⚠️ WARNING: Login took longer than expected (10-20 seconds)');
    } else {
      console.log('❌ FAIL: Login timeout issue detected (over 20 seconds)');
    }

    console.log(`\n📊 Login Performance Report:`);
    console.log(`   Login Time: ${loginTime} seconds`);
    console.log(`   Status: ${parseFloat(loginTime) < 10 ? 'PASS' : 'FAIL'}`);
    console.log(`   Expected: < 10 seconds for proper pooling configuration\n`);

  } catch (error) {
    const loginTime = ((Date.now() - startTime) / 1000).toFixed(2);
    console.error(`❌ Login failed after ${loginTime} seconds`);
    throw error;
  }
});
