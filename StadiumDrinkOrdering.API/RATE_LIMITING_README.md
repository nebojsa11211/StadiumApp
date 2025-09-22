# Rate Limiting and Brute Force Protection Implementation

## Overview

This implementation provides comprehensive rate limiting and brute force protection for the Stadium Drink Ordering API. It includes IP-based rate limiting, progressive delays, account lockouts, and comprehensive security monitoring.

## Features

### 1. Authentication Rate Limiting
- **Login Attempts**: 5 attempts per minute per IP address
- **Registration Attempts**: 3 attempts per hour per IP address
- **Progressive Delays**: Exponential backoff for repeated failed attempts
- **Account Lockouts**: Automatic lockout after 5 failed attempts for 15 minutes
- **IP Bans**: Automatic IP ban after 20 failed attempts for 60 minutes

### 2. General API Rate Limiting
- **General Endpoints**: 100 requests per minute per IP
- **Authenticated Users**: 200 requests per minute per IP
- **Burst Protection**: 1000 requests per 15 minutes per IP

### 3. Security Headers
- **X-Frame-Options**: DENY (prevent clickjacking)
- **X-Content-Type-Options**: nosniff (prevent MIME sniffing)
- **X-XSS-Protection**: 1; mode=block (XSS protection)
- **Strict-Transport-Security**: HTTPS enforcement
- **Content-Security-Policy**: XSS and injection prevention
- **Referrer-Policy**: Privacy protection

### 4. Monitoring and Management
- **Security Statistics**: Real-time monitoring via `/security/stats`
- **Admin Controls**: Manual IP ban/account lockout removal
- **Automatic Cleanup**: Background service for expired restrictions
- **Comprehensive Logging**: All security events logged centrally

## Configuration

### Rate Limiting Settings (appsettings.json)

```json
{
  "RateLimiting": {
    "EnableRateLimiting": true,
    "Authentication": {
      "LoginAttemptsPerMinute": 5,
      "RegisterAttemptsPerHour": 3,
      "TimeWindowSeconds": 60,
      "RetryAfterSeconds": 60
    },
    "General": {
      "RequestsPerMinute": 100,
      "AuthenticatedRequestsPerMinute": 200
    },
    "BruteForce": {
      "MaxFailedAttempts": 5,
      "LockoutDurationMinutes": 15,
      "MaxFailedAttemptsPerIP": 20,
      "IPBanDurationMinutes": 60,
      "ProgressiveDelay": {
        "Enabled": true,
        "BaseDelayMs": 1000,
        "MaxDelayMs": 30000,
        "Multiplier": 2.0
      }
    }
  }
}
```

### IP Rate Limiting Rules

```json
{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*:/auth/login",
        "Period": "1m",
        "Limit": 5
      },
      {
        "Endpoint": "*:/auth/register",
        "Period": "1h",
        "Limit": 3
      },
      {
        "Endpoint": "*:/*",
        "Period": "1m",
        "Limit": 100
      }
    ]
  }
}
```

## Database Schema

### New Tables Added

#### FailedAttempts
- `Id`: Primary key
- `IPAddress`: Client IP address (max 45 chars for IPv6)
- `Email`: Email address attempted (optional)
- `AttemptTime`: Timestamp of attempt
- `AttemptType`: Type of attempt (Login, Register)
- `UserAgent`: Browser user agent string
- `Context`: Additional failure context

#### AccountLockouts
- `Id`: Primary key
- `Email`: Locked account email
- `LockoutStart`: When lockout began
- `LockoutEnd`: When lockout expires
- `FailedAttemptCount`: Number of failures that triggered lockout
- `IsActive`: Whether lockout is currently active
- `Reason`: Lockout reason description

#### IPBans
- `Id`: Primary key
- `IPAddress`: Banned IP address
- `BanStart`: When ban began
- `BanEnd`: When ban expires
- `IsActive`: Whether ban is currently active
- `ViolationCount`: Number of violations that triggered ban
- `Reason`: Ban reason description

## API Endpoints

### Security Management (Admin Only)

#### GET `/security/stats`
Returns comprehensive security statistics:
```json
{
  "activeIPBans": 2,
  "activeAccountLockouts": 1,
  "failedAttemptsLast24Hours": 45,
  "failedAttemptsLastHour": 12,
  "attemptsByType": {
    "Login": 38,
    "Register": 7
  },
  "topOffendingIPs": [
    "192.168.1.100 (15 attempts)",
    "10.0.0.50 (8 attempts)"
  ]
}
```

#### DELETE `/security/ip-bans/{ipAddress}`
Manually removes an IP ban (Admin only).

#### DELETE `/security/account-lockouts/{email}`
Manually removes an account lockout (Admin only).

#### POST `/security/cleanup`
Manually triggers cleanup of expired restrictions (Admin only).

