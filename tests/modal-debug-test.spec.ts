import { test, expect } from '@playwright/test';

test.describe('Admin Modal Debug Testing', () => {
  test('Test Online Users modal debug functionality', async ({ page }) => {
    // Navigate to admin application
    await page.goto('https://localhost:9030');

    // Wait for Blazor to initialize (loading spinner to disappear)
    await page.waitForSelector('.spinner-border', { state: 'detached', timeout: 30000 });

    // Wait for login form to appear
    await page.waitForSelector('input[type="email"]', { timeout: 30000 });

    // Login as admin
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for dashboard to load
    await page.waitForSelector('h1', { timeout: 30000 });

    // Take screenshot of the dashboard
    await page.screenshot({ path: 'debug-dashboard.png', fullPage: true });

    // Look for the Modal Debug Panel
    const debugPanel = page.locator('text=🔧 Modal Debug Panel');
    await expect(debugPanel).toBeVisible({ timeout: 10000 });

    console.log('✅ Modal Debug Panel found on dashboard');

    // Test each debug button and capture console logs
    const consoleMessages: string[] = [];
    page.on('console', msg => {
      consoleMessages.push(`${msg.type()}: ${msg.text()}`);
      console.log(`Browser Console [${msg.type()}]: ${msg.text()}`);
    });

    // Test 1: Blazor Click
    console.log('\n🧪 Testing Blazor Click method...');
    const blazorButton = page.locator('button:has-text("Test Blazor Click")');
    await expect(blazorButton).toBeVisible();
    await blazorButton.click();
    await page.waitForTimeout(1000);

    // Test 2: JS Direct
    console.log('\n🧪 Testing JS Direct method...');
    const jsDirectButton = page.locator('button:has-text("Test JS Direct")');
    await expect(jsDirectButton).toBeVisible();
    await jsDirectButton.click();
    await page.waitForTimeout(1000);

    // Test 3: Bootstrap method
    console.log('\n🧪 Testing Bootstrap method...');
    const bootstrapButton = page.locator('button:has-text("Test Bootstrap")');
    await expect(bootstrapButton).toBeVisible();
    await bootstrapButton.click();
    await page.waitForTimeout(1000);

    // Test 4: jQuery method
    console.log('\n🧪 Testing jQuery method...');
    const jqueryButton = page.locator('button:has-text("Test jQuery")');
    await expect(jqueryButton).toBeVisible();
    await jqueryButton.click();
    await page.waitForTimeout(1000);

    // Test 5: Original Online Users card
    console.log('\n🧪 Testing original Online Users card...');
    const onlineUsersCard = page.locator('.card:has(.fas.fa-users)');
    await expect(onlineUsersCard).toBeVisible();
    await onlineUsersCard.click();
    await page.waitForTimeout(1000);

    // Check if modal actually appears
    const modal = page.locator('#onlineUsersModal');
    const isModalVisible = await modal.isVisible();
    console.log(`📊 Modal visibility after all tests: ${isModalVisible}`);

    // Check if modal has Bootstrap classes
    if (await modal.count() > 0) {
      const modalClasses = await modal.getAttribute('class');
      console.log(`📊 Modal classes: ${modalClasses}`);

      const modalStyle = await modal.getAttribute('style');
      console.log(`📊 Modal style: ${modalStyle}`);
    }

    // Take final screenshot
    await page.screenshot({ path: 'debug-final.png', fullPage: true });

    // Print all console messages
    console.log('\n📋 All Browser Console Messages:');
    consoleMessages.forEach((msg, index) => {
      console.log(`${index + 1}. ${msg}`);
    });

    // Check for Bootstrap and jQuery availability
    const hasBootstrap = await page.evaluate(() => {
      return typeof (window as any).bootstrap !== 'undefined';
    });

    const hasJQuery = await page.evaluate(() => {
      return typeof (window as any).$ !== 'undefined';
    });

    console.log(`\n🔍 JavaScript Library Availability:`);
    console.log(`   Bootstrap: ${hasBootstrap}`);
    console.log(`   jQuery: ${hasJQuery}`);

    // Get modal element details
    const modalDetails = await page.evaluate(() => {
      const modal = document.querySelector('#onlineUsersModal');
      if (modal) {
        return {
          exists: true,
          classes: modal.className,
          style: modal.getAttribute('style'),
          display: window.getComputedStyle(modal).display,
          visibility: window.getComputedStyle(modal).visibility,
          opacity: window.getComputedStyle(modal).opacity
        };
      }
      return { exists: false };
    });

    console.log('\n🔍 Modal Element Details:', JSON.stringify(modalDetails, null, 2));
  });
});