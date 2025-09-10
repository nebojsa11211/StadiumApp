# Port Configuration Documentation

## Overview
This document defines the **permanent and fixed port assignments** for all Stadium Drink Ordering System projects. These ports are consistent across all run configurations (dotnet run, IIS Express, Visual Studio, Docker).

## ✅ Fixed Port Assignments

### StadiumDrinkOrdering.API
**Description**: ASP.NET Core Web API Backend with Swagger documentation
- **HTTPS**: `7010`
- **HTTP**: `7011` 
- **Docker**: `9010` → Internal: `8080`
- **URLs**:
  - Development: https://localhost:7010 | http://localhost:7011
  - Docker: http://localhost:9010
  - Swagger: https://localhost:7010/swagger | http://localhost:9010/swagger (Docker)

### StadiumDrinkOrdering.Customer  
**Description**: Blazor Server Customer Frontend Application
- **HTTPS**: `7020`
- **HTTP**: `7021`
- **Docker**: `9020` → Internal: `8081`
- **URLs**:
  - Development: https://localhost:7020 | http://localhost:7021
  - Docker: http://localhost:9020

### StadiumDrinkOrdering.Admin
**Description**: Blazor Server Admin Management Application  
- **HTTPS**: `7030`
- **HTTP**: `7031`
- **Docker**: `9030` → Internal: `8082`
- **URLs**:
  - Development: https://localhost:7030 | http://localhost:7031
  - Docker: http://localhost:9030

### StadiumDrinkOrdering.Staff
**Description**: Blazor Server Staff Operations Application
- **HTTPS**: `7040` 
- **HTTP**: `7041`
- **Docker**: `9040` → Internal: `8083`
- **URLs**:
  - Development: https://localhost:7040 | http://localhost:7041
  - Docker: http://localhost:9040

## Port Assignment Strategy

### Development Ports (7000-7099)
- **7010-7019**: API services
- **7020-7029**: Customer applications
- **7030-7039**: Admin applications  
- **7040-7049**: Staff applications
- **7050-7099**: Reserved for future services

### Docker External Ports (9000-9099) 
- **9010-9019**: API services
- **9020-9029**: Customer applications
- **9030-9039**: Admin applications
- **9040-9049**: Staff applications
- **9050-9099**: Reserved for future services

### Internal Container Ports
- **8080**: API internal port
- **8081**: Customer internal port  
- **8082**: Admin internal port
- **8083**: Staff internal port

## Configuration Files Updated

### launchSettings.json Files
All projects have standardized launch profiles:
- **http**: HTTP-only profile
- **https**: HTTPS + HTTP profile  
- **[ProjectName]**: Main project profile (HTTPS + HTTP)
- **IIS Express**: Uses same ports as direct run

### Docker Compose
Updated `docker-compose.yml` with fixed external port mappings:
```yaml
ports:
  - "9010:8080"  # API
  - "9020:8081"  # Customer  
  - "9030:8082"  # Admin
  - "9040:8083"  # Staff
```

## Running the Applications

### Individual Development (dotnet run)
```bash
# API Backend
cd StadiumDrinkOrdering.API
dotnet run --launch-profile https
# → https://localhost:7010 | http://localhost:7011

# Customer App  
cd StladiumDrinkOrdering.Customer
dotnet run --launch-profile https
# → https://localhost:7020 | http://localhost:7021

# Admin App
cd StadiumDrinkOrdering.Admin  
dotnet run --launch-profile https
# → https://localhost:7030 | http://localhost:7031

# Staff App
cd StadiumDrinkOrdering.Staff
dotnet run --launch-profile https  
# → https://localhost:7040 | http://localhost:7041
```

### Visual Studio / IIS Express
Projects use the same fixed ports as dotnet run. No more random port assignments.

### Docker Compose
```bash
docker-compose up --build -d

# Access applications:
# API: http://localhost:9010
# Customer: http://localhost:9020  
# Admin: http://localhost:9030
# Staff: http://localhost:9040
```

## Network Communication

### Inter-Service Communication (Docker)
Services communicate using internal Docker network:
```
Customer → API: http://api:8080
Admin → API: http://api:8080  
Staff → API: http://api:8080
```

### External Access
Host machine accesses via external ports:
```
Host → API: http://localhost:9010
Host → Customer: http://localhost:9020
Host → Admin: http://localhost:9030  
Host → Staff: http://localhost:9040
```

## Testing & Validation

### Health Check Endpoints
- **API Health**: https://localhost:7010/health | http://localhost:9010/health
- **API Swagger**: https://localhost:7010/swagger | http://localhost:9010/swagger

### Port Availability Testing
```bash
# Check if ports are available
netstat -an | findstr :7010
netstat -an | findstr :7020  
netstat -an | findstr :7030
netstat -an | findstr :7040
```

### Playwright Test Configuration
Update test configurations to use fixed ports:
```typescript
const API_BASE_URL = 'http://localhost:9010';
const CUSTOMER_BASE_URL = 'http://localhost:9020';
const ADMIN_BASE_URL = 'http://localhost:9030';  
const STAFF_BASE_URL = 'http://localhost:9040';
```

## Troubleshooting

### Port Conflicts
If any port is already in use:
1. Stop the conflicting process
2. Or modify the specific port in both launchSettings.json and docker-compose.yml
3. Update this documentation

### Service Discovery Issues
Ensure Docker services use internal hostnames:
- API service: `api:8080` (not localhost)
- Database: `sqlserver:1433` (if using SQL Server container)

## Migration Notes

### Previous → Current Port Mapping
| Service | Old Development | Old Docker | New Development | New Docker |
|---------|----------------|------------|-----------------|------------|
| API | 7000/7001 | 9000 | 7010/7011 | 9010 |
| Customer | 7002/7003 | 9001 | 7020/7021 | 9020 |
| Admin | 7004/7005 | 9002→9004 | 7030/7031 | 9030 |  
| Staff | 7006/7007 | 9003 | 7040/7041 | 9040 |

### Breaking Changes
- All previous bookmarks and hardcoded URLs must be updated
- CI/CD pipelines need port updates
- Environment-specific configurations require updates
- Test configurations need port adjustments

---

## Maintenance

**Last Updated**: 2025-09-10  
**Next Review**: When adding new services or major infrastructure changes

**Validation Status**: ✅ All projects tested and verified with fixed ports

**Contact**: Development Team for port assignment requests or conflicts