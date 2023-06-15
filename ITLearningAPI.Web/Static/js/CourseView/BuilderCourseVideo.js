import {BuildNextStepButton} from "/js/CourseView/NextStep.js"

export function BuildCourseVideo(data) {
    const video = buildVideo(data);
    const title = buildTitle(data);
    const p1 = document.createElement("p");
    return [title, video, p1, BuildNextStepButton()];
}

function buildVideo(data) {
    const video = document.createElement("video");
    video.autoplay = true;
    video.controls = true;
    video.src = `/api/video?videoId=${data["itemId"]}`;
    video.classList.add("video-style");
    return video;
}

function buildTitle(data) {
    const titleDiv = document.createElement("div");
    titleDiv.classList.add("title");
    titleDiv.innerText = data["title"];
    return titleDiv;
}