// Bootstrap Modal Helper Functions with Enhanced Debugging
window.showBootstrapModal = function (modalId) {
    console.log('🔍 showBootstrapModal called with modalId:', modalId);

    // Check if Bootstrap is available
    if (typeof bootstrap === 'undefined') {
        console.error('❌ Bootstrap is not loaded!');
        alert('Error: Bootstrap is not loaded. Please refresh the page.');
        return;
    }

    const modalElement = document.getElementById(modalId);
    console.log('🔍 Modal element found:', modalElement);

    if (modalElement) {
        try {
            console.log('🔍 Creating Bootstrap modal instance...');
            const modal = new bootstrap.Modal(modalElement, {
                backdrop: true,
                keyboard: true,
                focus: true
            });
            console.log('✅ Bootstrap modal instance created:', modal);

            console.log('🔍 Showing modal...');
            modal.show();
            console.log('✅ Modal.show() called successfully');

            // Additional debugging - check if modal actually becomes visible
            setTimeout(() => {
                const isVisible = modalElement.classList.contains('show');
                console.log('🔍 Modal visibility check after 500ms:', isVisible);
                if (!isVisible) {
                    console.error('❌ Modal is not visible after show() was called');
                }
            }, 500);

        } catch (error) {
            console.error('❌ Error creating or showing modal:', error);
            alert('Error showing modal: ' + error.message);
        }
    } else {
        console.error('❌ Modal element not found with ID:', modalId);
        alert('Error: Modal element not found: ' + modalId);
    }
};

// Alternative modal show methods
window.forceShowModal = function (modalId) {
    console.log('🔍 forceShowModal called with modalId:', modalId);

    const modalElement = document.getElementById(modalId);
    if (!modalElement) {
        console.error('❌ Modal element not found');
        return false;
    }

    // Method 1: Bootstrap 5 way
    try {
        if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
            console.log('🔍 Trying Bootstrap 5 method...');
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.show();
            return true;
        }
    } catch (e) {
        console.log('❌ Bootstrap 5 method failed:', e);
    }

    // Method 2: jQuery way (if available)
    try {
        if (typeof $ !== 'undefined' && $.fn.modal) {
            console.log('🔍 Trying jQuery method...');
            $(modalElement).modal('show');
            return true;
        }
    } catch (e) {
        console.log('❌ jQuery method failed:', e);
    }

    // Method 3: Manual show with CSS classes
    try {
        console.log('🔍 Trying manual CSS method...');
        modalElement.classList.add('show');
        modalElement.style.display = 'block';
        modalElement.setAttribute('aria-hidden', 'false');
        document.body.classList.add('modal-open');

        // Add backdrop
        let backdrop = document.querySelector('.modal-backdrop');
        if (!backdrop) {
            backdrop = document.createElement('div');
            backdrop.className = 'modal-backdrop fade show';
            document.body.appendChild(backdrop);
        }
        return true;
    } catch (e) {
        console.log('❌ Manual method failed:', e);
    }

    return false;
};

window.hideBootstrapModal = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        }
    }
};

window.toggleBootstrapModal = function (modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        modal.toggle();
    }
};