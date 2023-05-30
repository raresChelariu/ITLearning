import { fetchHttpGet } from '/js/fetcher.js'

const courseId = GetCourseId();

const titleList = document.getElementById("courseTitleList");

GetCourseTitles(courseId)
    .then(response => {
        console.log(response);
        for (let i = 0; i < response.length; i++) {
            let itemTitle = response[i];
            let element = GetTitleElement(itemTitle);
            titleList.appendChild(element);
        }
    });

function GetCourseId() {
    const url = window.location.href;
    const tokens = url.split('/');
    return parseInt(tokens[tokens.length - 1]);
}

function GetCourseTitles(courseId) {
    return fetchHttpGet(`/api/course/${courseId}/titles`);
}

function GetTitleElement(data) {
    let title = `${data["type"]} - ${data["itemTitle"]}`;
    let element = document.createElement('a');
    element.dataset["itemid"] = data["itemId"];
    element.innerText = title;
    return element;
}

