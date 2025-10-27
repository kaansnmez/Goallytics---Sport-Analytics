// JavaScript for Football Statistics Website

// Global variables
let currentData = {
    predictions: [],
    leagues: [],
    teams: [],
    players: []
};

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    initializeApp();
    initializeAuth();
});

function initializeApp() {
    // Load initial data
    loadRecentPredictions();
    loadLeaguesData();
    
    // Add event listeners
    addEventListeners();
    
    // Initialize animations
    initializeAnimations();
}

function addEventListeners() {
    // Search functionality
    const searchInputs = document.querySelectorAll('.search-input');
    searchInputs.forEach(input => {
        input.addEventListener('input', handleSearch);
    });
    
    // Filter buttons
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(button => {
        button.addEventListener('click', handleFilter);
    });
    
    // Prediction refresh
    const refreshBtn = document.getElementById('refresh-predictions');
    if (refreshBtn) {
        refreshBtn.addEventListener('click', loadRecentPredictions);
    }
}

function initializeAnimations() {
    // Add fade-in animation to cards
    const cards = document.querySelectorAll('.card, .prediction-card, .league-card');
    cards.forEach((card, index) => {
        card.style.animationDelay = `${index * 0.1}s`;
        card.classList.add('fade-in');
    });
}

// Load recent predictions
async function loadRecentPredictions() {
    try {
        showLoading('recent-predictions');
        
        // Simulate API call - replace with actual API endpoint
        const predictions = await fetchPredictions();
        
        displayPredictions(predictions);
    } catch (error) {
        console.error('Error loading predictions:', error);
        showError('recent-predictions', 'Tahminler yüklenirken hata oluştu.');
    }
}

// Simulate API call for predictions
async function fetchPredictions() {
    // This would be replaced with actual API call
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve([
                {
                    id: 1,
                    homeTeam: 'Bayern Munich',
                    awayTeam: 'Borussia Dortmund',
                    homeOdds: 1.85,
                    awayOdds: 3.20,
                    drawOdds: 3.50,
                    prediction: '1',
                    confidence: 78,
                    date: '2024-01-15',
                    league: 'Bundesliga'
                },
                {
                    id: 2,
                    homeTeam: 'Real Madrid',
                    awayTeam: 'Barcelona',
                    homeOdds: 2.10,
                    awayOdds: 2.80,
                    drawOdds: 3.20,
                    prediction: 'X',
                    confidence: 65,
                    date: '2024-01-16',
                    league: 'La Liga'
                },
                {
                    id: 3,
                    homeTeam: 'Manchester City',
                    awayTeam: 'Liverpool',
                    homeOdds: 1.95,
                    awayOdds: 3.40,
                    drawOdds: 3.60,
                    prediction: '1',
                    confidence: 82,
                    date: '2024-01-17',
                    league: 'Premier League'
                }
            ]);
        }, 1000);
    });
}

function displayPredictions(predictions) {
    const container = document.getElementById('recent-predictions');
    if (!container) return;
    
    container.innerHTML = '';
    
    predictions.forEach(prediction => {
        const predictionCard = createPredictionCard(prediction);
        container.appendChild(predictionCard);
    });
}

