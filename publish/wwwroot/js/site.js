console.log("Site.js laddad - globala funktioner kan läggas här");

// T.ex. global logout funktion
async function logout() {
    await fetch('/Authorization/Logout', { method: 'POST', credentials: 'include' });
    window.location.href = '/Authorization/Login';
}