document.getElementById('sidebarToggle').addEventListener('click', function () {
    var sidebar = document.getElementById('sidebar');
    if (sidebar.classList.contains('d-none')) {
        sidebar.classList.remove('d-none');
        sidebar.classList.add('d-flex');
    } else {
        sidebar.classList.remove('d-flex');
        sidebar.classList.add('d-none');
    }
});
