/**
 * Stadium Viewer Enhanced Interactions
 * Modern, Accessible Stadium Visualization
 * Supports keyboard navigation, smart tooltips, and canvas controls
 */

class StadiumViewer {
    constructor() {
        this.currentZoom = 1;
        this.currentPan = { x: 0, y: 0 };
        this.isDragging = false;
        this.dragStart = { x: 0, y: 0 };
        this.canvasContext = null;
        this.sectors = [];
        this.focusedSectorIndex = 0;
        
        this.initialize();
    }
    
    /**
     * Initialize stadium viewer with enhanced features
     */
    initialize() {
        this.setupKeyboardNavigation();
        this.setupAccessibilityFeatures();
        this.setupTooltipManager();
        this.setupCanvasControls();
        
        // Announce to screen readers
        this.announceToScreenReader('Stadium viewer initialized with keyboard navigation support');
    }
    
    /**
     * Setup keyboard navigation for sectors
     */
    setupKeyboardNavigation() {
        document.addEventListener('keydown', (e) => {
            const activeElement = document.activeElement;
            
            if (activeElement && activeElement.classList.contains('sector-group')) {
                this.handleSectorKeyboardNavigation(e, activeElement);
            }
        });
    }
    
    /**
     * Handle keyboard navigation within stadium sectors
     */
    handleSectorKeyboardNavigation(e, currentSector) {
        const sectors = Array.from(document.querySelectorAll('.sector-group[tabindex="0"]'));
        const currentIndex = sectors.indexOf(currentSector);
        let nextIndex = currentIndex;
        
        switch (e.key) {
            case 'ArrowRight':
            case 'ArrowDown':
                nextIndex = (currentIndex + 1) % sectors.length;
                e.preventDefault();
                break;
            case 'ArrowLeft':
            case 'ArrowUp':
                nextIndex = (currentIndex - 1 + sectors.length) % sectors.length;
                e.preventDefault();
                break;
            case 'Home':
                nextIndex = 0;
                e.preventDefault();
                break;
            case 'End':
                nextIndex = sectors.length - 1;
                e.preventDefault();
                break;
            case 'Enter':
            case ' ':
                this.activateSector(currentSector);
                e.preventDefault();
                return;
        }
        
        if (nextIndex !== currentIndex) {
            sectors[nextIndex].focus();
            this.focusedSectorIndex = nextIndex;
            
            // Announce sector change to screen readers
            const sectorName = sectors[nextIndex].getAttribute('aria-label');
            this.announceToScreenReader(`Focused on ${sectorName}`);
        }
    }
    
    /**
     * Focus specific sector by ID
     */
    focusSector(sectorId) {
        const sector = document.querySelector(`[data-sector="${sectorId}"]`);
        if (sector && sector.hasAttribute('tabindex')) {
            sector.focus();
            
            // Smooth scroll into view if needed
            sector.scrollIntoView({
                behavior: 'smooth',
                block: 'nearest',
                inline: 'center'
            });
        }
    }
    
    /**
     * Activate sector (simulate click)
     */
    activateSector(sectorElement) {
        const clickEvent = new MouseEvent('click', {
            bubbles: true,
            cancelable: true,
            view: window
        });
        sectorElement.dispatchEvent(clickEvent);
    }
    
    /**
     * Setup accessibility features
     */
    setupAccessibilityFeatures() {
        // Add live region for announcements
        if (!document.getElementById('stadium-announcements')) {
            const liveRegion = document.createElement('div');
            liveRegion.id = 'stadium-announcements';
            liveRegion.setAttribute('aria-live', 'polite');
            liveRegion.setAttribute('aria-atomic', 'true');
            liveRegion.className = 'visually-hidden';
            document.body.appendChild(liveRegion);
        }
        
        // Setup sector hover announcements
        document.addEventListener('mouseover', (e) => {
            if (e.target.closest('.sector-group')) {
                const sector = e.target.closest('.sector-group');
                const label = sector.getAttribute('aria-label');
                if (label) {
                    this.announceToScreenReader(label, 'polite');
                }
            }
        });
    }
    
