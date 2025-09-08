/**
 * JSON Serialization Circular Reference Fix Tests
 * 
 * This test suite verifies that the JSON serialization fixes are working correctly
 * by testing API endpoints that previously failed due to circular reference issues.
 * 
 * Fixes tested:
 * 1. Program.cs: ReferenceHandler.Preserve configuration
 * 2. User.cs: [JsonIgnore] on Orders navigation property
 * 3. Drink.cs: [JsonIgnore] on OrderItems navigation property
 */

import { test, expect } from '@playwright/test';

const BASE_URL = 'http://localhost:7070';
const API_BASE_URL = `${BASE_URL}/api`;

// Test configuration
test.describe('JSON Serialization Circular Reference Fix Tests', () => {
  let adminToken: string;

  // Set up authentication token for protected endpoints
  test.beforeAll(async ({ request }) => {
    console.log('Setting up admin authentication for JSON serialization tests...');
    
    const loginResponse = await request.post(`${API_BASE_URL}/auth/login`, {
      data: {
        email: 'admin@stadium.com',
        password: 'Admin123!'
      }
    });
    
    if (loginResponse.ok()) {
      const loginData = await loginResponse.json();
      adminToken = loginData.token;
      console.log('Admin authentication successful');
    } else {
      console.warn('Admin login failed, some tests may be skipped');
    }
  });

  test('GET /api/orders - Should return valid JSON without circular references', async ({ request }) => {
    console.log('Testing GET /api/orders for JSON serialization...');

    const response = await request.get(`${API_BASE_URL}/orders`, {
      headers: adminToken ? {
        'Authorization': `Bearer ${adminToken}`
      } : {}
    });

    // Verify response status (should be 200, not 500 internal server error)
    expect(response.status()).toBe(200);

    // Get response text to check for circular reference errors
    const responseText = await response.text();
    
    // Check for circular reference error messages
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('ReferenceHandler');
    expect(responseText).not.toContain('System.Text.Json.JsonException');

    // Verify it's valid JSON
    let jsonData;
    expect(() => {
      jsonData = JSON.parse(responseText);
    }).not.toThrow();

    // Verify the response structure
    expect(Array.isArray(jsonData)).toBe(true);
    
    if (jsonData.length > 0) {
      const firstOrder = jsonData[0];
      
      // Verify essential order fields are present
      expect(firstOrder).toHaveProperty('id');
      expect(firstOrder).toHaveProperty('status');
      expect(firstOrder).toHaveProperty('createdAt');
      expect(firstOrder).toHaveProperty('totalAmount');
      
      // Verify that User.Orders navigation property is properly ignored
      if (firstOrder.user) {
        expect(firstOrder.user).not.toHaveProperty('orders');
      }
      
      // Verify OrderItems are included (not over-filtered)
      if (firstOrder.orderItems && firstOrder.orderItems.length > 0) {
        const firstOrderItem = firstOrder.orderItems[0];
        expect(firstOrderItem).toHaveProperty('drinkId');
        expect(firstOrderItem).toHaveProperty('quantity');
        
        // Verify Drink.OrderItems navigation property is properly ignored
        if (firstOrderItem.drink) {
          expect(firstOrderItem.drink).not.toHaveProperty('orderItems');
        }
      }
    }

    console.log(`✓ Orders endpoint returned ${jsonData.length} orders with valid JSON serialization`);
  });

  test('GET /api/drinks - Should return valid JSON without circular references', async ({ request }) => {
    console.log('Testing GET /api/drinks for JSON serialization...');

    const response = await request.get(`${API_BASE_URL}/drinks`);

    // Verify response status
    expect(response.status()).toBe(200);

    // Get response text to check for errors
    const responseText = await response.text();
    
    // Check for circular reference error messages
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('ReferenceHandler');
    expect(responseText).not.toContain('System.Text.Json.JsonException');

    // Verify it's valid JSON
    let jsonData;
    expect(() => {
      jsonData = JSON.parse(responseText);
    }).not.toThrow();

    // Verify the response structure
    expect(Array.isArray(jsonData)).toBe(true);
    
    if (jsonData.length > 0) {
      const firstDrink = jsonData[0];
      
      // Verify essential drink fields are present
      expect(firstDrink).toHaveProperty('id');
      expect(firstDrink).toHaveProperty('name');
      expect(firstDrink).toHaveProperty('price');
      expect(firstDrink).toHaveProperty('category');
      
      // Verify that Drink.OrderItems navigation property is properly ignored
      expect(firstDrink).not.toHaveProperty('orderItems');
    }

    console.log(`✓ Drinks endpoint returned ${jsonData.length} drinks with valid JSON serialization`);
  });

  test('GET /api/users - Should return valid JSON without circular references', async ({ request }) => {
    if (!adminToken) {
      test.skip('Admin authentication not available, skipping users endpoint test');
      return;
    }

    console.log('Testing GET /api/users for JSON serialization...');

    const response = await request.get(`${API_BASE_URL}/users`, {
      headers: {
        'Authorization': `Bearer ${adminToken}`
      }
    });

    // Verify response status
    expect(response.status()).toBe(200);

    // Get response text to check for errors
    const responseText = await response.text();
    
    // Check for circular reference error messages
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('ReferenceHandler');
    expect(responseText).not.toContain('System.Text.Json.JsonException');

    // Verify it's valid JSON
    let jsonData;
    expect(() => {
      jsonData = JSON.parse(responseText);
    }).not.toThrow();

    // Verify the response structure
    expect(Array.isArray(jsonData)).toBe(true);
    
    if (jsonData.length > 0) {
      const firstUser = jsonData[0];
      
      // Verify essential user fields are present
      expect(firstUser).toHaveProperty('id');
      expect(firstUser).toHaveProperty('email');
      expect(firstUser).toHaveProperty('role');
      expect(firstUser).toHaveProperty('username');
      
      // Verify that User.Orders navigation property is properly ignored
      expect(firstUser).not.toHaveProperty('orders');
      
      // Verify passwordHash is not exposed in API responses
      expect(firstUser).not.toHaveProperty('passwordHash');
    }

    console.log(`✓ Users endpoint returned ${jsonData.length} users with valid JSON serialization`);
  });

  test('POST /api/customer/orders/create - Should handle complex object creation without circular references', async ({ request }) => {
    console.log('Testing POST /api/customer/orders/create for complex object JSON serialization...');

    // First, get available drinks to create a valid order
    const drinksResponse = await request.get(`${API_BASE_URL}/drinks`);
    expect(drinksResponse.status()).toBe(200);
    
    const drinks = await drinksResponse.json();
    if (drinks.length === 0) {
      test.skip('No drinks available for order creation test');
      return;
    }

    // Create a sample order with complex nested objects
    const orderData = {
      customerEmail: 'test.customer@stadium.com',
      customerName: 'Test Customer',
      orderItems: [
        {
          drinkId: drinks[0].id,
          quantity: 2,
          unitPrice: drinks[0].price
        },
        // Add multiple items to test complex serialization
        ...(drinks.length > 1 ? [{
          drinkId: drinks[1].id,
          quantity: 1,
          unitPrice: drinks[1].price
        }] : [])
      ],
      ticketToken: 'TEST-TICKET-001',
      seatInfo: {
        section: 'A',
        row: 10,
        seat: 15
      },
      paymentInfo: {
        method: 'CreditCard',
        amount: drinks[0].price * 2 + (drinks.length > 1 ? drinks[1].price : 0)
      }
    };

    const response = await request.post(`${API_BASE_URL}/customer/orders/create`, {
      data: orderData,
      headers: {
        'Content-Type': 'application/json'
      }
    });

    // Verify response status (should be 201 for creation or 200 for success, not 500)
    expect([200, 201, 400].includes(response.status())).toBe(true);

    // Get response text to check for circular reference errors
    const responseText = await response.text();
    
    // Check for circular reference error messages
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('ReferenceHandler');
    expect(responseText).not.toContain('System.Text.Json.JsonException');

    // If successful, verify JSON structure
    if (response.status() === 200 || response.status() === 201) {
      let jsonData;
      expect(() => {
        jsonData = JSON.parse(responseText);
      }).not.toThrow();

      // Verify the response contains order information
      if (jsonData.orderId || jsonData.id) {
        expect(typeof (jsonData.orderId || jsonData.id)).toBe('number');
      }
    } else if (response.status() === 400) {
      // If it's a validation error, still verify JSON is valid
      let jsonData;
      expect(() => {
        jsonData = JSON.parse(responseText);
      }).not.toThrow();
      
      console.log('Order creation returned validation error (expected for test data)');
    }

    console.log(`✓ Order creation endpoint handled complex objects without JSON serialization errors`);
  });

  test('GET /api/orders/{id} - Should return single order without circular references', async ({ request }) => {
    if (!adminToken) {
      test.skip('Admin authentication not available, skipping specific order test');
      return;
    }

    console.log('Testing GET /api/orders/{id} for individual order JSON serialization...');

    // First get list of orders to find a valid ID
    const ordersResponse = await request.get(`${API_BASE_URL}/orders`, {
      headers: {
        'Authorization': `Bearer ${adminToken}`
      }
    });
    
    expect(ordersResponse.status()).toBe(200);
    const orders = await ordersResponse.json();
    
    if (orders.length === 0) {
      test.skip('No orders available for individual order test');
      return;
    }

    const orderId = orders[0].id;
    const response = await request.get(`${API_BASE_URL}/orders/${orderId}`, {
      headers: {
        'Authorization': `Bearer ${adminToken}`
      }
    });

    // Verify response status
    expect(response.status()).toBe(200);

    // Get response text to check for errors
    const responseText = await response.text();
    
    // Check for circular reference error messages
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('ReferenceHandler');
    expect(responseText).not.toContain('System.Text.Json.JsonException');

    // Verify it's valid JSON
    let jsonData;
    expect(() => {
      jsonData = JSON.parse(responseText);
    }).not.toThrow();

    // Verify order structure with relationships
    expect(jsonData).toHaveProperty('id', orderId);
    expect(jsonData).toHaveProperty('status');
    expect(jsonData).toHaveProperty('totalAmount');
    
    // Verify navigation properties are handled correctly
    if (jsonData.user) {
      expect(jsonData.user).not.toHaveProperty('orders');
    }
    
    if (jsonData.orderItems && jsonData.orderItems.length > 0) {
      for (const orderItem of jsonData.orderItems) {
        expect(orderItem).toHaveProperty('drinkId');
        expect(orderItem).toHaveProperty('quantity');
        
        if (orderItem.drink) {
          expect(orderItem.drink).not.toHaveProperty('orderItems');
        }
      }
    }

    console.log(`✓ Individual order endpoint returned valid JSON for order ${orderId}`);
  });

  test('API Health Check - Verify JSON serialization configuration is active', async ({ request }) => {
    console.log('Performing API health check to verify JSON configuration...');

    // Test a simple endpoint to verify the API is responding with correct JSON settings
    const response = await request.get(`${API_BASE_URL}/drinks`);
    
    expect(response.status()).toBe(200);
    
    // Check response headers for JSON content type
    const contentType = response.headers()['content-type'];
    expect(contentType).toContain('application/json');
    
    // Verify camelCase naming policy is applied
    const responseText = await response.text();
    const jsonData = JSON.parse(responseText);
    
    if (jsonData.length > 0) {
      const firstItem = jsonData[0];
      
      // Check for camelCase properties (configured in Program.cs)
      expect(firstItem).toHaveProperty('id');
      expect(firstItem).toHaveProperty('name');
      expect(firstItem).toHaveProperty('price');
      expect(firstItem).toHaveProperty('stockQuantity'); // should be camelCase
      expect(firstItem).toHaveProperty('isAvailable'); // should be camelCase
      expect(firstItem).toHaveProperty('createdAt'); // should be camelCase
    }

    console.log('✓ API health check passed - JSON serialization configuration is active');
  });

  test('Error Response Format - Verify error responses use proper JSON serialization', async ({ request }) => {
    console.log('Testing error response JSON serialization...');

    // Make a request to a non-existent endpoint to trigger an error response
    const response = await request.get(`${API_BASE_URL}/nonexistent-endpoint`);
    
    // Should return 404, not 500 (which would indicate serialization error)
    expect(response.status()).toBe(404);
    
    const responseText = await response.text();
    
    // Verify no serialization errors in error responses
    expect(responseText).not.toContain('circular reference');
    expect(responseText).not.toContain('object cycle');
    expect(responseText).not.toContain('System.Text.Json.JsonException');
    
    // Error response should still be valid JSON or plain text
    if (responseText.trim().startsWith('{')) {
      expect(() => {
        JSON.parse(responseText);
      }).not.toThrow();
    }

    console.log('✓ Error responses maintain proper JSON serialization');
  });
});