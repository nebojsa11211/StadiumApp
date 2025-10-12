import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for LOCAL testing (no Docker webServer)
 * Use this when running against locally running services (dotnet run)
 */
export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,
  reporter: [
    ['html', { outputFolder: 'playwright-report' }],
    ['list']
  ],
  use: {
    baseURL: 'https://localhost:7030',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    ignoreHTTPSErrors: true,
    actionTimeout: 30000,
    contextOptions: {
      ignoreHTTPSErrors: true,
    }
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  // NO webServer - expect services to be already running
  outputDir: 'test-results/',
  expect: {
    timeout: 10000
  },
});
