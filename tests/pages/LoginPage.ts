import { Page, expect } from '@playwright/test';
import { testConfig } from '../config';

export abstract class LoginPage {
  constructor(protected page: Page) {}

  abstract get emailSelector(): string;
  abstract get passwordSelector(): string;
  abstract get submitSelector(): string;
  abstract get errorSelector(): string;
  abstract get titleSelector(): string;
  abstract get expectedTitle(): string;
  abstract get loginUrl(): string;

  async navigate() {
    await this.page.goto(this.loginUrl);
    await this.waitForPageLoad();
  }

  async waitForPageLoad() {
    await this.page.waitForLoadState('networkidle');
    
    // Wait for loading spinner to disappear if present
    await this.page.waitForFunction(() => {
      const loadingElement = document.querySelector('[data-testid="loading"], .loading, [class*="loading"]');
      const loadingText = document.body.textContent?.includes('Loading...') || 
                         document.body.textContent?.includes('Initializing application...');
      return !loadingElement && !loadingText;
    }, { timeout: testConfig.timeouts.blazorLoad }).catch(() => {
      console.log('Loading state check timed out, continuing...');
    });

    // Wait for Blazor Server to be ready
    await this.page.waitForFunction(() => {
      return (window as any).Blazor !== undefined;
    }, { timeout: testConfig.timeouts.blazorLoad }).catch(() => {
      console.log('Blazor not detected on this page, continuing...');
    });
    
    // Additional wait for the login form to be fully rendered
    await this.page.waitForTimeout(1000);
  }

  async fillEmail(email: string) {
    await this.page.fill(this.emailSelector, email);
  }

  async fillPassword(password: string) {
    await this.page.fill(this.passwordSelector, password);
  }

  async clickSubmit() {
    await this.page.click(this.submitSelector);
  }

  async login(email: string, password: string) {
    await this.fillEmail(email);
    await this.fillPassword(password);
    await this.clickSubmit();
  }

  async expectLoginForm() {
    await expect(this.page.locator(this.titleSelector)).toContainText(this.expectedTitle);
    await expect(this.page.locator(this.emailSelector)).toBeVisible();
    await expect(this.page.locator(this.passwordSelector)).toBeVisible();
    await expect(this.page.locator(this.submitSelector)).toBeVisible();
  }

  async expectError(errorText: string) {
    await expect(this.page.locator(this.errorSelector)).toBeVisible();
    await expect(this.page.locator(this.errorSelector)).toContainText(errorText);
  }

  async expectLoadingState() {
    await expect(this.page.locator(this.submitSelector)).toBeDisabled();
  }
}

export class AdminLoginPage extends LoginPage {
  get emailSelector() { return '#admin-login-email-input'; }
  get passwordSelector() { return '#admin-login-password-input'; }
  get submitSelector() { return '#admin-login-submit-btn'; }
  get errorSelector() { return '#admin-login-error'; }
  get titleSelector() { return '#admin-login-title'; }
  get expectedTitle() { return 'Stadium Admin Login'; }
  get loginUrl() { return testConfig.adminApp + '/login'; }

  async expectLoadingState() {
    await super.expectLoadingState();
    await expect(this.page.locator('#admin-login-loading-text')).toContainText('Logging in...');
  }
}

// Customer app doesn't have a login page - it's publicly accessible
// Using a MenuPage instead for customer interactions
export class CustomerMenuPage {
  constructor(protected page: Page) {}

  async navigate() {
    await this.page.goto(testConfig.customerApp + '/menu');
    await this.waitForPageLoad();
  }

  async waitForPageLoad() {
    await this.page.waitForLoadState('networkidle');
    
    // Wait for menu items to load
    await this.page.waitForSelector('.menu-item, h5:has-text("Beer")', { timeout: 15000 });
    
    // Wait for Blazor
    await this.page.waitForFunction(() => {
      return (window as any).Blazor !== undefined;
    }, { timeout: testConfig.timeouts.blazorLoad }).catch(() => {
      console.log('Blazor not detected, continuing...');
    });
  }

  async addItemToCart(itemName: string, quantity: number = 1) {
    // Find the item and add it to cart
    const itemSection = this.page.locator(`h5:has-text("${itemName}")`).locator('..').locator('..');
    
    // Increase quantity
    for (let i = 0; i < quantity; i++) {
      await itemSection.locator('button:has-text("+")').click();
    }
    
    // Click add to cart
    await itemSection.locator('button:has-text("Add to Cart")').click();
  }

  async expectMenuVisible() {
    await expect(this.page.locator('h1:has-text("Stadium Drinks Menu")')).toBeVisible();
    await expect(this.page.locator('h5:has-text("Beer")')).toBeVisible();
  }

  async expectCartItems(count: number) {
    await expect(this.page.locator('.cart')).toContainText(`${count} items`);
  }
}

export class StaffLoginPage extends LoginPage {
  get emailSelector() { return 'input[placeholder="Enter your email"]'; }
  get passwordSelector() { return 'input[placeholder="Enter your password"]'; }
  get submitSelector() { return 'button:has-text("Sign In")'; }
  get errorSelector() { return '.alert-danger'; }
  get titleSelector() { return 'h3'; }
  get expectedTitle() { return 'Staff Login'; }
  get loginUrl() { return testConfig.staffApp + '/login'; }
}