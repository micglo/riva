window.addEventListener("load", function () {
    var a = document.querySelector("#PostLogoutRedirectUri");
    if (a) {
        window.location = a.href;
    }
});