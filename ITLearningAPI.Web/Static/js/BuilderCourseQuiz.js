export function BuildCourseQuiz(data)
{
    const title = buildTitle(data);
    
    return [title];
}

function buildTitle(data) {
    const title = document.createElement("div");
    title.classList.add("title");
    title.innerText = data["title"];
    return title;
}