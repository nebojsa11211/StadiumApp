import { Page, BrowserContext, APIRequestContext, expect } from '@playwright/test';

/**
 * Authentication Security Test Helpers
 *
 * Specialized helpers for testing the refactored authentication system
 * including security enhancements, token management, and authorization policies
 */

export interface TokenInfo {
  accessToken: string;
  refreshToken: string;
  expiresAt?: number;
}

export interface SecurityTestResult {
  success: boolean;
  statusCode?: number;
  hasSecurityHeaders?: boolean;
  responseTime?: number;
  errorMessage?: string;
}

export const API_BASE_URL = 'https://localhost:9010';
export const ADMIN_BASE_URL = 'https://localhost:9030';

/**
 * Extracts authentication tokens from browser storage
 */
export async function extractAuthTokens(page: Page): Promise<TokenInfo | null> {
  try {
    const tokens = await page.evaluate(() => {
      // Try localStorage first
      const accessToken = localStorage.getItem('accessToken');
      const refreshToken = localStorage.getItem('refreshToken');

      if (accessToken && refreshToken) {
        return { accessToken, refreshToken };
      }

      // Try cookies as fallback
      const cookieAccessToken = document.cookie
        .split('; ')
        .find(row => row.startsWith('accessToken='))
        ?.split('=')[1];

      const cookieRefreshToken = document.cookie
        .split('; ')
        .find(row => row.startsWith('refreshToken='))
        ?.split('=')[1];

      if (cookieAccessToken && cookieRefreshToken) {
        return {
          accessToken: cookieAccessToken,
          refreshToken: cookieRefreshToken
        };
      }

      return null;
    });

    return tokens;
  } catch (error) {
    console.warn('Failed to extract auth tokens:', error.message);
    return null;
  }
}

/**
 * Tests JWT token refresh functionality
 */
export async function testTokenRefresh(api: APIRequestContext, refreshToken: string): Promise<SecurityTestResult> {
  try {
    const startTime = Date.now();

    const response = await api.post('/api/auth/refresh', {
      data: { refreshToken },
      ignoreHTTPSErrors: true
    });

    const endTime = Date.now();
    const responseTime = endTime - startTime;

    if (response.status() === 200) {
      const data = await response.json();
      return {
        success: true,
        statusCode: response.status(),
        responseTime,
        hasSecurityHeaders: hasSecurityHeaders(response.headers())
      };
    }

    return {
      success: false,
      statusCode: response.status(),
      responseTime,
      errorMessage: `Token refresh failed with status ${response.status()}`
    };

  } catch (error) {
    return {
      success: false,
      errorMessage: error.message
    };
  }
}

/**
 * Tests API endpoint authorization with provided token
 */
export async function testEndpointAuthorization(
  api: APIRequestContext,
  endpoint: string,
  token: string,
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' = 'GET',
  payload?: any
): Promise<SecurityTestResult> {
  try {
    const startTime = Date.now();

    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    let response;
    switch (method) {
      case 'POST':
        response = await api.post(endpoint, { data: payload, headers, ignoreHTTPSErrors: true });
        break;
      case 'PUT':
        response = await api.put(endpoint, { data: payload, headers, ignoreHTTPSErrors: true });
        break;
      case 'DELETE':
        response = await api.delete(endpoint, { headers, ignoreHTTPSErrors: true });
        break;
      default:
        response = await api.get(endpoint, { headers, ignoreHTTPSErrors: true });
    }

    const endTime = Date.now();
    const responseTime = endTime - startTime;

    return {
      success: response.status() < 400,
      statusCode: response.status(),
      responseTime,
      hasSecurityHeaders: hasSecurityHeaders(response.headers())
    };

  } catch (error) {
    return {
      success: false,
      errorMessage: error.message
    };
  }
}

/**
 * Tests rate limiting on authentication endpoints
 */
