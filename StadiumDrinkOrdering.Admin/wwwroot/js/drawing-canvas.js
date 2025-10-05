// Stadium Drawing Tool - Canvas Drawing Functionality
let canvas, ctx;
let isDrawing = false;
let currentTool = ''; // No tool selected by default - allows click-to-edit on sectors
let currentColor = '#ff0000';
let currentLineWidth = 3;
let currentOpacity = 0.7;
let backgroundImage = null;
let layers = new Map();
let currentLayerId = 1;
let startX, startY;
let currentPath = [];

// Sector overlay management
let sectorOverlays = [];
let selectedSector = null;
let resizeHandle = null;
let dragOffsetX = 0;
let dragOffsetY = 0;

// Multi-shape drawing support
let currentShapeMode = 'rectangle'; // rectangle, triangle, polygon, rhombus, circular
let polygonVertices = []; // Temporary storage for polygon points during drawing
let isDrawingPolygon = false;
let helperTextElement = null;

// .NET interop reference
let dotNetHelper = null;

// Initialize canvas
window.initializeDrawingCanvas = function(canvasId, dotNetReference) {
    canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error('Canvas not found:', canvasId);
        return;
    }

    ctx = canvas.getContext('2d');
    dotNetHelper = dotNetReference;

    // Initialize default layer
    layers.set(1, {
        id: 1,
        visible: true,
        canvas: document.createElement('canvas'),
        ctx: null
    });

    const layer = layers.get(1);
    layer.canvas.width = canvas.width;
    layer.canvas.height = canvas.height;
    layer.ctx = layer.canvas.getContext('2d');

    // Add event listeners
    canvas.addEventListener('mousedown', handleMouseDown);
    canvas.addEventListener('mousemove', handleMouseMove);
    canvas.addEventListener('mouseup', handleMouseUp);
    canvas.addEventListener('mouseout', handleMouseUp);

    // Touch support for mobile
    canvas.addEventListener('touchstart', handleTouchStart, { passive: false });
    canvas.addEventListener('touchmove', handleTouchMove, { passive: false });
    canvas.addEventListener('touchend', handleTouchEnd, { passive: false });

    redrawCanvas();
};

// Set background image
window.setBackgroundImage = function(dataUrl) {
    const img = new Image();
    img.onload = function() {
        backgroundImage = img;
        redrawCanvas();
    };
    img.src = dataUrl;
};

// Clear background image
window.clearBackgroundImage = function() {
    backgroundImage = null;
    redrawCanvas();
};

// Update drawing properties
window.updateDrawingColor = function(color) {
    currentColor = color;
};

window.updateDrawingLineWidth = function(width) {
    currentLineWidth = width;
};

window.updateDrawingOpacity = function(opacity) {
    currentOpacity = opacity;
};

window.setDrawingTool = function(tool) {
    currentTool = tool.toLowerCase();
    console.log('🎨 Drawing tool changed to:', currentTool);

    // Reset polygon drawing state when changing tools
    if (currentTool !== 'createsector') {
        resetPolygonDrawing();
    }
};

// Set shape mode for sector creation
window.setShapeMode = function(shapeMode) {
    currentShapeMode = shapeMode.toLowerCase();
    resetPolygonDrawing();
    console.log('🔶 Shape mode changed to:', currentShapeMode);

    //Show helper text for polygon/triangle modes
    if (currentShapeMode === 'polygon' || currentShapeMode === 'triangle') {
        showHelperText(currentShapeMode === 'triangle' ?
            'Click 3 points to create triangle' :
            'Click to add vertices. Double-click to finish (min 3 vertices)');
    }
};

// Reset polygon drawing state
function resetPolygonDrawing() {
    polygonVertices = [];
    isDrawingPolygon = false;
    hideHelperText();
    redrawWithSectors();
}

// Layer management
window.toggleLayerVisibility = function(layerId, visible) {
    const layer = layers.get(layerId);
    if (layer) {
        layer.visible = visible;
        redrawCanvas();
    }
};

window.deleteLayer = function(layerId) {
    layers.delete(layerId);
    redrawCanvas();
};

window.clearLayer = function(layerId) {
    const layer = layers.get(layerId);
    if (layer && layer.ctx) {
        layer.ctx.clearRect(0, 0, layer.canvas.width, layer.canvas.height);
        redrawCanvas();
    }
};

