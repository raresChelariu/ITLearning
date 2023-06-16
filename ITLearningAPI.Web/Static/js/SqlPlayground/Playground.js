import {FetchHttpPostJson, FetchHttpGet} from "/js/Fetcher.js";
import {Grid} from "https://unpkg.com/gridjs?module";

let grid = null;

export function QueryResultToHtmlTable(courseId, queryText, displayElementId) {
    const request = {
        courseId: courseId,
        queryText: queryText
    };
    
    return new Promise(async (resolve, reject) => {
        try {
            const response = await FetchHttpPostJson("/api/sqlplayground/run", request);
            const parsedResponse = JSON.parse(response["result"] + ""); 
            if (Array.isArray(parsedResponse) && parsedResponse.length === 0)
            {
                reject("Empty array");
            }
            else {
                JsonToHtmlTable(parsedResponse, displayElementId);
                resolve(1);
            }
        }
        catch (err) {
            reject(err);
        }
    });
}


export function RecreateDatabase(courseId) {
    const request = {
        courseId: courseId
    };
    return FetchHttpPostJson("/api/sqlplayground/recreate", request);
}

function JsonToHtmlTable(jsonDataArray, targetContainerId) {
    console.log("the json data array");
    console.log(jsonDataArray);
    document.getElementById(targetContainerId).innerHTML = "";
    
    const keys = Object.keys(jsonDataArray[0]);
    console.log(keys);
    const data = createDataForGridJs(jsonDataArray, keys);
    console.log(data);
    
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
    
    for (let i = 0; i < data.length; i++) {
        const row = [];
        for (let j = 0; j < keys.length; j++) {
            row.push(data[i][keys[j]]);
        }
        result.push(row);
    }
    
    return result;
}

function GetCoursesWithSqlScripts(url) {
    FetchHttpGet(url)
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
        });
}

function createCourseOption(data) {
    const option = document.createElement("option");
    option.innerText = data.courseName;
    option.value = `${data.courseId}@${data.courseName}`;
    return option;
}

function SetButtonsOnClickHandlers() {
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
}

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