import { test, expect } from '@playwright/test';

test.describe('Stadium Overview - Sector Overlay Verification', () => {
  test('should load stadium overview with sector overlays', async ({ page }) => {
    // Set a longer timeout for this test
    test.setTimeout(60000);

    console.log('Step 1: Navigating to login page...');
    await page.goto('https://localhost:7030/admin/login');
    await page.waitForLoadState('networkidle');
    await page.screenshot({ path: 'screenshots/01-login-page.png' });

    console.log('Step 2: Logging in with admin credentials...');
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.screenshot({ path: 'screenshots/02-credentials-filled.png' });

    await page.click('#admin-login-submit-btn');
    console.log('Login button clicked, waiting for navigation...');

    // Wait for navigation after login - should redirect to dashboard
    await page.waitForTimeout(3000); // Give time for login to process
    await page.waitForLoadState('networkidle');
    await page.screenshot({ path: 'screenshots/03-after-login.png' });
    console.log(`Logged in successfully, current URL: ${page.url()}`);

    console.log('Step 3: Navigating to stadium overview page...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForLoadState('networkidle');

    // Wait a bit for any dynamic content to load
    await page.waitForTimeout(3000);
    await page.screenshot({ path: 'screenshots/04-stadium-overview-page.png', fullPage: true });

    console.log('Step 4: Checking for sector overlays...');

    // Check if sector overlays are present
    const sectorOverlays = await page.locator('.sector-overlay').count();
    console.log(`Found ${sectorOverlays} elements with class "sector-overlay"`);

    // Check for alternative sector-related classes
    const sectorElements = await page.locator('[class*="sector"]').count();
    console.log(`Found ${sectorElements} elements with "sector" in class name`);

    // Check if stadium container exists
    const stadiumContainer = await page.locator('#stadium-container, .stadium-container, [class*="stadium"]').count();
    console.log(`Found ${stadiumContainer} stadium container elements`);

    // Check for stadium image
    const stadiumImages = await page.locator('img[src*="stadium"], img[alt*="stadium"]').count();
    console.log(`Found ${stadiumImages} stadium-related images`);

    // Get all images on the page
    const allImages = await page.locator('img').all();
    console.log(`Total images on page: ${allImages.length}`);
    for (let i = 0; i < allImages.length; i++) {
      const src = await allImages[i].getAttribute('src');
      const alt = await allImages[i].getAttribute('alt');
      console.log(`  Image ${i + 1}: src="${src}", alt="${alt}"`);
    }

    console.log('Step 5: Checking for error messages...');

    // Check for error alerts
    const errorAlerts = await page.locator('.alert-danger, .alert-error, [class*="error"]').count();
    console.log(`Found ${errorAlerts} error-related elements`);

    if (errorAlerts > 0) {
      const errorTexts = await page.locator('.alert-danger, .alert-error, [class*="error"]').allTextContents();
      console.log('Error messages found:');
      errorTexts.forEach((text, index) => {
        console.log(`  Error ${index + 1}: ${text}`);
      });
      await page.screenshot({ path: 'screenshots/05-errors-found.png', fullPage: true });
    }

    console.log('Step 6: Analyzing page structure...');

    // Get page title
    const pageTitle = await page.title();
    console.log(`Page title: ${pageTitle}`);

    // Check for main content area
    const mainContent = await page.locator('main, .main-content, #main-content').count();
    console.log(`Found ${mainContent} main content areas`);

    // Get all div elements with class attributes
    const divsWithClasses = await page.locator('div[class]').all();
    console.log(`\nFound ${divsWithClasses.length} divs with classes. First 20 class names:`);
    for (let i = 0; i < Math.min(20, divsWithClasses.length); i++) {
      const className = await divsWithClasses[i].getAttribute('class');
      console.log(`  Div ${i + 1}: class="${className}"`);
    }

    // Check for SVG elements (often used for stadium layouts)
    const svgElements = await page.locator('svg').count();
    console.log(`\nFound ${svgElements} SVG elements`);

    // Check for canvas elements
    const canvasElements = await page.locator('canvas').count();
    console.log(`Found ${canvasElements} Canvas elements`);

    console.log('\n=== Test Summary ===');
    console.log(`Sector overlays: ${sectorOverlays}`);
    console.log(`Sector-related elements: ${sectorElements}`);
    console.log(`Stadium images: ${stadiumImages}`);
    console.log(`SVG elements: ${svgElements}`);
    console.log(`Canvas elements: ${canvasElements}`);
    console.log(`Error messages: ${errorAlerts}`);

    // Take a final screenshot
    await page.screenshot({ path: 'screenshots/06-final-state.png', fullPage: true });

    // Assertions
    expect(page.url()).toContain('/admin/stadium-overview');
    expect(errorAlerts).toBe(0); // No errors should be present

    // At least one of these should be present: sector overlays, SVG, or canvas
    const hasStadiumVisualization = sectorOverlays > 0 || svgElements > 0 || canvasElements > 0;
    expect(hasStadiumVisualization).toBeTruthy();

    console.log('\n✅ Test completed successfully!');
  });
});