export async function testRateLimit(
  api: APIRequestContext,
  endpoint: string,
  payload: any,
  maxAttempts: number = 15
): Promise<{ triggered: boolean; attemptsBeforeTrigger: number; finalStatusCode: number }> {
  let rateLimitTriggered = false;
  let attemptsBeforeTrigger = 0;
  let finalStatusCode = 0;

  for (let i = 0; i < maxAttempts; i++) {
    try {
      const response = await api.post(endpoint, {
        data: payload,
        ignoreHTTPSErrors: true,
        timeout: 5000
      });

      finalStatusCode = response.status();

      if (response.status() === 429) {
        rateLimitTriggered = true;
        attemptsBeforeTrigger = i + 1;
        break;
      }

      // Small delay between requests
      await new Promise(resolve => setTimeout(resolve, 100));

    } catch (error) {
      console.log(`Rate limit test request ${i + 1} failed: ${error.message}`);
    }
  }

  return {
    triggered: rateLimitTriggered,
    attemptsBeforeTrigger,
    finalStatusCode
  };
}

/**
 * Checks if response includes required security headers
 */
export function hasSecurityHeaders(headers: Record<string, string>): boolean {
  const requiredHeaders = [
    'x-content-type-options',
    'x-frame-options',
    'x-xss-protection'
  ];

  return requiredHeaders.every(header =>
    headers[header] || headers[header.toLowerCase()]
  );
}

/**
 * Validates security header values
 */
export function validateSecurityHeaders(headers: Record<string, string>): Record<string, boolean> {
  return {
    'x-content-type-options': headers['x-content-type-options'] === 'nosniff',
    'x-frame-options': ['DENY', 'SAMEORIGIN'].includes(headers['x-frame-options']),
    'x-xss-protection': headers['x-xss-protection'] === '1; mode=block',
    'referrer-policy': !!headers['referrer-policy']
  };
}

/**
 * Tests authentication flow end-to-end
 */
export async function testAuthenticationFlow(
  page: Page,
  credentials: { email: string; password: string },
  expectedSuccessUrl: string = '/dashboard'
): Promise<{ success: boolean; tokens?: TokenInfo; errorMessage?: string }> {
  try {
    // Navigate to login
    await page.goto('/login');

    // Fill credentials
    await page.fill('input[name="Email"]', credentials.email);
    await page.fill('input[name="Password"]', credentials.password);

    // Submit form
    await page.click('button[type="submit"]');

    // Wait for navigation
    await page.waitForLoadState('networkidle', { timeout: 15000 });

    // Check if login was successful
    if (page.url().includes(expectedSuccessUrl)) {
      const tokens = await extractAuthTokens(page);
      return {
        success: true,
        tokens: tokens || undefined
      };
    } else {
      return {
        success: false,
        errorMessage: `Login failed - redirected to ${page.url()}`
      };
    }

  } catch (error) {
    return {
      success: false,
      errorMessage: error.message
    };
  }
}

/**
 * Tests authorization policies across different roles
 */
export async function testRoleBasedAccess(
  api: APIRequestContext,
  endpoints: { endpoint: string; allowedRoles: string[] }[],
  userTokensByRole: Record<string, string>
): Promise<Record<string, Record<string, SecurityTestResult>>> {
  const results: Record<string, Record<string, SecurityTestResult>> = {};

  for (const { endpoint, allowedRoles } of endpoints) {
    results[endpoint] = {};

    for (const [role, token] of Object.entries(userTokensByRole)) {
      const shouldAllow = allowedRoles.includes(role);
      const result = await testEndpointAuthorization(api, endpoint, token);

      results[endpoint][role] = {
        ...result,
        success: shouldAllow ? result.success : !result.success
      };
    }
  }

  return results;
}

/**
 * Tests session persistence across page refreshes
 */
export async function testSessionPersistence(page: Page): Promise<boolean> {
  try {
    // Verify authenticated before refresh
    const beforeUrl = page.url();
    const beforeTokens = await extractAuthTokens(page);

    if (!beforeTokens?.accessToken) {
      return false;
    }

    // Refresh page
    await page.reload();
    await page.waitForLoadState('networkidle');

    // Check if still authenticated
    const afterUrl = page.url();
    const afterTokens = await extractAuthTokens(page);

    // Should stay on same page and have tokens
    return !afterUrl.includes('/login') &&
           !!afterTokens?.accessToken &&
           afterUrl === beforeUrl;

  } catch (error) {
    console.warn('Session persistence test failed:', error.message);
    return false;
  }
}

/**
 * Tests automatic token injection in API requests
 */
