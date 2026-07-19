/**
 * Keyboard Navigation Enhancement Module
 * WCAG 2.1.1 (Keyboard) - Level A
 * WCAG 2.1.2 (No Keyboard Trap) - Level A
 * WCAG 2.4.3 (Focus Order) - Level A
 * WCAG 2.4.7 (Focus Visible) - Level AA
 *
 * This module enhances keyboard navigation throughout the application by:
 * - Managing focus for modals and dialogs (focus trap)
 * - Enabling arrow key navigation in menus and lists
 * - Handling Escape key to close overlays
 * - Supporting Enter/Space for button activation
 * - Managing focus return after closing modals
 * - Preserving natural tab order
 */

(function (window, document) {
    'use strict';

    /**
     * KeyboardNavigation - Main accessibility enhancement class
     */
    class KeyboardNavigation {
        constructor() {
            // Key codes for keyboard events
            this.keys = {
                TAB: 9,
                ENTER: 13,
                ESC: 27,
                SPACE: 32,
                END: 35,
                HOME: 36,
                LEFT: 37,
                UP: 38,
                RIGHT: 39,
                DOWN: 40
            };

            // State management
            this.focusStack = []; // Stack to track focus before modals
            this.trapElements = new Set(); // Active focus traps
            this.menuStates = new Map(); // Menu open/close states

            this.init();
        }

        /**
         * Initialize keyboard navigation enhancements
         */
        init() {
            console.log('[A11y] Initializing keyboard navigation enhancements');

            // Set up global event listeners
            document.addEventListener('keydown', this.handleGlobalKeydown.bind(this));
            document.addEventListener('focusin', this.handleFocusChange.bind(this));

            // Initialize specific navigation patterns
            this.initializeMenuNavigation();
            this.initializeButtonEnhancements();
            this.initializeModalTrap();
            this.initializeDropdowns();

            console.log('[A11y] Keyboard navigation initialized successfully');
        }

        /**
         * Global keydown handler for application-wide shortcuts
         * @param {KeyboardEvent} event - The keyboard event
         */
        handleGlobalKeydown(event) {
            const key = event.keyCode || event.which;

            // Escape key - Close modals, dropdowns, etc.
            if (key === this.keys.ESC) {
                this.handleEscapeKey(event);
            }

            // Check if focus is trapped in a modal
            if (this.trapElements.size > 0) {
                this.handleFocusTrap(event);
            }
        }

        /**
         * Handle focus changes for accessibility announcements
         * @param {FocusEvent} event - The focus event
         */
        handleFocusChange(event) {
            const target = event.target;

            // Announce focused element to screen readers if it has aria-label or title
            if (target.getAttribute('aria-label') || target.title) {
                const label = target.getAttribute('aria-label') || target.title;
                this.announceToScreenReader(`Focused: ${label}`, 'polite');
            }
        }

        /**
         * Initialize menu navigation with arrow keys
         * WCAG 2.1.1 (Keyboard) - Level A
         */
        initializeMenuNavigation() {
            const menus = document.querySelectorAll('[role="menu"], [role="menubar"]');

            menus.forEach(menu => {
                menu.addEventListener('keydown', (event) => {
                    this.handleMenuKeydown(event, menu);
                });
            });

            console.log(`[A11y] Initialized ${menus.length} menu(s) with arrow key navigation`);
        }

        /**
         * Handle keyboard navigation within menus
         * @param {KeyboardEvent} event - The keyboard event
         * @param {HTMLElement} menu - The menu element
         */
        handleMenuKeydown(event, menu) {
            const key = event.keyCode || event.which;
            const menuItems = Array.from(menu.querySelectorAll('[role="menuitem"]:not([disabled])'));
            const currentIndex = menuItems.indexOf(document.activeElement);

            let handled = false;
            let newIndex = currentIndex;

            switch (key) {
                case this.keys.DOWN:
                case this.keys.RIGHT:
                    // Move to next item (wrap around to first)
                    newIndex = (currentIndex + 1) % menuItems.length;
                    handled = true;
                    break;

                case this.keys.UP:
                case this.keys.LEFT:
                    // Move to previous item (wrap around to last)
                    newIndex = currentIndex <= 0 ? menuItems.length - 1 : currentIndex - 1;
                    handled = true;
                    break;

                case this.keys.HOME:
                    // Move to first item
                    newIndex = 0;
                    handled = true;
                    break;

                case this.keys.END:
                    // Move to last item
                    newIndex = menuItems.length - 1;
                    handled = true;
                    break;

                case this.keys.ENTER:
                case this.keys.SPACE:
                    // Activate current menu item
                    if (currentIndex >= 0) {
                        menuItems[currentIndex].click();
                        handled = true;
                    }
                    break;
            }

            if (handled) {
                event.preventDefault();
                event.stopPropagation();

                if (newIndex !== currentIndex && menuItems[newIndex]) {
                    menuItems[newIndex].focus();
                }
            }
        }

        /**
         * Initialize button enhancements for custom elements
         * Ensures elements with role="button" respond to Enter and Space
         */
        initializeButtonEnhancements() {
            const customButtons = document.querySelectorAll('[role="button"]:not(button)');

            customButtons.forEach(button => {
                // Make sure it's focusable
                if (!button.hasAttribute('tabindex')) {
                    button.setAttribute('tabindex', '0');
                }

                // Add keyboard activation
                button.addEventListener('keydown', (event) => {
                    const key = event.keyCode || event.which;

                    if (key === this.keys.ENTER || key === this.keys.SPACE) {
                        event.preventDefault();
                        button.click();
                        this.announceToScreenReader('Activated', 'polite');
                    }
                });
            });

            console.log(`[A11y] Enhanced ${customButtons.length} custom button(s) with keyboard activation`);
        }

        /**
         * Initialize modal focus trap functionality
         * WCAG 2.1.2 (No Keyboard Trap) - Level A (with escape hatch)
         * WCAG 2.4.3 (Focus Order) - Level A
         */
        initializeModalTrap() {
            // Monitor for modal openings
            const observer = new MutationObserver((mutations) => {
                mutations.forEach(mutation => {
                    mutation.addedNodes.forEach(node => {
                        if (node.nodeType === Node.ELEMENT_NODE) {
                            // Check if it's a modal
                            if (node.classList && (node.classList.contains('modal') ||
                                node.getAttribute('role') === 'dialog' ||
                                node.getAttribute('role') === 'alertdialog')) {

                                // Wait for modal to be fully rendered
                                setTimeout(() => {
                                    if (node.classList.contains('show') || node.style.display !== 'none') {
                                        this.trapFocus(node);
                                    }
                                }, 100);
                            }
                        }
                    });

                    mutation.removedNodes.forEach(node => {
                        if (node.nodeType === Node.ELEMENT_NODE && this.trapElements.has(node)) {
                            this.releaseFocusTrap(node);
                        }
                    });
                });
            });

            observer.observe(document.body, {
                childList: true,
                subtree: true
            });

            console.log('[A11y] Modal focus trap monitoring initialized');
        }

        /**
         * Trap focus within a container (typically a modal)
         * @param {HTMLElement} container - The container to trap focus within
         */
        trapFocus(container) {
            // Save current focus to restore later
            this.focusStack.push(document.activeElement);
            this.trapElements.add(container);

            // Get all focusable elements
            const focusableElements = this.getFocusableElements(container);

            if (focusableElements.length === 0) {
                console.warn('[A11y] No focusable elements found in focus trap container');
                return;
            }

            // Focus first element
            focusableElements[0].focus();

            // Store focusable elements on container for later use
            container._focusableElements = focusableElements;

            console.log(`[A11y] Focus trapped in container with ${focusableElements.length} focusable elements`);

            // Announce modal opening
            const modalTitle = container.querySelector('[role="dialog"] h1, [role="dialog"] h2, [role="alertdialog"] h1');
            if (modalTitle) {
                this.announceToScreenReader(`Dialog opened: ${modalTitle.textContent}`, 'assertive');
            }
        }

        /**
         * Handle focus trap keyboard events (Tab and Shift+Tab)
         * @param {KeyboardEvent} event - The keyboard event
         */
        handleFocusTrap(event) {
            const key = event.keyCode || event.which;

            if (key !== this.keys.TAB) {
                return;
            }

            // Get the active trap (last one added)
            const activeTrap = Array.from(this.trapElements)[this.trapElements.size - 1];
            const focusableElements = activeTrap._focusableElements;

            if (!focusableElements || focusableElements.length === 0) {
                return;
            }

            const firstElement = focusableElements[0];
            const lastElement = focusableElements[focusableElements.length - 1];
            const activeElement = document.activeElement;

            // Handle Shift+Tab on first element - wrap to last
            if (event.shiftKey && activeElement === firstElement) {
                event.preventDefault();
                lastElement.focus();
            }
            // Handle Tab on last element - wrap to first
            else if (!event.shiftKey && activeElement === lastElement) {
                event.preventDefault();
                firstElement.focus();
            }
        }

        /**
         * Release focus trap and restore previous focus
         * @param {HTMLElement} container - The container to release
         */
        releaseFocusTrap(container) {
            this.trapElements.delete(container);

            // Restore previous focus
            const previousFocus = this.focusStack.pop();
            if (previousFocus && document.body.contains(previousFocus)) {
                previousFocus.focus();
                console.log('[A11y] Focus restored to previous element');
            }

            // Clean up stored elements
            delete container._focusableElements;

            // Announce modal closing
            this.announceToScreenReader('Dialog closed', 'polite');
        }

        /**
         * Get all focusable elements within a container
         * @param {HTMLElement} container - The container to search
         * @returns {Array<HTMLElement>} Array of focusable elements
         */
        getFocusableElements(container) {
            const focusableSelectors = [
                'a[href]',
                'button:not([disabled])',
                'input:not([disabled])',
                'select:not([disabled])',
                'textarea:not([disabled])',
                '[tabindex]:not([tabindex="-1"])',
                '[role="button"]:not([disabled])'
            ].join(', ');

            return Array.from(container.querySelectorAll(focusableSelectors))
                .filter(el => {
                    // Filter out hidden elements
                    return el.offsetWidth > 0 || el.offsetHeight > 0 || el.getClientRects().length > 0;
                });
        }

        /**
         * Handle Escape key press to close modals and dropdowns
         * @param {KeyboardEvent} event - The keyboard event
         */
        handleEscapeKey(event) {
            // Close active focus trap (modal)
            if (this.trapElements.size > 0) {
                const activeTrap = Array.from(this.trapElements)[this.trapElements.size - 1];

                // Find close button or trigger close
                const closeButton = activeTrap.querySelector('[data-dismiss="modal"], .modal-close, .btn-close');
                if (closeButton) {
                    closeButton.click();
                } else {
                    // Try to close via Bootstrap or custom method
                    if (window.bootstrap && window.bootstrap.Modal) {
                        const modal = window.bootstrap.Modal.getInstance(activeTrap);
                        if (modal) {
                            modal.hide();
                        }
                    }
                }

                event.preventDefault();
                return;
            }

            // Close open dropdowns
            const openDropdowns = document.querySelectorAll('.dropdown.show, .dropdown-menu.show');
            if (openDropdowns.length > 0) {
                openDropdowns.forEach(dropdown => {
                    const button = dropdown.querySelector('[data-bs-toggle="dropdown"]');
                    if (button) {
                        button.click();
                    }
                });
                event.preventDefault();
            }
        }

        /**
         * Initialize dropdown keyboard navigation
         */
        initializeDropdowns() {
            document.addEventListener('keydown', (event) => {
                const dropdown = event.target.closest('.dropdown');
                if (!dropdown) return;

                const key = event.keyCode || event.which;
                const toggle = dropdown.querySelector('[data-bs-toggle="dropdown"]');
                const menu = dropdown.querySelector('.dropdown-menu');
                const isOpen = dropdown.classList.contains('show');

                // Open dropdown with Enter or Space on toggle
                if (event.target === toggle && (key === this.keys.ENTER || key === this.keys.SPACE)) {
                    event.preventDefault();
                    toggle.click();

                    // Focus first menu item after opening
                    setTimeout(() => {
                        const firstItem = menu.querySelector('.dropdown-item:not([disabled])');
                        if (firstItem) {
                            firstItem.focus();
                        }
                    }, 100);
                }

                // Arrow navigation within open dropdown
                if (isOpen && (key === this.keys.UP || key === this.keys.DOWN)) {
                    event.preventDefault();
                    const items = Array.from(menu.querySelectorAll('.dropdown-item:not([disabled])'));
                    const currentIndex = items.indexOf(document.activeElement);
                    let newIndex;

                    if (key === this.keys.DOWN) {
                        newIndex = currentIndex < items.length - 1 ? currentIndex + 1 : 0;
                    } else {
                        newIndex = currentIndex > 0 ? currentIndex - 1 : items.length - 1;
                    }

                    items[newIndex].focus();
                }
            });

            console.log('[A11y] Dropdown keyboard navigation initialized');
        }

        /**
         * Announce message to screen readers using ARIA live region
         * WCAG 4.1.3 (Status Messages) - Level AA
         * @param {string} message - The message to announce
         * @param {string} priority - 'polite' or 'assertive'
         */
        announceToScreenReader(message, priority = 'polite') {
            let announcer = document.getElementById('keyboard-nav-announcer');

            if (!announcer) {
                announcer = document.createElement('div');
                announcer.id = 'keyboard-nav-announcer';
                announcer.className = 'sr-only';
                announcer.setAttribute('role', 'status');
                announcer.setAttribute('aria-live', priority);
                announcer.setAttribute('aria-atomic', 'true');
                document.body.appendChild(announcer);
            }

            // Update priority if different
            announcer.setAttribute('aria-live', priority);

            // Clear and set message with delay for screen reader announcement
            announcer.textContent = '';
            setTimeout(() => {
                announcer.textContent = message;
            }, 100);

            // Clear after announcement
            setTimeout(() => {
                announcer.textContent = '';
            }, 3000);
        }

        /**
         * Add focus trap to a specific element
         * Useful for programmatically creating modals
         * @param {string|HTMLElement} element - Element or selector
         */
        addFocusTrap(element) {
            const el = typeof element === 'string' ? document.querySelector(element) : element;
            if (el) {
                this.trapFocus(el);
            }
        }

        /**
         * Remove focus trap from a specific element
         * @param {string|HTMLElement} element - Element or selector
         */
        removeFocusTrap(element) {
            const el = typeof element === 'string' ? document.querySelector(element) : element;
            if (el) {
                this.releaseFocusTrap(el);
            }
        }
    }

    // Initialize keyboard navigation when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.keyboardNav = new KeyboardNavigation();
        });
    } else {
        window.keyboardNav = new KeyboardNavigation();
    }

    // Export for use in other modules
    window.KeyboardNavigation = KeyboardNavigation;

})(window, document);
