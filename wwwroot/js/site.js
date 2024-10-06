// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {

    //=====================================
    // Theme Onload Toast
    //=====================================
    window.addEventListener("load", () => {
        let myAlert = document.querySelectorAll('.toast')[0];
        if (myAlert) {
            let bsAlert = new bootstrap.Toast(myAlert);
            bsAlert.show();
        }
    })
});