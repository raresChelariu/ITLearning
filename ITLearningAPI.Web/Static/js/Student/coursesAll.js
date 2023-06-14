import {FetchHttpGet} from "/js/Fetcher.js";
import {CreateAlertError, CreateAlertWarning} from "/js/Alert.js";
import {BuildCourseCard} from "/js/CourseCardBuilder.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/all")
    .then(responseResult => {
        if (responseResult.length === 0) {
            listCourses.appendChild(CreateAlertWarning("Nu exista cursuri de parcurs!"));
            return;
        }
        for (let i = 0; i < responseResult.length; i++) {
            const course = BuildCourseCard(responseResult[i]);
            listCourses.appendChild(course);
        }
    })
    .catch(err => {
        const alert = CreateAlertError("Cursurile nu au putut fi aduse de la server!");
        listCourses.appendChild(alert);
        console.log(err);
    });

