import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright Configuration for Stadium Overview Testing
 * Tests against locally running services (no Docker)
 *
 * Prerequisites:
 * - API running on https://localhost:7010
 * - Admin running on https://localhost:7030
 * - Supabase database connected
 */

export default defineConfig({
  testDir: './tests',
  testMatch: '**/stadium-overview-database-test.spec.ts',

  // Test timeout
  timeout: 60000,
  expect: {
    timeout: 10000
  },

  // Run tests in serial to avoid conflicts
  fullyParallel: false,
  workers: 1,

  // Reporter
  reporter: [
    ['list'],
    ['html', { outputFolder: 'playwright-report-stadium', open: 'never' }]
  ],

  // Output
  outputDir: './test-results-stadium',

  use: {
    // Base URL
    baseURL: 'https://localhost:7030',

    // Browser options
    headless: false,
    viewport: { width: 1920, height: 1080 },
    ignoreHTTPSErrors: true,
    screenshot: 'on',
    video: 'retain-on-failure',
    trace: 'retain-on-failure',

    // Slow down for debugging
    launchOptions: {
      slowMo: 500
    }
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] }
    }
  ],

  // No webServer - expect services to be running already
  webServer: undefined
});
