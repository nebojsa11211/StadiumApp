import { test, expect } from '@playwright/test';

test.describe('Admin Dashboard Modal Functionality', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to admin login
    await page.goto('https://localhost:9030/login');

    // Handle certificate warning if it appears
    try {
      await page.getByText('Advanced').click({ timeout: 2000 });
      await page.getByText('Continue').click({ timeout: 2000 });
    } catch {
      // Certificate already accepted or not required
    }

    // Login as admin
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for dashboard to load
    await page.waitForSelector('text=Stadium Dashboard', { timeout: 10000 });
  });

  test('should open Online Users modal when clicking the card', async ({ page }) => {
    // Click on Online Users card
    const onlineUsersCard = page.locator('.card.border-info').first();
    await onlineUsersCard.click();

    // Wait a moment for modal to potentially open
    await page.waitForTimeout(2000);

    // Check if modal is visible
    const modal = page.locator('#onlineUsersModal');
    const isModalVisible = await modal.isVisible();

    if (!isModalVisible) {
      console.log('Modal not visible - checking for JavaScript errors');

      // Check browser console for errors
      page.on('console', msg => {
        if (msg.type() === 'error') {
          console.log('Console error:', msg.text());
        }
      });

      // Try clicking again
      await onlineUsersCard.click();
      await page.waitForTimeout(2000);
    }

    // Take screenshot for debugging
    await page.screenshot({ path: 'modal-test-online-users.png', fullPage: true });

    // Verify modal is visible
    await expect(modal).toBeVisible({ timeout: 5000 });

    // Verify modal content
    await expect(page.locator('#onlineUsersModal .modal-title')).toContainText('Online Users');
  });

  test('should open Active Orders modal when clicking the card', async ({ page }) => {
    // Click on Active Orders card
    const activeOrdersCard = page.locator('.card.border-success').first();
    await activeOrdersCard.click();

    // Wait for modal
    await page.waitForTimeout(2000);

    // Take screenshot
    await page.screenshot({ path: 'modal-test-active-orders.png', fullPage: true });

    // Check if modal is visible
    const modal = page.locator('#activeOrdersModal');
    await expect(modal).toBeVisible({ timeout: 5000 });

    // Verify modal content
    await expect(page.locator('#activeOrdersModal .modal-title')).toContainText('Active Orders');
  });

  test('should open Revenue Details modal when clicking the card', async ({ page }) => {
    // Click on Revenue card
    const revenueCard = page.locator('.card.border-primary').first();
    await revenueCard.click();

    // Wait for modal
    await page.waitForTimeout(2000);

    // Take screenshot
    await page.screenshot({ path: 'modal-test-revenue.png', fullPage: true });

    // Check if modal is visible
    const modal = page.locator('#revenueModal');
    await expect(modal).toBeVisible({ timeout: 5000 });

    // Verify modal content
    await expect(page.locator('#revenueModal .modal-title')).toContainText('Revenue Details');
  });

  test('should open Ticket Sales modal when clicking the card', async ({ page }) => {
    // Click on Tickets card
    const ticketsCard = page.locator('.card.border-warning').first();
    await ticketsCard.click();

    // Wait for modal
    await page.waitForTimeout(2000);

    // Take screenshot
    await page.screenshot({ path: 'modal-test-tickets.png', fullPage: true });

    // Check if modal is visible
    const modal = page.locator('#ticketModal');
    await expect(modal).toBeVisible({ timeout: 5000 });

    // Verify modal content
    await expect(page.locator('#ticketModal .modal-title')).toContainText('Ticket Sales Details');
  });

  test('check JavaScript console for errors', async ({ page }) => {
    // Set up console listener
    const consoleErrors: string[] = [];
    page.on('console', msg => {
      if (msg.type() === 'error') {
        consoleErrors.push(msg.text());
      }
    });

    // Try clicking each card
    await page.locator('.card.border-info').first().click();
    await page.waitForTimeout(1000);

    await page.locator('.card.border-success').first().click();
    await page.waitForTimeout(1000);

    await page.locator('.card.border-primary').first().click();
    await page.waitForTimeout(1000);

    await page.locator('.card.border-warning').first().click();
    await page.waitForTimeout(1000);

    // Report any errors found
    if (consoleErrors.length > 0) {
      console.log('JavaScript errors found:');
      consoleErrors.forEach(error => console.log('  -', error));
    } else {
      console.log('No JavaScript errors detected');
    }

    // Assert no errors
    expect(consoleErrors.length).toBe(0);
  });
});