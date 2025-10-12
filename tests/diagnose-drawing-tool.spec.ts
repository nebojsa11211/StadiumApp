import { test, expect } from '@playwright/test';

test('Diagnose Stadium Drawing Tool structure', async ({ page }) => {
  console.log('Navigating to admin login...');
  await page.goto('https://localhost:7030/admin/login');

  await page.fill('#admin-login-email-input', 'admin@stadium.com');
  await page.fill('#admin-login-password-input', 'admin123');
  await page.click('#admin-login-submit-btn');

  await page.waitForLoadState('networkidle');
  await page.waitForTimeout(2000);

  console.log('Navigating to Drawing Tool...');
  await page.goto('https://localhost:7030/admin/stadium-drawing-tool');
  await page.waitForLoadState('networkidle');
  await page.waitForTimeout(3000);

  console.log('Taking screenshot...');
  await page.screenshot({
    path: 'D:\\AiApps\\StadiumApp\\StadiumApp\\screenshots\\drawing-tool-diagnostic.png',
    fullPage: true
  });

  // Log all elements with 'sector' in their ID or class
  console.log('\n=== Looking for sector-related elements ===');

  const allElements = await page.locator('[id*="sector"], [class*="sector"]').all();
  console.log(`Found ${allElements.length} elements with 'sector' in ID/class or SECT text`);

  for (let i = 0; i < allElements.length; i++) {
    const elem = allElements[i];
    const tagName = await elem.evaluate(el => el.tagName);
    const id = await elem.getAttribute('id');
    const className = await elem.getAttribute('class');
    const text = await elem.textContent();

    console.log(`\nElement ${i + 1}:`);
    console.log(`  Tag: ${tagName}`);
    console.log(`  ID: ${id}`);
    console.log(`  Class: ${className}`);
    console.log(`  Text: ${text?.substring(0, 50)}`);
  }

  // Check for SVG elements
  console.log('\n=== Looking for SVG elements ===');
  const svgElements = await page.locator('svg').all();
  console.log(`Found ${svgElements.length} SVG elements`);

  // Check for canvas elements
  console.log('\n=== Looking for Canvas elements ===');
  const canvasElements = await page.locator('canvas').all();
  console.log(`Found ${canvasElements.length} Canvas elements`);

  // Check for clickable areas
  console.log('\n=== Looking for clickable sector elements ===');
  const clickableAreas = await page.locator('area, [onclick*="sector"], [data-sector]').all();
  console.log(`Found ${clickableAreas.length} clickable areas`);

  // Try to find SECT2 specifically
  console.log('\n=== Looking specifically for SECT2 ===');
  const sect2Elements = await page.locator('text=SECT2').all();
  console.log(`Found ${sect2Elements.length} elements containing "SECT2"`);

  for (let i = 0; i < sect2Elements.length; i++) {
    const elem = sect2Elements[i];
    const tagName = await elem.evaluate(el => el.tagName);
    const id = await elem.getAttribute('id');
    const className = await elem.getAttribute('class');
    const isClickable = await elem.isEnabled();

    console.log(`\nSECT2 Element ${i + 1}:`);
    console.log(`  Tag: ${tagName}`);
    console.log(`  ID: ${id}`);
    console.log(`  Class: ${className}`);
    console.log(`  Clickable: ${isClickable}`);
  }

  // Get page HTML structure
  console.log('\n=== Page Structure (main container) ===');
  const mainContent = await page.locator('.stadium-drawing-container, #stadium-canvas, .canvas-container').first();
  if (await mainContent.count() > 0) {
    const html = await mainContent.innerHTML();
    console.log(html.substring(0, 500));
  }
});
