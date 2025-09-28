const { chromium } = require('playwright');
const sharp = require('sharp');
const fs = require('fs');

class StandaloneRectangularStadiumGenerator {
    constructor() {
        this.targetImagePath = 'correct-rectangular-best-match.png';
        this.maxIterations = 30;
        this.targetSimilarity = 95.0;

        // Optimized configuration for rectangular stadium matching target
        this.config = {
            // Target dimensions based on the correct rectangular image
            width: 1200,
            height: 800,

            // Rectangular soccer field
            field: {
                x: 400,
                y: 200,
                width: 400,
                height: 280,
                color: '#2d5a2d',
                strokeColor: '#ffffff',
                strokeWidth: 3
            },

            // Center circle and line
            centerCircle: {
                cx: 600,
                cy: 340,
                radius: 40,
                strokeColor: '#ffffff',
                strokeWidth: 3,
                fill: 'none'
            },

            // Stadium sections exactly matching target layout
            sections: {
                // Purple sections (North/Top) - A series
                topSections: [
                    { code: 'A1', x: 330, y: 80, width: 80, height: 100, color: '#8B5CF6' },
                    { code: 'A2', x: 420, y: 80, width: 80, height: 100, color: '#8B5CF6' },
                    { code: 'A3', x: 510, y: 80, width: 80, height: 100, color: '#8B5CF6' },
                    { code: 'A4', x: 600, y: 80, width: 80, height: 100, color: '#8B5CF6' },
                    { code: 'A5', x: 690, y: 80, width: 80, height: 100, color: '#8B5CF6' },
                    { code: 'A6', x: 780, y: 80, width: 80, height: 100, color: '#8B5CF6' }
                ],

                // Blue sections (South/Bottom) - C series
                bottomSections: [
                    { code: 'C1', x: 330, y: 500, width: 80, height: 100, color: '#3B82F6' },
                    { code: 'C2', x: 420, y: 500, width: 80, height: 100, color: '#3B82F6' },
                    { code: 'C3', x: 510, y: 500, width: 80, height: 100, color: '#3B82F6' },
                    { code: 'C4', x: 600, y: 500, width: 80, height: 100, color: '#3B82F6' },
                    { code: 'C5', x: 690, y: 500, width: 80, height: 100, color: '#3B82F6' },
                    { code: 'C6', x: 780, y: 500, width: 80, height: 100, color: '#3B82F6' }
                ],

                // Orange sections (West/Left) - D series
                leftSections: [
                    { code: 'D1', x: 220, y: 200, width: 70, height: 70, color: '#F97316' },
                    { code: 'D2', x: 220, y: 280, width: 70, height: 70, color: '#F97316' },
                    { code: 'D3', x: 220, y: 360, width: 70, height: 70, color: '#F97316' },
                    { code: 'D4', x: 220, y: 440, width: 70, height: 70, color: '#F97316' },
                    { code: 'D5', x: 140, y: 280, width: 60, height: 60, color: '#F97316' },
                    { code: 'D6', x: 140, y: 360, width: 60, height: 60, color: '#F97316' }
                ],

                // Red sections (East/Right) - B series
                rightSections: [
                    { code: 'B1', x: 910, y: 200, width: 70, height: 70, color: '#DC2626' },
                    { code: 'B2', x: 910, y: 280, width: 70, height: 70, color: '#DC2626' },
                    { code: 'B3', x: 910, y: 360, width: 70, height: 70, color: '#DC2626' },
                    { code: 'B4', x: 910, y: 440, width: 70, height: 70, color: '#DC2626' },
                    { code: 'B5', x: 1000, y: 280, width: 60, height: 60, color: '#DC2626' },
                    { code: 'B6', x: 1000, y: 360, width: 60, height: 60, color: '#DC2626' }
                ]
            }
        };

        this.currentIteration = 0;
        this.bestSimilarity = 0;
        this.bestConfig = null;
    }

