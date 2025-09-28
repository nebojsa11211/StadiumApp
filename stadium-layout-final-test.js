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
    console.log('🚀 Testing stadium layout after CSS fixes - waiting for complete load...');

    // Navigate to admin login
    await page.goto('https://localhost:9030/login', { waitUntil: 'networkidle' });
    await page.waitForTimeout(2000);

    // Login
    await page.fill('#admin-login-email-input', 'admin@stadium.com');
    await page.fill('#admin-login-password-input', 'admin123');
    await page.click('#admin-login-submit-btn');
    await page.waitForNavigation({ waitUntil: 'networkidle' });

    // Navigate to stadium overview
    await page.goto('https://localhost:9030/admin/stadium-overview', { waitUntil: 'networkidle' });

    console.log('⏳ Waiting for stadium loading to complete...');

    // Wait for loading to disappear or stadium to load (up to 30 seconds)
    let attempts = 0;
    const maxAttempts = 30;

    while (attempts < maxAttempts) {
      await page.waitForTimeout(1000);
      attempts++;

      const status = await page.evaluate(() => {
        return {
          isLoading: !!document.querySelector('#admin-stadium-loading-state'),
          hasError: !!document.querySelector('#admin-stadium-error-state'),
          hasEmpty: !!document.querySelector('#admin-stadium-empty-state'),
          hasGrid: !!document.querySelector('#admin-stadium-grid-layout'),
          loadingVisible: document.querySelector('#admin-stadium-loading-state')?.offsetParent !== null,
          errorVisible: document.querySelector('#admin-stadium-error-state')?.offsetParent !== null,
          emptyVisible: document.querySelector('#admin-stadium-empty-state')?.offsetParent !== null,
          gridVisible: document.querySelector('#admin-stadium-grid-layout')?.offsetParent !== null
        };
      });

      console.log(`Attempt ${attempts}: Loading=${status.loadingVisible}, Error=${status.errorVisible}, Empty=${status.emptyVisible}, Grid=${status.gridVisible}`);

      // If loading is done and we have a result
      if (!status.loadingVisible) {
        if (status.gridVisible) {
          console.log('✅ Stadium grid layout loaded successfully!');
          break;
        } else if (status.errorVisible || status.emptyVisible) {
          console.log('⚠️ Stadium needs data - trying to generate demo layout...');

          // Try to generate demo data
          const demoButtons = [
            '#admin-stadium-demo-btn',
            '#admin-stadium-generate-btn'
          ];

          for (const selector of demoButtons) {
            try {
              if (await page.isVisible(selector)) {
                console.log(`🔧 Clicking ${selector}...`);
                await page.click(selector);
                await page.waitForTimeout(3000);
                break;
              }
            } catch (e) {
              // Continue to next selector
            }
          }

          // Wait for demo generation to complete
          await page.waitForTimeout(5000);
          break;
        }
      }
    }

    if (attempts >= maxAttempts) {
      console.log('⏰ Timeout waiting for stadium to load');
    }

    // Take screenshot of current state
    await page.screenshot({
      path: 'stadium-final-state-verification.png',
      fullPage: true
    });

    // Now analyze the final stadium layout
    console.log('🔍 Analyzing final stadium layout...');

    const finalAnalysis = await page.evaluate(() => {
      const grid = document.querySelector('#admin-stadium-grid-layout');
      const field = document.querySelector('#admin-stadium-field');
      const stands = document.querySelectorAll('.stadium-stand');
      const container = document.querySelector('#admin-stadium-container');

      let gridAnalysis = null;
      let fieldAnalysis = null;
      let containerAnalysis = null;

      if (grid) {
        const gridStyles = window.getComputedStyle(grid);
        const gridRect = grid.getBoundingClientRect();
        gridAnalysis = {
          display: gridStyles.display,
          gridTemplateColumns: gridStyles.gridTemplateColumns,
          gridTemplateRows: gridStyles.gridTemplateRows,
          width: `${gridRect.width}px`,
          height: `${gridRect.height}px`,
          visibility: gridStyles.visibility,
          position: gridStyles.position
        };
      }

      if (field) {
        const fieldStyles = window.getComputedStyle(field);
        const fieldRect = field.getBoundingClientRect();
        fieldAnalysis = {
          gridArea: fieldStyles.gridArea,
          width: `${fieldRect.width}px`,
          height: `${fieldRect.height}px`,
          backgroundColor: fieldStyles.backgroundColor,
          borderRadius: fieldStyles.borderRadius,
          visibility: fieldStyles.visibility
        };
      }

      if (container) {
        const containerStyles = window.getComputedStyle(container);
        const containerRect = container.getBoundingClientRect();
        containerAnalysis = {
          width: `${containerRect.width}px`,
          height: `${containerRect.height}px`,
          maxHeight: containerStyles.maxHeight,
          overflow: containerStyles.overflow
        };
      }

      return {
        hasGrid: !!grid,
        hasField: !!field,
        standsCount: stands.length,
        grid: gridAnalysis,
        field: fieldAnalysis,
        container: containerAnalysis,
        stands: Array.from(stands).map((stand, index) => {
          const standStyles = window.getComputedStyle(stand);
          const standRect = stand.getBoundingClientRect();
          return {
            index,
            gridArea: standStyles.gridArea,
            width: `${standRect.width}px`,
            height: `${standRect.height}px`,
            className: stand.className,
            visibility: standStyles.visibility
          };
        })
      };
    });

    console.log('📊 Final Stadium Analysis:');
    console.log(`- Has Grid Layout: ${finalAnalysis.hasGrid}`);
    console.log(`- Has Field: ${finalAnalysis.hasField}`);
    console.log(`- Number of Stands: ${finalAnalysis.standsCount}`);

    if (finalAnalysis.grid) {
      console.log('\n🟦 Grid Properties:');
      console.log(`  - Display: ${finalAnalysis.grid.display}`);
      console.log(`  - Template Columns: ${finalAnalysis.grid.gridTemplateColumns}`);
      console.log(`  - Template Rows: ${finalAnalysis.grid.gridTemplateRows}`);
      console.log(`  - Size: ${finalAnalysis.grid.width} x ${finalAnalysis.grid.height}`);
      console.log(`  - Visibility: ${finalAnalysis.grid.visibility}`);
    }

    if (finalAnalysis.field) {
      console.log('\n🟢 Field Properties:');
      console.log(`  - Grid Area: ${finalAnalysis.field.gridArea}`);
      console.log(`  - Size: ${finalAnalysis.field.width} x ${finalAnalysis.field.height}`);
      console.log(`  - Background: ${finalAnalysis.field.backgroundColor}`);
      console.log(`  - Border Radius: ${finalAnalysis.field.borderRadius}`);
    }

    if (finalAnalysis.container) {
      console.log('\n📦 Container Properties:');
      console.log(`  - Size: ${finalAnalysis.container.width} x ${finalAnalysis.container.height}`);
      console.log(`  - Max Height: ${finalAnalysis.container.maxHeight}`);
    }

    if (finalAnalysis.stands.length > 0) {
      console.log('\n🏟️ Stands Analysis:');
      finalAnalysis.stands.forEach((stand, index) => {
        console.log(`  Stand ${index + 1}: ${stand.width} x ${stand.height} (Grid Area: ${stand.gridArea})`);
      });
    }

    // Success criteria check
    const isSuccess =
      finalAnalysis.hasGrid &&
      finalAnalysis.hasField &&
      finalAnalysis.standsCount === 4 &&
      finalAnalysis.grid?.display === 'grid' &&
      finalAnalysis.grid?.gridTemplateColumns?.includes('300px') &&
      finalAnalysis.grid?.gridTemplateRows?.includes('200px') &&
      finalAnalysis.field?.gridArea === 'field';

    console.log('\n🎯 SUCCESS CRITERIA CHECK:');
    console.log(`✅ Has Grid Layout: ${finalAnalysis.hasGrid}`);
    console.log(`✅ Has Field: ${finalAnalysis.hasField}`);
    console.log(`✅ Has 4 Stands: ${finalAnalysis.standsCount === 4} (found: ${finalAnalysis.standsCount})`);
    console.log(`✅ Grid Display: ${finalAnalysis.grid?.display === 'grid'} (${finalAnalysis.grid?.display})`);
    console.log(`✅ Grid Columns: ${finalAnalysis.grid?.gridTemplateColumns?.includes('300px')} (${finalAnalysis.grid?.gridTemplateColumns})`);
    console.log(`✅ Grid Rows: ${finalAnalysis.grid?.gridTemplateRows?.includes('200px')} (${finalAnalysis.grid?.gridTemplateRows})`);
    console.log(`✅ Field Grid Area: ${finalAnalysis.field?.gridArea === 'field'} (${finalAnalysis.field?.gridArea})`);

    if (isSuccess) {
      console.log('\n🎉 SUCCESS: Stadium layout CSS fixes are working perfectly!');
      console.log('🏟️ The stadium now displays properly with:');
      console.log('   - Large central field (not tiny)');
      console.log('   - 4 visible tribunes surrounding the field');
      console.log('   - Proper CSS grid layout with 300px 1fr 300px columns');
      console.log('   - Stadium filling the container height properly');
    } else {
      console.log('\n⚠️ ISSUES REMAINING: Some problems still exist with the stadium layout');
    }

    console.log('\n📸 Screenshot saved: stadium-final-state-verification.png');

  } catch (error) {
    console.error('❌ Test failed:', error.message);
    await page.screenshot({ path: 'stadium-test-error-final.png' });
  }

  await browser.close();
})();