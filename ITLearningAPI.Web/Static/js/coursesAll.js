const listCourses = document.getElementById("listCourses");

fetch("https://localhost:7033/api/course/all", {
    method: "GET",
    mode: "cors",
    cache: "no-cache",
    credentials: "same-origin",
    referrerPolicy: "no-referrer"
}).then(apiResponse => {
    if (!apiResponse.ok) {
        console.log(apiResponse);
        return;
    }
    return apiResponse.json();
}).then(responseResult => {
    console.log(responseResult);
    for (let i = 0; i < responseResult.length; i++) {
        const course = document.createElement("button");

        course.innerText = responseResult[i]["name"];
        const courseId = responseResult[i]["id"];
        course.dataset.id = courseId;
        course.addEventListener("click", () => {
            window.location.replace(`/courses/${courseId}`);
        });

        listCourses.appendChild(course);
    }
});