    /**
     * Announce message to screen readers
     */
    announceToScreenReader(message, priority = 'polite') {
        const announcer = document.getElementById('stadium-announcements');
        if (announcer) {
            announcer.setAttribute('aria-live', priority);
            announcer.textContent = message;
            
            // Clear after announcement
            setTimeout(() => {
                announcer.textContent = '';
            }, 1000);
        }
    }
    
    /**
     * Setup smart tooltip management
     */
    setupTooltipManager() {
        this.tooltipManager = new TooltipManager();
    }
    
    /**
     * Setup canvas controls for detailed seat view
     */
    setupCanvasControls() {
        document.addEventListener('wheel', this.handleCanvasZoom.bind(this), { passive: false });
        document.addEventListener('mousedown', this.handleCanvasPanStart.bind(this));
        document.addEventListener('mousemove', this.handleCanvasPan.bind(this));
        document.addEventListener('mouseup', this.handleCanvasPanEnd.bind(this));
        document.addEventListener('touchstart', this.handleTouchStart.bind(this), { passive: false });
        document.addEventListener('touchmove', this.handleTouchMove.bind(this), { passive: false });
        document.addEventListener('touchend', this.handleTouchEnd.bind(this));
    }
    
    /**
     * Handle canvas zoom via wheel
     */
    handleCanvasZoom(e) {
        const canvas = document.querySelector('.seat-canvas');
        if (!canvas || !e.target.closest('.seat-canvas-container')) return;
        
        e.preventDefault();
        
        const zoomDelta = e.deltaY > 0 ? 0.9 : 1.1;
        this.zoom(zoomDelta);
    }
    
    /**
     * Zoom canvas
     */
    zoom(factor) {
        this.currentZoom *= factor;
        this.currentZoom = Math.max(0.5, Math.min(5, this.currentZoom)); // Limit zoom range
        
        const canvas = document.querySelector('.seat-canvas');
        if (canvas) {
            canvas.style.transform = `scale(${this.currentZoom}) translate(${this.currentPan.x}px, ${this.currentPan.y}px)`;
            
            // Announce zoom level to screen readers
            this.announceToScreenReader(`Zoom level ${Math.round(this.currentZoom * 100)}%`);
        }
    }
    
    /**
     * Reset zoom and pan
     */
    resetZoom() {
        this.currentZoom = 1;
        this.currentPan = { x: 0, y: 0 };
        
        const canvas = document.querySelector('.seat-canvas');
        if (canvas) {
            canvas.style.transform = 'scale(1) translate(0, 0)';
            
            this.announceToScreenReader('Zoom reset to 100%');
        }
    }
    
    /**
     * Handle canvas pan start
     */
    handleCanvasPanStart(e) {
        const canvas = document.querySelector('.seat-canvas');
        if (!canvas || !e.target.closest('.seat-canvas-container')) return;
        
        this.isDragging = true;
        this.dragStart = { x: e.clientX - this.currentPan.x, y: e.clientY - this.currentPan.y };
        
        document.body.style.cursor = 'grabbing';
        e.preventDefault();
    }
    
    /**
     * Handle canvas pan
     */
    handleCanvasPan(e) {
        if (!this.isDragging) return;
        
        this.currentPan = {
            x: e.clientX - this.dragStart.x,
            y: e.clientY - this.dragStart.y
        };
        
        const canvas = document.querySelector('.seat-canvas');
        if (canvas) {
            canvas.style.transform = `scale(${this.currentZoom}) translate(${this.currentPan.x}px, ${this.currentPan.y}px)`;
        }
    }
    
    /**
     * Handle canvas pan end
     */
    handleCanvasPanEnd(e) {
        this.isDragging = false;
        document.body.style.cursor = '';
    }
    
    /**
     * Handle touch start
     */
    handleTouchStart(e) {
        const canvas = document.querySelector('.seat-canvas');
        if (!canvas || !e.target.closest('.seat-canvas-container')) return;
        
        if (e.touches.length === 1) {
            // Single touch - pan
            this.isDragging = true;
            const touch = e.touches[0];
            this.dragStart = { x: touch.clientX - this.currentPan.x, y: touch.clientY - this.currentPan.y };
        } else if (e.touches.length === 2) {
            // Pinch zoom
            this.lastTouchDistance = this.getTouchDistance(e.touches[0], e.touches[1]);
        }
        
        e.preventDefault();
    }
    
