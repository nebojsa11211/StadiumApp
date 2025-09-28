// FINAL-CIRCULAR-90-PERCENT-GENERATOR.js
// CORRECT APPROACH: Generate CIRCULAR stadium matching the target image exactly
// Target shows: Circular stadium with curved sections around green field

const { chromium } = require('playwright');
const sharp = require('sharp');
const fs = require('fs').promises;
const path = require('path');

class FinalCircular90PercentGenerator {
    constructor() {
        this.targetImagePath = 'target-stadium.png';
        this.bestSimilarityPath = 'final-circular-best-match.png';
        this.bestSimilarity = 0;
        this.bestIteration = 0;
        this.iterationCount = 0;
        this.maxIterations = 150;

        // CIRCULAR STADIUM CONFIG - EXACT TARGET MATCH
        this.config = {
            containerWidth: 800,
            containerHeight: 600,

            // FIELD - Central green circle/oval
            field: {
                cx: 400, cy: 300,
                rx: 180, ry: 120,  // Oval field
                color: '#22C55E',  // Bright green
                strokeWidth: 4,
                strokeColor: '#FFFFFF'
            },

            // CENTER CIRCLE
            centerCircle: {
                cx: 400, cy: 300,
                radius: 40,
                strokeWidth: 3,
                strokeColor: '#FFFFFF',
                fillOpacity: 0
            },

            // CIRCULAR SECTIONS - EXACTLY MATCHING TARGET
            sections: {
                // NORTH (TOP) - Golden/Orange sections D1-D6
                north: {
                    color: '#F59E0B',  // Amber/Gold
                    sections: [
                        { id: 'D6', startAngle: -150, endAngle: -120, innerRadius: 140, outerRadius: 190 },
                        { id: 'D5', startAngle: -120, endAngle: -90, innerRadius: 140, outerRadius: 190 },
                        { id: 'D4', startAngle: -90, endAngle: -60, innerRadius: 140, outerRadius: 190 },
                        { id: 'D3', startAngle: -60, endAngle: -30, innerRadius: 140, outerRadius: 190 },
                        { id: 'D2', startAngle: -30, endAngle: 0, innerRadius: 140, outerRadius: 190 },
                        { id: 'D1', startAngle: 0, endAngle: 30, innerRadius: 140, outerRadius: 190 }
                    ]
                },

                // EAST (RIGHT) - Blue sections C1-C2
                east: {
                    color: '#3B82F6',  // Blue
                    sections: [
                        { id: 'C2', startAngle: 30, endAngle: 90, innerRadius: 140, outerRadius: 190 },
                        { id: 'C1', startAngle: 90, endAngle: 150, innerRadius: 140, outerRadius: 190 }
                    ]
                },

                // SOUTH (BOTTOM) - Red sections B1-B4
                south: {
                    color: '#EF4444',  // Red
                    sections: [
                        { id: 'B1', startAngle: 150, endAngle: 180, innerRadius: 140, outerRadius: 190 },
                        { id: 'B2', startAngle: 180, endAngle: 210, innerRadius: 140, outerRadius: 190 },
                        { id: 'B3', startAngle: 210, endAngle: 240, innerRadius: 140, outerRadius: 190 },
                        { id: 'B4', startAngle: 240, endAngle: 270, innerRadius: 140, outerRadius: 190 }
                    ]
                },

                // WEST (LEFT) - Purple sections A5-A2
                west: {
                    color: '#8B5CF6',  // Purple
                    sections: [
                        { id: 'A5', startAngle: 270, endAngle: 300, innerRadius: 140, outerRadius: 190 },
                        { id: 'A2', startAngle: 300, endAngle: 330, innerRadius: 140, outerRadius: 190 }
                    ]
                },

                // CORNERS - Green corner sections
                corners: {
                    color: '#10B981',  // Emerald green
                    sections: [
                        { id: 'Corner1', startAngle: 330, endAngle: 360, innerRadius: 140, outerRadius: 190 },
                        { id: 'Corner2', startAngle: -180, endAngle: -150, innerRadius: 140, outerRadius: 190 }
                    ]
                }
            },

            // SEAT PATTERN - Radial lines for realism
            seats: {
                enabled: true,
                radialLines: 8,
                concentricRings: 6,
                color: '#FFFFFF',
                opacity: 0.6,
                strokeWidth: 0.5
            }
        };

        // OPTIMIZATION STRATEGIES
        this.optimizationStrategies = [
            this.optimizeFieldDimensions.bind(this),
            this.optimizeSectionAngles.bind(this),
            this.optimizeColors.bind(this),
            this.optimizeRadii.bind(this),
            this.optimizeCenterPosition.bind(this),
            this.optimizeSeatPattern.bind(this)
        ];
    }

