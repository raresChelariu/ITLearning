import { FetchHttpGet } from "/js/Fetcher.js";

const listCourses = document.getElementById("listCourses");

FetchHttpGet("/api/course/author").then(responseResult => {
    console.log(responseResult);
    for (let i = 0; i < responseResult.length; i++) {
        const course = document.createElement("button");

        course.innerText = responseResult[i]["name"];
        const courseId = responseResult[i]["id"];
        course.dataset.id = courseId;
        course.addEventListener("click", () => {
            window.location.replace(`/course/${courseId}`);
        });

        listCourses.appendChild(course);
    }
});
