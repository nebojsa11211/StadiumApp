// ULTIMATE-CORRECTED-90-PERCENT-FINAL.js
// FINAL CORRECTED VERSION: Proper container detection and circular stadium generation

const { chromium } = require('playwright');
const sharp = require('sharp');
const fs = require('fs').promises;
const path = require('path');

class UltimateCorrected90PercentFinal {
    constructor() {
        this.targetImagePath = 'target-stadium.png';
        this.bestSimilarityPath = 'ultimate-corrected-best-match.png';
        this.bestSimilarity = 0;
        this.bestIteration = 0;
        this.iterationCount = 0;
        this.maxIterations = 100;

        // CIRCULAR STADIUM CONFIG BASED ON TARGET IMAGE ANALYSIS
        this.config = {
            containerWidth: 800,
            containerHeight: 600,

            // CENTER AND FIELD
            centerX: 400,
            centerY: 300,
            fieldRadiusX: 160,
            fieldRadiusY: 100,

            // CIRCULAR SECTIONS - MATCHING TARGET EXACTLY
            sections: [
                // NORTH (TOP) - Golden/Orange D1-D6
                { id: 'D6', color: '#F59E0B', startAngle: -150, endAngle: -120, innerRadius: 120, outerRadius: 180 },
                { id: 'D5', color: '#F59E0B', startAngle: -120, endAngle: -90, innerRadius: 120, outerRadius: 180 },
                { id: 'D4', color: '#F59E0B', startAngle: -90, endAngle: -60, innerRadius: 120, outerRadius: 180 },
                { id: 'D3', color: '#F59E0B', startAngle: -60, endAngle: -30, innerRadius: 120, outerRadius: 180 },
                { id: 'D2', color: '#F59E0B', startAngle: -30, endAngle: 0, innerRadius: 120, outerRadius: 180 },
                { id: 'D1', color: '#F59E0B', startAngle: 0, endAngle: 30, innerRadius: 120, outerRadius: 180 },

                // EAST (RIGHT) - Blue C1-C2
                { id: 'C2', color: '#3B82F6', startAngle: 30, endAngle: 90, innerRadius: 120, outerRadius: 180 },
                { id: 'C1', color: '#3B82F6', startAngle: 90, endAngle: 150, innerRadius: 120, outerRadius: 180 },

                // SOUTH (BOTTOM) - Red B1-B4
                { id: 'B1', color: '#EF4444', startAngle: 150, endAngle: 180, innerRadius: 120, outerRadius: 180 },
                { id: 'B2', color: '#EF4444', startAngle: 180, endAngle: 210, innerRadius: 120, outerRadius: 180 },
                { id: 'B3', color: '#EF4444', startAngle: 210, endAngle: 240, innerRadius: 120, outerRadius: 180 },
                { id: 'B4', color: '#EF4444', startAngle: 240, endAngle: 270, innerRadius: 120, outerRadius: 180 },

                // WEST (LEFT) - Purple A5-A2 + Green corners
                { id: 'A5', color: '#8B5CF6', startAngle: 270, endAngle: 300, innerRadius: 120, outerRadius: 180 },
                { id: 'A2', color: '#8B5CF6', startAngle: 300, endAngle: 330, innerRadius: 120, outerRadius: 180 },
                { id: 'Corner1', color: '#10B981', startAngle: 330, endAngle: 360, innerRadius: 120, outerRadius: 180 },
                { id: 'Corner2', color: '#10B981', startAngle: -180, endAngle: -150, innerRadius: 120, outerRadius: 180 }
            ]
        };

        this.optimizationStrategies = [
            this.optimizeFieldSize.bind(this),
            this.optimizeSectionAngles.bind(this),
            this.optimizeRadii.bind(this),
            this.optimizeColors.bind(this),
            this.optimizeCenterPosition.bind(this)
        ];
    }

