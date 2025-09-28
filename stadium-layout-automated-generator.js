const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
const sharp = require('sharp');

class StadiumLayoutGenerator {
    constructor() {
        this.browser = null;
        this.page = null;
        this.targetImagePath = 'target-stadium.png';
        this.currentIteration = 0;
        this.maxIterations = 100;
        this.bestSimilarity = 0;
        this.bestIteration = 0;
        this.targetThreshold = 0.95;

        // Stadium configuration that will be iteratively improved
        this.config = {
            fieldWidth: 400,
            fieldHeight: 280,
            cornerRadius: 40,
            sectorWidth: 80,
            sectorHeight: 60,
            seatSize: 2,
            seatsPerRow: 20,
            rowsPerSector: 5,
            sections: {
                north: { count: 6, color: '#8B5CF6', startAngle: 0, endAngle: 180 },
                east: { count: 4, color: '#EF4444', startAngle: 270, endAngle: 360 },
                south: { count: 6, color: '#3B82F6', startAngle: 180, endAngle: 270 },
                west: { count: 6, color: '#FBBF24', startAngle: 90, endAngle: 180 }
            }
        };
    }

    async initialize() {
        console.log('🏟️ Starting Stadium Layout Automated Generator...');

        // Launch browser
        this.browser = await chromium.launch({
            headless: false, // Set to true for faster execution
            slowMo: 100,
            args: ['--ignore-certificate-errors', '--ignore-ssl-errors', '--allow-running-insecure-content']
        });
        this.page = await this.browser.newPage();
        await this.page.setViewportSize({ width: 1200, height: 900 });

        // Ignore certificate errors
        await this.page.context().setExtraHTTPHeaders({
            'Accept': '*/*'
        });

        // Navigate to admin page - using localhost:7030 based on admin app logs
        await this.page.goto('https://localhost:7030/admin/stadium-overview', {
            waitUntil: 'domcontentloaded',
            timeout: 30000
        });

        // Wait for page load and try to login if needed
        try {
            await this.page.waitForSelector('#admin-stadium-container', { timeout: 5000 });
        } catch (error) {
            console.log('🔐 Need to login first...');
            await this.loginToAdmin();
            await this.page.goto('https://localhost:7030/admin/stadium-overview', {
                waitUntil: 'domcontentloaded',
                timeout: 30000
            });
            await this.page.waitForSelector('#admin-stadium-container', { timeout: 10000 });
        }

        console.log('✅ Successfully connected to admin page');
    }

    async loginToAdmin() {
        await this.page.goto('https://localhost:7030/login', {
            waitUntil: 'domcontentloaded',
            timeout: 30000
        });

        // Wait for login form to be visible
        await this.page.waitForSelector('#admin-login-email-input', { timeout: 5000 });

        await this.page.fill('#admin-login-email-input', 'admin@stadium.com');
        await this.page.fill('#admin-login-password-input', 'admin123');
        await this.page.click('#admin-login-submit-btn');

        // Wait for login to complete and redirect (may go to home page first)
        await this.page.waitForTimeout(3000);

        console.log('✅ Login completed, current URL:', this.page.url());
    }

    generateStadiumSVG() {
        const { fieldWidth, fieldHeight, cornerRadius, sections } = this.config;
        const totalWidth = fieldWidth + 200;
        const totalHeight = fieldHeight + 200;
        const centerX = totalWidth / 2;
        const centerY = totalHeight / 2;

        let svg = `
            <svg width="${totalWidth}" height="${totalHeight}" viewBox="0 0 ${totalWidth} ${totalHeight}"
                 style="background: #f0f0f0; border-radius: 10px;">

                <!-- Football Field -->
                <rect x="${centerX - fieldWidth/2}" y="${centerY - fieldHeight/2}"
                      width="${fieldWidth}" height="${fieldHeight}"
                      fill="#22C55E" stroke="#fff" stroke-width="3" rx="10"/>

                <!-- Center Circle -->
                <circle cx="${centerX}" cy="${centerY}" r="40"
                        fill="none" stroke="#fff" stroke-width="2"/>

                <!-- Center Line -->
                <line x1="${centerX}" y1="${centerY - fieldHeight/2}"
                      x2="${centerX}" y2="${centerY + fieldHeight/2}"
                      stroke="#fff" stroke-width="2"/>

                <!-- Goal Areas -->
                <rect x="${centerX - fieldWidth/2}" y="${centerY - 30}"
                      width="30" height="60"
                      fill="none" stroke="#fff" stroke-width="2"/>
                <rect x="${centerX + fieldWidth/2 - 30}" y="${centerY - 30}"
                      width="30" height="60"
                      fill="none" stroke="#fff" stroke-width="2"/>
        `;

        // Generate sections for each side
        svg += this.generateNorthSections(centerX, centerY, fieldWidth, fieldHeight);
        svg += this.generateSouthSections(centerX, centerY, fieldWidth, fieldHeight);
        svg += this.generateEastSections(centerX, centerY, fieldWidth, fieldHeight);
        svg += this.generateWestSections(centerX, centerY, fieldWidth, fieldHeight);

        svg += '</svg>';
        return svg;
    }

