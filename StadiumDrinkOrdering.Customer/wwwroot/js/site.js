// Stadium Ordering System - Customer App JavaScript

// Stripe Integration
let stripe = null;
let elements = null;
let cardElement = null;

// QR Scanner Integration
let qrVideo = null;
let qrCanvas = null;
let qrContext = null;
let qrScanInterval = null;

// Initialize Stripe Elements
window.initializeStripe = async (amount) => {
    try {
        // Initialize Stripe (replace with your publishable key)
        stripe = Stripe('pk_test_your_stripe_publishable_key_here');
        elements = stripe.elements();

        // Create card element
        const style = {
            base: {
                fontSize: '16px',
                color: '#424770',
                '::placeholder': {
                    color: '#aab7c4',
                },
                iconColor: '#666EE8',
                fontFamily: '"Segoe UI", Tahoma, Geneva, Verdana, sans-serif',
                fontSmoothing: 'antialiased',
            },
            invalid: {
                color: '#fa755a',
                iconColor: '#fa755a'
            }
        };

        cardElement = elements.create('card', { 
            style: style,
            hidePostalCode: true 
        });
        cardElement.mount('#stripe-card-element');

        // Handle real-time validation errors from the card Element
        cardElement.on('change', ({ error }) => {
            const displayError = document.getElementById('stripe-card-errors');
            if (error) {
                displayError.textContent = error.message;
            } else {
                displayError.textContent = '';
            }
        });

        return true;
    } catch (error) {
        console.error('Stripe initialization failed:', error);
        return false;
    }
};

// Process Stripe Payment
window.processStripePayment = async (clientSecret) => {
    try {
        if (!stripe || !cardElement) {
            throw new Error('Stripe not initialized');
        }

        const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: cardElement,
                billing_details: {
                    name: 'Stadium Customer',
                }
            }
        });

        if (error) {
            console.error('Payment failed:', error);
            const displayError = document.getElementById('stripe-card-errors');
            if (displayError) {
                displayError.textContent = error.message;
            }
            return false;
        } else if (paymentIntent && paymentIntent.status === 'succeeded') {
            console.log('Payment succeeded:', paymentIntent);
            return true;
        } else {
            console.error('Payment not completed:', paymentIntent);
            return false;
        }
    } catch (error) {
        console.error('Payment processing error:', error);
        return false;
    }
};

// QR Scanner Functions
window.startQRScanner = async () => {
    try {
        qrVideo = document.getElementById('qr-video');
        qrCanvas = document.getElementById('qr-canvas');
        
        if (!qrVideo || !qrCanvas) {
            throw new Error('QR scanner elements not found');
        }

        qrContext = qrCanvas.getContext('2d');

        // Request camera access
        const constraints = {
            video: {
                facingMode: { ideal: 'environment' }, // Use back camera if available
                width: { ideal: 300 },
                height: { ideal: 300 }
            }
        };

        const stream = await navigator.mediaDevices.getUserMedia(constraints);
        qrVideo.srcObject = stream;

        // Start scanning when video is ready
        qrVideo.addEventListener('loadedmetadata', () => {
            qrCanvas.width = qrVideo.videoWidth;
            qrCanvas.height = qrVideo.videoHeight;
            startQRDetection();
        });

        return true;
    } catch (error) {
        console.error('QR Scanner initialization failed:', error);
        
        // Show error message to user
        const errorMsg = error.name === 'NotAllowedError' 
            ? 'Camera access denied. Please allow camera permissions and try again.'
            : 'QR Scanner not supported or camera unavailable.';
            
        throw new Error(errorMsg);
    }
};

// Start QR Code Detection
function startQRDetection() {
    if (!qrVideo || !qrCanvas || !qrContext) return;

    qrScanInterval = setInterval(() => {
        if (qrVideo.readyState === qrVideo.HAVE_ENOUGH_DATA) {
            scanForQRCode();
        }
    }, 100); // Scan every 100ms
}

// Scan for QR Code
function scanForQRCode() {
    try {
        // Draw the video frame to canvas
        qrContext.drawImage(qrVideo, 0, 0, qrCanvas.width, qrCanvas.height);
        
        // Get image data
        const imageData = qrContext.getImageData(0, 0, qrCanvas.width, qrCanvas.height);
        
        // Use jsQR library to decode QR code
        if (typeof jsQR !== 'undefined') {
            const code = jsQR(imageData.data, imageData.width, imageData.height);
            
            if (code && code.data) {
                handleQRCodeDetected(code.data);
            }
        } else {
            // Fallback: Simple pattern detection (basic implementation)
            // This would need a more sophisticated approach in production
            console.log('jsQR library not loaded, using basic detection');
        }
    } catch (error) {
        console.error('QR scanning error:', error);
    }
}

// Handle QR Code Detection
function handleQRCodeDetected(qrData) {
    try {
        console.log('QR Code detected:', qrData);
        
        // Stop scanning
        stopQRScanner();
        
        // Parse QR data (could be JSON or simple string)
        let qrToken = qrData;
        
        try {
            const qrObject = JSON.parse(qrData);
            if (qrObject.Token) {
                qrToken = qrObject.Token;
            }
        } catch (e) {
            // QR data is not JSON, treat as token directly
        }
        
        // Navigate to validation page
        if (qrToken && qrToken.length > 10) {
            window.location.href = `/scan/${qrToken}`;
        } else {
            throw new Error('Invalid QR code format');
        }
        
    } catch (error) {
        console.error('QR code processing error:', error);
        showQRError('Invalid QR code. Please try again.');
    }
}

