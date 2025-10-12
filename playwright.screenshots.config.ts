import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: '.',
  testMatch: 'verify-stadium-complete.spec.ts',
  timeout: 90000,
  use: {
    baseURL: 'https://localhost:7030',
    ignoreHTTPSErrors: true,
    screenshot: 'on',
    trace: 'on',
    viewport: { width: 1400, height: 900 }
  },
  reporter: [['list']],
});
