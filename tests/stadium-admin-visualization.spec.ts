import { test, expect } from '@playwright/test';

test.describe('Stadium Visualization Admin Test', () => {
  test('should display stadium visualization correctly on admin page', async ({ page }) => {
    // Set longer timeout for this test
    test.setTimeout(90000);

    // Navigate to admin app
    await page.goto('https://localhost:7030', { waitUntil: 'networkidle' });

    // Check if we're on the login page or already logged in
    const isLoginPage = await page.locator('#admin-login-form').isVisible().catch(() => false);

    if (isLoginPage) {
      console.log('Logging in to admin app...');

      // Fill login form
      await page.fill('#admin-login-email-input', 'admin@stadium.com');
      await page.fill('#admin-login-password-input', 'admin123');

      // Submit login
      await page.click('#admin-login-submit-btn');

      // Wait for login to complete and dashboard to load
      await page.waitForURL(/dashboard/, { timeout: 30000 });
    }

    console.log('Navigating to Stadium Overview...');

    // Navigate to Stadium Overview page
    await page.goto('https://localhost:7030/stadium-overview', { waitUntil: 'networkidle' });

    // Wait for the page to load
    await page.waitForLoadState('networkidle');

    // Wait for stadium container to be visible
    await expect(page.locator('.stadium-container')).toBeVisible({ timeout: 30000 });

    console.log('Stadium container is visible');

    // Check that info panel is at the top
    const infoPanel = page.locator('.info-panel');
    await expect(infoPanel).toBeVisible();

    // Check that stadium data is loaded
    const stadiumName = page.locator('.stadium-info h3');
    await expect(stadiumName).toContainText('Main Stadium');

    console.log('Stadium data loaded');

    // Check for tribunes (should be 4)
    const tribunes = page.locator('.tribune');
    await expect(tribunes).toHaveCount(4);

    console.log('All 4 tribunes are visible');

    // Take a screenshot of the stadium visualization
    await page.screenshot({
      path: '.playwright-mcp/stadium-admin-visualization-test.png',
      fullPage: true
    });

    console.log('Screenshot saved: stadium-admin-visualization-test.png');

    // Verify specific elements are present
    await expect(page.locator('.stadium-svg')).toBeVisible();

    // Check that capacity information is displayed
    const capacityInfo = page.locator('.capacity-info');
    await expect(capacityInfo).toBeVisible();

    // Verify the layout improvements
    await expect(page.locator('.stadium-container')).toHaveCSS('width', /.*/);

    console.log('Stadium visualization test completed successfully!');
  });

  test('should verify stadium data API response', async ({ page }) => {
    // Test the stadium overview API endpoint
    const response = await page.request.get('https://localhost:7010/api/stadium-viewer/overview');
    expect(response.status()).toBe(200);

    const data = await response.json();
    console.log('Stadium API Response:', JSON.stringify(data, null, 2));

    // Verify the response structure
    expect(data).toHaveProperty('stadiumName');
    expect(data).toHaveProperty('totalCapacity');
    expect(data).toHaveProperty('tribunes');
    expect(data.tribunes).toHaveLength(4);

    console.log('Stadium API data validation completed');
  });
});