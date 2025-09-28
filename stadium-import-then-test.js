const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({
    headless: false,
    args: ['--ignore-certificate-errors', '--disable-web-security']
  });

  const context = await browser.newContext({
    viewport: { width: 1920, height: 1080 },
    ignoreHTTPSErrors: true
  });

  const page = await context.newPage();

  try {
    console.log('🚀 Testing stadium with proper data import first...');

    // Navigate directly to admin login
    console.log('📋 Navigating to admin login page...');
    await page.goto('https://localhost:9030/login');
    await page.waitForTimeout(3000);

    // Take screenshot of login page
    await page.screenshot({ path: 'stadium-import-01-login.png' });

    // Login with admin credentials
    console.log('🔐 Logging in as admin...');
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for login to complete - be more patient
    try {
      await page.waitForURL('**/admin**', { timeout: 15000 });
      console.log('✅ Login successful, redirected to admin dashboard');
    } catch (e) {
      console.log('⏳ Login taking longer, waiting more...');
      await page.waitForTimeout(5000);
    }

    // Take screenshot after login
    await page.screenshot({ path: 'stadium-import-02-after-login.png' });

    // First, let's go to structure management to import stadium data
    console.log('📁 Navigating to structure management...');
    await page.goto('https://localhost:9030/admin/stadium-structure');
    await page.waitForTimeout(3000);

    // Take screenshot of structure management
    await page.screenshot({ path: 'stadium-import-03-structure-page.png' });

    // Check if we have a demo button or import functionality
    const structureState = await page.evaluate(() => {
      return {
        hasImportBtn: !!document.querySelector('input[type="file"]'),
        hasGenerateBtn: !!document.querySelector('button:has-text("Generate")'),
        hasClearBtn: !!document.querySelector('button:has-text("Clear")'),
        currentText: document.body.innerText.substring(0, 1000)
      };
    });

    console.log('🏗️ Structure management state:', structureState);

    // Try to use the sample import
    console.log('📄 Attempting to import standard stadium sample...');

    // Look for sample download links or import buttons
    const sampleButtons = [
      'a[href*="standard"]',
      'button:has-text("Standard")',
      'a:has-text("standard-stadium.json")',
      'button:has-text("Import")'
    ];

    for (const selector of sampleButtons) {
      try {
        if (await page.isVisible(selector)) {
          console.log(`🔘 Found sample button: ${selector}`);
          await page.click(selector);
          await page.waitForTimeout(2000);
          break;
        }
      } catch (e) {
        // Continue to next selector
      }
    }

    // If we can't find samples, let's try to manually create stadium data via API
    console.log('🛠️ Trying to create demo stadium data via JavaScript...');

    const stadiumCreated = await page.evaluate(async () => {
      try {
        // Create a basic stadium structure
        const stadiumData = {
          name: "Demo Stadium",
          description: "Demo stadium for testing CSS layout",
          tribunes: [
            {
              code: "N",
              name: "North Tribune",
              rings: [{
                number: 1,
                name: "Lower Ring",
                sectors: [{
                  code: "NA",
                  name: "Sector A",
                  type: "standard",
                  rows: 20,
                  seatsPerRow: 25
                }, {
                  code: "NB",
                  name: "Sector B",
                  type: "standard",
                  rows: 20,
                  seatsPerRow: 25
                }]
              }]
            },
            {
              code: "S",
              name: "South Tribune",
              rings: [{
                number: 1,
                name: "Lower Ring",
                sectors: [{
                  code: "SA",
                  name: "Sector A",
                  type: "standard",
                  rows: 20,
                  seatsPerRow: 25
                }, {
                  code: "SB",
                  name: "Sector B",
                  type: "standard",
                  rows: 20,
                  seatsPerRow: 25
                }]
              }]
            },
            {
              code: "E",
              name: "East Tribune",
              rings: [{
                number: 1,
                name: "Lower Ring",
                sectors: [{
                  code: "EA",
                  name: "Sector A",
                  type: "standard",
                  rows: 15,
                  seatsPerRow: 20
                }]
              }]
            },
            {
              code: "W",
              name: "West Tribune",
              rings: [{
                number: 1,
                name: "Lower Ring",
                sectors: [{
                  code: "WA",
                  name: "Sector A",
                  type: "standard",
                  rows: 15,
                  seatsPerRow: 20
                }]
              }]
            }
          ]
        };

        // Try to call the import API
        const formData = new FormData();
        const blob = new Blob([JSON.stringify(stadiumData, null, 2)], { type: 'application/json' });
        formData.append('file', blob, 'demo-stadium.json');

        const response = await fetch('/api/stadium-structure/import', {
          method: 'POST',
          body: formData
        });

        return {
          success: response.ok,
          status: response.status,
          text: await response.text()
        };
      } catch (error) {
        return {
          success: false,
          error: error.message
        };
      }
    });

    console.log('🏟️ Stadium creation result:', stadiumCreated);

    // Now navigate to stadium overview to test the layout
    console.log('🎯 Navigating to stadium overview to test CSS fixes...');
    await page.goto('https://localhost:9030/admin/stadium-overview');
    await page.waitForTimeout(5000);

    // Take screenshot of stadium overview
    await page.screenshot({ path: 'stadium-import-04-overview.png' });

    // Wait for stadium to load or show data
    let attempts = 0;
    while (attempts < 15) {
      const state = await page.evaluate(() => {
        return {
          hasLoading: !!document.querySelector('#admin-stadium-loading-state'),
          hasError: !!document.querySelector('#admin-stadium-error-state'),
          hasEmpty: !!document.querySelector('#admin-stadium-empty-state'),
          hasGrid: !!document.querySelector('#admin-stadium-grid-layout'),
          loadingVisible: document.querySelector('#admin-stadium-loading-state')?.offsetParent !== null,
          gridVisible: document.querySelector('#admin-stadium-grid-layout')?.offsetParent !== null
        };
      });

      console.log(`Attempt ${attempts + 1}: Loading=${state.loadingVisible}, Grid=${state.gridVisible}`);

      if (!state.loadingVisible && state.gridVisible) {
        console.log('✅ Stadium layout loaded!');
        break;
      }

      attempts++;
      await page.waitForTimeout(1000);
    }

    // Test the final stadium layout with CSS fixes
    const finalTest = await page.evaluate(() => {
      const grid = document.querySelector('#admin-stadium-grid-layout');
      const field = document.querySelector('#admin-stadium-field');
      const stands = document.querySelectorAll('.stadium-stand');

      if (!grid) return { success: false, reason: 'No grid layout found' };

      const gridStyles = window.getComputedStyle(grid);
      const gridRect = grid.getBoundingClientRect();

      let fieldInfo = null;
      if (field) {
        const fieldStyles = window.getComputedStyle(field);
        const fieldRect = field.getBoundingClientRect();
        fieldInfo = {
          gridArea: fieldStyles.gridArea,
          width: fieldRect.width,
          height: fieldRect.height,
          backgroundColor: fieldStyles.backgroundColor
        };
      }

      return {
        success: true,
        grid: {
          display: gridStyles.display,
          gridTemplateColumns: gridStyles.gridTemplateColumns,
          gridTemplateRows: gridStyles.gridTemplateRows,
          width: gridRect.width,
          height: gridRect.height
        },
        field: fieldInfo,
        standsCount: stands.length,
        stands: Array.from(stands).map(stand => {
          const rect = stand.getBoundingClientRect();
          const styles = window.getComputedStyle(stand);
          return {
            width: rect.width,
            height: rect.height,
            gridArea: styles.gridArea,
            className: stand.className
          };
        })
      };
    });

    console.log('🎯 Final CSS Grid Test Results:');
    if (finalTest.success) {
      console.log(`✅ Grid Display: ${finalTest.grid.display}`);
      console.log(`✅ Grid Columns: ${finalTest.grid.gridTemplateColumns}`);
      console.log(`✅ Grid Rows: ${finalTest.grid.gridTemplateRows}`);
      console.log(`✅ Grid Size: ${finalTest.grid.width}x${finalTest.grid.height}`);
      console.log(`✅ Stands Count: ${finalTest.standsCount}`);

      if (finalTest.field) {
        console.log(`✅ Field Grid Area: ${finalTest.field.gridArea}`);
        console.log(`✅ Field Size: ${finalTest.field.width}x${finalTest.field.height}`);
        console.log(`✅ Field Background: ${finalTest.field.backgroundColor}`);
      }

      if (finalTest.standsCount === 4) {
        console.log('🎉 SUCCESS: All 4 tribunes are visible!');
        finalTest.stands.forEach((stand, i) => {
          console.log(`   Tribune ${i+1}: ${stand.width}x${stand.height} (${stand.gridArea})`);
        });
      }

      // Check if CSS fixes are working
      const cssFixesWorking =
        finalTest.grid.display === 'grid' &&
        finalTest.grid.gridTemplateColumns.includes('300px') &&
        finalTest.grid.gridTemplateRows.includes('200px') &&
        finalTest.field?.gridArea === 'field' &&
        finalTest.standsCount === 4;

      if (cssFixesWorking) {
        console.log('🎉 SUCCESS: CSS Grid Layout Fixes Are Working Perfectly!');
        console.log('✅ Stadium displays with large central field');
        console.log('✅ All 4 tribunes are properly positioned');
        console.log('✅ Grid template uses 300px 1fr 300px columns');
        console.log('✅ Grid template uses 200px 1fr 200px rows');
        console.log('✅ Field is positioned in center grid area');
      } else {
        console.log('⚠️ CSS fixes partially working but some issues remain');
      }
    } else {
      console.log(`❌ Stadium layout test failed: ${finalTest.reason}`);
    }

    // Take final screenshot
    await page.screenshot({ path: 'stadium-import-05-final-with-css-fixes.png', fullPage: true });

    console.log('\n📸 Screenshots saved:');
    console.log('- stadium-import-01-login.png');
    console.log('- stadium-import-02-after-login.png');
    console.log('- stadium-import-03-structure-page.png');
    console.log('- stadium-import-04-overview.png');
    console.log('- stadium-import-05-final-with-css-fixes.png');

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({ path: 'stadium-import-error.png' });
  }

  await browser.close();
})();