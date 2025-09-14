import { chromium, FullConfig } from '@playwright/test';

/**
 * Global setup for Playwright tests
 * Ensures Docker containers are ready and admin user exists
 */
export default async function globalSetup(config: FullConfig) {
  console.log('🚀 Starting global setup for Admin modernization tests...');

  // Wait for services to be fully ready
  const browser = await chromium.launch();
  const page = await browser.newPage({ ignoreHTTPSErrors: true });

  // Health check for API service
  let apiReady = false;
  let attempts = 0;
  const maxAttempts = 30;

  while (!apiReady && attempts < maxAttempts) {
    try {
      console.log(`⏳ Checking API health (attempt ${attempts + 1}/${maxAttempts})...`);
      const response = await page.goto('https://localhost:9010/api/drinks', {
        waitUntil: 'networkidle',
        timeout: 10000
      });

      if (response && (response.status() === 405 || response.status() === 200)) {
        apiReady = true;
        console.log('✅ API service is ready');
      }
    } catch (error) {
      attempts++;
      if (attempts < maxAttempts) {
        console.log('⏳ API not ready yet, waiting 5 seconds...');
        await page.waitForTimeout(5000);
      }
    }
  }

  if (!apiReady) {
    console.error('❌ API service failed to become ready');
    throw new Error('API service is not ready after maximum attempts');
  }

  // Health check for Admin app
  let adminReady = false;
  attempts = 0;

  while (!adminReady && attempts < maxAttempts) {
    try {
      console.log(`⏳ Checking Admin app health (attempt ${attempts + 1}/${maxAttempts})...`);
      const response = await page.goto('https://localhost:9030', {
        waitUntil: 'networkidle',
        timeout: 10000
      });

      if (response && response.status() === 200) {
        adminReady = true;
        console.log('✅ Admin application is ready');
      }
    } catch (error) {
      attempts++;
      if (attempts < maxAttempts) {
        console.log('⏳ Admin app not ready yet, waiting 5 seconds...');
        await page.waitForTimeout(5000);
      }
    }
  }

  if (!adminReady) {
    console.error('❌ Admin application failed to become ready');
    throw new Error('Admin application is not ready after maximum attempts');
  }

  // Verify admin user can log in
  try {
    console.log('🔐 Verifying admin user authentication...');
    await page.goto('https://localhost:9030/login');
    await page.fill('input[name="Email"]', 'admin@stadium.com');
    await page.fill('input[name="Password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for successful login redirect
    await page.waitForURL('**/dashboard', { timeout: 15000 });
    console.log('✅ Admin authentication verified');
  } catch (error) {
    console.error('❌ Admin authentication failed:', error);
    throw new Error('Admin user authentication verification failed');
  }

  await browser.close();
  console.log('🎉 Global setup completed successfully');
}