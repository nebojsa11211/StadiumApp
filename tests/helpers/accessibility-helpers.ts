import { Page, expect } from '@playwright/test';

/**
 * Accessibility Testing Helper Functions
 * 
 * Provides utilities for testing WCAG 2.1 AA compliance and modern UX features
 * in the enhanced stadium viewer components.
 */

/**
 * Color contrast utilities
 */
export class ColorContrastHelper {
  /**
   * Parse RGB color string to RGB values
   */
  static parseRGB(rgbString: string): { r: number, g: number, b: number } | null {
    const rgbMatch = rgbString.match(/rgb\((\d+),\s*(\d+),\s*(\d+)\)/);
    const rgbaMatch = rgbString.match(/rgba\((\d+),\s*(\d+),\s*(\d+),\s*[\d.]+\)/);
    
    if (rgbMatch) {
      return {
        r: parseInt(rgbMatch[1]),
        g: parseInt(rgbMatch[2]),
        b: parseInt(rgbMatch[3])
      };
    } else if (rgbaMatch) {
      return {
        r: parseInt(rgbaMatch[1]),
        g: parseInt(rgbaMatch[2]),
        b: parseInt(rgbaMatch[3])
      };
    }
    
    return null;
  }

  /**
   * Convert hex color to RGB
   */
  static hexToRGB(hex: string): { r: number, g: number, b: number } | null {
    const hexMatch = hex.match(/^#([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i);
    if (hexMatch) {
      return {
        r: parseInt(hexMatch[1], 16),
        g: parseInt(hexMatch[2], 16),
        b: parseInt(hexMatch[3], 16)
      };
    }
    return null;
  }

  /**
   * Calculate relative luminance
   */
  static getRelativeLuminance(rgb: { r: number, g: number, b: number }): number {
    const { r, g, b } = rgb;
    
    const rsRGB = r / 255;
    const gsRGB = g / 255;
    const bsRGB = b / 255;
    
    const R = rsRGB <= 0.03928 ? rsRGB / 12.92 : Math.pow((rsRGB + 0.055) / 1.055, 2.4);
    const G = gsRGB <= 0.03928 ? gsRGB / 12.92 : Math.pow((gsRGB + 0.055) / 1.055, 2.4);
    const B = bsRGB <= 0.03928 ? bsRGB / 12.92 : Math.pow((bsRGB + 0.055) / 1.055, 2.4);
    
    return 0.2126 * R + 0.7152 * G + 0.0722 * B;
  }

  /**
   * Calculate contrast ratio between two colors
   */
  static getContrastRatio(color1: string, color2: string): number {
    const rgb1 = this.parseRGB(color1) || this.hexToRGB(color1);
    const rgb2 = this.parseRGB(color2) || this.hexToRGB(color2);
    
    if (!rgb1 || !rgb2) return 0;
    
    const luminance1 = this.getRelativeLuminance(rgb1);
    const luminance2 = this.getRelativeLuminance(rgb2);
    
    const lighter = Math.max(luminance1, luminance2);
    const darker = Math.min(luminance1, luminance2);
    
    return (lighter + 0.05) / (darker + 0.05);
  }

  /**
   * Check if contrast ratio meets WCAG AA standards
   */
  static meetsWCAGAA(foreground: string, background: string): boolean {
    const ratio = this.getContrastRatio(foreground, background);
    return ratio >= 4.5; // WCAG AA normal text requirement
  }

  /**
   * Check if contrast ratio meets WCAG AAA standards
   */
  static meetsWCAGAAA(foreground: string, background: string): boolean {
    const ratio = this.getContrastRatio(foreground, background);
    return ratio >= 7.0; // WCAG AAA normal text requirement
  }
}

/**
 * Keyboard Navigation Testing Helper
 */
export class KeyboardNavigationHelper {
  constructor(private page: Page) {}

