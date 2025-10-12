import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for local testing (no Docker startup)
 * Use this when services are already running locally on ports 7010, 7020, 7030, 7040
 */
export default defineConfig({
  testDir: './tests',
  fullyParallel: false, // Run tests sequentially for better visibility in headed mode
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1, // Single worker for headed mode visibility

  reporter: [
    ['list'],
    ['html', { outputFolder: 'playwright-report' }]
  ],

  use: {
    baseURL: 'https://localhost:7030', // Admin app for this test
    trace: 'on',
    screenshot: 'on',
    video: 'on',
    ignoreHTTPSErrors: true,
    actionTimeout: 30000,
    viewport: { width: 1920, height: 1080 },
    contextOptions: {
      ignoreHTTPSErrors: true,
    }
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    }
  ],

  outputDir: 'test-results/',

  expect: {
    timeout: 10000
  },
});
