# SVG Rendering Diagnostic Test

## Overview
This diagnostic test investigates why SVG sector overlays are not visible in the Stadium Overview page of the Admin application.

## Test File
- **Location**: `tests/debug-svg-rendering.spec.ts`
- **Configuration**: `playwright.simple-test.config.ts`

## Prerequisites

### 1. Ensure Services Are Running
Before running the test, start the Admin and API services:

```bash
# Terminal 1 - API
cd StadiumDrinkOrdering.API
dotnet run --launch-profile https

# Terminal 2 - Admin
cd StadiumDrinkOrdering.Admin
dotnet run --launch-profile https
```

**Service URLs:**
- API: https://localhost:7010
- Admin: https://localhost:7030

### 2. Verify Stadium Data
Ensure stadium structure and sectors exist in the database:
- Login to Admin at https://localhost:7030/login
- Navigate to Stadium Overview
- Verify sectors are present in the database

## Running the Test

### Command
```bash
npx playwright test --config=playwright.simple-test.config.ts
```

### What the Test Does

The diagnostic test performs comprehensive DOM inspection:

#### 1. Login Process
- Navigates to Admin login page
- Authenticates as admin user
- Verifies successful login

#### 2. Navigation
- Goes to Stadium Overview page (`/admin/stadium-overview`)
- Waits for network idle
- Additional 5-second wait for full rendering

#### 3. SVG Overlay Inspection
Checks the `#admin-stadium-overview-svg-overlay` element:
- **Existence**: Whether the element exists in DOM
- **Visibility**: CSS `isVisible()` check
- **Computed Styles**:
  - display
  - opacity
  - z-index
  - position
  - width/height
  - top/left
  - visibility
  - transform
  - pointer-events
- **SVG Attributes**:
  - viewBox
  - width/height attributes
  - preserveAspectRatio
  - innerHTML preview

#### 4. Sector Groups Inspection
Examines `.sector-group` elements:
- Count of sector groups found
- For each group (up to 3):
  - ID and class attributes
  - Visibility status
  - Computed styles (display, opacity, visibility)
  - Bounding box coordinates

#### 5. SVG Paths Inspection
Analyzes `.sector-path` elements:
- Count of sector paths found
- For each path (up to 3):
  - ID and class attributes
  - Path data (`d` attribute)
  - Computed styles:
    - fill color
    - stroke color
    - stroke-width
    - opacity
    - display

#### 6. Parent Containers
Checks parent container elements:
- **Stadium Image Container** (`#admin-stadium-overview-stadium-image-container`):
  - Position
  - Width/height
  - Overflow
  - Display
- **Stadium Image** (`#admin-stadium-overview-stadium-image`):
  - Image src
  - Natural dimensions
  - Load status
  - Computed dimensions

#### 7. Console Monitoring
Captures all console output:
- Regular console messages
- Errors and warnings
- Page errors
- Network issues

#### 8. Screenshot Capture
Takes full-page screenshot: `svg-debug-screenshot.png`

## Expected Output

The test generates a comprehensive diagnostic report in the console:

```
========================================
SVG RENDERING DIAGNOSTIC TEST
========================================

Step 1: Logging in as admin...
✅ Login successful

Step 2: Navigating to Stadium Overview...
✅ Navigation complete

Step 3: Waiting 5 seconds for full page load...
✅ Wait complete

========================================
DOM INSPECTION: SVG OVERLAY
========================================

SVG Overlay Element:
  Exists: true/false
  Visible: true/false
  Display: block/none
  Opacity: 1/0
  Z-Index: auto/1000
  Position: absolute/relative
  Width: XXXpx
  Height: XXXpx
  ...

========================================
SECTOR GROUPS INSPECTION
========================================

Sector Groups Found: X
Group 1:
  ID: sector-group-1
  Visible: true/false
  ...

========================================
SVG PATHS INSPECTION
========================================

Sector Paths Found: X
Path 1:
  d: M 100 100 L 200 200...
  fill: rgb(...)
  stroke: rgb(...)
  ...

========================================
CONSOLE ERRORS & WARNINGS
========================================

✅ No console errors or warnings detected
OR
⚠️ Found X console errors/warnings:
1. [error] ...
2. [warning] ...

========================================
DIAGNOSTIC SUMMARY
========================================

Key Findings:
  ✓ SVG Overlay Exists: true/false
  ✓ SVG Overlay Visible: true/false
  ✓ Sector Groups Found: X
  ✓ Sector Paths Found: X
  ✓ Console Errors: X

========================================
TEST COMPLETE
========================================
```

