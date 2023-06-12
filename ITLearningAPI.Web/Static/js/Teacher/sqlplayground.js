import {FetchHttpGet, FetchHttpPostJson} from "/js/Fetcher.js";
import {Grid} from "https://unpkg.com/gridjs?module";

const pageIds = {
    SelectCourseList: "coursesList",
    QueryResultView: "queryResultView",
    ButtonRecreateDatabase: "buttonRecreateDb",
    ButtonRunSqlQuery: "buttonRunSqlQuery",
    InputQuery: "queryInput"
};

function GetCourseFromSelectedOption() {
    const select = document.getElementById(pageIds.SelectCourseList);
    const value = select.value;
    const tokens = value.split("@");
    return {
        courseId: parseInt(tokens[0]),
        courseName: tokens[1]
    };
}

const buttonRunSqlQuery = document.getElementById(pageIds.ButtonRunSqlQuery);
buttonRunSqlQuery.addEventListener("click", () => {
    const course = GetCourseFromSelectedOption();
    const inputQueryText = document.getElementById(pageIds.InputQuery);
    const queryText = inputQueryText.value;
    const request = {
        courseId: course.courseId,
        queryText: queryText
    };
    FetchHttpPostJson("/api/sqlplayground/run", request)
        .then(response => {
            console.log(response);
            JsonToHtmlTable(JSON.parse(response["result"]), pageIds.QueryResultView);
        })
        .catch(err => {
            alert("Interogarea nu a putut fi executat cu succes!")
            console.log(err);
        });
});

GetCoursesWithSqlScripts();

function GetCoursesWithSqlScripts() {
    FetchHttpGet("/api/course/author/withscripts")
        .then(response => {
            console.log(response);
            const select = document.getElementById(pageIds.SelectCourseList);
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
    option.value = `${data.courseId}@${data.courseName}`;
    return option;
}