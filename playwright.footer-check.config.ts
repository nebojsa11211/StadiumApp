import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  testMatch: 'check-footer-ui.spec.ts',
  timeout: 60000,
  use: {
    baseURL: 'https://localhost:8081',
    ignoreHTTPSErrors: true,
    screenshot: 'on',
    video: 'retain-on-failure',
  },
  reporter: [['list'], ['html', { outputFolder: 'playwright-report-footer' }]],
});
