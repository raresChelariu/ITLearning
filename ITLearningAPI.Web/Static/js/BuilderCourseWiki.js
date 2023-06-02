export function BuildCourseWiki(data)
{
    const title = buildTitle(data);
    const text = buildText(data);
    return [title, text]
}

function buildTitle(data) {
    const titleDiv = document.createElement("div");
    titleDiv.classList.add("title");
    titleDiv.innerText = data["title"];
    return titleDiv;
}

function buildText(data) {
    const textDiv = document.createElement("div");
    const text = data["courseWikiText"]
    textDiv.innerHTML = marked.parse(text);
    return textDiv;
}