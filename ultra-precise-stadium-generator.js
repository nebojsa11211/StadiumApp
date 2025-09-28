const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
const sharp = require('sharp');

class UltraPreciseStadiumGenerator {
    constructor() {
        this.browser = null;
        this.page = null;
        this.targetImagePath = 'target-stadium.png';
        this.currentIteration = 0;
        this.maxIterations = 200; // More iterations for precision
        this.bestSimilarity = 0;
        this.bestIteration = 0;
        this.targetThreshold = 0.90; // 90% target

        // Ultra-precise configuration based on target analysis
        this.config = {
            // Stadium dimensions - matching target proportions exactly
            stadiumWidth: 800,
            stadiumHeight: 560,

            // Field dimensions and position
            fieldWidth: 420,
            fieldHeight: 280,
            fieldOffsetX: 190,
            fieldOffsetY: 140,

            // Stand geometry - CURVED to match target
            stands: {
                north: {
                    sections: ['A1', 'A2', 'A3', 'VIP', 'SKYBOX', 'A4', 'A5', 'A6'],
                    colors: ['#8B2C8A', '#8B2C8A', '#8B2C8A', '#DAA520', '#C0C0C0', '#8B2C8A', '#8B2C8A', '#8B2C8A'],
                    startAngle: -15,
                    endAngle: 195,
                    innerRadius: 165,
                    outerRadius: 215,
                    rowSpacing: 3
                },
                south: {
                    sections: ['C6', 'C5', 'C4', 'C3', 'C2', 'C1'],
                    colors: ['#1E40AF', '#1E40AF', '#1E40AF', '#1E40AF', '#1E40AF', '#1E40AF'],
                    startAngle: 165,
                    endAngle: 375,
                    innerRadius: 165,
                    outerRadius: 215,
                    rowSpacing: 3
                },
                east: {
                    sections: ['B1', 'B2', 'B3', 'B4'],
                    colors: ['#DC2626', '#DC2626', '#DC2626', '#DC2626'],
                    startAngle: 45,
                    endAngle: 135,
                    innerRadius: 165,
                    outerRadius: 260,
                    rowSpacing: 3
                },
                west: {
                    sections: ['D6', 'D5', 'D4', 'D3', 'D2', 'D1'],
                    colors: ['#F59E0B', '#F59E0B', '#F59E0B', '#F59E0B', '#F59E0B', '#F59E0B'],
                    startAngle: 225,
                    endAngle: 315,
                    innerRadius: 165,
                    outerRadius: 260,
                    rowSpacing: 3
                }
            },

            // Fine-tuning parameters
            seatSize: 1.5,
            rowHeight: 4,
            sectionGap: 2,
            cornerRadius: 15
        };
    }

