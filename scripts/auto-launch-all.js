// Auto-launch script for Visual Studio multi-tab browser opening
// This script runs automatically after Docker services start

const { exec } = require('child_process');
const http = require('http');

// URLs to open
const urls = [
    'http://localhost:5002',   // Customer App
    'http://localhost:5003',   // Admin App
    'http://localhost:5001/swagger'  // API Swagger
];

// Wait for services to be ready
function waitForService(url, timeout = 30000) {
    return new Promise((resolve, reject) => {
        const startTime = Date.now();
        
        const checkService = () => {
            http.get(url, (res) => {
                if (res.statusCode === 200) {
                    console.log(`✅ ${url} is ready`);
                    resolve();
                } else {
                    setTimeout(checkService, 1000);
                }
            }).on('error', () => {
                if (Date.now() - startTime > timeout) {
                    reject(new Error(`Timeout waiting for ${url}`));
                } else {
                    setTimeout(checkService, 1000);
                }
            });
        };
        
        checkService();
    });
}

// Open all URLs in browser tabs
async function openAllTabs() {
    console.log('🚀 Starting multi-tab browser launch...');
    
    try {
        // Wait for all services to be ready
        console.log('⏳ Waiting for services to start...');
        await Promise.all([
            waitForService('http://localhost:5002'),
            waitForService('http://localhost:5003'),
            waitForService('http://localhost:5001/swagger')
        ]);
        
        console.log('🌐 Opening browser tabs...');
        
        // Open all URLs
        for (const url of urls) {
            console.log(`📍 Opening: ${url}`);
            exec(`start ${url}`);
            await new Promise(resolve => setTimeout(resolve, 500)); // Small delay between tabs
        }
        
        console.log('✅ All browser tabs opened successfully!');
        
    } catch (error) {
        console.error('❌ Error:', error.message);
        console.log('💡 Please manually open these URLs:');
        urls.forEach(url => console.log(`   ${url}`));
    }
}

// Run the script
openAllTabs();
