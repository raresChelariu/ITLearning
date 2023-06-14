import {FetchHttpGet} from "/js/Fetcher.js";
import {CreateAlertError} from "/js/Alert.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/author")
    .then(responseResult => {
        for (let i = 0; i < responseResult.length; i++) {
            const course = createCourseElement(responseResult[i]);
            listCourses.appendChild(course);
        }
    })
    .catch(err => {
        CreateAlertError("Cursurile nu au putut fi aduse de la server!");
        console.log(err);
    });

function createCourseElement(data) {
    const courseId = data["id"];

    const courseTitle = document.createElement("h2");
    courseTitle.innerText = data["name"];

    const description = document.createElement("p");
    description.innerText = "Some Course Description Here";

    const container = document.createElement("div")
    container.classList.add("card-content");
    container.appendChild(courseTitle);
    container.appendChild(description);

    const course = document.createElement("article");
    course.classList.add("card");
    course.appendChild(container);
    course.dataset.id = courseId;
    course.addEventListener("click", () => {
        window.location.replace(`/course/${courseId}`);
    });
    return course;
}
