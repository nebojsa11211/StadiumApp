import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for local development testing
 * Uses running local services instead of Docker
 */
export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,
  reporter: [['list']],

  use: {
    baseURL: 'https://localhost:7030', // Admin application
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    ignoreHTTPSErrors: true,
    actionTimeout: 30000,
    timeout: 180000, // Extended timeout for performance testing
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

  // No webServer configuration - assume services are already running
  outputDir: 'test-results/',

  expect: {
    timeout: 15000
  },
});