    async initialize() {
        console.log('🎯 Starting ULTRA-PRECISE Stadium Generator (Target: 90%+)...');

        this.browser = await chromium.launch({
            headless: false,
            slowMo: 50,
            args: ['--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
        });
        this.page = await this.browser.newPage();
        await this.page.setViewportSize({ width: 1400, height: 1000 });

        await this.page.goto('https://localhost:7030/admin/stadium-overview', {
            waitUntil: 'domcontentloaded',
            timeout: 30000
        });

        try {
            await this.page.waitForSelector('#admin-stadium-container', { timeout: 5000 });
        } catch (error) {
            console.log('🔐 Need to login first...');
            await this.loginToAdmin();
            await this.page.goto('https://localhost:7030/admin/stadium-overview', {
                waitUntil: 'domcontentloaded',
                timeout: 30000
            });

            // Wait for the page to load and handle any loading states
            await this.page.waitForTimeout(5000);

            // Try to find the container or create it if not found
            const containerExists = await this.page.$('#admin-stadium-container');
            if (!containerExists) {
                console.log('📦 Container not found, creating it...');
                await this.page.evaluate(() => {
                    const container = document.createElement('div');
                    container.id = 'admin-stadium-container';
                    container.style.width = '100%';
                    container.style.height = '600px';
                    container.style.display = 'flex';
                    container.style.justifyContent = 'center';
                    container.style.alignItems = 'center';
                    document.body.appendChild(container);
                });
            }
        }

        console.log('✅ Connected - preparing ultra-precise stadium generation');
    }

    async loginToAdmin() {
        try {
            await this.page.goto('https://localhost:7030/login', {
                waitUntil: 'domcontentloaded',
                timeout: 30000
            });

            // Wait for login form to appear
            await this.page.waitForSelector('#admin-login-email-input', { timeout: 10000 });

            console.log('🔐 Filling login credentials...');
            await this.page.fill('#admin-login-email-input', 'admin@stadium.com');
            await this.page.fill('#admin-login-password-input', 'admin123');

            console.log('🔐 Submitting login...');
            await this.page.click('#admin-login-submit-btn');

            // Wait for navigation or success
            await this.page.waitForTimeout(5000);

            console.log('✅ Login process completed');
        } catch (error) {
            console.error('❌ Login failed:', error.message);
            throw error;
        }
    }

    generateUltraPreciseStadiumSVG() {
        const { stadiumWidth, stadiumHeight, fieldWidth, fieldHeight, fieldOffsetX, fieldOffsetY, stands } = this.config;
        const centerX = stadiumWidth / 2;
        const centerY = stadiumHeight / 2;

        let svg = `
            <svg width="${stadiumWidth}" height="${stadiumHeight}" viewBox="0 0 ${stadiumWidth} ${stadiumHeight}"
                 style="background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%); border: 2px solid #dee2e6; border-radius: 20px;">

                <!-- Ultra-precise Football Field with exact proportions -->
                <rect x="${fieldOffsetX}" y="${fieldOffsetY}"
                      width="${fieldWidth}" height="${fieldHeight}"
                      fill="#22C55E" stroke="#ffffff" stroke-width="4" rx="15"/>

                <!-- Field markings - precise positioning -->
                <circle cx="${centerX}" cy="${centerY}" r="45"
                        fill="none" stroke="#ffffff" stroke-width="3"/>
                <circle cx="${centerX}" cy="${centerY}" r="3"
                        fill="#ffffff"/>

                <!-- Center Line -->
                <line x1="${centerX}" y1="${fieldOffsetY}"
                      x2="${centerX}" y2="${fieldOffsetY + fieldHeight}"
                      stroke="#ffffff" stroke-width="3"/>

                <!-- Goal Areas - precise dimensions -->
                <rect x="${fieldOffsetX}" y="${centerY - 35}"
                      width="35" height="70"
                      fill="none" stroke="#ffffff" stroke-width="3"/>
                <rect x="${fieldOffsetX + fieldWidth - 35}" y="${centerY - 35}"
                      width="35" height="70"
                      fill="none" stroke="#ffffff" stroke-width="3"/>

                <!-- Penalty Areas -->
                <rect x="${fieldOffsetX}" y="${centerY - 65}"
                      width="85" height="130"
                      fill="none" stroke="#ffffff" stroke-width="2"/>
                <rect x="${fieldOffsetX + fieldWidth - 85}" y="${centerY - 65}"
                      width="85" height="130"
                      fill="none" stroke="#ffffff" stroke-width="2"/>

                <!-- Field Text Labels -->
                <text x="${fieldOffsetX + 50}" y="${centerY}"
                      font-family="Arial, sans-serif" font-size="14" font-weight="bold"
                      fill="white" transform="rotate(-90, ${fieldOffsetX + 50}, ${centerY})"
                      text-anchor="middle">Z A P A D / W E S T</text>

                <text x="${fieldOffsetX + fieldWidth - 50}" y="${centerY}"
                      font-family="Arial, sans-serif" font-size="14" font-weight="bold"
                      fill="white" transform="rotate(90, ${fieldOffsetX + fieldWidth - 50}, ${centerY})"
                      text-anchor="middle">S E V E R / N O R T H</text>

                <text x="${centerX}" y="${fieldOffsetY + fieldHeight - 20}"
                      font-family="Arial, sans-serif" font-size="14" font-weight="bold"
                      fill="white" text-anchor="middle">I S T O K / E A S T</text>
        `;

        // Generate curved stands with precise positioning
        svg += this.generateCurvedStand('north', centerX, centerY);
        svg += this.generateCurvedStand('south', centerX, centerY);
        svg += this.generateCurvedStand('east', centerX, centerY);
        svg += this.generateCurvedStand('west', centerX, centerY);

        svg += '</svg>';
        return svg;
    }

    generateCurvedStand(standName, centerX, centerY) {
        const stand = this.config.stands[standName];
        const { sections, colors, startAngle, endAngle, innerRadius, outerRadius, rowSpacing } = stand;

        let svg = '';
        const angleRange = endAngle - startAngle;
        const sectionAngle = angleRange / sections.length;

        sections.forEach((sectionName, index) => {
            const sectionStartAngle = startAngle + (index * sectionAngle);
            const sectionEndAngle = startAngle + ((index + 1) * sectionAngle);
            const color = colors[index] || colors[0];

            // Create curved section path
            const path = this.createCurvedSectionPath(
                centerX, centerY,
                sectionStartAngle, sectionEndAngle,
                innerRadius, outerRadius
            );

            svg += `
                <g class="stadium-section" data-section="${sectionName}">
                    <path d="${path}"
                          fill="${color}"
                          stroke="#ffffff"
                          stroke-width="2"
                          opacity="0.9"/>

                    <!-- Section Label -->
                    <text x="${this.getSectionLabelX(centerX, centerY, sectionStartAngle, sectionEndAngle, (innerRadius + outerRadius) / 2)}"
                          y="${this.getSectionLabelY(centerX, centerY, sectionStartAngle, sectionEndAngle, (innerRadius + outerRadius) / 2)}"
                          font-family="Arial, sans-serif" font-size="16" font-weight="bold"
                          fill="white" text-anchor="middle" dominant-baseline="middle">
                        ${sectionName}
                    </text>

                    <!-- Concentric seat rows -->
                    ${this.generateConcentricSeats(centerX, centerY, sectionStartAngle, sectionEndAngle, innerRadius, outerRadius, rowSpacing)}
                </g>
            `;
        });

        return svg;
    }

    createCurvedSectionPath(centerX, centerY, startAngle, endAngle, innerRadius, outerRadius) {
        const startAngleRad = (startAngle * Math.PI) / 180;
        const endAngleRad = (endAngle * Math.PI) / 180;

        const x1 = centerX + innerRadius * Math.cos(startAngleRad);
        const y1 = centerY + innerRadius * Math.sin(startAngleRad);
        const x2 = centerX + outerRadius * Math.cos(startAngleRad);
        const y2 = centerY + outerRadius * Math.sin(startAngleRad);
        const x3 = centerX + outerRadius * Math.cos(endAngleRad);
        const y3 = centerY + outerRadius * Math.sin(endAngleRad);
        const x4 = centerX + innerRadius * Math.cos(endAngleRad);
        const y4 = centerY + innerRadius * Math.sin(endAngleRad);

        const largeArcFlag = endAngle - startAngle > 180 ? 1 : 0;

        return `M ${x1} ${y1}
                L ${x2} ${y2}
                A ${outerRadius} ${outerRadius} 0 ${largeArcFlag} 1 ${x3} ${y3}
                L ${x4} ${y4}
                A ${innerRadius} ${innerRadius} 0 ${largeArcFlag} 0 ${x1} ${y1}
                Z`;
    }

    generateConcentricSeats(centerX, centerY, startAngle, endAngle, innerRadius, outerRadius, rowSpacing) {
        let seats = '';
        const numRows = Math.floor((outerRadius - innerRadius) / rowSpacing);

        for (let row = 0; row < numRows; row++) {
            const radius = innerRadius + (row * rowSpacing) + (rowSpacing / 2);
            const circumference = 2 * Math.PI * radius;
            const angleRange = endAngle - startAngle;
            const arcLength = (angleRange / 360) * circumference;
            const seatsInRow = Math.floor(arcLength / 4); // 4px per seat

            for (let seat = 0; seat < seatsInRow; seat++) {
                const seatAngle = startAngle + (seat / seatsInRow) * angleRange;
                const seatAngleRad = (seatAngle * Math.PI) / 180;
                const seatX = centerX + radius * Math.cos(seatAngleRad);
                const seatY = centerY + radius * Math.sin(seatAngleRad);

                seats += `<circle cx="${seatX}" cy="${seatY}" r="1" fill="#2D3748" opacity="0.7"/>`;
            }
        }

        return seats;
    }

    getSectionLabelX(centerX, centerY, startAngle, endAngle, radius) {
        const midAngle = (startAngle + endAngle) / 2;
        const midAngleRad = (midAngle * Math.PI) / 180;
        return centerX + radius * Math.cos(midAngleRad);
    }

    getSectionLabelY(centerX, centerY, startAngle, endAngle, radius) {
        const midAngle = (startAngle + endAngle) / 2;
        const midAngleRad = (midAngle * Math.PI) / 180;
        return centerY + radius * Math.sin(midAngleRad);
    }

    async injectUltraPreciseStadium() {
        const svgContent = this.generateUltraPreciseStadiumSVG();

        await this.page.evaluate((svg) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svg;
                container.style.display = 'flex';
                container.style.justifyContent = 'center';
                container.style.alignItems = 'center';
                container.style.padding = '20px';
                container.style.backgroundColor = '#f8f9fa';
                container.style.minHeight = '600px';
            }
        }, svgContent);

