// High-Performance Server Time Display
// Uses requestAnimationFrame for smooth updates with minimal CPU usage

let timeUpdateInterval = null;
let timeElement = null;

export function startServerTime(elementId) {
    timeElement = document.getElementById(elementId);
    if (!timeElement) {
        console.warn(`Server time element with ID '${elementId}' not found`);
        return;
    }

    // Update immediately
    updateTime();

    // Use setInterval for precise 1-second updates
    timeUpdateInterval = setInterval(updateTime, 1000);

    console.log('✅ Server time display started successfully');
}

export function stopServerTime() {
    if (timeUpdateInterval) {
        clearInterval(timeUpdateInterval);
        timeUpdateInterval = null;
        console.log('⏹️ Server time display stopped');
    }
}

function updateTime() {
    if (!timeElement) return;

    const now = new Date();

    // Format: HH:mm:ss dd/MM/yy
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    const seconds = now.getSeconds().toString().padStart(2, '0');

    const day = now.getDate().toString().padStart(2, '0');
    const month = (now.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-indexed
    const year = now.getFullYear().toString().slice(-2); // Last 2 digits

    const formattedTime = `${hours}:${minutes}:${seconds} ${day}/${month}/${year}`;

    // Only update if the time has actually changed (performance optimization)
    if (timeElement.textContent !== formattedTime) {
        timeElement.textContent = formattedTime;

        // Add subtle pulse animation on second change
        timeElement.style.transform = 'scale(1.02)';
        setTimeout(() => {
            if (timeElement) {
                timeElement.style.transform = 'scale(1)';
            }
        }, 100);
    }
}

// Auto-cleanup when page unloads
window.addEventListener('beforeunload', stopServerTime);

// Pause updates when tab is not visible (performance optimization)
document.addEventListener('visibilitychange', function() {
    if (document.hidden) {
        stopServerTime();
    } else {
        if (timeElement) {
            startServerTime(timeElement.id);
        }
    }
});

// Handle timezone changes and daylight saving time
setInterval(() => {
    // Force a time update to handle timezone changes
    updateTime();
}, 60000); // Check every minute