// Mouse event handlers
function handleMouseDown(e) {
    const rect = canvas.getBoundingClientRect();
    startX = e.clientX - rect.left;
    startY = e.clientY - rect.top;

    // Handle click-to-edit for sectors ONLY when NO tool is selected (default state)
    // When any drawing tool is active (pen, rectangle, circle, line, eraser, select, createsector),
    // clicking sectors should NOT open the modal
    const noToolSelected = !currentTool || currentTool === '';

    if (noToolSelected) {
        const sector = getSectorAtPoint(startX, startY);
        if (sector && dotNetHelper) {
            console.log('🖱️ Clicked sector:', sector.SectorCode, '- Opening edit modal (no tool selected)');
            dotNetHelper.invokeMethodAsync('ShowSectorEditModal', sector.Id);
            return;
        }
    }

    // Handle Select tool for sector manipulation
    if (currentTool === 'select') {
        // Check if clicking on a sector
        const sector = getSectorAtPoint(startX, startY);

        if (sector) {
            // Check if clicking on resize handle
            resizeHandle = getResizeHandle(startX, startY, sector);

            if (resizeHandle) {
                // Starting resize operation
                selectedSector = sector;
                isDrawing = true;
                console.log('🔧 Resizing sector:', sector.SectorCode, 'handle:', resizeHandle);
            } else {
                // Starting move operation
                selectedSector = sector;
                isDrawing = true;
                dragOffsetX = startX - sector.x;
                dragOffsetY = startY - sector.y;
                console.log('📍 Moving sector:', sector.SectorCode);
            }
        } else {
            // Deselect
            selectedSector = null;
            resizeHandle = null;
        }

        redrawWithSectors();
        return;
    }

    // Handle CreateSector tool with multi-shape support
    if (currentTool === 'createsector') {
        if (currentShapeMode === 'polygon' || currentShapeMode === 'triangle') {
            // Polygon/Triangle: Click-to-add-vertex mode
            isDrawingPolygon = true;
            polygonVertices.push({ x: startX, y: startY });

            console.log(`📍 Added vertex ${polygonVertices.length} at:`, startX, startY);

            // Auto-complete triangle after 3 points
            if (currentShapeMode === 'triangle' && polygonVertices.length === 3) {
                finishPolygonCreation();
                return;
            }

            redrawWithPolygonPreview();
            return;
        } else {
            // Rectangle mode: Click-and-drag
            isDrawing = true;
            console.log('🆕 Starting new sector creation at:', startX, startY);
            return;
        }
    }

    // Normal drawing tools
    isDrawing = true;
    currentPath = [{ x: startX, y: startY }];

    const layer = layers.get(currentLayerId);
    if (layer && layer.ctx) {
        layer.ctx.globalAlpha = currentOpacity;
        layer.ctx.strokeStyle = currentColor;
        layer.ctx.fillStyle = currentColor;
        layer.ctx.lineWidth = currentLineWidth;
        layer.ctx.lineCap = 'round';
        layer.ctx.lineJoin = 'round';

        if (currentTool === 'pen' || currentTool === 'eraser') {
            layer.ctx.beginPath();
            layer.ctx.moveTo(startX, startY);
            if (currentTool === 'eraser') {
                layer.ctx.globalCompositeOperation = 'destination-out';
                layer.ctx.lineWidth = currentLineWidth * 2;
            } else {
                layer.ctx.globalCompositeOperation = 'source-over';
            }
        }
    }
}

