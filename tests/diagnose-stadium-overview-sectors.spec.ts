import { test, expect } from '@playwright/test';

test.describe('Stadium Overview Sector Overlay Diagnostic', () => {
  test.setTimeout(180000); // 3 minutes for comprehensive diagnostic

  let apiContext;

  test.beforeAll(async ({ playwright }) => {
    apiContext = await playwright.request.newContext({
      baseURL: 'https://localhost:7010',
      ignoreHTTPSErrors: true,
    });
  });

  test.afterAll(async () => {
    await apiContext?.dispose();
  });

  // Helper function for login
  async function loginAsAdmin(page) {
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(2000);
  }

  test('COMPREHENSIVE DIAGNOSTIC: Sector Overlay Visibility', async ({ page }) => {
    console.log('\n==============================================');
    console.log('STADIUM OVERVIEW SECTOR DIAGNOSTIC TEST');
    console.log('==============================================\n');

    // ==========================================
    // PHASE 1: Check Database API
    // ==========================================
    console.log('PHASE 1: Database API Check');
    console.log('----------------------------');

    try {
      const apiResponse = await apiContext.get('/api/StadiumSectorOverlay');
      const apiStatus = apiResponse.status();
      console.log(`✓ API Status: ${apiStatus}`);

      if (apiStatus === 200) {
        const sectorData = await apiResponse.json();
        console.log(`✓ Sectors in Database: ${sectorData.length}`);

        if (sectorData.length > 0) {
          console.log('\n📊 Sample Sector Data:');
          console.log(JSON.stringify(sectorData[0], null, 2));

          console.log('\n📋 All Sector Codes:');
          sectorData.forEach((sector, index) => {
            console.log(`  ${index + 1}. ${sector.sectorCode} - ${sector.name} (${sector.shapeType})`);
          });
        } else {
          console.log('❌ NO SECTORS FOUND IN DATABASE - This is the problem!');
        }
      } else {
        console.log(`❌ API Error: Status ${apiStatus}`);
        const errorText = await apiResponse.text();
        console.log(`   Error: ${errorText}`);
      }
    } catch (error) {
      console.log(`❌ API Call Failed: ${error.message}`);
    }

    console.log('\n');

    // ==========================================
    // PHASE 2: Check Drawing Tool
    // ==========================================
    console.log('PHASE 2: Drawing Tool Check');
    console.log('----------------------------');

    await loginAsAdmin(page);
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Take screenshot of drawing tool
    await page.screenshot({
      path: 'diagnostic-01-drawing-tool.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: diagnostic-01-drawing-tool.png');

    // Check if canvas exists
    const canvasExists = await page.locator('canvas#admin-drawing-tool-canvas').count() > 0;
    console.log(`✓ Canvas exists: ${canvasExists}`);

    // Check browser console logs for JavaScript errors
    const consoleLogs = [];
    page.on('console', msg => {
      const text = msg.text();
      consoleLogs.push(`[${msg.type()}] ${text}`);
      if (msg.type() === 'error') {
        console.log(`❌ Browser Console Error: ${text}`);
      }
    });

    // Wait for any JavaScript to complete
    await page.waitForTimeout(2000);

    // Check if sectors are loaded in drawing tool
    const drawingToolLog = await page.evaluate(() => {
      // Try to get sector data from JavaScript
      return {
        windowHasSectors: typeof window['sectorOverlays'] !== 'undefined',
        canvasInitialized: typeof window['initializeDrawingCanvas'] !== 'undefined',
      };
    });

    console.log(`✓ Canvas initialized: ${drawingToolLog.canvasInitialized}`);

    console.log('\n');

    // ==========================================
    // PHASE 3: Check Stadium Overview Page
    // ==========================================
    console.log('PHASE 3: Stadium Overview Page Check');
    console.log('--------------------------------------');

    await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Take screenshot of overview page
    await page.screenshot({
      path: 'diagnostic-02-stadium-overview.png',
      fullPage: true
    });
    console.log('✓ Screenshot saved: diagnostic-02-stadium-overview.png');

    // Check if stadium image exists
    const imageExists = await page.locator('img#admin-stadium-overview-blueprint-img').count() > 0;
    console.log(`✓ Stadium blueprint image exists: ${imageExists}`);

    let imageLoadedInfo = null;
    if (imageExists) {
      const imageSrc = await page.locator('img#admin-stadium-overview-blueprint-img').getAttribute('src');
      console.log(`✓ Image source: ${imageSrc}`);

      // Check if image is loaded
      imageLoadedInfo = await page.locator('img#admin-stadium-overview-blueprint-img').evaluate((img: HTMLImageElement) => {
        return {
          complete: img.complete,
          naturalWidth: img.naturalWidth,
          naturalHeight: img.naturalHeight,
          width: img.width,
          height: img.height
        };
      });
      console.log(`✓ Image loaded: ${imageLoadedInfo.complete}`);
      console.log(`  Natural size: ${imageLoadedInfo.naturalWidth}x${imageLoadedInfo.naturalHeight}`);
      console.log(`  Displayed size: ${imageLoadedInfo.width}x${imageLoadedInfo.height}`);
    }

    // Check if image container exists
    const containerExists = await page.locator('#admin-stadium-overview-image-container').count() > 0;
    console.log(`✓ Image container exists: ${containerExists}`);

    if (containerExists) {
      // Get container dimensions and position
      const containerInfo = await page.locator('#admin-stadium-overview-image-container').boundingBox();
      console.log(`✓ Container position: ${JSON.stringify(containerInfo)}`);
    }

    // ==========================================
    // PHASE 4: Check for Sector Overlay DIVs
    // ==========================================
    console.log('\nPHASE 4: Sector Overlay DIV Check');
    console.log('----------------------------------');

    const sectorOverlayCount = await page.locator('.sector-overlay').count();
    console.log(`✓ Sector overlay DIVs found: ${sectorOverlayCount}`);

    if (sectorOverlayCount > 0) {
      console.log('\n✅ SECTOR OVERLAYS ARE RENDERED IN DOM!');

      // Get details of each sector overlay
      const overlayDetails = await page.locator('.sector-overlay').evaluateAll((elements: HTMLElement[]) => {
        return elements.map((el, index) => ({
          index: index + 1,
          id: el.id,
          style: el.getAttribute('style'),
          classes: el.className,
          visible: window.getComputedStyle(el).display !== 'none',
          zIndex: window.getComputedStyle(el).zIndex,
          position: window.getComputedStyle(el).position,
          top: window.getComputedStyle(el).top,
          left: window.getComputedStyle(el).left,
          width: window.getComputedStyle(el).width,
          height: window.getComputedStyle(el).height,
          backgroundColor: window.getComputedStyle(el).backgroundColor,
          borderColor: window.getComputedStyle(el).borderColor,
          opacity: window.getComputedStyle(el).opacity,
          display: window.getComputedStyle(el).display,
          boundingBox: el.getBoundingClientRect()
        }));
      });

      console.log('\n📐 Sector Overlay Details:');
      overlayDetails.forEach(detail => {
        console.log(`\nSector ${detail.index}:`);
        console.log(`  ID: ${detail.id}`);
        console.log(`  Classes: ${detail.classes}`);
        console.log(`  Inline Style: ${detail.style}`);
        console.log(`  Computed Position: ${detail.position} (${detail.top}, ${detail.left})`);
        console.log(`  Computed Size: ${detail.width} x ${detail.height}`);
        console.log(`  Z-Index: ${detail.zIndex}`);
        console.log(`  Display: ${detail.display}`);
        console.log(`  Visible: ${detail.visible}`);
        console.log(`  Background: ${detail.backgroundColor}`);
        console.log(`  Border: ${detail.borderColor}`);
        console.log(`  Opacity: ${detail.opacity}`);
        console.log(`  Bounding Box: ${JSON.stringify(detail.boundingBox)}`);
      });
    } else {
      console.log('❌ NO SECTOR OVERLAY DIVS IN DOM');

      // Check if loading state is showing
      const loadingExists = await page.locator('#admin-stadium-overview-loading').count() > 0;
      console.log(`  Loading indicator visible: ${loadingExists}`);

      // Check if no-data message is showing
      const noDataExists = await page.locator('#admin-stadium-overview-no-data').count() > 0;
      console.log(`  No-data message visible: ${noDataExists}`);

      if (noDataExists) {
        const noDataText = await page.locator('#admin-stadium-overview-no-data').textContent();
        console.log(`  Message: ${noDataText}`);
      }
    }

    // ==========================================
    // PHASE 5: Check CSS Styles
    // ==========================================
    console.log('\nPHASE 5: CSS Styles Check');
    console.log('--------------------------');

    // Check if CSS file is loaded
    const cssLoaded = await page.evaluate(() => {
      const links = Array.from(document.querySelectorAll('link[rel="stylesheet"]'));
      return links.map((link: HTMLLinkElement) => link.href);
    });

    console.log('✓ Loaded CSS files:');
    cssLoaded.forEach(css => console.log(`  - ${css}`));

    const overlayCSS = cssLoaded.find(css => css.includes('stadium-image-overlay'));
    if (overlayCSS) {
      console.log('✅ stadium-image-overlay.css is loaded');
    } else {
      console.log('❌ stadium-image-overlay.css NOT FOUND');
    }

    // ==========================================
    // PHASE 6: Check Component State
    // ==========================================
    console.log('\nPHASE 6: Component State Check');
    console.log('-------------------------------');

    // Check network calls
    const networkCalls = [];
    page.on('response', response => {
      if (response.url().includes('StadiumSectorOverlay')) {
        networkCalls.push({
          url: response.url(),
          status: response.status(),
          statusText: response.statusText()
        });
      }
    });

    // Reload page to capture network calls
    await page.reload({ waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    console.log('\n📡 Network Calls to StadiumSectorOverlay API:');
    if (networkCalls.length > 0) {
      networkCalls.forEach(call => {
        console.log(`  ${call.status} - ${call.url}`);
      });
    } else {
      console.log('  ❌ No API calls detected - Component might not be calling API');
    }

    // Check browser console for component logs
    console.log('\n📝 Browser Console Logs (last 50):');
    consoleLogs.slice(-50).forEach(log => console.log(`  ${log}`));

    // ==========================================
    // PHASE 7: Root Cause Analysis
    // ==========================================
    console.log('\n\n==============================================');
    console.log('ROOT CAUSE ANALYSIS');
    console.log('==============================================\n');

    const diagnosticResults = {
      apiWorking: false,
      sectorsInDatabase: 0,
      sectorDivsRendered: sectorOverlayCount,
      imageLoaded: imageExists && imageLoadedInfo?.complete,
      cssLoaded: !!overlayCSS,
      apiCallsMade: networkCalls.length > 0
    };

    try {
      const apiResponse = await apiContext.get('/api/StadiumSectorOverlay');
      if (apiResponse.status() === 200) {
        const sectorData = await apiResponse.json();
        diagnosticResults.apiWorking = true;
        diagnosticResults.sectorsInDatabase = sectorData.length;
      }
    } catch (error) {
      // API check failed
    }

    console.log('DIAGNOSTIC SUMMARY:');
    console.log('-------------------');
    console.log(`API Working: ${diagnosticResults.apiWorking ? '✅' : '❌'}`);
    console.log(`Sectors in Database: ${diagnosticResults.sectorsInDatabase}`);
    console.log(`Sector DIVs Rendered: ${diagnosticResults.sectorDivsRendered}`);
    console.log(`Stadium Image Loaded: ${diagnosticResults.imageLoaded ? '✅' : '❌'}`);
    console.log(`CSS Loaded: ${diagnosticResults.cssLoaded ? '✅' : '❌'}`);
    console.log(`API Calls Made: ${diagnosticResults.apiCallsMade ? '✅' : '❌'}`);

    console.log('\n\nROOT CAUSE DETERMINATION:');
    console.log('-------------------------');

    if (!diagnosticResults.apiWorking) {
      console.log('❌ ISSUE: API is not responding');
      console.log('   FIX: Check if StadiumSectorOverlayController is running');
      console.log('   FILE: StadiumDrinkOrdering.API/Controllers/StadiumSectorOverlayController.cs');
    } else if (diagnosticResults.sectorsInDatabase === 0) {
      console.log('❌ ISSUE: No sectors exist in database');
      console.log('   FIX: Create sectors using Stadium Drawing Tool');
      console.log('   ACTION: Go to /admin/stadium-drawing-tool and create sectors');
    } else if (!diagnosticResults.apiCallsMade) {
      console.log('❌ ISSUE: Component is not calling the API');
      console.log('   FIX: Check LoadSectorOverlayConfig() method in StadiumOverview.razor.cs');
      console.log('   FILE: StadiumDrinkOrdering.Admin/Pages/StadiumOverview.razor.cs');
      console.log('   LINE: ~76-128');
    } else if (diagnosticResults.sectorDivsRendered === 0) {
      console.log('❌ ISSUE: Sector DIVs are not being rendered');
      console.log('   POSSIBLE CAUSES:');
      console.log('   1. sectorOverlays list is null or empty in component');
      console.log('   2. Razor @foreach loop condition is not met (line 61-67)');
      console.log('   3. Component is showing loading/no-data message instead');
      console.log('   FIX: Check component state in StadiumOverview.razor.cs');
      console.log('   FILE: StadiumDrinkOrdering.Admin/Pages/StadiumOverview.razor.cs');
      console.log('   CHECK: Lines 61-67 in StadiumOverview.razor for rendering conditions');
    } else if (!diagnosticResults.cssLoaded) {
      console.log('❌ ISSUE: CSS file not loaded');
      console.log('   FIX: Add reference to stadium-image-overlay.css in _Layout.cshtml');
      console.log('   FILE: StadiumDrinkOrdering.Admin/Pages/_Layout.cshtml');
    } else if (diagnosticResults.sectorDivsRendered > 0 && !diagnosticResults.imageLoaded) {
      console.log('⚠️  ISSUE: Sectors rendered but image not loaded');
      console.log('   FIX: Check stadium-blueprint.png exists in wwwroot/images/');
      console.log('   FILE: StadiumDrinkOrdering.Admin/wwwroot/images/stadium-blueprint.png');
    } else {
      console.log('✅ ALL COMPONENTS WORKING - Sectors should be visible!');
      console.log('   If still not visible, check:');
      console.log('   1. Z-index conflicts with other elements');
      console.log('   2. Opacity set to 0 in CSS');
      console.log('   3. Position values causing sectors to be off-screen');
    }

    // Take final screenshot
    await page.screenshot({
      path: 'diagnostic-03-final-state.png',
      fullPage: true
    });
    console.log('\n✓ Final screenshot saved: diagnostic-03-final-state.png');

    console.log('\n==============================================');
    console.log('DIAGNOSTIC COMPLETE');
    console.log('==============================================\n');

    // Assert that we have gathered diagnostic data
    expect(diagnosticResults).toBeDefined();
  });
});
