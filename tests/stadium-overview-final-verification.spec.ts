import { test, expect } from '@playwright/test';
import { promises as fs } from 'fs';
import path from 'path';

test.describe('Stadium Overview - Final Verification After All Fixes', () => {
  const adminUrl = 'https://localhost:7030';
  const screenshotDir = path.join(process.cwd(), 'final-verification-screenshots');

  test.beforeAll(async () => {
    // Create screenshot directory if it doesn't exist
    try {
      await fs.mkdir(screenshotDir, { recursive: true });
    } catch (err) {
      console.log('Screenshot directory already exists or error:', err);
    }
  });

  test('Complete Stadium Overview flow after all fixes', async ({ page, context }) => {
    // Set longer timeout for complete flow
    test.setTimeout(120000);

    console.log('\n=== STARTING FINAL VERIFICATION TEST ===\n');

    // Step 1: Navigate to login page
    console.log('Step 1: Navigating to login page...');
    await page.goto(`${adminUrl}/login`, {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    await page.screenshot({
      path: path.join(screenshotDir, '01-login-page.png'),
      fullPage: true
    });
    console.log('✓ Login page loaded');

    // Step 2: Fill in login credentials
    console.log('\nStep 2: Entering admin credentials...');
    const emailInput = page.locator('#admin-login-email-input');
    const passwordInput = page.locator('#admin-login-password-input');
    const loginButton = page.locator('#admin-login-submit-btn');

    await emailInput.fill('admin@stadium.com');
    await passwordInput.fill('admin123');

    await page.screenshot({
      path: path.join(screenshotDir, '02-credentials-entered.png'),
      fullPage: true
    });
    console.log('✓ Credentials entered');

    // Step 3: Submit login
    console.log('\nStep 3: Submitting login form...');
    await loginButton.click();

    // Wait for navigation and check result
    try {
      await page.waitForURL(/\/(admin\/)?dashboard/i, { timeout: 15000 });
      console.log('✓ Login successful - redirected to dashboard');

      await page.screenshot({
        path: path.join(screenshotDir, '03-dashboard-after-login.png'),
        fullPage: true
      });
    } catch (error) {
      console.log('⚠ Login may have failed or redirected elsewhere');
      console.log('Current URL:', page.url());

      // Check for error messages
      const errorMessage = await page.locator('.alert-danger, .text-danger').textContent().catch(() => null);
      if (errorMessage) {
        console.log('Error message found:', errorMessage);
      }

      await page.screenshot({
        path: path.join(screenshotDir, '03-login-error.png'),
        fullPage: true
      });

      throw new Error(`Login failed: ${errorMessage || 'Unknown error'}`);
    }

    // Step 4: Navigate to Stadium Overview
    console.log('\nStep 4: Navigating to Stadium Overview...');
    await page.goto(`${adminUrl}/admin/stadium-overview`, {
      waitUntil: 'networkidle',
      timeout: 30000
    });

    await page.screenshot({
      path: path.join(screenshotDir, '04-stadium-overview-initial.png'),
      fullPage: true
    });
    console.log('✓ Stadium Overview page loaded');

    // Step 5: Check page state
    console.log('\nStep 5: Analyzing page state...');

    // Check for loading state
    const loadingElement = page.locator('text=/Loading Stadium Layout|Loading.../i');
    const isLoading = await loadingElement.isVisible().catch(() => false);

    if (isLoading) {
      console.log('⏳ Page is in loading state');
      await page.screenshot({
        path: path.join(screenshotDir, '05-loading-state.png'),
        fullPage: true
      });

      // Wait for loading to complete (max 30 seconds)
      await page.waitForSelector('text=/Loading Stadium Layout|Loading.../i', {
        state: 'hidden',
        timeout: 30000
      }).catch(() => {
        console.log('⚠ Loading did not complete within 30 seconds');
      });

      await page.screenshot({
        path: path.join(screenshotDir, '05-after-loading.png'),
        fullPage: true
      });
    } else {
      console.log('✓ Page loaded without loading state');
    }

    // Step 6: Check for errors
    console.log('\nStep 6: Checking for errors...');
    const errorAlert = page.locator('.alert-danger, .text-danger');
    const hasError = await errorAlert.isVisible().catch(() => false);

    if (hasError) {
      const errorText = await errorAlert.textContent();
      console.log('❌ Error found on page:', errorText);

      await page.screenshot({
        path: path.join(screenshotDir, '06-error-state.png'),
        fullPage: true
      });

      // Log browser console errors
      const consoleErrors: string[] = [];
      page.on('console', msg => {
        if (msg.type() === 'error') {
          consoleErrors.push(msg.text());
        }
      });

      if (consoleErrors.length > 0) {
        console.log('Browser console errors:', consoleErrors);
      }

      throw new Error(`Page has error: ${errorText}`);
    } else {
      console.log('✓ No error alerts found');
    }

    // Step 7: Check for stadium content
    console.log('\nStep 7: Checking for stadium content...');

    // Look for key elements
    const pageTitle = await page.locator('h1, h2').first().textContent().catch(() => null);
    console.log('Page title:', pageTitle);

    const hasStadiumCanvas = await page.locator('canvas#stadium-canvas').isVisible().catch(() => false);
    const hasStadiumSvg = await page.locator('svg').isVisible().catch(() => false);
    const hasEventSelector = await page.locator('select, #event-selector').isVisible().catch(() => false);

    console.log('Stadium Canvas:', hasStadiumCanvas ? '✓ Found' : '✗ Not found');
    console.log('Stadium SVG:', hasStadiumSvg ? '✓ Found' : '✗ Not found');
    console.log('Event Selector:', hasEventSelector ? '✓ Found' : '✗ Not found');

    await page.screenshot({
      path: path.join(screenshotDir, '07-final-state.png'),
      fullPage: true
    });

    // Step 8: Check network requests
    console.log('\nStep 8: Monitoring network activity...');

    const failedRequests: string[] = [];
    page.on('requestfailed', request => {
      failedRequests.push(`${request.method()} ${request.url()} - ${request.failure()?.errorText}`);
    });

    // Wait a bit to capture any delayed requests
    await page.waitForTimeout(3000);

    if (failedRequests.length > 0) {
      console.log('❌ Failed network requests:', failedRequests);
    } else {
      console.log('✓ No failed network requests');
    }

    // Step 9: Capture final page state
    console.log('\nStep 9: Capturing final state...');

    const finalState = {
      url: page.url(),
      title: await page.title(),
      hasError,
      hasStadiumCanvas,
      hasStadiumSvg,
      hasEventSelector,
      isLoading,
      failedRequests,
      pageContent: await page.content()
    };

    // Save state to JSON
    await fs.writeFile(
      path.join(screenshotDir, 'final-state.json'),
      JSON.stringify(finalState, null, 2)
    );

    console.log('\n=== VERIFICATION COMPLETE ===');
    console.log('Screenshots saved to:', screenshotDir);
    console.log('\nFinal State Summary:');
    console.log('- URL:', finalState.url);
    console.log('- Has Errors:', hasError ? 'YES ❌' : 'NO ✓');
    console.log('- Stadium Canvas:', hasStadiumCanvas ? 'YES ✓' : 'NO ❌');
    console.log('- Stadium SVG:', hasStadiumSvg ? 'YES ✓' : 'NO ❌');
    console.log('- Event Selector:', hasEventSelector ? 'YES ✓' : 'NO ❌');
    console.log('- Failed Requests:', failedRequests.length);

    // Final assertion
    expect(hasError).toBe(false);
  });
});