    generateNorthSections(centerX, centerY, fieldWidth, fieldHeight) {
        const sections = ['A1', 'A2', 'A3', 'VIP', 'A4', 'A5', 'A6'];
        const sectionWidth = fieldWidth / sections.length;
        const yPos = centerY - fieldHeight/2 - 80;
        let svg = '';

        sections.forEach((section, index) => {
            const xPos = centerX - fieldWidth/2 + (index * sectionWidth);
            const color = section === 'VIP' ? '#D4AF37' : '#8B5CF6';

            svg += `
                <g class="stadium-section" data-section="${section}">
                    <rect x="${xPos}" y="${yPos}" width="${sectionWidth-2}" height="70"
                          fill="${color}" stroke="#fff" stroke-width="1" opacity="0.8"/>
                    <text x="${xPos + sectionWidth/2}" y="${yPos + 35}"
                          text-anchor="middle" fill="white" font-size="12" font-weight="bold">${section}</text>
                    ${this.generateSeatsInSection(xPos, yPos, sectionWidth-2, 70)}
                </g>
            `;
        });

        return svg;
    }

    generateSouthSections(centerX, centerY, fieldWidth, fieldHeight) {
        const sections = ['C6', 'C5', 'C4', 'C3', 'C2', 'C1'];
        const sectionWidth = fieldWidth / sections.length;
        const yPos = centerY + fieldHeight/2 + 10;
        let svg = '';

        sections.forEach((section, index) => {
            const xPos = centerX - fieldWidth/2 + (index * sectionWidth);

            svg += `
                <g class="stadium-section" data-section="${section}">
                    <rect x="${xPos}" y="${yPos}" width="${sectionWidth-2}" height="70"
                          fill="#3B82F6" stroke="#fff" stroke-width="1" opacity="0.8"/>
                    <text x="${xPos + sectionWidth/2}" y="${yPos + 35}"
                          text-anchor="middle" fill="white" font-size="12" font-weight="bold">${section}</text>
                    ${this.generateSeatsInSection(xPos, yPos, sectionWidth-2, 70)}
                </g>
            `;
        });

        return svg;
    }

    generateEastSections(centerX, centerY, fieldWidth, fieldHeight) {
        const sections = ['B1', 'B2', 'B3', 'B4'];
        const sectionHeight = fieldHeight / sections.length;
        const xPos = centerX + fieldWidth/2 + 10;
        let svg = '';

        sections.forEach((section, index) => {
            const yPos = centerY - fieldHeight/2 + (index * sectionHeight);

            svg += `
                <g class="stadium-section" data-section="${section}">
                    <rect x="${xPos}" y="${yPos}" width="80" height="${sectionHeight-2}"
                          fill="#EF4444" stroke="#fff" stroke-width="1" opacity="0.8"/>
                    <text x="${xPos + 40}" y="${yPos + sectionHeight/2}"
                          text-anchor="middle" fill="white" font-size="12" font-weight="bold">${section}</text>
                    ${this.generateSeatsInSection(xPos, yPos, 80, sectionHeight-2)}
                </g>
            `;
        });

        return svg;
    }

    generateWestSections(centerX, centerY, fieldWidth, fieldHeight) {
        const sections = ['D1', 'D2', 'D3', 'D4', 'D5', 'D6'];
        const sectionHeight = fieldHeight / sections.length;
        const xPos = centerX - fieldWidth/2 - 90;
        let svg = '';

        sections.forEach((section, index) => {
            const yPos = centerY - fieldHeight/2 + (index * sectionHeight);

            svg += `
                <g class="stadium-section" data-section="${section}">
                    <rect x="${xPos}" y="${yPos}" width="80" height="${sectionHeight-2}"
                          fill="#FBBF24" stroke="#fff" stroke-width="1" opacity="0.8"/>
                    <text x="${xPos + 40}" y="${yPos + sectionHeight/2}"
                          text-anchor="middle" fill="white" font-size="12" font-weight="bold">${section}</text>
                    ${this.generateSeatsInSection(xPos, yPos, 80, sectionHeight-2)}
                </g>
            `;
        });

        return svg;
    }

