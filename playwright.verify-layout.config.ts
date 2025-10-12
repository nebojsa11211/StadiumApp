import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  testMatch: 'verify-layout-final.spec.ts',
  timeout: 60000,
  fullyParallel: false,
  retries: 0,
  workers: 1,
  reporter: 'list',
  use: {
    baseURL: 'https://localhost:8081',
    ignoreHTTPSErrors: true,
    screenshot: 'on',
    video: 'on',
    trace: 'on',
    viewport: { width: 1280, height: 720 },
  },
});