function createPredictionCard(prediction) {
    const card = document.createElement('div');
    card.className = 'col-md-4 mb-4';
    
    const predictionResult = getPredictionText(prediction.prediction);
    const confidenceClass = getConfidenceClass(prediction.confidence);
    
    card.innerHTML = `
        <div class="prediction-card">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <span class="badge bg-primary">${prediction.league}</span>
                <span class="text-muted small">${formatDate(prediction.date)}</span>
            </div>
            
            <div class="d-flex align-items-center justify-content-between mb-3">
                <div class="d-flex align-items-center">
                    <div class="team-logo me-3">${prediction.homeTeam.charAt(0)}</div>
                    <div>
                        <div class="fw-bold">${prediction.homeTeam}</div>
                        <div class="text-muted small">Ev Sahibi</div>
                    </div>
                </div>
                
                <div class="text-center">
                    <div class="h5 mb-0">VS</div>
                </div>
                
                <div class="d-flex align-items-center">
                    <div>
                        <div class="fw-bold text-end">${prediction.awayTeam}</div>
                        <div class="text-muted small text-end">Deplasman</div>
                    </div>
                    <div class="team-logo ms-3">${prediction.awayTeam.charAt(0)}</div>
                </div>
            </div>
            
            <div class="d-flex justify-content-between align-items-center mb-3">
                <div class="prediction-odds">
                    <i class="fas fa-crystal-ball me-1"></i>
                    ${predictionResult}
                </div>
                <div class="prediction-confidence ${confidenceClass}">
                    %${prediction.confidence} Güven
                </div>
            </div>
            
            <div class="row text-center">
                <div class="col-4">
                    <div class="small text-muted">1</div>
                    <div class="fw-bold">${prediction.homeOdds}</div>
                </div>
                <div class="col-4">
                    <div class="small text-muted">X</div>
                    <div class="fw-bold">${prediction.drawOdds}</div>
                </div>
                <div class="col-4">
                    <div class="small text-muted">2</div>
                    <div class="fw-bold">${prediction.awayOdds}</div>
                </div>
            </div>
        </div>
    `;
    
    return card;
}

function getPredictionText(prediction) {
    switch(prediction) {
        case '1': return 'Ev Sahibi Kazanır';
        case 'X': return 'Beraberlik';
        case '2': return 'Deplasman Kazanır';
        default: return 'Bilinmiyor';
    }
}

function getConfidenceClass(confidence) {
    if (confidence >= 80) return 'bg-success';
    if (confidence >= 60) return 'bg-warning';
    return 'bg-danger';
}

// Load leagues data
async function loadLeaguesData() {
    try {
        // Simulate API call
        const leagues = await fetchLeagues();
        currentData.leagues = leagues;
    } catch (error) {
        console.error('Error loading leagues:', error);
    }
}

async function fetchLeagues() {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve([
                {
                    id: 1,
                    name: 'Premier League',
                    country: 'İngiltere',
                    teams: 20,
                    matches: 380,
                    logo: 'PL'
                },
                {
                    id: 2,
                    name: 'La Liga',
                    country: 'İspanya',
                    teams: 20,
                    matches: 380,
                    logo: 'LL'
                },
                {
                    id: 3,
                    name: 'Bundesliga',
                    country: 'Almanya',
                    teams: 18,
                    matches: 306,
                    logo: 'BL'
                },
                {
                    id: 4,
                    name: 'Serie A',
                    country: 'İtalya',
                    teams: 20,
                    matches: 380,
                    logo: 'SA'
                },
                {
                    id: 5,
                    name: 'Ligue 1',
                    country: 'Fransa',
                    teams: 20,
                    matches: 380,
                    logo: 'L1'
                }
            ]);
        }, 500);
    });
}

// Search functionality
function handleSearch(event) {
    const searchTerm = event.target.value.toLowerCase();
    const searchType = event.target.dataset.searchType;
    
    // Implement search logic based on searchType
    switch(searchType) {
        case 'predictions':
            searchPredictions(searchTerm);
            break;
        case 'teams':
            searchTeams(searchTerm);
            break;
        case 'players':
            searchPlayers(searchTerm);
            break;
    }
}

function searchPredictions(term) {
    // Filter predictions based on search term
    console.log('Searching predictions for:', term);
}

function searchTeams(term) {
    // Filter teams based on search term
    console.log('Searching teams for:', term);
}

function searchPlayers(term) {
    // Filter players based on search term
    console.log('Searching players for:', term);
}

// Filter functionality
function handleFilter(event) {
    const filterType = event.target.dataset.filterType;
    const filterValue = event.target.dataset.filterValue;
    
    // Remove active class from all filter buttons
    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    // Add active class to clicked button
    event.target.classList.add('active');
    
    // Apply filter
    applyFilter(filterType, filterValue);
}

function applyFilter(type, value) {
    console.log('Applying filter:', type, value);
    // Implement filter logic
}

// Utility functions
function showLoading(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = `
            <div class="text-center py-5">
                <div class="loading"></div>
                <div class="mt-3">Yükleniyor...</div>
            </div>
        `;
    }
}

