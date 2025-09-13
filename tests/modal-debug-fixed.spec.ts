import { test, expect } from '@playwright/test';

test.describe('Admin Modal Debug - Fixed Flow', () => {
  test('Test Online Users modal debug with proper login flow', async ({ page }) => {
    page.setDefaultTimeout(60000);

    console.log('🚀 Starting admin modal debug test...');

    // Navigate to admin application
    console.log('📍 Navigating to https://localhost:9030');
    await page.goto('https://localhost:9030');

    // Wait for page to load
    await page.waitForTimeout(3000);

    // Check if we see "Authentication Required" page
    const authRequiredText = await page.locator('text=🔐 Authentication Required').count();
    console.log(`🔒 Authentication Required page: ${authRequiredText > 0 ? 'Found' : 'Not found'}`);

    if (authRequiredText > 0) {
      // Click "Go to Login" link
      console.log('🖱️ Clicking "Go to Login" link...');
      await page.click('text=Go to Login');

      // Wait for login page to load
      await page.waitForTimeout(3000);
    }

    // Wait for login form to appear
    await page.waitForSelector('input[type="email"]', { timeout: 30000 });

    console.log('✅ Login form found, attempting login...');

    // Login as admin
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for dashboard to load
    await page.waitForSelector('h1', { timeout: 30000 });
    console.log('✅ Dashboard loaded');

    // Take screenshot of the dashboard
    await page.screenshot({ path: 'debug-dashboard-modal.png', fullPage: true });

    // Look for the Modal Debug Panel
    const debugPanel = page.locator('text=🔧 Modal Debug Panel');
    const debugPanelVisible = await debugPanel.count();
    console.log(`🔧 Modal Debug Panel: ${debugPanelVisible > 0 ? 'Found' : 'Not found'}`);

    if (debugPanelVisible === 0) {
      console.log('❌ Debug panel not found. Looking for what is on the page:');
      const bodyText = await page.locator('body').innerText();
      console.log('📝 Page content:', bodyText.substring(0, 1000));
      return;
    }

    // Start capturing console messages
    const consoleMessages: string[] = [];
    page.on('console', msg => {
      const message = `${msg.type()}: ${msg.text()}`;
      consoleMessages.push(message);
      console.log(`Browser Console [${msg.type()}]: ${msg.text()}`);
    });

    // Test debug buttons one by one
    const testButtons = [
      'Test Blazor Click',
      'Test JS Direct',
      'Test Bootstrap',
      'Test jQuery'
    ];

    for (const buttonText of testButtons) {
      console.log(`\n🧪 Testing "${buttonText}"...`);

      const button = page.locator(`button:has-text("${buttonText}")`);
      const buttonCount = await button.count();

      if (buttonCount > 0) {
        await button.click();
        await page.waitForTimeout(1500); // Wait longer for modal

        // Check if modal is visible after this method
        const modal = page.locator('#onlineUsersModal');
        const isModalVisible = await modal.isVisible();
        console.log(`   📊 Modal visible after ${buttonText}: ${isModalVisible}`);

        // If modal is visible, take screenshot
        if (isModalVisible) {
          await page.screenshot({ path: `modal-visible-${buttonText.replace(/\s+/g, '-').toLowerCase()}.png` });
          console.log(`   📸 Modal screenshot saved`);
        }
      } else {
        console.log(`   ❌ Button "${buttonText}" not found`);
      }
    }

    // Test the original Online Users card
    console.log('\n🧪 Testing original Online Users card...');
    const onlineUsersCard = page.locator('.card:has(.fas.fa-users)');
    const cardCount = await onlineUsersCard.count();

    if (cardCount > 0) {
      await onlineUsersCard.click();
      await page.waitForTimeout(1500);

      const modal = page.locator('#onlineUsersModal');
      const isModalVisible = await modal.isVisible();
      console.log(`   📊 Modal visible after card click: ${isModalVisible}`);
    } else {
      console.log(`   ❌ Online Users card not found`);
    }

    // Check JavaScript libraries availability
    const jsLibraries = await page.evaluate(() => {
      return {
        bootstrap: typeof (window as any).bootstrap !== 'undefined',
        jquery: typeof (window as any).$ !== 'undefined',
        blazor: typeof (window as any).Blazor !== 'undefined'
      };
    });

    console.log('\n🔍 JavaScript Libraries Available:');
    console.log(`   Bootstrap: ${jsLibraries.bootstrap}`);
    console.log(`   jQuery: ${jsLibraries.jquery}`);
    console.log(`   Blazor: ${jsLibraries.blazor}`);

    // Get detailed modal information
    const modalInfo = await page.evaluate(() => {
      const modal = document.querySelector('#onlineUsersModal');
      if (!modal) return { exists: false };

      const computedStyle = window.getComputedStyle(modal);
      return {
        exists: true,
        classes: modal.className,
        style: modal.getAttribute('style') || 'none',
        display: computedStyle.display,
        visibility: computedStyle.visibility,
        opacity: computedStyle.opacity,
        zIndex: computedStyle.zIndex,
        position: computedStyle.position
      };
    });

    console.log('\n🔍 Modal Element Details:');
    console.log(JSON.stringify(modalInfo, null, 2));

    // Print all console messages
    console.log('\n📋 Browser Console Messages:');
    consoleMessages.forEach((msg, index) => {
      console.log(`${index + 1}. ${msg}`);
    });

    // Final screenshot
    await page.screenshot({ path: 'modal-debug-final.png', fullPage: true });
    console.log('\n📸 Final screenshot saved: modal-debug-final.png');
  });
});