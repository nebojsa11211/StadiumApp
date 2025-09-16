# Environment Variables Configuration

This document describes all environment variables required for the StadiumDrinkOrdering.API to run securely without hardcoded secrets.

## 🔐 Security Overview

All sensitive configuration has been moved from hardcoded values to environment variables. This ensures:
- **No production secrets in source code**
- **Environment-specific configuration**
- **Secure deployment practices**
- **Easy configuration management**

## Required Environment Variables

### Database Configuration

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `ConnectionStrings__DefaultConnection` | Complete PostgreSQL connection string | Yes | `Host=your-host;Port=6543;Database=postgres;Username=your-user;Password=your-pass;...` |
| `DB_HOST` | Database host (alternative to full connection string) | No | `aws-1-eu-north-1.pooler.supabase.com` |
| `DB_PORT` | Database port | No | `6543` |
| `DB_NAME` | Database name | No | `postgres` |
| `DB_USERNAME` | Database username | No | `postgres.your-project` |
| `DB_PASSWORD` | Database password | No | `your-secure-password` |

**Connection String Priority:**
1. `ConnectionStrings__DefaultConnection` (if set)
2. Individual DB_* variables (if DB_HOST, DB_USERNAME, DB_PASSWORD are all set)
3. Configuration file with placeholder substitution
4. Development fallback (SQLite)

### JWT Authentication

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `JWT_SECRET_KEY` | JWT signing secret (minimum 32 characters) | **Yes** | `YourSuperSecretKeyThatIsAtLeast32CharactersLong!` |
| `JWT_ISSUER` | JWT token issuer | No | `StadiumDrinkOrdering` |
| `JWT_AUDIENCE` | JWT token audience | No | `StadiumDrinkOrdering` |

**Security Requirements:**
- JWT_SECRET_KEY must be at least 32 characters long
- Use cryptographically strong random values
- Different secrets for different environments

### Supabase Configuration

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `SUPABASE_URL` | Supabase project URL | Yes | `https://your-project.supabase.co` |
| `SUPABASE_API_KEY` | Supabase anonymous/public API key | Yes | `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...` |
| `SUPABASE_SERVICE_KEY` | Supabase service role key (if needed) | No | `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...` |

### Stripe Payment Configuration

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `STRIPE_PUBLISHABLE_KEY` | Stripe publishable key | Yes | `pk_test_51234567890abcdefghijklmnopqr...` |
| `STRIPE_SECRET_KEY` | Stripe secret key | **Yes** | `sk_test_51234567890abcdefghijklmnopqr...` |
| `STRIPE_WEBHOOK_SECRET` | Stripe webhook endpoint secret | Yes | `whsec_1234567890abcdefghijklmnopqr...` |

### CORS Configuration

| Variable | Description | Required | Example |
|----------|-------------|----------|---------|
| `CORS_ALLOWED_ORIGINS` | Comma-separated list of allowed origins | No | `https://myapp.com,https://admin.myapp.com` |

**Default CORS Origins (Development):**
- `https://localhost:7010` (API)
- `https://localhost:7020` (Customer)
- `https://localhost:7030` (Admin)
- `https://localhost:7040` (Staff)
- `https://admin:9030` (Docker Admin)
- `https://customer:9020` (Docker Customer)
- `https://staff:9040` (Docker Staff)

## Environment Setup Examples

### Development (.env file)

```bash
# Database (Local PostgreSQL or Supabase)
DB_HOST=localhost
DB_PORT=5432
DB_NAME=stadium_dev
DB_USERNAME=postgres
DB_PASSWORD=your_dev_password

# JWT Configuration
JWT_SECRET_KEY=DevSecretKeyForTesting123456789012345!
JWT_ISSUER=StadiumDrinkOrdering
JWT_AUDIENCE=StadiumDrinkOrdering

# Supabase (Development Project)
SUPABASE_URL=https://your-dev-project.supabase.co
SUPABASE_API_KEY=your_dev_supabase_key
SUPABASE_SERVICE_KEY=your_dev_service_key

# Stripe (Test Keys)
STRIPE_PUBLISHABLE_KEY=pk_test_your_test_publishable_key
STRIPE_SECRET_KEY=sk_test_your_test_secret_key
STRIPE_WEBHOOK_SECRET=whsec_your_test_webhook_secret

# CORS (Development)
CORS_ALLOWED_ORIGINS=https://localhost:7020,https://localhost:7030,https://localhost:7040
```

### Production Environment Variables

```bash
# Database (Production PostgreSQL/Supabase)
ConnectionStrings__DefaultConnection="Host=your-prod-host;Port=6543;Database=postgres;Username=postgres.yourproject;Password=your_secure_prod_password;Ssl Mode=Require;Trust Server Certificate=true;Connection Timeout=120;Command Timeout=600;Keepalive=60;Connection Idle Lifetime=600;Maximum Pool Size=20;Minimum Pool Size=2;Pooling=true;No Reset On Close=true;Include Error Detail=true;Read Buffer Size=8192;Write Buffer Size=8192;Socket Receive Buffer Size=8192;Socket Send Buffer Size=8192"

# JWT Configuration (Production)
JWT_SECRET_KEY=YourSuperSecureProductionKeyThatIsVeryLongAndRandomGenerated123456789!
JWT_ISSUER=StadiumDrinkOrdering
JWT_AUDIENCE=StadiumDrinkOrdering

# Supabase (Production Project)
SUPABASE_URL=https://your-prod-project.supabase.co
SUPABASE_API_KEY=your_prod_supabase_key
SUPABASE_SERVICE_KEY=your_prod_service_key

# Stripe (Live Keys)
STRIPE_PUBLISHABLE_KEY=pk_live_your_live_publishable_key
STRIPE_SECRET_KEY=sk_live_your_live_secret_key
STRIPE_WEBHOOK_SECRET=whsec_your_live_webhook_secret

# CORS (Production Domains)
CORS_ALLOWED_ORIGINS=https://yourdomain.com,https://admin.yourdomain.com,https://staff.yourdomain.com
```

