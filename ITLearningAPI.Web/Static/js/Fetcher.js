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

export function FetchHttpPostJson(path, requestBody) {
    const fetchPromise = fetch(path,
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

export function FetchHttpPostFormData(path, formData) {
    return fetch(path, {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        redirect: "follow",
        body: formData
    });
}