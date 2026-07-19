/**
 * Stadium Admin Theme System
 * Theme switching, persistence, and system preference detection
 * Version: 1.0.0
 * Last Updated: 2025-01-13
 */

class AdminThemeManager {
    constructor() {
        this.STORAGE_KEY = 'admin-theme-preference';
        this.THEME_ATTRIBUTE = 'data-theme';
        this.currentTheme = this.getStoredTheme() || this.getSystemTheme() || 'light';

        // Initialize theme system
        this.init();

        // Bind event listeners
        this.bindEvents();
    }

    /**
     * Initialize the theme system
     */
    init() {
        // Apply initial theme
        this.applyTheme(this.currentTheme);

        // Update UI elements
        this.updateThemeControls();

        // Log initialization
        this.debugLog('Theme system initialized', { currentTheme: this.currentTheme });
    }

    /**
     * Get the stored theme preference
     * @returns {string|null} Stored theme or null
     */
    getStoredTheme() {
        try {
            return localStorage.getItem(this.STORAGE_KEY);
        } catch (error) {
            this.debugLog('Error reading stored theme:', error);
            return null;
        }
    }

    /**
     * Get the system theme preference
     * @returns {string} System theme preference ('light' or 'dark')
     */
    getSystemTheme() {
        if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            return 'dark';
        }
        return 'light';
    }

    /**
     * Store theme preference
     * @param {string} theme - Theme to store
     */
    storeTheme(theme) {
        try {
            localStorage.setItem(this.STORAGE_KEY, theme);
            this.debugLog('Theme stored:', theme);
        } catch (error) {
            this.debugLog('Error storing theme:', error);
        }
    }

    /**
     * Apply theme to the document
     * @param {string} theme - Theme to apply ('light', 'dark', or 'auto')
     */
    applyTheme(theme) {
        const effectiveTheme = theme === 'auto' ? this.getSystemTheme() : theme;

        // Apply theme attribute to document element
        document.documentElement.setAttribute(this.THEME_ATTRIBUTE, effectiveTheme);

        // Apply theme to body for legacy compatibility
        document.body.setAttribute(this.THEME_ATTRIBUTE, effectiveTheme);

        // Update current theme
        this.currentTheme = theme;

        // Store preference
        this.storeTheme(theme);

        // Update meta theme-color for mobile browsers
        this.updateMetaThemeColor(effectiveTheme);

        // Dispatch custom event for other components to listen to
        this.dispatchThemeChangeEvent(effectiveTheme);

        this.debugLog('Theme applied:', { requested: theme, effective: effectiveTheme });
    }

    /**
     * Update meta theme-color for mobile browsers
     * @param {string} theme - Current effective theme
     */
    updateMetaThemeColor(theme) {
        let themeColorMeta = document.querySelector('meta[name="theme-color"]');

        if (!themeColorMeta) {
            themeColorMeta = document.createElement('meta');
            themeColorMeta.setAttribute('name', 'theme-color');
            document.head.appendChild(themeColorMeta);
        }

        const themeColor = theme === 'dark' ? '#0f172a' : '#f8fafc';
        themeColorMeta.setAttribute('content', themeColor);

        this.debugLog('Meta theme-color updated:', themeColor);
    }

    /**
     * Dispatch theme change event
     * @param {string} effectiveTheme - The effective theme being applied
     */
    dispatchThemeChangeEvent(effectiveTheme) {
        const event = new CustomEvent('admin-theme-changed', {
            detail: {
                theme: this.currentTheme,
                effectiveTheme: effectiveTheme,
                timestamp: new Date().toISOString()
            }
        });

        document.dispatchEvent(event);
        this.debugLog('Theme change event dispatched');
    }

    /**
     * Switch to the next theme in cycle
     */
    cycleTheme() {
        const themes = ['light', 'dark', 'auto'];
        const currentIndex = themes.indexOf(this.currentTheme);
        const nextIndex = (currentIndex + 1) % themes.length;
        const nextTheme = themes[nextIndex];

        this.setTheme(nextTheme);
        this.debugLog('Theme cycled to:', nextTheme);
    }

    /**
     * Set specific theme
     * @param {string} theme - Theme to set ('light', 'dark', or 'auto')
     */
    setTheme(theme) {
        if (!['light', 'dark', 'auto'].includes(theme)) {
            this.debugLog('Invalid theme requested:', theme);
            return false;
        }

        this.applyTheme(theme);
        this.updateThemeControls();

        // Add transition class for smooth theme switching
        this.addThemeTransition();

        return true;
    }

    /**
     * Get current theme
     * @returns {string} Current theme setting
     */
    getCurrentTheme() {
        return this.currentTheme;
    }

    /**
     * Get effective theme (resolves 'auto' to actual theme)
     * @returns {string} Effective theme ('light' or 'dark')
     */
    getEffectiveTheme() {
        return this.currentTheme === 'auto' ? this.getSystemTheme() : this.currentTheme;
    }

    /**
     * Add smooth transition during theme changes
     */
    addThemeTransition() {
        const transitionClass = 'admin-theme-transitioning';

        // Add transition class
        document.documentElement.classList.add(transitionClass);

        // Remove after transition completes
        setTimeout(() => {
            document.documentElement.classList.remove(transitionClass);
        }, 300);
    }

    /**
     * Update theme control UI elements
     */
    updateThemeControls() {
        // Update theme toggle buttons
        const themeButtons = document.querySelectorAll('[data-theme-toggle]');
        themeButtons.forEach(button => {
            const targetTheme = button.getAttribute('data-theme-toggle');
            button.classList.toggle('active', targetTheme === this.currentTheme);
            button.setAttribute('aria-pressed', targetTheme === this.currentTheme);
        });

        // Update theme select dropdowns
        const themeSelects = document.querySelectorAll('[data-theme-select]');
        themeSelects.forEach(select => {
            select.value = this.currentTheme;
        });

        // Update theme icons
        this.updateThemeIcons();

        this.debugLog('Theme controls updated');
    }

    /**
     * Update theme icons in UI
     */
    updateThemeIcons() {
        const themeIcons = document.querySelectorAll('[data-theme-icon]');
        const effectiveTheme = this.getEffectiveTheme();

        themeIcons.forEach(icon => {
            // Hide all icons first
            icon.style.display = 'none';

            // Show appropriate icon
            const iconTheme = icon.getAttribute('data-theme-icon');
            if (iconTheme === effectiveTheme ||
                (iconTheme === 'auto' && this.currentTheme === 'auto')) {
                icon.style.display = 'inline-block';
            }
        });
    }

    /**
     * Bind event listeners
     */
    bindEvents() {
        // Listen for system theme changes
        if (window.matchMedia) {
            const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
            mediaQuery.addEventListener('change', (e) => {
                if (this.currentTheme === 'auto') {
                    this.applyTheme('auto');
                    this.debugLog('System theme changed, auto theme updated');
                }
            });
        }

        // Listen for theme toggle button clicks
        document.addEventListener('click', (e) => {
            const themeToggle = e.target.closest('[data-theme-toggle]');
            if (themeToggle) {
                e.preventDefault();
                const targetTheme = themeToggle.getAttribute('data-theme-toggle');
                this.setTheme(targetTheme);
            }

            // Handle cycle theme buttons
            const themeCycle = e.target.closest('[data-theme-cycle]');
            if (themeCycle) {
                e.preventDefault();
                this.cycleTheme();
            }
        });

        // Listen for theme select changes
        document.addEventListener('change', (e) => {
            const themeSelect = e.target.closest('[data-theme-select]');
            if (themeSelect) {
                this.setTheme(themeSelect.value);
            }
        });

        // Listen for keyboard shortcuts
        document.addEventListener('keydown', (e) => {
            // Ctrl/Cmd + Shift + T to cycle themes
            if ((e.ctrlKey || e.metaKey) && e.shiftKey && e.key === 'T') {
                e.preventDefault();
                this.cycleTheme();
                this.showThemeToast();
            }
        });

        this.debugLog('Event listeners bound');
    }

    /**
     * Show theme change toast notification
     */
    showThemeToast() {
        const effectiveTheme = this.getEffectiveTheme();
        const themeLabel = this.getThemeLabel(this.currentTheme);

        // Create toast element
        const toast = document.createElement('div');
        toast.className = 'admin-theme-toast';
        toast.setAttribute('role', 'status');
        toast.setAttribute('aria-live', 'polite');
        toast.innerHTML = `
            <div class="admin-theme-toast-content">
                <span class="admin-theme-toast-icon" data-theme-icon="${effectiveTheme}">
                    ${this.getThemeIcon(effectiveTheme)}
                </span>
                <span class="admin-theme-toast-text">
                    Switched to ${themeLabel} theme
                </span>
            </div>
        `;

        // Add styles if not already present
        this.addToastStyles();

        // Add to page
        document.body.appendChild(toast);

        // Animate in
        requestAnimationFrame(() => {
            toast.classList.add('admin-theme-toast-show');
        });

        // Remove after delay
        setTimeout(() => {
            toast.classList.remove('admin-theme-toast-show');
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 300);
        }, 2000);
    }

    /**
     * Get theme label for display
     * @param {string} theme - Theme identifier
     * @returns {string} Human-readable theme label
     */
    getThemeLabel(theme) {
        const labels = {
            'light': 'Light',
            'dark': 'Dark',
            'auto': 'Auto'
        };
        return labels[theme] || theme;
    }

    /**
     * Get theme icon HTML
     * @param {string} theme - Theme identifier
     * @returns {string} SVG icon HTML
     */
    getThemeIcon(theme) {
        const icons = {
            'light': `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <circle cx="12" cy="12" r="5"/>
                        <line x1="12" y1="1" x2="12" y2="3"/>
                        <line x1="12" y1="21" x2="12" y2="23"/>
                        <line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/>
                        <line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/>
                        <line x1="1" y1="12" x2="3" y2="12"/>
                        <line x1="21" y1="12" x2="23" y2="12"/>
                        <line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/>
                        <line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
                      </svg>`,
            'dark': `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                       <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
                     </svg>`,
            'auto': `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                       <rect x="2" y="3" width="20" height="14" rx="2" ry="2"/>
                       <line x1="8" y1="21" x2="16" y2="21"/>
                       <line x1="12" y1="17" x2="12" y2="21"/>
                     </svg>`
        };
        return icons[theme] || icons['light'];
    }

    /**
     * Add toast notification styles
     */
    addToastStyles() {
        const styleId = 'admin-theme-toast-styles';

        if (document.getElementById(styleId)) {
            return;
        }

        const styles = document.createElement('style');
        styles.id = styleId;
        styles.textContent = `
            .admin-theme-toast {
                position: fixed;
                top: 20px;
                right: 20px;
                background: var(--admin-bg-elevated);
                border: 1px solid var(--admin-border-primary);
                border-radius: var(--admin-radius-lg);
                padding: 12px 16px;
                box-shadow: var(--admin-shadow-lg);
                z-index: var(--admin-z-toast, 1080);
                transform: translateX(100%);
                transition: all 0.3s ease;
                opacity: 0;
                max-width: 280px;
                color: var(--admin-text-primary);
                font-size: 14px;
                font-weight: 500;
            }

            .admin-theme-toast-show {
                transform: translateX(0);
                opacity: 1;
            }

            .admin-theme-toast-content {
                display: flex;
                align-items: center;
                gap: 8px;
            }

            .admin-theme-toast-icon {
                flex-shrink: 0;
                color: var(--admin-color-accent-600);
            }

            .admin-theme-toast-text {
                flex: 1;
            }

            @media (max-width: 480px) {
                .admin-theme-toast {
                    top: 10px;
                    right: 10px;
                    left: 10px;
                    max-width: none;
                }
            }
        `;

        document.head.appendChild(styles);
    }

    /**
     * Add CSS for smooth theme transitions
     */
    addThemeTransitionStyles() {
        const styleId = 'admin-theme-transition-styles';

        if (document.getElementById(styleId)) {
            return;
        }

        const styles = document.createElement('style');
        styles.id = styleId;
        styles.textContent = `
            .admin-theme-transitioning {
                transition:
                    color 0.3s ease,
                    background-color 0.3s ease,
                    border-color 0.3s ease,
                    box-shadow 0.3s ease !important;
            }

            .admin-theme-transitioning *,
            .admin-theme-transitioning *::before,
            .admin-theme-transitioning *::after {
                transition:
                    color 0.3s ease,
                    background-color 0.3s ease,
                    border-color 0.3s ease,
                    box-shadow 0.3s ease !important;
            }
        `;

        document.head.appendChild(styles);
    }

    /**
     * Debug logging (only in development)
     */
    debugLog(message, data = null) {
        if (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
            console.log(`[AdminTheme] ${message}`, data || '');
        }
    }

    /**
     * Get theme statistics
     * @returns {object} Theme usage statistics
     */
    getThemeStats() {
        return {
            current: this.currentTheme,
            effective: this.getEffectiveTheme(),
            stored: this.getStoredTheme(),
            system: this.getSystemTheme(),
            supportsSystemTheme: !!window.matchMedia,
            timestamp: new Date().toISOString()
        };
    }
}

/**
 * Global theme management utilities
 */
window.AdminTheme = {
    // Theme manager instance
    manager: null,

    /**
     * Initialize the theme system
     */
    init() {
        if (!this.manager) {
            this.manager = new AdminThemeManager();
            this.manager.addThemeTransitionStyles();
        }
        return this.manager;
    },

    /**
     * Set theme
     */
    setTheme(theme) {
        return this.manager?.setTheme(theme) || false;
    },

    /**
     * Get current theme
     */
    getCurrentTheme() {
        return this.manager?.getCurrentTheme() || 'light';
    },

    /**
     * Get effective theme
     */
    getEffectiveTheme() {
        return this.manager?.getEffectiveTheme() || 'light';
    },

    /**
     * Cycle through themes
     */
    cycleTheme() {
        this.manager?.cycleTheme();
    },

    /**
     * Get theme statistics
     */
    getStats() {
        return this.manager?.getThemeStats() || {};
    }
};

/**
 * Initialize theme system when DOM is ready
 */
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.AdminTheme.init();
    });
} else {
    window.AdminTheme.init();
}

/**
 * Export for module systems
 */
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { AdminThemeManager, AdminTheme: window.AdminTheme };
}