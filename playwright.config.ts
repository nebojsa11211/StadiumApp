import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for Stadium Admin modernization testing
 * Supports both Docker (https://localhost:9030) and Local (https://localhost:7030) environments
 */
export default defineConfig({
  testDir: './tests',
  /* Run tests in files in parallel */
  fullyParallel: true,
  /* Fail the build on CI if you accidentally left test.only in the source code. */
  forbidOnly: !!process.env.CI,
  /* Retry on CI only */
  retries: process.env.CI ? 2 : 0,
  /* Opt out of parallel tests on CI. */
  workers: process.env.CI ? 1 : undefined,
  /* Reporter to use. See https://playwright.dev/docs/test-reporters */
  reporter: [
    ['html', { outputFolder: 'playwright-report' }],
    ['junit', { outputFile: 'test-results/junit.xml' }],
    ['list']
  ],
  /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
  use: {
    /* Base URL to use in actions like `await page.goto('/')`. */
    baseURL: process.env.CUSTOMER_BASE_URL || 'https://localhost:7020',

    /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry',

    /* Take screenshot on failure */
    screenshot: 'only-on-failure',

    /* Record video on failure */
    video: 'retain-on-failure',

    /* Ignore HTTPS errors for self-signed certificates */
    ignoreHTTPSErrors: true,

    /* Global timeout for each action */
    actionTimeout: 30000,

    /* Global timeout for each test */
    timeout: 120000,

    /* Context options */
    contextOptions: {
      // Accept self-signed certificates
      ignoreHTTPSErrors: true,
    }
  },

  /* Configure projects for major browsers */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },

    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },

    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },

    /* Test against mobile viewports. */
    {
      name: 'Mobile Chrome',
      use: { ...devices['Pixel 5'] },
    },
    {
      name: 'Mobile Safari',
      use: { ...devices['iPhone 12'] },
    },

    /* Test against branded browsers. */
    {
      name: 'Microsoft Edge',
      use: { ...devices['Desktop Edge'], channel: 'msedge' },
    },
    {
      name: 'Google Chrome',
      use: { ...devices['Desktop Chrome'], channel: 'chrome' },
    },
  ],

  /* Run your local dev server before starting the tests */
  webServer: [
    {
      command: 'docker-compose up -d --wait',
      port: 9030,
      reuseExistingServer: !process.env.CI,
      ignoreHTTPSErrors: true,
      timeout: 180000, // 3 minutes for Docker containers to start
    }
  ],

  /* Test output directory */
  outputDir: 'test-results/',

  /* Global setup and teardown - commented out for initial testing */
  // globalSetup: require.resolve('./tests/global-setup.ts'),
  // globalTeardown: require.resolve('./tests/global-teardown.ts'),

  /* Expect timeout */
  expect: {
    timeout: 10000
  },
});