    /**
     * Handle touch move
     */
    handleTouchMove(e) {
        const canvas = document.querySelector('.seat-canvas');
        if (!canvas) return;
        
        if (e.touches.length === 1 && this.isDragging) {
            // Pan
            const touch = e.touches[0];
            this.currentPan = {
                x: touch.clientX - this.dragStart.x,
                y: touch.clientY - this.dragStart.y
            };
            
            canvas.style.transform = `scale(${this.currentZoom}) translate(${this.currentPan.x}px, ${this.currentPan.y}px)`;
        } else if (e.touches.length === 2) {
            // Pinch zoom
            const distance = this.getTouchDistance(e.touches[0], e.touches[1]);
            const zoomFactor = distance / this.lastTouchDistance;
            
            this.zoom(zoomFactor);
            this.lastTouchDistance = distance;
        }
        
        e.preventDefault();
    }
    
    /**
     * Handle touch end
     */
    handleTouchEnd(e) {
        this.isDragging = false;
        
        if (e.touches.length < 2) {
            this.lastTouchDistance = null;
        }
    }
    
    /**
     * Get distance between two touch points
     */
    getTouchDistance(touch1, touch2) {
        const dx = touch2.clientX - touch1.clientX;
        const dy = touch2.clientY - touch1.clientY;
        return Math.sqrt(dx * dx + dy * dy);
    }
    
    /**
     * Render seats on canvas with enhanced features
     */
    async renderSeatsOnCanvas(canvas, sectorData, eventId) {
        if (!canvas || !sectorData) return;
        
        const ctx = canvas.getContext('2d');
        this.canvasContext = ctx;
        
        // Clear canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        
        // Set up high DPI support
        const devicePixelRatio = window.devicePixelRatio || 1;
        const rect = canvas.getBoundingClientRect();
        
        canvas.width = rect.width * devicePixelRatio;
        canvas.height = rect.height * devicePixelRatio;
        
        ctx.scale(devicePixelRatio, devicePixelRatio);
        
        // Render seats
        this.renderSeats(ctx, sectorData, eventId);
        
        // Setup seat interaction
        this.setupSeatInteraction(canvas, sectorData);
    }
    
    /**
     * Render individual seats
     */
    renderSeats(ctx, sectorData, eventId) {
        const seatSize = 12;
        const seatPadding = 2;
        const rowHeight = seatSize + seatPadding;
        
        sectorData.rows.forEach((row, rowIndex) => {
            row.seats.forEach((seat, seatIndex) => {
                const x = seatIndex * (seatSize + seatPadding) + 10;
                const y = rowIndex * rowHeight + 10;
                
                // Determine seat color based on status
                let color = '#16a34a'; // Available - green
                if (seat.status === 'sold') {
                    color = '#dc2626'; // Sold - red
                } else if (seat.status === 'held') {
                    color = '#ca8a04'; // Reserved - amber
                } else if (!seat.isAccessible) {
                    color = '#6b7280'; // Blocked - gray
                }
                
                // Draw seat
                ctx.fillStyle = color;
                ctx.fillRect(x, y, seatSize, seatSize);
                
                // Add seat number for accessibility
                if (seat.isAccessible && seat.status === 'free') {
                    ctx.fillStyle = 'white';
                    ctx.font = '8px sans-serif';
                    ctx.textAlign = 'center';
                    ctx.fillText(seat.seatNumber.toString(), x + seatSize/2, y + seatSize/2 + 2);
                }
                
                // Store seat data for interaction
                seat._renderData = { x, y, width: seatSize, height: seatSize };
            });
            
            // Row label
            ctx.fillStyle = '#374151';
            ctx.font = 'bold 12px sans-serif';
            ctx.textAlign = 'right';
            ctx.fillText(`R${row.rowNumber}`, 8, rowIndex * rowHeight + 22);
        });
    }
    
