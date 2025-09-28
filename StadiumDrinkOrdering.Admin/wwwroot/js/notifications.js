// Toast Notification Manager
class NotificationManager {
    constructor() {
        this.container = this.createContainer();
        this.notifications = new Map();
        this.notificationCount = 0;
    }

    createContainer() {
        // Try to use existing toast-container from MainLayout first
        let container = document.getElementById('toast-container');
        if (!container) {
            // Fallback: create our own if it doesn't exist
            container = document.createElement('div');
            container.id = 'notification-container';
            container.className = 'position-fixed top-0 end-0 p-3';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }
        return container;
    }

    showToast(type, message, title, duration) {
        const id = `notification-${++this.notificationCount}`;
        const toast = this.createToastElement(id, type, message, title);

        this.container.appendChild(toast);
        this.notifications.set(id, { element: toast, timer: null });

        // Show the toast with animation
        this.animateIn(toast);

        // Auto-remove after duration (unless duration is 0)
        if (duration > 0) {
            const timer = setTimeout(() => this.removeToast(id), duration);
            this.notifications.get(id).timer = timer;
        }

        return id;
    }

    createToastElement(id, type, message, title) {
        const toast = document.createElement('div');
        toast.id = id;
        toast.className = `toast mb-2 ${this.getToastClasses(type)}`;
        toast.setAttribute('role', 'alert');

        const icon = this.getIcon(type);
        const bgClass = this.getBackgroundClass(type);
        const textColor = type === 'warning' ? 'text-dark' : 'text-white';

        toast.innerHTML = `
            <div class="toast-header ${bgClass} ${textColor} border-0">
                <i class="${icon} me-2"></i>
                <strong class="me-auto">${title || this.getDefaultTitle(type)}</strong>
                <button type="button" class="btn-close ${textColor === 'text-white' ? 'btn-close-white' : ''}" onclick="notificationManager.removeToast('${id}')" aria-label="Close"></button>
            </div>
            <div class="toast-body ${textColor} ${bgClass} opacity-75">
                ${message}
            </div>
        `;

        return toast;
    }

    getToastClasses(type) {
        return `notification-toast notification-${type}`;
    }

    getBackgroundClass(type) {
        switch (type) {
            case 'success': return 'bg-success';
            case 'error': return 'bg-danger';
            case 'warning': return 'bg-warning';
            case 'info': return 'bg-info';
            default: return 'bg-secondary';
        }
    }

    getIcon(type) {
        switch (type) {
            case 'success': return 'fas fa-check-circle';
            case 'error': return 'fas fa-exclamation-triangle';
            case 'warning': return 'fas fa-exclamation-circle';
            case 'info': return 'fas fa-info-circle';
            default: return 'fas fa-bell';
        }
    }

    getDefaultTitle(type) {
        switch (type) {
            case 'success': return 'Success';
            case 'error': return 'Error';
            case 'warning': return 'Warning';
            case 'info': return 'Information';
            default: return 'Notification';
        }
    }

    animateIn(toast) {
        // Start invisible
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100%)';
        toast.style.transition = 'all 0.3s ease-in-out';

        // Force reflow
        toast.offsetHeight;

        // Animate in
        setTimeout(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateX(0)';
        }, 10);
    }

    animateOut(toast, callback) {
        toast.style.transition = 'all 0.3s ease-in-out';
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100%)';
        toast.style.height = '0';
        toast.style.margin = '0';
        toast.style.padding = '0';

        setTimeout(callback, 300);
    }

    removeToast(id) {
        const notification = this.notifications.get(id);
        if (!notification) return;

        // Clear timer if exists
        if (notification.timer) {
            clearTimeout(notification.timer);
        }

        // Animate out and remove
        this.animateOut(notification.element, () => {
            if (notification.element.parentNode) {
                notification.element.parentNode.removeChild(notification.element);
            }
            this.notifications.delete(id);
        });
    }

    clearAll() {
        for (const [id] of this.notifications) {
            this.removeToast(id);
        }
    }

    // Utility method for showing specific types
    showSuccess(message, title, duration = 5000) {
        return this.showToast('success', message, title, duration);
    }

    showError(message, title, duration = 8000) {
        return this.showToast('error', message, title, duration);
    }

    showWarning(message, title, duration = 6000) {
        return this.showToast('warning', message, title, duration);
    }

    showInfo(message, title, duration = 5000) {
        return this.showToast('info', message, title, duration);
    }
}

// Create global instance
window.notificationManager = new NotificationManager();

// Add some basic styles if they don't exist
if (!document.getElementById('notification-styles')) {
    const styles = document.createElement('style');
    styles.id = 'notification-styles';
    styles.textContent = `
        .notification-toast {
            max-width: 350px;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
            border: 1px solid rgba(255, 255, 255, 0.125);
        }

        .notification-toast .toast-header {
            font-weight: 600;
        }

        .notification-toast .toast-body {
            font-size: 0.875rem;
            line-height: 1.5;
        }

        .notification-error {
            border-left: 4px solid #dc3545;
        }

        .notification-success {
            border-left: 4px solid #198754;
        }

        .notification-warning {
            border-left: 4px solid #ffc107;
        }

        .notification-info {
            border-left: 4px solid #0dcaf0;
        }

        @media (max-width: 576px) {
            #notification-container {
                right: 10px !important;
                left: 10px !important;
                padding: 10px !important;
            }

            .notification-toast {
                max-width: none;
            }
        }
    `;
    document.head.appendChild(styles);
}

// Global function for C# interop
window.showToast = function(message, type, title, duration) {
    return window.notificationManager.showToast(type || 'info', message, title, duration);
};

console.log('Notification system initialized');