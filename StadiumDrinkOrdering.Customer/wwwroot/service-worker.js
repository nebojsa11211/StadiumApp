/**
 * Stadium Drink Ordering - Service Worker
 * Version: 1.0.0
 *
 * This service worker implements:
 * - Cache-first strategy for static assets (HTML, CSS, JS, images)
 * - Network-first strategy for API calls with fallback
 * - Offline page fallback for navigation requests
 * - Background sync for failed POST requests
 * - Push notification infrastructure
 * - Cache versioning and automatic cleanup
 */

// Cache version - increment this to force cache refresh
const CACHE_VERSION = 'v1.0.0';
const CACHE_NAME = `stadium-orders-${CACHE_VERSION}`;

// Cache names for different resource types
const CACHES = {
    static: `${CACHE_NAME}-static`,
    dynamic: `${CACHE_NAME}-dynamic`,
    images: `${CACHE_NAME}-images`,
    api: `${CACHE_NAME}-api`
};

// Assets to cache immediately on install
const STATIC_ASSETS = [
    '/',
    '/offline.html',
    '/css/site.css',
    '/css/bootstrap/bootstrap.min.css',
    '/css/auth.css',
    '/js/site.js',
    '/manifest.json',
    '/_framework/blazor.server.js'
];

// API endpoints that should use network-first strategy
const API_ROUTES = [
    '/api/drinks',
    '/api/events',
    '/api/orders',
    '/api/customer/ticketing',
    '/api/customer/cart',
    '/api/customer/orders'
];

// Routes that should always try network first
const NETWORK_FIRST_ROUTES = [
    '/login',
    '/register',
    '/checkout',
    '/order-confirmation'
];

/**
 * SERVICE WORKER LIFECYCLE EVENTS
 */

// Install Event - Cache static assets
self.addEventListener('install', (event) => {
    console.log('[Service Worker] Installing service worker...', event);

    event.waitUntil(
        caches.open(CACHES.static).then((cache) => {
            console.log('[Service Worker] Precaching static assets');
            return cache.addAll(STATIC_ASSETS);
        }).catch((error) => {
            console.error('[Service Worker] Failed to cache static assets:', error);
        })
    );

    // Force the waiting service worker to become the active service worker
    self.skipWaiting();
});

