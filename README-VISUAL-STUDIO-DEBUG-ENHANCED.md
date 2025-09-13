# Visual Studio Docker Debug Configuration - Enhanced

This configuration ensures that Docker containers are **recreated fresh** on each debug session and **completely cleaned up** when debugging stops.

## 🚀 Enhanced Debug Experience

### What's New
- **Fresh containers on every debug start** - No stale data or cached containers
- **Complete cleanup on debug stop** - All containers, networks, and images are removed
- **Ephemeral SQL Server** - Database is reset on each debug session
- **Automatic health checks** - Services are verified before debugging starts

### Available Debug Profiles

1. **Docker Compose** - Standard debugging with existing containers
2. **Docker Compose (Fresh Start)** - **RECOMMENDED** - Recreates all containers fresh
3. **Docker Compose (Clean Stop)** - Manual cleanup when needed

## 🔧 Setup Instructions

### 1. Configure Visual Studio
1. Open the solution in Visual Studio
2. Set `docker-compose.dcproj` as the startup project
3. In the debug dropdown, select **"Docker Compose (Fresh Start)"**
4. Press F5 to start debugging with fresh containers

### 2. Automatic Container Management

#### On Debug Start:
- ✅ Stops and removes any existing containers
- ✅ Removes orphaned networks and volumes
- ✅ Builds fresh container images
- ✅ Starts all services
- ✅ Verifies all services are healthy

#### On Debug Stop:
- ✅ Stops all running containers
- ✅ Removes all containers and images
- ✅ Cleans up networks and volumes
- ✅ Frees up system resources

## 📋 Debug Profiles

### Docker Compose (Fresh Start) - RECOMMENDED
- **Command**: `scripts/vs-debug-start.ps1`
- **Behavior**: Complete fresh start with new containers
- **Use Case**: Every debug session

### Docker Compose (Clean Stop)
- **Command**: `scripts/vs-debug-stop.ps1`
- **Behavior**: Manual cleanup of all containers
- **Use Case**: When you need to clean up manually

### Standard Docker Compose
- **Behavior**: Uses existing containers if available
- **Use Case**: Quick restart without full rebuild

## 🎯 Usage

### Starting Debug Session
1. Select **"Docker Compose (Fresh Start)"** from the debug dropdown
2. Press F5 or click Start Debugging
3. Wait for the PowerShell script to complete (shows progress)
4. All services will be available at:
   - API: https://localhost:7010
   - Customer: https://localhost:7020
   - Admin: https://localhost:7030
   - Staff: https://localhost:7040

### Stopping Debug Session
1. Press Shift+F5 or click Stop Debugging
2. The cleanup script will automatically run
3. All containers will be removed

## 🔍 Troubleshooting

### Port Conflicts
If you get port conflicts, use **"Docker Compose (Fresh Start)"** which will clean up existing containers.

### Slow Startup
The first debug session will be slower due to image building. Subsequent sessions will be faster.

### Manual Cleanup
If containers get stuck, run:
```powershell
.\scripts\vs-debug-stop.ps1
```

## 📁 Files Created

- `scripts/vs-debug-start.ps1` - Fresh container startup
- `scripts/vs-debug-stop.ps1` - Complete cleanup
- `docker-compose.vs.debug.yml` - Ephemeral container configuration
- `.vs/launchSettings.json` - Enhanced debug profiles

## 🎉 Benefits

- **No stale data** between debug sessions
- **Consistent environment** every time
- **Automatic cleanup** prevents resource leaks
- **Faster iteration** with fresh containers
- **Better debugging** with clean state
