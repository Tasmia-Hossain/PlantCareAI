// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ---------------------------------------------------------------------
// Shared "confirm delete" modal
// Replaces the browser's native confirm() dialog with a themed Bootstrap
// modal. Any button with [data-confirm-delete] triggers it; the button
// must carry data-confirm-target (the id of the <form> to submit) and
// optionally data-confirm-message (custom warning text).
// ---------------------------------------------------------------------
document.addEventListener('DOMContentLoaded', function () {
    var modalEl = document.getElementById('confirmDeleteModal');
    if (!modalEl || typeof bootstrap === 'undefined') {
        return;
    }

    var modal = new bootstrap.Modal(modalEl);
    var messageEl = document.getElementById('confirmDeleteMessage');
    var confirmBtn = document.getElementById('confirmDeleteButton');
    var targetForm = null;

    document.querySelectorAll('[data-confirm-delete]').forEach(function (trigger) {
        trigger.addEventListener('click', function (e) {
            e.preventDefault();

            var formId = trigger.getAttribute('data-confirm-target');
            targetForm = formId ? document.getElementById(formId) : trigger.closest('form');

            var message = trigger.getAttribute('data-confirm-message');
            messageEl.textContent = message || "This action can't be undone.";

            modal.show();
        });
    });

    confirmBtn.addEventListener('click', function () {
        if (targetForm) {
            confirmBtn.disabled = true;
            confirmBtn.textContent = 'Deleting...';
            targetForm.submit();
        }
    });

    // Reset button state each time the modal is closed (covers Cancel/Esc/backdrop)
    modalEl.addEventListener('hidden.bs.modal', function () {
        confirmBtn.disabled = false;
        confirmBtn.textContent = 'Yes, Delete';
        targetForm = null;
    });
});
