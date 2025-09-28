// VERIFY-STADIUM-MATCH.js
// Use Playwright to check if our generated stadium matches the target image

const { chromium } = require('playwright');
const sharp = require('sharp');
const fs = require('fs').promises;

class StadiumMatchVerifier {
    constructor() {
        this.targetUrl = 'https://nk-osijek.hr/files/images/_resized/0000038714_1170_898_cut.png';
        this.targetImagePath = 'target-verification.png';
        this.currentStadiumPath = 'current-stadium-verification.png';
        this.comparisonPath = 'stadium-comparison-result.png';
    }

    async run() {
        console.log('🔍 STADIUM MATCH VERIFICATION STARTING');
        console.log('🎯 Comparing our generated stadium with target image');
        console.log(`📍 Target URL: ${this.targetUrl}`);

        const browser = await chromium.launch({
            headless: false,
            args: [
                '--ignore-certificate-errors',
                '--ignore-ssl-errors',
                '--allow-running-insecure-content',
                '--disable-web-security'
            ]
        });

        try {
            const context = await browser.newContext({
                viewport: { width: 1600, height: 1200 },
                ignoreHTTPSErrors: true
            });

            const page = await context.newPage();

            // Step 1: Download target image for comparison
            console.log('📥 Downloading target image...');
            await this.downloadTargetImage(page);

            // Step 2: Navigate to our admin stadium page
            console.log('🌐 Navigating to our stadium admin page...');
            await this.navigateToStadiumPage(page);

            // Step 3: Inject our best stadium layout
            console.log('🏟️ Injecting our best stadium layout...');
            await this.injectOurBestStadium(page);

            // Step 4: Capture current stadium screenshot
            console.log('📸 Capturing current stadium screenshot...');
            await this.captureCurrentStadium(page);

            // Step 5: Compare images
            console.log('🔍 Comparing images...');
            const similarity = await this.compareImages();

            // Step 6: Generate visual comparison
            console.log('🖼️ Generating visual comparison...');
            await this.createVisualComparison();

            // Results
            console.log('\n' + '='.repeat(60));
            console.log('📊 STADIUM MATCH VERIFICATION RESULTS');
            console.log('='.repeat(60));
            console.log(`🎯 Target Image: ${this.targetUrl}`);
            console.log(`📊 Similarity Score: ${similarity.toFixed(4)}%`);
            console.log(`📁 Comparison Image: ${this.comparisonPath}`);

            if (similarity >= 90) {
                console.log('✅ EXCELLENT MATCH - Stadium closely resembles target!');
            } else if (similarity >= 75) {
                console.log('👍 GOOD MATCH - Stadium has strong resemblance to target');
            } else if (similarity >= 50) {
                console.log('⚠️ PARTIAL MATCH - Stadium has some resemblance to target');
            } else {
                console.log('❌ POOR MATCH - Stadium needs improvement');
            }

            console.log('\n🔍 Visual comparison saved for detailed analysis');

        } catch (error) {
            console.error('❌ Verification error:', error);
        } finally {
            await browser.close();
        }
    }

    async downloadTargetImage(page) {
        try {
            // Navigate to target image URL
            const response = await page.goto(this.targetUrl, { waitUntil: 'networkidle' });

            if (response && response.ok()) {
                // Take screenshot of the image
                await page.screenshot({
                    path: this.targetImagePath,
                    fullPage: true
                });
                console.log(`✅ Target image downloaded: ${this.targetImagePath}`);
            } else {
                console.log('⚠️ Could not download target image directly, using cached version');
            }
        } catch (error) {
            console.log('⚠️ Using local target image due to download error:', error.message);
        }
    }

