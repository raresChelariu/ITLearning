import {FetchHttpGet} from "/js/Fetcher.js";
import {CreateAlertError, CreateAlertWarning} from "/js/Alert.js";
import {BuildCourseCard} from "/js/CourseCardBuilder.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/author")
    .then(responseResult => {
        if (responseResult.length === 0) {
            listCourses.appendChild(CreateAlertWarning("Nu ai niciun curs creat!"));
            return;
        }
        for (let i = 0; i < responseResult.length; i++) {
            const course = BuildCourseCard(responseResult[i]);
            listCourses.appendChild(course);
        }
    })
    .catch(err => {
        listCourses.appendChild(CreateAlertError("Cursurile nu au putut fi aduse de la server!"));
        console.log(err);
    });
