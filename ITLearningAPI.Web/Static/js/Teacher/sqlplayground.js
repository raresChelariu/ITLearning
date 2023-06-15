import {FetchHttpGet} from "/js/Fetcher.js";
import {QueryResultToHtmlTable, RecreateDatabase} from "/js/SqlPlayground/Playground.js"
import {CreateAlertError, CreateAlertWarning} from "/js/Alert.js";

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

    QueryResultToHtmlTable(course.courseId, queryText, pageIds.QueryResultView)
        .catch(err => {
            let alert = null;
            if (err === "Empty array") {
                alert = CreateAlertWarning("Interogarea nu a returnat niciun rand!");    
            }
            else {
                alert = CreateAlertError("Interogarea nu a putut fi executata cu succes!");
            }
            const parent = document.getElementById(pageIds.QueryResultView);
            parent.innerHTML = "";
            parent.appendChild(alert);
            console.log(err);
        });
});

const buttonRecreateDb = document.getElementById(pageIds.ButtonRecreateDatabase);
buttonRecreateDb.addEventListener("click", () => {
    const course = GetCourseFromSelectedOption();
    
    RecreateDatabase(course.courseId);
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

function createCourseOption(data) {
    const option = document.createElement("option");
    option.innerText = data.courseName;
    option.value = `${data.courseId}@${data.courseName}`;
    return option;
}