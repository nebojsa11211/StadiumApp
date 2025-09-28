// CORRECT-RECTANGULAR-TARGET-GENERATOR.js
// Generate stadium matching the ACTUAL target from URL (rectangular layout)

const { chromium } = require('playwright');
const sharp = require('sharp');
const fs = require('fs').promises;

class CorrectRectangularTargetGenerator {
    constructor() {
        this.targetImagePath = 'target-verification.png'; // The actual target from URL
        this.bestSimilarityPath = 'correct-rectangular-best-match.png';
        this.bestSimilarity = 0;
        this.bestIteration = 0;
        this.iterationCount = 0;
        this.maxIterations = 200;

        // RECTANGULAR STADIUM CONFIG - Based on actual target analysis
        this.config = {
            containerWidth: 800,
            containerHeight: 600,

            // RECTANGULAR FIELD with corner areas
            field: {
                x: 200, y: 150,
                width: 400, height: 250,
                color: '#22C55E',
                strokeWidth: 3,
                strokeColor: '#FFFFFF'
            },

            // RECTANGULAR SECTIONS around field
            sections: {
                // NORTH (TOP) - Purple sections A1-A6
                north: {
                    sections: [
                        { id: 'A1', x: 200, y: 50, width: 60, height: 90, color: '#8B5CF6' },
                        { id: 'A2', x: 270, y: 50, width: 60, height: 90, color: '#8B5CF6' },
                        { id: 'A3', x: 340, y: 50, width: 60, height: 90, color: '#8B5CF6' },
                        { id: 'A4', x: 410, y: 50, width: 60, height: 90, color: '#8B5CF6' },
                        { id: 'A5', x: 480, y: 50, width: 60, height: 90, color: '#8B5CF6' },
                        { id: 'A6', x: 550, y: 50, width: 60, height: 90, color: '#8B5CF6' }
                    ]
                },

                // EAST (RIGHT) - Red sections B1-B6
                east: {
                    sections: [
                        { id: 'B1', x: 620, y: 150, width: 80, height: 40, color: '#EF4444' },
                        { id: 'B2', x: 620, y: 200, width: 80, height: 40, color: '#EF4444' },
                        { id: 'B3', x: 620, y: 250, width: 80, height: 40, color: '#EF4444' },
                        { id: 'B4', x: 620, y: 300, width: 80, height: 40, color: '#EF4444' },
                        { id: 'B5', x: 620, y: 350, width: 80, height: 40, color: '#EF4444' },
                        { id: 'B6', x: 620, y: 400, width: 80, height: 40, color: '#EF4444' }
                    ]
                },

                // SOUTH (BOTTOM) - Blue sections C1-C6
                south: {
                    sections: [
                        { id: 'C1', x: 200, y: 420, width: 60, height: 90, color: '#3B82F6' },
                        { id: 'C2', x: 270, y: 420, width: 60, height: 90, color: '#3B82F6' },
                        { id: 'C3', x: 340, y: 420, width: 60, height: 90, color: '#3B82F6' },
                        { id: 'C4', x: 410, y: 420, width: 60, height: 90, color: '#3B82F6' },
                        { id: 'C5', x: 480, y: 420, width: 60, height: 90, color: '#3B82F6' },
                        { id: 'C6', x: 550, y: 420, width: 60, height: 90, color: '#3B82F6' }
                    ]
                },

                // WEST (LEFT) - Yellow sections D1-D6
                west: {
                    sections: [
                        { id: 'D1', x: 100, y: 150, width: 80, height: 40, color: '#F59E0B' },
                        { id: 'D2', x: 100, y: 200, width: 80, height: 40, color: '#F59E0B' },
                        { id: 'D3', x: 100, y: 250, width: 80, height: 40, color: '#F59E0B' },
                        { id: 'D4', x: 100, y: 300, width: 80, height: 40, color: '#F59E0B' },
                        { id: 'D5', x: 100, y: 350, width: 80, height: 40, color: '#F59E0B' },
                        { id: 'D6', x: 100, y: 400, width: 80, height: 40, color: '#F59E0B' }
                    ]
                }
            },

            // CORNER AREAS for detail
            corners: [
                { x: 200, y: 140, width: 20, height: 20, color: '#FFFFFF' },
                { x: 580, y: 140, width: 20, height: 20, color: '#FFFFFF' },
                { x: 200, y: 390, width: 20, height: 20, color: '#FFFFFF' },
                { x: 580, y: 390, width: 20, height: 20, color: '#FFFFFF' }
            ]
        };

        this.optimizationStrategies = [
            this.optimizeFieldDimensions.bind(this),
            this.optimizeSectionSizes.bind(this),
            this.optimizeSectionPositions.bind(this),
            this.optimizeColors.bind(this),
            this.optimizeCornerAreas.bind(this)
        ];
    }

