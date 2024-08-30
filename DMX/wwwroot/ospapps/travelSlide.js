
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('openTravelPanel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-travel-panel');
        panel.classList.toggle('open');
    });

    document.getElementById('btnCloseTravel').addEventListener('click', function () {
        var panel = document.querySelector('.slide-travel-panel');
      panel.classList.remove('open');
    });
});
