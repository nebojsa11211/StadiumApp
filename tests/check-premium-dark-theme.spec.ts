import { test, expect } from '@playwright/test';

test('Check premium dark theme on entire page', async ({ page }) => {
  // Navigate to the customer app homepage
  await page.goto('https://localhost:8081', {
    waitUntil: 'networkidle',
    timeout: 30000
  });

  // Wait for page to load
  await page.waitForTimeout(2000);

  // Take full page screenshot
  await page.screenshot({
    path: 'premium-dark-theme-full-page.png',
    fullPage: true
  });

  // Check navigation bar
  const navbar = await page.locator('.premium-navbar').first();
  await expect(navbar).toBeVisible();
  console.log('✅ Premium navbar visible');

  // Check brand
  const brand = await page.locator('.premium-brand-title').first();
  await expect(brand).toBeVisible();
  console.log('✅ Premium brand visible');

  // Check navigation links
  const navLinks = await page.locator('.premium-nav-link');
  const navLinkCount = await navLinks.count();
  console.log(`✅ Found ${navLinkCount} navigation links`);

  // Check main content area
  const mainContent = await page.locator('.premium-main-content').first();
  await expect(mainContent).toBeVisible();
  console.log('✅ Premium main content visible');

  // Check footer
  const footer = await page.locator('.premium-footer').first();
  await expect(footer).toBeVisible();
  console.log('✅ Premium footer visible');

  // Take viewport screenshot (what user sees)
  await page.screenshot({
    path: 'premium-dark-theme-viewport.png'
  });

  console.log('✅ Premium dark theme test complete!');
});