### Docker Compose Environment

```yaml
version: '3.8'
services:
  api:
    build: ./StadiumDrinkOrdering.API
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=stadium;Username=postgres;Password=${DB_PASSWORD};Ssl Mode=Disable
      - JWT_SECRET_KEY=${JWT_SECRET_KEY}
      - STRIPE_SECRET_KEY=${STRIPE_SECRET_KEY}
      - SUPABASE_URL=${SUPABASE_URL}
      - SUPABASE_API_KEY=${SUPABASE_API_KEY}
      - CORS_ALLOWED_ORIGINS=https://customer:9020,https://admin:9030,https://staff:9040
    depends_on:
      - db

  db:
    image: postgres:15
    environment:
      - POSTGRES_DB=stadium
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=${DB_PASSWORD}
```

## Security Best Practices

### 1. Secret Generation

```bash
# Generate secure JWT secret (Linux/macOS)
openssl rand -base64 32

# Generate secure JWT secret (PowerShell)
[System.Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
```

### 2. Environment Variable Storage

**Development:**
- Use `.env` files (add to `.gitignore`)
- Use development-specific values
- Never commit real credentials

**Production:**
- Use cloud provider secret management (AWS Secrets Manager, Azure Key Vault, etc.)
- Use container orchestration secrets (Kubernetes Secrets, Docker Swarm Secrets)
- Use environment variable injection in CI/CD pipelines

**Docker:**
- Use `.env` files for docker-compose
- Use Docker secrets for production
- Use build-time vs runtime secrets appropriately

### 3. Validation

The application validates:
- JWT secret key minimum length (32 characters)
- Required environment variables at startup
- Connection string format and accessibility
- Stripe key format validation

### 4. Fallback Behavior

**Secure Fallbacks:**
- Development-only placeholders in appsettings.Development.json
- Clear error messages for missing production secrets
- No hardcoded production values

**Error Handling:**
- Application fails fast if required secrets are missing
- Clear error messages indicate which environment variables are needed
- No secret values exposed in error logs

## Migration from Hardcoded Values

### Before (Insecure)
```csharp
var secretKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLong!"; // ❌ Hardcoded
var connectionString = "Host=aws-1-eu-north-1.pooler.supabase.com;Username=postgres.abc;Password=secret123;"; // ❌ Hardcoded
```

### After (Secure)
```csharp
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? configuration["JwtSettings:SecretKey"]; // ✅ Environment variable first

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
{
    throw new InvalidOperationException("JWT_SECRET_KEY must be set and at least 32 characters"); // ✅ Validation
}
```

## Troubleshooting

### Common Issues

**1. "JWT Secret Key is not configured"**
- Ensure `JWT_SECRET_KEY` environment variable is set
- Verify the key is at least 32 characters long
- Check environment variable naming (case sensitive on Linux)

**2. "Database connection failed"**
- Verify database environment variables are set correctly
- Check network connectivity to database host
- Validate connection string format

**3. "CORS policy errors"**
- Set `CORS_ALLOWED_ORIGINS` with your domain(s)
- Ensure URLs match exactly (including protocol and port)
- Check for trailing slashes in URLs

### Environment Variable Verification

```bash
# Check if environment variables are set (Linux/macOS)
echo $JWT_SECRET_KEY
echo $DB_HOST

# Check if environment variables are set (Windows)
echo %JWT_SECRET_KEY%
echo %DB_HOST%

# PowerShell
$env:JWT_SECRET_KEY
$env:DB_HOST
```

### Application Startup Logs

The application logs all configuration sources and final values (with passwords masked) on startup to help with debugging.

## Integration with CI/CD

### GitHub Actions Example

```yaml
name: Deploy API
on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Deploy to Production
        env:
          JWT_SECRET_KEY: ${{ secrets.JWT_SECRET_KEY }}
          ConnectionStrings__DefaultConnection: ${{ secrets.DATABASE_CONNECTION_STRING }}
          STRIPE_SECRET_KEY: ${{ secrets.STRIPE_SECRET_KEY }}
          SUPABASE_API_KEY: ${{ secrets.SUPABASE_API_KEY }}
        run: |
          # Your deployment commands here
```

### Azure DevOps Example

```yaml
variables:
  - group: 'Production-Secrets'  # Variable group with secured variables

steps:
  - task: DotNetCoreCLI@2
    displayName: 'Run API'
    env:
      JWT_SECRET_KEY: $(JWT_SECRET_KEY)
      ConnectionStrings__DefaultConnection: $(DATABASE_CONNECTION_STRING)
      STRIPE_SECRET_KEY: $(STRIPE_SECRET_KEY)
```

This configuration ensures that no production secrets are stored in source code while maintaining security and ease of deployment across different environments.