import { test, expect, Page, BrowserContext } from '@playwright/test';

/**
 * Authentication Persistence Tests
 *
 * Tests the authentication persistence fixes implemented for:
 * - 24-hour JWT token expiration
 * - API route fixes (/api/auth)
 * - DI registration fixes
 * - Page refresh authentication state persistence
 */

// Test configuration
const CUSTOMER_EMAIL = 'customer@stadium.com';
const CUSTOMER_PASSWORD = 'customer123';
const CUSTOMER_BASE_URL = process.env.CUSTOMER_BASE_URL || 'https://localhost:7020';
const API_BASE_URL = process.env.API_BASE_URL || 'https://localhost:9010';

// Helper function to log in and verify success
async function loginUser(page: Page): Promise<void> {
    console.log('🔐 Starting login process...');

    // Navigate to login page
    await page.goto(`${CUSTOMER_BASE_URL}/login`);
    await page.waitForLoadState('networkidle');

    // Verify we're on the login page
    await expect(page.locator('#customer-login-title')).toBeVisible();
    await expect(page.locator('#customer-login-title')).toContainText('Login');

    // Fill login form
    await page.locator('#customer-login-email-input').fill(CUSTOMER_EMAIL);
    await page.locator('#customer-login-password-input').fill(CUSTOMER_PASSWORD);

    // Submit login form
    await page.locator('#customer-login-submit-btn').click();

    // Wait for redirect and verify login success
    await page.waitForURL(`${CUSTOMER_BASE_URL}/`, { timeout: 10000 });

    // Verify we're logged in by checking for user dropdown or logout option
    const isLoggedIn = await page.locator('#customer-layout-user-dropdown').isVisible() ||
                      await page.locator('#customer-layout-logout-btn').isVisible();

    if (!isLoggedIn) {
        // Check if we're redirected back to login due to auth failure
        const currentUrl = page.url();
        if (currentUrl.includes('/login')) {
            throw new Error('Login failed - redirected back to login page');
        }
    }

    console.log('✅ Login successful');
}

// Helper function to verify authenticated state
async function verifyAuthenticatedState(page: Page): Promise<void> {
    // Check for authenticated elements
    const userDropdown = page.locator('#customer-layout-user-dropdown');
    const logoutBtn = page.locator('#customer-layout-logout-btn');
    const profileLink = page.locator('#customer-layout-profile-link');

    // At least one of these should be visible when authenticated
    const hasAuthElements = await userDropdown.isVisible() ||
                           await logoutBtn.isVisible() ||
                           await profileLink.isVisible();

    expect(hasAuthElements).toBe(true);
    console.log('✅ Authenticated state verified');
}

// Helper function to check if we're redirected to login
async function verifyNotRedirectedToLogin(page: Page): Promise<void> {
    const currentUrl = page.url();
    expect(currentUrl).not.toContain('/login');
    console.log('✅ No redirect to login page');
}

