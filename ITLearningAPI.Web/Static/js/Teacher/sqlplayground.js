import {FetchHttpGet} from "/js/Fetcher.js";
import {Grid} from "https://unpkg.com/gridjs?module";

const pageIds = {
    selectCourseList: "coursesList",
    QueryResultView: "queryResultView"
}
GetCoursesWithSqlScripts();


function GetCoursesWithSqlScripts() {
    FetchHttpGet("/api/course/author/withscripts")
        .then(response => {
            console.log(response);
            JsonToHtmlTable(response, pageIds.QueryResultView);
            const select = document.getElementById(pageIds.selectCourseList);
            for (let i = 0; i < response.length; i++) {
                const data = {
                    courseId: response[i]["id"],
                    courseName: response[i]["name"]
                };
                let option = createCourseOption(data);
                select.appendChild(option);
            }
        })
        .catch(err => {
            console.log(err);
        })
}

function JsonToHtmlTable(jsonDataArray, targetContainerId) {
    new Grid({
        data: jsonDataArray
    }).render(document.getElementById(targetContainerId));
}

function createCourseOption(data) {
    const option = document.createElement("option");
    option.innerText = data.courseName;
    option.value = data.courseId + "";
    return option;
}