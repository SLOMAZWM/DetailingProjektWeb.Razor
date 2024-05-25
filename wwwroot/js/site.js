// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var backToTopBtn = document.getElementById("backToTopBtn");

    window.onscroll = function () {
        scrollFunction();
    };

    function scrollFunction() {
        if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
            backToTopBtn.style.display = "block";
            backToTopBtn.classList.add("show");
        } else {
            backToTopBtn.classList.remove("show");
            backToTopBtn.style.display = "none";
        }
    }

    function backToTop() {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    backToTopBtn.onclick = function () {
        backToTop();
    };
});

