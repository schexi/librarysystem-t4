async function apiFetch(url, options = {}) {
    const token = localStorage.getItem('jwt_token');
    return fetch(url, {
        ...options,
        headers: {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': 'Bearer ' + token } : {}),
            ...(options.headers || {})
        }
    });
}

function logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_info');
    window.location.href = '/Authorization/Login';
}

function checkLogin() {
    const token = localStorage.getItem('jwt_token');
    const user  = JSON.parse(localStorage.getItem('user_info') || 'null');

    const loginBtn        = document.getElementById('nav-login-btn');
    const profileBtn      = document.getElementById('nav-profile-btn');
    const logoutBtn       = document.getElementById('nav-logout-btn');
    const navBooks        = document.getElementById('nav-books');
    const navCats         = document.getElementById('nav-categories');
    const navLoans        = document.getElementById('nav-loans');
    const navAdmin        = document.getElementById('nav-admin-loans');
    const navUsers        = document.getElementById('nav-admin-users');
    const navAdminMain    = document.getElementById('nav-admin-main');
    const navNotifications = document.getElementById('nav-notifications');
    const usernameEl      = document.getElementById('nav-username');

    if (token && user) {
        if (loginBtn)          loginBtn.style.display          = 'none';
        if (profileBtn)        profileBtn.style.display         = 'list-item';
        if (logoutBtn)         logoutBtn.style.display          = 'list-item';
        if (navBooks)          navBooks.style.display           = 'list-item';
        if (navCats)           navCats.style.display            = 'list-item';
        if (navLoans)          navLoans.style.display           = 'list-item';
        if (navNotifications)  navNotifications.style.display   = 'list-item';
        if (usernameEl)        usernameEl.textContent           = user.username || '';

        if (user.role === 'Admin') {
            if (navAdmin)      navAdmin.style.display           = 'list-item';
            if (navUsers)      navUsers.style.display           = 'list-item';
            if (navAdminMain)  navAdminMain.style.display       = 'list-item';
        }
    } else {
        if (loginBtn)          loginBtn.style.display           = 'list-item';
        if (profileBtn)        profileBtn.style.display          = 'none';
        if (logoutBtn)         logoutBtn.style.display           = 'none';
        if (navBooks)          navBooks.style.display            = 'none';
        if (navCats)           navCats.style.display             = 'none';
        if (navLoans)          navLoans.style.display            = 'none';
        if (navAdmin)          navAdmin.style.display            = 'none';
        if (navUsers)          navUsers.style.display            = 'none';
        if (navAdminMain)      navAdminMain.style.display        = 'none';
        if (navNotifications)  navNotifications.style.display    = 'none';
    }
}

checkLogin();