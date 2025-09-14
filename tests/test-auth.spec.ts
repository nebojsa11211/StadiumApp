import { test, expect } from '@playwright/test';
import { adminLogin } from './helpers/auth-helpers';

test('test admin login helper', async ({ page }) => {
  await adminLogin(page);

  // Should be on dashboard
  await expect(page).toHaveURL(/.*\/dashboard/);

  console.log('✅ Auth helper working correctly');
});