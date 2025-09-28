import { test, expect } from '@playwright/test';

test('Stadium Visualization Debug Investigation', async ({ page }) => {
  // Enable console logging
  page.on('console', msg => {
    console.log(`[BROWSER] ${msg.type()}: ${msg.text()}`);
  });

  // Enable error logging
  page.on('pageerror', error => {
    console.log(`[PAGE ERROR] ${error.message}`);
  });

  // Enable network failure logging
  page.on('requestfailed', request => {
    console.log(`[REQUEST FAILED] ${request.method()} ${request.url()} - ${request.failure()?.errorText}`);
  });

  console.log('🔍 Starting stadium visualization debug investigation...');

  try {
    // Navigate to Admin application
    console.log('📍 Navigating to Admin login page...');
    await page.goto('https://localhost:9030/login', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Take initial screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-debug-login.png',
      fullPage: true
    });

    // Login with admin credentials
    console.log('🔑 Logging in as admin...');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');

    // Wait for dashboard to load
    await page.waitForURL('**/admin/dashboard', { timeout: 15000 });
    console.log('✅ Successfully logged in');

    // Navigate to stadium overview
    console.log('📍 Navigating to stadium overview page...');
    await page.goto('https://localhost:9030/admin/stadium-overview', {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    // Take screenshot of stadium overview page
    await page.screenshot({
      path: '.playwright-mcp/stadium-overview-full.png',
      fullPage: true
    });

    // Wait a bit for any dynamic content to load
    await page.waitForTimeout(3000);

    // Check page title and basic elements
    const pageTitle = await page.title();
    console.log(`📄 Page title: ${pageTitle}`);

    // Check if main container exists
    const mainContainer = await page.locator('[id*="stadium"], [class*="stadium"], canvas, svg').count();
    console.log(`🏗️ Found ${mainContainer} stadium-related elements`);

    // Check for canvas elements specifically
    const canvasElements = await page.locator('canvas').count();
    console.log(`🎨 Found ${canvasElements} canvas elements`);

    // Check for SVG elements
    const svgElements = await page.locator('svg').count();
    console.log(`📐 Found ${svgElements} SVG elements`);

    // Get all elements on the page and their visibility
    const allElements = await page.evaluate(() => {
      const elements: Array<{selector: string, visible: boolean, hasContent: boolean, clientRect: any}> = [];

      // Check specific stadium-related selectors
      const selectors = [
        'canvas',
        'svg',
        '[id*="stadium"]',
        '[class*="stadium"]',
        '[id*="viewer"]',
        '[class*="viewer"]',
        '.stadium-map',
        '.stadium-container',
        '#stadium-canvas',
        '#stadium-svg'
      ];

      selectors.forEach(selector => {
        const els = document.querySelectorAll(selector);
        els.forEach((el, index) => {
          const rect = el.getBoundingClientRect();
          const computedStyle = window.getComputedStyle(el);
          elements.push({
            selector: `${selector}[${index}]`,
            visible: computedStyle.display !== 'none' &&
                    computedStyle.visibility !== 'hidden' &&
                    computedStyle.opacity !== '0',
            hasContent: rect.width > 0 && rect.height > 0,
            clientRect: {
              width: rect.width,
              height: rect.height,
              x: rect.x,
              y: rect.y
            }
          });
        });
      });

      return elements;
    });

    console.log('🔍 Stadium visualization elements analysis:');
    allElements.forEach(el => {
      console.log(`  ${el.selector}: visible=${el.visible}, hasSize=${el.hasContent}, size=${el.clientRect.width}x${el.clientRect.height}`);
    });

    // Check if there are any error messages on the page
    const errorElements = await page.locator('.alert-danger, .text-danger, .error, [class*="error"]').count();
    console.log(`⚠️ Found ${errorElements} error-related elements`);

    if (errorElements > 0) {
      const errorTexts = await page.locator('.alert-danger, .text-danger, .error, [class*="error"]').allTextContents();
      console.log('Error messages found:');
      errorTexts.forEach(text => console.log(`  - ${text}`));
    }

    // Check page content for clues
    const pageContent = await page.textContent('body');
    const hasStadiumText = pageContent?.includes('stadium') || pageContent?.includes('Stadium');
    console.log(`📝 Page contains stadium-related text: ${hasStadiumText}`);

    // Check for specific stadium overview elements
    const specificElements = await page.evaluate(() => {
      const results: any = {};

      // Check for common stadium visualization containers
      results.stadiumContainer = document.querySelector('[id*="stadium-container"], [class*="stadium-container"]') !== null;
      results.stadiumCanvas = document.querySelector('canvas') !== null;
      results.stadiumSvg = document.querySelector('svg') !== null;
      results.loadingIndicator = document.querySelector('[class*="loading"], [class*="spinner"]') !== null;
      results.noDataMessage = document.querySelector('[class*="no-data"], [class*="empty"]') !== null;

      // Get the actual HTML content
      results.bodyHtml = document.body.innerHTML.substring(0, 1000); // First 1000 chars

      return results;
    });

    console.log('🔍 Specific element checks:');
    Object.entries(specificElements).forEach(([key, value]) => {
      if (key !== 'bodyHtml') {
        console.log(`  ${key}: ${value}`);
      }
    });

    // Check network requests for stadium data
    console.log('🌐 Monitoring network requests...');
    const responses: string[] = [];
    page.on('response', response => {
      if (response.url().includes('stadium') || response.url().includes('viewer') || response.url().includes('api')) {
        responses.push(`${response.status()} ${response.url()}`);
      }
    });

    // Reload page to capture network requests
    await page.reload({ waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    console.log('📡 Stadium-related network requests:');
    responses.forEach(resp => console.log(`  ${resp}`));

    // Final screenshot after reload
    await page.screenshot({
      path: '.playwright-mcp/stadium-overview-after-reload.png',
      fullPage: true
    });

    // Log final HTML structure around stadium area
    const htmlStructure = await page.evaluate(() => {
      const body = document.body;
      const walker = document.createTreeWalker(
        body,
        NodeFilter.SHOW_ELEMENT,
        {
          acceptNode: (node) => {
            const element = node as Element;
            const id = element.id?.toLowerCase() || '';
            const className = element.className?.toLowerCase() || '';

            if (id.includes('stadium') || className.includes('stadium') ||
                element.tagName.toLowerCase() === 'canvas' ||
                element.tagName.toLowerCase() === 'svg') {
              return NodeFilter.FILTER_ACCEPT;
            }
            return NodeFilter.FILTER_SKIP;
          }
        }
      );

      const relevantElements: string[] = [];
      let node;
      while (node = walker.nextNode()) {
        const element = node as Element;
        relevantElements.push(
          `<${element.tagName.toLowerCase()} id="${element.id}" class="${element.className}" style="${element.getAttribute('style')}">${element.textContent?.substring(0, 100) || 'no-text'}</${element.tagName.toLowerCase()}>`
        );
      }

      return relevantElements;
    });

    console.log('🏗️ HTML structure of stadium-related elements:');
    htmlStructure.forEach(el => console.log(`  ${el}`));

  } catch (error) {
    console.error('❌ Error during investigation:', error);

    // Take error screenshot
    await page.screenshot({
      path: '.playwright-mcp/stadium-debug-error.png',
      fullPage: true
    });
  }

  console.log('🏁 Stadium visualization debug investigation completed');
});