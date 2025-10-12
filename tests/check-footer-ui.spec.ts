import { test, expect } from '@playwright/test';

test('Check footer UI and take screenshot', async ({ page }) => {
  // Navigate to the customer app
  await page.goto('https://localhost:8081', {
    waitUntil: 'networkidle',
    timeout: 30000
  });

  // Wait for the page to load
  await page.waitForTimeout(2000);

  // Scroll to the footer
  await page.evaluate(() => window.scrollTo(0, document.body.scrollHeight));
  await page.waitForTimeout(1000);

  // Take a full page screenshot
  await page.screenshot({
    path: 'footer-full-view.png',
    fullPage: true
  });

  // Take footer-specific screenshot
  const footer = await page.locator('footer').first();
  if (await footer.isVisible()) {
    await footer.screenshot({ path: 'footer-section.png' });

    // Get footer dimensions
    const box = await footer.boundingBox();
    console.log('Footer dimensions:', box);

    // Check for specific elements
    const brandLogo = await page.locator('footer .text-5xl').first();
    const statsCards = await page.locator('footer .grid.grid-cols-3');
    const newsletter = await page.locator('footer form');

    console.log('Brand logo visible:', await brandLogo.isVisible());
    console.log('Stats cards visible:', await statsCards.isVisible());
    console.log('Newsletter visible:', await newsletter.isVisible());

    // Check for wave divider
    const waveSvg = await page.locator('svg[data-name="Layer 1"]');
    console.log('Wave divider visible:', await waveSvg.isVisible());
  }

  // Take a viewport screenshot at the footer
  await page.screenshot({
    path: 'footer-viewport.png',
    fullPage: false
  });
});
