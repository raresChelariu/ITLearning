import {BuildNextStepButton} from "/js/CourseView/NextStep.js"

export function BuildCourseVideo(data) {
    const video = buildVideo(data);
    const title = buildTitle(data);
    return [title, video, BuildNextStepButton()];
}

function buildVideo(data) {
    const video = document.createElement("video");
    video.autoplay = true;
    video.controls = true;
    video.src = `/api/video?videoId=${data["itemId"]}`;
    return video;
}

function buildTitle(data) {
    const titleDiv = document.createElement("div");
    titleDiv.classList.add("title");
    titleDiv.innerText = data["title"];
    return titleDiv;
}