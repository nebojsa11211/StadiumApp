// Client-side image downscaling for admin-generated artwork.
//
// A generated poster is ~2 MB of PNG. The customer fixture strip renders up to 20 cards, so it
// needs a small variant; without one the landing page would pull tens of megabytes on mobile.
// Doing this in the browser keeps a server-side imaging dependency (ImageSharp/SkiaSharp) out of
// the solution — the admin generating the poster is the one machine that already has the pixels.
window.stadiumImageTools = {
    /**
     * Downscales a base64 image to fit within maxDimension (preserving aspect ratio) and re-encodes
     * it as JPEG. Returns raw base64 with no data: prefix, matching what the API expects.
     *
     * @param {string} base64Png - source image as raw base64 (no data: prefix)
     * @param {number} maxDimension - longest edge of the result, in pixels
     * @param {number} quality - JPEG quality, 0..1
     * @returns {Promise<string|null>} base64 JPEG, or null if the image could not be decoded
     */
    downscaleToBase64: function (base64Png, maxDimension, quality) {
        return new Promise((resolve) => {
            try {
                const img = new Image();

                img.onload = () => {
                    try {
                        const scale = Math.min(1, maxDimension / Math.max(img.width, img.height));
                        const w = Math.max(1, Math.round(img.width * scale));
                        const h = Math.max(1, Math.round(img.height * scale));

                        const canvas = document.createElement('canvas');
                        canvas.width = w;
                        canvas.height = h;

                        const ctx = canvas.getContext('2d');
                        // The thumbnail sits behind card text, so favour smooth downsampling.
                        ctx.imageSmoothingEnabled = true;
                        ctx.imageSmoothingQuality = 'high';
                        ctx.drawImage(img, 0, 0, w, h);

                        const dataUrl = canvas.toDataURL('image/jpeg', quality);
                        resolve(dataUrl.substring(dataUrl.indexOf('base64,') + 7));
                    } catch (e) {
                        console.warn('Thumbnail generation failed:', e);
                        resolve(null);
                    }
                };

                // A failed decode is not fatal — the caller simply saves without a thumbnail.
                img.onerror = () => resolve(null);
                img.src = 'data:image/png;base64,' + base64Png;
            } catch (e) {
                console.warn('Thumbnail generation failed:', e);
                resolve(null);
            }
        });
    },

    /**
     * Rasterizes any browser-renderable image (notably SVG, which is how club crests are stored)
     * into a PNG. The image-generation API only accepts PNG reference images, so crests must be
     * converted before they can be used to compose a poster.
     *
     * Drawn onto a transparent canvas and letterboxed to a square so a crest's aspect ratio
     * survives — the model is given the crest as-is, not stretched.
     *
     * @param {string} dataUrl - source image as a data: URL (any format the browser can decode)
     * @param {number} size - width/height of the square PNG produced
     * @returns {Promise<string|null>} base64 PNG with no prefix, or null if it could not be decoded
     */
    rasterizeToPngBase64: function (dataUrl, size) {
        return new Promise((resolve) => {
            try {
                const img = new Image();
                // SVG from a data: URL is same-origin, so the canvas stays untainted.
                img.onload = () => {
                    try {
                        const canvas = document.createElement('canvas');
                        canvas.width = size;
                        canvas.height = size;
                        const ctx = canvas.getContext('2d');
                        ctx.imageSmoothingEnabled = true;
                        ctx.imageSmoothingQuality = 'high';

                        // An SVG without intrinsic dimensions reports 0 — fall back to the target size.
                        const sw = img.width || size;
                        const sh = img.height || size;
                        const scale = Math.min(size / sw, size / sh);
                        const w = Math.max(1, Math.round(sw * scale));
                        const h = Math.max(1, Math.round(sh * scale));
                        ctx.drawImage(img, Math.round((size - w) / 2), Math.round((size - h) / 2), w, h);

                        const png = canvas.toDataURL('image/png');
                        resolve(png.substring(png.indexOf('base64,') + 7));
                    } catch (e) {
                        console.warn('Crest rasterization failed:', e);
                        resolve(null);
                    }
                };
                img.onerror = () => resolve(null);
                img.src = dataUrl;
            } catch (e) {
                console.warn('Crest rasterization failed:', e);
                resolve(null);
            }
        });
    }
};
