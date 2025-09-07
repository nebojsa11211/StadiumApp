// Stadium Analytics Dashboard - Chart.js Integration

let ordersChart = null;
let revenueChart = null;
let statusChart = null;

// Initialize all analytics charts
window.initializeAnalyticsCharts = (analyticsData) => {
    initializeOrdersChart(analyticsData);
    initializeRevenueChart(analyticsData);
    initializeStatusChart(analyticsData);
    
    // Auto-refresh charts every 30 seconds
    setInterval(() => {
        refreshChartsData();
    }, 30000);
};

// Orders Over Time Chart
function initializeOrdersChart(analyticsData) {
    const ctx = document.getElementById('ordersChart');
    if (!ctx) return;

    // Generate sample time-series data
    const labels = [];
    const data = [];
    const now = new Date();
    
    for (let i = 23; i >= 0; i--) {
        const time = new Date(now.getTime() - (i * 60 * 60 * 1000));
        labels.push(time.getHours().toString().padStart(2, '0') + ':00');
        data.push(Math.floor(Math.random() * 30) + 5);
    }

    if (ordersChart) {
        ordersChart.destroy();
    }

    ordersChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Orders',
                data: data,
                borderColor: '#2563eb',
                backgroundColor: 'rgba(37, 99, 235, 0.1)',
                borderWidth: 3,
                fill: true,
                tension: 0.4,
                pointBackgroundColor: '#2563eb',
                pointBorderColor: '#ffffff',
                pointBorderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#ffffff',
                    bodyColor: '#ffffff',
                    borderColor: '#2563eb',
                    borderWidth: 1,
                    cornerRadius: 8
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: '#64748b'
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        color: '#e2e8f0'
                    },
                    ticks: {
                        color: '#64748b'
                    }
                }
            },
            interaction: {
                intersect: false,
                mode: 'index'
            },
            animation: {
                duration: 1000,
                easing: 'easeInOutCubic'
            }
        }
    });
}

// Revenue Chart
function initializeRevenueChart(analyticsData) {
    const ctx = document.getElementById('revenueChart');
    if (!ctx) return;

    // Generate sample revenue data
    const labels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    const revenueData = [1200, 1900, 3000, 2500, 2200, 3200, 2800];
    const targetData = [2000, 2000, 2000, 2000, 2000, 2000, 2000];

    if (revenueChart) {
        revenueChart.destroy();
    }

    revenueChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Revenue',
                    data: revenueData,
                    backgroundColor: 'rgba(34, 197, 94, 0.8)',
                    borderColor: '#22c55e',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                },
                {
                    label: 'Target',
                    data: targetData,
                    type: 'line',
                    borderColor: '#f59e0b',
                    backgroundColor: 'rgba(245, 158, 11, 0.1)',
                    borderWidth: 3,
                    borderDash: [5, 5],
                    fill: false,
                    pointRadius: 0,
                    tension: 0
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        color: '#64748b',
                        usePointStyle: true,
                        padding: 20
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#ffffff',
                    bodyColor: '#ffffff',
                    borderColor: '#22c55e',
                    borderWidth: 1,
                    cornerRadius: 8,
                    callbacks: {
                        label: function(context) {
                            if (context.datasetIndex === 0) {
                                return 'Revenue: $' + context.parsed.y.toLocaleString();
                            } else {
                                return 'Target: $' + context.parsed.y.toLocaleString();
                            }
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: '#64748b'
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        color: '#e2e8f0'
                    },
                    ticks: {
                        color: '#64748b',
                        callback: function(value) {
                            return '$' + value.toLocaleString();
                        }
                    }
                }
            },
            animation: {
                duration: 1200,
                easing: 'easeInOutQuart'
            }
        }
    });
}

