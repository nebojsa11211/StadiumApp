import { Page } from '@playwright/test';
import { testConfig } from '../config';

/**
 * Wait for Blazor Server to be fully loaded and connected
 */
export async function waitForBlazorLoad(page: Page, timeout: number = testConfig.timeouts.blazorLoad) {
  try {
    // First, wait for the loading spinner to disappear
    await page.waitForFunction(() => {
      const loadingText = document.body.textContent || '';
      return !loadingText.includes('Initializing application...') && 
             !loadingText.includes('Loading...');
    }, { timeout });
    
    // Wait for Blazor to be defined
    await page.waitForFunction(() => {
      return typeof (window as any).Blazor !== 'undefined';
    }, { timeout: 10000 }).catch(() => {
      console.log('Blazor object not found, continuing...');
    });

    // Wait for SignalR connection if available
    await page.waitForFunction(() => {
      const connection = (window as any).connection;
      return !connection || connection.state === 'Connected';
    }, { timeout: 10000 }).catch(() => {
      console.log('SignalR connection not established, continuing...');
    });
    
    // Give a small buffer for final rendering
    await page.waitForTimeout(2000);
  } catch (error) {
    console.log('Blazor loading detection failed, continuing...');
  }
}

/**
 * Wait for SignalR connection to be established
 */
export async function waitForSignalRConnection(page: Page, timeout: number = 10000) {
  try {
    await page.waitForFunction(() => {
      const connection = (window as any).connection;
      return connection && connection.state === 'Connected';
    }, { timeout });
  } catch (error) {
    console.log('SignalR connection not established, continuing...');
  }
}

/**
 * Wait for page to be fully interactive
 */
export async function waitForPageInteractive(page: Page) {
  // Wait for network to be idle
  await page.waitForLoadState('networkidle', { timeout: testConfig.timeouts.navigation });
  
  // Wait for DOM to be ready
  await page.waitForLoadState('domcontentloaded');
  
  // Try to wait for Blazor if it's available
  await waitForBlazorLoad(page);
}

/**
 * Retry an action with exponential backoff
 */
export async function retryAction<T>(
  action: () => Promise<T>,
  maxRetries: number = 3,
  initialDelay: number = 1000
): Promise<T> {
  let lastError: Error | undefined;
  
  for (let i = 0; i < maxRetries; i++) {
    try {
      return await action();
    } catch (error) {
      lastError = error as Error;
      if (i < maxRetries - 1) {
        const delay = initialDelay * Math.pow(2, i);
        console.log(`Retry ${i + 1}/${maxRetries} after ${delay}ms...`);
        await new Promise(resolve => setTimeout(resolve, delay));
      }
    }
  }
  
  throw lastError || new Error('Action failed after retries');
}

/**
 * Wait for element to be stable (not changing)
 */
export async function waitForElementStable(
  page: Page,
  selector: string,
  timeout: number = 5000
) {
  const element = page.locator(selector);
  await element.waitFor({ state: 'visible', timeout });
  
  // Wait for the element's text content to be stable
  let previousText = '';
  let stableCount = 0;
  const checkInterval = 100;
  const checksNeeded = 3;
  
  while (stableCount < checksNeeded) {
    const currentText = await element.textContent() || '';
    if (currentText === previousText) {
      stableCount++;
    } else {
      stableCount = 0;
      previousText = currentText;
    }
    await page.waitForTimeout(checkInterval);
  }
}

/**
 * Safe navigation with Blazor awareness
 */
export async function navigateToPage(page: Page, url: string) {
  await page.goto(url);
  await waitForPageInteractive(page);
}

/**
 * Click with retry for Blazor Server apps
 */
export async function clickWithRetry(
  page: Page,
  selector: string,
  options?: { timeout?: number; retries?: number }
) {
  const { timeout = 5000, retries = 3 } = options || {};
  
  return retryAction(async () => {
    const element = page.locator(selector);
    await element.waitFor({ state: 'visible', timeout });
    await element.click();
  }, retries);
}

/**
 * Fill form field with retry
 */
export async function fillWithRetry(
  page: Page,
  selector: string,
  value: string,
  options?: { timeout?: number; retries?: number }
) {
  const { timeout = 5000, retries = 3 } = options || {};
  
  return retryAction(async () => {
    const element = page.locator(selector);
    await element.waitFor({ state: 'visible', timeout });
    await element.fill(value);
  }, retries);
}