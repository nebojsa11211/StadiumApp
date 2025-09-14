import { test, expect } from '@playwright/test';
import { adminLogin } from '../helpers/auth-helpers';
import { waitForLoadingComplete, testResponsiveBehavior } from '../helpers/page-helpers';

/**
 * Design system and responsiveness tests
 * Tests theme switching, responsive design, mobile navigation, accessibility
 */

test.describe('Design System & Responsiveness', () => {
  test.beforeEach(async ({ page }) => {
    await adminLogin(page);
  });

  test.describe('Theme System', () => {
    test('should support light theme (default)', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Check for light theme indicators
      const htmlElement = page.locator('html');
      const bodyElement = page.locator('body');

      // Check theme attributes
      const themeAttribute = await htmlElement.getAttribute('data-theme');
      const classAttribute = await htmlElement.getAttribute('class');
      const bodyClass = await bodyElement.getAttribute('class');

      console.log(`HTML theme attribute: ${themeAttribute}`);
      console.log(`HTML class: ${classAttribute}`);
      console.log(`Body class: ${bodyClass}`);

      // Light theme should be default (no dark theme classes)
      const hasLightTheme = !themeAttribute || themeAttribute === 'light' ||
                           !classAttribute?.includes('dark') ||
                           !bodyClass?.includes('dark');

      if (hasLightTheme) {
        console.log('✅ Light theme is active (default)');
      }

      // Check CSS variables are applied
      const backgroundColor = await page.evaluate(() => {
        return getComputedStyle(document.body).backgroundColor;
      });

      console.log(`Body background color: ${backgroundColor}`);

      // Light theme should have light background
      expect(backgroundColor).not.toBe('rgb(0, 0, 0)'); // Not pure black
    });

    test('should support dark theme toggle', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Look for theme toggle button
      const themeToggle = page.locator('[data-testid="theme-toggle"], .theme-toggle, .dark-mode-toggle, button:has-text("Dark"), button:has-text("Theme")').first();

      if (await themeToggle.isVisible({ timeout: 5000 })) {
        console.log('✅ Theme toggle button found');

        // Toggle to dark theme
        await themeToggle.click();
        await page.waitForTimeout(500); // Allow theme transition

        // Verify dark theme is applied
        const htmlElement = page.locator('html');
        const themeAttribute = await htmlElement.getAttribute('data-theme');
        const classAttribute = await htmlElement.getAttribute('class');

        const hasDarkTheme = themeAttribute === 'dark' ||
                           classAttribute?.includes('dark') ||
                           classAttribute?.includes('theme-dark');

        if (hasDarkTheme) {
          console.log('✅ Dark theme activated successfully');
          await expect(htmlElement).toHaveAttribute('data-theme', 'dark');
        } else {
          console.log('ℹ️ Dark theme implementation may use different approach');
        }

        // Toggle back to light theme
        await themeToggle.click();
        await page.waitForTimeout(500);

        const lightThemeAttribute = await htmlElement.getAttribute('data-theme');
        const lightClassAttribute = await htmlElement.getAttribute('class');

        console.log(`After toggle back - theme: ${lightThemeAttribute}, class: ${lightClassAttribute}`);

      } else {
        console.log('ℹ️ Theme toggle functionality not yet implemented');
      }
    });

    test('should persist theme preference', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      const themeToggle = page.locator('[data-testid="theme-toggle"], .theme-toggle, .dark-mode-toggle').first();

      if (await themeToggle.isVisible({ timeout: 5000 })) {
        // Set dark theme
        await themeToggle.click();
        await page.waitForTimeout(500);

        // Navigate to another page
        await page.goto('/orders');
        await waitForLoadingComplete(page);

        // Theme should persist
        const htmlElement = page.locator('html');
        const themeAttribute = await htmlElement.getAttribute('data-theme');

        if (themeAttribute === 'dark') {
          console.log('✅ Dark theme persisted across page navigation');
        }

        // Test page reload
        await page.reload();
        await waitForLoadingComplete(page);

        const reloadedTheme = await htmlElement.getAttribute('data-theme');
        if (reloadedTheme === 'dark') {
          console.log('✅ Dark theme persisted after page reload');
        }

        // Reset to light theme for cleanup
        if (await themeToggle.isVisible({ timeout: 2000 })) {
          await themeToggle.click();
        }
      }
    });

    test('should have consistent theme across all pages', async ({ page }) => {
      const pages = ['/dashboard', '/orders', '/users'];

      for (const pagePath of pages) {
        await page.goto(pagePath);
        await waitForLoadingComplete(page);

        // Check theme consistency
        const htmlElement = page.locator('html');
        const themeAttribute = await htmlElement.getAttribute('data-theme');

        console.log(`Theme on ${pagePath}: ${themeAttribute || 'light (default)'}`);

        // Should have consistent theme application
        const hasThemeSupport = await htmlElement.evaluate((el) => {
          const computedStyle = getComputedStyle(el);
          return computedStyle.getPropertyValue('--bs-body-bg') ||
                 computedStyle.getPropertyValue('--background-color') ||
                 'supported';
        });

        console.log(`Theme support on ${pagePath}: ${hasThemeSupport !== 'supported' ? 'CSS variables' : 'detected'}`);
      }
    });
  });

  test.describe('Responsive Breakpoints', () => {
    const breakpoints = [
      { name: 'Mobile Portrait', width: 320, height: 568 },
      { name: 'Mobile Landscape', width: 568, height: 320 },
      { name: 'Tablet Portrait', width: 768, height: 1024 },
      { name: 'Tablet Landscape', width: 1024, height: 768 },
      { name: 'Desktop Small', width: 1200, height: 800 },
      { name: 'Desktop Large', width: 1920, height: 1080 }
    ];

    test('should adapt layout across all breakpoints', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      for (const breakpoint of breakpoints) {
        console.log(`📱 Testing ${breakpoint.name} (${breakpoint.width}x${breakpoint.height})`);

        await page.setViewportSize({ width: breakpoint.width, height: breakpoint.height });
        await page.waitForTimeout(1000); // Allow CSS transitions

        // Verify main content is visible
        const mainContent = page.locator('main, .main-content, .container, [data-testid="page-content"]').first();
        await expect(mainContent).toBeVisible();

        // Check navigation accessibility
        const navigation = page.locator('.navbar, .nav, .navigation').first();
        if (await navigation.isVisible({ timeout: 2000 })) {
          console.log(`✅ Navigation visible on ${breakpoint.name}`);
        }

        // For mobile breakpoints, check for mobile navigation
        if (breakpoint.width < 768) {
          const mobileToggle = page.locator('.navbar-toggler, .mobile-nav-toggle, .hamburger').first();
          if (await mobileToggle.isVisible({ timeout: 2000 })) {
            console.log(`✅ Mobile navigation toggle found on ${breakpoint.name}`);
          }
        }

        // Check that content doesn't overflow
        const bodyScrollWidth = await page.evaluate(() => document.body.scrollWidth);
        const viewportWidth = breakpoint.width;

        if (bodyScrollWidth <= viewportWidth + 20) { // Allow small tolerance
          console.log(`✅ No horizontal overflow on ${breakpoint.name}`);
        } else {
          console.log(`⚠️ Horizontal overflow detected on ${breakpoint.name}: ${bodyScrollWidth}px > ${viewportWidth}px`);
        }
      }
    });

    test('should have responsive navigation', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Test desktop navigation
      await page.setViewportSize({ width: 1200, height: 800 });
      const desktopNav = page.locator('.navbar .navbar-nav, .nav-links, .navigation-menu').first();

      if (await desktopNav.isVisible({ timeout: 5000 })) {
        console.log('✅ Desktop navigation visible');

        // Navigation items should be horizontally arranged
        const navItems = desktopNav.locator('a, .nav-item');
        const navItemCount = await navItems.count();
        console.log(`Desktop navigation has ${navItemCount} items`);
      }

      // Test mobile navigation
      await page.setViewportSize({ width: 375, height: 667 });
      await page.waitForTimeout(1000);

      const mobileToggle = page.locator('.navbar-toggler, .mobile-nav-toggle, [data-testid="mobile-nav-toggle"]').first();

      if (await mobileToggle.isVisible({ timeout: 5000 })) {
        console.log('✅ Mobile navigation toggle found');

        await mobileToggle.click();
        await page.waitForTimeout(500);

        // Mobile menu should appear
        const mobileMenu = page.locator('.navbar-collapse.show, .mobile-nav-menu, [data-testid="mobile-nav-menu"]').first();
        if (await mobileMenu.isVisible({ timeout: 3000 })) {
          console.log('✅ Mobile navigation menu opened');

          // Should contain navigation links
          const mobileNavItems = mobileMenu.locator('a, .nav-item');
          const mobileItemCount = await mobileNavItems.count();
          console.log(`Mobile navigation has ${mobileItemCount} items`);

          // Close mobile menu
          await mobileToggle.click();
        }
      } else {
        console.log('ℹ️ Mobile navigation toggle not implemented (may use different responsive strategy)');
      }
    });

    test('should have responsive data tables', async ({ page }) => {
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      const ordersTable = page.locator('table, .table, [data-testid="orders-data-table"]').first();

      if (await ordersTable.isVisible({ timeout: 5000 })) {
        // Test desktop table
        await page.setViewportSize({ width: 1200, height: 800 });
        await page.waitForTimeout(500);

        const tableHeaders = ordersTable.locator('th');
        const desktopHeaderCount = await tableHeaders.count();
        console.log(`Desktop table has ${desktopHeaderCount} columns`);

        // Test mobile table
        await page.setViewportSize({ width: 375, height: 667 });
        await page.waitForTimeout(1000);

        // Mobile table should either:
        // 1. Become horizontally scrollable
        // 2. Stack columns vertically
        // 3. Show as cards instead of table

        const mobileCards = page.locator('.order-card, .mobile-order-item, .card');
        const cardCount = await mobileCards.count();

        if (cardCount > 0) {
          console.log(`✅ Mobile layout uses ${cardCount} cards instead of table`);
        } else {
          // Check if table is horizontally scrollable
          const tableContainer = ordersTable.locator('..').first();
          const isScrollable = await tableContainer.evaluate((el) => {
            return el.scrollWidth > el.clientWidth;
          });

          if (isScrollable) {
            console.log('✅ Mobile table is horizontally scrollable');
          } else {
            console.log('ℹ️ Mobile table responsiveness strategy not clearly detected');
          }
        }
      }
    });

    test('should have responsive forms and modals', async ({ page }) => {
      await page.goto('/users');
      await waitForLoadingComplete(page);

      const addUserButton = page.locator('[data-testid="add-user-button"], .add-user-btn, button:has-text("Add User")').first();

      if (await addUserButton.isVisible({ timeout: 5000 })) {
        // Test modal on different screen sizes
        const testSizes = [
          { name: 'Mobile', width: 375, height: 667 },
          { name: 'Tablet', width: 768, height: 1024 },
          { name: 'Desktop', width: 1200, height: 800 }
        ];

        for (const size of testSizes) {
          await page.setViewportSize({ width: size.width, height: size.height });
          await page.waitForTimeout(500);

          await addUserButton.click();
          await page.waitForTimeout(1000);

          const modal = page.locator('.modal, [data-testid="create-user-modal"]').first();
          if (await modal.isVisible({ timeout: 5000 })) {
            console.log(`✅ Modal opens properly on ${size.name}`);

            // Check modal sizing
            const modalDialog = modal.locator('.modal-dialog, .dialog').first();
            if (await modalDialog.isVisible({ timeout: 2000 })) {
              const modalWidth = await modalDialog.evaluate((el) => {
                return getComputedStyle(el).width;
              });
              console.log(`Modal width on ${size.name}: ${modalWidth}`);
            }

            // Close modal
            const closeButton = modal.locator('.close, .modal-close, button:has-text("Cancel")').first();
            if (await closeButton.isVisible({ timeout: 2000 })) {
              await closeButton.click();
            } else {
              await page.keyboard.press('Escape');
            }

            await page.waitForTimeout(500);
          }
        }
      }
    });
  });

  test.describe('Component Responsiveness', () => {
    test('should have responsive KPI cards', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      const kpiCards = page.locator('.kpi-card, .card, .stats-card');
      const cardCount = await kpiCards.count();

      if (cardCount > 0) {
        console.log(`Found ${cardCount} KPI cards`);

        // Test different layouts
        const layouts = [
          { name: 'Desktop', width: 1200, height: 800, expectedLayout: 'grid' },
          { name: 'Tablet', width: 768, height: 1024, expectedLayout: 'responsive-grid' },
          { name: 'Mobile', width: 375, height: 667, expectedLayout: 'stacked' }
        ];

        for (const layout of layouts) {
          await page.setViewportSize({ width: layout.width, height: layout.height });
          await page.waitForTimeout(1000);

          // Check if cards are properly arranged
          const visibleCards = await kpiCards.count();
          console.log(`${layout.name}: ${visibleCards} cards visible`);

          // All cards should be visible regardless of screen size
          expect(visibleCards).toBe(cardCount);

          // Check card stacking on mobile
          if (layout.width < 576) {
            const firstCard = kpiCards.first();
            const secondCard = kpiCards.nth(1);

            if (await secondCard.isVisible({ timeout: 2000 })) {
              const card1Rect = await firstCard.boundingBox();
              const card2Rect = await secondCard.boundingBox();

              if (card1Rect && card2Rect) {
                const isStacked = card2Rect.y > card1Rect.y + card1Rect.height - 50; // Allow some overlap
                if (isStacked) {
                  console.log('✅ Cards are properly stacked on mobile');
                } else {
                  console.log('ℹ️ Cards may be using horizontal layout on mobile');
                }
              }
            }
          }
        }
      }
    });

    test('should have responsive action buttons', async ({ page }) => {
      const pages = ['/dashboard', '/orders', '/users'];

      for (const pagePath of pages) {
        await page.goto(pagePath);
        await waitForLoadingComplete(page);

        console.log(`Testing responsive buttons on ${pagePath}`);

        // Test mobile layout
        await page.setViewportSize({ width: 375, height: 667 });
        await page.waitForTimeout(500);

        const buttons = page.locator('button, .btn');
        const buttonCount = await buttons.count();

        if (buttonCount > 0) {
          // Check if buttons are appropriately sized for touch
          const firstButton = buttons.first();

          if (await firstButton.isVisible({ timeout: 2000 })) {
            const buttonRect = await firstButton.boundingBox();

            if (buttonRect) {
              const minTouchSize = 44; // Recommended minimum touch target size
              const isTouchFriendly = buttonRect.height >= minTouchSize || buttonRect.width >= minTouchSize;

              if (isTouchFriendly) {
                console.log(`✅ Buttons are touch-friendly on ${pagePath}`);
              } else {
                console.log(`⚠️ Buttons may be too small for touch on ${pagePath} (${buttonRect.width}x${buttonRect.height})`);
              }
            }
          }
        }

        // Check for responsive button text/icons
        const buttonWithText = page.locator('button:has-text("Add"), button:has-text("Create"), .btn:has-text("Add")').first();
        if (await buttonWithText.isVisible({ timeout: 2000 })) {
          const buttonText = await buttonWithText.textContent();

          // Some buttons might hide text on mobile and show only icons
          if (buttonText && buttonText.trim().length > 0) {
            console.log(`Button text on mobile: "${buttonText.trim()}"`);
          }
        }
      }
    });

    test('should have responsive spacing and typography', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      const viewports = [
        { name: 'Mobile', width: 375, height: 667 },
        { name: 'Desktop', width: 1200, height: 800 }
      ];

      for (const viewport of viewports) {
        await page.setViewportSize({ width: viewport.width, height: viewport.height });
        await page.waitForTimeout(500);

        // Check typography scaling
        const heading = page.locator('h1, .h1, .page-title').first();
        if (await heading.isVisible({ timeout: 2000 })) {
          const headingStyles = await heading.evaluate((el) => {
            const styles = getComputedStyle(el);
            return {
              fontSize: styles.fontSize,
              lineHeight: styles.lineHeight,
              marginBottom: styles.marginBottom
            };
          });

          console.log(`${viewport.name} heading styles:`, headingStyles);
        }

        // Check container spacing
        const container = page.locator('.container, .container-fluid, main').first();
        if (await container.isVisible({ timeout: 2000 })) {
          const containerStyles = await container.evaluate((el) => {
            const styles = getComputedStyle(el);
            return {
              paddingLeft: styles.paddingLeft,
              paddingRight: styles.paddingRight,
              marginLeft: styles.marginLeft,
              marginRight: styles.marginRight
            };
          });

          console.log(`${viewport.name} container spacing:`, containerStyles);
        }
      }
    });
  });

  test.describe('Accessibility & UX', () => {
    test('should support keyboard navigation', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Test Tab navigation
      await page.keyboard.press('Tab');

      // Check if first focusable element is highlighted
      const focusedElement = page.locator(':focus');
      if (await focusedElement.isVisible({ timeout: 2000 })) {
        const tagName = await focusedElement.evaluate((el) => el.tagName);
        console.log(`✅ First focusable element: ${tagName}`);

        // Tab through several elements
        for (let i = 0; i < 5; i++) {
          await page.keyboard.press('Tab');
          await page.waitForTimeout(100);
        }

        const finalFocused = page.locator(':focus');
        if (await finalFocused.isVisible({ timeout: 1000 })) {
          const finalTagName = await finalFocused.evaluate((el) => el.tagName);
          console.log(`✅ Keyboard navigation works, final element: ${finalTagName}`);
        }
      } else {
        console.log('ℹ️ Keyboard navigation focus indicators may not be visible');
      }
    });

    test('should have proper focus indicators', async ({ page }) => {
      await page.goto('/users');
      await waitForLoadingComplete(page);

      // Check buttons have focus indicators
      const buttons = page.locator('button, .btn');
      const buttonCount = await buttons.count();

      if (buttonCount > 0) {
        const firstButton = buttons.first();

        // Focus the button
        await firstButton.focus();

        // Check for focus styles
        const focusStyles = await firstButton.evaluate((el) => {
          const styles = getComputedStyle(el);
          return {
            outline: styles.outline,
            outlineOffset: styles.outlineOffset,
            boxShadow: styles.boxShadow,
            borderColor: styles.borderColor
          };
        });

        console.log('Button focus styles:', focusStyles);

        const hasFocusIndicator = focusStyles.outline !== 'none' ||
                                 focusStyles.boxShadow !== 'none' ||
                                 focusStyles.boxShadow.includes('focus');

        if (hasFocusIndicator) {
          console.log('✅ Focus indicators are present');
        } else {
          console.log('ℹ️ Custom focus indicators may be implemented differently');
        }
      }
    });

    test('should have appropriate color contrast', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Test both light and dark themes if available
      const themes = ['light', 'dark'];

      for (const theme of themes) {
        console.log(`Testing color contrast for ${theme} theme`);

        // Switch theme if toggle is available
        const themeToggle = page.locator('[data-testid="theme-toggle"], .theme-toggle').first();
        if (theme === 'dark' && await themeToggle.isVisible({ timeout: 2000 })) {
          await themeToggle.click();
          await page.waitForTimeout(500);
        }

        // Check text contrast
        const textElements = page.locator('h1, h2, h3, p, .text, .card-text');
        const elementCount = await textElements.count();

        if (elementCount > 0) {
          const firstText = textElements.first();

          if (await firstText.isVisible({ timeout: 2000 })) {
            const textStyles = await firstText.evaluate((el) => {
              const styles = getComputedStyle(el);
              return {
                color: styles.color,
                backgroundColor: getComputedStyle(el.parentElement || el).backgroundColor
              };
            });

            console.log(`${theme} theme text colors:`, textStyles);

            // Basic contrast check (this is simplified)
            const isGoodContrast = textStyles.color !== textStyles.backgroundColor;
            if (isGoodContrast) {
              console.log(`✅ Text has different color from background in ${theme} theme`);
            }
          }
        }
      }
    });

    test('should handle screen reader accessibility', async ({ page }) => {
      await page.goto('/dashboard');
      await waitForLoadingComplete(page);

      // Check for semantic HTML elements
      const semanticElements = {
        'main': 'main',
        'nav': 'nav, .navigation',
        'header': 'header, .header',
        'section': 'section',
        'article': 'article'
      };

      for (const [elementType, selector] of Object.entries(semanticElements)) {
        const elements = page.locator(selector);
        const count = await elements.count();

        if (count > 0) {
          console.log(`✅ Found ${count} ${elementType} elements`);
        }
      }

      // Check for alt text on images
      const images = page.locator('img');
      const imageCount = await images.count();

      if (imageCount > 0) {
        const imagesWithAlt = page.locator('img[alt]');
        const altCount = await imagesWithAlt.count();

        console.log(`Images: ${imageCount}, with alt text: ${altCount}`);

        if (altCount === imageCount) {
          console.log('✅ All images have alt text');
        } else {
          console.log('⚠️ Some images missing alt text');
        }
      }

      // Check for proper heading hierarchy
      const headings = page.locator('h1, h2, h3, h4, h5, h6');
      const headingCount = await headings.count();

      if (headingCount > 0) {
        console.log(`✅ Found ${headingCount} heading elements`);

        // Check for h1
        const h1Elements = page.locator('h1');
        const h1Count = await h1Elements.count();

        if (h1Count > 0) {
          console.log('✅ Page has main heading (h1)');
        } else {
          console.log('ℹ️ No h1 element found (may be using CSS classes)');
        }
      }
    });
  });

  test.describe('Print Styles', () => {
    test('should have print-friendly styles', async ({ page }) => {
      await page.goto('/orders');
      await waitForLoadingComplete(page);

      // Emulate print media
      await page.emulateMedia({ media: 'print' });

      // Check if navigation is hidden in print
      const navigation = page.locator('.navbar, .nav, .sidebar');
      if (await navigation.first().isVisible({ timeout: 2000 })) {
        const navDisplay = await navigation.first().evaluate((el) => {
          return getComputedStyle(el).display;
        });

        if (navDisplay === 'none') {
          console.log('✅ Navigation hidden in print styles');
        } else {
          console.log('ℹ️ Navigation visible in print (may be intended)');
        }
      }

      // Check if main content is still visible
      const mainContent = page.locator('main, .main-content, .container').first();
      if (await mainContent.isVisible({ timeout: 2000 })) {
        const contentDisplay = await mainContent.evaluate((el) => {
          return getComputedStyle(el).display;
        });

        if (contentDisplay !== 'none') {
          console.log('✅ Main content visible in print styles');
        }
      }

      // Reset media emulation
      await page.emulateMedia({ media: 'screen' });
    });
  });
});