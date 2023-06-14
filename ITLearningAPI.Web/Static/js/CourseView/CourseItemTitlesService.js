import { FetchHttpGet } from "/js/Fetcher.js"
import { AddCorrectAnswerMarker, AddLoadingMarker } from "/js/CourseView/CorrectnessMarker.js"

export function SetCourseTitles(titleListId) {
    const courseId = GetCourseId();
    return FetchCourseTitles(courseId)
        .then(response => {
            console.log("SETTING COURSE TITLES");
            const titleList = document.getElementById(titleListId);
            titleList.innerHTML = "";
            const courseItemIds = [];
            console.log(response);
            for (let i = 0; i < response.length; i++) {
                let item = response[i];
                let element = GetTitleElement(item);
                courseItemIds.push(item["itemId"]);
                titleList.appendChild(element);
            }
            return courseItemIds;
        });    
}

function GetTitleElement(data) {
    let title = `${data["type"]} - ${data["itemTitle"]}`;
    let element = document.createElement("a");
    element.dataset["itemid"] = data["itemId"];
    let userProgress = data["progress"]; 
    switch (userProgress) {
        case 0: 
            element.innerText = title;
            break;
        case 1:
            element.innerHTML = AddLoadingMarker(title);
            break;
        case 2:
            element.innerHTML = AddCorrectAnswerMarker(title);
            break;
    }
    return element;
}

function GetCourseId() {
    const url = window.location.href;
    const tokens = url.split("/");
    return parseInt(tokens[tokens.length - 1]);
}

function FetchCourseTitles(courseId) {
    return FetchHttpGet(`/api/item/course/${courseId}`);
}