function handleMouseMove(e) {
    const rect = canvas.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    // Handle CreateSector preview
    if (currentTool === 'createsector' && isDrawing) {
        redrawWithSectors();
        // Draw preview rectangle
        ctx.save();
        ctx.strokeStyle = '#007bff';
        ctx.fillStyle = 'rgba(0, 123, 255, 0.2)';
        ctx.lineWidth = 2;
        ctx.setLineDash([5, 5]);

        const width = x - startX;
        const height = y - startY;
        ctx.strokeRect(startX, startY, width, height);
        ctx.fillRect(startX, startY, width, height);

        ctx.restore();
        return;
    }

    // Handle sector manipulation in Select mode
    if (currentTool === 'select') {
        if (isDrawing && selectedSector) {
            if (resizeHandle) {
                // Resize sector
                resizeSector(selectedSector, resizeHandle, x, y);
            } else {
                // Move sector
                selectedSector.x = x - dragOffsetX;
                selectedSector.y = y - dragOffsetY;

                // Update percentages
                selectedSector.LeftPercent = (selectedSector.x / canvas.width) * 100;
                selectedSector.TopPercent = (selectedSector.y / canvas.height) * 100;
            }
            redrawWithSectors();
        } else {
            // Update cursor based on what's under mouse
            updateCursor(x, y);
        }
        return;
    }

    // Normal drawing tools
    if (!isDrawing) return;

    currentPath.push({ x, y });

    const layer = layers.get(currentLayerId);
    if (!layer || !layer.ctx) return;

    if (currentTool === 'pen' || currentTool === 'eraser') {
        layer.ctx.lineTo(x, y);
        layer.ctx.stroke();
        redrawWithSectors(); // Keep sectors visible while drawing
    } else {
        // For shapes, redraw with preview
        redrawWithSectors(); // Keep sectors visible while drawing
        drawShapePreview(startX, startY, x, y);
    }
}

// Resize sector based on handle
function resizeSector(sector, handle, mouseX, mouseY) {
    const minSize = 20; // Minimum sector size

    switch (handle) {
        case 'nw':
            const newWidth = sector.x + sector.width - mouseX;
            const newHeight = sector.y + sector.height - mouseY;
            if (newWidth >= minSize && newHeight >= minSize) {
                sector.x = mouseX;
                sector.y = mouseY;
                sector.width = newWidth;
                sector.height = newHeight;
            }
            break;
        case 'ne':
            const newWidthNE = mouseX - sector.x;
            const newHeightNE = sector.y + sector.height - mouseY;
            if (newWidthNE >= minSize && newHeightNE >= minSize) {
                sector.y = mouseY;
                sector.width = newWidthNE;
                sector.height = newHeightNE;
            }
            break;
        case 'sw':
            const newWidthSW = sector.x + sector.width - mouseX;
            const newHeightSW = mouseY - sector.y;
            if (newWidthSW >= minSize && newHeightSW >= minSize) {
                sector.x = mouseX;
                sector.width = newWidthSW;
                sector.height = newHeightSW;
            }
            break;
        case 'se':
            const newWidthSE = mouseX - sector.x;
            const newHeightSE = mouseY - sector.y;
            if (newWidthSE >= minSize && newHeightSE >= minSize) {
                sector.width = newWidthSE;
                sector.height = newHeightSE;
            }
            break;
    }

    // Update percentages
    sector.LeftPercent = (sector.x / canvas.width) * 100;
    sector.TopPercent = (sector.y / canvas.height) * 100;
    sector.WidthPercent = (sector.width / canvas.width) * 100;
    sector.HeightPercent = (sector.height / canvas.height) * 100;
}

// Update cursor based on what's under mouse
function updateCursor(x, y) {
    const sector = getSectorAtPoint(x, y);

    if (sector) {
        const handle = getResizeHandle(x, y, sector);
        if (handle) {
            // Set resize cursor
            const cursors = {
                'nw': 'nw-resize',
                'ne': 'ne-resize',
                'sw': 'sw-resize',
                'se': 'se-resize'
            };
            canvas.style.cursor = cursors[handle];
        } else {
            // Set move cursor
            canvas.style.cursor = 'move';
        }
    } else {
        canvas.style.cursor = 'default';
    }
}

