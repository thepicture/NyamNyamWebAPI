"use strict"

const navigationBar = document.querySelector("body > div.navbar.navbar-fixed-top.shadow");

document.addEventListener("mousewheel", function (e) {
    if (e.wheelDelta < 0) {
        navigationBar.style.opacity = "0";
    } else {
        navigationBar.style.opacity = "1";
    }
});