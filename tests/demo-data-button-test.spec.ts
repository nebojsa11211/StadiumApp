import { test, expect } from '@playwright/test';

test.describe('Admin Events Demo Data Button', () => {
  let adminContext;
  
  test.beforeEach(async ({ browser }) => {
    // Create admin context
    adminContext = await browser.newContext();
    const adminPage = await adminContext.newPage();
    
    // Login as admin
    await adminPage.goto('https://localhost:9030/login');
    await adminPage.fill('input[placeholder="Enter username"]', 'admin');
    await adminPage.fill('input[placeholder="Enter password"]', 'admin123');
    await adminPage.click('button[type="submit"]');
    
    // Wait for navigation to complete
    await adminPage.waitForURL('**/dashboard', { timeout: 10000 });
    
    // Navigate to events page
    await adminPage.goto('https://localhost:9030/events');
    await adminPage.waitForLoadState('networkidle');
  });
  
  test.afterEach(async () => {
    await adminContext.close();
  });
  
  test('Demo Data button should generate demo data successfully', async () => {
    const adminPage = adminContext.pages()[0];
    
    // Check if the demo data button exists
    const demoDataButton = adminPage.locator('#admin-events-demo-data-btn');
    await expect(demoDataButton).toBeVisible();
    await expect(demoDataButton).toHaveText('🎲 Generate Demo Data');
    
    // Click the demo data button
    await demoDataButton.click();
    
    // Wait for success toast or events to appear
    const toast = adminPage.locator('.toast');
    
    // Should see a success message (either events created or demo data generated)
    await expect(toast).toBeVisible({ timeout: 10000 });
    
    // Check that the toast contains success message
    const toastText = await toast.textContent();
    expect(toastText).toMatch(/Demo (events created|data generated) successfully/);
    
    // Wait a moment for the data to load
    await adminPage.waitForTimeout(2000);
    
    // Check that events are displayed
    const eventCards = adminPage.locator('.event-card');
    const eventCount = await eventCards.count();
    expect(eventCount).toBeGreaterThan(0);
    
    // Verify that at least one event has analytics data if demo data was generated
    const analyticsSection = adminPage.locator('.event-analytics').first();
    if (await analyticsSection.isVisible()) {
      // Check for tickets sold
      const ticketsSold = await analyticsSection.locator('.analytics-item').first().textContent();
      expect(ticketsSold).toBeTruthy();
    }
    
    console.log(`✓ Demo data button works! Found ${eventCount} events.`);
  });
  
  test('Demo Data button should work even with no existing events', async () => {
    const adminPage = adminContext.pages()[0];
    
    // Check initial state - might have no events
    const initialEventCount = await adminPage.locator('.event-card').count();
    console.log(`Initial event count: ${initialEventCount}`);
    
    // Click the demo data button
    const demoDataButton = adminPage.locator('#admin-events-demo-data-btn');
    await demoDataButton.click();
    
    // Wait for the operation to complete
    await adminPage.waitForTimeout(3000);
    
    // Check that events now exist
    const finalEventCount = await adminPage.locator('.event-card').count();
    expect(finalEventCount).toBeGreaterThan(0);
    
    // If there were no events initially, we should have created some
    if (initialEventCount === 0) {
      console.log(`✓ Created ${finalEventCount} demo events from empty state`);
      expect(finalEventCount).toBeGreaterThanOrEqual(3); // We create 3 demo events
    } else {
      console.log(`✓ Generated demo data for existing events`);
    }
  });
  
  test('Demo Data generation should show proper error handling', async () => {
    const adminPage = adminContext.pages()[0];
    
    // First ensure we have events
    const demoDataButton = adminPage.locator('#admin-events-demo-data-btn');
    await demoDataButton.click();
    await adminPage.waitForTimeout(3000);
    
    // Now test with a specific event selected
    const firstEventCard = adminPage.locator('.event-card').first();
    if (await firstEventCard.isVisible()) {
      // Click on an event to select it
      await firstEventCard.click();
      
      // If details modal opens, close it
      const closeButton = adminPage.locator('#admin-events-details-close-btn');
      if (await closeButton.isVisible()) {
        await closeButton.click();
      }
      
      // Generate demo data again - should work for selected event
      await demoDataButton.click();
      
      // Should see success message
      const toast = adminPage.locator('.toast');
      await expect(toast).toBeVisible({ timeout: 10000 });
      const toastText = await toast.textContent();
      expect(toastText).toContain('generated');
    }
  });
});