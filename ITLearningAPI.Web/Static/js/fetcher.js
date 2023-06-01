export function FetchHttpGet(path) {
    const fetchPromise = fetch(path, {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json"
        },
        redirect: "follow",
        referrerPolicy: "no-referrer"
    });

    return fetchPromise.then((apiResponse) => {
        if (!apiResponse.ok) {
            return null;
        }
        return apiResponse.json();
    });
}

export function FetchHttpPost(path, requestBody) {
    const fetchPromise = fetch("api/user/login",
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
            body: JSON.stringify(requestBody)
        });
    return fetchPromise.then((apiResponse) => {
        if (!apiResponse.ok) {
            return null;
        }
        return apiResponse.json();
    });
}