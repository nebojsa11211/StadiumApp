// Authentication Session Bridge
// Synchronizes tokens from localStorage to server session state
// for hybrid client/server-side authentication in Blazor Server

window.authSessionBridge = {
    // Sync authentication tokens from localStorage to server session
    syncTokensToSession: function() {
        try {
            const token = localStorage.getItem('admin_auth_token');
            const tokenExpiration = localStorage.getItem('admin_auth_token_expiration');
            const refreshToken = localStorage.getItem('admin_auth_refresh_token');
            const refreshTokenExpiration = localStorage.getItem('admin_auth_refresh_token_expiration');
            const userEmail = localStorage.getItem('admin_auth_email');

            if (token) {
                // Make a call to server to store tokens in session
                this.syncToServerSession({
                    token: token,
                    tokenExpiration: tokenExpiration,
                    refreshToken: refreshToken,
                    refreshTokenExpiration: refreshTokenExpiration,
                    userEmail: userEmail
                });
            }
        } catch (error) {
            console.warn('Failed to sync tokens to session:', error);
        }
    },

    // Internal method to sync tokens to server session via API call
    syncToServerSession: async function(tokenData) {
        try {
            const response = await fetch('/api/auth/sync-session', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': tokenData.token ? `Bearer ${tokenData.token}` : ''
                },
                body: JSON.stringify(tokenData)
            });

            if (!response.ok) {
                console.warn('Failed to sync tokens to server session:', response.statusText);
            } else {
                console.debug('Tokens synced to server session successfully');
            }
        } catch (error) {
            console.warn('Network error syncing tokens to session:', error);
        }
    },

    // Monitor localStorage changes and sync to session
    setupTokenSyncListener: function() {
        // Listen for localStorage changes
        window.addEventListener('storage', (e) => {
            if (e.key && e.key.startsWith('admin_auth_')) {
                this.syncTokensToSession();
            }
        });

        // Also sync on focus (in case localStorage was changed in same tab)
        window.addEventListener('focus', () => {
            this.syncTokensToSession();
        });
    },

    // Initialize the session bridge
    initialize: function() {
        console.debug('Initializing auth session bridge');

        // Sync tokens immediately
        this.syncTokensToSession();

        // Setup listeners for future changes
        this.setupTokenSyncListener();

        // Sync periodically (every 30 seconds) to catch any missed changes
        setInterval(() => {
            this.syncTokensToSession();
        }, 30000);
    },

    // Manual sync trigger (can be called from Blazor components)
    triggerSync: function() {
        this.syncTokensToSession();
    },

    // Check if tokens are available in localStorage
    hasTokens: function() {
        try {
            return !!localStorage.getItem('admin_auth_token');
        } catch (error) {
            return false;
        }
    },

    // Clear tokens from localStorage (called on logout)
    clearTokens: function() {
        try {
            localStorage.removeItem('admin_auth_token');
            localStorage.removeItem('admin_auth_token_expiration');
            localStorage.removeItem('admin_auth_refresh_token');
            localStorage.removeItem('admin_auth_refresh_token_expiration');
            localStorage.removeItem('admin_auth_email');

            // Also clear from server session
            this.clearServerSession();
        } catch (error) {
            console.warn('Failed to clear tokens:', error);
        }
    },

    // Clear server session
    clearServerSession: async function() {
        try {
            await fetch('/api/auth/clear-session', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        } catch (error) {
            console.warn('Failed to clear server session:', error);
        }
    }
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        window.authSessionBridge.initialize();
    });
} else {
    // DOM is already ready
    window.authSessionBridge.initialize();
}

// Export for module environments
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.authSessionBridge;
}