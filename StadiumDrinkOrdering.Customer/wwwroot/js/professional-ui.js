// Professional UI Enhancements for Stadium Drinks
// Modern, interactive elements and smooth animations

document.addEventListener('DOMContentLoaded', function() {
    initializeProfessionalUI();
});

function initializeProfessionalUI() {
    // Initialize scroll animations
    initializeScrollAnimations();
    
    // Initialize interactive cards
    initializeInteractiveCards();
    
    // Initialize smooth scrolling
    initializeSmoothScrolling();
    
    // Initialize loading states
    initializeLoadingStates();
    
    // Initialize form enhancements
    initializeFormEnhancements();
}

// Scroll-triggered animations
function initializeScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
            }
        });
    }, observerOptions);

    // Observe elements for scroll animations
    document.querySelectorAll('.scroll-animate').forEach(el => {
        observer.observe(el);
    });
}

// Interactive card hover effects
function initializeInteractiveCards() {
    const cards = document.querySelectorAll('.card');
    
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-8px) scale(1.02)';
            this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
}

// Smooth scrolling for anchor links
function initializeSmoothScrolling() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Enhanced loading states
function initializeLoadingStates() {
    const buttons = document.querySelectorAll('.btn-loading');
    
    buttons.forEach(button => {
        const originalText = button.innerHTML;
        
        button.addEventListener('click', function() {
            if (!this.disabled) {
                this.innerHTML = '<span class="loading-spinner"></span><span class="ml-2">Loading...</span>';
                this.disabled = true;
                
                // Re-enable after 3 seconds (or when operation completes)
                setTimeout(() => {
                    this.innerHTML = originalText;
                    this.disabled = false;
                }, 3000);
            }
        });
    });
}

// Form field enhancements
function initializeFormEnhancements() {
    // Floating labels
    const formGroups = document.querySelectorAll('.form-group');
    
    formGroups.forEach(group => {
        const input = group.querySelector('.form-control');
        const label = group.querySelector('.form-label');
        
        if (input && label) {
            input.addEventListener('focus', function() {
                label.classList.add('floating');
                group.classList.add('focused');
            });
            
            input.addEventListener('blur', function() {
                if (!this.value) {
                    label.classList.remove('floating');
                }
                group.classList.remove('focused');
            });
            
            // Check if field has value on load
            if (input.value) {
                label.classList.add('floating');
            }
        }
    });
    
    // Input validation feedback
    const inputs = document.querySelectorAll('.form-control');
    
    inputs.forEach(input => {
        input.addEventListener('input', function() {
            if (this.checkValidity()) {
                this.classList.remove('invalid');
                this.classList.add('valid');
            } else {
                this.classList.remove('valid');
                this.classList.add('invalid');
            }
        });
    });
}

// Add CSS for enhanced animations
const style = document.createElement('style');
style.textContent = `
    /* Enhanced loading spinner */
    .loading-spinner {
        display: inline-block;
        width: 20px;
        height: 20px;
        border: 2px solid #ffffff;
        border-radius: 50%;
        border-top-color: transparent;
        animation: spin 1s linear infinite;
    }
    
    /* Floating label animation */
    .form-label.floating {
        transform: translateY(-1.5rem) scale(0.85);
        color: var(--stadium-green-600);
        font-weight: 600;
    }
    
    .form-group.focused .form-control {
        border-color: var(--stadium-green-500);
        box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.1);
    }
    
    /* Scroll animation classes */
    .scroll-animate {
        opacity: 0;
        transform: translateY(30px);
        transition: all 0.6s cubic-bezier(0.4, 0, 0.2, 1);
    }
    
    .scroll-animate.animate-in {
        opacity: 1;
        transform: translateY(0);
    }
    
    /* Enhanced card interactions */
    .card {
        position: relative;
        overflow: hidden;
    }
    
    .card::before {
        content: '';
        position: absolute;
        top: 0;
        left: -100%;
        width: 100%;
        height: 100%;
        background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
        transition: left 0.5s;
    }
    
    .card:hover::before {
        left: 100%;
    }
    
    /* Premium shimmer effect */
    .premium-shimmer {
        background: linear-gradient(90deg, #fbbf24, #f59e0b, #fbbf24);
        background-size: 200% 100%;
        animation: shimmer 2s infinite;
    }
    
    @keyframes shimmer {
        0% { background-position: -200% 0; }
        100% { background-position: 200% 0; }
    }
    
    /* Enhanced button interactions */
    .btn {
        position: relative;
        overflow: hidden;
    }
    
    .btn::after {
        content: '';
        position: absolute;
        top: 50%;
        left: 50%;
        width: 0;
        height: 0;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.3);
        transform: translate(-50%, -50%);
        transition: width 0.6s, height 0.6s;
    }
    
    .btn:active::after {
        width: 300px;
        height: 300px;
    }
    
    /* Mobile menu animations */
    .mobile-menu {
        max-height: 0;
        overflow: hidden;
        transition: max-height 0.3s ease-out;
    }
    
    .mobile-menu.open {
        max-height: 500px;
    }
    
    /* Enhanced focus states */
    .btn:focus-visible,
    .form-control:focus-visible,
    .nav-link:focus-visible {
        outline: 2px solid var(--stadium-green-500);
        outline-offset: 2px;
    }
    
    /* Custom scrollbar */
    ::-webkit-scrollbar {
        width: 8px;
    }
    
    ::-webkit-scrollbar-track {
        background: var(--gray-100);
    }
    
    ::-webkit-scrollbar-thumb {
        background: var(--gray-400);
        border-radius: 4px;
    }
    
    ::-webkit-scrollbar-thumb:hover {
        background: var(--gray-500);
    }
`;

document.head.appendChild(style);

// Utility functions
window.professionalUI = {
    // Show toast notification
    showToast: function(message, type = 'info') {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.textContent = message;
        document.body.appendChild(toast);
        
        setTimeout(() => {
            toast.classList.add('show');
        }, 100);
        
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(toast);
            }, 300);
        }, 3000);
    },
    
    // Smooth scroll to element
    scrollToElement: function(selector) {
        const element = document.querySelector(selector);
        if (element) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    },
    
    // Add loading state to button
    setButtonLoading: function(button, loading = true) {
        if (loading) {
            button.classList.add('btn-loading');
            button.disabled = true;
        } else {
            button.classList.remove('btn-loading');
            button.disabled = false;
        }
    }
};

// Enhanced animations for premium elements
function initializePremiumAnimations() {
    // Premium card hover effects
    const premiumCards = document.querySelectorAll('.card:has(.badge-premium)');
    
    premiumCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.classList.add('premium-shimmer');
        });
        
        card.addEventListener('mouseleave', function() {
            this.classList.remove('premium-shimmer');
        });
    });
}

// Initialize premium animations
setTimeout(initializePremiumAnimations, 1000);
