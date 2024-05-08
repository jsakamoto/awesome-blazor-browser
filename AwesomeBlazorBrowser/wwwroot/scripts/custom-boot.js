"use strict";

(() => {

    // Create an observer object to observe the CSS class list changes of the Reconnect modal UI.
    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            const classList = mutation.target.classList;

            // If the reconnect modal UI is given the "rejected" CSS class, 
            // it means that the connection to the server is restored, 
            // but the session is lost and can no longer be recovered.
            // In this case, there is no other way but to reload the current page.
            if (classList.contains('components-reconnect-rejected') === true) {
                window.location.reload();
            }
        });
    });

    // Start to observe.
    const reconnectModal = document.getElementById('components-reconnect-modal');
    if (reconnectModal !== null) {
        observer.observe(reconnectModal, { attributes: true, subtree: false });
    }
})();

Blazor.start({
    // Keep retrying each 2 seconds for a year (!).
    // (This is, in effect, an infinite number of retries.)
    reconnectionOptions: { maxRetries: 15768000, retryIntervalMilliseconds: 2000 },
    ssr: { disableDomPreservation: true }
});