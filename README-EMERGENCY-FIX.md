# Emergency Fix: Restore Visual Studio Debugging

## 🚨 **IMMEDIATE ACTION REQUIRED**

### **Step 1: Complete Reset**
**Run these commands in PowerShell as Administrator:**

```powershell
# Stop everything
docker stop $(docker ps -aq)

# Remove all containers
docker rm $(docker ps -aq)

# Remove all networks
docker network prune -f

# Remove all volumes
docker volume prune -f

# Reset to original state
cd "D:\StadionTest"
git checkout HEAD -- docker-compose.vs.debug.yml
git checkout HEAD -- StadiumDrinkOrdering.AppHost/Properties/launchSettings.json
```

### **Step 2: Verify Original Files**
**Check these files are restored:**
- `docker-compose.vs.debug.yml` - should have `restart: unless-stopped`
- `StadiumDrinkOrdering.AppHost/Properties/launchSettings.json` - should have standard profiles

### **Step 3: Test Debugging**
1. **Open Visual Studio**
2. **Set StadiumDrinkOrdering.AppHost as startup project**
3. **Select "Docker Compose" profile**
4. **Press F5**

### **Step 4: If Still Broken**
**Run this complete reset:**
```powershell
# Complete Docker reset
docker system prune -a -f
docker volume prune -f
docker network prune -f

# Rebuild
cd "D:\StadionTest"
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml build --no-cache
```

### **Step 5: Manual Cleanup Script**
**Create this file: `scripts/emergency-reset.bat`**
```batch
@echo off
echo EMERGENCY RESET - Docker Cleanup
docker stop $(docker ps -aq) 2>nul
docker rm $(docker ps -aq) 2>nul
docker network prune -f
docker volume prune -f
echo Reset complete - restart Visual Studio
pause
```

## ✅ **Working Configuration**
**Your Visual Studio should now work exactly as before with:**
- Standard "Docker Compose" profile
- Original docker-compose.vs.debug.yml
- No custom scripts interfering

## 🔄 **Future Prevention**
**Before debugging, always run:**
```powershell
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down
```

**This prevents port conflicts by ensuring fresh containers.**
