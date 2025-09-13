# Login Troubleshooting Guide

## Issue: Cannot login with admin@stadium.com/admin123

## Root Cause Analysis

The login issue is likely due to one of the following:

1. **Database not initialized**: The database may not be created yet, so the seeded admin user doesn't exist
2. **API service not running**: The API service might not be accessible
3. **Network connectivity**: The Admin app can't reach the API
4. **CORS issues**: Cross-origin requests might be blocked

## Verification Steps

### 1. Check Database Initialization
The database should be automatically created when the API starts, with the admin user seeded:
- Email: admin@stadium.com
- Password: admin123
- Role: Admin

### 2. Check API Accessibility
The Admin app is configured to use:
- Base URL: https://api:8443/ (from appsettings.json)
- Login endpoint: POST https://api:8443/api/auth/login

### 3. Common Issues and Solutions

#### Issue: Database not created
**Solution**: Ensure the API service is running first to initialize the database

#### Issue: API URL incorrect
**Solution**: Update the API URL in appsettings.json to match your environment

#### Issue: CORS blocking requests
**Solution**: The CORS configuration has been updated to allow proper origins

## Testing the Login

### Manual API Test
You can test the login API directly:
```bash
curl -k -X POST https://localhost:9010/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@stadium.com","password":"admin123"}'
```

### Expected Response
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@stadium.com",
    "role": "Admin"
  },
  "expiresAt": "2025-08-25T13:22:26.123Z"
}
```

## Quick Fix Steps

1. **Start the API service first** to ensure database initialization
2. **Wait 30 seconds** for database seeding to complete
3. **Try logging in** with admin@stadium.com/admin123
4. **Check browser console** for any network errors

## Environment-Specific Configuration

### For Docker Environment
- API URL: https://api:8443/
- Admin app URL: https://localhost:9030

### For Local Development
- API URL: https://localhost:7010/
- Admin app URL: https://localhost:7030

## Debug Commands

### Check if API is running
```bash
curl -k https://localhost:9010/health
```

### Check if database has admin user
```bash
# This would require database access, but the API should handle initialization
```

## Next Steps
1. Start the complete application using docker-compose
2. Wait for all services to be ready
3. Navigate to https://localhost:9030/login
4. Use admin@stadium.com/admin123 to login
