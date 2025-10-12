import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  testMatch: '**/admin-complete-polygon-test.spec.ts',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,
  reporter: [
    ['html', { outputFolder: 'playwright-report-complete-polygon' }],
    ['list'],
    ['json', { outputFile: 'test-results-complete-polygon/results.json' }]
  ],
  use: {
    baseURL: 'https://localhost:7030',
    trace: 'on',
    screenshot: 'on',
    video: 'on',
    ignoreHTTPSErrors: true,
    actionTimeout: 15000,
    navigationTimeout: 30000,
  },
  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1600, height: 900 },
      },
    },
  ],
  outputDir: 'test-results-complete-polygon/',
});
