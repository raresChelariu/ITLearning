import {FetchHttpPostJson} from "/js/Fetcher.js";

const buttonLogin = document.getElementById("buttonLogin");

buttonLogin.addEventListener("click", buttonLoginClick);

function buttonLoginClick() {
    if (!IsValidUi()) {
        return;
    }
    const inputEmail = document.getElementById("inputEmail");
    const inputPassword = document.getElementById("inputPassword");

    const loginRequestBody = {
        userIdentifier: inputEmail.value,
        password: inputPassword.value
    };

    FetchHttpPostJson("api/user/login", loginRequestBody)
        .then((responseResult) => {
            const responseBody = responseResult;
            const authToken = responseBody["token"];
            localStorage.setItem("userAuthToken", authToken + "");
            const role = responseBody["role"];
            if (role === "teacher") {
                window.location.replace("/teacher");
            } else {
                window.location.replace("/student");
            }
        })
        .catch((err) => {
            console.log(err);
            HandleFailure();
        });
}

function HandleFailure() {
    const inputEmail = document.getElementById("inputEmail");
    const inputPassword = document.getElementById("inputPassword");
    inputEmail.value = "";
    inputPassword.value = "";
    alert("Invalid credentials");
}

function IsValidUi() {
    const inputEmail = document.getElementById("inputEmail");
    const inputPassword = document.getElementById("inputPassword");

    const values = [inputEmail.value, inputPassword.value];

    for (const element of values) {
        let value = element;
        if (value === "" || value.trim().length === 0) {
            alert("Completeaza toate campurile!");
            return false;
        }
    }
    return true;
}