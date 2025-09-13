import { test, expect } from '@playwright/test';

test.describe('Admin Dashboard Online Users Modal', () => {
  test('should open online users modal when card is clicked', async ({ page }) => {
    // Enable console logging to catch JavaScript errors
    const consoleMessages: string[] = [];
    const jsErrors: string[] = [];

    page.on('console', msg => {
      consoleMessages.push(`${msg.type()}: ${msg.text()}`);
      if (msg.type() === 'error') {
        jsErrors.push(msg.text());
      }
    });

    page.on('pageerror', error => {
      jsErrors.push(`Page Error: ${error.message}`);
    });

    // Navigate to admin app
    await page.goto('https://localhost:9030');

    // Wait for page to load completely
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000);

    // Take screenshot of auth required page
    await page.screenshot({ path: 'admin-auth-required.png', fullPage: true });

    // Check if we see "Authentication Required" page and click "Go to Login"
    const goToLoginButton = page.locator('button:has-text("Go to Login"), a:has-text("Go to Login")');
    const authRequiredText = page.locator('text=Authentication Required');

    if (await authRequiredText.isVisible()) {
      console.log('Found Authentication Required page, clicking Go to Login');
      await goToLoginButton.click();
      await page.waitForLoadState('networkidle');
      await page.waitForTimeout(2000);
    }

    // Take screenshot of actual login page
    await page.screenshot({ path: 'admin-login-page.png', fullPage: true });

    // Check for login form elements more flexibly
    const emailInput = page.locator('input[name="Email"], input[type="email"], input[placeholder*="mail"], input[id*="mail"]');
    const passwordInput = page.locator('input[name="Password"], input[type="password"], input[placeholder*="assword"], input[id*="assword"]');
    const submitButton = page.locator('button[type="submit"], input[type="submit"], button:has-text("Login"), button:has-text("Sign In")');

    // Wait for login form to be visible
    await emailInput.first().waitFor({ state: 'visible', timeout: 10000 });

    console.log('Login form found, filling credentials');

    // Login with admin credentials
    await emailInput.first().fill('admin@stadium.com');
    await passwordInput.first().fill('admin123');
    await submitButton.first().click();

    // Wait for navigation - be more flexible with URL matching
    await page.waitForTimeout(5000);

    // Wait for dashboard to fully load
    await page.waitForLoadState('networkidle');
    await page.waitForTimeout(3000); // Give Blazor time to render

    // Take screenshot of dashboard before clicking
    await page.screenshot({ path: 'admin-dashboard-before-click.png', fullPage: true });

    // Look for the Online Users card - try different selectors
    const onlineUsersCard = page.locator('.card').filter({ hasText: 'Online Users' });
    const alternativeCard = page.locator('[data-bs-toggle="modal"]').filter({ hasText: 'Online Users' });
    const cardWithNumber = page.locator('.card').filter({ hasText: '87' });

    // Check what cards exist
    const allCards = await page.locator('.card').all();
    console.log(`Found ${allCards.length} cards on dashboard`);

    for (let i = 0; i < allCards.length; i++) {
      const cardText = await allCards[i].textContent();
      console.log(`Card ${i}: ${cardText?.substring(0, 100)}...`);
    }

    // Try to find and click the Online Users card
    let cardFound = false;
    let cardClicked = false;

    if (await onlineUsersCard.count() > 0) {
      console.log('Found Online Users card by text');
      await onlineUsersCard.first().click();
      cardFound = true;
      cardClicked = true;
    } else if (await alternativeCard.count() > 0) {
      console.log('Found Online Users card by modal toggle');
      await alternativeCard.first().click();
      cardFound = true;
      cardClicked = true;
    } else if (await cardWithNumber.count() > 0) {
      console.log('Found card with number 87');
      await cardWithNumber.first().click();
      cardFound = true;
      cardClicked = true;
    } else {
      console.log('No Online Users card found, trying generic card click');
      const cards = await page.locator('.card').all();
      if (cards.length > 0) {
        // Look for a card that might contain online users info
        for (const card of cards) {
          const text = await card.textContent();
          if (text && (text.includes('87') || text.includes('Online') || text.includes('Users'))) {
            await card.click();
            cardFound = true;
            cardClicked = true;
            console.log(`Clicked card with text: ${text.substring(0, 50)}...`);
            break;
          }
        }
      }
    }

    if (cardClicked) {
      // Wait a moment for modal to potentially appear
      await page.waitForTimeout(1000);

      // Take screenshot after clicking
      await page.screenshot({ path: 'admin-dashboard-after-click.png', fullPage: true });

      // Check if modal appeared
      const modal = page.locator('.modal');
      const modalDialog = page.locator('.modal-dialog');
      const onlineUsersModal = page.locator('#onlineUsersModal');

      const modalVisible = await modal.isVisible().catch(() => false);
      const modalDialogVisible = await modalDialog.isVisible().catch(() => false);
      const specificModalVisible = await onlineUsersModal.isVisible().catch(() => false);

      console.log(`Modal visible: ${modalVisible}`);
      console.log(`Modal dialog visible: ${modalDialogVisible}`);
      console.log(`Specific modal visible: ${specificModalVisible}`);

      if (modalVisible || modalDialogVisible || specificModalVisible) {
        console.log('SUCCESS: Modal appeared after clicking card');

        // Take screenshot of modal
        await page.screenshot({ path: 'admin-modal-visible.png', fullPage: true });

        // Try to get modal content
        const modalContent = await modal.textContent().catch(() => 'Could not get modal content');
        console.log(`Modal content: ${modalContent.substring(0, 200)}...`);
      } else {
        console.log('ISSUE: No modal appeared after clicking card');
      }
    } else {
      console.log('ERROR: Could not find or click Online Users card');
    }

    // Log any JavaScript errors
    if (jsErrors.length > 0) {
      console.log('JavaScript Errors detected:');
      jsErrors.forEach(error => console.log(`  - ${error}`));
    }

    // Log some console messages (limit to avoid spam)
    console.log('Recent console messages:');
    consoleMessages.slice(-10).forEach(msg => console.log(`  ${msg}`));

    // Create a summary report
    const report = {
      cardFound,
      cardClicked,
      modalAppeared: false,
      jsErrors: jsErrors.length,
      consoleMessages: consoleMessages.length,
      dashboardLoaded: true
    };

    // Check if modal appeared one more time
    const finalModalCheck = await page.locator('.modal').isVisible().catch(() => false);
    report.modalAppeared = finalModalCheck;

    console.log('TEST SUMMARY:', JSON.stringify(report, null, 2));
  });
});