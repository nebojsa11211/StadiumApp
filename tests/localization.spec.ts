import { test, expect } from '@playwright/test';

test.describe('Multilingual Support', () => {
    test.beforeEach(async ({ page }) => {
        // Clear cookies to start fresh
        await page.context().clearCookies();
    });

    test('Language switcher changes UI text on Customer app', async ({ page }) => {
        await page.goto('http://localhost:7003');
        
        // Wait for page to load
        await page.waitForLoadState('networkidle');
        
        // Verify Croatian (default)
        await expect(page.locator('h1')).toContainText('Dobrodošli u Stadionska Pića');
        await expect(page.locator('text=Naruči Pića')).toBeVisible();
        await expect(page.locator('text=Moje Narudžbe')).toBeVisible();
        
        // Switch to English
        await page.selectOption('.language-switcher select', 'en');
        await page.waitForLoadState('networkidle');
        
        // Verify English
        await expect(page.locator('h1')).toContainText('Welcome to Stadium Drinks');
        await expect(page.locator('text=Order Drinks')).toBeVisible();
        await expect(page.locator('text=My Orders')).toBeVisible();
    });
    
    test('Language preference persists across sessions', async ({ page }) => {
        await page.goto('http://localhost:7003');
        
        // Switch to English
        await page.selectOption('.language-switcher select', 'en');
        await page.waitForLoadState('networkidle');
        
        // Verify English is set
        await expect(page.locator('h1')).toContainText('Welcome to Stadium Drinks');
        
        // Reload page
        await page.reload();
        await page.waitForLoadState('networkidle');
        
        // Verify still in English
        await expect(page.locator('h1')).toContainText('Welcome to Stadium Drinks');
        
        // Verify language selector shows English as selected
        const selectedValue = await page.locator('.language-switcher select').inputValue();
        expect(selectedValue).toBe('en');
    });
    
    test('All feature cards are translated correctly', async ({ page }) => {
        await page.goto('http://localhost:7003');
        
        // Check Croatian feature cards
        await expect(page.locator('text=Širok Izbor')).toBeVisible();
        await expect(page.locator('text=Brza Dostava')).toBeVisible();
        await expect(page.locator('text=Jednostavno Naručivanje')).toBeVisible();
        
        // Switch to English
        await page.selectOption('.language-switcher select', 'en');
        await page.waitForLoadState('networkidle');
        
        // Check English feature cards
        await expect(page.locator('text=Wide Selection')).toBeVisible();
        await expect(page.locator('text=Fast Delivery')).toBeVisible();
        await expect(page.locator('text=Easy Ordering')).toBeVisible();
    });
    
    test('How it works section is translated', async ({ page }) => {
        await page.goto('http://localhost:7003');
        
        // Check Croatian steps
        await expect(page.locator('text=Kako funkcionira')).toBeVisible();
        await expect(page.locator('text=Pregledajte naš jelovnik')).toBeVisible();
        
        // Switch to English
        await page.selectOption('.language-switcher select', 'en');
        await page.waitForLoadState('networkidle');
        
        // Check English steps
        await expect(page.locator('text=How it works')).toBeVisible();
        await expect(page.locator('text=Browse our menu')).toBeVisible();
    });
    
    test('Language switcher works on Admin app', async ({ page }) => {
        await page.goto('http://localhost:7005');
        
        // Check that language switcher is present
        await expect(page.locator('.language-switcher select')).toBeVisible();
        
        // Default should be Croatian
        const selectedValue = await page.locator('.language-switcher select').inputValue();
        expect(selectedValue).toBe('hr');
    });
    
    test('Language switcher works on Staff app', async ({ page }) => {
        await page.goto('http://localhost:7007');
        
        // Check that language switcher is present
        await expect(page.locator('.language-switcher select')).toBeVisible();
        
        // Default should be Croatian
        const selectedValue = await page.locator('.language-switcher select').inputValue();
        expect(selectedValue).toBe('hr');
    });
    
    test('Cookie is set correctly when changing language', async ({ page }) => {
        await page.goto('http://localhost:7003');
        
        // Switch to English
        await page.selectOption('.language-switcher select', 'en');
        await page.waitForLoadState('networkidle');
        
        // Check cookie value
        const cookies = await page.context().cookies();
        const cultureCookie = cookies.find(c => c.name === '.AspNetCore.Culture');
        
        expect(cultureCookie).toBeDefined();
        expect(cultureCookie?.value).toContain('c%3Den');
        expect(cultureCookie?.value).toContain('uic%3Den');
    });
});

test.describe('Parallel Language Testing', () => {
    test('Multiple users can have different language preferences', async ({ browser }) => {
        // Create two separate browser contexts (like different users)
        const context1 = await browser.newContext();
        const context2 = await browser.newContext();
        
        const page1 = await context1.newPage();
        const page2 = await context2.newPage();
        
        // User 1 uses Croatian
        await page1.goto('http://localhost:7003');
        await expect(page1.locator('h1')).toContainText('Dobrodošli u Stadionska Pića');
        
        // User 2 switches to English
        await page2.goto('http://localhost:7003');
        await page2.selectOption('.language-switcher select', 'en');
        await page2.waitForLoadState('networkidle');
        await expect(page2.locator('h1')).toContainText('Welcome to Stadium Drinks');
        
        // User 1 still sees Croatian
        await page1.reload();
        await expect(page1.locator('h1')).toContainText('Dobrodošli u Stadionska Pića');
        
        // Clean up
        await context1.close();
        await context2.close();
    });
});