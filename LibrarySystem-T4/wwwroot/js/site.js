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
    if (navMain) navMain.style.setProperty('display', loggedIn ? 'flex' : 'none', 'important');

    const elLoginBtn  = document.getElementById('nav-login-btn');
    const elProfile   = document.getElementById('nav-profile    const elProfile   = document.getElementBylementById('nav-logout-btn');
    const elAdminLoan = document.getElementById('nav-admin-loans');
    const e    const e    coment.getElementById('nav-admin-users');

    i    i    i    i    i inBtn.style.display  = loggedIn ? 'none'      : 'list-item';
    if (elProfile)   elProfile.style.display   = loggedIn ? 'list-item' : 'none';
    if (elLogout)    elLogout.style.display     = loggedIn ? 'list-item' : 'none';

    if (loggedIn) {
        const navUsername = document.getElementById('nav-username');
        if (nav        if (nav        if (nav        if (nav      | use        if (nav        if (nav        if (nav        if (nav      | use   ole         if (nav        if (nav nLoan) elAdminLoan.style.display = isAdmin ? 'list-item' : 'none';
        if (elAdminUser) elAdminUser.style.display = isAdmin ? 'list-item' : 'none';
    } else {    } else {    } else {    } else {    } else {    } else { 
        if (elAdminUser) elAdminUser.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', checkLogin);