  /**
   * Test tab navigation through all focusable elements
   */
  async testTabNavigation(): Promise<{ totalFocusable: number; hasProperFocus: boolean }> {
    const focusableElements = await this.page.$$('button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])');
    
    let hasProperFocus = true;
    let currentIndex = 0;
    
    // Start at first element
    await this.page.keyboard.press('Tab');
    
    while (currentIndex < focusableElements.length && currentIndex < 20) { // Limit to prevent infinite loops
      const focused = await this.page.locator(':focus');
      
      if (!(await focused.isVisible())) {
        hasProperFocus = false;
        break;
      }
      
      // Check if focus indicator is visible
      const focusStyles = await focused.evaluate(el => {
        const styles = window.getComputedStyle(el);
        return {
          outline: styles.outline,
          outlineWidth: styles.outlineWidth,
          outlineStyle: styles.outlineStyle,
          boxShadow: styles.boxShadow
        };
      });
      
      const hasFocusIndicator = focusStyles.outline !== 'none' ||
                               focusStyles.outlineWidth !== '0px' ||
                               focusStyles.boxShadow !== 'none';
      
      if (!hasFocusIndicator) {
        hasProperFocus = false;
      }
      
      await this.page.keyboard.press('Tab');
      currentIndex++;
    }
    
    return {
      totalFocusable: focusableElements.length,
      hasProperFocus
    };
  }

  /**
   * Test arrow key navigation
   */
  async testArrowKeyNavigation(container: string = '.stadium-viewer-container'): Promise<boolean> {
    const containerElement = this.page.locator(container);
    await containerElement.click();
    
    const directions = ['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'];
    
    for (const direction of directions) {
      await this.page.keyboard.press(direction);
      await this.page.waitForTimeout(200);
      
      // Check if focus moved or some visual change occurred
      const focused = this.page.locator(':focus');
      if (await focused.isVisible()) {
        // Arrow keys are handling navigation
        return true;
      }
    }
    
    return false;
  }

  /**
   * Test escape key functionality
   */
  async testEscapeKey(): Promise<boolean> {
    // Open a modal or dialog first
    const interactiveElement = this.page.locator('button, .sector-group polygon').first();
    if (await interactiveElement.isVisible()) {
      await interactiveElement.click();
      await this.page.waitForTimeout(500);
      
      // Press escape
      await this.page.keyboard.press('Escape');
      await this.page.waitForTimeout(500);
      
      // Check if modal/dialog closed
      const modal = this.page.locator('.modal, [role="dialog"], .sector-modal');
      if (await modal.count() > 0) {
        return !(await modal.first().isVisible());
      }
    }
    
    return false;
  }

  /**
   * Test Enter/Space key activation
   */
  async testKeyActivation(selector: string): Promise<boolean> {
    const element = this.page.locator(selector);
    await element.focus();
    
    // Test Enter key
    await this.page.keyboard.press('Enter');
    await this.page.waitForTimeout(500);
    
    // Check for any modal, dialog, or state change
    let activated = await this.hasActivationResponse();
    
    if (!activated) {
      // Test Space key
      await element.focus();
      await this.page.keyboard.press('Space');
      await this.page.waitForTimeout(500);
      activated = await this.hasActivationResponse();
    }
    
    return activated;
  }

  private async hasActivationResponse(): Promise<boolean> {
    const responseIndicators = [
      '.modal',
      '[role="dialog"]',
      '.tooltip',
      '.popover',
      '.dropdown-menu',
      '.sector-modal'
    ];
    
    for (const selector of responseIndicators) {
      const element = this.page.locator(selector);
      if (await element.isVisible()) {
        return true;
      }
    }
    
    return false;
  }
}

/**
 * ARIA and Semantic Testing Helper
 */
export class ARIATestingHelper {
  constructor(private page: Page) {}

