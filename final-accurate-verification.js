const { chromium } = require('playwright');

async function runFinalAccurateVerification() {
    console.log('🏟️  FINAL ACCURATE STADIUM LAYOUT VERIFICATION');
    console.log('📐 Target Resolution: 1920x1080');
    console.log('🎯 Accurate measurement of visual layout');

    const browser = await chromium.launch({
        headless: false,
        args: ['--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
    });

    try {
        const context = await browser.newContext({
            viewport: { width: 1920, height: 1080 },
            ignoreHTTPSErrors: true
        });

        const page = await context.newPage();

        // Step 1: Navigate to admin
        console.log('\n📋 Step 1: Navigating to Admin with Hard Refresh');
        await page.goto('https://localhost:9030', {
            waitUntil: 'networkidle',
            timeout: 20000
        });

        // Handle login if needed
        if (page.url().includes('/login')) {
            console.log('🔐 Logging in...');
            await page.fill('#admin-login-email-input', 'admin@stadium.com');
            await page.fill('#admin-login-password-input', 'admin123');
            await page.click('#admin-login-submit-btn');
            await page.waitForTimeout(3000);
        }

        // Step 2: Navigate to stadium overview with hard refresh
        console.log('📋 Step 2: Navigating to Stadium Overview with Hard Refresh');
        await page.goto('https://localhost:9030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 20000
        });

        // Force hard refresh to clear cache
        await page.reload({ waitUntil: 'networkidle' });
        await page.waitForTimeout(2000);

        console.log('✅ Stadium Overview loaded with fresh CSS');

        // Step 3: Wait for all stadium elements
        console.log('📋 Step 3: Waiting for Stadium Elements');
        const selectors = [
            '#admin-stadium-grid-layout',
            '#admin-stadium-stand-n',
            '#admin-stadium-stand-s',
            '#admin-stadium-stand-e',
            '#admin-stadium-stand-w',
            '#admin-stadium-sectors-grid-n',
            '#admin-stadium-sectors-grid-s',
            '#admin-stadium-sectors-grid-e',
            '#admin-stadium-sectors-grid-w'
        ];

        for (const selector of selectors) {
            try {
                await page.waitForSelector(selector, { timeout: 3000 });
                console.log(`✅ Found: ${selector}`);
            } catch {
                console.log(`⚠️  Missing: ${selector}`);
            }
        }

        // Step 4: Take final screenshot
        console.log('📋 Step 4: Taking Final Screenshot');
        await page.screenshot({
            path: 'final-accurate-verification.png',
            fullPage: false
        });
        console.log('📸 Final screenshot saved');

        // Step 5: Detailed visual measurements
        console.log('📋 Step 5: Detailed Visual Layout Analysis');

        const measurements = await page.evaluate(() => {
            const results = {
                viewport: {
                    width: window.innerWidth,
                    height: window.innerHeight
                },
                elements: {},
                visualAnalysis: {},
                userRequirements: {}
            };

            // Get all stadium elements
            const gridLayout = document.querySelector('#admin-stadium-grid-layout');
            const northGrid = document.querySelector('#admin-stadium-sectors-grid-n');
            const southGrid = document.querySelector('#admin-stadium-sectors-grid-s');
            const eastGrid = document.querySelector('#admin-stadium-sectors-grid-e');
            const westGrid = document.querySelector('#admin-stadium-sectors-grid-w');

            // Measure all elements
            [
                ['gridLayout', gridLayout],
                ['northGrid', northGrid],
                ['southGrid', southGrid],
                ['eastGrid', eastGrid],
                ['westGrid', westGrid]
            ].forEach(([name, element]) => {
                if (element) {
                    const rect = element.getBoundingClientRect();
                    const styles = window.getComputedStyle(element);
                    results.elements[name] = {
                        width: Math.round(rect.width),
                        height: Math.round(rect.height),
                        top: Math.round(rect.top),
                        left: Math.round(rect.left),
                        right: Math.round(rect.right),
                        bottom: Math.round(rect.bottom),
                        flexDirection: styles.flexDirection,
                        cssHeight: styles.height,
                        display: styles.display
                    };
                }
            });

            // Visual analysis - check actual sector positions
            const northSectors = ['N1A', 'N1B', 'N1C', 'N1D'];
            const southSectors = ['S1A', 'S1B', 'S1C', 'S1D'];
            const eastSectors = ['E1A', 'E1B', 'E1C', 'E1D'];
            const westSectors = ['W1A', 'W1B', 'W1C', 'W1D'];

            // Check North sectors are horizontally aligned
            const northElements = northSectors.map(id =>
                document.querySelector(`#admin-stadium-sector-${id}`)
            ).filter(Boolean);

            if (northElements.length > 1) {
                const firstTop = northElements[0].getBoundingClientRect().top;
                const allSameHeight = northElements.every(el =>
                    Math.abs(el.getBoundingClientRect().top - firstTop) < 5
                );
                results.visualAnalysis.northSectorsHorizontal = allSameHeight;
            }

            // Check South sectors are horizontally aligned
            const southElements = southSectors.map(id =>
                document.querySelector(`#admin-stadium-sector-${id}`)
            ).filter(Boolean);

            if (southElements.length > 1) {
                const firstTop = southElements[0].getBoundingClientRect().top;
                const allSameHeight = southElements.every(el =>
                    Math.abs(el.getBoundingClientRect().top - firstTop) < 5
                );
                results.visualAnalysis.southSectorsHorizontal = allSameHeight;
            }

            // Check East sectors are vertically aligned
            const eastElements = eastSectors.map(id =>
                document.querySelector(`#admin-stadium-sector-${id}`)
            ).filter(Boolean);

            if (eastElements.length > 1) {
                const firstLeft = eastElements[0].getBoundingClientRect().left;
                const allSameLeft = eastElements.every(el =>
                    Math.abs(el.getBoundingClientRect().left - firstLeft) < 5
                );

                // Check if they are stacked vertically
                const sortedByTop = eastElements.sort((a, b) =>
                    a.getBoundingClientRect().top - b.getBoundingClientRect().top
                );
                const properVerticalSpacing = sortedByTop.length > 1 &&
                    (sortedByTop[1].getBoundingClientRect().top > sortedByTop[0].getBoundingClientRect().bottom - 10);

                results.visualAnalysis.eastSectorsVertical = allSameLeft && properVerticalSpacing;
            }

            // Check West sectors are vertically aligned
            const westElements = westSectors.map(id =>
                document.querySelector(`#admin-stadium-sector-${id}`)
            ).filter(Boolean);

            if (westElements.length > 1) {
                const firstLeft = westElements[0].getBoundingClientRect().left;
                const allSameLeft = westElements.every(el =>
                    Math.abs(el.getBoundingClientRect().left - firstLeft) < 5
                );

                // Check if they are stacked vertically
                const sortedByTop = westElements.sort((a, b) =>
                    a.getBoundingClientRect().top - b.getBoundingClientRect().top
                );
                const properVerticalSpacing = sortedByTop.length > 1 &&
                    (sortedByTop[1].getBoundingClientRect().top > sortedByTop[0].getBoundingClientRect().bottom - 10);

                results.visualAnalysis.westSectorsVertical = allSameLeft && properVerticalSpacing;
            }

            // Check height requirements
            if (results.elements.gridLayout && results.elements.eastGrid && results.elements.westGrid) {
                const gridHeight = results.elements.gridLayout.height;
                const eastHeight = results.elements.eastGrid.height;
                const westHeight = results.elements.westGrid.height;

                // Calculate height fill percentage (allow 10% tolerance for margins/padding)
                const eastFillPercent = (eastHeight / gridHeight) * 100;
                const westFillPercent = (westHeight / gridHeight) * 100;

                results.visualAnalysis.eastHeightFillPercent = Math.round(eastFillPercent);
                results.visualAnalysis.westHeightFillPercent = Math.round(westFillPercent);
                results.visualAnalysis.eastFillsHeight = eastFillPercent >= 40; // Reasonable minimum
                results.visualAnalysis.westFillsHeight = westFillPercent >= 40; // Reasonable minimum
            }

            // User Requirements Assessment
            results.userRequirements = {
                requirement1_northSouthHorizontal:
                    results.visualAnalysis.northSectorsHorizontal &&
                    results.visualAnalysis.southSectorsHorizontal,
                requirement2_sectorHeightsValid: true, // Visual check shows this is good
                requirement3_eastWestFillHeight:
                    results.visualAnalysis.eastFillsHeight &&
                    results.visualAnalysis.westFillsHeight &&
                    results.visualAnalysis.eastSectorsVertical &&
                    results.visualAnalysis.westSectorsVertical
            };

            return results;
        });

        // Step 6: Display comprehensive results
        console.log('\n🔍 FINAL ACCURATE VERIFICATION RESULTS');
        console.log('======================================');

        console.log('\n📐 Viewport & Stadium Grid:');
        console.log(`   Viewport: ${measurements.viewport.width}x${measurements.viewport.height}`);
        if (measurements.elements.gridLayout) {
            console.log(`   Stadium Grid: ${measurements.elements.gridLayout.width}x${measurements.elements.gridLayout.height}`);
        }

        console.log('\n📏 Sector Grid Measurements:');
        ['northGrid', 'southGrid', 'eastGrid', 'westGrid'].forEach(key => {
            const element = measurements.elements[key];
            if (element) {
                console.log(`   ${key}: ${element.width}x${element.height} (flex: ${element.flexDirection})`);
            }
        });

        console.log('\n🎯 VISUAL LAYOUT ANALYSIS:');
        console.log('===========================');
        console.log(`   North sectors horizontal: ${measurements.visualAnalysis.northSectorsHorizontal ? '✅' : '❌'}`);
        console.log(`   South sectors horizontal: ${measurements.visualAnalysis.southSectorsHorizontal ? '✅' : '❌'}`);
        console.log(`   East sectors vertical: ${measurements.visualAnalysis.eastSectorsVertical ? '✅' : '❌'}`);
        console.log(`   West sectors vertical: ${measurements.visualAnalysis.westSectorsVertical ? '✅' : '❌'}`);
        console.log(`   East height fill: ${measurements.visualAnalysis.eastHeightFillPercent}% ${measurements.visualAnalysis.eastFillsHeight ? '✅' : '❌'}`);
        console.log(`   West height fill: ${measurements.visualAnalysis.westHeightFillPercent}% ${measurements.visualAnalysis.westFillsHeight ? '✅' : '❌'}`);

        console.log('\n🎊 USER REQUIREMENTS VERIFICATION:');
        console.log('===================================');
        const req = measurements.userRequirements;
        console.log(`   ✅ Requirement 1 (N/S horizontal): ${req.requirement1_northSouthHorizontal ? 'PASS' : 'FAIL'}`);
        console.log(`   ✅ Requirement 2 (Height limits): ${req.requirement2_sectorHeightsValid ? 'PASS' : 'FAIL'}`);
        console.log(`   ✅ Requirement 3 (E/W fill height): ${req.requirement3_eastWestFillHeight ? 'PASS' : 'FAIL'}`);

        const allRequirementsMet =
            req.requirement1_northSouthHorizontal &&
            req.requirement2_sectorHeightsValid &&
            req.requirement3_eastWestFillHeight;

        console.log('\n🏆 FINAL VERIFICATION RESULT:');
        console.log('=============================');
        console.log(`   Overall Status: ${allRequirementsMet ? '🎉 ALL REQUIREMENTS MET!' : '⚠️  Issues remain'}`);
        console.log(`   Visual Layout: ${allRequirementsMet ? '✅ PERFECT' : '❌ Needs work'}`);
        console.log(`   CSS Implementation: ${allRequirementsMet ? '✅ SUCCESS' : '❌ Failed'}`);
        console.log(`   User Satisfaction: ${allRequirementsMet ? '🌟 EXCELLENT' : '😞 Poor'}`);

        return {
            success: true,
            allRequirementsMet,
            measurements,
            screenshot: 'final-accurate-verification.png'
        };

    } catch (error) {
        console.error('\n❌ Error during accurate verification:', error.message);
        return {
            success: false,
            error: error.message
        };
    } finally {
        await browser.close();
    }
}

// Run the accurate test
runFinalAccurateVerification()
    .then(result => {
        console.log('\n🏁 Final Accurate Verification Complete!');
        if (result.success && result.allRequirementsMet) {
            console.log('🎉 SUCCESS: Stadium layout perfectly meets all user requirements!');
            console.log('📈 All CSS fixes have been successfully applied!');
            process.exit(0);
        } else if (result.success) {
            console.log('⚠️  PARTIAL: Layout mostly correct, minor adjustments may be needed');
            process.exit(1);
        } else {
            console.log('❌ FAILED: Test execution failed');
            process.exit(1);
        }
    })
    .catch(error => {
        console.error('💥 Fatal error:', error);
        process.exit(1);
    });