        await this.page.waitForTimeout(200);
    }

    async takeScreenshot() {
        const screenshotPath = `ultra-iteration-${String(this.currentIteration).padStart(3, '0')}.png`;
        const containerElement = await this.page.$('#admin-stadium-container');
        if (containerElement) {
            await containerElement.screenshot({ path: screenshotPath });
            console.log(`📸 Ultra-precise screenshot saved: ${screenshotPath}`);
            return screenshotPath;
        }
        return null;
    }

    async calculateEnhancedSimilarity(screenshotPath) {
        try {
            // Enhanced similarity calculation with structural analysis
            const targetBuffer = await sharp(this.targetImagePath).png().toBuffer();
            const currentBuffer = await sharp(screenshotPath).png().toBuffer();

            // Resize both to same dimensions for precise comparison
            const targetMeta = await sharp(targetBuffer).metadata();
            const width = 800;
            const height = 560;

            const targetResized = await sharp(targetBuffer)
                .resize(width, height, { fit: 'fill' })
                .raw()
                .toBuffer();

            const currentResized = await sharp(currentBuffer)
                .resize(width, height, { fit: 'fill' })
                .raw()
                .toBuffer();

            // Multi-factor similarity calculation
            const pixelSimilarity = this.calculatePixelSimilarity(targetResized, currentResized);
            const structuralSimilarity = this.calculateStructuralSimilarity(targetResized, currentResized, width, height);
            const colorSimilarity = this.calculateColorSimilarity(targetResized, currentResized);

            // Weighted average for ultra-precise matching
            const overallSimilarity = (
                pixelSimilarity * 0.4 +
                structuralSimilarity * 0.4 +
                colorSimilarity * 0.2
            );

            return Math.max(0, Math.min(1, overallSimilarity));

        } catch (error) {
            console.error('Error calculating enhanced similarity:', error);
            return 0;
        }
    }

    calculatePixelSimilarity(target, current) {
        let totalDiff = 0;
        const totalPixels = Math.min(target.length, current.length);

        for (let i = 0; i < totalPixels; i += 3) { // RGB
            const rDiff = Math.abs(target[i] - current[i]);
            const gDiff = Math.abs(target[i + 1] - current[i + 1]);
            const bDiff = Math.abs(target[i + 2] - current[i + 2]);
            totalDiff += (rDiff + gDiff + bDiff) / 3;
        }

        return 1 - (totalDiff / (totalPixels / 3 * 255));
    }

    calculateStructuralSimilarity(target, current, width, height) {
        // Edge detection and shape matching
        let structuralScore = 0;
        const samplePoints = 1000;

        for (let i = 0; i < samplePoints; i++) {
            const x = Math.floor(Math.random() * width);
            const y = Math.floor(Math.random() * height);
            const index = (y * width + x) * 3;

            if (index < target.length && index < current.length) {
                const targetIntensity = (target[index] + target[index + 1] + target[index + 2]) / 3;
                const currentIntensity = (current[index] + current[index + 1] + current[index + 2]) / 3;

                const intensityDiff = Math.abs(targetIntensity - currentIntensity) / 255;
                structuralScore += 1 - intensityDiff;
            }
        }

        return structuralScore / samplePoints;
    }

    calculateColorSimilarity(target, current) {
        const targetColors = this.extractDominantColors(target);
        const currentColors = this.extractDominantColors(current);

        let colorMatchScore = 0;
        const expectedColors = ['purple', 'red', 'blue', 'yellow', 'green'];

        expectedColors.forEach(color => {
            if (targetColors.includes(color) && currentColors.includes(color)) {
                colorMatchScore += 0.2;
            }
        });

        return colorMatchScore;
    }

    extractDominantColors(buffer) {
        // Simplified color extraction
        const colors = [];
        for (let i = 0; i < buffer.length; i += 300) { // Sample every 100th pixel
            const r = buffer[i];
            const g = buffer[i + 1];
            const b = buffer[i + 2];

            if (r > 150 && g < 100 && b > 150) colors.push('purple');
            else if (r > 180 && g < 100 && b < 100) colors.push('red');
            else if (r < 100 && g < 100 && b > 150) colors.push('blue');
            else if (r > 200 && g > 150 && b < 100) colors.push('yellow');
            else if (r < 100 && g > 150 && b < 100) colors.push('green');
        }

        return [...new Set(colors)];
    }

    async applyUltraPreciseImprovement() {
        const strategies = [
            () => this.optimizeStandGeometry(),
            () => this.refineFieldProportions(),
            () => this.enhanceColorAccuracy(),
            () => this.improveSeatingDetail(),
            () => this.adjustAnglesAndCurves(),
            () => this.optimizeTextLabels(),
            () => this.finetuneOverallLayout(),
            () => this.maximizePrecision()
        ];

        const strategy = strategies[this.currentIteration % strategies.length];
        strategy();

        console.log(`🔧 Applied ultra-precise strategy ${this.currentIteration % strategies.length + 1}`);
    }

    optimizeStandGeometry() {
        // Adjust stand angles for perfect match
        this.config.stands.north.startAngle = -20 + (this.currentIteration * 0.5);
        this.config.stands.north.endAngle = 200 - (this.currentIteration * 0.3);
        this.config.stands.south.innerRadius = 160 + (this.currentIteration * 0.8);
    }

    refineFieldProportions() {
        // Fine-tune field dimensions
        this.config.fieldWidth = 420 + (this.currentIteration * 0.5);
        this.config.fieldHeight = 280 + (this.currentIteration * 0.3);
        this.config.fieldOffsetX = 190 - (this.currentIteration * 0.2);
    }

    enhanceColorAccuracy() {
        // Precise color matching
        this.config.stands.north.colors = this.config.stands.north.colors.map((color, i) => {
            if (color === '#8B2C8A') return `hsl(${300 + this.currentIteration * 0.5}, 70%, 45%)`;
            if (color === '#DAA520') return `hsl(${43 + this.currentIteration * 0.2}, 85%, 55%)`;
            return color;
        });
    }

    improveSeatingDetail() {
        // Enhance seat density and arrangement
        this.config.stands.north.rowSpacing = 3 + (this.currentIteration * 0.05);
        this.config.seatSize = 1.5 + (this.currentIteration * 0.02);
    }

    adjustAnglesAndCurves() {
        // Perfect the curved geometry
        Object.keys(this.config.stands).forEach(standName => {
            this.config.stands[standName].innerRadius += this.currentIteration * 0.1;
            this.config.stands[standName].outerRadius += this.currentIteration * 0.15;
        });
    }

    optimizeTextLabels() {
        // Ensure text positioning matches target
        this.config.fieldOffsetY = 140 + Math.sin(this.currentIteration * 0.1) * 2;
    }

    finetuneOverallLayout() {
        // Overall stadium dimensions optimization
        this.config.stadiumWidth = 800 + (this.currentIteration * 0.2);
        this.config.stadiumHeight = 560 + (this.currentIteration * 0.1);
    }

    maximizePrecision() {
        // Final precision adjustments
        if (this.currentIteration > 100) {
            // Ultra-fine adjustments in later iterations
            this.config.cornerRadius = 15 + Math.sin(this.currentIteration * 0.2) * 3;
        }
    }

    async runUltraPreciseIteration() {
        console.log(`\n🎯 Ultra-Precise Iteration ${this.currentIteration + 1}/${this.maxIterations}`);

        await this.injectUltraPreciseStadium();
        const screenshotPath = await this.takeScreenshot();

        if (!screenshotPath) {
            console.error('❌ Failed to take screenshot');
            return false;
        }

        const similarity = await this.calculateEnhancedSimilarity(screenshotPath);
        console.log(`📊 Enhanced Similarity: ${(similarity * 100).toFixed(3)}%`);

        if (similarity > this.bestSimilarity) {
            this.bestSimilarity = similarity;
            this.bestIteration = this.currentIteration;

            fs.copyFileSync(screenshotPath, 'ultra-best-match.png');
            console.log(`🎯 NEW ULTRA BEST! Similarity: ${(similarity * 100).toFixed(3)}%`);
        }

        if (similarity >= this.targetThreshold) {
            console.log(`🎉 TARGET ACHIEVED! 90%+ similarity reached: ${(similarity * 100).toFixed(3)}%`);
            return true;
        }

        await this.applyUltraPreciseImprovement();
        this.currentIteration++;
        return false;
    }

    async run() {
        try {
            await this.initialize();

            console.log(`🎯 ULTRA-PRECISE TARGET: ${(this.targetThreshold * 100)}%`);
            console.log(`🔄 Max iterations: ${this.maxIterations}`);

            let completed = false;
            while (this.currentIteration < this.maxIterations && !completed) {
                completed = await this.runUltraPreciseIteration();

                if (completed) break;

                if (this.currentIteration % 10 === 0) {
                    console.log(`📈 Progress: ${this.currentIteration}/${this.maxIterations} iterations, Best: ${(this.bestSimilarity * 100).toFixed(3)}%`);
                }

                await this.page.waitForTimeout(100);
            }

            await this.generateUltraFinalReport();

        } catch (error) {
            console.error('❌ Ultra-precise generation error:', error);
        } finally {
            if (this.browser) {
                await this.browser.close();
            }
        }
    }

    async generateUltraFinalReport() {
        const report = {
            mode: "ULTRA-PRECISE",
            targetThreshold: this.targetThreshold,
            totalIterations: this.currentIteration,
            bestSimilarity: this.bestSimilarity,
            bestIteration: this.bestIteration,
            achieved90Percent: this.bestSimilarity >= 0.90,
            finalConfig: this.config,
            timestamp: new Date().toISOString()
        };

        fs.writeFileSync('ultra-precise-report.json', JSON.stringify(report, null, 2));

        console.log('\n🎯 ULTRA-PRECISE FINAL REPORT');
        console.log('================================');
        console.log(`🔄 Total iterations: ${report.totalIterations}`);
        console.log(`🎯 Best similarity: ${(report.bestSimilarity * 100).toFixed(3)}%`);
        console.log(`📍 Best iteration: ${report.bestIteration + 1}`);
        console.log(`✅ 90%+ achieved: ${report.achieved90Percent ? 'YES! 🎉' : 'NO - continuing optimization'}`);
        console.log(`📁 Ultra-best match: ultra-best-match.png`);

        if (report.achieved90Percent) {
            console.log('\n🏆 MISSION ACCOMPLISHED: 90%+ SIMILARITY ACHIEVED!');
        } else {
            console.log(`\n⚡ BEST ATTEMPT: ${(report.bestSimilarity * 100).toFixed(3)}% - Ultra-optimized stadium saved`);
        }
    }
}

// Run the ultra-precise generator
async function main() {
    const generator = new UltraPreciseStadiumGenerator();
    await generator.run();
}

if (require.main === module) {
    main().catch(console.error);
}

module.exports = UltraPreciseStadiumGenerator;