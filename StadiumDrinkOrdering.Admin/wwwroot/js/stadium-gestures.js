/* ========================================
   Stadium Overview - Mobile Gesture Support
   Touch, Pan, Zoom, and Responsive Interactions
   ======================================== */

// Global variables for gesture management
let stadiumGestureState = {
    isInitialized: false,
    svgElement: null,
    originalTransform: null,
    currentZoom: 1.0,
    translateX: 0,
    translateY: 0,
    lastPinchDistance: 0,
    orientationChangeCallback: null,
    visibilityChangeCallback: null,
    resizeObserver: null
};

/**
 * Initialize stadium gesture support
 * @param {HTMLElement} containerElement - The SVG container element
 */
window.initializeStadiumGestures = function(containerElement) {
    try {
        if (!containerElement) {
            console.warn('Stadium gestures: Container element not found');
            return;
        }

        stadiumGestureState.svgElement = containerElement.querySelector('svg') || containerElement;
        stadiumGestureState.isInitialized = true;

        // Store original transform
        if (stadiumGestureState.svgElement) {
            stadiumGestureState.originalTransform = window.getComputedStyle(stadiumGestureState.svgElement).transform;
        }

        // Initialize touch support
        initializeTouchSupport(containerElement);

        // Initialize pinch zoom support
        initializePinchZoom(containerElement);

        // Initialize responsive behavior
        initializeResponsiveBehavior(containerElement);

        // Setup viewport meta tag for proper mobile scaling
        setupViewportMeta();

        console.log('Stadium gestures initialized successfully');

    } catch (error) {
        console.error('Error initializing stadium gestures:', error);
    }
};

/**
 * Setup viewport meta tag for proper mobile scaling
 */
function setupViewportMeta() {
    let viewport = document.querySelector('meta[name="viewport"]');
    if (!viewport) {
        viewport = document.createElement('meta');
        viewport.name = 'viewport';
        document.head.appendChild(viewport);
    }

    // Optimize for stadium viewing
    viewport.content = 'width=device-width, initial-scale=1.0, maximum-scale=3.0, minimum-scale=0.5, user-scalable=yes, viewport-fit=cover';
}

/**
 * Initialize touch support for panning
 * @param {HTMLElement} containerElement - The container element
 */
function initializeTouchSupport(containerElement) {
    if (!('ontouchstart' in window)) {
        return; // Not a touch device
    }

    let touchStartTime = 0;
    let lastTouchPosition = null;
    let touchMoved = false;

    // Prevent default touch behaviors that interfere with our gestures
    containerElement.addEventListener('touchstart', function(e) {
        touchStartTime = Date.now();
        touchMoved = false;

        if (e.touches.length === 1) {
            lastTouchPosition = {
                x: e.touches[0].clientX,
                y: e.touches[0].clientY
            };
        }
    }, { passive: false });

    containerElement.addEventListener('touchmove', function(e) {
        touchMoved = true;

        // Prevent page scrolling during pan gestures
        if (e.touches.length === 1) {
            e.preventDefault();
        }
    }, { passive: false });

    containerElement.addEventListener('touchend', function(e) {
        const touchDuration = Date.now() - touchStartTime;

        // Handle quick tap gestures (less than 200ms without movement)
        if (touchDuration < 200 && !touchMoved && e.changedTouches.length === 1) {
            handleQuickTap(e.changedTouches[0], containerElement);
        }
    }, { passive: true });
}

/**
 * Handle quick tap gestures for mobile interaction
 * @param {Touch} touch - The touch object
 * @param {HTMLElement} containerElement - The container element
 */
function handleQuickTap(touch, containerElement) {
    const rect = containerElement.getBoundingClientRect();
    const x = touch.clientX - rect.left;
    const y = touch.clientY - rect.top;

    // Check if tap is in the legend area (bottom 20% of container)
    const legendAreaStart = rect.height * 0.8;

    if (y > legendAreaStart) {
        // Tap in legend area - let normal click handlers process
        return;
    }

    // For taps in stadium area, enhance touch feedback
    provideTouchFeedback(touch.clientX, touch.clientY);
}

/**
 * Provide visual feedback for touch interactions
 * @param {number} x - Touch X coordinate
 * @param {number} y - Touch Y coordinate
 */
function provideTouchFeedback(x, y) {
    const feedback = document.createElement('div');
    feedback.style.cssText = `
        position: fixed;
        left: ${x - 15}px;
        top: ${y - 15}px;
        width: 30px;
        height: 30px;
        border-radius: 50%;
        background: rgba(59, 130, 246, 0.3);
        border: 2px solid rgba(59, 130, 246, 0.6);
        pointer-events: none;
        z-index: 9999;
        animation: touchFeedback 0.3s ease-out forwards;
    `;

    // Add CSS animation
    if (!document.getElementById('touch-feedback-styles')) {
        const style = document.createElement('style');
        style.id = 'touch-feedback-styles';
        style.textContent = `
            @keyframes touchFeedback {
                0% {
                    transform: scale(0.5);
                    opacity: 1;
                }
                100% {
                    transform: scale(1.5);
                    opacity: 0;
                }
            }
        `;
        document.head.appendChild(style);
    }

    document.body.appendChild(feedback);

    setTimeout(() => {
        if (feedback.parentNode) {
            feedback.parentNode.removeChild(feedback);
        }
    }, 300);
}

