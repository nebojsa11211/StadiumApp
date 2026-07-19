/**
 * ARIA Enhancements Module
 * WCAG 4.1.2 (Name, Role, Value) - Level A
 * WCAG 4.1.3 (Status Messages) - Level AA
 *
 * This module enhances ARIA attributes dynamically throughout the application:
 * - Manages aria-live regions for status announcements
 * - Updates aria-expanded, aria-selected, aria-checked states
 * - Provides ARIA labeling for dynamic content
 * - Announces loading states, errors, and success messages
 * - Manages aria-busy for asynchronous operations
 * - Implements proper ARIA roles for custom components
 */

(function (window, document) {
    'use strict';

    /**
     * ARIAEnhancements - Dynamic ARIA attribute management
     */
    class ARIAEnhancements {
        constructor() {
            // ARIA live region types
            this.liveRegions = {
                polite: null,    // Non-critical announcements
                assertive: null, // Critical announcements
                status: null     // Status updates
            };

            // State tracking
            this.loadingElements = new Set();
            this.expandableElements = new Map();

            this.init();
        }

        /**
         * Initialize ARIA enhancements
         */
        init() {
            console.log('[A11y] Initializing ARIA enhancements');

            // Create live regions
            this.createLiveRegions();

            // Set up mutation observer for dynamic content
            this.observeContentChanges();

            // Initialize existing elements
            this.enhanceExistingElements();

            // Set up event listeners
            this.setupEventListeners();

            console.log('[A11y] ARIA enhancements initialized successfully');
        }

        /**
         * Create ARIA live regions for announcements
         * WCAG 4.1.3 (Status Messages) - Level AA
         */
        createLiveRegions() {
            // Create polite live region for non-critical updates
            this.liveRegions.polite = this.createLiveRegion('aria-live-polite', 'polite', 'status');

            // Create assertive live region for critical updates
            this.liveRegions.assertive = this.createLiveRegion('aria-live-assertive', 'assertive', 'alert');

            // Create status region for status updates
            this.liveRegions.status = this.createLiveRegion('aria-live-status', 'polite', 'status');

            console.log('[A11y] Created 3 ARIA live regions for announcements');
        }

        /**
         * Create a single live region
         * @param {string} id - Unique ID for the region
         * @param {string} liveValue - 'polite' or 'assertive'
         * @param {string} role - ARIA role ('status', 'alert', etc.)
         * @returns {HTMLElement} The created live region
         */
        createLiveRegion(id, liveValue, role) {
            let region = document.getElementById(id);

            if (!region) {
                region = document.createElement('div');
                region.id = id;
                region.className = 'sr-only';
                region.setAttribute('role', role);
                region.setAttribute('aria-live', liveValue);
                region.setAttribute('aria-atomic', 'true');
                document.body.appendChild(region);
            }

            return region;
        }

        /**
         * Announce message to screen readers
         * @param {string} message - The message to announce
         * @param {string} priority - 'polite' (default), 'assertive', or 'status'
         */
        announce(message, priority = 'polite') {
            const region = this.liveRegions[priority] || this.liveRegions.polite;

            // Clear previous message
            region.textContent = '';

            // Set new message with delay to ensure screen readers pick it up
            setTimeout(() => {
                region.textContent = message;
                console.log(`[A11y] Announced (${priority}): ${message}`);
            }, 100);

            // Clear message after announcement
            setTimeout(() => {
                region.textContent = '';
            }, 3000);
        }

        /**
         * Announce loading state
         * @param {string} elementId - ID of element that's loading
         * @param {string} message - Custom loading message
         */
        announceLoading(elementId, message = 'Loading...') {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-busy', 'true');
                this.loadingElements.add(element);
                this.announce(message, 'polite');
            }
        }

        /**
         * Announce loading complete
         * @param {string} elementId - ID of element that finished loading
         * @param {string} message - Custom completion message
         */
        announceLoadingComplete(elementId, message = 'Loading complete') {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-busy', 'false');
                this.loadingElements.delete(element);
                this.announce(message, 'polite');
            }
        }

        /**
         * Announce error message
         * @param {string} message - Error message
         * @param {string} elementId - Optional element ID associated with error
         */
        announceError(message, elementId = null) {
            // Use assertive priority for errors
            this.announce(`Error: ${message}`, 'assertive');

            // If element provided, mark it as invalid
            if (elementId) {
                const element = document.getElementById(elementId);
                if (element) {
                    element.setAttribute('aria-invalid', 'true');

                    // Create or update error message element
                    const errorId = `${elementId}-error`;
                    let errorElement = document.getElementById(errorId);

                    if (!errorElement) {
                        errorElement = document.createElement('div');
                        errorElement.id = errorId;
                        errorElement.className = 'invalid-feedback';
                        errorElement.setAttribute('role', 'alert');
                        element.parentNode.appendChild(errorElement);
                    }

                    errorElement.textContent = message;
                    element.setAttribute('aria-describedby', errorId);
                }
            }
        }

        /**
         * Clear error state
         * @param {string} elementId - Element ID to clear error from
         */
        clearError(elementId) {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-invalid', 'false');
                element.removeAttribute('aria-describedby');

                const errorElement = document.getElementById(`${elementId}-error`);
                if (errorElement) {
                    errorElement.remove();
                }
            }
        }

        /**
         * Announce success message
         * @param {string} message - Success message
         */
        announceSuccess(message) {
            this.announce(`Success: ${message}`, 'polite');
        }

        /**
         * Update expandable element state
         * @param {string} elementId - ID of expandable element
         * @param {boolean} expanded - Whether element is expanded
         */
        updateExpandedState(elementId, expanded) {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-expanded', expanded.toString());
                this.expandableElements.set(element, expanded);

                // Announce state change
                const label = element.getAttribute('aria-label') || element.textContent;
                this.announce(`${label} ${expanded ? 'expanded' : 'collapsed'}`, 'polite');
            }
        }

        /**
         * Update selected state for selectable items
         * @param {string} elementId - ID of selectable element
         * @param {boolean} selected - Whether element is selected
         */
        updateSelectedState(elementId, selected) {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-selected', selected.toString());

                // If in a list, ensure aria-current is set
                if (selected) {
                    // Remove aria-current from siblings
                    const parent = element.parentElement;
                    if (parent) {
                        parent.querySelectorAll('[aria-selected="true"]').forEach(sibling => {
                            if (sibling !== element) {
                                sibling.setAttribute('aria-selected', 'false');
                                sibling.removeAttribute('aria-current');
                            }
                        });
                    }

                    element.setAttribute('aria-current', 'true');
                } else {
                    element.removeAttribute('aria-current');
                }
            }
        }

        /**
         * Update checked state for checkboxes and radio buttons
         * @param {string} elementId - ID of checkbox/radio element
         * @param {boolean} checked - Whether element is checked
         */
        updateCheckedState(elementId, checked) {
            const element = document.getElementById(elementId);

            if (element) {
                element.setAttribute('aria-checked', checked.toString());

                // For custom checkboxes, update visual state
                if (element.getAttribute('role') === 'checkbox' || element.getAttribute('role') === 'radio') {
                    const label = element.getAttribute('aria-label') || element.textContent;
                    this.announce(`${label} ${checked ? 'checked' : 'unchecked'}`, 'polite');
                }
            }
        }

        /**
         * Enhance form inputs with ARIA labels
         * @param {HTMLElement} form - The form element to enhance
         */
        enhanceForm(form) {
            const inputs = form.querySelectorAll('input, select, textarea');

            inputs.forEach(input => {
                // Ensure input has an associated label
                const label = form.querySelector(`label[for="${input.id}"]`);
                if (label && !input.getAttribute('aria-label') && !input.getAttribute('aria-labelledby')) {
                    input.setAttribute('aria-labelledby', label.id || this.generateId('label'));
                }

                // Add aria-required for required fields
                if (input.hasAttribute('required') && !input.getAttribute('aria-required')) {
                    input.setAttribute('aria-required', 'true');
                }

                // Add aria-describedby for help text
                const helpText = input.parentElement.querySelector('.form-text, .help-text');
                if (helpText && !input.getAttribute('aria-describedby')) {
                    const helpId = helpText.id || this.generateId('help');
                    helpText.id = helpId;
                    input.setAttribute('aria-describedby', helpId);
                }
            });

            console.log(`[A11y] Enhanced form with ${inputs.length} inputs`);
        }

        /**
         * Enhance navigation menu with ARIA attributes
         * @param {HTMLElement} nav - The navigation element
         */
        enhanceNavigation(nav) {
            // Set navigation role if not present
            if (!nav.getAttribute('role')) {
                nav.setAttribute('role', 'navigation');
            }

            // Set aria-label for navigation
            if (!nav.getAttribute('aria-label') && !nav.getAttribute('aria-labelledby')) {
                nav.setAttribute('aria-label', 'Main navigation');
            }

            // Enhance navigation items
            const navItems = nav.querySelectorAll('a, button');
            navItems.forEach(item => {
                // Mark current page
                if (window.location.pathname === item.getAttribute('href')) {
                    item.setAttribute('aria-current', 'page');
                }

                // Ensure menu items have proper role
                const listItem = item.closest('li');
                if (listItem && !listItem.getAttribute('role')) {
                    listItem.setAttribute('role', 'none');
                }
            });

            console.log('[A11y] Enhanced navigation with ARIA attributes');
        }

        /**
         * Enhance button elements with proper ARIA labels
         * @param {HTMLElement} container - Container to search for buttons
         */
        enhanceButtons(container = document) {
            const buttons = container.querySelectorAll('button:not([aria-label])');

            buttons.forEach(button => {
                // Skip if already has aria-label or aria-labelledby
                if (button.getAttribute('aria-label') || button.getAttribute('aria-labelledby')) {
                    return;
                }

                // Check for text content
                const text = button.textContent.trim();
                if (text) {
                    button.setAttribute('aria-label', text);
                    return;
                }

                // Check for icon-only buttons
                const icon = button.querySelector('i, svg, .icon');
                if (icon) {
                    const title = icon.getAttribute('title') || button.getAttribute('title');
                    if (title) {
                        button.setAttribute('aria-label', title);
                    } else {
                        console.warn('[A11y] Button without accessible label:', button);
                    }
                }
            });
        }

        /**
         * Enhance existing elements on page
         */
        enhanceExistingElements() {
            // Enhance forms
            document.querySelectorAll('form').forEach(form => {
                this.enhanceForm(form);
            });

            // Enhance navigation
            document.querySelectorAll('nav').forEach(nav => {
                this.enhanceNavigation(nav);
            });

            // Enhance buttons
            this.enhanceButtons();

            // Enhance cards with clickable areas
            document.querySelectorAll('.card[onclick], .card[data-href]').forEach(card => {
                if (!card.getAttribute('role')) {
                    card.setAttribute('role', 'button');
                }
                if (!card.getAttribute('tabindex')) {
                    card.setAttribute('tabindex', '0');
                }
            });

            console.log('[A11y] Enhanced existing page elements');
        }

        /**
         * Observe content changes and enhance new elements
         */
        observeContentChanges() {
            const observer = new MutationObserver((mutations) => {
                mutations.forEach(mutation => {
                    mutation.addedNodes.forEach(node => {
                        if (node.nodeType === Node.ELEMENT_NODE) {
                            // Enhance new forms
                            if (node.tagName === 'FORM') {
                                this.enhanceForm(node);
                            } else {
                                node.querySelectorAll('form').forEach(form => {
                                    this.enhanceForm(form);
                                });
                            }

                            // Enhance new buttons
                            this.enhanceButtons(node);

                            // Enhance new navigation
                            if (node.tagName === 'NAV') {
                                this.enhanceNavigation(node);
                            }
                        }
                    });
                });
            });

            observer.observe(document.body, {
                childList: true,
                subtree: true
            });

            console.log('[A11y] Content change observation active');
        }

        /**
         * Set up event listeners for state changes
         */
        setupEventListeners() {
            // Listen for Bootstrap collapse events
            document.addEventListener('show.bs.collapse', (event) => {
                const trigger = document.querySelector(`[data-bs-target="#${event.target.id}"]`);
                if (trigger) {
                    this.updateExpandedState(trigger.id, true);
                }
            });

            document.addEventListener('hide.bs.collapse', (event) => {
                const trigger = document.querySelector(`[data-bs-target="#${event.target.id}"]`);
                if (trigger) {
                    this.updateExpandedState(trigger.id, false);
                }
            });

            // Listen for Bootstrap modal events
            document.addEventListener('shown.bs.modal', (event) => {
                const modalTitle = event.target.querySelector('.modal-title');
                if (modalTitle) {
                    this.announce(`Dialog opened: ${modalTitle.textContent}`, 'polite');
                }
            });

            document.addEventListener('hidden.bs.modal', () => {
                this.announce('Dialog closed', 'polite');
            });

            console.log('[A11y] Event listeners configured');
        }

        /**
         * Generate unique ID for elements
         * @param {string} prefix - ID prefix
         * @returns {string} Unique ID
         */
        generateId(prefix = 'aria') {
            return `${prefix}-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
        }

        /**
         * Update page title and announce to screen readers
         * @param {string} title - New page title
         */
        updatePageTitle(title) {
            document.title = title;
            this.announce(`Page loaded: ${title}`, 'polite');
        }

        /**
         * Announce page navigation
         * @param {string} pageName - Name of the page navigated to
         */
        announceNavigation(pageName) {
            this.announce(`Navigated to ${pageName}`, 'polite');
        }

        /**
         * Create progress announcement for multi-step processes
         * @param {number} current - Current step
         * @param {number} total - Total steps
         * @param {string} stepName - Name of current step
         */
        announceProgress(current, total, stepName) {
            this.announce(`Step ${current} of ${total}: ${stepName}`, 'polite');
        }

        /**
         * Announce item count for lists and collections
         * @param {number} count - Number of items
         * @param {string} itemType - Type of items (e.g., "events", "orders")
         */
        announceItemCount(count, itemType) {
            const message = count === 0
                ? `No ${itemType} found`
                : count === 1
                    ? `1 ${itemType} found`
                    : `${count} ${itemType} found`;

            this.announce(message, 'polite');
        }
    }

    // Initialize ARIA enhancements when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.ariaEnhancements = new ARIAEnhancements();
        });
    } else {
        window.ariaEnhancements = new ARIAEnhancements();
    }

    // Export for use in other modules
    window.ARIAEnhancements = ARIAEnhancements;

})(window, document);
