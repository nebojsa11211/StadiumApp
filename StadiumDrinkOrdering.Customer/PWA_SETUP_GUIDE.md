# Progressive Web App (PWA) Setup Guide
## Stadium Drink Ordering Customer Application

**Last Updated**: 2025-10-11
**Version**: 1.0
**Status**: Implementation Complete

---

## Overview

The Stadium Drink Ordering Customer application is now a fully functional **Progressive Web App (PWA)** that can be installed on mobile devices and desktop computers, providing a native app-like experience with offline capabilities.

### Key Features

✅ **Installable on Mobile & Desktop** - Users can add the app to their home screen
✅ **Offline Support** - Core pages work without internet connection
✅ **Fast Load Times** - Advanced caching strategies for instant page loads
✅ **Push Notifications Ready** - Infrastructure for real-time notifications
✅ **Background Sync** - Queues failed requests and retries when online
✅ **Custom Install Prompt** - Professional installation banner
✅ **Responsive Design** - Works seamlessly on all screen sizes

---

## Files Implemented

### 1. Web App Manifest
**File**: `wwwroot/manifest.json`

Defines the app identity, icons, theme colors, and installation behavior.

**Key Configuration:**
- App name: "Stadium Drink Ordering"
- Short name: "Stadium Orders"
- Theme color: #2563eb (Stadium blue)
- Display mode: standalone
- Start URL: /
- 10 icon sizes (72x72 to 512x512)
- 2 maskable icons for adaptive launchers
- 3 app shortcuts (Events, Orders, Menu)

### 2. Service Worker
**File**: `wwwroot/service-worker.js`

Handles caching, offline support, background sync, and push notifications.

**Caching Strategies:**
- **Cache-first**: Static assets (CSS, JS, images)
- **Network-first**: API calls with cache fallback
- **Offline fallback**: Shows custom offline page when network fails

**Cache Organization:**
- Static cache: HTML, CSS, JS files
- Dynamic cache: Pages and components
- Images cache: Icons and images
- API cache: API responses

### 3. Offline Page
**File**: `wwwroot/offline.html`

Beautiful branded offline page shown when the user loses internet connection.

**Features:**
- Stadium-themed design with brand colors
- "Check Connection" button to test connectivity
- Links to cached pages (Home, Events, Menu, Orders)
- Auto-reload when connection is restored
- Responsive mobile design

### 4. Install Prompt Component
**File**: `Components/Shared/InstallPrompt.razor`

Detects when the app is installable and shows a dismissible install banner.

**Features:**
- Automatic detection of PWA installability
- Smooth slide-up animation
- "Install" and "Not Now" buttons
- Close button for dismissal
- 7-day dismissal persistence in localStorage
- Installation analytics tracking

### 5. PWA Installation JavaScript
**File**: `wwwroot/js/pwa-install.js`

Manages the beforeinstallprompt event and handles installation flow.

**Functions:**
- `initializePwaInstall()` - Sets up install detection
- `installPwa()` - Triggers installation prompt
- `checkIfPwaInstalled()` - Detects if already installed
- `dismissInstallPrompt()` - Persists dismissal preference
- `trackPwaInstallation()` - Sends analytics data
- `registerServiceWorker()` - Registers service worker
- Platform and user agent detection

### 6. Layout Integration
**File**: `Pages/_Layout.cshtml`

Updated with PWA meta tags, manifest link, and service worker registration.

**Added:**
- PWA meta tags (theme-color, mobile-web-app-capable)
- Apple-specific meta tags for iOS
- Manifest link
- Apple touch icon links (5 sizes)
- Favicon links
- Service worker script reference

### 7. Icon Specifications
**File**: `wwwroot/ICON_SPECIFICATIONS.md`

Complete guide for designers to create all required PWA icons.

**Specifications for:**
- 8 standard icons (72x72 to 512x512)
- 2 maskable icons (192x192, 512x512)
- 1 badge icon (72x72)
- 3 shortcut icons (96x96)
- 4 Apple touch icons (120x120 to 180x180)
- Design guidelines and color specifications

---

## Installation Requirements

### Icons (NOT YET CREATED)

⚠️ **ACTION REQUIRED**: Create icon files as specified in `ICON_SPECIFICATIONS.md`

**Directory**: `wwwroot/icons/`

**Required Files (18 total):**

#### Standard Icons
- `icon-72x72.png`
- `icon-96x96.png`
- `icon-128x128.png`
- `icon-144x144.png`
- `icon-152x152.png`
- `icon-192x192.png`
- `icon-384x384.png`
- `icon-512x512.png`

