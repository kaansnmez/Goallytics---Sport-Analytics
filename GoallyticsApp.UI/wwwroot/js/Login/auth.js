// Authentication JavaScript

// Global authentication state
let currentUser = null;
let authToken = null;

// Initialize authentication
document.addEventListener('DOMContentLoaded', function() {
    initializeAuth();
    addAuthEventListeners();
});

function initializeAuth() {
    // Check if user is already logged in
    const savedUser = localStorage.getItem('currentUser');
    const savedToken = localStorage.getItem('authToken');
    
    if (savedUser && savedToken) {
        currentUser = JSON.parse(savedUser);
        authToken = savedToken;
        updateUIForLoggedInUser();
    } else {
        updateUIForLoggedOutUser();
    }
}

function addAuthEventListeners() {
    // Login form - let native submit happen (disable JS interception)
    // const loginForm = document.getElementById('loginForm');
    // if (loginForm) {
    //     loginForm.addEventListener('submit', handleLogin);
    // }
    
    // Register form - let native submit happen (disable JS interception)
    // const registerForm = document.getElementById('registerForm');
    // if (registerForm) {
    //     registerForm.addEventListener('submit', handleRegister);
    // }
    
    // Password toggle buttons
    const togglePassword = document.getElementById('togglePassword');
    if (togglePassword) {
        togglePassword.addEventListener('click', () => togglePasswordVisibility('password', 'togglePassword'));
    }
    
    const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
    if (toggleConfirmPassword) {
        toggleConfirmPassword.addEventListener('click', () => togglePasswordVisibility('confirmPassword', 'toggleConfirmPassword'));
    }
    
    // Password strength checker
    const passwordInput = document.getElementById('password');
    if (passwordInput) {
        passwordInput.addEventListener('input', checkPasswordStrength);
    }
    
    // Social login buttons
    const googleLogin = document.getElementById('googleLogin');
    if (googleLogin) {
        googleLogin.addEventListener('click', handleGoogleLogin);
    }
    
    const facebookLogin = document.getElementById('facebookLogin');
    if (facebookLogin) {
        facebookLogin.addEventListener('click', handleFacebookLogin);
    }
    
    const googleRegister = document.getElementById('googleRegister');
    if (googleRegister) {
        googleRegister.addEventListener('click', handleGoogleRegister);
    }
    
    const facebookRegister = document.getElementById('facebookRegister');
    if (facebookRegister) {
        facebookRegister.addEventListener('click', handleFacebookRegister);
    }
}

// Login handler
async function handleLogin(event) {
    const form = event && event.target ? event.target : null;
    if (!form || form.getAttribute('data-js-submit') !== 'true') {
        // Do not intercept native submit unless explicitly enabled via data-js-submit="true"
        return;
    }
    // Intentionally do not call preventDefault; allow native POST to proceed
    
    const formData = new FormData(event.target);
    const loginData = {
        email: formData.get('email'),
        password: formData.get('password'),
        rememberMe: formData.get('rememberMe') === 'on'
    };
    
    // Clear previous errors
    clearFormErrors('loginForm');
    
    // Validate form
    if (!validateLoginForm(loginData)) {
        return;
    }
    
    try {
        showLoading('loginBtn', 'Giriş yapılıyor...');
        
        // Simulate API call
        const response = await loginUser(loginData);
        
        if (response.success) {
            currentUser = response.user;
            authToken = response.token;
            
            // Save to localStorage
            if (loginData.rememberMe) {
                localStorage.setItem('currentUser', JSON.stringify(currentUser));
                localStorage.setItem('authToken', authToken);
            } else {
                sessionStorage.setItem('currentUser', JSON.stringify(currentUser));
                sessionStorage.setItem('authToken', authToken);
            }
            
            // Update UI
            updateUIForLoggedInUser();
            
            // Redirect to home page
            window.location.href = 'index.html';
            
            showSuccess('Giriş başarılı! Hoş geldiniz.');
        } else {
            showError('loginForm', response.message || 'Giriş başarısız.');
        }
    } catch (error) {
        console.error('Login error:', error);
        showError('loginForm', 'Giriş sırasında bir hata oluştu.');
    } finally {
        hideLoading('loginBtn', 'Giriş Yap');
    }
}

