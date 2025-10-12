import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for performance testing
 * Uses local development services (no Docker dependency)
 */
export default defineConfig({
  testDir: './tests/performance',
  fullyParallel: false, // Run performance tests serially for accurate measurements
  forbidOnly: !!process.env.CI,
  retries: 0, // No retries for performance tests
  workers: 1, // Single worker for consistent performance measurements
  reporter: [
    ['html', { outputFolder: 'performance-report' }],
    ['list']
  ],
  use: {
    /* Use local Admin service */
    baseURL: 'https://localhost:7030',

    /* Collect trace for analysis */
    trace: 'on',

    /* Take screenshot for analysis */
    screenshot: 'on',

    /* Record video for analysis */
    video: 'on',

    /* Ignore HTTPS errors for self-signed certificates */
    ignoreHTTPSErrors: true,

    /* Extended timeouts for performance testing */
    actionTimeout: 60000,
    timeout: 180000, // 3 minutes per test

    /* Context options */
    contextOptions: {
      ignoreHTTPSErrors: true,
    }
  },

  /* Single browser for consistent performance measurements */
  projects: [
    {
      name: 'chromium-performance',
      use: {
        ...devices['Desktop Chrome'],
        // Additional performance-focused options
        launchOptions: {
          args: [
            '--disable-web-security',
            '--ignore-certificate-errors',
            '--disable-features=VizDisplayCompositor',
            '--enable-precise-memory-info'
          ]
        }
      },
    },
  ],

  /* No webServer - assumes local services are already running */
  // webServer: [], // Intentionally empty

  /* Test output directory */
  outputDir: 'performance-results/',

  /* Expect timeout */
  expect: {
    timeout: 30000
  },
});