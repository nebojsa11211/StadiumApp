/**
 * STADIUM MAXIMUM WIDTH INTERACTIONS
 * Enhanced JavaScript interactions for the maximum-width stadium layout
 * Provides responsive handling, performance optimizations, and smooth UX
 */

class StadiumMaximumWidthManager {
    constructor() {
        this.isInitialized = false;
        this.resizeObserver = null;
        this.intersectionObserver = null;
        this.performanceConfig = {
            debounceDelay: 250,
            throttleDelay: 100,
            enableGPUAcceleration: true,
            lazyLoadThreshold: 0.1
        };

        // Bind methods to maintain context
        this.handleResize = this.debounce(this.handleResize.bind(this), this.performanceConfig.debounceDelay);
        this.handleScroll = this.throttle(this.handleScroll.bind(this), this.performanceConfig.throttleDelay);
        this.handleSectorInteraction = this.handleSectorInteraction.bind(this);
        this.handleKeyboardNavigation = this.handleKeyboardNavigation.bind(this);
    }

    /**
     * Initialize the maximum width stadium interactions
     */
    init() {
        if (this.isInitialized) return;

        console.log('🏟️ Initializing Stadium Maximum Width Manager...');

        // Wait for DOM to be fully loaded
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupInteractions());
        } else {
            this.setupInteractions();
        }

        this.isInitialized = true;
    }

    /**
     * Setup all interactions and observers
     */
    setupInteractions() {
        this.setupViewportOptimizations();
        this.setupResponsiveHandling();
        this.setupSectorInteractions();
        this.setupKeyboardNavigation();
        this.setupPerformanceOptimizations();
        this.setupAccessibilityEnhancements();

        console.log('✅ Stadium Maximum Width Manager initialized successfully');
    }

    /**
     * Setup viewport-specific optimizations
     */
    setupViewportOptimizations() {
        const stadiumContainer = document.getElementById('admin-stadium-container');
        if (!stadiumContainer) return;

        // Add viewport classes based on screen size
        this.updateViewportClasses();

        // Setup resize observer for dynamic viewport changes
        if (window.ResizeObserver) {
            this.resizeObserver = new ResizeObserver(entries => {
                for (const entry of entries) {
                    this.handleContainerResize(entry);
                }
            });
            this.resizeObserver.observe(stadiumContainer);
        }

        // Fallback for older browsers
        window.addEventListener('resize', this.handleResize);
        window.addEventListener('orientationchange', this.handleResize);
    }

    /**
     * Update viewport classes based on current screen size
     */
    updateViewportClasses() {
        const stadiumLayout = document.getElementById('admin-stadium-flex-layout');
        if (!stadiumLayout) return;

        const viewportWidth = window.innerWidth;
        const viewportHeight = window.innerHeight;

        // Remove existing viewport classes
        stadiumLayout.classList.remove(
            'viewport-mobile', 'viewport-tablet', 'viewport-desktop',
            'viewport-large-desktop', 'viewport-ultra-wide', 'viewport-super-wide'
        );

        // Add appropriate viewport class
        if (viewportWidth >= 3840) {
            stadiumLayout.classList.add('viewport-super-wide');
        } else if (viewportWidth >= 2560) {
            stadiumLayout.classList.add('viewport-ultra-wide');
        } else if (viewportWidth >= 1440) {
            stadiumLayout.classList.add('viewport-large-desktop');
        } else if (viewportWidth >= 1024) {
            stadiumLayout.classList.add('viewport-desktop');
        } else if (viewportWidth >= 768) {
            stadiumLayout.classList.add('viewport-tablet');
        } else {
            stadiumLayout.classList.add('viewport-mobile');
        }

        // Set CSS custom properties for dynamic calculations
        document.documentElement.style.setProperty('--viewport-width', `${viewportWidth}px`);
        document.documentElement.style.setProperty('--viewport-height', `${viewportHeight}px`);
        document.documentElement.style.setProperty('--aspect-ratio', (viewportWidth / viewportHeight).toFixed(2));

        console.log(`📐 Viewport updated: ${viewportWidth}x${viewportHeight}`);
    }

    /**
     * Handle container resize events
     */
    handleContainerResize(entry) {
        const { width, height } = entry.contentRect;

        // Update container dimensions
        entry.target.style.setProperty('--container-width', `${width}px`);
        entry.target.style.setProperty('--container-height', `${height}px`);

        // Optimize sector grid based on available space
        this.optimizeSectorGrids(width, height);

        // Update field proportions
        this.updateFieldProportions(width, height);
    }

    /**
     * Optimize sector grids based on container dimensions
     */
    optimizeSectorGrids(containerWidth, containerHeight) {
        const sectorsGrids = document.querySelectorAll('.sectors-grid-maximum-density');

        sectorsGrids.forEach(grid => {
            const availableWidth = containerWidth * 0.2; // Approximate stand width
            const optimalSectorSize = Math.max(60, Math.min(130, availableWidth / 4));

            grid.style.setProperty('--optimal-sector-size', `${optimalSectorSize}px`);
            grid.style.gridTemplateColumns = `repeat(auto-fit, minmax(${optimalSectorSize}px, 1fr))`;
        });
    }

    /**
     * Update field proportions for different screen sizes
     */
    updateFieldProportions(containerWidth, containerHeight) {
        const field = document.getElementById('admin-stadium-field');
        if (!field) return;

        const availableSpace = Math.min(containerWidth * 0.6, containerHeight * 0.8);
        const fieldSize = Math.max(300, Math.min(800, availableSpace));

        field.style.setProperty('--dynamic-field-size', `${fieldSize}px`);
    }

    /**
     * Setup responsive handling for different breakpoints
     */
    setupResponsiveHandling() {
        // Create media query listeners for major breakpoints
        const breakpoints = [
            { name: 'mobile', query: '(max-width: 767px)' },
            { name: 'tablet', query: '(min-width: 768px) and (max-width: 1023px)' },
            { name: 'desktop', query: '(min-width: 1024px) and (max-width: 1439px)' },
            { name: 'large-desktop', query: '(min-width: 1440px) and (max-width: 2559px)' },
            { name: 'ultra-wide', query: '(min-width: 2560px)' }
        ];

        breakpoints.forEach(bp => {
            if (window.matchMedia) {
                const mediaQuery = window.matchMedia(bp.query);
                mediaQuery.addEventListener('change', (e) => {
                    if (e.matches) {
                        this.handleBreakpointChange(bp.name);
                    }
                });
            }
        });
    }

    /**
     * Handle breakpoint changes
     */
    handleBreakpointChange(breakpoint) {
        console.log(`📱 Breakpoint changed to: ${breakpoint}`);

        const stadiumLayout = document.getElementById('admin-stadium-flex-layout');
        if (!stadiumLayout) return;

        // Add breakpoint-specific data attribute
        stadiumLayout.setAttribute('data-current-breakpoint', breakpoint);

        // Trigger custom event for other components to listen
        window.dispatchEvent(new CustomEvent('stadiumBreakpointChange', {
            detail: { breakpoint, viewport: { width: window.innerWidth, height: window.innerHeight } }
        }));

        // Optimize interactions based on breakpoint
        this.optimizeForBreakpoint(breakpoint);
    }

    /**
     * Optimize interactions for specific breakpoints
     */
    optimizeForBreakpoint(breakpoint) {
        const isMobile = breakpoint === 'mobile';
        const isUltraWide = breakpoint === 'ultra-wide';

        // Adjust interaction patterns
        if (isMobile) {
            this.enableTouchOptimizations();
        } else {
            this.enableDesktopOptimizations();
        }

        // Adjust hover states
        if (isUltraWide) {
            this.enhanceHoverEffects();
        }
    }

    /**
     * Setup sector interactions for maximum width layout
     */
    setupSectorInteractions() {
        const sectors = document.querySelectorAll('.sector-optimized');

        sectors.forEach(sector => {
            // Enhanced hover effects for wide displays
            sector.addEventListener('mouseenter', this.handleSectorInteraction);
            sector.addEventListener('mouseleave', this.handleSectorInteraction);
            sector.addEventListener('focus', this.handleSectorInteraction);
            sector.addEventListener('blur', this.handleSectorInteraction);

            // Touch interactions for mobile
            sector.addEventListener('touchstart', this.handleSectorInteraction, { passive: true });
            sector.addEventListener('touchend', this.handleSectorInteraction, { passive: true });

            // Add GPU acceleration for smooth animations
            if (this.performanceConfig.enableGPUAcceleration) {
                sector.style.transform = 'translateZ(0)';
                sector.style.backfaceVisibility = 'hidden';
            }
        });
    }

    /**
     * Handle sector interaction events
     */
    handleSectorInteraction(event) {
        const sector = event.currentTarget;
        const sectorId = sector.getAttribute('data-sector');

        switch (event.type) {
            case 'mouseenter':
            case 'focus':
                this.highlightRelatedSectors(sector);
                this.showSectorPreview(sector);
                break;
            case 'mouseleave':
            case 'blur':
                this.clearHighlights();
                this.hideSectorPreview();
                break;
            case 'touchstart':
                this.handleTouchStart(sector);
                break;
            case 'touchend':
                this.handleTouchEnd(sector);
                break;
        }

        // Update ARIA live region for screen readers
        this.updateAriaLiveRegion(sector, event.type);
    }

    /**
     * Highlight related sectors (same stand or similar occupancy)
     */
    highlightRelatedSectors(targetSector) {
        const standId = targetSector.closest('.stadium-stand-optimized')?.getAttribute('data-stand');
        const occupancy = targetSector.getAttribute('data-occupancy');

        // Highlight sectors in same stand
        if (standId) {
            const relatedSectors = document.querySelectorAll(`[data-stand="${standId}"] .sector-optimized`);
            relatedSectors.forEach(sector => {
                sector.classList.add('sector-related-highlighted');
            });
        }

        // Highlight sectors with similar occupancy
        if (occupancy) {
            const similarSectors = document.querySelectorAll(`[data-occupancy="${occupancy}"]`);
            similarSectors.forEach(sector => {
                sector.classList.add('sector-occupancy-highlighted');
            });
        }
    }

    /**
     * Clear all highlights
     */
    clearHighlights() {
        document.querySelectorAll('.sector-related-highlighted, .sector-occupancy-highlighted')
            .forEach(sector => {
                sector.classList.remove('sector-related-highlighted', 'sector-occupancy-highlighted');
            });
    }

    /**
     * Setup keyboard navigation for accessibility
     */
    setupKeyboardNavigation() {
        const stadiumLayout = document.getElementById('admin-stadium-flex-layout');
        if (!stadiumLayout) return;

        stadiumLayout.addEventListener('keydown', this.handleKeyboardNavigation);

        // Set up focus management
        this.setupFocusManagement();
    }

    /**
     * Handle keyboard navigation
     */
    handleKeyboardNavigation(event) {
        const focusedElement = document.activeElement;
        const sectors = Array.from(document.querySelectorAll('.sector-optimized'));
        const currentIndex = sectors.indexOf(focusedElement);

        let nextElement = null;

        switch (event.key) {
            case 'ArrowRight':
            case 'ArrowDown':
                nextElement = sectors[currentIndex + 1] || sectors[0];
                break;
            case 'ArrowLeft':
            case 'ArrowUp':
                nextElement = sectors[currentIndex - 1] || sectors[sectors.length - 1];
                break;
            case 'Home':
                nextElement = sectors[0];
                break;
            case 'End':
                nextElement = sectors[sectors.length - 1];
                break;
            case 'PageDown':
                nextElement = sectors[Math.min(currentIndex + 10, sectors.length - 1)];
                break;
            case 'PageUp':
                nextElement = sectors[Math.max(currentIndex - 10, 0)];
                break;
        }

        if (nextElement) {
            event.preventDefault();
            nextElement.focus();
            this.scrollIntoViewIfNeeded(nextElement);
        }
    }

    /**
     * Setup performance optimizations
     */
    setupPerformanceOptimizations() {
        // Implement intersection observer for lazy loading
        if (window.IntersectionObserver) {
            this.intersectionObserver = new IntersectionObserver(
                this.handleIntersection.bind(this),
                { threshold: this.performanceConfig.lazyLoadThreshold }
            );

            // Observe all sectors for lazy interactions
            document.querySelectorAll('.sector-optimized').forEach(sector => {
                this.intersectionObserver.observe(sector);
            });
        }

        // Setup CPU/GPU performance monitoring
        this.setupPerformanceMonitoring();
    }

    /**
     * Handle intersection events for performance optimization
     */
    handleIntersection(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('sector-in-viewport');
                // Enable full interactions for visible sectors
                this.enableSectorInteractions(entry.target);
            } else {
                entry.target.classList.remove('sector-in-viewport');
                // Reduce interactions for non-visible sectors
                this.reduceSectorInteractions(entry.target);
            }
        });
    }

    /**
     * Setup accessibility enhancements
     */
    setupAccessibilityEnhancements() {
        // Enhance focus indicators for wide screens
        const style = document.createElement('style');
        style.textContent = `
            .sector-optimized:focus {
                outline: 3px solid var(--focus-ring-color, #3b82f6);
                outline-offset: 2px;
                box-shadow: 0 0 0 6px rgba(59, 130, 246, 0.2);
            }

            @media (min-width: 1440px) {
                .sector-optimized:focus {
                    outline-width: 4px;
                    outline-offset: 3px;
                    box-shadow: 0 0 0 8px rgba(59, 130, 246, 0.15);
                }
            }
        `;
        document.head.appendChild(style);

        // Setup ARIA live regions
        this.setupAriaLiveRegions();
    }

    /**
     * Utility methods
     */
    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    throttle(func, limit) {
        let inThrottle;
        return function executedFunction(...args) {
            if (!inThrottle) {
                func.apply(this, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    }

    scrollIntoViewIfNeeded(element) {
        const rect = element.getBoundingClientRect();
        const isVisible = rect.top >= 0 && rect.bottom <= window.innerHeight;

        if (!isVisible) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'center',
                inline: 'nearest'
            });
        }
    }

    updateAriaLiveRegion(sector, eventType) {
        const liveRegion = document.getElementById('stadium-status-updates');
        if (!liveRegion) return;

        const sectorName = sector.querySelector('.sector-name')?.textContent || 'Unknown Sector';
        const occupancy = sector.getAttribute('data-occupancy') || '0';

        let message = '';
        switch (eventType) {
            case 'focus':
            case 'mouseenter':
                message = `Focused on ${sectorName}, ${occupancy}% occupied`;
                break;
        }

        if (message) {
            liveRegion.textContent = message;
        }
    }

    /**
     * Cleanup method
     */
    destroy() {
        if (this.resizeObserver) {
            this.resizeObserver.disconnect();
        }
        if (this.intersectionObserver) {
            this.intersectionObserver.disconnect();
        }

        window.removeEventListener('resize', this.handleResize);
        window.removeEventListener('orientationchange', this.handleResize);

        this.isInitialized = false;
        console.log('🏟️ Stadium Maximum Width Manager destroyed');
    }

    // Placeholder methods for additional functionality
    handleResize() { this.updateViewportClasses(); }
    handleScroll() { /* Implement if needed */ }
    enableTouchOptimizations() { /* Touch-specific optimizations */ }
    enableDesktopOptimizations() { /* Desktop-specific optimizations */ }
    enhanceHoverEffects() { /* Enhanced hover for ultra-wide displays */ }
    showSectorPreview(sector) { /* Show sector preview tooltip */ }
    hideSectorPreview() { /* Hide sector preview tooltip */ }
    handleTouchStart(sector) { /* Touch start handling */ }
    handleTouchEnd(sector) { /* Touch end handling */ }
    setupFocusManagement() { /* Focus management setup */ }
    enableSectorInteractions(sector) { /* Enable full interactions */ }
    reduceSectorInteractions(sector) { /* Reduce interactions for performance */ }
    setupPerformanceMonitoring() { /* Performance monitoring */ }
    setupAriaLiveRegions() { /* ARIA live regions setup */ }
}

// Global stadium manager instance
let stadiumMaxWidthManager = null;

// Initialize when DOM is ready
function initializeStadiumMaximumWidth() {
    if (!stadiumMaxWidthManager) {
        stadiumMaxWidthManager = new StadiumMaximumWidthManager();
        stadiumMaxWidthManager.init();
    }
}

// Auto-initialize if on stadium overview page
if (window.location.pathname.includes('stadium-overview')) {
    initializeStadiumMaximumWidth();
}

// Export for manual initialization
window.StadiumMaximumWidthManager = StadiumMaximumWidthManager;
window.initializeStadiumMaximumWidth = initializeStadiumMaximumWidth;

console.log('🚀 Stadium Maximum Width Interactions script loaded');