export function CreateAlertError(text) {
    const closeButton = document.createElement("span");
    closeButton.classList.add("closebtn");
    closeButton.innerHTML = "&times;";
    closeButton.addEventListener("click", CloseButtonOnClick);
    
    const errorText = document.createTextNode(text);

    const div = document.createElement("div");
    div.classList.add("alert");
    div.appendChild(closeButton);
    div.appendChild(errorText);
    return div;
}

export function CreateAlertSuccess(text) {
    const alert = CreateAlertError(text);
    alert.classList.add("success");
    return alert;
}

function CloseButtonOnClick (event) {
    const div = event.target.parentElement;
    div.style.opacity = "0";
    const fadeAwayTimeout = 600;
    setTimeout(function () {
        div.style.display = "none";
    }, fadeAwayTimeout);
}