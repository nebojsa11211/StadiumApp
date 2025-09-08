// Console Interceptor for System Logging
class ConsoleInterceptor {
    constructor() {
        this.isEnabled = false;
        this.originalConsole = {
            log: console.log,
            error: console.error,
            warn: console.warn,
            info: console.info,
            debug: console.debug
        };
        
        this.apiBaseUrl = window.apiBaseUrl || '/api';
        this.init();
    }

    init() {
        // Check initial setting from localStorage or default to false
        this.isEnabled = localStorage.getItem('consoleToSystemLogging') === 'true';
        
        if (this.isEnabled) {
            this.enableInterception();
        }
    }

    enableInterception() {
        const self = this;
        
        console.log = function(...args) {
            self.originalConsole.log.apply(console, args);
            if (self.isEnabled) {
                self.sendToSystemLog(args.join(' '), 'Info', 'ConsoleLog');
            }
        };

        console.error = function(...args) {
            self.originalConsole.error.apply(console, args);
            if (self.isEnabled) {
                self.sendToSystemLog(args.join(' '), 'Error', 'ConsoleError');
            }
        };

        console.warn = function(...args) {
            self.originalConsole.warn.apply(console, args);
            if (self.isEnabled) {
                self.sendToSystemLog(args.join(' '), 'Warning', 'ConsoleWarning');
            }
        };

        console.info = function(...args) {
            self.originalConsole.info.apply(console, args);
            if (self.isEnabled) {
                self.sendToSystemLog(args.join(' '), 'Info', 'ConsoleInfo');
            }
        };

        console.debug = function(...args) {
            self.originalConsole.debug.apply(console, args);
            if (self.isEnabled) {
                self.sendToSystemLog(args.join(' '), 'Debug', 'ConsoleDebug');
            }
        };
    }

    disableInterception() {
        console.log = this.originalConsole.log;
        console.error = this.originalConsole.error;
        console.warn = this.originalConsole.warn;
        console.info = this.originalConsole.info;
        console.debug = this.originalConsole.debug;
    }

    toggle(enabled) {
        this.isEnabled = enabled;
        localStorage.setItem('consoleToSystemLogging', enabled.toString());
        
        if (enabled) {
            this.enableInterception();
        } else {
            this.disableInterception();
            // Re-enable interception structure but with disabled flag
            this.enableInterception();
        }
    }

    async sendToSystemLog(message, level, action) {
        if (!this.isEnabled) return;
        
        try {
            const logEntry = {
                message: message,
                level: level,
                action: action,
                category: 'AdminConsoleJS',
                source: 'Admin',
                timestamp: new Date().toISOString(),
                userEmail: this.getCurrentUserEmail(),
                userId: this.getCurrentUserId()
            };

            // Send to centralized logging API
            await fetch(`${this.apiBaseUrl}/logs/log-action`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${this.getAuthToken()}`
                },
                body: JSON.stringify(logEntry)
            });
        } catch (error) {
            // Use original console to avoid infinite loop
            this.originalConsole.error('[ConsoleInterceptor] Failed to send log to system:', error);
        }
    }

    getCurrentUserEmail() {
        // Try to get user email from DOM or other sources
        const userEmailElement = document.querySelector('[data-user-email]');
        return userEmailElement ? userEmailElement.getAttribute('data-user-email') : 'unknown@admin.local';
    }

    getCurrentUserId() {
        // Try to get user ID from DOM or other sources
        const userIdElement = document.querySelector('[data-user-id]');
        return userIdElement ? parseInt(userIdElement.getAttribute('data-user-id')) : null;
    }

    getAuthToken() {
        // Try to get auth token from localStorage or sessionStorage
        return localStorage.getItem('authToken') || sessionStorage.getItem('authToken') || '';
    }

    getStatus() {
        return {
            enabled: this.isEnabled,
            interceptorActive: console.log !== this.originalConsole.log
        };
    }
}

// Global instance
window.consoleInterceptor = new ConsoleInterceptor();

// Global functions for easy access
window.toggleConsoleToSystemLogging = function(enabled) {
    window.consoleInterceptor.toggle(enabled);
};

window.getConsoleInterceptorStatus = function() {
    return window.consoleInterceptor.getStatus();
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        // Re-initialize after DOM load to ensure proper setup
        window.consoleInterceptor.init();
    });
} else {
    // Already loaded
    window.consoleInterceptor.init();
}