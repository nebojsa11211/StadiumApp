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
    console.log('🚀 Testing stadium CSS layout fixes on local admin app...');

    // Navigate to admin login (local app)
    console.log('📋 Navigating to local admin login...');
    await page.goto('https://localhost:7030/login');
    await page.waitForTimeout(3000);

    // Take screenshot of login page
    await page.screenshot({ path: 'stadium-local-01-login.png' });

    // Login with admin credentials
    console.log('🔐 Logging in as admin...');
    await page.fill('input[type="email"]', 'admin@stadium.com');
    await page.fill('input[type="password"]', 'admin123');
    await page.click('button[type="submit"]');

    // Wait for login to complete
    await page.waitForTimeout(5000);

    // Take screenshot after login
    await page.screenshot({ path: 'stadium-local-02-after-login.png' });

    // Navigate to stadium overview
    console.log('🏟️ Navigating to stadium overview...');
    await page.goto('https://localhost:7030/admin/stadium-overview');
    await page.waitForTimeout(5000);

    // Take screenshot of initial stadium overview state
    await page.screenshot({ path: 'stadium-local-03-overview-initial.png' });

    // Check current stadium state
    const initialState = await page.evaluate(() => {
      return {
        hasLoading: !!document.querySelector('#admin-stadium-loading-state'),
        hasError: !!document.querySelector('#admin-stadium-error-state'),
        hasEmpty: !!document.querySelector('#admin-stadium-empty-state'),
        hasGrid: !!document.querySelector('#admin-stadium-grid-layout'),
        loadingVisible: document.querySelector('#admin-stadium-loading-state')?.offsetParent !== null,
        errorVisible: document.querySelector('#admin-stadium-error-state')?.offsetParent !== null,
        emptyVisible: document.querySelector('#admin-stadium-empty-state')?.offsetParent !== null,
        gridVisible: document.querySelector('#admin-stadium-grid-layout')?.offsetParent !== null,
        containerExists: !!document.querySelector('#admin-stadium-container')
      };
    });

    console.log('📊 Initial stadium state:', initialState);

    // If we have empty/error state, try to generate demo data
    if (initialState.emptyVisible || initialState.errorVisible) {
      console.log('🔧 Stadium needs data - attempting to generate demo layout...');

      const demoButtons = [
        '#admin-stadium-demo-btn',
        '#admin-stadium-generate-btn',
        'button:has-text("Generate Demo Layout")',
        'button:has-text("Generate")'
      ];

      let buttonClicked = false;
      for (const selector of demoButtons) {
        try {
          if (await page.isVisible(selector)) {
            console.log(`🔘 Clicking demo button: ${selector}`);
            await page.click(selector);
            buttonClicked = true;
            await page.waitForTimeout(5000);
            break;
          }
        } catch (e) {
          // Continue to next selector
        }
      }

      if (!buttonClicked) {
        console.log('⚠️ No demo buttons found, trying API call...');

        // Try to create demo data via JavaScript API call
        const demoResult = await page.evaluate(async () => {
          try {
            const stadiumData = {
              name: "CSS Test Stadium",
              description: "Stadium for testing CSS grid layout fixes",
              tribunes: [
                {
                  code: "N",
                  name: "North Tribune",
                  rings: [{
                    number: 1,
                    name: "Lower Ring",
                    sectors: [{
                      code: "NA",
                      name: "North A",
                      type: "standard",
                      rows: 25,
                      seatsPerRow: 30
                    }, {
                      code: "NB",
                      name: "North B",
                      type: "standard",
                      rows: 25,
                      seatsPerRow: 30
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
                      name: "South A",
                      type: "standard",
                      rows: 25,
                      seatsPerRow: 30
                    }, {
                      code: "SB",
                      name: "South B",
                      type: "standard",
                      rows: 25,
                      seatsPerRow: 30
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
                      name: "East A",
                      type: "standard",
                      rows: 20,
                      seatsPerRow: 25
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
                      name: "West A",
                      type: "standard",
                      rows: 20,
                      seatsPerRow: 25
                    }]
                  }]
                }
              ]
            };

            const formData = new FormData();
            const blob = new Blob([JSON.stringify(stadiumData, null, 2)], { type: 'application/json' });
            formData.append('file', blob, 'css-test-stadium.json');

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

        console.log('🏗️ Demo data creation result:', demoResult);

        if (demoResult.success) {
          console.log('✅ Demo stadium data created successfully!');
          // Refresh the page to load the new data
          await page.reload();
          await page.waitForTimeout(5000);
        }
      }
    }

    // Wait for stadium to load
    console.log('⏳ Waiting for stadium layout to finish loading...');
    let attempts = 0;
    while (attempts < 20) {
      const state = await page.evaluate(() => {
        return {
          loadingVisible: document.querySelector('#admin-stadium-loading-state')?.offsetParent !== null,
          gridVisible: document.querySelector('#admin-stadium-grid-layout')?.offsetParent !== null,
          hasStands: document.querySelectorAll('.stadium-stand').length
        };
      });

      console.log(`Attempt ${attempts + 1}: Loading=${state.loadingVisible}, Grid=${state.gridVisible}, Stands=${state.hasStands}`);

      if (!state.loadingVisible && state.gridVisible && state.hasStands > 0) {
        console.log('✅ Stadium layout loaded successfully!');
        break;
      }

      attempts++;
      await page.waitForTimeout(1000);
    }

    // Now test the CSS grid layout fixes
    console.log('🎯 Testing CSS grid layout fixes...');

    const cssTestResult = await page.evaluate(() => {
      const grid = document.querySelector('#admin-stadium-grid-layout');
      const field = document.querySelector('#admin-stadium-field');
      const stands = document.querySelectorAll('.stadium-stand');
      const container = document.querySelector('#admin-stadium-container');

      if (!grid) return { success: false, reason: 'Grid layout not found' };

      const gridStyles = window.getComputedStyle(grid);
      const gridRect = grid.getBoundingClientRect();

      let fieldAnalysis = null;
      if (field) {
        const fieldStyles = window.getComputedStyle(field);
        const fieldRect = field.getBoundingClientRect();
        fieldAnalysis = {
          gridArea: fieldStyles.gridArea,
          width: fieldRect.width,
          height: fieldRect.height,
          backgroundColor: fieldStyles.backgroundColor,
          borderRadius: fieldStyles.borderRadius,
          left: fieldRect.left,
          top: fieldRect.top,
          visibility: fieldStyles.visibility
        };
      }

      let containerAnalysis = null;
      if (container) {
        const containerStyles = window.getComputedStyle(container);
        const containerRect = container.getBoundingClientRect();
        containerAnalysis = {
          width: containerRect.width,
          height: containerRect.height,
          maxHeight: containerStyles.maxHeight,
          overflow: containerStyles.overflow
        };
      }

      return {
        success: true,
        grid: {
          display: gridStyles.display,
          gridTemplateColumns: gridStyles.gridTemplateColumns,
          gridTemplateRows: gridStyles.gridTemplateRows,
          width: gridRect.width,
          height: gridRect.height,
          gap: gridStyles.gap,
          visibility: gridStyles.visibility
        },
        field: fieldAnalysis,
        container: containerAnalysis,
        standsCount: stands.length,
        stands: Array.from(stands).map((stand, index) => {
          const standStyles = window.getComputedStyle(stand);
          const standRect = stand.getBoundingClientRect();
          return {
            index,
            gridArea: standStyles.gridArea,
            width: standRect.width,
            height: standRect.height,
            className: stand.className,
            visibility: standStyles.visibility,
            position: {
              left: standRect.left,
              top: standRect.top
            }
          };
        })
      };
    });

    // Take final screenshot showing the stadium with CSS fixes
    await page.screenshot({
      path: 'stadium-local-04-final-css-fixes.png',
      fullPage: true
    });

    // Analyze and report results
    console.log('\n🎯 CSS GRID LAYOUT ANALYSIS:');

    if (cssTestResult.success) {
      console.log(`✅ Grid Display: ${cssTestResult.grid.display}`);
      console.log(`✅ Grid Template Columns: ${cssTestResult.grid.gridTemplateColumns}`);
      console.log(`✅ Grid Template Rows: ${cssTestResult.grid.gridTemplateRows}`);
      console.log(`✅ Grid Size: ${cssTestResult.grid.width}px x ${cssTestResult.grid.height}px`);
      console.log(`✅ Stadium Gap: ${cssTestResult.grid.gap}`);

      if (cssTestResult.field) {
        console.log(`\n🟢 FIELD ANALYSIS:`);
        console.log(`   - Grid Area: ${cssTestResult.field.gridArea}`);
        console.log(`   - Size: ${cssTestResult.field.width}px x ${cssTestResult.field.height}px`);
        console.log(`   - Background: ${cssTestResult.field.backgroundColor}`);
        console.log(`   - Border Radius: ${cssTestResult.field.borderRadius}`);
        console.log(`   - Position: (${cssTestResult.field.left}, ${cssTestResult.field.top})`);
      }

      if (cssTestResult.container) {
        console.log(`\n📦 CONTAINER ANALYSIS:`);
        console.log(`   - Size: ${cssTestResult.container.width}px x ${cssTestResult.container.height}px`);
        console.log(`   - Max Height: ${cssTestResult.container.maxHeight}`);
      }

      console.log(`\n🏟️ TRIBUNES ANALYSIS (${cssTestResult.standsCount} found):`);
      cssTestResult.stands.forEach((stand, index) => {
        console.log(`   Tribune ${index + 1}: ${stand.width}px x ${stand.height}px`);
        console.log(`      - Grid Area: ${stand.gridArea}`);
        console.log(`      - Position: (${stand.position.left}, ${stand.position.top})`);
        console.log(`      - Class: ${stand.className}`);
      });

      // Check success criteria
      const isGridWorking = cssTestResult.grid.display === 'grid';
      const hasCorrectColumns = cssTestResult.grid.gridTemplateColumns.includes('300px') && cssTestResult.grid.gridTemplateColumns.includes('1fr');
      const hasCorrectRows = cssTestResult.grid.gridTemplateRows.includes('200px') && cssTestResult.grid.gridTemplateRows.includes('1fr');
      const hasFieldInCenter = cssTestResult.field?.gridArea === 'field';
      const hasAllTribunes = cssTestResult.standsCount === 4;
      const isFieldLargeEnough = cssTestResult.field?.width > 400 && cssTestResult.field?.height > 300;

      console.log('\n🎉 SUCCESS CRITERIA CHECK:');
      console.log(`✅ Grid Display Working: ${isGridWorking ? '✅ YES' : '❌ NO'}`);
      console.log(`✅ Correct Grid Columns: ${hasCorrectColumns ? '✅ YES' : '❌ NO'}`);
      console.log(`✅ Correct Grid Rows: ${hasCorrectRows ? '✅ YES' : '❌ NO'}`);
      console.log(`✅ Field in Center: ${hasFieldInCenter ? '✅ YES' : '❌ NO'}`);
      console.log(`✅ All 4 Tribunes: ${hasAllTribunes ? '✅ YES' : '❌ NO'}`);
      console.log(`✅ Field Large Enough: ${isFieldLargeEnough ? '✅ YES' : '❌ NO'}`);

      const allCriteriaPass = isGridWorking && hasCorrectColumns && hasCorrectRows && hasFieldInCenter && hasAllTribunes && isFieldLargeEnough;

      if (allCriteriaPass) {
        console.log('\n🎉🎉 SUCCESS: CSS GRID LAYOUT FIXES ARE WORKING PERFECTLY! 🎉🎉');
        console.log('🏟️ The stadium now displays correctly with:');
        console.log('   ✅ Large central field (not tiny anymore)');
        console.log('   ✅ 4 properly positioned tribunes around the field');
        console.log('   ✅ CSS grid using 300px 1fr 300px column template');
        console.log('   ✅ CSS grid using 200px 1fr 200px row template');
        console.log('   ✅ Field positioned in center grid area');
        console.log('   ✅ Stadium filling the container height properly');
        console.log('\n🔧 The user\'s CSS fixes have resolved the visibility issues!');
      } else {
        console.log('\n⚠️ PARTIAL SUCCESS: Some CSS fixes are working but issues remain');
      }

    } else {
      console.log(`❌ CSS test failed: ${cssTestResult.reason}`);
    }

    console.log('\n📸 Screenshots saved:');
    console.log('- stadium-local-01-login.png');
    console.log('- stadium-local-02-after-login.png');
    console.log('- stadium-local-03-overview-initial.png');
    console.log('- stadium-local-04-final-css-fixes.png');

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({ path: 'stadium-local-error.png' });
  }

  await browser.close();
})();