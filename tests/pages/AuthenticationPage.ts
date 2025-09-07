import { Page, expect } from '@playwright/test';
import { testConfig } from '../config';

export class AuthenticationPage {
  constructor(private page: Page) {}

  async expectAuthenticationRequired() {
    // Wait for authentication required UI to appear
    await expect(this.page.locator('h4:has-text("🔐 Authentication Required")')).toBeVisible({ timeout: 5000 });
    await expect(this.page.locator('h5:has-text("Please Sign In")')).toBeVisible();
    await expect(this.page.getByRole('button', { name: 'Sign In' })).toBeVisible();
    await expect(this.page.getByRole('button', { name: 'Create Account' })).toBeVisible();
  }

  async clickSignIn() {
    await this.page.click('button:has-text("Sign In")');
    await this.page.waitForLoadState('networkidle');
  }

  async clickCreateAccount() {
    await this.page.click('button:has-text("Create Account")');
    await this.page.waitForLoadState('networkidle');
  }

  async expectRedirectToLogin() {
    await expect(this.page).toHaveURL(/\/login/);
  }

  async expectRedirectWithReturnUrl(originalPath: string) {
    const url = this.page.url();
    expect(url).toContain('/login');
    expect(url).toContain('returnUrl');
    // The return URL should contain the original path
    expect(decodeURIComponent(url)).toContain(originalPath);
  }
}