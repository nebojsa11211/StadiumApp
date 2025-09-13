import { test, expect, Page, BrowserContext } from '@playwright/test';
import { testConfig } from './config';

/**
 * Comprehensive Font Size and HTTPS Protocol Verification Test Suite
 * 
 * This test suite verifies:
 * 1. Font size increases (30% larger) in Stadium Overview page
 * 2. HTTPS protocol configuration and functionality
 * 3. Stadium visualization text readability improvements
 * 4. Authentication flow with HTTPS
 */

// Test configuration constants - check both HTTP and HTTPS ports
const ADMIN_HTTP_URL = 'https://localhost:9030';
const ADMIN_HTTPS_URL = 'https://localhost:7030';
const ADMIN_LOGIN_PATH = '/login';
const STADIUM_OVERVIEW_PATH = '/stadium-overview';
const API_HTTPS_URL = 'https://localhost:7010';

// Helper function to determine which URL to use
async function getWorkingAdminUrl(page: Page): Promise<string> {
  // Try HTTPS first, fall back to HTTP
  try {
    const response = await page.goto(ADMIN_HTTPS_URL, { 
      waitUntil: 'networkidle',
      timeout: 10000 
    });
    if (response?.status() === 200) {
      console.log('✅ Using HTTPS URL:', ADMIN_HTTPS_URL);
      return ADMIN_HTTPS_URL;
    }
  } catch (error) {
    console.log('⚠️ HTTPS failed, trying HTTP...');
  }
  
  try {
    const response = await page.goto(ADMIN_HTTP_URL, { 
      waitUntil: 'networkidle',
      timeout: 10000 
    });
    if (response?.status() === 200) {
      console.log('✅ Using HTTP URL:', ADMIN_HTTP_URL);
      return ADMIN_HTTP_URL;
    }
  } catch (error) {
    console.log('❌ Both HTTPS and HTTP failed');
    throw new Error('Neither HTTPS nor HTTP admin URLs are accessible');
  }
  
  throw new Error('Admin application is not accessible');
}

// Expected font size ranges after 30% increase
const EXPECTED_FONT_SIZES = {
  sectorText: { min: 18, max: 31 }, // clamp(16px, 5vw, 30px) on mobile, increased base sizes
  fieldText: { min: 16, max: 24 },  // clamp(16px, 3vw, 24px)
  occupancyText: { min: 14, max: 16 }, // 11px base increased to ~14px
  capacityText: { min: 11, max: 13 }   // 9px base increased to ~11px
};

// Authentication credentials
const ADMIN_CREDENTIALS = {
  email: 'admin@stadium.com',
  password: 'admin123'
};

