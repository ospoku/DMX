document.addEventListener('DOMContentLoaded', () => {
    const editors = document.querySelectorAll('textarea.dmx-textarea');

    editors.forEach(textarea => {
        tinymce.init({
            target: textarea,
            height: 300,
            menubar: false,
            plugins: 'lists link image code',
            toolbar: 'undo redo | bold italic underline | bullist numlist | link image | code',
            branding: false,
            setup: function (editor) {
                // Optional: handle if the dialog closes
                const dialog = textarea.closest('dialog');
                if (dialog) {
                    dialog.addEventListener('close', () => {
                        editor.remove(); // clean up when dialog closes
                    });
                }
            }
        });
    });
});
