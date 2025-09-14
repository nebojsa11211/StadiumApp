import { Page, Locator, expect } from '@playwright/test';

/**
 * Page interaction helpers for Admin modernization tests
 */

/**
 * Waits for and verifies that a page component is loaded
 */
export async function waitForComponent(page: Page, selector: string, timeout: number = 10000) {
  await page.waitForSelector(selector, { state: 'visible', timeout });
  await expect(page.locator(selector)).toBeVisible();
}

/**
 * Verifies that multiple components are present on the page
 */
export async function verifyPageComponents(page: Page, selectors: string[]) {
  for (const selector of selectors) {
    await expect(page.locator(selector)).toBeVisible({ timeout: 10000 });
  }
}

/**
 * Waits for loading states to complete
 */
export async function waitForLoadingComplete(page: Page) {
  // Wait for loading skeletons to disappear
  try {
    await page.waitForSelector('[data-testid="loading-skeleton"]', { state: 'hidden', timeout: 15000 });
  } catch {
    // Loading skeleton might not exist, continue
  }

  // Wait for spinners to disappear
  try {
    await page.waitForSelector('.spinner, .loading-spinner', { state: 'hidden', timeout: 10000 });
  } catch {
    // Spinner might not exist, continue
  }

  // Wait for network to be idle
  await page.waitForLoadState('networkidle');
}

/**
 * Fills a form with the provided data
 */
export async function fillForm(page: Page, formData: Record<string, string>) {
  for (const [field, value] of Object.entries(formData)) {
    const selector = `input[name="${field}"], select[name="${field}"], textarea[name="${field}"], [data-testid="${field}"]`;
    await page.fill(selector, value);
  }
}

/**
 * Clicks a button and waits for the action to complete
 */
export async function clickAndWait(page: Page, selector: string, waitForSelector?: string) {
  await page.click(selector);

  if (waitForSelector) {
    await page.waitForSelector(waitForSelector, { timeout: 10000 });
  } else {
    // Default wait for network idle after click
    await page.waitForLoadState('networkidle');
  }
}

/**
 * Verifies success message appears
 */
export async function verifySuccessMessage(page: Page, message?: string) {
  const successLocator = page.locator('[data-testid="success-message"], .alert-success, .toast-success');
  await expect(successLocator).toBeVisible({ timeout: 10000 });

  if (message) {
    await expect(successLocator).toContainText(message);
  }
}

/**
 * Verifies error message appears
 */
export async function verifyErrorMessage(page: Page, message?: string) {
  const errorLocator = page.locator('[data-testid="error-message"], .alert-danger, .alert-error, .toast-error');
  await expect(errorLocator).toBeVisible({ timeout: 10000 });

  if (message) {
    await expect(errorLocator).toContainText(message);
  }
}

/**
 * Selects an option from a dropdown
 */
export async function selectDropdownOption(page: Page, dropdownSelector: string, optionValue: string) {
  await page.selectOption(dropdownSelector, optionValue);
  await page.waitForTimeout(500); // Brief wait for UI update
}

/**
 * Checks multiple checkboxes
 */
export async function checkMultiple(page: Page, selectors: string[]) {
  for (const selector of selectors) {
    await page.check(selector);
  }
}

/**
 * Verifies table has data
 */
export async function verifyTableHasData(page: Page, tableSelector: string = '[data-testid="data-table"]') {
  const table = page.locator(tableSelector);
  await expect(table).toBeVisible();

  // Check for data rows (excluding header)
  const rows = table.locator('tbody tr, .table-row:not(.table-header)');
  await expect(rows.first()).toBeVisible({ timeout: 10000 });
}

/**
 * Verifies table is empty or shows no data message
 */
export async function verifyTableEmpty(page: Page, tableSelector: string = '[data-testid="data-table"]') {
  // Either no rows or "no data" message
  const noDataMessage = page.locator('[data-testid="no-data"], text="No data", text="No records found"');
  const tableRows = page.locator(`${tableSelector} tbody tr, ${tableSelector} .table-row:not(.table-header)`);

  const hasNoDataMessage = await noDataMessage.isVisible({ timeout: 5000 });
  const hasNoRows = (await tableRows.count()) === 0;

  if (!hasNoDataMessage && !hasNoRows) {
    throw new Error('Expected table to be empty or show no data message');
  }
}

/**
 * Scrolls to an element and ensures it's visible
 */
export async function scrollToElement(page: Page, selector: string) {
  const element = page.locator(selector);
  await element.scrollIntoViewIfNeeded();
  await expect(element).toBeVisible();
}

/**
 * Takes a screenshot with a descriptive name
 */
export async function takeScreenshot(page: Page, name: string) {
  await page.screenshot({ path: `test-results/screenshots/${name}.png`, fullPage: true });
}

/**
 * Verifies responsive behavior by changing viewport
 */
export async function testResponsiveBehavior(page: Page, breakpoints: { name: string; width: number; height: number }[]) {
  for (const breakpoint of breakpoints) {
    console.log(`📱 Testing ${breakpoint.name} viewport (${breakpoint.width}x${breakpoint.height})`);
    await page.setViewportSize({ width: breakpoint.width, height: breakpoint.height });
    await page.waitForTimeout(1000); // Allow UI to adapt
  }
}

/**
 * Waits for SignalR connection to be established (if applicable)
 */
export async function waitForSignalRConnection(page: Page) {
  try {
    // Wait for SignalR connection indicator or hub ready state
    await page.waitForFunction(
      () => {
        // @ts-ignore
        return window.signalRConnection && window.signalRConnection.state === 'Connected';
      },
      { timeout: 15000 }
    );
    console.log('✅ SignalR connection established');
  } catch (error) {
    console.warn('⚠️ SignalR connection not detected or timed out');
  }
}

/**
 * Verifies modal dialog behavior
 */
export async function verifyModal(page: Page, modalSelector: string, isVisible: boolean = true) {
  const modal = page.locator(modalSelector);

  if (isVisible) {
    await expect(modal).toBeVisible({ timeout: 10000 });
    // Verify modal backdrop
    await expect(page.locator('.modal-backdrop, .backdrop')).toBeVisible();
  } else {
    await expect(modal).not.toBeVisible({ timeout: 10000 });
  }
}

/**
 * Closes modal by clicking backdrop or close button
 */
export async function closeModal(page: Page, modalSelector: string) {
  // Try close button first
  const closeButton = page.locator(`${modalSelector} .close, ${modalSelector} [data-dismiss="modal"], ${modalSelector} .modal-close`);

  if (await closeButton.isVisible({ timeout: 2000 })) {
    await closeButton.click();
  } else {
    // Fallback to clicking backdrop
    await page.keyboard.press('Escape');
  }

  // Verify modal is closed
  await expect(page.locator(modalSelector)).not.toBeVisible({ timeout: 5000 });
}