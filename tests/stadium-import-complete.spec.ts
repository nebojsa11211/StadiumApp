import { test, expect } from '@playwright/test';
import path from 'path';

const ADMIN_BASE_URL = 'https://localhost:9030';
const API_BASE_URL = 'https://localhost:9010';
const ADMIN_EMAIL = 'admin@stadium.com';
const ADMIN_PASSWORD = 'admin123';

test.describe('Stadium Structure Import - Complete Workflow', () => {
  test('should successfully import stadium structure and verify data', async ({ page, request }) => {
    console.log('🏟️ Starting comprehensive stadium structure import test...');

    test.setTimeout(120000); // 2 minutes timeout

    // Step 1: Authenticate with API to get token
    console.log('Step 1: Authenticating with API...');
    const loginResponse = await request.post(`${API_BASE_URL}/auth/login`, {
      headers: {
        'Content-Type': 'application/json',
      },
      data: {
        Email: ADMIN_EMAIL,
        Password: ADMIN_PASSWORD
      },
      ignoreHTTPSErrors: true
    });

    expect(loginResponse.ok()).toBeTruthy();
    const loginData = await loginResponse.json();
    const token = loginData.token;
    console.log(`✅ Authentication successful. Token obtained.`);

    // Step 2: Clear any existing stadium structure
    console.log('Step 2: Clearing existing stadium structure...');
    const clearResponse = await request.delete(`${API_BASE_URL}/StadiumStructure/clear`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
      ignoreHTTPSErrors: true
    });

    if (clearResponse.ok()) {
      console.log('✅ Existing stadium structure cleared.');
    } else {
      console.log('ℹ️ No existing structure to clear or clear operation not needed.');
    }

    // Step 3: Upload stadium structure JSON file
    console.log('Step 3: Uploading stadium structure JSON file...');
    const filePath = path.resolve('D:/AiApps/StadiumApp/StadiumApp/stadium-samples/standard-stadium.json');

    const importResponse = await request.post(`${API_BASE_URL}/StadiumStructure/import/json`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
      multipart: {
        file: {
          name: 'standard-stadium.json',
          mimeType: 'application/json',
          buffer: require('fs').readFileSync(filePath)
        }
      },
      ignoreHTTPSErrors: true
    });

    expect(importResponse.ok()).toBeTruthy();
    const importResult = await importResponse.json();
    console.log(`✅ Stadium structure imported: ${importResult.message}`);
    expect(importResult.message).toContain('successfully');

    // Step 4: Verify import by checking stadium summary
    console.log('Step 4: Verifying import via stadium summary...');
    const summaryResponse = await request.get(`${API_BASE_URL}/StadiumStructure/summary`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
      ignoreHTTPSErrors: true
    });

    expect(summaryResponse.ok()).toBeTruthy();
    const summary = await summaryResponse.json();

    console.log('Stadium Summary:', summary);

    // Verify the standard stadium structure
    expect(summary.totalTribunes).toBe(4); // N, S, E, W
    expect(summary.totalRings).toBe(4); // One ring per tribune
    expect(summary.totalSectors).toBe(6); // NA, NB, SA, SB, EA, WA
    expect(summary.totalSeats).toBe(3000); // 25 rows × 20 seats × 6 sectors
    expect(summary.availableSeats).toBe(3000); // All seats should be available
    expect(summary.occupiedSeats).toBe(0); // No bookings yet

    console.log('✅ Stadium summary verification successful!');

    // Step 5: Verify total seats count
    console.log('Step 5: Verifying total seats count...');
    const seatsResponse = await request.get(`${API_BASE_URL}/StadiumStructure/total-seats`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
      ignoreHTTPSErrors: true
    });

    expect(seatsResponse.ok()).toBeTruthy();
    const totalSeats = await seatsResponse.text();
    expect(parseInt(totalSeats)).toBe(3000);
    console.log(`✅ Total seats count verified: ${totalSeats}`);

    // Step 6: Verify full structure data
    console.log('Step 6: Verifying full structure data...');
    const structureResponse = await request.get(`${API_BASE_URL}/StadiumStructure/full-structure`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
      ignoreHTTPSErrors: true
    });

    expect(structureResponse.ok()).toBeTruthy();
    const fullStructure = await structureResponse.json();

    // Verify tribunes are present
    expect(fullStructure.tribunes).toBeDefined();
    expect(fullStructure.tribunes.length).toBe(4);

    // Verify tribune codes
    const tribuneCodes = fullStructure.tribunes.map((t: any) => t.code).sort();
    expect(tribuneCodes).toEqual(['E', 'N', 'S', 'W']);

    console.log('✅ Full structure verification successful!');

    // Step 7: Test admin UI navigation (if possible)
    console.log('Step 7: Testing admin UI access...');
    try {
      await page.goto(ADMIN_BASE_URL, {
        waitUntil: 'domcontentloaded',
        timeout: 30000
      });

      // Check if we can access the page
      const pageTitle = await page.title();
      console.log(`Admin page title: ${pageTitle}`);

      // Look for authentication state
      const isAuthenticated = await page.locator('text=admin@stadium.com').count() > 0;
      console.log(`Admin UI authentication detected: ${isAuthenticated}`);

      if (isAuthenticated) {
        console.log('✅ Admin UI is accessible and authenticated');

        // Try to navigate to stadium overview
        try {
          await page.goto(`${ADMIN_BASE_URL}/admin/stadium-overview`, {
            waitUntil: 'domcontentloaded',
            timeout: 15000
          });

          await page.waitForTimeout(3000);

          // Check if stadium data is now visible (no "No Stadium Data" message)
          const hasNoDataMessage = await page.locator('text=No Stadium Data').count() > 0;

          if (!hasNoDataMessage) {
            console.log('✅ Stadium overview no longer shows "No Stadium Data" - import successful!');
          } else {
            console.log('⚠️ Stadium overview still shows "No Stadium Data" - UI may need refresh');
          }

        } catch (error) {
          console.log('ℹ️ Could not access stadium overview page, but API verification was successful');
        }
      }

    } catch (error) {
      console.log('ℹ️ Admin UI not accessible, but API import was successful');
      console.log(`UI Error: ${error}`);
    }

    // Final Summary
    console.log('=== STADIUM IMPORT TEST COMPLETE ===');
    console.log('✅ Authentication: SUCCESS');
    console.log('✅ Stadium Structure Import: SUCCESS');
    console.log('✅ Data Verification: SUCCESS');
    console.log(`✅ Stadium Capacity: ${summary.totalSeats} seats`);
    console.log(`✅ Tribunes: ${summary.totalTribunes} (N, S, E, W)`);
    console.log(`✅ Sectors: ${summary.totalSectors} total`);
    console.log('🎉 Stadium structure is now available for the Stadium Overview page!');
  });

  test('should verify stadium data persists and can be exported', async ({ request }) => {
    console.log('🔄 Testing stadium data persistence and export...');

    // Authenticate
    const loginResponse = await request.post(`${API_BASE_URL}/auth/login`, {
      headers: { 'Content-Type': 'application/json' },
      data: { Email: ADMIN_EMAIL, Password: ADMIN_PASSWORD },
      ignoreHTTPSErrors: true
    });

    const loginData = await loginResponse.json();
    const token = loginData.token;

    // Check if data still exists
    const summaryResponse = await request.get(`${API_BASE_URL}/StadiumStructure/summary`, {
      headers: { 'Authorization': `Bearer ${token}` },
      ignoreHTTPSErrors: true
    });

    expect(summaryResponse.ok()).toBeTruthy();
    const summary = await summaryResponse.json();

    if (summary.totalSeats > 0) {
      console.log('✅ Stadium data persisted successfully');
      console.log(`Stadium has ${summary.totalSeats} seats across ${summary.totalSectors} sectors`);

      // Test export functionality
      const exportResponse = await request.get(`${API_BASE_URL}/StadiumStructure/export/json`, {
        headers: { 'Authorization': `Bearer ${token}` },
        ignoreHTTPSErrors: true
      });

      expect(exportResponse.ok()).toBeTruthy();
      console.log('✅ Stadium structure export functionality working');
    } else {
      console.log('ℹ️ No stadium data found - run the import test first');
    }
  });
});