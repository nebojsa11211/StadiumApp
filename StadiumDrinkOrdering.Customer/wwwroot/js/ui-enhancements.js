// UI Enhancements JavaScript - Stadium Drink Ordering Customer App
// Handles button ripple effects, smooth scrolling, and mobile interactions

(function() {
    'use strict';

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    function init() {
        initButtonRipples();
        initSmoothScroll();
        initLazyLoading();
        initAnimateOnScroll();
        initSkipToContent();
    }

    // Button Ripple Effect
    function initButtonRipples() {
        document.addEventListener('click', function(e) {
            const target = e.target.closest('.btn-ripple, .btn-animated');
            if (!target) return;

            const rect = target.getBoundingClientRect();
            const ripple = document.createElement('span');
            ripple.classList.add('ripple');

            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;

            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';

            target.appendChild(ripple);

            setTimeout(() => ripple.remove(), 600);
        });
    }

    // Smooth Scroll for Anchor Links
    function initSmoothScroll() {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function(e) {
                const href = this.getAttribute('href');
                if (href === '#' || href === '#!') return;

                const targetEl = document.querySelector(href);
                if (targetEl) {
                    e.preventDefault();
                    targetEl.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            });
        });
    }

    // Lazy Loading for Images
    function initLazyLoading() {
        if ('IntersectionObserver' in window) {
            const imageObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        if (img.dataset.src) {
                            img.src = img.dataset.src;
                            img.classList.add('loaded');
                            observer.unobserve(img);
                        }
                    }
                });
            });

            document.querySelectorAll('img[data-src]').forEach(img => {
                imageObserver.observe(img);
            });
        }
    }

    // Animate on Scroll
    function initAnimateOnScroll() {
        if ('IntersectionObserver' in window) {
            const animateObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('animate-visible');
                    }
                });
            }, {
                threshold: 0.1,
                rootMargin: '0px 0px -50px 0px'
            });

            // Observe elements that should animate on scroll
            document.querySelectorAll('.fade-in, .slide-up, .feature-card, .match-fixture-card').forEach(el => {
                animateObserver.observe(el);
            });
        }
    }

    // Skip to Content Link (Accessibility)
    function initSkipToContent() {
        const skipLink = document.querySelector('.skip-to-content');
        if (skipLink) {
            skipLink.addEventListener('click', function(e) {
                e.preventDefault();
                const mainContent = document.querySelector('main') || document.querySelector('[role="main"]');
                if (mainContent) {
                    mainContent.focus();
                    mainContent.scrollIntoView({ behavior: 'smooth' });
                }
            });
        }
    }

    // Export utility functions for Blazor components
    window.StadiumUI = {
        // Show toast notification (called from Blazor)
        showToast: function(message, type, duration) {
            console.log(`Toast: ${type} - ${message}`);
            // Toast handling is done via Blazor ToastNotification component
        },

        // Trigger confetti animation (success celebration)
        showConfetti: function() {
            // Simple confetti effect using CSS animation
            const confettiContainer = document.createElement('div');
            confettiContainer.className = 'confetti-container';
            document.body.appendChild(confettiContainer);

            for (let i = 0; i < 50; i++) {
                const confetti = document.createElement('div');
                confetti.className = 'confetti';
                confetti.style.left = Math.random() * 100 + '%';
                confetti.style.animationDelay = Math.random() * 3 + 's';
                confetti.style.backgroundColor = ['#10B981', '#FBBF24', '#0047BB', '#EF4444'][Math.floor(Math.random() * 4)];
                confettiContainer.appendChild(confetti);
            }

            setTimeout(() => confettiContainer.remove(), 5000);
        },

        // Add loading overlay to element
        addLoadingOverlay: function(elementId) {
            const element = document.getElementById(elementId);
            if (!element) return;

            const overlay = document.createElement('div');
            overlay.className = 'loading-overlay';
            overlay.innerHTML = `
                <div class="loading-spinner-wrapper">
                    <div class="stadium-loader">
                        <div class="stadium-loader-inner"></div>
                        <div class="stadium-ball">⚽</div>
                        <div class="stadium-loader-track"></div>
                    </div>
                    <p class="loading-message">Loading...</p>
                </div>
            `;
            element.style.position = 'relative';
            element.appendChild(overlay);
        },

        // Remove loading overlay
        removeLoadingOverlay: function(elementId) {
            const element = document.getElementById(elementId);
            if (!element) return;

            const overlay = element.querySelector('.loading-overlay');
            if (overlay) {
                overlay.remove();
            }
        },

        // Focus trap for modals (accessibility)
        trapFocus: function(elementId) {
            const modal = document.getElementById(elementId);
            if (!modal) return;

            const focusableElements = modal.querySelectorAll(
                'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
            );

            if (focusableElements.length === 0) return;

            const firstElement = focusableElements[0];
            const lastElement = focusableElements[focusableElements.length - 1];

            modal.addEventListener('keydown', function(e) {
                if (e.key !== 'Tab') return;

                if (e.shiftKey) {
                    if (document.activeElement === firstElement) {
                        lastElement.focus();
                        e.preventDefault();
                    }
                } else {
                    if (document.activeElement === lastElement) {
                        firstElement.focus();
                        e.preventDefault();
                    }
                }
            });

            firstElement.focus();
        }
    };

    // Initialize on Blazor navigation
    if (window.Blazor) {
        Blazor.addEventListener('enhancedload', () => {
            init();
        });
    }
})();

// CSS Animation Classes
const style = document.createElement('style');
style.textContent = `
    /* Confetti Animation */
    .confetti-container {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        pointer-events: none;
        z-index: 10000;
        overflow: hidden;
    }

    .confetti {
        position: absolute;
        width: 10px;
        height: 10px;
        top: -10px;
        opacity: 0;
        animation: confetti-fall 3s linear forwards;
    }

    @keyframes confetti-fall {
        0% {
            transform: translateY(0) rotate(0deg);
            opacity: 1;
        }
        100% {
            transform: translateY(100vh) rotate(720deg);
            opacity: 0;
        }
    }

    /* Animate on Scroll */
    .fade-in:not(.animate-visible),
    .slide-up:not(.animate-visible) {
        opacity: 0;
    }

    .fade-in.animate-visible {
        animation: fadeIn 0.6s ease-out forwards;
    }

    .slide-up.animate-visible {
        animation: slideUp 0.6s ease-out forwards;
    }

    /* Focus Visible */
    *:focus-visible {
        outline: 3px solid #10B981;
        outline-offset: 2px;
        border-radius: 4px;
    }
`;
document.head.appendChild(style);
