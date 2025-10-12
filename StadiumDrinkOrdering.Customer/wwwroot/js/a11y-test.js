/**
 * Accessibility Testing Utilities
 * WCAG 2.2 Level AA Compliance Testing
 *
 * This module provides testing utilities for accessibility validation:
 * - Color contrast checking (WCAG 1.4.3)
 * - Tab order visualization
 * - Heading hierarchy validation
 * - ARIA attribute validation
 * - Touch target size verification
 * - Keyboard navigation testing
 */

(function (window, document) {
    'use strict';

    /**
     * A11yTest - Accessibility testing utilities
     */
    class A11yTest {
        constructor() {
            this.results = {
                contrast: [],
                tabOrder: [],
                headings: [],
                aria: [],
                touchTargets: [],
                forms: []
            };

            this.wcagLevels = {
                AA_NORMAL: 4.5,      // WCAG AA for normal text (< 24px or < 19px bold)
                AA_LARGE: 3.0,       // WCAG AA for large text (≥ 24px or ≥ 19px bold)
                AAA_NORMAL: 7.0,     // WCAG AAA for normal text
                AAA_LARGE: 4.5       // WCAG AAA for large text
            };
        }

        /**
         * Run all accessibility tests
         * @returns {Object} Comprehensive test results
         */
        runAllTests() {
            console.log('[A11y Test] Running comprehensive accessibility tests...');

            this.results = {
                contrast: this.testColorContrast(),
                tabOrder: this.testTabOrder(),
                headings: this.testHeadingHierarchy(),
                aria: this.testARIAAttributes(),
                touchTargets: this.testTouchTargets(),
                forms: this.testFormAccessibility()
            };

            this.displayTestResults();
            return this.results;
        }

        /**
         * Test color contrast ratios
         * WCAG 1.4.3 (Contrast Minimum) - Level AA
         * @returns {Array} Array of contrast test results
         */
        testColorContrast() {
            console.log('[A11y Test] Testing color contrast...');
            const results = [];

            // Get all text elements
            const textElements = document.querySelectorAll('p, span, a, button, label, h1, h2, h3, h4, h5, h6, li, td, th, div');

            textElements.forEach(element => {
                const text = element.textContent.trim();
                if (!text || text.length === 0) return;

                const styles = window.getComputedStyle(element);
                const fgColor = styles.color;
                const bgColor = this.getBackgroundColor(element);
                const fontSize = parseFloat(styles.fontSize);
                const fontWeight = styles.fontWeight;

                // Determine if text is "large"
                const isLarge = fontSize >= 24 || (fontSize >= 19 && parseInt(fontWeight) >= 700);
                const requiredRatio = isLarge ? this.wcagLevels.AA_LARGE : this.wcagLevels.AA_NORMAL;

                // Calculate contrast ratio
                const ratio = this.calculateContrastRatio(fgColor, bgColor);

                if (ratio < requiredRatio) {
                    results.push({
                        element: element,
                        text: text.substring(0, 50),
                        foreground: fgColor,
                        background: bgColor,
                        ratio: ratio.toFixed(2),
                        required: requiredRatio,
                        passes: false,
                        isLarge: isLarge
                    });
                }
            });

            console.log(`[A11y Test] Found ${results.length} contrast issues`);
            return results;
        }

        /**
         * Calculate contrast ratio between two colors
         * @param {string} fg - Foreground color (CSS color)
         * @param {string} bg - Background color (CSS color)
         * @returns {number} Contrast ratio (1-21)
         */
        calculateContrastRatio(fg, bg) {
            const fgLuminance = this.getRelativeLuminance(fg);
            const bgLuminance = this.getRelativeLuminance(bg);

            const lighter = Math.max(fgLuminance, bgLuminance);
            const darker = Math.min(fgLuminance, bgLuminance);

            return (lighter + 0.05) / (darker + 0.05);
        }

        /**
         * Get relative luminance of a color
         * @param {string} color - CSS color string
         * @returns {number} Relative luminance (0-1)
         */
        getRelativeLuminance(color) {
            const rgb = this.parseColor(color);
            if (!rgb) return 1;

            // Convert to relative luminance
            const [r, g, b] = rgb.map(val => {
                val = val / 255;
                return val <= 0.03928 ? val / 12.92 : Math.pow((val + 0.055) / 1.055, 2.4);
            });

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        /**
         * Parse CSS color to RGB array
         * @param {string} color - CSS color string
         * @returns {Array|null} [r, g, b] array or null
         */
        parseColor(color) {
            const canvas = document.createElement('canvas');
            canvas.width = canvas.height = 1;
            const ctx = canvas.getContext('2d');

            ctx.fillStyle = color;
            ctx.fillRect(0, 0, 1, 1);
            const data = ctx.getImageData(0, 0, 1, 1).data;

            return [data[0], data[1], data[2]];
        }

        /**
         * Get effective background color of an element
         * @param {HTMLElement} element - The element
         * @returns {string} Background color (CSS color)
         */
        getBackgroundColor(element) {
            let el = element;

            while (el) {
                const bg = window.getComputedStyle(el).backgroundColor;
                if (bg && bg !== 'rgba(0, 0, 0, 0)' && bg !== 'transparent') {
                    return bg;
                }
                el = el.parentElement;
            }

            return 'rgb(255, 255, 255)'; // Default to white
        }

        /**
         * Test and visualize tab order
         * WCAG 2.4.3 (Focus Order) - Level A
         * @returns {Array} Array of focusable elements in tab order
         */
        testTabOrder() {
            console.log('[A11y Test] Testing tab order...');
            const results = [];

            const focusableSelectors = [
                'a[href]',
                'button:not([disabled])',
                'input:not([disabled])',
                'select:not([disabled])',
                'textarea:not([disabled])',
                '[tabindex]:not([tabindex="-1"])'
            ].join(', ');

            const focusableElements = Array.from(document.querySelectorAll(focusableSelectors))
                .filter(el => {
                    // Filter out hidden elements
                    return el.offsetWidth > 0 || el.offsetHeight > 0;
                })
                .sort((a, b) => {
                    // Sort by tabindex, then by DOM order
                    const tabA = parseInt(a.getAttribute('tabindex')) || 0;
                    const tabB = parseInt(b.getAttribute('tabindex')) || 0;

                    if (tabA !== tabB) {
                        return tabA - tabB;
                    }

                    // Compare position in document
                    return a.compareDocumentPosition(b) & Node.DOCUMENT_POSITION_FOLLOWING ? -1 : 1;
                });

            focusableElements.forEach((element, index) => {
                const label = element.getAttribute('aria-label') ||
                    element.getAttribute('title') ||
                    element.textContent.trim().substring(0, 30) ||
                    element.tagName;

                results.push({
                    index: index + 1,
                    element: element,
                    label: label,
                    tabindex: element.getAttribute('tabindex') || '0',
                    id: element.id || '(no id)'
                });
            });

            console.log(`[A11y Test] Found ${results.length} focusable elements`);
            return results;
        }

        /**
         * Visualize tab order on page
         */
        visualizeTabOrder() {
            const tabOrder = this.testTabOrder();

            // Remove existing indicators
            document.querySelectorAll('.a11y-tab-indicator').forEach(el => el.remove());

            tabOrder.forEach(item => {
                const indicator = document.createElement('div');
                indicator.className = 'a11y-tab-indicator';
                indicator.textContent = item.index;
                indicator.style.cssText = `
                    position: absolute;
                    background: #ff0000;
                    color: #ffffff;
                    font-weight: bold;
                    font-size: 14px;
                    padding: 4px 8px;
                    border-radius: 50%;
                    z-index: 10000;
                    pointer-events: none;
                `;

                const rect = item.element.getBoundingClientRect();
                indicator.style.top = `${rect.top + window.scrollY - 10}px`;
                indicator.style.left = `${rect.left + window.scrollX - 10}px`;

                document.body.appendChild(indicator);
            });

            console.log('[A11y Test] Tab order visualized on page');
        }

        /**
         * Clear tab order visualization
         */
        clearTabOrderVisualization() {
            document.querySelectorAll('.a11y-tab-indicator').forEach(el => el.remove());
        }

        /**
         * Test heading hierarchy
         * WCAG 1.3.1 (Info and Relationships) - Level A
         * @returns {Array} Array of heading issues
         */
        testHeadingHierarchy() {
            console.log('[A11y Test] Testing heading hierarchy...');
            const results = [];
            const headings = Array.from(document.querySelectorAll('h1, h2, h3, h4, h5, h6'));

            if (headings.length === 0) {
                results.push({
                    type: 'warning',
                    message: 'No headings found on page'
                });
                return results;
            }

            let previousLevel = 0;

            headings.forEach((heading, index) => {
                const level = parseInt(heading.tagName.charAt(1));
                const text = heading.textContent.trim();

                // Check for H1
                if (index === 0 && level !== 1) {
                    results.push({
                        type: 'error',
                        element: heading,
                        message: `First heading should be H1, found H${level}`,
                        text: text
                    });
                }

                // Check for skipped levels
                if (previousLevel > 0 && level > previousLevel + 1) {
                    results.push({
                        type: 'error',
                        element: heading,
                        message: `Heading level skipped from H${previousLevel} to H${level}`,
                        text: text
                    });
                }

                // Check for empty headings
                if (!text) {
                    results.push({
                        type: 'error',
                        element: heading,
                        message: `Empty H${level} heading found`,
                        text: '(empty)'
                    });
                }

                previousLevel = level;
            });

            // Check for multiple H1s
            const h1Count = headings.filter(h => h.tagName === 'H1').length;
            if (h1Count > 1) {
                results.push({
                    type: 'warning',
                    message: `Multiple H1 headings found (${h1Count}). Consider using only one H1 per page.`
                });
            }

            console.log(`[A11y Test] Found ${results.length} heading issues`);
            return results;
        }

        /**
         * Test ARIA attributes
         * WCAG 4.1.2 (Name, Role, Value) - Level A
         * @returns {Array} Array of ARIA issues
         */
        testARIAAttributes() {
            console.log('[A11y Test] Testing ARIA attributes...');
            const results = [];

            // Test buttons without labels
            const buttons = document.querySelectorAll('button, [role="button"]');
            buttons.forEach(button => {
                const hasLabel = button.getAttribute('aria-label') ||
                    button.getAttribute('aria-labelledby') ||
                    button.textContent.trim();

                if (!hasLabel) {
                    results.push({
                        type: 'error',
                        element: button,
                        message: 'Button without accessible label',
                        suggestion: 'Add aria-label or visible text content'
                    });
                }
            });

            // Test form inputs without labels
            const inputs = document.querySelectorAll('input:not([type="hidden"]), select, textarea');
            inputs.forEach(input => {
                const hasLabel = input.getAttribute('aria-label') ||
                    input.getAttribute('aria-labelledby') ||
                    document.querySelector(`label[for="${input.id}"]`);

                if (!hasLabel) {
                    results.push({
                        type: 'error',
                        element: input,
                        message: `${input.tagName} without associated label`,
                        suggestion: 'Add label element or aria-label attribute'
                    });
                }
            });

            // Test images without alt text
            const images = document.querySelectorAll('img');
            images.forEach(img => {
                if (!img.hasAttribute('alt')) {
                    results.push({
                        type: 'error',
                        element: img,
                        message: 'Image without alt attribute',
                        suggestion: 'Add alt="" for decorative images or descriptive alt text'
                    });
                }
            });

            // Test expandable elements
            const expandables = document.querySelectorAll('[aria-expanded]');
            expandables.forEach(expandable => {
                const expanded = expandable.getAttribute('aria-expanded');
                if (expanded !== 'true' && expanded !== 'false') {
                    results.push({
                        type: 'error',
                        element: expandable,
                        message: 'aria-expanded must be "true" or "false"',
                        current: expanded
                    });
                }
            });

            console.log(`[A11y Test] Found ${results.length} ARIA issues`);
            return results;
        }

        /**
         * Test touch target sizes
         * WCAG 2.5.5 (Target Size) - Level AAA
         * @returns {Array} Array of touch target issues
         */
        testTouchTargets() {
            console.log('[A11y Test] Testing touch target sizes...');
            const results = [];
            const minSize = 48; // Minimum 48x48px per WCAG AAA

            const interactiveElements = document.querySelectorAll('button, a, input, select, [role="button"], [onclick]');

            interactiveElements.forEach(element => {
                const rect = element.getBoundingClientRect();

                if (rect.width < minSize || rect.height < minSize) {
                    results.push({
                        type: 'warning',
                        element: element,
                        message: 'Touch target too small',
                        size: `${Math.round(rect.width)}x${Math.round(rect.height)}px`,
                        minimum: `${minSize}x${minSize}px`,
                        label: element.getAttribute('aria-label') || element.textContent.trim().substring(0, 30)
                    });
                }
            });

            console.log(`[A11y Test] Found ${results.length} touch target issues`);
            return results;
        }

        /**
         * Test form accessibility
         * WCAG 3.3.1 (Error Identification) - Level A
         * WCAG 3.3.2 (Labels or Instructions) - Level A
         * @returns {Array} Array of form accessibility issues
         */
        testFormAccessibility() {
            console.log('[A11y Test] Testing form accessibility...');
            const results = [];

            const forms = document.querySelectorAll('form');

            forms.forEach(form => {
                // Check for form name/label
                if (!form.getAttribute('aria-label') && !form.getAttribute('aria-labelledby')) {
                    results.push({
                        type: 'warning',
                        element: form,
                        message: 'Form without accessible name',
                        suggestion: 'Add aria-label or aria-labelledby'
                    });
                }

                // Check required fields
                const requiredInputs = form.querySelectorAll('[required]');
                requiredInputs.forEach(input => {
                    if (!input.getAttribute('aria-required')) {
                        results.push({
                            type: 'info',
                            element: input,
                            message: 'Required field without aria-required',
                            suggestion: 'Add aria-required="true"'
                        });
                    }

                    // Check for visual required indicator
                    const label = form.querySelector(`label[for="${input.id}"]`);
                    if (label && !label.textContent.includes('*') && !label.querySelector('.required')) {
                        results.push({
                            type: 'info',
                            element: label,
                            message: 'Required field without visual indicator',
                            suggestion: 'Add asterisk or "required" text'
                        });
                    }
                });

                // Check error messages
                const errorMessages = form.querySelectorAll('.invalid-feedback, .error-message');
                errorMessages.forEach(error => {
                    const relatedInput = error.previousElementSibling;
                    if (relatedInput && !relatedInput.getAttribute('aria-describedby')) {
                        results.push({
                            type: 'warning',
                            element: error,
                            message: 'Error message not associated with input',
                            suggestion: 'Use aria-describedby to link error to input'
                        });
                    }

                    if (!error.getAttribute('role')) {
                        results.push({
                            type: 'info',
                            element: error,
                            message: 'Error message without role',
                            suggestion: 'Add role="alert"'
                        });
                    }
                });
            });

            console.log(`[A11y Test] Found ${results.length} form accessibility issues`);
            return results;
        }

        /**
         * Display test results in console
         */
        displayTestResults() {
            console.log('\n========================================');
            console.log('ACCESSIBILITY TEST RESULTS');
            console.log('========================================\n');

            const categories = [
                { key: 'contrast', label: 'Color Contrast' },
                { key: 'tabOrder', label: 'Tab Order' },
                { key: 'headings', label: 'Heading Hierarchy' },
                { key: 'aria', label: 'ARIA Attributes' },
                { key: 'touchTargets', label: 'Touch Targets' },
                { key: 'forms', label: 'Form Accessibility' }
            ];

            categories.forEach(category => {
                const results = this.results[category.key];
                const issueCount = results.filter(r => r.type === 'error').length;
                const warningCount = results.filter(r => r.type === 'warning').length;

                console.log(`${category.label}: ${issueCount} errors, ${warningCount} warnings`);

                if (results.length > 0 && results.length <= 10) {
                    results.forEach(result => {
                        console.log(`  - ${result.message}`);
                    });
                }
            });

            console.log('\n========================================\n');
        }

        /**
         * Generate HTML report
         * @returns {string} HTML report string
         */
        generateHTMLReport() {
            let html = '<div style="font-family: system-ui; padding: 20px; max-width: 1200px; margin: 0 auto;">';
            html += '<h1>Accessibility Test Report</h1>';
            html += `<p>Generated: ${new Date().toLocaleString()}</p>`;

            const categories = [
                { key: 'contrast', label: 'Color Contrast', icon: '🎨' },
                { key: 'tabOrder', label: 'Tab Order', icon: '⌨️' },
                { key: 'headings', label: 'Heading Hierarchy', icon: '📝' },
                { key: 'aria', label: 'ARIA Attributes', icon: '♿' },
                { key: 'touchTargets', label: 'Touch Targets', icon: '👆' },
                { key: 'forms', label: 'Form Accessibility', icon: '📋' }
            ];

            categories.forEach(category => {
                const results = this.results[category.key];
                const errorCount = results.filter(r => r.type === 'error').length;
                const warningCount = results.filter(r => r.type === 'warning').length;

                html += `<div style="margin: 30px 0; padding: 20px; border: 1px solid #ddd; border-radius: 8px;">`;
                html += `<h2>${category.icon} ${category.label}</h2>`;
                html += `<p><strong>${errorCount}</strong> errors, <strong>${warningCount}</strong> warnings</p>`;

                if (results.length > 0) {
                    html += '<ul>';
                    results.forEach(result => {
                        const color = result.type === 'error' ? '#dc3545' : result.type === 'warning' ? '#ffc107' : '#17a2b8';
                        html += `<li style="color: ${color}; margin: 10px 0;">${result.message}`;
                        if (result.suggestion) {
                            html += `<br><em style="color: #666;">Suggestion: ${result.suggestion}</em>`;
                        }
                        html += '</li>';
                    });
                    html += '</ul>';
                } else {
                    html += '<p style="color: #28a745;">✓ No issues found</p>';
                }

                html += '</div>';
            });

            html += '</div>';
            return html;
        }
    }

    // Create global instance
    window.a11yTest = new A11yTest();

    // Expose useful functions globally
    window.runA11yTests = () => window.a11yTest.runAllTests();
    window.visualizeTabOrder = () => window.a11yTest.visualizeTabOrder();
    window.clearTabOrder = () => window.a11yTest.clearTabOrderVisualization();
    window.a11yReport = () => {
        const report = window.a11yTest.generateHTMLReport();
        const win = window.open('', 'A11y Report');
        win.document.write(report);
    };

    console.log('[A11y Test] Accessibility testing utilities loaded');
    console.log('Available commands:');
    console.log('  - runA11yTests()       : Run all tests');
    console.log('  - visualizeTabOrder()  : Show tab order on page');
    console.log('  - clearTabOrder()      : Hide tab order visualization');
    console.log('  - a11yReport()         : Generate HTML report in new window');

})(window, document);
