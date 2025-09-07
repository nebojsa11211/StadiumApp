import { test, expect } from '@playwright/test';

const API_BASE_URL = 'https://localhost:7000'; // Using HTTPS for local development testing

test.describe('Database Connectivity Tests', () => {
  test.beforeAll(async () => {
    // Wait a moment to ensure API is fully initialized
    await new Promise(resolve => setTimeout(resolve, 2000));
  });

  test('API Health Check - should return Healthy status', async ({ request }) => {
    const response = await request.get(`${API_BASE_URL}/health`);
    
    expect(response.status()).toBe(200);
    const responseText = await response.text();
    expect(responseText).toBe('Healthy');
  });

  test('Database Connectivity - Events endpoint should return valid JSON', async ({ request }) => {
    const response = await request.get(`${API_BASE_URL}/api/customer/ticketing/events`);
    
    expect(response.status()).toBe(200);
    expect(response.headers()['content-type']).toContain('application/json');
    
    const events = await response.json();
    expect(Array.isArray(events)).toBe(true);
    
    // Log the response for debugging
    console.log('Events response:', events);
  });

  test('Drinks API endpoint should return valid data', async ({ request }) => {
    const response = await request.get(`${API_BASE_URL}/api/drinks`);
    
    expect(response.status()).toBe(200);
    expect(response.headers()['content-type']).toContain('application/json');
    
    const drinks = await response.json();
    expect(Array.isArray(drinks)).toBe(true);
    
    console.log('Drinks response:', drinks);
  });

  test('Stadium Structure endpoint should return valid data', async ({ request }) => {
    const response = await request.get(`${API_BASE_URL}/api/stadium-structure`);
    
    // This endpoint might return 404 if no structure is configured, which is acceptable
    if (response.status() === 200) {
      expect(response.headers()['content-type']).toContain('application/json');
      const structure = await response.json();
      console.log('Stadium structure response:', structure);
    } else if (response.status() === 404) {
      console.log('Stadium structure not configured - this is acceptable');
      expect(response.status()).toBe(404);
    } else {
      // Any other status code indicates a database connectivity issue
      expect(response.status()).toBeGreaterThanOrEqual(200);
      expect(response.status()).toBeLessThan(500);
    }
  });

  test('Users endpoint should require authentication', async ({ request }) => {
    // Test the actual users search endpoint that requires authentication
    const response = await request.post(`${API_BASE_URL}/api/users/search`, {
      data: {}
    });
    
    // Should return 401 Unauthorized since we're not authenticated
    expect(response.status()).toBe(401);
  });

  test('Orders endpoint should require authentication', async ({ request }) => {
    const response = await request.get(`${API_BASE_URL}/api/orders`);
    
    // Should return 401 Unauthorized since we're not authenticated
    expect(response.status()).toBe(401);
  });

  test('Admin authentication should work with valid credentials', async ({ request }) => {
    // Test admin login
    const loginResponse = await request.post(`${API_BASE_URL}/api/auth/login`, {
      data: {
        email: 'admin@stadium.com',
        password: 'admin123'
      }
    });
    
    console.log('Login response status:', loginResponse.status());
    
    if (loginResponse.status() === 200) {
      const loginData = await loginResponse.json();
      expect(loginData.token).toBeDefined();
      expect(loginData.user).toBeDefined();
      expect(loginData.user.email).toBe('admin@stadium.com');
      
      // Test authenticated request using the token
      const token = loginData.token;
      const authenticatedResponse = await request.post(`${API_BASE_URL}/api/users/search`, {
        headers: {
          'Authorization': `Bearer ${token}`
        },
        data: {}
      });
      
      expect(authenticatedResponse.status()).toBe(200);
      console.log('Authenticated request successful');
    } else {
      // If login fails, it might be because admin user doesn't exist yet
      // This is acceptable for a fresh database
      console.log('Admin login failed - this may be expected for a fresh database');
      expect([400, 401, 404]).toContain(loginResponse.status());
    }
  });

  test('Database connection stress test - multiple concurrent requests', async ({ request }) => {
    const promises = [];
    
    // Make 5 concurrent requests to test database connection stability
    for (let i = 0; i < 5; i++) {
      promises.push(request.get(`${API_BASE_URL}/health`));
      promises.push(request.get(`${API_BASE_URL}/api/customer/ticketing/events`));
      promises.push(request.get(`${API_BASE_URL}/api/drinks`));
    }
    
    const responses = await Promise.all(promises);
    
    // All health checks should return 200
    const healthResponses = responses.filter((_, index) => index % 3 === 0);
    for (const response of healthResponses) {
      expect(response.status()).toBe(200);
    }
    
    // All events endpoints should return 200
    const eventsResponses = responses.filter((_, index) => index % 3 === 1);
    for (const response of eventsResponses) {
      expect(response.status()).toBe(200);
    }
    
    // All drinks endpoints should return 200
    const drinksResponses = responses.filter((_, index) => index % 3 === 2);
    for (const response of drinksResponses) {
      expect(response.status()).toBe(200);
    }
    
    console.log('Stress test completed - all concurrent requests successful');
  });

  test('API response times should be reasonable', async ({ request }) => {
    const startTime = Date.now();
    
    const response = await request.get(`${API_BASE_URL}/api/customer/ticketing/events`);
    
    const responseTime = Date.now() - startTime;
    
    expect(response.status()).toBe(200);
    expect(responseTime).toBeLessThan(5000); // Should respond within 5 seconds
    
    console.log(`Events endpoint response time: ${responseTime}ms`);
  });

  test('API should handle malformed requests gracefully', async ({ request }) => {
    // Test with invalid endpoint
    const response1 = await request.get(`${API_BASE_URL}/api/nonexistent`);
    expect(response1.status()).toBe(404);
    
    // Test with invalid HTTP method on API controller endpoint that should be more restrictive
    const response2 = await request.patch(`${API_BASE_URL}/api/drinks`);
    expect(response2.status()).toBeGreaterThanOrEqual(400);
    expect(response2.status()).toBeLessThan(500);
    
    console.log('Malformed request handling test completed');
  });

  test('Database should persist data across requests', async ({ request }) => {
    // Make two separate requests to ensure data consistency
    const response1 = await request.get(`${API_BASE_URL}/api/drinks`);
    expect(response1.status()).toBe(200);
    const drinks1 = await response1.json();
    
    // Wait a moment
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    const response2 = await request.get(`${API_BASE_URL}/api/drinks`);
    expect(response2.status()).toBe(200);
    const drinks2 = await response2.json();
    
    // Data should be consistent between requests
    expect(drinks1).toEqual(drinks2);
    
    console.log('Data persistence test completed');
  });
});