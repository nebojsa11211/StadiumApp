# Admin Modernization Test Suite

Comprehensive Playwright tests validating the Stadium Drink Ordering Admin application modernization.

## 🎯 Test Coverage

### Authentication & Navigation (`auth.spec.ts`)
- Admin login/logout flows
- Session management and persistence
- Protected route validation
- Multi-context user sessions
- Navigation between modernized pages

### Dashboard Modernization (`dashboard.spec.ts`)
- KPI cards display and functionality
- Charts and visualizations
- Real-time updates and auto-refresh
- Responsive design across breakpoints
- Performance optimization validation

### Orders Management (`orders.spec.ts`)
- Order listing and data table
- Advanced filtering (status, date, search)
- Bulk operations (accept, reject, select all)
- Individual order management
- Status workflow validation
- Mobile responsiveness

### Users Management (`users.spec.ts`)
- User listing and statistics
- User creation with validation
- Role-based filtering and search
- Bulk role assignment operations
- User edit/delete functionality
- Access control validation

### Design System & Responsiveness (`responsive.spec.ts`)
- Theme switching (light/dark mode)
- Mobile, tablet, desktop breakpoints
- Component responsiveness
- Navigation menu adaptation
- Accessibility features
- Print styles

### Performance & Loading (`performance.spec.ts`)
- Page load time benchmarks
- Loading state indicators
- Real-time update handling
- Concurrent user scenarios
- Memory leak detection
- Resource optimization

### Comprehensive Suite (`all-tests.spec.ts`)
- Essential functionality validation
- Critical path workflows
- Cross-page integration tests
- End-to-end admin scenarios

## 🚀 Quick Start

### Prerequisites
- Node.js and npm installed
- Admin application running on https://localhost:9030 (Docker) or https://localhost:7030 (Local)
- Valid admin credentials: `admin@stadium.com` / `admin123`

### Installation
```bash
npm install
npx playwright install
```

### Run All Tests
```bash
npm test
```

### Run Specific Test Suites
```bash
# Authentication tests
npm run test:auth

# Dashboard tests
npm run test:dashboard

# Orders management tests
npm run test:orders

# Users management tests
npm run test:users

# Responsiveness tests
npm run test:responsive

# Performance tests
npm run test:performance

# Essential tests only
npx playwright test tests/admin/all-tests.spec.ts
```

### Debug Mode
```bash
npm run test:debug
npm run test:headed
```

## 📊 Test Reports

After running tests, view detailed reports:
```bash
npm run report
```

Reports include:
- Test execution summary
- Screenshots of failures
- Video recordings (on failures)
- Performance metrics
- Coverage analysis

## 🔧 Configuration

### Environment Variables
- `ADMIN_BASE_URL`: Override default admin URL (default: https://localhost:9030)

### Docker vs Local Testing
Tests automatically detect and work with both:
- **Docker**: https://localhost:9030 (default)
- **Local**: https://localhost:7030 (set via environment)

### Browser Support
Tests run on:
- Chromium (primary)
- Firefox
- WebKit (Safari)
- Mobile Chrome
- Mobile Safari
- Microsoft Edge

## 📋 Test Categories

### ✅ Functional Tests
- User authentication and authorization
- Page navigation and routing
- Data display and management
- Form submission and validation
- Modal dialogs and interactions

### 📱 Responsive Tests
- Mobile viewport adaptation
- Tablet layout optimization
- Desktop functionality
- Touch-friendly interactions
- Viewport-specific features

### ⚡ Performance Tests
- Page load benchmarks (< 10 seconds)
- Resource loading optimization
- Memory usage monitoring
- Concurrent user handling
- Real-time update efficiency

### 🎨 Design System Tests
- Theme switching functionality
- Component consistency
- Typography and spacing
- Color contrast validation
- Accessibility compliance

## 🐛 Debugging Failed Tests

### Screenshots
Failed tests automatically capture screenshots in `test-results/`

### Video Recordings
Test failures include video recordings showing the exact failure point

### Console Logs
All console outputs are captured and included in reports

### Network Activity
Failed tests include network request/response details

## 📈 Performance Benchmarks

### Expected Load Times
- **Dashboard**: < 10 seconds
- **Orders Page**: < 12 seconds
- **Users Page**: < 10 seconds
- **Page Navigation**: < 8 seconds

### Resource Limits
- **Total Requests**: < 50 per page
- **Total Resources**: < 10MB per page
- **Memory Growth**: < 50MB during session
- **Concurrent Users**: Support 3+ simultaneous admins

## 🔒 Security Testing

### Authentication Validation
- Invalid credential rejection
- Session timeout handling
- Route protection verification
- Multi-user session isolation

### Input Validation
- Form field validation
- SQL injection prevention
- XSS protection
- CSRF token validation

## 🤝 Contributing

### Adding New Tests
1. Create test file in appropriate directory
2. Follow existing naming conventions
3. Use helper functions from `helpers/`
4. Include comprehensive assertions
5. Add performance benchmarks where applicable

### Test Structure
```typescript
test.describe('Feature Name', () => {
  test.beforeEach(async ({ page }) => {
    await adminLogin(page);
  });

  test('should validate specific functionality', async ({ page }) => {
    // Test implementation
  });
});
```

### Helper Functions
- `adminLogin(page)` - Authenticate admin user
- `waitForLoadingComplete(page)` - Wait for page to load
- `verifyPageComponents(page, selectors)` - Validate components
- `testResponsiveBehavior(page, breakpoints)` - Test responsiveness

## 📞 Support

For test failures or issues:
1. Check console output for error details
2. Review screenshot/video evidence
3. Verify application is running and accessible
4. Confirm admin credentials are correct
5. Check network connectivity to test environment

## 🎯 Success Criteria

Tests validate that the Admin modernization includes:
- ✅ Fast, responsive user interface
- ✅ Comprehensive data management
- ✅ Mobile-friendly design
- ✅ Theme switching capability
- ✅ Real-time updates
- ✅ Robust error handling
- ✅ Accessibility compliance
- ✅ Performance optimization