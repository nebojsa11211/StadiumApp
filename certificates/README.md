# SSL Certificates Directory

This directory contains SSL certificates used for HTTPS in Docker containers.

## Files
- `aspnetcore.pfx` - Development SSL certificate for ASP.NET Core applications
- `cert-info.txt` - Certificate information and password details

## Certificate Details
- **Type**: ASP.NET Core Development Certificate
- **Password**: `StadiumDev123!`
- **Usage**: HTTPS support for Docker containers
- **Trust**: Trusted on development machine only

## Security Notice
⚠️ **Development Only**: These certificates are for development use only and should not be used in production environments.

## Regenerating Certificates
To regenerate certificates:
```powershell
# From project root directory
.\generate-dev-certs.ps1
```

This will:
1. Clean existing development certificates
2. Generate new trusted certificate
3. Update this directory with new certificate files

## Container Usage
The certificate is mounted read-only in Docker containers:
- **Container Path**: `/https/aspnetcore.pfx`
- **Mount Type**: Read-only volume mount
- **Configuration**: Set via `ASPNETCORE_Kestrel__Certificates__Default__*` environment variables

## Troubleshooting
If HTTPS is not working:
1. Verify certificate exists: `dir aspnetcore.pfx`
2. Check certificate is trusted: Try accessing https://localhost:9030
3. Regenerate if needed: `.\generate-dev-certs.ps1`
4. Restart containers: `docker-compose restart`