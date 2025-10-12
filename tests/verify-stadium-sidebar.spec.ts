import { test, expect } from '@playwright/test';

test.describe('Stadium Overview - Sidebar Visibility', () => {
  test('should show admin sidebar on Stadium Overview page', async ({ page }) => {
    // Navigate to login page
    await page.goto('https://localhost:7030/login');

    // Login as admin
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for navigation to complete
    await page.waitForURL('https://localhost:7030/', { timeout: 10000 });

    // Navigate to Stadium Overview
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');

    // Check if sidebar is visible
    const sidebar = page.locator('#admin-layout-sidebar');
    await expect(sidebar).toBeVisible({ timeout: 5000 });

    // Verify sidebar contains navigation menu
    const navMenu = sidebar.locator('.nav-menu, nav, [class*="nav"]').first();
    await expect(navMenu).toBeVisible();

    // Check that stadium overview content is also visible
    const stadiumWrapper = page.locator('.stadium-overview-wrapper');
    await expect(stadiumWrapper).toBeVisible();

    // Verify both sidebar and content are in viewport
    const sidebarBox = await sidebar.boundingBox();
    const contentBox = await stadiumWrapper.boundingBox();

    expect(sidebarBox).toBeTruthy();
    expect(contentBox).toBeTruthy();

    if (sidebarBox && contentBox) {
      // Sidebar should be on the left
      expect(sidebarBox.x).toBeLessThan(contentBox.x);

      // Both should be visible (not overlapping completely)
      console.log('Sidebar position:', { x: sidebarBox.x, y: sidebarBox.y, width: sidebarBox.width, height: sidebarBox.height });
      console.log('Content position:', { x: contentBox.x, y: contentBox.y, width: contentBox.width, height: contentBox.height });
    }

    // Take a screenshot
    await page.screenshot({ path: 'stadium-overview-sidebar-check.png', fullPage: true });

    console.log('✅ Sidebar visibility verified on Stadium Overview page');
  });
});
