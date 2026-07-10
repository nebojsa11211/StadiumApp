// Stripe.js interop for the fan wallet top-up (async gateway path).
// Loads Stripe.js on demand, mounts a Card Element, and confirms a PaymentIntent client secret.
// The wallet itself is credited server-side by the signed webhook — this only handles card confirmation.
window.stripeWallet = (function () {
    let stripe = null;
    let elements = null;
    let card = null;

    // Load https://js.stripe.com/v3/ once (Stripe requires loading from their domain, not bundled).
    function loadStripeJs() {
        return new Promise((resolve, reject) => {
            if (window.Stripe) { resolve(); return; }
            const existing = document.getElementById('stripe-js');
            if (existing) {
                existing.addEventListener('load', () => resolve());
                existing.addEventListener('error', () => reject(new Error('Stripe.js failed to load')));
                return;
            }
            const s = document.createElement('script');
            s.id = 'stripe-js';
            s.src = 'https://js.stripe.com/v3/';
            s.async = true;
            s.onload = () => resolve();
            s.onerror = () => reject(new Error('Stripe.js failed to load'));
            document.head.appendChild(s);
        });
    }

    return {
        // Load Stripe.js, create a Card Element and mount it into #elementId.
        // Wires validation errors into #errorElementId. Returns true on success; rejects on failure.
        init: async function (publishableKey, elementId, errorElementId) {
            await loadStripeJs();
            if (!window.Stripe) throw new Error('Stripe.js unavailable');

            // Re-initialising (e.g. navigating back to the page) — tear the old element down first.
            if (card) { try { card.unmount(); } catch (e) { /* already gone */ } card = null; }

            stripe = window.Stripe(publishableKey);
            elements = stripe.elements();
            // Element renders in an iframe, so page CSS can't reach its text — theme it here to match the
            // dark wallet surface (default Stripe text is near-black and would be invisible).
            card = elements.create('card', {
                hidePostalCode: true,
                style: {
                    base: {
                        color: '#F4F5F7',
                        fontFamily: 'Inter, system-ui, sans-serif',
                        fontSize: '15px',
                        '::placeholder': { color: '#8A8F98' }
                    },
                    invalid: { color: '#FFB4B4', iconColor: '#FFB4B4' }
                }
            });

            const mountPoint = document.getElementById(elementId);
            if (!mountPoint) throw new Error('Card element mount point not found: ' + elementId);
            card.mount('#' + elementId);

            const errEl = errorElementId ? document.getElementById(errorElementId) : null;
            if (errEl) {
                card.on('change', function (ev) {
                    errEl.textContent = ev.error ? ev.error.message : '';
                });
            }
            return true;
        },

        // Confirm the mounted card against a PaymentIntent client secret.
        // Returns { success, error, status } — never throws, so the caller can render the message.
        confirm: async function (clientSecret) {
            if (!stripe || !card) {
                return { success: false, error: 'Kartično plaćanje nije spremno.', status: null };
            }
            try {
                const result = await stripe.confirmCardPayment(clientSecret, {
                    payment_method: { card: card }
                });
                if (result.error) {
                    return { success: false, error: result.error.message || 'Plaćanje nije uspjelo.', status: null };
                }
                const pi = result.paymentIntent;
                const status = pi ? pi.status : null;
                return {
                    success: status === 'succeeded',
                    error: status === 'succeeded' ? null : ('Status plaćanja: ' + status),
                    status: status
                };
            } catch (e) {
                return { success: false, error: (e && e.message) || 'Plaćanje nije uspjelo.', status: null };
            }
        },

        // Release the Element (called on component dispose).
        destroy: function () {
            if (card) { try { card.unmount(); } catch (e) { /* already gone */ } }
            card = null; elements = null; stripe = null;
        }
    };
})();
