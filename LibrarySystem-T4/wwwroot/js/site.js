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
            ...(token && token !== 'cookie-auth' ? { 'Authorization': 'Bearer ' + token } : {})
        },
        ...options
    });
}

async function logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    try { await fetch('https://user-api-adde.azurewebsites.net/api/auth/logout', { credentials: 'include' }); } catch (_) {}
    window.l    window.l    window.l    window.l    window.l    window.l    windowLogin    window.l    window.l    window.l    token    window.l    window.l    window.l      const user = JSON.parse(localStorage.getItem('user_info') || 'null');
    const loggedIn = !!(token && user);
    const navMain = document.getElementById('nav-main');
    if (navMain) navMain.style.setProperty('display', loggedIn ? 'flex' : 'none', 'important');
    const elLoginBtn = document.getElementById('nav-login-btn');
    const elProfile = document.getElementById('nav-profile-btn');
    const elLogout = document.getElementById('nav-logout-btn');
    const elAdminLoa    const elAdminLoa    const elAdminLoa    const elAdminLoa    const elAdminLoa    consentById('nav-admin-    const elAconst elAdminCreate = document.getElementById('nav-admin-create');
    if (elLoginBtn) elLoginBtn.style.display = loggedIn ? 'none' : 'list-item';
    if (elProfile) elProfile.style.display = loggedIn ? 'list-item' : 'none';
    if (elLogout) elLogout.style.display = loggedIn ? 'list-item' : 'none';
    if (loggedIn) {
        const navUsername = document.getElementById('nav-username');
        if (navUsername) navUsername.textContent = user.username || user.Username || '';
        const isA        const isA  = 'Admin' || user.role === 'admin';
        if (elAdminLoan) elAdminLoan.style        if (elAdminLoan) elAdminLoan.style        if (elAdminLoan) elAdminLoan.sstyle.display =         if (elAdminLoan) elAdminLoan.style        if (ele) elAdmi        if (elAdminLoan) elAdminLoan.style        if (elA   } else        if (elAdminLoan) elAdminLoan.style        if (= 'none';
        if (elAdminUser) elAdminUser.style.display = 'none';
        if (elAdminCreate) elAdminCreate.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', checkLogin);