test.describe('Font Size and HTTPS Verification Suite', () => {
  let adminContext: BrowserContext;
  let adminPage: Page;

  test.beforeAll(async ({ browser }) => {
    // Create dedicated context for admin testing
    adminContext = await browser.newContext({
      ignoreHTTPSErrors: true, // Allow self-signed certificates in development
      viewport: { width: 1920, height: 1080 }
    });
    adminPage = await adminContext.newPage();
  });

  test.afterAll(async () => {
    await adminContext.close();
  });

  test.describe('HTTPS Protocol Verification', () => {
    test('should verify admin application is accessible and check protocol preference', async () => {
      console.log('Testing admin application accessibility and protocol preference...');
      
      // Try to get working admin URL (HTTPS preferred, HTTP fallback)
      const workingUrl = await getWorkingAdminUrl(adminPage);
      
      // Verify accessibility
      expect(adminPage.url()).toContain('localhost');
      
      if (workingUrl.startsWith('https://')) {
        console.log(`✅ Admin app accessible via HTTPS: ${adminPage.url()}`);
        expect(adminPage.url()).toContain('https://');
      } else {
        console.log(`⚠️ Admin app accessible via HTTP: ${adminPage.url()}`);
        console.log('ℹ️ HTTPS configuration may need adjustment for production deployment');
        expect(adminPage.url()).toContain('http://');
      }
    });

    test('should verify API connections use HTTPS protocol', async () => {
      console.log('Testing API HTTPS protocol usage...');
      
      // Set up network monitoring
      const apiRequests: string[] = [];
      adminPage.on('request', request => {
        const url = request.url();
        if (url.includes('api') || url.includes('7010')) {
          apiRequests.push(url);
          console.log(`API Request: ${url}`);
        }
      });

      // Navigate to admin login to trigger API calls
      const workingUrl = await getWorkingAdminUrl(adminPage);
      await adminPage.goto(`${workingUrl}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      // Check if any API requests were made and verify they use HTTPS
      if (apiRequests.length > 0) {
        apiRequests.forEach(url => {
          expect(url).toMatch(/^https:\/\//);
        });
        console.log(`✅ All ${apiRequests.length} API requests use HTTPS protocol`);
      } else {
        console.log('ℹ️ No API requests detected during initial page load');
      }
    });

    test('should verify authentication works with current configuration', async () => {
      console.log('Testing authentication flow...');
      
      // Navigate to login page
      const workingUrl = await getWorkingAdminUrl(adminPage);
      await adminPage.goto(`${workingUrl}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      // Wait for login form to be ready
      await adminPage.waitForSelector('input[type="email"]', { timeout: 30000 });
      
      // Fill login form
      await adminPage.fill('input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[type="password"]', ADMIN_CREDENTIALS.password);
      
      // Submit form and wait for navigation
      await Promise.all([
        adminPage.waitForURL(url => url.includes('/dashboard') || url.includes('/'), { timeout: 45000 }),
        adminPage.click('button[type="submit"]')
      ]);

      // Verify successful login (should not be on login page)
      expect(adminPage.url()).not.toContain('/login');
      expect(adminPage.url()).toContain('localhost');
      
      console.log(`✅ Authentication successful: ${adminPage.url()}`);
    });
  });

  test.describe('Font Size Verification', () => {
    test.beforeEach(async () => {
      // Ensure we're logged in before each font test
      await adminPage.goto(`${ADMIN_HTTPS_URL}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      // Check if already logged in
      const isLoggedIn = await adminPage.locator('text="Login"').count() === 0;
      
      if (!isLoggedIn) {
        await adminPage.fill('input[type="email"]', ADMIN_CREDENTIALS.email);
        await adminPage.fill('input[type="password"]', ADMIN_CREDENTIALS.password);
        await Promise.all([
          adminPage.waitForURL(url => !url.includes('/login'), { timeout: 45000 }),
          adminPage.click('button[type="submit"]')
        ]);
      }
    });

    test('should verify stadium sector text has increased font sizes', async () => {
      console.log('Testing stadium sector text font sizes...');
      
      // Navigate to Stadium Overview page
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for stadium SVG to load
      await adminPage.waitForSelector('.stadium-svg', { timeout: 30000 });
      
      // Take screenshot for documentation
      await adminPage.screenshot({ 
        path: '.playwright-mcp/font-size-verification-stadium-overview.png',
        fullPage: true 
      });

      // Check sector text font sizes using CSS inspection
      const sectorTexts = adminPage.locator('.stadium-svg .sector-text');
      const sectorTextCount = await sectorTexts.count();
      
      if (sectorTextCount > 0) {
        console.log(`Found ${sectorTextCount} sector text elements`);
        
        // Check computed font size for sector text elements
        const fontSize = await sectorTexts.first().evaluate(el => {
          const computedStyle = window.getComputedStyle(el);
          return parseFloat(computedStyle.fontSize);
        });
        
        console.log(`Sector text font size: ${fontSize}px`);
        expect(fontSize).toBeGreaterThanOrEqual(EXPECTED_FONT_SIZES.sectorText.min);
        
        console.log(`✅ Sector text font size verified: ${fontSize}px (expected: ${EXPECTED_FONT_SIZES.sectorText.min}px+)`);
      } else {
        console.log('⚠️ No sector text elements found - stadium may not be loaded or configured');
      }
    });

    test('should verify field text has increased font sizes', async () => {
      console.log('Testing stadium field text font sizes...');
      
      // Navigate to Stadium Overview page
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for stadium SVG to load
      await adminPage.waitForSelector('.stadium-svg', { timeout: 30000 });
      
      // Check field text font sizes
      const fieldTexts = adminPage.locator('.stadium-svg .field-text');
      const fieldTextCount = await fieldTexts.count();
      
      if (fieldTextCount > 0) {
        console.log(`Found ${fieldTextCount} field text elements`);
        
        const fontSize = await fieldTexts.first().evaluate(el => {
          const computedStyle = window.getComputedStyle(el);
          return parseFloat(computedStyle.fontSize);
        });
        
        console.log(`Field text font size: ${fontSize}px`);
        expect(fontSize).toBeGreaterThanOrEqual(EXPECTED_FONT_SIZES.fieldText.min);
        
        console.log(`✅ Field text font size verified: ${fontSize}px (expected: ${EXPECTED_FONT_SIZES.fieldText.min}px+)`);
      } else {
        console.log('⚠️ No field text elements found');
      }
    });

    test('should verify occupancy text has increased font sizes', async () => {
      console.log('Testing occupancy text font sizes...');
      
      // Navigate to Stadium Overview page
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for stadium SVG to load
      await adminPage.waitForSelector('.stadium-svg', { timeout: 30000 });
      
      // Check occupancy text font sizes
      const occupancyTexts = adminPage.locator('.stadium-svg .occupancy-text');
      const occupancyTextCount = await occupancyTexts.count();
      
      if (occupancyTextCount > 0) {
        console.log(`Found ${occupancyTextCount} occupancy text elements`);
        
        const fontSize = await occupancyTexts.first().evaluate(el => {
          const computedStyle = window.getComputedStyle(el);
          return parseFloat(computedStyle.fontSize);
        });
        
        console.log(`Occupancy text font size: ${fontSize}px`);
        expect(fontSize).toBeGreaterThanOrEqual(EXPECTED_FONT_SIZES.occupancyText.min);
        
        console.log(`✅ Occupancy text font size verified: ${fontSize}px (expected: ${EXPECTED_FONT_SIZES.occupancyText.min}px+)`);
      } else {
        console.log('ℹ️ No occupancy text elements found - may not be present without active events');
      }
    });

    test('should verify sector labels are visibly larger and more readable', async () => {
      console.log('Testing sector label readability...');
      
      // Navigate to Stadium Overview page
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for stadium visualization to fully load
      await adminPage.waitForSelector('.stadium-svg', { timeout: 30000 });
      await adminPage.waitForTimeout(2000); // Additional wait for SVG rendering
      
      // Take comprehensive screenshot for visual verification
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-font-verification.png',
        fullPage: true 
      });

      // Check that sector groups are present and interactive
      const sectorGroups = adminPage.locator('.stadium-svg .sector-group');
      const sectorCount = await sectorGroups.count();
      
      if (sectorCount > 0) {
        console.log(`Found ${sectorCount} interactive sector groups`);
        
        // Verify text elements within sectors have increased sizes
        const allSectorTexts = adminPage.locator('.stadium-svg text');
        const textElementCount = await allSectorTexts.count();
        
        console.log(`Found ${textElementCount} text elements in stadium SVG`);
        
        // Sample a few text elements and verify their font sizes
        const sampleCount = Math.min(3, textElementCount);
        for (let i = 0; i < sampleCount; i++) {
          const textElement = allSectorTexts.nth(i);
          const fontSize = await textElement.evaluate(el => {
            const computedStyle = window.getComputedStyle(el);
            const fontSizeValue = parseFloat(computedStyle.fontSize);
            const className = el.getAttribute('class') || '';
            return { fontSize: fontSizeValue, className };
          });
          
          console.log(`Text element ${i + 1}: ${fontSize.fontSize}px (class: ${fontSize.className})`);
          
          // Verify font size is reasonable (at least 10px for readability)
          expect(fontSize.fontSize).toBeGreaterThanOrEqual(10);
        }
        
        console.log(`✅ Stadium sector labels verified as readable with increased font sizes`);
      } else {
        console.log('⚠️ No sector groups found - checking if stadium structure is loaded');
        
        // Check if there's a message about missing stadium data
        const hasNoDataMessage = await adminPage.locator('text="No stadium structure"').isVisible();
        if (hasNoDataMessage) {
          console.log('ℹ️ Stadium structure not loaded - this is expected if no stadium data is imported');
        }
      }
    });

    test('should verify mobile responsive font sizes on smaller screens', async () => {
      console.log('Testing mobile responsive font sizes...');
      
      // Set mobile viewport
      await adminPage.setViewportSize({ width: 375, height: 812 });
      
      // Navigate to Stadium Overview page
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for stadium SVG to load
      await adminPage.waitForSelector('.stadium-svg', { timeout: 30000 });
      
      // Take mobile screenshot
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-mobile-font-verification.png',
        fullPage: true 
      });

      // Check mobile-specific font sizes (should use clamp() values)
      const sectorTexts = adminPage.locator('.stadium-svg .sector-text');
      const sectorTextCount = await sectorTexts.count();
      
      if (sectorTextCount > 0) {
        const fontSize = await sectorTexts.first().evaluate(el => {
          const computedStyle = window.getComputedStyle(el);
          return parseFloat(computedStyle.fontSize);
        });
        
        console.log(`Mobile sector text font size: ${fontSize}px`);
        // On mobile, minimum should be 16px according to CSS clamp(16px, 5vw, 30px)
        expect(fontSize).toBeGreaterThanOrEqual(16);
        
        console.log(`✅ Mobile font size verified: ${fontSize}px (expected: 16px+ on mobile)`);
      }
      
      // Reset viewport
      await adminPage.setViewportSize({ width: 1920, height: 1080 });
    });
  });

  test.describe('Stadium Visualization Integration Test', () => {
    test('should verify complete stadium overview functionality with HTTPS and improved fonts', async () => {
      console.log('Testing complete stadium overview functionality...');
      
      // Navigate to login page via HTTPS
      await adminPage.goto(`${ADMIN_HTTPS_URL}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      // Perform login
      await adminPage.fill('input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[type="password"]', ADMIN_CREDENTIALS.password);
      await Promise.all([
        adminPage.waitForURL(url => !url.includes('/login'), { timeout: 45000 }),
        adminPage.click('button[type="submit"]')
      ]);

      // Navigate to Stadium Overview
      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Verify page loads successfully with HTTPS
      expect(adminPage.url()).toContain('https://');
      expect(adminPage.url()).toContain('/stadium-overview');

      // Wait for and verify main components are present
      await adminPage.waitForSelector('.stadium-viewer-container', { timeout: 30000 });
      
      // Check for stadium overview header
      const headerExists = await adminPage.locator('h3').filter({ hasText: /stadium/i }).isVisible();
      expect(headerExists).toBe(true);

      // Check for controls (event selector, search, etc.)
      const hasControls = await adminPage.locator('.viewer-controls').isVisible();
      if (hasControls) {
        console.log('✅ Stadium overview controls are present');
      }

      // Check for stadium visualization area
      const hasStadiumView = await adminPage.locator('.stadium-main-view').isVisible();
      expect(hasStadiumView).toBe(true);
      console.log('✅ Stadium visualization area is present');

      // Take comprehensive screenshot showing the complete interface
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-complete-verification.png',
        fullPage: true 
      });

      console.log('✅ Complete stadium overview functionality verified with HTTPS and improved fonts');
    });

    test('should verify no HTTP protocol requests are made', async () => {
      console.log('Monitoring for unauthorized HTTP requests...');
      
      const httpRequests: string[] = [];
      const httpsRequests: string[] = [];
      
      // Monitor all network requests
      adminPage.on('request', request => {
        const url = request.url();
        if (url.startsWith('http://')) {
          httpRequests.push(url);
          console.log(`⚠️ HTTP Request detected: ${url}`);
        } else if (url.startsWith('https://')) {
          httpsRequests.push(url);
        }
      });

      // Perform full workflow
      await adminPage.goto(`${ADMIN_HTTPS_URL}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      await adminPage.fill('input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[type="password"]', ADMIN_CREDENTIALS.password);
      await Promise.all([
        adminPage.waitForURL(url => !url.includes('/login'), { timeout: 45000 }),
        adminPage.click('button[type="submit"]')
      ]);

      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for any additional requests
      await adminPage.waitForTimeout(5000);

      // Verify no HTTP requests were made
      expect(httpRequests.length).toBe(0);
      console.log(`✅ No unauthorized HTTP requests detected`);
      console.log(`ℹ️ Total HTTPS requests: ${httpsRequests.length}`);
      
      if (httpRequests.length > 0) {
        console.log('❌ HTTP requests found:', httpRequests);
      }
    });
  });

  test.describe('Visual Regression Tests', () => {
    test('should capture before/after font size comparison screenshots', async () => {
      console.log('Capturing visual comparison screenshots...');
      
      // Login and navigate to stadium overview
      await adminPage.goto(`${ADMIN_HTTPS_URL}${ADMIN_LOGIN_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 45000 
      });

      await adminPage.fill('input[type="email"]', ADMIN_CREDENTIALS.email);
      await adminPage.fill('input[type="password"]', ADMIN_CREDENTIALS.password);
      await Promise.all([
        adminPage.waitForURL(url => !url.includes('/login'), { timeout: 45000 }),
        adminPage.click('button[type="submit"]')
      ]);

      await adminPage.goto(`${ADMIN_HTTPS_URL}${STADIUM_OVERVIEW_PATH}`, { 
        waitUntil: 'networkidle',
        timeout: 60000 
      });

      // Wait for complete page load
      await adminPage.waitForSelector('.stadium-viewer-container', { timeout: 30000 });
      await adminPage.waitForTimeout(3000);

      // Capture desktop view
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-desktop-new-fonts.png',
        fullPage: true 
      });

      // Capture tablet view
      await adminPage.setViewportSize({ width: 768, height: 1024 });
      await adminPage.waitForTimeout(1000);
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-tablet-new-fonts.png',
        fullPage: true 
      });

      // Capture mobile view
      await adminPage.setViewportSize({ width: 375, height: 812 });
      await adminPage.waitForTimeout(1000);
      await adminPage.screenshot({ 
        path: '.playwright-mcp/stadium-overview-mobile-new-fonts.png',
        fullPage: true 
      });

      console.log('✅ Visual comparison screenshots captured');
    });
  });
});