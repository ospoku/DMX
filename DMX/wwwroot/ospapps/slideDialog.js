document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-dialog-id]').forEach(button => {
        button.addEventListener('click', async function () {
            const dialogId = this.getAttribute('data-dialog-id');
            const dialog = document.getElementById(dialogId);
            const contentDiv = dialog.querySelector('div[id$="Content"]');
            const url = this.getAttribute('data-url');

            if (!dialog) {
                console.error(`Dialog with id "${dialogId}" not found.`);
                return;
            }

            // Fetch remote content if a URL is provided
            if (url && contentDiv) {
                try {
                    const response = await fetch(url);
                    const html = await response.text();
                    contentDiv.innerHTML = html;
                } catch (error) {
                    contentDiv.innerHTML = `<p style="color:red;">Failed to load content.</p>`;
                    console.error(error);
                }
            }

            // Show modal and trigger slide animation
            dialog.showModal();

            // Small delay to allow dialog to render before sliding in
            setTimeout(() => {
                dialog.classList.add('open');
            }, 10);
        });
    });

    // Close dialog when clicking the [data-close-dialog] button
    document.querySelectorAll('[data-close-dialog]').forEach(btn => {
        btn.addEventListener('click', function () {
            const dialog = this.closest('dialog');
            dialog.classList.remove('open');
            setTimeout(() => dialog.close(), 300); // matches transition duration
        });
    });

    // Optional: close dialog by clicking outside
    document.querySelectorAll('dialog.slide-in').forEach(dialog => {
        dialog.addEventListener('click', function (e) {
            const rect = dialog.getBoundingClientRect();
            const isInDialog =
                rect.top <= e.clientY && e.clientY <= rect.bottom &&
                rect.left <= e.clientX && e.clientX <= rect.right;

            if (!isInDialog) {
                dialog.classList.remove('open');
                setTimeout(() => dialog.close(), 300);
            }
        });
    });
});
