import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 1,
  workers: process.env.CI ? 1 : 3, // Reduce workers for stability
  reporter: 'html',
  // Global timeout settings for Blazor Server apps
  timeout: 90000, // 90 seconds for entire test
  expect: {
    timeout: 15000 // 15 seconds for assertions
  },
  use: {
    baseURL: 'http://localhost:9020',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    // Ignore SSL certificate errors for local development testing
    ignoreHTTPSErrors: true,
    // Extended timeouts for Blazor Server
    actionTimeout: 30000, // 30 seconds for actions like click, fill
    navigationTimeout: 45000, // 45 seconds for navigation
    // Wait for network idle after navigation
    waitForLoadState: 'networkidle',
  },
  projects: [
    {
      name: 'chromium',
      use: { 
        ...devices['Desktop Chrome'],
        // Browser-specific settings for Blazor
        launchOptions: {
          args: [
            '--disable-web-security',
            '--disable-features=VizDisplayCompositor',
            '--no-sandbox'
          ]
        },
        // Increase viewport timeout
        viewport: { width: 1280, height: 720 },
      },
    },
    // Stadium Viewer specific test configuration
    {
      name: 'stadium-viewer-desktop',
      testMatch: '**/stadium-viewer.spec.ts',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1920, height: 1080 },
        // Stadium viewer specific environment variables
        extraHTTPHeaders: {
          'Accept': 'application/json, text/html',
        },
      },
    },
    {
      name: 'stadium-viewer-tablet',
      testMatch: '**/stadium-viewer.spec.ts',
      use: {
        ...devices['iPad Pro'],
        viewport: { width: 1024, height: 768 },
      },
    },
    {
      name: 'stadium-viewer-mobile',
      testMatch: '**/stadium-viewer.spec.ts',
      use: {
        ...devices['iPhone 12'],
        viewport: { width: 390, height: 844 },
      },
    },
  ],
  // Remove webServer since containers are already running
});
