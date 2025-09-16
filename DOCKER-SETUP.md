# Docker Setup Guide

## Overview
This guide explains how to set up and run the Stadium Drink Ordering System using Docker with secure environment variable configuration.

## Prerequisites
- Docker Desktop installed and running
- Docker Compose v3.8 or higher
- PowerShell (for certificate generation on Windows)

## Quick Start

### 1. Environment Configuration
Copy the environment template and configure your values:

```bash
# Copy the template file
cp .env.docker.example .env.docker

# Edit with your actual values
# Update database connection, JWT secrets, and certificate passwords
```

### 2. Generate SSL Certificates
```powershell
# Generate development SSL certificates
.\generate-dev-certs.ps1
```

### 3. Start Services
```bash
# Start all services with environment variables
docker-compose --env-file .env.docker up --build -d

# Or use the default .env.docker file that's already configured
docker-compose up --build -d
```

### 4. Verify Services
- **API**: https://localhost:9010
- **Customer**: https://localhost:9020
- **Admin**: https://localhost:9030
- **Staff**: https://localhost:9040

## Environment Variables

### Required Variables (.env.docker)

#### Database Configuration
```bash
# PostgreSQL/Supabase connection string
DB_CONNECTION_STRING=Host=your-host;Port=5432;Database=your-db;Username=your-user;Password=your-password;SSL Mode=Require;Trust Server Certificate=true
```

#### Security Configuration
```bash
# JWT authentication (minimum 32 characters)
JWT_SECRET_KEY=YourSuperSecretKeyThatIsAtLeast32CharactersLong!
JWT_ISSUER=StadiumDrinkOrdering
JWT_AUDIENCE=StladiumDrinkOrdering

# SSL certificate password
SSL_CERT_PASSWORD=YourSecureCertPassword123!
```

#### Application Settings
```bash
# Environment type
ASPNETCORE_ENVIRONMENT=Development

# Container timezone
TZ=Europe/Zagreb

# Internal API URL for service communication
API_BASE_URL=https://api:8443/

# Logging levels
LOG_LEVEL_DEFAULT=Information
LOG_LEVEL_ASPNETCORE=Information
```

## File Structure

### Main Configuration Files
- **`docker-compose.yml`** - Base service definitions with environment variable placeholders
- **`docker-compose.override.yml`** - Development-specific logging overrides
- **`.env.docker`** - Environment variables (DO NOT commit to git)
- **`.env.docker.template`** - Template with documentation
- **`.env.docker.example`** - Example values for development

### Security Files
- **`certificates/aspnetcore.pfx`** - SSL certificate for HTTPS
- **`.gitignore`** - Ensures .env.docker is not committed

## Production Deployment

### Security Checklist
1. **Generate Strong Secrets**:
   ```bash
   # Generate JWT secret key
   openssl rand -base64 32

   # Generate certificate password
   openssl rand -base64 16
   ```

2. **Update Environment Variables**:
   - Set `ASPNETCORE_ENVIRONMENT=Production`
   - Use production database connection string
   - Update JWT secret key and certificate password
   - Set appropriate log levels (Warning or Error)

3. **Certificate Management**:
   - Replace development certificates with production SSL certificates
   - Use certificates from trusted Certificate Authority
   - Update certificate path and password in environment variables

4. **Database Security**:
   - Use production PostgreSQL instance
   - Configure proper firewall rules
   - Enable SSL/TLS for database connections
   - Use dedicated database user with minimal permissions

## Development vs Production

### Development (.env.docker.example)
```bash
# Enhanced logging for debugging
LOG_LEVEL_DEFAULT=Debug
LOG_LEVEL_ASPNETCORE=Information
ASPNETCORE_ENVIRONMENT=Development

# Development database
DB_CONNECTION_STRING=Host=dev-host;Database=dev_db;...

# Development certificate
SSL_CERT_PASSWORD=StadiumDev123!
```

### Production
```bash
# Minimal logging for performance
LOG_LEVEL_DEFAULT=Warning
LOG_LEVEL_ASPNETCORE=Warning
ASPNETCORE_ENVIRONMENT=Production

# Production database with security
DB_CONNECTION_STRING=Host=prod-host;Database=prod_db;Username=prod_user;Password=strong_password;...

# Strong certificate password
SSL_CERT_PASSWORD=ComplexProductionPassword123!@#
```

## Troubleshooting

### Common Issues

#### 1. Environment Variables Not Loading
```bash
# Check if .env.docker exists
ls -la .env.docker

# Verify environment file format (no spaces around =)
cat .env.docker

# Explicitly specify env file
docker-compose --env-file .env.docker up
```

#### 2. Database Connection Errors
```bash
# Check connection string format
# Ensure password doesn't contain special characters that need escaping
# Verify PostgreSQL/Supabase credentials

# Test connection from host machine
psql "postgresql://username:password@host:port/database?sslmode=require"
```

#### 3. SSL Certificate Issues
```bash
# Regenerate certificates
.\generate-dev-certs.ps1

# Check certificate file permissions
ls -la certificates/

# Verify certificate password matches environment variable
```

#### 4. Service Communication Errors
```bash
# Check Docker network
docker network ls
docker network inspect stadium-network

# Verify service DNS resolution
docker exec stadium-customer nslookup api
```

### Container Logs
```bash
# View all service logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f api
docker-compose logs -f customer
docker-compose logs -f admin
docker-compose logs -f staff

# Check container health
docker ps
```

### Network Diagnostics
```bash
# Test internal API connectivity
docker exec stadium-customer curl -k https://api:8443/api/health

# Test external HTTPS access
curl -k https://localhost:9030
```

## Maintenance

### Updating Environment Variables
1. Stop services: `docker-compose down`
2. Update `.env.docker` file
3. Restart services: `docker-compose up -d`

### Rotating Secrets
1. Generate new JWT secret key and certificate password
2. Update `.env.docker` with new values
3. Regenerate SSL certificates if needed
4. Restart all services

### Database Migrations
```bash
# Run migrations in API container
docker-compose exec api dotnet ef database update

# Or run migrations locally with connection string
cd StadiumDrinkOrdering.API
dotnet ef database update
```

## Support

### Docker Commands Reference
```bash
# Start services
docker-compose up -d

# Stop services
docker-compose down

# Rebuild and start
docker-compose up --build -d

# View logs
docker-compose logs -f [service-name]

# Restart specific service
docker-compose restart [service-name]

# Remove all containers and networks
docker-compose down --volumes --remove-orphans
```

### Health Checks
- **API Health**: https://localhost:9010/health
- **Container Status**: `docker ps`
- **Network Status**: `docker network ls`
- **Volume Status**: `docker volume ls`