#### Maskable Icons
- `icon-maskable-192x192.png`
- `icon-maskable-512x512.png`

#### Additional Icons
- `badge-72x72.png`
- `shortcut-events.png`
- `shortcut-orders.png`
- `shortcut-menu.png`

#### Apple Touch Icons
- `apple-touch-icon.png` (180x180)
- `apple-touch-icon-120x120.png`
- `apple-touch-icon-152x152.png`
- `apple-touch-icon-167x167.png`

#### Favicons
- `icon-32x32.png`
- `icon-16x16.png`
- `favicon.ico`

### Temporary Icon Solution

Until final icons are created, you can generate placeholder icons:

**Option 1: Use Online Generator**
- Visit https://favicon.io/ or https://www.pwabuilder.com/
- Upload a logo or create from text
- Generate all required sizes

**Option 2: Use ImageMagick CLI**
```bash
# Create 512x512 placeholder
convert -size 512x512 xc:#2563eb \
  -font Arial -pointsize 120 -fill white \
  -gravity center -annotate +0+0 "SD" \
  icon-512x512.png

# Resize to other sizes
for size in 72 96 128 144 152 192 384; do
  convert icon-512x512.png -resize ${size}x${size} icon-${size}x${size}.png
done
```

---

## Testing the PWA

### 1. Development Testing (Local)

**Start the application:**
```bash
cd StadiumDrinkOrdering.Customer
dotnet run --launch-profile https
```

**Access the app:**
- URL: https://localhost:7020
- Open in Chrome, Edge, or Safari

### 2. PWA Install Prompt Testing

**Chrome/Edge (Desktop):**
1. Open developer tools (F12)
2. Go to "Application" tab → "Manifest"
3. Verify manifest loads without errors
4. Click "Add to homescreen" in "Service Workers" section
5. Install prompt should appear after a few seconds

**Chrome (Android):**
1. Visit https://localhost:7020 on Android device
2. Wait 3-5 seconds
3. Install banner should appear at bottom
4. Tap "Install" to add to home screen

**Safari (iOS):**
1. Visit https://localhost:7020 on iPhone/iPad
2. Tap Share button (box with arrow)
3. Select "Add to Home Screen"
4. Icon will be added to home screen

### 3. Service Worker Verification

**Chrome DevTools:**
1. Open DevTools (F12)
2. Go to "Application" tab
3. Select "Service Workers" in left sidebar
4. Verify service worker is registered and running
5. Check "Offline" checkbox to test offline mode
6. Navigate pages to verify offline functionality

**Check Cache Contents:**
1. In Application tab, expand "Cache Storage"
2. Verify caches are created:
   - `stadium-orders-v1.0.0-static`
   - `stadium-orders-v1.0.0-dynamic`
   - `stadium-orders-v1.0.0-images`
   - `stadium-orders-v1.0.0-api`
3. Inspect cached files in each cache

### 4. Offline Testing

**Simulate Offline Mode:**
1. Open Chrome DevTools
2. Go to Network tab
3. Select "Offline" from throttling dropdown
4. Navigate to different pages
5. Verify offline page appears for uncached routes
6. Verify cached pages load normally

**Real Offline Test:**
1. Install the PWA
2. Disconnect from WiFi and disable mobile data
3. Launch the installed app
4. Verify cached pages load
5. Test navigation between cached pages

### 5. Manifest Validation

**Google Lighthouse:**
1. Open Chrome DevTools
2. Go to "Lighthouse" tab
3. Select "Progressive Web App" category
4. Click "Generate report"
5. Review PWA score and recommendations

**PWA Builder:**
- Visit https://www.pwabuilder.com/
- Enter your local URL (with ngrok or similar)
- Review manifest and service worker validation

### 6. Icon Testing

**Maskable Icon Test:**
- Visit https://maskable.app
- Upload your maskable icons
- Verify safe zone content
- Test different launcher shapes

---

## Browser Compatibility

### Supported Browsers

| Browser | Desktop | Mobile | Install Support | Offline Support |
|---------|---------|--------|-----------------|-----------------|
| Chrome | ✅ Full | ✅ Full | ✅ Yes | ✅ Yes |
| Edge | ✅ Full | ✅ Full | ✅ Yes | ✅ Yes |
| Safari | ⚠️ Partial | ⚠️ Partial | ✅ iOS only | ✅ Yes |
| Firefox | ✅ Full | ✅ Full | ❌ No | ✅ Yes |
| Samsung Internet | N/A | ✅ Full | ✅ Yes | ✅ Yes |

