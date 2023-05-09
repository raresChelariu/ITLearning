const buttonLogin = document.getElementById("buttonLogin");

buttonLogin.addEventListener("click", buttonLoginClick);

function buttonLoginClick() {
    const inputEmail = document.getElementById("inputEmail");
    const inputPassword = document.getElementById("inputPassword");

    const loginRequestBody = {
        userIdentifier: inputEmail.value,
        password: inputPassword.value
    };

    const handleFailure = () => {
        inputEmail.value = "";
        inputPassword.value = "";
        alert("Invalid credentials");
    };
    const fetchCallback = (apiResponse) => {
        if (!apiResponse.ok) {
            handleFailure();
            return;
        }
        apiResponse.json()
            .then((responseResult) => {
                const responseBody = responseResult;
                const authToken = responseBody["token"];
                localStorage.setItem("userAuthToken", authToken);
                const role = responseBody["role"];
                if (role === "teacher") {
                    window.location.replace("/teacher");
                } else {
                    window.location.replace("/student");
                }
            })
            .catch(err => {
                console.log(err);
                handleFailure();
            });

    };
    fetch("api/user/login",
        {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
            },
            redirect: "follow",
            referrerPolicy: "no-referrer",
            body: JSON.stringify(loginRequestBody)
        }).then(fetchCallback).catch((err) => {
            console.log(err);
            handleFailure();
        });
}