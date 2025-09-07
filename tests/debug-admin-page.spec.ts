import { test, expect } from '@playwright/test';
import { testConfig } from './config';

test('Debug Admin Login Page DOM', async ({ page }) => {
  // Navigate to admin login
  await page.goto(testConfig.adminApp + '/login');
  
  // Wait for loading to complete
  await page.waitForLoadState('networkidle', { timeout: 60000 });
  
  // Wait for loading spinner to disappear
  await page.waitForFunction(() => {
    const body = document.body.textContent || '';
    return !body.includes('Initializing application...') && !body.includes('Loading...');
  }, { timeout: 45000 });
  
  // Additional wait
  await page.waitForTimeout(5000);
  
  // Take a screenshot to see what we have
  await page.screenshot({ path: 'admin-login-debug.png', fullPage: true });
  
  // Get all the text content
  const bodyText = await page.textContent('body');
  console.log('Page body text:', bodyText);
  
  // Get all h1, h2, h3 elements
  const headings = await page.locator('h1, h2, h3, h4, h5').allTextContents();
  console.log('Page headings:', headings);
  
  // Get all form elements
  const forms = await page.locator('form').count();
  console.log('Number of forms:', forms);
  
  // Get all input elements
  const inputs = await page.locator('input').all();
  for (let i = 0; i < inputs.length; i++) {
    const input = inputs[i];
    const id = await input.getAttribute('id');
    const type = await input.getAttribute('type');
    const placeholder = await input.getAttribute('placeholder');
    console.log(`Input ${i}: id=${id}, type=${type}, placeholder=${placeholder}`);
  }
  
  // Get all button elements
  const buttons = await page.locator('button').all();
  for (let i = 0; i < buttons.length; i++) {
    const button = buttons[i];
    const id = await button.getAttribute('id');
    const text = await button.textContent();
    console.log(`Button ${i}: id=${id}, text=${text}`);
  }
  
  // Check if specific elements exist
  const adminTitleExists = await page.locator('#admin-login-title').count();
  const adminEmailExists = await page.locator('#admin-login-email-input').count();
  console.log('Admin title element count:', adminTitleExists);
  console.log('Admin email input count:', adminEmailExists);
  
  // This should pass - we're just debugging
  expect(bodyText).toBeTruthy();
});