import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright Configuration for Customer Authentication State Verification
 *
 * This config is specifically designed to test the authentication state
 * after login without full page reload (Blazor circuit persistence).
 */

export default defineConfig({
  testDir: './tests',
  testMatch: '**/customer-login-auth-state-verification.spec.ts',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: 0, // No retries - we want to see actual behavior
  workers: 1, // Single worker to avoid conflicts
  reporter: [
    ['html', { outputFolder: 'playwright-report-customer-auth', open: 'never' }],
    ['list'],
    ['json', { outputFile: 'customer-auth-test-results.json' }]
  ],
  use: {
    baseURL: 'https://localhost:8081',
    trace: 'on',
    screenshot: 'on',
    video: 'on',
    ignoreHTTPSErrors: true, // Accept self-signed certificates
    actionTimeout: 15000,
    navigationTimeout: 30000
  },
  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1920, height: 1080 }
      },
    },
  ],
  webServer: undefined // Services are already running
});
