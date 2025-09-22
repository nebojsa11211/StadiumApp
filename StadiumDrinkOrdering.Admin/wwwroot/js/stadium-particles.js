// Stadium Particle System
// Premium particle effects for enhanced visual appeal

(function() {
    'use strict';

    // Particle system configuration
    const config = {
        maxParticles: 50,
        particleSize: { min: 2, max: 6 },
        particleSpeed: { min: 0.5, max: 2 },
        particleLife: { min: 3000, max: 8000 },
        colors: [
            'rgba(37, 99, 235, 0.6)',   // Primary blue
            'rgba(16, 185, 129, 0.6)',  // Secondary green
            'rgba(245, 158, 11, 0.6)',  // Accent orange
            'rgba(139, 92, 246, 0.6)',  // Purple
            'rgba(236, 72, 153, 0.6)'   // Pink
        ],
        enabledRoutes: ['/admin/stadium-overview'],
        reducedMotion: window.matchMedia('(prefers-reduced-motion: reduce)').matches
    };

    // Particle class
    class Particle {
        constructor(container) {
            this.container = container;
            this.reset();
            this.createElement();
        }

        reset() {
            const containerRect = this.container.getBoundingClientRect();
            this.x = Math.random() * containerRect.width;
            this.y = containerRect.height + 20;
            this.targetY = -20;
            this.size = config.particleSize.min + Math.random() * (config.particleSize.max - config.particleSize.min);
            this.speed = config.particleSpeed.min + Math.random() * (config.particleSpeed.max - config.particleSpeed.min);
            this.color = config.colors[Math.floor(Math.random() * config.colors.length)];
            this.life = config.particleLife.min + Math.random() * (config.particleLife.max - config.particleLife.min);
            this.opacity = 0.8;
            this.drift = (Math.random() - 0.5) * 0.5;
            this.birthTime = Date.now();
        }

        createElement() {
            this.element = document.createElement('div');
            this.element.className = 'stadium-particle';
            this.element.style.cssText = `
                position: absolute;
                width: ${this.size}px;
                height: ${this.size}px;
                background: ${this.color};
                border-radius: 50%;
                pointer-events: none;
                z-index: 1;
                box-shadow: 0 0 ${this.size * 2}px ${this.color};
                transform: translateZ(0);
                will-change: transform, opacity;
            `;
            this.container.appendChild(this.element);
        }

        update() {
            const elapsed = Date.now() - this.birthTime;
            const progress = elapsed / this.life;

            if (progress >= 1) {
                return false; // Particle is dead
            }

            // Update position
            this.y -= this.speed;
            this.x += this.drift;

            // Update opacity (fade out near end of life)
            if (progress > 0.7) {
                this.opacity = 0.8 * (1 - (progress - 0.7) / 0.3);
            }

            // Apply to element
            this.element.style.transform = `translate3d(${this.x}px, ${this.y}px, 0)`;
            this.element.style.opacity = this.opacity;

            return true; // Particle is alive
        }

        destroy() {
            if (this.element && this.element.parentNode) {
                this.element.parentNode.removeChild(this.element);
            }
        }
    }

    // Particle system manager
    class ParticleSystem {
        constructor() {
            this.particles = [];
            this.container = null;
            this.animationId = null;
            this.isActive = false;
            this.lastSpawn = 0;
            this.spawnRate = 200; // milliseconds between spawns
        }

        init() {
            if (config.reducedMotion) {
                console.log('Particle system disabled due to reduced motion preference');
                return;
            }

            this.createContainer();
            this.start();
        }

        createContainer() {
            // Check if we're on a supported route
            const currentPath = window.location.pathname;
            const isSupported = config.enabledRoutes.some(route => currentPath.includes(route));

            if (!isSupported) {
                return;
            }

            // Find the stadium overview container
            const stadiumContainer = document.querySelector('.stadium-overview-container, .stadium-viewer-container');
            if (!stadiumContainer) {
                console.log('Stadium container not found, particle system not initialized');
                return;
            }

            // Create particle container
            this.container = document.createElement('div');
            this.container.className = 'particle-system-container';
            this.container.style.cssText = `
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                pointer-events: none;
                overflow: hidden;
                z-index: 0;
            `;

            stadiumContainer.style.position = 'relative';
            stadiumContainer.appendChild(this.container);

            console.log('Particle system container created');
        }

        start() {
            if (!this.container || this.isActive) {
                return;
            }

            this.isActive = true;
            this.animate();
            console.log('Particle system started');
        }

        stop() {
            if (!this.isActive) {
                return;
            }

            this.isActive = false;
            if (this.animationId) {
                cancelAnimationFrame(this.animationId);
            }

            // Clean up particles
            this.particles.forEach(particle => particle.destroy());
            this.particles = [];

            console.log('Particle system stopped');
        }

        spawnParticle() {
            if (this.particles.length >= config.maxParticles) {
                return;
            }

            const particle = new Particle(this.container);
            this.particles.push(particle);
        }

        animate() {
            if (!this.isActive) {
                return;
            }

            const now = Date.now();

            // Spawn new particles
            if (now - this.lastSpawn > this.spawnRate) {
                this.spawnParticle();
                this.lastSpawn = now;
            }

            // Update existing particles
            this.particles = this.particles.filter(particle => {
                const isAlive = particle.update();
                if (!isAlive) {
                    particle.destroy();
                }
                return isAlive;
            });

            this.animationId = requestAnimationFrame(() => this.animate());
        }

        destroy() {
            this.stop();
            if (this.container && this.container.parentNode) {
                this.container.parentNode.removeChild(this.container);
            }
            this.container = null;
        }
    }

    // Progressive loading bar
    class ProgressBar {
        constructor() {
            this.element = null;
            this.isActive = false;
        }

        create() {
            this.element = document.createElement('div');
            this.element.className = 'stadium-progress-bar';
            this.element.style.cssText = `
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 3px;
                background: linear-gradient(90deg,
                    var(--premium-primary),
                    var(--premium-secondary),
                    var(--premium-primary));
                background-size: 200% 100%;
                transform: translateX(-100%);
                transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                z-index: 10000;
                box-shadow: 0 0 10px rgba(37, 99, 235, 0.5);
            `;

            document.body.appendChild(this.element);
        }

        show() {
            if (!this.element) {
                this.create();
            }

            this.isActive = true;
            this.element.style.transform = 'translateX(0)';
            this.element.style.animation = 'progressMove 1.5s ease-in-out infinite';
        }

        hide() {
            if (!this.element || !this.isActive) {
                return;
            }

            this.isActive = false;
            this.element.style.transform = 'translateX(100%)';

            setTimeout(() => {
                if (this.element && this.element.parentNode) {
                    this.element.parentNode.removeChild(this.element);
                    this.element = null;
                }
            }, 300);
        }
    }

    // Global instances
    let particleSystem = new ParticleSystem();
    let progressBar = new ProgressBar();

    // Initialization
    function init() {
        // Initialize particle system when stadium container is available
        const checkForContainer = () => {
            const stadiumContainer = document.querySelector('.stadium-overview-container, .stadium-viewer-container');
            if (stadiumContainer) {
                particleSystem.init();
            } else {
                setTimeout(checkForContainer, 500);
            }
        };

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', checkForContainer);
        } else {
            checkForContainer();
        }
    }

    // Show progress bar on page navigation
    function showProgress() {
        progressBar.show();
    }

    function hideProgress() {
        setTimeout(() => {
            progressBar.hide();
        }, 500);
    }

    // Enhanced loading detection
    function setupLoadingDetection() {
        // Show progress on Blazor navigation
        if (window.Blazor) {
            window.Blazor.addEventListener('beforestart', showProgress);
            window.Blazor.addEventListener('start', hideProgress);
        }

        // Show progress on form submissions and AJAX requests
        document.addEventListener('submit', showProgress);

        // Intercept fetch requests
        const originalFetch = window.fetch;
        window.fetch = function(...args) {
            showProgress();
            return originalFetch.apply(this, args).finally(() => {
                hideProgress();
            });
        };

        // Intercept XMLHttpRequest
        const originalXHROpen = XMLHttpRequest.prototype.open;
        XMLHttpRequest.prototype.open = function(...args) {
            showProgress();
            this.addEventListener('loadend', hideProgress);
            return originalXHROpen.apply(this, args);
        };
    }

    // Viewport-based particle density
    function adjustParticleConfig() {
        const viewport = {
            width: window.innerWidth,
            height: window.innerHeight
        };

        // Reduce particles on smaller screens
        if (viewport.width < 768) {
            config.maxParticles = 20;
            config.spawnRate = 400;
        } else if (viewport.width < 1200) {
            config.maxParticles = 35;
            config.spawnRate = 300;
        }

        // Adjust spawn rate based on performance
        if (particleSystem && particleSystem.isActive) {
            particleSystem.spawnRate = config.spawnRate;
        }
    }

    // Performance monitoring
    let lastFrameTime = performance.now();
    let frameCount = 0;
    let fps = 60;

    function monitorPerformance() {
        const now = performance.now();
        frameCount++;

        if (now - lastFrameTime >= 1000) {
            fps = frameCount;
            frameCount = 0;
            lastFrameTime = now;

            // Adjust particle system based on FPS
            if (fps < 30 && config.maxParticles > 20) {
                config.maxParticles = Math.max(20, config.maxParticles - 5);
                console.log('Reducing particles due to low FPS:', fps);
            } else if (fps > 50 && config.maxParticles < 50) {
                config.maxParticles = Math.min(50, config.maxParticles + 5);
            }
        }

        requestAnimationFrame(monitorPerformance);
    }

    // Event listeners
    window.addEventListener('resize', adjustParticleConfig);
    window.addEventListener('beforeunload', () => {
        particleSystem.destroy();
        progressBar.hide();
    });

    // Blazor-specific events
    document.addEventListener('DOMContentLoaded', () => {
        setupLoadingDetection();
        adjustParticleConfig();

        // Start performance monitoring in development
        if (window.location.hostname === 'localhost') {
            monitorPerformance();
        }
    });

    // Public API
    window.StadiumParticles = {
        config,
        particleSystem,
        progressBar,
        init,
        showProgress,
        hideProgress,
        start: () => particleSystem.start(),
        stop: () => particleSystem.stop(),
        destroy: () => particleSystem.destroy()
    };

    // Auto-initialize
    init();

})();

// CSS for progress bar animation (inject dynamically)
if (!document.getElementById('progress-bar-styles')) {
    const style = document.createElement('style');
    style.id = 'progress-bar-styles';
    style.textContent = `
        @keyframes progressMove {
            0% {
                background-position: 0% 50%;
            }
            100% {
                background-position: 100% 50%;
            }
        }
    `;
    document.head.appendChild(style);
}