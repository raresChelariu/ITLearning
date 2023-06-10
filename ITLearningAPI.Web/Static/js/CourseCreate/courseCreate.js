import {FetchHttpPostJson} from "/js/Fetcher.js";
import {GetVideoStepBuilder} from "/js/CourseCreate/CreateStepVideo.js"
import {GetWikiStepBuilder} from "/js/CourseCreate/CreateStepWiki.js";
import {GetQuizStepBuilder} from "/js/CourseCreate/CreateStepQuiz.js";

const buttonCreateCourse = document.getElementById("buttonCreateCourse");
const selectItemType = document.getElementById("selectItemType");

buttonCreateCourse.addEventListener("click", buttonCreateCourseOnClick);
selectItemType.addEventListener("change", ShowItemBuilder);
hideStepBuilderBeforeCourseIsCreated();

function hideStepBuilderBeforeCourseIsCreated() {
    const stepBuilderBox = document.getElementById("stepBuilderBox");
    stepBuilderBox.hidden = true;    
}


function ShowItemBuilder(event) {
    const builderType = event.target.value;
    const parentElement = document.getElementById("stepBuilder");
    parentElement.innerHTML = "";
    let step = null;
    switch (builderType) {
        case "video":
            step = GetVideoStepBuilder();
            AppendStep(parentElement, step);
            return;
        case "wiki":
            step = GetWikiStepBuilder();
            AppendStep(parentElement, step);
            return;
        case "quizWithChoices":
            step = GetQuizStepBuilder();
            AppendStep(parentElement, step);
            return;
    }
}

function AppendStep(parent, step) {
    if (Array.isArray(step)) {
        for (let i = 0; i < step.length; i++) {
            parent.appendChild(step[i]);
        }
        return;
    }
    parent.appendChild(step);
}

function buttonCreateCourseOnClick() {
    const inputTitle = document.getElementById("inputCourseTitle");
    const courseTitle = inputTitle.value;
    const requestCourseCreate = {
        courseName: courseTitle
    };
    
    FetchHttpPostJson("/api/course", requestCourseCreate)
        .then((response) => {
            const panelCreateCourse = document.getElementById("panelCourseCreate");
            panelCreateCourse.dataset.id = response["courseId"] + "";
            inputTitle.disabled = true;
            buttonCreateCourse.hidden = true;
            showStepBuilderAfterCourseIsCreated();
            alert("Cursul a fost creat cu succees! Poti adauga pasi!");            
        })
        .catch(err => {
            alert("Cursul nu a putut fi creat");
            console.log(err);
        });
}

function showStepBuilderAfterCourseIsCreated() {
    const stepBuilderBox = document.getElementById("stepBuilderBox");
    stepBuilderBox.hidden = false;
}