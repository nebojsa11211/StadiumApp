# Docker HTTPS Setup Guide

## Overview
This guide explains how to set up HTTPS support for all Docker containers in the Stadium Drink Ordering System. The solution provides secure HTTPS access while maintaining backward compatibility with HTTP for internal container communication.

## ✅ HTTPS Configuration Complete

All Docker containers now support HTTPS access with proper SSL certificates:

- **🔐 API**: https://localhost:9010 (Internal: https://api:8443)
- **🔐 Customer**: https://localhost:9020 (Internal: https://customer:8444)  
- **🔐 Admin**: https://localhost:9030 (Internal: https://admin:8445)
- **🔐 Staff**: https://localhost:9040 (Internal: https://staff:8446)

## Architecture

### Port Mapping Strategy
Each service exposes both HTTP and HTTPS ports for maximum flexibility:

| Service | External HTTPS | External HTTP | Internal HTTPS | Internal HTTP |
|---------|---------------|---------------|----------------|---------------|
| API | 9010 | 9011 | 8443 | 8080 |
| Customer | 9020 | 9021 | 8444 | 8081 |
| Admin | 9030 | 9031 | 8445 | 8082 |
| Staff | 9040 | 9041 | 8446 | 8083 |

### Certificate Management
- **Certificate Location**: `./certificates/aspnetcore.pfx`
- **Container Mount**: `/https/aspnetcore.pfx` (read-only)
- **Password**: `StadiumDev123!`
- **Trust**: Development certificate trusted on host machine

## Setup Process

### 1. Certificate Generation

Run the certificate generation script:
```powershell
# Generate development certificates for Docker
.\generate-dev-certs.ps1
```

This script:
- Cleans existing development certificates
- Generates new trusted development certificate
- Creates password-protected certificate for Docker use
- Saves certificate info and password details

### 2. Docker Configuration

#### docker-compose.yml (Main Configuration)
```yaml
services:
  api:
    environment:
      - ASPNETCORE_URLS=https://+:8443;http://+:8080
      - ASPNETCORE_HTTP_PORTS=8080
      - ASPNETCORE_HTTPS_PORTS=8443
      - ASPNETCORE_Kestrel__Certificates__Default__Password=StadiumDev123!
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetcore.pfx
    ports:
      - "9010:8443"  # HTTPS
      - "9011:8080"  # HTTP
    volumes:
      - ./certificates:/https:ro
```

#### docker-compose.override.yml (Development Overrides)
The override file ensures development containers use HTTPS configuration and provides additional development ports (5001-5014).

### 3. Application Configuration

#### Program.cs Updates
Each application's Program.cs was updated to:
- Use HTTPS URLs when running in Docker containers
- Configure HTTPS API communication between services
- Enable HTTPS redirection middleware
- Handle SSL certificate validation for development

Example (Admin Program.cs):
```csharp
// Docker configuration - support both HTTP and HTTPS
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8445;http://+:8082";
    builder.WebHost.UseUrls(urls);
}

// Configure API client with HTTPS
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    var apiBaseUrl = containerEnv == "true" 
        ? "https://api:8443/" 
        : "https://localhost:7010/";
    client.BaseAddress = new Uri(apiBaseUrl);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    // Accept development certificates
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});
```

#### Dockerfile Updates
Each Dockerfile was updated to:
- Expose both HTTP and HTTPS ports
- Update health checks to try HTTPS first, fallback to HTTP
- Support certificate mounting

Example:
```dockerfile
EXPOSE 8082
EXPOSE 8445

# Health check with HTTPS support
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -k -f https://localhost:8445 || curl -f http://localhost:8082 || exit 1
```

## Usage

### Starting Services
```bash
# Start all services with HTTPS support
docker-compose up --build -d

# Check container status
docker ps

# View logs for HTTPS confirmation
docker logs stadium-admin --tail 10
```

### Expected Startup Messages
You should see logs indicating both HTTP and HTTPS binding:
```
Now listening on: https://[::]:8445
Now listening on: http://[::]:8082
```

### Testing HTTPS Access
```bash
# Test each service HTTPS endpoint
curl -k -I https://localhost:9010/api/drinks  # API
curl -k -I https://localhost:9020             # Customer
curl -k -I https://localhost:9030             # Admin  
curl -k -I https://localhost:9040             # Staff
```

## Security Considerations

### Development vs Production

#### Development (Current Setup)
- Uses self-signed development certificates
- Certificate validation disabled for service-to-service communication
- Certificates stored in repository (acceptable for development)

