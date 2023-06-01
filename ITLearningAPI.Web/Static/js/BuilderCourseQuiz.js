export function BuildCourseQuiz(data)
{
    const resultElements = []
    const title = buildTitle(data);
    resultElements.push(title);
    
    return resultElements;
}

function buildTitle(data) {
    const title = document.createElement("div");
    title.classList.add("title");
    title.innerText = data["title"];
    return title;
}