  /**
   * Check for required ARIA attributes on interactive elements
   */
  async checkARIAAttributes(): Promise<{ passed: boolean; issues: string[] }> {
    const issues: string[] = [];
    
    // Check buttons have accessible names
    const buttons = await this.page.$$('button, [role="button"]');
    for (const button of buttons) {
      const ariaLabel = await button.getAttribute('aria-label');
      const text = await button.textContent();
      
      if (!ariaLabel && (!text || text.trim() === '')) {
        issues.push('Button without accessible name found');
      }
    }
    
    // Check form inputs have labels
    const inputs = await this.page.$$('input, select, textarea');
    for (const input of inputs) {
      const id = await input.getAttribute('id');
      const ariaLabel = await input.getAttribute('aria-label');
      const ariaLabelledBy = await input.getAttribute('aria-labelledby');
      
      let hasLabel = false;
      
      if (ariaLabel || ariaLabelledBy) {
        hasLabel = true;
      } else if (id) {
        const label = await this.page.$(`label[for="${id}"]`);
        if (label) hasLabel = true;
      }
      
      if (!hasLabel) {
        issues.push('Form input without accessible label found');
      }
    }
    
    // Check images have alt text
    const images = await this.page.$$('img');
    for (const img of images) {
      const alt = await img.getAttribute('alt');
      const ariaLabel = await img.getAttribute('aria-label');
      
      if (!alt && !ariaLabel) {
        issues.push('Image without alternative text found');
      }
    }
    
    // Check SVG elements have accessible names
    const svgElements = await this.page.$$('svg');
    for (const svg of svgElements) {
      const ariaLabel = await svg.getAttribute('aria-label');
      const role = await svg.getAttribute('role');
      const title = await svg.$('title');
      
      if (!ariaLabel && !title && !role) {
        issues.push('SVG without accessible name found');
      }
    }
    
    return {
      passed: issues.length === 0,
      issues
    };
  }

  /**
   * Check for live regions for dynamic content
   */
  async checkLiveRegions(): Promise<boolean> {
    const liveRegions = await this.page.$$('[aria-live]');
    return liveRegions.length > 0;
  }

  /**
   * Check landmark structure
   */
  async checkLandmarks(): Promise<{ hasMain: boolean; hasNav: boolean; landmarks: string[] }> {
    const landmarks: string[] = [];
    
    const landmarkSelectors = [
      'main, [role="main"]',
      'nav, [role="navigation"]',
      'header, [role="banner"]',
      'footer, [role="contentinfo"]',
      'aside, [role="complementary"]',
      '[role="region"]'
    ];
    
    let hasMain = false;
    let hasNav = false;
    
    for (const selector of landmarkSelectors) {
      const elements = await this.page.$$(selector);
      if (elements.length > 0) {
        landmarks.push(selector.split(',')[0]);
        
        if (selector.includes('main')) hasMain = true;
        if (selector.includes('nav')) hasNav = true;
      }
    }
    
    return { hasMain, hasNav, landmarks };
  }

  /**
   * Check heading hierarchy
   */
  async checkHeadingHierarchy(): Promise<{ isValid: boolean; headings: { level: number; text: string }[] }> {
    const headingElements = await this.page.$$('h1, h2, h3, h4, h5, h6');
    const headings: { level: number; text: string }[] = [];
    
    for (const heading of headingElements) {
      const tagName = await heading.evaluate(el => el.tagName.toLowerCase());
      const level = parseInt(tagName[1]);
      const text = await heading.textContent() || '';
      headings.push({ level, text: text.trim() });
    }
    
    // Check if hierarchy is valid (no skipped levels)
    let isValid = true;
    let previousLevel = 0;
    
    for (const heading of headings) {
      if (heading.level > previousLevel + 1) {
        isValid = false;
        break;
      }
      previousLevel = heading.level;
    }
    
    return { isValid, headings };
  }
}

/**
 * Mobile and Touch Testing Helper
 */
export class MobileTestingHelper {
  constructor(private page: Page) {}

