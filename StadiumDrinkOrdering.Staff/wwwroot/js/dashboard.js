// Dashboard JavaScript Helpers
window.dashboardHelpers = {
    // Modal management
    showModal: function(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            const bootstrapModal = new bootstrap.Modal(modal);
            bootstrapModal.show();
        } else {
            console.warn(`Modal with ID '${modalId}' not found`);
        }
    },

    hideModal: function(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            const bootstrapModal = bootstrap.Modal.getInstance(modal);
            if (bootstrapModal) {
                bootstrapModal.hide();
            }
        } else {
            console.warn(`Modal with ID '${modalId}' not found`);
        }
    },

    hideAllModals: function() {
        const modals = document.querySelectorAll('.modal.show');
        modals.forEach(modal => {
            const bootstrapModal = bootstrap.Modal.getInstance(modal);
            if (bootstrapModal) {
                bootstrapModal.hide();
            }
        });
    },

    // Connection status helpers
    updateConnectionStatus: function(isConnected) {
        const statusElements = document.querySelectorAll('.connection-status');
        statusElements.forEach(element => {
            if (isConnected) {
                element.classList.remove('bg-danger');
                element.classList.add('bg-success');
                element.textContent = 'Connected';
            } else {
                element.classList.remove('bg-success');
                element.classList.add('bg-danger');
                element.textContent = 'Disconnected';
            }
        });
    },

    // Toast notifications
    showToast: function(message, type = 'info', duration = 3000) {
        // Create toast container if it doesn't exist
        let toastContainer = document.getElementById('toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.id = 'toast-container';
            toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
            toastContainer.style.zIndex = '1055';
            document.body.appendChild(toastContainer);
        }

        // Create toast element
        const toastId = 'toast-' + Date.now();
        const toastHtml = `
            <div id="${toastId}" class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="${duration}">
                <div class="toast-header">
                    <div class="toast-icon bg-${type} rounded me-2" style="width: 20px; height: 20px;"></div>
                    <strong class="me-auto">Dashboard</strong>
                    <small>${new Date().toLocaleTimeString()}</small>
                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;

        toastContainer.insertAdjacentHTML('beforeend', toastHtml);

        // Show toast
        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement);
        toast.show();

        // Remove toast element after it's hidden
        toastElement.addEventListener('hidden.bs.toast', function() {
            toastElement.remove();
        });
    },

    // Smooth scrolling
    scrollToElement: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    },

    // Loading states
    showLoadingSpinner: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            const spinner = document.createElement('div');
            spinner.className = 'loading-overlay d-flex justify-content-center align-items-center position-absolute w-100 h-100';
            spinner.style.backgroundColor = 'rgba(255, 255, 255, 0.8)';
            spinner.style.zIndex = '10';
            spinner.innerHTML = `
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            `;

            element.style.position = 'relative';
            element.appendChild(spinner);
        }
    },

    hideLoadingSpinner: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            const spinner = element.querySelector('.loading-overlay');
            if (spinner) {
                spinner.remove();
            }
        }
    },

    // Card animations
    animateCardValue: function(elementId, newValue, duration = 500) {
        const element = document.getElementById(elementId);
        if (element) {
            const currentValue = parseInt(element.textContent) || 0;
            const increment = (newValue - currentValue) / (duration / 16);
            let current = currentValue;

            const timer = setInterval(() => {
                current += increment;
                if ((increment > 0 && current >= newValue) || (increment < 0 && current <= newValue)) {
                    element.textContent = newValue;
                    clearInterval(timer);
                } else {
                    element.textContent = Math.round(current);
                }
            }, 16);
        }
    },

    // Utility functions
    formatDateTime: function(dateString) {
        const date = new Date(dateString);
        return date.toLocaleString();
    },

    formatTime: function(dateString) {
        const date = new Date(dateString);
        return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    },

    // Theme helpers
    toggleDarkMode: function() {
        document.body.classList.toggle('dark-mode');
        localStorage.setItem('dark-mode', document.body.classList.contains('dark-mode'));
    },

    initializeDarkMode: function() {
        if (localStorage.getItem('dark-mode') === 'true') {
            document.body.classList.add('dark-mode');
        }
    },

    // Data refresh helpers
    showDataRefreshIndicator: function() {
        const indicator = document.getElementById('data-refresh-indicator');
        if (indicator) {
            indicator.style.display = 'block';
        }
    },

    hideDataRefreshIndicator: function() {
        const indicator = document.getElementById('data-refresh-indicator');
        if (indicator) {
            indicator.style.display = 'none';
        }
    },

    // Accessibility helpers
    announceToScreenReader: function(message) {
        const announcement = document.createElement('div');
        announcement.setAttribute('aria-live', 'polite');
        announcement.setAttribute('aria-atomic', 'true');
        announcement.className = 'sr-only';
        announcement.textContent = message;

        document.body.appendChild(announcement);

        setTimeout(() => {
            document.body.removeChild(announcement);
        }, 1000);
    },

    // Initialize dashboard
    initialize: function() {
        console.log('Dashboard helpers initialized');
        this.initializeDarkMode();

        // Add global event listeners
        document.addEventListener('keydown', (e) => {
            // Escape key closes modals
            if (e.key === 'Escape') {
                this.hideAllModals();
            }
        });

        // Add connection status listeners
        window.addEventListener('online', () => {
            this.showToast('Connection restored', 'success');
            this.updateConnectionStatus(true);
        });

        window.addEventListener('offline', () => {
            this.showToast('Connection lost', 'danger');
            this.updateConnectionStatus(false);
        });
    }
};

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    window.dashboardHelpers.initialize();
});

// Blazor interop
window.blazorDashboard = {
    showToast: function(message, type) {
        window.dashboardHelpers.showToast(message, type);
    },

    updateCardValue: function(elementId, newValue) {
        window.dashboardHelpers.animateCardValue(elementId, newValue);
    },

    announceUpdate: function(message) {
        window.dashboardHelpers.announceToScreenReader(message);
    }
};