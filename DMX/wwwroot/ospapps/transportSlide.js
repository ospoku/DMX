
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('openTransportPanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-transport-panel');
        panel.classList.toggle('open');
    });

    document.getElementById('closeTransportPanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-transport-panel');
      panel.classList.remove('open');
    });
});