// Stop QR Scanner
window.stopQRScanner = () => {
    if (qrScanInterval) {
        clearInterval(qrScanInterval);
        qrScanInterval = null;
    }
    
    if (qrVideo && qrVideo.srcObject) {
        const tracks = qrVideo.srcObject.getTracks();
        tracks.forEach(track => track.stop());
        qrVideo.srcObject = null;
    }
};

// Show QR Scanner Error
function showQRError(message) {
    // This could trigger a Blazor method to show the error
    console.error('QR Scanner Error:', message);
    
    // You could also show a native alert or update UI directly
    if (typeof DotNet !== 'undefined' && window.blazorQRErrorCallback) {
        window.blazorQRErrorCallback(message);
    }
}

// Utility Functions

// Format currency
window.formatCurrency = (amount, currency = 'EUR') => {
    return new Intl.NumberFormat('en-EU', {
        style: 'currency',
        currency: currency
    }).format(amount);
};

// Show toast notification
window.showToast = (message, type = 'info', duration = 3000) => {
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `
        <div class="toast-content">
            <span class="toast-message">${message}</span>
            <button class="toast-close" onclick="this.parentElement.parentElement.remove()">×</button>
        </div>
    `;
    
    // Add CSS if not already added
    if (!document.getElementById('toast-styles')) {
        const style = document.createElement('style');
        style.id = 'toast-styles';
        style.textContent = `
            .toast {
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 10000;
                min-width: 300px;
                border-radius: 8px;
                box-shadow: 0 4px 15px rgba(0,0,0,0.15);
                animation: slideInRight 0.3s ease-out;
            }
            .toast-info { background: #d1ecf1; color: #0c5460; border: 1px solid #bee5eb; }
            .toast-success { background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
            .toast-warning { background: #fff3cd; color: #856404; border: 1px solid #ffeaa7; }
            .toast-error { background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
            .toast-content {
                padding: 15px 20px;
                display: flex;
                align-items: center;
                justify-content: space-between;
                gap: 10px;
            }
            .toast-message { font-weight: 500; flex: 1; }
            .toast-close {
                background: none;
                border: none;
                font-size: 1.2rem;
                cursor: pointer;
                color: inherit;
                padding: 0;
            }
            @keyframes slideInRight {
                from { transform: translateX(100%); }
                to { transform: translateX(0); }
            }
        `;
        document.head.appendChild(style);
    }
    
    document.body.appendChild(toast);
    
    // Auto-remove after duration
    if (duration > 0) {
        setTimeout(() => {
            if (toast.parentElement) {
                toast.style.animation = 'slideOutRight 0.3s ease-in';
                setTimeout(() => toast.remove(), 300);
            }
        }, duration);
    }
};

// Vibrate phone (for mobile feedback)
window.vibratePhone = (pattern = [100]) => {
    if ('vibrate' in navigator) {
        navigator.vibrate(pattern);
    }
};

// Check if device is mobile
window.isMobileDevice = () => {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
};

// Get device orientation
window.getOrientation = () => {
    return window.screen.orientation ? window.screen.orientation.angle : window.orientation;
};

// Progressive Web App (PWA) Functions
window.installPWA = () => {
    if (window.deferredPrompt) {
        window.deferredPrompt.prompt();
        window.deferredPrompt.userChoice.then((choiceResult) => {
            console.log('PWA install choice:', choiceResult.outcome);
            window.deferredPrompt = null;
        });
    }
};

// Service Worker Registration (for PWA)
window.registerServiceWorker = async () => {
    if ('serviceWorker' in navigator) {
        try {
            const registration = await navigator.serviceWorker.register('/sw.js');
            console.log('Service Worker registered:', registration);
            return true;
        } catch (error) {
            console.error('Service Worker registration failed:', error);
            return false;
        }
    }
    return false;
};

// Online/Offline Detection
window.addOfflineHandlers = () => {
    window.addEventListener('online', () => {
        showToast('Connection restored!', 'success', 2000);
        document.body.classList.remove('offline');
    });
    
    window.addEventListener('offline', () => {
        showToast('You are now offline. Some features may be limited.', 'warning', 5000);
        document.body.classList.add('offline');
    });
    
    // Add offline CSS
    if (!document.getElementById('offline-styles')) {
        const style = document.createElement('style');
        style.id = 'offline-styles';
        style.textContent = `
            .offline::before {
                content: "⚠️ Offline Mode";
                position: fixed;
                top: 0;
                left: 0;
                right: 0;
                background: #856404;
                color: white;
                text-align: center;
                padding: 8px;
                font-weight: bold;
                z-index: 9999;
                font-size: 0.9rem;
            }
            .offline { padding-top: 40px; }
        `;
        document.head.appendChild(style);
    }
};

// Initialize app when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Add offline handlers
    addOfflineHandlers();
    
    // Check if already offline
    if (!navigator.onLine) {
        document.body.classList.add('offline');
    }
    
    // PWA install prompt
    window.addEventListener('beforeinstallprompt', (e) => {
        e.preventDefault();
        window.deferredPrompt = e;
    });
    
    console.log('Stadium Ordering System initialized');
});

// Cleanup function
window.addEventListener('beforeunload', () => {
    stopQRScanner();
});

// Global error handler
window.addEventListener('error', (event) => {
    console.error('Global error:', event.error);
    
    // Don't show error toast for minor issues
    if (event.error && !event.error.message.includes('ResizeObserver')) {
        showToast('An unexpected error occurred. Please refresh the page if problems persist.', 'error', 5000);
    }
});

// Expose functions globally for Blazor interop
window.stadiumOrdering = {
    initializeStripe,
    processStripePayment,
    startQRScanner,
    stopQRScanner,
    formatCurrency,
    showToast,
    vibratePhone,
    isMobileDevice,
    getOrientation,
    installPWA,
    registerServiceWorker
};