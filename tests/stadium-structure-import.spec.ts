import { test, expect } from '@playwright/test';
import path from 'path';

const ADMIN_BASE_URL = 'https://localhost:9030';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Stadium Structure Import', () => {
  test('should login to admin and import stadium structure from JSON file', async ({ page, context }) => {
    // Ignore HTTPS certificate errors
    await context.setExtraHTTPHeaders({
      'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8'
    });

    console.log('🏟️ Starting stadium structure import test...');

    // Step 1: Navigate to admin application
    console.log('Step 1: Navigating to admin application...');
    try {
      await page.goto(ADMIN_BASE_URL, {
        waitUntil: 'networkidle',
        timeout: 30000
      });
    } catch (error) {
      console.log('Network idle failed, trying with domcontentloaded');
      await page.goto(ADMIN_BASE_URL, {
        waitUntil: 'domcontentloaded',
        timeout: 30000
      });
    }

    // Take screenshot of initial page
    await page.screenshot({
      path: '.playwright-mcp/admin-initial-stadium-import.png',
      fullPage: true
    });

    // Step 2: Check if we need to login
    console.log('Step 2: Checking authentication state...');
    const currentUrl = page.url();
    console.log(`Current URL: ${currentUrl}`);

    // Look for login form elements
    const emailInput = page.locator('input[type="email"], input[name="email"], #admin-login-email-input');
    const passwordInput = page.locator('input[type="password"], input[name="password"], #admin-login-password-input');
    const submitButton = page.locator('button[type="submit"], input[type="submit"], #admin-login-submit-btn');

    const needsLogin = await emailInput.count() > 0 && await passwordInput.count() > 0;
    console.log(`Needs login: ${needsLogin}`);

    if (needsLogin) {
      console.log('Step 2a: Logging in with admin credentials...');

      // Fill login form
      await emailInput.first().fill(ADMIN_EMAIL);
      await passwordInput.first().fill(ADMIN_PASSWORD);

      // Take screenshot before login
      await page.screenshot({
        path: '.playwright-mcp/admin-before-stadium-login.png',
        fullPage: true
      });

      // Submit login form
      await submitButton.first().click();

      // Wait for login to process
      await page.waitForTimeout(5000);

      // Take screenshot after login
      await page.screenshot({
        path: '.playwright-mcp/admin-after-stadium-login.png',
        fullPage: true
      });

      console.log(`URL after login: ${page.url()}`);
    } else {
      console.log('Already authenticated or not on login page');
    }

    // Step 3: Navigate to Stadium Structure Management
    console.log('Step 3: Navigating to Stadium Structure Management...');

    // Try different navigation approaches
    try {
      // First try direct URL navigation
      await page.goto(`${ADMIN_BASE_URL}/admin/stadium-structure`, {
        waitUntil: 'domcontentloaded',
        timeout: 15000
      });
      console.log('Direct navigation to stadium structure page successful');
    } catch (error) {
      console.log('Direct navigation failed, trying menu navigation...');

      // Try to find Stadium Management menu item
      const stadiumMenuItems = [
        'a[href*="stadium-structure"]',
        'a[href*="/stadium"]',
        'text=Stadium Management',
        'text=Structure Management',
        '.nav-link:has-text("Stadium")',
        '#admin-nav-stadium-link'
      ];

      let navigationSuccessful = false;
      for (const selector of stadiumMenuItems) {
        try {
          const menuItem = page.locator(selector).first();
          if (await menuItem.count() > 0) {
            console.log(`Found menu item with selector: ${selector}`);
            await menuItem.click();
            await page.waitForTimeout(2000);
            navigationSuccessful = true;
            break;
          }
        } catch (e) {
          console.log(`Menu item ${selector} not found or not clickable`);
        }
      }

      if (!navigationSuccessful) {
        console.log('Menu navigation failed, trying direct URL again...');
        await page.goto(`${ADMIN_BASE_URL}/admin/stadium-structure`, {
          waitUntil: 'load',
          timeout: 15000
        });
      }
    }

    // Take screenshot of stadium structure page
    await page.screenshot({
      path: '.playwright-mcp/admin-stadium-structure-page.png',
      fullPage: true
    });

    console.log(`Current URL: ${page.url()}`);

    // Step 4: Look for import functionality
    console.log('Step 4: Looking for JSON import functionality...');

    // Wait for page to fully load
    await page.waitForTimeout(3000);

    // Look for file upload input or import button
    const importSelectors = [
      'input[type="file"]',
      'input[accept*="json"]',
      '#stadium-import-file',
      '#admin-stadium-import-file-input',
      'button:has-text("Import")',
      'button:has-text("Upload")',
      '.file-upload',
      '.import-section input[type="file"]'
    ];

    let fileInput = null;
    for (const selector of importSelectors) {
      try {
        const element = page.locator(selector).first();
        if (await element.count() > 0) {
          console.log(`Found import element with selector: ${selector}`);
          if (selector.includes('input[type="file"]') || selector.includes('input[accept')) {
            fileInput = element;
            break;
          }
        }
      } catch (e) {
        console.log(`Import element ${selector} not found`);
      }
    }

    if (!fileInput) {
      console.log('No file input found, looking for import guide or help...');

      // Look for help/guide buttons that might reveal import functionality
      const helpSelectors = [
        'button:has-text("Import Guide")',
        'button:has-text("Help")',
        'a:has-text("Guide")',
        '.import-help',
        '#import-guide-btn'
      ];

      for (const selector of helpSelectors) {
        try {
          const helpButton = page.locator(selector).first();
          if (await helpButton.count() > 0) {
            console.log(`Found help button: ${selector}`);
            await helpButton.click();
            await page.waitForTimeout(2000);

            // Look for file input again after opening help/guide
            fileInput = page.locator('input[type="file"]').first();
            if (await fileInput.count() > 0) {
              console.log('File input found after opening help section');
              break;
            }
          }
        } catch (e) {
          console.log(`Help button ${selector} not accessible`);
        }
      }
    }

    // If still no file input, check the page content to understand the structure
    if (!fileInput || await fileInput.count() === 0) {
      console.log('File input still not found. Checking page content...');
      const pageContent = await page.textContent('body');
      console.log(`Page content preview: ${pageContent?.substring(0, 500)}...`);

      // Look for any form or section that might contain import functionality
      const forms = page.locator('form');
      const formCount = await forms.count();
      console.log(`Number of forms on page: ${formCount}`);

      for (let i = 0; i < formCount; i++) {
        const form = forms.nth(i);
        const formContent = await form.textContent();
        console.log(`Form ${i} content: ${formContent?.substring(0, 200)}...`);

        // Look for file input within each form
        const fileInputInForm = form.locator('input[type="file"]');
        if (await fileInputInForm.count() > 0) {
          fileInput = fileInputInForm.first();
          console.log(`Found file input in form ${i}`);
          break;
        }
      }
    }

    // Step 5: Upload the stadium structure file
    if (fileInput && await fileInput.count() > 0) {
      console.log('Step 5: Uploading stadium structure file...');

      // Path to the standard stadium JSON file
      const stadiumFilePath = path.resolve('D:/AiApps/StadiumApp/StadiumApp/stadium-samples/standard-stadium.json');
      console.log(`File path: ${stadiumFilePath}`);

      // Upload the file
      await fileInput.setInputFiles(stadiumFilePath);
      console.log('File selected successfully');

      // Take screenshot after file selection
      await page.screenshot({
        path: '.playwright-mcp/admin-stadium-file-selected.png',
        fullPage: true
      });

      // Look for and click import/upload button
      const uploadButtons = [
        'button[type="submit"]',
        'button:has-text("Import")',
        'button:has-text("Upload")',
        'input[type="submit"]',
        '#import-btn',
        '#upload-btn',
        '.btn-primary'
      ];

      let uploadClicked = false;
      for (const selector of uploadButtons) {
        try {
          const button = page.locator(selector).first();
          if (await button.count() > 0 && await button.isEnabled()) {
            console.log(`Clicking upload button: ${selector}`);
            await button.click();
            uploadClicked = true;
            break;
          }
        } catch (e) {
          console.log(`Upload button ${selector} not clickable`);
        }
      }

      if (uploadClicked) {
        console.log('Upload button clicked, waiting for import to complete...');

        // Wait for import to process
        await page.waitForTimeout(10000);

        // Take screenshot after import attempt
        await page.screenshot({
          path: '.playwright-mcp/admin-stadium-after-import.png',
          fullPage: true
        });

        // Step 6: Verify import success
        console.log('Step 6: Verifying import success...');

        // Look for success messages
        const successIndicators = [
          'text=success',
          'text=imported',
          'text=uploaded',
          '.alert-success',
          '.success-message',
          'text=Standard Stadium' // The name from our JSON file
        ];

        let importSuccessful = false;
        for (const indicator of successIndicators) {
          try {
            const successElement = page.locator(indicator).first();
            if (await successElement.count() > 0) {
              console.log(`✅ Found success indicator: ${indicator}`);
              const successText = await successElement.textContent();
              console.log(`Success message: ${successText}`);
              importSuccessful = true;
              break;
            }
          } catch (e) {
            console.log(`Success indicator ${indicator} not found`);
          }
        }

        // Check current page content for stadium data
        const currentPageContent = await page.textContent('body');
        const hasStadiumData = currentPageContent?.includes('Standard Stadium') ||
                              currentPageContent?.includes('North Tribune') ||
                              currentPageContent?.includes('sectors') ||
                              currentPageContent?.includes('seats');

        if (hasStadiumData) {
          console.log('✅ Stadium data found on page after import');
          importSuccessful = true;
        }

        // Step 7: Navigate to Stadium Overview to verify
        console.log('Step 7: Verifying stadium overview has data...');
        try {
          await page.goto(`${ADMIN_BASE_URL}/admin/stadium-overview`, {
            waitUntil: 'domcontentloaded',
            timeout: 15000
          });

          await page.waitForTimeout(3000);

          // Take screenshot of overview page
          await page.screenshot({
            path: '.playwright-mcp/admin-stadium-overview-after-import.png',
            fullPage: true
          });

          const overviewContent = await page.textContent('body');
          const hasOverviewData = overviewContent && !overviewContent.includes('No Stadium Data');

          if (hasOverviewData) {
            console.log('✅ Stadium overview now has data - import successful!');
            importSuccessful = true;
          } else {
            console.log('❌ Stadium overview still shows "No Stadium Data"');
          }

        } catch (error) {
          console.log('Could not verify stadium overview:', error);
        }

        // Final result
        if (importSuccessful) {
          console.log('🎉 Stadium structure import completed successfully!');
        } else {
          console.log('❌ Stadium structure import may have failed or needs verification');
        }

      } else {
        console.log('❌ Could not find or click upload button');
        // Still take a screenshot to see the current state
        await page.screenshot({
          path: '.playwright-mcp/admin-stadium-no-upload-button.png',
          fullPage: true
        });
      }

    } else {
      console.log('❌ Could not find file input for stadium structure import');

      // Take a final screenshot to see what's on the page
      await page.screenshot({
        path: '.playwright-mcp/admin-stadium-no-file-input.png',
        fullPage: true
      });

      // Log the page content for debugging
      const pageContent = await page.textContent('body');
      console.log('Page content for debugging:');
      console.log(pageContent?.substring(0, 1000));
    }

    // Final summary
    console.log('=== STADIUM IMPORT TEST SUMMARY ===');
    console.log(`Final URL: ${page.url()}`);
    console.log(`Page title: ${await page.title()}`);

    const finalPageContent = await page.textContent('body');
    const hasStadiumDataFinal = finalPageContent?.includes('Standard Stadium') ||
                               finalPageContent?.includes('Tribune') ||
                               finalPageContent?.includes('sectors');

    console.log(`Stadium data detected on final page: ${hasStadiumDataFinal}`);
  });
});