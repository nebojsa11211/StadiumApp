import { test, expect } from '@playwright/test';

test.describe('Compare Stadium Pages Sector Display', () => {
  test.setTimeout(90000);

  async function loginAsAdmin(page) {
    await page.goto('https://localhost:7030/admin/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForTimeout(3000);
  }

  test('Compare sectors between Drawing Tool and Stadium Overview', async ({ page }) => {
    console.log('\n==============================================');
    console.log('STADIUM PAGES SECTOR COMPARISON');
    console.log('==============================================\n');

    await loginAsAdmin(page);

    // ==================== DRAWING TOOL ====================
    console.log('📐 Testing Drawing Tool...');
    await page.goto('https://localhost:7030/admin/stadium-drawing-tool', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Check canvas dimensions
    const canvasInfo = await page.locator('#admin-drawing-tool-canvas').evaluate((canvas: HTMLCanvasElement) => {
      return {
        width: canvas.width,
        height: canvas.height,
        displayWidth: canvas.clientWidth,
        displayHeight: canvas.clientHeight
      };
    });
    console.log('\n🎨 Drawing Tool Canvas Info:');
    console.log(`  Canvas Internal Size: ${canvasInfo.width}x${canvasInfo.height}px`);
    console.log(`  Canvas Display Size: ${canvasInfo.displayWidth}x${canvasInfo.displayHeight}px`);

    // Take screenshot of Drawing Tool
    await page.screenshot({
      path: 'drawing-tool-sectors.png',
      fullPage: true
    });
    console.log('  Screenshot saved: drawing-tool-sectors.png');

    // ==================== STADIUM OVERVIEW ====================
    console.log('\n🏟️ Testing Stadium Overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview', { waitUntil: 'networkidle' });
    await page.waitForTimeout(3000);

    // Check sector count
    const sectorCount = await page.locator('.sector-overlay').count();
    console.log(`\n✓ Sector overlays found: ${sectorCount}`);

    // Get blueprint image dimensions
    const imageInfo = await page.locator('#admin-stadium-overview-blueprint-img').evaluate((img: HTMLImageElement) => {
      return {
        naturalWidth: img.naturalWidth,
        naturalHeight: img.naturalHeight,
        displayWidth: img.clientWidth,
        displayHeight: img.clientHeight
      };
    });
    console.log('\n🖼️ Stadium Overview Image Info:');
    console.log(`  Image Natural Size: ${imageInfo.naturalWidth}x${imageInfo.naturalHeight}px`);
    console.log(`  Image Display Size: ${imageInfo.displayWidth}x${imageInfo.displayHeight}px`);

    // Get first 5 sectors' details from both pages
    if (sectorCount > 0) {
      console.log('\n📊 First 5 Sectors Comparison:');

      for (let i = 0; i < Math.min(5, sectorCount); i++) {
        const sector = page.locator('.sector-overlay').nth(i);
        const sectorDetails = await sector.evaluate((el: HTMLElement) => {
          const style = window.getComputedStyle(el);
          const inlineStyle = el.getAttribute('style') || '';

          // Extract percentages from inline style
          const topMatch = inlineStyle.match(/top:\s*([\d.]+)%/);
          const leftMatch = inlineStyle.match(/left:\s*([\d.]+)%/);
          const widthMatch = inlineStyle.match(/width:\s*([\d.]+)%/);
          const heightMatch = inlineStyle.match(/height:\s*([\d.]+)%/);

          return {
            sectorCode: el.querySelector('.sector-label')?.textContent || 'N/A',
            inlineTopPercent: topMatch ? topMatch[1] : 'N/A',
            inlineLeftPercent: leftMatch ? leftMatch[1] : 'N/A',
            inlineWidthPercent: widthMatch ? widthMatch[1] : 'N/A',
            inlineHeightPercent: heightMatch ? heightMatch[1] : 'N/A',
            computedTop: style.top,
            computedLeft: style.left,
            computedWidth: style.width,
            computedHeight: style.height
          };
        });

        console.log(`\n  Sector ${i + 1}: ${sectorDetails.sectorCode}`);
        console.log(`    Inline CSS: top=${sectorDetails.inlineTopPercent}%, left=${sectorDetails.inlineLeftPercent}%, width=${sectorDetails.inlineWidthPercent}%, height=${sectorDetails.inlineHeightPercent}%`);
        console.log(`    Computed:   ${sectorDetails.computedTop} / ${sectorDetails.computedLeft} / ${sectorDetails.computedWidth} x ${sectorDetails.computedHeight}`);
      }
    }

    // Take screenshot of Stadium Overview
    await page.screenshot({
      path: 'stadium-overview-sectors.png',
      fullPage: true
    });
    console.log('\n  Screenshot saved: stadium-overview-sectors.png');

    // ==================== ANALYSIS ====================
    console.log('\n📋 ANALYSIS:');
    console.log('  ═══════════════════════════════════════════════');

    // Check if image aspect ratio matches canvas aspect ratio
    const canvasRatio = canvasInfo.width / canvasInfo.height;
    const imageRatio = imageInfo.naturalWidth / imageInfo.naturalHeight;
    const ratioMatch = Math.abs(canvasRatio - imageRatio) < 0.01;

    console.log(`  Canvas Aspect Ratio: ${canvasRatio.toFixed(4)} (${canvasInfo.width}x${canvasInfo.height})`);
    console.log(`  Image Aspect Ratio:  ${imageRatio.toFixed(4)} (${imageInfo.naturalWidth}x${imageInfo.naturalHeight})`);
    console.log(`  Aspect Ratios Match: ${ratioMatch ? '✅ YES' : '❌ NO - THIS IS THE PROBLEM!'}`);

    if (!ratioMatch) {
      console.log('\n  ⚠️ ROOT CAUSE IDENTIFIED:');
      console.log('  The Drawing Tool canvas (1200x700 = 1.714) has a DIFFERENT');
      console.log('  aspect ratio than the stadium blueprint image.');
      console.log('  This causes sectors to appear in different positions!');
      console.log('\n  🔧 SOLUTION:');
      console.log('  Update the canvas dimensions in StadiumDrawingTool.razor');
      console.log(`  to match the image: width="${imageInfo.naturalWidth}" height="${imageInfo.naturalHeight}"`);
    } else {
      console.log('\n  ✅ Aspect ratios match - sectors should align correctly!');
    }

    console.log('\n==============================================');
    console.log('COMPARISON COMPLETE');
    console.log('==============================================\n');
  });
});
