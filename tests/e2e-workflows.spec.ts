import { test, expect } from '@playwright/test';

test.describe('E2E Cross-Application Workflows', () => {
  test.beforeEach(async ({ context }) => {
    // Set up contexts for multiple tabs/applications
    const customerPage = await context.newPage();
    const adminPage = await context.newPage();
    const staffPage = await context.newPage();
  });

  test('INT-001: Complete ticket purchase flow', async ({ context }) => {
    const page = await context.newPage();
    
    // Navigate to customer application
    await page.goto('http://localhost:9001');
    
    // Step 1: Browse events
    await page.click('a[href*="/events"]');
    await expect(page).toHaveURL(/.*events/);
    await expect(page.locator('.events-container')).toBeVisible();
    
    // Wait for events to load
    await page.waitForSelector('.event-card', { timeout: 10000 });
    
    // Step 2: Select an event
    const eventCard = page.locator('.event-card').first();
    await eventCard.click();
    
    // Should navigate to event details
    await expect(page).toHaveURL(/.*event-details/);
    await expect(page.locator('.event-details')).toBeVisible();
    
    // Step 3: Select seats
    const section = page.locator('.stadium-section').first();
    await section.click();
    
    // Wait for seat selection modal
    await expect(page.locator('.seat-selection-modal')).toBeVisible();
    
    // Select available seats
    const availableSeats = page.locator('.seat.available').first();
    await availableSeats.click();
    
    // Add to cart
    await page.click('button:has-text("Add to Cart")');
    
    // Verify cart icon updates
    await expect(page.locator('.cart-count')).toContainText('1');
    
    // Step 4: Proceed to checkout
    await page.click('a[href*="/cart"]');
    await expect(page).toHaveURL(/.*cart/);
    
    // Verify items in cart
    await expect(page.locator('.cart-item')).toBeVisible();
    
    // Proceed to checkout
    await page.click('button:has-text("Checkout")');
    await expect(page).toHaveURL(/.*checkout/);
    
    // Step 5: Fill checkout form
    await page.fill('input[name="customerName"]', 'Test Customer');
    await page.fill('input[name="email"]', 'test@example.com');
    await page.fill('input[name="phone"]', '1234567890');
    
    // Fill payment info
    await page.fill('input[name="cardNumber"]', '4111111111111111');
    await page.fill('input[name="expiryDate"]', '12/25');
    await page.fill('input[name="cvv"]', '123');
    
    // Accept terms
    await page.check('input[name="acceptTerms"]');
    
    // Complete purchase
    await page.click('button:has-text("Complete Purchase")');
    
    // Step 6: Verify confirmation
    await expect(page).toHaveURL(/.*confirmation/);
    await expect(page.locator('.order-confirmation')).toBeVisible();
    await expect(page.locator('.ticket-details')).toBeVisible();
    
    console.log('✅ INT-001: Complete ticket purchase flow - PASSED');
  });

  test('INT-002: Admin creates event, customer purchases', async ({ context }) => {
    const adminPage = await context.newPage();
    const customerPage = await context.newPage();
    
    // ADMIN FLOW - Create Event
    await adminPage.goto('http://localhost:9002/login');
    
    // Login as admin
    await adminPage.fill('[data-testid="email"]', 'admin@stadium.com');
    await adminPage.fill('[data-testid="password"]', 'admin123');
    await adminPage.click('[data-testid="login-button"]');
    
    // Navigate to events management
    await adminPage.goto('http://localhost:9002/events');
    
    // Create new event
    await adminPage.click('button:has-text("Create New Event")');
    
    // Fill event form
    await adminPage.fill('input[name="name"]', 'E2E Test Event');
    await adminPage.fill('input[name="description"]', 'Test event for E2E testing');
    await adminPage.fill('input[name="date"]', '2025-12-31');
    await adminPage.fill('input[name="time"]', '19:00');
    await adminPage.selectOption('select[name="type"]', 'Sports');
    
    // Set pricing for sections
    await adminPage.fill('input[name="upperDeckPrice"]', '50');
    await adminPage.fill('input[name="lowerDeckPrice"]', '100');
    await adminPage.fill('input[name="premiumPrice"]', '200');
    
    // Save event
    await adminPage.click('button:has-text("Create Event")');
    
    // Verify event creation
    await expect(adminPage.locator('.toast.success')).toContainText('Event created successfully');
    
    // Activate the event
    const eventCard = adminPage.locator('.event-card:has-text("E2E Test Event")');
    await eventCard.locator('button:has-text("Activate")').click();
    
    // CUSTOMER FLOW - Purchase ticket for new event
    await customerPage.goto('http://localhost:9001/events');
    
    // Find and select the newly created event
    const customerEventCard = customerPage.locator('.event-card:has-text("E2E Test Event")');
    await expect(customerEventCard).toBeVisible();
    await customerEventCard.click();
    
    // Select seats and proceed with purchase
    const section = customerPage.locator('.stadium-section').first();
    await section.click();
    
    await expect(customerPage.locator('.seat-selection-modal')).toBeVisible();
    const availableSeats = customerPage.locator('.seat.available').first();
    await availableSeats.click();
    await customerPage.click('button:has-text("Add to Cart")');
    
    // Complete purchase flow
    await customerPage.click('a[href*="/cart"]');
    await customerPage.click('button:has-text("Checkout")');
    
    // Fill customer details
    await customerPage.fill('input[name="customerName"]', 'E2E Test Customer');
    await customerPage.fill('input[name="email"]', 'e2e@example.com');
    await customerPage.fill('input[name="phone"]', '9876543210');
    
    // Fill payment details
    await customerPage.fill('input[name="cardNumber"]', '4111111111111111');
    await customerPage.fill('input[name="expiryDate"]', '12/25');
    await customerPage.fill('input[name="cvv"]', '123');
    await customerPage.check('input[name="acceptTerms"]');
    
    await customerPage.click('button:has-text("Complete Purchase")');
    
    // Verify purchase completion
    await expect(customerPage).toHaveURL(/.*confirmation/);
    await expect(customerPage.locator('.order-confirmation')).toBeVisible();
    
    console.log('✅ INT-002: Admin creates event, customer purchases - PASSED');
  });

  test('INT-003: Order processing workflow', async ({ context }) => {
    const customerPage = await context.newPage();
    const staffPage = await context.newPage();
    
    // CUSTOMER FLOW - Place an order
    await customerPage.goto('http://localhost:9001');
    
    // Navigate to events and select one
    await customerPage.click('a[href*="/events"]');
    await customerPage.waitForSelector('.event-card');
    
    const eventCard = customerPage.locator('.event-card').first();
    await eventCard.click();
    
    // Select seats
    const section = customerPage.locator('.stadium-section').first();
    await section.click();
    
    await expect(customerPage.locator('.seat-selection-modal')).toBeVisible();
    const availableSeats = customerPage.locator('.seat.available').first();
    await availableSeats.click();
    await customerPage.click('button:has-text("Add to Cart")');
    
    // Complete order
    await customerPage.click('a[href*="/cart"]');
    await customerPage.click('button:has-text("Checkout")');
    
    await customerPage.fill('input[name="customerName"]', 'Order Test Customer');
    await customerPage.fill('input[name="email"]', 'order@example.com');
    await customerPage.fill('input[name="phone"]', '5551234567');
    
    await customerPage.fill('input[name="cardNumber"]', '4111111111111111');
    await customerPage.fill('input[name="expiryDate"]', '12/25');
    await customerPage.fill('input[name="cvv"]', '123');
    await customerPage.check('input[name="acceptTerms"]');
    
    await customerPage.click('button:has-text("Complete Purchase")');
    
    // Verify order completion
    await expect(customerPage).toHaveURL(/.*confirmation/);
    
    // STAFF FLOW - Process the order
    await staffPage.goto('http://localhost:9003/login');
    
    // Login as staff
    await staffPage.fill('input[name="email"]', 'staff@stadium.com');
    await staffPage.fill('input[name="password"]', 'staff123');
    await staffPage.click('button:has-text("Login")');
    
    // Check orders queue
    await staffPage.goto('http://localhost:9003/orders');
    
    // Find the new order
    const orderRow = staffPage.locator('.order-row:has-text("Order Test Customer")');
    await expect(orderRow).toBeVisible();
    
    // Accept the order
    await orderRow.locator('button:has-text("Accept")').click();
    
    // Mark as in preparation
    await orderRow.locator('button:has-text("In Preparation")').click();
    
    // Mark as ready
    await orderRow.locator('button:has-text("Ready")').click();
    
    // Complete delivery
    await orderRow.locator('button:has-text("Delivered")').click();
    
    // Verify order status
    await expect(orderRow.locator('.status')).toContainText('Delivered');
    
    console.log('✅ INT-003: Order processing workflow - PASSED');
  });

  test('INT-004: Real-time updates test', async ({ context }) => {
    const customerPage = await context.newPage();
    const staffPage = await context.newPage();
    
    // Setup customer page to monitor updates
    await customerPage.goto('http://localhost:9001');
    
    // Place an order first
    await customerPage.click('a[href*="/events"]');
    await customerPage.waitForSelector('.event-card');
    
    const eventCard = customerPage.locator('.event-card').first();
    await eventCard.click();
    
    const section = customerPage.locator('.stadium-section').first();
    await section.click();
    
    await expect(customerPage.locator('.seat-selection-modal')).toBeVisible();
    const availableSeats = customerPage.locator('.seat.available').first();
    await availableSeats.click();
    await customerPage.click('button:has-text("Add to Cart")');
    
    await customerPage.click('a[href*="/cart"]');
    await customerPage.click('button:has-text("Checkout")');
    
    await customerPage.fill('input[name="customerName"]', 'SignalR Test Customer');
    await customerPage.fill('input[name="email"]', 'signalr@example.com');
    await customerPage.fill('input[name="phone"]', '7778889999');
    
    await customerPage.fill('input[name="cardNumber"]', '4111111111111111');
    await customerPage.fill('input[name="expiryDate"]', '12/25');
    await customerPage.fill('input[name="cvv"]', '123');
    await customerPage.check('input[name="acceptTerms"]');
    
    await customerPage.click('button:has-text("Complete Purchase")');
    
    // Navigate to order tracking page
    await customerPage.goto('http://localhost:9001/my-orders');
    
    // Staff updates order status
    await staffPage.goto('http://localhost:9003/login');
    await staffPage.fill('input[name="email"]', 'staff@stadium.com');
    await staffPage.fill('input[name="password"]', 'staff123');
    await staffPage.click('button:has-text("Login")');
    
    await staffPage.goto('http://localhost:9003/orders');
    
    const orderRow = staffPage.locator('.order-row:has-text("SignalR Test Customer")');
    await expect(orderRow).toBeVisible();
    
    // Accept the order and check for real-time update on customer side
    await orderRow.locator('button:has-text("Accept")').click();
    
    // Check if customer page shows updated status (real-time via SignalR)
    await expect(customerPage.locator('.order-status:has-text("Accepted")')).toBeVisible({ timeout: 10000 });
    
    // Update to in preparation
    await orderRow.locator('button:has-text("In Preparation")').click();
    await expect(customerPage.locator('.order-status:has-text("In Preparation")')).toBeVisible({ timeout: 10000 });
    
    // Mark as ready
    await orderRow.locator('button:has-text("Ready")').click();
    await expect(customerPage.locator('.order-status:has-text("Ready")')).toBeVisible({ timeout: 10000 });
    
    console.log('✅ INT-004: Real-time updates test - PASSED');
  });

  test('INT-005: SignalR connection establishment', async ({ context }) => {
    const page = await context.newPage();
    
    // Test customer app SignalR connection
    await page.goto('http://localhost:9001');
    
    // Check for SignalR connection indicator
    const connectionStatus = page.locator('.signalr-status');
    await expect(connectionStatus).toContainText('Connected', { timeout: 10000 });
    
    // Test admin app SignalR connection
    await page.goto('http://localhost:9002/login');
    await page.fill('[data-testid="email"]', 'admin@stadium.com');
    await page.fill('[data-testid="password"]', 'admin123');
    await page.click('[data-testid="login-button"]');
    
    await page.goto('http://localhost:9002/dashboard');
    const adminConnectionStatus = page.locator('.signalr-status');
    await expect(adminConnectionStatus).toContainText('Connected', { timeout: 10000 });
    
    // Test staff app SignalR connection
    await page.goto('http://localhost:9003/login');
    await page.fill('input[name="email"]', 'staff@stadium.com');
    await page.fill('input[name="password"]', 'staff123');
    await page.click('button:has-text("Login")');
    
    const staffConnectionStatus = page.locator('.signalr-status');
    await expect(staffConnectionStatus).toContainText('Connected', { timeout: 10000 });
    
    console.log('✅ INT-005: SignalR connection establishment - PASSED');
  });

  test('INT-006: Database consistency - Seat reservation', async ({ context }) => {
    const page1 = await context.newPage();
    const page2 = await context.newPage();
    
    // Both users try to select the same seat simultaneously
    await Promise.all([
      page1.goto('http://localhost:9001/events'),
      page2.goto('http://localhost:9001/events')
    ]);
    
    // Both select the same event
    await Promise.all([
      page1.waitForSelector('.event-card'),
      page2.waitForSelector('.event-card')
    ]);
    
    const [eventCard1, eventCard2] = await Promise.all([
      page1.locator('.event-card').first(),
      page2.locator('.event-card').first()
    ]);
    
    await Promise.all([
      eventCard1.click(),
      eventCard2.click()
    ]);
    
    // Both try to select the same section and seat
    const [section1, section2] = await Promise.all([
      page1.locator('.stadium-section').first(),
      page2.locator('.stadium-section').first()
    ]);
    
    await Promise.all([
      section1.click(),
      section2.click()
    ]);
    
    // Wait for modals to appear
    await Promise.all([
      expect(page1.locator('.seat-selection-modal')).toBeVisible(),
      expect(page2.locator('.seat-selection-modal')).toBeVisible()
    ]);
    
    // Both try to select the same seat (Row A, Seat 1)
    const [seat1, seat2] = await Promise.all([
      page1.locator('.seat[data-row="A"][data-seat="1"]'),
      page2.locator('.seat[data-row="A"][data-seat="1"]')
    ]);
    
    // Simultaneously click the same seat
    await Promise.all([
      seat1.click(),
      seat2.click()
    ]);
    
    // Only one should succeed in selecting the seat
    const [addToCart1, addToCart2] = await Promise.all([
      page1.locator('button:has-text("Add to Cart")'),
      page2.locator('button:has-text("Add to Cart")')
    ]);
    
    // Try to add to cart simultaneously
    await Promise.all([
      addToCart1.click().catch(() => {}), // May fail
      addToCart2.click().catch(() => {}) // May fail
    ]);
    
    // Check that only one cart shows the item
    await Promise.all([
      page1.goto('http://localhost:9001/cart'),
      page2.goto('http://localhost:9001/cart')
    ]);
    
    // One should have the item, the other should be empty or show error
    const cart1Items = await page1.locator('.cart-item').count();
    const cart2Items = await page2.locator('.cart-item').count();
    
    // Only one cart should have the item (database consistency check)
    expect(cart1Items + cart2Items).toBe(1);
    
    console.log('✅ INT-006: Database consistency - Seat reservation - PASSED');
  });
});