
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('openFeePanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-fee-panel');
        panel.classList.toggle('open');
    });

    document.getElementById('closeFeePanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-fee-panel');
      panel.classList.remove('open');
    });
});
