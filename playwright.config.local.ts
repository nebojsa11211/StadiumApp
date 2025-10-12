import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for local testing without Docker webServer
 */
export default defineConfig({
  testDir: './tests',
  fullyParallel: false,
  retries: 0,
  workers: 1,
  reporter: [
    ['html', { outputFolder: 'playwright-report' }],
    ['list']
  ],
  use: {
    baseURL: 'https://localhost:7030',
    trace: 'on-first-retry',
    screenshot: 'on',
    video: 'retain-on-failure',
    ignoreHTTPSErrors: true,
    actionTimeout: 30000,
    navigationTimeout: 30000,
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  outputDir: 'test-results/',
  expect: {
    timeout: 10000
  },
});
