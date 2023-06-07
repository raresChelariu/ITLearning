import {FetchHttpPostJson} from "/js/Fetcher.js";

const buttonCreateCourse = document.getElementById("buttonCreateCourse");

buttonCreateCourse.addEventListener('click', buttonCreateCourseOnClick);

function buttonCreateCourseOnClick() {
    const inputTitle = document.getElementById("inputCourseTitle");
    const courseTitle = inputTitle.innerText;
    const requestCourseCreate = {
        courseName: courseTitle
    };
    FetchHttpPostJson("/api/course", requestCourseCreate)
        .then((response) => {
            const panelCreateCourse = document.getElementById("panelCourseCreate");
            panelCreateCourse.dataset.id = response["courseId"] + "";
            inputTitle.disabled = true;
        })
        .catch(err => {
            console.log(err);
        });
}