function handleMouseUp(e) {
    if (!isDrawing) return;
    isDrawing = false;

    const rect = canvas.getBoundingClientRect();
    const endX = e.clientX - rect.left;
    const endY = e.clientY - rect.top;

    // Handle CreateSector completion
    if (currentTool === 'createsector') {
        const width = endX - startX;
        const height = endY - startY;

        // Minimum size validation (20px)
        if (Math.abs(width) > 20 && Math.abs(height) > 20) {
            // Normalize coordinates (handle dragging in any direction)
            const x = width < 0 ? endX : startX;
            const y = height < 0 ? endY : startY;
            const w = Math.abs(width);
            const h = Math.abs(height);

            // Convert to percentages
            const leftPercent = (x / canvas.width) * 100;
            const topPercent = (y / canvas.height) * 100;
            const widthPercent = (w / canvas.width) * 100;
            const heightPercent = (h / canvas.height) * 100;

            console.log('✅ Sector created:', {
                left: leftPercent.toFixed(2) + '%',
                top: topPercent.toFixed(2) + '%',
                width: widthPercent.toFixed(2) + '%',
                height: heightPercent.toFixed(2) + '%'
            });

            // Invoke C# method to open modal
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('OnSectorDrawComplete',
                    leftPercent, topPercent, widthPercent, heightPercent);
            }
        } else {
            console.log('⚠️ Sector too small, minimum size is 20px');
        }

        redrawWithSectors();
        return;
    }

    // Handle sector manipulation in Select mode
    if (currentTool === 'select') {
        if (selectedSector && (resizeHandle || dragOffsetX !== 0 || dragOffsetY !== 0)) {
            console.log('✅ Sector updated:', selectedSector.SectorCode, {
                left: selectedSector.LeftPercent.toFixed(2) + '%',
                top: selectedSector.TopPercent.toFixed(2) + '%',
                width: selectedSector.WidthPercent.toFixed(2) + '%',
                height: selectedSector.HeightPercent.toFixed(2) + '%'
            });

            // Notify C# about the sector modification
            if (dotNetHelper) {
                console.log('📡 Notifying C# about sector modification');
                dotNetHelper.invokeMethodAsync('OnSectorModified',
                    selectedSector.Id,
                    selectedSector.TopPercent,
                    selectedSector.LeftPercent,
                    selectedSector.WidthPercent,
                    selectedSector.HeightPercent
                );
            }
        }
        resizeHandle = null;
        dragOffsetX = 0;
        dragOffsetY = 0;
        return;
    }

    // Normal drawing tools
    const layer = layers.get(currentLayerId);
    if (!layer || !layer.ctx) return;

    if (currentTool === 'rectangle') {
        drawRectangle(layer.ctx, startX, startY, endX, endY);
    } else if (currentTool === 'circle') {
        drawCircle(layer.ctx, startX, startY, endX, endY);
    } else if (currentTool === 'line') {
        drawLine(layer.ctx, startX, startY, endX, endY);
    }

    layer.ctx.globalCompositeOperation = 'source-over';
    redrawWithSectors(); // Keep sectors visible after drawing
}

