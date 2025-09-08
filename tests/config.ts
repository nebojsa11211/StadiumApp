// Test configuration file for Stadium App Playwright tests
export const testConfig = {
  // API endpoint
  api: process.env.API_URL || 'https://localhost:7000',
  
  // Application URLs - using local development ports (HTTP)
  adminApp: process.env.ADMIN_URL || 'http://localhost:7005',
  customerApp: process.env.CUSTOMER_URL || 'http://localhost:7002',
  staffApp: process.env.STAFF_URL || 'http://localhost:7006',
  
  // Test credentials - using admin account for all tests since other accounts may not exist
  credentials: {
    admin: {
      email: 'admin@stadium.com',
      password: 'password123'
    },
    customer: {
      email: 'admin@stadium.com',  // Using admin account for testing
      password: 'password123'
    },
    staff: {
      email: 'admin@stadium.com',  // Using admin account for testing
      password: 'password123',
      altEmail: 'admin@stadium.com',
      altPassword: 'password123'
    }
  },
  
  // Timeout settings for Blazor Server
  timeouts: {
    blazorLoad: 45000,   // Wait for Blazor Server to load (increased)
    navigation: 60000,    // Wait for navigation (increased)
    action: 30000,        // Wait for actions (increased)
    pageLoad: 45000,      // Wait for page to load completely
    elementWait: 20000    // Wait for elements to appear
  }
};

// Helper to get full URL for a specific app
export function getAppUrl(app: 'admin' | 'customer' | 'staff', path: string = ''): string {
  const baseUrl = testConfig[`${app}App`];
  return path ? `${baseUrl}${path}` : baseUrl;
}