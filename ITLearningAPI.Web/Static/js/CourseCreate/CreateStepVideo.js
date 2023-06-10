import {FetchHttpPostFormData} from "/js/Fetcher.js";
import {AddStepToSummary} from "/js/CourseCreate/CourseCreateSummary.js";

const urlVideoUpload = "/api/video/upload";

const stepIds = {
    FormStepVideo: "formStepVideo",
    InputVideoTitle: "inputTitleStepVideo",
}

export function GetVideoStepBuilder() {
    const form = document.createElement("form");
    
    form.id = stepIds.FormStepVideo;
    form.setAttribute("enctype", "multipart/form-data");
    form.setAttribute("method", "post");
    form.addEventListener("submit", VideoCreateRequest);
    
    form.setAttribute("action", urlVideoUpload);
    
    form.appendChild(createLabelTitle());
    form.appendChild(createInputTitle());
    form.appendChild(createInputVideo());
    form.appendChild(createFormSubmit());

    return form;
}

function createLabelTitle() {
    const labelTitle = document.createElement("label");
    labelTitle.setAttribute("for", stepIds.InputVideoTitle);
    labelTitle.innerText = "Titlu Video";
    return labelTitle;
}

function createInputTitle() {
    const inputTitle = document.createElement("input");
    inputTitle.id = stepIds.InputVideoTitle;
    inputTitle.setAttribute("name", "title");
    inputTitle.classList.add("endOfRow");
    return inputTitle;
}

function createInputVideo() {
    const inputVideo = document.createElement("input");
    inputVideo.setAttribute("type", "file");
    inputVideo.classList.add("endOfRow");
    inputVideo.setAttribute("name", "file");
    return inputVideo;
}

function createFormSubmit() {
    const submit = document.createElement("input");
    submit.type = "submit";
    submit.value = "Adauga pas";
    submit.classList.add("NextStep");
    return submit;
}

async function VideoCreateRequest(event) {
    event.preventDefault();
    const form = document.getElementById(stepIds.FormStepVideo);
    const formData = new FormData(form);
    const panelCreateCourse = document.getElementById("panelCourseCreate");
    const courseId = panelCreateCourse.dataset.id;
    
    formData.append("courseId", courseId);
    
    FetchHttpPostFormData(urlVideoUpload, formData)
        .then(() => {
            alert("Pasul Video a fost creat cu succes!");
            AddStepToSummary({
                stepTitle: document.getElementById(stepIds.InputVideoTitle).value
            });
        })
        .catch(err => {
            alert("Pasul Video nu a putut fi creat!");
            console.log(err);
        });
}