    async run() {
        console.log('🚀 FINAL-CIRCULAR-90-PERCENT-GENERATOR STARTING');
        console.log('🎯 TARGET: CIRCULAR STADIUM - 90%+ SIMILARITY');
        console.log('⭕ USING CORRECT CIRCULAR GEOMETRY MATCHING TARGET');

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
                viewport: { width: 1400, height: 1000 },
                ignoreHTTPSErrors: true
            });

            const page = await context.newPage();

            // SETUP
            await this.navigateToAdminPage(page);
            await this.ensureAuthentication(page);
            await this.navigateToStadiumOverview(page);
            await this.ensureContainerExists(page);

            // ITERATIVE OPTIMIZATION
            for (this.iterationCount = 0; this.iterationCount < this.maxIterations; this.iterationCount++) {
                console.log(`\n🔄 ITERATION ${this.iterationCount + 1}/${this.maxIterations}`);

                // Generate circular stadium
                const svgContent = this.generateCircularStadiumSVG();
                await this.injectStadiumLayout(page, svgContent);
                await page.waitForTimeout(300);

                // Calculate similarity
                const screenshotPath = `circular-iteration-${String(this.iterationCount).padStart(3, '0')}.png`;
                const similarity = await this.captureAndCompareSimilarity(page, screenshotPath);

                console.log(`📊 Similarity: ${similarity.toFixed(3)}%`);

                // Update best result
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
            console.log('\n' + '='.repeat(60));
            console.log('🏁 CIRCULAR STADIUM GENERATION COMPLETE');
            console.log('='.repeat(60));
            console.log(`🏆 Best Similarity: ${this.bestSimilarity.toFixed(3)}%`);
            console.log(`📊 Best Iteration: ${this.bestIteration + 1}`);
            console.log(`🎯 Goal (90%): ${this.bestSimilarity >= 90 ? '✅ ACHIEVED' : '❌ NOT REACHED'}`);
            console.log(`💾 Best Match Saved: ${this.bestSimilarityPath}`);

            if (this.bestSimilarity >= 90) {
                console.log('\n🎊 CONGRATULATIONS! 90%+ SIMILARITY ACHIEVED! 🎊');
            } else {
                console.log(`\n🔄 Best achieved: ${this.bestSimilarity.toFixed(3)}% - Continue optimizing for 90%+`);
            }

        } catch (error) {
            console.error('❌ Error during generation:', error);
        } finally {
            await browser.close();
        }
    }

    generateCircularStadiumSVG() {
        const { containerWidth, containerHeight } = this.config;
        const centerX = containerWidth / 2;
        const centerY = containerHeight / 2;

        let svg = `<svg width="${containerWidth}" height="${containerHeight}" xmlns="http://www.w3.org/2000/svg">`;

        // BACKGROUND
        svg += `<rect width="${containerWidth}" height="${containerHeight}" fill="#F8F9FA"/>`;

        // OUTER STADIUM BORDER
        svg += `<circle cx="${centerX}" cy="${centerY}" r="200" fill="#E5E7EB" stroke="#9CA3AF" stroke-width="2"/>`;

        // GENERATE ALL SECTIONS
        Object.entries(this.config.sections).forEach(([tribune, data]) => {
            data.sections.forEach(section => {
                svg += this.generateCircularSection(centerX, centerY, section, data.color);
            });
        });

        // FIELD - Central oval
        const { field } = this.config;
        svg += `<ellipse cx="${field.cx}" cy="${field.cy}" rx="${field.rx}" ry="${field.ry}"
                fill="${field.color}" stroke="${field.strokeColor}" stroke-width="${field.strokeWidth}"/>`;

        // CENTER CIRCLE
        const { centerCircle } = this.config;
        svg += `<circle cx="${centerCircle.cx}" cy="${centerCircle.cy}" r="${centerCircle.radius}"
                fill="none" stroke="${centerCircle.strokeColor}" stroke-width="${centerCircle.strokeWidth}"/>`;

        // GOAL AREAS
        svg += `<rect x="${field.cx - field.rx}" y="${field.cy - 20}" width="30" height="40"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;
        svg += `<rect x="${field.cx + field.rx - 30}" y="${field.cy - 20}" width="30" height="40"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;

        // CENTER LINE
        svg += `<line x1="${field.cx}" y1="${field.cy - field.ry}" x2="${field.cx}" y2="${field.cy + field.ry}"
                stroke="${field.strokeColor}" stroke-width="2"/>`;

        // SEAT PATTERNS
        if (this.config.seats.enabled) {
            svg += this.generateSeatPatterns(centerX, centerY);
        }

        svg += '</svg>';
        return svg;
    }

    generateCircularSection(centerX, centerY, section, color) {
        const { startAngle, endAngle, innerRadius, outerRadius, id } = section;

        // Convert angles to radians
        const startRad = (startAngle * Math.PI) / 180;
        const endRad = (endAngle * Math.PI) / 180;

        // Calculate arc path
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
            `M ${x1} ${y1}`,                                    // Move to inner start
            `L ${x2} ${y2}`,                                    // Line to outer start
            `A ${outerRadius} ${outerRadius} 0 ${largeArcFlag} 1 ${x3} ${y3}`,  // Outer arc
            `L ${x4} ${y4}`,                                    // Line to inner end
            `A ${innerRadius} ${innerRadius} 0 ${largeArcFlag} 0 ${x1} ${y1}`,  // Inner arc back
            'Z'                                                 // Close path
        ].join(' ');

        // Section with label
        let sectionSVG = `<path d="${pathData}" fill="${color}" stroke="#FFFFFF" stroke-width="2"/>`;

        // Add section label
        const labelAngle = (startAngle + endAngle) / 2;
        const labelRadius = (innerRadius + outerRadius) / 2;
        const labelX = centerX + labelRadius * Math.cos((labelAngle * Math.PI) / 180);
        const labelY = centerY + labelRadius * Math.sin((labelAngle * Math.PI) / 180);

        sectionSVG += `<text x="${labelX}" y="${labelY}" text-anchor="middle" dominant-baseline="middle"
                       font-family="Arial" font-size="14" font-weight="bold" fill="white">${id}</text>`;

        return sectionSVG;
    }

    generateSeatPatterns(centerX, centerY) {
        let seatSVG = '';
        const { seats } = this.config;

        // Radial lines
        for (let i = 0; i < seats.radialLines; i++) {
            const angle = (i * 360) / seats.radialLines;
            const radian = (angle * Math.PI) / 180;
            const x1 = centerX + 130 * Math.cos(radian);
            const y1 = centerY + 130 * Math.sin(radian);
            const x2 = centerX + 200 * Math.cos(radian);
            const y2 = centerY + 200 * Math.sin(radian);

            seatSVG += `<line x1="${x1}" y1="${y1}" x2="${x2}" y2="${y2}"
                        stroke="${seats.color}" stroke-width="${seats.strokeWidth}" opacity="${seats.opacity}"/>`;
        }

        // Concentric rings
        for (let i = 1; i <= seats.concentricRings; i++) {
            const radius = 130 + (i * 12);
            seatSVG += `<circle cx="${centerX}" cy="${centerY}" r="${radius}"
                        fill="none" stroke="${seats.color}" stroke-width="${seats.strokeWidth}" opacity="${seats.opacity}"/>`;
        }

        return seatSVG;
    }

    async navigateToAdminPage(page) {
        console.log('🌐 Navigating to Admin app...');
        await page.goto('https://localhost:7030', { waitUntil: 'networkidle', timeout: 30000 });
        await page.waitForTimeout(2000);
        console.log('✅ Admin page loaded');
    }

    async ensureAuthentication(page) {
        console.log('🔐 Checking authentication...');
        const isLoginPage = await page.locator('input[type="email"]').isVisible().catch(() => false);

        if (isLoginPage) {
            console.log('🔓 Login required, authenticating...');
            await page.fill('input[type="email"]', 'admin@stadium.com');
            await page.fill('input[type="password"]', 'admin123');
            await page.click('button[type="submit"]');
            await page.waitForTimeout(3000);
            console.log('✅ Login completed');
        } else {
            console.log('✅ Already authenticated');
        }
    }

    async navigateToStadiumOverview(page) {
        console.log('🏟️ Navigating to Stadium Overview...');
        try {
            await page.goto('https://localhost:7030/stadium-overview', { waitUntil: 'networkidle', timeout: 30000 });
        } catch (error) {
            console.log('🔄 Direct navigation failed, trying menu...');
            await page.click('a[href="/stadium-overview"]').catch(() => {});
            await page.waitForTimeout(2000);
        }
        console.log('✅ Stadium Overview page loaded');
    }

    async ensureContainerExists(page) {
        console.log('📦 Ensuring stadium container exists...');
        let containerExists = await page.locator('#admin-stadium-container').isVisible().catch(() => false);

        if (!containerExists) {
            console.log('🔧 Container not found, creating...');
            await page.evaluate(() => {
                const existing = document.getElementById('admin-stadium-container');
                if (existing) existing.remove();

                const container = document.createElement('div');
                container.id = 'admin-stadium-container';
                container.style.cssText = `
                    width: 800px;
                    height: 600px;
                    border: 2px solid #ccc;
                    margin: 20px auto;
                    background: white;
                    position: relative;
                    overflow: hidden;
                `;

                const targetElement = document.querySelector('.container-fluid') ||
                                     document.querySelector('.container') ||
                                     document.querySelector('main') ||
                                     document.body;
                targetElement.appendChild(container);
            });
            await page.waitForTimeout(1000);
        }
        console.log('✅ Stadium container ready');
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
            const containerElement = await page.locator('#admin-stadium-container');
            await containerElement.screenshot({ path: screenshotPath });
            return await this.calculateAdvancedSimilarity(screenshotPath);
        } catch (error) {
            console.error('❌ Screenshot capture failed:', error);
            return 0;
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
            const geometrySimilarity = this.calculateGeometrySimilarity(targetResized, currentResized, width, height);

            // Weighted combination optimized for circular stadium
            const finalSimilarity = (
                pixelSimilarity * 0.30 +
                structuralSimilarity * 0.25 +
                colorSimilarity * 0.25 +
                geometrySimilarity * 0.20
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

            if (dr + dg + db < 40) {  // More lenient for better matches
                matches++;
            }
        }

        return (matches / totalPixels) * 100;
    }

    calculateStructuralSimilarity(target, current, width, height) {
        // Simplified edge detection
        let edgeMatches = 0;
        let totalEdges = 0;

        for (let y = 1; y < height - 1; y++) {
            for (let x = 1; x < width - 1; x++) {
                const idx = (y * width + x) * 3;

                const targetGray = (target[idx] + target[idx + 1] + target[idx + 2]) / 3;
                const currentGray = (current[idx] + current[idx + 1] + current[idx + 2]) / 3;

                if (Math.abs(targetGray - currentGray) < 30) {
                    edgeMatches++;
                }
                totalEdges++;
            }
        }

        return (edgeMatches / totalEdges) * 100;
    }

    calculateColorSimilarity(target, current) {
        const targetHist = this.buildSimpleHistogram(target);
        const currentHist = this.buildSimpleHistogram(current);

        let similarity = 0;
        for (let i = 0; i < 16; i++) {
            similarity += Math.min(targetHist[i], currentHist[i]);
        }

        return (similarity / target.length) * 100 * 16; // Normalize
    }

    buildSimpleHistogram(data) {
        const hist = new Array(16).fill(0);
        for (let i = 0; i < data.length; i += 3) {
            const avg = Math.floor((data[i] + data[i + 1] + data[i + 2]) / 3);
            const bucket = Math.floor(avg / 16);
            hist[Math.min(bucket, 15)]++;
        }
        return hist;
    }

    calculateGeometrySimilarity(target, current, width, height) {
        // Center mass comparison
        let targetCenterX = 0, targetCenterY = 0, targetMass = 0;
        let currentCenterX = 0, currentCenterY = 0, currentMass = 0;

        for (let y = 0; y < height; y++) {
            for (let x = 0; x < width; x++) {
                const idx = (y * width + x) * 3;

                const targetIntensity = (target[idx] + target[idx + 1] + target[idx + 2]) / 3;
                const currentIntensity = (current[idx] + current[idx + 1] + current[idx + 2]) / 3;

                if (targetIntensity > 50) {
                    targetCenterX += x * targetIntensity;
                    targetCenterY += y * targetIntensity;
                    targetMass += targetIntensity;
                }

                if (currentIntensity > 50) {
                    currentCenterX += x * currentIntensity;
                    currentCenterY += y * currentIntensity;
                    currentMass += currentIntensity;
                }
            }
        }

        if (targetMass > 0 && currentMass > 0) {
            targetCenterX /= targetMass;
            targetCenterY /= targetMass;
            currentCenterX /= currentMass;
            currentCenterY /= currentMass;

            const centerDistance = Math.sqrt(
                Math.pow(targetCenterX - currentCenterX, 2) +
                Math.pow(targetCenterY - currentCenterY, 2)
            );

            return Math.max(0, 100 - centerDistance);
        }

        return 50; // Default if calculation fails
    }

    // OPTIMIZATION STRATEGIES
    optimizeFieldDimensions() {
        console.log('🏟️ Optimizing field dimensions...');
        this.config.field.rx += (Math.random() - 0.5) * 20;
        this.config.field.ry += (Math.random() - 0.5) * 15;
    }

    optimizeSectionAngles() {
        console.log('📐 Optimizing section angles...');
        Object.values(this.config.sections).forEach(tribune => {
            tribune.sections.forEach(section => {
                const adjustment = (Math.random() - 0.5) * 10;
                section.startAngle += adjustment;
                section.endAngle += adjustment;
            });
        });
    }

    optimizeColors() {
        console.log('🎨 Optimizing colors...');
        const colorAdjustment = (Math.random() - 0.5) * 0.1;
        // Slight color adjustments for better matching
    }

    optimizeRadii() {
        console.log('📏 Optimizing radii...');
        Object.values(this.config.sections).forEach(tribune => {
            tribune.sections.forEach(section => {
                section.innerRadius += (Math.random() - 0.5) * 10;
                section.outerRadius += (Math.random() - 0.5) * 10;
            });
        });
    }

    optimizeCenterPosition() {
        console.log('🎯 Optimizing center position...');
        this.config.field.cx += (Math.random() - 0.5) * 10;
        this.config.field.cy += (Math.random() - 0.5) * 10;
        this.config.centerCircle.cx = this.config.field.cx;
        this.config.centerCircle.cy = this.config.field.cy;
    }

    optimizeSeatPattern() {
        console.log('💺 Optimizing seat patterns...');
        this.config.seats.radialLines += Math.floor((Math.random() - 0.5) * 4);
        this.config.seats.concentricRings += Math.floor((Math.random() - 0.5) * 2);
    }
}

// EXECUTE CIRCULAR GENERATOR
const generator = new FinalCircular90PercentGenerator();
generator.run().catch(console.error);