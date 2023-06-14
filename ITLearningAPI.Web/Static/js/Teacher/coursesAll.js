import {FetchHttpGet} from "/js/Fetcher.js";
import {CreateAlertError} from "/js/Alert.js";
import {BuildCourseCard} from "/js/CourseCardBuilder.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/all")
    .then(responseResult => {
        for (let i = 0; i < responseResult.length; i++) {
            const course = BuildCourseCard(responseResult[i]);
            listCourses.appendChild(course);
        }
    }).catch(err => {
    CreateAlertError("Cursurile nu au putut fi aduse de la server!");
    console.log(err);
});