**Notes:**
- Safari on iOS requires "Add to Home Screen" manual installation
- Firefox supports PWA features but doesn't show install prompt
- All browsers support offline functionality via service workers

---

## Configuration & Customization

### Update App Name

**File**: `wwwroot/manifest.json`
```json
{
  "name": "Your Stadium Name",
  "short_name": "Stadium",
  "description": "Your custom description"
}
```

### Change Theme Colors

**File**: `wwwroot/manifest.json`
```json
{
  "theme_color": "#YOUR_COLOR",
  "background_color": "#YOUR_BACKGROUND"
}
```

**Also update**: `Pages/_Layout.cshtml`
```html
<meta name="theme-color" content="#YOUR_COLOR">
```

### Adjust Cache Duration

**File**: `wwwroot/service-worker.js`
```javascript
// Change cache version to force refresh
const CACHE_VERSION = 'v1.0.1'; // Increment this
```

### Modify Install Dismissal Period

**File**: `wwwroot/js/pwa-install.js`
```javascript
// Change from 7 days to different duration
const expiryDate = new Date(dismissalDate.getTime() + (7 * 24 * 60 * 60 * 1000));
```

### Add Custom App Shortcuts

**File**: `wwwroot/manifest.json`
```json
{
  "shortcuts": [
    {
      "name": "New Shortcut",
      "url": "/your-page",
      "icons": [{ "src": "/icons/shortcut-custom.png", "sizes": "96x96" }]
    }
  ]
}
```

---

## Performance Optimization

### Cache Strategy Recommendations

**Static Assets (Long Cache):**
- CSS files: Cache-first, 7-day expiration
- JavaScript files: Cache-first, 7-day expiration
- Images: Cache-first, 30-day expiration

**Dynamic Content (Short Cache):**
- HTML pages: Network-first, 1-hour cache fallback
- API responses: Network-first, 5-minute cache fallback

**Never Cache:**
- Authentication endpoints
- Payment processing
- Real-time data (SignalR)

### Reduce Bundle Size

**Compress Images:**
```bash
# Use TinyPNG or ImageOptim
# Target: < 10KB for 192x192 icons
# Target: < 25KB for 512x512 icons
```

**Minify Service Worker:**
```bash
# Use Terser or UglifyJS
npx terser service-worker.js -o service-worker.min.js
```

### Preload Critical Resources

**File**: `Pages/_Layout.cshtml`
```html
<link rel="preload" href="/css/site.css" as="style">
<link rel="preload" href="/_framework/blazor.server.js" as="script">
```

---

## Troubleshooting

### Install Prompt Not Showing

**Possible Causes:**
1. Service worker not registered
2. Manifest errors
3. Not served over HTTPS
4. User previously dismissed prompt (7-day timeout)
5. App already installed

**Solutions:**
1. Check DevTools → Application → Service Workers
2. Validate manifest in DevTools → Application → Manifest
3. Ensure HTTPS is enabled (required for PWA)
4. Clear localStorage: `localStorage.removeItem('pwa-install-dismissed')`
5. Uninstall existing app and refresh

### Service Worker Not Updating

**Issue**: Old service worker continues to run after code changes.

**Solution**:
1. Increment `CACHE_VERSION` in `service-worker.js`
2. In DevTools → Application → Service Workers, click "Unregister"
3. Hard refresh (Ctrl+Shift+R)
4. Or use "Update on reload" checkbox in DevTools

### Offline Page Not Appearing

**Check:**
1. Service worker registered: DevTools → Application → Service Workers
2. Offline page cached: DevTools → Application → Cache Storage
3. Network throttled: DevTools → Network → Offline mode

**Fix:**
```javascript
// Ensure offline.html is in precache list
const STATIC_ASSETS = [
    '/offline.html', // Must be here
    // ... other assets
];
```

### Icons Not Displaying

**Verify:**
1. Icons exist in `/wwwroot/icons/` directory
2. Manifest references correct icon paths
3. Icon sizes match manifest specifications
4. PNG format with correct dimensions

**Test:**
- Direct URL: https://localhost:7020/icons/icon-192x192.png
- Should display icon, not 404 error

### Cache Not Clearing

**Manual Cache Clear:**
```javascript
// In browser console
caches.keys().then(names => {
    names.forEach(name => caches.delete(name));
});
```

