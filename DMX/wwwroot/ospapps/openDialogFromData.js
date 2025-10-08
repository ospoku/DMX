//function openDialogFromData(button) {
//    const dialogId = button.getAttribute('data-dialog-id');
//    const userId = button.getAttribute('data-user-id');
//    const url = button.getAttribute('data-url');

//    const dialog = document.getElementById(dialogId);
//    const contentDiv = document.getElementById(dialogId + "Content");

//    if (!dialog || !contentDiv) {
//        console.error("Dialog or content div not found for ID:", dialogId);
//        return;
//    }

//    if (!url || !userId) {
//        console.error("Missing URL or userId");
//        return;
//    }

//    contentDiv.innerHTML = "<p>Loading...</p>";

//    fetch(`${url}?Id=${encodeURIComponent(userId)}`)
//        .then(res => res.text())
//        .then(html => {
//            contentDiv.innerHTML = html;
//            dialog.showModal();
//        })
//        .catch(err => {
//            contentDiv.innerHTML = "<p class='text-danger'>Failed to load content.</p>";
//            console.error(err);
//        });
//}
function openDialogFromData(button) {
    // Get required attributes
    const dialogId = button.getAttribute('data-dialog-id');
    const userId = button.getAttribute('data-user-id');
    const url = button.getAttribute('data-url');

    // Validate required elements
    const dialog = document.getElementById(dialogId);
    const contentDiv = document.getElementById(dialogId + "Content");
    if (!dialog || !contentDiv) {
        console.error("Dialog elements not found");
        return;
    }

    // Validate parameters
    if (!url || !userId) {
        console.error("Missing required parameters");
        contentDiv.innerHTML = `
            <div class="alert alert-warning">
                Configuration error. Please contact support.
            </div>
        `;
        return;
    }

    // Show loading state
    contentDiv.innerHTML = `
        <div class="text-center py-4">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2">Loading form data...</p>
        </div>
    `;

    // Set up abort controller
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 15000); // 15 second timeout

    // Make the request
    fetch(`${url}?Id=${encodeURIComponent(userId)}`, {
        signal: controller.signal
    })
        .then(response => {
            clearTimeout(timeoutId);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.text();
        })
        .then(html => {
            contentDiv.innerHTML = html;
            dialog.showModal();

            // Initialize any scripts in the loaded content
            const scripts = contentDiv.querySelectorAll('script');
            scripts.forEach(script => {
                const newScript = document.createElement('script');
                if (script.src) {
                    newScript.src = script.src;
                } else {
                    newScript.textContent = script.textContent;
                }
                document.body.appendChild(newScript).parentNode.removeChild(newScript);
            });
        })
        .catch(err => {
            clearTimeout(timeoutId);
            console.error('Error loading dialog content:', err);
            contentDiv.innerHTML = `
            <div class="alert alert-danger">
                <h5>Error Loading Content</h5>
                <p>${err.message}</p>
                <button onclick="document.getElementById('${dialogId}').close()" 
                        class="btn btn-sm btn-outline-danger">
                    Close
                </button>
            </div>
        `;
        });

    // Clean up on dialog close
    dialog.addEventListener('close', () => {
        clearTimeout(timeoutId);
        controller.abort();
    });
}