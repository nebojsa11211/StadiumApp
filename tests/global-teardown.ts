import { FullConfig } from '@playwright/test';

/**
 * Global teardown for Playwright tests
 * Cleanup operations after all tests complete
 */
async function globalTeardown(config: FullConfig) {
  console.log('🧹 Starting global teardown...');

  // Add any cleanup operations here if needed
  // For example, clearing test data, stopping services, etc.

  console.log('✅ Global teardown completed');
}

export default globalTeardown;