// Touch event handlers
function handleTouchStart(e) {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent('mousedown', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
}

function handleTouchMove(e) {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent('mousemove', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
}

function handleTouchEnd(e) {
    e.preventDefault();
    const mouseEvent = new MouseEvent('mouseup', {});
    canvas.dispatchEvent(mouseEvent);
}

// Drawing functions
function drawRectangle(context, x1, y1, x2, y2) {
    const width = x2 - x1;
    const height = y2 - y1;
    context.globalAlpha = currentOpacity;
    context.strokeRect(x1, y1, width, height);
    context.fillStyle = currentColor + '30'; // Add transparency to fill
    context.fillRect(x1, y1, width, height);
}

function drawCircle(context, x1, y1, x2, y2) {
    const radius = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
    context.globalAlpha = currentOpacity;
    context.beginPath();
    context.arc(x1, y1, radius, 0, 2 * Math.PI);
    context.stroke();
    context.fillStyle = currentColor + '30'; // Add transparency to fill
    context.fill();
}

function drawLine(context, x1, y1, x2, y2) {
    context.globalAlpha = currentOpacity;
    context.beginPath();
    context.moveTo(x1, y1);
    context.lineTo(x2, y2);
    context.stroke();
}

function drawShapePreview(x1, y1, x2, y2) {
    ctx.save();
    ctx.setLineDash([5, 5]);
    ctx.strokeStyle = currentColor;
    ctx.lineWidth = currentLineWidth;
    ctx.globalAlpha = currentOpacity * 0.5;

    if (currentTool === 'rectangle') {
        const width = x2 - x1;
        const height = y2 - y1;
        ctx.strokeRect(x1, y1, width, height);
    } else if (currentTool === 'circle') {
        const radius = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
        ctx.beginPath();
        ctx.arc(x1, y1, radius, 0, 2 * Math.PI);
        ctx.stroke();
    } else if (currentTool === 'line') {
        ctx.beginPath();
        ctx.moveTo(x1, y1);
        ctx.lineTo(x2, y2);
        ctx.stroke();
    }

    ctx.restore();
}

// Redraw entire canvas
function redrawCanvas() {
    if (!ctx) return;

    // Clear main canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Draw background image if exists
    if (backgroundImage) {
        const imgAspect = backgroundImage.width / backgroundImage.height;
        const canvasAspect = canvas.width / canvas.height;
        let drawWidth, drawHeight, offsetX = 0, offsetY = 0;

        if (imgAspect > canvasAspect) {
            drawWidth = canvas.width;
            drawHeight = canvas.width / imgAspect;
            offsetY = (canvas.height - drawHeight) / 2;
        } else {
            drawHeight = canvas.height;
            drawWidth = canvas.height * imgAspect;
            offsetX = (canvas.width - drawWidth) / 2;
        }

        ctx.drawImage(backgroundImage, offsetX, offsetY, drawWidth, drawHeight);
    }

    // Draw all visible layers in order
    const sortedLayers = Array.from(layers.values()).sort((a, b) => a.id - b.id);
    for (const layer of sortedLayers) {
        if (layer.visible && layer.canvas) {
            ctx.drawImage(layer.canvas, 0, 0);
        }
    }
}

// Export functionality
window.exportDrawingAsImage = function() {
    const link = document.createElement('a');
    link.download = 'stadium-drawing-' + new Date().getTime() + '.png';
    link.href = canvas.toDataURL();
    link.click();
};

window.downloadJSON = function(jsonString, filename) {
    const blob = new Blob([jsonString], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.download = filename;
    link.href = url;
    link.click();
    URL.revokeObjectURL(url);
};

// Export modified sector overlays
window.exportSectorOverlays = function() {
    if (sectorOverlays.length === 0) {
        console.warn('No sector overlays to export');
        return null;
    }

    // Convert pixel coordinates back to percentages and create clean export
    const exportData = sectorOverlays.map(sector => ({
        sectorCode: sector.SectorCode,
        name: sector.Name || sector.name,
        topPercent: parseFloat(sector.TopPercent.toFixed(2)),
        leftPercent: parseFloat(sector.LeftPercent.toFixed(2)),
        widthPercent: parseFloat(sector.WidthPercent.toFixed(2)),
        heightPercent: parseFloat(sector.HeightPercent.toFixed(2)),
        type: sector.Type || sector.type || 'standard'
    }));

    const config = {
        stadiumName: "HNK Rijeka Stadium",
        imageWidth: canvas.width,
        imageHeight: canvas.height,
        sectorOverlays: exportData
    };

    const jsonString = JSON.stringify(config, null, 2);
    console.log('✅ Exported sector configuration:', exportData.length, 'sectors');
    return jsonString;
};

// Set current layer
window.setCurrentLayer = function(layerId) {
    currentLayerId = layerId;

    // Create layer if it doesn't exist
    if (!layers.has(layerId)) {
        const newLayer = {
            id: layerId,
            visible: true,
            canvas: document.createElement('canvas'),
            ctx: null
        };
        newLayer.canvas.width = canvas.width;
        newLayer.canvas.height = canvas.height;
        newLayer.ctx = newLayer.canvas.getContext('2d');
        layers.set(layerId, newLayer);
    }
};

// Draw sector overlays on canvas
window.drawSectorOverlays = function(overlaysJson) {
    try {
        const overlays = JSON.parse(overlaysJson);
        console.log('📍 Drawing sector overlays:', overlays.length, 'sectors');

        if (!ctx || overlays.length === 0) {
            console.log('⚠️ No canvas context or no overlays to draw');
            return;
        }

        // Store overlays for interaction
        sectorOverlays = overlays.map(overlay => ({
            ...overlay,
            x: (overlay.LeftPercent / 100) * canvas.width,
            y: (overlay.TopPercent / 100) * canvas.height,
            width: (overlay.WidthPercent / 100) * canvas.width,
            height: (overlay.HeightPercent / 100) * canvas.height
        }));

        // Redraw everything
        redrawWithSectors();

        console.log('✅ Sector overlays drawn successfully');
    } catch (error) {
        console.error('❌ Error drawing sector overlays:', error);
    }
};

// Clear all sector overlays
window.clearSectorOverlays = function() {
    console.log('🧹 Clearing all sector overlays');
    sectorOverlays = [];
    selectedSector = null;
    if (ctx) {
        redrawCanvas(); // Redraw without sectors
    }
    console.log('✅ Sector overlays cleared');
};

// Redraw canvas with sectors
function redrawWithSectors() {
    if (!ctx) return;

    // Clear canvas first
    redrawCanvas();

    // Save context
    ctx.save();

    // Draw each sector overlay
    sectorOverlays.forEach(sector => {
        const isSelected = selectedSector === sector;

        // Different colors for different types
        let strokeColor = 'rgba(0, 123, 255, 0.7)'; // Blue for standard
        let fillColor = 'rgba(0, 123, 255, 0.15)';

        if (sector.Type === 'vip') {
            strokeColor = 'rgba(255, 215, 0, 0.8)'; // Gold for VIP
            fillColor = 'rgba(255, 215, 0, 0.2)';
        }

        if (isSelected) {
            strokeColor = 'rgba(255, 0, 0, 0.9)'; // Red for selected
            fillColor = 'rgba(255, 0, 0, 0.3)';
        }

        ctx.strokeStyle = strokeColor;
        ctx.fillStyle = fillColor;
        ctx.lineWidth = isSelected ? 3 : 2;

        // Draw rectangle
        ctx.strokeRect(sector.x, sector.y, sector.width, sector.height);
        ctx.fillRect(sector.x, sector.y, sector.width, sector.height);

        // Draw resize handles if selected
        if (isSelected) {
            drawResizeHandles(sector);
        }

        // Draw sector code label
        if (sector.SectorCode) {
            ctx.font = 'bold 16px Arial';
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';

            const labelX = sector.x + sector.width / 2;
            const labelY = sector.y + sector.height / 2;

            // Draw text background
            const textWidth = ctx.measureText(sector.SectorCode).width;
            ctx.fillStyle = 'rgba(255, 255, 255, 0.9)';
            ctx.fillRect(labelX - textWidth/2 - 5, labelY - 12, textWidth + 10, 24);

            // Draw text with border for better visibility
            ctx.strokeStyle = 'rgba(0, 0, 0, 0.3)';
            ctx.lineWidth = 3;
            ctx.strokeText(sector.SectorCode, labelX, labelY);

            // Draw text
            ctx.fillStyle = sector.Type === 'vip' ? 'rgba(255, 165, 0, 1)' : 'rgba(0, 100, 200, 1)';
            ctx.fillText(sector.SectorCode, labelX, labelY);
        }
    });

    // Restore context
    ctx.restore();
}

// Draw resize handles for selected sector
function drawResizeHandles(sector) {
    const handleSize = 8;
    const handles = [
        { x: sector.x, y: sector.y, cursor: 'nw-resize', name: 'nw' },
        { x: sector.x + sector.width, y: sector.y, cursor: 'ne-resize', name: 'ne' },
        { x: sector.x, y: sector.y + sector.height, cursor: 'sw-resize', name: 'sw' },
        { x: sector.x + sector.width, y: sector.y + sector.height, cursor: 'se-resize', name: 'se' }
    ];

    ctx.fillStyle = 'rgba(255, 0, 0, 0.8)';
    ctx.strokeStyle = 'rgba(255, 255, 255, 1)';
    ctx.lineWidth = 2;

    handles.forEach(handle => {
        ctx.fillRect(handle.x - handleSize/2, handle.y - handleSize/2, handleSize, handleSize);
        ctx.strokeRect(handle.x - handleSize/2, handle.y - handleSize/2, handleSize, handleSize);
    });
}

// Check if point is inside sector
function isPointInSector(x, y, sector) {
    return x >= sector.x && x <= sector.x + sector.width &&
           y >= sector.y && y <= sector.y + sector.height;
}

// Check if point is on resize handle
function getResizeHandle(x, y, sector) {
    const handleSize = 8;
    const handles = [
        { x: sector.x, y: sector.y, name: 'nw' },
        { x: sector.x + sector.width, y: sector.y, name: 'ne' },
        { x: sector.x, y: sector.y + sector.height, name: 'sw' },
        { x: sector.x + sector.width, y: sector.y + sector.height, name: 'se' }
    ];

    for (const handle of handles) {
        if (Math.abs(x - handle.x) <= handleSize && Math.abs(y - handle.y) <= handleSize) {
            return handle.name;
        }
    }
    return null;
}

// Get sector at point
function getSectorAtPoint(x, y) {
    // Check in reverse order (top to bottom) to get the topmost sector
    for (let i = sectorOverlays.length - 1; i >= 0; i--) {
        if (isPointInSector(x, y, sectorOverlays[i])) {
            return sectorOverlays[i];
        }
    }
    return null;
}

// ========== POLYGON DRAWING HELPER FUNCTIONS ==========

// Finish polygon creation and call .NET
function finishPolygonCreation() {
    if (polygonVertices.length < 3) {
        console.warn('Need at least 3 vertices for polygon');
        return;
    }

    // Convert canvas coordinates to percentages
    const vertices = polygonVertices.map(v => ({
        x: (v.x / canvas.width) * 100,
        y: (v.y / canvas.height) * 100
    }));

    console.log('✅ Polygon complete with', vertices.length, 'vertices');

    // Call .NET to show sector details modal with vertices
    if (dotNetHelper) {
        dotNetHelper.invokeMethodAsync('ShowSectorCreateModalWithVertices',
            currentShapeMode,
            vertices
        );
    }

    resetPolygonDrawing();
}

// Draw polygon preview during creation
function redrawWithPolygonPreview() {
    redrawWithSectors();

    if (polygonVertices.length === 0) return;

    ctx.save();
    ctx.strokeStyle = '#007bff';
    ctx.fillStyle = 'rgba(0, 123, 255, 0.2)';
    ctx.lineWidth = 2;
    ctx.setLineDash([5, 5]);

    // Draw lines between vertices
    ctx.beginPath();
    ctx.moveTo(polygonVertices[0].x, polygonVertices[0].y);
    for (let i = 1; i < polygonVertices.length; i++) {
        ctx.lineTo(polygonVertices[i].x, polygonVertices[i].y);
    }

    // If enough vertices, show preview of closing line
    if (polygonVertices.length >= 2) {
        ctx.setLineDash([2, 2]);
        ctx.strokeStyle = '#6c757d';
        ctx.lineTo(polygonVertices[0].x, polygonVertices[0].y);
    }

    ctx.stroke();

    // Fill if polygon is complete (3+ vertices)
    if (polygonVertices.length >= 3) {
        ctx.closePath();
        ctx.fill();
    }

    ctx.setLineDash([]);

    // Draw vertices as circles
    polygonVertices.forEach((v, i) => {
        ctx.beginPath();
        ctx.arc(v.x, v.y, 6, 0, 2 * Math.PI);
        ctx.fillStyle = i === 0 ? '#28a745' : '#007bff'; // First vertex green
        ctx.fill();
        ctx.strokeStyle = '#fff';
        ctx.lineWidth = 2;
        ctx.stroke();

        // Label vertex number
        ctx.fillStyle = '#fff';
        ctx.font = 'bold 10px Arial';
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        ctx.fillText(i + 1, v.x, v.y);
    });

    ctx.restore();

    // Update helper text
    const remaining = currentShapeMode === 'triangle' ? (3 - polygonVertices.length) : 0;
    if (currentShapeMode === 'triangle' && remaining > 0) {
        showHelperText(`Click ${remaining} more point(s)`);
    } else if (currentShapeMode === 'polygon') {
        showHelperText(`${polygonVertices.length} vertices. Double-click to finish (min 3).`);
    }
}

// Show helper text overlay
function showHelperText(message) {
    if (!helperTextElement) {
        helperTextElement = document.createElement('div');
        helperTextElement.id = 'canvas-helper-text';
        helperTextElement.style.cssText = `
            position: absolute;
            bottom: 20px;
            left: 50%;
            transform: translateX(-50%);
            background: rgba(0, 123, 255, 0.95);
            color: white;
            padding: 10px 20px;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 500;
            z-index: 1000;
            box-shadow: 0 4px 6px rgba(0,0,0,0.3);
            pointer-events: none;
        `;
        canvas.parentElement.style.position = 'relative';
        canvas.parentElement.appendChild(helperTextElement);
    }
    helperTextElement.textContent = message;
    helperTextElement.style.display = 'block';
}

// Hide helper text
function hideHelperText() {
    if (helperTextElement) {
        helperTextElement.style.display = 'none';
    }
}

// Handle double-click to finish polygon
canvas?.addEventListener('dblclick', function(e) {
    if (currentTool === 'createsector' && currentShapeMode === 'polygon') {
        e.preventDefault();
        if (polygonVertices.length >= 3) {
            finishPolygonCreation();
        }
    }
});

// Initialize on load
document.addEventListener('DOMContentLoaded', function() {
    console.log('Drawing canvas script loaded with multi-shape support');
});