## Troubleshooting

### Test Fails to Login
**Problem**: Cannot authenticate as admin
**Solution**:
1. Verify API is running on https://localhost:7010
2. Check database contains admin user
3. Reset admin password if needed

### Test Times Out
**Problem**: Test exceeds 60-second timeout
**Solution**:
1. Increase timeout in config file
2. Check if services are responsive
3. Verify network connectivity

### No SVG Elements Found
**Problem**: SVG overlay doesn't exist in DOM
**Possible Causes**:
1. Stadium structure not loaded from database
2. JavaScript error preventing rendering
3. Component not initialized properly
**Solution**: Check console errors in test output

### SVG Exists But Not Visible
**Problem**: SVG element exists but `isVisible()` returns false
**Possible Causes**:
1. CSS display: none
2. Opacity: 0
3. Width/height: 0
4. Positioned off-screen
5. Z-index issue (behind other elements)
**Solution**: Examine computed styles in test output

### Paths Have No Fill/Stroke
**Problem**: Paths exist but have no visible styling
**Possible Causes**:
1. CSS not loaded
2. Incorrect color values (transparent, white on white)
3. Fill: none or stroke: none
**Solution**: Check path computed styles

## Analyzing Results

### Key Indicators of Success
- ✅ SVG overlay exists: `true`
- ✅ SVG overlay visible: `true`
- ✅ Sector groups found: `> 0`
- ✅ Sector paths found: `> 0`
- ✅ Paths have valid `d` attribute
- ✅ Paths have fill or stroke color
- ✅ No console errors

### Common Issues

#### Issue: SVG Not Visible Despite Existing
**Check**:
1. Z-index value (should be high enough to be above image)
2. Position value (should be absolute)
3. Opacity (should be 1 or close to 1)
4. Width/height (should match container dimensions)

#### Issue: No Sector Groups Found
**Check**:
1. Database contains sector data
2. JavaScript successfully fetched sectors from API
3. Rendering logic executed without errors
4. Class names match expected values

#### Issue: Paths Have Invalid Coordinates
**Check**:
1. Coordinate calculation logic
2. Image dimensions vs SVG viewBox
3. Sector coordinate data in database

## HTML Report

After running the test, open the HTML report:

```bash
# Report location
playwright-report-svg-debug/index.html
```

The report includes:
- Test execution timeline
- Console logs
- Screenshots
- Network activity
- Video recording (if test fails)

## Next Steps After Diagnosis

Based on test results, potential fixes:

### If SVG Doesn't Exist
1. Check database connection
2. Verify API endpoints returning sector data
3. Review component initialization logic

### If SVG Exists But Not Visible
1. Fix CSS positioning/z-index
2. Adjust opacity values
3. Verify viewBox matches image dimensions

### If Paths Missing or Incorrect
1. Debug coordinate calculation
2. Fix path data generation
3. Verify sector polygon coordinates

### If Console Errors Present
1. Fix JavaScript errors first
2. Resolve API connection issues
3. Handle missing data gracefully

## Contact & Support
- Review CLAUDE.md for full project documentation
- Check Stadium Overview section for architecture details
- See SVG rendering implementation in `StadiumSvgRenderer.razor`
