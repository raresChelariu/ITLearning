import {BuildCourseWiki} from "/js/CourseView/BuilderCourseWiki.js";
import {BuildCourseQuiz} from "/js/CourseView/BuilderCourseQuiz.js";
import {BuildCourseVideo} from "/js/CourseView/BuilderCourseVideo.js";

export function BuildDomItemCollectionFromApiResponse(data) {
    console.log("Got to item builder");
    const itemType = data["type"];

    switch (itemType) {
        case 1:
            return BuildCourseQuiz(data);
        case 2:
            return BuildCourseWiki(data);
        case 3:
            return BuildCourseVideo(data);
        default:
            console.error(`Unexpected item type ${itemType}`);
    }
}