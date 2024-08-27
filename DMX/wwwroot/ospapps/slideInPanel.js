$(document).ready(function () {
    $('#openPanelBtn').click(function () {
        $('#slideInPanel').addClass('open');
    });

    $('#closePanelBtn').click(function () {
        $('#slideInPanel').removeClass('open');
    });
});
document.addEventListener('DOMContentLoaded', () => {
    const slideInPanel = document.getElementById('slideInPanel');
    const closeButton = document.getElementById('closeButton');

    function togglePanel() {
        slideInPanel.classList.toggle('active');
    }

    // Show panel (example: you may trigger this from another button or event)
    document.getElementById('showPanelButton').addEventListener('click', togglePanel);

    // Close panel
    closeButton.addEventListener('click', togglePanel);
});
