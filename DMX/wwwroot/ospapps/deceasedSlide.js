
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('openDeceasedPanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-deceased-panel');
        panel.classList.toggle('open');
    });

    document.getElementById('btnCloseDeceased').addEventListener('click', function () {
        var panel = document.querySelector('.slide-deceased-panel');
      panel.classList.remove('open');
    });
});
