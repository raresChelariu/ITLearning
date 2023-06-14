import { FetchHttpGet } from "/js/Fetcher.js"
import { BuildDomItemCollectionFromApiResponse } from "/js/CourseView/ItemBuilder.js"
import { SetCourseTitles } from "/js/CourseView/CourseItemTitlesService.js";

const pageIds = {
  TitleList: "courseTitleList",
  ItemParent: "itemContainer"
};

const courseId = GetCourseId();

const titleList = document.getElementById(pageIds.TitleList);

const itemParent = document.getElementById(pageIds.ItemParent);
itemParent.dataset.courseId = courseId + "";

    SetCourseTitles(pageIds.TitleList)
    .then(itemIds => {
        const firstItemId = itemIds[0];
        itemParent.dataset.itemId = firstItemId;
        return FetchHttpGet(`/api/item?itemId=${firstItemId}`);
    })
    .then(response => {
        console.log("First item here");
        console.log(response);
        const items = BuildDomItemCollectionFromApiResponse(response);
        for (let i = 0; i < items.length; i++) {
            itemParent.appendChild(items[i]);
        }
        hljs.highlightAll();
    });

function GetCourseId() {
    const url = window.location.href;
    const tokens = url.split("/");
    return parseInt(tokens[tokens.length - 1]);
}





