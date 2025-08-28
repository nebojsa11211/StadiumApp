import { test, expect } from '@playwright/test';

test.describe('Login Functionality Tests', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5003/login');
  });

  test('should display login form correctly', async ({ page }) => {
    await expect(page.locator('h3')).toContainText('Stadium Admin Login');
    await expect(page.locator('input[type="email"]')).toBeVisible();
    await expect(page.locator('input[type="password"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toBeVisible();
    await expect(page.locator('button[type="submit"]')).toContainText('Login');
  });

  test('should show error for empty fields', async ({ page }) => {
    await page.locator('button[type="submit"]').click();
    
    // Check for validation messages
    const emailInput = page.locator('input[type="email"]');
    const passwordInput = page.locator('input[type="password"]');
    
    await expect(emailInput).toHaveAttribute('required');
    await expect(passwordInput).toHaveAttribute('required');
  });

  test('should show error for invalid credentials', async ({ page }) => {
    await page.locator('input[type="email"]').fill('wrong@email.com');
    await page.locator('input[type="password"]').fill('wrongpassword');
    await page.locator('button[type="submit"]').click();
    
    await expect(page.locator('.alert-danger')).toBeVisible();
    await expect(page.locator('.alert-danger')).toContainText('Invalid email or password');
  });

  test('should login successfully with admin credentials', async ({ page }) => {
    await page.locator('input[type="email"]').fill('admin@stadium.com');
    await page.locator('input[type="password"]').fill('admin123');
    await page.locator('button[type="submit"]').click();
    
    // Wait for navigation
    await page.waitForURL('**/bartender');
    
    // Verify successful login
    await expect(page).toHaveURL(/.*\/bartender/);
    await expect(page.locator('h1')).toContainText('Bartender Dashboard');
  });

  test('should show loading state during login', async ({ page }) => {
    await page.locator('input[type="email"]').fill('admin@stadium.com');
    await page.locator('input[type="password"]').fill('admin123');
    
    const submitButton = page.locator('button[type="submit"]');
    await submitButton.click();
    
    await expect(submitButton).toBeDisabled();
    await expect(submitButton).toContainText('Logging in...');
  });

  test('should redirect to returnUrl after login', async ({ page }) => {
    await page.goto('http://localhost:5003/login?returnUrl=/orders');
    
    await page.locator('input[type="email"]').fill('admin@stadium.com');
    await page.locator('input[type="password"]').fill('admin123');
    await page.locator('button[type="submit"]').click();
    
    await page.waitForURL('**/orders');
    await expect(page).toHaveURL(/.*\/orders/);
  });

  test('should handle API connection errors gracefully', async ({ page }) => {
    // Mock API failure
    await page.route('**/api/auth/login', route => {
      route.abort('connectionfailed');
    });
    
    await page.locator('input[type="email"]').fill('admin@stadium.com');
    await page.locator('input[type="password"]').fill('admin123');
    await page.locator('button[type="submit"]').click();
    
    await expect(page.locator('.alert-danger')).toBeVisible();
    await expect(page.locator('.alert-danger')).toContainText('Login failed');
  });

  test('should validate email format', async ({ page }) => {
    await page.locator('input[type="email"]').fill('invalid-email');
    await page.locator('input[type="password"]').fill('admin123');
    await page.locator('button[type="submit"]').click();
    
    // Check for browser validation
    const emailInput = page.locator('input[type="email"]');
    await expect(emailInput).toHaveAttribute('type', 'email');
  });
});

test.describe('API Health Check', () => {
  test('should verify API is accessible', async ({ request }) => {
    const response = await request.get('http://localhost:5001/health');
    expect(response.ok()).toBeTruthy();
  });

  test('should verify login endpoint works', async ({ request }) => {
    const response = await request.post('http://localhost:5001/api/auth/login', {
      data: {
        email: 'admin@stadium.com',
        password: 'admin123'
      }
    });
    
    expect(response.ok()).toBeTruthy();
    const data = await response.json();
    expect(data).toHaveProperty('token');
    expect(data).toHaveProperty('user');
    expect(data.user.email).toBe('admin@stadium.com');
  });
});
