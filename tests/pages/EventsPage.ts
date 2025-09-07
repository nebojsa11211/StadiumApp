import { Page, expect } from '@playwright/test';
import { testConfig } from '../config';

export class EventsPage {
  constructor(private page: Page) {}

  async navigate() {
    await this.page.goto(testConfig.customerApp + '/events');
    await this.page.waitForLoadState('networkidle');
  }

  async expectEventsListVisible() {
    await expect(this.page.locator('h2:has-text("🎫 Available Events")')).toBeVisible();
    // Wait for at least one buy tickets button to be visible
    await expect(this.page.getByRole('button', { name: 'Buy Tickets' }).first()).toBeVisible();
  }

  async clickFirstEventBuyTickets() {
    await this.page.click(this.page.getByRole('button', { name: 'Buy Tickets' }).first());
    await this.page.waitForLoadState('networkidle');
  }

  async expectEventDetailsPage() {
    // Check for event details elements
    await expect(this.page.locator('h4').first()).toBeVisible();
    await expect(this.page.locator('h5:has-text("🏟️ Stadium Sections")')).toBeVisible();
  }

  async filterByDateRange(startDate: string, endDate: string) {
    await this.page.fill('input[type="date"]').first(), startDate);
    await this.page.fill('input[type="date"]').nth(1), endDate);
  }

  async filterByPriceRange(minPrice: string, maxPrice: string) {
    await this.page.fill('input[placeholder*="Min price"]', minPrice);
    await this.page.fill('input[placeholder*="Max price"]', maxPrice);
  }

  async getEventCount(): Promise<number> {
    const buttons = await this.page.getByRole('button', { name: 'Buy Tickets' }).count();
    return buttons;
  }
}