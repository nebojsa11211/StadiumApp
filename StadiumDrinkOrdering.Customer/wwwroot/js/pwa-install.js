/**
 * PWA Installation Manager
 *
 * Handles detection, installation, and tracking of the Progressive Web App.
 * Manages the beforeinstallprompt event and provides installation functionality.
 */

let deferredPrompt = null;
let dotNetReference = null;

// Initialize PWA install detection
window.initializePwaInstall = function (dotNetRef) {
    dotNetReference = dotNetRef;

    // Listen for beforeinstallprompt event
    window.addEventListener('beforeinstallprompt', (e) => {
        console.log('[PWA] beforeinstallprompt event fired');

        // Prevent the default mini-infobar from appearing
        e.preventDefault();

        // Store the event for later use
        deferredPrompt = e;

        // Show custom install prompt
        if (dotNetReference && !isInstallPromptDismissed()) {
            dotNetReference.invokeMethodAsync('ShowInstallPrompt');
        }
    });

    // Listen for app installation
    window.addEventListener('appinstalled', (e) => {
        console.log('[PWA] App installed successfully');

        // Clear the deferred prompt
        deferredPrompt = null;

        // Notify Blazor component
        if (dotNetReference) {
            dotNetReference.invokeMethodAsync('OnAppInstalled');
        }

        // Track installation
        trackPwaInstallation({
            timestamp: new Date().toISOString(),
            platform: getPlatform(),
            userAgent: getUserAgent()
        });
    });

    // Check if app is running in standalone mode (already installed)
    if (checkIfPwaInstalled()) {
        console.log('[PWA] App is running in standalone mode');
        if (dotNetReference) {
            dotNetReference.invokeMethodAsync('HideInstallPrompt');
        }
    }
};

// Trigger PWA installation
window.installPwa = async function () {
    if (!deferredPrompt) {
        console.error('[PWA] Installation prompt not available');
        return false;
    }

    try {
        // Show the install prompt
        deferredPrompt.prompt();

        // Wait for user response
        const choiceResult = await deferredPrompt.userChoice;

        console.log(`[PWA] User response: ${choiceResult.outcome}`);

        // Clear the deferred prompt
        deferredPrompt = null;

        // Return true if user accepted
        return choiceResult.outcome === 'accepted';
    } catch (error) {
        console.error('[PWA] Installation failed:', error);
        return false;
    }
};

// Check if PWA is already installed
window.checkIfPwaInstalled = function () {
    // Check if running in standalone mode
    const isStandalone = window.matchMedia('(display-mode: standalone)').matches ||
                        window.navigator.standalone ||
                        document.referrer.includes('android-app://');

    return isStandalone;
};

// Check if install prompt was dismissed
window.checkInstallPromptDismissed = function () {
    return isInstallPromptDismissed();
};

// Dismiss install prompt (persists in localStorage for 7 days)
window.dismissInstallPrompt = function () {
    const dismissalDate = new Date();
    const expiryDate = new Date(dismissalDate.getTime() + (7 * 24 * 60 * 60 * 1000)); // 7 days

    localStorage.setItem('pwa-install-dismissed', JSON.stringify({
        dismissed: true,
        dismissedAt: dismissalDate.toISOString(),
        expiresAt: expiryDate.toISOString()
    }));
};

// Track PWA installation for analytics
window.trackPwaInstallation = function (data) {
    try {
        // Store installation data
        const installations = JSON.parse(localStorage.getItem('pwa-installations') || '[]');
        installations.push(data);
        localStorage.setItem('pwa-installations', JSON.stringify(installations));

        console.log('[PWA] Installation tracked:', data);

        // Send to analytics endpoint if available
        if (typeof fetch !== 'undefined') {
            fetch('/api/analytics/pwa-install', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            }).catch(err => console.warn('[PWA] Failed to send analytics:', err));
        }
    } catch (error) {
        console.error('[PWA] Failed to track installation:', error);
    }
};

// Get platform information
window.getPlatform = function () {
    const userAgent = navigator.userAgent.toLowerCase();

    if (/android/.test(userAgent)) return 'Android';
    if (/iphone|ipad|ipod/.test(userAgent)) return 'iOS';
    if (/windows/.test(userAgent)) return 'Windows';
    if (/mac/.test(userAgent)) return 'macOS';
    if (/linux/.test(userAgent)) return 'Linux';

    return 'Unknown';
};

// Get user agent
window.getUserAgent = function () {
    return navigator.userAgent;
};

// Helper: Check if install prompt is currently dismissed
function isInstallPromptDismissed() {
    try {
        const dismissalData = localStorage.getItem('pwa-install-dismissed');
        if (!dismissalData) return false;

        const { dismissed, expiresAt } = JSON.parse(dismissalData);
        const now = new Date();
        const expiry = new Date(expiresAt);

        // Check if dismissal has expired
        if (now > expiry) {
            localStorage.removeItem('pwa-install-dismissed');
            return false;
        }

        return dismissed;
    } catch (error) {
        console.error('[PWA] Error checking dismissal status:', error);
        return false;
    }
}

// Register service worker
window.registerServiceWorker = async function () {
    if ('serviceWorker' in navigator) {
        try {
            const registration = await navigator.serviceWorker.register('/service-worker.js', {
                scope: '/'
            });

            console.log('[PWA] Service Worker registered successfully:', registration.scope);

            // Check for updates
            registration.addEventListener('updatefound', () => {
                const newWorker = registration.installing;
                console.log('[PWA] Service Worker update found');

                newWorker.addEventListener('statechange', () => {
                    if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                        console.log('[PWA] New Service Worker available');

                        // Notify user about update
                        if (confirm('A new version is available. Would you like to update?')) {
                            newWorker.postMessage({ type: 'SKIP_WAITING' });
                            window.location.reload();
                        }
                    }
                });
            });

            return true;
        } catch (error) {
            console.error('[PWA] Service Worker registration failed:', error);
            return false;
        }
    } else {
        console.warn('[PWA] Service Workers not supported in this browser');
        return false;
    }
};

// Unregister service worker (for testing/debugging)
window.unregisterServiceWorker = async function () {
    if ('serviceWorker' in navigator) {
        try {
            const registrations = await navigator.serviceWorker.getRegistrations();
            for (const registration of registrations) {
                await registration.unregister();
                console.log('[PWA] Service Worker unregistered');
            }
            return true;
        } catch (error) {
            console.error('[PWA] Service Worker unregistration failed:', error);
            return false;
        }
    }
    return false;
};

// Clear all caches (for testing/debugging)
window.clearPwaCache = async function () {
    if ('caches' in window) {
        try {
            const cacheNames = await caches.keys();
            await Promise.all(
                cacheNames.map(cacheName => caches.delete(cacheName))
            );
            console.log('[PWA] All caches cleared');
            return true;
        } catch (error) {
            console.error('[PWA] Failed to clear caches:', error);
            return false;
        }
    }
    return false;
};

// Check network status
window.checkNetworkStatus = function () {
    return navigator.onLine;
};

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    // Register service worker automatically
    registerServiceWorker();

    // Listen for online/offline events
    window.addEventListener('online', () => {
        console.log('[PWA] Connection restored');
    });

    window.addEventListener('offline', () => {
        console.log('[PWA] Connection lost');
    });
});
