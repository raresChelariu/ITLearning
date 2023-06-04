import { BuildCourseWiki } from "/js/BuilderCourseWiki.js";
import { BuildCourseQuiz } from "/js/BuilderCourseQuiz.js";

export function BuildDomItemCollectionFromApiResponse(data) {
    console.log("Got to item builder");
    const itemType = data["type"];
    
    switch (itemType) {
        case 1:
            return BuildCourseQuiz(data);
        case 2:
            return BuildCourseWiki(data);
        default:
            console.error(`Unexpected item type ${itemType}`);
    }
}