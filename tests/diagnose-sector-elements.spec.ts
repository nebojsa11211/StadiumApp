import { test } from '@playwright/test';

test('diagnose sector element selectors', async ({ page }) => {
  console.log('\n=== DIAGNOSING SECTOR ELEMENTS ===\n');

  // Login
  await page.goto('https://localhost:7030/login');
  await page.fill('#admin-login-email-input', 'admin@stadium.com');
  await page.fill('#admin-login-password-input', 'admin123');
  await page.click('#admin-login-submit-btn');
  await page.waitForURL(/\/$|\/admin/, { timeout: 10000 });

  // DRAWING TOOL
  console.log('=== DRAWING TOOL PAGE ===');
  await page.goto('https://localhost:7030/admin/stadium-drawing-tool');
  await page.waitForTimeout(2000);

  const drawingToolInfo = await page.evaluate(() => {
    const info: any = {
      canvasElements: [],
      svgElements: [],
      polygonElements: [],
      textElements: [],
      allDivs: []
    };

    // Check for canvas
    const canvases = document.querySelectorAll('canvas');
    info.canvasElements = Array.from(canvases).map(c => ({
      id: c.id,
      className: c.className,
      width: c.width,
      height: c.height
    }));

    // Check for SVG elements
    const svgs = document.querySelectorAll('svg');
    info.svgElements = Array.from(svgs).map(s => ({
      id: s.id,
      className: s.className.baseVal || s.className
    }));

    // Check for polygon/path elements in SVG
    const polygons = document.querySelectorAll('polygon, path[data-sector]');
    info.polygonElements = Array.from(polygons).map(p => ({
      tagName: p.tagName,
      id: p.id,
      className: (p as any).className.baseVal || (p as any).className,
      dataAttrs: Array.from(p.attributes)
        .filter(a => a.name.startsWith('data-'))
        .map(a => ({ name: a.name, value: a.value }))
    }));

    // Check for text elements with sector codes
    const texts = document.querySelectorAll('text, tspan, .sector-label');
    info.textElements = Array.from(texts).slice(0, 5).map(t => ({
      tagName: t.tagName,
      text: t.textContent?.trim(),
      className: (t as any).className.baseVal || (t as any).className
    }));

    // Check all divs with sector-related classes
    const divs = document.querySelectorAll('div[class*="sector"], div[data-sector]');
    info.allDivs = Array.from(divs).slice(0, 5).map(d => ({
      className: d.className,
      dataAttrs: Array.from(d.attributes)
        .filter(a => a.name.startsWith('data-'))
        .map(a => ({ name: a.name, value: a.value }))
    }));

    return info;
  });

  console.log('Drawing Tool Canvas:', JSON.stringify(drawingToolInfo.canvasElements, null, 2));
  console.log('Drawing Tool SVG:', JSON.stringify(drawingToolInfo.svgElements, null, 2));
  console.log('Drawing Tool Polygons:', JSON.stringify(drawingToolInfo.polygonElements, null, 2));
  console.log('Drawing Tool Text:', JSON.stringify(drawingToolInfo.textElements, null, 2));
  console.log('Drawing Tool Divs:', JSON.stringify(drawingToolInfo.allDivs, null, 2));

  // STADIUM OVERVIEW
  console.log('\n=== STADIUM OVERVIEW PAGE ===');
  await page.goto('https://localhost:7030/admin/stadium-overview');
  await page.waitForTimeout(3000);

  const overviewInfo = await page.evaluate(() => {
    const info: any = {
      sectorOverlays: [],
      sectorLabels: [],
      allSectorDivs: []
    };

    // Check for sector overlay divs
    const overlays = document.querySelectorAll('div[class*="sector"], div[data-sector], .sector-overlay');
    info.sectorOverlays = Array.from(overlays).slice(0, 5).map(o => ({
      className: o.className,
      id: o.id,
      style: (o as HTMLElement).style.cssText,
      dataAttrs: Array.from(o.attributes)
        .filter(a => a.name.startsWith('data-'))
        .map(a => ({ name: a.name, value: a.value })),
      text: o.textContent?.trim().substring(0, 50)
    }));

    // Check for sector labels
    const labels = document.querySelectorAll('.sector-code, .sector-label, [class*="label"]');
    info.sectorLabels = Array.from(labels).slice(0, 5).map(l => ({
      className: l.className,
      text: l.textContent?.trim(),
      parentClass: l.parentElement?.className
    }));

    // All divs with positioning
    const positioned = document.querySelectorAll('div[style*="position"]');
    info.allSectorDivs = Array.from(positioned).slice(0, 10).map(d => ({
      className: d.className,
      style: (d as HTMLElement).style.cssText.substring(0, 100),
      text: d.textContent?.trim().substring(0, 30)
    }));

    return info;
  });

  console.log('Overview Overlays:', JSON.stringify(overviewInfo.sectorOverlays, null, 2));
  console.log('Overview Labels:', JSON.stringify(overviewInfo.sectorLabels, null, 2));
  console.log('Overview Positioned Divs:', JSON.stringify(overviewInfo.allSectorDivs, null, 2));
});
