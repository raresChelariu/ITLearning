
const htmlEntities = {
    rightChoices: {
        htmlEncoding: "&#9989;",
        charCode: 9989
    },
    wrongChoices: {
        htmlEncoding: "&#10060;",
        charCode: 10060
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

function removeHtmlEntitiesFromString(text) {
    return text
        .replaceAll(String.fromCharCode(10060), "")
        .replaceAll(String.fromCharCode(9989), "");
}