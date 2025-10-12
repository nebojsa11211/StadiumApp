import { test } from '@playwright/test';

test('Debug sidebar styles on Stadium Overview', async ({ page }) => {
  // Login
  await page.goto('https://localhost:7030/login');
  await page.fill('#admin-login-email-input', 'admin@stadium.com');
  await page.fill('#admin-login-password-input', 'admin123');
  await page.click('#admin-login-submit-btn');
  await page.waitForURL('https://localhost:7030/');

  // Go to Stadium Overview
  await page.goto('https://localhost:7030/admin/stadium-overview');
  await page.waitForLoadState('networkidle');

  // Get sidebar element and its computed styles
  const sidebar = page.locator('#admin-layout-sidebar');

  const styles = await sidebar.evaluate((el) => {
    const computed = window.getComputedStyle(el);
    return {
      display: computed.display,
      width: computed.width,
      minWidth: computed.minWidth,
      maxWidth: computed.maxWidth,
      position: computed.position,
      visibility: computed.visibility,
      opacity: computed.opacity,
      zIndex: computed.zIndex,
      transform: computed.transform,
      overflow: computed.overflow
    };
  });

  console.log('Sidebar computed styles:', JSON.stringify(styles, null, 2));

  // Get parent page styles
  const pageStyles = await page.locator('#admin-layout-page').evaluate((el) => {
    const computed = window.getComputedStyle(el);
    return {
      display: computed.display,
      flexDirection: computed.flexDirection,
      width: computed.width
    };
  });

  console.log('Page container styles:', JSON.stringify(pageStyles, null, 2));

  // Check all applied stylesheets
  const appliedStyles = await sidebar.evaluate((el) => {
    const sheets = Array.from(document.styleSheets);
    const rules: string[] = [];

    sheets.forEach((sheet) => {
      try {
        const cssRules = Array.from(sheet.cssRules || []);
        cssRules.forEach((rule: any) => {
          if (rule.selectorText && el.matches(rule.selectorText)) {
            rules.push(`${rule.selectorText} { ${rule.style.cssText} }`);
          }
        });
      } catch (e) {
        // Skip cross-origin stylesheets
      }
    });

    return rules;
  });

  console.log('\nApplied CSS rules for sidebar:');
  appliedStyles.forEach(rule => console.log(rule));

  await page.screenshot({ path: 'sidebar-debug.png', fullPage: true });
});
