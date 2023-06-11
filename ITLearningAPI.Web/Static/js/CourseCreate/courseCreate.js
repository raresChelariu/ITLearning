import {FetchHttpPostFormData, FetchHttpPostJson} from "/js/Fetcher.js";
import {GetVideoStepBuilder} from "/js/CourseCreate/CreateStepVideo.js"
import {GetWikiStepBuilder} from "/js/CourseCreate/CreateStepWiki.js";
import {GetQuizStepBuilder} from "/js/CourseCreate/CreateStepQuiz.js";

const pageIds = {
    PanelUploadScript: "panelUploadScript",
    CheckboxNeedToAttachScript: "checkboxNeedToAttachScript",
    InputScriptUpload: "inputScriptUpload",
    PanelCourseCreate: "panelCourseCreate"
};
const panelUploadScript = document.getElementById(pageIds.PanelUploadScript);
panelUploadScript.style.display = "none";

const checkboxNeedToAttachScript = document.getElementById(pageIds.CheckboxNeedToAttachScript);
checkboxNeedToAttachScript.addEventListener("change", () => {
    const panelUploadScript = document.getElementById(pageIds.PanelUploadScript);
    if (TeacherWantsToAddScript() === true) {
        panelUploadScript.style.display = "block";
    } else {
        panelUploadScript.style.display = "none";
    }
});


const buttonCreateCourse = document.getElementById("buttonCreateCourse");
const selectItemType = document.getElementById("selectItemType");

buttonCreateCourse.addEventListener("click", buttonCreateCourseOnClick);
selectItemType.addEventListener("change", ShowItemBuilder);
hideStepBuilderBeforeCourseIsCreated();

function TeacherWantsToAddScript() {
    const checkboxNeedToAttachScript = document.getElementById(pageIds.CheckboxNeedToAttachScript);
    return checkboxNeedToAttachScript.checked;
}

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
            const panelCreateCourse = document.getElementById(pageIds.PanelCourseCreate);
            panelCreateCourse.dataset.id = response["courseId"] + "";
            inputTitle.disabled = true;
            buttonCreateCourse.hidden = true;
            showStepBuilderAfterCourseIsCreated();
            alert("Cursul a fost creat cu succees! Poti adauga pasi!");
            if (TeacherWantsToAddScript() !== true) {
                return;
            }
            SubmitCourseScript()
                .then(() => {
                    alert("Script adaugat cu succes!");
                })
                .catch((err) => {
                    alert("Scriptul nu a putut fi adaugat!");
                    console.log(err);
                });
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

function SubmitCourseScript() {
    const inputFile = document.getElementById(pageIds.InputScriptUpload);
    const courseId = document.getElementById(pageIds.PanelCourseCreate).dataset.id;

    const formData = new FormData();
    formData.append("fileScript", inputFile.files[0]);
    formData.append("courseId", courseId);

    return FetchHttpPostFormData("/api/courseScript", formData);
}