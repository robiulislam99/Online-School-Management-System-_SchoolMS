// ===== SIDEBAR TOGGLE =====
const toggleBtn = document.getElementById('sidebarToggle');
const sidebar = document.getElementById('sidebar');

if (toggleBtn && sidebar) {
    toggleBtn.addEventListener('click', () => {
        if (window.innerWidth <= 768) {
            sidebar.classList.toggle('mobile-open');
        } else {
            sidebar.classList.toggle('collapsed');
        }
    });
}

// ===== CLOSE SIDEBAR ON MOBILE WHEN CLICKING OUTSIDE =====
document.addEventListener('click', function (e) {
    if (window.innerWidth <= 768 && sidebar) {
        if (!sidebar.contains(e.target) && !toggleBtn.contains(e.target)) {
            sidebar.classList.remove('mobile-open');
        }
    }
});

// ===== AUTO HIDE ALERTS AFTER 4 SECONDS =====
setTimeout(() => {
    document.querySelectorAll('.alert-dismissible').forEach(alert => {
        const bsAlert = new bootstrap.Alert(alert);
        bsAlert.close();
    });
}, 4000);