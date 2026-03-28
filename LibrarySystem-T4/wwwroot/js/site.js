
const urlParams = new URLSearchParams(window.location.search);
const tokenFromUrl = urlParams.get('token');
const userFromUrl = urlParams.get('user');
if (tokenFromUrl && userFromUrl) {
    localStorage.setItem('jwt_token', decodeURIComponent(tokenFromUrl));
    localStorage.setItem('user_info', decodeURIComponent(userFromUrl));
    window.history.replaceState({}, '', window.location.pathname);
}
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

async function logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    try { await fetch('https://user-api-adde.azurewebsites.net/api/auth/logout', { credentials: 'include' }); } catch (_) {}
    window.location.href = 'https://user-api-adde.azurewebsites.net/Authorization/Login';
}

function checkLogin() {
    const token    = localStorage.getItem('jwt_token');
    const user     = JSON.parse(localStorage.getItem('user_info') || 'null');
    const loggedIn = !!(token && user);

    const navMain = document.getElementById('nav-main');
    if (navMain) navMain.style.setProperty('display', loggedIn ? 'flex' : 'none', 'import    if (navMain) navMain.style.setProperty('displntById('nav-login-btn');
    const elProfile   = document.getEleme    const elProfile    const elProfile   = document.geument.getElementById('nav-logout-btn');
    const elAdminLoan = document.getElementById('nav-admin-loans');
    const elAdminUser = document.    cementById('nav-admin-users');
    const elAdminCreate = document.getElementById('nav-admin-create');

    if (elLoginBtn)  elLoginBtn.style.display  = loggedIn ? 'none'      : 'list-item';
    if (elProfile)   elProfile.style.display   = loggedIn ? 'list-item' : 'none';
    if (elLogout)    elLogout.style.display     = loggedIn ? 'list-item' : 'none';

    if (loggedIn) {
        const navUsername = document.getElementById('nav-username');
        if (navUsername) navUsername.textContent = user.username || user.Username || '';
        const isAdmin = user.r        const isAdmin = user.r        const isA   if (elAdminLoan)   elAdminLoan.style.display   = isAdmin ? 'list-item' : 'none';
        if (elAdminUser)   elAdminUser.style.display   = isAdmin ? 'list-item' : 'none';
        if (elAdminCreate) elAdminCreate.style.display = isAdmin ? 'list-item' : 'none';
    } else {
        if (elAdminLoan)   elAdminLoan.style.display   = 'none';
        if (elAdminUser)   elAdminUser.style.display   = 'none';
        if (elAdminCreate) elAdminCreate.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', checkLogin);
