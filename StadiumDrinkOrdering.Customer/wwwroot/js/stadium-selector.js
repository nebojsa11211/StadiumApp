// Customer stadium selector canvas.
// Read-only render of the stadium blueprint + DB sector overlays (same geometry the Admin
// drawing tool produces), but with CLICKABLE sectors. Unlike the admin drawing-canvas.js this
// scales pointer coordinates from the displayed (responsive) size back to the canvas's intrinsic
// 1170x898 resolution, so hit-testing is correct even when the canvas is shrunk on a phone.
(function () {
    'use strict';

    function getState(canvasId) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return null;
        if (!canvas._stadiumSel) {
            canvas._stadiumSel = {
                canvas: canvas,
                ctx: canvas.getContext('2d'),
                bg: null,
                overlays: [],
                dotNet: null,
                wired: false
            };
        }
        return canvas._stadiumSel;
    }

    function drawBackground(s) {
        const ctx = s.ctx, canvas = s.canvas, bg = s.bg;
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        if (!bg) return;
        const imgAspect = bg.width / bg.height;
        const canvasAspect = canvas.width / canvas.height;
        let dw, dh, ox = 0, oy = 0;
        if (imgAspect > canvasAspect) {
            dw = canvas.width; dh = canvas.width / imgAspect; oy = (canvas.height - dh) / 2;
        } else {
            dh = canvas.height; dw = canvas.height * imgAspect; ox = (canvas.width - dw) / 2;
        }
        ctx.drawImage(bg, ox, oy, dw, dh);
    }

    // Pixel vertices for a polygon-shaped overlay, or null for a plain rectangle.
    function overlayVertices(s, o) {
        if (o.ShapeType && o.ShapeType.toLowerCase() !== 'rectangle' && o.ShapeData) {
            try {
                const raw = JSON.parse(o.ShapeData);
                if (Array.isArray(raw) && raw.length >= 3) {
                    return raw.map(function (v) {
                        return {
                            x: ((v.X != null ? v.X : v.x) / 100) * s.canvas.width,
                            y: ((v.Y != null ? v.Y : v.y) / 100) * s.canvas.height
                        };
                    });
                }
            } catch (e) { /* fall through to rectangle */ }
        }
        return null;
    }

    function pointInPolygon(x, y, verts) {
        let inside = false;
        for (let i = 0, j = verts.length - 1; i < verts.length; j = i++) {
            const xi = verts[i].x, yi = verts[i].y, xj = verts[j].x, yj = verts[j].y;
            const intersect = ((yi > y) !== (yj > y)) && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
            if (intersect) inside = !inside;
        }
        return inside;
    }

    function hitTest(s, x, y) {
        // Topmost first so overlapping sectors resolve to the one drawn last.
        for (let i = s.overlays.length - 1; i >= 0; i--) {
            const o = s.overlays[i];
            const verts = overlayVertices(s, o);
            if (verts) {
                if (pointInPolygon(x, y, verts)) return o;
            } else if (x >= o._x && x <= o._x + o._w && y >= o._y && y <= o._y + o._h) {
                return o;
            }
        }
        return null;
    }

    function colorsFor(o) {
        if (o.SoldOut) {
            return { stroke: 'rgba(130,130,140,0.9)', fill: 'rgba(130,130,140,0.30)' };
        }
        if (typeof o.AvailPct === 'number') {
            if (o.AvailPct <= 0.10) return { stroke: 'rgba(220,53,69,0.9)', fill: 'rgba(220,53,69,0.22)' };
            if (o.AvailPct <= 0.50) return { stroke: 'rgba(255,170,0,0.9)', fill: 'rgba(255,170,0,0.22)' };
            return { stroke: 'rgba(40,190,110,0.9)', fill: 'rgba(40,190,110,0.20)' };
        }
        if ((o.Type || '').toLowerCase() === 'vip') {
            return { stroke: 'rgba(255,215,0,0.9)', fill: 'rgba(255,215,0,0.22)' };
        }
        return { stroke: 'rgba(0,123,255,0.85)', fill: 'rgba(0,123,255,0.18)' };
    }

    function labelBox(ctx, text, cx, y, font, textColor) {
        ctx.font = font;
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        const tw = ctx.measureText(text).width;
        ctx.fillStyle = 'rgba(255,255,255,0.92)';
        ctx.fillRect(cx - tw / 2 - 6, y - 12, tw + 12, 24);
        ctx.fillStyle = textColor;
        ctx.fillText(text, cx, y);
    }

    function precompute(s) {
        s.overlays.forEach(function (o) {
            o._x = (o.LeftPercent / 100) * s.canvas.width;
            o._y = (o.TopPercent / 100) * s.canvas.height;
            o._w = (o.WidthPercent / 100) * s.canvas.width;
            o._h = (o.HeightPercent / 100) * s.canvas.height;
        });
    }

    function draw(s) {
        drawBackground(s);
        const ctx = s.ctx;
        ctx.save();
        s.overlays.forEach(function (o) {
            const c = colorsFor(o);
            ctx.strokeStyle = c.stroke;
            ctx.fillStyle = c.fill;
            ctx.lineWidth = 2;

            const verts = overlayVertices(s, o);
            if (verts) {
                ctx.beginPath();
                ctx.moveTo(verts[0].x, verts[0].y);
                for (let i = 1; i < verts.length; i++) ctx.lineTo(verts[i].x, verts[i].y);
                ctx.closePath();
                ctx.fill();
                ctx.stroke();
            } else {
                ctx.fillRect(o._x, o._y, o._w, o._h);
                ctx.strokeRect(o._x, o._y, o._w, o._h);
            }

            const cx = o._x + o._w / 2, cy = o._y + o._h / 2;
            const label = o.Label || o.SectorCode || '';
            if (label) {
                labelBox(ctx, label, cx, o.Sub ? cy - 9 : cy, 'bold 18px Inter, Arial, sans-serif', '#12131a');
                if (o.Sub) {
                    labelBox(ctx, o.Sub, cx, cy + 14, '600 13px Inter, Arial, sans-serif',
                        o.SoldOut ? '#c02434' : '#1a6b3a');
                }
            }
        });
        ctx.restore();
    }

    window.stadiumSelectorInit = function (canvasId, dotNetRef) {
        const s = getState(canvasId);
        if (!s) return;
        s.dotNet = dotNetRef;
        if (s.wired) return;
        s.wired = true;

        function handleClick(clientX, clientY) {
            const rect = s.canvas.getBoundingClientRect();
            if (!rect.width || !rect.height) return;
            const x = (clientX - rect.left) * (s.canvas.width / rect.width);
            const y = (clientY - rect.top) * (s.canvas.height / rect.height);
            const o = hitTest(s, x, y);
            if (o && s.dotNet) {
                s.dotNet.invokeMethodAsync('OnSectorClicked', String(o.Id), o.SectorCode || '');
            }
        }

        s.canvas.addEventListener('click', function (e) { handleClick(e.clientX, e.clientY); });
        s.canvas.addEventListener('touchend', function (e) {
            if (e.changedTouches && e.changedTouches.length) {
                e.preventDefault();
                const t = e.changedTouches[0];
                handleClick(t.clientX, t.clientY);
            }
        }, { passive: false });
    };

    window.stadiumSelectorSetBackground = function (canvasId, url) {
        const s = getState(canvasId);
        if (!s) return;
        const img = new Image();
        img.onload = function () { s.bg = img; draw(s); };
        // No stadium image uploaded (404) or a load error: keep the background blank and still
        // render the sectors on top of the plain canvas.
        img.onerror = function () { s.bg = null; draw(s); };
        img.src = url;
    };

    window.stadiumSelectorDraw = function (canvasId, overlaysJson) {
        const s = getState(canvasId);
        if (!s) return;
        try {
            s.overlays = JSON.parse(overlaysJson) || [];
        } catch (e) {
            s.overlays = [];
        }
        precompute(s);
        draw(s);
    };
})();