// Register handler
async function handleRegister(event) {
    const form = event && event.target ? event.target : null;
    if (!form || form.getAttribute('data-js-submit') !== 'true') {
        // Do not intercept native submit unless explicitly enabled via data-js-submit="true"
        return;
    }
    // Intentionally do not call preventDefault; allow native POST to proceed
    
    const formData = new FormData(event.target);
    const registerData = {
        firstName: formData.get('firstName'),
        lastName: formData.get('lastName'),
        email: formData.get('email'),
        username: formData.get('username'),
        password: formData.get('password'),
        confirmPassword: formData.get('confirmPassword'),
        phone: formData.get('phone'),
        birthDate: formData.get('birthDate'),
        favoriteTeam: formData.get('favoriteTeam'),
        terms: formData.get('terms') === 'on',
        newsletter: formData.get('newsletter') === 'on'
    };
    
    // Clear previous errors
    clearFormErrors('registerForm');
    
    // Validate form
    if (!validateRegisterForm(registerData)) {
        return;
    }
    
    try {
        showLoading('registerBtn', 'Kayıt oluşturuluyor...');
        
        // Simulate API call
        const response = await registerUser(registerData);
        
        if (response.success) {
            showSuccess('Kayıt başarılı! Giriş sayfasına yönlendiriliyorsunuz.');
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 2000);
        } else {
            showError('registerForm', response.message || 'Kayıt başarısız.');
        }
    } catch (error) {
        console.error('Register error:', error);
        showError('registerForm', 'Kayıt sırasında bir hata oluştu.');
    } finally {
        hideLoading('registerBtn', 'Kayıt Ol');
    }
}

// Form validation
function validateLoginForm(data) {
    let isValid = true;
    
    if (!data.email || !isValidEmail(data.email)) {
        showFieldError('email', 'Geçerli bir e-posta adresi giriniz.');
        isValid = false;
    }
    
    if (!data.password || data.password.length < 6) {
        showFieldError('password', 'Şifre en az 6 karakter olmalıdır.');
        isValid = false;
    }
    
    return isValid;
}

function validateRegisterForm(data) {
    let isValid = true;
    
    if (!data.firstName || data.firstName.length < 2) {
        showFieldError('firstName', 'Ad en az 2 karakter olmalıdır.');
        isValid = false;
    }
    
    if (!data.lastName || data.lastName.length < 2) {
        showFieldError('lastName', 'Soyad en az 2 karakter olmalıdır.');
        isValid = false;
    }
    
    if (!data.email || !isValidEmail(data.email)) {
        showFieldError('email', 'Geçerli bir e-posta adresi giriniz.');
        isValid = false;
    }
    
    if (!data.username || data.username.length < 3) {
        showFieldError('username', 'Kullanıcı adı en az 3 karakter olmalıdır.');
        isValid = false;
    }
    
    if (!data.password || data.password.length < 8) {
        showFieldError('password', 'Şifre en az 8 karakter olmalıdır.');
        isValid = false;
    }
    
    if (data.password !== data.confirmPassword) {
        showFieldError('confirmPassword', 'Şifreler eşleşmiyor.');
        isValid = false;
    }
    
    if (data.phone && !isValidPhone(data.phone)) {
        showFieldError('phone', 'Geçerli bir telefon numarası giriniz.');
        isValid = false;
    }
    
    if (!data.terms) {
        showFieldError('terms', 'Kullanım şartlarını kabul etmelisiniz.');
        isValid = false;
    }
    
    return isValid;
}

// Password strength checker
function checkPasswordStrength() {
    const password = document.getElementById('password').value;
    const strengthBar = document.getElementById('passwordStrength');
    const strengthText = document.getElementById('passwordStrengthText');
    
    if (!password) {
        strengthBar.style.width = '0%';
        strengthBar.className = 'progress-bar';
        strengthText.textContent = 'Şifre gücü';
        return;
    }
    
    let strength = 0;
    let strengthLabel = '';
    let strengthClass = '';
    
    // Length check
    if (password.length >= 8) strength += 20;
    if (password.length >= 12) strength += 10;
    
    // Character variety checks
    if (/[a-z]/.test(password)) strength += 10;
    if (/[A-Z]/.test(password)) strength += 10;
    if (/[0-9]/.test(password)) strength += 10;
    if (/[^A-Za-z0-9]/.test(password)) strength += 10;
    
    // Common patterns
    if (!/(.)\1{2,}/.test(password)) strength += 10; // No repeated characters
    if (!/123|abc|qwe/i.test(password)) strength += 10; // No common sequences
    
    if (strength < 30) {
        strengthLabel = 'Zayıf';
        strengthClass = 'bg-danger';
    } else if (strength < 60) {
        strengthLabel = 'Orta';
        strengthClass = 'bg-warning';
    } else if (strength < 80) {
        strengthLabel = 'İyi';
        strengthClass = 'bg-info';
    } else {
        strengthLabel = 'Güçlü';
        strengthClass = 'bg-success';
    }
    
    strengthBar.style.width = strength + '%';
    strengthBar.className = `progress-bar ${strengthClass}`;
    strengthText.textContent = `Şifre gücü: ${strengthLabel}`;
}

// Password visibility toggle
function togglePasswordVisibility(inputId, buttonId) {
    const input = document.getElementById(inputId);
    const button = document.getElementById(buttonId);
    const icon = button.querySelector('i');
    
    if (input.type === 'password') {
        input.type = 'text';
        icon.className = 'fas fa-eye-slash';
    } else {
        input.type = 'password';
        icon.className = 'fas fa-eye';
    }
}

// Social login handlers
function handleGoogleLogin() {
    // Simulate Google OAuth
    console.log('Google login initiated');
    showInfo('Google ile giriş özelliği yakında eklenecek.');
}

