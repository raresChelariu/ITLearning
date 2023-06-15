import {FetchHttpPostJson} from "/js/Fetcher.js";
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
                resolve(1)
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
