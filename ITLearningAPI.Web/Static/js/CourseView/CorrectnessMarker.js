
const htmlEntities = {
    rightChoices: {
        htmlEncoding: "&#9989;",
        charCode: 9989
    },
    wrongChoices: {
        htmlEncoding: "&#10060;",
        charCode: 10060
    },
    loading: {
        htmlEncoding: "&#128260;",
        charCode: 128260
    }
}

export function AddWrongAnswerMarker(text) {
    text = removeHtmlEntitiesFromString(text);
    return `${htmlEntities.wrongChoices.htmlEncoding} ${text}`;
}

export function AddCorrectAnswerMarker(text) {
    text = removeHtmlEntitiesFromString(text);
    return `${htmlEntities.rightChoices.htmlEncoding} ${text}`;
}

export function AddLoadingMarker(text) {
    text = removeHtmlEntitiesFromString(text);
    return `${htmlEntities.loading.htmlEncoding} ${text}`;
}
function removeHtmlEntitiesFromString(text) {
    return text
        .replaceAll(String.fromCharCode(htmlEntities.wrongChoices.charCode), "")
        .replaceAll(String.fromCharCode(htmlEntities.rightChoices.charCode), "")
        .replaceAll(String.fromCharCode(htmlEntities.loading.charCode), "");
}