// Auto Multi-Tab Launcher for Visual Studio
// This script runs automatically when Visual Studio starts debugging

const { exec } = require('child_process');
const http = require('http');

// Configuration
const urls = [
    'https://localhost:7020',   // Customer App
    'https://localhost:7030',   // Admin App
    'https://localhost:7010/swagger'  // API Swagger
];

const maxWaitTime = 30000; // 30 seconds
const checkInterval = 1000; // 1 second

console.log('🚀 Auto Multi-Tab Launcher Starting...');
console.log('📋 URLs to open:', urls.join(', '));

// Check if a service is ready
function checkService(url) {
    return new Promise((resolve) => {
        const req = http.get(url, (res) => {
            resolve(res.statusCode === 200);
        });
        
        req.on('error', () => resolve(false));
        req.setTimeout(2000, () => {
            req.destroy();
            resolve(false);
        });
    });
}

// Wait for all services to be ready
async function waitForAllServices() {
    console.log('⏳ Waiting for all services to start...');
    
    const startTime = Date.now();
    
    while (Date.now() - startTime < maxWaitTime) {
        const results = await Promise.all(urls.map(checkService));
        
        if (results.every(ready => ready)) {
            console.log('✅ All services are ready!');
            return true;
        }
        
        console.log(`⏳ Waiting... ${Math.round((Date.now() - startTime) / 1000)}s`);
        await new Promise(resolve => setTimeout(resolve, checkInterval));
    }
    
    console.log('⚠️ Some services may not be ready, opening tabs anyway...');
    return false;
}

// Open all URLs in browser tabs
async function openAllTabs() {
    console.log('🌐 Opening browser tabs...');
    
    for (let i = 0; i < urls.length; i++) {
        const url = urls[i];
        console.log(`📍 Opening tab ${i + 1}: ${url}`);
        
        // Use different methods based on OS
        const command = process.platform === 'win32' 
            ? `start ${url}` 
            : process.platform === 'darwin' 
                ? `open ${url}` 
                : `xdg-open ${url}`;
        
        exec(command);
        
        // Small delay between tabs
        await new Promise(resolve => setTimeout(resolve, 500));
    }
    
    console.log('✅ All tabs opened successfully!');
}

// Main execution
async function main() {
    try {
        await waitForAllServices();
        await openAllTabs();
    } catch (error) {
        console.error('❌ Error:', error.message);
    }
}

// Run immediately
main();
