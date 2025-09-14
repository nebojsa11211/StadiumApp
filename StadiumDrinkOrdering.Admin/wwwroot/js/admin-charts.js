/**
 * Admin Charts Module - Chart.js Integration for Dashboard
 * Provides chart initialization and update functionality for the admin dashboard
 */

window.adminCharts = {
    ordersChart: null,
    revenueChart: null,

    /**
     * Initialize the orders chart with hourly data
     */
    initializeOrdersChart: function(canvasId, data) {
        try {
            const ctx = document.getElementById(canvasId);
            if (!ctx) {
                console.warn(`Canvas element ${canvasId} not found`);
                return;
            }

            // Destroy existing chart if it exists
            if (this.ordersChart) {
                this.ordersChart.destroy();
            }

            this.ordersChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: data.labels || this.getDefaultHourlyLabels(),
                    datasets: [{
                        label: 'Orders',
                        data: data.values || [],
                        borderColor: 'var(--admin-primary-color)',
                        backgroundColor: 'var(--admin-primary-color-alpha)',
                        borderWidth: 2,
                        fill: true,
                        tension: 0.4,
                        pointBackgroundColor: 'var(--admin-primary-color)',
                        pointBorderColor: '#ffffff',
                        pointBorderWidth: 2,
                        pointRadius: 4,
                        pointHoverRadius: 6
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    interaction: {
                        intersect: false,
                        mode: 'index'
                    },
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            backgroundColor: 'var(--admin-card-background)',
                            titleColor: 'var(--admin-text-primary)',
                            bodyColor: 'var(--admin-text-primary)',
                            borderColor: 'var(--admin-border-color)',
                            borderWidth: 1,
                            cornerRadius: 8,
                            displayColors: false,
                            callbacks: {
                                title: function(context) {
                                    return `${context[0].label}:00`;
                                },
                                label: function(context) {
                                    return `${context.parsed.y} orders`;
                                }
                            }
                        }
                    },
                    scales: {
                        x: {
                            grid: {
                                display: false
                            },
                            border: {
                                display: false
                            },
                            ticks: {
                                color: 'var(--admin-text-secondary)',
                                font: {
                                    size: 12
                                }
                            }
                        },
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'var(--admin-border-color)',
                                drawBorder: false
                            },
                            border: {
                                display: false
                            },
                            ticks: {
                                color: 'var(--admin-text-secondary)',
                                font: {
                                    size: 12
                                },
                                stepSize: 1
                            }
                        }
                    },
                    elements: {
                        point: {
                            hoverBorderWidth: 3
                        }
                    }
                }
            });

            console.log('Orders chart initialized successfully');
        } catch (error) {
            console.error('Error initializing orders chart:', error);
        }
    },

    /**
     * Initialize the revenue chart with daily data
     */
    initializeRevenueChart: function(canvasId, data) {
        try {
            const ctx = document.getElementById(canvasId);
            if (!ctx) {
                console.warn(`Canvas element ${canvasId} not found`);
                return;
            }

            // Destroy existing chart if it exists
            if (this.revenueChart) {
                this.revenueChart.destroy();
            }

            this.revenueChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.labels || this.getDefaultWeeklyLabels(),
                    datasets: [{
                        label: 'Revenue',
                        data: data.values || [],
                        backgroundColor: 'var(--admin-success-color-alpha)',
                        borderColor: 'var(--admin-success-color)',
                        borderWidth: 1,
                        borderRadius: 4,
                        borderSkipped: false
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    interaction: {
                        intersect: false,
                        mode: 'index'
                    },
                    plugins: {
                        legend: {
                            display: false
                        },
                        tooltip: {
                            backgroundColor: 'var(--admin-card-background)',
                            titleColor: 'var(--admin-text-primary)',
                            bodyColor: 'var(--admin-text-primary)',
                            borderColor: 'var(--admin-border-color)',
                            borderWidth: 1,
                            cornerRadius: 8,
                            displayColors: false,
                            callbacks: {
                                title: function(context) {
                                    return context[0].label;
                                },
                                label: function(context) {
                                    return `$${context.parsed.y.toLocaleString('en-US', { minimumFractionDigits: 2 })}`;
                                }
                            }
                        }
                    },
                    scales: {
                        x: {
                            grid: {
                                display: false
                            },
                            border: {
                                display: false
                            },
                            ticks: {
                                color: 'var(--admin-text-secondary)',
                                font: {
                                    size: 12
                                }
                            }
                        },
                        y: {
                            beginAtZero: true,
                            grid: {
                                color: 'var(--admin-border-color)',
                                drawBorder: false
                            },
                            border: {
                                display: false
                            },
                            ticks: {
                                color: 'var(--admin-text-secondary)',
                                font: {
                                    size: 12
                                },
                                callback: function(value) {
                                    return '$' + value.toLocaleString();
                                }
                            }
                        }
                    }
                }
            });

            console.log('Revenue chart initialized successfully');
        } catch (error) {
            console.error('Error initializing revenue chart:', error);
        }
    },

    /**
     * Update the orders chart with new data
     */
    updateOrdersChart: function(data) {
        try {
            if (!this.ordersChart) {
                console.warn('Orders chart not initialized');
                return;
            }

            this.ordersChart.data.labels = data.labels || this.getDefaultHourlyLabels();
            this.ordersChart.data.datasets[0].data = data.values || [];
            this.ordersChart.update('active');

            console.log('Orders chart updated successfully');
        } catch (error) {
            console.error('Error updating orders chart:', error);
        }
    },

    /**
     * Update the revenue chart with new data
     */
    updateRevenueChart: function(data) {
        try {
            if (!this.revenueChart) {
                console.warn('Revenue chart not initialized');
                return;
            }

            this.revenueChart.data.labels = data.labels || this.getDefaultWeeklyLabels();
            this.revenueChart.data.datasets[0].data = data.values || [];
            this.revenueChart.update('active');

            console.log('Revenue chart updated successfully');
        } catch (error) {
            console.error('Error updating revenue chart:', error);
        }
    },

    /**
     * Get default hourly labels (00-23)
     */
    getDefaultHourlyLabels: function() {
        const labels = [];
        for (let i = 0; i < 24; i++) {
            labels.push(i.toString().padStart(2, '0'));
        }
        return labels;
    },

    /**
     * Get default weekly labels (last 7 days)
     */
    getDefaultWeeklyLabels: function() {
        const labels = [];
        const today = new Date();

        for (let i = 6; i >= 0; i--) {
            const date = new Date(today);
            date.setDate(date.getDate() - i);
            labels.push(date.toLocaleDateString('en-US', {
                month: 'short',
                day: 'numeric'
            }));
        }

        return labels;
    },

    /**
     * Resize charts when window is resized
     */
    resizeCharts: function() {
        try {
            if (this.ordersChart) {
                this.ordersChart.resize();
            }
            if (this.revenueChart) {
                this.revenueChart.resize();
            }
        } catch (error) {
            console.error('Error resizing charts:', error);
        }
    },

    /**
     * Update chart themes when theme changes
     */
    updateChartThemes: function() {
        try {
            // Update orders chart
            if (this.ordersChart) {
                this.ordersChart.update('none');
            }

            // Update revenue chart
            if (this.revenueChart) {
                this.revenueChart.update('none');
            }
        } catch (error) {
            console.error('Error updating chart themes:', error);
        }
    },

    /**
     * Cleanup charts
     */
    cleanup: function() {
        try {
            if (this.ordersChart) {
                this.ordersChart.destroy();
                this.ordersChart = null;
            }
            if (this.revenueChart) {
                this.revenueChart.destroy();
                this.revenueChart = null;
            }
        } catch (error) {
            console.error('Error cleaning up charts:', error);
        }
    }
};

// Handle window resize
window.addEventListener('resize', function() {
    adminCharts.resizeCharts();
});

// Handle visibility change to pause/resume chart animations
document.addEventListener('visibilitychange', function() {
    if (document.hidden) {
        // Pause animations when tab is not visible
        Chart.defaults.animation = false;
    } else {
        // Resume animations when tab becomes visible
        Chart.defaults.animation = true;
    }
});

// Initialize Chart.js defaults
Chart.defaults.font.family = 'var(--admin-font-family)';
Chart.defaults.color = 'var(--admin-text-secondary)';
Chart.defaults.plugins.legend.labels.usePointStyle = true;

console.log('Admin Charts module loaded successfully');