function handleFacebookLogin() {
    // Simulate Facebook OAuth
    console.log('Facebook login initiated');
    showInfo('Facebook ile giriş özelliği yakında eklenecek.');
}

function handleGoogleRegister() {
    // Simulate Google OAuth
    console.log('Google register initiated');
    showInfo('Google ile kayıt özelliği yakında eklenecek.');
}

function handleFacebookRegister() {
    // Simulate Facebook OAuth
    console.log('Facebook register initiated');
    showInfo('Facebook ile kayıt özelliği yakında eklenecek.');
}

// API simulation functions
async function loginUser(loginData) {
    return new Promise((resolve) => {
        setTimeout(() => {
            // Simulate successful login
            if (loginData.email === 'admin@test.com' && loginData.password === '123456') {
                resolve({
                    success: true,
                    user: {
                        id: 1,
                        firstName: 'Admin',
                        lastName: 'User',
                        email: loginData.email,
                        username: 'admin',
                        phone: '+90 555 123 45 67',
                        birthDate: '1990-01-01',
                        favoriteTeam: 'Manchester City',
                        memberSince: '2024-01-01',
                        level: 5,
                        points: 1250,
                        totalPredictions: 45,
                        correctPredictions: 35,
                        accuracyRate: 77.8,
                        rank: 15
                    },
                    token: 'mock-jwt-token-' + Date.now()
                });
            } else {
                resolve({
                    success: false,
                    message: 'E-posta veya şifre hatalı.'
                });
            }
        }, 1000);
    });
}

async function registerUser(registerData) {
    return new Promise((resolve) => {
        setTimeout(() => {
            // Simulate successful registration
            resolve({
                success: true,
                message: 'Kayıt başarılı!'
            });
        }, 1500);
    });
}

// UI update functions
function updateUIForLoggedInUser() {
    // Update navigation
    const navItems = document.querySelectorAll('.navbar-nav .nav-item');
    navItems.forEach(item => {
        const link = item.querySelector('.nav-link');
        if (link && link.textContent.includes('Giriş')) {
            item.style.display = 'none';
        }
    });
    
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
    // Show login/register links
    const navItems = document.querySelectorAll('.navbar-nav .nav-item');
    navItems.forEach(item => {
        const link = item.querySelector('.nav-link');
        if (link && link.textContent.includes('Giriş')) {
            item.style.display = 'block';
        }
    });
    
    // Remove user dropdown
    const userDropdown = document.querySelector('.user-dropdown');
    if (userDropdown) {
        userDropdown.remove();
    }
}

function createUserDropdown() {
    const dropdown = document.createElement('li');
    dropdown.className = 'nav-item dropdown user-dropdown';
    dropdown.innerHTML = `
        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown">
            <i class="fas fa-user me-1"></i>${currentUser ? currentUser.firstName : 'Kullanıcı'}
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

// Logout function
function logout() {
    currentUser = null;
    authToken = null;
    
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

// Utility functions
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function isValidPhone(phone) {
    const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/;
    return phoneRegex.test(phone);
}

function showFieldError(fieldId, message) {
    const field = document.getElementById(fieldId);
    const errorDiv = document.getElementById(fieldId + 'Error');
    
    if (field) {
        field.classList.add('is-invalid');
    }
    
    if (errorDiv) {
        errorDiv.textContent = message;
    }
}

function clearFormErrors(formId) {
    const form = document.getElementById(formId);
    if (form) {
        const invalidFields = form.querySelectorAll('.is-invalid');
        invalidFields.forEach(field => {
            field.classList.remove('is-invalid');
        });
        
        const errorMessages = form.querySelectorAll('.invalid-feedback');
        errorMessages.forEach(error => {
            error.textContent = '';
        });
    }
}

function showError(formId, message) {
    const form = document.getElementById(formId);
    if (form) {
        const errorDiv = form.querySelector('.error-message') || createErrorDiv(form);
        errorDiv.textContent = message;
        errorDiv.style.display = 'block';
    }
}

function createErrorDiv(form) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'alert alert-danger error-message mt-3';
    errorDiv.style.display = 'none';
    form.appendChild(errorDiv);
    return errorDiv;
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

function showInfo(message) {
    // Create info notification
    const notification = document.createElement('div');
    notification.className = 'alert alert-info position-fixed';
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        <i class="fas fa-info-circle me-2"></i>${message}
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

function showLoading(buttonId, text) {
    const button = document.getElementById(buttonId);
    if (button) {
        button.disabled = true;
        button.innerHTML = `<i class="fas fa-spinner fa-spin me-2"></i>${text}`;
    }
}

function hideLoading(buttonId, originalText) {
    const button = document.getElementById(buttonId);
    if (button) {
        button.disabled = false;
        button.innerHTML = originalText;
    }
}

// Export functions for global access
window.logout = logout;
window.currentUser = () => currentUser;
window.authToken = () => authToken;
window.isLoggedIn = () => currentUser !== null;