    /**
     * Setup seat click interaction
     */
    setupSeatInteraction(canvas, sectorData) {
        const handleSeatClick = (e) => {
            const rect = canvas.getBoundingClientRect();
            const x = (e.clientX - rect.left) * (canvas.width / rect.width);
            const y = (e.clientY - rect.top) * (canvas.height / rect.height);
            
            // Find clicked seat
            for (const row of sectorData.rows) {
                for (const seat of row.seats) {
                    if (seat._renderData) {
                        const { x: seatX, y: seatY, width, height } = seat._renderData;
                        if (x >= seatX && x <= seatX + width && y >= seatY && y <= seatY + height) {
                            if (seat.isAccessible && seat.status === 'free') {
                                this.selectSeat(seat);
                                this.announceToScreenReader(`Selected seat ${seat.code}`);
                            } else {
                                this.announceToScreenReader(`Seat ${seat.code} is not available`);
                            }
                            return;
                        }
                    }
                }
            }
        };
        
        canvas.addEventListener('click', handleSeatClick);
    }
    
    /**
     * Select/deselect seat
     */
    selectSeat(seat) {
        // This will be handled by the Blazor component
        // For now, just provide visual feedback
        if (seat.selected) {
            seat.selected = false;
        } else {
            seat.selected = true;
        }
        
        // Re-render to show selection
        if (this.canvasContext && this.currentSectorData) {
            this.renderSeats(this.canvasContext, this.currentSectorData, this.currentEventId);
        }
    }
}

/**
 * Smart Tooltip Manager
 * Handles dynamic positioning and content
 */
class TooltipManager {
    constructor() {
        this.tooltip = null;
        this.isVisible = false;
        this.hideTimeout = null;
        
        this.createTooltip();
    }
    
    createTooltip() {
        this.tooltip = document.createElement('div');
        this.tooltip.className = 'smart-tooltip';
        this.tooltip.style.cssText = `
            position: fixed;
            background: rgba(15, 23, 42, 0.95);
            color: white;
            padding: 12px 16px;
            border-radius: 8px;
            font-size: 14px;
            max-width: 280px;
            backdrop-filter: blur(10px);
            box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.25);
            border: 1px solid rgba(255, 255, 255, 0.1);
            z-index: 10000;
            opacity: 0;
            transform: translateY(8px);
            transition: opacity 200ms ease, transform 200ms ease;
            pointer-events: none;
        `;
        document.body.appendChild(this.tooltip);
    }
    
    show(content, x, y) {
        if (this.hideTimeout) {
            clearTimeout(this.hideTimeout);
            this.hideTimeout = null;
        }
        
        this.tooltip.innerHTML = content;
        
        // Calculate optimal position
        const position = this.calculatePosition(x, y);
        this.tooltip.style.left = `${position.x}px`;
        this.tooltip.style.top = `${position.y}px`;
        
        // Show tooltip
        this.tooltip.style.opacity = '1';
        this.tooltip.style.transform = 'translateY(0)';
        this.isVisible = true;
    }
    
    hide() {
        this.hideTimeout = setTimeout(() => {
            this.tooltip.style.opacity = '0';
            this.tooltip.style.transform = 'translateY(8px)';
            this.isVisible = false;
        }, 100);
    }
    
    calculatePosition(mouseX, mouseY) {
        const tooltip = this.tooltip;
        const viewport = {
            width: window.innerWidth,
            height: window.innerHeight
        };
        
        // Get tooltip dimensions
        tooltip.style.visibility = 'hidden';
        tooltip.style.opacity = '1';
        const rect = tooltip.getBoundingClientRect();
        tooltip.style.visibility = 'visible';
        tooltip.style.opacity = '0';
        
        let x = mouseX + 15;
        let y = mouseY - rect.height - 10;
        
        // Adjust horizontal position if tooltip goes off-screen
        if (x + rect.width > viewport.width - 10) {
            x = mouseX - rect.width - 15;
        }
        
        // Adjust vertical position if tooltip goes off-screen
        if (y < 10) {
            y = mouseY + 15;
        }
        
        return { x, y };
    }
}

// Global instance
window.stadiumViewer = new StadiumViewer();

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { StadiumViewer, TooltipManager };
}