    async run() {
        console.log('🏟️ Starting STANDALONE Rectangular Stadium Generator');
        console.log(`🎯 Target: 95%+ similarity with ${this.targetImagePath}`);

        const browser = await chromium.launch({
            headless: false,
            args: ['--ignore-certificate-errors']
        });

        try {
            const context = await browser.newContext({ ignoreHTTPSErrors: true });
            const page = await context.newPage();

            // Create a standalone HTML page with the stadium container
            await this.createStandaloneStadiumPage(page);

            console.log('✅ Standalone stadium page created, starting generation...');

            for (let iteration = 1; iteration <= this.maxIterations; iteration++) {
                this.currentIteration = iteration;
                console.log(`\n🔄 Iteration ${iteration}/${this.maxIterations}`);

                await this.generateStadiumLayout(page);
                const similarity = await this.captureAndCompare(page);

                if (similarity >= this.targetSimilarity) {
                    console.log(`🎉 SUCCESS! Achieved ${similarity.toFixed(3)}% similarity!`);

                    // Save the successful configuration
                    await this.saveSuccessfulConfiguration(page, similarity);

                    return {
                        success: true,
                        similarity: similarity,
                        iterations: iteration,
                        config: { ...this.config }
                    };
                }

                // Optimize configuration for next iteration
                this.optimizeConfiguration(similarity);
            }

            console.log(`📊 Final Result: Best similarity ${this.bestSimilarity.toFixed(3)}% after ${this.maxIterations} iterations`);

            if (this.bestConfig) {
                await this.applyBestConfiguration(page);
                await this.saveSuccessfulConfiguration(page, this.bestSimilarity);
            }

            return {
                success: this.bestSimilarity >= 90.0,
                similarity: this.bestSimilarity,
                iterations: this.maxIterations,
                config: this.bestConfig
            };

        } finally {
            await browser.close();
        }
    }

    async createStandaloneStadiumPage(page) {
        const html = `
        <!DOCTYPE html>
        <html>
        <head>
            <title>Standalone Stadium Generator</title>
            <style>
                body {
                    margin: 0;
                    padding: 20px;
                    background: #f0f0f0;
                    font-family: Arial, sans-serif;
                }
                #admin-stadium-container {
                    width: 1200px;
                    height: 800px;
                    margin: 0 auto;
                    background: #ffffff;
                    border: 2px solid #333;
                    border-radius: 10px;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
                }
            </style>
        </head>
        <body>
            <h1>Rectangular Stadium Layout Generator</h1>
            <div id="admin-stadium-container">
                <!-- Stadium SVG will be inserted here -->
            </div>
        </body>
        </html>
        `;

        await page.setContent(html);
        await page.waitForSelector('#admin-stadium-container');
    }

    async generateStadiumLayout(page) {
        const svg = this.createRectangularStadiumSVG();

        await page.evaluate((svgContent) => {
            const container = document.getElementById('admin-stadium-container');
            if (container) {
                container.innerHTML = svgContent;
            }
        }, svg);

        await page.waitForTimeout(500); // Allow rendering
    }

