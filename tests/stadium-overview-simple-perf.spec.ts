import { test, expect } from '@playwright/test';

test.describe('Stadium Overview Simple Performance Test', () => {
    test('Direct Stadium Overview Load Performance', async ({ page }) => {
        console.log('\n🚀 === STADIUM OVERVIEW DIRECT LOAD PERFORMANCE TEST ===\n');

        // Start timing
        const overallStartTime = Date.now();

        // Step 1: Login first
        console.log('🔐 Step 1: Logging in...');
        const loginStartTime = Date.now();

        await page.goto('https://localhost:7030/login', {
            waitUntil: 'networkidle',
            timeout: 30000
        });

        await page.fill('input[type="email"]', 'admin@stadium.com');
        await page.fill('input[type="password"]', 'admin123');

        // Click login and wait for redirect
        await Promise.all([
            page.waitForURL('**/dashboard', { timeout: 30000 }),
            page.click('button[type="submit"]')
        ]);

        const loginTime = Date.now() - loginStartTime;
        console.log(`✅ Login completed in ${loginTime}ms\n`);

        // Wait a moment for auth to settle
        await page.waitForTimeout(500);

        // Step 2: Navigate to Stadium Overview and measure performance
        console.log('🏟️  Step 2: Loading Stadium Overview page...');
        const stadiumStartTime = Date.now();

        // Track API calls
        const apiCalls: { url: string; duration: number; status: number }[] = [];

        page.on('response', async (response) => {
            const url = response.url();
            if (url.includes('/api/') || url.includes('stadium')) {
                const timing = response.timing();
                apiCalls.push({
                    url: url.split('?')[0],
                    duration: timing.responseEnd,
                    status: response.status()
                });
            }
        });

        // Navigate to Stadium Overview
        await page.goto('https://localhost:7030/admin/stadium-overview', {
            waitUntil: 'networkidle',
            timeout: 60000
        });

        // Wait for loading spinner to disappear
        try {
            const loadingSpinner = page.locator('text=/loading/i').first();
            if (await loadingSpinner.isVisible({ timeout: 2000 })) {
                console.log('   ⏳ Waiting for loading spinner to disappear...');
                await loadingSpinner.waitFor({ state: 'hidden', timeout: 60000 });
            }
        } catch (e) {
            // No loading spinner or already hidden
        }

        // Wait for stadium content to appear
        const contentSelectors = [
            '.stadium-viewer-container',
            '.sector',
            '[class*="stadium"]',
            'svg',
            '.stand'
        ];

        let contentFound = false;
        for (const selector of contentSelectors) {
            try {
                await page.waitForSelector(selector, { timeout: 5000 });
                contentFound = true;
                console.log(`   ✅ Found content: ${selector}`);
                break;
            } catch (e) {
                // Try next selector
            }
        }

        const stadiumLoadTime = Date.now() - stadiumStartTime;
        const totalTime = Date.now() - overallStartTime;

        console.log(`\n📊 === PERFORMANCE RESULTS ===`);
        console.log(`⏱️  Stadium Overview Load Time: ${stadiumLoadTime}ms (${(stadiumLoadTime / 1000).toFixed(2)}s)`);
        console.log(`⏱️  Total Time (with login): ${totalTime}ms (${(totalTime / 1000).toFixed(2)}s)`);
        console.log(`📡 API Calls Made: ${apiCalls.length}`);

        if (apiCalls.length > 0) {
            console.log(`\n🌐 API Call Breakdown:`);
            apiCalls.forEach((call, i) => {
                const urlPart = call.url.split('/').slice(-3).join('/');
                console.log(`   ${i + 1}. ${urlPart}: ${call.duration.toFixed(0)}ms (HTTP ${call.status})`);
            });

            // Find slowest API call
            const slowest = apiCalls.reduce((prev, current) =>
                current.duration > prev.duration ? current : prev
            );
            const slowestUrl = slowest.url.split('/').slice(-2).join('/');
            console.log(`\n🐌 Slowest API Call: ${slowestUrl} - ${slowest.duration.toFixed(0)}ms`);

            // Check for stadium viewer API
            const stadiumViewerCall = apiCalls.find(c => c.url.includes('stadiumviewer/overview'));
            if (stadiumViewerCall) {
                console.log(`🏟️  Stadium Viewer API: ${stadiumViewerCall.duration.toFixed(0)}ms`);
            }
        }

        // Performance rating
        console.log(`\n🎯 Performance Rating:`);
        if (stadiumLoadTime < 1000) {
            console.log(`   🟢 EXCELLENT: < 1 second`);
        } else if (stadiumLoadTime < 2000) {
            console.log(`   🟡 GOOD: 1-2 seconds`);
        } else if (stadiumLoadTime < 5000) {
            console.log(`   🟠 ACCEPTABLE: 2-5 seconds`);
        } else {
            console.log(`   🔴 SLOW: > 5 seconds - NEEDS OPTIMIZATION`);
        }

        // Take screenshot
        await page.screenshot({
            path: 'stadium-overview-loaded.png',
            fullPage: true
        });
        console.log(`\n📸 Screenshot saved: stadium-overview-loaded.png`);

        console.log(`\n=== END PERFORMANCE TEST ===\n`);

        // Assertions
        expect(contentFound).toBe(true);
        expect(stadiumLoadTime).toBeLessThan(10000); // Should load in under 10 seconds
    });

    test('Multiple Reload Performance Test', async ({ page }) => {
        console.log('\n🔄 === MULTIPLE RELOAD PERFORMANCE TEST ===\n');

        // Login first
        await page.goto('https://localhost:7030/login');
        await page.fill('input[type="email"]', 'admin@stadium.com');
        await page.fill('input[type="password"]', 'admin123');
        await Promise.all([
            page.waitForURL('**/dashboard', { timeout: 30000 }),
            page.click('button[type="submit"]')
        ]);

        const loadTimes: number[] = [];

        for (let i = 1; i <= 3; i++) {
            console.log(`\n🔄 Load #${i}:`);
            const startTime = Date.now();

            await page.goto('https://localhost:7030/admin/stadium-overview', {
                waitUntil: 'networkidle',
                timeout: 60000
            });

            // Wait for loading to complete
            try {
                await page.waitForSelector('.stadium-viewer-container, .sector, svg', {
                    timeout: 30000
                });
            } catch (e) {
                // Content might already be there
            }

            const loadTime = Date.now() - startTime;
            loadTimes.push(loadTime);
            console.log(`   ⏱️  Time: ${loadTime}ms (${(loadTime / 1000).toFixed(2)}s)`);

            // Wait a bit between reloads
            if (i < 3) await page.waitForTimeout(1000);
        }

        const avgTime = loadTimes.reduce((a, b) => a + b, 0) / loadTimes.length;
        const minTime = Math.min(...loadTimes);
        const maxTime = Math.max(...loadTimes);

        console.log(`\n📊 Summary:`);
        console.log(`   Average: ${avgTime.toFixed(0)}ms`);
        console.log(`   Min: ${minTime}ms`);
        console.log(`   Max: ${maxTime}ms`);
        console.log(`   Range: ${maxTime - minTime}ms`);

        // Check if caching is working (subsequent loads should be faster)
        if (loadTimes.length >= 2 && loadTimes[1] < loadTimes[0]) {
            console.log(`   💾 Cache Effect: YES (${loadTimes[0]}ms → ${loadTimes[1]}ms)`);
        } else {
            console.log(`   💾 Cache Effect: Minimal or None`);
        }

        console.log(`\n=== END RELOAD TEST ===\n`);

        expect(avgTime).toBeLessThan(10000);
    });
});