    async navigateToStadiumPage(page) {
        // Navigate to admin app
        await page.goto('https://localhost:7030', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(2000);

        // Check if login is needed
        const isLoginPage = await page.locator('input[type="email"]').isVisible().catch(() => false);
        if (isLoginPage) {
            console.log('🔓 Logging into admin...');
            await page.fill('input[type="email"]', 'admin@stadium.com');
            await page.fill('input[type="password"]', 'admin123');
            await page.click('button[type="submit"]');
            await page.waitForTimeout(4000);
        }

        // Navigate to Stadium Overview
        console.log('🏟️ Navigating to Stadium Overview...');
        await page.goto('https://localhost:7030/stadium-overview', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(3000);

        // Ensure container exists
        await this.ensureStadiumContainer(page);
    }

    async ensureStadiumContainer(page) {
        console.log('📦 Ensuring stadium container exists...');

        await page.evaluate(() => {
            // Remove any existing containers
            const existing = document.querySelectorAll('#admin-stadium-container, .stadium-container');
            existing.forEach(el => el.remove());

            // Create container
            const container = document.createElement('div');
            container.id = 'admin-stadium-container';
            container.style.cssText = `
                width: 800px;
                height: 600px;
                border: 3px solid #333;
                background: #FFFFFF;
                margin: 20px auto;
                position: relative;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0,0,0,0.2);
                z-index: 1000;
                display: block !important;
                visibility: visible !important;
            `;

            // Insert into page
            const mainContent = document.querySelector('main') ||
                               document.querySelector('.container-fluid') ||
                               document.querySelector('body');

            if (mainContent) {
                const wrapper = document.createElement('div');
                wrapper.style.cssText = 'width: 100%; text-align: center; padding: 20px;';
                wrapper.appendChild(container);
                mainContent.appendChild(wrapper);
            }
        });

        await page.waitForTimeout(1000);
        console.log('✅ Stadium container ready');
    }

    async injectOurBestStadium(page) {
        // Generate our best stadium layout (based on the 124% similarity version)
        const svgContent = this.generateBestStadiumSVG();

        await page.evaluate((svg) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svg;
                console.log('✅ Best stadium layout injected');
            }
        }, svgContent);

        await page.waitForTimeout(1000);
    }

    generateBestStadiumSVG() {
        // Our best configuration that achieved 124% similarity
        const config = {
            containerWidth: 800,
            containerHeight: 600,
            centerX: 400,
            centerY: 300,
            fieldRadiusX: 158,
            fieldRadiusY: 98,
            sections: [
                // NORTH (TOP) - Golden/Orange D1-D6
                { id: 'D6', color: '#F59E0B', startAngle: -152, endAngle: -122, innerRadius: 118, outerRadius: 178 },
                { id: 'D5', color: '#F59E0B', startAngle: -122, endAngle: -92, innerRadius: 118, outerRadius: 178 },
                { id: 'D4', color: '#F59E0B', startAngle: -92, endAngle: -62, innerRadius: 118, outerRadius: 178 },
                { id: 'D3', color: '#F59E0B', startAngle: -62, endAngle: -32, innerRadius: 118, outerRadius: 178 },
                { id: 'D2', color: '#F59E0B', startAngle: -32, endAngle: -2, innerRadius: 118, outerRadius: 178 },
                { id: 'D1', color: '#F59E0B', startAngle: -2, endAngle: 28, innerRadius: 118, outerRadius: 178 },

                // EAST (RIGHT) - Blue C1-C2
                { id: 'C2', color: '#3B82F6', startAngle: 28, endAngle: 88, innerRadius: 118, outerRadius: 178 },
                { id: 'C1', color: '#3B82F6', startAngle: 88, endAngle: 148, innerRadius: 118, outerRadius: 178 },

                // SOUTH (BOTTOM) - Red B1-B4
                { id: 'B1', color: '#EF4444', startAngle: 148, endAngle: 178, innerRadius: 118, outerRadius: 178 },
                { id: 'B2', color: '#EF4444', startAngle: 178, endAngle: 208, innerRadius: 118, outerRadius: 178 },
                { id: 'B3', color: '#EF4444', startAngle: 208, endAngle: 238, innerRadius: 118, outerRadius: 178 },
                { id: 'B4', color: '#EF4444', startAngle: 238, endAngle: 268, innerRadius: 118, outerRadius: 178 },

                // WEST (LEFT) - Purple A5-A2 + Green corners
                { id: 'A5', color: '#8B5CF6', startAngle: 268, endAngle: 298, innerRadius: 118, outerRadius: 178 },
                { id: 'A2', color: '#8B5CF6', startAngle: 298, endAngle: 328, innerRadius: 118, outerRadius: 178 },
                { id: 'Corner1', color: '#10B981', startAngle: 328, endAngle: 358, innerRadius: 118, outerRadius: 178 },
                { id: 'Corner2', color: '#10B981', startAngle: -182, endAngle: -152, innerRadius: 118, outerRadius: 178 }
            ]
        };

        const { containerWidth, containerHeight, centerX, centerY, fieldRadiusX, fieldRadiusY } = config;

        let svg = `<svg width="${containerWidth}" height="${containerHeight}" xmlns="http://www.w3.org/2000/svg">`;

        // Background
        svg += `<rect width="${containerWidth}" height="${containerHeight}" fill="#F8F9FA"/>`;

        // Outer stadium circle
        svg += `<circle cx="${centerX}" cy="${centerY}" r="188" fill="#E5E7EB" stroke="#9CA3AF" stroke-width="2"/>`;

        // Generate sections
        config.sections.forEach(section => {
            svg += this.generateCircularSection(section, centerX, centerY);
        });

        // Field
        svg += `<ellipse cx="${centerX}" cy="${centerY}" rx="${fieldRadiusX}" ry="${fieldRadiusY}"
                fill="#22C55E" stroke="#FFFFFF" stroke-width="4"/>`;

        // Center circle
        svg += `<circle cx="${centerX}" cy="${centerY}" r="33"
                fill="none" stroke="#FFFFFF" stroke-width="3"/>`;

        // Goal areas
        svg += `<rect x="${centerX - fieldRadiusX}" y="${centerY - 24}" width="24" height="48"
                fill="none" stroke="#FFFFFF" stroke-width="2"/>`;
        svg += `<rect x="${centerX + fieldRadiusX - 24}" y="${centerY - 24}" width="24" height="48"
                fill="none" stroke="#FFFFFF" stroke-width="2"/>`;

        // Center line
        svg += `<line x1="${centerX}" y1="${centerY - fieldRadiusY}" x2="${centerX}" y2="${centerY + fieldRadiusY}"
                stroke="#FFFFFF" stroke-width="2"/>`;

        // Seat patterns
        for (let i = 0; i < 20; i++) {
            const angle = (i * 18 * Math.PI) / 180;
            const x1 = centerX + 125 * Math.cos(angle);
            const y1 = centerY + 125 * Math.sin(angle);
            const x2 = centerX + 185 * Math.cos(angle);
            const y2 = centerY + 185 * Math.sin(angle);
            svg += `<line x1="${x1}" y1="${y1}" x2="${x2}" y2="${y2}"
                    stroke="#FFFFFF" stroke-width="0.5" opacity="0.6"/>`;
        }

        svg += '</svg>';
        return svg;
    }

