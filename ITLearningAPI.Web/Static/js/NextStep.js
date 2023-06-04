import {FetchHttpPost} from "/js/fetcher.js";
import {BuildDomItemCollectionFromApiResponse} from "/js/ItemBuilder.js";

export function BuildNextStepButton() {
    const element = document.createElement("button");
    element.innerText = "Mergi la pasul urmator";
    element.id = "buttonNextStep";
    element.classList.add("NextStep");
    element.addEventListener("click", buttonNextStepOnClick)
    return element;
}

function buttonNextStepOnClick() {
    const itemParent = document.getElementById("itemContainer");
    const currentItemId = itemParent.dataset.itemId;
    const courseId = itemParent.dataset.courseId;
    const nextItemRequest = {
        courseId: courseId,
        itemId: currentItemId
    };
    FetchHttpPost('/api/item/next', nextItemRequest)
        .then(HandleNextItemApiResponse)
        .catch(err => {
            console.log(err);
        });
}

function HandleNextItemApiResponse(response) {
    const itemParent = document.getElementById("itemContainer");
    console.log(response);
    let courseItemData = response["courseItem"];
    itemParent.dataset.itemId = courseItemData["itemId"];
    let isEndOfCourse = response["endOfCourse"];
    let itemCollection = BuildDomItemCollectionFromApiResponse(courseItemData);
    itemParent.innerHTML = "";
    for (let i = 0; i < itemCollection.length; i++) {
        itemParent.appendChild(itemCollection[i]);
    }
}