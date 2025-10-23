document.addEventListener('DOMContentLoaded', () => {
    const editors = document.querySelectorAll('textarea.dmx-textarea');

    editors.forEach(textarea => {
        const dialog = textarea.closest('Dialog');

        function initTinyMCE() {
            if (tinymce.get(textarea.id)) return; // prevent double init

            tinymce.init({
                target: textarea,
                height: 300,
                menubar: false,
                plugins: 'lists link image code',
                toolbar: 'undo redo | bold italic underline | bullist numlist | link image | code',
                branding: false,
                setup: function (editor) {
                    if (dialog) {
                        // Remove editor when dialog closes
                        dialog.addEventListener('close', () => {
                            if (tinymce.get(editor.id)) {
                                tinymce.get(editor.id).remove();
                            }
                        });
                    }
                }
            });
        }

        // Initialize when dialog is opened
        if (dialog) {
            dialog.addEventListener('open', initTinyMCE); // for programmatic open
            dialog.addEventListener('click', (e) => {
                if (e.target.tagName === 'BUTTON' && e.target.textContent.includes('Add Memo')) {
                    initTinyMCE();
                }
            });
        } else {
            initTinyMCE(); // fallback for non-dialog editors
        }
    });
});