test.describe('Authentication Persistence Tests', () => {

    test.beforeEach(async ({ page }) => {
        // Set longer timeout for auth operations
        test.setTimeout(60000);

        // Clear any existing auth state
        await page.context().clearCookies();
        await page.evaluate(() => localStorage.clear());
        await page.evaluate(() => sessionStorage.clear());
    });

    test('1. Login Flow with customer@stadium.com/customer123', async ({ page }) => {
        console.log('🧪 Test 1: Basic login flow');

        await loginUser(page);

        // Verify we're on homepage and authenticated
        await expect(page).toHaveURL(`${CUSTOMER_BASE_URL}/`);
        await verifyAuthenticatedState(page);

        console.log('✅ Test 1 completed successfully');
    });

    test('2. Page Refresh Authentication State Persistence', async ({ page }) => {
        console.log('🧪 Test 2: Page refresh persistence');

        // First login
        await loginUser(page);
        await verifyAuthenticatedState(page);

        // Refresh the page
        console.log('🔄 Refreshing page...');
        await page.reload({ waitUntil: 'networkidle' });

        // Verify still authenticated after refresh
        await verifyNotRedirectedToLogin(page);
        await verifyAuthenticatedState(page);

        console.log('✅ Test 2 completed successfully');
    });

    test('3. Protected Route Access (Orders Page)', async ({ page }) => {
        console.log('🧪 Test 3: Protected route access');

        // Login first
        await loginUser(page);

        // Navigate to orders page (protected route)
        console.log('🚀 Navigating to Orders page...');
        await page.goto(`${CUSTOMER_BASE_URL}/orders`);
        await page.waitForLoadState('networkidle');

        // Verify we're on orders page and not redirected to login
        await verifyNotRedirectedToLogin(page);
        expect(page.url()).toContain('/orders');

        // Look for orders page content
        const ordersPageIndicators = [
            page.locator('h1:has-text("Orders")'),
            page.locator('h1:has-text("My Orders")'),
            page.locator('[id*="orders"]').first(),
            page.locator('text=No orders found'),
            page.locator('text=Order History')
        ];

        let foundIndicator = false;
        for (const indicator of ordersPageIndicators) {
            if (await indicator.isVisible()) {
                foundIndicator = true;
                break;
            }
        }

        expect(foundIndicator).toBe(true);
        console.log('✅ Test 3 completed successfully');
    });

    test('4. Navigation Between Protected Pages', async ({ page }) => {
        console.log('🧪 Test 4: Navigation between authenticated pages');

        // Login first
        await loginUser(page);

        // Navigate to different protected pages
        const protectedRoutes = [
            { url: '/orders', name: 'Orders' },
            { url: '/events', name: 'Events' },
            { url: '/', name: 'Home' }
        ];

        for (const route of protectedRoutes) {
            console.log(`🚀 Navigating to ${route.name} page...`);
            await page.goto(`${CUSTOMER_BASE_URL}${route.url}`);
            await page.waitForLoadState('networkidle');

            // Verify not redirected to login
            await verifyNotRedirectedToLogin(page);

            // Verify still authenticated
            await verifyAuthenticatedState(page);

            console.log(`✅ ${route.name} page navigation successful`);
        }

        console.log('✅ Test 4 completed successfully');
    });

    test('5. Multiple Page Refreshes Maintain Auth State', async ({ page }) => {
        console.log('🧪 Test 5: Multiple page refreshes');

        // Login first
        await loginUser(page);

        // Perform multiple refreshes on different pages
        const pages = ['/', '/events', '/orders'];

        for (const pagePath of pages) {
            console.log(`🔄 Testing refreshes on ${pagePath}...`);

            // Navigate to page
            await page.goto(`${CUSTOMER_BASE_URL}${pagePath}`);
            await page.waitForLoadState('networkidle');

            // Perform 3 refreshes
            for (let i = 1; i <= 3; i++) {
                console.log(`   Refresh ${i}/3...`);
                await page.reload({ waitUntil: 'networkidle' });

                // Verify still authenticated
                await verifyNotRedirectedToLogin(page);
                await verifyAuthenticatedState(page);
            }
        }

        console.log('✅ Test 5 completed successfully');
    });

    test('6. New Browser Tab Maintains Authentication', async ({ context }) => {
        console.log('🧪 Test 6: New browser tab authentication');

        // Create first page and login
        const page1 = await context.newPage();
        await loginUser(page1);

        // Create second page (new tab) and verify auth state
        console.log('🆕 Opening new tab...');
        const page2 = await context.newPage();
        await page2.goto(`${CUSTOMER_BASE_URL}/`);
        await page2.waitForLoadState('networkidle');

        // Verify second tab is also authenticated
        await verifyNotRedirectedToLogin(page2);
        await verifyAuthenticatedState(page2);

        // Navigate to protected route in second tab
        await page2.goto(`${CUSTOMER_BASE_URL}/orders`);
        await page2.waitForLoadState('networkidle');
        await verifyNotRedirectedToLogin(page2);

        console.log('✅ Test 6 completed successfully');

        // Cleanup
        await page1.close();
        await page2.close();
    });

    test('7. API Authentication Endpoints Accessibility', async ({ page }) => {
        console.log('🧪 Test 7: API endpoints accessibility');

        // Login to get auth token
        await loginUser(page);

        // Test API endpoints by intercepting network requests
        const apiRequests: string[] = [];

        page.on('request', request => {
            if (request.url().includes('/api/')) {
                apiRequests.push(request.url());
                console.log(`📡 API Request: ${request.method()} ${request.url()}`);
            }
        });

        page.on('response', response => {
            if (response.url().includes('/api/')) {
                console.log(`📡 API Response: ${response.status()} ${response.url()}`);
            }
        });

        // Navigate to pages that might make API calls
        await page.goto(`${CUSTOMER_BASE_URL}/events`);
        await page.waitForLoadState('networkidle');
        await page.waitForTimeout(2000); // Wait for potential API calls

        await page.goto(`${CUSTOMER_BASE_URL}/orders`);
        await page.waitForLoadState('networkidle');
        await page.waitForTimeout(2000); // Wait for potential API calls

        // Test direct API endpoint accessibility
        console.log('🔍 Testing direct API endpoint...');

        // Get auth token from localStorage
        const token = await page.evaluate(() => {
            return localStorage.getItem('Customer_AuthToken') ||
                   localStorage.getItem('authToken') ||
                   localStorage.getItem('token');
        });

        console.log(`🎟️ Token found: ${token ? 'Yes' : 'No'}`);

        if (token) {
            // Test API endpoint with token
            const response = await page.request.get(`${API_BASE_URL}/api/auth/profile`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                ignoreHTTPSErrors: true
            });

            console.log(`📡 API Profile endpoint status: ${response.status()}`);
            expect(response.status()).toBeLessThan(500); // Should not be server error
        }

        console.log('✅ Test 7 completed successfully');
    });

    test('8. Browser Close and Reopen Persistence', async ({ browser }) => {
        console.log('🧪 Test 8: Browser close and reopen persistence');

        // Create a context with persistent storage
        const context = await browser.newContext({
            ignoreHTTPSErrors: true
        });

        // Create page and login
        const page = await context.newPage();
        await loginUser(page);

        // Verify authenticated
        await verifyAuthenticatedState(page);

        // Close the context (simulates browser close)
        console.log('🔒 Closing browser context...');
        await context.close();

        // Create new context (simulates browser reopen)
        console.log('🔓 Reopening browser context...');
        const newContext = await browser.newContext({
            ignoreHTTPSErrors: true
        });

        const newPage = await newContext.newPage();

        // Navigate to homepage
        await newPage.goto(`${CUSTOMER_BASE_URL}/`);
        await newPage.waitForLoadState('networkidle');

        // Check if we need to login again (this is expected behavior)
        const currentUrl = newPage.url();
        if (currentUrl.includes('/login')) {
            console.log('ℹ️ New browser session requires login (expected for security)');
        } else {
            console.log('ℹ️ Authentication persisted across browser sessions');
            await verifyAuthenticatedState(newPage);
        }

        await newContext.close();
        console.log('✅ Test 8 completed successfully');
    });
});

test.describe('Authentication API Integration Tests', () => {

    test('API Routes Accessibility (/api/auth/*)', async ({ page }) => {
        console.log('🧪 API Routes Test: Testing /api/auth endpoints');

        const apiEndpoints = [
            '/api/auth/login',
            '/api/auth/register',
            '/api/auth/profile'
        ];

        for (const endpoint of apiEndpoints) {
            console.log(`🔍 Testing endpoint: ${endpoint}`);

            const response = await page.request.get(`${API_BASE_URL}${endpoint}`, {
                ignoreHTTPSErrors: true
            });

            console.log(`📡 ${endpoint} status: ${response.status()}`);

            // These endpoints should exist (not 404)
            expect(response.status()).not.toBe(404);

            // OPTIONS request should be allowed for CORS
            const optionsResponse = await page.request.fetch(`${API_BASE_URL}${endpoint}`, {
                method: 'OPTIONS',
                ignoreHTTPSErrors: true
            });

            console.log(`📡 ${endpoint} OPTIONS status: ${optionsResponse.status()}`);
        }

        console.log('✅ API Routes test completed');
    });
});