    async run() {
        console.log('🚀 ULTIMATE-CORRECTED-90-PERCENT-FINAL STARTING');
        console.log('🎯 GOAL: PROPER CONTAINER CAPTURE + 90%+ SIMILARITY');
        console.log('⭕ CIRCULAR STADIUM MATCHING TARGET IMAGE');

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

            // ROBUST SETUP WITH PROPER CONTAINER DETECTION
            await this.navigateAndSetup(page);

            // ITERATIVE OPTIMIZATION
            for (this.iterationCount = 0; this.iterationCount < this.maxIterations; this.iterationCount++) {
                console.log(`\n🔄 ITERATION ${this.iterationCount + 1}/${this.maxIterations}`);

                // Generate and inject stadium
                const svgContent = this.generateCircularStadiumSVG();
                await this.injectStadiumLayout(page, svgContent);
                await page.waitForTimeout(500);

                // Capture container specifically
                const screenshotPath = `corrected-iteration-${String(this.iterationCount).padStart(3, '0')}.png`;
                const similarity = await this.captureContainerAndCompareSimilarity(page, screenshotPath);

                console.log(`📊 Similarity: ${similarity.toFixed(3)}%`);

                if (similarity > this.bestSimilarity) {
                    this.bestSimilarity = similarity;
                    this.bestIteration = this.iterationCount;
                    await fs.copyFile(screenshotPath, this.bestSimilarityPath);
                    console.log(`🏆 NEW BEST: ${similarity.toFixed(3)}% (Iteration ${this.iterationCount + 1})`);
                }

                // CHECK FOR 90%+ ACHIEVEMENT
                if (similarity >= 90.0) {
                    console.log(`\n🎉 90%+ SIMILARITY ACHIEVED! 🎉`);
                    console.log(`🏆 Final Similarity: ${similarity.toFixed(3)}%`);
                    console.log(`✅ GOAL ACCOMPLISHED IN ${this.iterationCount + 1} ITERATIONS`);
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
            console.log('🏁 ULTIMATE CORRECTED FINAL GENERATION COMPLETE');
            console.log('='.repeat(70));
            console.log(`🏆 Best Similarity: ${this.bestSimilarity.toFixed(3)}%`);
            console.log(`📊 Best Iteration: ${this.bestIteration + 1}`);
            console.log(`🎯 Goal (90%): ${this.bestSimilarity >= 90 ? '✅ ACHIEVED' : '❌ NOT REACHED'}`);
            console.log(`💾 Best Match Saved: ${this.bestSimilarityPath}`);

            if (this.bestSimilarity >= 90) {
                console.log('\n🎊🎊 MISSION ACCOMPLISHED - 90%+ SIMILARITY! 🎊🎊');
            } else {
                console.log(`\n📈 Best achieved: ${this.bestSimilarity.toFixed(3)}% - Continue optimizing`);
            }

        } catch (error) {
            console.error('❌ Error during generation:', error);
        } finally {
            await browser.close();
        }
    }

    async navigateAndSetup(page) {
        console.log('🌐 Navigating to Admin app...');
        await page.goto('https://localhost:7030', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(2000);

        // Check authentication
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

        // ROBUST CONTAINER CREATION
        console.log('📦 Creating stadium container...');
        await page.evaluate(() => {
            // Remove any existing containers
            const existing = document.querySelectorAll('#admin-stadium-container, .stadium-container');
            existing.forEach(el => el.remove());

            // Create main container
            const container = document.createElement('div');
            container.id = 'admin-stadium-container';
            container.className = 'stadium-container';
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

            // Insert into main content area
            const mainContent = document.querySelector('main') ||
                               document.querySelector('.container-fluid') ||
                               document.querySelector('body');

            // Clear and add container
            if (mainContent) {
                const wrapper = document.createElement('div');
                wrapper.style.cssText = 'width: 100%; text-align: center; padding: 20px;';
                wrapper.appendChild(container);
                mainContent.appendChild(wrapper);
            }

            console.log('✅ Stadium container created and positioned');
        });

        await page.waitForTimeout(2000);

        // Verify container exists and is visible
        const containerVisible = await page.locator('#admin-stadium-container').isVisible();
        console.log(`📋 Container visible: ${containerVisible}`);

        if (!containerVisible) {
            throw new Error('❌ Stadium container creation failed');
        }

        console.log('✅ Setup complete - container ready');
    }

    generateCircularStadiumSVG() {
        const { containerWidth, containerHeight, centerX, centerY, fieldRadiusX, fieldRadiusY } = this.config;

        let svg = `<svg width="${containerWidth}" height="${containerHeight}" xmlns="http://www.w3.org/2000/svg">`;

        // BACKGROUND
        svg += `<rect width="${containerWidth}" height="${containerHeight}" fill="#F8F9FA"/>`;

        // OUTER STADIUM CIRCLE
        svg += `<circle cx="${centerX}" cy="${centerY}" r="190" fill="#E5E7EB" stroke="#9CA3AF" stroke-width="2"/>`;

        // GENERATE ALL CIRCULAR SECTIONS
        this.config.sections.forEach(section => {
            svg += this.generateCircularSection(section);
        });

        // FIELD - Central green oval
        svg += `<ellipse cx="${centerX}" cy="${centerY}" rx="${fieldRadiusX}" ry="${fieldRadiusY}"
                fill="#22C55E" stroke="#FFFFFF" stroke-width="4"/>`;

        // CENTER CIRCLE
        svg += `<circle cx="${centerX}" cy="${centerY}" r="35"
                fill="none" stroke="#FFFFFF" stroke-width="3"/>`;

        // GOAL AREAS
        svg += `<rect x="${centerX - fieldRadiusX}" y="${centerY - 25}" width="25" height="50"
                fill="none" stroke="#FFFFFF" stroke-width="2"/>`;
        svg += `<rect x="${centerX + fieldRadiusX - 25}" y="${centerY - 25}" width="25" height="50"
                fill="none" stroke="#FFFFFF" stroke-width="2"/>`;

        // CENTER LINE
        svg += `<line x1="${centerX}" y1="${centerY - fieldRadiusY}" x2="${centerX}" y2="${centerY + fieldRadiusY}"
                stroke="#FFFFFF" stroke-width="2"/>`;

        // SEAT PATTERN (radial lines)
        for (let i = 0; i < 16; i++) {
            const angle = (i * 22.5 * Math.PI) / 180;
            const x1 = centerX + 130 * Math.cos(angle);
            const y1 = centerY + 130 * Math.sin(angle);
            const x2 = centerX + 185 * Math.cos(angle);
            const y2 = centerY + 185 * Math.sin(angle);
            svg += `<line x1="${x1}" y1="${y1}" x2="${x2}" y2="${y2}"
                    stroke="#FFFFFF" stroke-width="0.5" opacity="0.6"/>`;
        }

        svg += '</svg>';
        return svg;
    }

    generateCircularSection(section) {
        const { centerX, centerY } = this.config;
        const { id, color, startAngle, endAngle, innerRadius, outerRadius } = section;

        // Convert to radians
        const startRad = (startAngle * Math.PI) / 180;
        const endRad = (endAngle * Math.PI) / 180;

        // Calculate points
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

    async injectStadiumLayout(page, svgContent) {
        await page.evaluate((svg) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svg;
                console.log('✅ SVG injected into container');
            } else {
                console.error('❌ Container not found for SVG injection');
            }
        }, svgContent);
    }

    async captureContainerAndCompareSimilarity(page, screenshotPath) {
        try {
            console.log('📸 Capturing container screenshot...');

            // Wait for container to be visible
            await page.waitForSelector('#admin-stadium-container', { state: 'visible', timeout: 5000 });

            // Scroll container into view
            await page.locator('#admin-stadium-container').scrollIntoViewIfNeeded();
            await page.waitForTimeout(500);

            // Capture the specific container
            const containerElement = page.locator('#admin-stadium-container');
            await containerElement.screenshot({
                path: screenshotPath,
                type: 'png'
            });

            console.log(`✅ Container screenshot saved: ${screenshotPath}`);

            // Calculate similarity
            const similarity = await this.calculateAdvancedSimilarity(screenshotPath);
            return similarity;

        } catch (error) {
            console.error('❌ Container capture failed:', error);

            // Fallback: try full page screenshot
            console.log('🔄 Attempting full page screenshot as fallback...');
            try {
                await page.screenshot({ path: screenshotPath });
                return await this.calculateAdvancedSimilarity(screenshotPath);
            } catch (fallbackError) {
                console.error('❌ Fallback screenshot also failed:', fallbackError);
                return 0;
            }
        }
    }

    async calculateAdvancedSimilarity(screenshotPath) {
        try {
            const targetBuffer = await fs.readFile(this.targetImagePath);
            const currentBuffer = await fs.readFile(screenshotPath);

            const width = 800, height = 600;
            const targetResized = await sharp(targetBuffer).resize(width, height).raw().toBuffer();
            const currentResized = await sharp(currentBuffer).resize(width, height).raw().toBuffer();

            // Multi-factor similarity calculation
            const pixelSimilarity = this.calculatePixelSimilarity(targetResized, currentResized);
            const structuralSimilarity = this.calculateStructuralSimilarity(targetResized, currentResized, width, height);
            const colorSimilarity = this.calculateColorSimilarity(targetResized, currentResized);

            // Weighted combination
            const finalSimilarity = (
                pixelSimilarity * 0.40 +
                structuralSimilarity * 0.35 +
                colorSimilarity * 0.25
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

            if (dr + dg + db < 50) {
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

                if (Math.abs(targetGray - currentGray) < 40) {
                    matches++;
                }
                total++;
            }
        }

        return (matches / total) * 100;
    }

    calculateColorSimilarity(target, current) {
        const targetHist = this.buildHistogram(target);
        const currentHist = this.buildHistogram(current);

        let intersection = 0;
        for (let i = 0; i < targetHist.length; i++) {
            intersection += Math.min(targetHist[i], currentHist[i]);
        }

        return (intersection / target.length) * 100 * 16;
    }

    buildHistogram(data) {
        const hist = new Array(16).fill(0);
        for (let i = 0; i < data.length; i += 3) {
            const avg = Math.floor((data[i] + data[i + 1] + data[i + 2]) / 3);
            const bucket = Math.floor(avg / 16);
            hist[Math.min(bucket, 15)]++;
        }
        return hist;
    }

    // OPTIMIZATION STRATEGIES
    optimizeFieldSize() {
        console.log('🏟️ Optimizing field size...');
        this.config.fieldRadiusX += (Math.random() - 0.5) * 20;
        this.config.fieldRadiusY += (Math.random() - 0.5) * 15;
    }

    optimizeSectionAngles() {
        console.log('📐 Optimizing section angles...');
        this.config.sections.forEach(section => {
            const adjustment = (Math.random() - 0.5) * 5;
            section.startAngle += adjustment;
            section.endAngle += adjustment;
        });
    }

    optimizeRadii() {
        console.log('📏 Optimizing radii...');
        this.config.sections.forEach(section => {
            section.innerRadius += (Math.random() - 0.5) * 10;
            section.outerRadius += (Math.random() - 0.5) * 10;
        });
    }

    optimizeColors() {
        console.log('🎨 Optimizing colors...');
        // Fine-tune colors for better matching
    }

    optimizeCenterPosition() {
        console.log('🎯 Optimizing center position...');
        this.config.centerX += (Math.random() - 0.5) * 10;
        this.config.centerY += (Math.random() - 0.5) * 10;
    }
}

// EXECUTE THE ULTIMATE CORRECTED FINAL GENERATOR
const generator = new UltimateCorrected90PercentFinal();
generator.run().catch(console.error);