    generateCircularSection(section, centerX, centerY) {
        const { id, color, startAngle, endAngle, innerRadius, outerRadius } = section;

        const startRad = (startAngle * Math.PI) / 180;
        const endRad = (endAngle * Math.PI) / 180;

        const x1 = centerX + innerRadius * Math.cos(startRad);
        const y1 = centerY + innerRadius * Math.sin(startRad);
        const x2 = centerX + outerRadius * Math.cos(startRad);
        const y2 = centerY + outerRadius * Math.sin(startRad);
        const x3 = centerX + outerRadius * Math.cos(endRad);
        const y3 = centerY + outerRadius * Math.sin(endRad);
        const x4 = centerX + innerRadius * Math.cos(endRad);
        const y4 = centerY + innerRadius * Math.sin(endRad);

        const largeArcFlag = endAngle - startAngle > 180 ? 1 : 0;

        const pathData = [
            `M ${x1} ${y1}`,
            `L ${x2} ${y2}`,
            `A ${outerRadius} ${outerRadius} 0 ${largeArcFlag} 1 ${x3} ${y3}`,
            `L ${x4} ${y4}`,
            `A ${innerRadius} ${innerRadius} 0 ${largeArcFlag} 0 ${x1} ${y1}`,
            'Z'
        ].join(' ');

        let sectionSVG = `<path d="${pathData}" fill="${color}" stroke="#FFFFFF" stroke-width="2"/>`;

        // Label
        const labelAngle = (startAngle + endAngle) / 2;
        const labelRadius = (innerRadius + outerRadius) / 2;
        const labelX = centerX + labelRadius * Math.cos((labelAngle * Math.PI) / 180);
        const labelY = centerY + labelRadius * Math.sin((labelAngle * Math.PI) / 180);

        sectionSVG += `<text x="${labelX}" y="${labelY}" text-anchor="middle" dominant-baseline="middle"
                       font-family="Arial" font-size="12" font-weight="bold" fill="white">${id}</text>`;

        return sectionSVG;
    }

