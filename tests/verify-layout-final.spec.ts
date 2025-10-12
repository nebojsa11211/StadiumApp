import { test, expect } from '@playwright/test';

test('Verify Customer Layout is Correct', async ({ page }) => {
  console.log('🔍 Opening Customer app...');

  // Navigate to customer app
  await page.goto('https://localhost:8081', {
    waitUntil: 'networkidle',
    timeout: 30000
  });

  // Wait for page to fully load
  await page.waitForTimeout(2000);

  console.log('📸 Taking full page screenshot...');
  await page.screenshot({
    path: 'customer-layout-verification.png',
    fullPage: true
  });

  // Check if top bar exists
  const topBar = page.locator('.stadium-top-bar');
  await expect(topBar).toBeVisible();
  console.log('✅ Top bar is visible');

  // Check if navigation exists
  const nav = page.locator('.stadium-nav');
  await expect(nav).toBeVisible();
  console.log('✅ Navigation bar is visible');

  // Check if nav links are visible
  const navLinks = page.locator('.nav-links');
  await expect(navLinks).toBeVisible();
  console.log('✅ Navigation links are visible');

  // Get layout measurements
  const layout = await page.evaluate(() => {
    const topBar = document.querySelector('.stadium-top-bar') as HTMLElement;
    const nav = document.querySelector('.stadium-nav') as HTMLElement;
    const page = document.querySelector('.page') as HTMLElement;

    return {
      topBarPosition: topBar?.getBoundingClientRect(),
      navPosition: nav?.getBoundingClientRect(),
      pageFlexDirection: window.getComputedStyle(page).flexDirection,
      topBarWidth: topBar?.offsetWidth,
      navWidth: nav?.offsetWidth,
      windowWidth: window.innerWidth
    };
  });

  console.log('\n📊 Layout Measurements:');
  console.log('  Page flex direction:', layout.pageFlexDirection);
  console.log('  Window width:', layout.windowWidth, 'px');
  console.log('  Top bar width:', layout.topBarWidth, 'px');
  console.log('  Nav bar width:', layout.navWidth, 'px');
  console.log('  Top bar position:', `top=${layout.topBarPosition.top}, left=${layout.topBarPosition.left}`);
  console.log('  Nav bar position:', `top=${layout.navPosition.top}, left=${layout.navPosition.left}`);

  // Verify layout is correct
  expect(layout.pageFlexDirection).toBe('column');
  console.log('✅ Page flex direction is COLUMN (vertical stacking)');

  // Verify nav is below top bar
  expect(layout.navPosition.top).toBeGreaterThan(layout.topBarPosition.bottom);
  console.log('✅ Navigation is positioned BELOW top bar');

  // Verify both are full width (allowing for some tolerance)
  expect(layout.topBarWidth).toBeGreaterThan(layout.windowWidth - 50);
  expect(layout.navWidth).toBeGreaterThan(layout.windowWidth - 50);
  console.log('✅ Both top bar and nav are FULL WIDTH');

  // Check if nav links are horizontal
  const navLinksLayout = await page.evaluate(() => {
    const navLinks = document.querySelector('.nav-links') as HTMLElement;
    const firstLink = navLinks?.querySelector('li:first-child') as HTMLElement;
    const secondLink = navLinks?.querySelector('li:nth-child(2)') as HTMLElement;

    return {
      navLinksDisplay: window.getComputedStyle(navLinks).display,
      firstLinkRect: firstLink?.getBoundingClientRect(),
      secondLinkRect: secondLink?.getBoundingClientRect(),
      linkCount: navLinks?.querySelectorAll('li').length
    };
  });

  console.log('\n🔗 Navigation Links:');
  console.log('  Display mode:', navLinksLayout.navLinksDisplay);
  console.log('  Total links:', navLinksLayout.linkCount);
  console.log('  First link top:', navLinksLayout.firstLinkRect.top);
  console.log('  Second link top:', navLinksLayout.secondLinkRect.top);

  // Verify links are horizontal (same vertical position)
  const verticalDifference = Math.abs(
    navLinksLayout.firstLinkRect.top - navLinksLayout.secondLinkRect.top
  );
  expect(verticalDifference).toBeLessThan(5); // Allow small rounding differences
  console.log('✅ Navigation links are HORIZONTAL (not stacked vertically)');

  // Check content area
  const main = page.locator('main');
  await expect(main).toBeVisible();
  console.log('✅ Main content area is visible');

  console.log('\n🎉 LAYOUT VERIFICATION COMPLETE!');
  console.log('✅ All checks passed - layout is correct!');
});