/**
 * Initialize pinch zoom gesture support
 * @param {HTMLElement} containerElement - The container element
 */
function initializePinchZoom(containerElement) {
    if (!('ontouchstart' in window)) {
        return;
    }

    let initialDistance = 0;
    let initialZoom = 1.0;

    containerElement.addEventListener('touchstart', function(e) {
        if (e.touches.length === 2) {
            initialDistance = getDistance(e.touches[0], e.touches[1]);
            initialZoom = stadiumGestureState.currentZoom;
        }
    }, { passive: true });

    containerElement.addEventListener('touchmove', function(e) {
        if (e.touches.length === 2) {
            e.preventDefault();

            const currentDistance = getDistance(e.touches[0], e.touches[1]);
            const zoomFactor = currentDistance / initialDistance;
            const newZoom = Math.max(0.5, Math.min(3.0, initialZoom * zoomFactor));

            // Apply zoom transformation
            applyStadiumTransform(containerElement, newZoom,
                stadiumGestureState.translateX, stadiumGestureState.translateY);
        }
    }, { passive: false });
}

/**
 * Calculate distance between two touch points
 * @param {Touch} touch1 - First touch point
 * @param {Touch} touch2 - Second touch point
 * @returns {number} Distance between touch points
 */
function getDistance(touch1, touch2) {
    const dx = touch1.clientX - touch2.clientX;
    const dy = touch1.clientY - touch2.clientY;
    return Math.sqrt(dx * dx + dy * dy);
}

/**
 * Initialize responsive behavior monitoring
 * @param {HTMLElement} containerElement - The container element
 */
function initializeResponsiveBehavior(containerElement) {
    // Monitor container size changes
    if (window.ResizeObserver) {
        stadiumGestureState.resizeObserver = new ResizeObserver(entries => {
            handleContainerResize(entries[0], containerElement);
        });

        stadiumGestureState.resizeObserver.observe(containerElement);
    }

    // Monitor window resize
    window.addEventListener('resize', debounce(() => {
        handleWindowResize(containerElement);
    }, 250));
}

/**
 * Handle container resize events
 * @param {ResizeObserverEntry} entry - Resize observer entry
 * @param {HTMLElement} containerElement - The container element
 */
function handleContainerResize(entry, containerElement) {
    const { width, height } = entry.contentRect;

    // Adjust zoom constraints based on container size
    if (width < 400) {
        // Small container - limit zoom
        if (stadiumGestureState.currentZoom > 2.0) {
            applyStadiumTransform(containerElement, 2.0,
                stadiumGestureState.translateX, stadiumGestureState.translateY);
        }
    }

    // Reset translation if it's outside bounds
    const maxTranslate = Math.min(width, height) * 0.3;
    const constrainedX = Math.max(-maxTranslate, Math.min(maxTranslate, stadiumGestureState.translateX));
    const constrainedY = Math.max(-maxTranslate, Math.min(maxTranslate, stadiumGestureState.translateY));

    if (constrainedX !== stadiumGestureState.translateX || constrainedY !== stadiumGestureState.translateY) {
        applyStadiumTransform(containerElement, stadiumGestureState.currentZoom,
            constrainedX, constrainedY);
    }
}

/**
 * Handle window resize events
 * @param {HTMLElement} containerElement - The container element
 */
function handleWindowResize(containerElement) {
    // Reset transformation on significant window size changes
    const isMobile = window.innerWidth < 768;
    const isTablet = window.innerWidth >= 768 && window.innerWidth < 1024;

    if (isMobile && stadiumGestureState.currentZoom > 2.0) {
        // Reset zoom on mobile for better UX
        applyStadiumTransform(containerElement, 1.0, 0, 0);
    }
}

/**
 * Apply transformation to the stadium SVG
 * @param {HTMLElement} containerElement - The container element
 * @param {number} zoom - Zoom level
 * @param {number} translateX - X translation
 * @param {number} translateY - Y translation
 */
window.applyStadiumTransform = function(containerElement, zoom, translateX, translateY) {
    try {
        if (!containerElement) {
            console.warn('applyStadiumTransform: Container element not found');
            return;
        }

        const svgElement = containerElement.querySelector('svg') ||
                          containerElement.querySelector('.stadium-svg') ||
                          containerElement;

        if (!svgElement) {
            console.warn('applyStadiumTransform: SVG element not found');
            return;
        }

        // Update state
        stadiumGestureState.currentZoom = zoom;
        stadiumGestureState.translateX = translateX;
        stadiumGestureState.translateY = translateY;

        // Apply transform with smooth transition
        const transform = `translate(${translateX}px, ${translateY}px) scale(${zoom})`;

        svgElement.style.transition = 'transform 0.1s ease-out';
        svgElement.style.transform = transform;
        svgElement.style.transformOrigin = 'center center';

        // Remove transition after animation
        setTimeout(() => {
            if (svgElement.style) {
                svgElement.style.transition = '';
            }
        }, 100);

    } catch (error) {
        console.error('Error applying stadium transform:', error);
    }
};

