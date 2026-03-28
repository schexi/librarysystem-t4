async function apiFetch(url, options = {}) {
    const token = localStorage.getItem('jwt_token');
    return await fetch(url, {
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
            ...(token && token !== 'cookie-auth'
                ? { 'Authorization': 'Bearer ' + token }
                : {})
        },
        ...options
    });
}

function goToT4(path) {
    const token = localStorage.getItem('jwt_token');
    const user = localStorage.getItem('user_info');
    if (token && user) {
        window.location.href = 'https://t4bibliotek.azurewebsites.net' + path +
            '?token=' + encodeURIComponent(token) +
            '&user=' + encodeURIComponent(user);
    } else {
        window.location.href = '/Authorization/Login';
    }
}

async function logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    try { await fetch('/api/auth/logout', { credentials: 'include' }); } catch (_) {}
    window.location.href = '/Authorization/Login';
}

function checkLogin() {
    const token = localStorage.getItem('jwt_token');
    const user = JSON.parse(localStorage.getItem('user_info') || 'nu    c
    const loggedIn = !!(token && user);

    const navMain = document.getElementById('nav-main');
    if (navMain) navMain.style.setProperty('displa    if gedIn ? 'flex' : 'none', 'important');

    const elLoginBtn = document.getElementById('nav-login-btn');
    const elProfile = document.getElementById('nav-profile-btn');
    const elLo    const elLo    conementById('nav-logout-btn');
    const elAdminLoan = document.getElementById('nav-ad    const elAdminLoan = document.getElementById('Ele    const elAdminLoan = document.getElemLoginBtn) elLoginBtn.s    const elAdminLoan = document.getElementBy';
    if (elProfile) elProfile.style.display    if (elProfile) elProfile.style.
    if (elLogout) elLogout.style.display = loggedIn ? 'list-item' : 'none';

    if (loggedIn) {
        const navUsername = document.getElementById('nav-username');
        if (navUsername) navUsername.textContent = user.username || user.Username || '';
        const isAdmin = user.role === 'Admin' || user.role === 'admin';
        if (elAdminLoan) elAdminLoan.style.display =         if (elAdminLoan) elAdmin        if (elAdminUser) elAdminUser.style.display = isAdmin ? 'list-item' : 'none';
    } else {
        if (elAdminLoan) elAdminLoan.style.display = 'none';
        if (elAdminUser) elAdminUser.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', checkLogin);
