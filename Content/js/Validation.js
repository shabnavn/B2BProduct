function ValidatePass(input) {

    var regix = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*?])(?=.{8,})");

    if (regix.test(input)) {
        return true;

    } else {
        return false;
    }
}

function ValidateEmail(inputText) {

    var filter = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!filter.test(inputText)) {
        return false;
    } else {
        return true;
    }
}

function ValidateNumber(inputText) {

    var filter = /^9715[0245689]\d{7}$/;
    if (!filter.test(inputText)) {
        return false;
    } else {
        return true;
    }
}

function PasswordVisibility(input , icon) {
    var passwordInput = document.getElementById(input);
    var eyeIcon = document.getElementById(icon);
    if (passwordInput.type == "password") {
        passwordInput.type = "text";
        eyeIcon.classList.remove("fi-rs-eye-crossed");
        eyeIcon.classList.add("fi-rs-eye");
    } else {
        passwordInput.type = "password";
        eyeIcon.classList.remove("fi-rs-eye");
        eyeIcon.classList.add("fi-rs-eye-crossed");
    }
}