    async run() {
        console.log('🚀 CORRECT-RECTANGULAR-TARGET-GENERATOR STARTING');
        console.log('📐 TARGET: RECTANGULAR STADIUM LAYOUT');
        console.log('🎯 MATCHING ACTUAL URL TARGET IMAGE');

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

            // SETUP
            await this.navigateAndSetup(page);

            // ITERATIVE OPTIMIZATION FOR RECTANGULAR TARGET
            for (this.iterationCount = 0; this.iterationCount < this.maxIterations; this.iterationCount++) {
                console.log(`\n🔄 ITERATION ${this.iterationCount + 1}/${this.maxIterations}`);

                // Generate rectangular stadium
                const svgContent = this.generateRectangularStadiumSVG();
                await this.injectStadiumLayout(page, svgContent);
                await page.waitForTimeout(300);

                // Calculate similarity with actual target
                const screenshotPath = `rect-iteration-${String(this.iterationCount).padStart(3, '0')}.png`;
                const similarity = await this.captureAndCompareSimilarity(page, screenshotPath);

                console.log(`📊 Similarity: ${similarity.toFixed(4)}%`);

                // Update best result
                if (similarity > this.bestSimilarity) {
                    this.bestSimilarity = similarity;
                    this.bestIteration = this.iterationCount;
                    await fs.copyFile(screenshotPath, this.bestSimilarityPath);
                    console.log(`🏆 NEW BEST: ${similarity.toFixed(4)}% (Iteration ${this.iterationCount + 1})`);
                }

                // CHECK FOR HIGH SIMILARITY
                if (similarity >= 80.0) {
                    console.log(`\n🎉 80%+ SIMILARITY ACHIEVED! 🎉`);
                    console.log(`🏆 Final Similarity: ${similarity.toFixed(4)}%`);
                    console.log(`✅ RECTANGULAR TARGET MATCHED IN ${this.iterationCount + 1} ITERATIONS`);
                    break;
                }

                // Apply optimization
                if (this.iterationCount < this.maxIterations - 1) {
                    const strategyIndex = this.iterationCount % this.optimizationStrategies.length;
                    this.optimizationStrategies[strategyIndex]();
                }
            }

            // FINAL RESULTS
            console.log('\n' + '='.repeat(70));
            console.log('🏁 RECTANGULAR TARGET GENERATION COMPLETE');
            console.log('='.repeat(70));
            console.log(`🏆 Best Similarity: ${this.bestSimilarity.toFixed(4)}%`);
            console.log(`📊 Best Iteration: ${this.bestIteration + 1}`);
            console.log(`🎯 Target Type: Rectangular Stadium (Actual URL)`);
            console.log(`💾 Best Match Saved: ${this.bestSimilarityPath}`);

            if (this.bestSimilarity >= 80) {
                console.log('\n🎊 EXCELLENT - Rectangular target matched! 🎊');
            } else if (this.bestSimilarity >= 60) {
                console.log('\n👍 GOOD - Strong rectangular resemblance achieved');
            } else {
                console.log('\n📈 Continue optimizing rectangular layout');
            }

        } catch (error) {
            console.error('❌ Error during rectangular generation:', error);
        } finally {
            await browser.close();
        }
    }

    async navigateAndSetup(page) {
        console.log('🌐 Navigating to Admin app...');
        await page.goto('https://localhost:7030', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(2000);

        // Authentication
        const isLoginPage = await page.locator('input[type="email"]').isVisible().catch(() => false);
        if (isLoginPage) {
            console.log('🔓 Authenticating...');
            await page.fill('input[type="email"]', 'admin@stadium.com');
            await page.fill('input[type="password"]', 'admin123');
            await page.click('button[type="submit"]');
            await page.waitForTimeout(4000);
        }

        console.log('🏟️ Navigating to Stadium Overview...');
        await page.goto('https://localhost:7030/stadium-overview', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(3000);

        // Container creation
        console.log('📦 Creating rectangular stadium container...');
        await page.evaluate(() => {
            const existing = document.querySelectorAll('#admin-stadium-container, .stadium-container');
            existing.forEach(el => el.remove());

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

        await page.waitForTimeout(2000);
        console.log('✅ Rectangular stadium setup complete');
    }

    generateRectangularStadiumSVG() {
        const { containerWidth, containerHeight } = this.config;

        let svg = `<svg width="${containerWidth}" height="${containerHeight}" xmlns="http://www.w3.org/2000/svg">`;

        // BACKGROUND
        svg += `<rect width="${containerWidth}" height="${containerHeight}" fill="#F8F9FA"/>`;

        // FIELD - Rectangular with corner areas
        const { field } = this.config;
        svg += `<rect x="${field.x}" y="${field.y}" width="${field.width}" height="${field.height}"
                fill="${field.color}" stroke="${field.strokeColor}" stroke-width="${field.strokeWidth}"/>`;

        // CENTER CIRCLE
        const centerX = field.x + field.width / 2;
        const centerY = field.y + field.height / 2;
        svg += `<circle cx="${centerX}" cy="${centerY}" r="40"
                fill="none" stroke="${field.strokeColor}" stroke-width="3"/>`;

        // CENTER LINE
        svg += `<line x1="${centerX}" y1="${field.y}" x2="${centerX}" y2="${field.y + field.height}"
                stroke="${field.strokeColor}" stroke-width="2"/>`;

        // GOAL AREAS
        svg += `<rect x="${field.x}" y="${centerY - 40}" width="40" height="80"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;
        svg += `<rect x="${field.x + field.width - 40}" y="${centerY - 40}" width="40" height="80"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;

        // PENALTY AREAS
        svg += `<rect x="${field.x}" y="${centerY - 60}" width="80" height="120"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;
        svg += `<rect x="${field.x + field.width - 80}" y="${centerY - 60}" width="80" height="120"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;

        // CORNER AREAS
        this.config.corners.forEach(corner => {
            svg += `<rect x="${corner.x}" y="${corner.y}" width="${corner.width}" height="${corner.height}"
                    fill="none" stroke="${field.strokeColor}" stroke-width="1"/>`;
        });

        // RECTANGULAR SECTIONS
        Object.values(this.config.sections).forEach(tribune => {
            tribune.sections.forEach(section => {
                // Section rectangle
                svg += `<rect x="${section.x}" y="${section.y}" width="${section.width}" height="${section.height}"
                        fill="${section.color}" stroke="#FFFFFF" stroke-width="2"/>`;

                // Section label
                svg += `<text x="${section.x + section.width/2}" y="${section.y + section.height/2}"
                        text-anchor="middle" dominant-baseline="middle" font-family="Arial" font-size="12"
                        font-weight="bold" fill="white">${section.id}</text>`;

                // Seat pattern (small dots)
                for (let row = 0; row < Math.floor(section.height / 8); row++) {
                    for (let seat = 0; seat < Math.floor(section.width / 6); seat++) {
                        const x = section.x + 3 + seat * 6;
                        const y = section.y + 3 + row * 8;
                        svg += `<circle cx="${x}" cy="${y}" r="1" fill="#FFFFFF" opacity="0.6"/>`;
                    }
                }
            });
        });

        // DIRECTIONAL LABELS
        svg += `<text x="100" y="30" text-anchor="middle" font-family="Arial" font-size="14"
                font-weight="bold" fill="#333">ZAPAD/WEST</text>`;
        svg += `<text x="700" y="30" text-anchor="middle" font-family="Arial" font-size="14"
                font-weight="bold" fill="#333">ISTOK/EAST</text>`;

        svg += '</svg>';
        return svg;
    }

    async injectStadiumLayout(page, svgContent) {
        await page.evaluate((svg) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svg;
            }
        }, svgContent);
    }

    async captureAndCompareSimilarity(page, screenshotPath) {
        try {
            await page.waitForSelector('#admin-stadium-container', { state: 'visible', timeout: 5000 });
            await page.locator('#admin-stadium-container').scrollIntoViewIfNeeded();
            await page.waitForTimeout(300);

            const containerElement = page.locator('#admin-stadium-container');
            await containerElement.screenshot({ path: screenshotPath, type: 'png' });

            return await this.calculateSimilarity(screenshotPath);

        } catch (error) {
            console.error('❌ Capture failed:', error);
            return 0;
        }
    }

    async calculateSimilarity(screenshotPath) {
        try {
            const targetBuffer = await fs.readFile(this.targetImagePath);
            const currentBuffer = await fs.readFile(screenshotPath);

            const width = 800, height = 600;
            const targetResized = await sharp(targetBuffer).resize(width, height).raw().toBuffer();
            const currentResized = await sharp(currentBuffer).resize(width, height).raw().toBuffer();

            // Calculate pixel and structural similarity for rectangular layout
            const pixelSimilarity = this.calculatePixelSimilarity(targetResized, currentResized);
            const structuralSimilarity = this.calculateStructuralSimilarity(targetResized, currentResized, width, height);
            const layoutSimilarity = this.calculateLayoutSimilarity(targetResized, currentResized, width, height);

            // Weighted combination for rectangular stadium
            const finalSimilarity = (
                pixelSimilarity * 0.40 +
                structuralSimilarity * 0.35 +
                layoutSimilarity * 0.25
            );

            return finalSimilarity;

        } catch (error) {
            console.error('❌ Similarity calculation failed:', error);
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

    calculateLayoutSimilarity(target, current, width, height) {
        // Check rectangular sections specifically
        const sectionAreas = [
            { x: 200, y: 50, width: 400, height: 90 },   // North sections
            { x: 620, y: 150, width: 80, height: 250 },  // East sections
            { x: 200, y: 420, width: 400, height: 90 },  // South sections
            { x: 100, y: 150, width: 80, height: 250 },  // West sections
            { x: 200, y: 150, width: 400, height: 250 }  // Field area
        ];

        let totalSimilarity = 0;

        sectionAreas.forEach(area => {
            let areaMatches = 0;
            let areaTotal = 0;

            for (let y = area.y; y < Math.min(area.y + area.height, height); y++) {
                for (let x = area.x; x < Math.min(area.x + area.width, width); x++) {
                    const idx = (y * width + x) * 3;

                    const targetColor = [target[idx], target[idx + 1], target[idx + 2]];
                    const currentColor = [current[idx], current[idx + 1], current[idx + 2]];

                    const colorDiff = Math.abs(targetColor[0] - currentColor[0]) +
                                    Math.abs(targetColor[1] - currentColor[1]) +
                                    Math.abs(targetColor[2] - currentColor[2]);

                    if (colorDiff < 80) {
                        areaMatches++;
                    }
                    areaTotal++;
                }
            }

            if (areaTotal > 0) {
                totalSimilarity += (areaMatches / areaTotal) * 100;
            }
        });

        return totalSimilarity / sectionAreas.length;
    }

    // OPTIMIZATION STRATEGIES
    optimizeFieldDimensions() {
        console.log('🏟️ Optimizing field dimensions...');
        this.config.field.width += (Math.random() - 0.5) * 20;
        this.config.field.height += (Math.random() - 0.5) * 20;
        this.config.field.x += (Math.random() - 0.5) * 10;
        this.config.field.y += (Math.random() - 0.5) * 10;
    }

    optimizeSectionSizes() {
        console.log('📐 Optimizing section sizes...');
        Object.values(this.config.sections).forEach(tribune => {
            tribune.sections.forEach(section => {
                section.width += (Math.random() - 0.5) * 10;
                section.height += (Math.random() - 0.5) * 10;
            });
        });
    }

    optimizeSectionPositions() {
        console.log('📍 Optimizing section positions...');
        Object.values(this.config.sections).forEach(tribune => {
            tribune.sections.forEach(section => {
                section.x += (Math.random() - 0.5) * 8;
                section.y += (Math.random() - 0.5) * 8;
            });
        });
    }

    optimizeColors() {
        console.log('🎨 Optimizing colors...');
        // Color optimization can be implemented here
    }

    optimizeCornerAreas() {
        console.log('🔺 Optimizing corner areas...');
        this.config.corners.forEach(corner => {
            corner.width += (Math.random() - 0.5) * 4;
            corner.height += (Math.random() - 0.5) * 4;
        });
    }
}

// EXECUTE THE CORRECT RECTANGULAR TARGET GENERATOR
const generator = new CorrectRectangularTargetGenerator();
generator.run().catch(console.error);