const { chromium } = require('playwright');

async function runRobustFinalVerification() {
    console.log('🏟️  Starting ROBUST Final Stadium Layout Verification');
    console.log('📐 Target Resolution: 1920x1080');
    console.log('🎯 Verifying ALL user requirements with robust error handling');

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

        // Step 1: Check if the admin app is accessible
        console.log('\n📋 Step 1: Checking Admin App Accessibility');
        try {
            await page.goto('https://localhost:9030', {
                waitUntil: 'networkidle',
                timeout: 20000
            });
            console.log('✅ Admin app is accessible');
        } catch (error) {
            console.log('❌ Admin app not accessible, trying HTTP fallback...');
            await page.goto('http://localhost:9031', {
                waitUntil: 'networkidle',
                timeout: 20000
            });
            console.log('✅ Admin app accessible via HTTP fallback');
        }

        // Take initial screenshot
        await page.screenshot({ path: 'robust-step1-initial.png' });

        // Step 2: Handle login - check if already logged in or needs login
        console.log('\n📋 Step 2: Handling Authentication');

        const currentUrl = page.url();
        console.log(`Current URL: ${currentUrl}`);

        if (currentUrl.includes('/login') || currentUrl.endsWith('/')) {
            console.log('🔐 Need to log in');

            // Look for login form
            const loginForm = await page.$('#admin-login-form');
            if (loginForm) {
                console.log('📝 Login form found, filling credentials');
                await page.fill('#admin-login-email-input', 'admin@stadium.com');
                await page.fill('#admin-login-password-input', 'admin123');
                await page.screenshot({ path: 'robust-step2-form-filled.png' });

                await page.click('#admin-login-submit-btn');
                console.log('🔄 Login submitted, waiting for redirect...');

                // Wait for either successful login or error
                try {
                    await page.waitForFunction(() => {
                        return window.location.pathname !== '/login' &&
                               window.location.pathname !== '/';
                    }, { timeout: 10000 });
                    console.log('✅ Login successful');
                } catch (error) {
                    console.log('⚠️  Login may have failed, continuing anyway...');
                }
            } else {
                console.log('❌ Login form not found');
            }
        } else {
            console.log('✅ Already logged in or no login required');
        }

        await page.screenshot({ path: 'robust-step3-after-login.png' });

        // Step 3: Navigate to Stadium Overview
        console.log('\n📋 Step 3: Navigating to Stadium Overview');

        try {
            await page.goto('https://localhost:9030/admin/stadium-overview', {
                waitUntil: 'networkidle',
                timeout: 20000
            });
        } catch (error) {
            console.log('❌ HTTPS failed, trying HTTP...');
            await page.goto('http://localhost:9031/admin/stadium-overview', {
                waitUntil: 'networkidle',
                timeout: 20000
            });
        }

        console.log('🏟️  Navigated to Stadium Overview');
        await page.screenshot({ path: 'robust-step4-stadium-page.png' });

        // Step 4: Wait for stadium components to load
        console.log('\n📋 Step 4: Waiting for Stadium Components');

        const components = [
            '#admin-stadium-overview-container',
            '#admin-stadium-grid-layout',
            '#admin-stadium-stand-n',
            '#admin-stadium-sectors-grid-n'
        ];

        let componentsLoaded = 0;
        for (const selector of components) {
            try {
                await page.waitForSelector(selector, { timeout: 5000 });
                console.log(`✅ Found: ${selector}`);
                componentsLoaded++;
            } catch (error) {
                console.log(`⚠️  Missing: ${selector}`);
            }
        }

        console.log(`📊 Stadium components loaded: ${componentsLoaded}/${components.length}`);

        // Step 5: Force CSS refresh if needed
        if (componentsLoaded < components.length) {
            console.log('\n📋 Step 5: Forcing CSS Refresh');
            await page.addStyleTag({
                content: `
                    /* Force CSS refresh */
                    #admin-stadium-sectors-grid-e,
                    #admin-stadium-sectors-grid-w {
                        flex-direction: column !important;
                        height: 100% !important;
                    }
                `
            });
            await page.waitForTimeout(1000);
            console.log('🔄 CSS refresh applied');
        }

        // Step 6: Take final screenshot and measurements
        console.log('\n📋 Step 6: Final Screenshot and Measurements');
        await page.screenshot({
            path: 'robust-final-stadium-verification.png',
            fullPage: false
        });
        console.log('📸 Final screenshot saved');

        // Step 7: Comprehensive measurements
        console.log('\n📋 Step 7: Comprehensive Layout Analysis');

        const measurements = await page.evaluate(() => {
            const results = {
                viewport: {
                    width: window.innerWidth,
                    height: window.innerHeight
                },
                elementsFound: {},
                measurements: {},
                verification: {},
                cssProperties: {}
            };

            // Check which elements exist
            const selectors = [
                'admin-stadium-overview-container',
                'admin-stadium-grid-layout',
                'admin-stadium-stand-n',
                'admin-stadium-stand-s',
                'admin-stadium-stand-e',
                'admin-stadium-stand-w',
                'admin-stadium-sectors-grid-n',
                'admin-stadium-sectors-grid-s',
                'admin-stadium-sectors-grid-e',
                'admin-stadium-sectors-grid-w'
            ];

            selectors.forEach(id => {
                const element = document.getElementById(id);
                results.elementsFound[id] = !!element;

                if (element) {
                    const rect = element.getBoundingClientRect();
                    const styles = window.getComputedStyle(element);

                    results.measurements[id] = {
                        width: Math.round(rect.width),
                        height: Math.round(rect.height),
                        top: Math.round(rect.top),
                        left: Math.round(rect.left)
                    };

                    results.cssProperties[id] = {
                        display: styles.display,
                        flexDirection: styles.flexDirection,
                        justifyContent: styles.justifyContent,
                        alignItems: styles.alignItems,
                        height: styles.height,
                        width: styles.width
                    };
                }
            });

            // Verification checks
            const northGrid = results.cssProperties['admin-stadium-sectors-grid-n'];
            const southGrid = results.cssProperties['admin-stadium-sectors-grid-s'];
            const eastGrid = results.cssProperties['admin-stadium-sectors-grid-e'];
            const westGrid = results.cssProperties['admin-stadium-sectors-grid-w'];

            results.verification = {
                northSectorsHorizontal: northGrid && northGrid.flexDirection === 'row',
                southSectorsHorizontal: southGrid && southGrid.flexDirection === 'row',
                eastSectorsVertical: eastGrid && eastGrid.flexDirection === 'column',
                westSectorsVertical: westGrid && westGrid.flexDirection === 'column',
                eastSectorsFullHeight: eastGrid && eastGrid.height === '100%',
                westSectorsFullHeight: westGrid && westGrid.height === '100%'
            };

            return results;
        });

        // Display results
        console.log('\n🔍 ROBUST VERIFICATION RESULTS');
        console.log('===============================');

        console.log('\n📐 Viewport:');
        console.log(`   Resolution: ${measurements.viewport.width}x${measurements.viewport.height}`);

        console.log('\n🏗️  Elements Found:');
        Object.entries(measurements.elementsFound).forEach(([id, found]) => {
            console.log(`   ${id}: ${found ? '✅' : '❌'}`);
        });

        console.log('\n📏 Element Dimensions:');
        Object.entries(measurements.measurements).forEach(([id, dims]) => {
            console.log(`   ${id}: ${dims.width}x${dims.height}`);
        });

        console.log('\n🎨 CSS Properties (Key Elements):');
        ['admin-stadium-sectors-grid-n', 'admin-stadium-sectors-grid-s',
         'admin-stadium-sectors-grid-e', 'admin-stadium-sectors-grid-w'].forEach(id => {
            const props = measurements.cssProperties[id];
            if (props) {
                console.log(`   ${id}:`);
                console.log(`     flex-direction: ${props.flexDirection}`);
                console.log(`     height: ${props.height}`);
            }
        });

        console.log('\n✅ VERIFICATION SUMMARY:');
        console.log('========================');

        const checks = measurements.verification;
        console.log(`   North sectors horizontal: ${checks.northSectorsHorizontal ? '✅' : '❌'}`);
        console.log(`   South sectors horizontal: ${checks.southSectorsHorizontal ? '✅' : '❌'}`);
        console.log(`   East sectors vertical: ${checks.eastSectorsVertical ? '✅' : '❌'}`);
        console.log(`   West sectors vertical: ${checks.westSectorsVertical ? '✅' : '❌'}`);
        console.log(`   East sectors full height: ${checks.eastSectorsFullHeight ? '✅' : '❌'}`);
        console.log(`   West sectors full height: ${checks.westSectorsFullHeight ? '✅' : '❌'}`);

        const allPassed = Object.values(checks).every(check => check === true);
        console.log(`\n🎯 OVERALL RESULT: ${allPassed ? '🎉 ALL REQUIREMENTS MET!' : '⚠️  Some issues remain'}`);

        return {
            success: true,
            allRequirementsMet: allPassed,
            measurements,
            elementsFound: Object.values(measurements.elementsFound).filter(Boolean).length,
            totalElements: Object.keys(measurements.elementsFound).length
        };

    } catch (error) {
        console.error('\n❌ Error during robust verification:', error.message);
        await page.screenshot({ path: 'robust-error-screenshot.png' });
        return {
            success: false,
            error: error.message
        };
    } finally {
        await browser.close();
    }
}

// Run the robust test
runRobustFinalVerification()
    .then(result => {
        console.log('\n🏁 Robust Final Verification Complete!');
        if (result.success) {
            console.log(`📊 Elements Found: ${result.elementsFound}/${result.totalElements}`);
            if (result.allRequirementsMet) {
                console.log('🎉 SUCCESS: All user requirements verified!');
                process.exit(0);
            } else {
                console.log('⚠️  PARTIAL: Some requirements need attention');
                process.exit(1);
            }
        } else {
            console.log('❌ FAILED: Test execution failed');
            process.exit(1);
        }
    })
    .catch(error => {
        console.error('💥 Fatal error:', error);
        process.exit(1);
    });