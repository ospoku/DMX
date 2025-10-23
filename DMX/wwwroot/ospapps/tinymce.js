// Initialize TinyMCE for all textareas with the given class
function initializeAllEditors(selector = 'textarea.dmx-textarea') {
    // Remove any existing editors to prevent duplicates
    if (typeof tinymce !== 'undefined') {
        tinymce.remove();
    }

    // Select all target textareas
    const editors = document.querySelectorAll(selector);

    editors.forEach(textarea => {
        tinymce.init({
            target: textarea,
            height: 300,
            menubar: false,
            plugins: 'lists link image code',
            toolbar: 'undo redo | bold italic underline | bullist numlist | link image | code',
            branding: false,
            setup: function (editor) {
                const dialog = textarea.closest('dialog');
                if (dialog) {
                    dialog.addEventListener('close', () => {
                        editor.remove(); // Cleanup when dialog closes
                    });
                }
            }
        });
    });
}

// Run after DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    initializeAllEditors(); // call once on page load
});
