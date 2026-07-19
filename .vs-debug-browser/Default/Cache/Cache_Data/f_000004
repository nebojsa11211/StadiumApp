// Sector Preview Canvas
// Renders a single stadium sector realistically inside the Sector Edit preview modal.
// Unlike the flat HTML seat-grid it replaces, this draws the sector in its ACTUAL
// drawn shape (rectangle / triangle / rhombus / custom polygon / circular sector),
// then lays out the seats so they follow that geometry - matching the visual
// language of the drawing tool canvas (admin-drawing-tool-canvas).

(function () {
    // Original drawing-tool canvas dimensions. Sector coordinates are stored as
    // percentages (0-100) of this canvas, so we map percent -> these pixels first
    // to preserve the real-world aspect ratio before fitting into the preview.
    const ORIG_W = 1170;
    const ORIG_H = 898;

    // Seat colors mirror SectorEditModal.GetSeatColor(type)
    function seatColor(type) {
        switch ((type || 'standard').toLowerCase()) {
            case 'vip': return '#ffd700';        // Gold
            case 'premium': return '#6f42c1';    // Purple
            case 'wheelchair': return '#28a745'; // Green
            case 'family': return '#fd7e14';     // Orange
            case 'standard': return '#007bff';   // Blue
            default: return '#007bff';
        }
    }

    // Slightly stronger tone for the sector fill/outline than the seats themselves.
    function toRgba(hex, alpha) {
        const h = hex.replace('#', '');
        const r = parseInt(h.substring(0, 2), 16);
        const g = parseInt(h.substring(2, 4), 16);
        const b = parseInt(h.substring(4, 6), 16);
        return `rgba(${r}, ${g}, ${b}, ${alpha})`;
    }

    // Seats-per-row honoring variable seating patterns.
    function seatsForRow(sector, rowNumber) {
        if (sector.UseVariableSeating && sector.VariableSeatingData) {
            try {
                const patterns = JSON.parse(sector.VariableSeatingData);
                const p = patterns.find(p => rowNumber >= p.FromRow && rowNumber <= p.ToRow);
                if (p) return p.SeatsPerRow;
            } catch (e) { /* fall through to uniform */ }
        }
        return sector.SeatsPerRow || 20;
    }

    // --- Shape geometry, all in ORIGINAL canvas pixel space ---------------------

    // Returns { outline: [{x,y}...], rows: [ [{x,y}...] per seat ] }
    // outline = polygon points for the sector border; rows = seat centers per row.
    // marginFrac (0..~0.3) insets the seats from the shape borders.
    function buildGeometry(sector, marginFrac) {
        const shape = (sector.ShapeType || 'rectangle').toLowerCase();

        if (shape === 'circularsector') {
            return buildCircularGeometry(sector, marginFrac);
        }

        // Polygon-based shapes (triangle / rhombus / custompolygon) carry vertices
        // in ShapeData; everything else falls back to the rectangle bounding box.
        let verts = null;
        if (shape !== 'rectangle' && sector.ShapeData) {
            try {
                const parsed = JSON.parse(sector.ShapeData);
                if (Array.isArray(parsed) && parsed.length >= 3) {
                    verts = parsed.map(v => ({
                        x: (v.X / 100) * ORIG_W,
                        y: (v.Y / 100) * ORIG_H
                    }));
                }
            } catch (e) { verts = null; }
        }

        if (!verts) {
            // Rectangle from bounding-box percentages.
            const x = (sector.LeftPercent / 100) * ORIG_W;
            const y = (sector.TopPercent / 100) * ORIG_H;
            const w = (sector.WidthPercent / 100) * ORIG_W;
            const h = (sector.HeightPercent / 100) * ORIG_H;
            verts = [
                { x: x, y: y },
                { x: x + w, y: y },
                { x: x + w, y: y + h },
                { x: x, y: y + h }
            ];
        }

        return buildPolygonGeometry(sector, verts, marginFrac);
    }

    // Grid/scanline layout following the outline. A "row" (tier) steps across the
    // sector's SHORT axis; the seats within a row run along the LONG axis - exactly
    // how real stands are built (people sit shoulder-to-shoulder along the length,
    // tiers step back away from the pitch). So a tall/portrait sector gets vertical
    // rows of stacked seats; a wide/landscape sector gets horizontal rows.
    function buildPolygonGeometry(sector, verts, marginFrac) {
        const minX = Math.min(...verts.map(v => v.x));
        const maxX = Math.max(...verts.map(v => v.x));
        const minY = Math.min(...verts.map(v => v.y));
        const maxY = Math.max(...verts.map(v => v.y));
        const bw = maxX - minX;
        const bh = maxY - minY;
        const vertical = bh >= bw; // portrait -> rows run vertically along the height

        // Erode the polygon perpendicular to every edge so seats keep a UNIFORM gap
        // from all borders - including slanted/angled edges - not just an axis-aligned
        // bounding-box inset (which over-pulls on diagonals).
        const m = Math.min(Math.max(0, marginFrac || 0) * Math.min(bw, bh), Math.min(bw, bh) * 0.45);
        const inner = insetPolygon(verts, m);

        // Lay tiers/seats out inside the eroded polygon (no extra range inset needed).
        const iMinX = Math.min(...inner.map(v => v.x));
        const iMaxX = Math.max(...inner.map(v => v.x));
        const iMinY = Math.min(...inner.map(v => v.y));
        const iMaxY = Math.max(...inner.map(v => v.y));

        const rowsCount = Math.max(1, sector.Rows || 1);
        const rows = [];

        for (let r = 1; r <= rowsCount; r++) {
            const count = seatsForRow(sector, r);
            const seats = [];

            if (vertical) {
                // Tiers step across X; seats stack down Y (seats "stand vertically").
                const x = iMinX + ((r - 0.5) / rowsCount) * (iMaxX - iMinX);
                const ys = polygonScanline(inner, x, true);
                if (ys.length >= 2) {
                    const top = ys[0], bottom = ys[ys.length - 1];
                    for (let s = 1; s <= count; s++) {
                        seats.push({ x: x, y: top + ((s - 0.5) / count) * (bottom - top) });
                    }
                }
            } else {
                // Tiers step across Y; seats run along X.
                const y = iMinY + ((r - 0.5) / rowsCount) * (iMaxY - iMinY);
                const xs = polygonScanline(inner, y, false);
                if (xs.length >= 2) {
                    const left = xs[0], right = xs[xs.length - 1];
                    for (let s = 1; s <= count; s++) {
                        seats.push({ x: left + ((s - 0.5) / count) * (right - left), y: y });
                    }
                }
            }
            rows.push(seats);
        }

        return { outline: verts, rows: rows, closed: true };
    }

    // Shrink a [lo, hi] range inward by margin m on each side, without inverting it.
    function inset(lo, hi, m) {
        const maxM = (hi - lo) / 2 - 0.5; // keep a sliver of usable span
        const mm = Math.min(m, Math.max(0, maxM));
        return { lo: lo + mm, hi: hi - mm };
    }

    // Offset a simple polygon inward by distance d (perpendicular to each edge).
    // Each edge is shifted toward the centroid by d, then adjacent shifted edges are
    // intersected to form the new vertices. Falls back to the original on any failure.
    function insetPolygon(verts, d) {
        const n = verts.length;
        if (!(d > 0) || n < 3) return verts;

        let cx = 0, cy = 0;
        for (const v of verts) { cx += v.x; cy += v.y; }
        cx /= n; cy /= n;

        // Build inward-shifted edge lines (as point + direction).
        const lines = [];
        for (let i = 0; i < n; i++) {
            const a = verts[i], b = verts[(i + 1) % n];
            let nx = -(b.y - a.y), ny = (b.x - a.x); // edge perpendicular
            const len = Math.hypot(nx, ny) || 1;
            nx /= len; ny /= len;
            // Flip to point toward the centroid (inward).
            const mx = (a.x + b.x) / 2, my = (a.y + b.y) / 2;
            if ((cx - mx) * nx + (cy - my) * ny < 0) { nx = -nx; ny = -ny; }
            lines.push({ px: a.x + nx * d, py: a.y + ny * d, dx: b.x - a.x, dy: b.y - a.y });
        }

        // New vertex i = intersection of shifted edge (i-1) and shifted edge (i).
        const out = [];
        for (let i = 0; i < n; i++) {
            const l1 = lines[(i - 1 + n) % n], l2 = lines[i];
            const p = intersectLines(l1, l2);
            if (!p || !isFinite(p.x) || !isFinite(p.y)) return verts; // degenerate -> bail
            out.push(p);
        }
        return out;
    }

    // Intersection of two lines given as {px,py,dx,dy}.
    function intersectLines(l1, l2) {
        const denom = l1.dx * l2.dy - l1.dy * l2.dx;
        if (Math.abs(denom) < 1e-9) return { x: l2.px, y: l2.py }; // parallel -> use point
        const t = ((l2.px - l1.px) * l2.dy - (l2.py - l1.py) * l2.dx) / denom;
        return { x: l1.px + t * l1.dx, y: l1.py + t * l1.dy };
    }

    // Scanline intersections with the polygon edges.
    //   axisIsX=false: horizontal line at y -> returns sorted X intersections.
    //   axisIsX=true:  vertical line at x   -> returns sorted Y intersections.
    function polygonScanline(verts, pos, axisIsX) {
        const out = [];
        for (let i = 0, j = verts.length - 1; i < verts.length; j = i++) {
            const ai = axisIsX ? verts[i].x : verts[i].y;
            const aj = axisIsX ? verts[j].x : verts[j].y;
            if ((ai > pos) !== (aj > pos)) {
                const t = (pos - ai) / (aj - ai);
                if (axisIsX) {
                    out.push(verts[i].y + t * (verts[j].y - verts[i].y));
                } else {
                    out.push(verts[i].x + t * (verts[j].x - verts[i].x));
                }
            }
        }
        return out.sort((a, b) => a - b);
    }

    // Circular sector ("pizza slice"): rows are concentric arcs from an inner radius
    // out to the stored radius; seats spread along the angular span of each arc.
    function buildCircularGeometry(sector, marginFrac) {
        let meta;
        try { meta = JSON.parse(sector.ShapeData); } catch (e) { meta = null; }
        if (!meta) {
            // No arc metadata - degrade gracefully to a rectangle.
            return buildPolygonGeometry(sector, [
                { x: (sector.LeftPercent / 100) * ORIG_W, y: (sector.TopPercent / 100) * ORIG_H },
                { x: ((sector.LeftPercent + sector.WidthPercent) / 100) * ORIG_W, y: (sector.TopPercent / 100) * ORIG_H },
                { x: ((sector.LeftPercent + sector.WidthPercent) / 100) * ORIG_W, y: ((sector.TopPercent + sector.HeightPercent) / 100) * ORIG_H },
                { x: (sector.LeftPercent / 100) * ORIG_W, y: ((sector.TopPercent + sector.HeightPercent) / 100) * ORIG_H }
            ], marginFrac);
        }

        const cx = (meta.CenterX / 100) * ORIG_W;
        const cy = (meta.CenterY / 100) * ORIG_H;
        const outerRFull = (meta.Radius / 100) * ((ORIG_W + ORIG_H) / 2);
        const innerRFull = outerRFull * 0.35; // seating bowls are annular, not solid slices
        const a0Full = meta.StartAngle * Math.PI / 180;
        const a1Full = meta.EndAngle * Math.PI / 180;
        const rowsCount = Math.max(1, sector.Rows || 1);

        // Inset radially (both arc edges) and angularly (both radial edges).
        const mr = Math.max(0, marginFrac || 0) * (outerRFull - innerRFull);
        const rr = inset(innerRFull, outerRFull, mr);
        const innerR = rr.lo, outerR = rr.hi;
        const midR = (innerR + outerR) / 2;
        const ma = Math.max(0, marginFrac || 0) * (outerR - innerR) / midR; // radians
        const aa = inset(a0Full, a1Full, ma);
        const a0 = aa.lo, a1 = aa.hi;

        const rows = [];
        for (let r = 1; r <= rowsCount; r++) {
            const radius = innerR + ((r - 0.5) / rowsCount) * (outerR - innerR);
            const count = seatsForRow(sector, r);
            const seats = [];
            for (let s = 1; s <= count; s++) {
                const a = a0 + ((s - 0.5) / count) * (a1 - a0);
                seats.push({ x: cx + radius * Math.cos(a), y: cy + radius * Math.sin(a) });
            }
            rows.push(seats);
        }

        // Outline traces the FULL shape (inset only moves the seats inward).
        const outline = [];
        const steps = 24;
        for (let i = 0; i <= steps; i++) {
            const a = a0Full + (i / steps) * (a1Full - a0Full);
            outline.push({ x: cx + innerRFull * Math.cos(a), y: cy + innerRFull * Math.sin(a) });
        }
        for (let i = steps; i >= 0; i--) {
            const a = a0Full + (i / steps) * (a1Full - a0Full);
            outline.push({ x: cx + outerRFull * Math.cos(a), y: cy + outerRFull * Math.sin(a) });
        }

        return { outline: outline, rows: rows, closed: true };
    }

    // --- Rendering --------------------------------------------------------------

    // The real stadium map lives in the SAME 1170x898 coordinate space as the sector
    // percentages, so we can crop it directly to the sector region for context.
    const BG_SRC = '/images/stadium-blueprint.png';
    let bgImage = null;

    window.renderSectorPreview = function (canvasId, sectorJson, marginFrac, tipTemplate, dotNetRef, highlight) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) { console.error('Sector preview canvas not found:', canvasId); return; }

        let sector;
        try { sector = JSON.parse(sectorJson); }
        catch (e) { console.error('Invalid sector JSON for preview:', e); return; }

        canvas._tipTemplate = tipTemplate || 'Row {0}, Seat {1}';
        canvas._sectorCode = sector.SectorCode || '';
        canvas._dotNet = dotNetRef || null;
        // Optional seat(s) to ring + pulse. Accepts a single {row,seat} or an array of them
        // (used by the customer "locate my seat" — one clicked seat plus any selected/cart seats).
        canvas._highlights = normalizeHighlights(highlight);

        // Attach the per-seat hover tooltip once per canvas element.
        if (!canvas._tipBound) {
            canvas._tipBound = true;
            canvas.addEventListener('mousemove', onSeatHover);
            canvas.addEventListener('mouseleave', hideTip);
        }

        // Lazy-load the blueprint once, then redraw when it arrives so the map appears.
        if (!bgImage) {
            bgImage = new Image();
            bgImage.onload = () => drawPreview(canvas, sector, marginFrac || 0);
            bgImage.onerror = () => drawPreview(canvas, sector, marginFrac || 0);
            bgImage.src = BG_SRC;
        }
        drawPreview(canvas, sector, marginFrac || 0);
    };

    function drawPreview(canvas, sector, marginFrac) {
        const ctx = canvas.getContext('2d');
        const CW = canvas.width, CH = canvas.height;

        ctx.clearRect(0, 0, CW, CH);
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, CW, CH);

        const geo = buildGeometry(sector, marginFrac || 0);
        if (!geo.outline || geo.outline.length < 2) return;

        // Sector bounding box in blueprint pixels (blueprint is exactly ORIG_W x ORIG_H).
        const minX = Math.min(...geo.outline.map(p => p.x));
        const maxX = Math.max(...geo.outline.map(p => p.x));
        const minY = Math.min(...geo.outline.map(p => p.y));
        const maxY = Math.max(...geo.outline.map(p => p.y));
        const bw = Math.max(1, maxX - minX);
        const bh = Math.max(1, maxY - minY);

        // Expand the crop so a bit of the surrounding stadium shows around the sector.
        // Kept tight so the sector fills most of the frame (map reads large, not zoomed-out).
        const pad = 0.16 * Math.max(bw, bh);
        const cropX = Math.max(0, minX - pad);
        const cropY = Math.max(0, minY - pad);
        const cropW = Math.max(1, Math.min(ORIG_W, maxX + pad) - cropX);
        const cropH = Math.max(1, Math.min(ORIG_H, maxY + pad) - cropY);

        // One transform maps blueprint px -> canvas, for both the image and the overlays.
        const scale = Math.min(CW / cropW, CH / cropH);
        const dw = cropW * scale, dh = cropH * scale;
        const dx0 = (CW - dw) / 2, dy0 = (CH - dh) / 2;
        const T = p => ({ x: dx0 + (p.x - cropX) * scale, y: dy0 + (p.y - cropY) * scale });

        // 1) Real stadium map for this region (true colors + surrounding context).
        if (bgImage && bgImage.complete && bgImage.naturalWidth > 0) {
            ctx.drawImage(bgImage, cropX, cropY, cropW, cropH, dx0, dy0, dw, dh);
        }

        // 2) Highlight the selected sector's outline so it stands out from neighbours.
        ctx.beginPath();
        const o0 = T(geo.outline[0]);
        ctx.moveTo(o0.x, o0.y);
        for (let i = 1; i < geo.outline.length; i++) {
            const p = T(geo.outline[i]);
            ctx.lineTo(p.x, p.y);
        }
        if (geo.closed) ctx.closePath();
        ctx.lineJoin = 'round';
        ctx.strokeStyle = '#0d47a1';
        ctx.lineWidth = 3;
        ctx.stroke();

        // 3) Seat size derived from the tightest spacing so seats never overlap.
        let minGap = Infinity;
        for (const row of geo.rows) {
            for (let i = 1; i < row.length; i++) {
                const dx = (row[i].x - row[i - 1].x) * scale;
                const dy = (row[i].y - row[i - 1].y) * scale;
                minGap = Math.min(minGap, Math.hypot(dx, dy));
            }
        }
        if (geo.rows.length > 1) {
            for (let r = 1; r < geo.rows.length; r++) {
                if (geo.rows[r].length && geo.rows[r - 1].length) {
                    const dy = (geo.rows[r][0].y - geo.rows[r - 1][0].y) * scale;
                    minGap = Math.min(minGap, Math.abs(dy));
                }
            }
        }
        if (!isFinite(minGap)) minGap = 16;
        const seatSize = Math.max(4, Math.min(minGap * 0.75, 18));
        const radius = Math.min(seatSize / 4, 3);

        // 4) Seats as rows - white with a dark edge so they read as seats over any
        //    map color (yellow, green pitch, purple, ...). Also record each seat's
        //    canvas position + row/seat number for the hover tooltip.
        const seatHits = [];
        ctx.fillStyle = 'rgba(255,255,255,0.92)';
        ctx.strokeStyle = 'rgba(0,0,0,0.5)';
        ctx.lineWidth = 1;
        for (let ri = 0; ri < geo.rows.length; ri++) {
            const row = geo.rows[ri];
            for (let si = 0; si < row.length; si++) {
                const c = T(row[si]);
                roundRect(ctx, c.x - seatSize / 2, c.y - seatSize / 2, seatSize, seatSize, radius);
                ctx.fill();
                if (seatSize >= 5) ctx.stroke();
                seatHits.push({ x: c.x, y: c.y, half: seatSize / 2 + 2, row: ri + 1, seat: si + 1 });
            }
        }
        canvas._seatHits = seatHits;

        // 4b) Highlighted seat(s) (customer "locate my seat" + any selected/cart seats):
        //     a pink filled seat drawn into the scene now; an animated pulsing ring is
        //     layered on top afterwards by startPulse().
        const pulseTargets = [];
        for (const h of (canvas._highlights || [])) {
            const hs = seatHits.find(s => s.row === h.row && s.seat === h.seat);
            if (!hs) continue;
            const big = seatSize + 3;
            ctx.save();
            ctx.fillStyle = '#ff2d78';
            ctx.strokeStyle = '#ffffff';
            ctx.lineWidth = 1.5;
            roundRect(ctx, hs.x - big / 2, hs.y - big / 2, big, big, radius + 1);
            ctx.fill();
            ctx.stroke();
            ctx.restore();
            pulseTargets.push({ x: hs.x, y: hs.y, ring: Math.max(seatSize * 0.95, 9) * 1.35 });
        }
        canvas._pulseTargets = pulseTargets;

        // 5) Sector code label: white rounded badge with a blue border and blue
        //    text, matching the map's sector labels (e.g. "SECT1").
        if (sector.SectorCode) {
            let sx = 0, sy = 0, n = 0;
            for (const row of geo.rows) for (const seat of row) { const c = T(seat); sx += c.x; sy += c.y; n++; }
            const lx = n ? sx / n : CW / 2;
            const ly = n ? sy / n : CH / 2;

            ctx.font = 'bold 20px Arial';
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            const tw = ctx.measureText(sector.SectorCode).width;
            const bw2 = tw + 22, bh2 = 30;

            roundRect(ctx, lx - bw2 / 2, ly - bh2 / 2, bw2, bh2, 7);
            ctx.fillStyle = '#ffffff';
            ctx.fill();
            ctx.strokeStyle = '#1565c0';
            ctx.lineWidth = 2;
            ctx.stroke();

            ctx.fillStyle = '#1565c0';
            ctx.fillText(sector.SectorCode, lx, ly);
        }

        // Animate a pulsing ring over each highlighted seat (no-op when none).
        startPulse(canvas);
    }

    // Accept a single {row,seat} or an array; return a clean array of {row,seat}.
    function normalizeHighlights(h) {
        if (!h) return [];
        const arr = Array.isArray(h) ? h : [h];
        return arr.filter(x => x && x.row && x.seat).map(x => ({ row: x.row, seat: x.seat }));
    }

    // Pulsing highlight: snapshot the finished scene, then each frame redraw it and
    // stroke an expanding/fading ring on every highlighted seat. Stops automatically
    // when the canvas is re-rendered (token bump) or removed from the DOM.
    function startPulse(canvas) {
        const targets = canvas._pulseTargets || [];
        canvas._pulseToken = (canvas._pulseToken || 0) + 1; // supersede any running loop
        if (!targets.length) return;

        const ctx = canvas.getContext('2d');
        let base = canvas._baseCanvas;
        if (!base) { base = document.createElement('canvas'); canvas._baseCanvas = base; }
        base.width = canvas.width;
        base.height = canvas.height;
        base.getContext('2d').drawImage(canvas, 0, 0);

        const token = canvas._pulseToken;
        const t0 = performance.now();
        function frame(now) {
            if (canvas._pulseToken !== token || !document.body.contains(canvas)) return;
            const phase = (Math.sin((now - t0) / 700 * Math.PI) + 1) / 2; // 0..1, ~1.4s cycle
            ctx.drawImage(base, 0, 0);
            for (const tg of targets) {
                ctx.beginPath();
                ctx.arc(tg.x, tg.y, tg.ring * (1 + 0.6 * phase), 0, 2 * Math.PI);
                ctx.strokeStyle = 'rgba(255,45,120,' + (0.75 * (1 - phase) + 0.12).toFixed(3) + ')';
                ctx.lineWidth = 2.5;
                ctx.stroke();
            }
            requestAnimationFrame(frame);
        }
        requestAnimationFrame(frame);
    }

    // Darken a #rrggbb color toward black by fraction t (0..1).
    function darken(hex, t) {
        const h = hex.replace('#', '');
        const r = Math.round(parseInt(h.substring(0, 2), 16) * (1 - t));
        const g = Math.round(parseInt(h.substring(2, 4), 16) * (1 - t));
        const b = Math.round(parseInt(h.substring(4, 6), 16) * (1 - t));
        return `rgb(${r}, ${g}, ${b})`;
    }

    function roundRect(ctx, x, y, w, h, r) {
        ctx.beginPath();
        ctx.moveTo(x + r, y);
        ctx.arcTo(x + w, y, x + w, y + h, r);
        ctx.arcTo(x + w, y + h, x, y + h, r);
        ctx.arcTo(x, y + h, x, y, r);
        ctx.arcTo(x, y, x + w, y, r);
        ctx.closePath();
    }

    // --- Per-seat hover tooltip (row/seat + stadium seat code + QR) --------------

    let tipEl = null, tipRowEl = null, tipCodeEl = null, tipQrEl = null;
    let tipCurrentCode = null;              // seat code the tooltip is currently showing
    const qrCache = {};                     // seat code -> QR data URI

    function ensureTip() {
        if (!tipEl) {
            tipEl = document.createElement('div');
            tipEl.id = 'sector-preview-seat-tooltip';
            tipEl.style.cssText = 'position:fixed; z-index:10010; pointer-events:none; ' +
                'background:rgba(17,24,39,0.97); color:#fff; font:12px/1.3 Arial,sans-serif; ' +
                'padding:8px 10px; border-radius:8px; box-shadow:0 4px 14px rgba(0,0,0,0.4); ' +
                'white-space:nowrap; display:none; text-align:center;';

            tipRowEl = document.createElement('div');
            tipRowEl.style.cssText = 'font-weight:600; margin-bottom:2px;';

            tipCodeEl = document.createElement('div');
            tipCodeEl.style.cssText = 'font-family:monospace; font-size:11px; color:#9fd0ff; margin-bottom:6px;';

            tipQrEl = document.createElement('img');
            tipQrEl.alt = 'QR';
            tipQrEl.style.cssText = 'display:block; width:96px; height:96px; margin:0 auto; ' +
                'background:#fff; border-radius:4px; image-rendering:pixelated;';

            tipEl.appendChild(tipRowEl);
            tipEl.appendChild(tipCodeEl);
            tipEl.appendChild(tipQrEl);
            document.body.appendChild(tipEl);
        }
        return tipEl;
    }

    function positionTip(clientX, clientY) {
        const el = tipEl;
        const w = el.offsetWidth, h = el.offsetHeight;
        let x = clientX + 16, y = clientY + 16;
        if (x + w > window.innerWidth - 8) x = clientX - w - 14;
        if (y + h > window.innerHeight - 8) y = clientY - h - 14;
        el.style.left = Math.max(8, x) + 'px';
        el.style.top = Math.max(8, y) + 'px';
    }

    function hideTip() {
        tipCurrentCode = null;
        if (tipEl) tipEl.style.display = 'none';
    }

    function onSeatHover(e) {
        const canvas = e.currentTarget;
        const seats = canvas._seatHits;
        if (!seats || !seats.length) { hideTip(); return; }

        // Map cursor position to the canvas's internal coordinate space.
        const rect = canvas.getBoundingClientRect();
        const sx = canvas.width / rect.width;
        const sy = canvas.height / rect.height;
        const px = (e.clientX - rect.left) * sx;
        const py = (e.clientY - rect.top) * sy;

        let hit = null;
        for (const s of seats) {
            if (Math.abs(px - s.x) <= s.half && Math.abs(py - s.y) <= s.half) { hit = s; break; }
        }

        if (!hit) { hideTip(); canvas.style.cursor = 'default'; return; }

        canvas.style.cursor = 'pointer';
        const code = (canvas._sectorCode || 'SECT') + '-R' + hit.row + 'S' + hit.seat;
        ensureTip();

        // Only rebuild content when the hovered seat changes (avoids flicker).
        if (tipCurrentCode !== code) {
            tipCurrentCode = code;
            tipRowEl.textContent = (canvas._tipTemplate || 'Row {0}, Seat {1}')
                .replace('{0}', hit.row).replace('{1}', hit.seat);
            tipCodeEl.textContent = code;

            const cached = qrCache[code];
            if (cached) {
                tipQrEl.src = cached;
                tipQrEl.style.visibility = 'visible';
            } else {
                tipQrEl.style.visibility = 'hidden';
                if (canvas._dotNet) {
                    canvas._dotNet.invokeMethodAsync('GetSeatQrDataUri', code).then(uri => {
                        if (uri) {
                            qrCache[code] = uri;
                            // Only apply if the user is still hovering this same seat.
                            if (tipCurrentCode === code) {
                                tipQrEl.src = uri;
                                tipQrEl.style.visibility = 'visible';
                            }
                        }
                    }).catch(() => { });
                }
            }
            tipEl.style.display = 'block';
        }

        positionTip(e.clientX, e.clientY);
    }
})();
