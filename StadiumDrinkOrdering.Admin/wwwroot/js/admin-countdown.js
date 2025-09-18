/*
 * Admin Countdown Circle Manager
 * Handles visual countdown animation for auto-refresh indicators
 * Version: 1.0.0
 */

window.adminCountdown = (function() {
    'use strict';

    let countdownElement = null;
    let progressCircle = null;
    let isInitialized = false;

    // Initialize countdown circle
    function init() {
        if (isInitialized) return;

        // Find countdown circle element
        countdownElement = document.querySelector('.admin-countdown-circle');
        if (countdownElement) {
            progressCircle = countdownElement.querySelector('.admin-countdown-circle-progress');
            isInitialized = true;
            console.log('✅ Admin countdown circle initialized');
        }
    }

    // Start countdown animation
    function start() {
        init();
        if (!countdownElement) return;

        countdownElement.classList.remove('countdown-paused', 'countdown-reset');
        countdownElement.classList.add('countdown-active');

        if (progressCircle) {
            progressCircle.style.animationPlayState = 'running';
        }
    }

    // Pause countdown animation
    function pause() {
        if (!countdownElement) return;

        countdownElement.classList.remove('countdown-active');
        countdownElement.classList.add('countdown-paused');

        if (progressCircle) {
            progressCircle.style.animationPlayState = 'paused';
        }
    }

    // Restart countdown animation
    function restart() {
        if (!countdownElement) return;

        countdownElement.classList.remove('countdown-paused', 'countdown-active');
        countdownElement.classList.add('countdown-reset');

        // Force reflow to apply reset
        countdownElement.offsetHeight;

        // Start animation
        setTimeout(() => {
            countdownElement.classList.remove('countdown-reset');
            countdownElement.classList.add('countdown-active');

            if (progressCircle) {
                progressCircle.style.animationPlayState = 'running';
            }
        }, 50);
    }

    // Update countdown state based on remaining time
    function updateState(remainingSeconds) {
        if (!countdownElement) return;

        countdownElement.classList.remove('countdown-warning', 'countdown-danger', 'countdown-success');

        if (remainingSeconds <= 5) {
            countdownElement.classList.add('countdown-danger');
        } else if (remainingSeconds <= 10) {
            countdownElement.classList.add('countdown-warning');
        } else {
            countdownElement.classList.add('countdown-success');
        }

        // Update tooltip
        const tooltip = `Auto-refresh in ${remainingSeconds} seconds`;
        countdownElement.setAttribute('title', tooltip);
    }

    // Smooth progress update without animation conflicts
    function updateProgress(remainingSeconds, totalSeconds) {
        if (!progressCircle) return;

        const progress = remainingSeconds / totalSeconds;
        const dashOffset = 94.25 * (1 - progress); // Circumference calculation

        progressCircle.style.strokeDashoffset = dashOffset;
    }

    // Handle window resize for responsive design
    function handleResize() {
        if (!countdownElement) return;

        // Recalculate sizes if needed
        init();
    }

    // Public API
    return {
        init: init,
        start: start,
        pause: pause,
        restart: restart,
        updateState: updateState,
        updateProgress: updateProgress,
        handleResize: handleResize
    };
})();

// Auto-initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    adminCountdown.init();
});

// Handle resize events
window.addEventListener('resize', adminCountdown.handleResize);

// Handle Blazor component updates
document.addEventListener('DOMContentLoaded', function() {
    // Re-initialize when Blazor updates the DOM
    const observer = new MutationObserver(function(mutations) {
        mutations.forEach(function(mutation) {
            if (mutation.type === 'childList') {
                const addedNodes = Array.from(mutation.addedNodes);
                const hasCountdownCircle = addedNodes.some(node =>
                    node.nodeType === Node.ELEMENT_NODE &&
                    (node.classList?.contains('admin-countdown-circle') ||
                     node.querySelector?.('.admin-countdown-circle'))
                );

                if (hasCountdownCircle) {
                    setTimeout(() => adminCountdown.init(), 100);
                }
            }
        });
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
});

console.log('📊 Admin countdown manager loaded');