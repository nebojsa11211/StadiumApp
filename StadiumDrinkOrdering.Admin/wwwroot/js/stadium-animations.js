// Stadium Animations JavaScript Module
// Advanced interactions and animations for premium stadium interface

(function() {
    'use strict';

    // Animation configuration
    const config = {
        rippleDuration: 600,
        hoverScale: 1.05,
        clickScale: 0.98,
        parallaxIntensity: 20,
        particleCount: 5,
        zoomSpeed: 0.1,
        panSpeed: 2,
        reducedMotion: window.matchMedia('(prefers-reduced-motion: reduce)').matches
    };

    // State management
    const state = {
        currentZoom: 1,
        panX: 0,
        panY: 0,
        isDragging: false,
        dragStartX: 0,
        dragStartY: 0,
        hoveredSector: null,
        selectedSectors: new Set()
    };

    // Initialize animations when DOM is ready
    document.addEventListener('DOMContentLoaded', initializeAnimations);

    function initializeAnimations() {
        if (config.reducedMotion) {
            console.log('Stadium Animations: Reduced motion mode active');
            return;
        }

        // Initialize all animation components
        initializeRippleEffects();
        initializeParallaxEffects();
        initializeSectorAnimations();
        initializeZoomPan();
        initializeLoadingAnimations();
        initializeMicroInteractions();
        initializeDataTransitions();
    }

    // Ripple Effects for Clicks
    function initializeRippleEffects() {
        document.addEventListener('click', function(e) {
            const target = e.target.closest('.premium-btn, .premium-toggle-btn, .premium-action-btn, .stadium-sector');
            if (!target) return;

            const rect = target.getBoundingClientRect();
            const ripple = document.createElement('span');
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;

            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            ripple.classList.add('ripple-effect');

            target.appendChild(ripple);

            setTimeout(() => {
                ripple.remove();
            }, config.rippleDuration);
        });
    }

    // Parallax Effects for Depth
    function initializeParallaxEffects() {
        const parallaxElements = document.querySelectorAll('.parallax-element');
        if (!parallaxElements.length) return;

        document.addEventListener('mousemove', (e) => {
            const centerX = window.innerWidth / 2;
            const centerY = window.innerHeight / 2;
            const moveX = (e.clientX - centerX) / centerX;
            const moveY = (e.clientY - centerY) / centerY;

            parallaxElements.forEach((element, index) => {
                const depth = (index + 1) * config.parallaxIntensity;
                const translateX = moveX * depth;
                const translateY = moveY * depth;

                element.style.transform = `translate3d(${translateX}px, ${translateY}px, 0)`;
            });
        });
    }

    // Sector-specific Animations
    function initializeSectorAnimations() {
        const sectors = document.querySelectorAll('.stadium-sector');

        sectors.forEach((sector, index) => {
            // Add staggered animation delay
            sector.style.setProperty('--sector-index', index);

            // Enhanced hover effects
            sector.addEventListener('mouseenter', function() {
                if (!state.hoveredSector) {
                    state.hoveredSector = this;
                    animateSectorHover(this, true);
                }
            });

            sector.addEventListener('mouseleave', function() {
                if (state.hoveredSector === this) {
                    state.hoveredSector = null;
                    animateSectorHover(this, false);
                }
            });

            // Click animation
            sector.addEventListener('click', function() {
                animateSectorClick(this);
            });
        });
    }

    function animateSectorHover(sector, isEntering) {
        const glowEffect = sector.nextElementSibling?.nextElementSibling;
        if (glowEffect && glowEffect.classList.contains('sector-glow-effect')) {
            if (isEntering) {
                glowEffect.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
                glowEffect.style.opacity = '0.5';
                glowEffect.style.strokeWidth = '3';
            } else {
                glowEffect.style.opacity = '0';
                glowEffect.style.strokeWidth = '0';
            }
        }

        // Scale animation
        sector.style.transform = isEntering ?
            `scale(${config.hoverScale}) translateZ(0)` :
            'scale(1) translateZ(0)';
    }

    function animateSectorClick(sector) {
        // Quick scale down/up animation
        sector.style.transform = `scale(${config.clickScale}) translateZ(0)`;
        setTimeout(() => {
            sector.style.transform = `scale(${config.hoverScale}) translateZ(0)`;
        }, 100);

        // Toggle selection state
        if (state.selectedSectors.has(sector)) {
            state.selectedSectors.delete(sector);
            sector.classList.remove('active');
        } else {
            state.selectedSectors.add(sector);
            sector.classList.add('active');
        }

        // Create particle effect
        createParticleEffect(sector);
    }

    // Particle Effects
    function createParticleEffect(element) {
        const rect = element.getBoundingClientRect();
        const container = document.createElement('div');
        container.className = 'particle-container';
        container.style.position = 'fixed';
        container.style.left = rect.left + 'px';
        container.style.top = rect.top + 'px';
        container.style.width = rect.width + 'px';
        container.style.height = rect.height + 'px';
        container.style.pointerEvents = 'none';
        container.style.zIndex = '9999';

        for (let i = 0; i < config.particleCount; i++) {
            const particle = document.createElement('div');
            particle.className = 'particle';
            particle.style.left = Math.random() * rect.width + 'px';
            particle.style.top = Math.random() * rect.height + 'px';
            particle.style.animationDelay = (i * 0.1) + 's';
            container.appendChild(particle);
        }

        document.body.appendChild(container);

        setTimeout(() => {
            container.remove();
        }, 6000);
    }

    // Zoom and Pan Controls
    function initializeZoomPan() {
        const stadiumContainer = document.querySelector('.stadium-svg-renderer');
        if (!stadiumContainer) return;

        // Add zoom controls
        const zoomControls = createZoomControls();
        stadiumContainer.appendChild(zoomControls);

        // Handle mouse wheel zoom
        stadiumContainer.addEventListener('wheel', (e) => {
            e.preventDefault();
            const delta = e.deltaY > 0 ? -config.zoomSpeed : config.zoomSpeed;
            updateZoom(delta);
        });

        // Handle pan
        initializePan(stadiumContainer);
    }

    function createZoomControls() {
        const controls = document.createElement('div');
        controls.className = 'zoom-controls animated-zoom-controls';
        controls.innerHTML = `
            <button class="zoom-btn zoom-in-btn" title="Zoom In">
                <i class="bi bi-zoom-in"></i>
            </button>
            <button class="zoom-btn zoom-out-btn" title="Zoom Out">
                <i class="bi bi-zoom-out"></i>
            </button>
            <button class="zoom-btn zoom-reset-btn" title="Reset View">
                <i class="bi bi-arrows-fullscreen"></i>
            </button>
        `;

        controls.querySelector('.zoom-in-btn').addEventListener('click', () => updateZoom(0.2));
        controls.querySelector('.zoom-out-btn').addEventListener('click', () => updateZoom(-0.2));
        controls.querySelector('.zoom-reset-btn').addEventListener('click', resetView);

        return controls;
    }

    function updateZoom(delta) {
        state.currentZoom = Math.max(0.5, Math.min(3, state.currentZoom + delta));
        applyTransform();
    }

    function resetView() {
        state.currentZoom = 1;
        state.panX = 0;
        state.panY = 0;
        applyTransform();
    }

    function applyTransform() {
        const svg = document.querySelector('.stadium-svg');
        if (svg) {
            svg.style.transform = `scale(${state.currentZoom}) translate(${state.panX}px, ${state.panY}px)`;
            svg.style.transition = 'transform 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        }
    }

    function initializePan(container) {
        let isPanning = false;
        let startX, startY;

        container.addEventListener('mousedown', (e) => {
            if (e.button === 1 || (e.button === 0 && e.shiftKey)) {
                isPanning = true;
                startX = e.clientX - state.panX;
                startY = e.clientY - state.panY;
                container.style.cursor = 'grabbing';
            }
        });

        document.addEventListener('mousemove', (e) => {
            if (isPanning) {
                state.panX = (e.clientX - startX) / state.currentZoom;
                state.panY = (e.clientY - startY) / state.currentZoom;
                applyTransform();
            }
        });

        document.addEventListener('mouseup', () => {
            if (isPanning) {
                isPanning = false;
                container.style.cursor = '';
            }
        });
    }

    // Loading Animations
    function initializeLoadingAnimations() {
        // Animate skeleton loaders
        const skeletons = document.querySelectorAll('.stadium-loading-skeleton');
        skeletons.forEach(skeleton => {
            skeleton.classList.add('animated');
        });

        // Progress bars
        const progressBars = document.querySelectorAll('.occupancy-fill, .capacity-fill');
        progressBars.forEach(bar => {
            const width = bar.style.width;
            bar.style.width = '0';
            setTimeout(() => {
                bar.style.transition = 'width 1.5s cubic-bezier(0.4, 0, 0.2, 1)';
                bar.style.width = width;
            }, 100);
        });
    }

    // Micro-interactions
    function initializeMicroInteractions() {
        // Button magnetic effect
        const magneticButtons = document.querySelectorAll('.magnetic-btn');
        magneticButtons.forEach(button => {
            button.addEventListener('mousemove', (e) => {
                const rect = button.getBoundingClientRect();
                const x = e.clientX - rect.left - rect.width / 2;
                const y = e.clientY - rect.top - rect.height / 2;

                button.style.transform = `translate(${x * 0.1}px, ${y * 0.1}px)`;
            });

            button.addEventListener('mouseleave', () => {
                button.style.transform = '';
            });
        });

        // Input focus effects
        const inputs = document.querySelectorAll('.premium-input, .premium-select');
        inputs.forEach(input => {
            input.addEventListener('focus', () => {
                input.parentElement?.classList.add('focused');
            });

            input.addEventListener('blur', () => {
                input.parentElement?.classList.remove('focused');
            });
        });
    }

    // Data Transition Animations
    function initializeDataTransitions() {
        // Observe data changes and animate
        const observer = new MutationObserver((mutations) => {
            mutations.forEach(mutation => {
                if (mutation.type === 'childList') {
                    mutation.addedNodes.forEach(node => {
                        if (node.nodeType === 1) {
                            animateNewElement(node);
                        }
                    });
                }
            });
        });

        const dataContainers = document.querySelectorAll('.overview-cards, .detailed-stats, .availability-breakdown');
        dataContainers.forEach(container => {
            observer.observe(container, { childList: true, subtree: true });
        });
    }

    function animateNewElement(element) {
        element.style.opacity = '0';
        element.style.transform = 'translateY(20px)';

        requestAnimationFrame(() => {
            element.style.transition = 'all 0.5s cubic-bezier(0.4, 0, 0.2, 1)';
            element.style.opacity = '1';
            element.style.transform = 'translateY(0)';
        });
    }

    // Public API
    window.StadiumAnimations = {
        config,
        state,
        updateZoom,
        resetView,
        createParticleEffect,
        animateSectorClick,
        initialize: initializeAnimations
    };

})();

// Auto-initialize if stadium container exists
if (document.querySelector('.stadium-overview-container')) {
    window.addEventListener('load', () => {
        if (window.StadiumAnimations) {
            window.StadiumAnimations.initialize();
        }
    });
}