/**
 * Setup orientation change listener
 * @param {object} dotNetReference - .NET object reference for callbacks
 */
window.setupOrientationChangeListener = function(dotNetReference) {
    try {
        stadiumGestureState.orientationChangeCallback = dotNetReference;

        // Listen for orientation changes
        window.addEventListener('orientationchange', debounce(() => {
            setTimeout(() => {
                if (dotNetReference && dotNetReference.invokeMethodAsync) {
                    dotNetReference.invokeMethodAsync('OnOrientationChanged')
                        .catch(error => console.warn('Error calling OnOrientationChanged:', error));
                }
            }, 300); // Wait for orientation animation
        }, 100));

        // Listen for visibility changes
        document.addEventListener('visibilitychange', () => {
            if (dotNetReference && dotNetReference.invokeMethodAsync) {
                dotNetReference.invokeMethodAsync('OnVisibilityChanged', !document.hidden)
                    .catch(error => console.warn('Error calling OnVisibilityChanged:', error));
            }
        });

    } catch (error) {
        console.error('Error setting up orientation listener:', error);
    }
};

/**
 * Check if device has touch support
 * @returns {boolean} True if touch is supported
 */
window.isTouchDevice = function() {
    return ('ontouchstart' in window) ||
           (navigator.maxTouchPoints > 0) ||
           (navigator.msMaxTouchPoints > 0);
};

/**
 * Get viewport dimensions
 * @returns {number[]} Array of [width, height]
 */
window.getViewportDimensions = function() {
    return [
        window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth,
        window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight
    ];
};

/**
 * Cleanup gesture event listeners
 */
window.cleanupStadiumGestures = function() {
    try {
        if (stadiumGestureState.resizeObserver) {
            stadiumGestureState.resizeObserver.disconnect();
            stadiumGestureState.resizeObserver = null;
        }

        // Reset state
        stadiumGestureState = {
            isInitialized: false,
            svgElement: null,
            originalTransform: null,
            currentZoom: 1.0,
            translateX: 0,
            translateY: 0,
            lastPinchDistance: 0,
            orientationChangeCallback: null,
            visibilityChangeCallback: null,
            resizeObserver: null
        };

        console.log('Stadium gestures cleaned up');

    } catch (error) {
        console.error('Error cleaning up stadium gestures:', error);
    }
};

/**
 * Utility function to debounce frequent events
 * @param {Function} func - Function to debounce
 * @param {number} wait - Wait time in milliseconds
 * @returns {Function} Debounced function
 */
function debounce(func, wait) {
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

/**
 * Enhanced stadium sector highlighting for mobile
 * @param {string} sectorId - Sector ID to highlight
 */
window.highlightStadiumSector = function(sectorId) {
    try {
        // Remove previous highlights
        document.querySelectorAll('.sector-highlight').forEach(el => {
            el.classList.remove('sector-highlight');
        });

        // Add highlight to specified sector
        const sectorElement = document.querySelector(`[data-sector-id="${sectorId}"]`) ||
                             document.querySelector(`#sector-${sectorId}`);

        if (sectorElement) {
            sectorElement.classList.add('sector-highlight');

            // Scroll sector into view on mobile
            if (window.innerWidth < 768) {
                sectorElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'center',
                    inline: 'center'
                });
            }
        }

    } catch (error) {
        console.error('Error highlighting stadium sector:', error);
    }
};

/**
 * Performance optimization for mobile devices
 */
function optimizeForMobile() {
    if (window.innerWidth < 768) {
        // Reduce animation complexity on mobile
        document.body.classList.add('mobile-optimized');

        // Add mobile optimization styles if not present
        if (!document.getElementById('mobile-optimization-styles')) {
            const style = document.createElement('style');
            style.id = 'mobile-optimization-styles';
            style.textContent = `
                .mobile-optimized .stadium-svg * {
                    will-change: auto;
                }
                .mobile-optimized .sector-group {
                    transition: transform 0.1s ease-out !important;
                }
                .mobile-optimized .legend-panel {
                    backdrop-filter: none;
                }
            `;
            document.head.appendChild(style);
        }
    } else {
        document.body.classList.remove('mobile-optimized');
    }
}

// Initialize mobile optimizations when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', optimizeForMobile);
} else {
    optimizeForMobile();
}

// Re-optimize on window resize
window.addEventListener('resize', debounce(optimizeForMobile, 250));