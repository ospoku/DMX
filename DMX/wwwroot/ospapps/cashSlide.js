
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('openCashPanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-cash-panel');
        panel.classList.toggle('open');
    });

    document.getElementById('btnCloseCash').addEventListener('click', function () {
        var panel = document.querySelector('.slide-cash-panel');
      panel.classList.remove('open');
    });
});
