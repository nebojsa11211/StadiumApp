import { test, expect } from '@playwright/test';

test('debug login form structure', async ({ page }) => {
  await page.goto('https://localhost:9030', { ignoreHTTPSErrors: true });

  // Wait for page to load
  await page.waitForTimeout(5000);

  // Take a screenshot
  await page.screenshot({ path: 'login-debug.png', fullPage: true });

  // Log current URL
  console.log('Current URL:', page.url());

  // Log page title
  const title = await page.title();
  console.log('Page title:', title);

  // Find all input elements
  const inputs = await page.locator('input').all();
  console.log(`Found ${inputs.length} input elements`);

  for (let i = 0; i < inputs.length; i++) {
    const input = inputs[i];
    const type = await input.getAttribute('type');
    const name = await input.getAttribute('name');
    const id = await input.getAttribute('id');
    const placeholder = await input.getAttribute('placeholder');
    console.log(`Input ${i}: type=${type}, name=${name}, id=${id}, placeholder=${placeholder}`);
  }

  // Find all forms
  const forms = await page.locator('form').all();
  console.log(`Found ${forms.length} form elements`);

  // Find buttons
  const buttons = await page.locator('button').all();
  console.log(`Found ${buttons.length} button elements`);

  for (let i = 0; i < buttons.length; i++) {
    const button = buttons[i];
    const type = await button.getAttribute('type');
    const text = await button.textContent();
    console.log(`Button ${i}: type=${type}, text=${text}`);
  }

  // Check for "Go to Login" button and click it
  const loginButton = page.locator('button:has-text("Go to Login")');
  if (await loginButton.isVisible()) {
    console.log('Found "Go to Login" button, clicking...');
    await loginButton.click();
    await page.waitForTimeout(3000);

    // Check URL after clicking
    console.log('URL after clicking login button:', page.url());

    // Take another screenshot
    await page.screenshot({ path: 'after-login-click.png', fullPage: true });

    // Find inputs again
    const inputsAfter = await page.locator('input').all();
    console.log(`Found ${inputsAfter.length} input elements after clicking login`);

    for (let i = 0; i < inputsAfter.length; i++) {
      const input = inputsAfter[i];
      const type = await input.getAttribute('type');
      const name = await input.getAttribute('name');
      const id = await input.getAttribute('id');
      const placeholder = await input.getAttribute('placeholder');
      console.log(`Input ${i}: type=${type}, name=${name}, id=${id}, placeholder=${placeholder}`);
    }
  }

  // Check for any error messages
  const errorElements = await page.locator('.error, .alert, .alert-danger, .text-danger').all();
  console.log(`Found ${errorElements.length} error elements`);

  for (let i = 0; i < errorElements.length; i++) {
    const error = errorElements[i];
    const text = await error.textContent();
    console.log(`Error ${i}: ${text}`);
  }
});