#### Production Recommendations
For production deployment, implement:
- **Real SSL Certificates**: Use certificates from a trusted CA (Let's Encrypt, commercial CA)
- **Certificate Management**: Use Docker secrets or external certificate management
- **Environment Variables**: Store passwords in secure environment variables or secrets
- **Network Security**: Use Docker networks with proper firewall rules

### Certificate Security
- Development certificate password: `StadiumDev123!` (change for production)
- Certificate files are mounted read-only in containers
- Certificates are trusted only on development machine

## Troubleshooting

### Common Issues

#### 1. HTTPS Connection Refused
**Problem**: `curl: (7) Failed to connect to localhost:9030`
**Solution**: 
- Verify containers are running: `docker ps`
- Check logs: `docker logs stadium-admin`
- Ensure certificate exists: `dir certificates\aspnetcore.pfx`

#### 2. SSL Certificate Errors
**Problem**: Browser shows SSL warnings
**Solution**: 
- Regenerate certificates: `.\generate-dev-certs.ps1`
- Clear browser cache and certificates
- Use `-k` flag with curl for development testing

#### 3. Container Environment Variables
**Problem**: Containers not using HTTPS configuration
**Solution**:
- Check environment variables: `docker exec stadium-admin env | grep ASPNETCORE`
- Verify docker-compose.override.yml is not conflicting
- Rebuild containers: `docker-compose up --build -d`

#### 4. Port Conflicts
**Problem**: Ports already in use
**Solution**:
- Stop existing containers: `docker-compose down`
- Check port usage: `netstat -an | findstr 9030`
- Modify port mappings in docker-compose.yml if needed

### Verification Commands

```bash
# Check certificate file exists
dir certificates\aspnetcore.pfx

# Verify container environment
docker exec stadium-admin env | grep ASPNETCORE

# Test internal container communication
docker exec stadium-customer curl -k -I https://api:8443/api/drinks

# Check container logs for startup messages
docker logs stadium-admin | findstr "listening"
```

## Files Modified

### Configuration Files
- `docker-compose.yml` - Main HTTPS configuration
- `docker-compose.override.yml` - Development HTTPS overrides
- `generate-dev-certs.ps1` - Certificate generation script

### Application Files  
- `StadiumDrinkOrdering.Admin/Program.cs` - HTTPS support and API client config
- `StadiumDrinkOrdering.Customer/Program.cs` - HTTPS support and API client config
- `StadiumDrinkOrdering.Staff/Program.cs` - HTTPS support and API client config

### Docker Files
- `StadiumDrinkOrdering.API/Dockerfile` - HTTPS port exposure and health checks
- `StadiumDrinkOrdering.Admin/Dockerfile` - HTTPS port exposure and health checks  
- `StadiumDrinkOrdering.Customer/Dockerfile` - HTTPS port exposure and health checks
- `StadiumDrinkOrdering.Staff/Dockerfile` - HTTPS port exposure and health checks

## Monitoring and Maintenance

### Health Checks
All containers include health checks that verify both HTTPS and HTTP endpoints:
- Primary check: HTTPS endpoint with `-k` flag (accepts self-signed certificates)
- Fallback check: HTTP endpoint
- Interval: 30 seconds, timeout: 10 seconds, retries: 3

### Log Monitoring
Monitor container logs for SSL/TLS related errors:
```bash
# Monitor all containers
docker-compose logs -f

# Monitor specific service
docker logs -f stadium-admin

# Check for SSL errors
docker logs stadium-admin 2>&1 | grep -i ssl
```

### Certificate Renewal
Development certificates expire after 1 year. To renew:
1. Run `.\generate-dev-certs.ps1`
2. Restart containers: `docker-compose restart`
3. Clear browser cache if needed

## Success Indicators

✅ **Container Status**: All containers show "healthy" status
✅ **Port Mapping**: Both HTTP and HTTPS ports correctly mapped  
✅ **Logs**: Startup logs show binding to both HTTP and HTTPS addresses
✅ **Certificate**: Browser accepts self-signed certificate (with warning)
✅ **API Communication**: Services successfully communicate via HTTPS internally
✅ **External Access**: All applications accessible via HTTPS from host machine

## Next Steps

1. **Browser Access**: Open https://localhost:9030 for Admin panel
2. **Integration Testing**: Verify all application features work over HTTPS
3. **Performance Testing**: Ensure HTTPS doesn't significantly impact performance
4. **Production Planning**: Plan certificate management strategy for production deployment