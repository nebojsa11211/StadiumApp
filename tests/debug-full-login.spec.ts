import { test, expect } from '@playwright/test';

test('debug full login process', async ({ page }) => {
  await page.goto('https://localhost:9030/', { ignoreHTTPSErrors: true });

  // Take initial screenshot
  await page.screenshot({ path: 'login-step-1.png', fullPage: true });

  // Click "Go to Login" button
  const goToLoginButton = page.locator('button:has-text("Go to Login")');
  if (await goToLoginButton.isVisible({ timeout: 5000 })) {
    console.log('✅ Found "Go to Login" button');
    await goToLoginButton.click();
    await page.waitForTimeout(2000);
  }

  console.log('URL after clicking Go to Login:', page.url());
  await page.screenshot({ path: 'login-step-2.png', fullPage: true });

  // Fill in credentials
  const emailInput = page.locator('#admin-login-email-input');
  const passwordInput = page.locator('#admin-login-password-input');

  if (await emailInput.isVisible({ timeout: 5000 })) {
    console.log('✅ Email input found');
    await emailInput.fill('admin@stadium.com');
  } else {
    console.log('❌ Email input not found');
  }

  if (await passwordInput.isVisible({ timeout: 5000 })) {
    console.log('✅ Password input found');
    await passwordInput.fill('admin123');
  } else {
    console.log('❌ Password input not found');
  }

  await page.screenshot({ path: 'login-step-3.png', fullPage: true });

  // Find and click submit button
  const submitButtons = await page.locator('button').all();
  console.log(`Found ${submitButtons.length} buttons on login page`);

  for (let i = 0; i < submitButtons.length; i++) {
    const button = submitButtons[i];
    const type = await button.getAttribute('type');
    const text = await button.textContent();
    console.log(`Button ${i}: type=${type}, text="${text?.trim()}"`);
  }

  const submitButton = page.locator('button[type="submit"]');
  if (await submitButton.isVisible({ timeout: 2000 })) {
    console.log('✅ Submit button found, clicking...');
    await submitButton.click();
  } else {
    // Try any primary button
    const primaryButton = page.locator('.btn-primary, .btn:has-text("Login"), .btn:has-text("Sign")');
    if (await primaryButton.isVisible({ timeout: 2000 })) {
      console.log('✅ Primary button found, clicking...');
      await primaryButton.click();
    } else {
      console.log('❌ No submit button found');
      return;
    }
  }

  // Wait for response
  await page.waitForTimeout(5000);

  console.log('URL after submitting login:', page.url());
  await page.screenshot({ path: 'login-step-4.png', fullPage: true });

  // Check for any error messages
  const errorElements = await page.locator('.error, .alert-danger, .text-danger, .validation-summary-errors').all();
  console.log(`Found ${errorElements.length} error elements`);

  for (let i = 0; i < errorElements.length; i++) {
    const error = errorElements[i];
    const text = await error.textContent();
    if (text && text.trim()) {
      console.log(`Error ${i}: ${text.trim()}`);
    }
  }

  // Check current page content
  const pageContent = await page.content();
  if (pageContent.includes('Dashboard') || pageContent.includes('dashboard')) {
    console.log('✅ Dashboard content found');
  } else {
    console.log('❌ Dashboard content not found');
  }
});