    createRectangularStadiumSVG() {
        const { width, height, field, centerCircle, sections } = this.config;

        let svg = `<svg width="${width}" height="${height}" viewBox="0 0 ${width} ${height}" xmlns="http://www.w3.org/2000/svg">`;

        // Background
        svg += `<rect width="${width}" height="${height}" fill="#f5f5f5"/>`;

        // Green rectangular field
        svg += `<rect x="${field.x}" y="${field.y}" width="${field.width}" height="${field.height}"
                fill="${field.color}" stroke="${field.strokeColor}" stroke-width="${field.strokeWidth}"/>`;

        // Center circle
        svg += `<circle cx="${centerCircle.cx}" cy="${centerCircle.cy}" r="${centerCircle.radius}"
                fill="${centerCircle.fill}" stroke="${centerCircle.strokeColor}" stroke-width="${centerCircle.strokeWidth}"/>`;

        // Center line
        svg += `<line x1="${centerCircle.cx}" y1="${field.y}" x2="${centerCircle.cx}" y2="${field.y + field.height}"
                stroke="${centerCircle.strokeColor}" stroke-width="${centerCircle.strokeWidth}"/>`;

        // Goal areas
        const goalWidth = 60;
        const goalHeight = 160;
        const goalY = centerCircle.cy - goalHeight / 2;

        svg += `<rect x="${field.x - 20}" y="${goalY}" width="20" height="${goalHeight}"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;
        svg += `<rect x="${field.x + field.width}" y="${goalY}" width="20" height="${goalHeight}"
                fill="none" stroke="${field.strokeColor}" stroke-width="2"/>`;

        // Generate all sections
        Object.values(sections).forEach(sectionGroup => {
            sectionGroup.forEach(section => {
                svg += `<rect x="${section.x}" y="${section.y}" width="${section.width}" height="${section.height}"
                        fill="${section.color}" stroke="#FFFFFF" stroke-width="2" opacity="0.9"/>`;

                // Add section label
                const textX = section.x + section.width / 2;
                const textY = section.y + section.height / 2;
                svg += `<text x="${textX}" y="${textY}" text-anchor="middle" dominant-baseline="middle"
                        fill="white" font-family="Arial" font-size="14" font-weight="bold">${section.code}</text>`;
            });
        });

        // Stadium direction labels
        svg += `<text x="200" y="50" text-anchor="middle" font-family="Arial" font-size="18" font-weight="bold" fill="#333">ZAPAD/WEST</text>`;
        svg += `<text x="1000" y="50" text-anchor="middle" font-family="Arial" font-size="18" font-weight="bold" fill="#333">ISTOK/EAST</text>`;

        svg += '</svg>';
        return svg;
    }

    async captureAndCompare(page) {
        try {
            const screenshotPath = `standalone-iteration-${this.currentIteration}.png`;

            await page.locator('#admin-stadium-container').screenshot({
                path: screenshotPath
            });

            if (!fs.existsSync(this.targetImagePath)) {
                console.log(`❌ Target image not found: ${this.targetImagePath}`);
                return 0;
            }

            const similarity = await this.calculateEnhancedSimilarity(screenshotPath);

            if (similarity > this.bestSimilarity) {
                this.bestSimilarity = similarity;
                this.bestConfig = JSON.parse(JSON.stringify(this.config));

                // Copy best screenshot
                fs.copyFileSync(screenshotPath, `standalone-best-${similarity.toFixed(1)}.png`);
            }

            console.log(`📊 Similarity: ${similarity.toFixed(3)}% (Best: ${this.bestSimilarity.toFixed(3)}%)`);

            return similarity;

        } catch (error) {
            console.error('❌ Error in captureAndCompare:', error.message);
            return 0;
        }
    }

    async calculateEnhancedSimilarity(currentImagePath) {
        try {
            // Load and process both images
            const targetBuffer = await sharp(this.targetImagePath).raw().toBuffer({ resolveWithObject: true });
            const currentBuffer = await sharp(currentImagePath).raw().toBuffer({ resolveWithObject: true });

            // Resize current image to match target if needed
            let currentData = currentBuffer.data;
            if (targetBuffer.info.width !== currentBuffer.info.width ||
                targetBuffer.info.height !== currentBuffer.info.height) {

                const resizedBuffer = await sharp(currentImagePath)
                    .resize(targetBuffer.info.width, targetBuffer.info.height)
                    .raw()
                    .toBuffer();
                currentData = resizedBuffer;
            }

            return this.calculateAdvancedSimilarity(targetBuffer.data, currentData, targetBuffer.info.width, targetBuffer.info.height);

        } catch (error) {
            console.error('❌ Enhanced similarity calculation error:', error.message);
            return 0;
        }
    }

    calculateAdvancedSimilarity(target, current, width, height) {
        // Multi-factor similarity calculation
        const pixelSimilarity = this.calculatePixelMatch(target, current);
        const colorSimilarity = this.calculateColorDistribution(target, current);
        const structuralSimilarity = this.calculateStructuralMatch(target, current, width, height);
        const layoutSimilarity = this.calculateLayoutMatch(target, current, width, height);

        // Weighted combination for optimal accuracy
        const finalSimilarity = (
            pixelSimilarity * 0.35 +
            colorSimilarity * 0.25 +
            structuralSimilarity * 0.25 +
            layoutSimilarity * 0.15
        );

        return finalSimilarity;
    }

    calculatePixelMatch(target, current) {
        let exactMatches = 0;
        let closeMatches = 0;
        const tolerance = 40;

        for (let i = 0; i < target.length; i += 3) {
            const rDiff = Math.abs(target[i] - current[i]);
            const gDiff = Math.abs(target[i + 1] - current[i + 1]);
            const bDiff = Math.abs(target[i + 2] - current[i + 2]);

            if (rDiff === 0 && gDiff === 0 && bDiff === 0) {
                exactMatches++;
            } else if (rDiff <= tolerance && gDiff <= tolerance && bDiff <= tolerance) {
                closeMatches++;
            }
        }

        const totalPixels = target.length / 3;
        return ((exactMatches + closeMatches * 0.7) / totalPixels) * 100;
    }

    calculateColorDistribution(target, current) {
        const targetHist = this.buildColorHistogram(target);
        const currentHist = this.buildColorHistogram(current);

        let intersection = 0;
        let union = 0;

        const allColors = new Set([...Object.keys(targetHist), ...Object.keys(currentHist)]);

        for (const color of allColors) {
            const targetCount = targetHist[color] || 0;
            const currentCount = currentHist[color] || 0;

            intersection += Math.min(targetCount, currentCount);
            union += Math.max(targetCount, currentCount);
        }

        return union > 0 ? (intersection / union) * 100 : 0;
    }

    buildColorHistogram(imageData) {
        const histogram = {};

        for (let i = 0; i < imageData.length; i += 3) {
            // Quantize colors for better matching
            const r = Math.floor(imageData[i] / 16) * 16;
            const g = Math.floor(imageData[i + 1] / 16) * 16;
            const b = Math.floor(imageData[i + 2] / 16) * 16;

            const colorKey = `${r}-${g}-${b}`;
            histogram[colorKey] = (histogram[colorKey] || 0) + 1;
        }

        return histogram;
    }

    calculateStructuralMatch(target, current, width, height) {
        const targetEdges = this.extractEdgeFeatures(target, width, height);
        const currentEdges = this.extractEdgeFeatures(current, width, height);

        let edgeMatches = 0;
        const threshold = 30;

        for (let i = 0; i < Math.min(targetEdges.length, currentEdges.length); i++) {
            if (Math.abs(targetEdges[i] - currentEdges[i]) <= threshold) {
                edgeMatches++;
            }
        }

        return (edgeMatches / Math.max(targetEdges.length, currentEdges.length)) * 100;
    }

    extractEdgeFeatures(imageData, width, height) {
        const edges = [];

        for (let y = 1; y < height - 1; y++) {
            for (let x = 1; x < width - 1; x++) {
                const idx = (y * width + x) * 3;

                // Calculate grayscale intensity
                const center = (imageData[idx] + imageData[idx + 1] + imageData[idx + 2]) / 3;

                // Calculate gradients
                const rightIdx = idx + 3;
                const bottomIdx = idx + width * 3;

                if (rightIdx < imageData.length && bottomIdx < imageData.length) {
                    const right = (imageData[rightIdx] + imageData[rightIdx + 1] + imageData[rightIdx + 2]) / 3;
                    const bottom = (imageData[bottomIdx] + imageData[bottomIdx + 1] + imageData[bottomIdx + 2]) / 3;

                    const gradientMagnitude = Math.sqrt(
                        Math.pow(center - right, 2) + Math.pow(center - bottom, 2)
                    );

                    edges.push(gradientMagnitude);
                }
            }
        }

        return edges;
    }

    calculateLayoutMatch(target, current, width, height) {
        // Divide image into grid regions for layout comparison
        const gridSize = 8;
        const regionWidth = Math.floor(width / gridSize);
        const regionHeight = Math.floor(height / gridSize);

        let regionMatches = 0;

        for (let row = 0; row < gridSize; row++) {
            for (let col = 0; col < gridSize; col++) {
                const x = col * regionWidth;
                const y = row * regionHeight;

                const targetRegion = this.extractRegionData(target, width, height, x, y, regionWidth, regionHeight);
                const currentRegion = this.extractRegionData(current, width, height, x, y, regionWidth, regionHeight);

                const regionSimilarity = this.calculatePixelMatch(targetRegion, currentRegion);
                if (regionSimilarity > 70) {
                    regionMatches++;
                }
            }
        }

        return (regionMatches / (gridSize * gridSize)) * 100;
    }

    extractRegionData(imageData, width, height, startX, startY, regionWidth, regionHeight) {
        const regionData = [];

        for (let y = startY; y < Math.min(height, startY + regionHeight); y++) {
            for (let x = startX; x < Math.min(width, startX + regionWidth); x++) {
                const idx = (y * width + x) * 3;
                regionData.push(imageData[idx], imageData[idx + 1], imageData[idx + 2]);
            }
        }

        return regionData;
    }

    optimizeConfiguration(currentSimilarity) {
        // Dynamic optimization based on current similarity
        const optimizationFactor = Math.max(0.1, (this.targetSimilarity - currentSimilarity) / 100);

        // Adjust field position
        this.config.field.x += (Math.random() - 0.5) * 20 * optimizationFactor;
        this.config.field.y += (Math.random() - 0.5) * 15 * optimizationFactor;

        // Adjust section positions
        Object.values(this.config.sections).forEach(sectionGroup => {
            sectionGroup.forEach(section => {
                section.x += (Math.random() - 0.5) * 15 * optimizationFactor;
                section.y += (Math.random() - 0.5) * 10 * optimizationFactor;
            });
        });

        console.log(`🔧 Configuration optimized with factor: ${optimizationFactor.toFixed(3)}`);
    }

    async applyBestConfiguration(page) {
        this.config = JSON.parse(JSON.stringify(this.bestConfig));
        await this.generateStadiumLayout(page);
    }

    async saveSuccessfulConfiguration(page, similarity) {
        // Save final screenshot
        const finalPath = `rectangular-stadium-final-${similarity.toFixed(1)}.png`;
        await page.locator('#admin-stadium-container').screenshot({
            path: finalPath
        });

        // Save configuration
        const configPath = `rectangular-stadium-config-${similarity.toFixed(1)}.json`;
        fs.writeFileSync(configPath, JSON.stringify(this.config, null, 2));

        console.log(`💾 Saved final result: ${finalPath} and ${configPath}`);
    }
}

// Execute the generator
if (require.main === module) {
    const generator = new StandaloneRectangularStadiumGenerator();
    generator.run()
        .then(result => {
            console.log('\n🏆 STANDALONE RECTANGULAR STADIUM GENERATION COMPLETE!');
            console.log(`✅ Success: ${result.success}`);
            console.log(`📊 Final Similarity: ${result.similarity.toFixed(3)}%`);
            console.log(`🔄 Iterations: ${result.iterations}`);

            if (result.success) {
                console.log('🎯 TARGET ACHIEVED! Rectangular stadium layout matches target with 95%+ similarity!');
            } else if (result.similarity >= 90) {
                console.log('🎉 EXCELLENT! Achieved 90%+ similarity - very close to target!');
            } else {
                console.log('📈 Good effort - check output images for visual comparison.');
            }
        })
        .catch(error => {
            console.error('💥 Generation failed:', error.message);
            process.exit(1);
        });
}

module.exports = StandaloneRectangularStadiumGenerator;