// Order Status Distribution Chart
function initializeStatusChart(analyticsData) {
    const ctx = document.getElementById('statusChart');
    if (!ctx) return;

    const statusData = {
        'Pending': 15,
        'Preparing': 25,
        'Ready': 8,
        'Out for Delivery': 12,
        'Delivered': 180,
        'Cancelled': 3
    };

    const colors = {
        'Pending': '#f59e0b',
        'Preparing': '#3b82f6',
        'Ready': '#10b981',
        'Out for Delivery': '#8b5cf6',
        'Delivered': '#22c55e',
        'Cancelled': '#ef4444'
    };

    if (statusChart) {
        statusChart.destroy();
    }

    statusChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: Object.keys(statusData),
            datasets: [{
                data: Object.values(statusData),
                backgroundColor: Object.keys(statusData).map(status => colors[status]),
                borderColor: '#ffffff',
                borderWidth: 3,
                hoverBorderWidth: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '60%',
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        color: '#64748b',
                        usePointStyle: true,
                        padding: 20,
                        font: {
                            size: 12
                        }
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#ffffff',
                    bodyColor: '#ffffff',
                    borderColor: '#64748b',
                    borderWidth: 1,
                    cornerRadius: 8,
                    callbacks: {
                        label: function(context) {
                            const total = context.dataset.data.reduce((sum, value) => sum + value, 0);
                            const percentage = ((context.parsed * 100) / total).toFixed(1);
                            return context.label + ': ' + context.parsed + ' (' + percentage + '%)';
                        }
                    }
                }
            },
            animation: {
                animateRotate: true,
                animateScale: false,
                duration: 1500,
                easing: 'easeInOutCubic'
            }
        }
    });
}

// Refresh chart data (simulate real-time updates)
function refreshChartsData() {
    if (ordersChart) {
        // Add new data point and remove the oldest
        const newValue = Math.floor(Math.random() * 30) + 5;
        ordersChart.data.datasets[0].data.push(newValue);
        ordersChart.data.datasets[0].data.shift();
        
        // Update time labels
        const now = new Date();
        const newLabel = now.getHours().toString().padStart(2, '0') + ':' + now.getMinutes().toString().padStart(2, '0');
        ordersChart.data.labels.push(newLabel);
        ordersChart.data.labels.shift();
        
        ordersChart.update('none'); // No animation for real-time updates
    }
    
    // Randomly update revenue chart
    if (revenueChart && Math.random() > 0.7) {
        const datasetIndex = 0;
        const dataIndex = Math.floor(Math.random() * revenueChart.data.datasets[datasetIndex].data.length);
        const currentValue = revenueChart.data.datasets[datasetIndex].data[dataIndex];
        const change = (Math.random() - 0.5) * 200; // ±100 change
        revenueChart.data.datasets[datasetIndex].data[dataIndex] = Math.max(0, currentValue + change);
        revenueChart.update('none');
    }
    
    // Update status chart occasionally
    if (statusChart && Math.random() > 0.8) {
        const dataIndex = Math.floor(Math.random() * statusChart.data.datasets[0].data.length);
        const change = Math.floor((Math.random() - 0.5) * 6); // ±3 change
        const currentValue = statusChart.data.datasets[0].data[dataIndex];
        statusChart.data.datasets[0].data[dataIndex] = Math.max(0, currentValue + change);
        statusChart.update('none');
    }
}

// Export chart as image
window.exportChart = (chartType, filename = null) => {
    let chart = null;
    switch(chartType) {
        case 'orders': chart = ordersChart; break;
        case 'revenue': chart = revenueChart; break;
        case 'status': chart = statusChart; break;
    }
    
    if (chart) {
        const url = chart.toBase64Image();
        const link = document.createElement('a');
        link.href = url;
        link.download = filename || `stadium-${chartType}-chart.png`;
        link.click();
        
        // Show success toast
        if (typeof showToast !== 'undefined') {
            showToast(`Chart exported as ${link.download}`, 'success', 3000);
        }
    }
};

// Initialize charts when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    // Load Chart.js if not already loaded
    if (typeof Chart === 'undefined') {
        const script = document.createElement('script');
        script.src = 'https://cdn.jsdelivr.net/npm/chart.js@3.9.1/dist/chart.min.js';
        script.onload = function() {
            console.log('Chart.js loaded successfully');
        };
        document.head.appendChild(script);
    }
});

// Cleanup function
window.destroyAnalyticsCharts = () => {
    if (ordersChart) {
        ordersChart.destroy();
        ordersChart = null;
    }
    if (revenueChart) {
        revenueChart.destroy();
        revenueChart = null;
    }
    if (statusChart) {
        statusChart.destroy();
        statusChart = null;
    }
};

// Download file function for CSV exports
window.downloadFile = (filename, content) => {
    const blob = new Blob([content], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    
    link.setAttribute('href', url);
    link.setAttribute('download', filename);
    link.style.visibility = 'hidden';
    
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    
    URL.revokeObjectURL(url);
};