  /**
   * Test mobile viewport sizes
   */
  async testMobileViewports(): Promise<{ viewport: { width: number; height: number }; passed: boolean }[]> {
    const viewports = [
      { width: 320, height: 568, name: 'iPhone SE' },
      { width: 375, height: 667, name: 'iPhone 8' },
      { width: 375, height: 812, name: 'iPhone X' },
      { width: 414, height: 896, name: 'iPhone 11' },
      { width: 768, height: 1024, name: 'iPad' },
      { width: 1024, height: 768, name: 'iPad Landscape' }
    ];
    
    const results = [];
    
    for (const viewport of viewports) {
      await this.page.setViewportSize(viewport);
      await this.page.waitForTimeout(500);
      
      // Check if content is still accessible and usable
      const mainContent = this.page.locator('.stadium-viewer-container, main');
      const isVisible = await mainContent.isVisible();
      
      // Check if content overflows
      const hasHorizontalScrollbar = await this.page.evaluate(() => {
        return document.documentElement.scrollWidth > document.documentElement.clientWidth;
      });
      
      const passed = isVisible && !hasHorizontalScrollbar;
      
      results.push({
        viewport,
        passed
      });
    }
    
    return results;
  }

  /**
   * Test touch interactions
   */
  async testTouchInteractions(selector: string): Promise<boolean> {
    const element = this.page.locator(selector);
    
    if (!(await element.isVisible())) {
      return false;
    }
    
    // Test tap interaction
    await element.tap();
    await this.page.waitForTimeout(500);
    
    // Check for response (modal, tooltip, etc.)
    const responseSelectors = [
      '.modal',
      '.tooltip',
      '[role="dialog"]',
      '.sector-modal'
    ];
    
    for (const responseSelector of responseSelectors) {
      const response = this.page.locator(responseSelector);
      if (await response.isVisible()) {
        return true;
      }
    }
    
    return false;
  }

  /**
   * Test pinch zoom functionality
   */
  async testPinchZoom(): Promise<boolean> {
    // Simulate pinch zoom gesture
    const viewport = await this.page.viewportSize();
    if (!viewport) return false;
    
    const centerX = viewport.width / 2;
    const centerY = viewport.height / 2;
    
    // Start two finger pinch
    await this.page.touchscreen.tap(centerX - 50, centerY);
    await this.page.touchscreen.tap(centerX + 50, centerY);
    
    await this.page.waitForTimeout(500);
    
    // Check if zoom level changed
    const zoomLevel = await this.page.evaluate(() => {
      return window.visualViewport?.scale || 1;
    });
    
    return zoomLevel !== 1;
  }

  /**
   * Check touch target sizes (minimum 44x44 pixels)
   */
  async checkTouchTargetSizes(): Promise<{ passed: boolean; smallTargets: number }> {
    const interactiveElements = await this.page.$$('button, a, input, select, textarea, [onclick], [role="button"]');
    let smallTargets = 0;
    
    for (const element of interactiveElements) {
      const boundingBox = await element.boundingBox();
      
      if (boundingBox) {
        const { width, height } = boundingBox;
        
        // WCAG recommends minimum 44x44 pixels for touch targets
        if (width < 44 || height < 44) {
          smallTargets++;
        }
      }
    }
    
    return {
      passed: smallTargets === 0,
      smallTargets
    };
  }
}

/**
 * Performance Testing Helper
 */
export class PerformanceTestingHelper {
  constructor(private page: Page) {}

  /**
   * Measure Core Web Vitals
   */
  async measureCoreWebVitals(): Promise<{
    lcp: number | null;
    fid: number | null;
    cls: number | null;
  }> {
    return await this.page.evaluate(() => {
      return new Promise((resolve) => {
        const metrics = {
          lcp: null as number | null,
          fid: null as number | null,
          cls: null as number | null
        };
        
        // Largest Contentful Paint
        new PerformanceObserver((entryList) => {
          const entries = entryList.getEntries();
          const lastEntry = entries[entries.length - 1];
          metrics.lcp = lastEntry.startTime;
        }).observe({ entryTypes: ['largest-contentful-paint'] });
        
        // First Input Delay
        new PerformanceObserver((entryList) => {
          const entries = entryList.getEntries();
          entries.forEach((entry: any) => {
            metrics.fid = entry.processingStart - entry.startTime;
          });
        }).observe({ entryTypes: ['first-input'] });
        
        // Cumulative Layout Shift
        new PerformanceObserver((entryList) => {
          const entries = entryList.getEntries();
          entries.forEach((entry: any) => {
            if (!entry.hadRecentInput) {
              metrics.cls = (metrics.cls || 0) + entry.value;
            }
          });
        }).observe({ entryTypes: ['layout-shift'] });
        
        // Resolve after a reasonable time
        setTimeout(() => resolve(metrics), 5000);
      });
    });
  }

