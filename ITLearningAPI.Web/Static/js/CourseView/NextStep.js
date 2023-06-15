import {FetchHttpPostJson} from "/js/Fetcher.js";
import {BuildDomItemCollectionFromApiResponse} from "/js/CourseView/ItemBuilder.js";
import {SetCourseTitles} from "/js/CourseView/CourseItemTitlesService.js";

const htmlIds = {
    ButtonNextStep: "buttonNextStep",
    ItemParent: "itemContainer",
    TitleList: "courseTitleList",
};

export function BuildNextStepButton() {
    const element = document.createElement("button");
    element.innerText = "Mergi la pasul urmator";
    element.id = htmlIds.ButtonNextStep;
    element.classList.add("NextStep");
    element.addEventListener("click", buttonNextStepOnClick)
    return element;
}

function buttonNextStepOnClick() {
    const itemParent = document.getElementById(htmlIds.ItemParent);
    const currentItemId = itemParent.dataset.itemId;
    const courseId = itemParent.dataset.courseId;
    const nextItemRequest = {
        courseId: courseId,
        itemId: currentItemId
    };
    FetchHttpPostJson("/api/item/next", nextItemRequest)
        .then(response => {
            HandleNextItemApiResponse(response);
            SetCourseTitles(htmlIds.TitleList)
                .catch(err => {
                    console.log(err);
                });
        })
        .catch(err => {
            console.log(err);
        });
}

function HandleNextItemApiResponse(response) {
    console.log(response);
    const itemParent = document.getElementById(htmlIds.ItemParent);
    let isEndOfCourse = response["endOfCourse"];
    if (isEndOfCourse === true) {
        console.log("Got to end of course");
        itemParent.innerHTML = "";
        const messageEndOfCourse = document.createElement("h1");
        messageEndOfCourse.classList.add("message-end-of-course");
        messageEndOfCourse.innerText = "Ai terminat cu succes cursul!";
        messageEndOfCourse.style.textAlign = "center";
        itemParent.appendChild(messageEndOfCourse);
        party.confetti(itemParent);
        return;
    }
    let courseItemData = response["courseItem"];
    itemParent.dataset.itemId = courseItemData["itemId"];
    itemParent.innerHTML = "";
    let itemCollection = BuildDomItemCollectionFromApiResponse(courseItemData);
    for (let i = 0; i < itemCollection.length; i++) {
        itemParent.appendChild(itemCollection[i]);
    }
    hljs.highlightAll();
}