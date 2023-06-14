import {FetchHttpPostJson} from "/js/Fetcher.js";

const pageIds = {
    ButtonRegister: "buttonRegister",
    InputPassword: "inputPassword",
    InputConfirmPassword: "inputPasswordConfirm",
    InputEmail: "inputEmail",
    InputUsername: "inputUsername",
    SelectUserRole: "selectUserRole"
};

const buttonRegister = document.getElementById(pageIds.ButtonRegister);
buttonRegister.addEventListener("click", RegisterUser);

function RegisterUser() {
    const uiValidationRules = [PasswordMatch, AllTextFieldsNonEmpty, UserRoleIsNotEmptyValue];
    for (let i = 0; i < uiValidationRules.length; i++) {
        let rule = uiValidationRules[i];
        if (!rule()) {
            return;
        }
    }
    
    const email = document.getElementById(pageIds.InputEmail).value;
    const username = document.getElementById(pageIds.InputUsername).value;
    const password = document.getElementById(pageIds.InputPassword).value;
    const userRoleId = document.getElementById(pageIds.SelectUserRole).value;

    const requestBody = {
        username: username,
        email: email,
        password: password,
        role: userRoleId
    };
    console.log(requestBody);
    return;
    FetchHttpPostJson("/api/user/register", requestBody)
        .then(() => {
            alert("Te-ai inregistrat cu succes!");  
        })
        .catch(() => {
            alert("Inregistrarea a esuat! Mai incearca!");
        })
}

function AllTextFieldsNonEmpty() {
    const email = document.getElementById(pageIds.InputEmail).value;
    const username = document.getElementById(pageIds.InputUsername).value;
    const password = document.getElementById(pageIds.InputPassword).value;
    
    const values = [email, username, password];
    for (let i = 0; i < values.length; i++) {
        const value = values[i];
        if (value === null || value === undefined || 
            value === "" || value.trim().length === 0) {
            alert("Completeaza toate campurile!");
            return false;
        }
    }
    return true;
}

function PasswordMatch() {
    const passwordInput = document.getElementById(pageIds.InputPassword);
    const passwordConfirmInput = document.getElementById(pageIds.InputConfirmPassword);

    const isPasswordMatch = passwordInput.value === passwordConfirmInput.value;

    if (!isPasswordMatch) {
        alert("Parola trebuie sa coincida!");
        return false;
    }
    return true;
}

function UserRoleIsNotEmptyValue() {
    const userRoleId = document.getElementById(pageIds.SelectUserRole).value;
    if (userRoleId === 0) {
        alert("Alege un rol de utilizator!");
        return false;
    }
    return true;
}