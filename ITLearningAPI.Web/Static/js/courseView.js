import { FetchHttpGet } from '/js/fetcher.js'
import { BuildDomItemCollectionFromApiResponse } from '/js/itemBuilder.js'

const courseId = GetCourseId();

const titleList = document.getElementById("courseTitleList");

const itemParent = document.getElementById("itemContainer");

GetCourseTitles(courseId)
    .then(response => {
        const courseItemIds = [];
        console.log(response);
        for (let i = 0; i < response.length; i++) {
            let item = response[i];
            let element = GetTitleElement(item);
            courseItemIds.push(item["itemId"]);
            titleList.appendChild(element);
        }
        return courseItemIds;
    })
    .then(itemIds => {
        const firstItemId = itemIds[0];
        return FetchHttpGet(`/api/item?itemId=${firstItemId}`);
    })
    .then(response => {
        console.log('First item here');
        console.log(response);
        const items = BuildDomItemCollectionFromApiResponse(response);
        for (let i = 0; i < items.length; i++)
        {
            itemParent.appendChild(items[i]);
        }
    });

function GetCourseId() {
    const url = window.location.href;
    const tokens = url.split('/');
    return parseInt(tokens[tokens.length - 1]);
}

function GetCourseTitles(courseId) {
    return FetchHttpGet(`/api/item/course/${courseId}`);
}

function GetTitleElement(data) {
    let title = `${data["type"]} - ${data["itemTitle"]}`;
    let element = document.createElement('a');
    element.dataset["itemid"] = data["itemId"];
    element.innerText = title;
    return element;
}