#### GET `/security/config`
Returns current security configuration status.

## Error Responses

### Rate Limit Exceeded (429)
```json
{
  "error": "Rate limit exceeded",
  "retryAfter": "60"
}
```

### IP Banned (429)
```json
{
  "error": "IP address is temporarily banned",
  "retryAfter": "3600"
}
```

### Account Locked (423)
```json
{
  "error": "Account is temporarily locked",
  "lockoutEnd": "2024-01-01T15:30:00Z",
  "remainingMinutes": 12
}
```

## Progressive Delay Mechanism

The system implements exponential backoff for failed authentication attempts:

- **1st failure**: 1 second delay
- **2nd failure**: 2 second delay
- **3rd failure**: 4 second delay
- **4th failure**: 8 second delay
- **5th failure**: 16 second delay
- **Maximum**: 30 second delay

## Background Services

### RateLimitCleanupService
- **Runs**: Every hour
- **Purpose**: Cleanup expired IP bans, account lockouts, and old failed attempts
- **Retention**: Removes records older than 30 days

## Security Considerations

### IP Address Detection
The system correctly identifies client IP addresses even behind proxies:
1. Checks `X-Forwarded-For` header first
2. Falls back to `X-Real-IP` header
3. Uses connection remote IP as final fallback
4. Handles IPv6 loopback conversion

### Rate Limiting Headers
All rate-limited responses include appropriate headers:
- `X-RateLimit-Limit`: Request limit
- `X-RateLimit-Remaining`: Remaining requests
- `X-RateLimit-Reset`: Reset time
- `Retry-After`: Seconds to wait before retry

### Security Headers
All responses include comprehensive security headers for protection against common attacks.

## Testing

### Manual Testing
You can test the rate limiting by making multiple requests:

```bash
# Test login rate limiting
for i in {1..10}; do
  curl -X POST https://localhost:9010/auth/login \
    -H "Content-Type: application/json" \
    -d '{"email":"test@test.com","password":"wrong"}'
done
```

### Load Testing
The rate limiting is designed to handle high loads while protecting against abuse. Test with tools like Apache Bench or Artillery.js.

## Monitoring

### Key Metrics to Monitor
- Failed authentication attempts per hour
- Rate limit violations per endpoint
- IP bans and account lockouts
- Progressive delay activation frequency
- Security header compliance

### Alerting
Consider setting up alerts for:
- Spike in failed authentication attempts
- High number of IP bans
- Unusual geographic patterns in attacks
- Rate limiting service failures

## Production Deployment

### Environment Variables
```bash
# Optional: Custom rate limiting configuration
RATE_LIMIT_LOGIN_ATTEMPTS_PER_MINUTE=5
RATE_LIMIT_REGISTER_ATTEMPTS_PER_HOUR=3
RATE_LIMIT_ENABLE_PROGRESSIVE_DELAY=true

# Security headers
SECURITY_HEADERS_ENABLE_HSTS=true
SECURITY_HEADERS_ENABLE_CSP=true
```

### Load Balancer Configuration
If using a load balancer, ensure:
- `X-Forwarded-For` headers are properly set
- Real client IPs are preserved
- Health checks are excluded from rate limiting

### Database Performance
The rate limiting tables are properly indexed for performance:
- IP address and timestamp indexes
- Email and timestamp indexes
- Composite indexes for common queries

## Maintenance

### Regular Tasks
1. **Monitor Security Stats**: Check `/security/stats` regularly
2. **Review Failed Attempts**: Look for patterns in attack attempts
3. **Update Rate Limits**: Adjust limits based on legitimate usage patterns
4. **Clean Old Data**: The cleanup service handles this automatically

### Troubleshooting

#### Common Issues
1. **Legitimate users getting blocked**: Lower rate limits or implement whitelist
2. **High database load**: Ensure indexes are present and consider cleanup frequency
3. **Memory usage**: Monitor in-memory rate limiting cache size

#### Debug Mode
Enable detailed logging for troubleshooting:
```json
{
  "Logging": {
    "LogLevel": {
      "StadiumDrinkOrdering.API.Services.BruteForceProtectionService": "Debug",
      "AspNetCoreRateLimit": "Debug"
    }
  }
}
```

## Compliance

This implementation helps meet various compliance requirements:
- **GDPR**: Personal data protection and retention policies
- **SOC 2**: Security monitoring and access controls
- **OWASP**: Protection against brute force attacks (A07:2021)
- **PCI DSS**: If handling payment data, provides required security controls

## Future Enhancements

### Potential Improvements
1. **Geolocation Blocking**: Block requests from specific countries
2. **Device Fingerprinting**: Track devices for enhanced security
3. **Machine Learning**: Detect anomalous patterns automatically
4. **CAPTCHA Integration**: Add CAPTCHA after multiple failures
5. **Distributed Cache**: Use Redis for multi-instance deployments