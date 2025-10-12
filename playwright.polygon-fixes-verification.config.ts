import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  testMatch: 'admin-polygon-fixes-verification.spec.ts',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,
  reporter: [
    ['html', { outputFolder: 'playwright-report-polygon-fixes', open: 'never' }],
    ['list']
  ],
  use: {
    trace: 'on',
    screenshot: 'on',
    video: 'on',
    ignoreHTTPSErrors: true,
    actionTimeout: 15000,
    navigationTimeout: 30000
  },
  timeout: 120000,
  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1920, height: 1080 }
      }
    }
  ],
  outputDir: 'test-results-polygon-fixes/'
});
