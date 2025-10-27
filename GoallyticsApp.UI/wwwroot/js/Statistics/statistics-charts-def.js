// Statistics Charts JavaScript
// Tüm grafik fonksiyonları bu dosyada

/**
 * Ana başlatma fonksiyonu - Razor'dan çağrılır
 * @param {Object} chartData - Grafik verileri
 * @param {Object} marketData - Market istatistikleri
 * @param {Object} insightsData - İçgörüler
 */
function initializeStatisticsCharts(chartData, marketData, insightsData) {
    console.log('📊 Statistics charts initializing...');
    console.log('Chart Data:', chartData);
    console.log('Market Data:', marketData);
    console.log('Insights Data:', insightsData);
    
    // Grafikleri başlat
    initMarketAccuracyChart(chartData.marketAccuracy);
    initConfidenceAccuracyChart(chartData.confidenceAccuracy);
    initLeagueAccuracyChart(chartData.leagueAccuracy);
    initMonthlyPerformanceChart(chartData.monthlyPerformance);
    
    // Market kartlarını güncelle
    updateMarketCards(marketData);
    
    // İçgörüleri güncelle
    updateInsights(insightsData);
    
    console.log('✅ All charts initialized');
}

/**
 * Market Accuracy Bar Chart
 */
function initMarketAccuracyChart(data) {
    const ctx = document.getElementById('marketAccuracyChart');
    if (!ctx) {
        console.warn('marketAccuracyChart canvas not found');
        return;
    }
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['1X2', 'Alt/Üst 1.5', 'Alt/Üst 2.5', 'Alt/Üst 3.5', 'BTTS'],
            datasets: [{
                label: 'Doğruluk Oranı (%)',
                data: data.accuracies,
                backgroundColor: [
                    'rgba(54, 162, 235, 0.8)',
                    'rgba(75, 192, 192, 0.8)',
                    'rgba(54, 162, 235, 0.8)',
                    'rgba(255, 206, 86, 0.8)',
                    'rgba(153, 102, 255, 0.8)'
                ],
                borderColor: [
                    'rgb(54, 162, 235)',
                    'rgb(75, 192, 192)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 206, 86)',
                    'rgb(153, 102, 255)'
                ],
                borderWidth: 2,
                borderRadius: 8
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
                    callbacks: {
                        label: function(context) {
                            return 'Doğruluk: %' + context.parsed.y.toFixed(1);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    max: 100,
                    ticks: {
                        callback: function(value) {
                            return '%' + value;
                        }
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
    
    console.log('✓ Market Accuracy Chart initialized');
}

/**
 * Confidence vs Accuracy Line Chart
 */
function initConfidenceAccuracyChart(data) {
    const ctx = document.getElementById('confidenceAccuracyChart');
    if (!ctx) {
        console.warn('confidenceAccuracyChart canvas not found');
        return;
    }
    
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.confidenceBands,
            datasets: [
                {
                    label: 'Gerçek Doğruluk',
                    data: data.actualAccuracy,
                    borderColor: 'rgb(75, 192, 192)',
                    backgroundColor: 'rgba(75, 192, 192, 0.1)',
                    tension: 0.4,
                    fill: true,
                    borderWidth: 3,
                    pointRadius: 5,
                    pointHoverRadius: 7,
                    pointBackgroundColor: 'rgb(75, 192, 192)',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2
                },
                {
                    label: 'Beklenen Doğruluk',
                    data: data.expectedAccuracy,
                    borderColor: 'rgb(255, 99, 132)',
                    backgroundColor: 'rgba(255, 99, 132, 0.1)',
                    tension: 0.4,
                    fill: true,
                    borderWidth: 2,
                    borderDash: [5, 5],
                    pointRadius: 4,
                    pointHoverRadius: 6,
                    pointBackgroundColor: 'rgb(255, 99, 132)',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2
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
                        usePointStyle: true,
                        padding: 15
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.dataset.label + ': %' + context.parsed.y.toFixed(1);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    max: 100,
                    ticks: {
                        callback: function(value) {
                            return '%' + value;
                        }
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
    
    console.log('✓ Confidence Accuracy Chart initialized');
}

/**
 * League Accuracy Horizontal Bar Chart
 */
function initLeagueAccuracyChart(data) {
    const ctx = document.getElementById('leagueAccuracyChart');
    if (!ctx) {
        console.warn('leagueAccuracyChart canvas not found');
        return;
    }
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.leagues,
            datasets: [{
                label: 'Doğruluk Oranı (%)',
                data: data.accuracies,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.8)',
                    'rgba(54, 162, 235, 0.8)',
                    'rgba(255, 206, 86, 0.8)',
                    'rgba(75, 192, 192, 0.8)',
                    'rgba(153, 102, 255, 0.8)',
                    'rgba(255, 159, 64, 0.8)'
                ],
                borderColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 206, 86)',
                    'rgb(75, 192, 192)',
                    'rgb(153, 102, 255)',
                    'rgb(255, 159, 64)'
                ],
                borderWidth: 2,
                borderRadius: 6
            }]
        },
        options: {
            indexAxis: 'y', // Horizontal bar
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return 'Doğruluk: %' + context.parsed.x.toFixed(1);
                        }
                    }
                }
            },
            scales: {
                x: {
                    beginAtZero: true,
                    max: 100,
                    ticks: {
                        callback: function(value) {
                            return '%' + value;
                        }
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                y: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
    
    console.log('✓ League Accuracy Chart initialized');
}

/**
 * Monthly Performance Line Chart
 */
function initMonthlyPerformanceChart(data) {
    const ctx = document.getElementById('monthlyPerformanceChart');
    if (!ctx) {
        console.warn('monthlyPerformanceChart canvas not found');
        return;
    }
    
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.months,
            datasets: [{
                label: 'Doğruluk %',
                data: data.accuracies,
                borderColor: 'rgb(54, 162, 235)',
                backgroundColor: 'rgba(54, 162, 235, 0.1)',
                tension: 0.4,
                fill: true,
                borderWidth: 3,
                pointRadius: 5,
                pointHoverRadius: 7,
                pointBackgroundColor: 'rgb(54, 162, 235)',
                pointBorderColor: '#fff',
                pointBorderWidth: 2
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
                    callbacks: {
                        label: function(context) {
                            return 'Doğruluk: %' + context.parsed.y.toFixed(1);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    max: 100,
                    ticks: {
                        callback: function(value) {
                            return '%' + value;
                        }
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
    
    console.log('✓ Monthly Performance Chart initialized');
}

/**
 * Market kartlarını güncelle
 */
function updateMarketCards(markets) {
    updateMarketCard('market1x2', markets.market1x2);
    updateMarketCard('marketOU15', markets.marketOU15);
    updateMarketCard('marketOU25', markets.marketOU25);
    updateMarketCard('marketOU35', markets.marketOU35);
    updateMarketCard('marketBTTS', markets.marketBTTS);
    
    // Genel ortalama hesapla
    const overall = {
        total: markets.market1x2.total + markets.marketOU15.total + 
               markets.marketOU25.total + markets.marketOU35.total + 
               markets.marketBTTS.total,
        correct: markets.market1x2.correct + markets.marketOU15.correct + 
                 markets.marketOU25.correct + markets.marketOU35.correct + 
                 markets.marketBTTS.correct
    };
    overall.accuracy = overall.total > 0 ? (overall.correct / overall.total) * 100 : 0;
    updateMarketCard('overall', overall);
    
    console.log('✓ Market cards updated');
}

/**
 * Tek bir market kartını güncelle
 */
function updateMarketCard(marketId, data) {
    const totalEl = document.getElementById(marketId + 'Total');
    const correctEl = document.getElementById(marketId + 'Correct');
    const accuracyEl = document.getElementById(marketId + 'Accuracy');
    const progressEl = document.getElementById(marketId + 'Progress');
    
    if (totalEl) totalEl.textContent = formatNumber(data.total);
    if (correctEl) correctEl.textContent = formatNumber(data.correct);
    if (accuracyEl) accuracyEl.textContent = data.accuracy.toFixed(1) + '%';
    if (progressEl) progressEl.style.width = data.accuracy.toFixed(1) + '%';
}

/**
 * İçgörüleri güncelle
 */
function updateInsights(insights) {
    const bestMarketEl = document.getElementById('bestMarket');
    const bestMarketAccEl = document.getElementById('bestMarketAccuracy');
    const bestLeagueEl = document.getElementById('bestLeague');
    const bestLeagueAccEl = document.getElementById('bestLeagueAccuracy');
    const avgConfidenceEl = document.getElementById('avgConfidence');
    
    if (bestMarketEl) bestMarketEl.textContent = insights.bestMarket.name;
    if (bestMarketAccEl) bestMarketAccEl.textContent = '%' + insights.bestMarket.accuracy.toFixed(1);
    if (bestLeagueEl) bestLeagueEl.textContent = insights.bestLeague.name;
    if (bestLeagueAccEl) bestLeagueAccEl.textContent = '%' + insights.bestLeague.accuracy.toFixed(1);
    if (avgConfidenceEl) avgConfidenceEl.textContent = '%' + insights.avgConfidence.toFixed(1);
    
    console.log('✓ Insights updated');
}

/**
 * Sayı formatla (1000 -> 1.000)
 */
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

// Global scope'a export et (Razor'dan erişim için)
if (typeof window !== 'undefined') {
    window.initializeStatisticsCharts = initializeStatisticsCharts;
}

console.log('📈 statistics-charts.js loaded');

