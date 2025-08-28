# Visual Studio Docker Debug Setup - Final Solution

## Overview
This setup ensures that containers are **always recreated fresh** when starting debugging from Visual Studio and **completely cleaned up** when debugging stops. This solves the port conflict issues you've been experiencing.

## How It Works

### 🔧 Automatic Container Recreation
- **Before debugging starts**: All existing containers using the required ports are forcefully removed
- **During debugging**: Fresh containers are created with unique names to avoid conflicts
- **After debugging stops**: All containers, networks, and volumes are completely cleaned up

### 🚀 Starting Debug Session

1. **Open Visual Studio**
2. **Select the correct profile**: Choose **"Docker Compose (Fresh Start)"** from the debug dropdown
3. **Start debugging**: Press F5 or click the Start button
4. **Wait for completion**: The script will automatically:
   - Kill any processes using ports 5001, 5002, 5003, 9000, 9001, 9002
   - Remove any existing containers
   - Build fresh container images
   - Start new containers
   - Verify all services are ready

### 🛑 Stopping Debug Session

**Option 1: Automatic Cleanup**
- Simply stop debugging in Visual Studio (Shift+F5 or Stop button)
- Run the **"Docker Compose (Clean Stop)"** profile to force cleanup

**Option 2: Manual Cleanup**
- Run the cleanup script manually:
  ```powershell
  .\scripts\vs-debug-stop.ps1 "D:\StadionTest"
  ```

## Available Debug Profiles

### 🎯 Primary Profiles
1. **Docker Compose (Fresh Start)** ⭐ **RECOMMENDED**
   - Forcefully cleans up all existing containers
   - Recreates containers fresh every time
   - Automatically handles port conflicts
   - **Use this for regular debugging**

2. **Docker Compose (Clean Stop)**
   - Forcefully removes all containers and cleans up resources
   - Use this if you need to manually clean up after debugging

### 🔄 Alternative Profiles
3. **Docker Compose** - Standard Docker Compose (may have port conflicts)
4. **Docker Compose (Multi-Tab)** - Same as standard but launches browser
5. **Docker Compose (Admin)** - Launches admin interface directly
6. **Docker Compose (API)** - Launches API swagger directly

## Port Configuration
The following ports are used and automatically managed:
- **5001**: API service (http://localhost:5001)
- **5002**: Customer frontend (http://localhost:5002)
- **5003**: Admin frontend (http://localhost:5003)
- **1433**: SQL Server (internal only)

## Troubleshooting

### Port Already in Use Error
If you still get port conflicts:
1. **Use "Docker Compose (Fresh Start)" profile**
2. **Run cleanup manually**:
   ```powershell
   .\scripts\vs-debug-stop.ps1 "D:\StadionTest"
   ```
3. **Restart Docker Desktop** if needed

### Containers Not Starting
1. **Check Docker Desktop** is running
2. **Verify images build**:
   ```powershell
   docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml build
   ```
3. **Check logs**:
   ```powershell
   docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml logs
   ```

### Force Complete Reset
If all else fails, run this complete reset:
```powershell
# Stop all containers
docker stop $(docker ps -aq)

# Remove all containers
docker rm $(docker ps -aq)

# Remove all images
docker rmi $(docker images -q)

# Remove all volumes
docker volume prune -f

# Remove all networks
docker network prune -f
```

## Visual Studio Configuration

### Setting Default Profile
1. In Visual Studio, go to **Debug > Options**
2. Under **Docker Compose**, ensure **"Docker Compose (Fresh Start)"** is selected
3. This ensures fresh containers every time you debug

### Keyboard Shortcuts
- **F5**: Start debugging with fresh containers
- **Shift+F5**: Stop debugging and cleanup
- **Ctrl+F5**: Start without debugging (also uses fresh containers)

## File Structure
```
scripts/
├── vs-debug-start.ps1    # Enhanced start script with force cleanup
├── vs-debug-stop.ps1     # Enhanced stop script with force cleanup
docker-compose.vs.debug.yml  # Debug-specific compose configuration
.vs/launchSettings.json   # Visual Studio debug profiles
```

## Key Improvements
✅ **No more port conflicts** - ports are forcefully freed before starting  
✅ **Always fresh containers** - no stale data or configurations  
✅ **Automatic cleanup** - no manual intervention required  
✅ **Better error handling** - detailed logging and verification  
✅ **Multiple cleanup methods** - both automatic and manual options