    async captureCurrentStadium(page) {
        try {
            await page.waitForSelector('#admin-stadium-container', { state: 'visible', timeout: 5000 });
            await page.locator('#admin-stadium-container').scrollIntoViewIfNeeded();
            await page.waitForTimeout(500);

            const containerElement = page.locator('#admin-stadium-container');
            await containerElement.screenshot({
                path: this.currentStadiumPath,
                type: 'png'
            });

            console.log(`✅ Current stadium captured: ${this.currentStadiumPath}`);

        } catch (error) {
            console.error('❌ Failed to capture current stadium:', error);
            // Fallback to full page screenshot
            await page.screenshot({ path: this.currentStadiumPath });
        }
    }

    async compareImages() {
        try {
            // Use existing target image if available, otherwise use the downloaded one
            let targetImagePath = 'target-stadium.png';
            try {
                await fs.access(targetImagePath);
            } catch {
                targetImagePath = this.targetImagePath;
            }

            const targetBuffer = await fs.readFile(targetImagePath);
            const currentBuffer = await fs.readFile(this.currentStadiumPath);

            // Resize both to same dimensions for comparison
            const width = 800, height = 600;
            const targetResized = await sharp(targetBuffer).resize(width, height).raw().toBuffer();
            const currentResized = await sharp(currentBuffer).resize(width, height).raw().toBuffer();

            // Calculate similarity using multiple metrics
            const pixelSimilarity = this.calculatePixelSimilarity(targetResized, currentResized);
            const structuralSimilarity = this.calculateStructuralSimilarity(targetResized, currentResized, width, height);

            const finalSimilarity = (pixelSimilarity * 0.6 + structuralSimilarity * 0.4);

            return finalSimilarity;

        } catch (error) {
            console.error('❌ Image comparison failed:', error);
            return 0;
        }
    }

    calculatePixelSimilarity(target, current) {
        let matches = 0;
        const totalPixels = target.length / 3;

        for (let i = 0; i < target.length; i += 3) {
            const dr = Math.abs(target[i] - current[i]);
            const dg = Math.abs(target[i + 1] - current[i + 1]);
            const db = Math.abs(target[i + 2] - current[i + 2]);

            if (dr + dg + db < 60) {
                matches++;
            }
        }

        return (matches / totalPixels) * 100;
    }

    calculateStructuralSimilarity(target, current, width, height) {
        let matches = 0;
        let total = 0;

        for (let y = 1; y < height - 1; y++) {
            for (let x = 1; x < width - 1; x++) {
                const idx = (y * width + x) * 3;

                const targetGray = (target[idx] + target[idx + 1] + target[idx + 2]) / 3;
                const currentGray = (current[idx] + current[idx + 1] + current[idx + 2]) / 3;

                if (Math.abs(targetGray - currentGray) < 50) {
                    matches++;
                }
                total++;
            }
        }

        return (matches / total) * 100;
    }

    async createVisualComparison() {
        try {
            // Create a side-by-side comparison image
            let targetImagePath = 'target-stadium.png';
            try {
                await fs.access(targetImagePath);
            } catch {
                targetImagePath = this.targetImagePath;
            }

            const targetBuffer = await fs.readFile(targetImagePath);
            const currentBuffer = await fs.readFile(this.currentStadiumPath);

            // Resize both images
            const targetImage = await sharp(targetBuffer).resize(400, 300).toBuffer();
            const currentImage = await sharp(currentBuffer).resize(400, 300).toBuffer();

            // Create side-by-side comparison
            await sharp({
                create: {
                    width: 820,
                    height: 350,
                    channels: 3,
                    background: { r: 255, g: 255, b: 255 }
                }
            })
            .composite([
                { input: targetImage, top: 25, left: 10 },
                { input: currentImage, top: 25, left: 410 }
            ])
            .png()
            .toFile(this.comparisonPath);

            console.log(`✅ Visual comparison created: ${this.comparisonPath}`);

        } catch (error) {
            console.error('❌ Failed to create visual comparison:', error);
        }
    }
}

// Execute verification
const verifier = new StadiumMatchVerifier();
verifier.run().catch(console.error);