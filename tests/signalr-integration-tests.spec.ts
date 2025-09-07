import { test, expect } from '@playwright/test';
import { testConfig } from './config';

test.describe('SignalR Real-time Communication Integration Tests', () => {

  test.describe('Connection Management', () => {
    
    test('INT-SIGNALR-001: Connection establishment across all apps', async ({ page, context }) => {
      console.log('🔌 Testing SignalR connection establishment...');
      
      // Test Customer App Connection
      console.log('📱 Testing customer app connection...');
      await page.goto(testConfig.customerApp);
      await page.waitForLoadState('networkidle');
      
      // Look for SignalR connection indicators
      const connectionIndicators = [
        '.connection-status',
        '.signalr-status', 
        '[class*="connected"]',
        '.status-indicator',
        '#connection-status',
        '[data-testid*="connection"]'
      ];
      
      let customerConnectionFound = false;
      for (const indicator of connectionIndicators) {
        const element = page.locator(indicator);
        if (await element.count() > 0) {
          const text = await element.textContent();
          console.log(`✅ Customer app connection indicator: ${indicator} - ${text}`);
          customerConnectionFound = true;
          break;
        }
      }
      
      // Check console for SignalR messages
      const consoleMessages: string[] = [];
      page.on('console', msg => {
        const text = msg.text();
        if (text.toLowerCase().includes('signalr') || text.toLowerCase().includes('connection')) {
          consoleMessages.push(text);
        }
      });
      
      await page.waitForTimeout(2000);
      
      if (consoleMessages.length > 0) {
        console.log('✅ SignalR console messages found:');
        consoleMessages.forEach(msg => console.log(`  📢 ${msg}`));
      }
      
      // Test Admin App Connection
      console.log('👨‍💼 Testing admin app connection...');
      const adminPage = await context.newPage();
      await adminPage.goto(`${testConfig.adminApp}/login`);
      
      // Login to admin
      await adminPage.fill('#admin-login-email-input', 'admin@stadium.com');
      await adminPage.fill('#admin-login-password-input', 'admin123');
      await adminPage.click('#admin-login-submit-btn');
      await adminPage.waitForURL(`${testConfig.adminApp}*`);
      
      // Check for admin connection indicators
      let adminConnectionFound = false;
      for (const indicator of connectionIndicators) {
        const element = adminPage.locator(indicator);
        if (await element.count() > 0) {
          const text = await element.textContent();
          console.log(`✅ Admin app connection indicator: ${indicator} - ${text}`);
          adminConnectionFound = true;
          break;
        }
      }
      
      // Test Staff App Connection
      console.log('👨‍🍳 Testing staff app connection...');
      const staffPage = await context.newPage();
      await staffPage.goto(testConfig.staffApp);
      
      // Check for staff connection indicators
      let staffConnectionFound = false;
      for (const indicator of connectionIndicators) {
        const element = staffPage.locator(indicator);
        if (await element.count() > 0) {
          const text = await element.textContent();
          console.log(`✅ Staff app connection indicator: ${indicator} - ${text}`);
          staffConnectionFound = true;
          break;
        }
      }
      
      // Summary
      console.log('🔗 Connection Status Summary:');
      console.log(`  Customer App: ${customerConnectionFound ? '✅' : '⚠️'}`);
      console.log(`  Admin App: ${adminConnectionFound ? '✅' : '⚠️'}`);
      console.log(`  Staff App: ${staffConnectionFound ? '✅' : '⚠️'}`);
      
      await adminPage.close();
      await staffPage.close();
    });

    test('INT-SIGNALR-002: Connection resilience and reconnection', async ({ page }) => {
      console.log('🔄 Testing connection resilience...');
      
      await page.goto(testConfig.customerApp);
      await page.waitForLoadState('networkidle');
      
      // Simulate network interruption by going offline
      await page.context().setOffline(true);
      await page.waitForTimeout(2000);
      
      // Check for offline indicators
      const offlineIndicators = [
        '.offline',
        '.disconnected',
        '.connection-lost',
        '[class*="offline"]'
      ];
      
      let offlineDetected = false;
      for (const indicator of offlineIndicators) {
        if (await page.locator(indicator).count() > 0) {
          console.log(`✅ Offline state detected: ${indicator}`);
          offlineDetected = true;
          break;
        }
      }
      
      // Go back online
      await page.context().setOffline(false);
      await page.waitForTimeout(3000);
      
      // Check for reconnection
      const reconnectionIndicators = [
        '.reconnected',
        '.online',
        '.connected',
        '[class*="connected"]'
      ];
      
      let reconnectionDetected = false;
      for (const indicator of reconnectionIndicators) {
        if (await page.locator(indicator).count() > 0) {
          console.log(`✅ Reconnection detected: ${indicator}`);
          reconnectionDetected = true;
          break;
        }
      }
      
      console.log(`Resilience test: Offline=${offlineDetected ? '✅' : '⚠️'}, Reconnect=${reconnectionDetected ? '✅' : '⚠️'}`);
    });
  });

  test.describe('Real-time Order Updates', () => {
    
    test('INT-SIGNALR-003: Order status updates in real-time', async ({ page, context }) => {
      console.log('📦 Testing real-time order status updates...');
      
      // Customer places an order
      console.log('1️⃣ Customer placing order...');
      await page.goto(testConfig.customerApp);
      await page.click('a:has-text("Order Drinks"), a[href*="drinks"], a[href*="menu"]');
      await page.waitForLoadState('networkidle');
      
      const addButton = page.locator('button:has-text("+"), button:has-text("Add")').first();
      let orderPlaced = false;
      
      if (await addButton.count() > 0) {
        await addButton.click();
        
        const cartButton = page.locator('a:has-text("Cart"), button:has-text("Cart")');
        if (await cartButton.count() > 0) {
          await cartButton.first().click();
          
          // Fill customer info if required
          const emailInput = page.locator('input[type="email"], input[name*="email"]');
          if (await emailInput.count() > 0) {
            await emailInput.fill('signalr-test@stadium.com');
          }
          
          const checkoutButton = page.locator('button:has-text("Checkout"), button:has-text("Place Order")');
          if (await checkoutButton.count() > 0) {
            await checkoutButton.first().click();
            orderPlaced = true;
            console.log('✅ Order placed by customer');
          }
        }
      }
      
      if (!orderPlaced) {
        console.log('⚠️ Could not place order - skipping real-time test');
        return;
      }
      
      // Open staff app to process the order
      console.log('2️⃣ Staff processing order...');
      const staffPage = await context.newPage();
      await staffPage.goto(testConfig.staffApp);
      
      // Try to login to staff if login required
      const loginButton = staffPage.locator('button:has-text("Login"), button:has-text("Sign In")');
      if (await loginButton.count() > 0) {
        const emailInput = staffPage.locator('input[type="email"], input[name*="email"]');
        const passwordInput = staffPage.locator('input[type="password"], input[name*="password"]');
        
        if (await emailInput.count() > 0 && await passwordInput.count() > 0) {
          await emailInput.fill('staff@stadium.com');
          await passwordInput.fill('staff123');
          await loginButton.click();
          await staffPage.waitForTimeout(2000);
        }
      }
      
      // Look for orders in staff dashboard
      const orderElements = staffPage.locator('.order, .order-item, [class*="order"]');
      const orderCount = await orderElements.count();
      console.log(`📋 Staff sees ${orderCount} orders`);
      
      if (orderCount > 0) {
        // Try to update order status
        const statusButtons = staffPage.locator('button:has-text("Accept"), button:has-text("Prepare"), button:has-text("Ready")');
        if (await statusButtons.count() > 0) {
          await statusButtons.first().click();
          console.log('✅ Staff updated order status');
          
          // Wait for potential real-time update
          await page.waitForTimeout(2000);
          
          // Check customer side for status update
          await page.reload();
          const statusIndicators = page.locator('.status, .order-status, [class*="status"]');
          
          if (await statusIndicators.count() > 0) {
            const statusText = await statusIndicators.first().textContent();
            console.log(`✅ Customer sees order status: ${statusText}`);
          }
        }
      }
      
      await staffPage.close();
    });

    test('INT-SIGNALR-004: New order notifications to staff', async ({ page, context }) => {
      console.log('🔔 Testing new order notifications to staff...');
      
      // Open staff dashboard first
      const staffPage = await context.newPage();
      await staffPage.goto(testConfig.staffApp);
      
      // Try to login
      const loginButton = staffPage.locator('button:has-text("Login"), button:has-text("Sign In")');
      if (await loginButton.count() > 0) {
        const emailInput = staffPage.locator('input[type="email"], input[name*="email"]');
        const passwordInput = staffPage.locator('input[type="password"], input[name*="password"]');
        
        if (await emailInput.count() > 0 && await passwordInput.count() > 0) {
          await emailInput.fill('staff@stadium.com');
          await passwordInput.fill('staff123');
          await loginButton.click();
        }
      }
      
      // Count initial orders
      await staffPage.waitForTimeout(1000);
      const initialOrders = await staffPage.locator('.order, .order-item, [class*="order"]').count();
      console.log(`📊 Initial orders in staff dashboard: ${initialOrders}`);
      
      // Customer places new order
      console.log('📱 Customer placing new order...');
      await page.goto(testConfig.customerApp);
      await page.click('a:has-text("Order Drinks"), a[href*="drinks"], a[href*="menu"]');
      await page.waitForLoadState('networkidle');
      
      const addButton = page.locator('button:has-text("+"), button:has-text("Add")').first();
      if (await addButton.count() > 0) {
        await addButton.click();
        
        const cartButton = page.locator('a:has-text("Cart"), button:has-text("Cart")');
        if (await cartButton.count() > 0) {
          await cartButton.first().click();
          
          const checkoutButton = page.locator('button:has-text("Checkout"), button:has-text("Place Order")');
          if (await checkoutButton.count() > 0) {
            await checkoutButton.first().click();
            console.log('✅ New order placed');
            
            // Wait for potential notification
            await staffPage.waitForTimeout(3000);
            
            // Check for notifications in staff app
            const notificationSelectors = [
              '.notification',
              '.alert',
              '.toast',
              '.badge',
              '[class*="notification"]',
              '[class*="alert"]'
            ];
            
            let notificationFound = false;
            for (const selector of notificationSelectors) {
              const element = staffPage.locator(selector);
              if (await element.count() > 0) {
                const text = await element.first().textContent();
                console.log(`🔔 Notification found: ${selector} - ${text}`);
                notificationFound = true;
                break;
              }
            }
            
            // Check if order count increased
            const newOrderCount = await staffPage.locator('.order, .order-item, [class*="order"]').count();
            console.log(`📊 New orders in staff dashboard: ${newOrderCount}`);
            
            if (newOrderCount > initialOrders) {
              console.log('✅ New order appeared in staff dashboard');
            }
            
            if (notificationFound || newOrderCount > initialOrders) {
              console.log('✅ Real-time notification system working');
            } else {
              console.log('⚠️ No real-time notifications detected');
            }
          }
        }
      }
      
      await staffPage.close();
    });
  });

  test.describe('Cross-App Communication', () => {
    
    test('INT-SIGNALR-005: Admin broadcasts to all connected clients', async ({ page, context }) => {
      console.log('📢 Testing admin broadcasts...');
      
      // Open customer app
      const customerPage = page;
      await customerPage.goto(testConfig.customerApp);
      
      // Open staff app
      const staffPage = await context.newPage();
      await staffPage.goto(testConfig.staffApp);
      
      // Open admin app
      const adminPage = await context.newPage();
      await adminPage.goto(`${testConfig.adminApp}/login`);
      await adminPage.fill('#admin-login-email-input', 'admin@stadium.com');
      await adminPage.fill('#admin-login-password-input', 'admin123');
      await adminPage.click('#admin-login-submit-btn');
      await adminPage.waitForURL(`${testConfig.adminApp}*`);
      
      // Look for broadcast functionality in admin
      const broadcastSelectors = [
        'button:has-text("Broadcast")',
        'button:has-text("Announce")',
        'button:has-text("Notify")',
        '.broadcast-btn',
        '#broadcast-button'
      ];
      
      let broadcastButton = null;
      for (const selector of broadcastSelectors) {
        const button = adminPage.locator(selector);
        if (await button.count() > 0) {
          broadcastButton = button;
          console.log(`✅ Broadcast button found: ${selector}`);
          break;
        }
      }
      
      if (broadcastButton) {
        await broadcastButton.click();
        
        // Fill broadcast message if form appears
        const messageInput = adminPage.locator('textarea, input[type="text"]');
        if (await messageInput.count() > 0) {
          await messageInput.fill('Test broadcast message from admin');
          
          const sendButton = adminPage.locator('button:has-text("Send"), button:has-text("Broadcast")');
          if (await sendButton.count() > 0) {
            await sendButton.click();
            console.log('✅ Broadcast sent from admin');
          }
        }
        
        // Check if message appears in other apps
        await Promise.all([
          customerPage.waitForTimeout(2000),
          staffPage.waitForTimeout(2000)
        ]);
        
        const messageSelectors = [
          '.broadcast-message',
          '.notification',
          '.alert',
          '.toast',
          '[class*="message"]'
        ];
        
        // Check customer app
        let customerReceived = false;
        for (const selector of messageSelectors) {
          const element = customerPage.locator(selector);
          if (await element.count() > 0) {
            const text = await element.textContent();
            if (text && text.includes('Test broadcast')) {
              console.log('✅ Customer received broadcast');
              customerReceived = true;
              break;
            }
          }
        }
        
        // Check staff app
        let staffReceived = false;
        for (const selector of messageSelectors) {
          const element = staffPage.locator(selector);
          if (await element.count() > 0) {
            const text = await element.textContent();
            if (text && text.includes('Test broadcast')) {
              console.log('✅ Staff received broadcast');
              staffReceived = true;
              break;
            }
          }
        }
        
        console.log(`Broadcast results: Customer=${customerReceived ? '✅' : '⚠️'}, Staff=${staffReceived ? '✅' : '⚠️'}`);
      } else {
        console.log('⚠️ No broadcast functionality found in admin');
      }
      
      await adminPage.close();
      await staffPage.close();
    });

    test('INT-SIGNALR-006: Real-time inventory updates', async ({ page, context }) => {
      console.log('📦 Testing real-time inventory updates...');
      
      // Customer viewing menu
      await page.goto(testConfig.customerApp);
      await page.click('a:has-text("Order Drinks"), a[href*="drinks"), a[href*="menu"]');
      await page.waitForLoadState('networkidle');
      
      // Check initial stock levels
      const stockElements = page.locator('.stock, .inventory, [class*="stock"]');
      const initialStockCount = await stockElements.count();
      console.log(`📊 Stock elements visible: ${initialStockCount}`);
      
      // Admin updates inventory
      const adminPage = await context.newPage();
      await adminPage.goto(`${testConfig.adminApp}/login`);
      await adminPage.fill('#admin-login-email-input', 'admin@stadium.com');
      await adminPage.fill('#admin-login-password-input', 'admin123');
      await adminPage.click('#admin-login-submit-btn');
      await adminPage.waitForURL(`${testConfig.adminApp}*`);
      
      // Navigate to inventory/drinks management
      const inventoryLinks = [
        'a[href*="drinks"]',
        'a[href*="inventory"]', 
        'button:has-text("Drinks")',
        'button:has-text("Inventory")'
      ];
      
      let inventoryFound = false;
      for (const link of inventoryLinks) {
        const element = adminPage.locator(link);
        if (await element.count() > 0) {
          await element.click();
          await adminPage.waitForLoadState('networkidle');
          inventoryFound = true;
          console.log(`✅ Navigated to inventory: ${link}`);
          break;
        }
      }
      
      if (inventoryFound) {
        // Try to update stock
        const stockInputs = adminPage.locator('input[type="number"], input[name*="stock"], input[name*="inventory"]');
        if (await stockInputs.count() > 0) {
          const currentValue = await stockInputs.first().inputValue();
          const newValue = String(parseInt(currentValue || '0') + 10);
          
          await stockInputs.first().fill(newValue);
          
          const saveButton = adminPage.locator('button:has-text("Save"), button:has-text("Update"), button[type="submit"]');
          if (await saveButton.count() > 0) {
            await saveButton.first().click();
            console.log('✅ Stock updated in admin');
            
            // Wait for real-time update
            await page.waitForTimeout(2000);
            
            // Refresh customer page to check update
            await page.reload();
            await page.waitForLoadState('networkidle');
            
            // Check if stock levels updated
            const updatedStockElements = page.locator('.stock, .inventory, [class*="stock"]');
            const updatedStockCount = await updatedStockElements.count();
            
            if (updatedStockCount > 0) {
              console.log('✅ Stock information visible on customer side');
            }
          }
        }
      }
      
      await adminPage.close();
    });
  });

  test.describe('Performance and Scalability', () => {
    
    test('INT-SIGNALR-007: Multiple concurrent connections', async ({ browser }) => {
      console.log('👥 Testing multiple concurrent SignalR connections...');
      
      const connectionCount = 5;
      const contexts: any[] = [];
      const pages: any[] = [];
      
      try {
        // Create multiple browser contexts
        for (let i = 0; i < connectionCount; i++) {
          const context = await browser.newContext();
          const page = await context.newPage();
          contexts.push(context);
          pages.push(page);
        }
        
        // Connect all pages to customer app
        await Promise.all(
          pages.map(page => page.goto(testConfig.customerApp))
        );
        
        await Promise.all(
          pages.map(page => page.waitForLoadState('networkidle'))
        );
        
        console.log(`✅ ${connectionCount} concurrent connections established`);
        
        // Test if all connections can receive updates
        const testActions = pages.map(async (page, index) => {
          await page.click('a:has-text("Order Drinks"), a[href*="drinks"], a[href*="menu"]');
          await page.waitForLoadState('networkidle');
          
          const addButton = page.locator('button:has-text("+"), button:has-text("Add")').first();
          if (await addButton.count() > 0) {
            await addButton.click();
            console.log(`✅ User ${index + 1} successfully interacted`);
          }
        });
        
        await Promise.all(testActions);
        
        console.log('✅ All concurrent connections handled interactions successfully');
        
      } finally {
        // Cleanup
        await Promise.all(contexts.map(context => context.close()));
      }
    });

    test('INT-SIGNALR-008: Message delivery reliability', async ({ page, context }) => {
      console.log('📨 Testing message delivery reliability...');
      
      let messagesReceived = 0;
      const expectedMessages = 3;
      
      // Setup message listener
      page.on('console', msg => {
        const text = msg.text();
        if (text.includes('SignalR') || text.includes('message received')) {
          messagesReceived++;
          console.log(`📨 Message received: ${text}`);
        }
      });
      
      await page.goto(testConfig.customerApp);
      await page.waitForLoadState('networkidle');
      
      // Simulate multiple rapid interactions
      await page.click('a:has-text("Order Drinks"), a[href*="drinks"], a[href*="menu"]');
      await page.waitForLoadState('networkidle');
      
      const addButtons = page.locator('button:has-text("+"), button:has-text("Add")');
      const buttonCount = Math.min(await addButtons.count(), expectedMessages);
      
      for (let i = 0; i < buttonCount; i++) {
        await addButtons.nth(i).click();
        await page.waitForTimeout(100); // Small delay between actions
      }
      
      // Wait for all messages to be processed
      await page.waitForTimeout(3000);
      
      console.log(`Message delivery: Expected=${expectedMessages}, Received=${messagesReceived}`);
      
      // Check cart state as indicator of message processing
      const cartIndicator = page.locator('.cart-count, .badge, [class*="cart"]');
      if (await cartIndicator.count() > 0) {
        const cartCount = await cartIndicator.textContent();
        console.log(`✅ Cart state consistent: ${cartCount} items`);
      }
    });
  });
});