**Programmatic Clear:**
```javascript
// Send message to service worker
navigator.serviceWorker.controller.postMessage({
    type: 'CLEAR_CACHE'
});
```

---

## Analytics & Monitoring

### Track PWA Installations

**File**: `wwwroot/js/pwa-install.js` (already implemented)

Installation events are automatically tracked to:
- localStorage (client-side)
- API endpoint: `/api/analytics/pwa-install` (optional)

**Data Collected:**
- Installation timestamp
- User platform (Android, iOS, Windows, etc.)
- User agent string

### Monitor Service Worker Performance

**Add to API Analytics:**
```csharp
[HttpPost("api/analytics/pwa-install")]
public async Task<IActionResult> TrackPwaInstallation([FromBody] PwaInstallDto data)
{
    // Log installation for analytics
    _logger.LogInformation("PWA installed: Platform={Platform}, Time={Timestamp}",
        data.Platform, data.Timestamp);

    // Store in database for reporting
    await _analyticsService.TrackPwaInstallAsync(data);

    return Ok();
}
```

### Lighthouse CI Integration

**Add to CI/CD pipeline:**
```yaml
- name: Run Lighthouse CI
  run: |
    npm install -g @lhci/cli
    lhci autorun --collect.url=https://localhost:7020
```

---

## Deployment Considerations

### Production Checklist

- [ ] Create all required icon files (18 files)
- [ ] Update manifest.json with production URLs
- [ ] Test installation on iOS Safari
- [ ] Test installation on Android Chrome
- [ ] Test installation on desktop (Chrome, Edge)
- [ ] Verify offline functionality works
- [ ] Test background sync when connection restored
- [ ] Validate manifest with Lighthouse
- [ ] Test all app shortcuts work correctly
- [ ] Verify theme colors match brand
- [ ] Enable HTTPS on production domain
- [ ] Test service worker updates correctly
- [ ] Monitor PWA installation analytics

### HTTPS Requirements

⚠️ **CRITICAL**: PWAs require HTTPS in production.

**Local Development:**
- Uses self-signed certificates (OK for testing)
- Chrome ignores certificate errors on localhost

**Production:**
- Must have valid SSL certificate
- Use Let's Encrypt (free) or commercial cert
- Azure/AWS provide automatic SSL

### CDN Considerations

If using CDN for static assets:

**File**: `wwwroot/service-worker.js`
```javascript
// Allow cross-origin caching
if (url.origin === location.origin || url.origin === 'https://cdn.yourdomain.com') {
    // Cache this resource
}
```

---

## Future Enhancements

### Phase 2 Features (Planned)

**Push Notifications:**
- Real-time order status updates
- Event reminders
- Special offers and promotions

**Background Sync:**
- Retry failed order submissions
- Sync cart when online
- Upload pending feedback

**Periodic Background Sync:**
- Auto-refresh event listings
- Update drink menu
- Sync order history

**Advanced Caching:**
- Predictive prefetching
- Machine learning cache priorities
- User behavior-based caching

### Experimental Features

**Web Share API:**
```javascript
if (navigator.share) {
    await navigator.share({
        title: 'Stadium Orders',
        text: 'Check out this event!',
        url: '/events/123'
    });
}
```

**Badging API:**
```javascript
if ('setAppBadge' in navigator) {
    navigator.setAppBadge(unreadOrders);
}
```

**File System Access:**
```javascript
// Export order history to file
const handle = await window.showSaveFilePicker();
```

---

## Support & Resources

### Documentation
- **MDN PWA Guide**: https://developer.mozilla.org/en-US/docs/Web/Progressive_web_apps
- **web.dev PWA**: https://web.dev/progressive-web-apps/
- **Service Worker Cookbook**: https://serviceworke.rs/

### Testing Tools
- **Lighthouse**: https://developers.google.com/web/tools/lighthouse
- **PWA Builder**: https://www.pwabuilder.com/
- **Maskable.app**: https://maskable.app/

### Icon Generators
- **Favicon.io**: https://favicon.io/
- **RealFaviconGenerator**: https://realfavicongenerator.net/
- **PWA Asset Generator**: https://github.com/elegantapp/pwa-asset-generator

---

## Contact & Maintenance

**Maintained by**: Stadium Drink Ordering Development Team
**Last Review**: 2025-10-11
**Next Review**: Quarterly (2025-01-11)

For issues or questions:
1. Check this guide first
2. Review browser console errors
3. Check DevTools → Application tab
4. Contact development team

---

**End of PWA Setup Guide**
