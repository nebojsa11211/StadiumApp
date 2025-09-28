const { chromium } = require('playwright');

async function runFinalStadiumLayoutVerification() {
    console.log('🏟️  Starting FINAL Stadium Layout Verification Test');
    console.log('📐 Target Resolution: 1920x1080');
    console.log('🎯 Verifying ALL user requirements after CSS fix');

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

        // Step 1: Navigate to admin login
        console.log('\n📋 Step 1: Navigating to Admin Login');
        await page.goto('https://localhost:9030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Step 2: Login with admin credentials
        console.log('📋 Step 2: Logging in as admin');
        await page.fill('#admin-login-email-input', 'admin@stadium.com');
        await page.fill('#admin-login-password-input', 'admin123');
        await page.click('#admin-login-submit-btn');

        // Wait for login success
        await page.waitForURL('**/admin', { timeout: 15000 });
        console.log('✅ Admin login successful');

        // Step 3: Navigate to Stadium Overview
        console.log('📋 Step 3: Navigating to Stadium Overview');
        await page.goto('https://localhost:9030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        // Wait for stadium to load
        await page.waitForSelector('#admin-stadium-overview-container', { timeout: 15000 });
        console.log('✅ Stadium Overview page loaded');

        // Step 4: Take final screenshot
        console.log('📋 Step 4: Taking final verification screenshot');
        await page.screenshot({
            path: 'final-stadium-layout-verification.png',
            fullPage: false
        });
        console.log('📸 Final screenshot saved: final-stadium-layout-verification.png');

        // Step 5: Comprehensive Layout Measurements
        console.log('\n📋 Step 5: Measuring ALL Layout Components');

        const measurements = await page.evaluate(() => {
            const results = {
                viewport: {
                    width: window.innerWidth,
                    height: window.innerHeight
                },
                containers: {},
                sectors: {},
                grids: {},
                verification: {}
            };

            // Main stadium container
            const stadiumContainer = document.querySelector('#admin-stadium-overview-container');
            if (stadiumContainer) {
                const rect = stadiumContainer.getBoundingClientRect();
                results.containers.stadiumOverview = {
                    width: Math.round(rect.width),
                    height: Math.round(rect.height),
                    top: Math.round(rect.top),
                    left: Math.round(rect.left)
                };
            }

            // Stadium grid layout
            const gridLayout = document.querySelector('#admin-stadium-grid-layout');
            if (gridLayout) {
                const rect = gridLayout.getBoundingClientRect();
                results.grids.stadiumGrid = {
                    width: Math.round(rect.width),
                    height: Math.round(rect.height),
                    top: Math.round(rect.top),
                    left: Math.round(rect.left)
                };
            }

            // Individual stands and their sector grids
            const stands = ['n', 's', 'e', 'w'];
            stands.forEach(stand => {
                // Stand container
                const standElement = document.querySelector(`#admin-stadium-stand-${stand}`);
                if (standElement) {
                    const rect = standElement.getBoundingClientRect();
                    results.containers[`stand_${stand}`] = {
                        width: Math.round(rect.width),
                        height: Math.round(rect.height),
                        top: Math.round(rect.top),
                        left: Math.round(rect.left)
                    };
                }

                // Sectors grid for this stand
                const sectorsGrid = document.querySelector(`#admin-stadium-sectors-grid-${stand}`);
                if (sectorsGrid) {
                    const rect = sectorsGrid.getBoundingClientRect();
                    const styles = window.getComputedStyle(sectorsGrid);
                    results.grids[`sectors_${stand}`] = {
                        width: Math.round(rect.width),
                        height: Math.round(rect.height),
                        top: Math.round(rect.top),
                        left: Math.round(rect.left),
                        flexDirection: styles.flexDirection,
                        justifyContent: styles.justifyContent,
                        alignItems: styles.alignItems,
                        gap: styles.gap
                    };
                }
            });

            // Individual sectors
            const sectors = [
                'N1A', 'N1B', 'N1C', 'N1D',  // North sectors
                'S1A', 'S1B', 'S1C', 'S1D',  // South sectors
                'E1A', 'E1B', 'E1C', 'E1D',  // East sectors
                'W1A', 'W1B', 'W1C', 'W1D'   // West sectors
            ];

            sectors.forEach(sectorCode => {
                const sectorElement = document.querySelector(`#admin-stadium-sector-${sectorCode}`);
                if (sectorElement) {
                    const rect = sectorElement.getBoundingClientRect();
                    const styles = window.getComputedStyle(sectorElement);
                    results.sectors[sectorCode] = {
                        width: Math.round(rect.width),
                        height: Math.round(rect.height),
                        top: Math.round(rect.top),
                        left: Math.round(rect.left),
                        display: styles.display,
                        flexDirection: styles.flexDirection,
                        justifyContent: styles.justifyContent,
                        alignItems: styles.alignItems
                    };
                }
            });

            // VERIFICATION CHECKS - User Requirements

            // Requirement 1: North/South sectors should stay horizontal
            const northSectorsGrid = results.grids.sectors_n;
            const southSectorsGrid = results.grids.sectors_s;

            results.verification.northSectorsHorizontal =
                northSectorsGrid && northSectorsGrid.flexDirection === 'row';
            results.verification.southSectorsHorizontal =
                southSectorsGrid && southSectorsGrid.flexDirection === 'row';

            // Requirement 2: Sector heights should not exceed parent heights
            const heightChecks = {};
            stands.forEach(stand => {
                const standHeight = results.containers[`stand_${stand}`]?.height;
                const sectorsInStand = sectors.filter(s => s.toLowerCase().startsWith(stand));

                sectorsInStand.forEach(sectorCode => {
                    const sectorHeight = results.sectors[sectorCode]?.height;
                    if (standHeight && sectorHeight) {
                        heightChecks[sectorCode] = {
                            sectorHeight,
                            standHeight,
                            withinLimit: sectorHeight <= standHeight,
                            ratio: Math.round((sectorHeight / standHeight) * 100)
                        };
                    }
                });
            });
            results.verification.heightChecks = heightChecks;

            // Requirement 3: West/East sectors should fill full height of stadium grid
            const stadiumGridHeight = results.grids.stadiumGrid?.height;
            const westSectorsHeight = results.grids.sectors_w?.height;
            const eastSectorsHeight = results.grids.sectors_e?.height;

            results.verification.westSectorsFillHeight =
                stadiumGridHeight && westSectorsHeight &&
                (westSectorsHeight >= stadiumGridHeight * 0.95); // Allow 5% tolerance

            results.verification.eastSectorsFillHeight =
                stadiumGridHeight && eastSectorsHeight &&
                (eastSectorsHeight >= stadiumGridHeight * 0.95); // Allow 5% tolerance

            // East/West sectors should use column layout
            results.verification.eastSectorsVertical =
                results.grids.sectors_e && results.grids.sectors_e.flexDirection === 'column';
            results.verification.westSectorsVertical =
                results.grids.sectors_w && results.grids.sectors_w.flexDirection === 'column';

            return results;
        });

        // Step 6: Display comprehensive results
        console.log('\n🔍 FINAL VERIFICATION RESULTS');
        console.log('=====================================');

        console.log('\n📐 Viewport & Main Containers:');
        console.log(`   Viewport: ${measurements.viewport.width}x${measurements.viewport.height}`);
        console.log(`   Stadium Overview: ${measurements.containers.stadiumOverview?.width}x${measurements.containers.stadiumOverview?.height}`);
        console.log(`   Stadium Grid: ${measurements.grids.stadiumGrid?.width}x${measurements.grids.stadiumGrid?.height}`);

        console.log('\n🏟️  Stand Containers:');
        ['n', 's', 'e', 'w'].forEach(stand => {
            const standData = measurements.containers[`stand_${stand}`];
            if (standData) {
                console.log(`   Stand ${stand.toUpperCase()}: ${standData.width}x${standData.height}`);
            }
        });

        console.log('\n📋 Sectors Grid Layout:');
        ['n', 's', 'e', 'w'].forEach(stand => {
            const gridData = measurements.grids[`sectors_${stand}`];
            if (gridData) {
                console.log(`   ${stand.toUpperCase()} Sectors: ${gridData.width}x${gridData.height} (flex: ${gridData.flexDirection})`);
            }
        });

        console.log('\n🎯 USER REQUIREMENT VERIFICATION:');
        console.log('=====================================');

        // Requirement 1: Horizontal layout for North/South
        console.log('\n✅ Requirement 1: North/South sectors stay horizontal');
        console.log(`   North sectors horizontal: ${measurements.verification.northSectorsHorizontal ? '✅ YES' : '❌ NO'}`);
        console.log(`   South sectors horizontal: ${measurements.verification.southSectorsHorizontal ? '✅ YES' : '❌ NO'}`);

        // Requirement 2: Sector heights within parent limits
        console.log('\n✅ Requirement 2: Sector heights within parent limits');
        let allHeightsValid = true;
        Object.entries(measurements.verification.heightChecks).forEach(([sector, check]) => {
            const status = check.withinLimit ? '✅' : '❌';
            console.log(`   ${sector}: ${check.sectorHeight}px ≤ ${check.standHeight}px (${check.ratio}%) ${status}`);
            if (!check.withinLimit) allHeightsValid = false;
        });
        console.log(`   Overall height compliance: ${allHeightsValid ? '✅ ALL VALID' : '❌ ISSUES FOUND'}`);

        // Requirement 3: West/East sectors fill full height
        console.log('\n✅ Requirement 3: West/East sectors fill stadium grid height');
        console.log(`   West sectors fill height: ${measurements.verification.westSectorsFillHeight ? '✅ YES' : '❌ NO'}`);
        console.log(`   East sectors fill height: ${measurements.verification.eastSectorsFillHeight ? '✅ YES' : '❌ NO'}`);
        console.log(`   West sectors vertical layout: ${measurements.verification.westSectorsVertical ? '✅ YES' : '❌ NO'}`);
        console.log(`   East sectors vertical layout: ${measurements.verification.eastSectorsVertical ? '✅ YES' : '❌ NO'}`);

        console.log('\n📊 DETAILED SECTOR MEASUREMENTS:');
        console.log('=====================================');

        // Group sectors by orientation
        const northSouth = ['N1A', 'N1B', 'N1C', 'N1D', 'S1A', 'S1B', 'S1C', 'S1D'];
        const eastWest = ['E1A', 'E1B', 'E1C', 'E1D', 'W1A', 'W1B', 'W1C', 'W1D'];

        console.log('\n🔸 North/South Sectors (Should be horizontal):');
        northSouth.forEach(sector => {
            const data = measurements.sectors[sector];
            if (data) {
                console.log(`   ${sector}: ${data.width}x${data.height} (flex: ${data.flexDirection || 'N/A'})`);
            }
        });

        console.log('\n🔸 East/West Sectors (Should be vertical):');
        eastWest.forEach(sector => {
            const data = measurements.sectors[sector];
            if (data) {
                console.log(`   ${sector}: ${data.width}x${data.height} (flex: ${data.flexDirection || 'N/A'})`);
            }
        });

        // Final summary
        const allRequirementsMet =
            measurements.verification.northSectorsHorizontal &&
            measurements.verification.southSectorsHorizontal &&
            allHeightsValid &&
            measurements.verification.westSectorsFillHeight &&
            measurements.verification.eastSectorsFillHeight &&
            measurements.verification.westSectorsVertical &&
            measurements.verification.eastSectorsVertical;

        console.log('\n🎊 FINAL VERIFICATION SUMMARY:');
        console.log('=====================================');
        console.log(`Overall Status: ${allRequirementsMet ? '🎉 ALL REQUIREMENTS MET!' : '⚠️  Some issues remain'}`);
        console.log(`Resolution: ${measurements.viewport.width}x${measurements.viewport.height}`);
        console.log(`CSS Cache Issue: ${allRequirementsMet ? '✅ RESOLVED' : '❌ May still exist'}`);
        console.log(`Stadium Layout: ${allRequirementsMet ? '✅ FULLY COMPLIANT' : '⚠️  Needs attention'}`);

        return {
            success: true,
            allRequirementsMet,
            measurements,
            screenshot: 'final-stadium-layout-verification.png'
        };

    } catch (error) {
        console.error('❌ Error during final verification:', error.message);
        return {
            success: false,
            error: error.message
        };
    } finally {
        await browser.close();
    }
}

// Run the test
runFinalStadiumLayoutVerification()
    .then(result => {
        console.log('\n🏁 Final Verification Complete!');
        if (result.success && result.allRequirementsMet) {
            console.log('🎉 SUCCESS: All user requirements have been fully addressed!');
            console.log('📸 Screenshot saved for documentation');
            process.exit(0);
        } else if (result.success) {
            console.log('⚠️  PARTIAL: Some requirements may need attention');
            console.log('📸 Screenshot saved for analysis');
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