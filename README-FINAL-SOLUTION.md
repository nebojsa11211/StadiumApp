# Final Solution: Fix Port Conflicts in Visual Studio Docker Debugging

## 🎯 **Simple Solution - No Changes to Visual Studio**

### **Method 1: Quick Batch File (Recommended)**
**Before starting debugging in Visual Studio:**

1. **Double-click** `scripts\quick-cleanup.bat`
2. **Wait for "Cleanup Complete" message**
3. **Start debugging normally** in Visual Studio (F5)

### **Method 2: PowerShell Command**
**Run this in PowerShell before debugging:**
```powershell
cd "D:\StadionTest"
.\scripts\quick-cleanup.bat
```

### **Method 3: Manual Commands**
**Run these commands in PowerShell/Command Prompt:**
```powershell
cd "D:\StadionTest"
docker-compose -f docker-compose.yml -f docker-compose.vs.debug.yml down --remove-orphans --volumes
docker rm -f $(docker ps -aq) 2>$null
docker network prune -f
```

## 🔄 **Workflow**
1. **Before debugging**: Run cleanup script
2. **Start debugging**: Use Visual Studio normally (F5)
3. **After debugging**: Containers will stop automatically

## 📁 **Files Available**
- `scripts\quick-cleanup.bat` - **Simple double-click solution**
- `scripts\cleanup-before-debug.ps1` - **PowerShell version**
- `docker-compose.vs.debug.yml` - **Restored to original working state**

## ✅ **What This Fixes**
- **Port conflicts** (5001, 5002, 5003, 9000, 9001, 9002)
- **Container recreation** on each debug session
- **Clean startup** every time

## 🚀 **Usage Steps**
1. **Close Visual Studio** (if open)
2. **Double-click** `scripts\quick-cleanup.bat`
3. **Open Visual Studio**
4. **Start debugging** normally

**No changes to Visual Studio configuration needed - works with your existing setup!**
