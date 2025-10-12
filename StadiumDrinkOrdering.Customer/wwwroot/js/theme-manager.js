/**
 * Theme Manager for Stadium Drink Ordering Customer App
 * Handles dark/light mode switching with system preference detection
 * and localStorage persistence
 */

const ThemeManager = {
    STORAGE_KEY: 'customer-theme-preference',
    THEME_ATTR: 'data-theme',

    /**
     * Initialize theme on page load
     */
    init: function() {
        // Prevent flash of unstyled content
        document.documentElement.classList.add('no-transition');

        // Get saved preference or detect system preference
        const savedTheme = this.getSavedTheme();
        const theme = savedTheme || this.getSystemPreference();

        // Apply theme
        this.applyTheme(theme);

        // Re-enable transitions after initial load
        setTimeout(() => {
            document.documentElement.classList.remove('no-transition');
        }, 100);

        // Listen for system preference changes
        this.watchSystemPreference();

        console.log('Theme Manager initialized:', theme);
    },

    /**
     * Get saved theme preference from localStorage
     * @returns {string|null} 'light', 'dark', or null
     */
    getSavedTheme: function() {
        try {
            return localStorage.getItem(this.STORAGE_KEY);
        } catch (e) {
            console.warn('localStorage not available:', e);
            return null;
        }
    },

    /**
     * Save theme preference to localStorage
     * @param {string} theme - 'light' or 'dark'
     */
    saveTheme: function(theme) {
        try {
            localStorage.setItem(this.STORAGE_KEY, theme);
            console.log('Theme saved to localStorage:', theme);
        } catch (e) {
            console.warn('Could not save theme to localStorage:', e);
        }
    },

    /**
     * Detect system color scheme preference
     * @returns {string} 'light' or 'dark'
     */
    getSystemPreference: function() {
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            return 'dark';
        }
        return 'light';
    },

    /**
     * Apply theme to document
     * @param {string} theme - 'light' or 'dark'
     */
    applyTheme: function(theme) {
        const validTheme = theme === 'dark' ? 'dark' : 'light';
        document.documentElement.setAttribute(this.THEME_ATTR, validTheme);

        // Update meta theme-color for mobile browsers
        this.updateMetaThemeColor(validTheme);

        // Dispatch custom event for theme change
        window.dispatchEvent(new CustomEvent('themechange', {
            detail: { theme: validTheme }
        }));

        console.log('Theme applied:', validTheme);
    },

    /**
     * Toggle between light and dark themes
     * @returns {string} The new theme
     */
    toggle: function() {
        const currentTheme = this.getCurrentTheme();
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        this.applyTheme(newTheme);
        this.saveTheme(newTheme);

        return newTheme;
    },

    /**
     * Set specific theme
     * @param {string} theme - 'light' or 'dark'
     */
    setTheme: function(theme) {
        this.applyTheme(theme);
        this.saveTheme(theme);
    },

    /**
     * Get current active theme
     * @returns {string} 'light' or 'dark'
     */
    getCurrentTheme: function() {
        const theme = document.documentElement.getAttribute(this.THEME_ATTR);
        return theme === 'dark' ? 'dark' : 'light';
    },

    /**
     * Watch for system preference changes
     */
    watchSystemPreference: function() {
        if (window.matchMedia) {
            const darkModeQuery = window.matchMedia('(prefers-color-scheme: dark)');

            // Modern browsers
            if (darkModeQuery.addEventListener) {
                darkModeQuery.addEventListener('change', (e) => {
                    // Only apply if user hasn't set a preference
                    if (!this.getSavedTheme()) {
                        this.applyTheme(e.matches ? 'dark' : 'light');
                    }
                });
            }
            // Older browsers
            else if (darkModeQuery.addListener) {
                darkModeQuery.addListener((e) => {
                    if (!this.getSavedTheme()) {
                        this.applyTheme(e.matches ? 'dark' : 'light');
                    }
                });
            }
        }
    },

    /**
     * Update meta theme-color for mobile browsers
     * @param {string} theme - 'light' or 'dark'
     */
    updateMetaThemeColor: function(theme) {
        let metaThemeColor = document.querySelector('meta[name="theme-color"]');

        if (!metaThemeColor) {
            metaThemeColor = document.createElement('meta');
            metaThemeColor.name = 'theme-color';
            document.head.appendChild(metaThemeColor);
        }

        // Set color based on theme
        metaThemeColor.content = theme === 'dark' ? '#1e293b' : '#ffffff';
    },

    /**
     * Clear saved theme preference
     */
    clearPreference: function() {
        try {
            localStorage.removeItem(this.STORAGE_KEY);
            console.log('Theme preference cleared');
        } catch (e) {
            console.warn('Could not clear theme preference:', e);
        }
    },

    /**
     * Check if dark mode is active
     * @returns {boolean}
     */
    isDarkMode: function() {
        return this.getCurrentTheme() === 'dark';
    },

    /**
     * Check if light mode is active
     * @returns {boolean}
     */
    isLightMode: function() {
        return this.getCurrentTheme() === 'light';
    }
};

// Auto-initialize on DOM ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => ThemeManager.init());
} else {
    ThemeManager.init();
}

// Expose to window for Blazor interop
window.ThemeManager = ThemeManager;

// Export for module usage if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ThemeManager;
}
