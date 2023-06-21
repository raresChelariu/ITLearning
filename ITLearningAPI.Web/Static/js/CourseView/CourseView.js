import {FetchHttpGet, FetchHttpPostJson} from "/js/Fetcher.js"
import {BuildDomItemCollectionFromApiResponse} from "/js/CourseView/ItemBuilder.js"
import {SetCourseTitles} from "/js/CourseView/CourseItemTitlesService.js";

const pageIds = {
    TitleList: "courseTitleList",
    ItemParent: "itemContainer",
    ButtonResetProgress: "buttonResetProgress"
};

const buttonResetProgress = document.getElementById(pageIds.ButtonResetProgress);
buttonResetProgress.addEventListener("click", () => {
    const request = {
        courseId: GetCourseId()  
    };
    FetchHttpPostJson("/api/course/progress/reset", request)
        .then(() => {
            location.reload();
        })
        .catch(err => {
            
            console.log(err);
        });
});

const courseId = GetCourseId();

const itemParent = document.getElementById(pageIds.ItemParent);
itemParent.dataset.courseId = courseId + "";

SetCourseTitles(pageIds.TitleList)
    .then(() => {
        const currentUserItem = GetItemIdCurrent();
        itemParent.dataset.itemId = currentUserItem + "";
        return FetchHttpGet(`/api/item?itemId=${currentUserItem}`);
    })
    .then(response => {
        const items = BuildDomItemCollectionFromApiResponse(response);
        for (const element of items) {
            itemParent.appendChild(element);
        }
        hljs.highlightAll();
    });

function GetCourseId() {
    const url = window.location.href;
    const tokens = url.split("/");
    return parseInt(tokens[tokens.length - 1]);
}

function GetItemIdCurrent() {
    const titleList = document.getElementById(pageIds.TitleList);

    for (const child of titleList.children) {
        if (child.dataset.progress === "1") {
            return parseInt(child.dataset.itemid);
        }
    }

    return parseInt(titleList.children[titleList.children.length - 1].dataset.itemid);
}



