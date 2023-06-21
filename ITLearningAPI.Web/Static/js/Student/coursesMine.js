import {FetchHttpGet} from "/js/Fetcher.js";
import {CreateAlertError, CreateAlertWarning} from "/js/Alert.js";
import {BuildCourseCard} from "/js/CourseCardBuilder.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/student")
    .then(responseResult => {
        if (responseResult.length === 0) {
            listCourses.appendChild(CreateAlertWarning("Nu ai niciun curs deja inceput!"));
            return;
        }
        for (const element of responseResult) {
            const course = BuildCourseCard(element);
            listCourses.appendChild(course);
        }
    })
    .catch(err => {
        listCourses.appendChild(CreateAlertError("Cursurile nu au putut fi aduse de la server!"));
        console.log(err);
    });