function showError(containerId, message) {
    const container = document.getElementById(containerId);
    if (container) {
        container.innerHTML = `
            <div class="alert alert-danger text-center">
                <i class="fas fa-exclamation-triangle me-2"></i>
                ${message}
            </div>
        `;
    }
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

// C# Integration functions
function callCSharpMethod(methodName, parameters = {}) {
    // This function would be used to call C# methods from JavaScript
    // In a real application, this would use SignalR or similar technology
    console.log('Calling C# method:', methodName, parameters);
    
    // Example of how this might work with SignalR
    // connection.invoke(methodName, parameters)
    //     .then(result => {
    //         console.log('C# method result:', result);
    //     })
    //     .catch(error => {
    //         console.error('Error calling C# method:', error);
    //     });
}

// Authentication functions
function initializeAuth() {
    // Check if user is logged in
    const savedUser = localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser');
    const savedToken = localStorage.getItem('authToken') || sessionStorage.getItem('authToken');
    
    if (savedUser && savedToken) {
        updateUIForLoggedInUser();
    } else {
        updateUIForLoggedOutUser();
    }
}

function updateUIForLoggedInUser() {
    // Hide login/register buttons
    const loginNavItem = document.getElementById('loginNavItem');
    const registerNavItem = document.getElementById('registerNavItem');
    
    if (loginNavItem) loginNavItem.style.display = 'none';
    if (registerNavItem) registerNavItem.style.display = 'none';
    
    // Add user dropdown if not exists
    if (!document.querySelector('.user-dropdown')) {
        const navbar = document.querySelector('.navbar-nav');
        if (navbar) {
            const userDropdown = createUserDropdown();
            navbar.appendChild(userDropdown);
        }
    }
}

function updateUIForLoggedOutUser() {
    // Show login/register buttons
    const loginNavItem = document.getElementById('loginNavItem');
    const registerNavItem = document.getElementById('registerNavItem');
    
    if (loginNavItem) loginNavItem.style.display = 'block';
    if (registerNavItem) registerNavItem.style.display = 'block';
    
    // Remove user dropdown
    const userDropdown = document.querySelector('.user-dropdown');
    if (userDropdown) {
        userDropdown.remove();
    }
}

function createUserDropdown() {
    const user = JSON.parse(localStorage.getItem('currentUser') || sessionStorage.getItem('currentUser') || '{}');
    
    const dropdown = document.createElement('li');
    dropdown.className = 'nav-item dropdown user-dropdown';
    dropdown.innerHTML = `
        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown">
            <i class="fas fa-user me-1"></i>${user.firstName || 'Kullanıcı'}
        </a>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="profile.html"><i class="fas fa-user me-2"></i>Profilim</a></li>
            <li><a class="dropdown-item" href="settings.html"><i class="fas fa-cog me-2"></i>Ayarlar</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" href="#" onclick="logout()"><i class="fas fa-sign-out-alt me-2"></i>Çıkış Yap</a></li>
        </ul>
    `;
    return dropdown;
}

function logout() {
    // Clear storage
    localStorage.removeItem('currentUser');
    localStorage.removeItem('authToken');
    sessionStorage.removeItem('currentUser');
    sessionStorage.removeItem('authToken');
    
    // Update UI
    updateUIForLoggedOutUser();
    
    // Redirect to home page
    window.location.href = 'index.html';
    
    showSuccess('Çıkış yapıldı.');
}

function showSuccess(message) {
    // Create success notification
    const notification = document.createElement('div');
    notification.className = 'alert alert-success position-fixed';
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        <i class="fas fa-check-circle me-2"></i>${message}
        <button type="button" class="btn-close" onclick="this.parentElement.remove()"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentElement) {
            notification.remove();
        }
    }, 5000);
}

// Export functions for C# integration
window.FootballStats = {
    loadPredictions: loadRecentPredictions,
    loadLeagues: loadLeaguesData,
    searchPredictions: searchPredictions,
    searchTeams: searchTeams,
    searchPlayers: searchPlayers,
    callCSharpMethod: callCSharpMethod
};

// Export auth functions
window.logout = logout;
