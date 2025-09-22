import { test, expect } from '@playwright/test';

test('Debug admin email display', async ({ page }) => {
  console.log('Starting email debug test...');

  // Navigate to admin login
  await page.goto('https://localhost:7030/login');
  await page.waitForLoadState('networkidle');

  // Fill login form
  await page.fill('#admin-login-email-input', 'admin@stadium.com');
  await page.fill('#admin-login-password-input', 'admin123');

  // Submit login
  await page.click('#admin-login-submit-btn');

  // Wait for navigation to dashboard
  await page.waitForURL('**/admin', { timeout: 10000 });

  // Wait a moment for authentication state to initialize
  await page.waitForTimeout(2000);

  // Check if email is displayed in header
  const emailElement = await page.locator('small:has-text("👤")').first();
  const emailText = await emailElement.textContent();
  console.log('Email element text:', emailText);

  // Take a screenshot to see the current state
  await page.screenshot({ path: 'debug-email-display.png', fullPage: true });

  // Check localStorage for JWT token
  const token = await page.evaluate(() => localStorage.getItem('stadium_admin_token'));
  console.log('JWT Token:', token ? token.substring(0, 50) + '...' : 'null');

  // Decode JWT payload if token exists
  if (token) {
    const payload = await page.evaluate((token) => {
      try {
        const parts = token.split('.');
        if (parts.length === 3) {
          const payload = atob(parts[1]);
          return JSON.parse(payload);
        }
      } catch (e) {
        return null;
      }
      return null;
    }, token);

    console.log('JWT Payload:', JSON.stringify(payload, null, 2));

    if (payload && payload.email) {
      console.log('✓ Email found in JWT payload:', payload.email);
    } else {
      console.log('✗ No email found in JWT payload');
      console.log('Available claims:', Object.keys(payload || {}));
    }
  }

  // Check console logs for our debugging output
  page.on('console', msg => {
    if (msg.text().includes('JWT Claims Debug') || msg.text().includes('UserEmail property')) {
      console.log('BROWSER CONSOLE:', msg.text());
    }
  });

  // Refresh page to trigger authentication state check
  await page.reload();
  await page.waitForTimeout(2000);

  // Check email display again after refresh
  const emailElementAfterRefresh = await page.locator('small:has-text("👤")').first();
  const emailTextAfterRefresh = await emailElementAfterRefresh.textContent();
  console.log('Email element text after refresh:', emailTextAfterRefresh);

  // Expect the email to be displayed
  expect(emailTextAfterRefresh).toContain('admin@stadium.com');
});