    generateSeatsInSection(x, y, width, height) {
        const seatsPerRow = Math.floor(width / 4);
        const rows = Math.floor(height / 6);
        let seats = '';

        for (let row = 0; row < rows; row++) {
            for (let seat = 0; seat < seatsPerRow; seat++) {
                const seatX = x + 2 + (seat * (width / seatsPerRow));
                const seatY = y + 2 + (row * (height / rows));

                seats += `<circle cx="${seatX}" cy="${seatY}" r="1.5" fill="#333" opacity="0.6"/>`;
            }
        }

        return seats;
    }

    async injectStadiumLayout() {
        const svgContent = this.generateStadiumSVG();

        await this.page.evaluate((svg) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svg;
                container.style.display = 'flex';
                container.style.justifyContent = 'center';
                container.style.alignItems = 'center';
                container.style.padding = '20px';
                container.style.backgroundColor = '#f8f9fa';
            }
        }, svgContent);

        // Wait for DOM update
        await this.page.waitForTimeout(100);
    }

    async takeScreenshot() {
        const screenshotPath = `iteration-${String(this.currentIteration).padStart(3, '0')}.png`;

        // Take screenshot of the stadium container
        const containerElement = await this.page.$('#admin-stadium-container');
        if (containerElement) {
            await containerElement.screenshot({ path: screenshotPath });
            console.log(`📸 Screenshot saved: ${screenshotPath}`);
            return screenshotPath;
        }
        return null;
    }

    async calculateSimilarity(screenshotPath) {
        try {
            // Load target and current images
            const targetBuffer = await sharp(this.targetImagePath).png().toBuffer();
            const currentBuffer = await sharp(screenshotPath).png().toBuffer();

            // Get image metadata
            const targetMeta = await sharp(targetBuffer).metadata();
            const currentMeta = await sharp(currentBuffer).metadata();

            // Resize to match dimensions for comparison
            const width = Math.min(targetMeta.width, currentMeta.width);
            const height = Math.min(targetMeta.height, currentMeta.height);

            const targetResized = await sharp(targetBuffer)
                .resize(width, height)
                .raw()
                .toBuffer();

            const currentResized = await sharp(currentBuffer)
                .resize(width, height)
                .raw()
                .toBuffer();

            // Calculate simple similarity based on pixel differences
            let totalDiff = 0;
            const totalPixels = width * height * 3; // RGB channels

            for (let i = 0; i < Math.min(targetResized.length, currentResized.length); i++) {
                totalDiff += Math.abs(targetResized[i] - currentResized[i]);
            }

            const similarity = 1 - (totalDiff / (totalPixels * 255));
            return Math.max(0, similarity);

        } catch (error) {
            console.error('Error calculating similarity:', error);
            return 0;
        }
    }

    async improvementStep() {
        // Deterministic improvement strategies
        const strategies = [
            () => this.adjustFieldSize(),
            () => this.adjustSectionColors(),
            () => this.adjustSectionSizes(),
            () => this.addMoreSeatDetail(),
            () => this.adjustCornerRadius(),
            () => this.optimizeSectionLayout()
        ];

        const strategy = strategies[this.currentIteration % strategies.length];
        strategy();

        console.log(`🔧 Applied improvement strategy ${this.currentIteration % strategies.length + 1}`);
    }

    adjustFieldSize() {
        this.config.fieldWidth = Math.min(this.config.fieldWidth + 10, 500);
        this.config.fieldHeight = Math.min(this.config.fieldHeight + 5, 350);
    }

    adjustSectionColors() {
        // Fine-tune colors to match target better
        this.config.sections.north.color = '#9333EA'; // More purple
        this.config.sections.east.color = '#DC2626';  // Darker red
        this.config.sections.south.color = '#2563EB'; // Better blue
        this.config.sections.west.color = '#F59E0B';  // Better yellow
    }

    adjustSectionSizes() {
        this.config.sectorWidth = Math.min(this.config.sectorWidth + 2, 100);
        this.config.sectorHeight = Math.min(this.config.sectorHeight + 2, 80);
    }

    addMoreSeatDetail() {
        this.config.seatsPerRow = Math.min(this.config.seatsPerRow + 2, 30);
        this.config.rowsPerSector = Math.min(this.config.rowsPerSector + 1, 8);
    }

    adjustCornerRadius() {
        this.config.cornerRadius = Math.min(this.config.cornerRadius + 5, 60);
    }

    optimizeSectionLayout() {
        // Final optimization phase
        this.config.fieldWidth = 420;
        this.config.fieldHeight = 300;
        this.config.sectorWidth = 85;
        this.config.sectorHeight = 75;
    }

    async generateDiffImage(currentPath, similarity) {
        try {
            const diffPath = `diff-${String(this.currentIteration).padStart(3, '0')}.png`;

            // Create a simple diff visualization
            const targetImage = sharp(this.targetImagePath);
            const currentImage = sharp(currentPath);

            // For now, just copy the current image as diff (can be enhanced with actual diff)
            await currentImage.png().toFile(diffPath);

            console.log(`🔍 Diff image saved: ${diffPath} (similarity: ${(similarity * 100).toFixed(2)}%)`);
            return diffPath;
        } catch (error) {
            console.error('Error generating diff image:', error);
            return null;
        }
    }

    async runIteration() {
        console.log(`\n🔄 Iteration ${this.currentIteration + 1}/${this.maxIterations}`);

        // Generate and inject stadium layout
        await this.injectStadiumLayout();

        // Take screenshot
        const screenshotPath = await this.takeScreenshot();
        if (!screenshotPath) {
            console.error('❌ Failed to take screenshot');
            return false;
        }

        // Calculate similarity
        const similarity = await this.calculateSimilarity(screenshotPath);
        console.log(`📊 Similarity: ${(similarity * 100).toFixed(2)}%`);

        // Generate diff image
        await this.generateDiffImage(screenshotPath, similarity);

        // Track best result
        if (similarity > this.bestSimilarity) {
            this.bestSimilarity = similarity;
            this.bestIteration = this.currentIteration;

            // Copy best screenshot
            fs.copyFileSync(screenshotPath, 'best-match.png');
            fs.copyFileSync(`diff-${String(this.currentIteration).padStart(3, '0')}.png`, 'diff-best.png');

            console.log(`🎯 New best match! Similarity: ${(similarity * 100).toFixed(2)}%`);
        }

        // Check if we've reached the target threshold
        if (similarity >= this.targetThreshold) {
            console.log(`🎉 Target threshold reached! (${(similarity * 100).toFixed(2)}%)`);
            return true;
        }

        // Apply improvement strategy
        await this.improvementStep();

        this.currentIteration++;
        return false;
    }

    async run() {
        try {
            await this.initialize();

            console.log(`🎯 Target threshold: ${(this.targetThreshold * 100)}%`);
            console.log(`🔄 Max iterations: ${this.maxIterations}`);

            let completed = false;
            while (this.currentIteration < this.maxIterations && !completed) {
                completed = await this.runIteration();

                if (completed) break;

                // Small delay between iterations
                await this.page.waitForTimeout(500);
            }

            // Generate final report
            await this.generateFinalReport();

        } catch (error) {
            console.error('❌ Error during execution:', error);
        } finally {
            if (this.browser) {
                await this.browser.close();
            }
        }
    }

    async generateFinalReport() {
        const report = {
            totalIterations: this.currentIteration,
            bestIteration: this.bestIteration,
            bestSimilarity: this.bestSimilarity,
            targetThreshold: this.targetThreshold,
            thresholdReached: this.bestSimilarity >= this.targetThreshold,
            finalConfig: this.config,
            artifacts: {
                bestMatch: 'best-match.png',
                bestDiff: 'diff-best.png',
                allIterations: Array.from({ length: this.currentIteration }, (_, i) =>
                    `iteration-${String(i).padStart(3, '0')}.png`)
            }
        };

        fs.writeFileSync('stadium-generation-report.json', JSON.stringify(report, null, 2));

        console.log('\n📋 FINAL REPORT');
        console.log('================');
        console.log(`🔄 Total iterations: ${report.totalIterations}`);
        console.log(`🎯 Best similarity: ${(report.bestSimilarity * 100).toFixed(2)}%`);
        console.log(`📍 Best iteration: ${report.bestIteration + 1}`);
        console.log(`✅ Threshold reached: ${report.thresholdReached ? 'YES' : 'NO'}`);
        console.log(`📁 Best match saved as: ${report.artifacts.bestMatch}`);
        console.log(`🔍 Best diff saved as: ${report.artifacts.bestDiff}`);
        console.log(`📊 Full report saved as: stadium-generation-report.json`);

        if (report.thresholdReached) {
            console.log('\n🎉 SUCCESS: Stadium layout matches target image!');
        } else {
            console.log('\n⚠️  PARTIAL SUCCESS: Best attempt saved, but threshold not reached.');
        }
    }
}

// Run the generator
async function main() {
    const generator = new StadiumLayoutGenerator();
    await generator.run();
}

if (require.main === module) {
    main().catch(console.error);
}

module.exports = StadiumLayoutGenerator;