// DEVELOPMENT service worker: no offline caching (always go to the network) so code changes show
// up on the first reload. It ALSO actively self-heals: if a previous *published* build left an
// offline-caching service worker + cache installed in this browser, that stale cache can serve
// old .wasm/.dll assets that no longer match the current build - which crashes the Blazor WASM
// renderer with "STATUS_ACCESS_VIOLATION". To prevent that, on activation we take control
// immediately and delete every cache we find, so a stale offline cache can't keep poisoning loads.
self.addEventListener('install', event => {
    // Replace any waiting/old service worker right away instead of waiting for all tabs to close.
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil((async () => {
        // Nuke any caches left behind by a prior published/offline build.
        const keys = await caches.keys();
        await Promise.all(keys.map(key => caches.delete(key)));
        // Start controlling open clients immediately so the next fetch is a clean network fetch.
        await self.clients.claim();
    })());
});

// No fetch interception: everything goes straight to the network (no offline support in dev).
self.addEventListener('fetch', () => { });
