import { test, expect } from '@playwright/test';

test.describe('SVG Rendering Diagnosis - Stadium Overview', () => {
  test('diagnose SVG sector overlay visibility issues', async ({ page }) => {
    console.log('\n========================================');
    console.log('SVG RENDERING DIAGNOSTIC TEST');
    console.log('========================================\n');

    // Capture all console messages
    const consoleMessages: string[] = [];
    const consoleErrors: string[] = [];

    page.on('console', msg => {
      const text = `[${msg.type()}] ${msg.text()}`;
      consoleMessages.push(text);
      if (msg.type() === 'error' || msg.type() === 'warning') {
        consoleErrors.push(text);
        console.log(`🔴 Console ${msg.type()}: ${msg.text()}`);
      }
    });

    // Capture page errors
    page.on('pageerror', error => {
      const errorText = `Page Error: ${error.message}`;
      consoleErrors.push(errorText);
      console.log(`🔴 ${errorText}`);
    });

    try {
      // Step 1: Login as admin
      console.log('Step 1: Logging in as admin...');
      await page.goto('https://localhost:7030/login');
      await page.waitForLoadState('networkidle');

      await page.fill('#admin-login-email-input', 'admin@stadium.com');
      await page.fill('#admin-login-password-input', 'admin123');
      await page.click('#admin-login-submit-btn');

      // Wait for navigation after login
      await page.waitForURL('**/admin/**', { timeout: 10000 });
      console.log('✅ Login successful\n');

      // Step 2: Navigate to Stadium Overview
      console.log('Step 2: Navigating to Stadium Overview...');
      await page.goto('https://localhost:7030/admin/stadium-overview');
      await page.waitForLoadState('networkidle');
      console.log('✅ Navigation complete\n');

      // Step 3: Wait 5 seconds for full load
      console.log('Step 3: Waiting 5 seconds for full page load...');
      await page.waitForTimeout(5000);
      console.log('✅ Wait complete\n');

      // ==========================================
      // DOM INSPECTION: SVG Overlay
      // ==========================================
      console.log('========================================');
      console.log('DOM INSPECTION: SVG OVERLAY');
      console.log('========================================\n');

      const svgOverlay = page.locator('#admin-stadium-overview-svg-overlay');
      const svgExists = await svgOverlay.count() > 0;
      const svgVisible = svgExists ? await svgOverlay.isVisible() : false;

      console.log('SVG Overlay Element:');
      console.log(`  Exists: ${svgExists}`);
      console.log(`  Visible: ${svgVisible}`);

      if (svgExists) {
        const svgStyle = await svgOverlay.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            display: style.display,
            opacity: style.opacity,
            zIndex: style.zIndex,
            position: style.position,
            width: style.width,
            height: style.height,
            top: style.top,
            left: style.left,
            visibility: style.visibility,
            transform: style.transform,
            pointerEvents: style.pointerEvents
          };
        });

        console.log(`  Display: ${svgStyle.display}`);
        console.log(`  Opacity: ${svgStyle.opacity}`);
        console.log(`  Z-Index: ${svgStyle.zIndex}`);
        console.log(`  Position: ${svgStyle.position}`);
        console.log(`  Width: ${svgStyle.width}`);
        console.log(`  Height: ${svgStyle.height}`);
        console.log(`  Top: ${svgStyle.top}`);
        console.log(`  Left: ${svgStyle.left}`);
        console.log(`  Visibility: ${svgStyle.visibility}`);
        console.log(`  Transform: ${svgStyle.transform}`);
        console.log(`  Pointer Events: ${svgStyle.pointerEvents}`);

        // Get SVG attributes
        const svgAttrs = await svgOverlay.evaluate(el => ({
          viewBox: el.getAttribute('viewBox'),
          width: el.getAttribute('width'),
          height: el.getAttribute('height'),
          preserveAspectRatio: el.getAttribute('preserveAspectRatio'),
          innerHTML: el.innerHTML.substring(0, 200)
        }));

        console.log(`\nSVG Attributes:`);
        console.log(`  ViewBox: ${svgAttrs.viewBox}`);
        console.log(`  Width Attr: ${svgAttrs.width}`);
        console.log(`  Height Attr: ${svgAttrs.height}`);
        console.log(`  PreserveAspectRatio: ${svgAttrs.preserveAspectRatio}`);
        console.log(`  Inner HTML Preview: ${svgAttrs.innerHTML}...`);
      } else {
        console.log('  ⚠️ SVG overlay element not found in DOM');
      }

      // ==========================================
      // CHECK SECTOR GROUPS
      // ==========================================
      console.log('\n========================================');
      console.log('SECTOR GROUPS INSPECTION');
      console.log('========================================\n');

      const sectorGroups = await page.locator('.sector-group').all();
      console.log(`Sector Groups Found: ${sectorGroups.length}`);

      if (sectorGroups.length === 0) {
        console.log('⚠️ No sector groups found with class "sector-group"');

        // Check alternative selectors
        const gElements = await page.locator('svg g').all();
        console.log(`  Alternative: Found ${gElements.length} <g> elements in SVG`);

        const pathElements = await page.locator('svg path').all();
        console.log(`  Alternative: Found ${pathElements.length} <path> elements in SVG`);
      } else {
        for (let i = 0; i < Math.min(3, sectorGroups.length); i++) {
          const group = sectorGroups[i];
          const id = await group.getAttribute('id');
          const classAttr = await group.getAttribute('class');
          const visible = await group.isVisible();
          const boundingBox = await group.boundingBox();

          const groupStyle = await group.evaluate(el => {
            const style = window.getComputedStyle(el);
            return {
              display: style.display,
              opacity: style.opacity,
              visibility: style.visibility
            };
          });

          console.log(`\nGroup ${i + 1}:`);
          console.log(`  ID: ${id}`);
          console.log(`  Class: ${classAttr}`);
          console.log(`  Visible: ${visible}`);
          console.log(`  Display: ${groupStyle.display}`);
          console.log(`  Opacity: ${groupStyle.opacity}`);
          console.log(`  Visibility: ${groupStyle.visibility}`);
          console.log(`  BoundingBox: ${JSON.stringify(boundingBox)}`);
        }
      }

      // ==========================================
      // CHECK SVG PATHS
      // ==========================================
      console.log('\n========================================');
      console.log('SVG PATHS INSPECTION');
      console.log('========================================\n');

      const paths = await page.locator('.sector-path').all();
      console.log(`Sector Paths Found (class="sector-path"): ${paths.length}`);

      if (paths.length === 0) {
        console.log('⚠️ No paths found with class "sector-path"');

        // Check all paths in SVG
        const allPaths = await page.locator('svg path').all();
        console.log(`  Alternative: Found ${allPaths.length} total <path> elements in SVG`);

        if (allPaths.length > 0) {
          console.log('\nInspecting first 3 paths (any class):');
          for (let i = 0; i < Math.min(3, allPaths.length); i++) {
            const path = allPaths[i];
            const d = await path.getAttribute('d');
            const classAttr = await path.getAttribute('class');
            const id = await path.getAttribute('id');

            const pathStyle = await path.evaluate(el => {
              const style = window.getComputedStyle(el);
              return {
                fill: style.fill,
                stroke: style.stroke,
                strokeWidth: style.strokeWidth,
                opacity: style.opacity,
                display: style.display
              };
            });

            console.log(`\nPath ${i + 1}:`);
            console.log(`  ID: ${id}`);
            console.log(`  Class: ${classAttr}`);
            console.log(`  d: ${d?.substring(0, 60)}...`);
            console.log(`  fill: ${pathStyle.fill}`);
            console.log(`  stroke: ${pathStyle.stroke}`);
            console.log(`  stroke-width: ${pathStyle.strokeWidth}`);
            console.log(`  opacity: ${pathStyle.opacity}`);
            console.log(`  display: ${pathStyle.display}`);
          }
        }
      } else {
        for (let i = 0; i < Math.min(3, paths.length); i++) {
          const path = paths[i];
          const d = await path.getAttribute('d');
          const id = await path.getAttribute('id');

          const pathStyle = await path.evaluate(el => {
            const style = window.getComputedStyle(el);
            return {
              fill: style.fill,
              stroke: style.stroke,
              strokeWidth: style.strokeWidth,
              opacity: style.opacity,
              display: style.display
            };
          });

          console.log(`\nPath ${i + 1}:`);
          console.log(`  ID: ${id}`);
          console.log(`  d: ${d?.substring(0, 60)}...`);
          console.log(`  fill: ${pathStyle.fill}`);
          console.log(`  stroke: ${pathStyle.stroke}`);
          console.log(`  stroke-width: ${pathStyle.strokeWidth}`);
          console.log(`  opacity: ${pathStyle.opacity}`);
          console.log(`  display: ${pathStyle.display}`);
        }
      }

      // ==========================================
      // CHECK PARENT CONTAINERS
      // ==========================================
      console.log('\n========================================');
      console.log('PARENT CONTAINERS INSPECTION');
      console.log('========================================\n');

      // Check stadium image container
      const stadiumImageContainer = page.locator('#admin-stadium-overview-stadium-image-container');
      const imageContainerExists = await stadiumImageContainer.count() > 0;

      console.log('Stadium Image Container:');
      console.log(`  Exists: ${imageContainerExists}`);

      if (imageContainerExists) {
        const containerStyle = await stadiumImageContainer.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            position: style.position,
            width: style.width,
            height: style.height,
            overflow: style.overflow,
            display: style.display
          };
        });

        console.log(`  Position: ${containerStyle.position}`);
        console.log(`  Width: ${containerStyle.width}`);
        console.log(`  Height: ${containerStyle.height}`);
        console.log(`  Overflow: ${containerStyle.overflow}`);
        console.log(`  Display: ${containerStyle.display}`);
      }

      // Check stadium image
      const stadiumImage = page.locator('#admin-stadium-overview-stadium-image');
      const imageExists = await stadiumImage.count() > 0;

      console.log('\nStadium Image:');
      console.log(`  Exists: ${imageExists}`);

      if (imageExists) {
        const imageAttrs = await stadiumImage.evaluate(el => ({
          src: (el as HTMLImageElement).src,
          naturalWidth: (el as HTMLImageElement).naturalWidth,
          naturalHeight: (el as HTMLImageElement).naturalHeight,
          complete: (el as HTMLImageElement).complete
        }));

        const imageStyle = await stadiumImage.evaluate(el => {
          const style = window.getComputedStyle(el);
          return {
            width: style.width,
            height: style.height,
            display: style.display,
            position: style.position
          };
        });

        console.log(`  Src: ${imageAttrs.src}`);
        console.log(`  Natural Width: ${imageAttrs.naturalWidth}`);
        console.log(`  Natural Height: ${imageAttrs.naturalHeight}`);
        console.log(`  Loaded: ${imageAttrs.complete}`);
        console.log(`  Computed Width: ${imageStyle.width}`);
        console.log(`  Computed Height: ${imageStyle.height}`);
        console.log(`  Display: ${imageStyle.display}`);
        console.log(`  Position: ${imageStyle.position}`);
      }

      // ==========================================
      // CONSOLE ERRORS SUMMARY
      // ==========================================
      console.log('\n========================================');
      console.log('CONSOLE ERRORS & WARNINGS');
      console.log('========================================\n');

      if (consoleErrors.length === 0) {
        console.log('✅ No console errors or warnings detected');
      } else {
        console.log(`⚠️ Found ${consoleErrors.length} console errors/warnings:\n`);
        consoleErrors.forEach((error, idx) => {
          console.log(`${idx + 1}. ${error}`);
        });
      }

      // ==========================================
      // SCREENSHOT
      // ==========================================
      console.log('\n========================================');
      console.log('TAKING SCREENSHOT');
      console.log('========================================\n');

      await page.screenshot({
        path: 'svg-debug-screenshot.png',
        fullPage: true
      });
      console.log('✅ Screenshot saved: svg-debug-screenshot.png');

      // ==========================================
      // FINAL SUMMARY
      // ==========================================
      console.log('\n========================================');
      console.log('DIAGNOSTIC SUMMARY');
      console.log('========================================\n');

      console.log('Key Findings:');
      console.log(`  ✓ SVG Overlay Exists: ${svgExists}`);
      console.log(`  ✓ SVG Overlay Visible: ${svgVisible}`);
      console.log(`  ✓ Sector Groups Found: ${sectorGroups.length}`);
      console.log(`  ✓ Sector Paths Found: ${paths.length}`);
      console.log(`  ✓ Console Errors: ${consoleErrors.length}`);

      console.log('\n========================================');
      console.log('TEST COMPLETE');
      console.log('========================================\n');

    } catch (error) {
      console.error('\n🔴 TEST FAILED WITH ERROR:');
      console.error(error);

      // Take error screenshot
      await page.screenshot({
        path: 'svg-debug-error-screenshot.png',
        fullPage: true
      });
      console.log('📸 Error screenshot saved: svg-debug-error-screenshot.png');

      throw error;
    }
  });
});