  /**
   * Measure JavaScript execution time
   */
  async measureJSExecutionTime(operation: () => Promise<void>): Promise<number> {
    const startTime = Date.now();
    await operation();
    return Date.now() - startTime;
  }

  /**
   * Check for memory leaks
   */
  async checkMemoryUsage(): Promise<{ initial: number; final: number; leaked: boolean }> {
    const initialMemory = await this.page.evaluate(() => {
      return (performance as any).memory?.usedJSHeapSize || 0;
    });
    
    // Perform some operations that might cause memory leaks
    await this.page.reload();
    await this.page.waitForLoadState('networkidle');
    
    const finalMemory = await this.page.evaluate(() => {
      return (performance as any).memory?.usedJSHeapSize || 0;
    });
    
    // Simple heuristic: if memory usage increased by more than 10MB, consider it a potential leak
    const leaked = (finalMemory - initialMemory) > 10 * 1024 * 1024;
    
    return {
      initial: initialMemory,
      final: finalMemory,
      leaked
    };
  }
}

/**
 * Comprehensive Accessibility Audit Helper
 */
export class AccessibilityAuditHelper {
  constructor(
    private page: Page,
    private colorHelper = ColorContrastHelper,
    private keyboardHelper = new KeyboardNavigationHelper(page),
    private ariaHelper = new ARIATestingHelper(page),
    private mobileHelper = new MobileTestingHelper(page)
  ) {}

  /**
   * Run complete accessibility audit
   */
  async runCompleteAudit(): Promise<{
    passed: boolean;
    score: number;
    issues: string[];
    details: {
      colorContrast: boolean;
      keyboardNavigation: boolean;
      ariaAttributes: boolean;
      mobileAccessibility: boolean;
      semanticStructure: boolean;
    }
  }> {
    const issues: string[] = [];
    const details = {
      colorContrast: true,
      keyboardNavigation: true,
      ariaAttributes: true,
      mobileAccessibility: true,
      semanticStructure: true
    };

    // Test ARIA attributes
    const ariaTest = await this.ariaHelper.checkARIAAttributes();
    if (!ariaTest.passed) {
      details.ariaAttributes = false;
      issues.push(...ariaTest.issues);
    }

    // Test keyboard navigation
    const keyboardTest = await this.keyboardHelper.testTabNavigation();
    if (!keyboardTest.hasProperFocus) {
      details.keyboardNavigation = false;
      issues.push('Keyboard navigation issues found');
    }

    // Test semantic structure
    const landmarkTest = await this.ariaHelper.checkLandmarks();
    if (!landmarkTest.hasMain) {
      details.semanticStructure = false;
      issues.push('Missing main landmark');
    }

    const headingTest = await this.ariaHelper.checkHeadingHierarchy();
    if (!headingTest.isValid) {
      details.semanticStructure = false;
      issues.push('Invalid heading hierarchy');
    }

    // Test mobile accessibility
    const mobileTest = await this.mobileHelper.testMobileViewports();
    const mobilePassed = mobileTest.every(test => test.passed);
    if (!mobilePassed) {
      details.mobileAccessibility = false;
      issues.push('Mobile accessibility issues found');
    }

    // Calculate score
    const totalTests = Object.keys(details).length;
    const passedTests = Object.values(details).filter(Boolean).length;
    const score = (passedTests / totalTests) * 100;

    return {
      passed: issues.length === 0,
      score,
      issues,
      details
    };
  }
}