export async function testAutomaticTokenInjection(page: Page): Promise<boolean> {
  let tokenInjected = false;

  // Set up request interception
  await page.route('**/api/**', (route, request) => {
    const authHeader = request.headers()['authorization'];
    if (authHeader && authHeader.startsWith('Bearer ')) {
      tokenInjected = true;
    }
    route.continue();
  });

  try {
    // Navigate to a page that makes API calls
    await page.goto('/orders');
    await page.waitForLoadState('networkidle');

    return tokenInjected;

  } catch (error) {
    console.warn('Token injection test failed:', error.message);
    return false;
  }
}

/**
 * Tests concurrent authentication sessions
 */
export async function testConcurrentSessions(
  browser: any,
  credentials: { email: string; password: string },
  sessionCount: number = 3
): Promise<boolean[]> {
  const contexts: BrowserContext[] = [];
  const results: boolean[] = [];

  try {
    // Create multiple browser contexts
    for (let i = 0; i < sessionCount; i++) {
      const context = await browser.newContext({ ignoreHTTPSErrors: true });
      contexts.push(context);
    }

    // Login in each context
    const loginPromises = contexts.map(async (context, index) => {
      const page = await context.newPage();
      const result = await testAuthenticationFlow(page, credentials);
      return result.success;
    });

    const loginResults = await Promise.all(loginPromises);
    results.push(...loginResults);

    return results;

  } finally {
    // Clean up contexts
    await Promise.all(contexts.map(context => context.close()));
  }
}

/**
 * Monitors authentication performance
 */
export async function measureAuthenticationPerformance(
  page: Page,
  credentials: { email: string; password: string }
): Promise<{ loginTime: number; tokenExtractionTime: number; navigationTime: number }> {
  const startTime = Date.now();

  await page.goto('/login');
  const pageLoadTime = Date.now();

  await page.fill('input[name="Email"]', credentials.email);
  await page.fill('input[name="Password"]', credentials.password);

  const submitStartTime = Date.now();
  await page.click('button[type="submit"]');
  await page.waitForLoadState('networkidle');
  const loginCompleteTime = Date.now();

  const tokenStartTime = Date.now();
  await extractAuthTokens(page);
  const tokenCompleteTime = Date.now();

  return {
    loginTime: loginCompleteTime - submitStartTime,
    tokenExtractionTime: tokenCompleteTime - tokenStartTime,
    navigationTime: pageLoadTime - startTime
  };
}

/**
 * Tests logout cleanup
 */
export async function testLogoutCleanup(page: Page): Promise<boolean> {
  try {
    // Verify authenticated state before logout
    const beforeTokens = await extractAuthTokens(page);
    if (!beforeTokens?.accessToken) {
      return false;
    }

    // Perform logout
    const logoutButton = page.locator('button:has-text("Logout"), [data-testid="logout-button"]');
    if (await logoutButton.isVisible({ timeout: 5000 })) {
      await logoutButton.click();
    } else {
      // Try alternative logout methods
      await page.click('text="Logout"');
    }

    // Wait for logout to complete
    await page.waitForLoadState('networkidle');

    // Verify cleanup
    const afterTokens = await extractAuthTokens(page);
    const isOnLoginPage = page.url().includes('/login');

    // Should have no tokens and be on login page
    return !afterTokens?.accessToken && isOnLoginPage;

  } catch (error) {
    console.warn('Logout cleanup test failed:', error.message);
    return false;
  }
}

/**
 * Validates JWT token structure
 */
export function validateJWTStructure(token: string): boolean {
  try {
    const parts = token.split('.');
    if (parts.length !== 3) {
      return false;
    }

    // Decode header and payload (basic structure check)
    const header = JSON.parse(atob(parts[0]));
    const payload = JSON.parse(atob(parts[1]));

    return !!(header.alg && header.typ && payload.sub);

  } catch (error) {
    return false;
  }
}

/**
 * Creates test context with authentication
 */
export async function createAuthenticatedTestContext(
  browser: any,
  credentials: { email: string; password: string }
): Promise<{ context: BrowserContext; page: Page; tokens: TokenInfo | null }> {
  const context = await browser.newContext({ ignoreHTTPSErrors: true });
  const page = await context.newPage();

  const authResult = await testAuthenticationFlow(page, credentials);

  if (!authResult.success) {
    await context.close();
    throw new Error(`Authentication failed: ${authResult.errorMessage}`);
  }

  return {
    context,
    page,
    tokens: authResult.tokens || null
  };
}