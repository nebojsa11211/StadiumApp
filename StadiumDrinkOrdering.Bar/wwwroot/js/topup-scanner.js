// Ticket top-up QR scanner (bar counter). Uses the browser-native BarcodeDetector API together with
// getUserMedia so no third-party library or CDN asset is needed. When BarcodeDetector is unavailable
// the caller is told (supported:false) and falls back to manual code entry. getUserMedia requires a
// secure context — this app is HTTPS-only, so it works on localhost/https.
window.barTopupScanner = (function () {
    let stream = null;
    let detector = null;
    let dotNetRef = null;
    let videoEl = null;
    let loopHandle = null;
    let running = false;

    function releaseCamera() {
        running = false;
        if (loopHandle) {
            clearTimeout(loopHandle);
            loopHandle = null;
        }
        if (stream) {
            // Stop every track so the camera indicator turns off and the device is released.
            stream.getTracks().forEach(function (t) { t.stop(); });
            stream = null;
        }
        if (videoEl) {
            try { videoEl.pause(); } catch (e) { /* ignore */ }
            videoEl.srcObject = null;
            videoEl = null;
        }
        dotNetRef = null;
        detector = null;
    }

    async function scanTick() {
        if (!running || !detector || !videoEl) return;
        try {
            const codes = await detector.detect(videoEl);
            if (codes && codes.length > 0) {
                const value = codes[0].rawValue;
                if (value && dotNetRef) {
                    const ref = dotNetRef;
                    // Single-shot: hand the decoded text to .NET, then release the camera.
                    releaseCamera();
                    try {
                        await ref.invokeMethodAsync('OnQrDecoded', value);
                    } catch (e) {
                        console.warn('barTopupScanner: OnQrDecoded invoke failed', e);
                    }
                    return;
                }
            }
        } catch (e) {
            // Transient detect errors (e.g. video not ready yet) shouldn't kill the loop.
            console.warn('barTopupScanner: detect failed', e);
        }
        if (running) {
            loopHandle = setTimeout(scanTick, 250);
        }
    }

    async function start(videoElementId, ref) {
        // Ensure a clean slate if a previous session wasn't stopped.
        releaseCamera();

        if (typeof window.BarcodeDetector === 'undefined') {
            return { supported: false, started: false, error: 'BarcodeDetector unavailable' };
        }
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            return { supported: false, started: false, error: 'getUserMedia unavailable' };
        }

        try {
            detector = new window.BarcodeDetector({ formats: ['qr_code'] });
        } catch (e) {
            return { supported: false, started: false, error: 'QR format unsupported' };
        }

        videoEl = document.getElementById(videoElementId);
        if (!videoEl) {
            return { supported: true, started: false, error: 'Video element not found' };
        }

        try {
            stream = await navigator.mediaDevices.getUserMedia({
                video: { facingMode: 'environment' },
                audio: false
            });
        } catch (e) {
            releaseCamera();
            return { supported: true, started: false, error: (e && e.name) ? e.name : 'Camera error' };
        }

        try {
            videoEl.srcObject = stream;
            videoEl.setAttribute('playsinline', 'true');
            videoEl.muted = true;
            await videoEl.play();
        } catch (e) {
            releaseCamera();
            return { supported: true, started: false, error: 'Video play failed' };
        }

        dotNetRef = ref;
        running = true;
        loopHandle = setTimeout(scanTick, 300);
        return { supported: true, started: true, error: null };
    }

    function stop() {
        releaseCamera();
    }

    return { start: start, stop: stop };
})();
