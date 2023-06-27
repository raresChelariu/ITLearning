import {FetchHttpPostJson, FetchHttpGet} from "/js/Fetcher.js";
import {Grid} from "https://unpkg.com/gridjs?module";
import {CreateAlertError, CreateAlertWarning} from "/js/Alert.js";

let grid = null;


export function PreviewQueryWithSyntaxHighlighting() {
    const previewer = document.getElementById(pageIds.QueryHighlightPreview);

    document.getElementById(pageIds.InputQuery).addEventListener("input", UpdateQueryPreview);
}

function UpdateQueryPreview() {
    const userSqlQuery = document.getElementById(pageIds.InputQuery).value;
    const previewer = document.getElementById(pageIds.QueryHighlightPreview);
    // marked.use({
    //    
    // });
    previewer.innerHTML = marked.parse(userSqlQuery);
    hljs.highlightAll();
}

export function QueryResultToHtmlTable(courseId, queryText, displayElementId) {
    const request = {
        courseId: courseId,
        queryText: queryText
    };

    return new Promise(async (resolve, reject) => {
        try {
            const response = await FetchHttpPostJson("/api/sqlplayground/run", request);
            const parsedResponse = JSON.parse(response["result"] + "");
            if (Array.isArray(parsedResponse) && parsedResponse.length === 0) {
                reject("Empty array");
            } else {
                JsonToHtmlTable(parsedResponse, displayElementId);
                resolve(1);
            }
        } catch (err) {
            reject(err);
        }
    });
}


export function RecreateDatabase(courseId) {
    const request = {
        courseId: courseId
    };
    return FetchHttpPostJson("/api/sqlplayground/recreate", request)
        .then(() => {
            alert("Baza de date a fost recreata cu succes!");
        }).catch(() => {
            alert("Baza de date nu a putut fi recreată!");
        });
}

function JsonToHtmlTable(jsonDataArray, targetContainerId) {
    document.getElementById(targetContainerId).innerHTML = "";

    const keys = Object.keys(jsonDataArray[0]);
    const data = createDataForGridJs(jsonDataArray, keys);

    if (grid === null) {
        grid = new Grid({
            columns: keys,
            sort: true,
            pagination: true,
            fixedHeader: true,
            data: data
        });
        grid.render(document.getElementById(targetContainerId));
        return;
    }
    grid.updateConfig({
        columns: keys,
        data: data
    }).forceRender();

}

function createDataForGridJs(data, keys) {
    const result = [];

    for (const element of data) {
        const row = [];
        for (const key of keys) {
            row.push(element[key]);
        }
        result.push(row);
    }

    return result;
}

export function GetCoursesWithSqlScripts(url) {
    FetchHttpGet(url)
        .then(response => {
            const select = document.getElementById(pageIds.SelectCourseList);
            for (const element of response) {
                const data = {
                    courseId: element["id"],
                    courseName: element["name"]
                };
                let option = createCourseOption(data);
                select.appendChild(option);
            }
        })
        .catch(err => {
            console.log(err);
        });
}

function createCourseOption(data) {
    const option = document.createElement("option");
    option.innerText = data.courseName;
    option.value = `${data.courseId}@${data.courseName}`;
    return option;
}

export function SetButtonOnClickHandlers() {
    const buttonRunSqlQuery = document.getElementById(pageIds.ButtonRunSqlQuery);
    buttonRunSqlQuery.addEventListener("click", () => {
        const course = GetCourseFromSelectedOption();
        const inputQueryText = document.getElementById(pageIds.InputQuery);
        const queryText = inputQueryText.value;

        QueryResultToHtmlTable(course.courseId, queryText, pageIds.QueryResultView)
            .catch(err => {
                let alert;
                if (err === "Empty array") {
                    alert = CreateAlertWarning("Interogarea nu a returnat niciun rand!");
                } else {
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
}

const pageIds = {
    SelectCourseList: "coursesList",
    QueryResultView: "queryResultView",
    ButtonRecreateDatabase: "buttonRecreateDb",
    ButtonRunSqlQuery: "buttonRunSqlQuery",
    InputQuery: "queryInput",
    QueryHighlightPreview: "queryHighlightPreview"
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