// Activate Event - Clean up old caches
self.addEventListener('activate', (event) => {
    console.log('[Service Worker] Activating service worker...', event);

    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    // Delete old cache versions
                    if (!Object.values(CACHES).includes(cacheName)) {
                        console.log('[Service Worker] Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );

    // Take control of all pages immediately
    return self.clients.claim();
});

/**
 * FETCH EVENT - Request interception and caching strategies
 */
self.addEventListener('fetch', (event) => {
    const { request } = event;
    const url = new URL(request.url);

    // Skip cross-origin requests
    if (url.origin !== location.origin) {
        return;
    }

    // API requests - Network-first with cache fallback
    if (isApiRequest(url.pathname)) {
        event.respondWith(networkFirstStrategy(request, CACHES.api));
        return;
    }

    // Network-first routes (auth, checkout)
    if (isNetworkFirstRoute(url.pathname)) {
        event.respondWith(networkFirstStrategy(request, CACHES.dynamic));
        return;
    }

    // Image requests - Cache-first with network fallback
    if (isImageRequest(request)) {
        event.respondWith(cacheFirstStrategy(request, CACHES.images));
        return;
    }

    // Static assets - Cache-first strategy
    if (isStaticAsset(url.pathname)) {
        event.respondWith(cacheFirstStrategy(request, CACHES.static));
        return;
    }

    // Navigation requests - Network-first with offline fallback
    if (request.mode === 'navigate') {
        event.respondWith(
            fetch(request).catch(() => {
                return caches.match('/offline.html');
            })
        );
        return;
    }

    // Default - Network-first with cache fallback
    event.respondWith(networkFirstStrategy(request, CACHES.dynamic));
});

/**
 * CACHING STRATEGIES
 */

// Cache-first: Check cache, fallback to network, update cache
async function cacheFirstStrategy(request, cacheName) {
    const cachedResponse = await caches.match(request);

    if (cachedResponse) {
        console.log('[Service Worker] Cache hit:', request.url);
        return cachedResponse;
    }

    console.log('[Service Worker] Cache miss, fetching:', request.url);

    try {
        const networkResponse = await fetch(request);

        // Cache successful responses
        if (networkResponse && networkResponse.status === 200) {
            const cache = await caches.open(cacheName);
            cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        console.error('[Service Worker] Fetch failed:', error);

        // Return offline page for navigation requests
        if (request.mode === 'navigate') {
            return caches.match('/offline.html');
        }

        throw error;
    }
}

// Network-first: Try network, fallback to cache
async function networkFirstStrategy(request, cacheName) {
    try {
        const networkResponse = await fetch(request);

        // Cache successful responses
        if (networkResponse && networkResponse.status === 200) {
            const cache = await caches.open(cacheName);
            cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        console.log('[Service Worker] Network failed, checking cache:', request.url);

        const cachedResponse = await caches.match(request);

        if (cachedResponse) {
            console.log('[Service Worker] Returning cached response');
            return cachedResponse;
        }

        // Return offline page for navigation requests
        if (request.mode === 'navigate') {
            return caches.match('/offline.html');
        }

        throw error;
    }
}

/**
 * BACKGROUND SYNC - Retry failed requests when online
 */
self.addEventListener('sync', (event) => {
    console.log('[Service Worker] Background sync triggered:', event.tag);

    if (event.tag === 'sync-orders') {
        event.waitUntil(syncOrders());
    }

    if (event.tag === 'sync-cart') {
        event.waitUntil(syncCart());
    }
});

async function syncOrders() {
    console.log('[Service Worker] Syncing pending orders...');

    try {
        // Retrieve pending orders from IndexedDB
        const pendingOrders = await getPendingOrders();

        // Retry each pending order
        for (const order of pendingOrders) {
            try {
                const response = await fetch('/api/customer/orders/create', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(order)
                });

                if (response.ok) {
                    console.log('[Service Worker] Order synced successfully:', order.id);
                    await removePendingOrder(order.id);
                }
            } catch (error) {
                console.error('[Service Worker] Failed to sync order:', error);
            }
        }
    } catch (error) {
        console.error('[Service Worker] Order sync failed:', error);
    }
}

async function syncCart() {
    console.log('[Service Worker] Syncing cart data...');
    // Implement cart sync logic if needed
}

/**
 * PUSH NOTIFICATIONS - Handle push messages
 */
self.addEventListener('push', (event) => {
    console.log('[Service Worker] Push notification received:', event);

    let notificationData = {
        title: 'Stadium Orders',
        body: 'You have a new update',
        icon: '/icons/icon-192x192.png',
        badge: '/icons/badge-72x72.png',
        tag: 'stadium-notification',
        requireInteraction: false
    };

    // Parse push data if available
    if (event.data) {
        try {
            const data = event.data.json();
            notificationData = { ...notificationData, ...data };
        } catch (e) {
            notificationData.body = event.data.text();
        }
    }

    event.waitUntil(
        self.registration.showNotification(notificationData.title, {
            body: notificationData.body,
            icon: notificationData.icon,
            badge: notificationData.badge,
            tag: notificationData.tag,
            requireInteraction: notificationData.requireInteraction,
            data: notificationData.data || {},
            actions: [
                {
                    action: 'view',
                    title: 'View'
                },
                {
                    action: 'close',
                    title: 'Close'
                }
            ]
        })
    );
});

// Handle notification clicks
self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker] Notification clicked:', event);

    event.notification.close();

    if (event.action === 'view' || !event.action) {
        event.waitUntil(
            clients.openWindow(event.notification.data.url || '/')
        );
    }
});

/**
 * HELPER FUNCTIONS
 */

function isApiRequest(pathname) {
    return API_ROUTES.some(route => pathname.startsWith(route));
}

function isNetworkFirstRoute(pathname) {
    return NETWORK_FIRST_ROUTES.some(route => pathname.startsWith(route));
}

function isImageRequest(request) {
    return request.destination === 'image';
}

function isStaticAsset(pathname) {
    return pathname.match(/\.(css|js|woff|woff2|ttf|eot|svg)$/);
}

// IndexedDB helpers for background sync (simplified)
async function getPendingOrders() {
    // TODO: Implement IndexedDB retrieval
    return [];
}

async function removePendingOrder(orderId) {
    // TODO: Implement IndexedDB removal
}

/**
 * MESSAGE HANDLER - Communication with the app
 */
self.addEventListener('message', (event) => {
    console.log('[Service Worker] Message received:', event.data);

    if (event.data && event.data.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }

    if (event.data && event.data.type === 'CLEAR_CACHE') {
        event.waitUntil(
            caches.keys().then((cacheNames) => {
                return Promise.all(
                    cacheNames.map((cacheName) => caches.delete(cacheName))
                );
            })
        );
    }
});
