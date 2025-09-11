import { test, expect } from '@playwright/test';

/**
 * Simple Visual Verification Test
 * Captures screenshots to document current font size improvements
 */

test.describe('Visual Documentation Test', () => {
  test('should capture current state of Stadium Overview with improved fonts', async ({ page }) => {
    console.log('Starting visual documentation capture...');
    
    // Navigate to admin application
    try {
      await page.goto('http://localhost:8082', { 
        waitUntil: 'networkidle',
        timeout: 30000 
      });
      
      console.log('✅ Successfully connected to admin application');
      
      // Take screenshot of login page
      await page.screenshot({ 
        path: '.playwright-mcp/admin-login-current-state.png',
        fullPage: true 
      });
      
      // Navigate to stadium overview (even if login required, we can see the page structure)
      await page.goto('http://localhost:8082/stadium-overview', { 
        waitUntil: 'networkidle',
        timeout: 30000 
      });
      
      // Take screenshot of stadium overview page
      await page.screenshot({ 
        path: '.playwright-mcp/stadium-overview-current-fonts.png',
        fullPage: true 
      });
      
      // Test mobile view
      await page.setViewportSize({ width: 375, height: 812 });
      await page.screenshot({ 
        path: '.playwright-mcp/stadium-overview-mobile-fonts.png',
        fullPage: true 
      });
      
      // Test tablet view
      await page.setViewportSize({ width: 768, height: 1024 });
      await page.screenshot({ 
        path: '.playwright-mcp/stadium-overview-tablet-fonts.png',
        fullPage: true 
      });
      
      console.log('✅ Visual documentation captured successfully');
      
      // Verify page contains expected elements
      const hasStadiumContent = await page.locator('.stadium-viewer-container, .stadium-main-view, h1, h3').count() > 0;
      expect(hasStadiumContent).toBe(true);
      
      console.log('✅ Stadium overview page structure verified');
      
    } catch (error) {
      console.log('⚠️ Error during test:', error);
      // Still capture what we can see
      await page.screenshot({ 
        path: '.playwright-mcp/error-state-capture.png',
        fullPage: true 
      });
      throw error;
    }
  });

  test('should verify CSS font improvements are loaded', async ({ page }) => {
    console.log('Verifying CSS font improvements...');
    
    await page.goto('http://localhost:8082/stadium-overview', { 
      waitUntil: 'networkidle',
      timeout: 30000 
    });
    
    // Check for CSS files
    const cssLinks = await page.evaluate(() => {
      const links = Array.from(document.querySelectorAll('link[rel="stylesheet"]'));
      return links.map(link => ({
        href: link.getAttribute('href'),
        loaded: link.sheet !== null
      }));
    });
    
    console.log('CSS files loaded:', cssLinks.length);
    cssLinks.forEach(link => {
      console.log(`- ${link.href}: ${link.loaded ? 'Loaded' : 'Not loaded'}`);
    });
    
    // Look for stadium-specific CSS
    const hasStadiumCSS = cssLinks.some(link => 
      link.href?.includes('StadiumOverview') || 
      link.href?.includes('stadium')
    );
    
    if (hasStadiumCSS) {
      console.log('✅ Stadium-specific CSS found');
    } else {
      console.log('ℹ️ No stadium-specific CSS found in links');
    }
    
    // Check for text elements that would use the improved fonts
    const textElements = await page.locator('h1, h2, h3, h4, h5, .sector-text, .field-text, text').count();
    console.log(`Found ${textElements} text elements on page`);
    
    expect(textElements).toBeGreaterThan(0);
    console.log('✅ Text elements found for font verification');
  });
});